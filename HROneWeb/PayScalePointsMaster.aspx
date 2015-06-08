<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayScalePointsMaster.aspx.cs" Inherits="PayScalePointsMaster" MasterPageFile="~/MainMasterPage.master" %>

<%@ Register Src="~/controls/DetailToolBar.ascx" TagName="DetailToolBar" TagPrefix="tb" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/RecordListFooter.ascx" TagName="RecordListFooter" TagPrefix="tb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
		<table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_page_title">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Pay Scale Points Setup" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Scheme" />: <asp:DropDownList runat="server" ID="SchemeSelect" AutoPostBack="true" OnSelectedIndexChanged="AsAtDate_Changed" />

<%--                    <asp:Label ID="Label2" runat="server" Text="Effective As At Date" />: <asp:DropDownList runat="server" ID="YearSelect" AutoPostBack="true" OnSelectedIndexChanged="YearSelect_SelectedIndexChanged" />--%>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Effective As At" />: <uc1:WebDatePicker id="AsAtDate" runat="server" ShowDateFormatLabel="false" AutoPostBack="true" OnChanged="AsAtDate_Changed" /> </td>
                </td>
            </tr>
            <tr>
                <td>
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
                <%--<asp:AsyncPostBackTrigger ControlID="YearSelect" EventName="SelectedIndexChanged" />--%>
                <asp:AsyncPostBackTrigger ControlID="toolBar" />
                <asp:PostBackTrigger ControlID="btnUpload" />
            </Triggers>
            <ContentTemplate>
        <table border="0" width="100%" class="pm_list_section" cellspacing="0" cellpadding="1">
            <col width="26px" /> 
            <col width="150px" /> 
            <col width="150px" /> 
            <col width="150px" /> 
            <tr>
                <td class="pm_list_header">
                    <asp:Panel ID="SelectAllPanel" runat="server">
                    <input type="checkbox" onclick="checkAll('<%=Repeater.ClientID %>','DeleteItem',this.checked);" />
                    </asp:Panel>                    
                </td>
                <td class="pm_list_header">
                </td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_EffectiveDate" OnClick="ChangeOrder_Click" Text="Effective Date"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_ExpiryDate" OnClick="ChangeOrder_Click" Text="Expiry Date"/></td>
                <td align="left" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_SchemeCode" OnClick="ChangeOrder_Click" Text="Scheme Code"/></td>
                <td align="right" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_Point" OnClick="ChangeOrder_Click" Text="Point"/></td>
                <td align="right" class="pm_list_header">
                    <asp:LinkButton runat="server" ID="_Salary" OnClick="ChangeOrder_Click" Text="Salary"/></td>
            </tr>
            <tr id="AddPanel" runat="server">
                <td class="pm_list">
                </td>
                <td align="center" class="pm_list" >
                    <asp:Button ID="Add" runat="server" Text="Add" CssClass="button" OnClick="Add_Click" /></td>
                <td align="left" class="pm_list">
                    <uc1:WebDatePicker id="EffectiveDate" runat="server" ShowDateFormatLabel="false"/> </td>
                <td align="left" class="pm_list">
                    <uc1:WebDatePicker id="ExpiryDate" runat="server" ShowDateFormatLabel="false"/> </td>
                <td align="left" class="pm_list">
                    <asp:TextBox ID="SchemeCode" runat="server" /></td>
                <td align="right" class="pm_list">
                    <asp:TextBox ID="Point" runat="server" /></td>
                <td align="right" class="pm_list">
                    <asp:TextBox ID="Salary" runat="server" /></td>
            </tr>
            <asp:DataList ID="Repeater" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" ShowFooter="false"  
                ShowHeader="false" OnItemCommand="Repeater_ItemCommand" OnItemDataBound="Repeater_ItemDataBound">
                <EditItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="PayScaleMapID" />
                        </td>
                        <td class="pm_list" align="center">
                            <asp:Button ID="Save" runat="server" Text="Save" CssClass="button" />
                            <asp:Button ID="Cancel" runat="server" Text="Cancel" CssClass="button"/>
                        </td>

                        <td align="left" class="pm_list">
                            <uc1:WebDatePicker id="EffectiveDate" runat="server" ShowDateFormatLabel="false"/> </td>
                        <td align="left" class="pm_list">
                            <uc1:WebDatePicker id="ExpiryDate" runat="server" ShowDateFormatLabel="false"/> </td>
                        <td align="left" class="pm_list">
                            <asp:TextBox ID="SchemeCode" runat="server"/></td>
                        <td align="right" class="pm_list">
                            <asp:TextBox ID="Point" runat="server"/></td>
                        <td align="right" class="pm_list">
                            <asp:TextBox ID="Salary" runat="server"/></td>
                    </tr>
                </EditItemTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="pm_list" align="center">
                            <asp:CheckBox ID="DeleteItem" runat="server" />
                            <input type="hidden" runat="server" id="PayScaleMapID" />
                        </td>
                        <td class="pm_list" align="center">
                            <asp:Button ID="Edit" runat="server" CssClass="button" Text="Edit" />
                        </td>
                        <td align="left" class="pm_list">
                             <%#sbinding.getFValue(Container.DataItem,"EffectiveDate","yyyy-MM-dd")%></td>
                        <td align="left" class="pm_list">
                             <%#sbinding.getFValue(Container.DataItem,"ExpiryDate","yyyy-MM-dd")%></td>
                        <td align="left" class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"SchemeCode")%> </td>
                        <td align="right" class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"Point")%> </td>
                        <td align="right" class="pm_list">
                            <%#sbinding.getValue(Container.DataItem,"Salary")%> </td>                            
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
        <asp:Panel ID="ImportPanel" runat="server" >          
        <table class="pm_section" width="100%">
            <tr>
                <td class="pm_search_header">
                    <asp:Label ID="Label3" runat="server" Text="Upload Pay Scale Points by File" />:</td>
                <td class="pm_search">
                    <asp:FileUpload ID="BatchFile" runat="server" Width="400" />
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="button" OnClick="btnUpload_Click" />
                </td>
            </tr>
        </table>
        </asp:Panel>
</asp:Content> 