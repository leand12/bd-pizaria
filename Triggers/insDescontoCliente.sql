drop trigger Pizaria.insDescontoCliente;
go

go
create trigger Pizaria.insDescontoCliente on Pizaria.DescontoCliente
instead of insert
as
	begin
		set nocount on;
		declare @cliente_email	as nvarchar(255);
		declare @codigo			as int;

		select @cliente_email = cli_email, @codigo = des_codigo from inserted;

		if @codigo is null or (exists(select des_codigo from Pizaria.DescontoCliente where cli_email=@cliente_email and des_codigo=@codigo))
			return;

		begin try
			insert into Pizaria.DescontoCliente (cli_email,des_codigo) Values (@cliente_email,@codigo)
		end try
		begin catch
			raiserror('Error',16,1);
			return;
		end catch
	end
go