drop function Pizaria.showEstafeta;
go

go
create function Pizaria.showEstafeta () returns Table
as
	return (
		select nome, contato, morada, lotacao, hora_abertura, hora_fecho from Pizaria.Restaurante
	)
go
