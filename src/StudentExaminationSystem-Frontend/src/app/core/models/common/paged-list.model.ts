import {PaginationMetadataModel} from './pagination-metadata.model';

export interface PagedListModel<Tmodel> {
  pagination: PaginationMetadataModel,
  data: Tmodel[];
}
