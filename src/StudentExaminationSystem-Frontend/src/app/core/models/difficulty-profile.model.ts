import {BaseRequestModel, BaseResponseModel} from './common/base-model';

export interface GetDifficultyProfileModel extends BaseResponseModel {
  id: number;
  name: string;
  easyQuestionsPercent: number;
  mediumQuestionsPercent: number;
  hardQuestionsPercent: number;
}

export interface CreateDifficultyProfileModel extends BaseRequestModel {
  name: string;
  easyQuestionsPercent: number;
  mediumQuestionsPercent: number;
  hardQuestionsPercent: number;
}

export interface UpdateDifficultyProfileModel extends BaseRequestModel {
  name?: string | null;
  easyQuestionsPercent?: number | null;
  mediumQuestionsPercent?: number | null;
  hardQuestionsPercent?: number | null;
}
