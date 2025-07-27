import {HttpClient, HttpParams} from '@angular/common/http';
import {inject, Injectable} from '@angular/core';
import {routes} from '../constants/routs';
import {BaseResourceParametersModel} from '../models/common/base-resource-parameters.model';
import {PagedListModel} from '../models/common/paged-list.model';
import {GetStudentListModel} from '../models/student.model';
import {DropdownModel} from '../models/common/common.model';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  httpClient = inject(HttpClient);
  private readonly API_URL = routes.baseUrl;
  private readonly route = routes.students;

  getAllPaged(queryParams: BaseResourceParametersModel) {
    const params = new HttpParams({ fromObject: { ...queryParams } });
    return this.httpClient.get<PagedListModel<GetStudentListModel>>(this.API_URL + this.route, { params });
  }

  toggleStudentStatus(id: number) {
    return this.httpClient.get(this.API_URL + this.route + '/' + id + '/state');
  }

  getStudentSubjects(id: string) {
    return this.httpClient.get<DropdownModel[]>(this.API_URL + routes.studentSubjects + '/' + id);
  }

}
