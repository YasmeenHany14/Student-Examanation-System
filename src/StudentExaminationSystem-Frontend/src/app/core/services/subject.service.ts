import {inject, Injectable} from '@angular/core';
import {routes} from '../constants/routs';
import {HttpClient, HttpHeaders, HttpParams,} from '@angular/common/http';
import {DropdownModel} from '../models/common/common.model';
import {CreateSubjectModel, GetSubjectModel, UpdateSubjectModel} from '../models/subject.model';
import {PagedListModel} from '../models/common/paged-list.model';
import {tap} from 'rxjs';
import {BaseResourceParametersModel} from '../models/common/base-resource-parameters.model';

@Injectable({
  providedIn: 'root'
})

export class SubjectService {
  private readonly API_URL = routes.baseUrl;
  private readonly httpClient = inject(HttpClient);

  getSubjectsDropdown() {
    return this.httpClient.get<DropdownModel[]>(this.API_URL + routes.subjectsDropdown);
  }

  getSubjectsPaged(queryParams: BaseResourceParametersModel) {
    const params = new HttpParams({fromObject: {...queryParams}});
    return this.httpClient.get<PagedListModel<GetSubjectModel>>(this.API_URL + routes.subjects, { params });
  }


  createSubject(subject: CreateSubjectModel) {
    return this.httpClient.post(this.API_URL + routes.subjects, subject);
  }

  deleteSubject(id: number) {
    return this.httpClient.delete(this.API_URL + routes.subjects + '/' + id);
  }

  updateSubject(id: number, patchDoc: any) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json-patch+json',
    'Accept': 'text/plain' });
    console.log('Headers:', headers);
    return this.httpClient.patch(this.API_URL + routes.subjects + '/' + id, patchDoc, { headers });
  }

  getStudentSubjects() {

  }

}
