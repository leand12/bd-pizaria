drop trigger Pizaria.delEstafeta;
go

go
create trigger Pizaria.delEstafeta on Pizaria.Estafeta
instead of delete
as
	begin
		set nocount on;
		declare @email as nvarchar(255);

		select @email = email from deleted;

		begin try
			if (exists(select top 1 email from Pizaria.Estafeta join Pizaria.Encomenda on estafeta_email=email where email=@email))
			begin
				update Pizaria.Estafeta set res_contato = null where email=@email
				update Pizaria.Encomenda set estafeta_email = (select Pizaria.FindBestEstafeta()) where estafeta_email=@email
				return;
			end
			else
				update Pizaria.Estafeta set res_contato = null where email=@email
		end try
		begin catch
			raiserror('Error',16,1);
			return;
		end catch
	end
go