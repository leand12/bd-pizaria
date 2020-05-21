drop procedure Pizaria.insEncomenda
go

go

create procedure Pizaria.insEncomenda
	@cliente_email			nvarchar(255),
	@estafeta_email			nvarchar(255),
	@endereco_fisico		varchar(50),
	@hora					date,		
	@metodo_pagamento		varchar(30),
	@des_codigo				int
as
	begin
		insert into Pizaria.Encomenda (cliente_email,estafeta_email,endereco_fisico,hora,metodo_pagamento,des_codigo) values
		(@cliente_email, @estafeta_email, @endereco_fisico, @hora, @metodo_pagamento, @des_codigo)
	end
go