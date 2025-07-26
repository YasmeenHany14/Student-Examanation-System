import {Component, Input, Output, EventEmitter, inject, OnChanges, OnInit} from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule, FormArray } from '@angular/forms';
import { CreateQuestionModel } from '../../../core/models/question.model';
import {isInvalid, showSuccessMessage, validateFormBeforeSubmit} from '../../../shared/utils/form.utlis';
import { DialogModule } from 'primeng/dialog';
import {InputText} from 'primeng/inputtext';
import {AutoFormErrorDirective} from '../../../shared/directives/auto-form-error.directive';
import {MessageService} from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import {QuestionService} from '../../../core/services/question.service';
import {oneCorrectAnswerValidator, choicesRangeValidator} from './question.validator';
import { SelectModule } from 'primeng/select';
import { CheckboxModule } from 'primeng/checkbox';
import { SubjectService } from '../../../core/services/subject.service';
import { CommonModule } from '@angular/common';
import { DifficultyDropdown } from '../../../core/enums/difficulty';
import {DropdownModel} from '../../../core/models/common/common.model';


@Component({
  selector: 'app-question-form',
  templateUrl: './question-form.html',
  styleUrls: ['./question-form.scss'],
  imports: [
    ReactiveFormsModule,
    DialogModule,
    InputText,
    AutoFormErrorDirective,
    ButtonModule,
    SelectModule,
    CheckboxModule,
    CommonModule
  ]
})
export class QuestionForm implements OnChanges, OnInit {
  @Input() visible: boolean = false;

  @Output() close = new EventEmitter<void>();
  @Output() saved = new EventEmitter();

  private messageService = inject(MessageService);
  private questionService = inject(QuestionService);
  private subjectService = inject(SubjectService);

  subjects: DropdownModel[] = [];
  difficulties: DropdownModel[] = [];

  form = new FormGroup({
    content: new FormControl('', [
      Validators.required,
      Validators.maxLength(300)
    ]),
    subjectId: new FormControl(0, Validators.required),
    difficultyId: new FormControl(0, Validators.required),
    questionChoices: new FormArray([], [
      Validators.required,
      choicesRangeValidator,
      oneCorrectAnswerValidator
    ])
  });

  get questionChoicesArray() {
    return this.form.get('questionChoices') as FormArray;
  }

  ngOnInit() {
    this.loadDropdownData();
  }

  ngOnChanges() {
    if (this.visible) {
      this.setupForm();
    }
  }

  private loadDropdownData() {
    this.subjectService.getDropdownOptions().subscribe({
      next: (subjects) => {
        this.subjects = subjects;
      }
    });

    this.difficulties = DifficultyDropdown;
  }

  private createChoiceFormGroup(content: string = '', isCorrect: boolean = false) {
    return new FormGroup({
      content: new FormControl(content, [
        Validators.required,
        Validators.maxLength(150),
        Validators.minLength(1),
      ]),
      isCorrect: new FormControl(isCorrect)
    });
  }

  addChoice() {
    if (this.questionChoicesArray.length < 5) {
      this.questionChoicesArray.push(this.createChoiceFormGroup());
    }
  }

  removeChoice(index: number) {
    if (this.questionChoicesArray.length > 2) {
      this.questionChoicesArray.removeAt(index);
    }
  }

  onCorrectAnswerChange(index: number) {
    this.questionChoicesArray.controls.forEach((control, i) => {
      if (i !== index) {
        control.get('isCorrect')?.setValue(false);
      }
    });
  }

  canAddChoice(): boolean {
    return this.questionChoicesArray.length < 5;
  }

  canRemoveChoice(): boolean {
    return this.questionChoicesArray.length > 2;
  }

  onSubmit() {
    if (!validateFormBeforeSubmit(this.messageService, this.form)) {
      return;
    }
    this.createQuestion();
  }

  private createQuestion() {
    const newQuestion: CreateQuestionModel = {
      content: this.form.value.content!,
      subjectId: this.form.value.subjectId!,
      difficultyId: this.form.value.difficultyId!,
      questionChoices: this.form.value.questionChoices!
    };

    const subscription = this.questionService.createModel<CreateQuestionModel>(newQuestion).subscribe({
      next: (result) => {
        showSuccessMessage(this.messageService, 'create', 'Question');
        subscription.unsubscribe();
        this.onClose();
      }
    });
  }

  onClose() {
    this.form.reset();
    this.form.markAsPristine();
    this.form.markAsUntouched();
    this.questionChoicesArray.clear();
    this.close.emit();
  }

  isInvalid = isInvalid;

  private setupForm() {
    this.form.reset({
      content: '',
      subjectId: 0,
      difficultyId: 0,
    });

    // Initialize with 2 empty choices
    this.questionChoicesArray.clear();
    this.addChoice();
    this.addChoice();
  }
}
