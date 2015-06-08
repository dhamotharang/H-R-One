
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION = '1.0.26a'
BEGIN

	BEGIN TRANSACTION 

	IF NOT EXISTS(	SELECT * FROM sys.indexes 
					WHERE name='IX_CostAllocationDetailHElement_CostAllocationDetailID' AND object_id = OBJECT_ID('CostAllocationDetailHElement')) 
	BEGIN
		CREATE NONCLUSTERED INDEX IX_CostAllocationDetailHElement_CostAllocationDetailID ON dbo.CostAllocationDetailHElement 
		(
			CostAllocationDetailID ASC
		)
	END

	-- Insert version of Database --
	UPDATE	SystemParameter 
	SET		ParameterValue='1.0.27'
	WHERE	ParameterCode='DBVERSION';	

	SELECT  @DBVERSION='1.0.27';
	
	COMMIT TRANSACTION
END

