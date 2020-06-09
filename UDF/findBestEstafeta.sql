drop function Pizaria.FindBestEstafeta;
go

go
create function Pizaria.FindBestEstafeta() returns nvarchar(255) 
as
	begin
		declare @email nvarchar(255);
		declare @count  int;
		select top 1 @email = email, @count = (select count(*)
		from Pizaria.Encomenda join Pizaria.Estafeta on estafeta_email=email
		where estafeta_email = E.email)
		from Pizaria.Estafeta as E
		order by 2 asc
		return @email
    end
go
