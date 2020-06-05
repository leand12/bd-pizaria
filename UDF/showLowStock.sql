drop function Pizaria.showLowStock;
go

go
create function Pizaria.showLowStock () returns Table
as
	return (
			select nome, preco, Ingrediente.quantidade_disponivel as stock_ing, Bebida.quantidade_disponivel as stock_beb
			from  Pizaria.Item left join Pizaria.Ingrediente on Item.ID=Ingrediente.ID
			left join Pizaria.Bebida on Bebida.ID=Item.ID
			where Ingrediente.quantidade_disponivel <= 50
			or Bebida.quantidade_disponivel <= 50
			)
go



