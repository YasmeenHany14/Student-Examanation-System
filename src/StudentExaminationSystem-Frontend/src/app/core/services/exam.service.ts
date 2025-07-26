import {Injectable} from '@angular/core';
import {routes} from '../constants/routs';
import {BaseCrudService} from './base-crud.service';

@Injectable({
  providedIn: 'root'
})
export class ExamService extends BaseCrudService {
    protected override route: string;

    constructor() {
      super();
      this.route = routes.exam;
    }
}
