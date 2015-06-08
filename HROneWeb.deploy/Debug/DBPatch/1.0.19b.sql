
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.19a'
BEGIN
	BEGIN TRANSACTION 

	INSERT INTO [SystemFunction]
           ([FunctionCode]
           ,[Description]
           ,[FunctionCategory]
           ,[FunctionIsHidden])
    VALUES
           ('CST006','Cost Center Export Report','Cost Center', 0);

	UPDATE LeaveType
	SET LeaveTypeIsESSAllowableAdvanceBalance = 0;

	UPDATE PayrollGroup
	SET		PayGroupIsPublic = 0;

	TRUNCATE TABLE PayrollGroupUsers;

	INSERT INTO PayrollGroupUsers
		(PayGroupID, UserID)
	SELECT p.PayGroupID, u.UserID
	FROM	PayrollGroup p, Users u;


	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.19b'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.19b';
	
	COMMIT TRANSACTION
END

