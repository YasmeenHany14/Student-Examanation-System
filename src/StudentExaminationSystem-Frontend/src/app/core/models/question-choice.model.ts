import {BaseRequestModel, BaseResponseModel} from './common/base-model';

export interface QuestionChoiceHistoryModel extends BaseResponseModel {
  choiceText: string;
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
