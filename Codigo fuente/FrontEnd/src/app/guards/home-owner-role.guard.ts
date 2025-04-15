import { CanActivateFn } from '@angular/router';

export const HomeOwnerRoleGuard: CanActivateFn = (route, state) => {
  const user = JSON.parse(localStorage.getItem('connectedUser') || '{}');
  const isHomeowner = user?.role === 'homeOwner';

  if (!isHomeowner) {
    alert('Acceso denegado. Solo permitido para dueños de hogar.');
    window.location.href = '/';
    return false;
  }
  return true;
};
