import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('sessionToken'); 
    
    if (token) {
      const newRequest = req.clone({
        setHeaders: {
          'Authorization': `Bearer ${token}` 
        }
      });
      return next.handle(newRequest);
    } else {
      return next.handle(req); 
    }
  }
}

