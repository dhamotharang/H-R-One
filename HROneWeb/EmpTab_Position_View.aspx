<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmpTab_Position_View.aspx.cs"    Inherits="EmpTab_Position_View"  MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/Emp_RecurringPayment_List.ascx" TagName="Emp_RecurringPayment_List"
    TagPrefix="uc3" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Emp_LeftMenu.ascx" TagName="Emp_LeftMenu" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Emp_PositionInfo_List.ascx" TagName="Emp_PositionInfo_List"
    TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="Employee Information" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label Text="View Employee Profile" runat="server" />:
                    <%=Emp_Common1.obj.EmpEngSurname %>
                    <%=Emp_Common1.obj.EmpEngOtherName%>
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" class="pm_section" cellpadding="0" cellspacing="0">
            <col width="10%" />
            <tr>
                <td valign="top" align="left" >
                    <uc1:Emp_LeftMenu ID="Emp_LeftMenu2" runat="server" />
                </td>
                <td valign="top">
                    <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
                        <tr>
                            <td>
                                <asp:Button ID="Back" runat="server" Text="- Back -" OnClick="Back_Click" UseSubmitBehavior="false"
                                    cssclass="button" />
                            </td>
                            <td align="right">
                                <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                            </td>
                            <!-- Start 0000173, Miranda, 2015-04-26 -->
                            <td class="pm_search" align="right">
                                <asp:TextBox runat="server" ID="EmpNo" />
                                <asp:Button ID="Jump" runat="server" Text="Jump" CssClass="button" OnClick="Jump_Click" />
                            </td>
                            <!-- End 0000173, Miranda, 2015-04-26 -->
                        </tr>
                    </table>
                    <uc1:Emp_Common ID="Emp_Common1" runat="server"></uc1:Emp_Common>
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                    <ContentTemplate >
                    <uc1:Emp_PositionInfo_List ID="Emp_PositionInfo_List" runat="server" />
                    <br />
                    <table border="0" width="100%" class="pm_section_title">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Recurring Payment"/>
                            </td>
                        </tr>
                    </table>
                    <uc3:Emp_RecurringPayment_List ID="Emp_RecurringPayment_List1" runat="server" ShowHistory="true" ShowNonPayrollItem="false" ShowPayrollItem="true"/>
                    <br />
                    <table border="0" width="100%" class="pm_section_title">
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Non-payroll Item"/>
                            </td>
                        </tr>
                    </table>
                    <uc3:Emp_RecurringPayment_List ID="Emp_RecurringPayment_List2" runat="server" ShowHistory="true" ShowNonPayrollItem="true" ShowPayrollItem="false"  />
                    </ContentTemplate> 
                    </asp:UpdatePanel> 
                </td>
            </tr>
        </table>
</asp:Content> 