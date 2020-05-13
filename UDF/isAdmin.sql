drop function Pizaria.isAdmin;
go

go
create function Pizaria.isAdmin (@email nvarchar(255)) returns int
as
    begin
        if (not exists( select *
                    from Pizaria.[Administrador]
                    where email=@email
                    )
            )
            return 0
        return 1
    end
go