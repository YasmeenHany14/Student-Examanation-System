import {Injectable} from '@angular/core';
import {routes} from '../constants/routs';
import {BaseCrudService} from './base-crud.service';
import {LoadExamModel} from '../models/exam.model';

@Injectable({
  providedIn: 'root'
})
export class ExamService extends BaseCrudService {
    protected override route: string;

    constructor() {
      super();
      this.route = routes.exam;
    }

    getExamForSubject(subjectId: number) {
      return this.httpClient.get<LoadExamModel>(this.API_URL + this.route + '/subject/' + subjectId);
    }

    submitExam(exam: LoadExamModel){
      return this.httpClient.post<LoadExamModel>(this.API_URL + this.route, exam);
    }
}
