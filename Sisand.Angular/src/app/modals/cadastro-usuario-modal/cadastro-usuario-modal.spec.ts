import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CadastroUsuarioModal } from './cadastro-usuario-modal';

describe('CadastroUsuarioModal', () => {
  let component: CadastroUsuarioModal;
  let fixture: ComponentFixture<CadastroUsuarioModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CadastroUsuarioModal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CadastroUsuarioModal);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
