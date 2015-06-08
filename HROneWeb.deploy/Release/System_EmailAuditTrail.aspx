<%@ page language="C#" autoeventwireup="true" inherits="System_EmailAuditTrail, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">

        <input type="hidden" id="UserGroupID" runat="server" name="ID" />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Auto E-mail Audit Trail" />
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     BackButton_Visible="false" 
                     NewButton_Visible="false" 
                     EditButton_Visible="false" 
                     DeleteButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnSaveButton_Click ="Save_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="20%" />
            <col width="70%" />
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="E-mail Address" />:<br />
                    (<asp:Label ID="Label12" runat="server" Text="Separate address with ENTER" />)
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="txtEMAIL_AUDIT_TRAIL_ADDRESS" runat="Server" TextMode="multiLine" Wrap="true" Columns="90" Rows="5"/>
                </td>
            </tr>
        </table>
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate >
        <table width="100%" cellspacing="0" class="pm_section">
            <col width="10%" />
            <col width="10%" />
            <col width="10%" />
            <col width="70%" />
            <tr>
                <td class="pm_field_title" colspan="3">
                    <asp:Label runat="server" Text="Function List" /></td>
                <td class="pm_field_title" >
                    <asp:DropDownList ID="cboReportFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboReportFilter_SelectedIndexChanged">
                        <asp:ListItem Value="ALL" Text="--- Show All Functions ---" Selected="True"  />
                        <asp:ListItem Value="System" Text="System" />
                        <asp:ListItem Value="Security" Text="Security" />
                        <asp:ListItem Value="Personnel" Text="Personnel" />
                        <asp:ListItem Value="Leave" Text="Leave" />
                        <asp:ListItem Value="Payroll" Text="Payroll" />
                        <asp:ListItem Value="MPF" Text="Pension" />
                        <asp:ListItem Value="CostCenter" Text="Cost Center" />
                        <asp:ListItem Value="Attendance" Text="Attendance" />
                        <asp:ListItem Value="Taxation" Text="Taxation" />
                        <asp:ListItem Value="Training" Text="Training" />
                        <asp:ListItem Value="PersonnelReports" Text="Employee Reports" Enabled="false"  />
                        <asp:ListItem Value="PayrollReports" Text="Payroll & MPF Reports" Enabled="false" />
                        <asp:ListItem Value="TaxationReports" Text="Taxation Reports" Enabled="false" />
                    </asp:DropDownList> 
                </td>
            </tr>
            <asp:Panel ID="SystemPanel" runat="server" >
            <tr>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=SystemPermissions.ClientID %>','SystemFunctionEmailAlertInsert',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=SystemPermissions.ClientID %>','SystemFunctionEmailAlertUpdate',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=SystemPermissions.ClientID %>','SystemFunctionEmailAlertDelete',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="System" /></td>
            </tr>
            <asp:Repeater ID="SystemPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertInsert" runat="server" />
                            <asp:Label runat="server" Text="Insert" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertUpdate" runat="server" />
                            <asp:Label runat="server" Text="Update"/>
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertDelete" runat="server" />
                            <asp:Label ID="Label13" runat="server" Text="Delete"/>
                        </td>
                        <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            </asp:Panel>
            <asp:Panel ID="SecurityPanel" runat="server" >
            <tr>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=SecurityPermissions.ClientID %>','SystemFunctionEmailAlertInsert',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=SecurityPermissions.ClientID %>','SystemFunctionEmailAlertUpdate',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=SecurityPermissions.ClientID %>','SystemFunctionEmailAlertDelete',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="Security" /></td>
            </tr>
            <asp:Repeater ID="SecurityPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertInsert" runat="server" />
                            <asp:Label runat="server" Text="Insert" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertUpdate" runat="server" />
                            <asp:Label runat="server" Text="Update"/>
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertDelete" runat="server" />
                            <asp:Label ID="Label13" runat="server" Text="Delete"/>
                        </td>
                        <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            </asp:Panel>
            <asp:Panel ID="PersonnelPanel" runat="server" >
            <tr>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PersonnelPermissions.ClientID %>','SystemFunctionEmailAlertInsert',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PersonnelPermissions.ClientID %>','SystemFunctionEmailAlertUpdate',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PersonnelPermissions.ClientID %>','SystemFunctionEmailAlertDelete',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="Personnel" /></td>
            </tr>
            <asp:Repeater ID="PersonnelPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertInsert" runat="server" />
                            <asp:Label runat="server" Text="Insert" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertUpdate" runat="server" />
                            <asp:Label runat="server" Text="Update"/>&nbsp;&nbsp;
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertDelete" runat="server" />
                            <asp:Label ID="Label13" runat="server" Text="Delete"/>
                        </td>
                        <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            </asp:Panel>
            <asp:Panel ID="LeavePanel" runat="server" >
            <tr>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=LeavePermissions.ClientID %>','SystemFunctionEmailAlertInsert',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=LeavePermissions.ClientID %>','SystemFunctionEmailAlertUpdate',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=LeavePermissions.ClientID %>','SystemFunctionEmailAlertDelete',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="Leave" /></td>
            </tr>
            <asp:Repeater ID="LeavePermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertInsert" runat="server" />
                            <asp:Label runat="server" Text="Insert" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertUpdate" runat="server" />
                            <asp:Label runat="server" Text="Update"/>
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertDelete" runat="server" />
                            <asp:Label ID="Label13" runat="server" Text="Delete"/>
                        </td>
                        <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>            
            </asp:Panel>
            <asp:Panel ID="PayrollPanel" runat="server" >
            <tr>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PayrollPermissions.ClientID %>','SystemFunctionEmailAlertInsert',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PayrollPermissions.ClientID %>','SystemFunctionEmailAlertUpdate',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PayrollPermissions.ClientID %>','SystemFunctionEmailAlertDelete',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="Payroll" /></td>
            </tr>
            <asp:Repeater ID="PayrollPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertInsert" runat="server" />
                            <asp:Label runat="server" Text="Insert" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertUpdate" runat="server" />
                            <asp:Label runat="server" Text="Update"/>
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertDelete" runat="server" />
                            <asp:Label ID="Label13" runat="server" Text="Delete"/>
                        </td>
                        <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            </asp:Panel>
            <asp:Panel ID="MPFPanel" runat="server" >
            <tr>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=MPFPermissions.ClientID %>','SystemFunctionEmailAlertInsert',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=MPFPermissions.ClientID %>','SystemFunctionEmailAlertUpdate',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=MPFPermissions.ClientID %>','SystemFunctionEmailAlertDelete',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="Pension" /></td>
            </tr>
            <asp:Repeater ID="MPFPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertInsert" runat="server" />
                            <asp:Label runat="server" Text="Insert" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertUpdate" runat="server" />
                            <asp:Label runat="server" Text="Update"/>
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertDelete" runat="server" />
                            <asp:Label ID="Label13" runat="server" Text="Delete"/>
                        </td>
                        <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            </asp:Panel>
            <asp:Panel ID="CostCenterPanel" runat="server" >            
                <tr>
                    <td class="pm_field_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=CostCenterPermissions.ClientID %>','SystemFunctionEmailAlertInsert',this.checked);" />
                    </td>
                    <td class="pm_field_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=CostCenterPermissions.ClientID %>','SystemFunctionEmailAlertUpdate',this.checked);" />
                    </td>
                    <td class="pm_field_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=CostCenterPermissions.ClientID %>','SystemFunctionEmailAlertDelete',this.checked);" />
                    </td>
                    <td class="pm_field_header" style="text-align: left">
                        <asp:Label ID="Label1" runat="server" Text="Cost Center" /></td>
                </tr>
                <asp:Repeater ID="CostCenterPermissions" runat="server" OnItemDataBound="ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="SystemFunctionEmailAlertInsert" runat="server" />
                                <asp:Label ID="Label2" runat="server" Text="Insert" />
                            </td>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="SystemFunctionEmailAlertUpdate" runat="server" />
                                <asp:Label ID="Label3" runat="server" Text="Update"/>
                            </td>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="SystemFunctionEmailAlertDelete" runat="server" />
                                <asp:Label ID="Label13" runat="server" Text="Delete"/>
                            </td>
                            <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>            
            </asp:Panel>
            <asp:Panel ID="AttendancePanel" runat="server" >            
                <tr>
                    <td class="pm_field_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=AttendancePermissions.ClientID %>','SystemFunctionEmailAlertInsert',this.checked);" />
                    </td>
                    <td class="pm_field_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=AttendancePermissions.ClientID %>','SystemFunctionEmailAlertUpdate',this.checked);" />
                    </td>
                    <td class="pm_field_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=AttendancePermissions.ClientID %>','SystemFunctionEmailAlertDelete',this.checked);" />
                    </td>
                    <td class="pm_field_header" style="text-align: left">
                        <asp:Label ID="Label4" runat="server" Text="Attendance" /></td>
                </tr>
                <asp:Repeater ID="AttendancePermissions" runat="server" OnItemDataBound="ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="SystemFunctionEmailAlertInsert" runat="server" />
                                <asp:Label ID="Label2" runat="server" Text="Insert" />
                            </td>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="SystemFunctionEmailAlertUpdate" runat="server" />
                                <asp:Label ID="Label3" runat="server" Text="Update"/>
                            </td>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="SystemFunctionEmailAlertDelete" runat="server" />
                                <asp:Label ID="Label13" runat="server" Text="Delete"/>
                            </td>
                            <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>            
            </asp:Panel>
            <asp:Panel ID="TaxationPanel" runat="server" >
            <tr>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=TaxationPermissions.ClientID %>','SystemFunctionEmailAlertInsert',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=TaxationPermissions.ClientID %>','SystemFunctionEmailAlertUpdate',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=TaxationPermissions.ClientID %>','SystemFunctionEmailAlertDelete',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="Taxation" /></td>
            </tr>
            <asp:Repeater ID="TaxationPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertInsert" runat="server" />
                            <asp:Label runat="server" Text="Insert" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertUpdate" runat="server" />
                            <asp:Label runat="server" Text="Update"/>
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertDelete" runat="server" />
                            <asp:Label ID="Label13" runat="server" Text="Delete"/>
                        </td>
                        <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            </asp:Panel>
            <asp:Panel ID="TrainingPanel" runat="server" >            
                <tr>
                    <td class="pm_field_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=TrainingPermissions.ClientID %>','SystemFunctionEmailAlertInsert',this.checked);" />
                    </td>
                    <td class="pm_field_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=TrainingPermissions.ClientID %>','SystemFunctionEmailAlertUpdate',this.checked);" />
                    </td>
                    <td class="pm_field_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=TrainingPermissions.ClientID %>','SystemFunctionEmailAlertDelete',this.checked);" />
                    </td>
                    <td class="pm_field_header" style="text-align: left">
                        <asp:Label ID="Label5" runat="server" Text="Training" /></td>
                </tr>
                <asp:Repeater ID="TrainingPermissions" runat="server" OnItemDataBound="ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="SystemFunctionEmailAlertInsert" runat="server" />
                                <asp:Label ID="Label2" runat="server" Text="Insert" />
                            </td>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="SystemFunctionEmailAlertUpdate" runat="server" />
                                <asp:Label ID="Label3" runat="server" Text="Update"/>
                            </td>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="SystemFunctionEmailAlertDelete" runat="server" />
                                <asp:Label ID="Label13" runat="server" Text="Delete"/>
                            </td>
                            <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>            
            </asp:Panel>
            <asp:Panel ID="PersonnelReportsPanel" runat="server" Visible="false" >
            <tr>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PersonnelReportsPermissions.ClientID %>','SystemFunctionEmailAlertInsert',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PersonnelReportsPermissions.ClientID %>','SystemFunctionEmailAlertUpdate',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PersonnelReportsPermissions.ClientID %>','SystemFunctionEmailAlertDelete',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="Employee Reports" /></td>
            </tr>
            <asp:Repeater ID="PersonnelReportsPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertInsert" runat="server" />
                            <asp:Label runat="server" Text="Insert" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertUpdate" runat="server" />
                            <asp:Label runat="server" Text="Update"/>
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertDelete" runat="server" />
                            <asp:Label ID="Label13" runat="server" Text="Delete"/>
                        </td>
                        <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>            
            </asp:Panel>
            <asp:Panel ID="PayrollReportsPanel" runat="server" Visible="false" >
            <tr>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PayrollReportsPermissions.ClientID %>','SystemFunctionEmailAlertInsert',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PayrollReportsPermissions.ClientID %>','SystemFunctionEmailAlertUpdate',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PayrollReportsPermissions.ClientID %>','SystemFunctionEmailAlertDelete',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label ID="Label6" runat="server" Text="Payroll & MPF Reports" /></td>
            </tr>
            <asp:Repeater ID="PayrollReportsPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertInsert" runat="server" />
                            <asp:Label ID="Label7" runat="server" Text="Insert" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertUpdate" runat="server" />
                            <asp:Label ID="Label8" runat="server" Text="Update"/>
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertDelete" runat="server" />
                            <asp:Label ID="Label13" runat="server" Text="Delete"/>
                        </td>
                        <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>            
            </asp:Panel>
            <asp:Panel ID="TaxationReportsPanel" runat="server" Visible="false" >
            <tr>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=TaxationReportsPermissions.ClientID %>','SystemFunctionEmailAlertInsert',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=TaxationReportsPermissions.ClientID %>','SystemFunctionEmailAlertUpdate',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=TaxationReportsPermissions.ClientID %>','SystemFunctionEmailAlertDelete',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label ID="Label9" runat="server" Text="Taxation Reports" /></td>
            </tr>
            <asp:Repeater ID="TaxationReportsPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertInsert" runat="server" />
                            <asp:Label ID="Label10" runat="server" Text="Insert" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertUpdate" runat="server" />
                            <asp:Label ID="Label11" runat="server" Text="Update"/>
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="SystemFunctionEmailAlertDelete" runat="server" />
                            <asp:Label ID="Label13" runat="server" Text="Delete"/>
                        </td>
                        <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>     
            </asp:Panel>       
        </table>
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 