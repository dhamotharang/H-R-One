<%@ page language="C#" autoeventwireup="true" inherits="Report_Payroll_EEOverallPaymentSummary_List, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/ReportExportControl.ascx" TagName="ReportExportControl" TagPrefix="uc3" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
<script type="text/javascript" >

function changeToFirstDateOfMonth(textBoxObject)
{
    var strDate =textBoxObject.value;
    var dateValue =Date.parse(strDate);
    if (dateValue!=null)
    {
    dateValue.setDate(1);
    var strMonth="0" + (dateValue.getMonth()+1);
    //var strDay="0" + dateValue.getDate();
    textBoxObject.value = dateValue.getFullYear() + "-" + strMonth.substr(strMonth.length - 2,2) + "-01";
    }
}
</script>
        
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="lblReportHeader" runat="server" Text="Employee Overall Payment Summary" />
                </td>
            </tr>
        </table>

            <uc2:EmployeeSearchControl id="EmployeeSearchControl1" runat="server" >
            <AdditionElements >
                <tr>
<%--                    <td class="pm_search_header">
                        <asp:Label ID="Label1" runat="server" Text="Payroll Group" />:
                    </td>
                    <td class="pm_search">
                        <asp:DropDownList runat="server" ID="PayGroup" />
                    </td>
--%>
                    <td class="pm_search_header">
                        <asp:Label ID="Label2" runat="server" Text="Payroll Period From" />:
                    </td>
                    <td class="pm_search">
                        <uc1:WebDatePicker id="PayPeriodFr" runat="server"  CssClass="pm_required" DateFormatString="yyyy-mm-01" ShowDateFormatLabel="false"  />
                    </td>
                </tr>
            </AdditionElements>
            </uc2:EmployeeSearchControl> 
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                    <asp:Button ID="Reset" runat="server" Text="Reset" CssClass="button" OnClick="Reset_Click" />
                </td>
            </tr>
        </table>
        <br />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Employee List" />
                </td>
            </tr>
        </table>
        <table class="pm_section" width="100%">
            <tr>
                <td>
                    <uc3:ReportExportControl id="ReportExportControl1" runat="server"  OnClick="btnGenerate_Click" ButtonCssClass="button"/>
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
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" /></td>
                <td align="left" class="pm_list_header" colspan="2">
                    <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" /></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" /></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpStatus" OnClick="ChangeOrder_Click" Text="Status" /></td>                     
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                        </td>
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_VIew.aspx?EmpID=" + binding.getValue(Container.DataItem,"EmpID"))%>">
                                <%#binding.getValue(Container.DataItem,"EmpNo")%>
                            </a>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"EmpEngSurname")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"EmpEngOtherName")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"EmpAlias")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"EmpStatus")%>
                        </td>                          
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
            <tr>
                <td>
                    <uc3:ReportExportControl id="ReportExportControl2" runat="server"  OnClick="btnGenerate_Click" ButtonCssClass="button"/>
                </td>
                <td align="right">
                    <tb:RecordListFooter id="ListFooter" runat="server"
                        ShowAllRecords="true"
                      />
                </td>
            </tr>
        </table>
</asp:Content> 