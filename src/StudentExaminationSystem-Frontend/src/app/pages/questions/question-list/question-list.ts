import { Component, Output, EventEmitter, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuestionListModel } from '../../../core/models/question.model';
import { GetSubjectModel } from '../../../core/models/subject.model';
import { QuestionService } from '../../../core/services/question.service';
import { SubjectService } from '../../../core/services/subject.service';
import {Difficulty, DifficultyDropdown} from '../../../core/enums/difficulty';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { Spinner } from '../../../shared/components/spinner/spinner';
import { NoDataToShowComponent } from '../../../shared/components/no-data-to-show/no-data-to-show';
import { ViewQuestionDialog } from '../view-question-dialog/view-question-dialog';
import { BaseResourceParametersModel } from '../../../core/models/resource-parameters/base-resource-parameters.model';
import {ToggleSwitch} from 'primeng/toggleswitch';
import { SelectModule } from 'primeng/select';
import {DropdownModel} from '../../../core/models/common/common.model';
import {ActivatedRoute, Router} from '@angular/router';
import {changeQueryParams, getFromQueryParams} from '../../../shared/utils/nav.utils';

@Component({
  selector: 'app-question-list',
  templateUrl: './question-list.html',
  imports: [
    CommonModule,
    TableModule,
    ButtonModule,
    ToggleSwitch,
    FormsModule,
    Spinner,
    NoDataToShowComponent,
    SelectModule,
    ViewQuestionDialog
  ],
  styleUrls: ['./question-list.scss']
})
export class QuestionList implements OnInit {
  questions = signal<QuestionListModel[]>([]);
  subjects = signal<GetSubjectModel[]>([]);
  difficultyOptions = signal<DropdownModel[]>([]);
  loading = signal(false);
  isError = signal(false);
  totalRecords = signal(0);
  isSubjectsLoading = signal(true);
  pageSize: number = 10;
  currentPage: number = 1;

  // Filters
  selectedSubject = signal<GetSubjectModel | null>(null);
  selectedDifficulty = signal<DropdownModel | null>(null);

  // View question dialog
  viewQuestionDialogVisible = signal(false);
  selectedQuestionForView = signal<QuestionListModel | null>(null);

  private questionService = inject(QuestionService);
  private subjectService = inject(SubjectService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  @Output() delete = new EventEmitter<number>();

  async ngOnInit() {
    this.setupDifficultyOptions();
    await this.loadSubjects().then( () => {
      const subjectId = getFromQueryParams(this.route, 'subjectId');
      if (subjectId) {
        const subject = this.subjects().find(s => s.id.toString() === subjectId);
        this.selectedSubject.set(subject || null);
      }
      this.loadQuestions();
    });
  }

  setupDifficultyOptions() {
    const options = DifficultyDropdown;
    this.difficultyOptions.set(options);
  }

  loadSubjects(): Promise<void> {
    this.isSubjectsLoading.set(true);
    return new Promise((resolve, reject) => {
      this.subjectService.getDropdownOptions<GetSubjectModel>().subscribe({
        next: (subjects) => {
          this.subjects.set(subjects);
          this.isSubjectsLoading.set(false);
          resolve();
        },
        error: (err) => {
          reject(err);
        }
      });
    });
  }

  onSubjectChange(subject: GetSubjectModel | null) {
    this.selectedSubject.set(subject);
    if (subject) {
      this.loadQuestions();
    } else {
      this.questions.set([]);
      this.totalRecords.set(0);
    }
    changeQueryParams(this.route, this.router, 'subjectId',  subject ? subject.id.toString() : '', subject ? 'Update' : 'Remove');
  }

  onDifficultyChange() {
    if (this.selectedSubject()) {
      this.loadQuestions();
    }
  }

  loadQuestions(event?: any) {
    if (!this.selectedSubject()) {
      return;
    }

    this.loading.set(true);
    const page = event ? event.first / event.rows + 1 : 1;
    const pageSize = event ? event.rows : this.pageSize;

    this.currentPage = page;
    this.pageSize = pageSize;

    const queryParams: any = {
      PageNumber: page,
      PageSize: pageSize,
      SubjectId: this.selectedSubject()!.id
    };

    if (this.selectedDifficulty()) {
      queryParams.DifficultyId = this.selectedDifficulty();
    }

    this.questionService.getAllPaged<BaseResourceParametersModel, QuestionListModel>(queryParams).subscribe({
      next: (result) => {
        this.questions.set(result.data);
        this.totalRecords.set(result.pagination.totalCount);
        this.loading.set(false);
        this.isError.set(false);
      },
      error: (_err) => {
        this.loading.set(false);
        this.isError.set(true);
      }
    });
  }

  onLazyLoad(event: any) {
    this.loadQuestions(event);
  }

  onDelete(id: number) {
    this.delete.emit(id);
  }

  viewQuestion(question: QuestionListModel) {
    const questionWithSubjectName = {
      ...question,
      subjectName: this.getSubjectName(question.subjectId)
    };
    this.selectedQuestionForView.set(questionWithSubjectName);
    this.viewQuestionDialogVisible.set(true);
  }

  closeViewDialog() {
    this.viewQuestionDialogVisible.set(false);
    this.selectedQuestionForView.set(null);
  }

  refreshData() {
    this.loadQuestions();
  }

  truncateText(text: string, maxLength: number = 15): string {
    return text.length > maxLength ? text.substring(0, maxLength) + '...' : text;
  }

  getDifficultyName(difficultyId: number): string {
    const option = this.difficultyOptions().find(d => d.id === difficultyId);
    return option ? option.name : 'Unknown';
  }

  getSubjectName(subjectId: number): string {
    const subject = this.subjects().find(s => s.id === subjectId);
    return subject ? subject.name : 'Unknown';
  }

  onToggleStatus(question: any) {
    // Store the original state in case we need to revert
    const originalState = question.isActive;

    const subscription = this.questionService.toggleStatus(question.id).subscribe({
      next: () => {
        this.updateQuestionStatus(question.id, !originalState);
        subscription.unsubscribe();
      },
      error: () => {
        this.updateQuestionStatus(question.id, originalState);
      }
    });
  }

  private updateQuestionStatus(questionId: number, status: boolean) {
    const questions = this.questions();
    const questionIndex = questions.findIndex(q => q.id === questionId);
    if (questionIndex !== -1) {
      const updatedQuestions = [...questions];
      updatedQuestions[questionIndex] = { ...updatedQuestions[questionIndex], isActive: status };
      this.questions.set(updatedQuestions);
    }
  }

}
