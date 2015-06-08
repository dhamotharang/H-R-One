
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.30a'
BEGIN

	BEGIN TRANSACTION 

	ALTER TABLE RequestLeaveApplication
		ADD RequestLeaveAppDateFromAM nvarchar(10) NULL,
			RequestLeaveAppDateToAM nvarchar(10) NULL ;
		
	ALTER TABLE EmpRequest
		ADD EmpRequestFromDateAM nvarchar(10) NULL,
			EmpRequestToDateAM nvarchar(10) NULL ;
		
	ALTER TABLE LeaveApplication
		ADD LeaveAppDateFromAM nvarchar(10) NULL,
			LeaveAppDateToAM nvarchar(10) NULL ;
	
	CREATE TABLE BankSwift(
		BankSwiftID int IDENTITY(1,1) NOT NULL,
		BankCode nvarchar(50) NULL,
		BankName nvarchar(255) NULL,
		SwiftCode nvarchar(25) NULL,
		LocalClearingCode nvarchar(10) NULL,
		CountryCode nvarchar(10)
	) ;

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.31'
	WHERE	ParameterCode='DBVERSION';
	
	SELECT  @DBVERSION='1.0.31';
	
	COMMIT TRANSACTION
END

