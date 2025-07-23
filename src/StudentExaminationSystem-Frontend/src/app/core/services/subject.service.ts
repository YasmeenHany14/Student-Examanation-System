import {inject, Injectable} from '@angular/core';
import {routes} from '../constants/routs';
import {HttpClient} from '@angular/common/http';
import {DropdownModel} from '../models/common/common.model';
import {CreateSubjectModel, GetSubjectModel} from '../models/subject.model';
import {PagedListModel} from '../models/common/paged-list.model';

@Injectable({
  providedIn: 'root'
})

export class SubjectService {
  private readonly API_URL = routes.baseUrl;
  private readonly httpClient = inject(HttpClient);

  getSubjectsDropdown() {
    return this.httpClient.get<DropdownModel[]>(this.API_URL + routes.subjectsDropdown);
  }

  getSubjectsPaged() {
    return this.httpClient.get<PagedListModel<GetSubjectModel>>(this.API_URL + routes.subjects)
    pipe(
      tap(response => {
        console.log(response);
      })
    )
  }
  
  createSubject(subject: CreateSubjectModel) {
    return this.httpClient.post(this.API_URL + routes.subjects);
  }

  deleteSubject(id: number) {
    return this.httpClient.delete(this.API_URL + routes.subjects + '/' + id);
  }

  updateSubject() {

  }

  getStudentSubjects() {

  }

}
