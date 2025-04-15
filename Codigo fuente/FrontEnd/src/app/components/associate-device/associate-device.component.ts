import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HomeService } from '../../services/home.service';

@Component({
  selector: 'app-associate-device',
  templateUrl: './associate-device.component.html',
  styleUrls: ['./associate-device.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule] 
})
export class AssociateDeviceComponent implements OnInit {
  associateDeviceForm: FormGroup;
  homeId: string = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private homeService: HomeService,
    private router: Router
  ) {
    this.associateDeviceForm = this.fb.group({
      deviceName: ['', Validators.required],
      deviceModel: ['', Validators.required],
      connected: [false, Validators.required]
    });
  }

  ngOnInit(): void {
    this.homeId = this.route.snapshot.paramMap.get('homeId') || '';
  }

  onSubmit(): void {
    if (this.associateDeviceForm.valid) {
      const request = this.associateDeviceForm.value;
      this.homeService.associateDevice(this.homeId, request).subscribe({
        next: () => {
          alert('Dispositivo asociado correctamente');
          this.router.navigate(['/home-details', this.homeId]);
        },
        error: (err) => {
          alert(err.error?.message || 'Ocurrió un error al asociar el dispositivo');
        }
      });
    }
  }
}
