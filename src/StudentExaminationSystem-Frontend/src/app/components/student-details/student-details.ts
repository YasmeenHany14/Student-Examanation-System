import {Component, computed, inject, input} from '@angular/core';
import {StudentDetailsModel} from '../../core/models/student.model';
import {AuthService} from '../../core/services/auth.service';
import {CommonModule} from '@angular/common';
import {StudentService} from '../../core/services/student.service';

@Component({
  selector: 'app-student-details',
  imports: [CommonModule],
  templateUrl: './student-details.html',
  styleUrl: './student-details.scss'
})
export class StudentDetails {
  studentDetails = input.required<StudentDetailsModel>();
  private studentService = inject(StudentService);
  private authService = inject(AuthService);

  age = computed(() => {
    const birthdate = new Date(this.studentDetails().birthdate);
    const today = new Date();
    let age = today.getFullYear() - birthdate.getFullYear();
    const monthDiff = today.getMonth() - birthdate.getMonth();

    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthdate.getDate())) {
      age--;
    }

    return age;
  });
}
