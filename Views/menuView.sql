drop view Pizaria.MenuView

go

create view Pizaria.MenuView as
select qq.nome, qq.preco, qq.ID as ID from 
	(
	select nome, preco, Menu.ID from (((Pizaria.Menu join Pizaria.Item on Item.ID=Menu.ID)
	join Pizaria.MenuProduto on Menu.ID=men_ID)
	join Pizaria.Bebida on Bebida.ID=pro_ID) where quantidade_disponivel>=quantidade
	) as qq 
join PizaView on PizaView.ID=qq.ID

go
