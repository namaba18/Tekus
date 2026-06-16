import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { Subject, debounceTime } from 'rxjs';
import { AuthService } from '../core/auth/auth.service';
import { Pagination } from '../core/pagination/pagination';
import { Supplier } from '../suppliers/supplier.models';
import { SupplierService } from '../suppliers/supplier.service';

type SupplierSortField = 'name' | 'nit' | 'email' | 'webPage';

@Component({
  selector: 'app-home',
  imports: [RouterLink, FormsModule, Pagination],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home implements OnInit, OnDestroy {
  protected readonly username = signal('');
  protected readonly suppliers = signal<Supplier[]>([]);
  protected readonly isLoading = signal(false);
  protected readonly errorMessage = signal<string | null>(null);

  protected readonly pageNumber = signal(1);
  protected readonly pageSize = 10;
  protected readonly totalCount = signal(0);
  protected readonly totalPages = signal(1);

  protected searchTerm = '';
  protected readonly sortBy = signal<SupplierSortField>('name');
  protected readonly sortDescending = signal(false);

  private readonly searchChanged = new Subject<void>();

  constructor(
    private readonly authService: AuthService,
    private readonly supplierService: SupplierService,
    private readonly router: Router
  ) {
    this.username.set(this.authService.getUsername() ?? '');
  }

  ngOnInit(): void {
    this.searchChanged.pipe(debounceTime(300)).subscribe(() => {
      this.pageNumber.set(1);
      this.load();
    });

    this.load();
  }

  ngOnDestroy(): void {
    this.searchChanged.complete();
  }

  onSearchChange(): void {
    this.searchChanged.next();
  }

  sort(field: SupplierSortField): void {
    if (this.sortBy() === field) {
      this.sortDescending.set(!this.sortDescending());
    } else {
      this.sortBy.set(field);
      this.sortDescending.set(false);
    }

    this.pageNumber.set(1);
    this.load();
  }

  onPageChange(page: number): void {
    this.pageNumber.set(page);
    this.load();
  }

  private load(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.supplierService
      .getAll({
        pageNumber: this.pageNumber(),
        pageSize: this.pageSize,
        search: this.searchTerm || undefined,
        sortBy: this.sortBy(),
        sortDescending: this.sortDescending()
      })
      .subscribe({
        next: (result) => {
          this.suppliers.set(result.items);
          this.totalCount.set(result.totalCount);
          this.totalPages.set(result.totalPages);
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
