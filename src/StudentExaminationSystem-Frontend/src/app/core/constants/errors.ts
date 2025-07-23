import {ToastConfig} from '../models/common/errors.model';
import {routes} from './routs';

type ErrorConfig = ToastConfig & {
  defaultMessage: string;
};

export const ERROR_CONFIG: Record<string, ErrorConfig> = {
  Unauthorized: {
    defaultMessage: 'Session expired. Please log in again.',
    severity: 'error',
    life: 5000,
    redirectUrl: routes.authLogin
  },
  InvalidRefreshToken: {
    defaultMessage: 'Your session has expired',
    severity: 'error',
    life: 5000,
    redirectUrl: routes.authLogin,
    useDescription: true
  },
  ValidationError: {
    defaultMessage: 'Please fix the form errors',
    severity: 'error',
    life: 6000,
    closable: true,
    useDescription: true
  },
  WrongCredentials: {
    defaultMessage: 'Invalid username or password',
    severity: 'warn',
    life: 5000,
    closable: true,
  },
  Forbidden: {
    defaultMessage: 'Access denied. You do not have permission to perform this action.',
    severity: 'error',
    life: 5000,
    closable: true
  },
  NotFound: {
    defaultMessage: 'The requested resource was not found.',
    severity: 'warn',
    life: 4000,
    closable: true
  },
  InternalServerError: {
    defaultMessage: 'Server error occurred. Please try again later.',
    severity: 'error',
    life: 5000,
    closable: true
  },
  UnknownError: {
    defaultMessage: 'An unexpected error occurred. Please try again.',
    severity: 'error',
    life: 4000,
    closable: true
  },
  // Missing backend error types
  InvalidInput: {
    defaultMessage: 'The input provided is invalid.',
    severity: 'warn',
    life: 5000,
    closable: true
  },
  CannotGenerateToken: {
    defaultMessage: 'Unable to login at this time. Please try again later.',
    severity: 'error',
    life: 6000,
    closable: true
  },
  BadRequest: {
    defaultMessage: 'The request could not be understood due to invalid format.',
    severity: 'warn',
    life: 5000,
    closable: true
  },
  UserNotActive: {
    defaultMessage: 'Your account is currently disabled. Please contact support.',
    severity: 'error',
    life: 6000,
    closable: true
  },
  default: {
    defaultMessage: 'An unexpected error occurred',
    severity: 'error',
    life: 4000
  }
};

// HTTP Status Code to Error Code mapping
export const HTTP_STATUS_ERROR_MAP: Record<number, string> = {
  400: 'BadRequest',
  401: 'Unauthorized',
  403: 'Forbidden',
  404: 'NotFound',
  422: 'ValidationError',
  500: 'InternalServerError'
};
