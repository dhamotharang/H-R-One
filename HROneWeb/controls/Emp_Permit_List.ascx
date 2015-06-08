<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_Permit_List.ascx.cs"
    Inherits="Emp_Permit_List" %>
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
    <col width="100px" />
    <col width="100px" />
    <col width="100px" />
    <tr>
        <td class="pm_list_header" align="center">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','DeleteItem',this.checked);" />
                    </asp:Panel>     
        </td>
<%--        <td align="left" class="pm_list_header">
            </td>
--%>        
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_PermitTypeID" OnClick="ChangeOrder_Click" Text="Type" /></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpPermitNo" OnClick="ChangeOrder_Click" Text="Permit No." /></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpPermitIssueDate" OnClick="ChangeOrder_Click" Text="Issue Date" /></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpPermitExpiryDate" OnClick="ChangeOrder_Click" Text="Expiry Date" /></td>
        <td align="left" class="pm_list_header">
            <asp:Label runat="server" ID="_EmpPermitRemark" Text="Remark" /></td>
        
    </tr>
    <asp:Panel ID="AddPanel" runat="server" Visible="false" >
        <tr>
            <td class="pm_list">
            </td>
<%--            <td class="pm_list" align="center">
                <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" />
            </td>
--%>            <td class="pm_list">
                <asp:DropDownList ID="PermitTypeID" runat="server">
                </asp:DropDownList>
            </td>
            <td class="pm_list" align="right">
                <asp:TextBox ID="EmpPermitNo" runat="server" />
            </td>
            <td class="pm_list">
                <uc1:WebDatePicker id="EmpPermitIssueDate" runat="server" ShowDateFormatLabel="false"/>

            </td>
            <td class="pm_list">
                <uc1:WebDatePicker id="EmpPermitExpiryDate" runat="server" ShowDateFormatLabel="false"/>
            </td>
            <td class="pm_list" align="right">
                <asp:TextBox ID="EmpPermitRemark" runat="server" TextMode="MultiLine" />
            </td>
        </tr>
    </asp:Panel>
    <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" ShowFooter="false" ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound" RepeatDirection="Horizontal">
        <EditItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="DeleteItem" runat="server" />
                    <input type="hidden" runat="server" id="EmpPermitID" />
                </td>
<%--                <td class="pm_list" align="center">
                    <asp:Button ID="Save" CssClass="button" runat="server" Text="Save" />
                    <asp:Button ID="Cancel" CssClass="button" runat="server" Text="Cancel" />
                </td>
--%>                <td class="pm_list">
                    <asp:DropDownList ID="PermitTypeID" runat="server">
                    </asp:DropDownList>
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="EmpPermitNo" runat="server" />
                </td>
                <td class="pm_list">
                    <uc1:WebDatePicker id="EmpPermitIssueDate" runat="server" ShowDateFormatLabel="false" />
                </td>
                <td class="pm_list">
                    <uc1:WebDatePicker id="EmpPermitExpiryDate" runat="server" ShowDateFormatLabel="false"/>
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="EmpPermitRemark" runat="server" TextMode="MultiLine" />
                </td>
            </tr>
        </EditItemTemplate>
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="DeleteItem" runat="server" />
                    <input type="hidden" runat="server" id="EmpPermitID" />
                </td>
<%--                <td class="pm_list" align="center">
                    <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                </td>
--%>                <td class="pm_list">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_Permit_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID") + "&EmpPermitID=" + sbinding.getValue(Container.DataItem,"EmpPermitID"))%>">
                    <%#sbinding.getValue(Container.DataItem, "PermitTypeID")%>
                    </a>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "EmpPermitNo")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getFValue(Container.DataItem, "EmpPermitIssueDate","yyyy-MM-dd")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getFValue(Container.DataItem, "EmpPermitExpiryDate", "yyyy-MM-dd")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "EmpPermitRemark")%>
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
  />