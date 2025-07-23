// errors.model.ts
export type ApiError = {
  code: string;
  description: string;
  message?: string;
  details?: Record<string, string[]>;
};

export type ToastConfig = {
  severity?: 'error' | 'warn' | 'info' | 'success';
  life?: number;
  redirectUrl?: string;
  useDescription?: boolean;
  closable?: boolean;
};

export type FrontendError = ApiError & {
  toastConfig: ToastConfig;
  getDisplayMessage: () => string;
};
