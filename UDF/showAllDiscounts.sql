drop function Pizaria.showAllDiscounts;
go

go
create function Pizaria.showAllDiscounts (@cliente_email nvarchar(255)) returns Table
as
	return (select codigo, percentagem, inicio, fim from Pizaria.Desconto where Pizaria.isValidDiscount(codigo, @cliente_email)!=0)
go