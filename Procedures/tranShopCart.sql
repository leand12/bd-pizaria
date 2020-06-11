drop procedure Pizaria.TranShopCart;
go

go
create procedure Pizaria.TranShopCart 
    @cliente_email			nvarchar(255),
	@endereco_fisico		varchar(50),
	@hora					datetime,		
	@metodo_pagamento		varchar(30),
	@des_codigo				int,
	@lista					varchar(max),
    @response	            varchar(50) output
as
	begin
		set nocount on

		IF (@lista='' or @lista = null)
		begin
			set @response = 'Shop Cart is Empty'
			return
		end

        declare @estafeta_email	nvarchar(255) 
        set @estafeta_email = Pizaria.findBestestafeta()
        
        begin try
		begin tran
			insert into Pizaria.DescontoCliente (cli_email,des_codigo) Values (@cliente_email,@des_codigo)
			
            declare @last_ID int
            Exec Pizaria.insEncomenda @cliente_email=@cliente_email, @estafeta_email=@estafeta_email, @endereco_fisico=@endereco_fisico, @hora=@hora, @metodo_pagamento=@metodo_pagamento, @des_codigo=@des_codigo, @last_ID=@last_ID output

			declare @pos int = 0
			declare @len int = 0
			declare @item_ID int
			declare @quantidade int
			WHILE CHARINDEX(',', @lista, @pos+1)>0
			BEGIN
				set @len = CHARINDEX(',', @lista, @pos+1) - @pos
				set @item_ID = cast(SUBSTRING(@lista, @pos, @len) as int)
				set @pos = CHARINDEX(',', @lista, @pos+@len) +1
				set @len = CHARINDEX(',', @lista, @pos+1) - @pos
				set @quantidade =cast( SUBSTRING(@lista, @pos, @len) as int)
				set @pos = CHARINDEX(',', @lista, @pos+@len) +1

                declare @itemType varchar(15)
                set @itemType=Pizaria.findItemType(@item_ID)
                if (@itemType='Menu')
                begin
                    IF EXISTS (select top 1 men_ID from Pizaria.MenuProduto left outer join Pizaria.Piza on pro_ID=Piza.ID
					left outer join Pizaria.Bebida on Bebida.ID=pro_ID
					left outer join Pizaria.PizaIngrediente on piz_ID=Piza.ID
					left outer join Pizaria.Ingrediente on ing_ID=Ingrediente.ID
					where Ingrediente.quantidade_disponivel - PizaIngrediente.quantidade*MenuProduto.quantidade*@quantidade < 0
					or Bebida.quantidade_disponivel - MenuProduto.quantidade*@quantidade < 0
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
                        return
                    end
    
                    update Pizaria.Ingrediente
                    set quantidade_disponivel = quantidade_disponivel - @quantidade
                    from Pizaria.Ingrediente
                    where ID=@item_ID
                end

                insert into Pizaria.EncomendaItem (enc_ID, item_ID, quantidade) values (@last_ID, @item_ID, @quantidade)
            END
		commit tran   
        end try
        begin catch
			set @response='Error'
			rollback
            return
        end catch
        
		set @response='Success'
	end
go
