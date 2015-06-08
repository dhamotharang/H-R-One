

DECLARE @DBVERSION as varchar(100);
SET @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.9' 
Begin
	
	CREATE TABLE TextTransformation
	(
		TextTransformationID INT NOT NULL IDENTITY (1, 1),
		TextTransformationOriginalString NVARCHAR(255) NULL,
		TextTransformationReplacedTo NVARCHAR(255) NULL,
		CONSTRAINT PK_TextTransformation PRIMARY KEY CLUSTERED 
		(
			TextTransformationID
		) 		
	)

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='2.0'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



