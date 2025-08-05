import { Component, Output, EventEmitter, OnInit, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuestionListModel } from '../../../core/models/question.model';
import { GetSubjectModel } from '../../../core/models/subject.model';
import { QuestionService } from '../../../core/services/question.service';
import { SubjectService } from '../../../core/services/subject.service';
import {Difficulty, DifficultyDropdown} from '../../../core/enums/difficulty';
import {TableLazyLoadEvent, TableModule} from 'primeng/table';
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
import {
  QuestionResourceParametersModel
} from '../../../core/models/resource-parameters/question-resource-parameters.model';
import {Toolbar} from 'primeng/toolbar';
import {IconField} from 'primeng/iconfield';
import {InputIcon} from 'primeng/inputicon';
import {InputText} from 'primeng/inputtext';
import {SortEvent} from 'primeng/api';

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
    ViewQuestionDialog,
    Toolbar,
    IconField,
    InputIcon,
    InputText
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
  paginationInfo = signal<QuestionResourceParametersModel>({
    PageNumber: 1,
    PageSize: 10,
    OrderBy: null,
    subjectId: null,
    difficultyId: null,
    searchQuery: null
  });

  // View question dialog
  viewQuestionDialogVisible = signal(false);
  selectedQuestionForView = signal<QuestionListModel | null>(null);
  searchQuery = "";

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
        this.paginationInfo().subjectId = subject ? subject.id : null;
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

  onSubjectChange(subject: number | null) {
    this.paginationInfo().subjectId = subject;
    this.paginationInfo().PageNumber = 1;

    if (subject) {
      this.loadQuestions();
    } else {
      this.questions.set([]);
      this.totalRecords.set(0);
    }
    changeQueryParams(this.route, this.router, 'subjectId',  subject ? subject.toString() : '', subject ? 'Update' : 'Remove');
  }

  onDifficultyChange(difficulty: number | null) {
    this.paginationInfo().difficultyId = difficulty ? difficulty : null;

    if (this.paginationInfo().subjectId) {
      this.loadQuestions();
    }
  }

  loadQuestions() {
    if (!this.paginationInfo().subjectId) {
      return;
    }
    this.loading.set(true);
    this.isError.set(false);

    this.questionService.getAllPaged<QuestionResourceParametersModel, QuestionListModel>(this.paginationInfo()).subscribe({
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

  customSort(event: SortEvent) {
    const sortField = event.field;
    const sortOrder = event.order !== 1 ? 'asc' : 'desc';
    const pagination = this.paginationInfo();
    this.paginationInfo().OrderBy = sortField ? `${sortField} ${sortOrder}` : null;
  }
  onLazyLoad(event: TableLazyLoadEvent) {
    const pagination = this.paginationInfo();
    this.paginationInfo.set({
      ...pagination,
      PageNumber: event ? event.first! / event.rows! + 1 : 1,
      PageSize: event ? event.rows! : pagination.PageSize
    });
    this.loadQuestions();
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

  onSearchQuestion() {
    if (this.searchQuery.trim() === "") {
      if (this.paginationInfo().searchQuery === "")
        return;
    }
    this.paginationInfo().searchQuery = this.searchQuery == "" ? null : this.searchQuery;
    this.loadQuestions();
  }
}
