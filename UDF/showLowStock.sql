drop function Pizaria.showLowStock;
go

go
create function Pizaria.showLowStock () returns Table
as
	return (
			select Item.ID, nome, preco, Ingrediente.quantidade_disponivel as stock_ing, Bebida.quantidade_disponivel as stock_beb
			from  Pizaria.Item left join Pizaria.Ingrediente on Item.ID=Ingrediente.ID
			left join Pizaria.Bebida on Bebida.ID=Item.ID
			where Ingrediente.quantidade_disponivel <= 50
			or Bebida.quantidade_disponivel <= 50
			)
go

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
			update Pizaria.Ingrediente set quantidade_disponivel += @amount
		else if (exists(select top 1 ID from Pizaria.Bebida where ID = @ID))
			update Pizaria.Bebida set quantidade_disponivel += @amount
	end
go