<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserGroup_Edit.aspx.cs" Inherits="UserGroup_Edit" MasterPageFile="~/MainMasterPage.master" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">

        <input type="hidden" id="UserGroupID" runat="server" name="ID" />
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="User Group Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <%=CurID<=0?HROne.Common.WebUtility.GetLocalizedString("Add"):HROne.Common.WebUtility.GetLocalizedString("Edit") %>
                    <asp:Label runat="server" Text="User Group" />:
                    <%=UserGroupName.Text %>
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     EditButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnSaveButton_Click ="Save_Click"
                     OnDeleteButton_Click="Delete_Click"
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
                    <asp:Label runat="server" Text="Name" />:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="UserGroupName" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Description" />:
                </td>
                <td class="pm_field">
                    <asp:TextBox ID="UserGroupDesc" runat="Server" />
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
            <col width="80%" />
            <tr>
                <td class="pm_field_title" colspan="2">
                    <asp:Label runat="server" Text="Function Permissions" /></td>
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
                        <asp:ListItem Value="eChannel" Text="e-channel" />
                        <asp:ListItem Value="PersonnelReports" Text="Employee Reports" />
                        <asp:ListItem Value="PayrollReports" Text="Payroll & MPF Reports" />
                        <asp:ListItem Value="TaxationReports" Text="Taxation Reports" />
                    </asp:DropDownList> 
                </td>
            </tr>
            <asp:Panel ID="SystemPanel" runat="server" >
            <tr>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=SystemPermissions.ClientID %>','Permission',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=SystemPermissions.ClientID %>','WritePermission',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="System" /></td>
            </tr>
            <asp:Repeater ID="SystemPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="Permission" runat="server" />
                            <asp:Label runat="server" Text="Read" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="WritePermission" runat="server" />
                            <asp:Label runat="server" Text="Read/Write" />
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
                    <input type="checkbox" onclick="checkAll('<%=SecurityPermissions.ClientID %>','Permission',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=SecurityPermissions.ClientID %>','WritePermission',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="Security" /></td>
            </tr>
            <asp:Repeater ID="SecurityPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="Permission" runat="server" />
                            <asp:Label runat="server" Text="Read" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="WritePermission" runat="server" />
                            <asp:Label runat="server" Text="Read/Write" />
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
                    <input type="checkbox" onclick="checkAll('<%=PersonnelPermissions.ClientID %>','Permission',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PersonnelPermissions.ClientID %>','WritePermission',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="Personnel" /></td>
            </tr>
            <asp:Repeater ID="PersonnelPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="Permission" runat="server" />
                            <asp:Label runat="server" Text="Read" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="WritePermission" runat="server" />
                            <asp:Label runat="server" Text="Read/Write" />&nbsp;&nbsp;
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
                    <input type="checkbox" onclick="checkAll('<%=LeavePermissions.ClientID %>','Permission',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=LeavePermissions.ClientID %>','WritePermission',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="Leave" /></td>
            </tr>
            <asp:Repeater ID="LeavePermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="Permission" runat="server" />
                            <asp:Label runat="server" Text="Read" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="WritePermission" runat="server" />
                            <asp:Label runat="server" Text="Read/Write" />
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
                    <input type="checkbox" onclick="checkAll('<%=PayrollPermissions.ClientID %>','Permission',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PayrollPermissions.ClientID %>','WritePermission',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="Payroll" /></td>
            </tr>
            <asp:Repeater ID="PayrollPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="Permission" runat="server" />
                            <asp:Label runat="server" Text="Read" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="WritePermission" runat="server" />
                            <asp:Label runat="server" Text="Read/Write" />
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
                    <input type="checkbox" onclick="checkAll('<%=MPFPermissions.ClientID %>','Permission',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=MPFPermissions.ClientID %>','WritePermission',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="Pension" /></td>
            </tr>
            <asp:Repeater ID="MPFPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="Permission" runat="server" />
                            <asp:Label runat="server" Text="Read" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="WritePermission" runat="server" />
                            <asp:Label runat="server" Text="Read/Write" />
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
                        <input type="checkbox" onclick="checkAll('<%=CostCenterPermissions.ClientID %>','Permission',this.checked);" />
                    </td>
                    <td class="pm_field_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=CostCenterPermissions.ClientID %>','WritePermission',this.checked);" />
                    </td>
                    <td class="pm_field_header" style="text-align: left">
                        <asp:Label ID="Label1" runat="server" Text="Cost Center" /></td>
                </tr>
                <asp:Repeater ID="CostCenterPermissions" runat="server" OnItemDataBound="ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="Permission" runat="server" />
                                <asp:Label ID="Label2" runat="server" Text="Read" />
                            </td>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="WritePermission" runat="server" />
                                <asp:Label ID="Label3" runat="server" Text="Read/Write" />
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
                        <input type="checkbox" onclick="checkAll('<%=AttendancePermissions.ClientID %>','Permission',this.checked);" />
                    </td>
                    <td class="pm_field_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=AttendancePermissions.ClientID %>','WritePermission',this.checked);" />
                    </td>
                    <td class="pm_field_header" style="text-align: left">
                        <asp:Label ID="Label4" runat="server" Text="Attendance" /></td>
                </tr>
                <asp:Repeater ID="AttendancePermissions" runat="server" OnItemDataBound="ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="Permission" runat="server" />
                                <asp:Label ID="Label2" runat="server" Text="Read" />
                            </td>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="WritePermission" runat="server" />
                                <asp:Label ID="Label3" runat="server" Text="Read/Write" />
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
                    <input type="checkbox" onclick="checkAll('<%=TaxationPermissions.ClientID %>','Permission',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=TaxationPermissions.ClientID %>','WritePermission',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="Taxation" /></td>
            </tr>
            <asp:Repeater ID="TaxationPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="Permission" runat="server" />
                            <asp:Label runat="server" Text="Read" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="WritePermission" runat="server" />
                            <asp:Label runat="server" Text="Read/Write" />
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
                        <input type="checkbox" onclick="checkAll('<%=TrainingPermissions.ClientID %>','Permission',this.checked);" />
                    </td>
                    <td class="pm_field_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=TrainingPermissions.ClientID %>','WritePermission',this.checked);" />
                    </td>
                    <td class="pm_field_header" style="text-align: left">
                        <asp:Label ID="Label5" runat="server" Text="Training" /></td>
                </tr>
                <asp:Repeater ID="TrainingPermissions" runat="server" OnItemDataBound="ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="Permission" runat="server" />
                                <asp:Label ID="Label2" runat="server" Text="Read" />
                            </td>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="WritePermission" runat="server" />
                                <asp:Label ID="Label3" runat="server" Text="Read/Write" />
                            </td>
                            <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>            
            </asp:Panel>
            <asp:Panel ID="eChannelPanel" runat="server" >            
                <tr>
                    <td class="pm_field_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=eChannelPermissions.ClientID %>','Permission',this.checked);" />
                    </td>
                    <td class="pm_field_header" align="center">
                        <input type="checkbox" onclick="checkAll('<%=eChannelPermissions.ClientID %>','WritePermission',this.checked);" />
                    </td>
                    <td class="pm_field_header" style="text-align: left">
                        <asp:Label ID="Label12" runat="server" Text="e-channel" /></td>
                </tr>
                <asp:Repeater ID="eChannelPermissions" runat="server" OnItemDataBound="ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="Permission" runat="server" />
                                <asp:Label ID="Label2" runat="server" Text="Read" />
                            </td>
                            <td class="pm_field" style="white-space:nowrap;">
                                <input type="checkbox" id="WritePermission" runat="server" />
                                <asp:Label ID="Label3" runat="server" Text="Read/Write" />
                            </td>
                            <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>            
            </asp:Panel>
            <asp:Panel ID="PersonnelReportsPanel" runat="server" >
            <tr>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PersonnelReportsPermissions.ClientID %>','Permission',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PersonnelReportsPermissions.ClientID %>','WritePermission',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label runat="server" Text="Employee Reports" /></td>
            </tr>
            <asp:Repeater ID="PersonnelReportsPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="Permission" runat="server" />
                            <asp:Label runat="server" Text="Read" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="WritePermission" runat="server" />
                            <asp:Label runat="server" Text="Read/Write" />
                        </td>
                        <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>            
            </asp:Panel>
            <asp:Panel ID="PayrollReportsPanel" runat="server" >
            <tr>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PayrollReportsPermissions.ClientID %>','Permission',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=PayrollReportsPermissions.ClientID %>','WritePermission',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label ID="Label6" runat="server" Text="Payroll & MPF Reports" /></td>
            </tr>
            <asp:Repeater ID="PayrollReportsPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="Permission" runat="server" />
                            <asp:Label ID="Label7" runat="server" Text="Read" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="WritePermission" runat="server" />
                            <asp:Label ID="Label8" runat="server" Text="Read/Write" />
                        </td>
                        <td class="pm_field">
                            <asp:Label ID="FunctionCode" runat="server" />
                            <asp:Label ID="Description" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>            
            </asp:Panel>
            <asp:Panel ID="TaxationReportsPanel" runat="server" >
            <tr>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=TaxationReportsPermissions.ClientID %>','Permission',this.checked);" />
                </td>
                <td class="pm_field_header" align="center">
                    <input type="checkbox" onclick="checkAll('<%=TaxationReportsPermissions.ClientID %>','WritePermission',this.checked);" />
                </td>
                <td class="pm_field_header" style="text-align: left">
                    <asp:Label ID="Label9" runat="server" Text="Taxation Reports" /></td>
            </tr>
            <asp:Repeater ID="TaxationReportsPermissions" runat="server" OnItemDataBound="ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="Permission" runat="server" />
                            <asp:Label ID="Label10" runat="server" Text="Read" />
                        </td>
                        <td class="pm_field" style="white-space:nowrap;">
                            <input type="checkbox" id="WritePermission" runat="server" />
                            <asp:Label ID="Label11" runat="server" Text="Read/Write" />
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