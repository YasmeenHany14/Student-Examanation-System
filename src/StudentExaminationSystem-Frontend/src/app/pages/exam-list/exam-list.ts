import { Component, OnInit, signal, inject } from '@angular/core';
import { ExamListModel } from '../../core/models/exam.model';
import { ExamService } from '../../core/services/exam.service';
import { AuthService } from '../../core/services/auth.service';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { Spinner } from '../../shared/components/spinner/spinner';
import { NoDataToShowComponent } from '../../shared/components/no-data-to-show/no-data-to-show';
import { BaseResourceParametersModel } from '../../core/models/common/base-resource-parameters.model';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-exam-list',
  imports: [
    TableModule,
    ButtonModule,
    TagModule,
    Spinner,
    NoDataToShowComponent,
    DatePipe
  ],
  templateUrl: './exam-list.html',
  styleUrl: './exam-list.scss'
})
export class ExamList implements OnInit {
  exams = signal<ExamListModel[]>([]);
  loading = signal(false);
  isError = signal(false);
  totalRecords = signal(0);
  pageSize: number = 5;
  currentPage: number = 1;

  private examService = inject(ExamService);
  private authService = inject(AuthService);

  ngOnInit() {
    this.loadExams();
  }

  loadExams(event?: any) {
    this.loading.set(true);
    const page = event ? event.first / event.rows + 1 : 1;
    const pageSize = event ? event.rows : this.pageSize;

    this.currentPage = page;
    this.pageSize = pageSize;

    const queryParams = {
      PageNumber: page,
      PageSize: pageSize
    };

    this.examService.getAllPaged<BaseResourceParametersModel, ExamListModel>(queryParams).subscribe({
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

  onLazyLoad(event: any) {
    this.loadExams(event);
  }

  onViewDetails(exam: ExamListModel) {
    // TODO: Implement view details functionality
    console.log('View details for exam:', exam);
  }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }

  refreshData() {
    this.loadExams();
  }
}
