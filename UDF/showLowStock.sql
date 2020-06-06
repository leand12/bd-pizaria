drop function Pizaria.showLowStock;
go

go
create function Pizaria.showLowStock () returns @temp table(ID int, nome varchar(50), preco decimal(19,2), quantidade_disponivel int)
as
	begin
		insert into @temp
			select Item.ID, nome, preco, quantidade_disponivel
			from  Pizaria.Item join Pizaria.Ingrediente on Item.ID=Ingrediente.ID
			where Ingrediente.quantidade_disponivel <= 50
			order by 4 asc
		insert into @temp
			select Item.ID, nome, preco, quantidade_disponivel
			from  Pizaria.Item join Pizaria.Bebida on Bebida.ID=Item.ID
			where Bebida.quantidade_disponivel <= 50
			order by 4 asc
		return
	end
go