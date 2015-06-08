<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attendance_RosterTable_Import.aspx.cs"    Inherits="Attendance_RosterTable_Import" MasterPageFile="~/MainMasterPage.master" EnableViewState="false" %>

<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="Import Roster Table" runat="server" />
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
                    <asp:FileUpload ID="CNDImportFile" runat="server" Width="400" />
                    <asp:Button ID="Upload" runat="server" Text="Upload" OnClick="Upload_Click"  CssClass="button"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:PostBackTrigger ControlID="Upload" />
            </Triggers>
            <ContentTemplate>
        <asp:Panel ID="ImportSection" runat="server" Visible="false" >
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="50px" />
                <col width="100px" />
                <tr>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" /></td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpName" OnClick="ChangeOrder_Click" Text="Name" /></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster01" Text="1"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster02" Text="2"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster03" Text="3"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster04" Text="4"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster05" Text="5"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster06" Text="6"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster07" Text="7"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster08" Text="8"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster09" Text="9"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster10" Text="10"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster11" Text="11"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster12" Text="12"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster13" Text="13"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster14" Text="14"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster15" Text="15"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster16" Text="16"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster17" Text="17"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster18" Text="18"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster19" Text="19"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster20" Text="20"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster21" Text="21"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster22" Text="22"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster23" Text="23"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster24" Text="24"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster25" Text="25"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster26" Text="26"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster27" Text="27"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster28" Text="28"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster29" Text="29"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster30" Text="30"/></td>
                    <td align="left" class="pm_list_header">
                        <asp:Label runat="server" ID="_Roster31" Text="31"/></td>
                </tr>
                <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                    ShowFooter="false" ShowHeader="false" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list">
                                    <%#sbinding.getValue(Container.DataItem,"EmpNo")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"EmpName")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster01")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster02")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster03")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster04")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster05")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster06")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster07")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster08")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster09")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster10")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster11")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster12")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster13")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster14")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster15")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster16")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster17")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster18")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster19")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster20")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster21")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster22")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster23")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster24")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster25")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster26")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster27")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster28")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster29")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster30")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"Roster31")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:DataList>
                <tr>
                <td colspan="5"><asp:Button ID="Import" Text="Import" runat="server" OnClick="Import_Click" CssClass="button"/>
                    <tb:RecordListFooter id="ListFooter" runat="server"
                        ShowAllRecords="true" Visible="false" 
                      />
                </td>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content> 