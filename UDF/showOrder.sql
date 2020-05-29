drop function Pizaria.showOrder;
go

go
create function Pizaria.showOrder (@ID int) returns Table
as
	return (select item_ID, quantidade, nome from Pizaria.EncomendaItem join Pizaria.Item on item_ID=ID where enc_ID=@ID)
go