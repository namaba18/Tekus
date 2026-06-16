export interface Service {
  id: string;
  name: string;
  hourlyRate: number;
  supplierId: string;
}

export interface CreateServiceRequest {
  name: string;
  hourlyRate: number;
  supplierId: string;
}
