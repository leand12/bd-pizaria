drop procedure Pizaria.insEncomenda
go

go

create procedure Pizaria.insEncomenda
	@cliente_email			nvarchar(255),
	@estafeta_email			nvarchar(255),
	@endereco_fisico		varchar(50),
	@hora					datetime,		
	@metodo_pagamento		varchar(30),
	@des_codigo				int,
	@last_ID				int		output
as
	begin
		begin try
			insert into Pizaria.Encomenda (cliente_email,estafeta_email,endereco_fisico,hora,metodo_pagamento,des_codigo) values
			(@cliente_email, @estafeta_email, @endereco_fisico, @hora, @metodo_pagamento, @des_codigo)

			SET @last_ID = SCOPE_IDENTITY()
		end try
		begin catch
			RAISERROR('Error Inserting Order',16,1)
			return
		end catch
	end
go