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

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule,
    InputTextModule,
    ReactiveFormsModule,
    Message,
    ButtonModule,
    IftaLabel
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
    this.authService.login(this.loginDetails).subscribe({
      next: (response) => {
        this.messageService.add({
          severity: 'success',
          summary: 'Login Successful',
          detail: 'You have successfully logged in.',
          life: 3000,
          closable: true
        })
      },
    });
  }
}
