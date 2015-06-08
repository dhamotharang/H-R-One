<%@ control language="C#" autoeventwireup="true" inherits="Misccode_sel_LeftMenu, HROneWeb.deploy" %>

<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_submenu_section">

	<tr>
		<td class="pm_submenu_item">

			<a href="~/EmploymentType.aspx" runat="server"><asp:Label runat="server" Text="Employment Type" /></a>
		</td>
	</tr>
	<tr>
		<td class="pm_submenu_item">
			<a href="~/CessationReason.aspx" runat="server"><asp:Label runat="server" Text="Cessation Reason" /></a>
		</td>
	</tr>
	<tr>
		<td class="pm_submenu_item">
			<a href="~/Position.aspx" runat="server"><asp:Label  runat="server" Text="Position" /></a>
		</td>
	</tr>
	<tr>
		<td class="pm_submenu_item">
			<a href="~/Qualification.aspx" runat="server"><asp:Label  runat="server" Text="Qualification" /></a>
		</td>
	</tr>
	<tr>
		<td class="pm_submenu_item">
			<a href="~/Skill.aspx" runat="server"><asp:Label runat="server" Text="Skill" /></a>
		</td>
	</tr>
	<tr>
		<td class="pm_submenu_item">
			<a href="~/SkillLevel.aspx" runat="server"><asp:Label runat="server" Text="Skill Level" /></a>
		</td>
	</tr>
	<tr>
		<td class="pm_submenu_item">
			<a href="~/Rank.aspx" runat="server"><asp:Label runat="server" Text="Rank" /></a>
		</td>
	</tr>
	<tr>
		<td class="pm_submenu_item">
			<a href="~/StaffType.aspx" runat="server"><asp:Label runat="server" Text="Staff Type" /></a>
		</td>
	</tr>
	<tr runat="server" id="PermitTypePanel">
		<td class="pm_submenu_item">
			<a href="~/PermitType.aspx" runat="server"><asp:Label ID="Label1" runat="server" Text="Permit Type" /></a>
		</td>
	</tr>
	<tr runat="server" id="DocumentTypePanel">
		<td class="pm_submenu_item">
			<a href="~/DocumentType.aspx" runat="server"><asp:Label ID="Label2" runat="server" Text="Document Type" /></a>
		</td>
	</tr>
	<tr runat="server" id="CostCenterPanel">
		<td class="pm_submenu_item">
			<a href="~/CostCenter.aspx" runat="server"><asp:Label ID="lblCostCenter" runat="server" Text="Cost Center" /></a>
		</td>
	</tr>
	<tr runat="server" id="TrainingPanel">
		<td class="pm_submenu_item">
			<a href="~/TrainingCourse.aspx" runat="server"><asp:Label ID="Label3" runat="server" Text="Training Course" /></a>
		</td>
	</tr>
    <%-- Start 0000092, KuangWei, 2014-09-09 --%>
	<tr runat="server" id="PlaceofBirthPanel">
		<td class="pm_submenu_item">
			<a href="~/PlaceofBirth.aspx" runat="server"><asp:Label runat="server" Text="Place of Birth" /></a>
		</td>
	</tr>
	<tr runat="server" id="CountryofIssuePanel">
		<td class="pm_submenu_item">
			<a href="~/CountryofIssue.aspx" runat="server"><asp:Label runat="server" Text="Country of Issue" /></a>
		</td>
	</tr>
	<tr runat="server" id="NationalityPanel">
		<td class="pm_submenu_item">
			<a href="~/Nationality.aspx" runat="server"><asp:Label runat="server" Text="Nationality" /></a>
		</td>
	</tr>		    
    <%-- End 0000092, KuangWei, 2014-09-09 --%>
</table>
