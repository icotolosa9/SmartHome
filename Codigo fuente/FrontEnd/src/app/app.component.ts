import { Component } from '@angular/core';
import { TopNavComponent } from './components/topnav/topnav.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [TopNavComponent, RouterOutlet],  
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent { }