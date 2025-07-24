export interface BaseResourceParametersModel {
  [key: string]: string | number | boolean | readonly (string | number | boolean)[];
  PageNumber: number;
  PageSize: number;
}
