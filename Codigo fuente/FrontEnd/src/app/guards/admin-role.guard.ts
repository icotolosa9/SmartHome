import { CanActivateFn } from '@angular/router';

export const AdminRoleGuard: CanActivateFn = (route, state) => {
  const user = JSON.parse(localStorage.getItem('connectedUser') || '{}');
  const isAdmin = user?.role === 'admin';

  if (!isAdmin) {
    alert('Acceso denegado. Solo permitido para administradores.');
    window.location.href = '/';
    return false;
  }
  return true;
};