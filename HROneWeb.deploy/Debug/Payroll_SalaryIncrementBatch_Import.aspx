<%@ page language="C#" autoeventwireup="true" inherits="Payroll_SalaryIncrementBatch_Import, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" enableviewstate="false" viewStateEncryptionMode="Always" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="ID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="ILabel1" Text="Upload File" runat="server" />
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
                    <asp:Label ID="Label1" runat="server" Text="Upload file path" />:</td>
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
        </asp:UpdatePanel> 
</asp:Content> 