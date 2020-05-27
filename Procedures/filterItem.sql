drop procedure Pizaria.filterItem;
go

go
create procedure Pizaria.filterItem 
	@price			DECIMAL(19,4),
	@name			varchar(30),
	@item_type		varchar(30)
as
	begin
		if (@item_type = 'Piza')
			begin
					select nome, preco, Piza.ID as ID
					from Pizaria.Piza join Pizaria.Item on Piza.ID=Item.ID
					where preco<=@price and nome like @name+'%'

			end

		else if (@item_type = 'Bebida')
			begin
				
					select nome, preco, quantidade_disponivel
					from  Pizaria.Bebida join Pizaria.Item on Item.ID=Bebida.ID
					where preco<=@price and nome like @name+'%'
					order by preco
			end

		else if (@item_type = 'Menu')
			begin
					
						select nome, preco, Menu.ID as ID
					from  Pizaria.Menu join Pizaria.Item on Menu.ID=Item.ID 
					where preco<=@price and nome like @name+'%'
					order by preco
			end

		else if (@item_type = 'Ingredientes')
			begin
				
					select nome, preco, quantidade_disponivel
					from  Pizaria.Ingrediente join Pizaria.Item on Ingrediente.ID=Item.ID 
					where preco<=@price and nome like @name+'%'
					order by preco
			end
	end
go


--select * from Pizaria.filterItem(7, DEFAULT, 'Piza') order by 1 desc -- 2 ordem ascendente

-- trigger ou qql merda pa verificar quantidades disponiveis de ing e bebidas da encomenda
-- admin no futuro pode dar restock se tivermos tempo
-- 

