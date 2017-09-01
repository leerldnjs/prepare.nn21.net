<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectTransportWayShipping.aspx.cs" Inherits="Admin_Dialog_SelectTransportWayShipping" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
    	var DA;
    	var Loded = new Array();
    	window.onload = function () {
    		DA = dialogArguments;
    		Admin.TransportBBCLLoad(DA[0], DA[1], function (result) {
    			if (result[0] != "N") {
    				Loded = result;
    				var InnerHTML = "";
    				for (var i = 0; i < result.length; i++) {
    					var Each = result[i].split("#@!");
    					InnerHTML += "<tr >" +
							"<td rowspan=\"2\" class=\"TBody1\" >&nbsp;" + Each[1] + "</td>" +
							"<td rowspan=\"2\" class=\"TBody1\" >&nbsp;" + Each[2] + "</td>" +
							"<td rowspan=\"2\" class=\"TBody1\" >&nbsp;" + Each[3] + "</td>" +
							"<td style=\"text-align:center;\">" + Each[4] + " ~</td>" +
							"<td><input type=\"button\" value=\"Select\" onclick=\"SelThis('" + i + "');\" /></td></tr>" +
							"<tr><td class=\"TBody1\" >&nbsp;" + Each[5] + "</td><td class=\"TBody1\"><input type=\"button\" value=\"Delete\" style=\"color:red;\" onclick=\"DelThis('" + Each[0] + "');\" /> </td></tr>";
    				}
    				document.getElementById("PnSavedList").innerHTML = "<table border='0' cellpadding='0' cellspacing='0' ><tr>" +
						"<td class='THead1' style=\"width:200px;\" >" + form1.tjsqkrghltkaud.value +
						"</td><td class='THead1' style=\"width:200px;\"  >" + form1.tjsqkraud.value +
						"</td><td class='THead1' style=\"width:200px;\"  >" + form1.tncnfwktkdgh.value +
						"</td><td class='THead1' style=\"width:200px;\"  >" + form1.tjswjrgkd.value + "~" + form1.ehckrgkd.value +
						"</td><td class='THead1' >&nbsp;" +
						"</td></tr>" + InnerHTML + "</table>";
    			}
    		}, function (result) { alert(result); });
    	}
    	function BTN_Submit_Click() {
    		var Value = form1.TBShipCompanyName.value + "#@!" + form1.TBShipName.value + "#@!" + form1.TBShipperName.value + "#@!" + form1.TBDepartureRegion.value + "#@!" + form1.TBArrivalRegion.value;
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
    	function DelThis(pk) {
    		if (confirm("Will be Deleted. Right?")) {
    			Admin.TransportBBCLDelete(pk, function (result) {
    				if (result == "1") {
    					alert("SUCCESS");
    					self.close();
    				}
    			}, function (result) { alert("ERROR : " + result); });
    		}
    	}
    </script>
    <link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />    
</head>
<body style="background-color:#999999; padding:5px; overflow:auto; ">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
	<div style="width:950px;   padding-left:10px; padding-right:10px;  background-color:white;">
        <fieldset style="padding:10px;">
            <legend><strong><%=GetGlobalResourceObject("qjsdur", "tlsrbemdfhr") %></strong></legend>
            <div><%=GetGlobalResourceObject("qjsdur", "tjsqkrghltkaud") %> : <input type="text" id="TBShipCompanyName" /></div>
            <div><%=GetGlobalResourceObject("qjsdur", "tjsqkraud") %> : <input type="text" id="TBShipName" /></div>
            <div><%=GetGlobalResourceObject("qjsdur", "tncnfwktkdgh") %> : <input type="text" id="TBShipperName" /></div>
            <div><%=GetGlobalResourceObject("qjsdur", "tjswjrgkd") %> : <input type="text" id="TBDepartureRegion" /></div>
            <div><%=GetGlobalResourceObject("qjsdur", "ehckrgkd") %> : <input type="text" id="TBArrivalRegion" /></div>
            <div><input type="button" value="Submit" onclick="BTN_Submit_Click();" /> </div>
        </fieldset>
        <div id="PnSavedList"></div>
		<input type="hidden" id="tjsqkrghltkaud" value="<%=GetGlobalResourceObject("qjsdur", "tjsqkrghltkaud") %>" />
		<input type="hidden" id="tjsqkraud" value="<%=GetGlobalResourceObject("qjsdur", "tjsqkraud") %>" />
		<input type="hidden" id="tncnfwktkdgh" value="<%=GetGlobalResourceObject("qjsdur", "tncnfwktkdgh") %>" />
		<input type="hidden" id="tjswjrgkd" value="<%=GetGlobalResourceObject("qjsdur", "tjswjrgkd") %>" />
		<input type="hidden" id="ehckrgkd" value="<%=GetGlobalResourceObject("qjsdur", "ehckrgkd") %>" />
    </div>
    </form>
</body>
</html>
