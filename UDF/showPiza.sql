drop function Pizaria.showPiza;
go

go
create function Pizaria.showPiza (@ID int) returns Table
as
	return (select nome, preco, quantidade
			from Pizaria.Piza join Pizaria.PizaIngrediente on Piza.ID=piz_ID
			join (select * from Pizaria.ingredienteAvail()) as I on ing_ID=I.ID
			where Piza.ID=@ID
			)
go