import {BaseResourceParametersModel} from './base-resource-parameters.model';
export interface StudentResourceParametersModel extends BaseResourceParametersModel {
  OrderBy: string | null;
  Name: string | null;
}
