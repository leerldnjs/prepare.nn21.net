<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequestModifyHistory.aspx.cs" Inherits="Request_RequestModifyHistory" %>
<%@ Register Src="~/Admin/LogedWithoutRecentRequest11.ascx"  tagname="LogedWithoutRecentRequest" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		function ModifyHistoryConfirm(Pk) {
			Request.RequestModifyConfirm(Pk, function (result) {
				if (result == "1") {
					alert("SUCCESS");
					document.getElementById("BTN[" + Pk + "]").disabled = "disabled";
				}
				else {
					alert(result);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
	</script>
</head>
<body style="background-color:#E4E4E4; width:1100px; margin:0 auto; padding-top:10px;" >
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Request.asmx"/></Services></asp:ScriptManager>
	<uc1:LogedWithoutRecentRequest ID="LogedWithoutRecentRequest1" runat="server" />
    <div style="background-color:White; width:1050px; height:100%; padding:25px;">
		<p><%=HTMLTab %></p>
		<%=HTMLModifyHistory %>
    </div>
    </form>
</body>
</html>
