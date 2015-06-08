<%@ page language="C#" autoeventwireup="true" inherits="EmpTab_Pension_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/Emp_MPFPlan_List.ascx" TagName="Emp_MPFPlan_List" TagPrefix="uc4" %>
<%@ Register Src="~/controls/Emp_AVCPlan_List.ascx" TagName="Emp_AVCPlan_List" TagPrefix="uc5" %>
<%@ Register Src="~/controls/Emp_ORSOPlan_List.ascx" TagName="Emp_ORSOPlan_List" TagPrefix="uc6" %>
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
                    <asp:Label Text="View Employee Profile" runat="server" />:
                    <%=Emp_Common1.obj.EmpEngSurname %>
                    <%=Emp_Common1.obj.EmpEngOtherName%>
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" class="pm_section">
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
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                        <tr>
                            <td>
                                <asp:Label Text="MPF Plan" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <uc4:Emp_MPFPlan_List ID="Emp_MPFPlan_List1" runat="server" />
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                        <tr>
                            <td>
                                <asp:Label Text="AVC Plan" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <uc5:Emp_AVCPlan_List ID="Emp_AVCPlan_List1" runat="server" />
                    <br />
                    <asp:Panel ID="EmpORSOPanel" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                            <tr>
                                <td>
                                    <asp:Label Text="P-Fund Plan" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <uc6:Emp_ORSOPlan_List ID="EmpORSOPlanList" runat="server" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
</asp:Content> 