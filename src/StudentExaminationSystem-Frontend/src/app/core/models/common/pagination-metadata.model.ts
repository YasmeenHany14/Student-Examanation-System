export interface PaginationMetadataModel {
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  Links: PaginationLinksModel;
}


export interface PaginationLinksModel {
  nextPage?: number;
  previousPage?: number;
}
