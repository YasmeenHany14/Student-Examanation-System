import { AbstractControl, ValidationErrors, FormArray } from '@angular/forms';

export function oneCorrectAnswerValidator(control: AbstractControl): ValidationErrors | null {
  if (!control.value || !Array.isArray(control.value)) {
    return null;
  }

  const correctAnswers = control.value.filter((choice: any) => choice.isCorrect === true);

  if (correctAnswers.length !== 1) {
    return { oneCorrectAnswerRequired: true };
  }

  return null;
}

export function choicesRangeValidator(control: AbstractControl): ValidationErrors | null {
  if (!control.value || !Array.isArray(control.value)) {
    return null;
  }

  const choicesCount = control.value.length;

  if (choicesCount < 2 || choicesCount > 5) {
    return { choicesRange: { min: 2, max: 5, actual: choicesCount } };
  }

  return null;
}
