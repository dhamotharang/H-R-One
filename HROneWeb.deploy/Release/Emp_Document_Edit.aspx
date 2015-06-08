<%@ page language="C#" autoeventwireup="true" inherits="Emp_Document_Edit, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/Emp_Common.ascx" TagName="Emp_Common" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpDocumentID" runat="server" name="ID" />
        <input type="hidden" id="EmpID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="Employee Information" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="ActionHeader" runat="server" Text="Edit" />
                    <asp:Label runat="server" Text="Document" />
                </td>
            </tr>
        </table>
        
            
                
        
        <uc1:Emp_Common ID="Emp_Common1" runat="server"></uc1:Emp_Common>
        <br />
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     EditButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnSaveButton_Click ="Save_Click"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label runat="server" Text="Type" />:
                </td>
                <td class="pm_field" >
                    <asp:DropDownList ID="DocumentTypeID" runat="server" />
                </td>
                <asp:Panel ID="ExistingFilePanel" runat="server" >
                    <td class="pm_field_header" >
                        <asp:Label runat="server" Text="File name" />:
                    </td>
                    <td class="pm_field" >
                        <asp:Label ID="EmpDocumentOriginalFileName" runat="Server" />
                    </td>
                </asp:Panel>
                <asp:Panel ID="NewFilePanel" runat="server" >
                    <td class="pm_field_header" >
                        <asp:Label ID="Label1" runat="server" Text="Upload File" />:
                    </td>
                    <td class="pm_field" >
                        <asp:FileUpload runat="server" ID="UploadDocumentFile" CssClass="pm_required" />
                    </td>
                </asp:Panel>
            </tr>
            <tr>
                <td class="pm_field_header" runat="server" >
                    <asp:Label runat="server" Text="Description" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="EmpDocumentDesc" runat="Server" />
                </td>
            </tr>
        </table>
</asp:Content> 