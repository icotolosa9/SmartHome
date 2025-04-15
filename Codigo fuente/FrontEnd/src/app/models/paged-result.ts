export interface PagedResult<T> {
    results: T[];
    totalCount: number;
    pageNumber: number;
    pageSize: number;
    totalPages: number;
  }