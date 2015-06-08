	Update EmpPayroll
		Set EmpPayValueDate = (Select PayBatchValueDate from PayrollBatch pb where pb.PayBatchID=EmpPayroll.PayBatchID and not PayBatchValueDate is null)
		where EmpPayValueDate is Null
