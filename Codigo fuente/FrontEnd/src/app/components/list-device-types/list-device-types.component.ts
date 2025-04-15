import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DeviceService } from '../../services/device.service';

@Component({
  selector: 'app-list-device-types',
  templateUrl: './list-device-types.component.html',
  styleUrls: ['./list-device-types.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})

export class ListDeviceTypesComponent implements OnInit {
  deviceTypes: string[] = [];

  constructor(private deviceService: DeviceService) {}

  ngOnInit(): void {
    this.getDeviceTypes();
  }

  getDeviceTypes(): void {
    this.deviceService.getSupportedDeviceTypes().subscribe({
      next: (data: string[]) => { 
        this.deviceTypes = data;
      },
      error: (error) => {
        console.error('Error al obtener los tipos de dispositivos:', error);
      }
    });
  }
}
