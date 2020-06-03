drop function Pizaria.CountEncEntregues;
go

go
create function Pizaria.CountEncEntregues (@email nvarchar(255)) returns Table
as
	return (select count(*) as count_enc from Pizaria.Estafeta join Pizaria.EncomendaEntregue on email=est_email where email=@email)
go

