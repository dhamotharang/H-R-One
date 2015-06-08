SET IDENTITY_INSERT PaymentType ON

INSERT INTO [PaymentType] ([PaymentTypeID],[PaymentTypeCode],[PaymentTypeDesc])VALUES(1,'BASICSAL','Basic Salary')
INSERT INTO [PaymentType] ([PaymentTypeID],[PaymentTypeCode],[PaymentTypeDesc])VALUES(2,'LEAVEDEDUCT','Leave Deduction')
INSERT INTO [PaymentType] ([PaymentTypeID],[PaymentTypeCode],[PaymentTypeDesc])VALUES(3,'LEAVEALLOW','Leave Allowance')
INSERT INTO [PaymentType] ([PaymentTypeID],[PaymentTypeCode],[PaymentTypeDesc])VALUES(4,'MPFMCEE','Employee Mandatory Contribution')
INSERT INTO [PaymentType] ([PaymentTypeID],[PaymentTypeCode],[PaymentTypeDesc])VALUES(5,'MPFMCER','Employer Mandatory Contribution')
INSERT INTO [PaymentType] ([PaymentTypeID],[PaymentTypeCode],[PaymentTypeDesc])VALUES(6,'MPFVCEE','Employee Voluntary Contribution')
INSERT INTO [PaymentType] ([PaymentTypeID],[PaymentTypeCode],[PaymentTypeDesc])VALUES(7,'MPFVCER','Employer Voluntary Contribution')
INSERT INTO [PaymentType] ([PaymentTypeID],[PaymentTypeCode],[PaymentTypeDesc])VALUES(8,'OTPAY','Overtime Pay')
INSERT INTO [PaymentType] ([PaymentTypeID],[PaymentTypeCode],[PaymentTypeDesc])VALUES(9,'OTHERS','Other Payment')
INSERT INTO [PaymentType] ([PaymentTypeID],[PaymentTypeCode],[PaymentTypeDesc])VALUES(10,'PFUNDEE','Employee P-Fund Contribution')
INSERT INTO [PaymentType] ([PaymentTypeID],[PaymentTypeCode],[PaymentTypeDesc])VALUES(11,'PFUNDER','Employer P-Fund Contribution')
INSERT INTO [PaymentType] ([PaymentTypeID],[PaymentTypeCode],[PaymentTypeDesc])VALUES(12,'LSPSP','Long Service Payment / Severance Payment')
INSERT INTO [PaymentType] ([PaymentTypeID],[PaymentTypeCode],[PaymentTypeDesc])VALUES(13,'BONUS','Bonus')
INSERT INTO [PaymentType] ([PaymentTypeID],[PaymentTypeCode],[PaymentTypeDesc])VALUES(14,'COMMISSION','Commission')

SET IDENTITY_INSERT PaymentType OFF