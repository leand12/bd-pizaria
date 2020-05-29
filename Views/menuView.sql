drop view Pizaria.MenuView

go

create view Pizaria.MenuView as

select Item.Id, nome, preco from ((Pizaria.Menu join Pizaria.Item on Item.ID=Menu.ID)
	join Pizaria.MenuProduto on Menu.ID=men_ID) join
	(
	(select ID from Pizaria.BebidaView) UNION (select ID from Pizaria.PizaView)
	) as qq
	on qq.ID=pro_ID
	group by Item.Id, nome, preco

go
