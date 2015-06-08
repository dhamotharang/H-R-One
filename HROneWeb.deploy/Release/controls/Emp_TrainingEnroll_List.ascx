<%@ control language="C#" autoeventwireup="true" inherits="Emp_TrainingEnroll_List, HROneWeb.deploy" %>
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
                     NewButton_Visible="false" 
                     DeleteButton_Visible="false" 
                     OnNewButton_Click ="New_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
            </tr>
        </table>
<table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
    <col width="26px" />
    <col width="200px" />
    <col width="100px" />
    <col width="100px" />
    <col width="100px" />
    <tr>
        <td class="pm_list_header" align="center">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                    </asp:Panel>     
        </td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_TrainingCourseID" OnClick="ChangeOrder_Click" Text="Training Course"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_TrainingSeminarDateFrom" OnClick="ChangeOrder_Click" Text="From"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_TrainingSeminarDateTo" OnClick="ChangeOrder_Click" Text="To"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_TrainingSeminarDuration" OnClick="ChangeOrder_Click" Text="Duration"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_TrainingSeminarTrainer" OnClick="ChangeOrder_Click" Text="Trainer"></asp:LinkButton></td>
        
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "TrainingCourseName")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getFValue(Container.DataItem, "TrainingSeminarDateFrom", "yyyy-MM-dd")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getFValue(Container.DataItem, "TrainingSeminarDateTo", "yyyy-MM-dd")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "TrainingSeminarDuration")%>
                    <%#sbinding.getValue(Container.DataItem, "TrainingSeminarDurationUnit")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "TrainingSeminarTrainer")%>
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
