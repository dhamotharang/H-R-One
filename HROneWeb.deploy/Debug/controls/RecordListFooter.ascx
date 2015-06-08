<%@ control language="C#" autoeventwireup="true" inherits="RecordListFooter, HROneWeb.deploy" enableviewstate="false" %>
<input type="hidden" id="Order" runat="server" />
<input type="hidden" id="OrderBy" runat="server" />
<input type="hidden" id="CurPage" runat="server" />
<input type="hidden" id="NumPage" runat="server" />
<input type="hidden" id="NumRecord" runat="server" />
<input type="hidden" id="RecordsPerPage" runat="server" />
<table width="100%" cellspacing="0" cellpadding="0" class="pm_list_pagenav">
    <tr>
        <td align="right">
            <asp:LinkButton ID="FirstPage" runat="server" OnClick="FirstPage_Click" CssClass="pm_link_pagenav" >
                <img height="11" alt="Start" id="FirstPageImg" runat="server" src="images/start.gif"
                    width="13" style="text-align:center; border:0;" />
                    &nbsp;<asp:Label ID="Label1"  Text="Start" runat="server" />&nbsp;&nbsp;
            </asp:LinkButton>
            <asp:LinkButton ID="PrevPage" runat="server" OnClick="PrevPage_Click" CssClass="pm_link_pagenav" >
                <img height="11" alt="Previous" id="PrevPageImg" runat="server" src="images/previous.gif"
                    width="8" style="text-align:center; border:0;" />
                    &nbsp;<asp:Label ID="Label2" Text="Previous" runat="server"/>&nbsp;
            </asp:LinkButton>
            <span class="inside_text_blackform"><asp:Label ID="lblRecordCount" runat="server" /></span> &nbsp;
            <asp:LinkButton ID="NextPage" runat="server" OnClick="NextPage_Click" CssClass="pm_link_pagenav" >
                &nbsp;<asp:Label ID="Label4" Text="Next" runat="server" />&nbsp;
                <img height="11" alt="Next" id="NextPageImg" runat="server" src="images/next.gif"
                    width="8" style="text-align:center; border:0;"  />
            </asp:LinkButton>
            <asp:LinkButton ID="LastPage" runat="server" OnClick="LastPage_Click" CssClass="pm_link_pagenav" >
                &nbsp;<asp:Label ID="Label5"  Text="End" runat="server" />&nbsp;
                <img height="11" alt="End" id="LastPageImg" runat="server" src="images/end.gif"
                    width="13" style="text-align:center; border:0;"  />
            </asp:LinkButton>
        </td>
    </tr>
</table>