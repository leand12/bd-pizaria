drop function Pizaria.isEmployed;
go

go
create function Pizaria.isEmployed (@email nvarchar(255)) returns int
as
    begin
        if ((select res_contato from Pizaria.Estafeta where email=@email) is not NULL)
            return 0
		return 1
    end
go