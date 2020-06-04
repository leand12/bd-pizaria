drop procedure Pizaria.updRestaurante
go
go
create procedure Pizaria.updRestaurante
	@contato int,
	@nome varchar(50),
	@morada varchar(50),
	@lotacao int,
	@hora_abertura time, 
	@hora_fecho time,
	@dono nvarchar(255)
as
	begin
		set nocount on
			begin try
				IF (EXISTS (SELECT TOP 1 contato FROM  Pizaria.Restaurante WHERE contato=@contato))
				begin
					update Pizaria.Restaurante
					set nome = @nome, morada = @morada, lotacao = @lotacao, hora_abertura = @hora_abertura, hora_fecho = @hora_fecho, dono = @dono
					where contato = @contato
					return
				end
			end try
			begin catch
				RAISERROR('Error Updating the Restaurant',16,1)
				return
			end catch
	end
go