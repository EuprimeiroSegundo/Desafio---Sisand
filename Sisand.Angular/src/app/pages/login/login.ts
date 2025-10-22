import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AutenticacaoService } from '../../services/autenticacao/autenticacao-service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  private authService = inject(AutenticacaoService);
  private router = inject(Router);

  loginData = {
    loginEmail: '',
    senha: ''
  };
  
  error = false;

  constructor() {
    if (this.authService.estaLogado()) {
      this.router.navigate(['/home']);
    }
  }

  onSubmit(): void {
    this.error = false;
    
    this.authService.login(this.loginData).subscribe({
      next: (response) => {
        console.log('Login realizado com sucesso!', response);
      },
      error: (err) => {
        console.error('Erro no login:', err);
        this.error = true;
      }
    });
  }
}
