drop function Pizaria.showEstafeta;
go

go
create function Pizaria.showEstafeta (@ID int) returns Table
as
	return (select *
			from Pizaria.Restaurante join Pizaria.Estafeta on res_contato=@ID
			)
go