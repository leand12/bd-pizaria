drop function Pizaria.showOrderHistory;
go

create function Pizaria.showOrderHistory (@email nvarchar(255)) returns table
as
	return(
	select ID,cli_email,endereco_fisico,hora,metodo_pagamento from Pizaria.EncomendaEntregue where est_email=@email
	)
go