import {Component, inject, input, OnInit, signal, output, OnChanges, Input} from '@angular/core';
import {DialogModule} from 'primeng/dialog';
import {StudentService} from '../../core/services/student.service';
import {StudentDetailsModel} from '../../core/models/student.model';
import {StudentDetails} from '../student-details/student-details';
import {Spinner} from '../../shared/components/spinner/spinner';
import {Button} from 'primeng/button';
import {Message} from 'primeng/message';
import {single} from 'rxjs';

@Component({
  selector: 'app-student-details-dialog',
  imports: [DialogModule, StudentDetails, Spinner, Button, Message],
  templateUrl: './student-details-dialog.html',
  styleUrl: './student-details-dialog.scss'
})
export class StudentDetailsDialog implements OnInit, OnChanges {
  selectedStudentId = input<number | null>(null);
  visible = input<boolean>(false);
  close = output<boolean>();

  studentDetails = signal<StudentDetailsModel | null>(null);
  loading = signal(false);
  error = signal(false);

  private studentService = inject(StudentService);

  ngOnInit() {
    if (this.selectedStudentId() && this.visible()) {
      this.loadStudentDetails();
    }
  }

  ngOnChanges() {
    if (this.selectedStudentId() && this.visible() && !this.studentDetails()) {
      this.loadStudentDetails();
    }

    if (!this.visible()) {
      this.resetDialog();
    }
  }

  onClose() {
    this.close.emit(false);
  }

  private loadStudentDetails() {
    const studentId = this.selectedStudentId();
    if (!studentId) return;

    this.loading.set(true);
    this.error.set(false);

    this.studentService.getById<StudentDetailsModel>(studentId).subscribe({
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

  }

  private resetDialog() {
    this.studentDetails.set(null);
    this.loading.set(false);
    this.error.set(false);
  }
}
