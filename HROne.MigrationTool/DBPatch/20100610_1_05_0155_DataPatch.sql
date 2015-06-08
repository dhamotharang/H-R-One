insert into UserCompany (UserID, CompanyID)
select UserID, CompanyID from Users, Company
