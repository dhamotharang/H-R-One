
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.28'
BEGIN

	BEGIN TRANSACTION 

	-- Store the next file sequence number for bank file
	CREATE TABLE BankFileDailySeq
	(
		BankFileDailySeqID int NOT NULL IDENTITY (1, 1),
		BankCode nvarchar(255) NOT NULL,
		BankFileDate datetime NOT NULL,
		BankFileSeq int NOT NULL,

		CONSTRAINT PK_BankFileDailySeq PRIMARY KEY CLUSTERED 
		(
			BankFileDailySeqID
		)
	);

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.28a'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.28a';
	
	COMMIT TRANSACTION
END

