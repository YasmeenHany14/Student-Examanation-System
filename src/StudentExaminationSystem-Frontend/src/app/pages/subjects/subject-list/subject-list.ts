import { Component, Output, EventEmitter, OnInit, signal, inject } from '@angular/core';
import { GetSubjectModel } from '../../../core/models/subject.model';
import { SubjectService } from '../../../core/services/subject.service';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { Spinner } from '../../../shared/components/spinner/spinner';
import { NoDataToShowComponent } from '../../../shared/components/no-data-to-show/no-data-to-show';
import { BaseResourceParametersModel } from '../../../core/models/common/base-resource-parameters.model';
import { SubjectConfigPopover } from '../subject-config-popover/subject-config-popover';
// import { PopoverModule } from 'primeng/popover';

@Component({
  selector: 'app-subject-list',
  templateUrl: './subject-list.html',
  imports: [
    TableModule,
    ButtonModule,
    Spinner,
    NoDataToShowComponent,
    SubjectConfigPopover,
  ],
  styleUrls: ['./subject-list.scss']
})
export class SubjectList implements OnInit {
  subjects = signal<GetSubjectModel[]>([]);
  loading = signal(false);
  isError = signal(false);
  totalRecords = signal(0);
  pageSize: number = 5;
  currentPage: number = 1;

  private subjectService = inject(SubjectService);

  @Output() edit = new EventEmitter<GetSubjectModel>();
  @Output() delete = new EventEmitter<number>();
  @Output() viewConfig = new EventEmitter<number>();
  @Output() editConfig = new EventEmitter<number>();
  @Output() createConfig = new EventEmitter<number>();

  ngOnInit() {
    this.loadSubjects();
  }

  loadSubjects(event?: any) {
    this.loading.set(true);
    const page = event ? event.first / event.rows + 1 : 1;
    const pageSize = event ? event.rows : this.pageSize;

    this.currentPage = page;
    this.pageSize = pageSize;

    const queryParams = {
      PageNumber: page,
      PageSize: pageSize
    };

    this.subjectService.getAllPaged<BaseResourceParametersModel, GetSubjectModel>(queryParams).subscribe({
      next: (result) => {
        this.subjects.set(result.data);
        this.totalRecords.set(result.pagination.totalCount);
        this.loading.set(false);
        this.isError.set(false);
      },
      error: (err) => {
        this.loading.set(false);
        this.isError.set(true);
      }
    });
  }

  onLazyLoad(event: any) {
    this.loadSubjects(event);
  }

  onEdit(subject: GetSubjectModel) {
    this.edit.emit(subject);
  }

  onDelete(id: number) {
    this.delete.emit(id);
  }

  refreshData() {
    this.loadSubjects();
  }

  updateSubject(updatedSubject: GetSubjectModel) {
    const index = this.subjects().findIndex(s => s.id === updatedSubject.id);
    if (index !== -1) {
      const updatedSubjects = [...this.subjects()];
      updatedSubjects[index] = updatedSubject;
      this.subjects.set(updatedSubjects);
    }
  }

  onViewConfig(id: number) {
    this.viewConfig.emit(id);
  }

  onEditConfig(id: number) {
    this.editConfig.emit(id);
  }

  onCreateConfig(id: number) {
    this.createConfig.emit(id);
  }
}
