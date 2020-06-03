drop function Pizaria.showEstafeta;
go

go
create function Pizaria.showEstafeta (@email nvarchar(255)) returns Table
as
	return (
		select E.email, nome, 
		(select count(*) as count_enc from Pizaria.Estafeta join Pizaria.EncomendaEntregue on email=est_email where email=E.email) as delivered,
		(select count(*) as count_enc from Pizaria.Estafeta join Pizaria.Encomenda on email=estafeta_email where email=E.email) as to_deliver
		from Pizaria.Estafeta as E join Pizaria.Utilizador on E.email=Utilizador.email
	)
go
