drop procedure Pizaria.insRestaurante
go
go
create procedure Pizaria.insRestaurante
	@contato int,
	@nome varchar(50),
	@morada varchar(50),
	@lotacao int,
	@hora_abertura time, 
	@hora_fecho time,
	@dono nvarchar(255),
	@response nvarchar(255)='' output
as
	begin
		set nocount on
			begin try
				IF (EXISTS (SELECT TOP 1 contato FROM  Pizaria.Restaurante WHERE contato=@contato))
				begin
					set @response = 'A Restaurant with that contact is already registered.'
					return
				end
				ELSE
				begin
					set @response = 'Sucess!'
					INSERT INTO Pizaria.Restaurante (contato,nome,morada,lotacao,hora_abertura,hora_fecho,dono)
					VALUES (@contato, @nome, @morada, @lotacao, @hora_abertura, @hora_fecho, @dono)
					return
				end
			end try
			begin catch
				RAISERROR('Error Adding a Restaurant',16,1)
				return
			end catch
	end
go