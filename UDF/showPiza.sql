drop function Pizaria.showPiza;
go

go
create function Pizaria.showPiza (@ID int) returns Table
as
	return (select nome, preco, quantidade, pic
			from Pizaria.Piza join Pizaria.PizaIngrediente on Piza.ID=piz_ID join Pizaria.IngredienteView on ing_ID=IngredienteView.ID
			where Piza.ID=@ID
			)
go