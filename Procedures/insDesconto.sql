drop procedure Pizaria.insDesconto
go
go
create procedure Pizaria.insDesconto
	@codigo			int,
	@percentagem	int,
	@inicio			datetime,
	@fim			datetime,
	@response	nvarchar(255)='' output
as
	begin
		set nocount on
		begin try
			set @response = 'Sucess!'
			if(exists(select top 1 codigo from Pizaria.Desconto where codigo=@codigo))
				set @response = 'Code already exists'
			else if(@fim <= @inicio)
				set @response = 'Start Date must be first than the End Date'
			else
				insert into Pizaria.Desconto(codigo, percentagem, inicio, fim) values (@codigo, @percentagem, @inicio, @fim)
		end try
		begin catch
			set @response = 'Error Inserting Discount Code'
			RAISERROR('Error Inserting Discount Code',16,1)
			return
		end catch
	end
go