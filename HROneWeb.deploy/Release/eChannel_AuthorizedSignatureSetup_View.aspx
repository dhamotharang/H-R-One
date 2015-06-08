<%@ page language="C#" masterpagefile="~/MainMasterPage.master" autoeventwireup="true" inherits="eChannel_AuthorizedSignatureSetup_View, HROneWeb.deploy" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" Runat="Server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                   <asp:Label ID="Label1" EnableViewState="false" Text="Authorized Signature Setup" runat="server" />
                </td>
            </tr>
        </table>
    <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
        <tr>
            <td>
                <tb:DetailToolBar ID="toolBar" runat="server"
                 NewButton_Visible="false" 
                 OnEditButton_Click="Edit_Click" 
                 DeleteButton_Visible="false" 
                 BackButton_Visible="false"
                 SaveButton_Visible="false"
                  />
            </td>
            <td align="right">
            </td>
        </tr>
    </table>
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="25%" />
            <col width="75%" />
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="lblPARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_AUTOPAY" runat="server" EnableViewState="false" Text="Require signature for autopay instruction file" />
                </td>
                <td align="left" class="pm_field">
                    <asp:Label ID="chkPARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_AUTOPAY_FILE" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="Require signature for MPF file" />
                </td>
                <td align="left" class="pm_field">
                    <asp:Label ID="chkPARAM_CODE_ECHANNEL_SIGNATURE_REQUIRED_FOR_MPF_FILE" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="left" class="pm_field_header" >
                    <asp:Label ID="lblPARAM_CODE_ECHANNEL_NUM_SIGNATURE_REQUIRED" runat="server" EnableViewState="false" Text="Number of signature required" />
                </td>
                <td align="left" class="pm_field">
                    <asp:Label ID="txtPARAM_CODE_ECHANNEL_NUM_SIGNATURE_REQUIRED" runat="server" />
                </td>
            </tr>
        </table>

        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                   <asp:Label ID="Label4" EnableViewState="false" Text="User for Signature" runat="server" />
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
            <col width="150px" />
            <col width="200px" />
            <col width="150px" />
            <col width="100px" />
            <tr>
                <td class="pm_list_header" align="center">
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_LoginID" OnClick="ChangeOrder_Click" EnableViewState="false" Text="User ID"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_UserName" OnClick="ChangeOrder_Click" EnableViewState="false" Text="User Name"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_UserAccountStatus" OnClick="ChangeOrder_Click" EnableViewState="false" Text="Status"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_ExpiryDate" OnClick="ChangeOrder_Click" EnableViewState="false" Text="Expiry Date"/></td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" Visible="false"  />
                        </td>
                        <td class="pm_list">
                            <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "User_View.aspx?UserID=" + binding.getValue(Container.DataItem,"UserID"))%>">
                                <%#binding.getValue(Container.DataItem,"LoginID")%>
                            </a>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem,"UserName")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getValue(Container.DataItem, "UserAccountStatus")%>
                        </td>
                        <td class="pm_list">
                            <%#binding.getFValue(Container.DataItem,"ExpiryDate")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <tb:RecordListFooter id="ListFooter" runat="server"
             ShowAllRecords="true" 
          />

        </ContentTemplate >
    </asp:UpdatePanel> 
    </asp:Content>
