<%@ page language="C#" autoeventwireup="true" inherits="AVCPlan_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="AVCPlanID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" Text="AVC Plan Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label Text="AVC Plan " runat="server" />:
                    <%=AVCPlanCode.Text %>
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     EditButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnSaveButton_Click ="Save_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate> 
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
    		<tr>
                <td class="pm_field_title"  colspan="2">
                   <asp:Label runat="server" Text="Additional Voluntary Contribution Plan" /></td>
			</tr>
			<tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Plan Code" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="AVCPlanCode" runat="Server"/>
                </td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label Text="Description" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="AVCPlanDesc" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Use residual for employer" />:
                </td>
                <td class="pm_field">
                    <!-- Start 0000106, Ricky So, 2014/10/22 -->
                   <asp:CheckBox ID="AVCPlanEmployerResidual" runat="server" /> &nbsp &nbsp &nbsp &nbsp Cap:
                   <asp:TextBox ID="AVCPlanEmployerResidualCap" runat="server" />
                    <!-- End 0000106, Ricky So, 2014/10/22 -->
                </td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Use residual for employee" />:
                </td>
                <td class="pm_field">
                    <!-- Start 0000106, Ricky So, 2014/10/22 -->
                    <asp:CheckBox ID="AVCPlanEmployeeResidual" runat="server" /> &nbsp &nbsp &nbsp &nbsp Cap:
                    <asp:TextBox ID="AVCPlanEmployeeResidualCap" runat="server" />
                    <!-- End 0000106, Ricky So, 2014/10/22 -->
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Employee AVC exemption period the same as MPF" />:
                </td>
                <td class="pm_field">
                     <asp:CheckBox ID="AVCPlanUseMPFExemption" runat="server" />   
                </td>
			</tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Start Contribute on Date of Join" />:
                </td>
                <td class="pm_field">
                    <asp:CheckBox ID="AVCPlanJoinDateStart" runat="server" />  
                </td>
			</tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Contribute for age over MPF maximum age limit" />:
                </td>
                <td class="pm_field">
                    <asp:CheckBox ID="AVCPlanContributeMaxAge" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Contribute for income less than MPF minimum relevant income" />:
                </td>
                <td class="pm_field">
					<asp:CheckBox ID="AVCPlanContributeMinRI" runat="server" /> 
                </td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Max. employer's voluntary contribution" />
                </td>
                <td class="pm_field">
					<asp:TextBox ID="AVCPlanMaxEmployerVC" runat="server" />
                </td>
            </tr>
			<tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Max. employee's voluntary contribution" />
                </td>
                <td class="pm_field">
					<asp:TextBox ID="AVCPlanMaxEmployeeVC" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel6" runat="server" Text="Rounding Rule for Employer Contribution" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="AVCPlanEmployerRoundingRule" runat="server" />
                    <asp:DropDownList ID="AVCPlanEmployerDecimalPlace" runat="server" />
                    <asp:Label ID="ILabel7" runat="server" Text="decimal place(s)" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel8" runat="server" Text="Rounding Rule for Employee Contribution" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="AVCPlanEmployeeRoundingRule" runat="server" />
                    <asp:DropDownList ID="AVCPlanEmployeeDecimalPlace" runat="server" />
                    <asp:Label ID="ILabel9" runat="server" Text="decimal place(s)" />
                </td>
            </tr>
            <tr id="NotRemoveContributionFromTopUpRow" runat="server" >                
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" Text="Do not deduct MPF Only payment being contributed" />:<br />
                    <asp:Label ID="Label5" runat="server" Text="(Required to define which payment is contributed after MPF Only)" />
                </td>
                <td class="pm_field">
                    <asp:CheckBox ID="AVCPlanNotRemoveContributionFromTopUp" runat="server" />
                </td>
            </tr>
		</table>

        <br />
        
        <asp:PlaceHolder ID="PaymentCeilingSection" runat="server" Visible="true" >
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label3" Text="Payment Setting" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" /> 
            <col width="200px" /> 
            <col width="400px" /> 
            <tr>
                <td class="pm_list_header">
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PaymentCode" Text="Payment Code" OnClick="ChangeOrder_Click" />
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PaymentCodeDesc" Text="Description" OnClick="ChangeOrder_Click" />
                </td>
                <td align="center" class="pm_list_header">
                    <asp:Label runat="server" ID="_AVCPlanPaymentCeiling" Text="Ceiling Amount" />
                </td>
                <td align="center" class="pm_list_header">
                    <asp:Label runat="server" ID="Label2" Text="Contributed after MPF Only" />
                </td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" Visible="false" />
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem, "PaymentCode")%>
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem, "PaymentCodeDesc")%>
                        </td>
                        <td class="pm_list" align="center">
                            <asp:TextBox ID="AVCPlanPaymentCeilingAmount" runat="server" />
                        </td>
                        <td class="pm_list" align="right">
                            <asp:CheckBox ID="AVCPlanPaymentConsiderAfterMPF" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <tb:RecordListFooter id="ListFooter" runat="server"
            ShowAllRecords="true" 
          />
    </asp:PlaceHolder>
    </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content> 