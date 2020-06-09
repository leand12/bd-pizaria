drop function Pizaria.getEncPreco;
go
go
create function Pizaria.getEncPreco(@ID int) returns decimal(19,2)
as
begin
	declare @val decimal(19,2)
	if (exists(select top 1 ID from Pizaria.Encomenda where ID = @ID))
		set @val = (select sum(preco*quantidade) as total 
				from Pizaria.Encomenda
				join Pizaria.EncomendaItem on Encomenda.ID=enc_ID
				join Pizaria.Item on item_ID=Item.ID
				where enc_ID = @ID)*Pizaria.getDesconto(@ID)
	else if(exists(select top 1 ID from Pizaria.EncomendaEntregue where ID = @ID))
		set @val = (select sum(preco*quantidade) as total 
			from Pizaria.EncomendaEntregue
			join Pizaria.EncEntregueItem on EncomendaEntregue.ID=enc_ID
			join Pizaria.Item on item_ID=Item.ID
			where enc_ID = @ID)*Pizaria.getDesconto(@ID)
	return @val
end
go