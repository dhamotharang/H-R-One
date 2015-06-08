
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.30'
BEGIN

	BEGIN TRANSACTION 

	CREATE TABLE ESSAnnouncement(
		ESSAnnouncementID int IDENTITY(1,1) NOT NULL,
		ESSAnnouncementCode nvarchar(255) NOT NULL,
		ESSAnnouncementEffectiveDate datetime NULL,
		ESSAnnouncementExpiryDate datetime NULL,
		ESSAnnouncementContent nvarchar(max) NULL,
		ESSAnnouncementTargetCompanies nvarchar(255) NULL,
		ESSAnnouncementTargetRanks nvarchar(255) NULL,
	 CONSTRAINT PK_ESSAnnouncement PRIMARY KEY CLUSTERED 
	 (
		ESSAnnouncementID ASC
	 )
	);


	INSERT INTO SystemFunction VALUES('TLS001', 'Announcements', 'System', -1);
	

	INSERT INTO SystemFunction VALUES('PER022', 'Employee Final Payment List', 'Personnel', 0);
	

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.30a'
	WHERE	ParameterCode='DBVERSION';
	
	SELECT  @DBVERSION='1.0.30a';
	
	COMMIT TRANSACTION
END

