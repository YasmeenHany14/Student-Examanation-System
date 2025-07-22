import {AbstractControl, ValidationErrors} from '@angular/forms';

export function passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
  const password = control.get('password');
  const confirmPassword = control.get('confirmPassword');

  if (password && confirmPassword && password.value !== confirmPassword.value) {
    return { passwordMismatch: true };
  }
  return null;
}

export function birthdateValidator(control: AbstractControl): ValidationErrors | null {
  const birthdate = control.value;
  const today = new Date();
  const birthDateObj = new Date(birthdate);

  // Check if the birthdate is a valid date
  if (isNaN(birthDateObj.getTime())) {
    return { invalidBirthdate: true };
  }

  // Check if the birthdate is in the future
  if (birthDateObj > today) {
    return { futureBirthdate: true };
  }

  return null;
}

export function passwordValidator(control: AbstractControl): ValidationErrors | null {
  const password = control.value;
  const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
  if (!regex.test(password)) {
    return { weakPassword: true };
  }

  return null;
}
