import {BaseResponseModel} from './common/base-model';
import {QuestionHistoryModel} from './question.mode';

export interface ExamListModel extends BaseResponseModel {
  id: number;
  studentName?: string;
  subjectName: string;
  examDate: string; // ISO date string
  finalScore: number;
  passed: boolean;
}

export interface GetExamHistoryModel extends BaseResponseModel {
  finalScore: number;
  passed: boolean;
  questionHistory: QuestionHistoryModel[];
}
