import {FormGroup} from '@angular/forms';

export function getErrorMessages(form: FormGroup, controlName: string): string[] {
  const control = form.get(controlName);
  if (!control || !control.errors) return [];

  return Object.keys(control.errors).map(key => {
    const error = control.errors![key];
    switch (key) {
      case 'required': return 'This field is required';
      case 'minlength': return `Minimum length is ${error.requiredLength}`;
      case 'maxlength': return `Maximum length is ${error.requiredLength}`;
      case 'email': return 'Please enter a valid email address';
      case 'pattern': return 'Invalid format';
      case 'weakPassword': return 'Password should contain at least 1 Uppercase, 1 Lowercase, 1 Number and 1 Special Character';
      case 'passwordMismatch': return 'Passwords do not match';
      case 'invalidBirthdate': return 'Birthdate cannot be in the future or invalid';
      default: return 'Invalid value';
    }
  });
}

export function isInvalid(form: FormGroup, controlName: string): boolean {
  const control = form.get(controlName);
  return !!(control && control.invalid && (control.dirty || control.touched));
}
