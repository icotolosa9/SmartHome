import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { CreateCompanyOwner } from '../../models/create-company-owner';
import { ReactiveFormsModule } from '@angular/forms'; 

@Component({
  selector: 'app-create-company-owner',
  templateUrl: './create-company-owner.component.html',
  styleUrls: ['./create-company-owner.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule] 
})
export class CreateCompanyOwnerComponent {
  createOwnerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router
  ) {
    this.createOwnerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit(): void {
    if (this.createOwnerForm.valid) {
      const newCompanyOwner: CreateCompanyOwner = this.createOwnerForm.value;
      this.userService.createCompanyOwner(newCompanyOwner).subscribe({
        next: () => {
          alert('Cuenta de dueño de empresa creada exitosamente.');
          this.router.navigate(['/list-accounts']); 
        },
        error: (error) => {
          alert(error.error?.message || 'Ocurrió un error al crear el dueño de empresa.');
        }
      });
    }
  }
}
