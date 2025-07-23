import {gender} from '../enums/gender';

export interface RegisterStudentRequest {
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
