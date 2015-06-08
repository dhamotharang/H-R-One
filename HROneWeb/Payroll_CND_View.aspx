<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_CND_View.aspx.cs"    Inherits="Payroll_CND_View" MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/Emp_Header.ascx" TagName="Emp_Header" TagPrefix="uc1" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    <input type="hidden" id="EmpID" runat="server"  name="ID" />

	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Claims and Deductions" runat="server" />
                </td>
            </tr>
        </table>
    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_section_title">
        <tr>
            <td>
                <asp:Label Text="Claims and Deductions Detail" runat="server" />:
            </td>
        </tr>
    </table>
    <uc1:Emp_Header ID="ucEmp_Header" runat="server" />
    
        
            
    
    <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_section_title">
        <tr>
            <td>
                <asp:Label ID="Label1" Text="Outstanding Records" runat="server" />:
            </td>
        </tr>
    </table>
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
        <col width="50px" />
        <col width="100px" />
        <col width="250px" />
        <col width="85px" />
        <col width="85px" />
        <col width="150px" />
        <col width="75px" />
        <col width="140px" />
        <tr>
            <td class="pm_list_header" >
            </td>
            <td class="pm_list_header" >
            </td>
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="_CNDEffDate" OnClick="ChangeOrder_Click" Text="Date" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:LinkButton runat="server" ID="_PayCodeID" OnClick="ChangeOrder_Click" Text="Payment" />
            </td>
<%--            <td align="left" class="pm_list_header" >
            </td>--%>
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="_CNDAmount" OnClick="ChangeOrder_Click" Text="Amount" />
            </td>
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="_CNDPayMethod" OnClick="ChangeOrder_Click" Text="Method" />
            </td>
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="_EmpAccID" OnClick="ChangeOrder_Click" Text="Bank Account Number" />
            </td>
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="_CNDNumOfDayAdj" OnClick="ChangeOrder_Click" Text="Days Adjust" />
            </td>
            <td id="CostCenterHeaderCell" runat="server" align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="_CostCenterID" OnClick="ChangeOrder_Click" Text="Cost Center"></asp:LinkButton>
            </td>
