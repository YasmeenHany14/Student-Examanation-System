import {Component, inject, input, OnInit, signal, output, OnChanges, Input, DestroyRef} from '@angular/core';
import {DialogModule} from 'primeng/dialog';
import {StudentService} from '../../core/services/student.service';
import {StudentDetailsModel} from '../../core/models/student.model';
import {StudentDetails} from '../student-details/student-details';
import {Spinner} from '../../shared/components/spinner/spinner';
import {Button} from 'primeng/button';
import {SubjectService} from '../../core/services/subject.service';
import {DropdownModel} from '../../core/models/common/common.model';
import {Select} from 'primeng/select';
import {FormsModule} from '@angular/forms';
import {MessageService} from 'primeng/api';

@Component({
  selector: 'app-student-details-dialog',
  imports: [DialogModule, Button, StudentDetails, Select, FormsModule, Spinner],
  templateUrl: './student-details-dialog.html',
  styleUrl: './student-details-dialog.scss'
})
export class StudentDetailsDialog implements OnInit, OnChanges {
  selectedStudentId = input<string | null>(null);
  @Input() visible: boolean = false;
  close = output<boolean>();

  studentDetails = signal<StudentDetailsModel | null>(null);
  subjectsToChooseFrom = signal<DropdownModel[]>([]);
  selectedCourseToAdd: DropdownModel | null = null;
  showAddCourse = signal(false);
  loadingSubjects = signal(false);

  loading = signal(false);
  error = signal(false);

  private studentService = inject(StudentService);
  private subjectService = inject(SubjectService);
  private messageService = inject(MessageService);
  private destroyRef = inject(DestroyRef);

  ngOnInit() {
    if (this.selectedStudentId() && this.visible) {
      this.loadStudentDetails();
    }
  }

  ngOnChanges() {
    if (this.selectedStudentId() && this.visible && !this.studentDetails()) {
      this.loadStudentDetails();
    }

    if (!this.visible) {
      this.resetDialog();
    }
  }

  onClose() {
    console.log('Dialog closed');
    this.visible = false;
    this.close.emit(true);
  }

  private loadStudentDetails() {
    this.loading.set(true);
    this.error.set(false);

    this.studentService.getByGuid<StudentDetailsModel>(this.selectedStudentId()!).subscribe({
      next: (result) => {
        this.studentDetails.set(result);
        this.loading.set(false);
      },
      error: (_err) => {
        this.error.set(true);
        this.loading.set(false);
      }
    });
  }

  onAddCourse() {
    this.loadingSubjects.set(true);
    this.loadSubjects();
    this.showAddCourse.set(true);
  }

  onCancelAddCourse() {
    this.showAddCourse.set(false);
    this.selectedCourseToAdd = null;
  }

  onConfirmAddCourse() {
    if (!this.selectedCourseToAdd) return;
    const selectedSubject = this.subjectsToChooseFrom().find(s => s.id === this.selectedCourseToAdd?.id);
    if (selectedSubject) {
      this.onAddCourseSelected(selectedSubject);
    }
  }

  onAddCourseSelected(subject: DropdownModel) {
    const subscription = this.studentService.addStudentSubject(this.selectedStudentId()!, subject.id).subscribe({
      next: () => {
        this.subjectsToChooseFrom.update(subjects => {
          return subjects.filter(s => s.id !== subject.id);
        });
        this.studentDetails.update(details => {
          if (details) {
            details.courses.push(subject);
          }
          return details;
        });
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: `Course ${subject.name} added successfully.`,
        });
      }
    });
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }
  private loadSubjects() {
    const subscription = this.subjectService.getDropdownOptions<DropdownModel>().subscribe({
      next: (result) => {
        const subjects = result.filter(subject => {
          return this.studentDetails()?.courses?.every(course => course.id !== subject.id);
        })
        this.subjectsToChooseFrom.set(subjects);
      }, error : (_err) => {
        this.subjectsToChooseFrom.set([]);
      }
    });
    this.loadingSubjects.set(false);
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }

  private resetDialog() {
    this.studentDetails.set(null);
    this.loading.set(false);
    this.error.set(false);
  }
}
