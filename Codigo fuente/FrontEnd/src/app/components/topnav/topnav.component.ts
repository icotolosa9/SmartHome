import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { Notification } from '../../models/notification';
import { FormsModule } from '@angular/forms';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-topnav',
  standalone: true,
  imports: [RouterModule, CommonModule, FormsModule],
  templateUrl: './topnav.component.html',
  styleUrls: ['./topnav.component.css']
})
export class TopNavComponent implements OnInit { 
  user: User = {} as User;
  notifications: Notification[] = [];
  hasUnreadNotifications = false;
  showPopover = false;
  filters = { deviceType: '', creationDate: '', read: '' };

  constructor(private router: Router, private authService: AuthService, private userService: UserService, private notificationService: NotificationService) { }

  get userRole(): string {
    return this.authService.getUserRole(); 
  }

  ngOnInit(): void {
    const sessionData = localStorage.getItem('connectedUser');
    if (sessionData) {
      this.user = JSON.parse(sessionData);
    };
    this.loadNotifications();
    this.notificationService.getHasUnreadNotifications().subscribe((hasUnread) => {
      this.hasUnreadNotifications = hasUnread;
    });
  }

  logout() {
    localStorage.removeItem('sessionToken');
    localStorage.removeItem('connectedUser');
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    const loggedIn = this.authService.isLoggedIn();
    return loggedIn;
  }
  
  loadNotifications(): void {
    this.userService.getNotifications().subscribe({
      next: (data) => {
        this.notifications = data;
        this.hasUnreadNotifications = this.notifications.some(notification => !notification.isRead);
      },
      error: (err) => console.error('Error al cargar las notificaciones:', err)
    });
  }

  toggleNotifications(): void {
    this.showPopover = !this.showPopover;

    if (this.showPopover) {
      this.loadNotifications();
    }
  }

  markAsRead(notificationId: string): void {
    this.userService.markNotificationAsRead(notificationId).subscribe({
      next: () => {
        const notification = this.notifications.find(n => n.id === notificationId);
        if (notification) {
          notification.isRead = true;
        }
        this.hasUnreadNotifications = this.notifications.some(n => !n.isRead);
      },
      error: (err) => console.error('Error al marcar como leída la notificación:', err)
    });
  }

  applyFilters(): void {
    const creationDate = this.filters.creationDate ? new Date(this.filters.creationDate) : undefined;
    this.userService
      .getNotifications(this.filters.deviceType, creationDate, this.filters.read === '' ? undefined : this.filters.read === 'true')
      .subscribe({
        next: (data) => {
          this.notifications = data;
          this.hasUnreadNotifications = this.notifications.some((n) => !n.isRead);
        },
        error: (err) => console.error('Error al aplicar filtros:', err),
      });
  }

}