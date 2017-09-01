<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectTransportWayCar.aspx.cs" Inherits="Admin_Dialog_SelectTransportWayCar" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script type="text/javascript">
		var DA = new Array();
		var Loded = new Array();
		window.onload = function () {
			//DA[0] : TransportBBCL;
			//DA[1] : OurBranchPk;
			DA = dialogArguments;
			Admin.TransportBBCLLoad(DA[0], DA[1], function (result) {
				if (result[0] != "N") {
					Loded = result;
					var InnerHTML = "";
					for (var i = 0; i < result.length; i++) {
						var Each = result[i].split("#@!");
						InnerHTML += "<tr><td>" + Each[1] + "</td><td>" + Each[2] + "</td><td>" + Each[3] + "</td><td>" + Each[4] + "</td><td><input type=\"button\" value=\"Select\" onclick=\"SelThis('" + i + "');\" /> </td><td><input type=\"button\" value=\"Delete\" onclick=\"DelThis('" + Each[0] + "');\" /></td></tr>";
					}
					document.getElementById("PnSavedList").innerHTML = "<table border='0' cellpadding='0' cellspacing='0' ><tr><td>" + form1.HCarName.value + "</td><td>" + form1.HDepartureRegion.value + "</td><td>" + form1.HArrivalRegion.value + "</td><td>" + form1.HDriverTEL.value + "</td><td>&nbsp;</td><td>&nbsp;</td></tr>" + InnerHTML + "</table>";
				}
			}, function (result) { alert(result); });
		}
		function BTN_Submit_Click() {
			var Value = form1.TBCarCompanyName.value + "#@!" + form1.TBDepartureRegion.value + "#@!" + form1.TBArrivalRegion.value + "#@!" + form1.TBDriverTEL.value;
			Admin.TransportBBCLInsert(DA[0], DA[1], Value, function (result) {
				alert("Success");
				window.returnValue = true;
				returnValue = result + "#@!" + Value;
				self.close();
			}, function (result) { alert(result); });
		}
		function SelThis(Value) {
			window.returnValue = true;
			returnValue = Loded[Value];
			self.close();
		}
		function DelThis(Value) {
			Admin.TransportBBCLDelete(Value, function (result) {
				if (result == "1") {
					window.returnValue = true;
					returnValue = "D";
					self.close();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
    </script>
    <link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />    
</head>
<body style="background-color:#999999; padding:10px; overflow-x:hidden; ">
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
    <div style="width:560px;   padding-left:10px; padding-right:10px;  background-color:white;">
		<fieldset style="padding:10px;">
			<legend><strong><%=GetGlobalResourceObject("qjsdur", "tlsrbemdfhr") %></strong></legend>
			<div><%=GetGlobalResourceObject("qjsdur", "ckfidghltkaud") %> : <input type="text" id="TBCarCompanyName" /></div>
			<div><%=GetGlobalResourceObject("qjsdur", "cnfqkfwl") %> : <input type="text" id="TBDepartureRegion" /></div>
			<div><%=GetGlobalResourceObject("qjsdur", "ehckrwl") %> : <input type="text" id="TBArrivalRegion" /></div>
			<div><%=GetGlobalResourceObject("qjsdur", "ckfiddusfkrcj") %> : <input type="text" id="TBDriverTEL" /></div>
			<div><input type="button" value="Submit" onclick="BTN_Submit_Click();" /> </div>
		</fieldset>
		<div id="PnSavedList"></div>
		<input type="hidden" id="HCarName" value="<%=GetGlobalResourceObject("qjsdur", "ckfidghltkaud") %>" />
		<input type="hidden" id="HDepartureRegion" value="<%=GetGlobalResourceObject("qjsdur", "cnfqkfwl") %>" />
		<input type="hidden" id="HArrivalRegion" value="<%=GetGlobalResourceObject("qjsdur", "ehckrwl") %>" />
		<input type="hidden" id="HDriverTEL" value="<%=GetGlobalResourceObject("qjsdur", "ckfiddusfkrcj") %>" />
    </div>
    </form>
</body>
</html>