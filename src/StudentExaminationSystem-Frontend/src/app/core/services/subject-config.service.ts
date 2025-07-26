import {Injectable} from '@angular/core';
import {BaseCrudService} from './base-crud.service';
import {routes} from '../constants/routs';
import {Observable} from 'rxjs';
import {CreateSubjectConfigModel, GetSubjectConfigModel, UpdateSubjectConfigModel} from '../models/subject-config.model';
import {CreateSubjectModel} from '../models/subject.model';
import {BaseRequestModel} from '../models/common/base-model';
import {HttpHeaders} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class SubjectConfigService extends BaseCrudService {
  protected override route: string = '';

  constructor() {
    super();
  }

  createSubjectConfig(subjectId: number, model: CreateSubjectConfigModel): Observable<Object> {
    const url = this.API_URL + routes.SetSubjectConfig(subjectId);
    return this.httpClient.post(url, model);
  }

  getModelById(subjectId: number): Observable<GetSubjectConfigModel> {
    this.route = routes.SetSubjectConfig(subjectId);
    return this.httpClient.get<GetSubjectConfigModel>(this.API_URL + this.route);
  }

  updateSubjectConfig(subjectId: number, patchDoc: any): Observable<Object> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json-patch+json',
      'Accept': 'text/plain'
    });
    const url = this.API_URL + routes.SetSubjectConfig(subjectId);
    return this.httpClient.patch(url, patchDoc, { headers });
  }
}
