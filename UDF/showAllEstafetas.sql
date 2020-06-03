drop function Pizaria.showAllEstafetas;
go

go
create function Pizaria.showAllEstafetas () returns Table
as
	return (SELECT Estafeta.email, [Utilizador].nome as est_nome, [Utilizador].contato, Restaurante.nome as res_nome  FROM Pizaria.Estafeta join Pizaria.[Utilizador] on [Utilizador].email=Estafeta.email join Pizaria.Restaurante on res_contato=Restaurante.contato where res_contato is not null)
go