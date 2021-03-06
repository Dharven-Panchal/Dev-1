USE [DataCrawling]
GO
/****** Object:  Table [dbo].[CaseTypes]    Script Date: 11-08-2021 14:23:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CaseTypes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CaseType] [nvarchar](200) NULL,
	[CTValue] [bigint] NULL,
 CONSTRAINT [PK_CaseTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourtRecords]    Script Date: 11-08-2021 14:23:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourtRecords](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Fname] [nvarchar](50) NULL,
	[Mname] [nvarchar](50) NULL,
	[Lname] [nvarchar](50) NULL,
	[Address1] [nvarchar](max) NULL,
	[Address2] [nvarchar](max) NULL,
	[State] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[PostalCode] [nvarchar](50) NULL,
	[CaseNumber] [nvarchar](50) NULL,
	[CaseType] [nvarchar](50) NULL,
	[CaseDate] [datetime] NULL,
	[PartyName] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[NewBalance] [nvarchar](50) NULL,
	[BusinessName] [nvarchar](50) NULL,
 CONSTRAINT [PK_CourtRecords] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CaseTypes] ON 

INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (1, N'90 Day Extension', 26)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (2, N'Accounting', 27)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (3, N'Administrative Hold', 126)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (4, N'Adoption', 127)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (5, N'Affirmation of Parental Status', 128)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (6, N'Aging Adult Protective Service', 129)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (7, N'Annulment', 80)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (8, N'Appeals', 29)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (9, N'Appeals (AP)', 72)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (10, N'Arbitration', 28)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (11, N'Auto Negligence', 30)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (12, N'Baker Act', 130)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (13, N'Baker Act Ex Parte', 131)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (14, N'BC - Business Transactions', 132)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (15, N'BC - Ejectment - Prop Value btwn $50,000-250,000', 133)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (16, N'BC - Ejectment - Property Value above $250,000', 134)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (17, N'Breach of Agreement/Contract', 32)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (18, N'CA - Bond Validation', 136)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (19, N'CA - Business Transactions', 137)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (20, N'CA - Enforce Foreign Judgment (Money)', 139)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (21, N'CA - Enforcement Foreign Judgment', 140)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (22, N'CA - Establish Foreign Judgment', 141)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (23, N'CA - Game Fish Forfeiture', 142)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (24, N'CA - Landlord Bond', 143)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (25, N'Capacity Limited', 144)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (26, N'Capacity Total', 145)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (27, N'Caveat by Creditor', 1)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (28, N'Caveat by Heir/Beneficiary/Kin', 2)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (29, N'Certiorari', 33)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (30, N'Child Abduction', 81)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (31, N'Child in Need of Services', 146)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (32, N'Child Support', 82)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (33, N'Civil - Trust', 70)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (34, N'Condominium', 34)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (35, N'Conservatorship', 4)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (36, N'County Ordinance (CO)', 73)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (37, N'Criminal BC Cases (Non Reporting)', 147)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (38, N'Criminal Felony (CF)', 74)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (39, N'Criminal Traffic (CT)', 75)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (40, N'Curatorship', 5)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (41, N'Custody', 83)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (42, N'Damages', 35)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (43, N'Declaratory Judgment', 36)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (44, N'Delayed Birth Certificate', 84)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (45, N'Delinquency', 148)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (46, N'Dependency', 149)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (47, N'Dependency Non-Petition', 150)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (48, N'Developmental Service Retardation', 151)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (49, N'Disposition of Personal Property w/o Admin', 6)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (50, N'Dissolution of Marriage', 85)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (51, N'Dissolve Corporation', 37)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (52, N'Distress', 38)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (53, N'Domestic Prisoner', 152)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (54, N'Ejectment', 39)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (55, N'Emergency Medical', 153)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (56, N'Emergency Surgery', 154)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (57, N'Eminent Domain', 40)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (58, N'Enforcement Foreign Judgment', 86)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (59, N'Eviction', 41)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (60, N'Exploitation of Vulnerable Adult Injunction', 185)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (61, N'Family - Other', 89)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (62, N'Foreclosure', 42)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (63, N'Foreign Guardian', 7)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (64, N'Foreign Judgment', 43)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (65, N'Foreign Record', 8)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (66, N'Foreign Will', 9)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (67, N'Forfeiture', 44)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (68, N'Formal Administration', 10)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (69, N'Fugitive Warrant Cases (Non Reporting)', 155)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (70, N'Grandparent Visitation', 90)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (71, N'Guardian Advocate', 11)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (72, N'Guardianship of the Person', 13)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (73, N'Guardianship of the Person and Property', 12)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (74, N'Guardianship of the Property', 14)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (75, N'Guardianship Other', 15)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (76, N'Habeas Corpus', 46)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (77, N'Incapacity Limited', 157)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (78, N'Incapacity Total', 158)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (79, N'Indebtedness', 47)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (80, N'Infraction (IN)', 76)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (81, N'Injunction', 49)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (82, N'Injunction (Family)', 159)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (83, N'Injunctions', 87)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (84, N'Interpleader', 50)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (85, N'Involuntary Civil Commitment Sexually Violent Pred', 160)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (86, N'Judicial Intervention', 161)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (87, N'Juvenile Adoption', 162)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (88, N'Juvenile Support Order', 164)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (89, N'Lien Foreclosure', 51)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (90, N'Malpractice', 52)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (91, N'Mandamus', 53)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (92, N'Mental Health Other', 165)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (93, N'Minor&#39;s Contract', 16)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (94, N'Minor&#39;s Settlement', 17)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (95, N'Misdemeanor (MM)', 77)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (96, N'Municipal Ordinance (MO)', 78)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (97, N'Negligence', 54)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (98, N'Non-Monetary', 55)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (99, N'Notice of Trust', 19)
GO
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (100, N'Other', 57)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (101, N'Other Negligence', 56)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (102, N'Parental Responsibility and Time-sharing', 166)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (103, N'Partition', 58)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (104, N'Paternity', 88)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (105, N'Personal Injury Protection', 59)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (106, N'Petition Approval Settlement', 60)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (107, N'Petition In Re Non Age', 167)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (108, N'Petition Restraining Order', 168)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (109, N'Preneed Guardian', 169)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (110, N'Probate Other', 20)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (111, N'Products Liability', 61)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (112, N'Professional Guardian', 170)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (113, N'Prohibition', 62)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (114, N'Promissory Note', 63)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (115, N'Quiet Title', 64)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (116, N'Replevin', 65)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (117, N'Rescission', 66)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (118, N'Risk Protection Petition', 184)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (119, N'Safe Deposit Box', 21)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (120, N'SC - Replevin $1001-$2500', 171)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (121, N'Sexually Transmissible Diseases', 172)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (122, N'Shelter', 173)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (123, N'Specific Performance', 67)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (124, N'Stalking', 174)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (125, N'Summary Administration over $1000', 22)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (126, N'Summary Administration under $1000', 23)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (127, N'Tax Litigation', 68)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (128, N'Temporary Custody by Extended Family Member', 176)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (129, N'Termination of Parental rights', 177)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (130, N'Termination of Parental rights No Fee', 178)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (131, N'Timeshare Foreclosure', 69)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (132, N'Toll Infraction', 179)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (133, N'Traffic Infractions (TR)', 79)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (134, N'Truancy', 180)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (135, N'Trust', 24)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (136, N'Tuberculosis', 181)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (137, N'VA Guardianship', 182)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (138, N'VR - Vehicle Repair', 183)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (139, N'Wills For Safekeeping', 25)
INSERT [dbo].[CaseTypes] ([Id], [CaseType], [CTValue]) VALUES (140, N'Writ Cases', 71)
SET IDENTITY_INSERT [dbo].[CaseTypes] OFF
/****** Object:  StoredProcedure [dbo].[sp_GetCaseTypes]    Script Date: 11-08-2021 14:23:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[sp_GetCaseTypes]
as
begin
select 0 as id, '----Select All----' as CaseType,'0' as CTValue
union all
select id,casetype,cast(CTValue as nvarchar(50)) as CTValue from casetypes


end
GO
/****** Object:  StoredProcedure [dbo].[sp_GetReports]    Script Date: 11-08-2021 14:23:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[sp_GetReports]
@FromDate nvarchar(50),
@ToDate nvarchar(50),
@businessname nvarchar(50)=null
as
begin
---select @FromDate, @ToDate

select CaseNumber as [Case Number],CaseDate as [Case Date],CaseType as [Type],PartyName as [Party Name], Fname as [First Name], Mname as [Middle Name],
Lname as [Last Name],Address1,Address2,State as [State],City,PostalCode,NewBalance as [New Balance]
from CourtRecords where
((@FromDate IS NULL) OR (CONVERT(char(10), CreateDate ,126) BETWEEN @FromDate AND @ToDate))
And ((@businessname IS NULL) OR (BusinessName like @businessname + '%'))

end

GO
/****** Object:  StoredProcedure [dbo].[sp_InsertCaseRecord]    Script Date: 11-08-2021 14:23:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_InsertCaseRecord]
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
		WITH(Fname NVARCHAR(50),Mname NVARCHAR(50),Lname NVARCHAR(50), Address1 NVARCHAR(50), Address2 NVARCHAR(50), State NVARCHAR(50),City NVARCHAR(50),ZipCode NVARCHAR(50),CaseNumber NVARCHAR(50),CaseType NVARCHAR(50),CaseDate NVARCHAR(50),PartyName NVARCHAR(50),NewBalance Nvarchar(50),BusinessName Nvarchar(50))	

		declare @caseno as nvarchar(50)
		select @caseno = CaseNumber from #CaseData_t 


	BEGIN TRANSACTION
	IF	@Errorcode ='0'
	begin
		if exists(select 1 from #CaseData_t)
		begin 
			if not exists(select 1 from CourtRecords where CaseNumber=@caseno)
			begin
			 insert into CourtRecords(Fname,Mname,Lname,Address1,Address2,State,City,PostalCode,CaseNumber,CaseType,CaseDate,PartyName,CreateDate,NewBalance,BusinessName)
			   select Fname,Mname,Lname,Address1,Address2,State,City,ZipCode,CaseNumber,CaseType,CaseDate,PartyName,GETDATE(),NewBalance,BusinessName from #CaseData_t
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
GO
