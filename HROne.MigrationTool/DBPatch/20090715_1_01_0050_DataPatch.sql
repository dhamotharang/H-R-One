	Update CostAllocationDetail 
		set CostAllocationDetailIsContribution = 0
		where CostAllocationDetailIsContribution is null
	Update CostAllocationDetail 
		set CostAllocationDetailIsContribution = 1
		where PaymentCodeID in 
		(	Select PaymentCodeID 
			From PaymentCode
			Where PaymentTypeID in
			(	Select PaymentTypeID
				From PaymentType
				Where PaymentTypeCode IN ('MPFER','MPFEE','TOPUPER','TOPUPEE', 'PFUNDER','PFUNDEE')
			)
		)
		AND CostAllocationDetailAmount<0