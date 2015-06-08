<%@ control language="C#" autoeventwireup="true" inherits="HROne.UI.Scheduler.Attendance_RosterTable_Edit_AdvancedTemplate, HROneESS.deploy" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="rsAdvancedEdit rsAdvancedModal" style="position: relative">
    <div class="rsModalBgTopLeft">
    </div>
    <div class="rsModalBgTopRight">
    </div>
    <div class="rsModalBgBottomLeft">
    </div>
    <div class="rsModalBgBottomRight">
    </div>
    <%-- Title bar. --%>
    <div class="rsAdvTitle">
        <%-- The rsAdvInnerTitle element is used as a drag handle when the form is modal. --%>
        <h1 class="rsAdvInnerTitle"><asp:Label runat="server" Text="Detail" /></h1>
<asp:LinkButton runat="server" ID="AdvancedEditCloseButton" CssClass="rsAdvEditClose"
CommandName="Cancel" CausesValidation="false" ToolTip='<%# Owner.Localization.AdvancedClose %>'>
            <%= Owner.Localization.AdvancedClose %>
        </asp:LinkButton>
    </div>
    <div class="rsAdvContentWrapper">
        <%-- Scroll container - when the form height exceeds MaximumHeight scrollbars will appear on this element--%>
        <div class="rsAdvOptionsScroll">
            <asp:Panel runat="server" ID="BasicControlsPanel" CssClass="rsAdvBasicControls">
                <input type="hidden" id="RosterTableID" runat="server" name="ID" />
                <input type="hidden" id="EmpID" runat="server" name="EmpID" />
                <table>
                    <tr>
                        <td><asp:Label ID="Label1" runat="server" Text="Employee No." />:</td>
                        <td><asp:Label ID="EmpNo" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="Label2" runat="server" Text="Employee Name" />:</td>
                        <td><asp:Label ID="EmpName" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="Label3" runat="server" Text="Date" />:</td>
                        <td><asp:Label ID="RosterTableDate" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="Label4" runat="server" Text="Client" />:</td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" >
                            <ContentTemplate >
                                <asp:DropDownList ID="RosterClientID" runat="server" OnSelectedIndexChanged="RosterClientID_SelectedIndexChanged" AutoPostBack="true"  />
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="Label5" runat="server" Text="Site" />:</td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                            <Triggers >
                                <asp:AsyncPostBackTrigger ControlID="RosterClientID" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate >
                                <asp:DropDownList ID="RosterClientSiteID" runat="server"  OnSelectedIndexChanged="RosterClientSiteID_SelectedIndexChanged"  AutoPostBack="true" />
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="Label6" runat="server" Text="Roster Code" />:</td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" >
                            <Triggers >
                                <asp:AsyncPostBackTrigger ControlID="RosterClientSiteID" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="RosterClientID" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate >
                                <asp:DropDownList ID="RosterCodeID" runat="server" OnSelectedIndexChanged="RosterCodeID_SelectedIndexChanged" AutoPostBack="true" />
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="Label7" runat="server" Text="Time" />:</td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" >
                            <Triggers >
                                <asp:AsyncPostBackTrigger ControlID="RosterCodeID" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate >
                                <asp:TextBox ID="RosterTableOverrideInTime" runat="server" /> - <asp:TextBox ID="RosterTableOverrideOutTime" runat="server" /> 
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                
            </asp:Panel> 

        </div>
        <asp:Panel runat="server" ID="ButtonsPanel" CssClass="rsAdvancedSubmitArea">
            <div class="rsAdvButtonWrapper">
                  <asp:Button runat="server" ID="SubmitButton" Text="Update" OnClick="SubmitButton_Click"
	                />
                  <asp:Button runat="server" ID="CancelButton" Text="Cancel" CommandName="Cancel"
	                />
            </div>
        </asp:Panel> 
    </div>
</div>
