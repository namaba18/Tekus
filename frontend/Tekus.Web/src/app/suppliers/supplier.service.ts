import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../core/api.config';
import { toHttpParams } from '../core/paged-query.util';
import { PagedQuery, PagedResult } from '../core/paged-result';
import { Supplier } from './supplier.models';

@Injectable({ providedIn: 'root' })
export class SupplierService {
  constructor(private readonly http: HttpClient) {}

  getAll(query: PagedQuery): Observable<PagedResult<Supplier>> {
    return this.http.get<PagedResult<Supplier>>(`${API_BASE_URL}/api/suppliers`, {
      params: toHttpParams(query)
    });
  }

  getById(id: string): Observable<Supplier> {
    return this.http.get<Supplier>(`${API_BASE_URL}/api/suppliers/${id}`);
  }
}
