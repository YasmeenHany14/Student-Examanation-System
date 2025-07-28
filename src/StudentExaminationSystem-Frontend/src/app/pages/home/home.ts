import {Component, inject} from '@angular/core';
import {AuthService} from '../../core/services/auth.service';
import {Dashboard} from '../../components/dashboard/dashboard';
import {ExamList} from '../exam-list/exam-list';

@Component({
  selector: 'app-home',
  imports: [
    Dashboard,
    ExamList
  ],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home {
  private authService = inject(AuthService);

  isAdmin = this.authService.isAdmin();
}
