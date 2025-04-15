import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service'; 
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { User } from '../../models/user';
import { NotificationService } from '../../services/notification.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [AuthService]
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  isLoading: boolean = false; 
  errorMessage: string = '';

  constructor(public authService: AuthService, public router: Router, public userService: UserService, public notificationService: NotificationService) { }

  onLogin() {
    this.errorMessage = ''; 
    if (!this.email || !this.password) {
      this.errorMessage = 'Por favor, ingresa tu correo y contraseña.';
      return;
    }
    this.isLoading = true; 
    this.authService.login(this.email, this.password).subscribe(
      (user: User) => {
        localStorage.setItem('sessionToken', JSON.stringify(user.token));
        localStorage.setItem('connectedUser', JSON.stringify(user));

        this.notificationService.resetNotifications();
        this.loadNotifications();

        this.router.navigate(['/home']);
      },
      (error: HttpErrorResponse) => {
        this.isLoading = false;
        this.errorMessage = error.error?.message || 'Ocurrió un error al iniciar sesión.';
      }
    );
  }

  private loadNotifications(): void {
    this.userService.getNotifications().subscribe({
      next: (notifications) => {
        this.notificationService.updateUnreadStatus(notifications);
      },
      error: (err) => console.error('Error al cargar notificaciones:', err),
    });
  }
}
