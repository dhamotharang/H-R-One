<%@ control language="C#" autoeventwireup="true" inherits="Payroll_GenerateMPFFile_Manulife, HROneWeb.deploy" %>
<asp:HiddenField ID="CurMPFPlanID" runat="server" />
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="15%" />
    <tr>
        <td class="pm_field_title" colspan="3">
            <asp:Label ID="Label1" Text="Manulife MPF File Detail" runat="server" />
        </td>
    </tr>
    <tr >
        <td class="pm_field_header" >
            <asp:Label ID="Label4" Text="Sequence No." runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:Textbox ID="SequenceNum" runat="server" Text="1" style="text-align:right" Columns="2" MaxLength="2" />
            <asp:Label ID="Label2" Text="(Change this value if you have submited the MPF file today)" runat="server" />:
        </td>
    </tr>
 </table>
