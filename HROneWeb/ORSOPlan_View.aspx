<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ORSOPlan_View.aspx.cs" Inherits="ORSOPlan_View" MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="ORSOPlanID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="P-Fund Plan Setup" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label Text="View P-Fund Plan" runat="server" />:
                    <%=ORSOPlanCode.Text %>
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

        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width ="30%" />
            <col width ="70%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Plan Code" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="ORSOPlanCode" runat="Server" /></td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Description" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="ORSOPlanDesc" runat="Server" /></td>
            </tr>
            
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Scheme Number" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="ORSOPlanSchemeNo" runat="Server" /></td>
            </tr>
            
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Name of Employer" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="ORSOPlanCompanyName" runat="Server" /></td>
            </tr>    
                    
            <tr>
                <td class="pm_field_header" >
                    <asp:Label Text="Pay Center" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="ORSOPlanPayCenter" runat="Server" /></td>
            </tr>    
                  
                  
            <tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Use residual for employer" />:
                </td>
                <td class="pm_field">
                    <!-- Start 0000084, Ricky So, 2014/08/21 -->                
                    <asp:Label ID="ORSOPlanEmployerResidual" runat="server" />  &nbsp &nbsp &nbsp &nbsp Cap:
                    <asp:Label ID="ORSOPlanEmployerResidualCap" runat="server" />
                    <!-- End 0000084, Ricky So, 2014/08/21 -->                
                </td>
			</tr>
			
			<tr>
                <td class="pm_field_header">
                    <asp:Label runat="server" Text="Use residual for employee" />:
                </td>
                <td class="pm_field">
                    <!-- Start 0000084, Ricky So, 2014/08/21 -->                
                    <asp:Label ID="ORSOPlanEmployeeResidual" runat="server" /> &nbsp &nbsp &nbsp &nbsp Cap:
                    <asp:Label ID="ORSOPlanEmployeeResidualCap" runat="server" />
                    <!-- End 0000084, Ricky So, 2014/08/21 -->                
                </td>
            </tr>                  
                  
                  
                  
                  
                  
                  
                  
                  
                  
                  
                  
                  
                  
                        
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Max. employer's contribution" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="ORSOPlanMaxEmployerVC" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label Text="Max. employee's contribution" runat="server" />:</td>
                <td class="pm_field">
                    <asp:Label ID="ORSOPlanMaxEmployeeVC" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="pm_field_header">
                    <asp:Label ID="ILabel5" runat="server" Text="Rounding Rule for Employer Contribution" />  :</td>
                <td class="pm_field">
                    <asp:Label ID="ORSOPlanEmployerRoundingRule" runat="server" />
                    <asp:Label ID="ORSOPlanEmployerDecimalPlace" runat="server" />
                    <asp:Label ID="ILabel6" runat="server" Text="decimal place(s)" />
                </td>
            </tr>
            <tr>                
                <td class="pm_field_header">
                    <asp:Label ID="ILabel7" runat="server" Text="Rounding Rule for Employee Contribution" />  :</td>
                <td class="pm_field">
                    <asp:Label ID="ORSOPlanEmployeeRoundingRule" runat="server" />
                    <asp:Label ID="ORSOPlanEmployeeDecimalPlace" runat="server" />
                    <asp:Label ID="ILabel8" runat="server" Text="decimal place(s)" />
                </td>            
            </tr>
        </table>
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
            <asp:AsyncPostBackTrigger ControlID="Delete" EventName="Click"/>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        <ContentTemplate >
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" /> 
            <col width="120px" /> 
            <col width="150px" /> 
            <tr>
                <td class="pm_list_header">
                </td>
                <td  class="pm_list_header">
                </td>
                <td align="left" class="pm_list_header">
                    <asp:Label runat="server" ID="_ORSOPlanDetailYearOfService" Text="Year of Service less than" />
                </td>
                <td align="left" class="pm_list_header">
                    <asp:Label runat="server" Text="Type" />
                </td>
                <td align="left" class="pm_list_header">
                    <asp:Label runat="server" ID="_ORSOPlanDetailEE" Text="% of Contribution" />
                </td>
                <td align="left" class="pm_list_header">
                    <asp:Label runat="server" ID="_ORSOPlanDetailEEFix" Text="Fix Contribution Amount" />
                </td>

            </tr>
            <tr id="AddPanel1" runat="server" >
                <td class="pm_list" rowspan="2">
                </td>
                <td class="pm_list" align="center" rowspan="2">
                    <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" /></td>
                <td class="pm_list" rowspan="2">
                    <asp:TextBox ID="ORSOPlanDetailYearOfService" runat="server" Columns="3" style="text-align:right;" /></td>
                <td class="pm_list">
                    <asp:Label runat="server" Text="Employer" />
                </td>
                <td class="pm_list">
                    <asp:TextBox ID="ORSOPlanDetailER" runat="server" Columns="3" style="text-align:right;" /></td>
                <td class="pm_list">
                    <asp:TextBox ID="ORSOPlanDetailERFix" runat="server" Columns="10" style="text-align:right;" /></td>
            </tr>
            <tr id="AddPanel2" runat="server">
                <td class="pm_list">
                    <asp:Label ID="ILabel1" runat="server" Text="Employee" />
                </td>
                <td class="pm_list">
                    <asp:TextBox ID="ORSOPlanDetailEE" runat="server" Columns="3" style="text-align:right;" /></td>
                <td class="pm_list">
                    <asp:TextBox ID="ORSOPlanDetailEEFix" runat="server" Columns="10" style="text-align:right;" /></td>
            </tr>
            <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                ShowFooter="false" ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
                <EditItemTemplate>
                    <tr>
                        <td class="pm_list" align="center" rowspan="2">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="ORSOPlanDetailID" />
                        </td>
                        <td class="pm_list" align="center" rowspan="2">
                            <asp:Button ID="Save" runat="server" Text="Save" CssClass="button" />
                            <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="button" />
                        </td>
                        <td class="pm_list" rowspan="2">
                            <asp:TextBox ID="ORSOPlanDetailYearOfService" runat="server" Columns="3" style="text-align:right;" />
                        </td>
                        <td class="pm_list">
                            <asp:Label ID="ILabel2" runat="server" Text="Employer"/>
                        </td>
                        <td class="pm_list">
                            <asp:TextBox ID="ORSOPlanDetailER" runat="server" Columns="3" style="text-align:right;" />
                        </td>
                        <td class="pm_list">
                            <asp:TextBox ID="ORSOPlanDetailERFix" runat="server" Columns="10" style="text-align:right;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list">
                            <asp:Label ID="ILabel1" runat="server" Text="Employee" />
                        </td>
                        <td class="pm_list">
                            <asp:TextBox ID="ORSOPlanDetailEE" runat="server" Columns="3" style="text-align:right;" />
                        </td>

                        <td class="pm_list">
                            <asp:TextBox ID="ORSOPlanDetailEEFix" runat="server" Columns="10" style="text-align:right;" />
                        </td>
                    </tr>
                </EditItemTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center" rowspan="2">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="ORSOPlanDetailID" />
                        </td>
                        <td class="pm_list" align="center" rowspan="2">
                            <asp:Button ID="Edit" runat="server" Text="Edit" CssClass="button" />
                        </td>
                        <td class="pm_list" rowspan="2">
                            <%#sbinding.getValue(Container.DataItem,"ORSOPlanDetailYearOfService")%>
                        </td>
                        <td class="pm_list">
                            <asp:Label ID="ILabel2" runat="server" Text="Employer" />
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"ORSOPlanDetailER")%>
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"ORSOPlanDetailERFix")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="pm_list">
                            <asp:Label ID="ILabel1" runat="server" Text="Employee" />
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"ORSOPlanDetailEE")%>
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"ORSOPlanDetailEEFix")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:DataList>
        </table>
        <tb:RecordListFooter id="ListFooter" runat="server"
            ShowAllRecords="true" ListOrderBy="ORSOPlanDetailYearOfService" ListOrder="true" 
          />
    </ContentTemplate> 
    </asp:UpdatePanel> 
</asp:Content> 