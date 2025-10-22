import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Home } from './pages/home/home';
import { autenticacaoGuard } from './guard/autenticacao-guard';

export const routes: Routes = [

  { path: 'login', component: Login },
  { 
    path: 'home', 
    component: Home,
    canActivate: [autenticacaoGuard] 
  },
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '**', redirectTo: 'home' } 
];
