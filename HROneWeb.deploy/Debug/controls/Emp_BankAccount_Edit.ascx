<%@ control language="C#" autoeventwireup="true" inherits="Emp_BankAccount_Edit_Control, HROneWeb.deploy" %>
        <input type="hidden" id="ID" runat="server" name="ID" />
        <input type="hidden" id="HiddenEmpID" runat="server" name="EmpID" />
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" Text="Bank Account Number" runat="server" />:</td>
                <td class="pm_field" >
                <table border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="center">
                            <asp:TextBox ID="EmpBankCode" runat="Server" />
                        </td>
                        <td align="center">-</td>
                        <td align="center">
                            <asp:TextBox ID="EmpBranchCode" runat="Server" />
                        </td>
                        <td align="center">-</td>
                        <td align="center">
                            <asp:TextBox ID="EmpAccountNo" runat="Server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_field" align="center" style="border:0;">
                            <asp:Label ID="Label2" Text="Bank Code" runat="Server" />
                        </td>
                        <td></td>
                        <td class="pm_field" align="center" style="border:0;">
                            <asp:Label ID="Label3" Text="Branch Code" runat="Server" />
                        </td>
                        <td></td>
                        <td class="pm_field" align="center" style="border:0;">
                            <asp:Label ID="Label4" Text="Account No." runat="Server" />
                        </td>
                    </tr>
                </table>
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label5" Text="Holder Name" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="EmpBankAccountHolderName" runat="Server"/></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" Text="Is Default Account" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:CheckBox ID="EmpAccDefault" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label7" Text="Remark" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="EmpBankAccountRemark" runat="Server" /></td>
            </tr>
        </table>
