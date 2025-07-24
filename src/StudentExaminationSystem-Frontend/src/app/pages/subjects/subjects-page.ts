import {Component, signal, inject, ViewChild} from '@angular/core';
import { SubjectService } from '../../core/services/subject.service';
import { GetSubjectModel } from '../../core/models/subject.model';
import { ButtonModule } from 'primeng/button';
import { SubjectForm } from './subject-form/subject-form';
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
    DeleteConfirmationDialog,
    SubjectList
  ],
  styleUrls: ['./subjects-page.scss']
})
export class SubjectsPage {
  @ViewChild(SubjectList) subjectList!: SubjectList;

  formVisible = signal(false);
  formMode = signal<'create' | 'edit'>('create');
  selectedSubject = signal<GetSubjectModel | null>(null);
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
      this.subjectService.deleteSubject(this.subjectIdToDelete).subscribe({
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
}
