<%@ Page Language="C#" AutoEventWireup="true" Culture="auto" UICulture="auto"  CodeFile="SelectTransportWay.aspx.cs" Inherits="Admin_Dialog_SelectTransportWay" Debug="true" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Transport</title>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
	    var TRANSPORTCL = "";
	    var BRANCHPK = "";
		var ACCOUNTID="";
		var BBHPK="";
		var DA=new Array();
		
		var AirHtml;
        var CarHtml ;
        var ShipHtml ;
        var HandCarry;
		var FCLHtml;
		var LCLHtml;

		function ArrivalDateSetSameWithDepartureDate() {
			document.getElementById("TBArrivalDate").value = parseInt(document.getElementById("TBDepartureDate").value) + 1;
		}
		window.onload = function () {
			AirHtml = "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
        "<tr><td>" + form1.gkdrhdaud.value + "</td><td><input type=\"text\" id=\"TBAirCompanyName\" readonly=\"readonly\" onclick=\"ShowModal();\" /><input type=\"hidden\" id=\"HTransportBBCLPk\" /></td><td>MASTER B/L No</td><td><input type=\"text\" id=\"TBMasterBL\" /></td></tr>" +
        "<tr><td>" + form1.cnfqkfrhdgkd.value + "</td><td><input type=\"text\" id=\"TBDepartureRegion\" /></td><td>" + form1.cnfqkfdPwjddlf.value + "</td><td><input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" onclick=\"openModal(TBDepartureDate, './calendar.html', 240, 220); ArrivalDateSetSameWithDepartureDate(); return false; \" />&nbsp;&nbsp;<input id=\"TBDepartureHour\"maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
        "<tr><td>" + form1.ehckrwrhdgkd.value + "</td><td><input type=\"text\" id=\"TBArrivalRegion\" /></td><td>" + form1.ehckrdPwjddlf.value + "</td><td><input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" onclick=\"openModal(TBArrivalDate, './calendar.html', 240, 220); return false;\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\" type=\"text\" maxLength='2' style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
        "<tr><td>" + form1.gkdrhdaud.value + "</td><td><input type=\"text\" id=\"TBAirName\" /></td><td></td><td></td></tr>" +
        "<tr><td>" + form1.cnfqkfgkdrhdtkdusfkrcj.value + "</td><td><input type=\"text\" id=\"TBAirDepartureTEL\" /></td><td>" + form1.ehckrgkdrhdtkdusfkrcj.value + "</td><td><input type=\"text\" id=\"TBAirArrivalTEL\" /></td></tr></table>";
			CarHtml = "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
        "<tr><td><input type=\"button\" value=\"미리저장\" onclick=\"ShowModal();\" /></td></tr>" +
		"<tr><td>" + form1.ckfidghltkaud.value + "</td><td><input type=\"text\" id=\"TBCarCompanyName\" /></td><td>" + form1.ghkanfthdwkdqjsgh.value + "</td><td><input type=\"text\" id=\"TBMasterBL\" /></td></tr>" +
        "<tr><td>" + form1.cnfqkfwl.value + "</td><td><input type=\"text\" id=\"TBDepartureRegion\" /></td><td>" + form1.cnfqkfdPwjddlf.value + "</td><td><input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" onclick=\"openModal(TBDepartureDate, './calendar.html', 240, 220); ArrivalDateSetSameWithDepartureDate(); return false;\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
        "<tr><td>" + form1.ehckrwl.value + "</td><td><input type=\"text\" id=\"TBArrivalRegion\" /></td><td>" + form1.ehckrdPwjddlf.value + "</td><td><input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" onclick=\"openModal(TBArrivalDate, './calendar.html', 240, 220); return false;\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
        "<tr><td>" + form1.ckfidqjsgh.value + "</td><td><input type=\"text\" id=\"TBCarNo\" /></td><td>" + form1.ckfidrbrur.value + "</td><td><input type=\"text\" id=\"TBCarSize\" /></td></tr>" +
        "<tr><td>" + form1.ckfiddusfkrcj.value + "</td><td><input type=\"text\" id=\"TBDriverTEL\" /></td><td>" + form1.rltktjdaud.value + "</td><td><input type=\"text\" id=\"TBDriverName\" /></td></tr></table>";
			ShipHtml = "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\"><tr><td>" + form1.tjsqkrghltkaud.value + "</td><td><input type=\"text\" id=\"TBShipCompanyName\" readonly=\"readonly\" onclick=\"ShowModal();\"   /><input type=\"hidden\" id=\"HTransportBBCLPk\" /></td><td>MASTER B/L</td><td><input type=\"text\" id=\"TBMasterBL\" /></td></tr>" +
        "<tr><td>" + form1.tjswjrgkd.value + "</td><td><input type=\"text\" id=\"TBDepartureRegion\" /></td><td>" + form1.cnfqkfdPwjddlf.value + "</td><td><input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" onclick=\"openModal(TBDepartureDate, './calendar.html', 240, 220); ArrivalDateSetSameWithDepartureDate(); return false;\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
        "<tr><td>" + form1.ehckrgkd.value + "</td><td><input type=\"text\" id=\"TBArrivalRegion\" /></td><td>" + form1.ehckrdPwjddlf.value + "</td><td><input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" onclick=\"openModal(TBArrivalDate, './calendar.html', 240, 220); return false;\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
        "<tr><td>" + form1.tjsqkraud.value + "</td><td><input type=\"text\" id=\"TBShipName\" /></td><td>&nbsp;</td><td>&nbsp;</td></tr>" +
        "<tr><td>" + form1.zjsxpdlsjrbrur.value + "</td><td ><input type=\"text\" id=\"TBContainerSize\" /></td><td>" + form1.gkdck.value + "</td><td><input type=\"text\" id=\"TBShippingTime\" /></td></tr>" +
        "<tr><td>" + form1.zjsxpdlsjqjsgh.value + "</td><td><input type=\"text\" id=\"TBContainerNo\" /></td><td>" + form1.Tlfqjsgh.value + "</td><td><input type=\"text\" id=\"TBSealNo\" /></td></tr></table>";
			HandCarry = "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\"><tr><td>Hand carry Name</td><td><input type=\"text\" id=\"TBHandCarryCompanyName\" readonly=\"readonly\" onclick=\"ShowModal();\" /><input type=\"hidden\" id=\"HTransportBBCLPk\" /></td><td>MASTER B/L</td><td><input type=\"text\" id=\"TBMasterBL\" /></td></tr>" +
        "<tr><td>" + form1.tjswjrgkd.value + "</td><td><input type=\"text\" id=\"TBDepartureRegion\" /></td><td>" + form1.cnfqkfdPwjddlf.value + "</td><td><input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" onclick=\"openModal(TBDepartureDate, './calendar.html', 240, 220); ArrivalDateSetSameWithDepartureDate(); return false;\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
        "<tr><td>" + form1.ehckrgkd.value + "</td><td><input type=\"text\" id=\"TBArrivalRegion\" /></td><td>" + form1.ehckrdPwjddlf.value + "</td><td><input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" onclick=\"openModal(TBArrivalDate, './calendar.html', 240, 220); return false;\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
        "<tr><td>Departure Staff TEL</td><td><input type=\"text\" id=\"TBHandCarryDepartureTEL\" /></td><td>Arrival Staff TEL</td><td><input type=\"text\" id=\"TBHandCarryArrivalTEL\" /></td></tr></table>";
			FCLHtml = "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\"><tr><td>" + form1.tjsqkrghltkaud.value + "</td><td><input type=\"text\" id=\"TBShipCompanyName\" readonly=\"readonly\" onclick=\"ShowModal();\"   /><input type=\"hidden\" id=\"HTransportBBCLPk\" /></td><td>MASTER B/L</td><td><input type=\"text\" id=\"TBMasterBL\" /></td></tr>" +
        "<tr><td>" + form1.tjswjrgkd.value + "</td><td><input type=\"text\" id=\"TBDepartureRegion\" /></td><td>" + form1.cnfqkfdPwjddlf.value + "</td><td><input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" onclick=\"openModal(TBDepartureDate, './calendar.html', 240, 220); ArrivalDateSetSameWithDepartureDate(); return false;\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
        "<tr><td>" + form1.ehckrgkd.value + "</td><td><input type=\"text\" id=\"TBArrivalRegion\" /></td><td>" + form1.ehckrdPwjddlf.value + "</td><td><input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" onclick=\"openModal(TBArrivalDate, './calendar.html', 240, 220); return false;\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
        "<tr><td>" + form1.tjsqkraud.value + "</td><td><input type=\"text\" id=\"TBShipName\" /></td><td>&nbsp;</td><td>&nbsp;</td></tr>" +
        "<tr><td>" + form1.zjsxpdlsjrbrur.value + "</td><td ><input type=\"text\" id=\"TBContainerSize\" /></td><td>" + form1.gkdck.value + "</td><td><input type=\"text\" id=\"TBShippingTime\" /></td></tr>" +
        "<tr><td>" + form1.zjsxpdlsjqjsgh.value + "</td><td><input type=\"text\" id=\"TBContainerNo\" /></td><td>" + form1.Tlfqjsgh.value + "</td><td><input type=\"text\" id=\"TBSealNo\" /></td></tr></table>";
			LCLHtml = "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\"><tr><td>Agency Name</td><td><input type=\"text\" id=\"TBShipCompanyName\" readonly=\"readonly\" onclick=\"ShowModal();\"   /><input type=\"hidden\" id=\"HTransportBBCLPk\" /></td><td>B/L No</td><td><input type=\"text\" id=\"TBMasterBL\" /></td></tr>" +
        "<tr><td>" + form1.tjswjrgkd.value + "</td><td><input type=\"text\" id=\"TBDepartureRegion\" /></td><td>" + form1.cnfqkfdPwjddlf.value + "</td><td><input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" onclick=\"openModal(TBDepartureDate, './calendar.html', 240, 220); ArrivalDateSetSameWithDepartureDate(); return false;\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
        "<tr><td>" + form1.ehckrgkd.value + "</td><td><input type=\"text\" id=\"TBArrivalRegion\" /></td><td>" + form1.ehckrdPwjddlf.value + "</td><td><input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" onclick=\"openModal(TBArrivalDate, './calendar.html', 240, 220); return false;\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
        "<tr><td>" + form1.tjsqkraud.value + "</td><td><input type=\"text\" id=\"TBShipName\" /></td><td>&nbsp;</td><td>&nbsp;</td></tr>" +
        "<tr><td>" + form1.zjsxpdlsjrbrur.value + "</td><td ><input type=\"text\" id=\"TBContainerSize\" /></td><td>&nbsp;</td><td>&nbsp;</td></tr>" +
        "<tr><td>" + form1.zjsxpdlsjqjsgh.value + "</td><td><input type=\"text\" id=\"TBContainerNo\" /></td><td>" + form1.Tlfqjsgh.value + "</td><td><input type=\"text\" id=\"TBSealNo\" /></td></tr>"+
		"<tr><td>Departure Staff</td><td><input type=\"text\" id=\"TBHandCarryDepartureTEL\" /></td><td>Arrival Staff</td><td><input type=\"text\" id=\"TBHandCarryArrivalTEL\" /></td></tr></table>";
			if (form1.HCheckPostBack.value == "Y") {
				alert(form1.dhksfy.value);
				opener.location.reload();
				self.close();
				return false;
			}
			var uri = location.href;
			if (uri.indexOf("?") == -1) {
				ACCOUNTID = opener.form1.HAccountID.value;
				BRANCHPK = opener.form1.HBranchPk.value;
			}
			else {
				form1.HIsModify.value = "Y";
				BBHPK = dialogArguments[0];
				ACCOUNTID = dialogArguments[1];
				BRANCHPK = dialogArguments[2];
				SelectTransportCL(dialogArguments[3]);
				document.getElementById("TransportCL").value = dialogArguments[3];
				Admin.LoadTransportBBHead(BBHPK, function (result) {
					document.getElementById("TBMasterBL").value = result[0];
					document.getElementById("ToBranch").value = result[1];
					var FromDateTime = result[2].split(".");
					document.getElementById("TBDepartureDate").value = FromDateTime[0] + FromDateTime[1] + FromDateTime[2];
					var fromtime = FromDateTime[3].trim().split(":");
					document.getElementById("TBDepartureHour").value = fromtime[0];
					document.getElementById("TBDepartureMin").value = fromtime[1];

					var ToDateTime = result[3].split(".");
					document.getElementById("TBArrivalDate").value = ToDateTime[0] + ToDateTime[1] + ToDateTime[2];
					var Totime = ToDateTime[3].trim().split(":");
					document.getElementById("TBArrivalHour").value = Totime[0];
					document.getElementById("TBArrivalMin").value = Totime[1];

					//var Comment=form1.TBComment.value;
					var Description = result[4].split("#@!");
					switch (document.getElementById("TransportCL").value) {
						case "1":
							document.getElementById("TBAirCompanyName").value = Description[0];
							document.getElementById("TBDepartureRegion").value = Description[1];
							document.getElementById("TBArrivalRegion").value = Description[2];
							document.getElementById("TBAirName").value = Description[3];
							document.getElementById("TBAirDepartureTEL").value = Description[4];
							document.getElementById("TBAirArrivalTEL").value = Description[5];
							break;
						case "2":
							document.getElementById("TBCarCompanyName").value = Description[0];
							document.getElementById("TBDepartureRegion").value = Description[1];
							document.getElementById("TBArrivalRegion").value = Description[2];
							document.getElementById("TBCarNo").value = Description[3];
							document.getElementById("TBCarSize").value = Description[4];
							document.getElementById("TBDriverName").value = Description[5];
							document.getElementById("TBDriverTEL").value = Description[6];
							break;
						case "3":
							document.getElementById("TBShipCompanyName").value = Description[0];
							document.getElementById("TBDepartureRegion").value = Description[1];
							document.getElementById("TBArrivalRegion").value = Description[2];
							document.getElementById("TBShipName").value = Description[3];
							document.getElementById("TBShipperName").value = Description[4];
							document.getElementById("TBContainerSize").value = Description[5];
							document.getElementById("TBShippingTime").value = Description[6];
							document.getElementById("TBContainerNo").value = Description[7];
							document.getElementById("TBSealNo").value = Description[8];
							break;
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function SelectTransportCL(value) {
			TRANSPORTCL = value;
			switch (TRANSPORTCL) {
				case "0": document.getElementById("PnTransport").innerHTML = ""; break;
				case "1": document.getElementById("PnTransport").innerHTML = AirHtml; break;
				case "2": document.getElementById("PnTransport").innerHTML = CarHtml; break;
				case "3": document.getElementById("PnTransport").innerHTML = ShipHtml; break;
				case "4": document.getElementById("PnTransport").innerHTML = HandCarry; break;
				case "5": document.getElementById("PnTransport").innerHTML = FCLHtml; break;
				case "6": document.getElementById("PnTransport").innerHTML = LCLHtml; break;
			}
		}
		function ShowModal() {
			var dialogArgument = new Array();
			dialogArgument[0] = TRANSPORTCL;
			dialogArgument[1] = BRANCHPK;
			switch (TRANSPORTCL) {
				case "1":
					var retVal = window.showModalDialog('SelectTransportWayAir.aspx', dialogArgument, 'dialogWidth=700px;dialogHeight=400px;resizable=1;status=0;scroll=1;help=0;');
					try {
						var ReturnArray = retVal.split('#@!');
						form1.HTransportBBCLPk.value = ReturnArray[0];
						form1.TBAirCompanyName.value = ReturnArray[1];
						form1.TBAirName.value = ReturnArray[2];
						form1.TBDepartureRegion.value = ReturnArray[3];
						form1.TBArrivalRegion.value = ReturnArray[4];
						form1.TBAirDepartureTEL.value = ReturnArray[5];
						form1.TBAirArrivalTEL.value = ReturnArray[6];
					}
					catch (ex) { return false; }
					break;
				case "2":
					var retVal = window.showModalDialog('SelectTransportWayCar.aspx', dialogArgument, 'dialogWidth=700px;dialogHeight=400px;resizable=1;status=0;scroll=1;help=0;');
					try {
						var ReturnArray = retVal.split('#@!');
						document.getElementById("TBCarCompanyName").value = ReturnArray[1];
						document.getElementById("TBDepartureRegion").value = ReturnArray[2];
						document.getElementById("TBArrivalRegion").value = ReturnArray[3];
						document.getElementById("TBDriverTEL").value = ReturnArray[4];
					}
					catch (ex) { alert(ex.Message); return false; }
					break;
				case "3":
					var retVal = window.showModalDialog('SelectTransportWayShipping.aspx', dialogArgument, 'dialogWidth=1000px;dialogHeight=400px;resizable=1;status=0;scroll=1;help=0;');
					try {
						var ReturnArray = retVal.split('#@!');
						form1.HTransportBBCLPk.value = ReturnArray[0];
						document.getElementById("TBShipCompanyName").value = ReturnArray[1];
						document.getElementById("TBShipName").value = ReturnArray[2];
						document.getElementById("TBShipperName").value = ReturnArray[3];
						document.getElementById("TBDepartureRegion").value = ReturnArray[4];
						document.getElementById("TBArrivalRegion").value = ReturnArray[5];
					}
					catch (ex) { return false; }
					break;
				case "4":
					var retVal = window.showModalDialog('SelectTransportWayHandcarry.aspx', dialogArgument, 'dialogWidth=700px;dialogHeight=400px;resizable=1;status=0;scroll=1;help=0;');
					try {
						var ReturnArray = retVal.split('#@!');
						form1.HTransportBBCLPk.value = ReturnArray[0];
						document.getElementById("TBHandCarryCompanyName").value = ReturnArray[1];
						document.getElementById("TBDepartureRegion").value = ReturnArray[2];
						document.getElementById("TBArrivalRegion").value = ReturnArray[3];
						document.getElementById("TBHandCarryDepartureTEL").value = ReturnArray[4];
						document.getElementById("TBHandCarryArrivalTEL").value = ReturnArray[5];
					}
					catch (ex) { return false; }
					break;
			}
		}
		function openModal(obj, file_name, width, height) {//날짜 입력 폼 띄우기
			height = height + 20;
			var rand = Math.random() * 4;
			window.showModalDialog('../../Common/' + file_name + '?rand=' + rand, obj, 'dialogWidth=' + width + 'px;dialogHeight=' + height + 'px;resizable=0;status=0;scroll=0;help=0');
		}
		function InsertRow() {
			var objTable = document.getElementById('TransportPay');
			var lastRow = objTable.rows.length;
			var thisLineNo = lastRow - 2;
			var objRow1 = objTable.insertRow(lastRow);
			var objCell1 = objRow1.insertCell();
			var objCell2 = objRow1.insertCell();

			var MonetaryUnit = "18";
			if (thisLineNo != 0) { MonetaryUnit = document.getElementById('TransportPayment[' + (thisLineNo - 1) + '][MonetaryUnit]').value; }
			objCell1.align = "center";
			objCell2.align = "center";

			objCell1.innerHTML = "<input type=\"text\" id=\"TransportPayment[" + thisLineNo + "][Description]\" style=\"width:90px;\" />";
			objCell2.innerHTML = "<select id=\"TransportPayment[" + thisLineNo + "][MonetaryUnit]\" size=\"1\"><option value=\"18\">RMB ￥</option><option value=\"19\">USD $</option><option value=\"20\">KRW ￦</option><option value=\"21\">JPY Y</option><option value=\"22\">HKD HK$</option><option value=\"23\">EUR €</option></select>" +
                                                "<input type=\"text\" id=\"TransportPayment[" + thisLineNo + "][Value]\" style=\"width:125px; text-align:center;\" />";
			document.getElementById("TransportPayment[" + thisLineNo + "][MonetaryUnit]").value = MonetaryUnit;
		}
		function BtnSubmitClick() {
			if (form1.TransportCL.value == "0") {
				alert(form1.dnsthdqkdqjqdmftjsxorgowntpdy.value);
				return false;
			}
			var ToBranchPk = document.getElementById("ToBranch").value;
			if (BRANCHPK == ToBranchPk && form1.HIsModify.value != "Y") {
				alert("같은지역으로는 못보냅니다.");
				return false;
			}
			if (document.getElementById("TBDepartureDate").value == "") {
				alert(form1.cnfqkfdPwjddlfdmsvlftntkgkddlqslek.value);
				return false;
			}
			if (document.getElementById("TBArrivalDate").value == "") {
				alert("도착예정일은 필수사항입니다.");
				return false;
			}
			var BLNo = document.getElementById("TBMasterBL").value;
			var FromDateTime = document.getElementById("TBDepartureDate").value.substr(0, 4) + "." + document.getElementById("TBDepartureDate").value.substr(4, 2) + "." + document.getElementById("TBDepartureDate").value.substr(6, 2) + ". " + document.getElementById("TBDepartureHour").value + ":" + document.getElementById("TBDepartureMin").value;
			var ToDateTime = document.getElementById("TBArrivalDate").value.substr(0, 4) + "." + document.getElementById("TBArrivalDate").value.substr(4, 2) + "." + document.getElementById("TBArrivalDate").value.substr(6, 2) + ". " + document.getElementById("TBArrivalHour").value + ":" + document.getElementById("TBArrivalMin").value;
			var Comment = form1.TBComment.value;
			switch (TRANSPORTCL) {
				case "1":
					var InfoSum = document.getElementById("TBAirCompanyName").value + "#@!" +
												document.getElementById("TBDepartureRegion").value + "#@!" +
												document.getElementById("TBArrivalRegion").value + "#@!" +
												document.getElementById("TBAirName").value + "#@!" +
												document.getElementById("TBAirDepartureTEL").value + "#@!" +
												document.getElementById("TBAirArrivalTEL").value;
					break;
				case "2":
					var InfoSum = document.getElementById("TBCarCompanyName").value + "#@!" +
												document.getElementById("TBDepartureRegion").value + "#@!" +
												document.getElementById("TBArrivalRegion").value + "#@!" +
												document.getElementById("TBCarNo").value + "#@!" +
												document.getElementById("TBCarSize").value + "#@!" +
												document.getElementById("TBDriverName").value + "#@!" +
												document.getElementById("TBDriverTEL").value;
					break;
				case "3":
					var InfoSum = document.getElementById("TBShipCompanyName").value + "#@!" +
												document.getElementById("TBDepartureRegion").value + "#@!" +
												document.getElementById("TBArrivalRegion").value + "#@!" +
												document.getElementById("TBShipName").value + "#@!#@!" +
												document.getElementById("TBContainerSize").value + "#@!" +
												document.getElementById("TBShippingTime").value + "#@!" +
												document.getElementById("TBContainerNo").value + "#@!" +
												document.getElementById("TBSealNo").value;
					break;
				case "4":
					var InfoSum = document.getElementById("TBHandCarryCompanyName").value + "#@!" +
												document.getElementById("TBDepartureRegion").value + "#@!" +
												document.getElementById("TBArrivalRegion").value + "#@!" +
												document.getElementById("TBHandCarryDepartureTEL").value + "#@!" +
												document.getElementById("TBHandCarryArrivalTEL").value;
					break;
			}
			if (form1.HIsModify.value == "Y") {
				Admin.UpdateTransportBBHead(BBHPK, TRANSPORTCL, BLNo, ToBranchPk, FromDateTime, ToDateTime, InfoSum, Comment, ACCOUNTID, function (result) {
					form1.HSavedPk.value = result;
					form1.TBComment.value = result;
					self.close();
					return false;
				}, function (result) { alert("ERROR : " + result); });
			}
			else {
				Admin.InsertTransportBBHead(TRANSPORTCL, BLNo, BRANCHPK, ToBranchPk, FromDateTime, ToDateTime, InfoSum, Comment, ACCOUNTID, function (result) {
					form1.HSavedPk.value = result;
					form1.TBComment.value = result;
					opener.location.reload();
					self.close(); return false;
				}, function (result) { alert("ERROR : " + result); });
			}
		}
	</script>
    <link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />    
</head>
<body style="background-color:#999999; padding:10px; overflow-x:hidden; ">
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
	<div style="width:560px;   padding-left:10px; padding-right:10px;  background-color:white;">
		<input type="hidden" id="HSavedPk" name="HSavedPk" />
		<input type="hidden" id="HCheckPostBack" value="<%=IsUpload %>" />
		<select id="TransportCL" onchange="SelectTransportCL(this.value)">
			<option value="0"><%=GetGlobalResourceObject("qjsdur", "dnsthdqkdqjq") %></option>
            <option value="1"><%=GetGlobalResourceObject("qjsdur", "gkdrhd") %></option>
			<option value="2"><%=GetGlobalResourceObject("qjsdur", "ckfid") %></option>
			<option value="3"><%=GetGlobalResourceObject("qjsdur", "tjsqkr") %></option>
			<option value="4">hand carry</option>
			<option value="5">FCL</option>
			<option value="6">LCL</option>
		</select><%=OurBranchSelect %>
		<div id="PnTransport"></div>
		<div><%=GetGlobalResourceObject("qjsdur", "zhapsxm")%> : <textarea id="TBComment" cols="50" rows="4" ></textarea></div>

		<div id="PnBTNSubmit" style="padding:10px;">
			<input type="button" value="SUBMIT" onclick="BtnSubmitClick();" style="width:370px; height:50px;" /> 
			<input type="hidden" id="HIsModify" value="N" />
			<input type="hidden" id="cnfqkfwl" value="<%=GetGlobalResourceObject("qjsdur", "cnfqkfwl") %>" />
			<input type="hidden" id="zjsxpdlsjrbrur" value="<%=GetGlobalResourceObject("qjsdur", "zjsxpdlsjrbrur") %>" />
			<input type="hidden" id="zjsxpdlsjqjsgh" value="<%=GetGlobalResourceObject("qjsdur", "zjsxpdlsjqjsgh") %>" />
			<input type="hidden" id="ckfiddusfkrcj" value="<%=GetGlobalResourceObject("qjsdur", "ckfiddusfkrcj") %>" />
			<input type="hidden" id="ckfidghltkaud" value="<%=GetGlobalResourceObject("qjsdur", "ckfidghltkaud") %>" />
			<input type="hidden" id="gkdrhdtk" value="<%=GetGlobalResourceObject("qjsdur", "gkdrhdtk") %>" />
			<input type="hidden" id="cnfqkfrhdgkd" value="<%=GetGlobalResourceObject("qjsdur", "cnfqkfrhdgkd") %>" />
			<input type="hidden" id="cnfqkfdPwjddlf" value="<%=GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") %>" />
			<input type="hidden" id="ehckrwrhdgkd" value="<%=GetGlobalResourceObject("qjsdur", "ehckrwrhdgkd") %>" />
			<input type="hidden" id="ehckrdPwjddlf" value="<%=GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") %>" />
			<input type="hidden" id="gkdrhdaud" value="<%=GetGlobalResourceObject("qjsdur", "gkdrhdaud") %>" />
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
		</div>
    </div>
    </form>
</body>

</html>