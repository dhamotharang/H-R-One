<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Emp_Import.aspx.cs" Inherits="Emp_Import" MasterPageFile="~/MainMasterPage.master" EnableViewState="false"  %>

<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">

        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="Import Employee Information" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel2" Text="Import Details" runat="server" />
                </td>
            </tr>
        </table>
        
            
                
        
        <table class="pm_section" width="100%">
            <tr>
                <td class="pm_search_header">
                    <asp:Label runat="server" Text="Upload file path" /></td>
                <td class="pm_search">
                    <asp:FileUpload ID="EmpImportFile" runat="server" Width="400" />
                </td>
            </tr>
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="FileFormat" runat="server" Text="File Format" />
                </td>
                <td class="pm_search">
                    <asp:DropDownList ID="FileFormatList" runat="server" >
                        <asp:ListItem Text="Default" Value="Default"  />
                        <asp:ListItem Text="EZ-Pay" Value="EZ-Pay" Enabled="false" />
                    </asp:DropDownList> 
                </td>
            </tr>
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label1" runat="server" Text="Password for ZIP file" />
                </td>
                <td class="pm_search">
                    <asp:TextBox ID="ZipPassword" runat="server" TextMode="Password" />
                </td>
            </tr>
            <tr>
                <td class="pm_search_header">
                </td>
                <td class="pm_search">
                    <asp:CheckBox ID="chkAutoCreateCode" runat="server" Text="Create the code required automatically" />
                </td>
            </tr>
            <tr>
                <td class="pm_search" colspan="2">
                    <asp:Button ID="Upload" runat="server" Text="Upload" OnClick="Upload_Click" CssClass="button"/>
                </td>
            </tr>
        </table>
        <asp:Panel ID="ImportSection" runat="server" Visible="false" >
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <tr>
                    <td class="pm_list_header">
                <%--
                        <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                    --%>
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" /></td>
                    <td align="left" class="pm_list_header" colspan="2">
                        <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" /></td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" /></td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="LinkButton1" OnClick="ChangeOrder_Click" Text="Action" /></td>
             </tr>
                <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                    ShowFooter="false" ShowHeader="false" OnItemDataBound="Repeater_ItemDataBound" >
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list" align="center">
                        <%--
                                <asp:CheckBox ID="ItemSelect" runat="server" />
                            --%>
                                <input type="hidden" runat="server" id="UploadEmpID" />
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"EmpNo")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"EmpEngSurname")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"EmpEngOtherName")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"EmpAlias")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"ImportAction")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:DataList>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
                <tr>
                <td>
                    <asp:Button ID="Import" Text="Import" runat="server" OnClick="Import_Click" CssClass="button"/>
                </td>
                <td>
                    <tb:RecordListFooter id="ListFooter" runat="server"
                        ShowAllRecords="true" 
                      />
                </td>
                </tr>
            </table> 
        </asp:Panel>
</asp:Content> 