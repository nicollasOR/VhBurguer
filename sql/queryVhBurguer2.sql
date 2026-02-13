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
StatusProduto BIT DEFAULT 1

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
DataExpiracao DATETIME2(0) NOT NULL,
StatusPromocao BIT DEFAULT 1
)

CREATE TABLE Log_AlteracaoProduto
(
 Log_AlteracaoId INT PRIMARY KEY IDENTITY, 
 DataAlteracao DATETIME2(0) NOT NULL,
 --PrecoAnterior DECIMAL(10,2) NOT NULL,
 --NomeAnterior VARCHAR(100) NOT NULL,
 PrecoAnterior DECIMAL(10,2),
 NomeAnterior VARCHAR(100),
 ProdutoId INT FOREIGN KEY REFERENCES Produto(ProdutoId)
);

--criando nossas triggers
	--DELETE -> EXCLUIR O USUARIO == inativar o usuario => StatusUsuario = 0


CREATE TABLE ProdutoCategoria
(
ProdutoId INT NOT NULL,
CategoriaId INT NOT NULL,

-- PRIMARY KEY
CONSTRAINT FK_ProdutoCategoria_PK PRIMARY KEY(ProdutoId, CategoriaId),

-- FOREIGN KEYS
CONSTRAINT ProdutoCategoria_Produto FOREIGN KEY (ProdutoId) REFERENCES Produto(ProdutoId) ON DELETE CASCADE,
CONSTRAINT ProdutoCategoria_Categoria FOREIGN KEY (CategoriaId) REFERENCES Categoria(CategoriaId) ON DELETE CASCADE
)




CREATE TABLE ProdutoPromocao
(
ProdutoId INT NOT NULL,
PromocaoId INT NOT NULL,

CONSTRAINT FK_ProdutoPromocao_PK PRIMARY KEY(ProdutoId, PromocaoId),

CONSTRAINT ProdutoPromocao_Produto_FK FOREIGN KEY(ProdutoId) REFERENCES Produto(ProdutoId) ON DELETE CASCADE,
CONSTRAINT ProdutoPromocao_Promocao_FK FOREIGN KEY(PromocaoId) REFERENCEs Promocao(PromocaoId) ON DELETE CASCADE
)
GO


	create trigger trg_ExcluirProduto
	ON Produto
	INSTEAD OF DELETE 
	AS BEGIN 
	UPDATE prod SET StatusProduto = 0
	FROM Produto prod
	INNER JOIN deleted d on d.ProdutoId = prod.ProdutoId
	END;
	GO

	create trigger trg_ExclusaoUsuario
	ON Usuario
	INSTEAD OF DELETE
	AS BEGIN
			UPDATE usr SET StatusUsuario = 0
			FROM Usuario usr
			INNER JOIN deleted d ON d.UsuarioId = usr.UsuarioId
			END;
			GO
			-- TODA VEZ QUE ALTERARMOS A TABELA PRODUTO ELE VAI PEGAR E GERAR UM REGISTRO NOVO NA TABELA PRODUTO

			Create trigger trg_AlteracaoProduto
			ON Produto
			AFTER UPDATE
			AS BEGIN
			INSERT INTO Log_AlteracaoProduto(DataAlteracao, ProdutoId,
											NomeAnterior, PrecoAnterior) 
			SELECT GETDATE(), ProdutoId, Nome, Preco FROM deleted
			END;
			GO