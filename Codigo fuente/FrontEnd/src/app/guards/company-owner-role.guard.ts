import { CanActivateFn } from '@angular/router';

export const CompanyOwnerRoleGuard: CanActivateFn = (route, state) => {
  const user = JSON.parse(localStorage.getItem('connectedUser') || '{}');
  const isCompanyOwner = user?.role === 'companyOwner';

  if (!isCompanyOwner) {
    alert('Acceso denegado. Solo permitido para dueños de empresas.');
    window.location.href = '/';
    return false;
  }
  return true;
};
