import { Component, OnInit, signal, inject } from '@angular/core';
import { StudentService } from '../../core/services/student.service';
import { GetStudentListModel } from '../../core/models/student.model';
import { BaseResourceParametersModel } from '../../core/models/resource-parameters/base-resource-parameters.model';
import {TableLazyLoadEvent, TableModule} from 'primeng/table';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { ButtonModule } from 'primeng/button';
import { TooltipModule } from 'primeng/tooltip';
import { FormsModule } from '@angular/forms';
import { Spinner } from '../../shared/components/spinner/spinner';
import { NoDataToShowComponent } from '../../shared/components/no-data-to-show/no-data-to-show';
import {StudentDetailsDialog} from '../../components/student-details-dialog/student-details-dialog';
import {SortEvent} from 'primeng/api';
import {StudentResourceParametersModel} from '../../core/models/resource-parameters/student-resource-parameters.model';
import {Toolbar} from 'primeng/toolbar';
import { ToolbarModule } from 'primeng/toolbar';
import { InputTextModule } from 'primeng/inputtext';
import {IconField} from 'primeng/iconfield';
import {InputIcon} from 'primeng/inputicon';


@Component({
  selector: 'app-students',
  imports: [
    TableModule,
    ToggleSwitchModule,
    ButtonModule,
    TooltipModule,
    FormsModule,
    Spinner,
    NoDataToShowComponent,
    StudentDetailsDialog,
    Toolbar,
    ToolbarModule,
    InputTextModule,
    IconField,
    InputIcon,
  ],
  templateUrl: './students.html',
  styleUrl: './students.scss'
})
export class Students implements OnInit {
  students = signal<GetStudentListModel[]>([]);
  loading = signal(false);
  isError = signal(false);
  totalRecords = signal(0);

  paginationInfo = signal<StudentResourceParametersModel>({
    PageNumber: 1,
    PageSize: 10,
    OrderBy: null,
    Name: null,
  });
  searchQuery = "";

  // student details popup vars
  visible = signal(false);
  selectedStudentId = signal<string | null>(null);

  private studentService = inject(StudentService);

  ngOnInit() {
    this.loadStudents();
  }

  loadStudents() {
    this.loading.set(true);

    this.studentService.getAllPaged<StudentResourceParametersModel ,GetStudentListModel>(this.paginationInfo()).subscribe({
      next: (result) => {
        this.students.set(result.data);
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
    this.paginationInfo().OrderBy = sortField ? `${sortField} ${sortOrder}` : null;
  }

  onLazyLoad(event: TableLazyLoadEvent) {
    this.paginationInfo().PageNumber = event ? event.first! / event.rows! + 1 : 1;
    this.paginationInfo().PageSize = event ? event.rows! : this.paginationInfo().PageSize;
    this.loadStudents();
  }

  onToggleStatus(student: GetStudentListModel) {
    // Store the original state in case we need to revert
    const originalState = student.isActive;

    const subscription = this.studentService.toggleStudentStatus(student.id).subscribe({
      next: () => {
        const students = this.students();
        const studentIndex = students.findIndex(s => s.id === student.id);
        if (studentIndex !== -1) {
          const updatedStudents = [...students];
          updatedStudents[studentIndex] = { ...updatedStudents[studentIndex], isActive: !originalState };
          this.students.set(updatedStudents);
          subscription.unsubscribe();
        }
      },
    });
  }

  onSearchStudent() {
    if (this.searchQuery.trim() === "") {
      if (this.paginationInfo().Name === "")
        return;
    }
    this.paginationInfo().Name = this.searchQuery === "" ? null : this.searchQuery;
    this.paginationInfo().PageNumber = 1;
    this.loadStudents();
  }

  onViewStudentDetails(studentId: string) {
    this.selectedStudentId.set(studentId);
    this.visible.set(true);
  }

  onCloseDialog() {
    this.visible.set(false);
    this.selectedStudentId.set(null);
  }
}
