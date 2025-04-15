import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { PagedResult } from '../models/paged-result';
import { Company } from '../models/company';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private apiUrl = `${environment.apiUrl}/companies`;

  constructor(private http: HttpClient) {}

  listCompanies(pageNumber: number, pageSize: number, companyName?: string, ownerFullName?: string): Observable<PagedResult<Company>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (companyName) params = params.set('companyName', companyName);
    if (ownerFullName) params = params.set('companyOwnerFullName', ownerFullName);

    return this.http.get<PagedResult<Company>>(this.apiUrl, { params });
  }

  createCompany(companyData: { Name: string; LogoURL: string; Rut: string; ValidatorModel?: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}`, companyData);
  }

  getValidators(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/validators`);
  }

  getMyCompany(): Observable<Company> {
    return this.http.get<Company>(`${this.apiUrl}/my-company`);
  }
}
