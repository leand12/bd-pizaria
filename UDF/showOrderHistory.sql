drop function Pizaria.showOrderHistory;
go


create function Pizaria.showOrderHistory (@email nvarchar(255)) returns @temp Table(ID int, cli_email nvarchar(255),
endereco_fisico varchar(50), hora datetime,metodo_pagamento	varchar(30))
as
	begin
		insert into @temp select ID,cli_email,endereco_fisico,hora,metodo_pagamento from Pizaria.EncomendaEntregue where cli_email=@email
		insert into @temp select ID,cli_email,endereco_fisico,hora,metodo_pagamento from Pizaria.EncomendaEntregue where est_email=@email
		return
	end
go