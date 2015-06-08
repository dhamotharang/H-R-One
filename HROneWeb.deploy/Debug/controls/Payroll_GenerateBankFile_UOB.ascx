<%@ control language="C#" autoeventwireup="true" inherits="Payroll_GenerateBankFile_UOB, HROneWeb.deploy" enableviewstate="false" %>
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="15%" />
    <tr>
        <td class="pm_field_title" colspan="3">
            <asp:Label Text="UOB Bank File Detail" runat="server" />
        </td>
    </tr>
    <!--
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="lblBatchID" Text="Batch ID" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="BatchID" runat="server" MaxLength="5" Columns="5" CssClass="pm_required" />
            (<asp:Label ID="Label5" Text="Max. 5 character" runat="server" />)
        </td>
    </tr>
    -->
    <!--
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Bank File Seq" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="FileSeq" runat="server" Text="" MaxLength="2" Columns="2" CssClass="pm_required" />
        </td>
    </tr>
    -->
    
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="lblParticulars" Text="Particulars" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="Particulars" runat="server" Text="Salary" MaxLength="6" Columns="6" CssClass="pm_required" />
            (<asp:Label Text="Max. 6 characters" runat="server" />)
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="Label3" Text="Transaction Type" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:DropDownList ID="TransactionType" runat="server" >
                <asp:ListItem Text="Salary Credit" Value="SalaryCredit" />
            </asp:DropDownList>
        </td>
    </tr>
    <!--
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
    -->
 </table>
