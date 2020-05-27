create view Pizaria.PizaView as
select nome, preco, Piza.ID, pic from Pizaria.Piza join Pizaria.Item on Item.ID=Piza.ID