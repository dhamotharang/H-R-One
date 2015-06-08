<%@ page language="C#" autoeventwireup="true" inherits="Payroll_IncentivePayment_Generate_CND, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="footer" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Export Claims And Deduction Template for Incentive Payment" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="SearchUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />
        </Triggers>

        <ContentTemplate >
            <uc2:EmployeeSearchControl id="EmployeeSearchControl1" runat="server" />
        </ContentTemplate >
        </asp:UpdatePanel>

        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
<%--            <tr>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" Text="Achievement Date" />: 
                    <uc1:WebDatePicker id="PeriodFr" runat="server" OnChanged="PeriodFr_OnChanged" AutoPostBack="true" ShowDateFormatLabel="false"   />
                    <asp:Label runat="server" Text="To"  />:
                    <uc1:WebDatePicker id="PeriodTo" runat="server" ShowDateFormatLabel="false"  />
                </td>
            </tr>
--%>        
            <tr>
                <td>
                    <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                    <asp:Button ID="Reset" runat="server" Text="Reset" CssClass="button" OnClick="Reset_Click" />
                </td>
            </tr>

            <tr>
            
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" runat="server" Text="Import Batch" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="BatchID" runat="Server" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label7" runat="server" Text="Payment Date" />:
                </td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="PaymentDate" runat="server" ShowDateFormatLabel="true" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label6" runat="server" Text="Payment Code" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="PaymentCode" runat="Server"  />
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
        <table class="pm_section" width="100%">
<%--                <td align="left">
                    <asp:Button ID="btnBack" runat="Server" Text="- Back -" CssClass="button" OnClick="btnBack_Click" />
                    <asp:Button ID="btnExport" runat="Server" Text="Generate" CssClass="button" OnClick="btnExport_Click" />
--%>
                <td colspan="2">
                        <tb:DetailToolBar id="toolBar" runat="server"
                         NewButton_Visible="false" 
                         EditButton_Visible="false" 
                         SaveButton_Visible="false"
                         CustomButton1_Visible="true" 
                         DeleteButton_Visible="false" 
                         OnBackButton_Click="btnBack_Click"
                         CustomButton1_Name="Generate"
                         OnCustomButton1_Click="btnExport_Click"
                          />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Reset" EventName="Click" />

<%--            <asp:PostBackTrigger ControlID="btnExport"  />--%>

        </Triggers>

        <ContentTemplate >
        <%--
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="150px" />
            <col width="150px" />
            <col width="350px"/>
            <col width="150px" />
            <tr>
                <td class="pm_list_header" align="center" >
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);"  visible="false"/>
                </td>
                <td align="left" class="pm_list_header" >
                    <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" /></td>
                <td align="left" class="pm_list_header" colspan="2" >
                    <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" /></td>
                <td align="left" class="pm_list_header" >
                    <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" /></td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                        </td>
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Payroll_IncentivePayment_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID"))%>">
                                <%#sbinding.getValue(Container.DataItem,"EmpNo")%>
                            </a>
                        </td>
                        <td class="pm_list" >
                            <%#sbinding.getValue(Container.DataItem,"EmpEngSurname")%>
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"EmpEngOtherName")%>
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"EmpAlias")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <footer:RecordListFooter id="ListFooter" runat="server" ShowAllRecords="true" />--%>
        
         <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="50px" />
            <col width="100px" />
            <col width="200px" />
            <col width="100px" />
            <col width="100px" />
            <col width="75px" />
            <tr>
                <td class="pm_list_header" >
                </td>
                <td align="left" class="pm_list_header" >
                    <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" /></td>
                <td align="left" class="pm_list_header" colspan="2">
                    <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" /></td>
                <td align="left" class="pm_list_header" >
                    <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" /></td>
                <td align="left" class="pm_list_header" >
                    <asp:LinkButton runat="server" ID="_IPEffDate" OnClick="ChangeOrder_Click" Text="As At Date" /></td>
                <td align="right" class="pm_list_header" >
                    <asp:LinkButton runat="server" ID="_IPPercent" OnClick="ChangeOrder_Click" Text="Incentive(%)" />
                </td>
                <td class="pm_list_header"></td>
            </tr>
            <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                ShowFooter="false" ShowHeader="false" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center" >
                            <input type="hidden" runat="server" id="IPID" />
                        </td>
                        <td class="pm_list">
                            <%--
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Payroll_IncentivePayment_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID"))%>">
                                <%#sbinding.getValue(Container.DataItem,"EmpNo")%>
                            </a>
                            --%>
                            <%#sbinding.getValue(Container.DataItem,"EmpNo")%>
                            
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"EmpEngSurname")%>
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"EmpEngOtherName")%>
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"EmpAlias")%>
                        </td>
                        <td class="pm_list">
                            <asp:Label ID="IPEffDate" runat="server" />
                        </td>                        
                        <td class="pm_list" align="right">
                            <asp:Label ID="IPPercent" runat="server" />
                        </td>
                        <td class="pm_list"></td>
                    </tr>
                </ItemTemplate>
            </asp:DataList>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
            <tr>
                <td align="right">
                    <footer:RecordListFooter id="ListFooter" runat="server"
                        OnFirstPageClick="ChangePage"
                        OnPrevPageClick="ChangePage"
                        OnNextPageClick="ChangePage"
                        OnLastPageClick="ChangePage"
                      />
                </td>
            </tr>
        </table>
        
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content> 