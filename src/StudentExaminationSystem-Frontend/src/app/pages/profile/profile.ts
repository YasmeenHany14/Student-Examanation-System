import {Component, computed, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {AuthService} from '../../core/services/auth.service';
import {StudentService} from '../../core/services/student.service';
import {Router} from '@angular/router';
import {routes} from '../../core/constants/routs';
import {StudentDetails} from '../../components/student-details/student-details';
import {StudentDetailsModel} from '../../core/models/student.model';
import {Spinner} from '../../shared/components/spinner/spinner';

@Component({
  selector: 'app-profile',
  imports: [
    Spinner,
    StudentDetails
  ],
  templateUrl: './profile.html',
  styleUrl: './profile.scss'
})
export class Profile implements OnInit {
  private authService = inject(AuthService);
  private studentService = inject(StudentService);
  private router = inject(Router);
  private destroyRef = inject(DestroyRef);

  loading = signal(true);
  error = signal<string | null>(null);
  studentDetails = signal<StudentDetailsModel | null>(null);

  // isAdmin = computed(() => this.authService.isAdmin);
  isAdmin = computed(() => {
    const isAdmin = this.authService.isAdmin();
    return isAdmin;
  })
  userId = computed(() => this.authService.getUserId());

  ngOnInit(): void {
    this.loading.set(true);
    this.error.set(null);
    if(this.isAdmin()) {
      this.router.navigate([routes.adminHomePage]);
    } else {
      this.loadStudentDetails(this.userId()!);
    }
  }

  loadStudentDetails(userId: string) {
    const subscription = this.studentService.getByGuid<StudentDetailsModel>(userId).subscribe({
      next: student => {
        this.loading.set(false);
        this.studentDetails.set(student);
        this.error.set(null);
      },
      error: err => {
        this.loading.set(false);
        this.router.navigate([routes.notFoundPage]);
        this.error.set(err.message || 'An error occurred while loading student details.');
      }
    })
    this.destroyRef.onDestroy(() => {
      subscription.unsubscribe();
    });
  }

}
