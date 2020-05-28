
drop procedure Pizaria.insClienteItem;
go


go
create procedure Pizaria.insClienteItem 
    @cli_email  nvarchar(255),
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
					foreign key(cli_email) references Pizaria.Cliente(email),
					foreign key(item_ID) references Pizaria.Item(ID)
			);           
        end
    
    IF (not EXISTS (SELECT TOP 1 cli_email, item_ID FROM Pizaria.##ClienteItem WHERE item_ID=@item_ID AND cli_email=@cli_email ))
        begin
            insert Pizaria.##ClienteItem (cli_email, item_ID, quantidade) values (@cli_email, @item_ID, 1)
        end
    else
        begin
            update Pizaria.##ClienteItem set quantidade=quantidade+1 where item_ID=@item_ID and cli_email=@cli_email
        end
end
go
