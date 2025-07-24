import { BaseRequestModel } from './common/base-model';

export interface RegisterStudentRequest extends BaseRequestModel {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  confirmPassword: string;
  birthdate: string; // ISO date string
  gender: number,
  joinDate: string,
  courseIds: number[]
}
