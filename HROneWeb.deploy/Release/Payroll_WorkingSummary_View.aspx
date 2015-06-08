<%@ page language="C#" autoeventwireup="true" inherits="Payroll_WorkingSummary_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/Emp_Header.ascx" TagName="Emp_Header" TagPrefix="uc1" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <input type="hidden" id="EmpID" runat="server"  name="ID" />

	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Employee Working Summary" runat="server" />
                </td>
            </tr>
        </table>
    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_section_title">
        <tr>
            <td>
                <asp:Label Text="Detail" runat="server" />:
            </td>
        </tr>
    </table>
    <uc1:Emp_Header ID="ucEmp_Header" runat="server" />
    
        
            
    
    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
        <tr>
            <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     EditButton_Visible="false" 
                     SaveButton_Visible="false"
                     OnBackButton_Click="Back_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
            </td>
        </tr>
    </table>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
    <ContentTemplate >
    <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
        <col width="26px" /> 
        <col width="120px" /> 
        <col width="150px" /> 
        <tr>
            <td class="pm_list_header">
            </td>
            <td class="pm_list_header">
            </td>
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="_EmpWorkingSummaryAsOfDate" OnClick="ChangeOrder_Click" Text="As Of Date" />
            </td>
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="_EmpWorkingSummaryRestDayEntitled" OnClick="ChangeOrder_Click" Text="Rest Day Entitled" />
            </td>
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="_EmpWorkingSummaryRestDayTaken" OnClick="ChangeOrder_Click" Text="Rest Day Taken" />
            </td>
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="_EmpWorkingSummaryTotalWorkingDays" OnClick="ChangeOrder_Click" Text="Total Working Day(s)" />
            </td>
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="_EmpWorkingSummaryTotalWorkingHours" OnClick="ChangeOrder_Click" Text="Total Hours Worked" />
            </td>
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="_EmpWorkingSummaryTotalLunchTimeHours" OnClick="ChangeOrder_Click" Text="Total Hours for Meal Break" />
            </td>
        </tr>
        <asp:Panel ID="WorkingSummaryAddPanel" runat="server">
        <tr>
            <td class="pm_list">
            </td>
            <td class="pm_list" align="center">
                <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" />
            </td>
            <td class="pm_list" style="white-space:nowrap">
                <uc1:WebDatePicker id="EmpWorkingSummaryAsOfDate" runat="server" />
            </td>
            <td class="pm_list" align="right">
                <asp:TextBox ID="EmpWorkingSummaryRestDayEntitled" runat="server" style="text-align:right" />
            </td>
            <td class="pm_list" align="right">
                <asp:TextBox ID="EmpWorkingSummaryRestDayTaken" runat="server" style="text-align:right" />
            </td>
            <td class="pm_list" align="right">
                <asp:TextBox ID="EmpWorkingSummaryTotalWorkingDays" runat="server" style="text-align:right" />
            </td>
            <td class="pm_list" align="right">
                <asp:TextBox ID="EmpWorkingSummaryTotalWorkingHours" runat="server" style="text-align:right" />
            </td>
            <td class="pm_list" align="right">
                <asp:TextBox ID="EmpWorkingSummaryTotalLunchTimeHours" runat="server" style="text-align:right" />
            </td>
        </tr>
        </asp:Panel>
        <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false"
            ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
            <EditItemTemplate>
                <tr>
                    <td class="pm_list" align="center" >
                        <asp:CheckBox ID="DeleteItem" runat="server" />
                        <input type="hidden" runat="server" id="EmpWorkingSummaryID" />
                    </td>
                    <td class="pm_list" align="center">
                        <asp:Button ID="Save" CssClass="button" runat="server" Text="Save" />
                        <asp:Button ID="Cancel" CssClass="button" runat="server" Text="Cancel" />
                    </td>
                    <td class="pm_list" style="white-space:nowrap">
                        <uc1:WebDatePicker id="EmpWorkingSummaryAsOfDate" runat="server" />
                    </td>
                    <td class="pm_list" align="right">
                        <asp:TextBox ID="EmpWorkingSummaryRestDayEntitled" runat="server" style="text-align:right" />
                    </td>
                    <td class="pm_list" align="right">
                        <asp:TextBox ID="EmpWorkingSummaryRestDayTaken" runat="server" style="text-align:right" />
                    </td>
                    <td class="pm_list" align="right">
                        <asp:TextBox ID="EmpWorkingSummaryTotalWorkingDays" runat="server" style="text-align:right" />
                    </td>
                    <td class="pm_list" align="right">
                        <asp:TextBox ID="EmpWorkingSummaryTotalWorkingHours" runat="server" style="text-align:right" />
                    </td>
                    <td class="pm_list" align="right">
                        <asp:TextBox ID="EmpWorkingSummaryTotalLunchTimeHours" runat="server" style="text-align:right" />
                    </td>
                </tr>
            </EditItemTemplate>
            <ItemTemplate>
                <tr>
                    <td class="pm_list" align="center" >
                        <asp:CheckBox ID="DeleteItem" runat="server" />
                        <input type="hidden" runat="server" id="EmpWorkingSummaryID" />
                    </td>
                    <td class="pm_list" align="center">
                        <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                    </td>
                    <td class="pm_list" style="white-space:nowrap">
                        <%#sbinding.getFValue(Container.DataItem, "EmpWorkingSummaryAsOfDate", "yyyy-MM-dd")%>
                    </td>
                    <td class="pm_list" align="right">
                        <%#sbinding.getFValue(Container.DataItem, "EmpWorkingSummaryRestDayEntitled", "0.00")%>
                    </td>
                    <td class="pm_list" align="right">
                        <%#sbinding.getFValue(Container.DataItem, "EmpWorkingSummaryRestDayTaken", "0.00")%>
                    </td>
                    <td class="pm_list" align="right">
                        <%#sbinding.getFValue(Container.DataItem, "EmpWorkingSummaryTotalWorkingDays", "0.00")%>
                    </td>
                    <td class="pm_list" align="right">
                        <%#sbinding.getFValue(Container.DataItem, "EmpWorkingSummaryTotalWorkingHours", "0.00")%>
                    </td>
                    <td class="pm_list" align="right">
                        <%#sbinding.getFValue(Container.DataItem, "EmpWorkingSummaryTotalLunchTimeHours", "0.00")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:DataList>
    </table>
    <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
        <tr>
            <td align="right">
                <tb:RecordListFooter id="ListFooter" runat="server"
                    ListOrderBy="EmpWorkingSummaryAsOfDate"
                    ListOrder="false" 
                    ShowAllRecords="true" 
                  />
            </td>
        </tr>
    </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />

</asp:Content> 