create view Pizaria.BebidaView as
select nome, preco, Bebida.ID, quantidade_disponivel from Pizaria.Bebida join Pizaria.Item on Item.ID=Bebida.ID
where quantidade_disponivel > 0