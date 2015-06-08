<%@ page language="C#" autoeventwireup="true" inherits="Emp_List, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server"> 
    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
        <tr>
            <td>
                <asp:Label runat="server" Text="Employee Setup" />
            </td>
        </tr>
    </table>
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label ID="Label12" runat="server" Text="Employee Search" />
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
        <tr>
            <td>
                <asp:Button ID="Search" runat="server" Text="Search" CssClass="button" OnClick="Search_Click" />
                <asp:Button ID="Reset" runat="server" Text="Reset" CssClass="button" OnClick="Reset_Click" />
            </td>
        </tr>
    </table>

    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label runat="server" Text="Employee List" />
            </td>
        </tr>
    </table>
        
    <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
        <tr>
            <td>
                <tb:DetailToolBar id="toolBar" runat="server"
                 BackButton_Visible="false"
                 EditButton_Visible="false" 
                 SaveButton_Visible="false" 
                 OnNewButton_Click ="New_Click"
                 OnDeleteButton_Click="Delete_Click" />
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
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>

        <ContentTemplate >
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
<%--                <col width="26px" />
                <col width="100px" />
                <col width="100px" />
                <col width="100px"/>
                <col width="150px" />
                <col width="100px" />
--%>            
                <col width="25px" />  <%--checkbox --%>
                <col width="12%" />   <%--Emp No --%>
                <col width="14%" />   <%--Name --%>
                <col width="18%" />   <%--Name 2 --%>
                <col width="12%" />   <%--Alias --%>
                <col width="15%" />   <%--Company --%>
                <col width="10%" />   <%--DIV --%>
                <col width="10%" />   <%--DEP --%>
                <col width="10%" />   <%--TEAM --%>
                
    
                <tr>
                    <td class="pm_list_header" align="center">
                        <asp:Panel ID="SelectAllPanel" runat="server">
                            <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                        </asp:Panel> 
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" />
                    </td>
                    <td align="left" class="pm_list_header" colspan="2" >
                        <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" />
                    </td>

                    <td align="left" class="pm_list_header" runat="server" id="col_company">
                        <asp:Label runat="server" ID="_Company" Text="Company" />
                        <%-- asp:LinkButton runat="server" ID="_Company" OnClick="ChangeOrder_Click" Text="Company" / --%>
                    </td>
                    <td align="left" class="pm_list_header" runat="server" id="col_h1">
                        <asp:Label runat="server" ID="_Hierarchy1" Text="H1" />
                        <%-- asp:LinkButton runat="server" ID="_Hierarchy1" OnClick="ChangeOrder_Click" Text="H1" /--%>
                    </td>
                    <td align="left" class="pm_list_header" runat="server" id="col_h2">
                        <asp:Label runat="server" ID="_Hierarchy2" Text="H2" />
                        <%-- asp:LinkButton runat="server" ID="_Hierarchy2" OnClick="ChangeOrder_Click" Text="H2" /--%>
                    </td>
                    <td align="left" class="pm_list_header" runat="server" id="col_h3">
                        <asp:Label runat="server" ID="_Hierarchy3" Text="H3" />
                        <%-- asp:LinkButton runat="server" ID="_Hierarchy3" OnClick="ChangeOrder_Click" Text="H3" /--%>
                    </td>

                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpStatus" OnClick="ChangeOrder_Click" Text="Status" />
                    </td>                    
                </tr>
                <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list" align="center">
                                <input type="hidden" runat="server" id="EmpID" />
                                <asp:CheckBox ID="ItemSelect" runat="server" />
                            </td>
                            <td class="pm_list">
                                <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_View.aspx?EmpID=" + binding.getValue(Container.DataItem,"EmpID"))%>"><%#binding.getValue(Container.DataItem,"EmpNo")%></a>
                            </td>
                            <td class="pm_list" >
                                <%#binding.getValue(Container.DataItem,"EmpEngSurname")%>
                            </td>
                            <td class="pm_list">
                                <%#binding.getValue(Container.DataItem,"EmpEngOtherName")%>
                            </td>
                            <td class="pm_list">
                                <%#binding.getValue(Container.DataItem,"EmpAlias")%>
                            </td>
                            <td class="pm_list" align="left">
                                <asp:Label id="EmpCompany" runat="server" />
                           </td>
                            <td class="pm_list" align="left">
                                <asp:Label id="EmpHierarchy1" runat="server" />
                            </td>
                            <td class="pm_list" align="left">
                                <asp:Label ID="EmpHierarchy2" runat="server" />
                            </td>
                            <td class="pm_list" align="left">
                                <asp:Label ID="EmpHierarchy3" runat="server" />
                            </td>
                            
                            <td class="pm_list">
                                <%#binding.getValue(Container.DataItem,"EmpStatus")%>
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
                ListOrderBy="EmpNo"
                ListOrder="true" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>