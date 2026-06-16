import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';
import { Login } from './core/auth/login/login';
import { Home } from './home/home';
import { SupplierServices } from './services/supplier-services/supplier-services';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: '', component: Home, canActivate: [authGuard] },
  { path: 'suppliers/:supplierId/services', component: SupplierServices, canActivate: [authGuard] }
];
