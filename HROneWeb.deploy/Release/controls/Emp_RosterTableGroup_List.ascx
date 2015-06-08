<%@ control language="C#" autoeventwireup="true" inherits="Emp_RosterTableGroup_List, HROneWeb.deploy" %>
<%@ Register Src="DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<input type="hidden" id="EmpID" runat="server" name="ID" />
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
    <col width="100px" />
    <col width="100px" />
    <tr>
        <td class="pm_list_header" align="center">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                    </asp:Panel>     
        </td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_EmpRosterTableGroupEffFr" OnClick="ChangeOrder_Click" Text="From"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_EmpRosterTableGroupEffTo" OnClick="ChangeOrder_Click" Text="To"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_RosterTableGroupID" OnClick="ChangeOrder_Click" Text="RosterTableGroup"></asp:LinkButton>
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpRosterTableGroupIsSupervisor" OnClick="ChangeOrder_Click" Text="Supervisor"></asp:LinkButton>?
        </td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
                </td>
                <td class="pm_list">
		        <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_RosterTableGroup_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID") + "&EmpRosterTableGroupID=" + sbinding.getValue(Container.DataItem,"EmpRosterTableGroupID"))%>">
                        <%#sbinding.getFValue(Container.DataItem, "EmpRosterTableGroupEffFr")%>
		        </a>
                </td>
                <td class="pm_list">
                    <%#sbinding.getFValue(Container.DataItem, "EmpRosterTableGroupEffTo")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "RosterTableGroupID")%>                    
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "EmpRosterTableGroupIsSupervisor")%>                    
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
<tb:RecordListFooter id="ListFooter" runat="server"
    OnFirstPageClick="FirstPage_Click"
    OnPrevPageClick="PrevPage_Click"
    OnNextPageClick="NextPage_Click"
    OnLastPageClick="LastPage_Click"
    ListOrderBy="EmpRosterTableGroupEffFr"
    ListOrder="false" 
  />