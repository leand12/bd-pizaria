drop function Pizaria.ingredienteAvail

go

create function Pizaria.ingredienteAvail() returns Table
as
return(
select nome, preco, Ingrediente.ID, quantidade_disponivel
from Pizaria.Ingrediente join Pizaria.Item on Item.ID=Ingrediente.ID
where quantidade_disponivel > 0
)
go