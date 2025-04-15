import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HomeService } from '../../services/home.service';
import { Router } from '@angular/router';
import { Home } from '../../models/home';

@Component({
  selector: 'app-create-home',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-home.component.html',
  styleUrls: ['./create-home.component.css']
})
export class CreateHomeComponent {
  newHome: Home = {
    name: '',
    address: '',
    location: '',
    capacity: 0
  };
  latitude: number | null = null;
  longitude: number | null = null;

  constructor(private homeService: HomeService, private router: Router) {}

  createHome(): void {
    if (this.isValidForm()) {
      this.newHome.location = `(${this.latitude}, ${this.longitude})`;
      this.homeService.createHome(this.newHome).subscribe({
        next: () => this.router.navigate(['/list-homes']),
        error: (error) => console.error('Error al crear el hogar:', error)
      });
    } else {
      alert('Por favor, completa todos los campos.');
    }
  }

  isValidForm(): boolean {
    return !!(this.newHome.name && this.newHome.address && this.latitude !== null && this.longitude !== null && this.newHome.capacity);
  }
}
