drop trigger Pizaria.delEstafeta;
go

go
create trigger Pizaria.delEstafeta on Pizaria.Estafeta
instead of delete
as
    begin
        set nocount on;
        declare @email as nvarchar(255);

        select @email = email from deleted;

        begin try
			declare @ID int;
            if (exists(select top 1 email from Pizaria.Estafeta join Pizaria.Encomenda on estafeta_email=email where email=@email))
				begin
					update Pizaria.Estafeta set res_contato = null where email=@email;
					--cursor para colocar o melhor estafeta em cada uma das encomendas
					DECLARE c CURSOR FAST_FORWARD
					FOR select ID from Pizaria.Encomenda where estafeta_email=@email;
					OPEN c;
					FETCH NEXT FROM c into @ID;  

					WHILE @@FETCH_STATUS = 0  
					BEGIN
						update Pizaria.Encomenda set estafeta_email = (select Pizaria.FindBestEstafeta()) where ID=@ID
						FETCH NEXT FROM c into @ID;  
					END;
					CLOSE c;
					DEALLOCATE c;
				end
            else
				begin
					update Pizaria.Estafeta set res_contato = null where email=@email
				end
        end try
        begin catch
            raiserror('Error',16,1);
            return;
        end catch
    end
go

delete from Pizaria.Estafeta where email='a.enim@loremluctus.edu'
