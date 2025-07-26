import {Component, signal, inject, ViewChild} from '@angular/core';
import { QuestionService } from '../../core/services/question.service';
import { QuestionForm } from './question-form/question-form';
import {showDeleteSuccessMessage} from '../../shared/utils/form.utlis';
import {MessageService} from 'primeng/api';
import { QuestionList } from './question-list/question-list';
import {DeleteConfirmationDialog} from '../../shared/components/delete-confirmation-dialog/delete-confirmation-dialog';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-questions',
  templateUrl: './questions.html',
  imports: [
    QuestionForm,
    DeleteConfirmationDialog,
    QuestionList,
    ButtonModule,
  ],
  styleUrls: ['./questions.scss']
})
export class QuestionsPage {
  @ViewChild(QuestionList) questionList!: QuestionList;

  // Question form properties
  formVisible = signal(false);

  deleteDialogVisible = false;
  questionIdToDelete: number | null = null;
  messageService = inject(MessageService);
  questionService = inject(QuestionService);

  openCreate() {
    this.formVisible.set(true);
  }

  closeForm() {
    this.formVisible.set(false);
  }

  onSaved() {
    this.questionList.refreshData();
    this.formVisible.set(false);
  }

  deleteQuestion(id: number) {
    this.questionIdToDelete = id;
    this.deleteDialogVisible = true;
  }

  onDeleteConfirm() {
    if (this.questionIdToDelete !== null) {
      this.questionService.deleteModel(this.questionIdToDelete).subscribe({
        next: () => {
          this.questionList.refreshData();
          this.deleteDialogVisible = false;
          this.questionIdToDelete = null;
          showDeleteSuccessMessage(this.messageService, 'Question');
        }
      });
    }
  }

  onDeleteCancel() {
    this.deleteDialogVisible = false;
    this.questionIdToDelete = null;
  }
}
