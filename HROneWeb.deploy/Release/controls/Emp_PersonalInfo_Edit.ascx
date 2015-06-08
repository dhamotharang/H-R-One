<%@ control language="C#" autoeventwireup="true" inherits="Emp_PersonalInfo_Edit, HROneWeb.deploy" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
        <input type="hidden" id="ID" runat="server" name="ID" />
        <input type="hidden" id="OldID" runat="server" name="OldID" />
        <asp:Panel ID="RoleEmpInfoPanel" runat="server" Visible="false" >
        
        <asp:Label id="SuppressHKIDWarning" runat="server" Text="false" Visible="false" EnableViewState="true"/>

        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label runat="server" EnableViewState="false" Text="Role options" />
                </td>
            </tr>
            <tr id="NewRoleEmpNoRoleRow" runat="server" >
                <td class="pm_field_header" >
                    <asp:Label ID="lblNewRoleEmpNo" runat="server" EnableViewState="false" Text="Emp. No for new role" />
                </td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="NewRoleEmpNo" runat="Server" class="pm_required" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label32" runat="server" EnableViewState="false" Text="Options" />
                </td>
                <td class="pm_field" colspan="3">
                    <asp:CheckBox ID="EmpIsCombinePaySlip" runat="server" EnableViewState="false" Text="Combine Payslip to Master" /><br />
                    <asp:CheckBox ID="EmpIsCombineMPF" runat="server" EnableViewState="false" Text="Combine MPF to Master" /><br />
                    <asp:CheckBox ID="EmpIsCombineTax" runat="server" EnableViewState="false" Text="Combine Tax to Master" />
                </td>
            </tr>
        </table> 
        <br />
        </asp:Panel >
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_title" colspan="4">
                    <asp:Label ID="Label33" EnableViewState="false" Text="Employee Info" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="EmpNo" />
                </td>
                <td class="pm_field" >
                    <asp:TextBox ID="EmpNo" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label14" runat="server" EnableViewState="false" Text="Alias" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="EmpAlias" runat="Server"  /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="Surname" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpEngSurname" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label3" runat="server" EnableViewState="false" Text="Other Name" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpEngOtherName" runat="Server"  /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label13" runat="server" EnableViewState="false" Text="Gender" />:</td>
                <td class="pm_field">
                    <asp:DropDownList ID="EmpGender" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" EnableViewState="false" Text="Chinese Name" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpChiFullName" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label10" runat="server" EnableViewState="false" Text="HKID" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpHKID" runat="Server" />(<asp:TextBox ID="EmpHKID_Digit"
                        runat="Server" Columns="1" MaxLength="1" style="text-align:center" />)
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="Date of Birth" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="EmpDateOfBirth" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label11" runat="server" EnableViewState="false" Text="Join Date" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker ID="EmpDateOfJoin" runat="server" OnChanged="empDateOfJoin_Changed" AutoPostBack="true" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label12" runat="server" EnableViewState="false" Text="Service Start Date" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="EmpServiceDate" runat="server" />
                </td>
            </tr>

        </table>
        <br />
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label15" runat="server" EnableViewState="false" Text="Marital Status" />:
                </td>
                <td class="pm_field" >
                    <asp:DropDownList ID="EmpMaritalStatus" runat="Server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label16" runat="server" EnableViewState="false" Text="Place Of Birth" />:
                </td>
                <td class="pm_field" >
                    <%-- Start 0000092, KuangWei, 2014-09-09 --%>
                    <%--<asp:TextBox ID="EmpPlaceOfBirth" runat="Server" />--%>
	                <asp:DropDownList ID="EmpPlaceOfBirthID" runat="Server" />
                    <%-- End 0000092, KuangWei, 2014-09-09 --%>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label17" runat="server" EnableViewState="false" Text="Passport No." />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpPassportNo" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label18" runat="server" EnableViewState="false" Text="Country of Issue" />:</td>
                <td class="pm_field">
                    <%-- Start 0000092, KuangWei, 2014-09-10 --%>                
                    <%--<asp:TextBox ID="EmpPassportIssuedCountry" runat="Server" />--%>
	                <asp:DropDownList ID="EmpPassportIssuedCountryID" runat="Server" />
                    <%-- End 0000092, KuangWei, 2014-09-10 --%>                    
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label19" runat="server" EnableViewState="false" Text="Passport Expiry Date" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="EmpPassportExpiryDate" runat="server" />
                </td>
                <td class="pm_field_header">                   
                    <asp:Label ID="Label20" runat="server" EnableViewState="false" Text="Nationality" />:</td>
                <td class="pm_field">
                    <%-- Start 0000092, KuangWei, 2014-09-10 --%>                   
                    <%--<asp:TextBox ID="EmpNationality" runat="Server" />--%>
                    <asp:DropDownList ID="EmpNationalityID" runat="Server" />
                    <%-- End 0000092, KuangWei, 2014-09-10 --%>                       
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" EnableViewState="false" Text="Residential Address" />:
                </td>
                <td class="pm_field">
                    <asp:Button ID="btnChangeAddressMode" runat="server" EnableViewState="false" Text="Switch Display Mode" OnClick="btnChangeAddressMode_Click" CssClass="button"/><br />
                    <asp:Panel ID="FullResAddrPanel" runat="server" >
                    <asp:TextBox ID="EmpResAddr" runat="Server" Columns="30" Rows="5" TextMode="multiLine" /><br />
                    </asp:Panel>
                    <asp:Panel ID="ThreeLineResAddrPanel" runat="server" Visible="false"  >
                    <asp:TextBox ID="EmpResAddr1" runat="Server" Columns="35" /><br />
                    <asp:TextBox ID="EmpResAddr2" runat="Server" Columns="35" /><br />
                    <asp:TextBox ID="EmpResAddr3" runat="Server" Columns="35" /><br />
                    </asp:Panel>
                    <asp:DropDownList ID="EmpResAddrAreaCode" runat="Server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label7" runat="server" EnableViewState="false" Text="Correspondence" />:</td>
                <td class="pm_field" valign="top">
                    <asp:TextBox ID="EmpCorAddr" runat="Server" Columns="30" Rows="5" TextMode="multiLine" /><br />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label8" runat="server" EnableViewState="false" Text="Probation Period" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpProbaPeriod" runat="Server" />
                    <asp:DropDownList ID="EmpProbaUnit" runat="Server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label25" runat="server" EnableViewState="false" Text="Probation Last Date" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="EmpProbaLastDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label26" runat="server" EnableViewState="false" Text="Notice Period" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpNoticePeriod" runat="Server"  />
                    <asp:DropDownList ID="EmpNoticeUnit" runat="Server" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label9" runat="server" EnableViewState="false" Text="Home" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpHomePhoneNo" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label21" runat="server" EnableViewState="false" Text="Mobile No." />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpMobileNo" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label22" runat="server" EnableViewState="false" Text="Office" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpOfficePhoneNo" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label23" runat="server" EnableViewState="false" Text="Personal E-mail Address" />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpEmail" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label24" runat="server" EnableViewState="false" Text="Company E-mail Address" />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="EmpInternalEmail" runat="Server"  />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label27" EnableViewState="false" Text="Override Minimum Wage" runat="server"  />?</td>
                <td class="pm_field" >
                    <asp:CheckBox ID="EmpIsOverrideMinimumWage" runat="Server" /></td>
                <td class="pm_field_header">
                    <asp:Label ID="Label28" EnableViewState="false" Text="New Minimun Hourly Rate" runat="server"  />:</td>
                <td class="pm_field" >
                    <asp:TextBox ID="EmpOverrideMinimumHourlyRate" runat="Server" style="text-align:right"/></td>
            </tr>            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label29" runat="server" EnableViewState="false" Text="Remark" />:</td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="Remark" runat="Server" Columns="90" Rows="5" TextMode="multiLine" />
                </td>
            </tr>
            <tr id="AttendanceRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label30" runat="server" EnableViewState="false" Text="Time Card No." />:</td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpTimeCardNo" runat="Server"  />
                </td>
            </tr>
            <tr runat="server">
                <%-- Start 0000067, Miranda, 2014-08-07 --%>
                <td class="pm_field_header">
                    <asp:Label ID="Label31" runat="server" EnableViewState="false" Text="Original Hire Date" />:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="EmpOriginalHireDate" runat="server" />
                </td>
                <%-- End 0000067, Miranda, 2014-08-07 --%>
            </tr>
            <tr id="PayscaleRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label34" runat="server" EnableViewState="false" Text="Next Salary Increment Date"/>:</td>
                <td class="pm_field">
                    <uc1:WebDatePicker id="EmpNextSalaryIncrementDate" runat="server" />
                </td>
            </tr>


        </table>
        <br />
        <asp:Panel ID="EmpExtraFieldPanel" runat="server">
        <asp:Repeater ID="EmpExtraFieldGroupRepeater" runat="server" OnItemDataBound="EmpExtraFieldGroupRepeater_ItemDataBound">
        <ItemTemplate>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_title" colspan="4">
                <asp:Label ID="EmpExtraFieldGroupName" runat="server" EnableViewState="false" Text="Extra Information" /></td>
            <asp:Repeater ID="EmpExtraField" runat="server" OnItemDataBound="EmpExtraField_ItemDataBound">
                <ItemTemplate>
                        <%# "</tr><tr>" %>
                        <td class="pm_field_header" >
                            <%#((HROne.Lib.Entities.EEmpExtraField)Container.DataItem).EmpExtraFieldName%>
                        </td> 
                        <td class="pm_field" >
                            <asp:TextBox ID="EmpExtraFieldValue" runat="server" Columns="35" />
                            <uc1:WebDatePicker ID="EmpExtraFieldValueDateControl" runat="server" />
                        </td>
                </ItemTemplate>
                <AlternatingItemTemplate>
                        <td class="pm_field_header" >
                            <%#((HROne.Lib.Entities.EEmpExtraField)Container.DataItem).EmpExtraFieldName%>
                        </td> 
                        <td class="pm_field" >
                            <asp:TextBox ID="EmpExtraFieldValue" runat="server" Columns="35"  />
                            <uc1:WebDatePicker ID="EmpExtraFieldValueDateControl" runat="server" />
                        </td>
                </AlternatingItemTemplate>
            </asp:Repeater>
            </tr>
        </table>
        </ItemTemplate>
        </asp:Repeater>
        </asp:Panel>
           <br />
        <asp:Panel ID="ESSTable" runat="server">
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
        <col width="15%" />
        <col width="35%" />
        <col width="15%" />
        <col width="35%" />
        <tr>
            <td class="pm_field_title" colspan="4" >
                <asp:Label ID="Label35" runat="server" EnableViewState="false" Text="For Employee Self Service" /></td>
        </tr>
        <tr>
            <td class="pm_field_header">
                <asp:Label ID="Label36" runat="server" EnableViewState="false" Text="Reset Password" />?</td>
            <td class="pm_field">
                <asp:Checkbox ID="IsResetESSPassword" runat="server" />
            </td>
            <td class="pm_field_header">
            </td>
            <td class="pm_field">
            </td>
        </tr>
        </table>
        </asp:Panel>
