drop function Pizaria.statsEncomenda;
go

go
create function Pizaria.statsEncomenda (
	@item_type		varchar(30)
	)	returns @returnTable TABLE 
                     (nome varchar(30) not NULL, 
                      preco decimal(19,2) not NULL,
					  ID	int not null,
					  num_vendas int not null)
as
	begin
		if (@item_type = 'Piza')
			begin
				INSERT INTO @returnTable
					select top 3 * from (
						select nome, preco, PizaView.ID, count(PizaView.ID) as num_vendas
                        from Pizaria.EncomendaItem join Pizaria.PizaView on EncomendaItem.item_ID=PizaView.ID
                        group by nome, preco, PizaView.ID
                    ) as qq order by num_vendas desc
				RETURN;
			end
		else if (@item_type = 'Menu')
			begin
				INSERT INTO @returnTable
					select top 3 * from (
                        select nome, preco, MenuView.ID, count(MenuView.ID) as num_vendas
                        from Pizaria.EncomendaItem join Pizaria.MenuView on EncomendaItem.item_ID=MenuView.ID
                        group by nome, preco, MenuView.ID
                    ) as qq order by num_vendas desc
			end
		RETURN;
	end
go


--select * from Pizaria.statsEncomenda('Piza', 1)


-- trigger ou qql merda pa verificar quantidades disponiveis de ing e bebidas da encomenda
-- admin no futuro pode dar restock se tivermos tempo


