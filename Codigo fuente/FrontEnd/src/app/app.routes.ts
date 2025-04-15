import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { AdminRoleGuard } from './guards/admin-role.guard';
import { HomeOwnerRoleGuard } from './guards/home-owner-role.guard';
import { CompanyOwnerRoleGuard } from './guards/company-owner-role.guard';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { CreateCompanyComponent } from './components/create-company/create-company.component';
import { ListCompaniesComponent } from './components/list-companies/list-companies.component';
import { RegisterDeviceComponent } from './components/register-device/register-device.component';
import { ListHomesComponent } from './components/list-homes/list-homes.component';
import { HomeDetailsComponent } from './components/home-details/home-details.component';
import { ListAccountsComponent } from './components/list-accounts/list-accounts.component';
import { CreateCompanyOwnerComponent } from './components/create-company-owner/create-company-owner.component';
import { ListDevicesComponent } from './components/list-devices/list-devices.component';
import { ListDeviceTypesComponent } from './components/list-device-types/list-device-types.component';
import { CreateHomeComponent } from './components/create-home/create-home.component';
import { CreateAdminComponent } from './components/create-admin/create-admin.component';
import { CreateHomeOwnerComponent } from './components/create-home-owner/create-home-owner.component';
import { AddHomeMembersComponent } from './components/add-home-members/add-home-members.component';
import { AssociateDeviceComponent } from './components/associate-device/associate-device.component';
import { ImportDevicesComponent } from './components/import-devices/import-devices.component';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },  
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'create-company', component: CreateCompanyComponent, canActivate: [CompanyOwnerRoleGuard] },
  { path: 'list-companies', component: ListCompaniesComponent },
  { path: 'register-device', component: RegisterDeviceComponent, canActivate: [CompanyOwnerRoleGuard] },
  { path: 'list-homes', component: ListHomesComponent, canActivate: [AuthGuard] },
  { path: 'home-details/:id', component: HomeDetailsComponent },
  { path: 'list-accounts', component: ListAccountsComponent, canActivate: [AdminRoleGuard] },
  { path: 'create-company-owner', component: CreateCompanyOwnerComponent, canActivate: [AdminRoleGuard] },
  { path: 'list-devices', component: ListDevicesComponent, canActivate: [AuthGuard] },
  { path: 'list-device-types', component: ListDeviceTypesComponent, canActivate: [AuthGuard] },
  { path: 'create-home', component: CreateHomeComponent },
  { path: 'create-admin', component: CreateAdminComponent, canActivate: [AdminRoleGuard] },
  { path: 'register', component: CreateHomeOwnerComponent },
  { path: 'add-member/:homeId', component: AddHomeMembersComponent },
  { path: 'associate-device/:homeId', component: AssociateDeviceComponent },
  { path: 'import-devices', component: ImportDevicesComponent, canActivate: [CompanyOwnerRoleGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }