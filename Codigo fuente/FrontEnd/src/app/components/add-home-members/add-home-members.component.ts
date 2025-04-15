import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, FormControl, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { HomeService } from '../../services/home.service';
import { ActivatedRoute } from '@angular/router';
import { RouterModule, Router } from '@angular/router';

@Component({
  selector: 'app-add-member',
  templateUrl: './add-home-members.component.html',
  standalone: true,
  imports: [FormsModule, CommonModule, ReactiveFormsModule, RouterModule],
  styleUrls: ['./add-home-members.component.css']
})

export class AddHomeMembersComponent implements OnInit {
  membersForm: FormGroup;
  availablePermissions = ['listDevices', 'AssignDevice', 'RenameDevice', 'AddDevice'];
  homeId: string | null = null;

  constructor(private fb: FormBuilder, private route: ActivatedRoute, private homeService: HomeService, private router: Router) {
    this.membersForm = this.fb.group({
      members: this.fb.array([]),
    });
  }

  ngOnInit(): void {
    this.homeId = this.route.snapshot.paramMap.get('homeId');
    this.addMember(); 
  }

  get members(): FormArray {
    return this.membersForm.get('members') as FormArray;
  }

  addMember(): void {
    const memberForm = this.fb.group({
      UserEmail: ['', Validators.required],
      Permissions: [[]],
    });
    this.members.push(memberForm);
  }

  removeMember(index: number): void {
    this.members.removeAt(index);
  }

  onPermissionChange(index: number, permission: string, event: Event): void {
    const member = this.members.at(index);
    const permissions = member.get('Permissions')?.value || [];
    const isChecked = (event.target as HTMLInputElement).checked;

    if (isChecked) {
      permissions.push(permission);
    } else {
      const idx = permissions.indexOf(permission);
      if (idx !== -1) {
        permissions.splice(idx, 1);
      }
    }

    member.get('Permissions')?.setValue(permissions);
  }

  submitMembers(): void {
    if (this.membersForm.valid && this.homeId) {
      const payload = { Members: this.membersForm.value.members };
      this.homeService.addMemberToHome(this.homeId, payload).subscribe({
        next: () => {
          alert('Miembros agregados exitosamente');
          this.router.navigate(['/home-details', this.homeId]);
        },
        error: (error) => {
          alert(error.error?.message || 'Ocurrió un error al agregar miembros.');
        },
      });
    } else {
      console.log('Formulario inválido o falta el homeId');
    }
  }
}