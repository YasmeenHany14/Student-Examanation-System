import {BaseRequestModel, BaseResponseModel} from './common/base-model';

export interface CreateSubjectModel extends BaseRequestModel {
  name: string;
  code: string;
}

export interface GetSubjectModel extends BaseResponseModel {
  id: number;
  name: string;
  code: string | null;
  hasConfiguration?: boolean | null;
}

export interface UpdateSubjectModel extends BaseRequestModel {
  name: string;
  code: string;
}
