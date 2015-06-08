<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Emp_RecurringPayment_Edit.ascx.cs" Inherits="Emp_RecurringPayment_Edit_Control" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
        <input type="hidden" id="ID" runat="server" name="ID" />
        <input type="hidden" id="PrevID" runat="server" name="ID" />
        <input type="hidden" id="HiddenEmpID" runat="server" name="OldID" />
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label1" runat="server" EnableViewState="false" Text="From" />:
                </td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="EmpRPEffFr" runat="server" />
                </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label2" runat="server" EnableViewState="false" Text="To" />:
                </td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="EmpRPEffTo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label3" runat="server" EnableViewState="false" Text="Payment Code" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="PayCodeID" runat="Server" AutoPostBack="true"/>
                </td>
            </tr>
            
            <tr id="MonthlyCommissionRow1" runat="server" visible="false">
                <td class="pm_field_header">
                    <asp:Label ID="Label14" runat="server" EnableViewState="false" Text="Target Salary" />:
                </td>
                
                <td class="pm_field">
                    <asp:TextBox ID="EmpRPBasicSalary" runat="Server"  AutoPostBack="true" OnTextChanged="FPS_Changed"/>
                </td>
                
                <td class="pm_field_header">
                    <asp:Label ID="Label15" runat="server" EnableViewState="false" Text="FPS." />:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpRPFPS" runat="Server" AutoPostBack="true" OnTextChanged="FPS_Changed" />%
                </td>
            </tr>
            <tr id="PayscaleRow1" runat="server" visible="false">
                <td class="pm_field_header">
                    <asp:Label ID="Label12" runat="server" EnableViewState="false" Text="PayScale Scheme" />:
                </td>
                
                <td class="pm_field">
                    <asp:DropDownList ID="SchemeCode" runat="Server" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="SchemeCode_Changed"/>
                </td>
                
                <td class="pm_field_header">
                    <asp:Label ID="Label13" runat="server" EnableViewState="false" Text="PayScale Capacity" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="CapacitySelect" runat="Server"  AutoPostBack="true"  OnSelectedIndexChanged="CapacitySelect_Changed" />
                </td>
            </tr>
            <tr id="PayscaleRow2" runat="server" visible="false">
                <td class="pm_field_header">
                    <asp:Label ID="Label16" runat="server" EnableViewState="false" Text="PayScale Point" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="PointSelect" runat="Server" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="PointSelect_Changed"/>
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" EnableViewState="false" Text="Amount" />:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="EmpRPAmount" runat="Server" Enabled="true"/>
                    <asp:DropDownList ID="CurrencyID" runat="Server" Visible="false" />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label5" runat="server" EnableViewState="false" Text="Unit" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="EmpRPUnit" runat="Server"  AutoPostBack="true"/>
                    <asp:CheckBox ID="EmpRPUnitPeriodAsDaily" runat="server" AutoPostBack="true" EnableViewState="false" Text="Calculate as daily" />
                </td>
            </tr>
            <tr id="EmpRPUnitPeriodAsDailyPayFormIDRow" runat="server" visible="false" >
                <td class="pm_field_header">
                    <asp:Label ID="Label11" runat="server" EnableViewState="false" Text="Prorata Formula" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="EmpRPUnitPeriodAsDailyPayFormID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label6" runat="server" EnableViewState="false" Text="Method" />:
                </td>
                <td class="pm_field">
                    <asp:DropDownList ID="EmpRPMethod" runat="Server" AutoPostBack="true"  />
                </td>
                <td class="pm_field_header">
                    <asp:Label ID="Label7" runat="server" EnableViewState="false" Text="Non-payroll Item" />?
                </td>
                <td class="pm_field">
                    <asp:CheckBox ID="EmpRPIsNonPayrollItem" runat="Server" />
                </td>
            </tr>
            <tr id="BankAccountRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label8" runat="server" EnableViewState="false" Text="Bank Account Number" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="EmpAccID" runat="Server" AutoPostBack="true" />
                    <asp:Label ID="lblDefaultBankAccount" runat="server" />
                </td>
            </tr>
            <tr id="CostCenterRow" runat="server" >
                <td class="pm_field_header">
                    <asp:Label ID="Label9" runat="server" EnableViewState="false" Text="Cost Center" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:DropDownList ID="CostCenterID" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="Label10" runat="server" EnableViewState="false" Text="Remark" />:
                </td>
                <td class="pm_field" colspan="3">
                    <asp:TextBox ID="EmpRPRemark" runat="Server" TextMode="MultiLine" Columns="50" Rows="5" />
                </td>
            </tr>
        </table>
