drop function Pizaria.showEncEntregue;
go

go
create function Pizaria.showEncEntregue (@ID int) returns Table
as
	return (select nome, preco, quantidade
			from Pizaria.EncEntregueItem join Pizaria.Item on item_ID=ID where enc_ID=@ID
			)
go
