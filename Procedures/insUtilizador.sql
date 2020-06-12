drop procedure Pizaria.insUtilizador;
go
go
create procedure Pizaria.insUtilizador
	@email nvarchar(255), 
    @nome varchar(50), 
    @contato int,
    @pass varchar(30),
    @response nvarchar(255) output
as
begin
	set nocount on
	IF (not EXISTS (SELECT TOP 1 email FROM  Pizaria.[Utilizador] WHERE email=@email))
	begin
		declare @salt uniqueidentifier=newid()
		begin try
			insert into Pizaria.[Utilizador] (email, nome, contato, salt, pass)
			values (@email, @nome, @contato, @salt,HASHBYTES('SHA2_512', @pass+CAST(@salt AS NVARCHAR(36))))
			SET @response='Success'
		end try
		begin catch
			set @response=error_message()
		end catch
	end
	else
		SET @response='Username already exists'
end
go