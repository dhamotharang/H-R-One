<%@ control language="C#" autoeventwireup="true" inherits="Emp_PersonalInfo, HROneWeb.deploy" %>

<input type="hidden" id="EmpID" runat="server" name="ID" />

<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="20%" />
    <col width="30%" />
    <col width="20%" />
    <col width="30%" />
    <tr>
        <td class="pm_field_header" >
            <asp:Label Text="Marital Status" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpMaritalStatus" runat="Server" />
        </td>
        <td class="pm_field_header" >
            <asp:Label Text="Place Of Birth" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpPlaceOfBirth" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label Text="Passport No." runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpPassportNo" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label Text="Country of Issue" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpPassportIssuedCountry" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label13" Text="Passport Expiry Date" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpPassportExpiryDate" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label17" Text="Nationality" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpNationality" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label Text="Residential Address" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpResAddr" runat="Server" /><br />
            <asp:Label ID="EmpResAddrAreaCode" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label Text="Correspondence" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpCorAddr" runat="Server" /><br />
            <asp:Label ID="Label2" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label Text="Probation Period" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpProbaPeriod" runat="Server" />
            <asp:Label ID="EmpProbaUnit" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label16" Text="Probation Last Date" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpProbaLastDate" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label14" Text="Notice Period" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpNoticePeriod" runat="Server" />
            <asp:Label ID="EmpNoticeUnit" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label10" Text="Home" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpHomePhoneNo" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label15" Text="Mobile No." runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpMobileNo" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label11" Text="Office" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpOfficePhoneNo" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label3" Text="Personal E-mail Address" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpEmail" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label12" Text="Company E-mail Address" runat="server" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpInternalEmail" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label ID="Label5" Text="Override Minimum Wage" runat="server"  />?
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpIsOverrideMinimumWage" runat="Server" />
        </td>
        <td class="pm_field_header">
            <asp:Label ID="Label18" Text="New Minimun Hourly Rate" runat="server"  />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpOverrideMinimumHourlyRate" runat="Server" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header">
            <asp:Label Text="Remark" runat="server" />:
        </td>
        <td class="pm_field" colspan="3">
            <asp:Label ID="Remark" runat="Server" />
        </td>
    </tr>
    <tr id="AttendanceRow" runat="server" >
        <td class="pm_field_header">
            <asp:Label ID="Label1" runat="server" Text="Time Card No." />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpTimeCardNo" runat="Server"  />
        </td>
     </tr>
     <tr runat="server" >
        <%-- Start 0000067, Miranda, 2014-08-07 --%>
        <td class="pm_field_header">
            <asp:Label ID="Label20" runat="server" Text="Original Hire Date" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpOriginalHireDate" runat="Server"  />
        </td>
        <%-- End 0000067, Miranda, 2014-08-07 --%>
    </tr>

    <tr id="PayscaleRow1" runat="server" >
        <td class="pm_field_header">
            <asp:Label ID="Label19" runat="server" Text="Next Salary Increment Date" />:
        </td>
        <td class="pm_field">
            <asp:Label ID="EmpNextSalaryIncrementDate" runat="Server"  />
        </td>
    </tr>
    
</table>

<br />
<asp:PlaceHolder ID="EmpUniformSection" runat="server" Visible="false" >
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <tr>
        <td class="pm_field_title" colspan="6" >
            <asp:Label ID="Label6" runat="server" Text="Uniform Information" />
        </td>
    </tr>
    <tr>
        <td class="pm_field_header" >
            <asp:Label ID="Label7" Text="UNIFORM_B" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpUniformB" runat="Server" />
        </td>
        <td class="pm_field_header" >
            <asp:Label ID="Label9" Text="UNIFORM_W" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpUniformW" runat="Server" />
        </td>
        <td class="pm_field_header" >
            <asp:Label ID="Label8" Text="UNIFORM_H" runat="server" />:
        </td>
        <td class="pm_field" >
            <asp:Label ID="EmpUniformH" runat="Server" />
        </td>
    </tr>
</table>
<br />
</asp:PlaceHolder>
<asp:Panel ID="EmpExtraFieldPanel" runat="server" >
<asp:Repeater ID="EmpExtraFieldGroupRepeater" runat="server" OnItemDataBound="EmpExtraFieldGroupRepeater_ItemDataBound">
<ItemTemplate>
<table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
    <col width="20%" />
    <col width="30%" />
    <col width="20%" />
    <col width="30%" />
    <tr>
        <td class="pm_field_title" colspan="4" >
            <asp:Label ID="EmpExtraFieldGroupName" runat="server" Text="Extra Information" />
        </td>
    </tr>
    <tr>
        <asp:Repeater ID="EmpExtraField" runat="server" OnItemDataBound="EmpExtraField_ItemDataBound">
            <ItemTemplate>
                <td class="pm_field_header" >
                    <%#((HROne.Lib.Entities.EEmpExtraField)Container.DataItem).EmpExtraFieldName%>
                </td>
                <td class="pm_field" >
                    <asp:Label ID="EmpExtraFieldValue" runat="server" />
                </td>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <td class="pm_field_header" >
                    <%#((HROne.Lib.Entities.EEmpExtraField)Container.DataItem).EmpExtraFieldName%>
                </td>
                <td class="pm_field" >
                    <asp:Label ID="EmpExtraFieldValue" runat="server" />
                </td>
                <% Response.Write("</tr><tr>"); %>
            </AlternatingItemTemplate>    
        </asp:Repeater>
    </tr>
</table>
</ItemTemplate>
</asp:Repeater>
</asp:Panel>
<br />
<asp:Panel ID="ESSTable" runat="server" Visible="false">
<table width="100%" cellspacing="0" cellpadding="0" class="pm_field_section" >
    <col width="20%" />
    <col width="30%" />
    <col width="20%" />
    <col width="30%" />
        <tr>
            <td class="pm_field_title" colspan="4" >
                <asp:Label ID="Label4" runat="server" Text="For Employee Self Service" />
            </td>
        </tr>
        <tr>
            <td class="pm_field_header" >
            </td>
            <td class="pm_field" >
            </td>
        </tr>
</table>
</asp:Panel>
