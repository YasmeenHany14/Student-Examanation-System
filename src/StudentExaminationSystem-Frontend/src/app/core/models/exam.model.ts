import {BaseResponseModel} from './common/base-model';
import {LoadQuestionModel, QuestionHistoryModel} from './question.model';

export interface ExamListModel extends BaseResponseModel {
  id: number;
  studentName?: string;
  subjectName: string;
  examDate: string; // ISO date string
  finalScore: number;
  examStatus: number;
  passed: boolean;
}

export interface GetExamHistoryModel extends BaseResponseModel {
  subjectName: string;
  finalScore: number;
  passed: boolean;
  questions: QuestionHistoryModel[];
}

export interface LoadExamModel extends BaseResponseModel {
  id: number;
  subjectId: number;
  examEndTime: string; // ISO date string
  questions: LoadQuestionModel[];
}
