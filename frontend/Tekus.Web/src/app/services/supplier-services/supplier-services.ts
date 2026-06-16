import { DecimalPipe } from '@angular/common';
import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Subject, debounceTime } from 'rxjs';
import { Pagination } from '../../core/pagination/pagination';
import { Supplier } from '../../suppliers/supplier.models';
import { SupplierService } from '../../suppliers/supplier.service';
import { Service } from '../service.models';
import { ServiceService } from '../service.service';

type ServiceSortField = 'name' | 'hourlyRate';

@Component({
  selector: 'app-supplier-services',
  imports: [RouterLink, DecimalPipe, FormsModule, Pagination],
  templateUrl: './supplier-services.html',
  styleUrl: './supplier-services.scss'
})
export class SupplierServices implements OnInit, OnDestroy {
  protected readonly supplier = signal<Supplier | null>(null);
  protected readonly services = signal<Service[]>([]);
  protected readonly isLoading = signal(false);
  protected readonly errorMessage = signal<string | null>(null);

  protected readonly pageNumber = signal(1);
  protected readonly pageSize = 10;
  protected readonly totalCount = signal(0);
  protected readonly totalPages = signal(1);

  protected searchTerm = '';
  protected readonly sortBy = signal<ServiceSortField>('name');
  protected readonly sortDescending = signal(false);

  protected readonly isFormOpen = signal(false);
  protected readonly isSaving = signal(false);
  protected readonly formError = signal<string | null>(null);
  protected newServiceName = '';
  protected newServiceHourlyRate: number | null = null;

  private readonly searchChanged = new Subject<void>();
  private supplierId = '';

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly supplierService: SupplierService,
    private readonly serviceService: ServiceService
  ) {}

  ngOnInit(): void {
    this.supplierId = this.route.snapshot.paramMap.get('supplierId') ?? '';
    if (!this.supplierId) {
      this.router.navigate(['/']);
      return;
    }

    this.searchChanged.pipe(debounceTime(300)).subscribe(() => {
      this.pageNumber.set(1);
      this.load();
    });

    this.supplierService.getById(this.supplierId).subscribe({
      next: (supplier) => this.supplier.set(supplier),
      error: () => this.errorMessage.set('No se pudo cargar el proveedor.')
    });

    this.load();
  }

  ngOnDestroy(): void {
    this.searchChanged.complete();
  }

  onSearchChange(): void {
    this.searchChanged.next();
  }

  sort(field: ServiceSortField): void {
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

    this.serviceService
      .getBySupplier(this.supplierId, {
        pageNumber: this.pageNumber(),
        pageSize: this.pageSize,
        search: this.searchTerm || undefined,
        sortBy: this.sortBy(),
        sortDescending: this.sortDescending()
      })
      .subscribe({
        next: (result) => {
          this.services.set(result.items);
          this.totalCount.set(result.totalCount);
          this.totalPages.set(result.totalPages);
          this.isLoading.set(false);
        },
        error: () => {
          this.errorMessage.set('No se pudo cargar la lista de servicios.');
          this.isLoading.set(false);
        }
      });
  }

  openForm(): void {
    this.formError.set(null);
    this.newServiceName = '';
    this.newServiceHourlyRate = null;
    this.isFormOpen.set(true);
  }

  cancelForm(): void {
    this.isFormOpen.set(false);
  }

  submitForm(): void {
    if (!this.newServiceName.trim() || !this.newServiceHourlyRate || this.newServiceHourlyRate <= 0) {
      this.formError.set('Ingresa un nombre y una tarifa por hora válida.');
      return;
    }

    this.isSaving.set(true);
    this.formError.set(null);

    this.serviceService
      .create({
        name: this.newServiceName.trim(),
        hourlyRate: this.newServiceHourlyRate,
        supplierId: this.supplierId
      })
      .subscribe({
        next: () => {
          this.isSaving.set(false);
          this.isFormOpen.set(false);
          this.load();
        },
        error: () => {
          this.formError.set('No se pudo crear el servicio.');
          this.isSaving.set(false);
        }
      });
  }
}
