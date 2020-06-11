drop procedure Pizaria.filterItem;
go

go
create procedure Pizaria.filterItem 
	@price 			DECIMAL(19,4) = 99999,
	@name			varchar(30) = '',
	@item_type		varchar(30)
as
	begin
		if (@item_type = 'Piza')
			begin
					select nome, preco, ID, pic
					from Pizaria.pizaAvail()
					where preco<=@price and nome like '%'+@name+'%'
			end

		else if (@item_type = 'Bebida')
			begin
				
					select nome, preco, quantidade_disponivel
					from  Pizaria.bebidaAvail()
					where preco<=@price and nome like '%'+@name+'%'
					order by preco
			end

		else if (@item_type = 'Menu')
			begin
					select nome, preco, ID
					from  Pizaria.menuAvail()
					where preco<=@price and nome like '%'+@name+'%'
					order by preco
			end

		else if (@item_type = 'Ingredientes')
			begin
				
					select nome, preco, quantidade_disponivel
					from  Pizaria.ingredienteAvail()
					where preco<=@price and nome like '%'+@name+'%'
					order by preco
			end
	end
go

