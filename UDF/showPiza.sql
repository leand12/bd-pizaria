drop function Pizaria.showPiza;
go

go
create function Pizaria.showPiza (@ID int) returns Table
as
	return (select nome, preco, quantidade
			from Pizaria.Piza join Pizaria.PizaIngrediente on Piza.ID=piz_ID join Pizaria.Ingrediente on ing_ID=Ingrediente.ID join Pizaria.Item on Ingrediente.ID=Item.ID
			where Piza.ID=@ID
			)
go