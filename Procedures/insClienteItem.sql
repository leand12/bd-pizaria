
drop procedure Pizaria.insClienteItem;
go


go
create procedure Pizaria.insClienteItem 
    @cli_email  nvarchar(255),
    @quantity   int,
    @item_ID    int
as
begin
    set nocount on
	
    IF (NOT EXISTS (SELECT * FROM tempdb.sys.objects WHERE [name] = '##ClienteItem'))
        begin
			create table Pizaria.##ClienteItem(
					cli_email	nvarchar(255),
					item_ID		int,
					quantidade	int				not null,
					primary key(cli_email,item_ID),
			);           
        end
    
    IF (not EXISTS (SELECT TOP 1 cli_email, item_ID FROM Pizaria.##ClienteItem WHERE item_ID=@item_ID AND cli_email=@cli_email ))
        begin
            insert Pizaria.##ClienteItem (cli_email, item_ID, quantidade) values (@cli_email, @item_ID, @quantity)
        end
    else
        begin
            update Pizaria.##ClienteItem set quantidade=quantidade+@quantity where item_ID=@item_ID and cli_email=@cli_email
        end
end
go

exec Pizaria.insClienteItem @cli_email='cliente@gmail.com', @quantity=1, @item_ID=1

