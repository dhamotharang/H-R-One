<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Payroll_GenerateMPFFile_Manulife.ascx.cs" Inherits="Payroll_GenerateMPFFile_Manulife" %>
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
