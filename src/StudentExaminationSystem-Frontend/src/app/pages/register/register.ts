import {Component, inject, OnInit, signal} from '@angular/core';
import {RegisterStudentRequest} from '../../core/models/student.model';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {getErrorMessages, isInvalid} from '../../shared/utils/form.utlis';
import {MessageService} from 'primeng/api';
import {AuthService} from '../../core/services/auth.service';
import {birthdateValidator, passwordMatchValidator} from './register.validators';
import {InputText} from 'primeng/inputtext';
import { DatePickerModule } from 'primeng/datepicker';
import {Password} from 'primeng/password';
import {Message} from 'primeng/message';
import {ButtonModule} from 'primeng/button';
import {IftaLabel} from 'primeng/iftalabel';
import {Select} from 'primeng/select';
import {gender} from '../../core/enums/gender';
import {MultiSelect} from 'primeng/multiselect';
import {SubjectService} from '../../core/services/subject.service';
import {DropdownModel} from '../../core/models/common/common.model';
import {routes} from '../../core/constants/routs';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [
    FormsModule,
    InputText,
    ReactiveFormsModule,
    Message,
    ButtonModule,
    IftaLabel,
    Password,
    DatePickerModule,
    Select,
    MultiSelect,
    RouterLink
  ],
  templateUrl: './register.html',
  styleUrl: './register.scss'
})
export class Register implements OnInit {
  private readonly messageService = inject(MessageService);
  private authService = inject(AuthService);
  private subjectService = inject(SubjectService);
  courses = signal<DropdownModel[]>([]);
  loading = signal(true);

  ngOnInit(): void {
    const subscription = this.subjectService.getSubjectsDropdown().subscribe({
      next: (subjects) => {
        this.courses.set(subjects);
        this.loading.set(false);
        subscription.unsubscribe();
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to load subjects.',
          life: 3000,
          closable: true
        })
      }
    })
  }

  genders = [
    { id: gender.female, label: 'Female' },
    { id: gender.male, label: 'Male' }
  ];

  registerDetails: RegisterStudentRequest = {
    email: '',
    password: '',
    firstName: '',
    lastName: '',
    confirmPassword: '',
    birthdate: (new Date()).toISOString(),
    gender: 0,
    joinDate: (new Date()).toISOString(),
    courseIds: []
  }

  // create birthdate and password validators

  signUpForm = new FormGroup({
    email: new FormControl<string>(this.registerDetails.email, {
      validators: [Validators.required, Validators.email, Validators.minLength(6)],
      updateOn: 'blur',
    }),
    firstName: new FormControl(this.registerDetails.firstName, {
      validators: [Validators.required, Validators.minLength(2), Validators.maxLength(20)],
      updateOn: 'blur',
    }),
    lastName: new FormControl(this.registerDetails.lastName, {
      validators: [Validators.required, Validators.minLength(2), Validators.maxLength(20)],
      updateOn: 'blur',
    }),
    password: new FormControl(this.registerDetails.password, {
      validators: [Validators.required, Validators.minLength(8), Validators.maxLength(20)],
      updateOn: 'blur',
    }),
    confirmPassword: new FormControl(this.registerDetails.confirmPassword, {
      validators: [passwordMatchValidator],
      updateOn: 'blur',
    }),
    birthdate: new FormControl(this.registerDetails.birthdate, {
      validators: [Validators.required, birthdateValidator],
    }),
    gender: new FormControl(0, {
      validators: [Validators.required],
    }),
    joinDate: new FormControl(this.registerDetails.joinDate, {
      validators: [Validators.required],
    }),
    courseIds: new FormControl(this.registerDetails.courseIds, {
      validators: [Validators.required],
    })
  })

  getErrorMessages = getErrorMessages;
  isInvalid = isInvalid;

  onSubmit() {
    if (this.signUpForm.invalid) {
      this.messageService.add({
        severity: 'error',
        summary: 'Form Error',
        detail: 'Please correct the errors in the form.',
        life: 3000,
        closable: true
      });
      return;
    }

    this.fillFromForm();
    // this.registerDetails = this.signUpForm.value
    const subscription = this.authService.registerStudent(this.registerDetails).subscribe({
      next: (response) => {
        this.messageService.add({
          severity: 'success',
          summary: 'Registration Successful',
          detail: 'You have successfully registered, you will now be redirected to the login page.',
          life: 3000,
          closable: true
        });
        subscription.unsubscribe();
        setTimeout(() => {
          window.location.href = routes.authLogin;
        }, 5000);
      }
    })
  }


  private fillFromForm() {
    this.registerDetails.email = this.signUpForm.value.email!;
    this.registerDetails.firstName = this.signUpForm.value.firstName!;
    this.registerDetails.lastName = this.signUpForm.value.lastName!;
    this.registerDetails.password = this.signUpForm.value.password!;
    this.registerDetails.confirmPassword = this.signUpForm.value.confirmPassword!;
    this.registerDetails.gender = this.signUpForm.value.gender!;
    this.registerDetails.courseIds = this.signUpForm.value.courseIds!;
    this.registerDetails.birthdate = (this.signUpForm.value.birthdate! as unknown as Date).toISOString().split('T')[0];
    this.registerDetails.joinDate = (this.signUpForm.value.joinDate! as unknown as Date).toISOString();
  }
}
