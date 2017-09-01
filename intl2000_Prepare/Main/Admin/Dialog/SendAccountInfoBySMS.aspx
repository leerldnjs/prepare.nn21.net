<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendAccountInfoBySMS.aspx.cs" Inherits="Admin_Dialog_SendAccountInfoBySMS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script type="text/javascript">
		var AccountPk;
		window.onload = function () {
			form1.TB_CompanyName.value = dialogArguments[0];
			form1.TB_Name.value = dialogArguments[1];
			form1.TB_Mobile.value = dialogArguments[2];
			form1.TB_ID.value = dialogArguments[3];
			AccountPk = dialogArguments[4];
		}
		function BTN_Submit() {
			Admin.SendAccountInfoBySMS(form1.TB_CompanyName.value, form1.TB_Name.value, form1.TB_Mobile.value, form1.TB_ID.value, function (result) {
				if (result == "1") {
					alert("Success");
					this.close();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
	</script>
</head>
<body>
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
    <div style="padding:10px;">
		<p>CompanyName : <input type="text" id="TB_CompanyName" readonly="readonly" /></p>
		<p>Name : <input type="text" id="TB_Name" readonly="readonly" /></p>
		<p>ID : <input type="text" id="TB_ID" readonly="readonly" /></p>
		<p>Mobile : <input type="text" id="TB_Mobile" /></p>
		<input type="button" value="Send SMS" onclick="BTN_Submit();" />
    </div>
    </form>
</body>
</html>