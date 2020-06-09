drop function Pizaria.showOrderClientHistory;
go

create function Pizaria.showOrderClientHistory (@email nvarchar(255)) returns table
as
	return (
		select ID, contato, nome, endereco_fisico, est_email as estafeta_email, hora
		from Pizaria.EncomendaEntregue join Pizaria.Utilizador on est_email=email
		where cli_email=@email
	)
go

select * from Pizaria.Encomenda where cliente_email='cliente@gmail.com'

select * from Pizaria.Estafeta where email='imperdiet@tortor.org'
select * from Pizaria.showEstafeta('estafeta@gmail.com')
print Pizaria.findBestEstafeta()