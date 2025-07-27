import {Component, OnInit, inject, signal, OnDestroy, DestroyRef} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { StudentService } from '../../core/services/student.service';
import { ExamService } from '../../core/services/exam.service';
import { AuthService } from '../../core/services/auth.service';
import { DropdownModel } from '../../core/models/common/common.model';

@Component({
  selector: 'app-take-exam',
  standalone: true,
  imports: [CommonModule, CardModule, ButtonModule],
  templateUrl: './take-exam.html',
  styleUrls: ['./take-exam.scss']
})
export class TakeExam implements OnInit {
  private studentService = inject(StudentService);
  private examService = inject(ExamService);
  private authService = inject(AuthService);
  private router = inject(Router);
  private destroyRef = inject(DestroyRef);

  subjects = signal<DropdownModel[]>([]);
  selectedSubject = signal<DropdownModel | null>(null);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  ngOnInit() {
    this.loadStudentSubjects();
  }

  loadStudentSubjects() {
    this.loading = signal<boolean>(false);
    this.error = signal<string | null>(null);

    const studentId = this.authService.getUserId();
    if (!studentId) {
      this.error.set('Unable to identify student. Please log in again.');
      this.loading.set(false);
      return;
    }

    const subscription = this.studentService.getStudentSubjects(studentId.toString()).subscribe({
      next: (subjects) => {
        this.subjects.set(subjects);
        this.loading.set(false);
      },
      error: (error) => {
        this.error.set('Failed to load subjects. Please try again.');
        this.loading.set(false);
        console.error('Error loading student subjects:', error);
      }
    });
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }

  selectSubject(subject: DropdownModel) {
    this.selectedSubject.set(subject);
  }

  startExam() {
    if (!this.selectedSubject) {
      this.error.set('Please select a subject to start the exam.');
      return;
    }

    this.loading.set(true);
    this.error.set(null);

    const subscription = this.examService.getExamForSubject(this.selectedSubject()?.id!).subscribe({
      next: (exam) => {
        this.loading.set(true);
        // Navigate to exam page with exam data
        this.router.navigate(['/home/exam', this.selectedSubject()?.id], {
          state: { examData: exam }
        });
      },
      error: (error) => {
        this.loading.set(false);
        if (error.status === 400) {
          this.error.set('You have already taken this exam or it is not available.');
        } else {
          this.error.set('Failed to start exam. Please try again.');
        }
        this.selectedSubject.set(null);
        console.error('Error starting exam:', error);
      }
    });
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }

  disableExamButton() {
    return !this.selectedSubject() || this.loading();
  }
}
