import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { HttpErrorResponse } from '@angular/common/http';
import { ApiError, FrontendError } from '../models/errors.model';
import { toFrontendError } from '../../shared/utils/error.utils';
import { HTTP_STATUS_ERROR_MAP } from '../constants/errors';

@Injectable({
  providedIn: 'root'
})

//TODO: AI generated, needs review and adjustments

export class ErrorService {
  private readonly router = inject(Router);
  private readonly messageService = inject(MessageService);

  handleHttpError(httpError: HttpErrorResponse): void {
    const apiError = this.extractApiError(httpError);
    this.handleApiError(apiError);
  }

  handleApiError(apiError: ApiError): void {
    const frontendError = toFrontendError(apiError);
    this.displayErrorToast(frontendError);
    this.handleErrorNavigation(frontendError);
  }

  displayCustomError(message: string, severity: 'error' | 'warn' | 'info' | 'success' = 'error', life: number = 5000): void {
    this.messageService.add({
      severity,
      summary: this.getSummaryForSeverity(severity),
      detail: message,
      life,
      closable: true
    });
  }

  private extractApiError(httpError: HttpErrorResponse): ApiError {
    // If the backend returns a structured error
    if (httpError.error?.code && httpError.error?.description) {
      return {
        code: httpError.error.code,
        description: httpError.error.description,
      };
    }

    // Use centralized HTTP status mapping
    const errorCode = HTTP_STATUS_ERROR_MAP[httpError.status] || 'UnknownError';

    return {
      code: errorCode,
      description: httpError.error?.description || httpError.statusText,
    };
  }

  private extractValidationDetails(httpError: HttpErrorResponse): Record<string, string[]> | undefined {
    return httpError.error?.errors || httpError.error?.validationErrors;
  }

  private displayErrorToast(frontendError: FrontendError): void {
    const { toastConfig } = frontendError;

    this.messageService.add({
      severity: toastConfig.severity || 'error',
      summary: this.getSummaryForSeverity(toastConfig.severity || 'error'),
      detail: frontendError.getDisplayMessage(),
      life: toastConfig.life || 5000,
      closable: toastConfig.closable !== false
    });
  }

  private handleErrorNavigation(frontendError: FrontendError): void {
    if (frontendError.toastConfig.redirectUrl) {
      setTimeout(() => {
        this.router.navigate([frontendError.toastConfig.redirectUrl]);
      }, 1000);
    }
  }

  private getSummaryForSeverity(severity: 'error' | 'warn' | 'info' | 'success'): string {
    const summaryMap = {
      error: 'Error',
      warn: 'Warning',
      info: 'Information',
      success: 'Success'
    };
    return summaryMap[severity];
  }
}
