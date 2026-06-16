import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';
import { Login } from './core/auth/login/login';
import { Home } from './home/home';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: '', component: Home, canActivate: [authGuard] }
];
