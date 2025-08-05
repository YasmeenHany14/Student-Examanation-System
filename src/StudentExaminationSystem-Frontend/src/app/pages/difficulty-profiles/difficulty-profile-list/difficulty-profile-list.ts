import { Component, Output, EventEmitter, OnInit, signal, inject } from '@angular/core';
import { GetDifficultyProfileModel } from '../../../core/models/difficulty-profile.model';
import { DifficultyProfileService } from '../../../core/services/difficulty-profile.service';
import {TableLazyLoadEvent, TableModule} from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { Spinner } from '../../../shared/components/spinner/spinner';
import { NoDataToShowComponent } from '../../../shared/components/no-data-to-show/no-data-to-show';
import {
  DifficultyProfileResourceParametersModel
} from '../../../core/models/resource-parameters/difficulty-resource-parameters.model';
import {FormsModule} from '@angular/forms';
import {IconField} from 'primeng/iconfield';
import {InputIcon} from 'primeng/inputicon';
import {InputText} from 'primeng/inputtext';
import {Toolbar} from 'primeng/toolbar';
import {SortEvent} from 'primeng/api';

@Component({
  selector: 'app-difficulty-profile-list',
  templateUrl: './difficulty-profile-list.html',
  imports: [
    TableModule,
    ButtonModule,
    Spinner,
    NoDataToShowComponent,
    FormsModule,
    IconField,
    InputIcon,
    InputText,
    Toolbar,
  ],
  styleUrls: ['./difficulty-profile-list.scss']
})
export class DifficultyProfileList implements OnInit {
  difficultyProfiles = signal<GetDifficultyProfileModel[]>([]);
  loading = signal(false);
  isError = signal(false);
  totalRecords = signal(0);

  paginationInfo = signal<DifficultyProfileResourceParametersModel>({
    PageNumber: 1,
    PageSize: 10,
    OrderBy: null,
    Name: null,
  });
  searchQuery = '';

  private difficultyProfileService = inject(DifficultyProfileService);

  @Output() edit = new EventEmitter<GetDifficultyProfileModel>();
  @Output() delete = new EventEmitter<number>();

  ngOnInit() {
    this.loadDifficultyProfiles();
  }

  loadDifficultyProfiles() {
    this.loading.set(true);
    this.isError.set(false);

    this.difficultyProfileService.getAllPaged<DifficultyProfileResourceParametersModel, GetDifficultyProfileModel>(this.paginationInfo()).subscribe({
      next: (result) => {
        this.difficultyProfiles.set(result.data);
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
    this.paginationInfo().PageNumber = event ? event.first! / event.rows! + 1 : 1;
    this.paginationInfo().PageSize = event ? event.rows! : this.paginationInfo().PageSize;
    this.loadDifficultyProfiles();
  }

  onEdit(difficultyProfile: GetDifficultyProfileModel) {
    this.edit.emit(difficultyProfile);
  }

  onDelete(id: number) {
    this.delete.emit(id);
  }

  refreshData() {
    this.loadDifficultyProfiles();
  }

  updateDifficultyProfile(updatedDifficultyProfile: GetDifficultyProfileModel) {
    const index = this.difficultyProfiles().findIndex(dp => dp.id === updatedDifficultyProfile.id);
    if (index !== -1) {
      const updatedDifficultyProfiles = [...this.difficultyProfiles()];
      updatedDifficultyProfiles[index] = updatedDifficultyProfile;
      this.difficultyProfiles.set(updatedDifficultyProfiles);
    }
  }

  customSort(event: SortEvent) {
    const sortField = event.field;
    const sortOrder = event.order !== 1 ? 'asc' : 'desc';
    this.paginationInfo().OrderBy = sortField ? `${sortField} ${sortOrder}` : null;
  }

  onSearchQuestion() {
    if (this.searchQuery.trim() === "") {
      if (this.paginationInfo().Name === "")
        return;
    }
    this.paginationInfo().Name = this.searchQuery === "" ? null : this.searchQuery;
    this.paginationInfo().PageNumber = 1;
    this.loadDifficultyProfiles();
  }
}
