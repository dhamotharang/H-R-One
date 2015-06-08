<%@ control language="C#" autoeventwireup="true" inherits="Emp_Spouse_List, HROneWeb.deploy" %>
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
    <col width="200px" />
    <col width="150px" />
    <col width="100px" />
    <col width="50px" />
    <tr>
        <td class="pm_list_header" align="center">
                                <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                    </asp:Panel>     
        </td>

        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpSpouseSurname" OnClick="ChangeOrder_Click" Text="Surname"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpSpouseOtherName" OnClick="ChangeOrder_Click" Text="Other Name"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpSpouseChineseName" OnClick="ChangeOrder_Click" Text="Chinese Name"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:LinkButton runat="server" ID="_EmpSpouseHKID" OnClick="ChangeOrder_Click" Text="HKID"></asp:LinkButton></td>
        <td align="left" class="pm_list_header">
            <asp:Label runat="server" ID="EmpSpouseAge" Text="Age" /></td>
    </tr>
    <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td class="pm_list" align="center">
                    <asp:CheckBox ID="ItemSelect" runat="server" />
                </td>
                <td class="pm_list">
                    <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_Spouse_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID") + "&EmpSpouseID=" + sbinding.getValue(Container.DataItem,"EmpSpouseID"))%>">
                        <%#sbinding.getValue(Container.DataItem,"EmpSpouseSurname")%>
                    </a>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "EmpSpouseOtherName")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "EmpSpouseChineseName")%>
                </td>
                <td class="pm_list">
                    <%#sbinding.getValue(Container.DataItem, "EmpSpouseHKID")%>
                </td>
                <td class="pm_list">
                    <%# AppUtils.GetAge(Eval("EmpSpouseDateOfBirth")).ToString("0.000")%>
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