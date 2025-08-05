export interface BaseResourceParametersModel {
  [key: string]: string | number | boolean | readonly (string | number | boolean)[] | null;
  PageNumber: number;
  PageSize: number;
  OrderBy: string | null;
}
