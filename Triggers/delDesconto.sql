drop trigger Pizaria.delDesconto;
go

go
create trigger Pizaria.delDesconto on Pizaria.Desconto
instead of delete
as
	begin
		set nocount on;
		declare @codigo			as int;
		declare @percentagem	as int;
		declare @inicio			as datetime;
		declare @fim			as datetime;

		select @codigo = codigo, @percentagem = percentagem, @inicio = inicio, @fim = fim from deleted;
		begin try

			if (exists(select des_codigo from Pizaria.Encomenda where des_codigo=@codigo union select des_codigo from Pizaria.EncomendaEntregue where des_codigo=@codigo))
			begin	
				raiserror('Discount is in Use',16,1);
				return;
			end
		
			delete from Pizaria.DescontoCliente where des_codigo=@codigo

			delete from Pizaria.Desconto where codigo=@codigo
		end try
		begin catch
			raiserror('Error',16,1);
			return;
		end catch
	end
go