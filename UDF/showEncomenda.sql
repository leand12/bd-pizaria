drop function Pizaria.showEncomenda;
go

go
create function Pizaria.showEncomenda (@ID int) returns Table
as
	return (select nome, preco, quantidade
			from Pizaria.EncomendaItem join Pizaria.Item on item_ID=ID where enc_ID=@ID
			)
go
