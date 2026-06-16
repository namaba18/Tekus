import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../core/api.config';
import { CreateServiceRequest, Service } from './service.models';

@Injectable({ providedIn: 'root' })
export class ServiceService {
  constructor(private readonly http: HttpClient) {}

  getBySupplier(supplierId: string): Observable<Service[]> {
    return this.http.get<Service[]>(`${API_BASE_URL}/api/suppliers/${supplierId}/services`);
  }

  create(request: CreateServiceRequest): Observable<Service> {
    return this.http.post<Service>(`${API_BASE_URL}/api/services`, request);
  }
}
