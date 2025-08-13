import { Component, OnInit, signal, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ExamListModel } from '../../core/models/exam.model';
import { ExamService } from '../../core/services/exam.service';
import { AuthService } from '../../core/services/auth.service';
import {TableLazyLoadEvent, TableModule} from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { Spinner } from '../../shared/components/spinner/spinner';
import { NoDataToShowComponent } from '../../shared/components/no-data-to-show/no-data-to-show';
import { DatePipe } from '@angular/common';
import {ExamStatus} from '../../core/enums/exam-status';
import {ExamResourceParametersModel} from '../../core/models/resource-parameters/exam-resource-parameters.model';
import {FormsModule} from '@angular/forms';
import {IconField} from 'primeng/iconfield';
import {InputIcon} from 'primeng/inputicon';
import {InputText} from 'primeng/inputtext';
import {Toolbar} from 'primeng/toolbar';
import {SortEvent} from 'primeng/api';
import {routes} from '../../core/constants/routs';

@Component({
  selector: 'app-exam-list',
  imports: [
    TableModule,
    ButtonModule,
    TagModule,
    Spinner,
    NoDataToShowComponent,
    DatePipe,
    FormsModule,
    IconField,
    InputIcon,
    InputText,
    Toolbar
  ],
  templateUrl: './exam-list.html',
  styleUrl: './exam-list.scss'
})
export class ExamList implements OnInit {
  exams = signal<ExamListModel[]>([]);
  loading = signal(false);
  isError = signal(false);
  totalRecords = signal(0);

  paginationInfo = signal<ExamResourceParametersModel>({
    PageNumber: 1,
    PageSize: 10,
    OrderBy: null,
    searchQuery: null,
  });

  searchQuery = "";

  private examService = inject(ExamService);
  private authService = inject(AuthService);
  private router = inject(Router);


  ngOnInit() {
    this.loadExams();
  }

  loadExams() {
    this.loading.set(true);
    this.isError.set(false);

    this.examService.getAllPaged<ExamResourceParametersModel, ExamListModel>(this.paginationInfo()).subscribe({
      next: (result) => {
        this.exams.set(result.data);
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

  onLazyLoad(event: TableLazyLoadEvent) {
    this.paginationInfo().PageNumber = event ? event.first! / event.rows! + 1 : 1;
    this.paginationInfo().PageSize = event ? event.rows! : this.paginationInfo().PageSize;
    this.loadExams();
  }

  onViewDetails(exam: ExamListModel) {
    // 1- add examId to query params
    this.router.navigate(["home/exam-details"], {
      queryParams: { examId: exam.id },
      queryParamsHandling: 'merge'
    });
  }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  getExamTag(exam: ExamListModel): string {
    let tag = ExamStatus[exam.examStatus] || 'Unknown';
    if (exam.examStatus === ExamStatus.Completed) {
      tag = exam.passed ? 'Passed' : 'Failed';
    }
    return tag;
  }

  getExamTagSeverity(exam: ExamListModel): string {
    let severity = 'info';
    if (exam.examStatus === ExamStatus.Completed) {
      severity = exam.passed ? 'success' : 'danger';
    } else if (exam.examStatus === ExamStatus.Running || exam.examStatus === ExamStatus.PendingEvaluation) {
      severity = 'warn';
    }
    return severity;
  }

  refreshData() {
    this.loadExams();
  }

  customSort(event: SortEvent) {
    const sortField = event.field;
    const sortOrder = event.order !== 1 ? 'asc' : 'desc';
    this.paginationInfo().OrderBy = sortField ? `${sortField} ${sortOrder}` : null;
  }

  onSearchExam() {
    if (this.searchQuery.trim() === "") {
      if (this.paginationInfo().searchQuery === "")
        return;
    }
    this.paginationInfo().searchQuery = this.searchQuery === "" ? null : this.searchQuery;
    this.paginationInfo().PageNumber = 1;
    this.loadExams();
  }

  goToRunningExam(subjectId: number) {
    if (subjectId) {
      this.router.navigate([routes.home + routes.exam + "/" + subjectId]);
    }
  }
}
