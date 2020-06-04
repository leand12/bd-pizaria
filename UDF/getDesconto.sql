drop function Pizaria.getDesconto;
go

go
create function Pizaria.getDesconto (@ID int) returns decimal(9,2)
as
    begin
		declare @val decimal(9,2)

		if ((select des_codigo from Pizaria.EncomendaEntregue where ID=@ID) is null)
			set @val = 1.00;
		else
			set @val =(select 1.00-percentagem/100.00 from Pizaria.EncomendaEntregue join Pizaria.Desconto on des_codigo=codigo where ID=@ID)
		return @val
    end
go