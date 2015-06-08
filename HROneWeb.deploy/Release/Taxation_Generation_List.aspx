<%@ page language="C#" autoeventwireup="true" inherits="Taxation_Generation_List, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">

		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Generate Taxation Records" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Generate Taxation Records" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="TaxFormUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TaxCompID" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="TaxFormType" EventName="SelectedIndexChanged" />
        </Triggers>

        <ContentTemplate >
        <table width="100%" class="pm_section">
            <col width="30%" />
            <col width="70%" />
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Taxation Company" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="TaxCompID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="TaxCompID_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Taxation Form Type" />:
                </td>
                <td class="pm_field">IR56
                    <asp:DropDownList ID="TaxFormType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="TaxFormType_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Year" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList runat="server" ID="YearSelect" AutoPostBack="true" OnSelectedIndexChanged="YearSelect_SelectedIndexChanged" />
                </td>
            </tr>
        </table>
        </ContentTemplate> 
        </asp:UpdatePanel> 
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TaxCompID" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="TaxFormType" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="YearSelect" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID ="btnGenerate" EventName="Click" />
        </Triggers>
        <ContentTemplate >
        <asp:Panel ID="panelEmpList" runat="server">
            <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" >
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
            </Triggers>

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
            <br /><br />
			<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Employee List" />
                    </td>
                </tr>
            </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                      <asp:Button ID="btnGenerate" runat="server" Text="Generate" CSSClass="button" OnClick="btnGenerate_Click" />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table> 
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="26px" />
                <col width="150px" />
                <col width="150px" />
                <col width="300px"/>
                <col width="150px" />
                <col width="100px" />
                <col width="150px" />
                <tr>
                    <td class="pm_list_header"  align="center">
                        <asp:Panel ID="TaxGenEmpSelectAllPanel" runat="server">
                            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                        </asp:Panel>
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" />
                    </td>
                    <td align="left" class="pm_list_header" colspan="2">
                        <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpDateOfJoin" OnClick="ChangeOrder_Click" Text="Date of Join" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpTermLastDate" OnClick="ChangeOrder_Click" Text="Last Employment Date" />
                    </td>
                </tr>
                <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list" align="center">
                                <asp:CheckBox ID="ItemSelect" runat="server" />
                            </td>
                            <td class="pm_list">
                                <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID"))%>">
                                    <%#sbinding.getValue(Container.DataItem, "EmpNo")%>
                                </a>
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
                            <td class="pm_list">
                                <%#sbinding.getFValue(Container.DataItem, "EmpDateOfJoin", "yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getFValue(Container.DataItem, "EmpTermLastDate", "yyyy-MM-dd")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <tb:RecordListFooter id="ListFooter" runat="server"
                ShowAllRecords="true" visible="true" 
              />
            <br />
			<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Taxation Generated Before" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="26px" />
                <col width="100px" />
                <col width="100px" />
                <col width="200px"/>
                <col width="100px" />
                <col width="100px" />
                <col width="150px" />
                <col width="150px" />
                <col width="100px" />
                <tr>
                    <td class="pm_list_header" align="center">
                        <asp:Panel ID="Panel1" runat="server">
                            <input type="checkbox" onclick="checkAll('<%=TaxGenerated_Repeater.ClientID %>','ItemSelect',this.checked);" />
                        </asp:Panel>
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="TaxGenerated_EmpNo" OnClick="TaxGenerated_ChangeOrder_Click" Text="EmpNo" />
                    </td>
                    <td align="left" class="pm_list_header" colspan="2">
                        <asp:LinkButton runat="server" ID="TaxGenerated_EmpEngFullName" OnClick="TaxGenerated_ChangeOrder_Click" Text="Name" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="TaxGenerated_EmpAlias" OnClick="TaxGenerated_ChangeOrder_Click" Text="Alias" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="TaxGenerated_EmpDateOfJoin" OnClick="TaxGenerated_ChangeOrder_Click" Text="Date of Join" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="TaxGenerated_EmpTermLastDate" OnClick="TaxGenerated_ChangeOrder_Click" Text="Last Employment Date" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="TaxGenerated_TaxEmpGeneratedDate" OnClick="TaxGenerated_ChangeOrder_Click" Text="Last Generation Date" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:Label runat="server" ID="TaxGenerated_TaxEmpGeneratedByUserID" Text="Generated By" />
                    </td>
                </tr>
                <asp:Repeater ID="TaxGenerated_Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list" align="center">
                                <asp:CheckBox ID="ItemSelect" runat="server" />
                            </td>
                            <td class="pm_list">
                                <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID"))%>">
                                    <%#sbinding.getValue(Container.DataItem, "EmpNo")%>
                                </a>
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
                            <td class="pm_list">
                                <%#sbinding.getFValue(Container.DataItem, "EmpDateOfJoin", "yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getFValue(Container.DataItem, "EmpTermLastDate", "yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getFValue(Container.DataItem, "TaxEmpGeneratedDate", "yyyy-MM-dd HH:mm:ss")%>
                            </td>
                            <td class="pm_list">
                                <asp:Label ID="TaxEmpGeneratedByUserID" runat="server" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <tb:RecordListFooter id="TaxGenerated_ListFooter" runat="server"
                ShowAllRecords="true" visible="true" 
              />
        </asp:Panel>
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 