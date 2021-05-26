drop function dbo.Ping
go
drop assembly SBT_Utils
GO
-- creando el assembly
create assembly SBT_Utils authorization dbo 
 from 'D:\bdatos\sql_server\respaldo\Utils\SBT.Apps.Utils.dll'  
 with permission_set = /*safe*/ external_access;  
go
-- declarando la funcion
CREATE FUNCTION dbo.Ping (@host nvarchar(50), @timeOut smallint) returns nvarchar(100)
as external name [SBT_Utils].[SBT.Apps.Utils.PingHost].[DoPing]
go
grant execute on dbo.Ping to public
go

-- probando la funcion
declare @host1 nvarchar(50) = 'betomovilasus'
declare @host2 nvarchar(50) = '192.168.1.106'
select @host1, dbo.Ping(@host1, 1000)
select @host2, dbo.Ping(@host2, 1000)


