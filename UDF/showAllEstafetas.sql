drop function Pizaria.showAllEstafetas;
go

go
create function Pizaria.showAllEstafetas () returns Table
as
	return (
	select R.nome as restaurante, [Utilizador].nome,E.email,
		(select count(*) as count_enc from Pizaria.Estafeta join Pizaria.EncomendaEntregue on email=est_email where email=E.email) as delivered,
		(select count(*) as count_enc from Pizaria.Estafeta join Pizaria.Encomenda on email=estafeta_email where email=E.email) as to_deliver
		from Pizaria.Estafeta as E join Pizaria.[Utilizador] on E.email=Utilizador.email join Pizaria.Restaurante as R on res_contato=R.contato
	)
go