import { Component, OnInit } from '@angular/core';
import { DeviceService } from '../../services/device.service'; 
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-import-devices',
  templateUrl: './import-devices.component.html',
  styleUrls: ['./import-devices.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule],
})
export class ImportDevicesComponent implements OnInit {
  importers: string[] = []; 
  selectedImporter: string | null = null; 
  importStatus: string | null = null; 
  importError: string | null = null; 

  constructor(private deviceService: DeviceService, private router: Router) {}

  ngOnInit(): void {
    this.loadImporters();
  }

  loadImporters(): void {
    this.deviceService.getImporters().subscribe({
      next: (data) => (this.importers = data),
      error: (err) => {
        console.error('Error al cargar los importadores:', err);
        this.importError = 'No se pudieron cargar los importadores.';
      },
    });
  }

  importDevices(): void {
    if (!this.selectedImporter) {
      this.importError = 'Por favor, seleccione un importador.';
      return;
    }

    this.importStatus = null;
    this.importError = null;

    this.deviceService.importDevices(this.selectedImporter).subscribe({
      next: (response) => {
        this.importStatus = response.message;
        alert('¡Dispositivos importados exitosamente!');
        this.router.navigate(['/list-companies']); 
      },
      error: (err) => {
        console.error('Error durante la importación:', err);
        this.importError = err.error?.message || 'Ocurrió un error durante la importación.';
      },
    });
  }
}
