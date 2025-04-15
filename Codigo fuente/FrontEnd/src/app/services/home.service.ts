import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Home } from '../models/home';
import { Device } from '../models/device';
import { HomeMember } from '../models/home-member';
import { Room } from '../models/room';
import { AssignDeviceToRoomRequest } from '../models/assign-device-to-room-request';

@Injectable({
  providedIn: 'root',
})
export class HomeService {
  private apiUrl = `${environment.apiUrl}/homes`;

  constructor(private http: HttpClient) {}

  getMyHomes(): Observable<Home[]> {
    return this.http.get<Home[]>(`${this.apiUrl}/my-homes`);
  }

  getHomeMembers(homeId: string): Observable<HomeMember[]> {
    return this.http.get<HomeMember[]>(`${this.apiUrl}/${homeId}/members`);
  }

  createHome(homeData: any): Observable<any> {
    return this.http.post(this.apiUrl, homeData);
  }

  updateHomeName(homeId: string, newName: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${homeId}`, { NewName: newName });
  }

  addMemberToHome(homeId: string, membersPayload: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/${homeId}/members`, membersPayload);
  }

  associateDevice(homeId: string, request: { deviceName: string; deviceModel: string; connected: boolean }): Observable<any> {
    return this.http.post(`${this.apiUrl}/${homeId}/devices`, request);
  }

  getDevices(homeId: string, roomName?: string): Observable<Device[]> {
    let params = new HttpParams();
    if (roomName) { params = params.set('roomName', roomName); }
    return this.http.get<Device[]>(`${this.apiUrl}/${homeId}/devices`, { params });
  }

  createRoom(request: { name: string; homeId: string }): Observable<Room> {
    return this.http.post<Room>(`${this.apiUrl}/rooms`, request);
  }
  
  assignDeviceToRoom(homeId: string, request: AssignDeviceToRoomRequest): Observable<{ message: string }> {
    return this.http.put<{ message: string }>(`${this.apiUrl}/${homeId}/device-to-room`, request);
  }

  updateDeviceName(hardwareId: string, request: { newName: string }): Observable<{ message: string }> {
    return this.http.put<{ message: string }>(`${this.apiUrl}/${hardwareId}/name`, request);
  }

  setDeviceStatus(hardwareId: string, status: boolean): Observable<{ message: string }> {
    return this.http.put<{ message: string }>(`${this.apiUrl}/${hardwareId}/status?status=${status}`, {});
  } 

  updateMemberNotifications(homeId: string, updates: { homeOwnerEmail: string; isNotificationEnabled: boolean }[]): Observable<any> {
    return this.http.put(`${this.apiUrl}/${homeId}/notification-settings`, updates);
  }  
}
