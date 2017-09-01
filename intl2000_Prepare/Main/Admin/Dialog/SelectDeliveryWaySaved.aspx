<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectDeliveryWaySaved.aspx.cs" Inherits="Admin_Dialog_SelectDeliveryWaySaved" %>
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
						if (Each[0] == "") { Each[0] = "&nbsp;" }
						if (Each[1] == "") { Each[1] = "&nbsp;" }
						if (Each[2] == "") { Each[2] = "&nbsp;" }
						if (Each[3] == "") { Each[3] = "&nbsp;" }
						if (Each[4] == "") { Each[4] = "&nbsp;" }
						if (Each[5] == "") { Each[5] = "&nbsp;" }
						if (Each[6] == "") { Each[6] = "&nbsp;" }

						InnerHTML += "<tr><td>" + Each[1] + "</td><td>" + Each[2] + "</td><td>" + Each[3] + "</td><td>" + Each[4] + "</td><td>" + Each[5] + "</td><td>" + Each[6] + "</td><td><input type=\"button\" value=\"Select\" onclick=\"SelThis('" + i + "');\" /> </td><td><input type=\"button\" value=\"Delete\" id='BTN_DELETE" + Each[0] + "' onclick=\"DelThis('" + Each[0] + "');\" /></td></tr>";
					}
					document.getElementById("PnSavedList").innerHTML = "<table border='1' cellpadding='0' cellspacing='0'  style=\"width:730px; \"><tr>" +
					"<td style=\"width:100px; \">Type</td>" +
					"<td style=\"width:100px; \">Title</td>" +
					"<td style=\"width:120px;\">TEL</td>" +
					"<td style=\"width:90px; \">Driver Name</td>" +
					"<td style=\"width:120px; \">Driver Mobile</td>" +
					"<td>&nbsp;</td>" +
					"<td style=\"width:70px; \">&nbsp;</td>" +
					"<td style=\"width:70px; \">&nbsp;</td>" +
					"</tr>" + InnerHTML + "</table>";
				}
			}, function (result) { alert(result); });
		}
		function BTN_Submit_Click() {
			var Value = form1.Delivery_Type.value + "#@!" + form1.Delivery_Title.value + "#@!" + form1.Delivery_TEL.value + "#@!" + form1.Delivery_DriverName.value + "#@!" + form1.Delivery_DriverMobile.value + "#@!" + form1.Delivery_CarSize.value;
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
					document.getElementById("BTN_DELETE" + Value).disabled = "disabled";
				}
			}, function (result) { alert("ERROR : " + result); });
		}
    </script>
	<link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />    
</head>
<body style="background-color:#999999; padding:10px; overflow-x:hidden; ">
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
    <div style="width:740px;   padding-left:10px; padding-right:10px;  background-color:white;">
		<div id="PnSavedList"></div>
		<fieldset style="padding:10px;">
			<legend><strong>Add Delivery</strong></legend>
			<p>TYPE : <input type="text" id="Delivery_Type" /></p>
			<p>TITLE : <input type="text" id="Delivery_Title" /></p>
			<p>TEL : <input type="text" id="Delivery_TEL" /></p>
			<p>Driver Name : <input type="text" id="Delivery_DriverName" /></p>
			<p>Driver Mobile : <input type="text" id="Delivery_DriverMobile" /></p>
			<p>Car Size : <input type="text" id="Delivery_CarSize" /></p>
			<div><input type="button" value="Submit" onclick="BTN_Submit_Click();" /> </div>
		</fieldset>
		<input type="hidden" id="HCarName" value="<%=GetGlobalResourceObject("qjsdur", "ckfidghltkaud") %>" />
		<input type="hidden" id="HDepartureRegion" value="<%=GetGlobalResourceObject("qjsdur", "cnfqkfwl") %>" />
		<input type="hidden" id="HArrivalRegion" value="<%=GetGlobalResourceObject("qjsdur", "ehckrwl") %>" />
		<input type="hidden" id="HDriverTEL" value="<%=GetGlobalResourceObject("qjsdur", "ckfiddusfkrcj") %>" />
    </div>
    </form>
</body>
</html>