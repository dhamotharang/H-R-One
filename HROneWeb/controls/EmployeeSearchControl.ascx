<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EmployeeSearchControl.ascx.cs" Inherits="EmployeeSearchControl" %>
<%@ Register Src="~/controls/HierarchyCheckBoxList.ascx" TagName="HierarchyCheckBoxList" TagPrefix="uc1" %>
<%@ Register Src="~/controls/WebDatePicker.ascx" TagName="WebDatePicker" TagPrefix="uc1" %>
<%@ Register Src="~/controls/AdvancedCheckBoxList.ascx" TagName="AdvancedCheckBoxList" TagPrefix="uc1" %>
<%--    <script type="text/javascript" >
        function toggleAdvancedOption()
        {
            alert('<%=txtShowAdvanced.ClientID %>');
            toggleLayer('SearchEmployeeControl'); 
            toggleLayer('AdvancedSearchButton'); 
            toggleLayer('HideAdvancedSearchButton');
            alert('<%=txtShowAdvanced.ClientID %>');

            var showAdvancedCtrl = document.getElementById('<%=txtShowAdvanced.ClientID %>');
            if (showAdvancedCtrl.value=="true")
            {
                showAdvancedCtrl.value="false";
            }
            else
            {
                showAdvancedCtrl.value="true";
            }
        }
    </script>--%>
    <table width="100%" class="pm_section">
    <col width="15%" />
    <col width="35%" />
    <col width="15%" />
    <col width="35%" />
    <tr>
        <td class="pm_search_header" >
            <asp:Label ID="Label1" runat="server" Text="Employee No" />:
        </td>
        <td class="pm_search" >
            <asp:TextBox runat="server" ID="EmpNo" /></td>
        <td class="pm_search_header" >
            <asp:Label ID="Label2" runat="server" Text="Alias" />
            :
        </td>
        <td class="pm_search" >
            <asp:TextBox runat="server" ID="EmpAlias" /></td>
    </tr>
    <tr>
        <td class="pm_search_header">
            <asp:Label ID="Label3" runat="server" Text="Surname" />:
        </td>
        <td class="pm_search">
            <asp:TextBox runat="server" ID="EmpEngSurname" /></td>
        <td class="pm_search_header">
            <asp:Label ID="Label4" runat="server" Text="Other Name" />:
        </td>
        <td class="pm_search">
            <asp:TextBox runat="server" ID="EmpEngOtherName" /></td>
    </tr>
    <tr>
        <td class="pm_search_header">
            <asp:Label ID="Label9" runat="server" Text="Status" />:
        </td>
        <td class="pm_search" colspan="3">
        <!-- "A" Equivalent to "Active" in previous version -->
        <!-- "T" Equivalent to "Terminated" in previous version -->
            <asp:DropDownList runat="server" ID="EmpStatus" >
                <asp:ListItem Text="All" Value="" Selected="True" />
                <asp:ListItem Text="ACTIVE_STAFF" Value="A" />
                <asp:ListItem Text="Active / Will be terminated" Value="AT" />
                <asp:ListItem Text="Will be terminated / Terminated" Value="T" />
                <asp:ListItem Text="Terminated" Value="TERMINATED"  />
            </asp:DropDownList> 
        </td>
    </tr>
    </table>
    <input type="hidden" runat="server" id="txtShowAdvanced" />
    <div id="AdvancedSearchButton" style="display:block;" >
    <a href="javascript:void(0)" onclick="javascript:toggleAdvancedOption();" >
        <img src="images/bottom.GIF" alt="bottom.gif" style="border-width:0px" />
        <asp:Label ID="ShowSearchDetail" runat="server" Text="Show Advanced Options" Font-Size="XX-Small" />
    </a> 
    </div>    
    <div id="SearchEmployeeControl" style="display:none;"  >
    <table width="100%" class="pm_section">
    <col width="15%" />
    <col width="35%" />
    <col width="15%" />
    <col width="35%" />
    <tr>
        <td class="pm_search_header" >
            <asp:Label ID="Label5" runat="server" Text="Chinese Name" />:
        </td>
        <td class="pm_search" >
            <asp:TextBox runat="server" ID="EmpChiFullName" /></td>
        <td class="pm_search_header" >
            <asp:Label ID="Label6" runat="server" Text="Gender" />:
        </td>
        <td class="pm_search" >
            <asp:DropDownList runat="server" ID="EmpGender" /></td>
    </tr>
    <tr>
        <td class="pm_search_header">
            <asp:Label ID="Label14" runat="server" Text="HKID" />:
        </td>
        <td class="pm_search">
            <asp:TextBox runat="server" ID="EmpHKID" /></td>
        <td class="pm_search_header">
            <asp:Label ID="Label15" runat="server" Text="Passport No" />:
        </td>
        <td class="pm_search">
            <asp:TextBox runat="server" ID="EmpPassportNo" /></td>
    </tr>
    <tr>
        <td class="pm_search_header">
            <asp:Label ID="Label8" runat="server" Text="Join Date" />:
        </td>
        <td class="pm_search" colspan="3">
            <uc1:WebDatePicker id="JoinDateFrom" runat="server" />
            -
            <uc1:WebDatePicker id="JoinDateTo" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_search_header">
            <asp:Label ID="Label16" runat="server" Text="Last Employment Date" />:
        </td>
        <td class="pm_search" colspan="3">
            <uc1:WebDatePicker id="LastDateFrom" runat="server" />
            -
            <uc1:WebDatePicker id="LastDateTo" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_search_header">
           <asp:Label ID="Label20" runat="server" Text="Hierarchy" /> :
        </td>
        <td class="pm_search" colspan="3">
            <uc1:HierarchyCheckBoxList id="HierarchyCheckBoxList1" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="pm_search_header" >
           <asp:Label ID="Label10" runat="server" Text="Position"  /> :
        </td> 
        <td class="pm_search" colspan="3">
            <uc1:AdvancedCheckBoxList ID="PositionList" runat="server" />
        </td> 
    </tr>
    <tr>
        <td class="pm_search_header" >
           <asp:Label ID="Label17" runat="server" Text="Rank" /> :
        </td> 
        <td class="pm_search" colspan="3">
            <uc1:AdvancedCheckBoxList ID="RankList" runat="server" />
        </td> 
    </tr>
    <tr>
        <td class="pm_search_header" >
           <asp:Label ID="Label18" runat="server" Text="Employment Type" /> :
        </td> 
        <td class="pm_search" colspan="3">
            <uc1:AdvancedCheckBoxList ID="EmploymentTypeList" runat="server"  />
        </td> 
    </tr>
    <tr>
        <td class="pm_search_header" >
           <asp:Label ID="Label19" runat="server" Text="Staff Type" /> :
        </td> 
        <td class="pm_search" colspan="3">
            <uc1:AdvancedCheckBoxList ID="StaffTypeList" runat="server"  />
        </td> 
    </tr>
    <tr>
        <td class="pm_search_header">
           <asp:Label ID="Label13" runat="server" Text="Payroll Group" /> :
        </td>
        <td class="pm_search" colspan="3">
            <uc1:AdvancedCheckBoxList ID="PayrollGroupList" runat="server"  />
        </td>
    </tr>

    </table>
    </div>
    <div id="HideAdvancedSearchButton" style="display:none; " >
    <a href="javascript:void(0)" onclick="javascript:toggleAdvancedOption();" >
        <img src="images/top.GIF" alt="top.gif" style="border-width:0px"/>
        <asp:Label ID="Label7" runat="server" Text="Hide Advanced Options" Font-Size="XX-Small"  />
    </a> 
    </div>    
    <table width="100%" class="pm_section">
    <col width="15%" />
    <col width="35%" />
    <col width="15%" />
    <col width="35%" />
        <asp:PlaceHolder runat="server" ID="Placeholder1" />
    </table> 
    
<%--<script type="text/javascript" >
        if (document.all.<%=txtShowAdvanced.ClientID %>.value=="true")
        {
            toggleLayer('SearchEmployeeControl'); 
            toggleLayer('AdvancedSearchButton'); 
            toggleLayer('HideAdvancedSearchButton');
        }
</script>
--%><br />