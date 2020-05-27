drop function Pizaria.findItemType;
go

go
create function Pizaria.findItemType (@ID int) returns varchar(15)
as
    begin
        if (exists( select *
                    from Pizaria.Menu
                    where Menu.ID=@ID
                    )
            )
            return 'Menu'
        if (exists( select *
                    from Pizaria.Piza
                    where Piza.ID=@ID
                    )
            )
            return 'Piza'
        if (exists( select *
                    from Pizaria.Bebida
                    where Bebida.ID=@ID
                    )
            )
            return 'Bebida'
        if (exists( select *
                    from Pizaria.Ingrediente
                    where Ingrediente.ID=@ID
                    )
            )
            return 'Ingrediente'
		return 'None'
    end
go