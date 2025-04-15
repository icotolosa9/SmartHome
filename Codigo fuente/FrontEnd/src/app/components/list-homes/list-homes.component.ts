import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HomeService } from '../../services/home.service';
import { Home } from '../../models/home';

interface EditableHome extends Home {
  isEditing: boolean;
}

@Component({
  selector: 'app-list-homes',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './list-homes.component.html',
  styleUrls: ['./list-homes.component.css']
})
export class ListHomesComponent implements OnInit {
  homes: EditableHome[] = [];
  isLoadingHomes: boolean = false;

  constructor(private homeService: HomeService) {}

  ngOnInit(): void {
    this.loadHomes();
  }

  loadHomes(): void {
    this.isLoadingHomes = true;
    this.homeService.getMyHomes().subscribe({
      next: (data) => { this.homes = data.map(home => ({ ...home, isEditing: false })); this.isLoadingHomes = false; },
      error: (error: any) => { console.error('Error al cargar hogares:', error); this.isLoadingHomes = false; }
    });
  }

  toggleEdit(home: EditableHome): void {
    home.isEditing = !home.isEditing;
  }

  saveHomeName(home: EditableHome): void {
    if (home.id !== undefined) {
      this.homeService.updateHomeName(home.id, home.name).subscribe({
        next: (response) => {
          home.isEditing = false;
          console.log(response.message); 
        },
        error: (error: any) => alert(error.error?.message || 'Ocurrió un error al actualizar el nombre del hogar')
      });
    } else {
      console.error('El ID del hogar es undefined');
    }
  }
}