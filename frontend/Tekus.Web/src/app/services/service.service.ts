import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../core/api.config';
import { toHttpParams } from '../core/paged-query.util';
import { PagedQuery, PagedResult } from '../core/paged-result';
import { CreateServiceRequest, Service } from './service.models';

@Injectable({ providedIn: 'root' })
export class ServiceService {
  constructor(private readonly http: HttpClient) {}

  getBySupplier(supplierId: string, query: PagedQuery): Observable<PagedResult<Service>> {
    return this.http.get<PagedResult<Service>>(`${API_BASE_URL}/api/suppliers/${supplierId}/services`, {
      params: toHttpParams(query)
    });
  }

  create(request: CreateServiceRequest): Observable<Service> {
    return this.http.post<Service>(`${API_BASE_URL}/api/services`, request);
  }
}
