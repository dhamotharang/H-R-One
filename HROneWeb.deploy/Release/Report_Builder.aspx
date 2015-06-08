<%@ page language="C#" masterpagefile="~/MainMasterPage.master" autoeventwireup="true" inherits="Report_Builder, HROneWeb.deploy" viewStateEncryptionMode="Always" %>

<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContentPlaceHolder" runat="Server">
    <%--    <script type="text/javascript">
    function showandhidepanels(section){
        switch(section){
            case 'Step1NextBtn' : showPanel2(); break;
            case 'Step2NextBtn' : showPanel3(); break;
            case 'Step2BackBtn' : showPanel1(); break;
            case 'Step3NextBtn' : showPanel4(); break;
            case 'Step3BackBtn' : showPanel2(); break;
        }
    }
    
    function showPanel1(){
        document.getElementById("rbstep1").style.display = "block";
        document.getElementById("rbstep2").style.display = "none";
        document.getElementById("rbstep3").style.display = "none";
        document.getElementById("rbstep4").style.display = "none";
        
        //$("#rbstep1").show();
        //$("#rbstep2").hide();
        //$("#rbstep3").hide();
        //$("#rbstep4").hide();
    }
    
    function showPanel2(){
        document.getElementById("rbstep1").style.display = "none";
        document.getElementById("rbstep2").style.display = "block";
        document.getElementById("rbstep3").style.display = "none";
        document.getElementById("rbstep4").style.display = "none";
    }
    
        function showPanel3(){
        document.getElementById("rbstep1").style.display = "none";
        document.getElementById("rbstep2").style.display = "none";
        document.getElementById("rbstep3").style.display = "block";
        document.getElementById("rbstep4").style.display = "none";
    }
    
        function showPanel4(){
        document.getElementById("rbstep1").style.display = "none";
        document.getElementById("rbstep2").style.display = "none";
        document.getElementById("rbstep3").style.display = "none";
        document.getElementById("rbstep4").style.display = "block";
    }
    </script>--%>
    <div>
        <!--Step 1 Panel -->
        <asp:Panel runat="server" ID="rbstep1" Visible="true">
            <table class="pm_section" width="100%">
                <tr>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                            <tr>
                                <td>
                                    <asp:Label ID="lblReportHeader" runat="server" Text="Report Builder - Step 1" />
                                </td>
                                <td align="left">
                                    <asp:Button runat="server" ID="Step1NextBtn" CssClass="button" Text=" Next " Visible="true"
                                        OnClick="Step1NextBtn_OnClick" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_table_td">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="Step1ChooseTemplateRad" GroupName="rdRadio" runat="server" Text="Choose a template"
                                        Visible="true" Checked="true" AutoPostBack="true" OnCheckedChanged="rdRadio_Change" />
                                </td>
                                <td class="pm_search">
                                    <asp:DropDownList ID="ModuleName" runat="Server" AutoPostBack="true" Visible="true"
                                        OnSelectedIndexChanged="ModuleName_Changed" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:50%"></td>
                            </tr>
                            <tr>
                                <td class="pm_search" style="width:50%">
                                    <asp:RadioButton ID="Step1SelectFavariteRad" GroupName="rdRadio" runat="server" Text="Select from your favarite"
                                        Visible="true" AutoPostBack="true" OnCheckedChanged="rdRadio_Change" Width="200px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:ListBox runat="server" ID="Step1FavriteTemplateList" SelectionMode="Single"
                                        Rows="10" Enabled="false" OnSelectedIndexChanged="Step1FavriteTemplateList_OnChange" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <!--Step 2 Panel -->
        <asp:Panel runat="server" ID="rbstep2" Visible="false">
            <table class="pm_section" width="100%">
                <tr>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Report Builder - Step 2" />
                                </td>
                                <td align="left">
                                    <asp:Button runat="server" ID="Step2BackBtn" CssClass="button" Text=" Back " Visible="true"
                                        OnClick="Step2BackBtn_OnClick" />
                                </td>
                                <td align="left">
                                    <asp:Button runat="server" ID="Step2NextBtn" CssClass="button" Text=" Next " Visible="true"
                                        OnClick="Step2NextBtn_OnClick" />
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_table_td">
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Select fields to display" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="pm_search" style="margin-left: 20px">
                                    <div class="searchemployeecheckboxlist">
                                        <asp:CheckBoxList ID="CheckBoxListObject" runat="server" CellPadding="0" CellSpacing="0"
                                            RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Table" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <!--Step 3 Panel -->
        <asp:Panel runat="server" ID="rbstep3" Visible="false">
            <table class="pm_section" width="100%">
                <tr>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Report Builder - Step 3" />
                                </td>
                                <td align="left">
                                    <asp:Button runat="server" ID="Step3BackBtn" CssClass="button" Text=" Back " Visible="true"
                                        OnClick="Step3BackBtn_OnClick" />
                                </td>
                                <td align="left">
                                    <asp:Button runat="server" ID="Step3NextBtn" CssClass="button" Text=" Next " Visible="true"
                                        OnClick="Step3NextBtn_OnClick" />
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                            <tr>
                                <td>
                                    <asp:Label ID="lbCriteria" runat="Server" Text="Selection Criteria" />
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                            <tr>
                                <td class="pm_search" style="width:30%">
                                    <asp:DropDownList ID="FilterFieldCheckBoxList" runat="Server" AutoPostBack="true"
                                        Visible="true" OnSelectedIndexChanged="SelectedField_Changed" />
                                </td>
                                <td style="width:70%">
                                    <asp:DropDownList ID="CharCriteriaD" runat="Server" Visible="false" />
                                    <asp:DropDownList ID="SizeCriteriaD" runat="Server" Visible="false" />
                                    <uc1:WebDatePicker ID="FilterDatePicker" runat="server" Visible="false" />
                                    <asp:TextBox ID="Step3CriteriaTextField" runat="server" Visible="false" />
                                    <asp:Button ID="Step3AddFilterBtn" runat="server" Text="Add Filter" Enabled="false"
                                        OnClick="Step3AddFilterBtn_OnClick" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="searchemployeecheckboxlist">
                                        <asp:CheckBoxList ID="CriteriaFieldCBL" runat="server" CellPadding="0" CellSpacing="0"
                                            RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Table" Width="300px" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <!--Step 4 Panel -->
        <asp:Panel runat="server" ID="rbstep4" Visible="false">
            <table class="pm_section" width="100%">
                <tr>
                    <td style="width: 610px">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="pm_section_title">
                            <tr>
                                <td style="width: 546px">
                                    <asp:Label ID="Label2" runat="server" Text="Report Builder - Step 4" />
                                </td>
                                <td align="left" style="width: 139px">
                                    <asp:Button ID="StartOverBtn" runat="server" CssClass="button" Text=" Start Over "
                                        Visible="true" UseSubmitBehavior="false" OnClick="StartOverBtn_OnClick" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="FinishBtn" runat="server" CssClass="button" Text=" Finish " Visible="true"
                                        UseSubmitBehavior="false" OnClick="FinishBtn_OnClick" />
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lbExportData" runat="server" Text="Export Data" Width="148px" CssClass="head" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lbFileName" runat="server" Text="Filename" Width="133px" />
                                </td>
                                <td style="width: 384px">
                                    <asp:TextBox runat="server" ID="ExportFileName" Width="300px" />
                                </td>
                                <td>
                                    <asp:Button ID="ExportBtn" runat="server" CssClass="button" Text=" Export " Visible="true"
                                        UseSubmitBehavior="false" OnClick="Step4ExportBtn_OnClick" Width="70px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp</td>
                            </tr>
                            <tr>
                                <td style="width: 265px">
                                    <asp:Label ID="Label4" runat="server" Text="Save Template" Width="133px" />
                                </td>
                                <td style="width: 384px">
                                    <asp:TextBox runat="server" ID="TemplateName" Width="300px" />
                                </td>
                                <td>
                                    <asp:Button ID="SaveTemplateBtn" runat="server" CssClass="button" Text=" Save " Visible="true"
                                        UseSubmitBehavior="false" OnClick="SaveTemplateBtn_OnClick" Width="70px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
