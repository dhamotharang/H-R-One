--  Insert Position Code
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('ACC','Accountant')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('AO','Administration Officer')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('ANA','Analyst')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('ASS','Assistant')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('CLE','Clerk')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('CON','Consultant')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('ENG','Engineer')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('MAN','Manager')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('MER','Merchandiser')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('PRE','President')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('PRO','Programmer')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('REC','Receptionist')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('SL','Sales')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('SEC','Secretary')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('SUP','Supervisor')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('TRA','Trainee')
INSERT INTO [Position]
           ([PositionCode]
           ,[PositionDesc])
     VALUES
           ('CEO','Chief Executive Officer')

--  Insert Qualification
INSERT INTO [Qualification]
           ([QualificationCode]
           ,[QualificationDesc])
     VALUES
           ('DOC','Doctorate')
INSERT INTO [Qualification]
           ([QualificationCode]
           ,[QualificationDesc])
     VALUES
           ('MAS','Master')
INSERT INTO [Qualification]
           ([QualificationCode]
           ,[QualificationDesc])
     VALUES
           ('DEG','Degree')
INSERT INTO [Qualification]
           ([QualificationCode]
           ,[QualificationDesc])
     VALUES
           ('HDIP','High Diploma')
INSERT INTO [Qualification]
           ([QualificationCode]
           ,[QualificationDesc])
     VALUES
           ('DIP','Diploma')
INSERT INTO [Qualification]
           ([QualificationCode]
           ,[QualificationDesc])
     VALUES
           ('CER','Certificate')
INSERT INTO [Qualification]
           ([QualificationCode]
           ,[QualificationDesc])
     VALUES
           ('ALV','A-Level')
INSERT INTO [Qualification]
           ([QualificationCode]
           ,[QualificationDesc])
     VALUES
           ('OLV','O-Level')
INSERT INTO [Qualification]
           ([QualificationCode]
           ,[QualificationDesc])
     VALUES
           ('PRI','Primary')

		 
--	Insert Rank
INSERT INTO [Rank]
           ([RankCode]
           ,[RankDesc])
     VALUES
           ('JUN','Junior')
INSERT INTO [Rank]
           ([RankCode]
           ,[RankDesc])
     VALUES
           ('GEN','General')
INSERT INTO [Rank]
           ([RankCode]
           ,[RankDesc])
     VALUES
           ('SEN','Senior')
INSERT INTO [Rank]
           ([RankCode]
           ,[RankDesc])
     VALUES
           ('SUP','Supervisor')
INSERT INTO [Rank]
           ([RankCode]
           ,[RankDesc])
     VALUES
           ('MAN','Manager')
INSERT INTO [Rank]
           ([RankCode]
           ,[RankDesc])
     VALUES
           ('DIR','Director')

INSERT INTO USERRANK
	(UserID, RankID)
	SELECT UserID, RankID FROM Users, Rank

--	Insert Skill
INSERT INTO [Skill]
           ([SkillCode]
           ,[SkillDesc])
     VALUES
           ('ENG','English')
INSERT INTO [Skill]
           ([SkillCode]
           ,[SkillDesc])
     VALUES
           ('MAN','Mandarin')
INSERT INTO [Skill]
           ([SkillCode]
           ,[SkillDesc])
     VALUES
           ('JAP','Japanese')
INSERT INTO [Skill]
           ([SkillCode]
           ,[SkillDesc])
     VALUES
           ('COM','Computer')
INSERT INTO [Skill]
           ([SkillCode]
           ,[SkillDesc])
     VALUES
           ('MSW','Microsoft Word')
INSERT INTO [Skill]
           ([SkillCode]
           ,[SkillDesc])
     VALUES
           ('MSE','Microsoft Excel')
INSERT INTO [Skill]
           ([SkillCode]
           ,[SkillDesc])
     VALUES
           ('MGM','Management')
		   
--	Insert Skill Level
INSERT INTO [SkillLevel]
           ([SkillLevelCode]
           ,[SkillLevelDesc])
     VALUES
           ('EXC','Excellent')
INSERT INTO [SkillLevel]
           ([SkillLevelCode]
           ,[SkillLevelDesc])
     VALUES
           ('GD','Good')
INSERT INTO [SkillLevel]
           ([SkillLevelCode]
           ,[SkillLevelDesc])
     VALUES
           ('AVG','Average')
