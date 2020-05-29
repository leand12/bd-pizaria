drop function Pizaria.showAllOrders;
go

go
create function Pizaria.showAllOrders (@cliente_email nvarchar(255)) returns Table
as
	return (select ID, contato, nome, endereco_fisico, hora from Pizaria.Encomenda join Pizaria.Utilizador on estafeta_email=email where cliente_email=@cliente_email)
go