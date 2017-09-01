<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectTransportWayAir.aspx.cs" Inherits="Admin_Dialog_SelectTransportWayAir" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var DA= new Array();
        window.onload = function () {
        	//DA[0] : TransportBBCL;
        	//DA[1] : OurBranchPk;
        	DA = dialogArguments;
        	Admin.TransportBBCLLoad(DA[0], DA[1], function (result) {
        		if (result[0].substr(0, 1) != "N") {
        			var HTML = "";
        			for (var i = 0; i < result.length; i++) {
        				var Each = result[i].split("#@!");
        				HTML += "<tr><td>" + Each[1] + "</td><td>" + Each[2] + "</td><td>" + Each[3] + "</td><td>" + Each[4] + "</td><td><input type=\"button\" value=\"Select\" onclick=\"SelThis('" + result[i] + "');\" /> </td></tr>";
        			}
        			document.getElementById("PnSavedList").innerHTML = "<table border='1' cellpadding='0' cellspacing='0' ><tr><td>" + form1.gkdrhdtk.value + "</td><td>" + form1.gkdrhdaud.value + "</td><td>" + form1.cnfqkfrhdgkd.value + "</td><td>" + form1.ehckrwrhdgkd.value + "</td><td>&nbsp;</td></tr>" + HTML + "</table>";
        		}
        	}, function (result) { alert(result); });
        }
        function BTN_Submit_Click() {
        	var Value = form1.TBAirCompanyName.value + "#@!" + form1.TBAirName.value + "#@!" + form1.TBDepartureRegion.value + "#@!" + form1.TBArrivalRegion.value + "#@!" + form1.TBDepartureTEL.value + "#@!" + form1.TBArrivalTEL.value;
        	Admin.TransportBBCLInsert(DA[0], DA[1], Value, function (result) {
        		alert("Success");
        		window.returnValue = true;
        		returnValue = result + "#@!" + Value;
        		self.close();
        	}, function (result) { alert(result); });
        }
        function SelThis(Value) {
            window.returnValue = true;
            returnValue = Value;
            self.close();
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
            <div><%=GetGlobalResourceObject("qjsdur", "gkdrhdtk") %> : <input type="text" id="TBAirCompanyName" /></div>
            <div><%=GetGlobalResourceObject("qjsdur", "gkdrhdaud") %> : <input type="text" id="TBAirName" /></div>
			<div><%=GetGlobalResourceObject("qjsdur", "cnfqkfrhdgkd") %> : <input type="text" id="TBDepartureRegion" /></div>
			<div><%=GetGlobalResourceObject("qjsdur", "ehckrwrhdgkd")%> : <input type="text" id="TBArrivalRegion" /></div>
            <div><%=GetGlobalResourceObject("qjsdur", "cnfqkfgkdrhdtkdusfkrcj")%> : <input type="text" id="TBDepartureTEL" /></div>
            <div><%=GetGlobalResourceObject("qjsdur", "ehckrgkdrhdtkdusfkrcj")%> : <input type="text" id="TBArrivalTEL" /></div>
            <div><input type="button" value="<%=GetGlobalResourceObject("qjsdur", "ghkrdls") %>" onclick="BTN_Submit_Click();" /> </div>
        </fieldset>
        <div id="PnSavedList"></div>
		<input type="hidden" id="gkdrhdtk" value="<%=GetGlobalResourceObject("qjsdur", "gkdrhdtk") %>" />
		<input type="hidden" id="gkdrhdaud" value="<%=GetGlobalResourceObject("qjsdur", "gkdrhdaud") %>" />
		<input type="hidden" id="cnfqkfrhdgkd" value="<%=GetGlobalResourceObject("qjsdur", "cnfqkfrhdgkd") %>" />
		<input type="hidden" id="ehckrwrhdgkd" value="<%=GetGlobalResourceObject("qjsdur", "ehckrwrhdgkd") %>" />
    </div>
    </form>
</body>
</html>