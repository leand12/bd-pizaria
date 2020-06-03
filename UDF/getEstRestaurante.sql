drop function Pizaria.getEstRestaurante;
go

go
create function Pizaria.getEstRestaurante (@email nvarchar(255)) returns Table
as
	return (
		select nome, contato, morada, lotacao, hora_abertura, hora_fecho, dono from Pizaria.Estafeta join Pizaria.Restaurante on res_contato=contato where email=@email
	)
go

select * from Pizaria.getEstRestaurante('estafeta@gmail.com')