import {BaseRequestModel, BaseResponseModel} from './common/base-model';

export interface GetSubjectConfigModel extends BaseResponseModel{
  Id: number;
  TotalQuestions: number;
  DurationMinutes: number;
  DifficultyProfileId: number;
  DifficultyProfileSpecifications: string
}

export interface CreateSubjectConfigModel extends BaseRequestModel {
  TotalQuestions: number;
  DurationMinutes: number;
  DifficultyProfileId: number;
}

export interface UpdateSubjectConfigModel extends BaseRequestModel {
  TotalQuestions?: number | null;
  DurationMinutes?: number | null;
  DifficultyProfileId?: number | null;
}
