import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../models/user';
import { PagedResult } from '../models/paged-result';
import { CreateCompanyOwner } from '../models/create-company-owner';
import { CreateHomeOwner } from '../models/create-home-owner';
import { Notification } from '../models/notification';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${environment.apiUrl}/users`;

  constructor(private http: HttpClient) {}

  listAccounts(pageNumber: number, pageSize: number, fullName?: string, role?: string): Observable<PagedResult<User>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (fullName) {
      params = params.set('fullName', fullName);
    }
    if (role) {
      params = params.set('role', role);
    }
    return this.http.get<PagedResult<User>>(`${this.apiUrl}`, { params });
  }

  createCompanyOwner(companyOwnerData: CreateCompanyOwner): Observable<any> {
    return this.http.post(`${this.apiUrl}/company-owners`, companyOwnerData);
  }

  createAdmin(adminData: { firstName: string; lastName: string; email: string; password: string }): Observable<any> {
    return this.http.post(`${this.apiUrl}/admins`, adminData);
  }

  deleteAdmin(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  createHomeOwner(homeOwnerData: CreateHomeOwner): Observable<any> {
    return this.http.post(`${this.apiUrl}/home-owners`, homeOwnerData);
  }

  getNotifications(deviceType?: string, creationDate?: Date, read?: boolean): Observable<Notification[]> {
    let params = new HttpParams();
    if (deviceType) params = params.set('DeviceType', deviceType);
    if (creationDate) params = params.set('CreationDate', creationDate.toISOString());
    if (read !== undefined) params = params.set('Read', read.toString());
    return this.http.get<Notification[]>(`${this.apiUrl}/notifications`, { params });
  }

  markNotificationAsRead(notificationId: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/notifications/${notificationId}`, null);
  } 
}