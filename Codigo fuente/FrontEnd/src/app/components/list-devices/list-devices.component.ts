import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DeviceService } from '../../services/device.service';
import { Device } from '../../models/device';
import { PagedResult } from '../../models/paged-result';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-list-devices',
  templateUrl: './list-devices.component.html',
  styleUrls: ['./list-devices.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
})
export class ListDevicesComponent implements OnInit {
  devices: Device[] = [];
  pageNumber = 1;
  pageSize = 10;
  totalItems = 0;
  isLoading: boolean = false;
  filters = { name: '', model: '', company: '', type: '' };

  constructor(private deviceService: DeviceService) {}

  ngOnInit(): void {
    this.getDevices();
  }

  getDevices(): void {
    this.isLoading = true; 
    this.deviceService.listDevices(this.pageNumber, this.pageSize, this.filters).subscribe({
        next: (response) => {
          this.devices = response.results;
          this.totalItems = response.totalCount;
          this.isLoading = false; 
        },
        error: (err) => {
          console.error('Error al cargar dispositivos:', err);
          this.isLoading = false;
        },
      });
  }

  applyFilters(): void {
    this.pageNumber = 1;
    this.getDevices();
  }

  nextPage(): void {
    this.pageNumber++;
    this.getDevices();
  }

  prevPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.getDevices();
    }
  }
}
