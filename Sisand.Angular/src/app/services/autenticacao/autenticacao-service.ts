import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { LoginReponse } from '../../models/login-reponse';

const NIVEL_PERMISSAO: { [key: string]: number } = {
    'admin': 3,
    'usuario': 2,
    'visitante': 1,
};

@Injectable({
  providedIn: 'root'
})
export class AutenticacaoService {
private http = inject(HttpClient);
  private router = inject(Router);

  private readonly AUTENTICACAO_URL = 'http://localhost:8080/api/Autenticacao'; 
  private readonly USER_KEY = 'currentUser'; 

  private userSignal = signal<LoginReponse | null>(this.getStoredUser());

  currentUser = this.userSignal.asReadonly(); 

  constructor() {
    this.userSignal.set(this.getStoredUser());
  }

  login(credentials: any): Observable<any> {
    return this.http.post<LoginReponse>(`${this.AUTENTICACAO_URL}/Login`, credentials).pipe(
      tap((UsuarioLogin: LoginReponse) => {
        this.setStoredUser(UsuarioLogin);
        this.userSignal.set(UsuarioLogin);

        this.router.navigate(['/home']);
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.USER_KEY);
    this.userSignal.set(null);
    this.router.navigate(['/login']);
  }

  estaLogado(): boolean {
    return !!this.userSignal();
  }
  
  private setStoredUser(UsuarioLogin: LoginReponse): void {
    localStorage.setItem(this.USER_KEY, JSON.stringify(UsuarioLogin));
  }

  private getStoredUser(): LoginReponse | null {
    const userString = localStorage.getItem(this.USER_KEY);
    return userString ? JSON.parse(userString) : null;
  }

  getNivelPermissao(): number {
    const user = this.currentUser();
    if (!user) {
      return 0;
    }
    return NIVEL_PERMISSAO[user.usuario?.permissao] || 0;
  }

  temPermissaoMinima(nivelRequerido: number): boolean {
    return this.getNivelPermissao() >= nivelRequerido;
  }
}