import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {BaseService} from './base.service';
import {DropdownModel} from '../models/common/common.model';

@Injectable({
  providedIn: 'root'
})
export abstract class DropdownBaseService extends BaseService {
  protected abstract dropdownRoute: string;

  getDropdownOptions<TResponse extends DropdownModel>(): Observable<TResponse[]> {
    return this.httpClient.get<TResponse[]>(this.API_URL + this.dropdownRoute);
  }
}
