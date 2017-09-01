<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Warehouse_Logistics.aspx.cs" Inherits="Logistics_Warehouse_Logistics" %>
<%@ Register Src="~/Logistics/Loged.ascx" TagName="Loged" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Company Info</title>
	<style type="text/css">
		.tdSubT { border-bottom:2px solid #93A9B8; padding-top:30px; padding-bottom:3px; }
		.td01{background-color:#f5f5f5; text-align:center; height:25px; padding-top:4px; padding-bottom:4px; border-bottom:1px dotted #E8E8E8;}
		.td02{width:250px; padding-top:4px;padding-bottom:4px; padding-left:10px; border-bottom:1px dotted #E8E8E8; background-color:White;	}
		.td023{padding-top:4px;padding-bottom:4px; padding-left:10px;border-bottom:1px dotted #E8E8E8;background-color:White;}
		.tdStaffBody{text-align:center; padding:5px; border:dotted 1px #E8E8E8; background-color:White;}
    </style>
    <link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script src="../Common/RegionCode.js?version=20131029" type="text/javascript"></script>
	<script type="text/javascript">
		
		var COMPANYPK;
		var PRESIDENTNAME;
		var PRESIDENTEMAIL;
		var RESPONSIBLESTAFF;
		var STAFFSUM = new Array();
		var WAREHOUSESUM = new Array();
		window.onload = function () {
			var Url = location.href;
			var GetValue = Url.substring(Url.indexOf("?") + 1);
			var EachValue = GetValue.split("&");
			var tempcount;
			
			if (EachValue[0].substr(0, 8) == "Language") { tempcount = 1; }
			else { tempcount = 0; }
			if (EachValue[tempcount] == "M=View") {
				COMPANYPK = EachValue[(tempcount + 1)].toString().substr(2);
				Admin.LoadCompanyInfo(COMPANYPK, function (result) {
					if (result == "N") { return false; }
					var EachGroup = result.split("%!$@#");
					var Company = EachGroup[0].split("#@!");
					
					var StaffRowCount = 0;
					var WarehouseCount = 0;
					for (var i = 1; i < EachGroup.length; i++) {
						var Each = EachGroup[i].split("#@!");
						switch (Each[0]) {
							case "A":
								if (Each[2] + "" != "") { InsertStaff('Company'); }
								else { InsertStaff(''); }
								STAFFSUM[StaffRowCount] = EachGroup[i].substr(4);
								document.getElementById("TBStaffGubun[" + StaffRowCount + "]").value = "A";
								document.getElementById("TBStaffPk[" + StaffRowCount + "]").value = Each[1];
								document.getElementById("TBStaffDuties[" + StaffRowCount + "]").value = Each[3];
								document.getElementById("TBStaffName[" + StaffRowCount + "]").value = Each[4];
								document.getElementById("TBStaffTEL[" + StaffRowCount + "]").value = Each[5];
								document.getElementById("TBStaffEmail[" + StaffRowCount + "]").value = Each[7];
								if (Each[2] + "" != "") {
									document.getElementById("TBStaffGubun[" + StaffRowCount + "]").value = "C";
									document.getElementById("TBStaffID[" + StaffRowCount + "]").value = Each[2];
									document.getElementById("TBStaffDuties[" + StaffRowCount + "]").disabled = "disabled";
									document.getElementById("TBStaffName[" + StaffRowCount + "]").disabled = "disabled";
								}
								document.getElementById("TBStaffMobile0[" + StaffRowCount + "]").value = Each[6];
								document.getElementById("IsEmailSMS[" + StaffRowCount + "]").value = Each[8];
								document.getElementById("TBStaffMemo[" + StaffRowCount + "]").value = Each[9] + "";
								StaffRowCount++;
								break;
							case "W":
								WAREHOUSESUM[WarehouseCount] = new Array();
								WAREHOUSESUM[WarehouseCount][0] = Each[1];
								WAREHOUSESUM[WarehouseCount][1] = Each[2];
								WAREHOUSESUM[WarehouseCount][2] = Each[3];
								WAREHOUSESUM[WarehouseCount][3] = Each[4];
								WAREHOUSESUM[WarehouseCount][4] = Each[5];
								WAREHOUSESUM[WarehouseCount][5] = Each[6];
								WAREHOUSESUM[WarehouseCount][6] = Each[7];
								InsertWarehouse();
								document.getElementById("TBWarehouse[" + WarehouseCount + "]Pk").value = Each[1];
								document.getElementById("TBWarehouse[" + WarehouseCount + "][0]").value = Each[2];
								document.getElementById("TBWarehouse[" + WarehouseCount + "][1]").value = Each[3];
								document.getElementById("TBWarehouse[" + WarehouseCount + "][2]").value = Each[4];
								document.getElementById("TBWarehouse[" + WarehouseCount + "][3]").value = Each[5];
								document.getElementById("TBWarehouse[" + WarehouseCount + "][4]").value = Each[6];
								document.getElementById("TBWarehouse[" + WarehouseCount + "][5]").value = Each[7];
								WarehouseCount++;
								break;
							
						}
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function InsertWarehouse() {		// 상품추가
			var objTable = document.getElementById("TabWarehouse");
			objTable.appendChild(document.createElement("TBODY"));
			var lastRow = objTable.rows.length;
			var thisLineNo = lastRow - 2;
			var objRow1 = objTable.insertRow(lastRow);
			var objCell1 = objRow1.insertCell(); var objCell2 = objRow1.insertCell(); var objCell3 = objRow1.insertCell(); var objCell4 = objRow1.insertCell(); var objCell5 = objRow1.insertCell(); var objCell6 = objRow1.insertCell();
			objCell1.align = "center"; objCell2.align = "center"; objCell3.align = "center"; objCell4.align = "center"; objCell5.align = "center"; objCell6.align = "center";
			objCell1.innerHTML = "<input type=\"hidden\" id=\"TBWarehouse[" + thisLineNo + "]Pk\" /><input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][0]\" style=\"width:90px;\" />";
			objCell2.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][1]\" style=\"width:315px;\" />";
			objCell3.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][2]\" style=\"width:90px;\" />";
			objCell4.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][3]\" style=\"width:60px;\" />";
			objCell5.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][4]\" style=\"width:110px;\" />";
			objCell6.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][5]\" style=\"width:100px;\" />&nbsp;"+
												"<span onclick=\"DELETEWAREHOUSE('" + thisLineNo + "');\" style=\"color:Red; cursor:hand;\" >X</span>";
		}
		
	    function InsertStaff(gubun) {
	    	var objTable = document.getElementById("TabStaff");
	    	objTable.appendChild(document.createElement("TBODY"));
	    	var lastRow = objTable.rows.length;
	    	var thisLineNo = lastRow - 2;
	    	var objRow1 = objTable.insertRow(lastRow);
	    	var objCell1 = objRow1.insertCell();
	    	var objCell2 = objRow1.insertCell();
	    	var objCell3 = objRow1.insertCell();
	    	var objCell4 = objRow1.insertCell();
	    	var objCell5 = objRow1.insertCell();
	    	var objCell6 = objRow1.insertCell();
	    	var objCell7 = objRow1.insertCell();

	    	objCell1.align = "center";
	    	objCell2.align = "center";
	    	objCell3.align = "center";
	    	objCell4.align = "center";
	    	objCell5.align = "center";
	    	objCell6.align = "center";
	    	objCell7.align = "left";

	    	var rightIcon = "";

	    	objCell1.innerHTML = "<input type=\"hidden\" id=\"TBStaffGubun[" + thisLineNo + "]\" value=\"N\" /><input type=\"hidden\" id=\"TBStaffPk[" + thisLineNo + "]\" /><input type=\"hidden\" id=\"TBStaffID[" + thisLineNo + "]\" /><input type=\"text\" id=\"TBStaffDuties[" + thisLineNo + "]\" style=\"width:60px; text-align:center;\" />";
	    	objCell2.innerHTML = "<input type=\"text\" id=\"TBStaffName[" + thisLineNo + "]\" style=\"width:60px;\" />";
	    	objCell3.innerHTML = "<input type=\"text\" id=\"TBStaffTEL[" + thisLineNo + "]\" style=\"width:90px;\" />";
			objCell4.innerHTML = "<select id=\"IsEmailSMS[" + thisLineNo + "]\" ><option value=\"3\">ALL</option><option value=\"2\">Email</option><option value=\"1\">SMS</option><option value=\"0\">X</option> </select>";
	    	objCell5.innerHTML = "<input type=\"text\" id=\"TBStaffEmail[" + thisLineNo + "]\" style=\"width:145px;\" />";
	    	objCell6.innerHTML = "<input type=\"text\" id=\"TBStaffMobile0[" + thisLineNo + "]\" style=\"width:105px;\" />";
	    	objCell7.innerHTML = "<input type=\"text\" id=\"TBStaffMemo[" + thisLineNo + "]\" style=\"width:200px;\" /> "+rightIcon;
	    }
		function OnlyNum(obj) {
	    	val = obj.value;
	    	re = /[^0-9]/gi;
	    	if (re.test(val)) {
	    		alert("숫자만 입력가능합니다");
	    		obj.select();
	    		obj.focus();
	    	}
	    }
	    function moveNext(from, to, length) {
	    	if (from.value.length == length) {
	    		document.getElementById(to).select();
	    		document.getElementById(to).focus();
	    	}
	    }
	    function ONFAILED(result) { window.alert(result); }
	    
	    function onlyEmail(obj) {
	    	if (obj.value.length > 0) {
	    		re = /^[0-9a-zA-Z]([-_\.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_\.]?[0-9a-zA-Z])*.[a-zA-Z]{2,3}$/i;
	    		if (obj.value.length < 6 || !re.test(obj.value)) { alert("메일형식이 맞지 않습니다.\n"); obj.select(); obj.focus(); return false; }
	    	}
	    }

	    function CompanyModify() {
	    	var TOstaffsum = "";
	    	var TOwarehousesum = "";

	    	var StaffRow = document.getElementById('TabStaff').rows.length - 2;
	    	for (var i = 0; i < StaffRow; i++) {
	    	
	    		if (STAFFSUM[i] != document.getElementById("TBStaffPk[" + i+ "]").value + "#@!" +
	    				document.getElementById("TBStaffID[" + i + "]").value + "#@!" +
	    				document.getElementById("TBStaffDuties[" + i + "]").value + "#@!" +
	    				document.getElementById("TBStaffName[" + i + "]").value + "#@!" +
	    				document.getElementById("TBStaffTEL[" + i + "]").value + "#@!" +
	    				document.getElementById("TBStaffMobile0[" + i + "]").value + "#@!" +
	    				document.getElementById("TBStaffEmail[" + i + "]").value + "#@!" +
	    				document.getElementById("IsEmailSMS[" + i + "]").value + "#@!" +
	    				document.getElementById("TBStaffMemo[" + i + "]").value) {
	    			if (document.getElementById("TBStaffDuties[" + i + "]").value == "" && document.getElementById("TBStaffName[" + i + "]").value == "") { }
	    			else {
	    				
	    				TOstaffsum += document.getElementById("TBStaffPk[" + i + "]").value + "#@!" +
	    										document.getElementById("TBStaffDuties[" + i + "]").value + "#@!" +
	    										document.getElementById("TBStaffName[" + i + "]").value + "#@!" +
	    										document.getElementById("TBStaffTEL[" + i + "]").value + "#@!" +
	    										document.getElementById("TBStaffMobile0[" + i + "]").value + "#@!" +
	    										document.getElementById("TBStaffEmail[" + i + "]").value + "#@!" +
	    										document.getElementById("IsEmailSMS[" + i + "]").value + "#@!" +
	    										document.getElementById("TBStaffMemo[" + i + "]").value + "%!$@#";
	    			}
	    		}
	    	}
	    	var WarehouseRow = document.getElementById("TabWarehouse").rows.length-2;
	    	for (var i = 0; i < WarehouseRow; i++) {
	    		if (WAREHOUSESUM.length > i) {
	    			if (WAREHOUSESUM[i][1] != document.getElementById("TBWarehouse[" + i + "][0]").value ||
						WAREHOUSESUM[i][2] != document.getElementById("TBWarehouse[" + i + "][1]").value ||
						WAREHOUSESUM[i][3] != document.getElementById("TBWarehouse[" + i + "][2]").value ||
						WAREHOUSESUM[i][4] != document.getElementById("TBWarehouse[" + i + "][3]").value ||
						WAREHOUSESUM[i][5] != document.getElementById("TBWarehouse[" + i + "][4]").value ||
						WAREHOUSESUM[i][6] != document.getElementById("TBWarehouse[" + i + "][5]").value) {
	    				TOwarehousesum += document.getElementById("TBWarehouse[" + i + "]Pk").value + "@@" +
													document.getElementById("TBWarehouse[" + i + "][0]").value + "@@" +
	    											 document.getElementById("TBWarehouse[" + i + "][1]").value + "@@" +
	    											 document.getElementById("TBWarehouse[" + i + "][2]").value + "@@" +
													 document.getElementById("TBWarehouse[" + i + "][3]").value + "@@" +
	    											 document.getElementById("TBWarehouse[" + i + "][4]").value + "@@" +
													 document.getElementById("TBWarehouse[" + i + "][5]").value + "####";
	    			}
	    		}
	    		else
				{
					if ("" != document.getElementById("TBWarehouse[" + i + "][0]").value ||
						"" != document.getElementById("TBWarehouse[" + i + "][1]").value ||
						"" != document.getElementById("TBWarehouse[" + i + "][2]").value ||
						"" != document.getElementById("TBWarehouse[" + i + "][3]").value ||
						"" != document.getElementById("TBWarehouse[" + i + "][4]").value ||
						"" != document.getElementById("TBWarehouse[" + i + "][5]").value) {
						TOwarehousesum += document.getElementById("TBWarehouse[" + i + "]Pk").value + "@@" +
													document.getElementById("TBWarehouse[" + i + "][0]").value + "@@" +
	    											 document.getElementById("TBWarehouse[" + i + "][1]").value + "@@" +
	    											 document.getElementById("TBWarehouse[" + i + "][2]").value + "@@" +
													 document.getElementById("TBWarehouse[" + i + "][3]").value + "@@" +
	    											 document.getElementById("TBWarehouse[" + i + "][4]").value + "@@" +
													 document.getElementById("TBWarehouse[" + i + "][5]").value + "####";
					}
	    		}
	    	}

	    	if (TOstaffsum == "" && TOwarehousesum == "") {
	    		alert("변경된 내역이 없습니다. ");
	    		return false;
	    	}
	    	Admin.UpdateCompanyInfo_Logistics(COMPANYPK, TOstaffsum, TOwarehousesum, function (result) {
	    		if (result == "1") { alert("수정완료"); location.reload(); }
	    		else { alert(result); }
	    	}, function (result) { alert("ERROR : " + result); });
	    }
	    
	    
	    function DELETEWAREHOUSE(rowcount) {
	    	if (confirm("this row will be delete")) {
	    		Admin.DeleteWarehouse(document.getElementById("TBWarehouse[" + rowcount + "]Pk").value, function (result) {
	    			if (result == "1") {
	    				alert("Success");
	    				location.reload();
	    			}
	    		}, function (result) { alert("ERROR : " + result); });
			}
		}
	    
	</script>
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px; ">
    <form id="form1" runat="server">
				
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
	<uc1:Loged ID="Loged1" runat="server" />
        
    <div style="background-color:White; width:850px; height:100%; padding:25px;">
	
	<table id="TabStaff" style="background-color:White;width:850px;"  border="0" cellpadding="0" cellspacing="0">
		<thead>
			<tr><td class="tdSubT" colspan="7">&nbsp;&nbsp;&nbsp;<strong>staff</strong> <input type="button" value="add" onclick="InsertStaff('');" /></td></tr>
			<tr style="height:30px;" >
				<td bgcolor="#F5F5F5" height="20" align="center" style="width:70px;" ><%=GetGlobalResourceObject("Member", "Duties") %></td>
				<td bgcolor="#F5F5F5" align="center" style="width:70px;" ><%=GetGlobalResourceObject("Member", "Name") %></td>
				<td bgcolor="#F5F5F5" align="center" style="width:120px;" >TEL</td >
				<td bgcolor="#F5F5F5" align="center" style="width:70px;" >MSG</td >
				<td bgcolor="#F5F5F5" align="center" style="width:155px;" >E-mail</td>
				<td bgcolor="#F5F5F5" align="center" style="width:125px;" >Mobile</td >
				<td bgcolor="#F5F5F5" align="center"  >Working hours / Memo</td>
			</tr>
		</thead>
		<tbody></tbody>
	</table>
	<div style="background-color:#777777; height:1px; font-size:1px; "></div>
	<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
	<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
	<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
	<div style="background-color:#EEEEEE; height:1px; font-size:1px; "></div>
	<div style="background-color:#FFFFFF; height:1px; font-size:1px; "></div>
	<table id="TabWarehouse" style="background-color:White; width:850px;" border="0" cellpadding="0" cellspacing="0">
		<thead>
			<tr><td class="tdSubT" colspan="6">&nbsp;&nbsp;&nbsp;<strong>Warehouse</strong> <input type="button" value="add" onclick="InsertWarehouse();" /></td></tr>
			<tr style="height:30px;" >
				<td bgcolor="#F5F5F5" height="20" align="center" style="width:95px;" ><%=GetGlobalResourceObject("qjsdur", "tkdgh") %></td>
				<td bgcolor="#F5F5F5" align="center" ><%=GetGlobalResourceObject("Member", "Address") %></td>
				<td bgcolor="#F5F5F5" align="center" style="width:95px;" >TEL</td>
				<td bgcolor="#F5F5F5" align="center" style="width:75px;" ><%=GetGlobalResourceObject("qjsdur", "ekaekdwkaud") %></td>
				<td bgcolor="#F5F5F5" align="center" style="width:110px;" >Mobile</td >
				<td bgcolor="#F5F5F5" align="center" style="width:125px;" >Memo</td >
			</tr>
		</thead>
		<tbody></tbody>
	</table>
	<div style="background-color:#FFFFFF; height:10px; font-size:10px; "></div>
	<div style="background-color:#777777; height:1px; font-size:1px; "></div>
	<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
	<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
	<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
	<div style="background-color:#EEEEEE; height:1px; font-size:1px; "></div>
	<div style="background-color:#FFFFFF; height:1px; font-size:1px; "></div>
	<div style="background-color:#FFFFFF; padding-top:20px; padding-bottom:15px; text-align:center; ">
		<%--<span style="font-size:20px; font-weight:bold;"> <%=GetGlobalResourceObject("qjsdur", "ekaekdwk") %> </span>--%>
		<input type="button" id="BTN_Submit" style="width:150px; height:50px; " value="<%=GetGlobalResourceObject("Member", "Modify") %>" onclick="CompanyModify()" />
		<span id="SPDELETE"></span>
        <span id="SPRestore"></span>
        <span id="SPHardDELETE"></span>
        <input type="hidden" id="HUsedNumber" value="<%=GetGlobalResourceObject("Alert", "CompanyCodeBeingUsed") %>" />
		<input type="hidden" id="HAccountID" value="<%=MEMBERINFO[2] %>" />
		<input type="hidden" id="DEBUG" />
		<div style="clear:both;">&nbsp;</div>
	</div>
	</div>
    </form>
</body>
</html>