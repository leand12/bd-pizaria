drop function Pizaria.menuAvail
go
go

create function Pizaria.menuAvail() returns Table
as
return(
select Item.Id, nome, preco from ((Pizaria.Menu join Pizaria.Item on Item.ID=Menu.ID)
	join Pizaria.MenuProduto on Menu.ID=men_ID) join
	(
	(select ID from Pizaria.bebidaAvail()) UNION (select ID from Pizaria.pizaAvail())
	) as qq
	on qq.ID=pro_ID
	group by Item.Id, nome, preco
	)
go