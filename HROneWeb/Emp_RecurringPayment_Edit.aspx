<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_RecurringPayment_Edit.aspx.cs"    Inherits="Emp_RecurringPayment_Edit" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Emp_RecurringPayment_Edit.ascx" TagName="Emp_RecurringPayment_Edit_Control" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Emp_RecurringPayment_List.ascx" TagName="Emp_RecurringPayment_List" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
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
                    <asp:Label ID="ActionHeader" runat="server" Text="Edit" />
                    <asp:Label runat="server" Text="Recurring Payment" />
                </td>
            </tr>
        </table>
        <uc1:Emp_Common id="Emp_Common1" runat="server"></uc1:Emp_Common><br /> 
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
        <uc1:Emp_RecurringPayment_Edit_Control ID="Emp_RecurringPayment_Edit1"  runat="server"  />
        <asp:Panel ID="RecurringPaymentHistoryPanel" runat="server" >
        <br />
        <%-- Start 0000166, KuangWei, 2015-02-03 --%>
       <table id="winson_header" border="0" width="100%" class="pm_section_title" runat="server" visible="false">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Winson Customized Roster Information" />
                </td>
            </tr>
        </table>
        <table id="winson_content" border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section" runat="server" visible="false">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
           <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Shift Duty Code" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="ShiftDutyCode" runat="Server" Visible="true" />
                </td>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Payment Calculation Formula Code" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="PayCalFormulaCode" runat="Server" Visible="true" />
                </td>
            </tr> 
        </table>
       <br /> 
       <%-- End 0000166, KuangWei, 2015-02-03 --%> 
        <table border="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Recurring Payment History" />
                </td>
            </tr>
        </table>
        <uc1:Emp_RecurringPayment_List ID="Emp_RecurringPayment_List1" runat="server" ShowHistory="true" AllowModify="false"  ShowNonPayrollItem="true" ShowPayrollItem="true"/>
        </asp:Panel> 
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content> 