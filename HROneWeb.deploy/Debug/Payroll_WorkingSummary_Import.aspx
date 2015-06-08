<%@ page language="C#" autoeventwireup="true" inherits="Payroll_WorkingSummary_Import, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" enableviewstate="false" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="Import Employee Working Summary" runat="server" />
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
                <td class="pm_search">
                    
                </td>
                <td>
                    <asp:Label ID="connString" runat="server" />
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
                <col width="26px" />
                <col width="50px" />
                <col width="50px" />
                <col width="100px" />
                <col width="50px" />
                <col width="75px" />
                <col width="100px" />
                <col width="100px" />
                <col width="100px" />
                <col width="100px" />
                <col width="100px" />
                <tr>
                    <td class="pm_list_header" >
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" /></td>
                    <td align="left" class="pm_list_header" colspan="2">
                        <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" /></td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" /></td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpWorkingSummaryAsOfDate" OnClick="ChangeOrder_Click" Text="As Of Date" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpWorkingSummaryRestDayEntitled" OnClick="ChangeOrder_Click" Text="Rest Day Entitled" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpWorkingSummaryRestDayTaken" OnClick="ChangeOrder_Click" Text="Rest Day Taken" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpWorkingSummaryTotalWorkingDays" OnClick="ChangeOrder_Click" Text="Total Working Day(s)" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpWorkingSummaryTotalWorkingHours" OnClick="ChangeOrder_Click" Text="Total Hours Worked" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpWorkingSummaryTotalLunchTimeHours" OnClick="ChangeOrder_Click" Text="Total Hours for Meal Break" />
                    </td>
                </tr>
                <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                    ShowFooter="false" ShowHeader="false" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list" align="center" >
                        <%--
                                <asp:CheckBox ID="ItemSelect" runat="server" />
                            --%>
                            </td>
                            <td class="pm_list">
                                <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Payroll_CND_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID"))%>">
                                    <%#sbinding.getValue(Container.DataItem,"EmpNo")%>
                                </a>
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
                                <%#sbinding.getFValue(Container.DataItem, "EmpWorkingSummaryAsOfDate", "yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getFValue(Container.DataItem, "EmpWorkingSummaryRestDayEntitled", "0.00")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getFValue(Container.DataItem, "EmpWorkingSummaryRestDayTaken", "0.00")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getFValue(Container.DataItem, "EmpWorkingSummaryTotalWorkingDays", "0.00")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getFValue(Container.DataItem, "EmpWorkingSummaryTotalWorkingHours", "0.00")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getFValue(Container.DataItem, "EmpWorkingSummaryTotalLunchTimeHours", "0.00")%>
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
                    <td align="right">
                        <tb:RecordListFooter id="ListFooter" runat="server"
                            ShowAllRecords="true"
                          />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 