<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmpTab_LeaveBalanceAdjustment_View.aspx.cs" Inherits="EmpTab_LeaveBalanceAdjustment_View"  MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/Emp_LeaveBalanceAdjustment_List.ascx" TagName="Emp_LeaveBalanceAdjustment_List" TagPrefix="uc4" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Emp_LeftMenu.ascx" TagName="Emp_LeftMenu" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
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
                    <asp:Label Text="Employee Profile" runat="server" />:
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
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="right">
                                <asp:Button ID="Back" runat="server" Text="- Back -" OnClick="Back_Click" UseSubmitBehavior="false"
                                    cssclass="button" />
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                            </td>
                        </tr>
                    </table>
                    <uc1:Emp_Common ID="Emp_Common1" runat="server" />
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                        <tr>
                            <td>
                                <asp:Label Text="Leave Adjustment" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <uc4:Emp_LeaveBalanceAdjustment_List ID="Emp_LeaveBalanceAdjustment_List1" runat="server"></uc4:Emp_LeaveBalanceAdjustment_List>
                </td>
            </tr>
        </table>
</asp:Content> 