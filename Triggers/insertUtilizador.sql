drop trigger Pizaria.insertUtilizador;
go

go
create trigger Pizaria.insertUtilizador on Pizaria.[Utilizador]
instead of insert
as
	begin
		set nocount on;

		declare @email as nvarchar(255);
		declare @nome as varchar(50);
		declare @contato as int;
		declare @pass as varchar(30);
		select @email = email, @nome = nome, @contato = contato, @pass = pass from inserted;
		if exists(select * from Pizaria.[Utilizador] where email=@email )
		begin
			raiserror ('User email already in use', 1, 1); 
		end
		else
		begin
			insert into Pizaria.[Utilizador] (email, nome, contato, pass)
			values (@email, @nome, @contato, @pass)
		end
	end
go