update PayrollGroup
set
PayGroupTerminatedALCompensationDailyFormula = (Select PayFormID from PayrollProrataFormula where PayFormCode='DAW'),
PayGroupTerminatedALCompensationPaymentCodeID = (Select Top 1 LeaveCodeLeaveAllowFormula from LeaveCode where LeaveTypeID=(Select LeaveTypeID from LeaveType where LeaveType='ANNUAL')),
PayGroupTerminatedPaymentInLieuMonthlyBaseMethod ='MAW',
PayGroupTerminatedPaymentInLieuDailyFormula = (Select PayFormID from PayrollProrataFormula where PayFormCode='DAW'),
PayGroupTerminatedPaymentInLieuERPaymentCodeID = (Select  Top 1 PaymentCodeID from PaymentCode where PaymentCode='PAYINLIEU'),
PayGroupTerminatedPaymentInLieuEEPaymentCodeID = (Select  Top 1 PaymentCodeID from PaymentCode where PaymentCode='PAYINLIEU'),
PayGroupTerminatedLSPSPMonthlyBaseMethod  ='MAW',
PayGroupTerminatedLSPPaymentCodeID  = (Select Top 1 PaymentCodeID from PaymentCode where PaymentCode='LSP'),
PayGroupTerminatedSPPaymentCodeID = (Select  Top 1 PaymentCodeID from PaymentCode where PaymentCode='SP')
where PayGroupTerminatedALCompensationDailyFormula is NULL
AND PayGroupTerminatedALCompensationPaymentCodeID is NULL
AND PayGroupTerminatedPaymentInLieuMonthlyBaseMethod is NULL
AND PayGroupTerminatedPaymentInLieuDailyFormula is NULL
AND PayGroupTerminatedPaymentInLieuERPaymentCodeID is NULL
AND PayGroupTerminatedPaymentInLieuEEPaymentCodeID is NULL
AND PayGroupTerminatedLSPSPMonthlyBaseMethod  is NULL
AND PayGroupTerminatedLSPPaymentCodeID  is NULL
AND PayGroupTerminatedSPPaymentCodeID is NULL