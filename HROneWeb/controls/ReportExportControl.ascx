<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReportExportControl.ascx.cs" Inherits="ReportExportControl" %>
                            <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" CommandArgument=""/>
                            <asp:Button ID="btnGenerateExcel" runat="server" Text="Export to Excel"  CommandArgument="EXCEL"/>
                            <asp:Button ID="btnGenerateWord" runat="server" Text="Export to Word" CommandArgument="WORD"/>
