<%@ control language="C#" autoeventwireup="true" inherits="HierarchyCheckBoxList, HROneESS.deploy" %>

<div style="visibility:visible;  position:relative;" >
    <asp:Label ID="lblOptionList" runat="server" style="background-color:White; "  />
    <asp:Image ID="btnMore" runat="server" ImageUrl="~/images/Add_blue.png" Width="15px" Height="15px" />
</div> 
<div id="checkBoxListDiv" class="ModalPopupDiv" runat="server" style="visibility:hidden; display:none; position:absolute;" >
    <table >
        <col width="100%" />
        <asp:Repeater ID="CompanyRepeater" runat="server" OnItemDataBound="CompanyRepeater_ItemDataBound">
            <ItemTemplate >
            <tr class="pm_list_header">
                <td >
                    <div style="visibility:visible;  position:relative; width:100%; white-space:nowrap;" >
                        <asp:Image ID="btnExpand" runat="server" ImageUrl="~/images/Collapsed.png" style="visibility:visible;"/>
                        <asp:Image ID="btnCollapse" runat="server" ImageUrl="~/images/Expanded.png" style="visibility:hidden;display:none;" />
                        <input type="hidden" runat="server" id="CompanyID" />
                        <asp:CheckBox ID="ItemSelect" runat="server" />
                    </div> 
                    <div id="checkBoxListDiv" class="ModalPopupDiv" runat="server" style="visibility:hidden; display:none; position:relative;" >
                    <table width="100%" >
                        <col width="100%" />
                        <tr class="pm_field_header">
                            <td >
                                <asp:Repeater ID="HierarchyLevel" runat="server" OnItemDataBound="HierarchyLevel_ItemDataBound">
                                    <ItemTemplate>
                                        <div style="visibility:visible;  position:relative;" >
                                            <asp:Image ID="btnExpand" runat="server" ImageUrl="~/images/Collapsed.png" style="visibility:visible;"/>
                                            <asp:Image ID="btnCollapse" runat="server" ImageUrl="~/images/Expanded.png" style="visibility:hidden;display:none;" />
                                            <asp:Label ID="HLevel" runat="server" />:<asp:Label ID="lblOptionList" runat="server"   />
                                        </div> 
                                        <div id="checkBoxListDiv" class="ModalPopupDiv" runat="server" style="visibility:hidden; display:none; position:relative;" >
                                            <div class="searchemployeecheckboxlist">
                                                <asp:CheckBoxList ID="HierarchyElementList" runat="server"  CellPadding="0" CellSpacing="0" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Table" />
                                            </div>
                                            <asp:Button ID="btnSelectAll" runat="server"  UseSubmitBehavior="false" Text="Select All" OnClientClick="return false;" CSSClass="button" />
                                            <asp:Button ID="btnClearAll" runat="server"  UseSubmitBehavior="false" Text="Unselect All" OnClientClick="return false;" CSSClass="button" />
                                        </div> 
                                    </ItemTemplate>
                                </asp:Repeater>
                            </td>
                        </tr> 
                    </table> 
                    </div>
                </td>
            </tr>
            </ItemTemplate>
        </asp:Repeater>
        <tr>
            <td>
                <asp:Button ID="btnClose" runat="server"  UseSubmitBehavior="false" Text="Close" OnClientClick="return false;" CSSClass="button" />
                <asp:Button ID="btnSelectAll" runat="server"  UseSubmitBehavior="false" Text="Select All" OnClientClick="return false;" CSSClass="button" />
                <asp:Button ID="btnClearAll" runat="server"  UseSubmitBehavior="false" Text="Unselect All" OnClientClick="return false;" CSSClass="button" />
            </td>
        </tr>
    </table>
</div>