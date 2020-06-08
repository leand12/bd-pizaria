drop function Pizaria.FindBestEstafeta;
go

go
create function Pizaria.FindBestEstafeta() returns nvarchar(255) 
as
	begin
		declare @email nvarchar(255);
		declare @count  int;
		select top 1 @email=estafeta_email, @count = count(estafeta_email)
		from Pizaria.Encomenda join Pizaria.Estafeta on estafeta_email=email
		group by estafeta_email, res_contato
		order by 2 asc
		return @email
    end
go

delete from Pizaria.Encomenda where ID=1100