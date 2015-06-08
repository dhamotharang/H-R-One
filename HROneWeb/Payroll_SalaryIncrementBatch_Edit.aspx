<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Payroll_SalaryIncrementBatch_Edit.aspx.cs" Inherits="Payroll_SalaryIncrementBatch_Edit"  MasterPageFile="~/MainMasterPage.master"  %>

<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>
<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/EmployeeSearchControl.ascx" TagName="EmployeeSearchControl" TagPrefix="uc2" %>



<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="BatchID" runat="server" name="ID" />
        
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label Text="PayScale - Salary Increment Batch" runat="server" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
<%--
                <td>
                  <asp:Label Text="View Backpay Process" runat="server" />:
                    <%=.Text %>
                </td>
--%>
            </tr>
        </table>

        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     EditButton_Visible="false"
                     DeleteButton_Visible="true"
                     SaveButton_Visible="true"
                     OnBackButton_Click="Back_Click"
                     OnDeleteButton_Click="Delete_Click"
                     OnSaveButton_Click="Save_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <col width="15%" />
            <col width="35%" />
            <col width="15%" />
            <col width="35%" />
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label3" Text="As At Date " runat="server" />:</td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="AsAtDate" runat="server" /> </td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label2" Text="Status " runat="server" />:</td>
                <td class="pm_field"  >
                    <asp:Label ID="Status" runat="Server" /></td>
            </tr>            
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label4" Text="Deferred Batch" runat="server"  />?</td>
                <td class="pm_field" >
                    <asp:CheckBox ID="DeferredBatch" runat="server" visible="true" OnCheckedChanged="DeferredBatch_OnClick" AutoPostBack="true" />

                <td class="pm_field" ></td>
                <td class="pm_field" ></td>
            </tr>
            <tr id="CNDRow" visible="false" runat="server">
                <td class="pm_field_header" >
                    <asp:Label ID="Label8" Text="Payment Date" runat="server" />:</td>
                <td class="pm_field" >
                    <uc1:WebDatePicker id="PaymentDate" runat="server" /></td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label12" Text="Payment Code" runat="server" />:</td>
                <td class="pm_field" >
                    <asp:DropDownList ID="PaymentCodeID" runat="Server"  />
                </td>                    
            </tr>            
            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label5" Text="Upload Date Time " runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="UploadDateTime" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label6" Text="Uploaded By " runat="server" />:</td>
                <td class="pm_field"  >
                    <asp:Label ID="UploadBy" runat="Server" /></td>                    
            </tr>

            <tr>
                <td class="pm_field_header" >
                    <asp:Label ID="Label7" Text="Confirm Date Time " runat="server" />:</td>
                <td class="pm_field" >
                    <asp:Label ID="ConfirmDateTime" runat="Server" /></td>
                <td class="pm_field_header" >
                    <asp:Label ID="Label9" Text="Confirmed By " runat="server" />:</td>
                <td class="pm_field"  >
                    <asp:Label ID="ConfirmBy" runat="Server" /></td>                    
            </tr>            
        </table>
        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="toolbar"/>
        </Triggers>
        </asp:UpdatePanel >
        
        
        <table class="pm_section" width="100%">
            <tr>
                <td align="left">
                    <asp:Button ID="btnExport" runat="Server" Text="Re-Generate Employee List" CssClass="button" OnClick="btnExport_Click" />
<%--                    
                    <asp:Button ID="btnImport" runat="Server" Text="Import New Point Records" CssClass="button" OnClick="btnImport_Click" />
                    <asp:Button ID="btnConfirm" runat="Server" Text="Confirm Salary Increment" CssClass="button" OnClick="btnConfirm_Click" />
--%>            </td>
<%--                
                <td align="right">
                    <asp:Button ID="Button1" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
--%>        
                <td align="left">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button ID="Button1" runat="Server" Text="Delete" CssClass="button" OnClick="btnDelete_Click" />
                </td>
                <td align="left">
                </td>
            </tr>
        </table>

        <ContentTemplate >
        <uc2:EmployeeSearchControl id="EmployeeSearchControl1" runat="server" visible="false" />
        
        <table border="0" width="100%" cellpadding="0" cellspacing="0" class="pm_list_section">
            <col width="26px" />
            <col width="100px" />
            <col width="200px" />
            <col width="150px" />
            <col width="200px" />
            <col width="100px" />
            <col width="100px" />
            <col width="100px" />
            <col width="100px" />
            <tr>
                <td class="pm_list_header" align="center">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                        <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','ItemSelect',this.checked);" />
                    </asp:Panel>                   
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EmpNo" OnClick="ChangeOrder_Click" Text="Employee No." /></td>
                <td align="left" class="pm_list_header" colspan="2">
                    <asp:LinkButton runat="server" ID="_EmpName" OnClick="ChangeOrder_Click" Text="Employee Name" /></td>
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_SchemeCode" OnClick="ChangeOrder_Click" Text="Scheme Code" /></td>                    
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_Capacity" OnClick="ChangeOrder_Click" Text="Capacity" /></td>                    
                <td  class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_CurrentPoint" OnClick="ChangeOrder_Click" Text="Current Point" /></td>
                <td class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_NewPoint" OnClick="ChangeOrder_Click" Text="New Point" /></td>
            </tr>
            <asp:Repeater ID="Repeater" runat="server" OnItemDataBound="Repeater_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="ItemSelect" runat="server" />
                            <input type="hidden" runat="server" id="DetailID" value=<%#detailBinding.getValue(Container.DataItem,"DetailID")%> />
                        </td>
                        <td class="pm_list">
                           <a href="<%#HROne.Common.WebUtility.URLwithEncryptQueryString(Session, "Emp_View.aspx?EmpID=" + detailBinding.getValue(Container.DataItem,"EmpID"))%>"><%#detailBinding.getValue(Container.DataItem, "EmpNo")%></a>
                            <%--#detailBinding.getValue(Container.DataItem,"EmpNo")--%>
                        </td>
                        <td class="pm_list">
                            <%#detailBinding.getValue(Container.DataItem, "EmpEngSurname")%>
                        </td>
                        <td class="pm_list">
                            <%#detailBinding.getValue(Container.DataItem, "EmpEngOthername")%>
                        </td>
                        <td class="pm_list">
                            <%#detailBinding.getValue(Container.DataItem, "SchemeCode")%>
                        </td>
                        <td class="pm_list">
                            <%#detailBinding.getValue(Container.DataItem, "Capacity")%>
                        </td>
                        <td class="pm_list">
                            <%#detailBinding.getValue(Container.DataItem, "CurrentPoint")%>
                        </td>
                        <td class="pm_list">
                            <%#detailBinding.getValue(Container.DataItem, "NewPoint")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <tb:RecordListFooter id="ListFooter" runat="server" ShowAllRecords="true" />
        </ContentTemplate> 
</asp:Content>
