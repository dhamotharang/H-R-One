<%@ page language="C#" autoeventwireup="true" inherits="Taxation_PaymentMapping_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">

    <input type="hidden" id="TaxPayID" runat="server" name="ID" />
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Taxation Category Mapping" />
                </td>
            </tr>
        </table>
     <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                <asp:Label runat="server" Text="Taxation Category Mapping" />
            </td>
        </tr>
    </table>
    
        
            
    
    <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
        <tr>
            <td>
                <tb:DetailToolBar id="toolBar" runat="server"
                 NewButton_Visible="false" 
                 EditButton_Visible="false" 
                 DeleteButton_Visible="false" 
                 OnBackButton_Click="Back_Click"
                 OnSaveButton_Click ="Save_Click"
                  />
            </td>
            <td align="right">
        <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
            </td>
        </tr>
    </table>
    <table width="100%" class="pm_section">
        <tr>
            <td class="pm_field_header">
                <asp:Label runat="server" Text="Taxation Form Type" />:
            </td>
            <td class="pm_field">IR56
                <asp:Label ID="TaxFormType" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label runat="server" Text="Taxation Payment Type" />:
            </td>
            <td class="pm_field">
                <asp:Label ID="TaxPayCode" runat="server" />
                <asp:TextBox ID="TaxPayNature" runat="server"></asp:TextBox>
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
    <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="toolbar"/>
    </Triggers>
    <ContentTemplate >
            <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
                <col width="26px" />
                <col width="200px" />
                <tr>
                    <td class="pm_list_header">
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_PaymentCode" OnClick="ChangeOrder_Click" Text="Code"/>
                    </td>
                    <td align="left" class="pm_list_header">
                        <asp:LinkButton runat="server" ID="_PaymentCodeDesc" OnClick="ChangeOrder_Click" Text="Description"/>
                    </td>
                </tr>
                <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_list" align="center">
                                <asp:CheckBox ID="ItemSelect" runat="server" />
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "PaymentCode")%>
                            </td>
                            <td class="pm_list">
                                <%#sbinding.getValue(Container.DataItem, "PaymentCodeDesc")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
                <tr>
                    <td align="right">
                        <tb:RecordListFooter id="ListFooter" runat="server"
                            ShowAllRecords="true" visible="true" 
                          />
                    </td>
                </tr>
            </table>
    </ContentTemplate>
    </asp:UpdatePanel> 


</asp:Content> 