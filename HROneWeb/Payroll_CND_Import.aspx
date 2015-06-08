<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_CND_Import.aspx.cs" Inherits="Payroll_CND_Import" MasterPageFile="~/MainMasterPage.master" EnableViewState="false" %>

<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="Import Claims and Deductions" runat="server" />
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
                <col width="75px" />
                <col width="75px" />
                <col width="100px" />
                <col width="100px" />
                <col width="50px" />
                <tr>
                    <td class="pm_list_header" >
                <%--
                        <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                    --%>
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="EmpNo" />
                    </td>
                    <td align="left" class="pm_list_header"  colspan="2">
                        <asp:LinkButton runat="server" ID="_EmpEngFullName" OnClick="ChangeOrder_Click" Text="Name" /></td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpAlias" OnClick="ChangeOrder_Click" Text="Alias" /></td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_CNDEffDate" OnClick="ChangeOrder_Click" Text="Date" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_PayCodeID" OnClick="ChangeOrder_Click" Text="Payment" />
                    </td>
                    <%--            
                <td align="left" class="pm_list_header" >
            </td>
            --%>
                    <td align="left" class="pm_list_header"  >
                        <asp:LinkButton runat="server" ID="_CNDAmount" OnClick="ChangeOrder_Click" Text="Amount" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_CNDPayMethod" OnClick="ChangeOrder_Click" Text="Method" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_EmpAccID" OnClick="ChangeOrder_Click" Text="Bank Account Number" />
                    </td>
                    <td id="CostCenterHeaderCell" runat="server" align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_CostCenterID" OnClick="ChangeOrder_Click" Text="Cost Center" />
                    </td>
                    <td align="left" class="pm_list_header" >
                        <asp:LinkButton runat="server" ID="_CNDNumOfDayAdj" OnClick="ChangeOrder_Click" Text="Days Adjust" />
                    </td>
                    <%--            
                <td align="left" class="pm_list_header">
                <asp:LinkButton runat="server" ID="_CNDRemark" OnClick="ChangeOrder_Click">Remark</asp:LinkButton>
            </td>
            --%>
                </tr>
                <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                    ShowFooter="false" ShowHeader="false" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr id="detailRow" runat="server">
                            <td class="pm_list" align="center" rowspan="2">
                        <%--
                                <asp:CheckBox ID="ItemSelect" runat="server" />
                            --%>
                                <input type="hidden" runat="server" id="UploadCNDID" />
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
                                <%#sbinding.getFValue(Container.DataItem,"CNDEffDate","yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem,"PayCodeID")%>
                            </td>
                            <td class="pm_list" align="right" visible="false" >
                                <%#sbinding.getValue(Container.DataItem, "CurrencyID")%>
                            </td>
                            <td class="pm_list" align="right">
                                <%#sbinding.getFValue(Container.DataItem, "CNDAmount", "$#,##0.00")%>
                            </td>
                            <td class="pm_list" align="center">
                                <%#sbinding.getValue(Container.DataItem, "CNDPayMethod")%>
                            </td>
                            <td class="pm_list" style="white-space:nowrap;">
                                <asp:Label ID="EmpAccID" runat="server" />
                            </td>
                            <td id="CostCenterDetailCell" runat="server" class="pm_list" >
                                <asp:Label ID="CostCenterID" runat="server" />
                            </td>
                            <td class="pm_list" align="right" >
                                <%#sbinding.getValue(Container.DataItem, "CNDNumOfDayAdj")%>
                            </td>
                        </tr>
                        <tr>
                            <td id="RemarkCell" runat="server" class="pm_list" align="left" colspan="9">
                                <asp:Label ID="Label2" runat="server" Text="Remark" />: &nbsp
                                <%#sbinding.getValue(Container.DataItem, "CNDRemark")%>
                            </td>
                            <td class="pm_list"  colspan="2">
                                <asp:Label ID="Label4" runat="server" Text="Rest Payment"/>?&nbsp
                                 <%#sbinding.getValue(Container.DataItem, "CNDIsRestDayPayment")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="detailRow" runat="server">
                            <td class="pm_list_alt_row" align="center" rowspan="2">
                        <%--
                                <asp:CheckBox ID="ItemSelect" runat="server" />
                            --%>
                                <input type="hidden" runat="server" id="UploadCNDID" />
                            </td>
                            <td class="pm_list_alt_row">
                                <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Payroll_CND_View.aspx?EmpID=" + sbinding.getValue(Container.DataItem,"EmpID"))%>">
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
                            <td class="pm_list_alt_row">
                                <%#sbinding.getFValue(Container.DataItem,"CNDEffDate","yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list_alt_row">
                                <%#sbinding.getValue(Container.DataItem,"PayCodeID")%>
                            </td>
                            <td class="pm_list_alt_row" align="right" visible="false" >
                                <%#sbinding.getValue(Container.DataItem, "CurrencyID")%>
                            </td>
                            <td class="pm_list_alt_row" align="right">
                                <%#sbinding.getFValue(Container.DataItem, "CNDAmount", "$#,##0.00")%>
                            </td>
                            <td class="pm_list_alt_row" align="center">
                                <%#sbinding.getValue(Container.DataItem, "CNDPayMethod")%>
                            </td>
                            <td class="pm_list_alt_row" style="white-space:nowrap;">
                                <asp:Label ID="EmpAccID" runat="server" />
                            </td>
                            <td id="CostCenterDetailCell" runat="server" class="pm_list_alt_row" >
                                <asp:Label ID="CostCenterID" runat="server" />
                            </td>
                            <td class="pm_list_alt_row" align="right" >
                                <%#sbinding.getValue(Container.DataItem, "CNDNumOfDayAdj")%>
                            </td>
                        </tr>
                        <tr>
                            <td id="RemarkCell" runat="server" class="pm_list_alt_row" align="left" colspan="9">
                                <asp:Label ID="Label2" runat="server" Text="Remark" />: &nbsp
                                <%#sbinding.getValue(Container.DataItem, "CNDRemark")%>
                            </td>
                            <td class="pm_list_alt_row"  colspan="2">
                                <asp:Label ID="Label4" runat="server" Text="Rest Payment"/>?&nbsp
                                 <%#sbinding.getValue(Container.DataItem, "CNDIsRestDayPayment")%>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
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