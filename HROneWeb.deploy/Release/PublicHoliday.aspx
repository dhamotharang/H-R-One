<%@ page language="C#" autoeventwireup="true" inherits="PublicHoliday, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Public Holiday Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label runat="server" Text="Public Holiday Year" />: <asp:DropDownList runat="server" ID="YearSelect" AutoPostBack="true" OnSelectedIndexChanged="YearSelect_SelectedIndexChanged" />
                    <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" CssClass="button" />
                </td>
            </tr>
        </table>
        
            
                
        
        <table width="100%" cellpadding="0" cellspacing="0" class="pm_button_section">
            <tr>
                <td>
                    <tb:DetailToolBar id="toolBar" runat="server"
                     NewButton_Visible="false" 
                     EditButton_Visible="false" 
                     BackButton_Visible="false"
                     SaveButton_Visible ="false"
                     OnDeleteButton_Click="Delete_Click"
                      />
                </td>
                <td align="right">
                    <asp:Button ID="btnHelp" runat="server" CSSClass="button" Text=" Help" Visible="false" UseSubmitBehavior="false" OnClientClick="openHelp(); return false;"/>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="ListUpdatePanel" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="YearSelect" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="toolBar" />
                <asp:PostBackTrigger ControlID="btnUpdate" />
            </Triggers>
            <ContentTemplate>
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" /> 
            <col width="120px" /> 
            <col width="200px" /> 
            <tr>
                <td class="pm_list_header">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','DeleteItem',this.checked);" />
                    </asp:Panel>                    
                </td>
                <td class="pm_list_header">
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PublicHolidayDate" OnClick="ChangeOrder_Click" Text="Date"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_PublicHolidayDesc" OnClick="ChangeOrder_Click" Text="Description"/></td>
            </tr>
            <tr id="AddPanel" runat="server">
                <td class="pm_list">
                </td>
                <td class="pm_list" align="center">
                    <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" /></td>
                <td class="pm_list">
                    <uc1:WebDatePicker id="PublicHolidayDate" runat="server" />
                </td>
                    
                <td class="pm_list">
                    <asp:TextBox ID="PublicHolidayDesc" runat="server" /></td>
            </tr>
            <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false"  
                ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
                <EditItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="PublicHolidayID" />
                        </td>
                        <td class="pm_list" align="center">
                            <asp:Button ID="Save" runat="server" Text="Save" CssClass="button" />
                            <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="button"/>
                        </td>
                        <td class="pm_list">
                            <uc1:WebDatePicker id="PublicHolidayDate" runat="server" />
                        </td>
                        <td class="pm_list">
                            <asp:TextBox ID="PublicHolidayDesc" runat="server" />
                        </td>
                    </tr>
                </EditItemTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="PublicHolidayID" />
                        </td>
                        <td class="pm_list" align="center">
                            <asp:Button ID="Edit" runat="server" CssClass="button" Text="Edit" />
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getFValue(Container.DataItem,"PublicHolidayDate","yyyy-MM-dd")%>
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"PublicHolidayDesc")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:DataList>
        </table>
        <tb:RecordListFooter id="ListFooter" runat="server"
            ShowAllRecords="true" visible="true" 
          />
          </ContentTemplate> 
          </asp:UpdatePanel> 
          <br />
        <asp:Panel ID="HolidayImport" runat="server" >          
        <table class="pm_section" width="100%">
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label1" runat="server" Text="Update Holiday Table by File" />:</td>
                <td class="pm_search">
                    <asp:FileUpload ID="HolidayTableFile" runat="server" Width="400" />
                    <%-- Start 0000099, KuangWei, 2014-09-29 --%>
                    <asp:Button ID="btnUpdate" runat="server" Text="Import" CssClass="button" OnClick="btnUpdate_Click" />
                    <%-- End 0000099, KuangWei, 2014-09-29 --%>
                </td>
            </tr>
        </table>
        </asp:Panel>
</asp:Content> 