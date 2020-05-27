drop procedure Pizaria.insClienteItem;
go

go
create procedure Pizaria.insClienteItem 
    @cli_email  nvarchar(255),
    @item_ID    int
as
begin
    set nocount on

    IF (not EXISTS (SELECT TOP 1 cli_email, item_ID FROM Pizaria.ClienteItem WHERE item_ID=@item_ID AND cli_email=@cli_email ))
        begin
            insert Pizaria.ClienteItem (cli_email, item_ID, quantidade) values (@cli_email, @item_ID, 1)
        end
    else
        begin
            update Pizaria.ClienteItem set quantidade=quantidade+1 where item_ID=@item_ID and cli_email=@cli_email
        end

end
go

exec Pizaria.insClienteItem @cli_email='cliente@gmail.com', @item_ID=5
exec Pizaria.insClienteItem @cli_email='cliente@gmail.com', @item_ID=5