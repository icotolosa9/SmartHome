import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DeviceService } from '../../services/device.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-device',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './register-device.component.html',
  styleUrl: './register-device.component.css'
})

export class RegisterDeviceComponent implements OnInit {
  deviceName = '';
  deviceModel = '';
  deviceDescription = '';
  photographs: string[] = []; 
  newPhotoUrl = '';
  deviceType = '';
  deviceTypes = [
    { label: 'Sensor de Ventana', value: 'windowSensor' },
    { label: 'Cámara', value: 'camera' },
    { label: 'Lámpara Inteligente', value: 'smartLamp' },
    { label: 'Sensor de Movimiento', value: 'motionSensor' }
  ];
  additionalFields = {
    indoorUse: false,
    outdoorUse: false,
    motionDetection: false,
    personDetection: false,
  };

  constructor(private deviceService: DeviceService, private router: Router) {}

  ngOnInit(): void {}

  onDeviceTypeChange(): void {
    if (this.deviceType !== 'camera') {
      this.additionalFields = {
        indoorUse: false,
        outdoorUse: false,
        motionDetection: false,
        personDetection: false,
      };
    }
  }

  addPhoto(): void {
    if (this.newPhotoUrl) {
      this.photographs.push(this.newPhotoUrl);
      this.newPhotoUrl = ''; 
    }
  }

  removePhoto(index: number): void {
    this.photographs.splice(index, 1); 
  }

  isFormValid(): boolean {
    const isCameraValid = this.deviceType === 'camera' 
    ? !(this.additionalFields.motionDetection === this.additionalFields.personDetection)  
    : true; 
    return (
      this.deviceName.trim() !== '' &&
      this.deviceModel.trim() !== '' &&
      this.deviceDescription.trim() !== '' &&
      this.photographs.length > 0 &&
      this.deviceType.trim() !== '' && isCameraValid
    );
  }

  isInvalidDetectionSelection(): boolean {
    return !(this.additionalFields.motionDetection === this.additionalFields.personDetection); 
  }

  onSubmit(): void {
    const payload: any = {
      name: this.deviceName,
      modelNumber: this.deviceModel,
      description: this.deviceDescription,
      photographs: this.photographs,
      type: this.deviceType,
    };
  
    if (this.deviceType === 'camera') {
      payload.indoorUse = this.additionalFields.indoorUse;
      payload.outdoorUse = this.additionalFields.outdoorUse;
      payload.motionDetection = this.additionalFields.motionDetection;
      payload.personDetection = this.additionalFields.personDetection;
    }
  
    this.deviceService.registerDevice(payload).subscribe({
      next: () => {
        alert('Dispositivo registrado con éxito');
        this.router.navigate(['/list-companies']); 
      },
      error: (err) => {
        if (err.error && err.error.message) {
          alert(`Error al registrar el dispositivo: ${err.error.message}`);
        } else {
          alert('Error inesperado al registrar el dispositivo.');
        }
        console.error('Error al registrar el dispositivo:', err);
      },
    });
  }
}
