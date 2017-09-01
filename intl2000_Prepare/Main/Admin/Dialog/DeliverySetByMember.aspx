<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeliverySetByMember.aspx.cs" Inherits="Admin_Dialog_DeliverySetByMember" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Delivery Set ByMember</title>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="../../Common/jquery-1.10.2.min.js"></script>
	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
	<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
	<script type="text/javascript">
		var TOTALBOXCOUNT;
		var BOXCOUNTLIMIT;
		var NOTSELECTEDBOXCOUNT;
		var StorageName;
		var CompanyName;
		var PackingUnit;
		var IsSelecetedDelivery = false;
		window.onload = function () {
			$("#FromDate").datepicker();
			$("#FromDate").datepicker("option", "dateFormat", "yymmdd");
			$("#ArrivalDate").datepicker();
			$("#ArrivalDate").datepicker("option", "dateFormat", "yymmdd");

			LoadDeliveryDefaultSetting();
			LoadSavedDelivery();
		}
		function LoadDeliveryDefaultSetting() {
			Admin.AddDeliveryPlaceOnload(form1.HConsigneePk.value, function (result) {
				for (var i = 0; i < result.length; i++) {
					var innerhtml = "";
					if (result[i] == "N") { continue; }
					var eachrow = result[i].split("####");
					CompanyName = eachrow[0];
					for (var j = 1; j < eachrow.length; j++) {
						if (eachrow[j] == "") { continue; }
						var each = eachrow[j].split("@@");
						innerhtml += "<li>";
						if (each[0] != "") { innerhtml += each[0] + " : "; }
						innerhtml += each[1] + " " + each[2];
						if (each[3] != "") { innerhtml += " " + each[3]; }
						if (each[4] != "") { innerhtml += "(" + each[4] + ")"; }
						if (each[0] != "") {
							innerhtml += "&nbsp;&nbsp;&nbsp;<input type=\"button\" value=\"선택\" onclick=\"SelThisWarehouse('" + each[0] + " : " + each[1] + "', '" + each[2] + "', '" + each[3] + "', '" + each[4] + "')\" />";
						}
						else {
							innerhtml += "&nbsp;&nbsp;&nbsp;<input type=\"button\" value=\"선택\" onclick=\"SelThisWarehouse('" + each[1] + "', '" + each[2] + "', '" + each[3] + "', '" + each[4] + "')\" />";
						}
						innerhtml += "</li>";
					}
					if (innerhtml != "") {
						document.getElementById("SavedWarehouse").innerHTML = "<ul style=\"list-style-type:none;\"><li><strong>Saved Warehouse</strong></li><ul style=\"list-style-type:disc;\">" + innerhtml + "</ul>";
					}
					break;
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function SelThisWarehouse(address, tel, staff, staffmobile) {
			form1.ToAddress.value = address;
			form1.ToTEL.value = tel;
			form1.StaffName.value = staff;
			form1.StaffMobile.value = staffmobile;
			BTN_Submit_Click();
		}
		function LoadSavedDelivery() {
			Admin.ModifyDeliveryPlaceOnLoad(form1.HOurBranchStorageOutPk.value, form1.HRequestFormPk.value, function (result) {
				if (result[0] == "N") {
					alert("ERROR"); opener.location.reload(); self.close(); return false;
				}
				if (form1.HOurBranchStorageOutPk.value == "N") {
					form1.HTransportBetweenCompanyPk.value = "N";
					form1.HStorageCode.value = "0";
					var ArrivalTime = result[0];
					if (ArrivalTime != "") {
						form1.ArrivalDate.value = ArrivalTime.substr(0, 8);
						if (ArrivalTime.length > 8) { form1.ArrivalHour.value = ArrivalTime.substr(8, 2); }
						if (ArrivalTime.length > 10) { form1.ArrivalMin.value = ArrivalTime.substr(10); }
					}
					form1.FromDate.value = result[0];

					TOTALBOXCOUNT = result[1];
					BOXCOUNTLIMIT = result[1];
					NOTSELECTEDBOXCOUNT = 0;
					document.getElementById("TotalBoxCount").innerHTML = result[1];
					document.getElementById("PackingUnit").innerHTML = result[2];
					document.getElementById("WeightNVolume").innerHTML = result[3] + "Kg " + result[4] + "CBM";
					form1.HWeight.value = result[3];
					form1.HVolume.value = result[4];
					form1.LeftBoxCount.value = BOXCOUNTLIMIT;
					if (result[6] == "") {
						form1.DepositWhereAfter.checked = true;
					}
					else {
						form1.DepositWhereBefore.checked = true;
						form1.MonetaryUnitCL.value = result[5];
					}
					form1.HPackingUnit.value = result[7];
				}
				else {
					if (result[0] != "") { document.getElementById("SPStorageName").innerHTML = " : " + result[0]; }
					StorageName = result[0];
					form1.HTransportBetweenCompanyPk.value = result[2];

					TOTALBOXCOUNT = result[5];
					BOXCOUNTLIMIT = parseInt(result[4]) + parseInt(result[1]);
					NOTSELECTEDBOXCOUNT = parseInt(result[4]);
					document.getElementById("TotalBoxCount").innerHTML = BOXCOUNTLIMIT;
					document.getElementById("PackingUnit").innerHTML = " / " + result[5] + " " + result[6];
					PackingUnit = result[6];
					document.getElementById("WeightNVolume").innerHTML = result[7] + "Kg " + result[8] + "CBM";
					form1.LeftBoxCount.value = result[1];
					form1.HWeight.value = result[7];
					form1.HVolume.value = result[8];
					form1.FromDate.value = result[15];
					var ArrivalTime = result[16];
					if (ArrivalTime != "") {
						form1.ArrivalDate.value = ArrivalTime.substr(0, 8);
						if (ArrivalTime.length > 8) { form1.ArrivalHour.value = ArrivalTime.substr(8, 2); }
						if (ArrivalTime.length > 10) { form1.ArrivalMin.value = ArrivalTime.substr(10); }
					}
					var WarehouseInfo
					if (result[17] == "") {
						WarehouseInfo = new Array('', '', '');
					}
					else {
						WarehouseInfo = result[17].split("@@");
					}
					form1.ToAddress.value = WarehouseInfo[0];
					form1.ToTEL.value = WarehouseInfo[1];
					form1.StaffName.value = WarehouseInfo[2];

					form1.StaffMobile.value = result[18];
					if (result[19] == "1") {
						form1.DepositWhereAfter.checked = true;
					}
					else {
						form1.DepositWhereBefore.checked = true;
					}
					form1.HPackingUnit.value = result[22];
					form1.HStorageCode.value = result[23];
					form1.HTransportBetweenBranchPk.value = result[24];
					//form1.MemberMemo.value = result[25];
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function BTN_Submit_Click() {
			if (form1.LeftBoxCount.value == "0" || form1.LeftBoxCount.value == "") {
				if (parseInt(form1.LeftBoxCount.value) > parseInt(BOXCOUNTLIMIT)) {
					alert("배차할수 있는 수량보다 많습니다. 배차할수 없습니다.");
					form1.BTNSubmit.style.visibility = "visible";
					return false;
				}
				alert("박스수량이 정확하지 않습니다. 배차할수 없습니다.");
				form1.BTNSubmit.style.visibility = "visible";
				return false;
			}
			var ArrivalTime = form1.ArrivalDate.value + form1.ArrivalHour.value + form1.ArrivalMin.value;
			var WarehouseInfo = form1.ToAddress.value + "@@" + form1.ToTEL.value + "@@" + form1.StaffName.value;

			var DepositWhere = "1";
			if (form1.DepositWhereBefore.checked == true) { DepositWhere = "0"; }
			Admin.SetDeliveryPlaceByMember(form1.HRequestFormPk.value, form1.HConsigneePk.value, form1.HStorageCode.value, form1.LeftBoxCount.value, NOTSELECTEDBOXCOUNT, BOXCOUNTLIMIT, TOTALBOXCOUNT, form1.HPackingUnit.value, form1.FromDate.value, ArrivalTime,
				WarehouseInfo, form1.StaffMobile.value, DepositWhere, form1.HAccountID.value, function (result) {
			//WarehouseInfo, form1.StaffMobile.value, form1.MemberMemo.value, DepositWhere, form1.HAccountID.value, function (result) {
				if (result == "1") {
					alert("Success");
					opener.location.reload();
					self.close();
					return false;
				}
				else {
					alert(result);
					form1.DEBUG.value = result;
					return false;
				}
			}, function (result) {
				alert("ERROR1 : " + result);
			});
		}
	</script>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Admin.asmx" />
			</Services>
		</asp:ScriptManager>
		<div style="padding-left: 10px; padding-right: 10px; width: 660px;">
			<fieldset>
				<legend><strong>Company Info</strong></legend>
				<div id="SavedWarehouse"></div>
				<div id="DeliveryHistory"></div>
			</fieldset>
		</div>
		<div style="width: 660px; padding: 10px;">
			<fieldset style="padding: 10px;">
				<legend><strong><%=GetGlobalResourceObject("qjsdur", "cnfrhwl") %></strong><span id="SPStorageName"></span></legend>
				<table>
					<tr>
						<td>BOX COUNT</td>
						<td><input type="text" style="width:30px; text-align:right; " id="LeftBoxCount" onclick="this.select();" readonly="readonly"/> / <span id="TotalBoxCount"></span><span id="PackingUnit"></span>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<span id="WeightNVolume"></span></td>
					</tr>
					<tr>
						<td>Start Date</td>
						<td>
							<input id="FromDate" size="10" style="text-align: center;" type="text" readonly="readonly" /></td>
					</tr>
					<tr>
						<td>Arrival Date</td>
						<td>
							<input id="ArrivalDate" size="10" style="text-align: center;" type="text" readonly="readonly" />&nbsp;&nbsp;
					<input id="ArrivalHour" maxlength='2' type="text" style="width: 18px; text-align: center;" />
							:
							<input id="ArrivalMin" maxlength='2' type="text" style="width: 18px; text-align: center;" /></td>
					</tr>
					<tr>
						<td><%=GetGlobalResourceObject("qjsdur", "ekaekdwkaud") %></td>
						<td>
							<input type="text" id="StaffName" /></td>
					</tr>
					<tr>
						<td>Mobile</td>
						<td>
							<input type="text" id="StaffMobile" /></td>
					</tr>
					<tr>
						<td>Address</td>
						<td>
							<textarea rows="3" cols="33" id="ToAddress" style="overflow: auto;"></textarea></td>
					</tr>
					<tr>
						<td>TEL</td>
						<td>
							<input type="text" id="ToTEL" /></td>
					</tr>
					<%--<tr>
						<td>Memo</td>
						<td>
							<input type="text" id="MemberMemo" /></td>
					</tr>--%>
					<tr>
						<td colspan="2">
							<input type="radio" name="DepositWhere" id="DepositWhereBefore" value="0" /><label for="DepositWhereBefore"><%=GetGlobalResourceObject("qjsdur", "gusqnf") %></label>
							<input type="radio" name="DepositWhere" id="DepositWhereAfter" value="1" checked="checked" /><label for="DepositWhereAfter"><%=GetGlobalResourceObject("qjsdur", "ckrqnf") %></label>
							<input type="text" id="MonetaryUnitCL" readonly="readonly" style="border: 0px; width: 10px;" /></td>
					</tr>
				</table>
			</fieldset>

			<input type="hidden" id="HOurBranchStorageOutPk" value="<%=OurBranchStorageOutPk %>" />
			<input type="hidden" id="HRequestFormPk" value="<%=RequestFormPk %>" />
			<input type="hidden" id="HConsigneePk" value="<%=CONSIGNEEPK %>" />
			<input type="hidden" id="HAccountID" value="<%=ACCOUNTID %>" />
			<input type="hidden" id="DEBUG" onclick="this.select();" />
			<input type="hidden" id="HPackingUnit" />
			<input type="hidden" id="HWeight" />
			<input type="hidden" id="HTransportBetweenBranchPk" />
			<input type="hidden" id="HTransportBetweenCompanyPk" />
			<input type="hidden" id="HVolume" />
			<input type="hidden" id="HStorageCode" />
		</div>
		<div style="padding: 10px; text-align: center;">
			<input type="button" value="SUBMIT" onclick="BTN_Submit_Click();" />&nbsp;&nbsp;&nbsp;
			<input type="button" value="CANCLE" onclick="window.close();" />
		</div>
	</form>
</body>
</html>
