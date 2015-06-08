

DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from dbo.SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='0.8' 
Begin

	ALTER TABLE CompanyInbox ADD
		CompanyInboxExpiryDate DATETIME NULL

	ALTER TABLE CompanyAutopayFile ADD
		CompanyAutopayFileTransactionReference NVARCHAR(20) NULL

	ALTER TABLE CompanyMPFFile ADD
		CompanyMPFFileTransactionReference NVARCHAR(20) NULL

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
    )
    VALUES
           ('ADM001','Database Server')

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
    )
    VALUES
           ('ADM002','Company Database')

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
    )
    VALUES
           ('ADM003','User Account')

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
    )
    VALUES
           ('ADM004','System Parameter')

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
    )
    VALUES
           ('ADM005','Public Holiday')
           
	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
    )
    VALUES
           ('ADM006','Generate/renew Key Pair')

	INSERT INTO UserFunction (UserID, FunctionID)
	SELECT usr.UserID, sf.FunctionID
	FROM Users usr, SystemFunction sf
	WHERE NOT EXISTS
	(	
		SELECT * 
		FROM UserFunction ugf
		WHERE ugf.UserID=usr.UserID
		AND ugf.FunctionID=sf.FunctionID
	)
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='0.9'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');
END
ELSE
print ('Incorrect Version: ' + @DBVERSION);



