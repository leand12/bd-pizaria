drop procedure Pizaria.register;
go

go
create procedure Pizaria.register
	@type int,
	@email nvarchar(255), 
    @nome varchar(50), 
    @contato int,
    @pass varchar(30),
    @res_contato int,
	@idade int,
	@genero char,
	@morada nvarchar(255),
    @response nvarchar(255) output
as
begin
	set nocount on
	IF (not EXISTS (SELECT TOP 1 email FROM  Pizaria.[Utilizador] WHERE email=@email)) -- será redundante com o trigger?
	begin
		declare @salt uniqueidentifier=newid()
		begin try
			insert into Pizaria.[Utilizador] (email, nome, contato, salt, pass)
			values (@email, @nome, @contato, @salt,HASHBYTES('SHA2_512', @pass + CAST(@salt AS NVARCHAR(36))))
		
			if @type = 0
			begin
				insert into Pizaria.[Administrador] (email)
				values (@email)
			end
			if @type = 1
			begin
				insert into Pizaria.[Estafeta] (email, res_contato)
				values (@email, @res_contato)
			end
			else
			begin
				insert into Pizaria.[Cliente] (email, idade, genero, morada)
				values (@email, @idade, @genero, @morada)
			end

			set @response='User successfully registered'
		end try
		begin catch
			set @response=error_message()
		end catch
	end
	else
		SET @response='Username already exists'
end
go