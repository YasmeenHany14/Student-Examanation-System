import {ApiError, FrontendError, ToastConfig} from '../../core/models/common/errors.model';
import {ERROR_CONFIG} from '../../core/constants/errors';


//TODO: AI generated, needs review and adjustments
export function toFrontendError(apiError: ApiError): FrontendError {
  const config = ERROR_CONFIG[apiError.code] || ERROR_CONFIG['default'];

  // Destructure to separate message and toast properties
  const { defaultMessage, ...toastConfig } = config;

  // Determine display message (user-provided > backend message > default)
  let message = apiError.message || defaultMessage;
  if(apiError.code === "ValidationError")
    message = apiError.message!;

  return {
    ...apiError,
    message,  // User-friendly message
    toastConfig: toastConfig as ToastConfig,

    // Dynamic message resolver
    getDisplayMessage: function() {
      return this.toastConfig.useDescription ? this.description : this.message!;
    }
  };
}
