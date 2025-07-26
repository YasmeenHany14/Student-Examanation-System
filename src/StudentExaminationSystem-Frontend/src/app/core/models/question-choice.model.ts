import {BaseResponseModel} from './common/base-model';

export interface QuestionChoiceHistoryModel extends BaseResponseModel {
  choiceText: string;
  isCorrect: boolean;
  isSelected: boolean;
}
