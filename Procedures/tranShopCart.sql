drop procedure Pizaria.TranShopCart;
go

go
create procedure Pizaria.TranShopCart 
    @cliente_email			nvarchar(255),
	@estafeta_email			nvarchar(255),
	@endereco_fisico		varchar(50),
	@hora					date,		
	@metodo_pagamento		varchar(30),
	@des_codigo				int
    @response	            nvarchar(255)='' output
as
	begin
        set nocount on

        begin tran
            Exec insEncomenda @cliente_email=@cliente_email, @estafeta_email=@estafeta_email, @endereco_fisico=@endereco_fisico, @hora=@hora, @metodo_pagamento=@metodo_pagamento, @des_codigo=@des_codigo    

            DECLARE @cli_email as nvarchar(255), @item_ID as int, @quantidade as int;

            DECLARE C CURSOR FAST_FORWARD
            FOR SELECT cli_email, item_ID, quantity FROM Pizaria.ClienteItem where clie_email=@cliente_email;

            OPEN C;

            FETCH C INTO @cliente_email, @item_ID, @quantidade;

            WHILE @@FETCH_STATUS = 0
            BEGIN
                declare @itemType varchar(15)
                set @itemType=findItemType(@item_ID)
                if (@itemType="Menu")
                begin
                    
                end
                if (@itemType="Piza")
                begin

                end
                if (@itemType="Bebida")
                begin

                end
                if (@itemType="Ingrediente")
                begin

                end
                
                FETCH C INTO @cliente_email, @quantidade, @qty;
            END;

            CLOSE C;
            DEALLOCATE C;
         
        commit tran
    end
go