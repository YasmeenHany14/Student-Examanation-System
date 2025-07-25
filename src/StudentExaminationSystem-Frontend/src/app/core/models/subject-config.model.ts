import {BaseRequestModel, BaseResponseModel} from './common/base-model';

export interface GetSubjectConfigModel extends BaseResponseModel{
  id: number;
  totalQuestions: number;
  durationMinutes: number;
  difficultyProfileId: number;
  difficultyProfileSpecifications?: string
}

export interface CreateSubjectConfigModel extends BaseRequestModel {
  totalQuestions: number;
  durationMinutes: number;
  difficultyProfileId: number;
}

export interface UpdateSubjectConfigModel extends BaseRequestModel {
  totalQuestions?: number | null;
  durationMinutes?: number | null;
  difficultyProfileId?: number | null;
}
