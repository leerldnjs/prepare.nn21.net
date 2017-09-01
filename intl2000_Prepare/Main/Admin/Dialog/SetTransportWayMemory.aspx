<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetTransportWayMemory.aspx.cs" Inherits="Admin_Dialog_SetTransportWayMemory" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
    	var DA = new Array();
    	var InsertValue = new Array();
    	var TitleNo;
    	var Check = "1";
    	window.onload = function () {
    		//DA[0] : TransportBBCL;
    		//DA[1] : OurBranchPk;

    		DA = dialogArguments;

    		switch (DA[0]) {
    			case "1":
    				InsertValue[0] = form1.gkdrhdtk.value;
    				InsertValue[1] = form1.gkdrhdaud.value;
    				InsertValue[2] = form1.cnfqkfrhdgkd.value;
    				InsertValue[3] = form1.ehckrwrhdgkd.value;
    				InsertValue[4] = form1.cnfqkfgkdrhdtkdusfkrcj.value;
    				InsertValue[5] = form1.ehckrgkdrhdtkdusfkrcj.value;
    				TitleNo = new Array(0, 1, 2, 3);
    				break;
    			case "7": 	//예전엔 2 car
    				InsertValue[0] = form1.ckfidghltkaud.value;
    				InsertValue[1] = form1.cnfqkfwl.value;
    				InsertValue[2] = form1.ehckrwl.value;
    				InsertValue[3] = form1.rltktjdaud.value;
    				InsertValue[4] = form1.ckfidrbrur.value;
    				InsertValue[5] = form1.ckfiddusfkrcj.value;
    				TitleNo = new Array(0, 1, 2, 3);
    				break;
    			case "3":
    				InsertValue[0] = form1.tjsqkrghltkaud.value;
    				InsertValue[1] = form1.tjsqkraud.value;
    				InsertValue[2] = form1.tjswjrgkd.value;
    				InsertValue[3] = form1.ehckrgkd.value
    				TitleNo = new Array(0, 1, 2, 3);
    				break;
    			case "4":
    				InsertValue[0] = "Title";
    				InsertValue[1] = form1.tjswjrgkd.value;
    				InsertValue[2] = form1.ehckrgkd.value;
    				InsertValue[3] = "Departure Staff";
    				InsertValue[4] = "Arrival Staff";
    				TitleNo = new Array(0, 1, 2);
    				break;
    			case "5":
    				InsertValue[0] = "Title | Memo";
    				InsertValue[1] = form1.tjsqkrghltkaud.value;
    				InsertValue[2] = form1.tjsqkraud.value;
    				InsertValue[3] = form1.tjswjrgkd.value;
    				InsertValue[4] = form1.ehckrgkd.value;
    				TitleNo = new Array(0, 1, 2, 3, 4);
    				break;
    			case "6":
    				InsertValue[0] = "Agency Name";
    				InsertValue[1] = form1.tjswjrgkd.value;
    				InsertValue[2] = form1.ehckrgkd.value;
    				InsertValue[3] = "Departure Staff";
    				InsertValue[4] = "Arrival Staff";
    				TitleNo = new Array(0, 1, 2, 3, 4);
    				break;
    		}

    		var temp = "";
    		for (var i = 0; i < InsertValue.length; i++) {
    			
    					temp += "<div>" + InsertValue[i] + " : <input type=\"text\" id=\"TBInsert[" + i + "]\"  /></div>";
    			
    		}
    		document.getElementById("PnInsert").innerHTML = "<fieldset style=\"padding:10px;\">" +
    								"<legend><strong>" + form1.tlsrbemdfhr.value + "</strong></legend>" + temp +
    								"<div><input type=\"button\" value=\"Submit\" onclick=\"BTN_Submit_Click();\" /> </div>" +
    								"</fieldset>";
    		Admin.TransportBBCLLoad(DA[0], DA[1], function (result) {
    			if (result[0].substr(0, 1) != "N") {
    				var HTML = "";
    				for (var i = 0; i < result.length; i++) {
    					var Each = result[i].split("#@!");
    					var InnerTR = "";
    					for (var j = 0; j < TitleNo.length; j++) {
    						InnerTR += "<td class=\"TBody1\" >" + Each[TitleNo[j] + 1] + "</td>";
    					}
    					HTML += "<tr>" + InnerTR + "<td class=\"TBody1\" ><input type=\"hidden\" id=\"HSum[" + i + "]\" value=\"" + result[i] + "\" />" +
							"<input type=\"button\" value=\"S\" ID=\"BTNSel[" + i + "]\" onclick=\"SelThis('" + i + "');\" />" +
							"<input type=\"button\" value=\"D\" style=\"color:red;\" ID=\"BTNDel[" + i + "]\" onclick=\"DelThis('" + i + "' ,'" + Each[0] + "');\" /></td></tr>";
    				}
    				var TempTitle = "";
    				for (var i = 0; i < TitleNo.length; i++) {
    					TempTitle += "<td class=\"THead1\" >" + InsertValue[TitleNo[i]] + "</td>";
    				}
    				document.getElementById("PnSavedList").innerHTML = "<table border='0' cellpadding='0' cellspacing='0' style=\"width:560px; \" ><tr style=\"height:20px; \">" +
						TempTitle + "<td class=\"THead1\"  >&nbsp;</td></tr>" + HTML + "</table>";
    			}
    		}, function (result) { alert(result); });

    	}
    	function OnlyEnglishNum(obj) {
    		if (document.getElementById(obj).value.length > 0) {
    			regA1 = new RegExp(/^[0-9a-zA-Z,.-\/\ ]+$/);
    			if (!regA1.test(document.getElementById(obj).value)) {
    				alert("영문과 숫자만 입력가능합니다.");
    				document.getElementById(obj).select();
    				document.getElementById(obj).focus();
    				Check = "0";
    			}
    		}
    	}
    	function BTN_Submit_Click() {
    		Check = "1";
    		for (var i = 0; i < InsertValue.length; i++) {
    			if (DA[0] == "7") {
    			
    			} else {
    				if (DA[0] == "1" && i == 4 || i == 5) {
    			
    				}
    				else if (DA[0] == "4" && i == 4 || i == 5) {
    			
    				}
    				else if (DA[0] == "6" && i == 3 || i == 4) {
    			
    				} else {
    					OnlyEnglishNum("TBInsert["+i+"]");
    				}
    			}
    		}

    		if (Check == "0") {
    			return false;
    		}

    		var Value = "";
    		for (var i = 0; i < InsertValue.length; i++) {
    			if (i == 0) {
    				Value += document.getElementById("TBInsert[" + i + "]").value;
    			}
    			else {
    				Value += "#@!" + document.getElementById("TBInsert[" + i + "]").value;
    			}
    		}
    		
			Admin.TransportBBCLInsert(DA[0], DA[1], Value, function (result) {
    			alert("Success");
    			window.returnValue = true;
    			returnValue = result + "#@!" + Value;
    			self.close();
    		}, function (result) { alert(result); });
    	}
    	function SelThis(Value) {
    		window.returnValue = true;
    		returnValue = document.getElementById("HSum[" + Value + "]").value;
    		self.close();
    	}
    	function DelThis(Count, Pk) {
    		if (confirm("Will be Deleted. Right?")) {
    			Admin.TransportBBCLDelete(Pk, function (result) {
    				if (result == "1") {
    					document.getElementById("BTNSel[" + Count + "]").disabled = "disabled";
    					document.getElementById("BTNDel[" + Count + "]").disabled = "disabled";
    				}
    			}, function (result) { alert("ERROR : " + result); });
    		}
    	}

    </script>
    <link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />    
