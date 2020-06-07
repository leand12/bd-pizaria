drop function Pizaria.statsEncomenda;
go

go
create function Pizaria.statsEncomenda (
	@item_type		varchar(30),
	@order			int
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
						select nome, preco, P.ID, count(P.ID) as num_vendas
						from Pizaria.EncomendaItem join (select * from Pizaria.PizaAvail()) as P on EncomendaItem.item_ID=P.ID
						group by nome, preco, P.ID
					) as qq order by
						case when @order = 0 then num_vendas end desc,
						case when @order = 1 then num_vendas end asc
					RETURN;
			end
		else if (@item_type = 'Menu')
			begin				
				INSERT INTO @returnTable
					select top 3 * from (
						select nome, preco, M.ID, count(M.ID) as num_vendas
						from Pizaria.EncomendaItem join (select * from Pizaria.menuAvail()) as M on EncomendaItem.item_ID=M.ID
						group by nome, preco, M.ID
					) as qq order by
						case when @order = 0 then num_vendas end desc,
						case when @order = 1 then num_vendas end asc
					RETURN;
			end
		RETURN;
	end
go