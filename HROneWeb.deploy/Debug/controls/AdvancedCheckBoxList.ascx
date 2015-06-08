<%@ control language="C#" autoeventwireup="true" inherits="AdvancedCheckBoxList, HROneWeb.deploy" %>
<div style="visibility:visible;  position:relative;" >
    <asp:Label ID="lblOptionList" runat="server" style="background-color:White; "  />
    <asp:Image ID="btnMore" runat="server" ImageUrl="~/images/Add_blue.png" Width="15px" Height="15px" />
</div>
<div id="checkBoxListDiv" class="ModalPopupDiv" runat="server" style="visibility:hidden; display:none; position:absolute;" >
    <div class="searchemployeecheckboxlist">
        <asp:CheckBoxList ID="CheckBoxListObject" runat="server"  CellPadding="0" CellSpacing="0" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Table" />
    </div>
    <asp:Button ID="btnClose" runat="server"  UseSubmitBehavior="false" Text="Close" OnClientClick="return false;" CSSClass="button" />
    <asp:Button ID="btnSelectAll" runat="server"  UseSubmitBehavior="false" Text="Select All" OnClientClick="return false;" CSSClass="button" />
    <asp:Button ID="btnClearAll" runat="server"  UseSubmitBehavior="false" Text="Unselect All" OnClientClick="return false;" CSSClass="button" />
</div>
