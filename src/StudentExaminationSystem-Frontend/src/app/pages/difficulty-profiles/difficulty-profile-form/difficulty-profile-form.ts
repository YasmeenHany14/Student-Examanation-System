import {Component, Input, Output, EventEmitter, OnInit, inject, OnChanges} from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import {
  CreateDifficultyProfileModel,
  GetDifficultyProfileModel,
  UpdateDifficultyProfileModel
} from '../../../core/models/difficulty-profile.model';
import {GeneratePatchDocument, isInvalid, showSuccessMessage, validateFormBeforeSubmit} from '../../../shared/utils/form.utlis';
import { DialogModule } from 'primeng/dialog';
import {InputText} from 'primeng/inputtext';
import {InputNumberModule} from 'primeng/inputnumber';
import {AutoFormErrorDirective} from '../../../shared/directives/auto-form-error.directive';
import {MessageService} from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import {DifficultyProfileService} from '../../../core/services/difficulty-profile.service';
import {sumEquals100Validator} from './difficulty-profile.validator';
import {FormGroupErrorDirective} from '../../../shared/directives/form-group-error.directive';
import {Message} from 'primeng/message';

@Component({
  selector: 'app-difficulty-profile-form',
  templateUrl: './difficulty-profile-form.html',
  styleUrls: ['./difficulty-profile-form.scss'],
  imports: [ReactiveFormsModule, DialogModule, InputText, InputNumberModule, AutoFormErrorDirective, ButtonModule, FormGroupErrorDirective, Message]
})
export class DifficultyProfileForm implements OnChanges {
  @Input() visible: boolean = false;
  @Input() mode: 'create' | 'edit' = 'create';
  @Input() difficultyProfile: GetDifficultyProfileModel = {
    id: 0,
    name: '',
    easyQuestionsPercent: 0,
    mediumQuestionsPercent: 0,
    hardQuestionsPercent: 0
  };
  @Output() close = new EventEmitter<void>();
  @Output() saved = new EventEmitter<GetDifficultyProfileModel>();
  private messageService = inject(MessageService);
  private difficultyProfileService = inject(DifficultyProfileService);

  form = new FormGroup({
    name: new FormControl('', Validators.required),
    percentages: new FormGroup({
      easyQuestionsPercent: new FormControl(0, {
          validators: [Validators.required, Validators.min(0), Validators.max(100)],
          updateOn: 'blur'
        }),
      mediumQuestionsPercent: new FormControl(0, {
        validators: [Validators.required, Validators.min(0), Validators.max(100)],
          updateOn: 'blur'
        }),
      hardQuestionsPercent: new FormControl(0, {
        validators: [Validators.required, Validators.min(0), Validators.max(100)],
        updateOn: 'blur'
        }),
      }, [sumEquals100Validator])
  });

  ngOnChanges() {
    if (this.visible) {
      this.setupForm();
    }
  }

  onSubmit() {
    if (!validateFormBeforeSubmit(this.messageService, this.form)) {
      return;
    }

    if (this.mode === 'edit') {
      this.updateDifficultyProfile();
    } else {
      this.createDifficultyProfile();
    }
    this.saved.emit();
  }

  private updateDifficultyProfile() {
    const updateModel: UpdateDifficultyProfileModel = {
      name: this.form.value.name!,
      easyQuestionsPercent: this.difficultyProfile.easyQuestionsPercent!,
      mediumQuestionsPercent: this.difficultyProfile.mediumQuestionsPercent!,
      hardQuestionsPercent: this.difficultyProfile.hardQuestionsPercent!,
    }

    const formCopy = new FormGroup({
      name: this.form.controls.name,
      easyQuestionsPercent: this.form.controls.percentages?.controls.easyQuestionsPercent,
      mediumQuestionsPercent: this.form.controls.percentages?.controls.mediumQuestionsPercent,
      hardQuestionsPercent: this.form.controls.percentages?.controls.hardQuestionsPercent
    });

    const patchDoc = GeneratePatchDocument(formCopy, updateModel);
    const subscription = this.difficultyProfileService.updateModel(this.difficultyProfile.id, patchDoc).subscribe({
      next: (result) => {
        showSuccessMessage(this.messageService, this.mode === 'edit' ? 'update' : 'create', 'Difficulty Profile');
        this.saved.emit({
          id: this.difficultyProfile.id,
          name: this.form.value.name!,
          easyQuestionsPercent: this.form.value.percentages?.easyQuestionsPercent!,
          mediumQuestionsPercent: this.form.value.percentages?.mediumQuestionsPercent!,
          hardQuestionsPercent: this.form.value.percentages?.hardQuestionsPercent!,
        });
        subscription.unsubscribe();
        this.onClose();
      },
    })
  }

  private createDifficultyProfile() {
    const newDifficultyProfile: CreateDifficultyProfileModel = {
      name: this.form.value.name!,
      easyQuestionsPercent: this.form.value.percentages?.easyQuestionsPercent!,
      mediumQuestionsPercent: this.form.value.percentages?.mediumQuestionsPercent!,
      hardQuestionsPercent: this.form.value.percentages?.hardQuestionsPercent!,
    }
    const subscription = this.difficultyProfileService.createModel<CreateDifficultyProfileModel>(newDifficultyProfile).subscribe({
      next: (result) => {
        showSuccessMessage(this.messageService, this.mode === 'edit' ? 'update' : 'create', 'Difficulty Profile');
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

  private setupForm() {
    if (this.mode === 'edit') {
      this.form.patchValue({
        name: this.difficultyProfile.name,
        percentages: {
          easyQuestionsPercent: this.difficultyProfile.easyQuestionsPercent,
          mediumQuestionsPercent: this.difficultyProfile.mediumQuestionsPercent,
          hardQuestionsPercent: this.difficultyProfile.hardQuestionsPercent,
        }
      })
    } else {
      this.form.reset({
        name: '',
        percentages: {
          easyQuestionsPercent: 0,
          mediumQuestionsPercent: 0,
          hardQuestionsPercent: 0
        }
      });
    }
  }
}
