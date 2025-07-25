import {FormGroup} from '@angular/forms';
import * as jsonpatch from 'fast-json-patch';
import { applyOperation } from 'fast-json-patch';
import { MessageService } from 'primeng/api';

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

export function GeneratePatchDocument(
  form: FormGroup,
  entity: Object | any[],
  fieldsToCheck: string[] = []
){
  const patchDoc = jsonpatch.compare(entity, form.value);

  return patchDoc.filter(op => {
    if (op.op !== 'replace' && op.op !== 'add') return false;

    // If no field filter, include all
    if (fieldsToCheck.length === 0) return true;

    // Extract the top-level field name
    const pathParts = op.path.split('/').filter(p => p);
    const topLevelField = pathParts[0];

    // Check if this field is in our filter list
    return fieldsToCheck.includes(topLevelField);
  });
}

export function showSuccessMessage(messageService: MessageService, mode: 'create' | 'update', entityName: string) {
  messageService.add({
    severity: 'success',
    summary: 'Success',
    detail: `${entityName} ${mode === 'create' ? 'created' : 'updated'} successfully.`,
  });
}

export function showDeleteSuccessMessage(messageService: MessageService, entityName: string) {
  messageService.add({
    severity: 'success',
    summary: 'Success',
    detail: `${entityName} deleted successfully.`,
  });
}

export function validateFormBeforeSubmit(messageService: MessageService, form: FormGroup): boolean {
  if (form.invalid) {
    form.markAllAsTouched();
    messageService.add({
      severity: 'error',
      summary: 'Validation Error',
      detail: 'Please fix all form validation problems first.',
    });
    return false;
  }
  return true;
}
