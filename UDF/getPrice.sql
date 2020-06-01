drop function Pizaria.getPrice;
go

go
create function Pizaria.getPrice (@ID int) returns decimal(19,2)
as
    begin
		declare @price decimal(19,2)
        set @price = (select preco from Pizaria.Item where ID=@ID)
		return @price
    end
go