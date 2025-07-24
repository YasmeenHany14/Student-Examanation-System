import {Injectable} from '@angular/core';
import {routes} from '../constants/routs';
import {DropdownModel} from '../models/common/common.model';
import {BaseService} from './base.service';

@Injectable({
  providedIn: 'root'
})

export class SubjectService extends BaseService {
  protected override route: string;

  constructor() {
    super();
    this.route = routes.subjects;
  }

  getSubjectsDropdown() {
    return this.httpClient.get<DropdownModel[]>(this.API_URL + routes.subjectsDropdown);
  }

  getStudentSubjects() {

  }

}
