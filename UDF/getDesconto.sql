drop function Pizaria.getDesconto;
go

go
create function Pizaria.getDesconto (@ID int) returns decimal(9,2)
as
    begin
		declare @val decimal(9,2)

		if ((select des_codigo from Pizaria.EncomendaEntregue where ID=@ID) is not null)
			set @val =(select 1.00-percentagem/100.00 from Pizaria.EncomendaEntregue join Pizaria.Desconto on des_codigo=codigo where ID=@ID)
		else if ((select des_codigo from Pizaria.Encomenda where ID=@ID) is not null)
			set @val =(select 1.00-percentagem/100.00 from Pizaria.Encomenda join Pizaria.Desconto on des_codigo=codigo where ID=@ID)
		else
			set @val = 1.00;
		return @val
    end
go