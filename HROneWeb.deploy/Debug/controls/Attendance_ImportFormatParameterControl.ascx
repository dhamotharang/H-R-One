<%@ control language="C#" autoeventwireup="true" inherits="Attendance_ImportFormatParameterControl, HROneWeb.deploy" %>
<table class="pm_list_section" width="100%" >
    <tr>
        <td class="pm_list_header" ><asp:Label ID="lblFieldHeader" runat="server" Text="Field" /></td>
        <td class="pm_list_header" ><asp:Label ID="lblColumnHeader" runat="server" Text="Column" /></td>
        <td class="pm_list_header" ><asp:Label ID="lblOther" runat="server" /></td>
    </tr>
    <tr>
        <td class="pm_list" ><asp:Label ID="lblDate" runat="server" Text="Date" /></td>
        <td class="pm_list" >
            <asp:DropDownList ID="cbxDateColumnIndex" runat="server" >
                <asp:ListItem Text="1" Value="1" selected="True"/>
                <asp:ListItem Text="2" Value="2" />
                <asp:ListItem Text="3" Value="3" />
                <asp:ListItem Text="4" Value="4" />
                <asp:ListItem Text="5" Value="5" />
                <asp:ListItem Text="6" Value="6" />
                <asp:ListItem Text="7" Value="7" />
                <asp:ListItem Text="8" Value="8" />
                <asp:ListItem Text="9" Value="9" />
                <asp:ListItem Text="10" Value="10" />
            </asp:DropDownList>
        </td>
        <td class="pm_list" >
            <asp:Label ID="lblDateFormat" runat="server" Text="Sequence"/>
            <asp:DropDownList ID="cbxDateSequence" runat="server" >
                <asp:ListItem Text="Year/Month/Day" Value="YMD" />
                <asp:ListItem Text="Day/Month/Year" Value="DMY" />
                <asp:ListItem Text="Month/Day/Year" Value="MDY" />
            </asp:DropDownList><br />
            <asp:Label ID="Label2" runat="server" Text="Year Format"/>
            <asp:DropDownList ID="cbxYearFormat" runat="server" >
                <asp:ListItem Text="YY" Value="YY" />
                <asp:ListItem Text="YYYY" Value="YYYY" />
            </asp:DropDownList><br />
            <asp:Label ID="Label1" runat="server" Text="Date Separator"/>
            <asp:DropDownList ID="cbxDateSeparator" runat="server" >
                <asp:ListItem Text="(null)" Value="" />
                <asp:ListItem Text="-" Value="-" />
                <asp:ListItem Text="/" Value="/" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="pm_list" ><asp:Label ID="Label3" runat="server" Text="Time" /></td>
        <td class="pm_list" >
            <asp:DropDownList ID="cbxTimeColumnIndex" runat="server" >
                <asp:ListItem Text="1" Value="1" />
                <asp:ListItem Text="2" Value="2" selected="True" />
                <asp:ListItem Text="3" Value="3" />
                <asp:ListItem Text="4" Value="4" />
                <asp:ListItem Text="5" Value="5" />
                <asp:ListItem Text="6" Value="6" />
                <asp:ListItem Text="7" Value="7" />
                <asp:ListItem Text="8" Value="8" />
                <asp:ListItem Text="9" Value="9" />
                <asp:ListItem Text="10" Value="10" />
            </asp:DropDownList>
        </td>
        <td class="pm_list" >
            <asp:Label ID="Label6" runat="server" Text="Time Separator"/>
            <asp:DropDownList ID="cbxTimeSeparator" runat="server" >
                <asp:ListItem Text="(null)" Value="" />
                <asp:ListItem Text=":" Value=":" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="pm_list" ><asp:Label ID="Label10" runat="server" Text="2nd Date" /></td>
        <td class="pm_list" colspan="2">
            <asp:DropDownList ID="cbxDateColumnIndex2" runat="server" >
                <asp:ListItem Text="Not Selected" Value="0" selected="True" />
                <asp:ListItem Text="1" Value="1" />
                <asp:ListItem Text="2" Value="2" />
                <asp:ListItem Text="3" Value="3" />
                <asp:ListItem Text="4" Value="4" />
                <asp:ListItem Text="5" Value="5" />
                <asp:ListItem Text="6" Value="6" />
                <asp:ListItem Text="7" Value="7" />
                <asp:ListItem Text="8" Value="8" />
                <asp:ListItem Text="9" Value="9" />
                <asp:ListItem Text="10" Value="10" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="pm_list" ><asp:Label ID="Label9" runat="server" Text="2nd Time" /></td>
        <td class="pm_list" colspan="2" >
            <asp:DropDownList ID="cbxTimeColumnIndex2" runat="server" >
                <asp:ListItem Text="Not Selected" Value="0" selected="True" />
                <asp:ListItem Text="1" Value="1" />
                <asp:ListItem Text="2" Value="2" />
                <asp:ListItem Text="3" Value="3" />
                <asp:ListItem Text="4" Value="4" />
                <asp:ListItem Text="5" Value="5" />
                <asp:ListItem Text="6" Value="6" />
                <asp:ListItem Text="7" Value="7" />
                <asp:ListItem Text="8" Value="8" />
                <asp:ListItem Text="9" Value="9" />
                <asp:ListItem Text="10" Value="10" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="pm_list" ><asp:Label ID="Label4" runat="server" Text="Location" /></td>
        <td class="pm_list" >
            <asp:DropDownList ID="cbxLocationColumnIndex" runat="server" >
                <asp:ListItem Text="1" Value="1" />
                <asp:ListItem Text="2" Value="2" />
                <asp:ListItem Text="3" Value="3" selected="True"/>
                <asp:ListItem Text="4" Value="4" />
                <asp:ListItem Text="5" Value="5" />
                <asp:ListItem Text="6" Value="6" />
                <asp:ListItem Text="7" Value="7" />
                <asp:ListItem Text="8" Value="8" />
                <asp:ListItem Text="9" Value="9" />
                <asp:ListItem Text="10" Value="10" />
            </asp:DropDownList>
        </td>
        <td>&nbsp</td>
    </tr>
    <tr>
        <td class="pm_list" ><asp:Label ID="Label5" runat="server" Text="Time Card No." /></td>
        <td class="pm_list" >
            <asp:DropDownList ID="cbxTimeCardNumColumnIndex" runat="server" >
                <asp:ListItem Text="1" Value="1" />
                <asp:ListItem Text="2" Value="2" />
                <asp:ListItem Text="3" Value="3" />
                <asp:ListItem Text="4" Value="4" selected="True"/>
                <asp:ListItem Text="5" Value="5" />
                <asp:ListItem Text="6" Value="6" />
                <asp:ListItem Text="7" Value="7" />
                <asp:ListItem Text="8" Value="8" />
                <asp:ListItem Text="9" Value="9" />
                <asp:ListItem Text="10" Value="10" />
            </asp:DropDownList>
        </td>
        <td>&nbsp</td>
    </tr>
    <tr>
        <td class="pm_list_header" colspan="4" ><asp:Label ID="Label8" runat="server" Text="Settings for Plain Text/CSV" /></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblFieldDelimiter" runat="server" Text="ColumnDelimiter" /></td>
        <td><asp:DropDownList ID="cbxColumnDelimiter" runat="server" >
                <asp:ListItem Text="(Space)" Value=" " selected="True"/>
                <asp:ListItem Text="," Value="," />
            </asp:DropDownList>

        </td>
        <td>&nbsp</td>
    </tr>
    <tr>
        <td><asp:Label ID="Label7" runat="server" Text="File has header" />?</td>
        <td><asp:CheckBox ID="chkHasHeader" runat="server" />
        </td>
        <td>&nbsp</td>
    </tr>
    <tr class="pm_list_header">
        <td colspan="3">
            <asp:Button ID="btnSave" runat="server" Text="Save Settings" OnClick="btnSave_Click" CssClass="button" />
        </td>
    </tr>
</table>

