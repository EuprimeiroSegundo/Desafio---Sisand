import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AutenticacaoService } from '../../services/autenticacao/autenticacao-service';
import { ApiService } from '../../services/api/api-service';
import { NIVEL_PERMISSAO, PERMISSOES_DISPONIVEIS } from '../../utils/permissoes';
import { CommonModule } from '@angular/common';
import { Usuario } from '../../models/usuario';
import { firstValueFrom } from 'rxjs';
import { AlterarUsuario } from '../../models/alterar-usuario';

declare var bootstrap: any;

@Component({
  selector: 'app-cadastro-usuario-modal',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './cadastro-usuario-modal.html',
  styleUrl: './cadastro-usuario-modal.scss'
})
export class CadastroUsuarioModal {
private fb = inject(FormBuilder);
  private authService = inject(AutenticacaoService);
  private usuarioService = inject(ApiService);
  
  alterarSenha = false; 
  private userInModal: Usuario | null = null;

  @Input() userToEdit: Usuario | null = null; 

  @Output() modalFechado = new EventEmitter<void>();
  @Output() usuarioCadastrado = new EventEmitter<void>();
  @Output() usuarioAlterado = new EventEmitter<any>(); 

  cadastroForm!: FormGroup;
  permissoesDisponiveis: string[] = [];
  modalRef: any;
  loading = false;
  erroCadastro: string | null = null;
  isEditing = false;
  isLoggedUserAdmin = false;

  ngOnInit(): void {
    this.isLoggedUserAdmin = this.authService.temPermissaoMinima(3);
    this.inicializarFormulario();
  }

  inicializarFormulario(): void {
    this.cadastroForm = this.fb.group({
      login: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]],
      nomeCompleto: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(150)]],
      telefone: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(11), Validators.pattern(/^\d+$/)]],
      permissao: ['', Validators.required],
      senha: [{ value: '', disabled: true }], 
    });
  }
  

  public async abrirModal(loginToEdit: string | null = null): Promise<void> {
    this.userInModal = null; 
    this.erroCadastro = null;
    this.isEditing = !!loginToEdit;
    this.loading = !!loginToEdit;

    if (loginToEdit) {
        try {
            this.userInModal = await firstValueFrom(this.usuarioService.getBuscarUsuarioPorLoginEmail(loginToEdit));
        } catch (error) {
            this.erroCadastro = 'Não foi possível carregar os dados do usuário para edição.';
            this.loading = false;
            return;
        }
    }

    this.loading = false;
    this.setupForm(this.userInModal);
    this.abrirBootstrapModal('cadastroUsuarioModal');
  }

  setupForm(userData: Usuario | null): void {
    this.cadastroForm.enable(); 
    this.alterarSenha = false;
    this.cadastroForm.get('senha')?.clearValidators();
    this.cadastroForm.get('senha')?.disable(); 
    this.cadastroForm.get('senha')?.setValue('');
    this.cadastroForm.get('senha')?.updateValueAndValidity();

    if (userData) {
      this.cadastroForm.patchValue(userData);
      this.cadastroForm.get('login')?.disable(); 
      
      if (!this.isLoggedUserAdmin) {
          this.cadastroForm.get('permissao')?.disable();
      }

    } else {
      this.cadastroForm.reset();
      this.onAlterarSenhaChange(true); 
    }
    
    this.filtrarPermissoes(userData);
  }

  onAlterarSenhaChange(checked: boolean): void {
    this.alterarSenha = checked;
    const senhaControl = this.cadastroForm.get('senha');

    if (checked) {
      senhaControl?.enable();
      senhaControl?.setValidators([Validators.required, Validators.minLength(8), Validators.maxLength(150)]);
    } else {
      senhaControl?.disable();
      senhaControl?.setValue('');
      senhaControl?.clearValidators();
    }
    senhaControl?.updateValueAndValidity();
  }

  filtrarPermissoes(userData: Usuario | null): void {
    const nivelUsuarioLogado = this.authService.getNivelPermissao();
    this.permissoesDisponiveis = PERMISSOES_DISPONIVEIS.filter(permissao => {
      return NIVEL_PERMISSAO[permissao] <= nivelUsuarioLogado;
    });

    const permissaoPadrao = userData 
      ? userData.permissao
      : this.permissoesDisponiveis.reduce((a, b) => NIVEL_PERMISSAO[a] > NIVEL_PERMISSAO[b] ? a : b);

    if (this.cadastroForm.get('permissao')?.enabled) {
      this.cadastroForm.get('permissao')?.setValue(permissaoPadrao);
    }
  }


  onSubmit(): void {
    this.erroCadastro = null;

    if (this.cadastroForm.invalid) {
      this.cadastroForm.markAllAsTouched(); 
      return;
    }

    this.loading = true;
    const formData = this.cadastroForm.getRawValue(); 

    if (this.isEditing) {
        this.handleUpdateFlow(formData);
    } else {
        this.cadastrarUsuario(formData);
    }
  }

  handleUpdateFlow(formData: any): void {
    const { login, senha, ...rest } = formData;
    const needsPasswordChange = this.alterarSenha;

    const dadosUsuario: AlterarUsuario = { 
        login, 
        ...rest 
    };

    this.usuarioService.putAlterarUsuario(dadosUsuario).subscribe({
        next: () => {
            if (needsPasswordChange) {
                this.updatePassword(login, senha, formData);
            } else {
                this.loading = false;
                this.fecharModal();
                this.usuarioAlterado.emit(formData); 
            }
        },
        error: (err) => {
            this.loading = false;
            this.erroCadastro = err.error?.message || 'Erro ao alterar dados básicos. Tente novamente.';
        }
    });
  }


  updatePassword(login: string, senha: string, originalFormData: any): void {
    const dadosSenha = {
      login: login, 
      novaSenha: senha
    };

    this.usuarioService.putAlterarSenhaUsuario(dadosSenha).subscribe({
        next: () => {
            this.loading = false;
            this.fecharModal();
            this.usuarioAlterado.emit(originalFormData);
        },
        error: (err) => {
            this.loading = false;
            this.erroCadastro = err.error?.message || 'Dados básicos alterados, mas a senha falhou ao ser atualizada. Tente novamente ou contate o suporte.';
        }
    });
  }

  cadastrarUsuario(formData: any): void {
    this.usuarioService.postCadastraUsuari(formData).subscribe({
      next: () => {
        this.loading = false;
        this.fecharModal();
        this.usuarioCadastrado.emit();
      },
      error: (err) => {
        this.loading = false;
        this.erroCadastro = typeof err.error === 'string' 
        ? err.error 
        : 'Erro ao cadastrar usuário. Tente novamente.';
      }
    });
  }

  abrirBootstrapModal(elementId: string): void {
    this.modalRef = new bootstrap.Modal(document.getElementById(elementId), {});
    this.modalRef.show();
  }

  fecharModal(): void {
    this.userInModal = null; 
    this.alterarSenha = false;
    this.modalRef?.hide();
    this.modalFechado.emit();
  }
}
