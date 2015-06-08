<%@ control language="C#" autoeventwireup="true" inherits="Payroll_GenerateBankFile_SCB, HROneWeb.deploy" %>
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="15%" />
    <tr>
        <td class="pm_field_title" colspan="3">
            <asp:Label Text="Standard Chartered Bank File Detail" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="Label1" Text="File Format" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:DropDownList ID="FileType" runat="server" AutoPostBack="true" >
                <asp:ListItem Text="DGP without MPF" Value="DGPOnly" />
                <asp:ListItem Text="DGP with MPF" Value="DGPwMPF" />
                <%-- Start 0000197, KuangWei, 2015-05-27 --%>
                <asp:ListItem Text="i-Payment" Value="iPayment" />
                <%-- End 0000197, KuangWei, 2015-05-27 --%>
            </asp:DropDownList>
        </td>
    </tr>
    <asp:PlaceHolder ID="DGPOnlyPanel" runat="server" >
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Customer Reference" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="CustomerReference" runat="server" MaxLength="16" />
            (<asp:Label Text="Max. 16 character" runat="server" />)
        </td>
    </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="DGPwMPFPanel" runat="server" >
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="Label5" Text="ER CMG Reference" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="ERCMGReference" runat="server" MaxLength="20" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="Label2" Text="Batch Description" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="BatchDescription" runat="server" MaxLength="16" />
            (<asp:Label ID="Label3" Text="Max. 16 character" runat="server" />)
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="Label4" Text="Include MPF Contribution" runat="server" />?
        </td>
        <td class="pm_field" colspan="2">
            <asp:CheckBox ID="IncludeMPFRecord" runat="server" Checked="true" />
        </td>
    </tr>
    </asp:PlaceHolder>
    <%-- Start 0000197, KuangWei, 2015-05-27 --%>
    <asp:PlaceHolder ID="PaymentCentrePanel" runat="server" > 
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Customer Reference" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="PaymentCustomerReference" runat="server" MaxLength="16" />
            (<asp:Label Text="Max. 16 character" runat="server" />)
        </td>
    </tr> 
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Customer Memo" runat="server" />:
        </td>
        <td class="pm_field" colspan="2">
            <asp:TextBox ID="PaymentCustomerMemo" runat="server" MaxLength="255" Width="300" TextMode="MultiLine" Rows="6" />
            (<asp:Label Text="Max. 255 character" runat="server" />)
        </td>
    </tr>           
    </asp:PlaceHolder>
    <%-- End 0000197, KuangWei, 2015-05-27 --%>
 </table>
