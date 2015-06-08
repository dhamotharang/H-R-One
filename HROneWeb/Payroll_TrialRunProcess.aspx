<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_TrialRunProcess.aspx.cs"    Inherits="Payroll_TrialRunProcess" MasterPageFile="~/MainMasterPage.master"  %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Title" Text="Payroll Trial Run" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="SubTitle" Text="Payroll Trial Run Detail" runat="server" />:
                </td>
            </tr>
        </table>
    <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="500" Enabled="false" >

    </asp:Timer>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers >
            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
        </Triggers>
    <ContentTemplate >
    <table width="100%">
        <tr>
            <td class="pm_field" align="center">
                <asp:Label ID="lblProgressMessage" runat="server" Text="Payroll trial run is processing" /> ...
            </td>
        </tr>
        <tr>
            <td class="pm_field" align="center">
                <asp:Label ID="lblProgress" runat="server" Text=""/>
            </td>
        </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content> 