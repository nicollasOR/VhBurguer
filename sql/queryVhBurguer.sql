CREATE DATABASE VH_Burguer

GO

--USE VH_Burguer

GO

CREATE TABLE Usuario
(
UsuarioId INT PRIMARY KEY IDENTITY,
Nome VARCHAR(60) NOT NULL, 
Email VARCHAR(150) NOT NULL,
Senha VARBINARY(32) NOT NULL,
StatusUsuario BIT DEFAULT 1
);

GO

CREATE TABLE Produto
(
ProdutoId INT PRIMARY KEY IDENTITY, 
Nome VARCHAR(100) UNIQUE,
Descricao VARCHAR(MAX) NOT NULL,
Imagem VARBINARY(MAX) NOT NULL,
Preco DECIMAL (10,2) NOT NULL,
Status BIT DEFAULT 1

/*
Jeito da Professora com foreign key

UsuarioId INT FOREIGN KEY REFERENCES Usuario(UsuarioId);

*/


)


GO

CREATE TABLE Categoria
(
CategoriaId INT PRIMARY KEY IDENTITY,
Nome VARCHAR(50) NOT NULL

);

CREATE TABLE Promocao
(
PromocaoId INT PRIMARY KEY IDENTITY,
Nome VARCHAR(100) NOT NULL,
DataExpiracao DATETIME NOT NULL,
StatusPromocao BIT DEFAULT 1
)

CREATE TABLE Log_AlteracaoProduto
(
 Log_AlteracaoId INT PRIMARY KEY IDENTITY, 
 DataAlteracao DATETIME NOT NULL,
 PrecoAnterior DECIMAL(10,2) NOT NULL,
 NomeAnterior VARCHAR(100) NOT NULL,
 --ProdutoId INT FOREIGN KEY REFERENCES Produto(ProdutoId)
);

CREATE TABLE ProdutoCategoria
(
ProdutoId INT NOT NULL,
CategoriaId INT NOT NULL,

-- PRIMARY KEY
CONSTRAINT FK_ProdutoCategoria_PK PRIMARY KEY(ProdutoId, CategoriaId),

-- FOREIGN KEYS
CONSTRAINT ProdutoCategoria_Produto FOREIGN KEY (ProdutoId) REFERENCES Produto(ProdutoId) ON DELETE CASCADE,
CONSTRAINT ProdutoCategoria_Categoria FOREIGN KEY (CategoriaId) REFERENCES Categoria(CategoriaId) ON DELETE CASCADE
);

CREATE TABLE ProdutoPromocao
(
ProdutoId INT NOT NULL,
PromocaoId INT NOT NULL,

CONSTRAINT FK_ProdutoPromocao_PK PRIMARY KEY(ProdutoId, PromocaoId),

CONSTRAINT ProdutoPromocao_Produto_FK FOREIGN KEY(ProdutoId) REFERENCES Produto(ProdutoId) ON DELETE CASCADE,
CONSTRAINT ProdutoPromocao_Promocao_FK FOREIGN KEY(PromocaoId) REFERENCEs Promocao(PromocaoId) ON DELETE CASCADE
)