import {Component, Input, Output, EventEmitter, OnInit, signal, inject} from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import {CreateSubjectModel, GetSubjectModel} from '../../../core/models/subject.model';
import {GeneratePatchDocument, isInvalid, showSuccessMessage, validateFormBeforeSubmit} from '../../../shared/utils/form.utlis';
import { DialogModule } from 'primeng/dialog';
import {InputText} from 'primeng/inputtext';
import {AutoFormErrorDirective} from '../../../shared/directives/auto-form-error.directive';
import {MessageService} from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import {SubjectService} from '../../../core/services/subject.service';


@Component({
  selector: 'app-subject-form',
  templateUrl: './subject-form.html',
  styleUrls: ['./subject-form.scss'],
  imports: [ReactiveFormsModule, DialogModule, InputText, AutoFormErrorDirective, ButtonModule]
})
export class SubjectForm implements OnInit {
  @Input() visible: boolean = false;
  @Input() mode: 'create' | 'edit' = 'create';
  @Input() subject: GetSubjectModel = { id: 0, name: '', code: '', hasConfiguration: false };
  @Output() close = new EventEmitter<void>();
  @Output() saved = new EventEmitter<GetSubjectModel>();
  private messageService = inject(MessageService);
  private subjectService = inject(SubjectService);

  form = new FormGroup({
    name: new FormControl('', Validators.required),
    code: new FormControl('', Validators.required),
  });

  ngOnInit() {
    if (this.mode === 'edit' && this.subject) {
      this.form.patchValue({
        name: this.subject.name,
        code: this.subject.code,
      });
    }
  }

  ngOnChanges() {
    if (this.visible) {
      if (this.mode === 'edit' && this.subject) {
        this.form.patchValue({
          name: this.subject.name,
          code: this.subject.code,
        });
      } else {
        this.form.reset();
      }
    }
  }

  onSubmit() {
    if (!this.form.valid) {
      this.messageService.add({
        severity: 'error',
        summary: 'Validation Error',
        detail: 'Please fill in all required fields.',
      });
      this.form.markAllAsTouched();
      return;
    }

    if (this.mode === 'edit') {
      this.updateSubject();
    } else {
      this.createSubject();
    }
    this.saved.emit();

  }

  private updateSubject() {
    if (!validateFormBeforeSubmit(this.messageService, this.form)) {
      return;
    }
    const patchDoc = GeneratePatchDocument(this.form, this.subject, ["name", "code"]);
    const subscription = this.subjectService.updateModel(this.subject.id, patchDoc).subscribe({
      next: (result) => {
        showSuccessMessage(this.messageService, this.mode === 'edit' ? 'update' : 'create', 'Subject');
        this.saved.emit({
          id: this.subject.id,
          name: this.form.value.name!,
          code: this.form.value.code!,
        });
        subscription.unsubscribe();
        this.onClose();
      },
    })
  }

  private createSubject() {
    const newSubject: CreateSubjectModel = {
      name: this.form.value.name!,
      code: this.form.value.code!,
    }
    const subscription = this.subjectService.createModel<CreateSubjectModel>(newSubject).subscribe({
      next: (result) => {
        showSuccessMessage(this.messageService, this.mode === 'edit' ? 'update' : 'create', 'Subject');
        subscription.unsubscribe();
        this.onClose();
      }
    });
  }

  onClose() {
    this.form.reset();
    this.form.markAsPristine();
    this.form.markAsUntouched();
    this.close.emit();
  }

  isInvalid = isInvalid;
}
