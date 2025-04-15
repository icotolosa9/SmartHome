import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-list-accounts',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './list-accounts.component.html',
  styleUrl: './list-accounts.component.css'
})
export class ListAccountsComponent implements OnInit {
  accounts: User[] = [];
  pageNumber = 1;
  pageSize = 10;
  totalItems = 0;
  loading: boolean = false;
  fullName: string = '';
  role: string = '';

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadAccounts();
  }

  loadAccounts(): void {
    this.loading = true;
    this.userService.listAccounts(this.pageNumber, this.pageSize, this.fullName, this.role).subscribe({
      next: (response) => {
        this.accounts = response.results;
        this.totalItems = response.totalCount;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error al cargar las cuentas', err);
        this.loading = false;
      }
    });
  }

  applyFilters(): void {
    this.pageNumber = 1; 
    this.loadAccounts();
  }

  nextPage(): void {
    if (this.pageNumber * this.pageSize < this.totalItems) {
      this.pageNumber++;
      this.loadAccounts();
    }
  }

  prevPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadAccounts();
    }
  }

  deleteAdmin(id: string): void {
    if (confirm('¿Estás seguro de que deseas eliminar este administrador?')) {
      this.userService.deleteAdmin(id).subscribe({
        next: () => {
          alert('Administrador eliminado correctamente.');
          this.loadAccounts();
        },
        error: (err) => {
          console.error('Error al eliminar administrador:', err);
          alert('Hubo un problema al intentar eliminar el administrador.');
        }
      });
    }
  }
}