import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MessageModule } from 'primeng/message';
import { CommonModule } from '@angular/common';
import { getErrorMessages, isInvalid } from '../../utils/form.utlis';

@Component({
  selector: 'app-form-error',
  standalone: true,
  imports: [MessageModule, CommonModule],
  template: `
    @if (isInvalid(form, fieldName)) {
      @for (errorMsg of getErrorMessages(form, fieldName); track errorMsg) {
        <p-message severity="error" size="small" variant="simple">{{ errorMsg }}</p-message>
      }
    }
  `
})
export class FormErrorComponent {
  @Input() form!: FormGroup;
  @Input() fieldName!: string;

  isInvalid = isInvalid;
  getErrorMessages = getErrorMessages;
}
