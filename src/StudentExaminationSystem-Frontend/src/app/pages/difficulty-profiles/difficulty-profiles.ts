import {Component, signal, inject, ViewChild} from '@angular/core';
import { DifficultyProfileService } from '../../core/services/difficulty-profile.service';
import { GetDifficultyProfileModel } from '../../core/models/difficulty-profile.model';
import { DifficultyProfileForm } from './difficulty-profile-form/difficulty-profile-form';
import {showDeleteSuccessMessage} from '../../shared/utils/form.utlis';
import {MessageService} from 'primeng/api';
import { DifficultyProfileList } from './difficulty-profile-list/difficulty-profile-list';
import {DeleteConfirmationDialog} from '../../shared/components/delete-confirmation-dialog/delete-confirmation-dialog';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-difficulty-profiles',
  templateUrl: './difficulty-profiles.html',
  imports: [
    DifficultyProfileForm,
    DeleteConfirmationDialog,
    DifficultyProfileList,
    ButtonModule,
  ],
  styleUrls: ['./difficulty-profiles.scss']
})
export class DifficultyProfilesPage {
  @ViewChild(DifficultyProfileList) difficultyProfileList!: DifficultyProfileList;

  // Difficulty profile form properties
  formVisible = signal(false);
  formMode = signal<'create' | 'edit'>('create');
  selectedDifficultyProfile = signal<GetDifficultyProfileModel | null>(null);

  deleteDialogVisible = false;
  difficultyProfileIdToDelete: number | null = null;
  messageService = inject(MessageService);
  difficultyProfileService = inject(DifficultyProfileService);

  openCreate() {
    this.formMode.set('create');
    this.selectedDifficultyProfile.set(null);
    this.formVisible.set(true);
  }

  openEdit(difficultyProfile: GetDifficultyProfileModel) {
    this.formMode.set('edit');
    this.selectedDifficultyProfile.set(difficultyProfile);
    this.formVisible.set(true);
  }

  closeForm() {
    this.formVisible.set(false);
  }

  onSaved(difficultyProfile: GetDifficultyProfileModel) {
    this.closeForm();
    if (this.formMode() === 'edit') {
      this.difficultyProfileList.updateDifficultyProfile(difficultyProfile);
    } else {
      this.difficultyProfileList.refreshData();
    }
  }

  deleteDifficultyProfile(id: number) {
    this.difficultyProfileIdToDelete = id;
    this.deleteDialogVisible = true;
  }

  onDeleteConfirm() {
    if (this.difficultyProfileIdToDelete !== null) {
      this.difficultyProfileService.deleteModel(this.difficultyProfileIdToDelete).subscribe({
        next: () => {
          this.difficultyProfileList.refreshData();
          this.deleteDialogVisible = false;
          this.difficultyProfileIdToDelete = null;
          showDeleteSuccessMessage(this.messageService, 'Difficulty Profile');
        }
      });
    }
  }

  onDeleteCancel() {
    this.deleteDialogVisible = false;
    this.difficultyProfileIdToDelete = null;
  }
}

