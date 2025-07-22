import {inject, Injectable} from '@angular/core';
import {routes} from '../constants/routs';
import {HttpClient} from '@angular/common/http';
import {DropdownModel} from '../models/common.model';

@Injectable({
  providedIn: 'root'
})

export class SubjectService {
  private readonly API_URL = routes.baseUrl;
  private readonly httpClient = inject(HttpClient);

  getSubjectsDropdown() {
    return this.httpClient.get<DropdownModel[]>(this.API_URL + routes.subjectsDropdown);
  }

}
