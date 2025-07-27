import {BaseRequestModel, BaseResponseModel} from './common/base-model';
import {
  CreateQuestionChoiceModel,
  QuestionChoiceExamModel,
  QuestionChoiceModel
} from './question-choice.model';

export interface QuestionListModel extends BaseResponseModel {
  id: number;
  content: string;
  subjectId: number;
  difficultyId: number;
  isActive: boolean;
  subjectName?: string;
  choices: QuestionChoiceModel[];
}

export interface CreateQuestionModel extends BaseRequestModel {
  subjectId: number;
  difficultyId: number;
  content: string;
  choices: CreateQuestionChoiceModel[];
}

export interface QuestionHistoryModel extends BaseResponseModel {
  id: number;
  content: string;
  choices: QuestionChoiceExamModel[];
}

export interface LoadQuestionModel extends QuestionHistoryModel {
  questionOrder: number;
}
