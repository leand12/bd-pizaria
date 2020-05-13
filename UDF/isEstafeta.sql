drop function Pizaria.isEstafeta;
go

go
create function Pizaria.isEstafeta (@email nvarchar(255)) returns int
as
    begin
        if (not exists( select *
                    from Pizaria.[Estafeta]
                    where email=@email
                    )
            )
            return 0
        return 1
    end
go