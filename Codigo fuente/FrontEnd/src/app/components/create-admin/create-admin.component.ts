import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-admin',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './create-admin.component.html',
  styleUrls: ['./create-admin.component.css']
})
export class CreateAdminComponent {
  firstName = '';
  lastName = '';
  email = '';
  password = '';

  constructor(private userService: UserService, private router: Router) {}

  onSubmit() {
    const adminData = {
      firstName: this.firstName,
      lastName: this.lastName,
      email: this.email,
      password: this.password,
    };

    this.userService.createAdmin(adminData).subscribe({
      next: () => {
        alert('Administrador creado exitosamente');
        this.router.navigate(['/list-accounts']);
      },
      error: (error) => {
        console.error('Error al crear el administrador:', error);
        alert(error.error?.message || 'Ocurrió un error al crear el administrador.');
      },
    });
  }
}
