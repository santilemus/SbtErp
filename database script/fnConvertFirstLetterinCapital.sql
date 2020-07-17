/****** Object:  UserDefinedFunction [dbo].[ConvertFirstLetterinCapital]    Script Date: 22/6/2020 09:27:30 ******/
set ansi_nulls on
go
set quoted_identifier on
go
create function [dbo].[fnConvertFirstLetterinCapital](@Text varchar(5000)) returns varchar(5000) 
as 
begin
  declare @Index int;
  declare @FirstChar char(1);
  declare @LastChar char(1);
  declare @String varchar(5000);
  set @String = lower(@Text);
  set @Index = 1;
  while @Index <= len(@Text)
  begin
	set @FirstChar = substring(@Text, @Index, 1);
	set @LastChar = iif(@Index = 1, ' ', substring(@Text, @Index - 1, 1));
	if @LastChar in(' ', ';', ':', '!', '?', ',', '.', '_', '-', '/', '&', '''', '(', '#', '*', '$', '@')
    begin
	  if @FirstChar != '''' or upper(@FirstChar) != 'S'
		set @String = stuff(@String, @Index, 1, upper(@FirstChar));
	end;
	set @Index = @Index + 1;
  end;
  return @String;
end;
go