<%--            <td align="left" class="pm_list_header">
                <asp:LinkButton runat="server" ID="_CNDRemark" OnClick="ChangeOrder_Click">Remark</asp:LinkButton>
            </td>--%>
        </tr>
        <asp:Panel ID="CNDAddPanel" runat="server">
        <tr id="detailRow" runat="server" >
            <td class="pm_list_alt_row"  rowspan="2">
            </td>
            <td class="pm_list_alt_row"  align="center" rowspan="2">
                <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" />
            </td>
            <td class="pm_list_alt_row" style="white-space:nowrap; ">
                <uc1:WebDatePicker id="CNDEffDate" runat="server" ShowDateFormatLabel="false" />
            </td>
            <td class="pm_list_alt_row" >
                <asp:DropDownList ID="PayCodeID" runat="server" Width="250px" />
            </td>
            <td class="pm_list_alt_row"  align="right" visible="false" >
                <asp:DropDownList ID="CurrencyID" runat="server" />
            </td>
            <td class="pm_list_alt_row"  align="right">
                <asp:TextBox ID="CNDAmount" runat="server" Columns="10" style="text-align:right" />
            </td>
            <td class="pm_list_alt_row" align="center">
                <asp:DropDownList ID="CNDPayMethod" runat="server" />
            </td>
            <td class="pm_list_alt_row" >
                <asp:DropDownList ID="EmpAccID" runat="server" Width="150px"/>
            </td>
            <td class="pm_list_alt_row" align="right">
                <asp:TextBox ID="CNDNumOfDayAdj" runat="server" style="text-align:right" />
            </td>
            <td id="CostCenterDetailCell" runat="server" class="pm_list_alt_row" >
                <asp:DropDownList ID="CostCenterID" runat="server" Width="140px" />
            </td>
        </tr>
        <tr>
            <td id="RemarkCell" runat="server" class="pm_list_alt_row"  align="left" colspan="5">
                <asp:Label ID="Label3" runat="server" Text="Remark" />:&nbsp
                <asp:TextBox ID="CNDRemark" runat="server" Columns="75"/>
            </td>
            <td class="pm_list_alt_row"  colspan="2">
                <asp:CheckBox ID="CNDIsRestDayPayment" runat="server" Text="Rest Payment"/>
            </td>
        </tr>
        </asp:Panel>
        <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false"
            ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
            <EditItemTemplate>
                <tr id="detailRow" runat="server" >
                    <td class="pm_list_edit"  align="center" rowspan="2">
                        <asp:CheckBox ID="DeleteItem" runat="server" />
                        <input type="hidden" runat="server" id="CNDID" />
                    </td>
                    <td class="pm_list_edit"  align="center" rowspan="2">
                        <asp:Button ID="Save" CssClass="button" runat="server" Text="Save" />
                        <asp:Button ID="Cancel" CssClass="button" runat="server" Text="Cancel" />
                    </td>
                    <td class="pm_list_edit" style="white-space:nowrap; ">
                        <uc1:WebDatePicker id="CNDEffDate" runat="server" ShowDateFormatLabel="false" />
                    </td>
                    <td class="pm_list_edit" >
                        <asp:DropDownList ID="PayCodeID" runat="server" Width="250px" />
                    </td>
                    <td class="pm_list_edit"  align="right" visible="false" >
                        <asp:DropDownList ID="CurrencyID" runat="server" />
                    </td>
                    <td class="pm_list_edit"  align="right">
                        <asp:TextBox ID="CNDAmount" runat="server" Columns="10" style="text-align:right" />
                    </td>
                    <td class="pm_list_edit" align="center">
                        <asp:DropDownList ID="CNDPayMethod" runat="server" />
                    </td>
                    <td class="pm_list_edit" >
                        <asp:DropDownList ID="EmpAccID" runat="server" Width="150px"/>
                    </td>
                    <td class="pm_list_edit" align="right">
                        <asp:TextBox ID="CNDNumOfDayAdj" runat="server" style="text-align:right" />
                    </td>
                    <td id="CostCenterDetailCell" runat="server" class="pm_list_edit" >
                        <asp:DropDownList ID="CostCenterID" runat="server" Width="140px"/>
                    </td>
                </tr>
                <tr>
                    <td id="RemarkCell" runat="server" class="pm_list_edit"  align="left" colspan="5">
                        <asp:Label ID="Label3" runat="server" Text="Remark" />:&nbsp
                        <asp:TextBox ID="CNDRemark" runat="server" Columns="75"/>
                    </td>
                    <td class="pm_list_edit"  colspan="2">
                        <asp:CheckBox ID="CNDIsRestDayPayment" runat="server" Text="Rest Payment"/>
                    </td>
                </tr>
            </EditItemTemplate>
            <ItemTemplate>
                <tr id="detailRow" runat="server" >
                    <td class="pm_list"  align="center" rowspan="2">
                        <asp:CheckBox ID="DeleteItem" runat="server" />
                        <input type="hidden" runat="server" id="CNDID" />
                    </td>
                    <td class="pm_list"  align="center" rowspan="2">
                        <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                    </td>
                    <td class="pm_list" style="white-space:nowrap; ">
                        <%#sbinding.getFValue(Container.DataItem,"CNDEffDate","yyyy-MM-dd")%>
                    </td>
                    <td class="pm_list" >
                        <%#sbinding.getValue(Container.DataItem,"PayCodeID")%>
                    </td>
                    <td class="pm_list"  align="right" visible="false" >
                        <%#sbinding.getValue(Container.DataItem,"CurrencyID")%>
                    </td>
                    <td class="pm_list" align="right" >
                        <%#sbinding.getFValue(Container.DataItem, "CNDAmount", "$#,##0.00")%>
                    </td>
                    <td class="pm_list"  align="center" >
                        <%#sbinding.getValue(Container.DataItem, "CNDPayMethod")%>
                    </td>
                    <td class="pm_list" >
                        <asp:Label ID="EmpAccID" runat="server" />
                    </td>
                    <td class="pm_list"  align="right">
                        <%#sbinding.getValue(Container.DataItem, "CNDNumOfDayAdj")%>
                    </td>
                    <td id="CostCenterDetailCell" runat="server" class="pm_list" >
                        <asp:Label ID="CostCenterID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td id="RemarkCell" runat="server" class="pm_list"  align="left" colspan="5">
                        <asp:Label ID="Label2" runat="server" Text="Remark" />: &nbsp
                        <%#sbinding.getValue(Container.DataItem, "CNDRemark")%>
                    </td>
                    <td class="pm_list"  colspan="2">
                        <asp:Label ID="Label4" runat="server" Text="Rest Payment"/>?&nbsp
                         <%#sbinding.getValue(Container.DataItem, "CNDIsRestDayPayment")%>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr id="detailRow" runat="server" >
                    <td class="pm_list_alt_row"  align="center" rowspan="2">
                        <asp:CheckBox ID="DeleteItem" runat="server" />
                        <input type="hidden" runat="server" id="CNDID" />
                    </td>
                    <td class="pm_list_alt_row"  align="center" rowspan="2">
                        <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                    </td>
                    <td class="pm_list_alt_row" style="white-space:nowrap; ">
                        <%#sbinding.getFValue(Container.DataItem,"CNDEffDate","yyyy-MM-dd")%>
                    </td>
                    <td class="pm_list_alt_row" >
                        <%#sbinding.getValue(Container.DataItem,"PayCodeID")%>
                    </td>
                    <td class="pm_list_alt_row"  align="right" visible="false" >
                        <%#sbinding.getValue(Container.DataItem,"CurrencyID")%>
                    </td>
                    <td class="pm_list_alt_row" align="right" >
                        <%#sbinding.getFValue(Container.DataItem, "CNDAmount", "$#,##0.00")%>
                    </td>
                    <td class="pm_list_alt_row"  align="center" >
                        <%#sbinding.getValue(Container.DataItem, "CNDPayMethod")%>
                    </td>
                    <td class="pm_list_alt_row" >
                        <asp:Label ID="EmpAccID" runat="server" />
                    </td>
                    <td class="pm_list_alt_row"  align="right">
                        <%#sbinding.getValue(Container.DataItem, "CNDNumOfDayAdj")%>
                    </td>
                    <td id="CostCenterDetailCell" runat="server" class="pm_list_alt_row" >
                        <asp:Label ID="CostCenterID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td id="RemarkCell" runat="server" class="pm_list_alt_row"  align="left" colspan="5">
                        <asp:Label ID="Label2" runat="server" Text="Remark" />: &nbsp
                        <%#sbinding.getValue(Container.DataItem, "CNDRemark")%>
                    </td>
                    <td class="pm_list_alt_row"  colspan="2">
                        <asp:Label ID="Label4" runat="server" Text="Rest Payment"/>?&nbsp
                         <%#sbinding.getValue(Container.DataItem, "CNDIsRestDayPayment")%>
                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:DataList>
    </table>
    <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
        <tr>
            <td align="right">
                <tb:RecordListFooter id="ListFooter" runat="server"
                    ShowAllRecords="true" 
                  />
            </td>
        </tr>
    </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <table border="0" width="100%" class="pm_section_title" cellspacing="0" cellpadding="1">
        <tr>
            <td>
                <asp:Label ID="Label2" Text="History Records" runat="server" />:
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate >
    <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
        <col width="26px" />
        <col width="75px" />
        <col width="210px" />
        <col width="75px" />
        <col width="75px" />
        <col width="150px" />
        <col width="75px" />
        <col width="140px" />
        <tr>
            <td class="pm_list_header" >
            </td>
            <td class="pm_list_header" runat="server" visible="false" >
            </td>
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="PayrollProcessed_CNDEffDate" OnClick="PayrollProcessedChangeOrder_Click" Text="Date" />
            </td>
            <td align="left" class="pm_list_header">
                <asp:LinkButton runat="server" ID="PayrollProcessed_PayCodeID" OnClick="PayrollProcessedChangeOrder_Click" Text="Payment" />
            </td>
