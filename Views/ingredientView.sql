drop view Pizaria.IngredienteView

go

create view Pizaria.IngredienteView as
select nome, preco, Ingrediente.ID, quantidade_disponivel from Pizaria.Ingrediente join Pizaria.Item on Item.ID=Ingrediente.ID
where quantidade_disponivel > 0

go