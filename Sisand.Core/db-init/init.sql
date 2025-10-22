IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'Sisand')
BEGIN
    CREATE DATABASE Sisand;
END
GO

USE Sisand;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuario' AND type = 'U')
BEGIN
    CREATE TABLE Usuario (
        UsuarioId INT PRIMARY KEY IDENTITY(1,1),
        Login varchar (50) NOT NULL UNIQUE,
        Senha varchar (250) NOT NULL,
        Email varchar (150) NOT NULL,
        NomeCompleto varchar (150) NOT NULL,
        Telefone varchar (15) NOT NULL,
        Permissao varchar (25) NOT NULL,
        CriadoEm DATETIME NOT NULL DEFAULT GETDATE(),
        AlteradoEm DATETIME NOT NULL DEFAULT GETDATE()
    );
END
GO

SET IDENTITY_INSERT Usuario ON;
GO

IF NOT EXISTS (SELECT * FROM Usuario WHERE UsuarioId = 1)
BEGIN
    INSERT INTO Usuario (UsuarioId, Login, Senha, Email, NomeCompleto, Telefone, Permissao, CriadoEm, AlteradoEm)
    VALUES
    (1, 'SisandAdmin', '$2a$11$lodpntljzvBHMxhEHOJr4uLE3QLRA6T49PzlmAAGag5EDK..osF5W', 'SisandAdmin@sisand.com.br', 'Sisand Administrador', '41912345678','admin', GETDATE(), GETDATE());
END
GO

IF NOT EXISTS (SELECT * FROM Usuario WHERE UsuarioId = 2)
BEGIN
    INSERT INTO Usuario (UsuarioId, Login, Senha, Email, NomeCompleto, Telefone, Permissao, CriadoEm, AlteradoEm)
    VALUES
    (2, 'SisandUsuario', '$2a$11$J.ndlDaXRFD3u17vuoLHWeKopi65hA.eTJq5oEb6h6ykos.vUu4Iy', 'SisandUsuario@sisand.com.br', 'Sisand Usuário', '41987654321','usuario', GETDATE(), GETDATE());
END
GO

IF NOT EXISTS (SELECT * FROM Usuario WHERE UsuarioId = 3)
BEGIN
    INSERT INTO Usuario (UsuarioId, Login, Senha, Email, NomeCompleto, Telefone, Permissao, CriadoEm, AlteradoEm)
    VALUES
    (3, 'SisandVisitante', '$2a$11$yvnIw.2dy/GWhUXS6Sa7N.8p6pTGKPDDE9.KRd56vLOy3z2GqNGta', 'SisandVisitante@sisand.com.br', 'Sisand Visitante', '41921346587','visitante', GETDATE(), GETDATE());
END
GO

SET IDENTITY_INSERT Usuario OFF;
GO