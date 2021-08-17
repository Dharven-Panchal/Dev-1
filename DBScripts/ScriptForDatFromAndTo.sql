use DataCrawling
go
ALTER TABLE CourtRecords
ADD DateFrom date;
ALTER TABLE CourtRecords
ADD DateTo date;
Go
USE [DataCrawling]
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertCaseRecord]    Script Date: 17-08-2021 11:57:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_InsertCaseRecord]
(
@sXml XML='<NewDataSet />'
)
AS
BEGIN
   -- SET NOCOUNT OFF added to get number of rows affected by this query.
	SET NOCOUNT OFF;

	DECLARE @xmldocumentHandler INT 
 	EXEC sp_xml_preparedocument @xmldocumentHandler OUTPUT, @sXml 
	DECLARE @Errorcode	NVARCHAR(max)='0'

		SELECT * INTO #CaseData_t FROM OPENXML(@xmldocumentHandler, '/NewDataSet/CourtRecord_t',2) 
		WITH(Fname NVARCHAR(50),Mname NVARCHAR(50),Lname NVARCHAR(50), Address1 NVARCHAR(50), Address2 NVARCHAR(50), State NVARCHAR(50),City NVARCHAR(50),ZipCode NVARCHAR(50),CaseNumber NVARCHAR(50),CaseType NVARCHAR(50),CaseDate NVARCHAR(50),PartyName NVARCHAR(50),NewBalance Nvarchar(50),BusinessName Nvarchar(50),DateFrom Nvarchar(50),DateTo Nvarchar(50))	
		
		declare @caseno as nvarchar(50)
		select @caseno = CaseNumber from #CaseData_t 


	BEGIN TRANSACTION
	IF	@Errorcode ='0'
	begin
		if exists(select 1 from #CaseData_t)
		begin 
			if not exists(select 1 from CourtRecords where CaseNumber=@caseno)
			begin
			--select '1'
			   insert into CourtRecords(Fname,Mname,Lname,Address1,Address2,State,City,PostalCode,CaseNumber,CaseType,CaseDate,PartyName,CreateDate,NewBalance,BusinessName,DateFrom,DateTo)
			   select Fname,Mname,Lname,Address1,Address2,State,City,ZipCode,CaseNumber,CaseType,CaseDate,PartyName,GETDATE(),NewBalance,BusinessName,CONVERT(SMALLDATETIME, DateFrom,105),CONVERT(SMALLDATETIME, DateTo,105) from #CaseData_t
			   set @Errorcode  =@@ERROR
			end
		End

    End

	IF	@Errorcode='0'
	BEGIN 
		COMMIT TRANSACTION 
	END
	 ELSE 
         ROLLBACK TRANSACTION 


END