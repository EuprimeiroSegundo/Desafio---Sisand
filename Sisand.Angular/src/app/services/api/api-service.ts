import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, ObservedValuesFromArray } from 'rxjs';
import { Usuario } from '../../models/usuario';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private http = inject(HttpClient);

  private readonly API_URL = 'http://localhost:8080/api/Usuario'; 

  getBuscarUsuarioPorLoginEmail(loginEmail: any): Observable<Usuario> {
    return this.http.get<Usuario>(`${this.API_URL}/BuscarUsuarioPorLoginEmail/${loginEmail}`)
  }

  getListarUsuarios(): Observable<Usuario[]> {
    return this.http.get<Usuario[]>(`${this.API_URL}/ListarUsuarios`)
  }

  postCadastraUsuari(cadastroUsuario: any): Observable<any> {
    return this.http.post(`${this.API_URL}/CadastrarUsuario`, cadastroUsuario)
  }

  putAlterarUsuario(alterarUsuario: any): Observable<any> {
    return this.http.put(`${this.API_URL}/AtualizarUsuario`, alterarUsuario);
  }

  putAlterarSenhaUsuario(alterarSenhaUsuario: any): Observable<any> {
    return this.http.put(`${this.API_URL}/AlterarSenhaUsuario`, alterarSenhaUsuario);
  }

  deleteDeletarUsuario(loginDeletar: any): Observable<any> {
    return this.http.delete(`${this.API_URL}/DeletarUsuario/${loginDeletar}`)
  }
}
