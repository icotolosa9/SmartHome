import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CompanyService } from '../../services/company.service';
import { AuthService } from '../../services/auth.service';
import { Company } from '../../models/company';

@Component({
  selector: 'app-list-companies',
  templateUrl: './list-companies.component.html',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  styleUrls: ['./list-companies.component.css'],
})

export class ListCompaniesComponent implements OnInit {
  companies: Company[] = [];
  pageNumber = 1;
  pageSize = 10;
  totalItems = 0;
  isLoading: boolean = false;
  company: any = null;
  companyName: string = '';
  ownerFullName: string = '';
  isAdmin: boolean = false;
  isCompanyOwner: boolean = false;

  constructor(private companyService: CompanyService, private authService: AuthService) {}

  ngOnInit(): void {
    const userRole = this.authService.getUserRole();
    this.isAdmin = userRole === 'admin';
    this.isCompanyOwner = userRole === 'companyOwner';

    if (this.isCompanyOwner) {
      this.getMyCompany();
    } else if (this.isAdmin) {
      this.loadCompanies();
    }
  }

  getMyCompany(): void {
    this.companyService.getMyCompany().subscribe({
      next: (company) => {
        this.company = company;  
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error al obtener la empresa:', error);
        this.isLoading = false;
        this.company = null;  
      }
    });
  }

  loadCompanies(): void {
    this.isLoading = true; 
    this.companyService.listCompanies(this.pageNumber, this.pageSize, this.companyName, this.ownerFullName).subscribe({
      next: (response) => {
        this.companies = response.results;
        this.totalItems = response.totalCount;
        this.isLoading = false; 
      },
      error: (err) => {
        console.error('Error al cargar las empresas', err);
        this.isLoading = false; 
      },
    });
  }
  
  applyFilters(): void {
    this.pageNumber = 1;
    this.loadCompanies();
  }

  nextPage(): void {
    if (this.pageNumber * this.pageSize < this.totalItems) {
      this.pageNumber++;
      this.loadCompanies();
    }
  }

  prevPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadCompanies();
    }
  }
}
