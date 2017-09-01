<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransportShipperName.aspx.cs" Inherits="Request_Dialog_TransportShipperName" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>.</title>
	<script src="../../Common/public.js" type="text/javascript"></script>
	<link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script src="../../Common/jquery-1.10.2.min.js"></script>
	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
	<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
	<script type="text/javascript">
		var sessonid;
		window.onload = function () {
			sessonid = form1.ID.value;
			Request.LoadAdminDefault_Transport(function (result) {
				var html = "";
				if (result[0] != "0") {
					for (var i = 0; i < result.length; i++) {
						var each = result[i].split("##");
						html += "<tr >";
						html +=
								"	<td style='border-bottom:dotted 1px #93A9B8; '><strong>&nbsp;&nbsp;Name:</strong><strong>" + each[1] + "</strong><br />&nbsp;&nbsp;<strong>Addr:</strong>" + each[2] + "<br />&nbsp;&nbsp;<strong>Memo:</strong><strong>" + each[3] + "</strong>" +
								"	<td style='text-align:center; border-bottom:dotted 1px #93A9B8; width:100px;'><input type=\"button\" style='width:60px;' value=\"select\" onclick=\"InsertSuccess2('" + result[i] + "')\" />";
						if (sessonid == "ilyt0" || sessonid == "ilic30") {
							html += "	<input type=\"button\" style='width:60px; color:red;' value=\"delete\" onclick=\"Del('" + each[0] + "')\" />";
						}
						html += "</td></tr>";
					}
				}

				document.getElementById("PnClearanceHead").innerHTML = "<table style=\"width:710px;\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
				"<tr><td style='border-bottom:dotted 1px #93A9B8; '>&nbsp;</td>" +
				"<td style='border-bottom:dotted 1px #93A9B8; '></td></tr>" + html + "</table>";
			}, function (result) { alert("ERROR : " + result); });

			if (sessonid == "ilic66" || sessonid == "ilyt0" || sessonid == "ilic30") {

			}
			else {
				$("#PnClearanceBody").hide();
			}
		}
		function InsertSuccess2(result) {
			var each = result.split("##");
			Request.CompanyInDocument_CompanyNoAdd_Transport(each[0], function (result) {
				if (result != "") {
					window.returnValue = true;
					returnValue = result;
					self.close();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function InsertShipperInDocument_Transport(gubunCL, name, addr, memo) {
			if (name == "") { alert(form1.HMustCompanyName.value); return false; }
			if (addr == "") { alert(form1.HMustAddress.value); return false; }

			name = name.replace(/'/gi, "`");
			addr = addr.replace(/'/gi, "`");
			memo = memo.replace(/'/gi, "`");

			Request.InsertShipperNameInDocument_Transport(gubunCL, name, addr, memo, function (result) {
				if (result != "") {
					window.returnValue = true;
					returnValue = result;
					self.close();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function Del(target) {
			if (confirm("really delete this?")) {
				Request.DelShipperNameInDocument(target, function (result) {
					if (result == "1") {
						alert(form1.HDelComplete.value);
						window.returnValue = true;
						returnValue = 'D';
						self.close();
					}
					else { alert(form1.HError.value); }
				}, function (result) { alert("ERROR : " + result); });
			}
		}
	</script>
</head>
<body style="padding: 15px; width: 750px;">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="RequestFormService" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Request.asmx" />
			</Services>
		</asp:ScriptManager>
		<fieldset>
			<legend><strong><%=GetGlobalResourceObject("RequestForm", "Clearance") %></strong></legend>
			<div style="line-height: 20px; padding: 10px;">
				<div id="PnClearanceHead"></div>
			</div>
		</fieldset>

		<fieldset>
			<legend><strong>Insert</strong></legend>
			<div style="line-height: 20px; padding: 10px;">
				<div id="PnClearanceBody">
					<table style="width: 710px;" border="0" cellpadding="0" cellspacing="0">
						<tr>
							<td>&nbsp;&nbsp;Name : &nbsp;&nbsp;&nbsp;&nbsp;<input id="Name" style="width: 450px; text-align: left;" type="text" /><br />
								&nbsp;&nbsp;Address : &nbsp;<input id="Address" style="width: 450px; text-align: left;" type="text" /><br />
								&nbsp;&nbsp;Memo: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input id="Memo" style="width: 450px; text-align: left;" type="text" /></td>
							<td>
								<input style="width: 70px;" onclick="InsertShipperInDocument_Transport('8', form1.Name.value, form1.Address.value, form1.Memo.value);" type="button" value="insert" /></td>
						</tr>
					</table>
				</div>
			</div>
		</fieldset>
		<input type="hidden" id="ID" value="<%=Request.Params["ID"] %>" />
		<input type="hidden" id="HDelComplete" value="<%=GetGlobalResourceObject("Alert", "DeleteComplete") %>" />
	</form>
</body>
</html>
