drop procedure Pizaria.statsRestaurante;
go

go
create procedure Pizaria.statsRestaurante
as
	begin
		create table #returnTable (restaurante varchar(55), email nvarchar(255), nome varchar(50), delivered int, total_orders int, total_price int)
		declare @contato int
		
		DECLARE c CURSOR
		FOR select contato from Pizaria.Restaurante;
		OPEN c;
		FETCH NEXT FROM c INTO @contato;
		WHILE @@FETCH_STATUS = 0  
		BEGIN
			insert into #returnTable (restaurante, email, nome, delivered, total_orders, total_price)
			select top 1 R.nome as restaurante, E.email, [Utilizador].nome,
			(
			select count(*) as count_enc from Pizaria.Estafeta join Pizaria.EncomendaEntregue on email=est_email join  Pizaria.Restaurante as R on R.contato=E.res_contato where email=E.email and R.contato = E.res_contato
			) as delivered,
			(
			select count(*) as count_enc from Pizaria.Estafeta join Pizaria.EncomendaEntregue on email=est_email where res_contato = @contato
			) as total_orders,
			(
			select sum(Item.preco*EncEntregueItem.quantidade*Pizaria.getDesconto(EncomendaEntregue.ID)) as total
			from Pizaria.Estafeta join Pizaria.EncomendaEntregue on email=est_email
			join Pizaria.EncEntregueItem on EncomendaEntregue.ID=EncEntregueItem.enc_ID
			join Pizaria.Item on item_ID=Item.ID where restaurante = @contato
			) as total_price
			from Pizaria.Estafeta as E join Pizaria.Utilizador on E.email=Utilizador.email join  Pizaria.Restaurante as R on R.contato=E.res_contato
			where R.contato=@contato
			order by 4 desc
			FETCH NEXT FROM c INTO @contato;
		end
		close c;
		DEALLOCATE c;
		select * from #returnTable
		return
	end
go