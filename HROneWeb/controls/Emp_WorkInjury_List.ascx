<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_WorkInjury_List.ascx.cs"
    Inherits="Emp_WorkInjury_List" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %> 
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>

<input type="hidden" id="EmpID" runat="server" name="ID" />
<%--<table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
    <tr>
        <td>
            <asp:Button ID="Delete" CssClass="button" runat="server" Text="Delete" OnClick="Delete_Click" />
        </td>
    </tr>
</table>
--%>
<table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
    <tr>
        <td>
            <tb:DetailToolBar id="toolBar" runat="server"
             BackButton_Visible="false"
             EditButton_Visible="false" 
             SaveButton_Visible="false" 
             OnNewButton_Click ="New_Click"
             OnDeleteButton_Click="Delete_Click"
              />
        </td>
    </tr>
</table>
<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
    <col width="26px" />
    <col width="150px" />
    <tr>
        <td class="pm_list_header" align="center">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','DeleteItem',this.checked);" />
                    </asp:Panel>     
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpWorkInjuryRecordAccidentDate" OnClick="ChangeOrder_Click" Text="Accident Date" /></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpWorkInjuryRecordInjuryNature" OnClick="ChangeOrder_Click" Text="Injury Nature" /></td>
        
    </tr>
    <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" ShowFooter="false" ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound" RepeatDirection="Horizontal">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="DeleteItem" runat="server" />
                    <input type="hidden" runat="server" id="EmpWorkInjuryRecordID" />
                </td>
                <td class="pm_list">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_WorkInjury_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID") + "&EmpWorkInjuryRecordID=" + sbinding.getValue(Container.DataItem,"EmpWorkInjuryRecordID"))%>">
                    <%#sbinding.getFValue(Container.DataItem, "EmpWorkInjuryRecordAccidentDate","yyyy-MM-dd")%>
                    </a>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "EmpWorkInjuryRecordInjuryNature")%>
                </td>
            </tr>
        </ItemTemplate>
    </asp:DataList>
</table>
<tb:RecordListFooter id="ListFooter" runat="server"
    OnFirstPageClick="FirstPage_Click"
    OnPrevPageClick="PrevPage_Click"
    OnNextPageClick="NextPage_Click"
    OnLastPageClick="LastPage_Click"
    ListOrderBy="EmpWorkInjuryRecordAccidentDate" ListOrder="false" 
  />