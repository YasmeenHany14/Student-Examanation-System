import { Component, Output, EventEmitter, OnInit, signal, inject } from '@angular/core';
import { GetSubjectModel } from '../../../core/models/subject.model';
import { SubjectService } from '../../../core/services/subject.service';
import {TableModule} from 'primeng/table';
import {ButtonModule} from 'primeng/button';

@Component({
  selector: 'app-subject-list',
  templateUrl: './subject-list.html',
  imports: [
    TableModule,
    ButtonModule,
  ],
  styleUrls: ['./subject-list.scss']
})
export class SubjectList implements OnInit {
  subjects = signal<GetSubjectModel[]>([]);
  loading = signal(false);
  totalRecords = signal(0);
  pageSize: number = 5;
  currentPage: number = 1;

  private subjectService = inject(SubjectService);

  @Output() edit = new EventEmitter<GetSubjectModel>();
  @Output() delete = new EventEmitter<number>();
  @Output() subjectsLoaded = new EventEmitter<GetSubjectModel[]>();

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

    this.subjectService.getSubjectsPaged(queryParams).subscribe({
      next: (result) => {
        this.subjects.set(result.data);
        this.totalRecords.set(result.pagination.totalCount);
        this.loading.set(false);
        this.subjectsLoaded.emit(result.data);
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
}
