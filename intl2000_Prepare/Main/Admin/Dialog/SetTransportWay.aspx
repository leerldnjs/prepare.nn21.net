<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetTransportWay.aspx.cs" Inherits="Admin_Dialog_SetTransportWay" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Transport</title>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script src="../../Common/jquery-1.10.2.min.js"></script>
	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
	<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>

	<script type="text/javascript">
		var TRANSPORTCL = "";
		var BRANCHPK = "";
		var ACCOUNTID = "";
		var BBHPK = "";
		var DA = new Array();
		var Check;

		var AirHtml;
		var Format = new Array();
		var CarHtml;
		var ShipHtml;
		var HandCarry;
		var FCLHtml;
		var LCLHtml;

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
		function ArrivalDateSetSameWithDepartureDate() {
			document.getElementById("TBArrivalDate").value = parseInt(document.getElementById("TBDepartureDate").value) + 1;
		}
		window.onload = function () {
			Format[1] = new Array(); 	//AIR 
			Format[1][0] = form1.gkdrhdaud.value;
			Format[1][1] = "<input type=\"text\" id=\"TBAirCompanyName\" readonly=\"readonly\" onclick=\"ShowModal();\" /><input type=\"hidden\" id=\"HTransportBBCLPk\" />";
			Format[1][2] = "MASTER B/L No";
			Format[1][3] = "<input type=\"text\" id=\"TBMasterBL\"  />";
			Format[1][4] = form1.cnfqkfrhdgkd.value;
			Format[1][5] = "<input type=\"text\" id=\"TBDepartureRegion\" />";
			Format[1][6] = form1.cnfqkfdPwjddlf.value;
			Format[1][7] = "<input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\"maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" />";
			Format[1][8] = form1.ehckrwrhdgkd.value;
			Format[1][9] = "<input type=\"text\" id=\"TBArrivalRegion\" />";
			Format[1][10] = form1.ehckrdPwjddlf.value;
			Format[1][11] = "<input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\" type=\"text\" maxLength='2' style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" />";
			Format[1][12] = form1.gkdrhdaud.value;
			Format[1][13] = "<input type=\"text\" id=\"TBAirName\" />";
			Format[1][14] = "&nbsp;";
			Format[1][15] = "&nbsp;";
			Format[1][16] = form1.cnfqkfgkdrhdtkdusfkrcj.value;
			Format[1][17] = "<input type=\"text\" id=\"TBAirDepartureTEL\" />";
			Format[1][18] = form1.ehckrgkdrhdtkdusfkrcj.value;
			Format[1][19] = "<input type=\"text\" id=\"TBAirArrivalTEL\" />";

			Format[7] = new Array(); //CAR
			Format[7][0] = "<input type=\"button\" value=\"미리저장\" onclick=\"ShowModal();\" /><input type=\"hidden\" id=\"HTransportBBCLPk\" />";
			Format[7][1] = "&nbsp;";
			Format[7][2] = "&nbsp;";
			Format[7][3] = "&nbsp;";
			Format[7][4] = form1.ckfidghltkaud.value;
			Format[7][5] = "<input type=\"text\" id=\"TBCarCompanyName\" />";
			Format[7][6] = form1.ghkanfthdwkdqjsgh.value;
			Format[7][7] = "<input type=\"text\" id=\"TBMasterBL\" />";
			Format[7][8] = form1.cnfqkfwl.value;
			Format[7][9] = "<input type=\"text\" id=\"TBDepartureRegion\" />";
			Format[7][10] = form1.cnfqkfdPwjddlf.value;
			Format[7][11] = "<input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />";
			Format[7][12] = form1.ehckrwl.value;
			Format[7][13] = "<input type=\"text\" id=\"TBArrivalRegion\" />";
			Format[7][14] = form1.ehckrdPwjddlf.value;
			Format[7][15] = "<input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />";
			Format[7][16] = form1.ckfidqjsgh.value;
			Format[7][17] = "<input type=\"text\" id=\"TBCarNo\" />";
			Format[7][18] = form1.ckfidrbrur.value;
			Format[7][19] = "<input type=\"text\" id=\"TBCarSize\" />";
			Format[7][20] = form1.ckfiddusfkrcj.value;
			Format[7][21] = "<input type=\"text\" id=\"TBDriverTEL\" />";
			Format[7][22] = form1.rltktjdaud.value;
			Format[7][23] = "<input type=\"text\" id=\"TBDriverName\" />";

			Format[7][24] = form1.zjsxpdlsjrbrur.value;
			Format[7][25] = "<input type=\"text\" id=\"TBContainerSize\" />";
			Format[7][26] = "&nbsp;";
			Format[7][27] = "&nbsp;";
			Format[7][28] = form1.zjsxpdlsjqjsgh.value;
			Format[7][29] = "<input type=\"text\" id=\"TBContainerNo\" maxlength=\"11\"/>";
			Format[7][30] = form1.Tlfqjsgh.value;
			Format[7][31] = "<input type=\"text\" id=\"TBSealNo\" />";			
			

			Format[3] = new Array(); 	//SHIP  
			Format[3][0] = form1.tjsqkrghltkaud.value;
			Format[3][1] = "<input type=\"text\" id=\"TBShipCompanyName\" readonly=\"readonly\" onclick=\"ShowModal();\"   /><input type=\"hidden\" id=\"HTransportBBCLPk\" />";
			Format[3][2] = "MASTER B/L";
			Format[3][3] = "<input type=\"text\" id=\"TBMasterBL\" />";
			Format[3][4] = form1.tjswjrgkd.value;
			Format[3][5] = "<input type=\"text\" id=\"TBDepartureRegion\"/>";
			Format[3][6] = form1.cnfqkfdPwjddlf.value;
			Format[3][7] = "<input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />";
			Format[3][8] = form1.ehckrgkd.value;
			Format[3][9] = "<input type=\"text\" id=\"TBArrivalRegion\"  />";
			Format[3][10] = form1.ehckrdPwjddlf.value;
			Format[3][11] = "<input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />";
			Format[3][12] = form1.tjsqkraud.value;
			Format[3][13] = "<input type=\"text\" id=\"TBShipName\" />";
			Format[3][14] = "&nbsp;";
			Format[3][15] = "&nbsp;";
			Format[3][16] = form1.zjsxpdlsjrbrur.value;
			Format[3][17] = "<input type=\"text\" id=\"TBContainerSize\" />";
			Format[3][18] = form1.gkdck.value;
			Format[3][19] = "<input type=\"text\" id=\"TBShippingTime\" />";
			Format[3][20] = form1.zjsxpdlsjqjsgh.value;
			Format[3][21] = "<input type=\"text\" id=\"TBContainerNo\" maxlength=\"11\"/>";
			Format[3][22] = form1.Tlfqjsgh.value;
			Format[3][23] = "<input type=\"text\" id=\"TBSealNo\" />";
			Format[3][24] = "Shipper";
			Format[3][25] = "<input type=\"text\" id=\"TBShipper\" readonly=\"readonly\" onclick=\"SelectCompanyInDocument();\"/>";
			Format[3][26] = "Address";
			Format[3][27] = "<input type=\"text\" id=\"TBAddress\" />";

			Format[4] = new Array();  
			Format[4][0] = "Hand carry Name";
			Format[4][1] = "<input type=\"text\" id=\"TBHandCarryCompanyName\" readonly=\"readonly\" onclick=\"ShowModal();\" /><input type=\"hidden\" id=\"HTransportBBCLPk\" />";
			Format[4][2] = "MASTER B/L";
			Format[4][3] = "<input type=\"text\" id=\"TBMasterBL\" />";
			Format[4][4] = form1.tjswjrgkd.value;
			Format[4][5] = "<input type=\"text\" id=\"TBDepartureRegion\"/>";
			Format[4][6] = form1.cnfqkfdPwjddlf.value;
			Format[4][7] = "<input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />";
			Format[4][8] = form1.ehckrgkd.value;
			Format[4][9] = "<input type=\"text\" id=\"TBArrivalRegion\" />";
			Format[4][10] = form1.ehckrdPwjddlf.value;
			Format[4][11] = "<input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />";
			Format[4][12] = "Departure Staff TEL";
			Format[4][13] = "<input type=\"text\" id=\"TBHandCarryDepartureTEL\" />";
			Format[4][14] = "Arrival Staff TEL";
			Format[4][15] = "<input type=\"text\" id=\"TBHandCarryArrivalTEL\" />";

			Format[5] = new Array(); 	//FCL    
			Format[5][0] = form1.tjsqkrghltkaud.value;
			Format[5][1] = "<input type=\"text\" id=\"TBShipCompanyName\" readonly=\"readonly\" onclick=\"ShowModal();\" /><input type=\"hidden\" id=\"HTransportBBCLPk\" />";
			Format[5][2] = "MASTER B/L";
			Format[5][3] = "<input type=\"text\" id=\"TBMasterBL\" />";
			Format[5][4] = form1.tjswjrgkd.value;
			Format[5][5] = "<input type=\"text\" id=\"TBDepartureRegion\" />";
			Format[5][6] = form1.cnfqkfdPwjddlf.value;
			Format[5][7] = "<input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />";
			Format[5][8] = form1.ehckrgkd.value;
			Format[5][9] = "<input type=\"text\" id=\"TBArrivalRegion\" />";
			Format[5][10] = form1.ehckrdPwjddlf.value;
			Format[5][11] = "<input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />";
			Format[5][12] = form1.tjsqkraud.value;
			Format[5][13] = "<input type=\"text\" id=\"TBShipName\"  />";
			Format[5][14] = "Title | Memo";
			Format[5][15] = "<input type=\"text\" id=\"TBFCLTitle\" />";
			Format[5][16] = form1.zjsxpdlsjrbrur.value;
			Format[5][17] = "<input type=\"text\" id=\"TBContainerSize\" />";
			Format[5][18] = form1.gkdck.value;
			Format[5][19] = "<input type=\"text\" id=\"TBShippingTime\" />";
			Format[5][20] = form1.zjsxpdlsjqjsgh.value;
			Format[5][21] = "<input type=\"text\" id=\"TBContainerNo\" maxlength=\"11\"/>";
			Format[5][22] = form1.Tlfqjsgh.value;
			Format[5][23] = "<input type=\"text\" id=\"TBSealNo\" />";
			Format[5][24] = "Shipper";
			Format[5][25] = "<input type=\"text\" id=\"TBShipper\" readonly=\"readonly\" onclick=\"SelectCompanyInDocument();\"/>";
			Format[5][26] = "Address";
			Format[5][27] = "<input type=\"text\" id=\"TBAddress\" />";

			Format[6] = new Array(); //LCL       
			Format[6][0] = "Agency Name";
			Format[6][1] = "<input type=\"text\" id=\"TBShipCompanyName\" readonly=\"readonly\" onclick=\"ShowModal();\"   /><input type=\"hidden\" id=\"HTransportBBCLPk\" />";
			Format[6][2] = "B/L No";
			Format[6][3] = "<input type=\"text\" id=\"TBMasterBL\" />";
			Format[6][4] = form1.tjswjrgkd.value;
			Format[6][5] = "<input type=\"text\" id=\"TBDepartureRegion\"  />";
			Format[6][6] = form1.cnfqkfdPwjddlf.value;
			Format[6][7] = "<input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />";
			Format[6][8] = form1.ehckrgkd.value;
			Format[6][9] = "<input type=\"text\" id=\"TBArrivalRegion\" />";
			Format[6][10] = form1.ehckrdPwjddlf.value;
			Format[6][11] = "<input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" readonly=\"readonly\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\"  maxLength='2'  type=\"text\" style=\"width:18px;text-align:center;\" />";
			Format[6][12] = form1.tjsqkraud.value;
			Format[6][13] = "<input type=\"text\" id=\"TBShipName\" />";
			Format[6][14] = "&nbsp;";
			Format[6][15] = "&nbsp;";
			Format[6][16] = form1.zjsxpdlsjrbrur.value;
			Format[6][17] = "<input type=\"text\" id=\"TBContainerSize\" />";
			Format[6][18] = "&nbsp;";
			Format[6][19] = "&nbsp;";
			Format[6][20] = form1.zjsxpdlsjqjsgh.value;
			Format[6][21] = "<input type=\"text\" id=\"TBContainerNo\" maxlength=\"11\"/>";
			Format[6][22] = form1.Tlfqjsgh.value;
			Format[6][23] = "<input type=\"text\" id=\"TBSealNo\" />";
			Format[6][24] = "Departure Staff";
			Format[6][25] = "<input type=\"text\" id=\"TBHandCarryDepartureTEL\" />";
			Format[6][26] = "Arrival Staff";
			Format[6][27] = "<input type=\"text\" id=\"TBHandCarryArrivalTEL\" />";
			Format[6][28] = "Shipper";
			Format[6][29] = "<input type=\"text\" id=\"TBShipper\" readonly=\"readonly\" onclick=\"SelectCompanyInDocument();\"/>";
			Format[6][30] = "Address";
			Format[6][31] = "<input type=\"text\" id=\"TBAddress\" />";

			if (form1.HCheckPostBack.value == "Y") {
				alert(form1.dhksfy.value);
				opener.location.reload();
				self.close();
				return false;
			}
			ACCOUNTID = opener.form1.HAccountID.value;
			BRANCHPK = opener.form1.HBranchPk.value;
		}
		function SelectTransportCL(value) {
			TRANSPORTCL = value;
			var Temp = "";
			var transport = parseInt(value);
			for (var i = 0; i < Format[transport].length; i++) {
				if (i % 4 == 0) {
					Temp += "<tr><td>" + Format[transport][i] + "</td>";
				}
				else if (i % 4 == 3) {
					Temp += "<td>" + Format[transport][i] + "</td></tr>";
				}
				else {
					Temp += "<td>" + Format[transport][i] + "</td>";
				}
			}
			document.getElementById("PnTransport").innerHTML = "<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" + Temp + "</table>";
			$("#TBDepartureDate").datepicker();
			$("#TBDepartureDate").datepicker("option", "dateFormat", "yymmdd");
			$("#TBArrivalDate").datepicker();
			$("#TBArrivalDate").datepicker("option", "dateFormat", "yymmdd");

		}
		function ShowModal() {
			var dialogArgument = new Array();
			
			dialogArgument[0] = TRANSPORTCL;
			dialogArgument[1] = BRANCHPK;
			var retVal = window.showModalDialog('SetTransportWayMemory.aspx', dialogArgument, 'dialogWidth=590px;dialogHeight=400px;resizable=1;status=0;scroll=1;help=0;');
			try {
				var ReturnArray = retVal.split('#@!');
				switch (TRANSPORTCL) {
					case "1":
						form1.HTransportBBCLPk.value = ReturnArray[0];
						form1.TBAirCompanyName.value = ReturnArray[1];
						form1.TBAirName.value = ReturnArray[2];
						form1.TBDepartureRegion.value = ReturnArray[3];
						form1.TBArrivalRegion.value = ReturnArray[4];
						form1.TBAirDepartureTEL.value = ReturnArray[5];
						form1.TBAirArrivalTEL.value = ReturnArray[6];
						break;
					case "3":
						form1.HTransportBBCLPk.value = ReturnArray[0];
						document.getElementById("TBShipCompanyName").value = ReturnArray[1];
						document.getElementById("TBShipName").value = ReturnArray[2];
						document.getElementById("TBDepartureRegion").value = ReturnArray[3];
						document.getElementById("TBArrivalRegion").value = ReturnArray[4];
						break;
					case "4":
						form1.HTransportBBCLPk.value = ReturnArray[0];
						document.getElementById("TBHandCarryCompanyName").value = ReturnArray[1];
						document.getElementById("TBDepartureRegion").value = ReturnArray[2];
						document.getElementById("TBArrivalRegion").value = ReturnArray[3];
						document.getElementById("TBHandCarryDepartureTEL").value = ReturnArray[4];
						document.getElementById("TBHandCarryArrivalTEL").value = ReturnArray[5];
						break;
					case "5"://FCL
						form1.HTransportBBCLPk.value = ReturnArray[0];
						document.getElementById("TBFCLTitle").value = ReturnArray[1];
						document.getElementById("TBShipCompanyName").value = ReturnArray[2];
						document.getElementById("TBShipName").value = ReturnArray[3];
						document.getElementById("TBDepartureRegion").value = ReturnArray[4];
						document.getElementById("TBArrivalRegion").value = ReturnArray[5];
						break;
					case "6":
						form1.HTransportBBCLPk.value = ReturnArray[0];
						document.getElementById("TBShipCompanyName").value=ReturnArray[1];
						document.getElementById("TBDepartureRegion").value=ReturnArray[2];
						document.getElementById("TBArrivalRegion").value=ReturnArray[3];
						document.getElementById("TBHandCarryDepartureTEL").value=ReturnArray[4];
						document.getElementById("TBHandCarryArrivalTEL").value = ReturnArray[5];
						break;
					case "7":
						document.getElementById("HTransportBBCLPk").value = ReturnArray[0];
						document.getElementById("TBCarCompanyName").value = ReturnArray[1];
						document.getElementById("TBDepartureRegion").value = ReturnArray[2];
						document.getElementById("TBArrivalRegion").value = ReturnArray[3];
						document.getElementById("TBDriverName").value = ReturnArray[4];
						document.getElementById("TBCarSize").value = ReturnArray[5];
						document.getElementById("TBDriverTEL").value = ReturnArray[6];
						break;
				}
			}
			catch (ex) { return false; }
		}
		function SelectCompanyInDocument() {								
			var retVal = window.showModalDialog('../../Request/Dialog/TransportShipperName.aspx?ID='+ACCOUNTID, '', 'dialogWidth=800px;dialogHeight=400px;resizable=1;status=0;scroll=1;help=0;');
			try {
				if (retVal == 'D') { }
				else {
					var ReturnArray = retVal.split('##');
					document.getElementById("TBShipper").value = ReturnArray[1];
					document.getElementById("TBAddress").value = ReturnArray[2];
				}
			}
			catch (err) { return false; }

		}
		
		function BtnSubmitClick() {
			Check = "1";
			var TransportBBCL = document.getElementById("HTransportBBCLPk").value;
			switch (TRANSPORTCL) {
				case "1":
					OnlyEnglishNum("TBDepartureRegion");
					OnlyEnglishNum("TBArrivalRegion");
					OnlyEnglishNum("TBAirName");
					break;
				case "3":
					OnlyEnglishNum("TBMasterBL");
					OnlyEnglishNum("TBDepartureRegion");
					OnlyEnglishNum("TBArrivalRegion");
					OnlyEnglishNum("TBShipName");
					break;
				case "4":
					OnlyEnglishNum("TBDepartureRegion");
					OnlyEnglishNum("TBArrivalRegion");
					break;
				case "5":
					OnlyEnglishNum("TBDepartureRegion");
					OnlyEnglishNum("TBArrivalRegion");
					OnlyEnglishNum("TBShipName");
					OnlyEnglishNum("TBFCLTitle");
					break;
				case "6":
					OnlyEnglishNum("TBDepartureRegion");
					OnlyEnglishNum("TBArrivalRegion");
					OnlyEnglishNum("TBShipName");
					break;
			}

			if (Check == "0") {
				return false;
			}

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
			var FromDateTime = document.getElementById("TBDepartureDate").value.substr(0, 4) + "." + document.getElementById("TBDepartureDate").value.substr(4, 2) + "." + document.getElementById("TBDepartureDate").value.substr(6, 2) + ". " + document.getElementById("TBDepartureHour").value + ":" + document.getElementById("TBDepartureMin").value;
			var ToDateTime = document.getElementById("TBArrivalDate").value.substr(0, 4) + "." + document.getElementById("TBArrivalDate").value.substr(4, 2) + "." + document.getElementById("TBArrivalDate").value.substr(6, 2) + ". " + document.getElementById("TBArrivalHour").value + ":" + document.getElementById("TBArrivalMin").value;
			//var InfoSum = new Array();
			var InfoSum = "";
			var BLNo = "";
			
			switch (TRANSPORTCL) {
				case "1":
					BLNo = document.getElementById("TBMasterBL").value;
					InfoSum = document.getElementById("TBAirCompanyName").value + "#@!" +
						document.getElementById("TBDepartureRegion").value + "#@!" +
						document.getElementById("TBArrivalRegion").value + "#@!" +
						document.getElementById("TBAirName").value + "#@!" +
						document.getElementById("TBAirDepartureTEL").value + "#@!" +
						document.getElementById("TBAirArrivalTEL").value;
					break;
				case "3":
					BLNo = document.getElementById("TBMasterBL").value;
					InfoSum = document.getElementById("TBShipCompanyName").value + "#@!" +
						document.getElementById("TBDepartureRegion").value + "#@!" +
						document.getElementById("TBArrivalRegion").value + "#@!" +
						document.getElementById("TBShipName").value + "#@!" +
						document.getElementById("TBContainerSize").value + "#@!" +
						document.getElementById("TBShippingTime").value + "#@!" +
						document.getElementById("TBContainerNo").value + "#@!" +
						document.getElementById("TBSealNo").value + "#@!" +
						document.getElementById("TBShipper").value + "#@!" +
						document.getElementById("TBAddress").value;
					break;
				case "4":
					BLNo = document.getElementById("TBMasterBL").value;
					InfoSum = document.getElementById("TBHandCarryCompanyName").value + "#@!" +
						document.getElementById("TBDepartureRegion").value + "#@!" +
						document.getElementById("TBArrivalRegion").value + "#@!" +
						document.getElementById("TBHandCarryDepartureTEL").value + "#@!" +
						document.getElementById("TBHandCarryArrivalTEL").value;
					break;
				case "5":
					BLNo = document.getElementById("TBMasterBL").value;
					InfoSum = document.getElementById("TBShipCompanyName").value + "#@!" +
						document.getElementById("TBDepartureRegion").value + "#@!" +
						document.getElementById("TBArrivalRegion").value + "#@!" +
						 document.getElementById("TBShipName").value + "#@!" +
						 document.getElementById("TBFCLTitle").value + "#@!" +
						document.getElementById("TBContainerSize").value + "#@!" +
						 document.getElementById("TBShippingTime").value + "#@!" +
						document.getElementById("TBContainerNo").value + "#@!" +
						document.getElementById("TBSealNo").value + "#@!" +
						document.getElementById("TBShipper").value + "#@!" +
						document.getElementById("TBAddress").value;
					break;
				case "6":
					BLNo = document.getElementById("TBMasterBL").value;
					InfoSum = document.getElementById("TBShipCompanyName").value + "#@!" +
						document.getElementById("TBDepartureRegion").value + "#@!" +
						document.getElementById("TBArrivalRegion").value + "#@!" +
						document.getElementById("TBShipName").value + "#@!" +
						document.getElementById("TBContainerSize").value + "#@!" +
						document.getElementById("TBContainerNo").value + "#@!" +
						document.getElementById("TBSealNo").value + "#@!" +
						document.getElementById("TBHandCarryDepartureTEL").value + "#@!" +
						document.getElementById("TBHandCarryArrivalTEL").value;
					break;
				case "7":
					TRANSPORTCL = "2";
					BLNo = document.getElementById("TBMasterBL").value;
					InfoSum = document.getElementById("TBCarCompanyName").value + "#@!" +
						document.getElementById("TBDepartureRegion").value + "#@!" +
						document.getElementById("TBArrivalRegion").value + "#@!" +
						document.getElementById("TBCarNo").value + "#@!" +
						document.getElementById("TBCarSize").value + "#@!" +
						document.getElementById("TBDriverTEL").value + "#@!" +
						document.getElementById("TBDriverName").value + "#@!" +
						document.getElementById("TBContainerSize").value + "#@!" +
						document.getElementById("TBContainerNo").value + "#@!" +
						document.getElementById("TBSealNo").value + "#@!";
					break;
			}

			if (form1.HIsModify.value == "Y") {
				Admin.UpdateTransportBBHead(BBHPK, TRANSPORTCL, BLNo, ToBranchPk, FromDateTime, ToDateTime, InfoSum, ACCOUNTID, function (result) {
					form1.HSavedPk.value = result;
					self.close();
					return false;
				}, function (result) { alert("ERROR : " + result); });
			}
			else {
				Admin.InsertTransportBBHead_(TRANSPORTCL, BLNo, BRANCHPK, ToBranchPk, FromDateTime, ToDateTime, InfoSum, ACCOUNTID, function (result) {
					if (result == "1") {
						opener.location.reload();
						self.close();
						return false;
					}
					else {
						form1.HDebug.value = result;
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
	</script>
    <link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />    
</head>
<body style="background-color:#999999; overflow-x:hidden; ">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
	<div style="width:570px;   padding-left:5px; padding-right:5px; padding-top:5px; background-color:white;">
		<input type="hidden" id="HDebug" onclick="select.this()" />
		<input type="hidden" id="HSavedPk" name="HSavedPk" />
		<input type="hidden" id="HCheckPostBack" value="<%=IsUpload %>" />
		<p>&nbsp;&nbsp;&nbsp;&nbsp;
			<select id="TransportCL" onchange="SelectTransportCL(this.value)">
				<option value="0"><%=GetGlobalResourceObject("qjsdur", "dnsthdqkdqjq") %></option>
				<option value="1"><%=GetGlobalResourceObject("qjsdur", "gkdrhd") %></option>
				<option value="7"><%=GetGlobalResourceObject("qjsdur", "ckfid") %></option>
				<option value="3"><%=GetGlobalResourceObject("qjsdur", "tjsqkr") %></option>
				<option value="4">hand carry</option>
				<option value="5">FCL</option>
				<option value="6">LCL</option>
			</select>
			<%=OurBranchSelect %>
		</p>
		<div id="PnTransport"></div> 
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