</head>
<body>
    <form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
		<div style="width:560px;   padding-left:10px; padding-right:10px;  background-color:white;">
			<div id="PnInsert"></div>
			<div id="PnSavedList"></div>
			<input type="hidden" id="gkdrhdtk" value="<%=GetGlobalResourceObject("qjsdur", "gkdrhdtk") %>" />
			<input type="hidden" id="gkdrhdaud" value="<%=GetGlobalResourceObject("qjsdur", "gkdrhdaud") %>" />
			<input type="hidden" id="cnfqkfrhdgkd" value="<%=GetGlobalResourceObject("qjsdur", "cnfqkfrhdgkd") %>" />
			<input type="hidden" id="ehckrwrhdgkd" value="<%=GetGlobalResourceObject("qjsdur", "ehckrwrhdgkd") %>" />
			
			<input type="hidden" id="cnfqkfwl" value="<%=GetGlobalResourceObject("qjsdur", "cnfqkfwl") %>"  />
			<input type="hidden" id="zjsxpdlsjrbrur" value="<%=GetGlobalResourceObject("qjsdur", "zjsxpdlsjrbrur") %>" />
			<input type="hidden" id="zjsxpdlsjqjsgh" value="<%=GetGlobalResourceObject("qjsdur", "zjsxpdlsjqjsgh") %>" />
			<input type="hidden" id="ckfiddusfkrcj" value="<%=GetGlobalResourceObject("qjsdur", "ckfiddusfkrcj") %>" />
			<input type="hidden" id="ckfidghltkaud" value="<%=GetGlobalResourceObject("qjsdur", "ckfidghltkaud") %>" />
			<input type="hidden" id="cnfqkfdPwjddlf" value="<%=GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") %>" />
			<input type="hidden" id="ehckrdPwjddlf" value="<%=GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") %>" />
			<input type="hidden" id="cnfqkfgkdrhdtkdusfkrcj" value="<%=GetGlobalResourceObject("qjsdur", "cnfqkfgkdrhdtkdusfkrcj") %>" />
			<input type="hidden" id="ehckrgkdrhdtkdusfkrcj" value="<%=GetGlobalResourceObject("qjsdur", "ehckrgkdrhdtkdusfkrcj") %>" />
			<input type="hidden" id="ghkanfthdwkdqjsgh" value="<%=GetGlobalResourceObject("qjsdur", "ghkanfthdwkdqjsgh") %>" />
			<input type="hidden" id="ehckrwl" value="<%=GetGlobalResourceObject("qjsdur", "ehckrwl") %>" />
			<input type="hidden" id="ckfidqjsgh" value="<%=GetGlobalResourceObject("qjsdur", "ckfidqjsgh") %>" />
			<input type="hidden" id="ckfidrbrur" value="<%=GetGlobalResourceObject("qjsdur", "ckfidrbrur") %>" />
			<input type="hidden" id="rltktjdaud" value="<%=GetGlobalResourceObject("qjsdur", "rltktjdaud") %>" />
			<input type="hidden" id="tjsqkrghltkaud" value="<%=GetGlobalResourceObject("qjsdur", "tjsqkrghltkaud") %>" />
			<input type="hidden" id="tjswjrgkd" value="<%=GetGlobalResourceObject("qjsdur", "tjswjrgkd") %>" />
			<input type="hidden" id="ehckrgkd" value="<%=GetGlobalResourceObject("qjsdur", "ehckrgkd") %>" />
			<input type="hidden" id="tjsqkraud" value="<%=GetGlobalResourceObject("qjsdur", "tjsqkraud") %>" />
			<input type="hidden" id="tncnfwktkdgh" value="<%=GetGlobalResourceObject("qjsdur", "tncnfwktkdgh") %>" />
			<input type="hidden" id="gkdck" value="<%=GetGlobalResourceObject("qjsdur", "gkdck") %>" />
			<input type="hidden" id="Tlfqjsgh" value="<%=GetGlobalResourceObject("qjsdur", "Tlfqjsgh") %>" />
			<input type="hidden" id="cnfqkfdPwjddlfdmsvlftntkgkddlqslek" value="<%=GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlfdmsvlftntkgkddlqslek") %>" />
			<input type="hidden" id="dnsthdqkdqjqdmftjsxorgowntpdy" value="<%=GetGlobalResourceObject("qjsdur", "dnsthdqkdqjqdmftjsxorgowntpdy") %>" />
			<input type="hidden" id="dhksfy" value="<%=GetGlobalResourceObject("qjsdur", "dhksfy") %>" />
			<input type="hidden" id="tlsrbemdfhr" value="<%=GetGlobalResourceObject("qjsdur", "tlsrbemdfhr") %>" />
			<input type="hidden" id="ghkrdls" value="<%=GetGlobalResourceObject("qjsdur", "ghkrdls") %>" />
		</div>
    </form>
</body>
</html>