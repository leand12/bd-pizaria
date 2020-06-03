drop procedure Pizaria.insEstafeta
go
go
create procedure Pizaria.insEstafeta
	@email			nvarchar(255),
	@nome			varchar(50),
	@contato		int,
	@res_contato	int,
	@response	nvarchar(255)='' output
as
	begin
		set nocount on
		begin try
			set @response = 'Sucess!'
			IF (EXISTS (SELECT TOP 1 email FROM  Pizaria.[Utilizador] WHERE email=@email))
			begin
				set @response = 'User already registered.'
				if (exists(SELECT TOP 1 email FROM  Pizaria.Estafeta WHERE email=@email and res_contato=null))
				begin
					set @response = 'Courier has been Rehired!'
					update Pizaria.Estafeta set res_contato = @res_contato where email=@email
					update Pizaria.[Utilizador] set nome = @nome, contato = @contato where email=@email
				end
			end
			ELSE
			begin
			    DECLARE @pass VARCHAR(12)
			    DECLARE @BinaryData VARBINARY(12)
			    DECLARE @CharacterData VARCHAR(12)
 
				set @BinaryData = CRYPT_GEN_RANDOM (12)
 
				Set @CharacterData=cast ('' as xml).value ('xs:base64Binary(sql:variable("@BinaryData"))',
							   'varchar (max)')
   
				SET @pass = @CharacterData

				Exec Pizaria.register @type = 1, @email = @email, @nome = @nome, @contato = @contato, @pass = @pass, @res_contato = @res_contato,  @idade = null, @genero = null, @morada = null, @response=@response output
			end
		end try
		begin catch
			RAISERROR('Error Adding a Courier',16,1)
			return
		end catch
	end
go