drop function Pizaria.showOrdersToDel;
go

go
create function Pizaria.showOrdersToDel (@estafeta_email nvarchar(255)) returns Table
as
	return (select ID,endereco_fisico, hora, contato, nome, cliente_email  from Pizaria.Encomenda join Pizaria.Utilizador on cliente_email=email where estafeta_email=@estafeta_email)
go