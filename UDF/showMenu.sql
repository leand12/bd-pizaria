drop function Pizaria.showMenu;
go

go
create function Pizaria.showMenu (@ID int) returns Table
as
	return (select nome, preco, quantidade
			from  Pizaria.Menu join Pizaria.MenuProduto on Menu.ID=men_ID join Pizaria.Produto on pro_ID=Produto.ID join Pizaria.Item on Produto.ID=Item.ID 
			where Menu.ID=@ID
			)
go