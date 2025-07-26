import {BaseResponseModel} from './common/base-model';
import {CreateQuestionChoiceModel, QuestionChoiceHistoryModel, QuestionChoiceModel} from './question-choice.model';

export interface QuestionListModel extends BaseResponseModel {
  id: number;
  content: string;
  subjectId: number;
  difficultyId: number;
  isActive: boolean;
  subjectName?: string;
  choices: QuestionChoiceModel[];
}

export interface CreateQuestionModel extends QuestionListModel {
  subjectId: number;
  difficultyId: number;
  content: string;
  QuestionChoices: CreateQuestionChoiceModel[];
}

export interface QuestionHistoryModel extends BaseResponseModel {
  question: string;
  choices: QuestionChoiceHistoryModel[];
}
