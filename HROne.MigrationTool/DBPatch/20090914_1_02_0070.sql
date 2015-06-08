DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.02.0069' 
BEGIN

Update PaymentCode
set PaymentCodeDisplaySeqNo = 97
where PaymentTypeID in
(Select PaymentTypeID from PaymentType where PaymentTypeCode like 'MPFEE')
and PaymentCodeDisplaySeqNo = 99

Update PaymentCode
set PaymentCodeDisplaySeqNo = 97
where PaymentTypeID in
(Select PaymentTypeID from PaymentType where PaymentTypeCode like 'MPFER')
and PaymentCodeDisplaySeqNo = 99

Update PaymentCode
set PaymentCodeDisplaySeqNo = 98
where PaymentTypeID in
(Select PaymentTypeID from PaymentType where PaymentTypeCode like 'TOPUPEE')
and PaymentCodeDisplaySeqNo = 99

Update PaymentCode
set PaymentCodeDisplaySeqNo = 98
where PaymentTypeID in
(Select PaymentTypeID from PaymentType where PaymentTypeCode like 'TOPUPER')
and PaymentCodeDisplaySeqNo = 99

		           
	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.02.0070'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





