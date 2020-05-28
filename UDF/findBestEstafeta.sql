drop function Pizaria.FindBestEstafeta;
go

go
create function Pizaria.FindBestEstafeta () returns nvarchar(255) 
as
	begin
		declare @email nvarchar(255)
        select top 1 @email=estafeta_email
        from Pizaria.Encomenda
        group by estafeta_email
        HAVING COUNT (estafeta_email)= (select MIN(num_enc)
        from (select count(estafeta_email) as num_enc from Pizaria.Encomenda
        group by estafeta_email) as EstafetaNumEncomendas)
		return @email
    end
go


