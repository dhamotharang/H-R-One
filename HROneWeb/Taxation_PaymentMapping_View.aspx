<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Taxation_PaymentMapping_View.aspx.cs"    Inherits="Taxation_PaymentMapping_View" MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="lblReportHeader" runat="server" Text="Taxation Category Mapping" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Taxation Category Mapping" />
                    <asp:Button ID="btnPrint" runat="server" Text="Print Mapping List" OnClick="btnPrint_Click" CssClass="button" />
                </td>
            </tr>
        </table>
        <table width="100%" class="pm_section">
            <col width="20%" />
            <col width="80%" />
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Taxation Form Type" />:
                </td>
                <td class="pm_field">IR56
                    <asp:DropDownList ID="TaxFormType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="TaxFormType_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Taxation Payment Type" />:
                </td>
                <td class="pm_field">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional"  >
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="TaxFormType" />
                    </Triggers>
                    <ContentTemplate>
                    <asp:DropDownList ID="TaxPayID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="TaxPayID_SelectedIndexChanged" Width="90%" />
                <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     SaveButton_Visible="false" 
                     BackButton_Visible="false" 
                     OnEditButton_Click ="Edit_Click"
                     DeleteButton_Visible="false" 
                      />
                    </ContentTemplate>
                    </asp:UpdatePanel> 
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Payment List" />
                </td>
            </tr>
        </table>
        <%-- use 1 update panel to prevent bug for refresh footer list everytime --%>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
            <asp:AsyncPostBackTrigger ControlID="TaxFormType" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="TaxPayID" EventName="SelectedIndexChanged" />
        </Triggers>
        <ContentTemplate >
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="150px" />
            <col width="700px" />
            <tr>
                <td class="pm_list_header" >
                </td>
                <td align="left" class="pm_list_header" >
                    <asp:LinkButton runat="server" ID="_PaymentCode" OnClick="paymentMappedChangeOrder_Click" Text="Code"/></td>
                <td align="left" class="pm_list_header" >
                    <asp:LinkButton runat="server" ID="_PaymentCodeDesc" OnClick="paymentMappedChangeOrder_Click" Text="Description"/></td>
            </tr>
            <asp:Repeater ID="paymentMappedRepeater" runat="server" OnItemDataBound="paymentMappedRepeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" Visible="False" />
                        </td>
                        <td class="pm_list">
                            <%#taxFormSBinding.getValue(Container.DataItem, "PaymentCode")%>
                        </td>
                        <td class="pm_list">
                            <%#taxFormSBinding.getValue(Container.DataItem, "PaymentCodeDesc")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <tb:RecordListFooter id="paymentMappedListFooter" runat="server"
            OnFirstPageClick="paymentMappedFirstPage_Click"
            OnPrevPageClick="paymentMappedPrevPage_Click"
            OnNextPageClick="paymentMappedNextPage_Click"
            OnLastPageClick="paymentMappedLastPage_Click"
          />
          <br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Payment not mapped to any taxation payment" />
                </td>
            </tr>
        </table>
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" />
            <col width="150px" />
            <col width="700px" />
            <tr>
                <td class="pm_list_header" >
                </td>
                <td align="left" class="pm_list_header" >
                    <asp:LinkButton runat="server" ID="paymentNotMapped_PaymentCode" OnClick="paymentNotMappedChangeOrder_Click" Text="Code"/></td>
                <td align="left" class="pm_list_header" >
                    <asp:LinkButton runat="server" ID="paymentNotMapped_PaymentCodeDesc" OnClick="paymentNotMappedChangeOrder_Click" Text="Description"/></td>
            </tr>
            <asp:Repeater ID="paymentNotMappedRepeater" runat="server" OnItemDataBound="paymentNotMappedRepeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" Visible="False" />
                        </td>
                        <td class="pm_list">
                            <%#taxFormSBinding.getValue(Container.DataItem, "PaymentCode")%>
                        </td>
                        <td class="pm_list">
                            <%#taxFormSBinding.getValue(Container.DataItem, "PaymentCodeDesc")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <tb:RecordListFooter id="paymentNotMappedListFooter" runat="server"
            OnFirstPageClick="paymentNotMappedFirstPage_Click"
            OnPrevPageClick="paymentNotMappedPrevPage_Click"
            OnNextPageClick="paymentNotMappedNextPage_Click"
            OnLastPageClick="paymentNotMappedLastPage_Click"
          />
        </ContentTemplate>
        </asp:UpdatePanel> 
</asp:Content> 