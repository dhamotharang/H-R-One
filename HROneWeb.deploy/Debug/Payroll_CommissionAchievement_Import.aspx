<%@ page language="C#" autoeventwireup="true" inherits="Payroll_CommissionAchievement_Import, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" enableviewstate="false" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="Import Commission Achievement" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel2" Text="Import Details" runat="server" />
                </td>
            </tr>
            <tr>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnBack" runat="Server" Text="- Back -" CssClass="button" OnClick="btnBack_Click" />
                </td>
            </tr>
        </table>
        
            
                
        
        <table class="pm_section" width="100%">
            <tr>
                <td class="pm_search_header">
                    <asp:Label runat="server" Text="Upload file path" />:</td>
                <td class="pm_search">
                    <asp:FileUpload ID="CAImportFile" runat="server" Width="400" />
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
                <col width="100px" />
                <col width="200px" />
                <col width="120px" />
                <col width="140px" />
                <col width="75px" />
                <tr>
                    <td class="pm_list_header" >
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" />
                    </td>
                    <td align="left" class="pm_list_header"  colspan="2">
                        <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" /></td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" /></td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_CAEffDate" OnClick="ChangeOrder_Click" Text="As At Date" /></td>                       
                    <td align="right" class="pm_list_header"  >
                        <asp:LinkButton runat="server" ID="_CAPercent" OnClick="ChangeOrder_Click" Text="Achievement %" />
                    </td>
                    <td class="pm_list_header"></td>
                </tr>
                <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                    ShowFooter="false" ShowHeader="false" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr id="detailRow" runat="server">
                            <td class="pm_list" align="center">
                                <input type="hidden" runat="server" id="UploadCAID" />
                            </td>
                            <td class="pm_list">
                                <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Payroll_CommissionAchievement_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID"))%>">
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
                                <%#sbinding.getFValue(Container.DataItem,"CAEffDate","yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getFValue(Container.DataItem, "CAPercent", "#,##0.00")%>
                            </td>
                            <td class="pm_list" ></td>
                        </tr>
                    </ItemTemplate>
                    <%--<AlternatingItemTemplate>
                        <tr id="detailRow" runat="server">
                            <td class="pm_list_alt_row" align="center" rowspan="2">
                                <input type="hidden" runat="server" id="UploadCAID" />
                            </td>
                            <td class="pm_list_alt_row">
                                <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Payroll_CommissionAchievement_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID"))%>">
                                    <%#sbinding.getValue(Container.DataItem,"EmpNo")%>
                                </a>
                            </td>
                            <td class="pm_list_alt_row">
                                <%#sbinding.getValue(Container.DataItem,"EmpEngSurname")%>
                            </td>
                            <td class="pm_list_alt_row">
                                <%#sbinding.getValue(Container.DataItem,"EmpEngOtherName")%>
                            </td>
                            <td class="pm_list_alt_row">
                                <%#sbinding.getValue(Container.DataItem,"EmpAlias")%>
                            </td>
                            <td class="pm_list_alt_row" align="right">
                                <%#sbinding.getFValue(Container.DataItem, "CAPercent", "#,##0.00%")%>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>--%>
                </asp:DataList>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Remark" ForeColor="black" />
                        <asp:TextBox ID="txtRemark" runat="server" Columns="50" />
                        <asp:Button ID="Import" Text="Import" runat="server" OnClick="Import_Click" CssClass="button"/>
                    </td>

                    <td align="right">
                    <tb:RecordListFooter id="ListFooter" runat="server"
                        OnFirstPageClick="ChangePage"
                        OnPrevPageClick="ChangePage"
                        OnNextPageClick="ChangePage"
                        OnLastPageClick="ChangePage"
                      />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 