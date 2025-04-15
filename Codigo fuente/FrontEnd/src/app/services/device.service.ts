import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Device } from '../models/device';
import { PagedResult } from '../models/paged-result';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  private apiUrl = `${environment.apiUrl}/devices`;

  constructor(private http: HttpClient) {}

  listDevices(pageNumber: number, pageSize: number, filters: any): Observable<PagedResult<Device>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (filters.name) params = params.set('name', filters.name);
    if (filters.model) params = params.set('model', filters.model);
    if (filters.company) params = params.set('company', filters.company);
    if (filters.type) params = params.set('devicetype', filters.type);

    return this.http.get<PagedResult<Device>>(this.apiUrl, { params });
  }

  getSupportedDeviceTypes(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/types`);
  }

  registerDevice(deviceData: any): Observable<any> {
    let endpoint = '';
    switch (deviceData.type) {
      case 'windowSensor':
        endpoint = `${this.apiUrl}/window-sensors`;
        break;
      case 'camera':
        endpoint = `${this.apiUrl}/cameras`;
        break;
      case 'smartLamp':
        endpoint = `${this.apiUrl}/smart-lamps`;
        break;
      case 'motionSensor':
        endpoint = `${this.apiUrl}/motion-sensors`;
        break;
      default:
        return new Observable(observer => {
          observer.error('Tipo de dispositivo no soportado');
        });
    }
    return this.http.post(endpoint, deviceData);
  }

  toggleWindowState(hardwareId: string, isOpen: boolean): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.apiUrl}/${hardwareId}/window-actions`, isOpen);
  }
  
  toggleSmartLamp(hardwareId: string, isOn: boolean): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.apiUrl}/${hardwareId}/smart-light-actions`, isOn);
  }
  
  detectMotionSensor(hardwareId: string): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.apiUrl}/${hardwareId}/sensor-motion-actions`, {});
  }
  
  detectMotionCamera(hardwareId: string): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.apiUrl}/${hardwareId}/motion-detected-actions`, {});
  }
  
  detectPersonCamera(deviceId: string, personEmail: string): Observable<{ message: string }> {
  return this.http.post<{ message: string }>(
    `${environment.apiUrl}/devices/${deviceId}/person-detected-actions`,
    personEmail,
    { headers: { 'Content-Type': 'application/json' } } 
    );
  }

  getImporters(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/importers`);
  }

  importDevices(importerType: string): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.apiUrl}/imported-devices?importerType=${encodeURIComponent(importerType)}`,
      {}
    );
  }
}
