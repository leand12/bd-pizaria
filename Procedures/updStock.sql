drop procedure Pizaria.updStock;
go

go
create procedure Pizaria.updStock
	@ID int,
	@amount int
as
	begin
		set nocount on
		if (exists(select top 1 ID from Pizaria.Ingrediente where ID = @ID))
			update Pizaria.Ingrediente set quantidade_disponivel += @amount where ID = @ID
		else if (exists(select top 1 ID from Pizaria.Bebida where ID = @ID))
			update Pizaria.Bebida set quantidade_disponivel += @amount where ID = @ID
	end
go