import {Component, inject} from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService} from '../../core/services/auth.service';
import {LoginRequest} from '../../core/models/user.model';
import {Message} from 'primeng/message';
import {getErrorMessages, isInvalid} from '../../shared/utils/form.utlis';
import {IftaLabel} from 'primeng/iftalabel';
import {MessageService} from 'primeng/api';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule,
    InputTextModule,
    ReactiveFormsModule,
    Message,
    ButtonModule,
    IftaLabel,
    RouterLink
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  private readonly messageService = inject(MessageService);
  private authService = inject(AuthService);

  loginDetails: LoginRequest = {
    email: '',
    password: ''
  }

  loginForm = new FormGroup({
    email: new FormControl('', {
      validators: [Validators.required, Validators.email, Validators.minLength(6)],
      updateOn: 'blur'
    }),
    password: new FormControl('', {
      validators: [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(20)
      ],
      updateOn: 'blur'
    })
  })

  getErrorMessages = getErrorMessages;
  isInvalid = isInvalid;

  onSubmit() {
    this.loginDetails.email = this.loginForm.value.email!;
    this.loginDetails.password = this.loginForm.value.password!;

    this.authService.login(this.loginDetails).subscribe({
      next: (response) => {
        this.messageService.add({
          severity: 'success',
          summary: 'Login Successful',
          detail: 'You have successfully logged in, welcome back!',
          life: 3000,
          closable: true
        })
        // Redirect to the home page or dashboard
        this.authService.redirectAfterLogin();
      },
    });
  }
}
