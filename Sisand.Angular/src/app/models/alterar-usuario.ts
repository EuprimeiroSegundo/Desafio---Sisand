import { Usuario } from "./usuario";

export interface AlterarUsuario extends Usuario {
    senha: string; 
}