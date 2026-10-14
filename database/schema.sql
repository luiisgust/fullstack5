-- MySQL 8.0.16+; execute uma vez em um banco novo.
CREATE DATABASE IF NOT EXISTS catalogo_filmes_exemplo CHARACTER SET utf8mb4;
USE catalogo_filmes_exemplo;
CREATE TABLE generos (Id INT AUTO_INCREMENT PRIMARY KEY, Nome VARCHAR(80) NOT NULL);
CREATE TABLE diretores (Id INT AUTO_INCREMENT PRIMARY KEY, Nome VARCHAR(120) NOT NULL, Nacionalidade VARCHAR(80) NOT NULL);
CREATE TABLE filmes (
 Id INT AUTO_INCREMENT PRIMARY KEY, Titulo VARCHAR(150) NOT NULL,
 AnoLancamento INT NOT NULL, DuracaoMinutos INT NOT NULL, GeneroId INT NOT NULL, DiretorId INT NOT NULL,
 FOREIGN KEY (GeneroId) REFERENCES generos(Id) ON DELETE RESTRICT,
 FOREIGN KEY (DiretorId) REFERENCES diretores(Id) ON DELETE RESTRICT,
 CHECK (DuracaoMinutos > 0)
);
CREATE TABLE avaliacoes (
 Id INT AUTO_INCREMENT PRIMARY KEY, FilmeId INT NOT NULL, Nota DECIMAL(3,1) NOT NULL,
 Comentario VARCHAR(500) NOT NULL, DataAvaliacao DATETIME(6) NOT NULL,
 FOREIGN KEY (FilmeId) REFERENCES filmes(Id) ON DELETE RESTRICT, CHECK (Nota >= 0 AND Nota <= 10)
);
INSERT INTO generos (Id,Nome) VALUES (1,'Ficção científica'),(2,'Animação');
INSERT INTO diretores (Id,Nome,Nacionalidade) VALUES (1,'Christopher Nolan','Britânica'),(2,'Hayao Miyazaki','Japonesa');
INSERT INTO filmes (Id,Titulo,AnoLancamento,DuracaoMinutos,GeneroId,DiretorId) VALUES
 (1,'Interestelar',2014,169,1,1),(2,'A Viagem de Chihiro',2001,125,2,2);
INSERT INTO avaliacoes (FilmeId,Nota,Comentario,DataAvaliacao) VALUES (1,9.0,'Exemplo para estudo.',NOW());
SELECT * FROM filmes;
