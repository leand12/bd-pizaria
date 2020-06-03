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
			if (exists(select email from Pizaria.Estafeta join Pizaria.Encomenda on estafeta_email=email where email=@email))
			begin
				raiserror('Error',16,1);
				return;
			end
			update Pizaria.Estafeta set res_contato = null where email=@email
		end try
		begin catch
			raiserror('Error',16,1);
			return;
		end catch
	end
go