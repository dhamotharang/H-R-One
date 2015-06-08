<%@ control language="C#" autoeventwireup="true" inherits="WebDatePicker, HROneESS.deploy" %>
<asp:TextBox ID="DateTextBox" runat="server" Columns="10" MaxLength="10" />
<img height="15" src="calendar/calendar.gif" alt="calendar" width="16" border="0" onclick="popUpCalendar(this,document.getElementById('<%=DateTextBox.ClientID%>'),'<%=DateFormatString%>');" />
<span style="font-size: 8pt;">
<asp:Label ID="DateFormatLabel" runat="server"  />
</span>
