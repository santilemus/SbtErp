USE [Sbt_Erp]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =================================================================================
-- Author:		Santiago Enrique Lemus
-- Create date: 12/05/2020
-- Description:	Retornar los días de un año, para identificar los años que son bisiesto
-- Nota       : 
-- =================================================================================
create FUNCTION [dbo].[fnDaysInYear] 
(
   @Year smallint
)
returns smallint 
as
begin
  declare @dias smallint
  set @dias = case day(dateadd(dd, -1, convert(datetime, cast(@Year as varchar(4)) + '0301', 112))) 
                when 28  then 365 else 366 
              end
  return @dias
end
go
grant execute on [dbo].[fnDaysInYear] to public
go
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retornar los días de un año, para identificar los años que son bisiesto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'fnDaysInYear'
go

