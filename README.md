# ⚒️ Sisand - Desafio de Construção


## 📄 Resumo do Projeto

Este projeto foi proposto durante uma entrevista com a Sisand. Trata-se de uma aplicação de cadastro, alteração e remoção de usuários, com funcionalidades adicionais de níveis de permissão e lógica de liberação de ações, visando maior controle e segurança.

---

## 🚀 Tecnologias

- Angular
- C#
- SQL Server
- Docker

**Status do projeto:** Concluído

---

## 🛠️ O que foi feito

Desde o início, pensei em adicionar funcionalidades que elevassem a complexidade e o valor do projeto, além do que foi solicitado. Foram implementados três níveis de permissão:

### **Administrador**

- **Cadastro:** Pode criar novos usuários com nível de permissão igual ou inferior (admin, usuário, visitante). Botão localizado na parte superior da tabela.
- **Alteração:** Pode editar qualquer usuário, incluindo sua própria conta. Cada linha da tabela possui botões de alterar; a própria conta também pode ser alterada pelo botão superior.
- **Remoção:** Pode remover qualquer usuário, exceto a si próprio. Cada linha possui botão para remoção.

### **Usuário**

- **Cadastro:** Pode criar novos usuários com nível igual ou inferior (usuário, visitante).
- **Alteração:** Pode alterar apenas sua própria conta. Outros aparecem desabilitados.
- **Remoção:** Não possui permissão para remover usuários.

### **Visitante**

- Acesso somente leitura. Pode visualizar usuários, mas não cadastrar, alterar ou remover.

---

## 💻 Como rodar o projeto

### 🔧 Backend

Para subir os containers via Docker Compose, execute na pasta raiz do repositório:

```bash
docker-compose up --build -d
```

Após a inicialização, execute o script SQL localizado em:

```
pasta_raiz_projeto/Sisan.Core/db-init/init.sql
```

Para testes de endpoints, utilize o arquivo Postman:

```
pasta_raiz_projeto/Sisan.Core/Endpoints/Sisand.postman.json
```

---

### 🌐 Frontend

Primeiro, instale as dependências:

```bash
npm install
```

Depois, execute para iniciar a aplicação:

```bash
ng serve
```

A aplicação estará disponível em:

```
http://localhost:4200
```

---

## ⚙️ Como usar a aplicação

Na primeira execução, o sistema apresenta a tela de login. As credenciais iniciais são:

### Administrador

```json
{
  "login": "SisandAdmin",
  "senha": "SisandAdminSenhaImpenetravel"
}
```

### Usuário

```json
{
  "login": "SisandUsuario",
  "senha": "SisandUsuarioSenhaFortissima"
}
```

### Visitante

```json
{
  "login": "SisandVisitante",
  "senha": "SisandVisitanteSenhaImpensavel"
}
```

---

## ⚠️ Alguns detalhes importantes

- Campos de cadastro possuem tamanhos máximos e mínimos por segurança e padronização.
- Senhas são criptografadas com JWT.
- Rotas protegidas por token válido (expiração de 1 hora).
- Validações no frontend garantem segurança adicional.
- Máscaras de telefone e redirecionamentos automáticos reforçam a segurança.

---

## 😔 O que poderia ser melhorado

Tive dificuldades com o Docker, especialmente na carga inicial do banco SQL Server, pois ele não possui ferramentas como o MySQL para rodar scripts automaticamente. Também enfrentei desafios ao tentar colocar o frontend em Docker, devido à configuração do Nginx e containers Node.

---

## ✅ Conclusão

Este projeto foi uma oportunidade de aprendizado, reforçando conhecimentos em Bootstrap, Docker e controle de permissões. Apesar dos obstáculos, o resultado final foi gratificante e espero que atenda às expectativas de quem avaliar.