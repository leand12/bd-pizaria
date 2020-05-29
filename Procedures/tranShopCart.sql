drop procedure Pizaria.TranShopCart;
go

go
create procedure Pizaria.TranShopCart 
    @cliente_email			nvarchar(255),
	@endereco_fisico		varchar(50),
	@hora					date,		
	@metodo_pagamento		varchar(30),
	@des_codigo				int,
    @response	            varchar(50) output
as
	begin
		IF (NOT EXISTS ( SELECT * FROM tempdb..sysobjects WHERE [name] = '##ClienteItem') ) OR (NOT EXISTS (select * from Pizaria.##ClienteItem where cli_email=@cliente_email))
		begin
			set @response = 'Shop Cart is Empty'
			return
		end

        set nocount on
        declare @estafeta_email	nvarchar(255) 
        set @estafeta_email = Pizaria.findBestestafeta()
        
        
        begin try
		begin tran
            declare @last_ID int
            Exec Pizaria.insEncomenda @cliente_email=@cliente_email, @estafeta_email=@estafeta_email, @endereco_fisico=@endereco_fisico, @hora=@hora, @metodo_pagamento=@metodo_pagamento, @des_codigo=@des_codigo, @last_ID=@last_ID output    
        
            DECLARE @cli_email as nvarchar(255), @item_ID as int, @quantidade as int;
            
            DECLARE C CURSOR FAST_FORWARD
            FOR SELECT cli_email, item_ID, quantidade FROM Pizaria.##ClienteItem where cli_email=@cliente_email;

            OPEN C;

            FETCH C INTO @cliente_email, @item_ID, @quantidade;

            WHILE @@FETCH_STATUS = 0
            BEGIN
                declare @itemType varchar(15)
                set @itemType=Pizaria.findItemType(@item_ID)
                if (@itemType='Menu')
                begin
                    IF EXISTS (select top 1 men_ID from Pizaria.MenuProduto join Pizaria.Piza on pro_ID=Piza.ID
                    join Pizaria.Bebida on Bebida.ID=pro_ID
                    join Pizaria.PizaIngrediente on piz_ID=Piza.ID
                    join Pizaria.Ingrediente on ing_ID=Ingrediente.ID 
                    where Ingrediente.quantidade_disponivel - PizaIngrediente.quantidade*MenuProduto.quantidade*@quantidade < 0 
                    and Bebida.quantidade_disponivel - MenuProduto.quantidade < 0
                    and men_ID=@item_ID 
                    )
                    begin
                        rollback tran
                        set @response='Number of Products not Available'
                        return
                    end

                    update Pizaria.Ingrediente
                    set quantidade_disponivel = quantidade_disponivel - PizaIngrediente.quantidade*MenuProduto.quantidade*@quantidade
                    from Pizaria.MenuProduto join Pizaria.Piza on pro_ID=Piza.ID
                    join Pizaria.PizaIngrediente on piz_ID=Piza.ID
                    join Pizaria.Ingrediente on ing_ID=Ingrediente.ID 
                    where men_ID=@item_ID

                    update Pizaria.Bebida
                    set quantidade_disponivel = quantidade_disponivel - MenuProduto.quantidade*@quantidade
                    from Pizaria.MenuProduto join Pizaria.Bebida on Bebida.ID=pro_ID
                    where men_ID=@item_ID

                end
                if (@itemType='Piza')
                begin
                    IF EXISTS (select top 1 piz_ID from Pizaria.PizaIngrediente
                    join Pizaria.Ingrediente on ing_ID=Ingrediente.ID 
                    where Ingrediente.quantidade_disponivel - PizaIngrediente.quantidade*@quantidade < 0 
                    and piz_ID=@item_ID
                    )
                    begin
                        rollback tran
                        set @response='Number of Products not Available'
                        return
                    end
    
                    update Pizaria.Ingrediente
                    set quantidade_disponivel = quantidade_disponivel - PizaIngrediente.quantidade*@quantidade
                    from Pizaria.PizaIngrediente join Pizaria.Ingrediente on ing_ID=Ingrediente.ID 
                    where piz_ID=@item_ID
                end
                if (@itemType='Bebida')
                begin
                    IF EXISTS (select top 1 ID from Pizaria.Bebida
                    where quantidade_disponivel - @quantidade < 0 
                    and ID=@item_ID
                    )
                    begin
                        rollback tran
                        set @response='Number of Products not Available'
                        return
					end

                    update Pizaria.Bebida
                    set quantidade_disponivel = quantidade_disponivel - @quantidade
                    from Pizaria.Bebida
                    where ID=@item_ID
                end
                if (@itemType='Ingrediente')
                begin
                    IF EXISTS (select top 1 ID from Pizaria.Ingrediente
                    where quantidade_disponivel - @quantidade < 0 
                    and ID=@item_ID
                    )
                    begin
                        rollback tran
						set @response='Number of Products not Available'
                        return
                    end
    
                    update Pizaria.Ingrediente
                    set quantidade_disponivel = quantidade_disponivel - @quantidade
                    from Pizaria.Ingrediente
                    where ID=@item_ID
                end

                insert into Pizaria.EncomendaItem (enc_ID, item_ID, quantidade) values (@last_ID, @item_ID, @quantidade)
                
                FETCH C INTO @cliente_email, @item_ID, @quantidade;
            END;

            CLOSE C;
            DEALLOCATE C;
		commit tran   
        end try
        begin catch
			set @response='Error'
			delete from Pizaria.##ClienteItem where cli_email=@cliente_email
			rollback
            return
        end catch
        
		set @response='Success'

		delete from Pizaria.##ClienteItem where cli_email=@cliente_email
    end
go
