import {Injectable} from '@angular/core';
import {BaseCrudService} from './base-crud.service';
import {routes} from '../constants/routs';

@Injectable({
  providedIn: 'root'
})
export class QuestionService extends BaseCrudService {
  protected override route: string;

  constructor() {
    super();
    this.route = routes.question;
  }

  toggleStatus(questionId: number) {
    return this.httpClient.get(this.API_URL + this.route + '/' + questionId +'/toggle-status');
  }

}
