drop function Pizaria.showOrderClient;
go

go
create function Pizaria.showOrderClient (@ID int) returns Table
as
	return (
	select item_ID, quantidade, nome from Pizaria.EncEntregueItem join Pizaria.Item on item_ID=ID where enc_ID=@ID
	)
go