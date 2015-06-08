<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Leave_BalanceAdjustment_Import.aspx.cs"    Inherits="Leave_BalanceAdjustment_Import" MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="Import Leave Balance Adjustment" runat="server" />
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
                    <asp:Label runat="server" Text="Upload file path" />:</td>
                <td class="pm_search">
                    <asp:FileUpload ID="ImportFile" runat="server" Width="400" />
                    <asp:Button ID="Upload" runat="server" Text="Upload" OnClick="Upload_Click"  CssClass="button"/>
                    <asp:Button ID="btnExportTemplate" runat="server" Text="Export Template" OnClick="btnExportTemplate_Click"  CssClass="button"/>
                </td>
                <td class="pm_search">
                </td>
                <td>
                    <asp:Label ID="connString" runat="server" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="ImportSection" runat="server" Visible="false" >
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <tr>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" /></td>
                    <td align="left" class="pm_list_header"  colspan="2">
                        <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" /></td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" /></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_LeaveBalAdjDate" Text="Adjust Date"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_LeaveTypeID" Text="Leave Type"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_LeaveBalAdjType" Text="Adjust Type"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_LeaveBalAdjValue" Text="Adjust Value"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_LeaveBalAdjRemark" Text="Remark"/></td>
                </tr>
                <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list">
                                    <%#sbinding.getValue(Container.DataItem,"EmpNo")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "EmpEngSurname")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "EmpEngOtherName")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "EmpAlias")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getFValue(Container.DataItem, "LeaveBalAdjDate","yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "LeaveTypeID")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "LeaveBalAdjType")%>
                            </td>
                            <td class="pm_list" style="text-align:right">
                                <%#sbinding.getFValue(Container.DataItem, "LeaveBalAdjValue","0.####")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "LeaveBalAdjRemark")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                <td colspan="16">
                    <asp:Button ID="Import" Text="Import" runat="server" OnClick="Import_Click" CssClass="button"/>                    
                    <tb:RecordListFooter id="ListFooter" runat="server"
                        ShowAllRecords="true" Visible="false" 
                      />
                </td>
                </tr>
            </table>
        </asp:Panel>
</asp:Content> 