<!--            <td align="left" class="pm_list_header" >
            </td>-->
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="PayrollProcessed_CNDAmount" OnClick="PayrollProcessedChangeOrder_Click" Text="Amount" />
            </td>
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="PayrollProcessed_CNDPayMethod" OnClick="PayrollProcessedChangeOrder_Click" Text="Method" />
            </td>
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="PayrollProcessed_EmpAccID" OnClick="PayrollProcessedChangeOrder_Click" Text="Bank Account Number" />
            </td>
            <td align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="PayrollProcessed_CNDNumOfDayAdj" OnClick="PayrollProcessedChangeOrder_Click" Text="Days Adjust" />
            </td>
            <td id="PayrollProcessed_CostCenterHeaderCell" runat="server" align="left" class="pm_list_header" >
                <asp:LinkButton runat="server" ID="PayrollProcessed_CostCenterID" OnClick="PayrollProcessedChangeOrder_Click" Text="Cost Center"></asp:LinkButton>
            </td>
        </tr>
        <asp:DataList ID="PayrollProcessedRepeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false"
            ShowHeader="false" OnItemDataBound="Repeater_ItemDataBound">
            <ItemTemplate>
                <tr id="detailRow" runat="server" >
                    <td class="pm_list" align="center" rowspan="2">
                        <asp:CheckBox ID="DeleteItem" runat="server" />
                        <input type="hidden" runat="server" id="CNDID" />
                    </td>
                    <td class="pm_list" align="center" rowspan="2" runat="server" visible="false" >
                        <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                    </td>
                    <td class="pm_list" style="white-space:nowrap;">
                        <%#sPayrollProcessedBinding.getFValue(Container.DataItem,"CNDEffDate","yyyy-MM-dd")%>
                    </td>
                    <td class="pm_list">
                        <%#sPayrollProcessedBinding.getValue(Container.DataItem, "PayCodeID")%>
                    </td>
                    <td class="pm_list" align="right" visible="false" >
                        <%#sbinding.getValue(Container.DataItem,"CurrencyID")%>
                    </td>
                    <td class="pm_list" align="right" style="white-space:nowrap;">
                        <%#sPayrollProcessedBinding.getFValue(Container.DataItem, "CNDAmount", "$#,##0.00")%>
                    </td>
                    <td class="pm_list" align="center">
                        <%#sPayrollProcessedBinding.getValue(Container.DataItem, "CNDPayMethod")%>
                    </td>
                    <td class="pm_list" style="white-space:nowrap;">
                        <asp:Label ID="EmpAccID" runat="server" />
                    </td>
                    <td class="pm_list" align="right" >
                        <%#sbinding.getValue(Container.DataItem, "CNDNumOfDayAdj")%>
                    </td>
                    <td id="CostCenterDetailCell" runat="server" class="pm_list" >
                        <asp:Label ID="CostCenterID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td id="RemarkCell" runat="server" class="pm_list" align="left" colspan="3">
                        <asp:Label ID="Label2" runat="server" Text="Remark" />: &nbsp
                        <%#sbinding.getValue(Container.DataItem, "CNDRemark")%>
                    </td>
                    <td class="pm_list"  colspan="2">
                        <asp:Label ID="Label4" runat="server" Text="Rest Payment"/>?&nbsp
                         <%#sbinding.getValue(Container.DataItem, "CNDIsRestDayPayment")%>
                    </td>
                    <td class="pm_list"  colspan="2">
                        <asp:Label ID="Label5" runat="server" Text="Payroll Process Cycle"/>:&nbsp
                        <asp:Label ID="PayPeriodID" runat="Server" />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr id="detailRow" runat="server" >
                    <td class="pm_list_alt_row" align="center" rowspan="2">
                        <asp:CheckBox ID="DeleteItem" runat="server" />
                        <input type="hidden" runat="server" id="CNDID" />
                    </td>
                    <td id="Td1" class="pm_list_alt_row" align="center" rowspan="2" runat="server" visible="false" >
                        <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                    </td>
                    <td class="pm_list_alt_row" style="white-space:nowrap;">
                        <%#sPayrollProcessedBinding.getFValue(Container.DataItem,"CNDEffDate","yyyy-MM-dd")%>
                    </td>
                    <td class="pm_list_alt_row">
                        <%#sPayrollProcessedBinding.getValue(Container.DataItem, "PayCodeID")%>
                    </td>
                    <td class="pm_list_alt_row" align="right" visible="false" >
                        <%#sbinding.getValue(Container.DataItem,"CurrencyID")%>
                    </td>
                    <td class="pm_list_alt_row" align="right" style="white-space:nowrap;">
                        <%#sPayrollProcessedBinding.getFValue(Container.DataItem, "CNDAmount", "$#,##0.00")%>
                    </td>
                    <td class="pm_list_alt_row" align="center">
                        <%#sPayrollProcessedBinding.getValue(Container.DataItem, "CNDPayMethod")%>
                    </td>
                    <td class="pm_list_alt_row" >
                        <asp:Label ID="EmpAccID" runat="server" />
                    </td>
                    <td class="pm_list_alt_row" align="right" >
                        <%#sbinding.getValue(Container.DataItem, "CNDNumOfDayAdj")%>
                    </td>
                    <td id="CostCenterDetailCell" runat="server" class="pm_list_alt_row" >
                        <asp:Label ID="CostCenterID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td id="RemarkCell" runat="server" class="pm_list_alt_row" align="left" colspan="3">
                        <asp:Label ID="Label2" runat="server" Text="Remark" />: &nbsp
                        <%#sbinding.getValue(Container.DataItem, "CNDRemark")%>
                    </td>
                    <td class="pm_list_alt_row"  colspan="2">
                        <asp:Label ID="Label4" runat="server" Text="Rest Payment"/>?&nbsp
                         <%#sbinding.getValue(Container.DataItem, "CNDIsRestDayPayment")%>
                    </td>
                    <td class="pm_list_alt_row"  colspan="2">
                        <asp:Label ID="Label5" runat="server" Text="Payroll Process Cycle"/>:&nbsp
                        <asp:Label ID="PayPeriodID" runat="Server" />
                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:DataList>
    </table>
    <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
        <tr>
            <td align="right">
                <tb:RecordListFooter id="PayrollProcessedListFooter" runat="server"
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