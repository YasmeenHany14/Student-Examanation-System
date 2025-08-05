import { Component, Output, EventEmitter, OnInit, signal, inject } from '@angular/core';
import { GetDifficultyProfileModel } from '../../../core/models/difficulty-profile.model';
import { DifficultyProfileService } from '../../../core/services/difficulty-profile.service';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { Spinner } from '../../../shared/components/spinner/spinner';
import { NoDataToShowComponent } from '../../../shared/components/no-data-to-show/no-data-to-show';
import { BaseResourceParametersModel } from '../../../core/models/resource-parameters/base-resource-parameters.model';

@Component({
  selector: 'app-difficulty-profile-list',
  templateUrl: './difficulty-profile-list.html',
  imports: [
    TableModule,
    ButtonModule,
    Spinner,
    NoDataToShowComponent,
  ],
  styleUrls: ['./difficulty-profile-list.scss']
})
export class DifficultyProfileList implements OnInit {
  difficultyProfiles = signal<GetDifficultyProfileModel[]>([]);
  loading = signal(false);
  isError = signal(false);
  totalRecords = signal(0);
  pageSize: number = 5;
  currentPage: number = 1;

  private difficultyProfileService = inject(DifficultyProfileService);

  @Output() edit = new EventEmitter<GetDifficultyProfileModel>();
  @Output() delete = new EventEmitter<number>();

  ngOnInit() {
    this.loadDifficultyProfiles();
  }

  loadDifficultyProfiles(event?: any) {
    this.loading.set(true);
    const page = event ? event.first / event.rows + 1 : 1;
    const pageSize = event ? event.rows : this.pageSize;

    this.currentPage = page;
    this.pageSize = pageSize;

    const queryParams = {
      PageNumber: page,
      PageSize: pageSize
    };

    this.difficultyProfileService.getAllPaged<BaseResourceParametersModel, GetDifficultyProfileModel>(queryParams).subscribe({
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
    this.loadDifficultyProfiles(event);
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
}
