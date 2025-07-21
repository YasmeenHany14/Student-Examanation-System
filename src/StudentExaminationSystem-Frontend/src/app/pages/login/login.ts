import {Component, inject} from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import {FormsModule} from '@angular/forms';
import {AuthService} from '../../core/services/auth.service';
import {LoginRequest} from '../../core/models/user.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  private authService = inject(AuthService);

  loginDetails: LoginRequest = {
    email: '',
    password: ''
  }

  onLogin() {
    this.authService.login(this.loginDetails).subscribe({
      next: (response) => {
        console.log('Login successful', response);
        // Handle successful login, e.g., redirect to dashboard
      },
      error: (error) => {
        console.error('Login failed', error);
        // Handle login error, e.g., show an error message
      }
    });
  }

}
