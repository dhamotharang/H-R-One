<%@ control language="C#" autoeventwireup="true" inherits="WebDatePicker, HROneWeb.deploy" %>
<asp:TextBox ID="DateTextBox" runat="server" Columns="10" MaxLength="10" />
<asp:PlaceHolder ID="PopUpSection" runat="server" >
<img height="15" src="calendar/calendar.gif" alt="calendar" width="16" style="border-width:0;" onclick="popUpCalendar(this,document.getElementById('<%=DateTextBox.ClientID%>'),'<%=DateFormatString%>')" />
<span style="font-size: 8pt;">
<asp:Label ID="DateFormatLabel" runat="server"  />
</span>
</asp:PlaceHolder>