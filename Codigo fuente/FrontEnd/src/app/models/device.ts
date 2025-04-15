export interface Device {
  id: string;
  name: string;
  model: string;
  mainPicture: string;
  company: string;
  connected: boolean;
  openOrOn?: boolean; 
  roomName?: string;
}