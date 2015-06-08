<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AVCPlan_View.aspx.cs" Inherits="AVCPlan_View" MasterPageFile="~/MainMasterPage.master"  %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="AVCPlanID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="AVC Plan Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label Text="View AVC Plan" runat="server" />:
                    <%=AVCPlanCode.Text %>
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     SaveButton_Visible="false" 
                     OnBackButton_Click="Back_Click"
                     OnEditButton_Click ="Edit_Click"
                     OnDeleteButton_Click="Delete_ClickTop"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <tr>
                <td class="pm_field_title" colspan="7">
                    <asp:Label ID="Label1" Text="Additional Voluntary Contribution" runat="server" />
                </td>
                <td class="pm_field_title" align="right">
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="30%" />
            <col width="70%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Plan Code" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AVCPlanCode" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Description" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AVCPlanDesc" runat="Server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Use residual for employer" runat="server" />:
                </td>
                <td class="pm_field">
                    <!-- Start 0000106, Ricky So, 2014/10/22 -->
                   <asp:Label ID="AVCPlanEmployerResidual" runat="server" />  &nbsp &nbsp &nbsp &nbsp Cap:
                   <asp:Label ID="AVCPlanEmployerResidualCap" runat="server" />
                    <!-- End 0000106, Ricky So, 2014/10/22 -->
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Use residual for employee" runat="server" />:
                </td>
                <td class="pm_field">
                    <!-- Start 0000106, Ricky So, 2014/10/22 -->
                   <asp:Label ID="AVCPlanEmployeeResidual" runat="server" /> &nbsp &nbsp &nbsp &nbsp Cap:
                   <asp:Label ID="AVCPlanEmployeeResidualCap" runat="server" />
                    <!-- End 0000106, Ricky So, 2014/10/22 -->
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Employee AVC exemption period the same as MPF" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AVCPlanUseMPFExemption" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Start Contribute on Date of Join" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AVCPlanJoinDateStart" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Contribute for age over MPF maximum age limit" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AVCPlanContributeMaxAge" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Contribute for income less than MPF minimum relevant income" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AVCPlanContributeMinRI" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Max. employer's voluntary contribution" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AVCPlanMaxEmployerVC" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Max. employee's voluntary contribution" runat="server" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AVCPlanMaxEmployeeVC" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel5" runat="server" Text="Rounding Rule for Employer Contribution" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AVCPlanEmployerRoundingRule" runat="server" />
                    <asp:Label ID="AVCPlanEmployerDecimalPlace" runat="server" />
                    <asp:Label ID="ILabel6" runat="server" Text="decimal place(s)" />
                </td>
            </tr>
            <tr>                
                <td class="pm_field_header">
                    <asp:Label ID="ILabel7" runat="server" Text="Rounding Rule for Employee Contribution" />:
                </td>
                <td class="pm_field">
                    <asp:Label ID="AVCPlanEmployeeRoundingRule" runat="server" />
                    <asp:Label ID="AVCPlanEmployeeDecimalPlace" runat="server" />
                    <asp:Label ID="ILabel8" runat="server" Text="decimal place(s)" />
                </td>            
            </tr>
            <tr id="NotRemoveContributionFromTopUpRow" runat="server" >                
                <td class="pm_field_header">
                    <asp:Label ID="Label4" runat="server" Text="Do not deduct MPF Only payment being contributed" />:<br />
                    <asp:Label ID="Label6" runat="server" Text="(Required to define which payment is contributed after MPF Only)" />
                </td>
                <td class="pm_field">
                    <asp:Label ID="AVCPlanNotRemoveContributionFromTopUp" runat="server" />
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td align="right">
                </td>
            </tr>
        </table>
        <br />

        <asp:PlaceHolder ID="PaymentCeilingSection" runat="server" Visible="true" >
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label3" Text="Payment Setting" runat="server" />
                </td>
            </tr>
        </table>

        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" /> 
            <col width="200px" /> 
            <col width="400px" /> 
            <tr>
                <td class="pm_list_header">
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PaymentCode" OnClick="AVCPlanCeiling_ChangeOrder_Click" Text="Payment Code"/>
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PaymentCodeDesc" OnClick="AVCPlanCeiling_ChangeOrder_Click" Text="Description"/>
                </td>
                <td align="right" class="pm_list_header" >
                    <asp:LinkButton runat="server" ID="_AVCPlanPaymentCeilingAmount" OnClick="AVCPlanCeiling_ChangeOrder_Click" Text="Ceiling Amount"/>
                </td>
                <td align="center" class="pm_list_header">
                    <asp:Label runat="server" ID="Label5" Text="Contributed after MPF Only" />
                </td>
            </tr>
            <asp:Repeater ID="avcCeilingRepeater" runat="server" >
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" Visible="False" />
                        </td>
                        <td class="pm_list">
                            <%#ceilingbinding.getValue(Container.DataItem, "PaymentCode")%>
                        </td>
                        <td class="pm_list">
                            <%#ceilingbinding.getValue(Container.DataItem, "PaymentCodeDesc")%>
                        </td>
                        <td class="pm_list" align="right">
                            <%#ceilingbinding.getFValue(Container.DataItem, "AVCPlanPaymentCeilingAmount", "0.00")%>
                        </td>
                        <td class="pm_list" align="right">
                            <%#ceilingbinding.getValue(Container.DataItem, "AVCPlanPaymentConsiderAfterMPF")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <tb:RecordListFooter id="avcCeilingListFooter" runat="server"
             ShowAllRecords="true" 
          />
        </asp:PlaceHolder>            
        <br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label2" Text="Year of Service" runat="server" />
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <asp:Button ID="Delete" CssClass="button" runat="server" Text="Delete" OnClick="Delete_Click" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
            <asp:AsyncPostBackTrigger ControlID="Delete" EventName="Click" />
        </Triggers>
        <ContentTemplate >
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" /> 
            <col width="120px" /> 
            <col width="150px" /> 
            <tr>
                <td class="pm_list_header">
                </td>
                <td class="pm_list_header">
                </td>
                <td align="left" class="pm_list_header">
                    <asp:Label runat="server" ID="_AVCPlanDetailYearOfService" Text="Year of Service less than" />
                </td>
                <td align="center" class="pm_list_header">
                    <asp:Label runat="server" Text="Type" />
                </td>
                <td align="right" class="pm_list_header">
                    <asp:Label runat="server" ID="_AVCPlanDetailEEBelowRI" Text="AVC below MAX" />
                </td>
                <td align="right" class="pm_list_header">
                    <asp:Label runat="server" ID="_AVCPlanDetailEEAboveRI" Text="AVC above MAX" />
                </td>
                <td align="right" class="pm_list_header">
                    <asp:Label runat="server" ID="_AVCPlanDetailEEFix" Text="Fix Contribution Amount" />
                </td>
            </tr>
            <tr id="AddPanel1" runat="server" >
                <td class="pm_list" rowspan="2">
                </td>
                <td class="pm_list" align="center" rowspan="2">
                    <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" />
                </td>
                <td class="pm_list" rowspan="2" align="right">
                    <asp:TextBox ID="AVCPlanDetailYearOfService" runat="server" Columns="3" />
                </td>
                <td class="pm_list" align="center">
                    <asp:Label runat="server" Text="Employer" />
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="AVCPlanDetailERBelowRI" runat="server" Columns="3" />
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="AVCPlanDetailERAboveRI" runat="server" Columns="3" />
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="AVCPlanDetailERFix" runat="server" Columns="10" />
                </td>
            </tr>
            <tr id="AddPanel2" runat="server">
                <td class="pm_list" align="center">
                    <asp:Label ID="ILabel1" runat="server" Text="Employee" />
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="AVCPlanDetailEEBelowRI" runat="server" Columns="3" />
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="AVCPlanDetailEEAboveRI" runat="server" Columns="3" />
                </td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="AVCPlanDetailEEFix" runat="server" Columns="10" />
                </td>
            </tr>
            <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                ShowFooter="false" ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
                <EditItemTemplate>
                    <tr>
                        <td class="pm_list" align="center" rowspan="2">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="AVCPlanDetailID" />
                        </td>
                        <td class="pm_list" align="center" rowspan="2">
                            <asp:Button ID="Save" runat="server" Text="Save" CssClass="button" />
                            <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="button" />
                        </td>
                        <td class="pm_list" rowspan="2" align="right">
                            <asp:TextBox ID="AVCPlanDetailYearOfService" runat="server" Columns="3"/>
                        </td>
                        <td class="pm_list"  align="center">
                            <asp:Label ID="ILabel2" runat="server" Text="Employer"/>
                        </td>
                        <td class="pm_list" align="right">
                            <asp:TextBox ID="AVCPlanDetailERBelowRI" runat="server" Columns="3"/>
                        </td>
                        <td class="pm_list" align="right">
                            <asp:TextBox ID="AVCPlanDetailERAboveRI" runat="server" Columns="3"/>
                        </td>
                        <td class="pm_list" align="right">
                            <asp:TextBox ID="AVCPlanDetailERFix" runat="server" Columns="10"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:Label ID="ILabel1" runat="server" Text="Employee" />
                        </td>
                        <td class="pm_list" align="right">
                            <asp:TextBox ID="AVCPlanDetailEEBelowRI" runat="server" Columns="3"/>
                        </td>
                        <td class="pm_list" align="right">
                            <asp:TextBox ID="AVCPlanDetailEEAboveRI" runat="server" Columns="3"/>
                        </td>
                        <td class="pm_list" align="right">
                            <asp:TextBox ID="AVCPlanDetailEEFix" runat="server" Columns="10"/>
                        </td>
                    </tr>
                </EditItemTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center" rowspan="2">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="AVCPlanDetailID" />
                        </td>
                        <td class="pm_list" align="center" rowspan="2">
                            <asp:Button ID="Edit" runat="server" Text="Edit" CssClass="button" />
                        </td>
                        <td class="pm_list" rowspan="2" align="right">
                            <%#sbinding.getValue(Container.DataItem,"AVCPlanDetailYearOfService")%>
                        </td>
                        <td class="pm_list" align="center">
                            <asp:Label ID="ILabel2" runat="server" Text="Employer" />
                        </td>
                        <td class="pm_list" align="right">
                            <%#sbinding.getValue(Container.DataItem,"AVCPlanDetailERBelowRI")%>
                        </td>
                        <td class="pm_list" align="right">
                            <%#sbinding.getValue(Container.DataItem,"AVCPlanDetailERAboveRI")%>
                        </td>
                        <td class="pm_list" align="right">
                            <%#sbinding.getValue(Container.DataItem,"AVCPlanDetailERFix")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:Label ID="ILabel1" runat="server" Text="Employee" />
                        </td>
                        <td class="pm_list" align="right">
                            <%#sbinding.getValue(Container.DataItem,"AVCPlanDetailEEBelowRI")%>
                        </td>
                        <td class="pm_list" align="right">
                            <%#sbinding.getValue(Container.DataItem,"AVCPlanDetailEEAboveRI")%>
                        </td>
                        <td class="pm_list" align="right">
                            <%#sbinding.getValue(Container.DataItem,"AVCPlanDetailEEFix")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:DataList>
            </table>
        <tb:RecordListFooter id="ListFooter" runat="server"
             ShowAllRecords="true" ListOrderBy="AVCPlanDetailYearOfService" ListOrder="true" 
          />
        </ContentTemplate> 
        </asp:UpdatePanel> 
</asp:Content> 