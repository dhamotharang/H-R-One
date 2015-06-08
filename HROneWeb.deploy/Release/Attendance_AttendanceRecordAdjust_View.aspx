<%@ page language="C#" autoeventwireup="true" inherits="Attendance_AttendanceRecordAdjust_View, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>



<%@ Register Src="~/controls/Emp_Header.ascx" TagName="Emp_Header" TagPrefix="uc1" %>
<%@ Register Src="~/controls/Attendance_AttendanceRecordList.ascx" TagName="Attendance_AttendanceRecordList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
        <input type="hidden" id="EmpID" runat="server" name="ID" />
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Title" Text="Attendance Record Adjustment" runat="server" />
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <asp:Button ID="Back" Text="- Back -" runat="server" CssClass="button" OnClick="Back_Click" UseSubmitBehavior="false" />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openAlert(); return false;"/>
                </td>
            </tr>
        </table>
        <table border="0" width="100%" cellspacing="0" cellpadding="0" class="pm_field_section">
            <tr>
                <td colspan="2">
                    <uc1:Emp_Header ID="ucEmp_Header" runat="server" />
                </td>
            </tr>

        </table>
        
             
        
        
    <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional" >
    <Triggers>
    </Triggers>
    <ContentTemplate >
        <uc1:Attendance_AttendanceRecordList ID="Attendance_AttendanceRecordList" runat="server"  />
   </ContentTemplate> 
   </asp:UpdatePanel> 

</asp:Content> 