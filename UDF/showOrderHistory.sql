drop function Pizaria.showOrderHistory;
go


create function Pizaria.showOrderHistory (@type int, @email nvarchar(255)) returns @temp Table(ID int, cli_email nvarchar(255),
endereco_fisico varchar(50), hora datetime,metodo_pagamento	varchar(30))
as
	begin

		if @type = 0
			insert into @temp select ID,cli_email,endereco_fisico,hora,metodo_pagamento from Pizaria.EncomendaEntregue where cli_email=@email
		if (@type = 1)
			insert into @temp select ID,cli_email,endereco_fisico,hora,metodo_pagamento from Pizaria.EncomendaEntregue where est_email=@email
		return
	end
go