import {HttpClient, HttpParams} from '@angular/common/http';
import {inject, Injectable} from '@angular/core';
import {routes} from '../constants/routs';
import {BaseResourceParametersModel} from '../models/resource-parameters/base-resource-parameters.model';
import {PagedListModel} from '../models/common/paged-list.model';
import {GetStudentListModel} from '../models/student.model';
import {DropdownModel} from '../models/common/common.model';
import {BaseCrudService} from './base-crud.service';

@Injectable({
  providedIn: 'root'
})
export class StudentService extends BaseCrudService {
  protected override route: string;

  constructor() {
    super();
    this.route = routes.students;
  }

  toggleStudentStatus(id: number) {
    return this.httpClient.get(this.API_URL + this.route + '/' + id + '/state');
  }

  getStudentSubjects(id: string) {
    return this.httpClient.get<DropdownModel[]>(this.API_URL + routes.studentSubjects + '/' + id);
  }

  addStudentSubject(studentId: string, subjectId: number) {
    return this.httpClient.post(this.API_URL + this.route + '/' + studentId, subjectId);
  }

}
