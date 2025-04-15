import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/sessions`;

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<User> {
    const loginData = { Email: email, Password: password };
    return this.http.post<User>(`${this.apiUrl}/login`, loginData);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('sessionToken');
  }

  getUserRole(): string {
    const user = JSON.parse(localStorage.getItem('connectedUser') || '{}');
    return user?.role || '';
  }

}
