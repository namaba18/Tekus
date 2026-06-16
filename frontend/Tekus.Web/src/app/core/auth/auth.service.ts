import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { LoginRequest, LoginResponse } from './auth.models';

const TOKEN_KEY = 'tekus_token';
const USERNAME_KEY = 'tekus_username';
const API_BASE_URL = 'http://localhost:5259';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly tokenSignal = signal<string | null>(localStorage.getItem(TOKEN_KEY));
  readonly isAuthenticated = signal<boolean>(!!localStorage.getItem(TOKEN_KEY));

  constructor(private readonly http: HttpClient) {}

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${API_BASE_URL}/api/auth/login`, request).pipe(
      tap((response) => {
        localStorage.setItem(TOKEN_KEY, response.token);
        localStorage.setItem(USERNAME_KEY, response.username);
        this.tokenSignal.set(response.token);
        this.isAuthenticated.set(true);
      })
    );
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USERNAME_KEY);
    this.tokenSignal.set(null);
    this.isAuthenticated.set(false);
  }

  getToken(): string | null {
    return this.tokenSignal();
  }

  getUsername(): string | null {
    return localStorage.getItem(USERNAME_KEY);
  }
}
