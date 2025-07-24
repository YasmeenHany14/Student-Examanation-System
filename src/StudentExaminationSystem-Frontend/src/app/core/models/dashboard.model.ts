import { BaseResponseModel } from './common/base-model';

export interface AdminDashboardResponse extends BaseResponseModel {
  totalUsers: number;
  totalExamsCompleted: number;
  passedExamsCount: number;
  failedExamsCount: number;
}
