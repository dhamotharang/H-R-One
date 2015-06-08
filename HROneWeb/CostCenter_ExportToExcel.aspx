<%@ Page Language="C#" MasterPageFile="~/MainMasterPage.master" AutoEventWireup="true" CodeFile="CostCenter_ExportToExcel.aspx.cs" Inherits="CostCenter_ExportToExcel" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Cost Center Export Report" />
            </td>
        </tr>
    </table>
    
<%--    <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
        </Triggers>
        <ContentTemplate >--%>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td class="pm_field_header">
                        <asp:Label ID="Label3" runat="server" Text="Year/Month"  />:
                    </td>
                    <td class="pm_field">
                        <asp:TextBox ID="Year" runat="server" AutoPostBack="false" MaxLength="4" Columns="4" />
                        <asp:DropDownList ID="Month" runat="server" AutoPostBack="false" >
                            <asp:ListItem Text="No Selected" Value="" />
                            <asp:ListItem Text="January" Value="1" />
                            <asp:ListItem Text="February" Value="2" />
                            <asp:ListItem Text="March" Value="3" />
                            <asp:ListItem Text="April" Value="4" />
                            <asp:ListItem Text="May" Value="5" />
                            <asp:ListItem Text="June" Value="6" />
                            <asp:ListItem Text="July" Value="7" />
                            <asp:ListItem Text="August" Value="8" />
                            <asp:ListItem Text="September" Value="9" />
                            <asp:ListItem Text="October" Value="10" />
                            <asp:ListItem Text="November" Value="11" />
                            <asp:ListItem Text="December" Value="12" />
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <uc2:EmployeeSearchControl id="EmployeeSearchControl1" runat="server" EmpStatusValue="A" />

<%--        </ContentTemplate>
    </asp:UpdatePanel>
--%>
    
<%--    <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExport1" />
        <asp:PostBackTrigger ControlID="btnExport" />
    </Triggers>
      
    <ContentTemplate>
--%>
    <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
        <tr>
            <td>
                <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                <asp:Button ID="Reset" runat="server" Text="Reset" CssClass="button" OnClick="Reset_Click" />
            </td>
        </tr>
    </table>
    
    <asp:Panel ID="panelPayPeriod" runat="server" Visible="false">
        <asp:Panel ID="panelCostAllocationAdjustmentDetail" runat="server">
            
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Employee List" />
                    </td>
                </tr>
            </table>
            <table class="pm_section" width="100%">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                    </td>
                </tr>
            </table>
            
            <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
                <tr>
                    <td>
                        <asp:button ID="btnExport1" runat="server" Text="Generate" CssClass="button" OnClick="btnExport1_Click" />
                    </td>
                </tr>
            </table>

            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="26px" />
                <col width="150px" />
                <colgroup width="350px" >
                    <col width="150px" />
                    <col />
                </colgroup> 
                <col width="150px" />
                <col width="100px" />
                <tr>
                    <td class="pm_list_header" align="center">
                        <asp:Panel ID="ConfirmPayrollSelectAllPanel" runat="server">
                            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                        </asp:Panel>
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" />
                    </td>
                    <td align="left" class="pm_list_header" colspan="2">
                        <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" />
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" />
                    </td>
                </tr>
                <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list" align="center">
                                <asp:CheckBox ID="ItemSelect" runat="server" Checked="True" />
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "EmpNo")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "EmpEngSurname")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "EmpEngOtherName")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "EmpAlias")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
                <tr>
                    <td>
                        <asp:button ID="btnExport" runat="server" Text="Generate" CssClass="button" OnClick="btnExport_Click" />
                    </td>

                    <td align="right">
                        <tb:RecordListFooter id="ListFooter" runat="server"
                            ShowAllRecords="true" visible="true" 
                          />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
<%--    </ContentTemplate> 
    </asp:UpdatePanel> 
--%></asp:Content>

