import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HomeService } from '../../services/home.service';
import { HomeMember } from '../../models/home-member';
import { Device } from '../../models/device';
import { Room } from '../../models/room';
import { AssignDeviceToRoomRequest } from '../../models/assign-device-to-room-request';
import { DeviceService } from '../../services/device.service';
import { UserService } from '../../services/user.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-home-details',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './home-details.component.html',
  styleUrls: ['./home-details.component.css'],
})
export class HomeDetailsComponent implements OnInit {
  homeId: string;
  members: HomeMember[] = [];
  devices: Device[] = [];
  roomName: string = '';
  isEditingNotifications: boolean = false;
  loadingMembers: boolean = true;
  loadingDevices: boolean = true;
  showMembers = false;
  showDevices = false;

  constructor( private route: ActivatedRoute, private homeService: HomeService, private deviceService: DeviceService, private userService: UserService, private notificationService: NotificationService) {
    this.homeId = this.route.snapshot.paramMap.get('id')!;
  }

  ngOnInit(): void {
    this.homeId = this.route.snapshot.paramMap.get('id')!;
  }

  toggleMembers(): void {
    this.showMembers = !this.showMembers;
    if (this.showMembers && !this.members.length) {
      this.getHomeMembers();
    }
  }

  toggleDevices(): void {
    this.showDevices = !this.showDevices;
    if (this.showDevices && !this.devices.length) {
      this.loadDevices();
    }
  }

  getHomeMembers(): void {
    this.loadingMembers = true;
    this.homeService.getHomeMembers(this.homeId).subscribe({
      next: (data) => {
        this.members = data || [];
        this.loadingMembers = false;
      },
      error: (err) => {
        alert(err.error?.message || 'Error al cargar miembros del hogar');
        this.loadingMembers = false;
      }
    });
  }
  
  loadDevices(): void {
    this.loadingDevices = true;
    this.homeService.getDevices(this.homeId, this.roomName).subscribe({
      next: (response) => {
        this.devices = response || []; 
        this.loadingDevices = false;
      },
      error: (err) => { console.error('Error al cargar dispositivos', err); alert(err.error?.message || 'Error al cargar dispositivos'); 
        this.loadingDevices = false; },
    });
  }

  applyFilter(): void { this.loadDevices(); }

  openCreateRoomModal(): void {
    const roomName = prompt('Ingrese el nombre del cuarto:');
    if (roomName) {
      const request = { name: roomName, homeId: this.homeId };
      this.homeService.createRoom(request).subscribe({
        next: (room: Room) => {
          alert(`El cuarto "${room.name}" fue creado exitosamente.`);
        },
        error: (err) => { console.error('Error al crear el cuarto', err); },
      });
    }
  }

  openAssignRoomModal(device: Device): void {
    const roomName = prompt(
      `Ingrese el nombre del cuarto para asignar el dispositivo "${device.name}":`
    );
    if (roomName) {
      const request: AssignDeviceToRoomRequest = { hardwareId: device.id, roomName: roomName };
      this.homeService.assignDeviceToRoom(this.homeId, request).subscribe({
        next: (response) => {
          alert(response.message); 
          this.loadDevices();
        },
        error: (err) => {
          console.error('Error al asignar el dispositivo al cuarto', err);
          alert(err.error?.message || 'Error al asignar el dispositivo al cuarto');
        },
      });
    }
  }

  openEditDeviceNameModal(device: Device): void {
    const newName = prompt(`Ingrese el nuevo nombre para el dispositivo "${device.name}":`);
    if (newName && newName.trim()) {
      this.homeService.updateDeviceName(device.id, { newName }).subscribe({
        next: (response) => {
          alert(response.message); 
          this.loadDevices(); 
        },
        error: (err) => {
          alert(err.error?.message || 'Error al actualizar el nombre del dispositivo');
        },
      });
    }
  }

  toggleDeviceConnection(device: Device, status: boolean): void {
    this.homeService.setDeviceStatus(device.id, status).subscribe({
      next: (response) => {
        alert(response.message); 
        this.loadDevices(); 
      },
      error: (err) => {
        console.error('Error al cambiar el estado de conexión del dispositivo', err);
        alert(err.error?.message || 'Error al cambiar el estado de conexión del dispositivo');
      },
    });
  }

  triggerAction(device: any): void {
    switch (device.deviceType) {
      case 'Smart Lamp':
        const lampAction = device.openOrOn ? 'apagar' : 'encender';
        const lampConfirmation = confirm(`¿Quieres ${lampAction} la lámpara "${device.name}"?`);
        if (lampConfirmation) {
          this.deviceService.toggleSmartLamp(device.id, !device.openOrOn).subscribe({
            next: (response) => {
              alert(response.message);
              device.openOrOn = !device.openOrOn;
              this.updateMyNotifications();
            },
            error: (err) => console.error('Error:', err),
          });
        }
        this.loadDevices(); 
        break;
      case 'Window Sensor':
        const windowAction = device.openOrOn ? 'cerrar' : 'abrir';
        const windowConfirmation = confirm(`¿Quieres ${windowAction} la ventana "${device.name}"?`);
        if (windowConfirmation) {
          this.deviceService.toggleWindowState(device.id, !device.openOrOn).subscribe({
            next: (response) => {
              alert(response.message);
              device.openOrOn = !device.openOrOn;
              this.updateMyNotifications(); 
            },
            error: (err) => console.error('Error:', err),
          });
        }
        this.loadDevices(); 
        break;
        case 'Motion Sensor':
         this.deviceService.detectMotionSensor(device.id).subscribe({
           next: (response) => { alert(response.message); this.updateMyNotifications(); }, 
           error: (err) => console.error('Error:', err),
         });
         break;
         case 'Camera':
          if (device.supportsMotionDetection) {
            const motionConfirmation = confirm(`¿Quieres detectar movimiento con la cámara "${device.name}"?`);
            if (motionConfirmation) {
              this.deviceService.detectMotionCamera(device.id).subscribe({
                next: (response) => { alert(response.message); this.updateMyNotifications(); },
                error: (err) => console.error('Error:', err),
              });
            }
          }
          if (device.supportsPersonDetection) {
            const personEmail = prompt(`Ingresa el correo de la persona detectada con la cámara "${device.name}":`);
            if (personEmail) {
              this.deviceService.detectPersonCamera(device.id, `"${personEmail}"`).subscribe({
                next: (response) => { alert(response.message); this.updateMyNotifications(); },
                error: (err) => console.error('Error:', err),
              });
            }
          }
          break;
      default:
        alert('Acción no soportada para este dispositivo.');
        break;
    }
  }

  private updateMyNotifications(): void {
    this.userService.getNotifications().subscribe({
      next: (notifications) => {
        this.notificationService.updateUnreadStatus(notifications);
      },
      error: (err) => alert(err.error?.message || 'Error al cargar las notificaciones'),
    });
  }
  
  updateNotifications(): void {
    const updates = this.members.map(member => ({
      homeOwnerEmail: member.email,
      isNotificationEnabled: member.isNotificationsOn
    }));
  
    this.homeService.updateMemberNotifications(this.homeId, updates).subscribe({
      next: () => {
        alert('Notificaciones actualizadas correctamente.');
        this.isEditingNotifications = false;
      },
      error: (error) => {
        console.error('Error al actualizar notificaciones:', error);
        alert(error.error?.message || 'Error al actualizar notificaciones.');
      }
    });
  }  
  
  toggleEditNotifications(): void {
    this.isEditingNotifications = !this.isEditingNotifications;
  }
}
