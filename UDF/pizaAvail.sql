drop function Pizaria.pizaAvail
go
go
create function Pizaria.pizaAvail()  returns table
as
	return (select nome, preco, Piza.ID, pic from (((Pizaria.Item join Pizaria.Piza on Item.ID=Piza.ID)
	join Pizaria.PizaIngrediente on Piza.Id=piz_ID) join Pizaria.Ingrediente on Ingrediente.ID=ing_ID)
	where quantidade_disponivel>=quantidade
	group by nome, preco, Piza.ID, pic)
go