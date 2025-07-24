import {PaginationMetadataModel} from './pagination-metadata.model';
import { BaseResponseModel } from './base-model';

export interface PagedListModel<TModel extends BaseResponseModel> extends BaseResponseModel {
  pagination: PaginationMetadataModel,
  data: TModel[];
}
