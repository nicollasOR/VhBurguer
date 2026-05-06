CREATE DATABASE VH_Burguer
GO

--USE VH_Burguer

GO

CREATE TABLE Usuario
(
UsuarioId INT PRIMARY KEY IDENTITY,
Nome VARCHAR(60) NOT NULL, 
Email VARCHAR(150) UNIQUE NOT NULL,
Senha VARBINARY(32) NOT NULL,
StatusUsuario BIT DEFAULT 1
);

GO

CREATE TABLE Produto
(
ProdutoId INT PRIMARY KEY IDENTITY, 
Nome VARCHAR(100) UNIQUE NOT NULL,
Descricao NVARCHAR(MAX) NOT NULL,
Imagem VARBINARY(MAX) NOT NULL,
Preco DECIMAL (10,2) NOT NULL,
StatusProduto BIT DEFAULT 1
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
 PrecoAnterior DECIMAL(10,2),
 NomeAnterior VARCHAR(100),
 ProdutoId INT FOREIGN KEY REFERENCES Produto(ProdutoId)
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
)




CREATE TABLE ProdutoPromocao
(
ProdutoId INT NOT NULL,
PromocaoId INT NOT NULL,
PrecoAtual DECIMAL(10,2) NOT NULL,

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

			Create trigger trg_AlteracaoProduto
			ON Produto
			AFTER UPDATE
			AS BEGIN
			INSERT INTO Log_AlteracaoProduto(DataAlteracao, ProdutoId,
											NomeAnterior, PrecoAnterior) 
			SELECT GETDATE(), ProdutoId, Nome, Preco FROM deleted
			END;
			GO




			INSERT INTO Usuario (Nome, Email, Senha)
VALUES
('Carlos Lima', 'carlos@vhburguer.com', HASHBYTES('SHA2_256', '123456')),
('Ana Souza', 'ana@vhburguer.com', HASHBYTES('SHA2_256', 'senha123')),
('João Pedro', 'joao@vhburguer.com', HASHBYTES('SHA2_256', 'abc123'));
GO


INSERT INTO Categoria (Nome)
VALUES
('Tradicional'),
('Vegetariano'),
('Vegano'),
('Bebidas'),
('Sobremesas');
GO


INSERT INTO Produto (Nome, Descricao, Imagem, Preco)
VALUES
('VH Classic Burger', 'Hambúrguer com pão brioche, carne e cheddar.', CONVERT(VARBINARY(MAX), 'img1'), 29.90),
('VH Bacon Burger', 'Hambúrguer com bacon crocante e molho especial.', CONVERT(VARBINARY(MAX), 'img2'), 34.90),
('Batata Rústica', 'Batatas temperadas com ervas.', CONVERT(VARBINARY(MAX), 'img3'), 14.90),
('Hambúrguer Vegano', 'Hambúrguer 100% vegetal.', CONVERT(VARBINARY(MAX), 'img4'), 32.90),
('Refrigerante Lata', 'Bebida gelada 350ml.', CONVERT(VARBINARY(MAX), 'img5'), 6.00);
GO


INSERT INTO ProdutoCategoria (ProdutoId, CategoriaId)
VALUES
(1, 1),
(2, 1),
(3, 1),
(4, 3),
(5, 4);
GO


INSERT INTO Promocao (Nome, DataExpiracao)
VALUES
('Promo Semana', '2026-06-30 23:59:59'),
('Happy Hour', '2026-05-20 23:59:59');
GO


INSERT INTO ProdutoPromocao (ProdutoId, PromocaoId, PrecoAtual)
VALUES
(1, 1, 24.90),
(2, 1, 29.90),
(3, 2, 9.90),
(5, 2, 4.50);
GO