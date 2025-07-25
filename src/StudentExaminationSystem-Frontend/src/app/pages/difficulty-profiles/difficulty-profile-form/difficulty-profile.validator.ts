import {AbstractControl, FormGroup, ValidationErrors} from '@angular/forms';

export function sumEquals100Validator(control: AbstractControl): ValidationErrors | null {
  const easy = control.get('easyQuestionsPercent')?.value || 0;
  const medium = control.get('mediumQuestionsPercent')?.value || 0;
  const hard = control.get('hardQuestionsPercent')?.value || 0;

  const total = easy + medium + hard;

  return total === 100 ? null : { sumNotEqualTo100: true };
}
