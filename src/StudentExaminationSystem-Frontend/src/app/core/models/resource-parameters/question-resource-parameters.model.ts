import {BaseResourceParametersModel} from './base-resource-parameters.model';

export interface QuestionResourceParametersModel extends BaseResourceParametersModel {
  subjectId: number | null;
  difficultyId: number | null;
  searchQuery: string | null;
}
