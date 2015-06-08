<%@ control language="C#" autoeventwireup="true" inherits="ReportExportControl, HROneWeb.deploy" %>
                            <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" CommandArgument=""/>
                            <asp:Button ID="btnGenerateExcel" runat="server" Text="Export to Excel"  CommandArgument="EXCEL"/>
                            <asp:Button ID="btnGenerateWord" runat="server" Text="Export to Word" CommandArgument="WORD"/>
