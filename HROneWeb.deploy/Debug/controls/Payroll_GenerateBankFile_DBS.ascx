<%@ control language="C#" autoeventwireup="true" inherits="Payroll_GenerateBankFile_DBS, HROneWeb.deploy" enableviewstate="false" %>
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="15%" />
    <tr>
        <td class="pm_field_title" colspan="3">
            <asp:Label Text="DBS Bank File Detail" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="lblBatchID" Text="Batch ID" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="BatchID" runat="server" MaxLength="5" Columns="5" CssClass="pm_required" />
            (<asp:Label ID="Label5" Text="Max. 5 character" runat="server" />)
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="lblBatchName" Text="Batch Name" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="BatchName" runat="server" Text="Salary" MaxLength="25" Columns="25" CssClass="pm_required" />
            (<asp:Label Text="Max. 25 characters" runat="server" />)
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="Label3" Text="Transaction Type" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:DropDownList ID="TransactionType" runat="server" >
                <asp:ListItem Text="Direct Debit" Value="DirectDebit" />
                <asp:ListItem Text="Salary Credit" Value="SalaryCredit" />
                <asp:ListItem Text="Sundry Credit" Value="SundryCredit" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="Label1" Text="Second Party Reference" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="SecondPartyReference" Text="Salary" runat="server" MaxLength="18" Columns="18" CssClass="pm_required" />
            (<asp:Label ID="Label2" Text="Max. 18 character" runat="server" />)<br />
            <asp:CheckBox ID="IncludeEmpNo" runat="server" Text="Include Employee No. in second party reference" Checked="true" />
        </td>
    </tr>
 </table>
