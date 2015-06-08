<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_FinalPayment_Edit.aspx.cs" Inherits="Emp_FinalPayment_Edit" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpFinalPayID" runat="server" name="ID" />
        <input type="hidden" id="EmpID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Employee Information" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label Text="Final Payment" runat="server" />
                    :
                </td>
            </tr>
        </table>
        
             
        
        <uc1:Emp_Common ID="Emp_Common1" runat="server" />
        <br />
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
        <asp:AsyncPostBackTrigger ControlID="toolBar" />
    </Triggers>
    <ContentTemplate >
        <table border="0" width="100%" cellspacing="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Payment Code" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="PayCodeID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label5" Text="Final Pay Amount" runat="server" />:
                </td>
                <td class="pm_field" >
                    HK$<asp:TextBox ID="EmpFinalPayAmount" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label Text="Pay Method" runat="server" />:
                </td>
                <td class="pm_field" >
                    <asp:DropDownList ID="EmpFinalPayMethod" runat="Server" AutoPostBack="true"/>
                </td>
            </tr>
            <tr id="BankAccountRow" runat="server">
                <td class="pm_field_header">
                    <asp:Label ID="Label4" Text="Bank Account Number" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="EmpAccID" runat="Server" AutoPostBack="true" />
                    <asp:Label ID="lblDefaultBankAccount" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" Text="Days Adjust" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpFinalPayNumOfDayAdj" runat="Server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label1" Text="Rest Payment" runat="server" />?
                </td>
                <td class="pm_field">
                    <asp:CheckBox ID="EmpFinalPayIsRestDayPayment" runat="Server" />
                </td>
            </tr>
            <tr  id="CostCenterRow" runat="server">
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Cost Center" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="CostCenterID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Remark" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="EmpFinalPayRemark" runat="Server" TextMode="MultiLine" Columns="35" Rows="5" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" Text="Is AutoGen" runat="server" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:Label ID="EmpFinalPayIsAutoGen" runat="Server" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content> 