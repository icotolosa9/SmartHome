import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Notification } from '../models/notification';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private hasUnreadNotifications = new BehaviorSubject<boolean>(false); 

  getHasUnreadNotifications(): Observable<boolean> {
    return this.hasUnreadNotifications.asObservable();
  }

  updateUnreadStatus(notifications: Notification[]): void {
    const hasUnread = notifications.some((n) => !n.isRead);
    this.hasUnreadNotifications.next(hasUnread);
  }
  
  resetNotifications(): void {
    this.hasUnreadNotifications.next(false);
  }
}
