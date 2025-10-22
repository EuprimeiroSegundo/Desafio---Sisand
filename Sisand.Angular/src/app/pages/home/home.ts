import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { AutenticacaoService } from '../../services/autenticacao/autenticacao-service';
import { ApiService } from '../../services/api/api-service';
import { Usuario } from '../../models/usuario';
import { CommonModule } from '@angular/common';
import { TelefonePipe } from '../../pipes/telefone/telefone-pipe';
import { CadastroUsuarioModal } from '../../modals/cadastro-usuario-modal/cadastro-usuario-modal';
import { Router } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PERMISSOES_DISPONIVEIS } from '../../utils/permissoes';

declare var bootstrap: any; 

interface FilterForm {
  login: string | null;
  nomeCompleto: string | null;
  permissao: string | null;
}

@Component({
  selector: 'app-home',
  imports: [CommonModule, TelefonePipe, CadastroUsuarioModal, ReactiveFormsModule, FormsModule],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class Home implements OnInit{
@ViewChild(CadastroUsuarioModal) usuarioFormModal!: CadastroUsuarioModal;

  public authService = inject(AutenticacaoService);
  private usuarioService = inject(ApiService);
  private router = inject(Router);

  usuarios: Usuario[] = [];
  filteredUsuarios: Usuario[] = [];
  loading = true;
  error = false;
  
  loginToDelete: string | null = null;
  deleteModalRef: any;
  deleteLoading = false;
  deleteError: string | null = null;

  permissoesDisponiveis = ['Todos', ...PERMISSOES_DISPONIVEIS];
  filterForm: FilterForm = { login: null, nomeCompleto: null, permissao: 'Todos' };
  filterModalRef: any;


  ngOnInit(): void {
    this.carregarUsuarios();
  }

  canSeeActions(usuario: Usuario): boolean {
    const nivelLogado = this.authService.getNivelPermissao();
    const loginLogado = this.authService.currentUser()?.usuario?.login;
    
    if (nivelLogado < 2) {
      return false;
    }

    if (nivelLogado >= 3) {
      return true;
    }

    return loginLogado === usuario.login;
  }

  canDeleteUser(usuario: Usuario): boolean {
    const nivelLogado = this.authService.getNivelPermissao();
    const loginLogado = this.authService.currentUser()?.usuario?.login;

    if (nivelLogado < 3) {
      return false;
    }
    
    return loginLogado !== usuario.login;
  }

  carregarUsuarios(): void {
    this.loading = true;
    this.error = false;

    this.usuarioService.getListarUsuarios().subscribe({
      next: (data) => {
        this.usuarios = data;
        this.applyFilters();
        this.loading = false;
      },
      error: (err) => {
        console.error('Erro ao carregar lista de usuários:', err);
        this.error = true;
        this.loading = false;
      }
    });
  }
  
  cadastrarUsuario(): void {
    this.usuarioFormModal.abrirModal(null); 
  }

  alterarDadosProprios(): void {
    const login = this.authService.currentUser()?.usuario?.login;
    if (login) {
      this.usuarioFormModal.abrirModal(login);
    }
  }

  alterarUsuario(usuario: Usuario): void {
    this.usuarioFormModal.abrirModal(usuario.login);
  }

  deletarUsuario(login: string): void {
    this.loginToDelete = login;
    this.deleteError = null;
    this.deleteLoading = false;

    if (!this.deleteModalRef) {
        this.deleteModalRef = new bootstrap.Modal(document.getElementById('deleteConfirmModal'), {});
    }
    this.deleteModalRef.show();
  }

  confirmDelete(): void {
    if (!this.loginToDelete) return;

    this.deleteLoading = true;
    this.deleteError = null;

    this.usuarioService.deleteDeletarUsuario(this.loginToDelete).subscribe({
        next: () => {
            this.deleteLoading = false;
            this.deleteModalRef.hide();
            this.loginToDelete = null; 
            this.carregarUsuarios(); 
        },
        error: (err) => {
            this.deleteError = err.error?.message || 'Erro ao deletar usuário. Tente novamente.';
            this.deleteLoading = false;
        }
    });
  }

  cancelDelete(): void {
    this.deleteModalRef.hide();
    this.loginToDelete = null;
  }

  showFilterModal(): void {
    if (!this.filterModalRef) {
        this.filterModalRef = new bootstrap.Modal(document.getElementById('filterModal'), {});
    }
    this.filterModalRef.show();
  }
  
  isFilterButtonDisabled(): boolean {
    const login = this.filterForm.login?.trim();
    const nomeCompleto = this.filterForm.nomeCompleto?.trim();
    const permissao = this.filterForm.permissao;

    return !login && !nomeCompleto && permissao === 'Todos';
  }

  applyFilters(): void {
    const { login, nomeCompleto, permissao } = this.filterForm;

    const lowerLogin = login?.trim().toLowerCase() || '';
    const lowerNomeCompleto = nomeCompleto?.trim().toLowerCase() || '';
    const filterPermissao = permissao !== 'Todos' ? permissao : null;

    this.filteredUsuarios = this.usuarios.filter(usuario => {
      let match = true;

      if (lowerLogin) {
        match = match && usuario.login.toLowerCase().includes(lowerLogin);
      }

      if (lowerNomeCompleto) {
        match = match && usuario.nomeCompleto.toLowerCase().includes(lowerNomeCompleto);
      }

      if (filterPermissao) {
        match = match && usuario.permissao === filterPermissao;
      }

      return match;
    });

    if (this.filterModalRef) {
      this.filterModalRef.hide();
    }
  }
  
  clearFilters(): void {
    this.filterForm = { login: null, nomeCompleto: null, permissao: 'Todos' };
    this.applyFilters();
  }
  
  onUsuarioCadastrado(): void {
    this.carregarUsuarios();
  }

  onUsuarioAlterado(dadosAtualizados: any): void {
    const userLogado = this.authService.currentUser();
    const loginAlterado = dadosAtualizados.login;
    
    if (userLogado && userLogado.usuario?.login === loginAlterado) {
      
      if (dadosAtualizados.senha) {
        
        const credenciais = {
          loginEmail: loginAlterado,
          senha: dadosAtualizados.senha
        };

        this.authService.login(credenciais).subscribe({
          next: () => {
            this.carregarUsuarios();
          },
          error: (err) => {
            this.authService.logout();
            this.router.navigate(['/login']);
          }
        });
        return; 
      }
    } 
    
    this.carregarUsuarios(); 
  }

  logout(): void {
    this.authService.logout();
  }
}