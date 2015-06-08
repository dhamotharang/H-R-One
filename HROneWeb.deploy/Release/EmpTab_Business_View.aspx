<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmpTab_Business_View.aspx.cs"    Inherits="Emp_Business_View" MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/Emp_RecurringPayment.ascx" TagName="Emp_RecurringPayment"
    TagPrefix="uc4" %>

<%@ Register Src="~/controls/Emp_PositionInfo.ascx" TagName="Emp_PositionInfo" TagPrefix="uc3" %>

<%@ Register Src="~/controls/Emp_PersonalInfo.ascx" TagName="Emp_PersonalInfo" TagPrefix="uc2" %>

<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Emp_LeftMenu.ascx" TagName="Emp_LeftMenu" TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpID" runat="server" name="ID" />
        
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="Employee Information" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Employee" /> : 
                    <%=Emp_Common1.obj.EmpEngSurname %> <%=Emp_Common1.obj.EmpEngOtherName%> 
                    <asp:Label runat="server" Text="Profile" />
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
                                <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                            </td>
                        </tr>
                    </table>
                    <uc1:Emp_Common id="Emp_Common1" runat="server"/>
                    <br />
                    <uc3:Emp_PositionInfo id="Emp_PositionInfo1" runat="server"/>
                    <br />
                    <uc4:Emp_RecurringPayment ID="Emp_RecurringPayment1" runat="server" />
                    
                </td>
            </tr>
        </table>
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td align="right">
                </td>
            </tr>
        </table>
</asp:Content> --%>