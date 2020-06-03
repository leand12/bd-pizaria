drop function Pizaria.FindBestEstafeta;
go

go
create function Pizaria.FindBestEstafeta() returns nvarchar(255) 
as
	begin
		declare @email nvarchar(255)

		set @email = (
			select top 1 email from Pizaria.Encomenda join Pizaria.Estafeta on estafeta_email=email
			group by email, res_contato
			HAVING res_contato is not null and COUNT (estafeta_email) = (
				select MIN(num_enc) from (
					select count(estafeta_email) as num_enc from Pizaria.Encomenda join Pizaria.Estafeta on estafeta_email=email
					group by estafeta_email, res_contato
					HAVING res_contato is not null
				) as EstafetaNumEncomendas
			)
		)
		return @email
    end
go