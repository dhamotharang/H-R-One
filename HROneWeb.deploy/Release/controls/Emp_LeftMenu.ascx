<%@ control language="C#" autoeventwireup="true" inherits="Emp_LeftMenu, HROneWeb.deploy" enableviewstate="false" %>
<input type="hidden" id="EmpID" runat="server" name="ID" />
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_submenu_section">
  <tr>
    <td class="pm_submenu_item">
      <a href="~/Emp_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label runat="server" Text="Personal Information" /></a>
    </td>
  </tr>
  <tr>
    <td class="pm_submenu_item">
      <a href="~/EmpTab_BankAccount_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label runat="server" Text="Bank Account" /></a>
    </td>
  </tr>
  <tr>
    <td class="pm_submenu_item">
      <a href="~/EmpTab_Family_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label runat="server" Text="Spouse/Dependant" /></a>
    </td>
  </tr>
  <tr>
    <td class="pm_submenu_item">
      <a href="~/EmpTab_EmergencyContact_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label ID="Label4" runat="server" Text="Emergency Contact" /></a>
    </td>
  </tr>
  <tr>
    <td class="pm_submenu_item">
      <a href="~/EmpTab_SkillSet_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label runat="server" Text="Qualification/Skills" /></a>
    </td>
  </tr>
  <tr runat="server" id="PermitPanel">
    <td class="pm_submenu_item">
      <a href="~/EmpTab_Permit_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label ID="Label1" runat="server" Text="Work Permit/License" /></a>
    </td>
  </tr>
  <tr>
    <td class="pm_submenu_item">
      <a href="~/EmpTab_WorkingExperience_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label ID="Label3" runat="server" Text="Working Experience" /></a>
    </td>
  </tr>
  <tr>
    <td class="pm_submenu_item">
      <a href="~/EmpTab_Residence_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label runat="server" Text="Accommodation" /></a>
    </td>
  </tr>
  <tr runat="server" id="ContractPanel">
    <td class="pm_submenu_item">
      <a href="~/EmpTab_Contract_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label runat="server" Text="Contracts" /></a>
    </td>
  </tr>
  <tr runat="server" id="DocumentPanel">
    <td class="pm_submenu_item">
      <a href="~/EmpTab_Document_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label ID="Label2" runat="server" Text="Document" /></a>
    </td>
  </tr>
  <%-- Start 0000070, Miranda, 2014-08-27 --%>
  <tr runat="server" id="BenefitPanel">
    <td class="pm_submenu_item">
      <a id="A2" href="~/EmpTab_Benefit_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label ID="Label9" runat="server" Text="Benefit" /></a>
    </td>
  </tr>
  <tr runat="server" id="BeneficiariesPanel">
    <td class="pm_submenu_item">
      <a id="A3" href="~/EmpTab_Beneficiaries_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label ID="Label10" runat="server" Text="Beneficiaries" /></a>
    </td>
  </tr>
  <%-- End 0000070, Miranda, 2014-08-27 --%>
<%--  <tr>
    <td class="pm_submenu_item">
      <a href="~/EmpTab_Business_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label runat="server" Text="Current Status" />
      </a>
    </td>
  </tr>
--%>
  <tr>
    <td class="pm_submenu_item">
      <a href="~/EmpTab_Position_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label runat="server" Text="Position History" /></a>
    </td>
  </tr>
  <tr>
    <td class="pm_submenu_item">
      <a href="~/EmpTab_Pension_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label runat="server" Text="Pension History" /></a>
    </td>
  </tr>
  <tr runat="server" id="RosterTableGroupPanel">
    <td class="pm_submenu_item">
      <a id="A1" href="~/EmpTab_RosterTableGroup_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label ID="Label8" runat="server" Text="Roster Table Group" /></a>
    </td>
  </tr>
  <tr runat="server" id="CostCenterPanel">
    <td class="pm_submenu_item">
      <a href="~/EmpTab_CostCenter_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label ID="lblCostCenter" runat="server" Text="Cost Center" /></a>
    </td>
  </tr>
  <tr runat="server" id="TrainingPanel">
    <td class="pm_submenu_item">
      <a href="~/EmpTab_TrainingEnroll_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label ID="Label5" runat="server" Text="Training Record" /></a>
    </td>
  </tr>
  <tr>
    <td class="pm_submenu_item">
      <a href="~/EmpTab_Termination_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label runat="server" Text="Termination" /></a>
    </td>
  </tr>
	<tr>
		<td class="pm_submenu_item">
			<a href="~/EmpTab_LeaveApplication_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label runat="server" Text="Leave Applications &amp; History" /></a>
		</td>
	</tr>
	<tr runat="server" id="CompensationLeavePanel">
		<td class="pm_submenu_item">
			<a href="~/EmpTab_CompensationLeaveEntitle_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label ID="Label7" runat="server" Text="Compensation Leave Entitlement" /></a>
		</td>
	</tr>
	<tr>
		<td class="pm_submenu_item">
			<a href="~/EmpTab_LeaveBalance_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label runat="server" Text="Leave Balance" /></a>
		</td>
	</tr>
	<tr>
		<td class="pm_submenu_item">
			<a href="~/EmpTab_WorkInjury_View.aspx?EmpID=%EmpID%" runat="server" ><asp:Label ID="Label6" runat="server" Text="Injury Record" /></a>
		</td>
	</tr>
</table>
