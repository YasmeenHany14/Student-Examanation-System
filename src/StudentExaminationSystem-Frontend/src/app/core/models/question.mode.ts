import {BaseResponseModel} from './common/base-model';
import {QuestionChoiceHistoryModel} from './question-choice.model';

export interface QuestionHistoryModel extends BaseResponseModel {
  question: string;
  choices: QuestionChoiceHistoryModel[];
}
