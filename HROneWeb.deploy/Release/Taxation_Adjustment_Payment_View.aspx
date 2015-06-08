<%@ page language="C#" autoeventwireup="true" inherits="Taxation_Adjustment_Payment_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/Taxation_Emp_Header.ascx" TagName="Taxation_Emp_Header"
    TagPrefix="uc1" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">

        <input type="hidden" id="TaxEmpID" runat="server" name="ID" />
        <input type="hidden" id="TaxFormType" runat="server" name="TaxFormType" />
        
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Taxation Adjustment" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr  >
                <td>
                    <asp:Label Text="Taxation Payment Detail" runat="server" />
                    :
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
                        <tr>
                            <td>
                                                          <asp:Button ID="Back" runat="server" Text="- Back -" OnClick="Back_Click" UseSubmitBehavior="false"
                                    cssclass="button" />  

                            </td>
                            <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <uc1:Taxation_Emp_Header ID="ucTaxation_Emp_Header" runat="server" />
                </td>
            </tr>
        </table>
        
            
                
        
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar ID="toolBar" runat="server" NewButton_Visible="false" EditButton_Visible="false" BackButton_Visible="false" SaveButton_Visible="false" OnDeleteButton_Click="Delete_Click" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="toolBar" />
            </Triggers>
            <ContentTemplate>
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" /> 
            <col width="120px" /> 
            <col width="150px" /> 
            <tr>
                <td class="pm_list_header">
                </td>
                <td class="pm_list_header">
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_TaxPayID" OnClick="ChangeOrder_Click" Text="Payment" />
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_TaxEmpPayNature" OnClick="ChangeOrder_Click" Text="Nature" />
                </td>

                <asp:Panel ID="TaxFormBFGPanel1" runat="server">
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_TaxEmpPayPeriodFr" OnClick="ChangeOrder_Click" Text="From" />
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_TaxEmpPayPeriodTo" OnClick="ChangeOrder_Click" Text="To" />
                    </td>
                </asp:Panel>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_TaxEmpPayAmount" OnClick="ChangeOrder_Click" Text="Amount" />
                </td>
            </tr>
            <asp:Panel ID="TaxEmpPaymentAddPanel" runat="server">
                <tr>
                    <td class="pm_list">
                    </td>
                    <td class="pm_list" align="center">
                        <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" />
                    </td>
                    <td class="pm_list">
                        <asp:DropDownList ID="TaxPayID" runat="server" />
                    </td>
                    <td class="pm_list">
                    </td>
                    <asp:Panel ID="TaxFormBFGPanel2" runat="server">
                        <td class="pm_list" style="white-space:nowrap;" >
                            <uc1:WebDatePicker id="TaxEmpPayPeriodFr" runat="server" ShowDateFormatLabel="false" />
                        </td>
                        <td class="pm_list" style="white-space:nowrap;">
                            <uc1:WebDatePicker id="TaxEmpPayPeriodTo" runat="server" ShowDateFormatLabel="false" />
                        </td>
                    </asp:Panel>
                    <td class="pm_list" align="right">
                        <asp:TextBox ID="TaxEmpPayAmount" runat="server" Columns="10" style="text-align:right;" />
                    </td>
                </tr>
            </asp:Panel>
            <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                ShowFooter="false" ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
                <EditItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="TaxEmpPayID" />
                        </td>
                        <td class="pm_list" align="center">
                            <asp:Button ID="Save" CssClass="button" runat="server" Text="Save" />
                            <asp:Button ID="Cancel" CssClass="button" runat="server" Text="Cancel" />
                        </td>
                        <td class="pm_list">
                            <asp:DropDownList ID="TaxPayID" runat="server"/>
                        </td>
                        <td class="pm_list">
                            <asp:TextBox ID="TaxEmpPayNature" runat="server" />
                        </td>

                        <asp:Panel ID="TaxFormBFGPanel" runat="server">
                            <td class="pm_list" style="white-space:nowrap;">
                                <uc1:WebDatePicker id="TaxEmpPayPeriodFr" runat="server" ShowDateFormatLabel="false" />
                            </td>
                            <td class="pm_list" style="white-space:nowrap;">
                                <uc1:WebDatePicker id="TaxEmpPayPeriodTo" runat="server" ShowDateFormatLabel="false" />
                            </td>
                        </asp:Panel>
                        <td class="pm_list" align="right">
                            <asp:TextBox ID="TaxEmpPayAmount" runat="server" Columns="10" style="text-align:right;"/>
                        </td>
                    </tr>
                </EditItemTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="TaxEmpPayID" />
                        </td>
                        <td class="pm_list" align="center">
                            <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem, "TaxPayID")%>
                        </td>
                        <td class="pm_list">
                            <asp:Label ID="TaxEmpPayNature" runat="server" Text='<%#sbinding.getValue(Container.DataItem, "TaxEmpPayNature")%>' />
                        </td>
                        <asp:Panel ID="TaxFormBFGPanel" runat="server">
                            <td class="pm_list" style="white-space:nowrap;">
                                <%#sbinding.getFValue(Container.DataItem, "TaxEmpPayPeriodFr", "yyyy-MM-dd")%>
                            </td>
                            <td class="pm_list" style="white-space:nowrap;">
                                <%#sbinding.getFValue(Container.DataItem, "TaxEmpPayPeriodTo", "yyyy-MM-dd")%>
                            </td>
                        </asp:Panel>
                        <td class="pm_list" align="right">
                            <%#sbinding.getFValue(Container.DataItem, "TaxEmpPayAmount", "$#,##0")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:DataList>
        </table>
        <tb:RecordListFooter id="ListFooter" runat="server"
            ShowAllRecords="true" visible="true" 
          />
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content> 