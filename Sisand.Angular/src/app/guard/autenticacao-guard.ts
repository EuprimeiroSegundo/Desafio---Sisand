import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AutenticacaoService } from '../services/autenticacao/autenticacao-service';

export const autenticacaoGuard: CanActivateFn = (route, state) => {
  const authService = inject(AutenticacaoService);
  const router = inject(Router);

  if (authService.estaLogado()) {
    return true; 
  }
  return router.createUrlTree(['/login']);
};