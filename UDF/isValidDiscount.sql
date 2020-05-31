drop function Pizaria.isValidDiscount;
go

go
create function Pizaria.isValidDiscount (@code int, @email nvarchar(255)) returns int
as
    begin
        if (exists( select *
                    from Pizaria.DescontoCliente
                    where cli_email=@email and des_codigo=@code
                    )
            )
            return 0
        if (not exists( select * from Desconto
                    where codigo=@code and SYSDATETIME() <= fim and SYSDATETIME() >= inicio
                    )
            )
            return 0
        return 1
    end
go
