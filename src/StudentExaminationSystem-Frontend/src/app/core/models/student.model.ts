import {BaseRequestModel, BaseResponseModel} from './common/base-model';
import {DropdownModel} from './common/common.model';

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

export interface GetStudentListModel extends BaseResponseModel {
  id: number;
  name: string;
  isActive: boolean;
}

export interface StudentDetailsModel extends BaseResponseModel {
  id: number;
  name: string;
  birthdate: string; // ISO date string
  joinDate: string; // ISO date string
  courses: DropdownModel[];
}