INSERT INTO [SkillLevel]
           ([SkillLevelCode]
           ,[SkillLevelDesc])
     VALUES
           ('BAVG','Below Average')
INSERT INTO [SkillLevel]
           ([SkillLevelCode]
           ,[SkillLevelDesc])
     VALUES
           ('PR','Poor')

--	Insert Staff Type
INSERT INTO [StaffType]
           ([StaffTypeCode]
           ,[StaffTypeDesc])
     VALUES
           ('LO','Local')

--	Insert Cessation Reason

INSERT INTO [CessationReason]
           ([CessationReasonCode]
           ,[CessationReasonDesc]
           ,[CessationReasonIsSeverancePay]
           ,[CessationReasonIsLongServicePay])
     VALUES
           ('DN','Death',0,0)
INSERT INTO [CessationReason]
           ([CessationReasonCode]
           ,[CessationReasonDesc]
           ,[CessationReasonIsSeverancePay]
           ,[CessationReasonIsLongServicePay])
     VALUES
           ('DS','Summary dismissal',0,0)
INSERT INTO [CessationReason]
           ([CessationReasonCode]
           ,[CessationReasonDesc]
           ,[CessationReasonIsSeverancePay]
           ,[CessationReasonIsLongServicePay])
     VALUES
           ('ER','Early retirement',0,0)
INSERT INTO [CessationReason]
           ([CessationReasonCode]
           ,[CessationReasonDesc]
           ,[CessationReasonIsSeverancePay]
           ,[CessationReasonIsLongServicePay])
     VALUES
           ('LR','Late retirement',0,0)
INSERT INTO [CessationReason]
           ([CessationReasonCode]
           ,[CessationReasonDesc]
           ,[CessationReasonIsSeverancePay]
           ,[CessationReasonIsLongServicePay])
     VALUES
           ('NR','Normal retirement',0,0)
INSERT INTO [CessationReason]
           ([CessationReasonCode]
           ,[CessationReasonDesc]
           ,[CessationReasonIsSeverancePay]
           ,[CessationReasonIsLongServicePay])
     VALUES
           ('PC','Termination prior to commencement of contributions',0,0)
INSERT INTO [CessationReason]
           ([CessationReasonCode]
           ,[CessationReasonDesc]
           ,[CessationReasonIsSeverancePay]
           ,[CessationReasonIsLongServicePay])
     VALUES
           ('PD','Permanent departure from Hong Kong',0,0)
INSERT INTO [CessationReason]
           ([CessationReasonCode]
           ,[CessationReasonDesc]
           ,[CessationReasonIsSeverancePay]
           ,[CessationReasonIsLongServicePay])
     VALUES
           ('RE','Redundancy',0,0)
INSERT INTO [CessationReason]
           ([CessationReasonCode]
           ,[CessationReasonDesc]
           ,[CessationReasonIsSeverancePay]
           ,[CessationReasonIsLongServicePay])
     VALUES
           ('TI','Total incapacity',0,0)
INSERT INTO [CessationReason]
           ([CessationReasonCode]
           ,[CessationReasonDesc]
           ,[CessationReasonIsSeverancePay]
           ,[CessationReasonIsLongServicePay])
     VALUES
           ('WB','Resignation',0,0)

INSERT INTO [CessationReason]
           ([CessationReasonCode]
           ,[CessationReasonDesc]
           ,[CessationReasonIsSeverancePay]
           ,[CessationReasonIsLongServicePay])
     VALUES
           ('GO','Member transfer between companies',0,0)

-- Insert Employment Type
INSERT INTO [EmploymentType]
           ([EmploymentTypeCode]
           ,[EmploymentTypeDesc])
     VALUES
           ('CONT','Contract')
INSERT INTO [EmploymentType]
           ([EmploymentTypeCode]
           ,[EmploymentTypeDesc])
     VALUES
           ('PERM','Permanent')
INSERT INTO [EmploymentType]
           ([EmploymentTypeCode]
           ,[EmploymentTypeDesc])
     VALUES
           ('PT','Part Time')
INSERT INTO [EmploymentType]
           ([EmploymentTypeCode]
           ,[EmploymentTypeDesc])
     VALUES
           ('TEMP','Temporary')
INSERT INTO [EmploymentType]
           ([EmploymentTypeCode]
           ,[EmploymentTypeDesc])
     VALUES
           ('TRAIN','Trainee')

-- Insert Payment Code

INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('ACHIEVEMENT','Achievement Bonus',9,0,1,0,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('ALOW','Allowance',9,1,1,1,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('ATTN','Attendance Bonus',9,0,1,0,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('BAS','Basic Salary',1,1,1,1,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('BON','Bonus',9,1,1,1,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('BSADJ','Basic Salary Adjustment',1,0,1,0,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('COMM','Commission',9,0,1,0,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('COMPEN','COMPENSATION',9,0,0,0,0,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('DOUBLEPAY','13th month',1,1,1,1,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('HOA','Housing Allowance',9,0,1,1,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('LDED','Leave Deduction',2,0,1,1,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('LEAVEALLOW','Leave Allowance',3,0,1,1,0,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('LPP','Leave Payment',3,0,1,1,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('LSP','Long Service Payment',9,0,0,0,0,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('MLP','Matermity Leave Pay',3,0,1,1,0,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('FMLP','Full Pay Matermity Leave',3,0,1,1,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('MCEE','Employee Mandatory Contribution',4,0,0,0,0,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('MCER','Employer Mandatory Contribution',5,0,0,0,0,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('NOTEE','Payment in lieu of Notice (Employee)',9,0,0,0,0,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('NOTER','Payment in lieu of Notice (Employer)',9,0,0,0,0,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('NPML','No Pay Maternity Leave',2,0,1,1,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('NPSL','No Pay Sick Leave',2,0,1,1,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('PFUNDEE','Employee P-Fund Contribution',10,0,0,0,0,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('OTHERS','OTHERS',9,0,0,0,0,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('OTP','Overtime Payment',8,0,1,1,0,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('PB','Performance Bonus',9,0,1,0,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('SHD','Statutory Holiday Deduction',2,0,1,0,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('ALD','Annual Leave Deduction',2,0,1,0,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('ALP','Annual Leave Pay',3,0,1,1,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('SLD','Sick Leave Deduction',2,0,1,0,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('FPSL','Full Pay Sick Leave',3,0,1,1,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('SLP','Sick Leave Pay',3,0,1,1,0,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('MLD','Maternity Leave Deduction',2,0,1,0,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('NPLD','No Pay Leave Deduction',2,0,1,0,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('SN','SHORT NOTICE',9,0,0,0,0,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('SP','Severance Payment',9,0,0,0,0,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('SPB','Salary Paid Back',1,0,1,0,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('SHP','Statutory Holiday Pay',3,0,1,1,1,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('VCEE','Employee Voluntary Contribution',6,0,0,0,0,0,2,'TO')
INSERT INTO [PaymentCode]
           ([PaymentCode]
           ,[PaymentCodeDesc]
           ,[PaymentTypeID]
           ,[PaymentCodeIsProrata]
           ,[PaymentCodeIsMPF]
           ,[PaymentCodeIsTopUp]
           ,[PaymentCodeIsWages]
           ,[PaymentCodeIsORSO]
           ,[PaymentCodeDecimalPlace]
           ,[PaymentCodeRoundingRule])
     VALUES
           ('VCER','Employer Voluntary Contribution',7,0,0,0,0,0,2,'TO')

--	Insert Leave Code

INSERT INTO LeaveCode
(LeaveCode,LeaveCodeDesc, LeaveCodePayRatio,LeaveTypeID,LeaveCodeLeaveAllowFormula,LeaveCodeLeaveDeductPaymentCodeID,LeaveCodeLeaveDeductFormula,LeaveCodeLeaveAllowPaymentCodeID)
Select 'AL','Annual Leave',1,LeaveType.LeaveTypeID, A.PayFormID ,PD.PaymentCodeID, D.PayFormID, PA.PaymentCodeID  from PayrollProrataFormula A, PayrollProrataFormula D,PaymentCode PD, PaymentCode PA, LeaveType where LeaveType='ANNUAL' and D.PayFormCode='SYS001' and A.PayFormCode='DAW' and PD.PaymentCode='ALD' and PA.PaymentCode='ALP'

INSERT INTO LeaveCode
(LeaveCode,LeaveCodeDesc, LeaveCodePayRatio,LeaveTypeID,LeaveCodeLeaveAllowFormula,LeaveCodeLeaveDeductPaymentCodeID,LeaveCodeLeaveDeductFormula,LeaveCodeLeaveAllowPaymentCodeID)
Select 'FPSL','Full Paid Sick Leave',1,LeaveType.LeaveTypeID, A.PayFormID ,PD.PaymentCodeID, D.PayFormID, PA.PaymentCodeID  from PayrollProrataFormula A, PayrollProrataFormula D,PaymentCode PD, PaymentCode PA, LeaveType where LeaveType='SLCAT1' and D.PayFormCode='SYS001' and A.PayFormCode='SYS001' and PD.PaymentCode='SLD' and PA.PaymentCode='FPSL'

INSERT INTO LeaveCode
(LeaveCode,LeaveCodeDesc, LeaveCodePayRatio,LeaveTypeID,LeaveCodeLeaveAllowFormula,LeaveCodeLeaveDeductPaymentCodeID,LeaveCodeLeaveDeductFormula,LeaveCodeLeaveAllowPaymentCodeID)
Select 'SLL','Sick Leave In Law (4/5)',0.8,LeaveType.LeaveTypeID, A.PayFormID ,PD.PaymentCodeID, D.PayFormID, PA.PaymentCodeID  from PayrollProrataFormula A, PayrollProrataFormula D,PaymentCode PD, PaymentCode PA, LeaveType where LeaveType='SLCAT1' and D.PayFormCode='SYS001' and A.PayFormCode='DAW' and PD.PaymentCode='SLD' and PA.PaymentCode='SLP'

INSERT INTO LeaveCode
(LeaveCode,LeaveCodeDesc, LeaveCodePayRatio,LeaveTypeID,LeaveCodeLeaveAllowFormula,LeaveCodeLeaveDeductPaymentCodeID,LeaveCodeLeaveDeductFormula,LeaveCodeLeaveAllowPaymentCodeID)
Select 'NPL','No Pay Leave',0.0,LeaveType.LeaveTypeID, A.PayFormID ,PD.PaymentCodeID, D.PayFormID, PA.PaymentCodeID  from PayrollProrataFormula A, PayrollProrataFormula D,PaymentCode PD, PaymentCode PA, LeaveType where LeaveType='NPL' and D.PayFormCode='SYS001' and A.PayFormCode='SYS001' and PD.PaymentCode='NPLD' and PA.PaymentCode='LPP'

INSERT INTO LeaveCode
(LeaveCode,LeaveCodeDesc, LeaveCodePayRatio,LeaveTypeID,LeaveCodeLeaveAllowFormula,LeaveCodeLeaveDeductPaymentCodeID,LeaveCodeLeaveDeductFormula,LeaveCodeLeaveAllowPaymentCodeID)
Select 'FPML','Full Pay Maternity Leave',1,LeaveType.LeaveTypeID, A.PayFormID ,PD.PaymentCodeID, D.PayFormID, PA.PaymentCodeID  from PayrollProrataFormula A, PayrollProrataFormula D,PaymentCode PD, PaymentCode PA, LeaveType where LeaveType='MAL' and D.PayFormCode='SYS001' and A.PayFormCode='SYS001' and PD.PaymentCode='MLD' and PA.PaymentCode='FMLP'


INSERT INTO LeaveCode
(LeaveCode,LeaveCodeDesc, LeaveCodePayRatio,LeaveTypeID,LeaveCodeLeaveAllowFormula,LeaveCodeLeaveDeductPaymentCodeID,LeaveCodeLeaveDeductFormula,LeaveCodeLeaveAllowPaymentCodeID)
Select 'CML','Compensation Leave',1,LeaveType.LeaveTypeID, A.PayFormID ,PD.PaymentCodeID, D.PayFormID, PA.PaymentCodeID  from PayrollProrataFormula A, PayrollProrataFormula D,PaymentCode PD, PaymentCode PA, LeaveType where LeaveType='OL' and D.PayFormCode='SYS001' and A.PayFormCode='SYS001' and PD.PaymentCode='LDED' and PA.PaymentCode='LPP'

INSERT INTO LeaveCode
(LeaveCode,LeaveCodeDesc, LeaveCodePayRatio,LeaveTypeID,LeaveCodeLeaveAllowFormula,LeaveCodeLeaveDeductPaymentCodeID,LeaveCodeLeaveDeductFormula,LeaveCodeLeaveAllowPaymentCodeID)
Select 'MLL','Maternity Leave In Law',0.8,LeaveType.LeaveTypeID, A.PayFormID ,PD.PaymentCodeID, D.PayFormID, PA.PaymentCodeID  from PayrollProrataFormula A, PayrollProrataFormula D,PaymentCode PD, PaymentCode PA, LeaveType where LeaveType='MAL' and D.PayFormCode='SYS001' and A.PayFormCode='DAW' and PD.PaymentCode='MLD' and PA.PaymentCode='MLP'

INSERT INTO LeaveCode
(LeaveCode,LeaveCodeDesc, LeaveCodePayRatio,LeaveTypeID,LeaveCodeLeaveAllowFormula,LeaveCodeLeaveDeductPaymentCodeID,LeaveCodeLeaveDeductFormula,LeaveCodeLeaveAllowPaymentCodeID)
Select 'NPML','No Pay Maternity Leave',0,LeaveType.LeaveTypeID, A.PayFormID ,PD.PaymentCodeID, D.PayFormID, PA.PaymentCodeID  from PayrollProrataFormula A, PayrollProrataFormula D,PaymentCode PD, PaymentCode PA, LeaveType where LeaveType='MAL' and D.PayFormCode='SYS001' and A.PayFormCode='DAW' and PD.PaymentCode='NPML' and PA.PaymentCode='LPP'

INSERT INTO LeaveCode
(LeaveCode,LeaveCodeDesc, LeaveCodePayRatio,LeaveTypeID,LeaveCodeLeaveAllowFormula,LeaveCodeLeaveDeductPaymentCodeID,LeaveCodeLeaveDeductFormula,LeaveCodeLeaveAllowPaymentCodeID)
Select 'NPSL','No Pay Sick Leave',0,LeaveType.LeaveTypeID, A.PayFormID ,PD.PaymentCodeID, D.PayFormID, PA.PaymentCodeID  from PayrollProrataFormula A, PayrollProrataFormula D,PaymentCode PD, PaymentCode PA, LeaveType where LeaveType='NPL' and D.PayFormCode='SYS001' and A.PayFormCode='DAW' and PD.PaymentCode='NPSL' and PA.PaymentCode='LPP'

INSERT INTO LeaveCode
(LeaveCode,LeaveCodeDesc, LeaveCodePayRatio,LeaveTypeID,LeaveCodeLeaveAllowFormula,LeaveCodeLeaveDeductPaymentCodeID,LeaveCodeLeaveDeductFormula,LeaveCodeLeaveAllowPaymentCodeID)
Select 'ML','Marriage Leave',1,LeaveType.LeaveTypeID, A.PayFormID ,PD.PaymentCodeID, D.PayFormID, PA.PaymentCodeID  from PayrollProrataFormula A, PayrollProrataFormula D,PaymentCode PD, PaymentCode PA, LeaveType where LeaveType='OL' and D.PayFormCode='SYS001' and A.PayFormCode='SYS001' and PD.PaymentCode='LDED' and PA.PaymentCode='LPP'

INSERT INTO LeaveCode
(LeaveCode,LeaveCodeDesc, LeaveCodePayRatio,LeaveTypeID,LeaveCodeLeaveAllowFormula,LeaveCodeLeaveDeductPaymentCodeID,LeaveCodeLeaveDeductFormula,LeaveCodeLeaveAllowPaymentCodeID)
Select 'TL','Training Leave',1,LeaveType.LeaveTypeID, A.PayFormID ,PD.PaymentCodeID, D.PayFormID, PA.PaymentCodeID  from PayrollProrataFormula A, PayrollProrataFormula D,PaymentCode PD, PaymentCode PA, LeaveType where LeaveType='OL' and D.PayFormCode='SYS001' and A.PayFormCode='SYS001' and PD.PaymentCode='LDED' and PA.PaymentCode='LPP'

INSERT INTO LeaveCode
(LeaveCode,LeaveCodeDesc, LeaveCodePayRatio,LeaveTypeID,LeaveCodeLeaveAllowFormula,LeaveCodeLeaveDeductPaymentCodeID,LeaveCodeLeaveDeductFormula,LeaveCodeLeaveAllowPaymentCodeID)
Select 'EL','Examination Leave',1,LeaveType.LeaveTypeID, A.PayFormID ,PD.PaymentCodeID, D.PayFormID, PA.PaymentCodeID  from PayrollProrataFormula A, PayrollProrataFormula D,PaymentCode PD, PaymentCode PA, LeaveType where LeaveType='OL' and D.PayFormCode='SYS001' and A.PayFormCode='SYS001' and PD.PaymentCode='LDED' and PA.PaymentCode='LPP'

