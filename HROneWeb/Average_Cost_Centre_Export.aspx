<%@ Page Language="C#" MasterPageFile="~/MainMasterPage.master" AutoEventWireup="true" CodeFile="Average_Cost_Centre_Export.aspx.cs" Inherits="Average_Cost_Centre_Export" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%-- Start 0000185, KuangWei, 2015-04-21 --%>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
<%-- End 0000185, KuangWei, 2015-04-21 --%>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Average Cost Centre Export" />
                </td>
            </tr>
        </table>
        <br />

        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Report Parameters" />
                </td>
            </tr>
        </table>
        <table width="100%" class="pm_section">
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" Text="Report Period (Year/Month)" />:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="Year" runat="server" MaxLength="4" Width="50" /> <%-- AutoPostBack="True" OnTextChanged="DateRange_Changed" /> --%>
                    <asp:DropDownList ID="Month" runat="server" /> <%-- AutoPostBack="True" OnSelectedIndexChanged="DateRange_Changed" /> --%>
                    &nbsp &nbsp &nbsp &nbsp to:&nbsp &nbsp &nbsp &nbsp     
                    <asp:TextBox ID="Year2" runat="server" MaxLength="4" Width="50" /> <%-- AutoPostBack="True" OnTextChanged="DateRange_Changed" />--%>
                    <asp:DropDownList ID="Month2" runat="server" /> <%-- AutoPostBack="True" OnSelectedIndexChanged="DateRange_Changed" />--%> 
<%-- 
                  <asp:DropDownList ID="Step1ChooseTemplateD" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DateRange_Changed" />                                                
--%>
                </td>                
            </tr>
        </table>
        <br />

        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Employee Filter" />
                </td>
            </tr>
        </table>
        
       <%-- Start 0000185, KuangWei, 2015-04-21 --%>
        <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" >
            <ContentTemplate >
                <uc2:EmployeeSearchControl id="EmployeeSearchControl1" runat="server" />
            </ContentTemplate >
        </asp:UpdatePanel>
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                    <asp:Button ID="Reset" runat="server" Text="Reset" CssClass="button" OnClick="Reset_Click" />
                </td>
            </tr>
        </table>    
        <%-- End 0000185, KuangWei, 2015-04-21 --%>
                
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Year" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="Month" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="Year2" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="Month2" EventName="SelectedIndexChanged" />        
            <asp:PostBackTrigger ControlID="btnExport" />
            <asp:PostBackTrigger ControlID="btnExport2" />
        </Triggers>
        <ContentTemplate>
        <br />
        <asp:Panel ID="panelPayPeriod" runat="server">
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
                            <asp:button ID="btnExport2" runat="server" Text="Generate" CssClass="button" OnClick="btnExport_Click" />
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
                                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "EmpTab_CostCenter_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID"))%>"><%#sbinding.getValue(Container.DataItem, "EmpNo")%></a>
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
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content>

