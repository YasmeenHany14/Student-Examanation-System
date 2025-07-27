import {BaseRequestModel, BaseResponseModel} from './common/base-model';
import {
  CreateQuestionChoiceModel,
  LoadQuestionChoiceModel,
  QuestionChoiceHistoryModel,
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
  content: string;
  choices: QuestionChoiceHistoryModel[];
}

export interface LoadQuestionModel extends BaseResponseModel {
  id: number;
  content: string;
  questionOrder: number;
  choices: LoadQuestionChoiceModel[];
}
