drop function Pizaria.showRestaurante;
go

go
create function Pizaria.showRestaurante () returns Table
as
	return (
		select nome, contato, morada, lotacao, hora_abertura, hora_fecho from Pizaria.Restaurante
	)
go