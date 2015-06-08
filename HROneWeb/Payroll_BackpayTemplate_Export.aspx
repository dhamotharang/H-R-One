<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_BackpayTemplate_Export.aspx.cs" Inherits="Payroll_BackpayTemplate_Export" MasterPageFile="~/MainMasterPage.master" EnableViewState="true" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="ID" runat="server" name="ID" />
        
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="PayScale - Backpay Template Export" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel2" Text="Export Details" runat="server" />
                </td>
            </tr>
        </table>
        
        <asp:UpdatePanel>
        <ContentTemplate>
            <table width="100%" class="pm_section">
                <tr>
                    <td class="pm_field_header" >
                        <asp:Label ID="Label1" runat="server" Text="Scheme Code" />:
                    </td>
                    <td class="pm_field">
                        
                        <asp:DropDownList ID="SchemeCode" runat="Server" />
                    </td>
                </tr>
                <tr>
                    <td class="pm_field_header" >
                        <asp:Label ID="Label7" runat="server" Text="Backpay Date" />:
                    </td>
                    <td class="pm_field">
                        <uc1:WebDatePicker id="BackpayDate" runat="server" ShowDateFormatLabel="true"  />
                    </td>
                </tr>
                <tr>
                    <td class="pm_field_header" >
                        <asp:Label ID="Label6" runat="server" Text="Payment Code for backpay" />:
                    </td>
                    <td class="pm_field">
                        <asp:DropDownList ID="PaymentCode" runat="Server"  />
                    </td>
                </tr>
            </table>
        
       
            <table class="pm_section" width="100%">
                <tr>
                    <td class="pm_search_header">
                    </td>
                </tr>
                <tr>
                    <td class="pm_search" >
                        <br /><br /><br /><br />
                        
<%--                        <asp:Label ID="Label4" runat="server" Text="Export data entry template for inputting new salary information" />:
--%>                        
                        <asp:Button ID="Button1" runat="server" Text="Export Template" OnClick="btnExport_Click"  CssClass="button"/>
                    </td>
                    <td class="pm_search">
                        
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" />
                    </td>
                </tr>
            </table>

        </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content> 