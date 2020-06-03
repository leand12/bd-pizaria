drop trigger Pizaria.delEncomenda;
go

go
create trigger Pizaria.delEncomenda on Pizaria.Encomenda
instead of delete
as
	begin
		set nocount on;
		declare @ID					as int;
		declare @cliente_email		as nvarchar(255);
		declare @estafeta_email		as nvarchar(255);
		declare @endereco_fisico	as varchar(50);
		declare @hora				as datetime;
		declare @metodo_pagamento	as varchar(30);	
		declare @des_codigo			as int;

		select @ID = ID, @cliente_email = cliente_email, @estafeta_email = estafeta_email, @endereco_fisico = endereco_fisico, @hora = hora, @metodo_pagamento = metodo_pagamento from deleted;
		

		insert into Pizaria.EncomendaEntregue (ID, cli_email, est_email, endereco_fisico, hora, metodo_pagamento)
			values (@ID, @cliente_email, @estafeta_email, @endereco_fisico, @hora, @metodo_pagamento)

		insert into Pizaria.EncEntregueItem (enc_ID , item_ID, quantidade)
			(select * from Pizaria.EncEntregueItem where enc_ID=@ID)
		
		delete from Pizaria.EncomendaItem where enc_ID=@ID
		
		delete from Pizaria.Encomenda where ID=@ID
	end
go