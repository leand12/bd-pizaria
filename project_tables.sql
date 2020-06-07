--create schema Pizaria
drop table Pizaria.PizaIngrediente;
drop table Pizaria.EncomendaItem;
drop table Pizaria.EncEntregueItem;
drop table Pizaria.MenuProduto;
drop table Pizaria.Menu;
drop table Pizaria.Bebida;
drop table Pizaria.Piza;
drop table Pizaria.Ingrediente;
drop table Pizaria.Produto;
drop table Pizaria.Item;
drop table Pizaria.Encomenda;
drop table Pizaria.EncomendaEntregue;
drop table Pizaria.DescontoCliente;
drop table Pizaria.Desconto;
drop table Pizaria.Cliente;
drop table Pizaria.Estafeta;
drop table Pizaria.Restaurante;
drop table Pizaria.Administrador;
drop table Pizaria.Utilizador;


create table Pizaria.Utilizador (
	email			nvarchar(255),
	nome			varchar(50)		not null,
	contato			int				not null,
	salt UNIQUEIDENTIFIER			not null,
	pass			binary(64)		not null,
	primary key (email)
);
create table Pizaria.Administrador (
	email			nvarchar(255),
	primary key (email),
	foreign key (email) references Pizaria.Utilizador(email)
);
create table Pizaria.Restaurante (
	contato					int,
	nome					varchar(50)	not null,
	morada					varchar(50)	not null,
	lotacao					int			not null,
	hora_abertura			time		not null,
	hora_fecho				time		not null,
	dono					nvarchar(255)	not null,
	primary key (contato),
	foreign key (dono) references Pizaria.Administrador(email)
);
create table Pizaria.Estafeta (
	email			nvarchar(255),
	res_contato		int,
	primary key (email),
	foreign key (email) references Pizaria.Utilizador(email),
	foreign key (res_contato) references Pizaria.Restaurante(contato)
);
create table Pizaria.Cliente (
	email			nvarchar(255),
	idade			int,
	genero			char,
	morada			nvarchar(255),
	primary key (email),
	foreign key (email) references Pizaria.Utilizador(email)
);
create table Pizaria.Desconto(
	codigo			int,
	percentagem		int			not null,
	inicio			date		not null,
	fim				date		not null,
	primary key (codigo)
);
create table Pizaria.DescontoCliente (
	cli_email		nvarchar(255)		not null,
	des_codigo		int				not null,
	primary key (cli_email, des_codigo),
	foreign key (cli_email) references Pizaria.Cliente(email),
	foreign key (des_codigo) references Pizaria.Desconto(codigo)
);
create table Pizaria.Encomenda(
	ID					int				identity (1200,1),
	cliente_email		nvarchar(255)	not null,
	estafeta_email		nvarchar(255)	not null,
	endereco_fisico		varchar(50)		not null,
	hora				datetime		not null,
	metodo_pagamento	varchar(30)		not null,
	des_codigo			int,
	primary key(ID),
	foreign key(estafeta_email) references Pizaria.Estafeta(email),
	foreign key(des_codigo) references Pizaria.Desconto(codigo),
	foreign key(cliente_email) references Pizaria.Cliente(email)
);
create table Pizaria.EncomendaEntregue (
	ID					int				not null,
	cli_email			nvarchar(255)	not null,
	est_email			nvarchar(255)	not null,
	endereco_fisico		varchar(50)		not null,
	hora				datetime		not null,
	metodo_pagamento	varchar(30)		not null,
	restaurante			int				not null,
	des_codigo			int,
	primary key (ID),
	foreign key (cli_email) references Pizaria.Cliente(email),
	foreign key (restaurante) references Pizaria.Restaurante(contato),
	foreign key (est_email) references Pizaria.Estafeta(email),
	foreign key (des_codigo) references Pizaria.Desconto(codigo),
);
create table Pizaria.Item(
	ID			int,
	nome		varchar(30)		not null,
	preco		DECIMAL(19,2)			not null,
	primary key(ID)
);
create table Pizaria.EncomendaItem(
	enc_ID			int		not null,
	item_ID			int		not null,
	quantidade		int		not null,
	primary key(enc_ID,item_ID),
	foreign key(enc_ID) references Pizaria.Encomenda(ID),
	foreign key(item_ID) references Pizaria.Item(ID)
);
create table Pizaria.EncEntregueItem(
	enc_ID			int		not null,
	item_ID			int		not null,
	quantidade		int		not null,
	primary key(enc_ID,item_ID),
	foreign key(enc_ID) references Pizaria.EncomendaEntregue(ID),
	foreign key(item_ID) references Pizaria.Item(ID)
);
create table Pizaria.Produto(
	ID					int,
	primary key(ID),
	foreign key(ID) references Pizaria.Item(ID)
);
create table Pizaria.Bebida(
	ID						int,
	quantidade_disponivel	int		not null,
	primary key(ID),
	foreign key(ID) references Pizaria.Produto(ID)
);
create table Pizaria.Piza(
	ID					int,
	pic					NVARCHAR(MAX)	not null, 
	primary key(ID),
	foreign key(ID) references Pizaria.Produto(ID)
);
create table Pizaria.Ingrediente(
	ID						int,
	quantidade_disponivel	int		not null,
	primary key(ID),
	foreign key(ID) references Pizaria.Produto(ID)
);
create table Pizaria.PizaIngrediente(
	piz_ID		int,
	ing_ID		int,
	quantidade	int		not null,
	primary key(piz_ID,ing_ID),
	foreign key(piz_ID) references Pizaria.Piza(ID),
	foreign key(ing_ID) references Pizaria.Ingrediente(ID)
);
create table Pizaria.Menu(
	ID					int,
	primary key(ID),
	foreign key(ID) references Pizaria.Item(ID)
);
create table Pizaria.MenuProduto(
	men_ID		int,
	pro_ID		int,
	quantidade	int		not null,
	primary key(men_ID,pro_ID),
	foreign key(men_ID) references Pizaria.Menu(ID),
	foreign key(pro_ID) references Pizaria.Produto(ID)
);