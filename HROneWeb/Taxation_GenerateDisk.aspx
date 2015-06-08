<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Taxation_GenerateDisk.aspx.cs"    Inherits="Taxation_GenerateDisk" MasterPageFile="~/MainMasterPage.master" %>



<%@ Register Src="~/controls/Payroll_PeriodInfo.ascx" TagName="Payroll_PeriodInfo"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">

		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="lblReportHeader" runat="server" Text="Taxation Diskette Generation" />
                </td>
            </tr>
        </table>

    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
        <tr>
            <td>
                <asp:Label runat="server" Text="Taxation Company Information" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="TaxFormIDUpdatePanel" runat="server" UpdateMode="conditional" >
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="TaxCompID" EventName="SelectedIndexChanged" />
    </Triggers>
    <ContentTemplate >
    <table width="100%" class="pm_section">
        <tr>
            <td class="pm_field_header">
                <asp:Label runat="server" Text="Taxation Company" />:
            </td>
            <td class="pm_field">
                <asp:DropDownList ID="TaxCompID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="TaxCompID_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label runat="server" Text="Year" />:
            </td>
            <td class="pm_field">
                    <asp:DropDownList runat="server" ID="TaxFormID" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label runat="server" Text="Name of Signature" />:
            </td>
            <td class="pm_field">
                <asp:TextBox ID="txtNameOfSignature" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label runat="server" Text="File Format" />:
            </td>
            <td class="pm_field">
                <asp:DropDownList runat="server" ID="TaxFileFormat">                    
                    <asp:ListItem Text="CD-ROM" Value="TEXT" Selected="True" />
                    <asp:ListItem Text="e-filing Service (Testing)" Value="XML" />
                </asp:DropDownList> 
            </td>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
    
        
                    <asp:Button ID="btnGenerateFile" runat="server" Text="Generate File" CSSClass="button" 
                        onclick="btnGenerateFile_Click" />
    
                    <asp:Button ID="btnPrint" runat="server" Text="Generate Control List" CSSClass="button" 
                        onclick="btnPrint_Click" />
</asp:Content> 