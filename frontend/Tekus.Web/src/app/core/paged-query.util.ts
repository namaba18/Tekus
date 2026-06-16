import { HttpParams } from '@angular/common/http';
import { PagedQuery } from './paged-result';

export function toHttpParams(query: PagedQuery): HttpParams {
  let params = new HttpParams()
    .set('pageNumber', query.pageNumber)
    .set('pageSize', query.pageSize);

  if (query.search) {
    params = params.set('search', query.search);
  }
  if (query.sortBy) {
    params = params.set('sortBy', query.sortBy);
  }
  if (query.sortDescending) {
    params = params.set('sortDescending', query.sortDescending);
  }

  return params;
}
