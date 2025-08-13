import {Component, Input, Output, EventEmitter, OnInit, OnChanges, signal, inject} from '@angular/core';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import {CreateSubjectConfigModel, GetSubjectConfigModel, UpdateSubjectConfigModel} from '../../../core/models/subject-config.model';
import {
  GeneratePatchDocument,
  isInvalid,
  showSuccessMessage,
  validateFormBeforeSubmit
} from '../../../shared/utils/form.utlis';
import { DialogModule } from 'primeng/dialog';
import {AutoFormErrorDirective} from '../../../shared/directives/auto-form-error.directive';
import {MessageService} from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import {SubjectConfigService} from '../../../core/services/subject-config.service';
import { InputNumberModule } from 'primeng/inputnumber';
import { SelectModule } from 'primeng/select';
import {DropdownModel} from '../../../core/models/common/common.model';
import {DifficultyProfileService} from '../../../core/services/difficulty-profile.service';
import {GetSubjectModel} from '../../../core/models/subject.model';


@Component({
  selector: 'app-subject-config-form',
  templateUrl: './subject-config-form.html',
  styleUrls: ['./subject-config-form.scss'],
  imports: [ReactiveFormsModule, DialogModule, AutoFormErrorDirective, ButtonModule, InputNumberModule, SelectModule]
})
export class SubjectConfigForm implements OnInit, OnChanges {
  @Input() visible: boolean = false;
  @Input() mode: 'create' | 'edit' | 'view' = 'create';
  @Input() subjectId!: number;
  @Output() close = new EventEmitter<void>();
  @Output() saved = new EventEmitter<boolean>();

  private messageService = inject(MessageService);
  private subjectConfigService = inject(SubjectConfigService);
  private difficultyProfileService = inject(DifficultyProfileService);

  dropdownLoading = signal<boolean>(false);
  subjectConfig = signal<GetSubjectConfigModel | null>(null);
  difficultyProfiles = signal<DropdownModel[]>([]);

  form = new FormGroup({
    totalQuestions: new FormControl<number>(0, [Validators.required, Validators.min(1)]),
    durationMinutes: new FormControl<number>(0, [Validators.required, Validators.min(1)]),
    difficultyProfileId: new FormControl<number>(1, [Validators.required, Validators.min(1)]),
  });

  ngOnInit() {
  }

  ngOnChanges() {
    if (this.visible) {
      if (this.mode === 'view' || this.mode === 'edit') {
        this.loadSubjectConfig();
      } else {
        this.setupForm();
      }

      if (this.mode === 'view') {
        this.form.disable();
      } else {
        this.form.enable();
      }

      if (this.mode !== 'view') {
        this.dropdownLoading.set(true);
        this.loadDifficultyProfiles();
      }
    }
  }

  private loadSubjectConfig() {
    this.subjectConfigService.getModelById(this.subjectId).subscribe({
      next: (config) => {
        if( this.mode === 'view' || this.mode === 'edit') {
          this.subjectConfig.set(config);
        }
        this.setupForm();
      },
    });
  }

  private loadDifficultyProfiles() {
    const subscription = this.difficultyProfileService.getDropdownOptions<DropdownModel>().subscribe({
      next: (profiles: DropdownModel[]) => {
        this.difficultyProfiles.set(profiles);
        this.dropdownLoading.set(false);
        if (this.mode === 'edit') {
          this.form.patchValue({
            difficultyProfileId: this.subjectConfig()?.difficultyProfileId || 1,
          });
          console.log(profiles);
        }
        subscription.unsubscribe();
      }
    })
  }

  private setupForm() {
    if (this.mode === 'create') {
      this.form.reset({
        totalQuestions: 0,
        durationMinutes: 0,
        difficultyProfileId: 1,
      });
      return;
    }
      this.form.patchValue({
        totalQuestions: this.subjectConfig()?.totalQuestions || 0,
        durationMinutes: this.subjectConfig()?.durationMinutes || 0,
        difficultyProfileId: this.subjectConfig()?.difficultyProfileId || 1,
      })
  }

  onSubmit() {
    if (this.mode === 'view') {
      this.onClose();
      return;
    }

    if (!validateFormBeforeSubmit(this.messageService, this.form)) {
      return;
    }

    if (this.mode === 'edit') {
      this.updateSubjectConfig();
    } else {
      this.createSubjectConfig();
    }
  }

  private updateSubjectConfig() {
    const updateModel: UpdateSubjectConfigModel = {
      totalQuestions: this.subjectConfig()?.totalQuestions,
      durationMinutes: this.subjectConfig()?.durationMinutes,
      difficultyProfileId: this.subjectConfig()?.difficultyProfileId,
    };

    // Generate patch document for the update
    const patchDoc = GeneratePatchDocument(this.form, updateModel);

    const subscription = this.subjectConfigService.updateSubjectConfig(this.subjectId, patchDoc).subscribe({
      next: (result) => {
        showSuccessMessage(this.messageService, 'update', 'Subject Configuration');
        this.saved.emit(true);
        subscription.unsubscribe();
        this.onClose();
      },
    });
  }

  private createSubjectConfig() {
    const newSubjectConfig: CreateSubjectConfigModel = {
      totalQuestions: this.form.value.totalQuestions!,
      durationMinutes: this.form.value.durationMinutes!,
      difficultyProfileId: this.form.value.difficultyProfileId!,
    };

    const subscription = this.subjectConfigService.createSubjectConfig(this.subjectId, newSubjectConfig).subscribe({
      next: (result) => {
        showSuccessMessage(this.messageService, 'create', 'Subject Configuration');
        this.saved.emit(true);
        subscription.unsubscribe();
        this.onClose();
      },
    });
  }

  onClose() {
    this.form.reset();
    this.form.markAsPristine();
    this.form.markAsUntouched();
    this.form.enable();
    this.close.emit();
  }

  getDialogHeader(): string {
    switch (this.mode) {
      case 'create':
        return 'Create Subject Configuration';
      case 'edit':
        return 'Edit Subject Configuration';
      case 'view':
        return 'View Subject Configuration';
      default:
        return 'Subject Configuration';
    }
  }

  getSubmitButtonLabel(): string {
    switch (this.mode) {
      case 'create':
        return 'Create';
      case 'edit':
        return 'Save';
      case 'view':
        return 'Close';
      default:
        return 'Save';
    }
  }

  isInvalid = isInvalid;
}
