import { DecimalPipe } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Supplier } from '../../suppliers/supplier.models';
import { SupplierService } from '../../suppliers/supplier.service';
import { Service } from '../service.models';
import { ServiceService } from '../service.service';

@Component({
  selector: 'app-supplier-services',
  imports: [RouterLink, DecimalPipe],
  templateUrl: './supplier-services.html',
  styleUrl: './supplier-services.scss'
})
export class SupplierServices implements OnInit {
  protected readonly supplier = signal<Supplier | null>(null);
  protected readonly services = signal<Service[]>([]);
  protected readonly isLoading = signal(false);
  protected readonly errorMessage = signal<string | null>(null);

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

    this.load();
  }

  private load(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.supplierService.getById(this.supplierId).subscribe({
      next: (supplier) => this.supplier.set(supplier),
      error: () => this.errorMessage.set('No se pudo cargar el proveedor.')
    });

    this.serviceService.getBySupplier(this.supplierId).subscribe({
      next: (services) => {
        this.services.set(services);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('No se pudo cargar la lista de servicios.');
        this.isLoading.set(false);
      }
    });
  }
}
