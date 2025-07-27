import {BaseRequestModel, BaseResponseModel} from './common/base-model';

export interface QuestionChoiceExamModel extends BaseResponseModel { // used for history and live exam and submission
  id: number;
  content: string;
  isCorrect: boolean;
  isSelected: boolean;
}

export interface QuestionChoiceModel extends BaseResponseModel {
  id: number;
  content: string;
  isCorrect: boolean;
}

export interface CreateQuestionChoiceModel extends BaseRequestModel {
  content: string;
  isCorrect: boolean;
}
