import {BaseResourceParametersModel} from './base-resource-parameters.model';

export interface ExamResourceParametersModel extends BaseResourceParametersModel {
  searchQuery: string | null;
}
