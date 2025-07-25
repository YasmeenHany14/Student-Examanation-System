import {Component, signal, inject, ViewChild} from '@angular/core';
import { SubjectService } from '../../core/services/subject.service';
import { SubjectConfigService } from '../../core/services/subject-config.service';
import { GetSubjectModel } from '../../core/models/subject.model';
import { ButtonModule } from 'primeng/button';
import { SubjectForm } from './subject-form/subject-form';
import { SubjectConfigForm } from './subject-config-form/subject-config-form';
import { DeleteConfirmationDialog } from '../../shared/components/delete-confirmation-dialog/delete-confirmation-dialog';
import {showDeleteSuccessMessage} from '../../shared/utils/form.utlis';
import {MessageService} from 'primeng/api';
import { SubjectList } from './subject-list/subject-list';

@Component({
  selector: 'app-subjects-page',
  templateUrl: './subjects-page.html',
  imports: [
    ButtonModule,
    SubjectForm,
    SubjectConfigForm,
    DeleteConfirmationDialog,
    SubjectList,
  ],
  styleUrls: ['./subjects-page.scss']
})
export class SubjectsPage {
  @ViewChild(SubjectList) subjectList!: SubjectList;

  // Subject form properties
  formVisible = signal(false);
  formMode = signal<'create' | 'edit'>('create');
  selectedSubject = signal<GetSubjectModel | null>(null);

  // Subject config form properties
  configFormVisible = signal(false);
  configFormMode = signal<'create' | 'edit' | 'view'>('create');
  selectedSubjectId = signal<number | null>(null);

  deleteDialogVisible = false;
  subjectIdToDelete: number | null = null;
  messageService = inject(MessageService);
  subjectService = inject(SubjectService);

  openCreate() {
    this.formMode.set('create');
    this.selectedSubject.set(null);
    this.formVisible.set(true);
  }

  openEdit(subject: GetSubjectModel) {
    this.formMode.set('edit');
    this.selectedSubject.set(subject);
    this.formVisible.set(true);
  }

  closeForm() {
    this.formVisible.set(false);
  }

  onSaved(subject: GetSubjectModel) {
    this.closeForm();
    if (this.formMode() === 'edit') {
      this.subjectList.updateSubject(subject);
    } else {
      this.subjectList.refreshData();
    }
  }

  deleteSubject(id: number) {
    this.subjectIdToDelete = id;
    this.deleteDialogVisible = true;
  }

  onDeleteConfirm() {
    if (this.subjectIdToDelete !== null) {
      this.subjectService.deleteModel(this.subjectIdToDelete).subscribe({
        next: () => {
          this.subjectList.refreshData();
          this.deleteDialogVisible = false;
          this.subjectIdToDelete = null;
          showDeleteSuccessMessage(this.messageService, 'Subject');
        }
      });
    }
  }

  onDeleteCancel() {
    this.deleteDialogVisible = false;
    this.subjectIdToDelete = null;
  }

  openConfigForm(id: number, mode: 'create' | 'edit' | 'view') {
    this.selectedSubjectId.set(id);
    this.configFormMode.set(mode);
    this.configFormVisible.set(true);
  }

  closeConfigForm() {
    this.configFormVisible.set(false);
    this.selectedSubjectId.set(null);
  }

  onConfigSaved(success: boolean) {
    const subjectId = this.selectedSubjectId();
    this.closeConfigForm();
    // Update the subject's hasConfiguration property and refresh the list if needed
    if (success && subjectId != null) {
      const subjects = this.subjectList.subjects();
      const subject = subjects.find(s => s.id === subjectId);
      if (subject) {
        subject.hasConfiguration = true;
        this.subjectList.updateSubject(subject);
      }
    }
  }

  openViewConfig($event: number) {
    this.openConfigForm($event, 'view');
  }

  openEditConfig($event: number) {
    this.openConfigForm($event, 'edit');
  }

  openCreateConfig($event: number) {
    this.openConfigForm($event, 'create');
  }
}
