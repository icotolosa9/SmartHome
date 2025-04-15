import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { CreateHomeOwner } from '../../models/create-home-owner';

@Component({
  selector: 'app-create-home-owner',
  templateUrl: './create-home-owner.component.html',
  styleUrls: ['./create-home-owner.component.css'],
  imports: [FormsModule],
  standalone: true
})
export class CreateHomeOwnerComponent {
  homeOwnerData: CreateHomeOwner = { firstName: '', lastName: '', email: '', password: '', profilePhoto: '' };

  constructor(private userService: UserService, private router: Router) {}

  onSubmit(): void {
    this.userService.createHomeOwner(this.homeOwnerData).subscribe({
      next: () => {
        alert('Cuenta creada exitosamente');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error('Error al crear la cuenta de dueño de hogar:', err);
        alert(err.error?.message || 'Ocurrió un error al crear el dueño de hogar.');
      }
    });
  }

  isFormValid(): boolean {
    return (
      this.homeOwnerData.firstName.trim() !== '' &&
      this.homeOwnerData.lastName.trim() !== '' &&
      this.homeOwnerData.email.trim() !== '' &&
      this.homeOwnerData.password.trim() !== '' &&
      this.homeOwnerData.profilePhoto.trim() !== ''
    );
  }
  
}
