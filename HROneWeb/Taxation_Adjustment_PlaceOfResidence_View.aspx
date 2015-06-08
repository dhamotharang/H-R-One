<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Taxation_Adjustment_PlaceOfResidence_View.aspx.cs"    Inherits="Taxation_Adjustment_PlaceOfResidence_View" MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/Taxation_Emp_Header.ascx" TagName="Taxation_Emp_Header"
    TagPrefix="uc1" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">

        <input type="hidden" id="TaxEmpID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Taxation Adjustment" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Taxation Place of Residence Detail" />
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
                                    <asp:Button ID="Back" runat="server" Text="- Back -" OnClick="Back_Click" UseSubmitBehavior="false" CSSClass="button" />

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
            <col width="100px" /> 
            <col width="100px" /> 
            <col width="100px" /> 
            <tr>
                <td class="pm_list_header">
                </td>
                <td class="pm_list_header">
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_TaxEmpPlacePeriodFr" OnClick="ChangeOrder_Click"
                        Text="From" />
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_TaxEmpPlacePeriodTo" OnClick="ChangeOrder_Click"
                        Text="To" />
                </td>
                <td align="left" class="pm_list_header" colspan="2">
                    <asp:LinkButton runat="server" ID="_TaxEmpPlaceAddress" OnClick="ChangeOrder_Click"
                        Text="Detail" />
                </td>
            </tr>
            <asp:Panel ID="TaxEmpPoRAddPanel" runat="server">
                <tr>
                    <td class="pm_list_alt_row" rowspan="6">
                    </td>
                    <td class="pm_list_alt_row" rowspan="6" align="center">
                        <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" />
                    </td>
                    <td class="pm_list_alt_row" rowspan="6">
                        <uc1:WebDatePicker id="TaxEmpPlacePeriodFr" runat="server" ShowDateFormatLabel="false" />
                    </td>
                    <td class="pm_list_alt_row" rowspan="6">
                        <uc1:WebDatePicker id="TaxEmpPlacePeriodTo" runat="server" ShowDateFormatLabel="false" />
                    </td>
                    <td class="pm_list_alt_row">
                        <asp:Label runat="server" Text="Address" />:
                    </td>
                    <td class="pm_list_alt_row">
                        <asp:TextBox ID="TaxEmpPlaceAddress" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="pm_list_alt_row">
                        <asp:Label runat="server" Text="Nature" />:
                    </td>
                    <td class="pm_list_alt_row">
                        <asp:TextBox ID="TaxEmpPlaceNature" runat="server" MaxLength="19" />
                    </td>
                </tr>
                <tr>
                    <td class="pm_list_alt_row">
                        <asp:Label runat="server" Text="Rent Paid to Landlord by Employer" />:
                    </td>
                    <td class="pm_list_alt_row">
                        <asp:TextBox ID="TaxEmpPlaceERRent" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="pm_list_alt_row">
                        <asp:Label runat="server" Text="Rent Paid to Landlord by Employee" />:
                    </td>
                    <td class="pm_list_alt_row">
                        <asp:TextBox ID="TaxEmpPlaceEERent" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="pm_list_alt_row">
                        <asp:Label runat="server" Text="Rent Refunded To Employee" />:
                    </td>
                    <td class="pm_list_alt_row">
                        <asp:TextBox ID="TaxEmpPlaceEERentRefunded" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="pm_list_alt_row">
                        <asp:Label runat="server" Text="Rent Paid to Employer By Employee" />:
                    </td>
                    <td class="pm_list_alt_row">
                        <asp:TextBox ID="TaxEmpPlaceERRentByEE" runat="server" />
                    </td>
                </tr>
            </asp:Panel>
            <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                ShowFooter="false" ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
                <EditItemTemplate>
                    <tr>
                        <td class="pm_list_edit" rowspan="6" align="center">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="TaxEmpPlaceID" />
                        </td>
                        <td class="pm_list_edit" rowspan="6" align="center">
                            <asp:Button ID="Save" CssClass="button" runat="server" Text="Save" />
                            <asp:Button ID="Cancel" CssClass="button" runat="server" Text="Cancel" />
                        </td>
                        <td class="pm_list_edit" rowspan="6">
                            <uc1:WebDatePicker id="TaxEmpPlacePeriodFr" runat="server" ShowDateFormatLabel="false" />
                        </td>
                        <td class="pm_list_edit" rowspan="6">
                            <uc1:WebDatePicker id="TaxEmpPlacePeriodTo" runat="server" ShowDateFormatLabel="false" />
                        </td>
                        <td class="pm_list_edit">
                            <asp:Label runat="server" Text="Address" />:
                        </td>
                        <td class="pm_list_edit">
                            <asp:TextBox ID="TaxEmpPlaceAddress" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list_edit">
                            <asp:Label runat="server" Text="Nature" />:
                        </td>
                        <td class="pm_list_edit">
                            <asp:TextBox ID="TaxEmpPlaceNature" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list_edit">
                            <asp:Label runat="server" Text="Rent Paid to Landlord by Employer" />:
                        </td>
                        <td class="pm_list_edit">
                            <asp:TextBox ID="TaxEmpPlaceERRent" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list_edit">
                            <asp:Label runat="server" Text="Rent Paid to Landlord by Employee" />:
                        </td>
                        <td class="pm_list_edit">
                            <asp:TextBox ID="TaxEmpPlaceEERent" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list_edit">
                            <asp:Label runat="server" Text="Rent Refunded To Employee" />:
                        </td>
                        <td class="pm_list_edit">
                            <asp:TextBox ID="TaxEmpPlaceEERentRefunded" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list_edit">
                            <asp:Label runat="server" Text="Rent Paid to Employer By Employee" />:
                        </td>
                        <td class="pm_list_edit">
                            <asp:TextBox ID="TaxEmpPlaceERRentByEE" runat="server" />
                        </td>
                    </tr>
                </EditItemTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center" rowspan="6">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="TaxEmpPlaceID" />
                        </td>
                        <td class="pm_list" align="center" rowspan="6">
                            <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                        </td>
                        <td class="pm_list" rowspan="6">
                            <%#sbinding.getFValue(Container.DataItem, "TaxEmpPlacePeriodFr", "yyyy-MM-dd")%>
                        </td>
                        <td class="pm_list" rowspan="6">
                            <%#sbinding.getFValue(Container.DataItem, "TaxEmpPlacePeriodTo", "yyyy-MM-dd")%>
                        </td>
                        <td class="pm_list">
                            <asp:Label runat="server" Text="Address" />:
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem, "TaxEmpPlaceAddress")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list">
                            <asp:Label runat="server" Text="Nature" />:
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem, "TaxEmpPlaceNature")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list">
                            <asp:Label runat="server" Text="Rent Paid to Landlord by Employer" />:
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getFValue(Container.DataItem, "TaxEmpPlaceERRent", "$#,##0.00")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list">
                            <asp:Label runat="server" Text="Rent Paid to Landlord by Employee" />:
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getFValue(Container.DataItem, "TaxEmpPlaceEERent", "$#,##0.00")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list">
                            <asp:Label runat="server" Text="Rent Refunded To Employee" />:
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getFValue(Container.DataItem, "TaxEmpPlaceEERentRefunded", "$#,##0.00")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list">
                            <asp:Label runat="server" Text="Rent Paid to Employer By Employee" />:
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getFValue(Container.DataItem, "TaxEmpPlaceERRentByEE", "$#,##0.00")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr>
                        <td class="pm_list_alt_row" align="center" rowspan="6">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="TaxEmpPlaceID" />
                        </td>
                        <td class="pm_list_alt_row" align="center" rowspan="6">
                            <asp:Button ID="Edit" CssClass="button" runat="server" Text="Edit" />
                        </td>
                        <td class="pm_list_alt_row" rowspan="6">
                            <%#sbinding.getFValue(Container.DataItem, "TaxEmpPlacePeriodFr", "yyyy-MM-dd")%>
                        </td>
                        <td class="pm_list_alt_row" rowspan="6">
                            <%#sbinding.getFValue(Container.DataItem, "TaxEmpPlacePeriodTo", "yyyy-MM-dd")%>
                        </td>
                        <td class="pm_list_alt_row">
                            <asp:Label ID="Label2" runat="server" Text="Address" />:
                        </td>
                        <td class="pm_list_alt_row">
                            <%#sbinding.getValue(Container.DataItem, "TaxEmpPlaceAddress")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list_alt_row">
                            <asp:Label ID="Label3" runat="server" Text="Nature" />:
                        </td>
                        <td class="pm_list_alt_row">
                            <%#sbinding.getValue(Container.DataItem, "TaxEmpPlaceNature")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list_alt_row">
                            <asp:Label ID="Label4" runat="server" Text="Rent Paid to Landlord by Employer" />:
                        </td>
                        <td class="pm_list_alt_row">
                            <%#sbinding.getFValue(Container.DataItem, "TaxEmpPlaceERRent", "$#,##0.00")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list_alt_row">
                            <asp:Label ID="Label5" runat="server" Text="Rent Paid to Landlord by Employee" />:
                        </td>
                        <td class="pm_list_alt_row">
                            <%#sbinding.getFValue(Container.DataItem, "TaxEmpPlaceEERent", "$#,##0.00")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list_alt_row">
                            <asp:Label ID="Label6" runat="server" Text="Rent Refunded To Employee" />:
                        </td>
                        <td class="pm_list_alt_row">
                            <%#sbinding.getFValue(Container.DataItem, "TaxEmpPlaceEERentRefunded", "$#,##0.00")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list_alt_row">
                            <asp:Label ID="Label7" runat="server" Text="Rent Paid to Employer By Employee" />:
                        </td>
                        <td class="pm_list_alt_row">
                            <%#sbinding.getFValue(Container.DataItem, "TaxEmpPlaceERRentByEE", "$#,##0.00")%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:DataList>
        </table>
        <tb:RecordListFooter id="ListFooter" runat="server"
            ShowAllRecords="true" visible="true" 
          />
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content> 