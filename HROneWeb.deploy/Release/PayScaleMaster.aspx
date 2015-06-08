<%@ page language="C#" autoeventwireup="true" inherits="PayScaleMaster, HROneWeb.deploy" masterpagefile="~/MainMasterPage.master" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Pay Scale Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Scheme" />: <asp:DropDownList runat="server" ID="SchemeSelect" AutoPostBack="true" OnSelectedIndexChanged="SchemeSelect_SelectedIndexChanged" />
                    
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
<%--                <asp:AsyncPostBackTrigger ControlID="SchemeSelect" EventName="SchemeSelected_IndexChanged" />  --%>
                <asp:AsyncPostBackTrigger ControlID="toolBar" />
                <asp:PostBackTrigger ControlID="btnUpload" />
            </Triggers>
            <ContentTemplate>
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" /> 
            <col width="250px" /> 
            <col width="200px" />
            <col width="150px" />
            <col width="150px" />
            <col width="150px" />
            <col width="150px" /> 
            <col width="20px" />
            <tr>
                <td class="pm_list_header">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','DeleteItem',this.checked);" />
                    </asp:Panel>                    
                </td>
                <td class="pm_list_header">
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_SchemeCode" OnClick="ChangeOrder_Click" Text="Scheme"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_Capacity" OnClick="ChangeOrder_Click" Text="Capacity"/></td>
                <td align="right" class="pm_list_header" align="right">
                    <asp:LinkButton runat="server" ID="_FirstPoint" OnClick="ChangeOrder_Click" Text="First Point"/></td>
                <td align="right" class="pm_list_header" align="right">
                    <asp:LinkButton runat="server" ID="_MidPoint" OnClick="ChangeOrder_Click" Text="Mid Point"/></td>
                <td align="right" class="pm_list_header" align="right">
                    <asp:LinkButton runat="server" ID="_LastPoint" OnClick="ChangeOrder_Click" Text="Last Point"/></td>
                <td class="pm_list" />
            </tr>
            <tr id="AddPanel" runat="server">
                <td class="pm_list">
                </td>
                <td class="pm_list" align="center">
                    <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" /></td>
                <td class="pm_list">
                    <asp:TextBox ID="SchemeCode" runat="server" /></td>
                <td class="pm_list">
                    <asp:TextBox ID="Capacity" runat="server" /></td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="FirstPoint" runat="server" /></td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="MidPoint" runat="server" /></td>
                <td class="pm_list" align="right">
                    <asp:TextBox ID="LastPoint" runat="server" /></td>
                <td class="pm_list" />
            </tr>
            <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false"  
                ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
                <EditItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="PayScaleID" />
                        </td>
                        <td class="pm_list" align="center">
                            <asp:Button ID="Save" runat="server" Text="Save" CssClass="button" />
                            <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="button"/>
                        </td>

                        <td class="pm_list">
                            <asp:TextBox ID="SchemeCode" runat="server" /></td>
                        <td class="pm_list">
                            <asp:TextBox ID="Capacity" runat="server" /></td>
                        <td class="pm_list" align="right">
                            <asp:TextBox ID="FirstPoint" runat="server" /></td>
                        <td class="pm_list" align="right">
                            <asp:TextBox ID="MidPoint" runat="server" /></td>
                        <td class="pm_list" align="right">
                            <asp:TextBox ID="LastPoint" runat="server" /></td>
                        <td class="pm_list" />
                    </tr>
                </EditItemTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="PayScaleID" />
                        </td>
                        <td class="pm_list" align="center">
                            <asp:Button ID="Edit" runat="server" CssClass="button" Text="Edit" />
                        </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"SchemeCode")%> </td>
                        <td class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"Capacity")%> </td>
                        <td class="pm_list" align="right">
                            <%#sbinding.getValue(Container.DataItem,"FirstPoint")%> </td>
                        <td class="pm_list" align="right">
                            <%#sbinding.getValue(Container.DataItem,"MidPoint")%> </td>                            
                        <td class="pm_list" align="right">
                            <%#sbinding.getValue(Container.DataItem,"LastPoint")%> </td>
                        <td class="pm_list" />
                    </tr>
                </ItemTemplate>
            </asp:DataList>
        </table>
        <tb:RecordListFooter id="ListFooter" runat="server"
            OnFirstPageClick="FirstPage_Click"
            OnPrevPageClick="PrevPage_Click"
            OnNextPageClick="NextPage_Click"
            OnLastPageClick="LastPage_Click"
            ListOrderBy="SchemeCode"
            ListOrder="true" 
        />
        </ContentTemplate> 
        </asp:UpdatePanel> 
        <br />
        <asp:Panel ID="IMPORT" runat="server" >          
        <table class="pm_section" width="100%">
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label3" runat="server" Text="Upload Pay Scale by File" />:</td>
                <td class="pm_search">
                    <asp:FileUpload ID="BatchFile" runat="server" Width="400" />
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="button" OnClick="btnUpload_Click" />
                </td>
            </tr>
        </table>
        </asp:Panel>
</asp:Content> 