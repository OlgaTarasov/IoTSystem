USE master
GO

IF DB_ID (N'SignalDB') IS NOT NULL
DROP DATABASE SignalDB
GO

CREATE DATABASE SignalDB
GO

CREATE LOGIN [signaladmin] WITH PASSWORD = '12345';
GO

ALTER LOGIN [signaladmin] WITH DEFAULT_DATABASE = SignalDB
GO

USE SignalDB
GO

CREATE USER [signaladmin] FOR LOGIN [signaladmin] 
GO

EXEC sp_addrolemember 'db_owner', 'signaladmin';

--ALTER AUTHORIZATION ON DATABASE::SignalDB to [signaladmin];


CREATE TABLE [dbo].[Source](
	[SourceRecID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Code] [int] NOT NULL,
	[Source] [varchar](20) NOT NULL
CONSTRAINT [PK_T_SOURCE] PRIMARY KEY CLUSTERED 
(
	[SourceRecID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Source]
ADD CONSTRAINT AK_SourceCode UNIQUE (Code)
GO

CREATE TABLE [dbo].[Signals]( 
	[SignalRecID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[SourceRecID] [int] NOT NULL,
	[Value] [int] NOT NULL,
	[IsAnomaly] [tinyint] NOT NULL,
	[TransmittedTime] [datetime] NOT NULL,
	[DateEntered] [datetime] NOT NULL
 CONSTRAINT [PK_T_SIGNAL] PRIMARY KEY CLUSTERED 
(
	[SignalRecID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Signals] ADD  DEFAULT (getdate()) FOR [DateEntered]
GO

INSERT INTO [dbo].[Source]
(Code, Source)
VALUES
 (1, 'Sine')
,(2, 'State')
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetSignals] 
	@SourceCode int,			
	@Page int,
	@Size int
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @SourceRecID int = (Select SourceRecID from Source with (nolock) where Code = @SourceCode)
	SELECT 
	_signals.SignalRecID as ID
	,_signals.Value
	,_signals.TransmittedTime as TimeStamp
	,_signals.IsAnomaly
	,_source.Source
	from Signals _signals with (nolock)
	inner join Source _source with (nolock) on _signals.SourceRecID = _source.SourceRecID
	WHERE _signals.SourceRecID = @SourceRecID 
	ORDER BY SignalRecID desc
	OFFSET (@Page -1) * @size rows
	FETCH next @Size rows only
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[InsertSignal] 
	@SourceCode int, 
	@Value int, 
	@TimeStamp datetime,
	@IsAnomaly tinyint
AS
DECLARE @RESULT AS INT = 0
BEGIN
	BEGIN TRY
	BEGIN TRANSACTION
		DECLARE @SourceRecID int = (select SourceRecID from Source where Code = @SourceCode)
		DECLARE @TableTmp TABLE( RecID int);

		INSERT INTO [dbo].[Signals]
		([SourceRecID], [Value], [TransmittedTime], [IsAnomaly])
		OUTPUT inserted.SignalRecID INTO @TableTmp
		VALUES
		(@SourceRecID, @Value, @TimeStamp, @IsAnomaly)

		SET @RESULT = (SELECT RecID FROM @TableTmp)

	END TRY
    BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION; 
		END
        SET @RESULT = 0
        SELECT ERROR_NUMBER() AS ErrorNumber, (select (ERROR_PROCEDURE()+'; line: '+CONVERT(varchar(10),ERROR_LINE())+'; ' + ERROR_MESSAGE())) AS ErrorMessage;
		RETURN
    END CATCH

    SELECT @RESULT as ID

	COMMIT TRANSACTION
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetSource] 
AS
BEGIN
	SET NOCOUNT ON
	SELECT 
	SourceRecID as ID
	,Code
	,Source
	FROM Source  with (nolock)
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAnimalySignals]
	@Page int, 
	@Size int
AS
BEGIN
	SELECT 
	_signals.SignalRecID as ID
	,_signals.Value
	,_signals.TransmittedTime as TimeStamp
	,_signals.IsAnomaly
	,_source.Source
	from Signals _signals with (nolock)
	inner join Source _source with (nolock) on _signals.SourceRecID = _source.SourceRecID
	WHERE _signals.IsAnomaly = 1
	ORDER BY _signals.SignalRecID desc
	OFFSET (@Page -1) * @size rows
    FETCH next @Size rows only
END