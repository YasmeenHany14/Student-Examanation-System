import { Component, Output, EventEmitter, OnInit, signal, inject } from '@angular/core';
import { GetSubjectModel } from '../../../core/models/subject.model';
import { SubjectService } from '../../../core/services/subject.service';
import {TableLazyLoadEvent, TableModule} from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { Spinner } from '../../../shared/components/spinner/spinner';
import { NoDataToShowComponent } from '../../../shared/components/no-data-to-show/no-data-to-show';
import { SubjectConfigPopover } from '../subject-config-popover/subject-config-popover';
import { SubjectResourceParametersModel } from '../../../core/models/resource-parameters/subject-resource-parameters.model';
import {FormsModule} from '@angular/forms';
import {IconField} from 'primeng/iconfield';
import {InputIcon} from 'primeng/inputicon';
import {InputText} from 'primeng/inputtext';
import {Toolbar} from 'primeng/toolbar';
import {SortEvent} from 'primeng/api';

@Component({
  selector: 'app-subject-list',
  templateUrl: './subject-list.html',
  imports: [
    TableModule,
    ButtonModule,
    Spinner,
    NoDataToShowComponent,
    SubjectConfigPopover,
    FormsModule,
    IconField,
    InputIcon,
    InputText,
    Toolbar,
  ],
  styleUrls: ['./subject-list.scss']
})
export class SubjectList implements OnInit {
  subjects = signal<GetSubjectModel[]>([]);
  loading = signal(false);
  isError = signal(false);
  totalRecords = signal(0);

  paginationInfo = signal<SubjectResourceParametersModel>({
    PageNumber: 1,
    PageSize: 10,
    OrderBy: null,
    Name: null,
  });
  searchQuery = "";

  private subjectService = inject(SubjectService);

  @Output() edit = new EventEmitter<GetSubjectModel>();
  @Output() delete = new EventEmitter<number>();
  @Output() viewConfig = new EventEmitter<number>();
  @Output() editConfig = new EventEmitter<number>();
  @Output() createConfig = new EventEmitter<number>();

  ngOnInit() {
    this.loadSubjects();
  }

  loadSubjects() {
    this.loading.set(true);

    this.subjectService.getAllPaged<SubjectResourceParametersModel, GetSubjectModel>(this.paginationInfo()).subscribe({
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

  onLazyLoad(event: TableLazyLoadEvent) {
    this.paginationInfo().PageNumber = event ? event.first! / event.rows! + 1 : 1;
    this.paginationInfo().PageSize = event ? event.rows! : this.paginationInfo().PageSize;

    this.loadSubjects();
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

  onSearchSubject() {
    if (this.searchQuery.trim() === "") {
      if (this.paginationInfo().Name === "")
        return;
    }
    this.paginationInfo().Name = this.searchQuery === "" ? null : this.searchQuery;
    this.paginationInfo().PageNumber = 1;
    this.loadSubjects();
  }

  customSort(event: SortEvent) {
    const sortField = event.field;
    const sortOrder = event.order !== 1 ? 'asc' : 'desc';
    this.paginationInfo().OrderBy = sortField ? `${sortField} ${sortOrder}` : null;
  }
}
