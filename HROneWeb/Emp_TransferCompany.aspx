<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_TransferCompany.aspx.cs" Inherits="Emp_TransferCompany"  MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpPosID" runat="server" name="ID" />
        <input type="hidden" id="EmpID" runat="server" name="ID" />
        <input type="hidden" id="Flow" runat="server" name="ID" />
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
                    <asp:Label ID="ActionHeader" runat="server" Text="New" />
                    <asp:Label ID="Label2" Text="Position Information" runat="server" />
                </td>
            </tr>
        </table>
        
             
        
        <uc1:Emp_Common ID="Emp_Common1" runat="server"></uc1:Emp_Common>
        <br />
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                </td>            
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolBar" />
        </Triggers>
        <ContentTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <col width="20%" />
            <col width="30%" />
            <col width="50%" />
            <tr>
                <td>
                </td>
                <td class="pm_field_header">
                    <asp:Label Text="Old" runat="server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel11" Text="New" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel4" Text="EmpNo" runat="server" />
                    :</td>
                <td class="pm_field">
                    <asp:Label ID="OldEmpNo" runat="Server" /></td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpNo" runat="Server" /></td>
            </tr>            
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel3" Text="Company" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="OldCompanyID" runat="Server" />
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="CompanyID" runat="Server" AutoPostBack="true" OnSelectedIndexChanged="CompanyID_SelectedIndexChanged"/>
                </td>
            </tr>
            <asp:Repeater ID="HierarchyLevel" runat="server" OnItemDataBound="HierarchyLevel_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field_header">
                            <%#((HROne.Lib.Entities.EHierarchyLevel )Container.DataItem).HLevelDesc%>
                        </td>
                        <td class="pm_field">
                            <asp:Label ID="OldHElementID" runat="server" />
                        </td>
                        <td class="pm_field">
                            <asp:DropDownList ID="HElementID" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel6" Text="Position" runat="server" />
                    :</td>
                <td class="pm_field">
                    <asp:Label ID="OldPositionID" runat="Server" /></td>
                <td class="pm_field">
                    <asp:DropDownList ID="PositionID" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel7" Text="Rank" runat="server" />
                    :</td>
                <td class="pm_field">
                    <asp:Label ID="OldRankID" runat="Server" /></td>
                <td class="pm_field">
                    <asp:DropDownList ID="RankID" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel8" Text="Staff Type" runat="server" />
                    :</td>
                <td class="pm_field">
                    <asp:Label ID="OldStaffTypeID" runat="Server" /></td>
                <td class="pm_field">
                    <asp:DropDownList ID="StaffTypeID" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel9" Text="Payroll Group" runat="server" />
                    :</td>
                <td class="pm_field">
                    <asp:Label ID="OldPayGroupID" runat="Server" /></td>
                <td class="pm_field">
                    <asp:DropDownList ID="PayGroupID" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel10" Text="Leave Plan" runat="server" />
                    :</td>
                <td class="pm_field">
                    <asp:Label ID="OldLeavePlanID" runat="Server" /></td>
                <td class="pm_field">
                    <asp:DropDownList ID="LeavePlanID" runat="Server" /></td>
            </tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     EditButton_Visible="false" 
                     BackButton_Visible="false"
                     OnSaveButton_Click ="Save_Click"
                     DeleteButton_Visible="false"
                      />
                </td>            
            </tr>
        </table>
</asp:Content> 