import {Component, signal, inject, ViewChild} from '@angular/core';
import { QuestionService } from '../../core/services/question.service';
import { QuestionListModel } from '../../core/models/question.mode';
import {showDeleteSuccessMessage} from '../../shared/utils/form.utlis';
import {MessageService} from 'primeng/api';
import { QuestionList } from './question-list/question-list';
import {DeleteConfirmationDialog} from '../../shared/components/delete-confirmation-dialog/delete-confirmation-dialog';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-questions',
  templateUrl: './questions.html',
  imports: [
    DeleteConfirmationDialog,
    QuestionList,
    ButtonModule,
  ],
  styleUrls: ['./questions.scss']
})
export class QuestionsPage {
  @ViewChild(QuestionList) questionList!: QuestionList;

  deleteDialogVisible = false;
  questionIdToDelete: number | null = null;
  messageService = inject(MessageService);
  questionService = inject(QuestionService);

  openCreate() {
    // TODO: This will be implemented later
    console.log('Create question clicked');
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
