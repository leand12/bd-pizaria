create view Pizaria.MenuView as
select nome, preco, Menu.ID from Pizaria.Menu join Pizaria.Item on Item.ID=Menu.ID 