import { Component, OnInit, signal, inject } from '@angular/core';
import { StudentService } from '../../core/services/student.service';
import { GetStudentListModel } from '../../core/models/student.model';
import { BaseResourceParametersModel } from '../../core/models/common/base-resource-parameters.model';
import { TableModule } from 'primeng/table';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { ButtonModule } from 'primeng/button';
import { TooltipModule } from 'primeng/tooltip';
import { FormsModule } from '@angular/forms';
import { Spinner } from '../../shared/components/spinner/spinner';
import { NoDataToShowComponent } from '../../shared/components/no-data-to-show/no-data-to-show';
import {StudentDetailsDialog} from '../../components/student-details-dialog/student-details-dialog';


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
  ],
  templateUrl: './students.html',
  styleUrl: './students.scss'
})
export class Students implements OnInit {
  students = signal<GetStudentListModel[]>([]);
  loading = signal(false);
  isError = signal(false);
  totalRecords = signal(0);
  pageSize: number = 5;
  currentPage: number = 1;

  // student details popup vars
  visible = signal(false);
  selectedStudentId = signal<string | null>(null);

  private studentService = inject(StudentService);

  ngOnInit() {
    this.loadStudents();
  }

  loadStudents(event?: any) {
    this.loading.set(true);
    const page = event ? event.first / event.rows + 1 : 1;
    const pageSize = event ? event.rows : this.pageSize;

    this.currentPage = page;
    this.pageSize = pageSize;

    const queryParams: BaseResourceParametersModel = {
      PageNumber: page,
      PageSize: pageSize
    };

    this.studentService.getAllPaged<BaseResourceParametersModel ,GetStudentListModel>(queryParams).subscribe({
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

  onLazyLoad(event: any) {
    this.loadStudents(event);
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

  onViewStudentDetails(studentId: string) {
    this.selectedStudentId.set(studentId);
    this.visible.set(true);
  }

  onCloseDialog() {
    this.visible.set(false);
    this.selectedStudentId.set(null);
  }
}
