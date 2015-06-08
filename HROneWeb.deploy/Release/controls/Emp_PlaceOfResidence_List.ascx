<%@ control language="C#" autoeventwireup="true" inherits="Emp_PlaceOfResidence_List, HROneWeb.deploy" %>
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
    <col width="75px" />
    <col width="75px" />
    <col width="300px" />
    <col width="100px" />
    <tr>
        <td class="pm_list_header" align="center">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                    </asp:Panel>            
        </td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_EmpPoRFrom" OnClick="ChangeOrder_Click" Text="From"></asp:LinkButton></td>
        <td align="left" class="pm_list_header" >
            <asp:LinkButton runat="server" ID="_EmpPoRTo" OnClick="ChangeOrder_Click" Text="To"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpPoRPropertyAddr" OnClick="ChangeOrder_Click" Text="Address"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpPoRNature" OnClick="ChangeOrder_Click" Text="Nature"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpPoRLandLord" OnClick="ChangeOrder_Click" Text="Landlord"></asp:LinkButton></td>

    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
                </td>
                <td class="pm_list">
		    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_Residence_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID") + "&EmpPoRID=" + sbinding.getValue(Container.DataItem,"EmpPoRID"))%>">
                    <%#sbinding.getFValue(Container.DataItem, "EmpPoRFrom")%>
		    </a>
                </td>
                <td class="pm_list">
                    <%#sbinding.getFValue(Container.DataItem,"EmpPoRTo")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem,"EmpPoRPropertyAddr")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "EmpPoRNature")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "EmpPoRLandLord")%>
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
  />