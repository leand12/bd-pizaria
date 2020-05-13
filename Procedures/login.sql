drop procedure Pizaria.UsersLogin;
go


go
create procedure Pizaria.UsersLogin
    @email		nvarchar(255),
    @pass		varchar(30),
    @response	nvarchar(255)='' output
as
begin
    set nocount on

	declare @userEmail nvarchar(255)

    if exists (select top 1 email from Pizaria.Utilizador where email=@email)
    begin
		set @userEmail=(select email from Pizaria.Utilizador where email=@email and pass=@pass)
		
		if(@userEmail is null)
			set @response='Incorrect password'
		else
			set @response='User successfully logged in'
    end
    else
		set @response='Invalid login'

end
go