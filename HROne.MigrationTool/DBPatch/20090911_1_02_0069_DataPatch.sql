Update PaymentCode
set PaymentCodeDisplaySeqNo = 1
where PaymentTypeID in
(Select PaymentTypeID from PaymentType where PaymentTypeCode='BASICSAL')
and PaymentCodeDisplaySeqNo is NULL

Update PaymentCode
set PaymentCodeDisplaySeqNo = 10
where PaymentTypeID in
(Select PaymentTypeID from PaymentType where PaymentTypeCode='LEAVEDEDUCT')
and PaymentCodeDisplaySeqNo is NULL

Update PaymentCode
set PaymentCodeDisplaySeqNo = 20
where PaymentTypeID in
(Select PaymentTypeID from PaymentType where PaymentTypeCode='LEAVEALLOW')
and PaymentCodeDisplaySeqNo is NULL

Update PaymentCode
set PaymentCodeDisplaySeqNo = 30
where PaymentTypeID in
(Select PaymentTypeID from PaymentType where PaymentTypeCode='OTPAY')
and PaymentCodeDisplaySeqNo is NULL

Update PaymentCode
set PaymentCodeDisplaySeqNo = 40
where PaymentTypeID in
(Select PaymentTypeID from PaymentType where PaymentTypeCode='LSPSP')
and PaymentCodeDisplaySeqNo is NULL

Update PaymentCode
set PaymentCodeDisplaySeqNo = 50
where PaymentTypeID in
(Select PaymentTypeID from PaymentType where PaymentTypeCode='OTHERS')
and PaymentCodeDisplaySeqNo is NULL

Update PaymentCode
set PaymentCodeDisplaySeqNo = 99
where PaymentTypeID in
(Select PaymentTypeID from PaymentType where PaymentTypeCode like '%EE')
and PaymentCodeDisplaySeqNo is NULL

Update PaymentCode
set PaymentCodeDisplaySeqNo = 99
where PaymentTypeID in
(Select PaymentTypeID from PaymentType where PaymentTypeCode like '%ER')
and PaymentCodeDisplaySeqNo is NULL

Update AttendancePlan
set AttendancePlanOTMinsRoundingRule='DOWN'
where AttendancePlanOTMinsRoundingRule is null
Update AttendancePlan
set AttendancePlanLateMinsRoundingRule='DOWN'
where AttendancePlanLateMinsRoundingRule is null