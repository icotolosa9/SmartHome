import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CompanyService } from '../../services/company.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-create-company',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './create-company.component.html',
  styleUrls: ['./create-company.component.css']
})
export class CreateCompanyComponent implements OnInit {
  companyName = '';
  logoURL = '';
  rut = '';
  validatorModel: string = '';
  validators: string[] = [];

  constructor(private companyService: CompanyService, private router: Router, private http: HttpClient) {}

  ngOnInit(): void {
    this.loadValidators();
  }

  loadValidators(): void {
    this.companyService.getValidators().subscribe({
      next: (data) => (this.validators = data),
      error: (err) => console.error('Error al cargar validadores:', err),
    });
  }

  onSubmit() {
    const companyData = { Name: this.companyName, LogoURL: this.logoURL, Rut: this.rut, validatorModel: this.validatorModel};
    this.companyService.createCompany(companyData).subscribe({
      next: () => {
        alert('Empresa creada con éxito');
        this.router.navigate(['/list-companies']);
      },
      error: (error) => {
        console.error('Error al crear la empresa:', error);
        alert(error.error?.message || 'Ocurrió un error al crear la empresa');
      }
    });
  }
}
