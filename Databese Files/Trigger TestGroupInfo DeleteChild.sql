-- ================================================
-- Template generated from Template Explorer using:
-- Create Trigger (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- See additional Create Trigger templates for more
-- examples of different Trigger statements.
--
-- This block of comments will not be included in
-- the definition of the function.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
use NJSFDB;
go

--判断是否存在触发器，如果存在则删除
if exists(select * from sysobjects where name='DeleteChild')
drop trigger DeleteChild;
go

CREATE TRIGGER DeleteChild 
   ON  dbo.TestGroupInfo 
   AFTER  DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	begin
		delete from dbo.TestItemInfo where Parent in (select GUID from deleted);
	end
	


END
GO
