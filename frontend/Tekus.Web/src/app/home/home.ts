import { Component, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../core/auth/auth.service';
import { Supplier } from '../suppliers/supplier.models';
import { SupplierService } from '../suppliers/supplier.service';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home implements OnInit {
  protected readonly username = signal('');
  protected readonly suppliers = signal<Supplier[]>([]);
  protected readonly isLoading = signal(false);
  protected readonly errorMessage = signal<string | null>(null);

  constructor(
    private readonly authService: AuthService,
    private readonly supplierService: SupplierService,
    private readonly router: Router
  ) {
    this.username.set(this.authService.getUsername() ?? '');
  }

  ngOnInit(): void {
    this.loadSuppliers();
  }

  loadSuppliers(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.supplierService.getAll().subscribe({
      next: (suppliers) => {
        this.suppliers.set(suppliers);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('No se pudo cargar la lista de proveedores.');
        this.isLoading.set(false);
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
