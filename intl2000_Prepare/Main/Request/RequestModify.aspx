<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeFile="RequestModify.aspx.cs" Inherits="Request_RequestModify" %>
<%@ Register src="../Member/LogedTopMenu.ascx" tagname="Loged" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script src="../Common/jquery-1.4.2.min.js" type="text/javascript"></script>
	<script src="../Common/jquery.cluetip.js" type="text/javascript"></script>
	<link href="../Common/jquery.cluetip.css" rel="stylesheet" type="text/css" />
	<script src="../Common/public.js" type="text/javascript"></script>
	<script src="../Common/RegionCodeForRequest.js?version=20131029" type="text/javascript"></script>
	<script type="text/javascript">		$(document).ready(function () { $('a.title').cluetip({ splitTitle: '|' }); });	</script>
	<script type="text/javascript">
		var BEFORE = new Array();
		var ITEMBEFORE = new Array();
		var ITEMPK = new Array();
		var COMPANYPK;
		var REQUESTFORMPK;
		var MODE;
		var DEPARTUREGUBUN;
		var ARRIVALREGIONGUBUN;
		window.onload = function () {
			var lis = document.getElementById('cssdropdown').getElementsByTagName('li');
			for (i = 0; i < lis.length; i++) {
				var li = lis[i];
				if (li.className == 'headlink') {
					li.onmouseover = function () { this.getElementsByTagName('ul').item(0).style.display = 'block'; document.getElementById("TopMenu").style.height = '130px'; }
					li.onmouseout = function () { this.getElementsByTagName('ul').item(0).style.display = 'none'; document.getElementById("TopMenu").style.height = '100%'; }
				}
			}
			var URI = location.href;
			SetMonetaryUnit("$");
			if (URI.indexOf("?") != -1) {
				var UriSpliteByQ = URI.split('?');
				var QueryString = "";
				if (UriSpliteByQ[1].toString().substr(0, 1) == "M") {
					// M=Mode, S=RequestFormPk, P=CompanyPk
					var EachQueryString = UriSpliteByQ[1].toString().split("&");
					MODE = EachQueryString[0].toString().substr(2);
					REQUESTFORMPK = EachQueryString[1].toString().substr(2);
					COMPANYPK = EachQueryString[2].toString().substr(2);
					Request.LoadDocuments(REQUESTFORMPK, MODE, COMPANYPK, function (result) {
						//alert(result);
						//						form1.DEBUG.value = result;
						//						return false;
						/*
						R.ShipperPk, R.ConsigneePk, R.AccountID, R.ConsigneeCCLPk, R.ShipperCode, 
						R.CompanyInDocumentPk, R.ConsigneeCode, R.DepartureDate, R.ArrivalDate, R.DepartureRegionCode, 
						R.ArrivalRegionCode, R.ShipperStaffName, R.ShipperStaffTEL, R.ShipperStaffMobile, R.TransportWayCL, 
						R.JubsuWayCL, R.PaymentWayCL, R.PaymentWhoCL, R.DocumentRequestCL, R.MonetaryUnitCL, 
						R.PickupRequestDate, R.NotifyPartyName, R.NotifyPartyAddress, R.Memo, R.RequestDate, 
						ShipperC.CompanyName AS ShipperCompanyName, ConsigneeC. CompanyName AS ConsigneeCompanyName, CCL.TargetCompanyName, DepartureRegion.Name AS DepartureName , ArrivalRegion.Name AS ArrivalName, 
						(SELECT count(*) FROM RequestFormAdditionalInfo where RequestFormPk=@RequestFormPk and (GubunCL=70 or GubunCL=71)) AS ModifyCount, R.StepCL, CID.Name AS CIDName, CID.Address AS CIDAddress
						*/
						//alert(result);
						var EachRow = result.split("####");
						//alert(EachRow[0]);
						BEFORE = EachRow[0].split("@@");
						form1.TB_DepartureDate.value = BEFORE[7];
						form1.TB_ArrivalDate.value = BEFORE[8];
						form1.TB_DepartureRegion.value = BEFORE[32];
						form1.TB_ArrivalRegion.value = BEFORE[33];
						DEPARTUREGUBUN = "Code";
						form1.gcodeExport.value = BEFORE[9];
						ARRIVALREGIONGUBUN = "Code";
						form1.gcodeImport.value = BEFORE[10];
						document.getElementById("TDCompanyName").innerHTML = BEFORE[25];
						if (BEFORE[5] == "0") { form1.CustomerClearanceCompanyName.value = form1.H_ClearanceSubstitute.value; }
						else { form1.CustomerClearanceCompanyName.value = BEFORE[36]; }

						document.getElementById("ItemStaffName[0]").value = BEFORE[11];
						if (BEFORE[12] != "") {
							var temp = BEFORE[12].toString().split('-');
							form1.ItemStaffTEL00.value = temp[0];
							form1.ItemStaffTEL01.value = temp[1];
							form1.ItemStaffTEL02.value = temp[2];
						}

						if (BEFORE[13] != "") {
							var temp = BEFORE[13].toString().split('-');
							form1.ItemStaffMobile00.value = temp[0];
							form1.ItemStaffMobile01.value = temp[1];
							form1.ItemStaffMobile02.value = temp[2];
						}
						var TransportWay = "";
						switch (BEFORE[14] + "") {
							case "27": TransportWay = "LCL"; break;
							case "28": TransportWay = "항공특송"; break;
							case "29": TransportWay = "항공운송"; break;
							case "30": TransportWay = "복합운송 3일"; break;
							case "31": TransportWay = "FCL"; break;
							case "32": TransportWay = "복합운송 4일"; break;
							case "33": TransportWay = "복합운송 5일"; break;
							case "35": TransportWay = "복합운송 7일"; break;
							case "36": TransportWay = "복합운송 2일"; break;
							case "37": TransportWay = "복합운송 6일"; break;
							case "38": TransportWay = "복합운송 항공"; break;
							case "39": TransportWay = "복합운송 차량"; break;
							case "40": TransportWay = "내륙 항공운송"; break;
							case "41": TransportWay = "내륙 해상운송"; break;
							//case "42": TransportWay = "내륙 항공운송"; break;
							case "43": TransportWay = "내륙 육로운송"; break;
							default: TransportWay = form1.alwlwjd.value; break;
						}
						document.getElementById("BeforeTransportWay").innerHTML = "&nbsp; " + TransportWay + "&nbsp;&nbsp;&nbsp;&nbsp;";
						switch (BEFORE[15]) {
							case "1": document.getElementById("ItemTransWay_Pickup[0]").checked = true;
								Click_ItemTransWay(0, "ItemTransWay_Pickup[0]");
								if (BEFORE[20] != "") {
									document.getElementById("TB_PickupRequestDate0").value = BEFORE[20].substr(0, 6);
									form1.PickupRequestTimeH.value = BEFORE[20].substr(6, 2);
									form1.PickupRequestTimeM.value = BEFORE[20].substr(8, 2);
								}
								break;
							case "2": document.getElementById("ItemTransWay_Direct[0]").checked = true; break;
							case "3": document.getElementById("ItemTransWay_ETC").checked = true;
						}
						/*
						form1.TB_CustomerListPk.value = BEFORE[3];
						form1.HConsigneeCode.value = BEFORE[6];
						form1.HConsigneePk.value = BEFORE[1];
						*/
						form1.TB_ImportCompanyName.value = BEFORE[27];
						if (BEFORE[21] != "") {
							form1.TBSameAsAbove.style.width = "1px";
							document.getElementById("PnSameAsAbove").style.visibility = "hidden";
							document.getElementById("PnSameAsAbove").style.width = "5px";
							document.getElementById("PnSetNotifyParty").style.visibility = "Visible";
							switch (BEFORE[16]) {
								case "3": form1.paymenyLC.checked = true; break;
								case "4": form1.paymenyTT.checked = true; break;
							}
							form1.TB_NotifyName.value = BEFORE[21];
							form1.TB_NotifyAddress.value = BEFORE[22];
						}
						if (BEFORE[18] != "") {
							var temp = BEFORE[18].split('!');
							for (var i = 0; i < temp.length; i++) {
								switch (temp[i]) {
									case "10": document.getElementById('CertificateOfOrigin').checked = true; break;
									case "11": document.getElementById('FoodSanitation').checked = true; break;
									case "12": document.getElementById('ElectricSafety').checked = true; break;
									case "24": document.getElementById('ProductCheked').checked = true; break;
									case "25": document.getElementById('SuChec').checked = true; break;
								}
							}
						}
						switch (BEFORE[17]) {
							case "5": form1.EXW.checked = true; break;
							case "6": form1.DDP.checked = true; break;
							case "7": form1.CNF.checked = true; break;
							case "8": form1.FOB.checked = true; break;
							case "9": form1.ETC.checked = true; break;
						}
						if (BEFORE[23] != "") {
							form1.TB_ItemTransWay_ETC.value = BEFORE[23].substr(0, BEFORE[23].indexOf("$$$"));
							form1.ETCTextarea.value = BEFORE[23].substr(BEFORE[23].indexOf("$$$") + 3, BEFORE[23].indexOf("^^^") - BEFORE[23].indexOf("$$$") - 3);
							form1.TextareaETC.value = BEFORE[23].substr(BEFORE[23].indexOf("^^^") + 3, BEFORE[23].Length - BEFORE[23].indexOf("^^^") - 3);
						}
						switch (BEFORE[24]) {
							case '18': SetMonetaryUnit("￥"); break;
							case '19': SetMonetaryUnit("$"); break;
							case '20': SetMonetaryUnit("￦"); break;
							case '21': SetMonetaryUnit("Y"); break;
							case '22': SetMonetaryUnit("$"); break;
							case '23': SetMonetaryUnit("€"); break;
						}
						for (var i = 1; i < EachRow.length; i++) {
							//	0		[RequestFormItemsPk], [RequestFormPk], [ItemCode], [MarkNNumber], [Description]
							//5		,[Label], [Material], [Quantity], [QuantityUnit], [PackedCount]
							//10		, [PackingUnit], [GrossWeight], [Volume], [UnitPrice], [Amount]
							//15		,[LastModify]
							Each = EachRow[i].split("!");
							//alert(EachRow[i]);
							if (i != 1) { InsertRow('0'); }
							if (Each[3] != "") { document.getElementById("Item[0][" + (i - 1) + "][boxNo]").value = Each[3]; }
							if (Each[4] != "") { document.getElementById("Item[0][" + (i - 1) + "][ItemName]").value = Each[4]; }
							if (Each[5] != "") { document.getElementById("Item[0][" + (i - 1) + "][Brand]").value = Each[5]; }
							if (Each[6] != "") { document.getElementById("Item[0][" + (i - 1) + "][Matarial]").value = Each[6]; }
							if (Each[7] != "") { document.getElementById("Item[0][" + (i - 1) + "][Quantity]").value = NumberFormat(Each[7]); }
							if (Each[8] != "") { document.getElementById("Item[0][" + (i - 1) + "][QuantityUnit]").value = Each[8]; }
							if (Each[9] != "") { document.getElementById("Item[0][" + (i - 1) + "][PackedCount]").value = NumberFormat(Each[9]); }
							if (Each[10] != "") { document.getElementById("Item[0][" + (i - 1) + "][PackingUnit]").value = Each[10]; }
							if (Each[13] != "") { document.getElementById("Item[0][" + (i - 1) + "][UnitCost]").value = NumberFormat(Each[13]); }
							if (Each[14] != "") { document.getElementById("Item[0][" + (i - 1) + "][Price]").value = NumberFormat(Each[14]); }
							if (Each[11] != "") { document.getElementById("Item[0][" + (i - 1) + "][Weight]").value = NumberFormat(Each[11]); }
							if (Each[12] != "") { document.getElementById("Item[0][" + (i - 1) + "][Volume]").value = NumberFormat(Each[12]); }
							ITEMPK[i - 1] = Each[0];
							ITEMBEFORE[i - 1] = document.getElementById("Item[0][" + (i - 1) + "][boxNo]").value + "#@!" +
														document.getElementById("Item[0][" + (i - 1) + "][ItemName]").value + "#@!" +
														document.getElementById("Item[0][" + (i - 1) + "][Brand]").value + "#@!" +
														document.getElementById("Item[0][" + (i - 1) + "][Matarial]").value + "#@!" +
														document.getElementById("Item[0][" + (i - 1) + "][Quantity]").value + "#@!" +
														document.getElementById("Item[0][" + (i - 1) + "][QuantityUnit]").value + "#@!" +
														document.getElementById("Item[0][" + (i - 1) + "][PackedCount]").value + "#@!" +
														document.getElementById("Item[0][" + (i - 1) + "][PackingUnit]").value + "#@!" +
														document.getElementById("Item[0][" + (i - 1) + "][UnitCost]").value + "#@!" +
														document.getElementById("Item[0][" + (i - 1) + "][Price]").value + "#@!" +
														document.getElementById("Item[0][" + (i - 1) + "][Weight]").value + "#@!" +
														document.getElementById("Item[0][" + (i - 1) + "][Volume]").value;
						}
						GetTotal("Price");
						GetTotal("PackedCount");
						SetUnsongWayByGcode();
					},
					function (result) { alert(result); });
				}
			}
		}
		var DateSpan = 0;
		function delopt(selectCtrl, mTxt) {
			if (selectCtrl.options) {
				for (i = selectCtrl.options.length; i >= 0; i--) { selectCtrl.options[i] = null; }
			}
			selectCtrl.options[0] = new Option(mTxt, "")
			selectCtrl.selectedIndex = 0;
		}
		function NumberFormat(number) { if (number == "" || number == "0") { return number; } else { return parseInt(number * 10000) / 10000; } }
		function GetTotal(Which) {
			var RowLength = document.getElementById('ItemTable[0]').rows.length - 2;
			var MaxCountUnder1 = 0;
			for (i = 0; i < RowLength; i++) {
				var ThisValue = document.getElementById('Item[0][' + i + '][' + Which + ']').value.toString();
				if (ThisValue.indexOf(".") != -1 && ThisValue.length - ThisValue.indexOf(".") - 1 > MaxCountUnder1) {
					MaxCountUnder1 = ThisValue.length - ThisValue.indexOf(".") - 1;
				}
			}
			var TotalValue = 0;
			var ToInt = Math.pow(10, MaxCountUnder1);
			for (i = 0; i < RowLength; i++) {
				var ThisValue = document.getElementById('Item[0][' + i + '][' + Which + ']').value.toString().replace(/,/g, '');
				if (ThisValue != "") {
					TotalValue += parseInt(parseFloat(ThisValue) * ToInt);
					//TotalValue += parseFloat(document.getElementById('Item[0][' + i + '][' + Which + ']').value.toString().replace(',', ''));
				}
			}
			var FloatTotalValue = parseInt(TotalValue + 0.00000001) / ToInt;
			if (TotalValue != 0) { document.getElementById('total[0][' + Which + ']').value = FloatTotalValue; }
		}
		function InsertRow(rowC) {		// 상품추가
			var objTable = document.getElementById('ItemTable[' + rowC + ']');
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
			var objCell8 = objRow1.insertCell();
			var objCell9 = objRow1.insertCell();
			var objCell10 = objRow1.insertCell();

			var MonetaryUnit;
			switch (document.getElementById('Item[0][MonetaryUnit]')[document.getElementById("Item[0][MonetaryUnit]").selectedIndex].value) {
				case '18': MonetaryUnit = "￥"; break;
				case '19': MonetaryUnit = "$"; break;
				case '20': MonetaryUnit = "￦"; break;
				case '21': MonetaryUnit = "Y"; break;
				case '22': MonetaryUnit = "$"; break;
				case '23': MonetaryUnit = "€"; break;
				default: MonetaryUnit = "$"; break;
			}
			objCell1.align = "center";
			objCell2.align = "center";
			objCell3.align = "center";
			objCell4.align = "center";
			objCell5.align = "center";
			objCell6.align = "center";
			objCell7.align = "center";
			objCell8.align = "center";
			objCell9.align = "center";
			objCell10.align = "center";

			objCell1.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][boxNo]' onkeydown=\"MoveNextByEnter('Item[" + rowC + "][" + thisLineNo + "][ItemName]')\" onkeyup='CountBox(" + rowC + "," + thisLineNo + ",this)' size='6' />";
			objCell2.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][ItemName]' onkeydown=\"MoveNextByEnter('Item[" + rowC + "][" + thisLineNo + "][Brand]')\" size='25' />";
			objCell3.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Brand]' onkeydown=\"MoveNextByEnter('Item[" + rowC + "][" + thisLineNo + "][Matarial]')\" size='8'  />";
			objCell4.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Matarial]' onkeydown=\"MoveNextByEnter('Item[" + rowC + "][" + thisLineNo + "][Quantity]')\" size='6' />";
			objCell5.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Quantity]' onkeydown=\"MoveNextByEnter('Item[" + rowC + "][" + thisLineNo + "][UnitCost]')\"  onkeyup=\"QuantityXUnitCost(" + rowC + "," + thisLineNo + "); GetTotal('Price');\"  size='5' /><select id='Item[" + rowC + "][" + thisLineNo + "][QuantityUnit]'><option value='40'>PCS</option><option value='41'>PRS</option><option value='42'>SET</option><option value='43'>S/F</option><option value='44'>YDS</option><option value='45'>M</option><option value='46'>KG</option><option value='47'>DZ</option><option value='48'>L</option><option value='49'>Box</option><option value='50'>SQM</option><option value='51'>M2</option><option value='52'>RO</option></select>";
			objCell6.innerHTML = "<input type='text' style='border:0; width:10px;' id='Item[" + rowC + "][" + thisLineNo + "][MonetaryUnit][0]' value='" + MonetaryUnit + "' /><input type='text' id='Item[" + rowC + "][" + thisLineNo + "][UnitCost]' onkeyup=\"QuantityXUnitCost(" + rowC + "," + thisLineNo + "); GetTotal('Price');\" onkeydown=\"MoveNextByEnter('Item[" + rowC + "][" + thisLineNo + "][PackedCount]')\"  size='5' />";
			objCell7.innerHTML = "<input type='text' style='border:0; width:10px;' id='Item[" + rowC + "][" + thisLineNo + "][MonetaryUnit][1]'  value='" + MonetaryUnit + "' /><input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Price]' readonly='readonly' size='9' />";
			objCell8.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][PackedCount]' onkeydown=\"MoveNextByEnter('Item[" + rowC + "][" + thisLineNo + "][Weight]')\" onkeyup=\"BoxTotalCount('0');\" size='6' /> <select id='Item[" + rowC + "][" + thisLineNo + "][PackingUnit]' size='1'><option value='15'>CT</option><option value='16'>RO</option><option value='17'>PA</option></select>";
			objCell9.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Weight]' size='5' onkeydown=\"MoveNextByEnter('Item[" + rowC + "][" + thisLineNo + "][Volume]')\"  />";
			objCell10.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Volume]' size='5' onkeydown=\"MoveNextByEnterAndAddRow('Item[" + rowC + "][" + (thisLineNo + 1) + "][boxNo]');\"  />";
		}

		function SetMonetaryUnit(value) {
			var i;
			var RowLength = document.getElementById('ItemTable[0]').rows.length - 2;
			var totalCount = 0;
			for (i = 0; i < RowLength; i++) {
				document.getElementById('Item[0][' + i + '][MonetaryUnit][0]').value = value;
				document.getElementById('Item[0][' + i + '][MonetaryUnit][1]').value = value;
			}
			document.getElementById('Item[0][MonetaryUnit][Total]').value = value;
		}
		var ISMODIFYDEPARTUREREGION = false;
		var ISMODIFYARRIVALREGION = false;
		var ISMODIFYTRANSPORTWAY = false;
		function Show(which) {
			document.getElementById(which).style.visibility = "visible";
			switch (which) {
				case "PnDepartureRegion": ISMODIFYDEPARTUREREGION = true; break;
				case "PnArrivalRegion": ISMODIFYARRIVALREGION = true; break;
				case "PnTransportWay":
					ISMODIFYTRANSPORTWAY = true;
					SetUnsongWayByGcode();
					break;
			}
		}

		function PopShipperInDocument() {	//신규고객 등록
			var DialogResult = 'TB_ImportStaffTEL0';
			var retVal = window.showModalDialog('./Dialog/ShipperNameSelection.aspx?S=' + document.getElementById("TB_pk").value.trim(), DialogResult, 'dialogWidth=700px;dialogHeight=400px;resizable=0;status=0;scroll=1;help=0;');
			try {
				if (retVal == '1') {
					form1.CustomerClearancePk.value = "0";
					form1.CustomerClearanceCompanyName.value = form1.H_ClearanceSubstitute.value;
				}
				else if (retVal == 'D') { PopShipperInDocument(); }
				else {
					var ReturnArray = retVal.split('##');
					form1.CustomerClearancePk.value = ReturnArray[0];
					form1.CustomerClearanceCompanyName.value = ReturnArray[1];
					form1.CustomerClearanceAddress.value = ReturnArray[2];
				}
			}
			catch (err) { return false; }
		}

		function Click_ItemTransWay(f, j) {	//접수방법 선택하기 : 직접배송 / 픽업요청
			if (j == 'ItemTransWay_Direct[' + f + ']') { document.getElementById("Pn_Pickup[" + f + "]").style.visibility = 'hidden'; }
			else if (j == 'ItemTransWay_ETC') {
				//document.getElementById("Pn_Direct[" + f + "]").style.visibility = 'hidden';
				document.getElementById("Pn_Pickup[" + f + "]").style.visibility = 'hidden';
				document.getElementById("TB_ItemTransWay_ETC").focus();
			}
			else { document.getElementById("Pn_Pickup[" + f + "]").style.visibility = 'visible'; }
		}
		function SelectChangeMonetaryUnit(idx) {
			switch (document.getElementById('Item[0][MonetaryUnit]')[idx].value) {
				case '18': SetMonetaryUnit("￥"); break;
				case '19': SetMonetaryUnit("$"); break;
				case '20': SetMonetaryUnit("￦"); break;
				case '21': SetMonetaryUnit("Y"); break;
				case '22': SetMonetaryUnit("$"); break;
				case '23': SetMonetaryUnit("€"); break;
			}
		}
		function SetMonetaryUnit(value) {
			var i;
			var RowLength = document.getElementById('ItemTable[0]').rows.length - 2;
			var totalCount = 0;
			for (i = 0; i < RowLength; i++) {
				document.getElementById('Item[0][' + i + '][MonetaryUnit][0]').value = value;
				document.getElementById('Item[0][' + i + '][MonetaryUnit][1]').value = value;
			}
			document.getElementById('Item[0][MonetaryUnit][Total]').value = value;
		}
		function cate1change(first, second, third, obj, idx, TB_RegionPk) {
			delopt(document.getElementById(second), ":: " + form1.HArrivalLocal1.value + " ::");
			delopt(document.getElementById(third), ":: " + form1.HArrivalLocal2.value + " ::");
			//if (idx == 6) {
			//	document.getElementById(third).style.visibility = "hidden";
			//	document.getElementById(second).style.visibility = "hidden";
			//}
			if (idx == 1 || idx == 4 || idx == 5 || idx == 6) { document.getElementById(third).style.visibility = "hidden"; } // 한국선택시
			else { document.getElementById(third).style.visibility = "visible"; } // 중국선택시

			if (cate2[idx]) { for (i = 1; i < cate2[idx].length; i++) { document.getElementById(second).options[i] = new Option(cate2[idx][i].value, cate2[idx][i].text); } }
			document.getElementById(TB_RegionPk).value = document.getElementById(first).options[idx].value;
			if (TB_RegionPk == "gcodeExport") { DEPARTUREGUBUN = "Pk"; }
			else if (TB_RegionPk == "gcodeImport") { ARRIVALREGIONGUBUN = "Pk"; }			
		}
		function SetTransportWayCL(idx) {
			document.getElementById("UnsongWayCode").value = idx;
			switch (idx) {
				case '31': document.getElementById("FCLOnly").style.visibility = "visible"; break;
				default: document.getElementById("FCLOnly").style.visibility = "hidden"; break;
			}
		}
		function SetUnsongWayByGcode() {
			var from = document.getElementById("gcodeExport").value;
			var to = document.getElementById("gcodeImport").value;
			var days = form1.H_Days.value;
			if (from != '' && to != '') {
				Request.LoadTransportWayCL2(DEPARTUREGUBUN, from, ARRIVALREGIONGUBUN, to, function (result) {
					var resultSplit = result.split("@@");
					var ResultHtml = "";
					var FirstLine = "";
					var SecondLine = "";
					var ThirdLine = "";
					var FourthLine = "";
					var Temp;
					for (var i = 0; i < resultSplit.length - 1; i++) {
						var EachValue = resultSplit[i].split("!");
						Temp = "<input type=\"radio\" name=\"TransportWayCL\" id=\"TransportWayCL" + EachValue[0] + "\" onclick=\"SetTransportWayCL(this.value)\" value=\"" + EachValue[0] + "\" /><label for=\"TransportWayCL" + EachValue[0] + "\">" + EachValue[1].toString() + "</label>&nbsp;&nbsp;&nbsp;&nbsp;";
						switch (EachValue[2].substr(0, 1)) {
							case "1": FirstLine += Temp; break;
							case "2": SecondLine += Temp; break;
							case "3": ThirdLine += Temp; break;
							case "4": FourthLine += Temp; break;
						}
					}
					if (FirstLine != "") { FirstLine = "<strong>항공 </strong> " + FirstLine.replace(/항공 /g, "") + "<br />"; }
					if (SecondLine != "") { SecondLine = "<strong>복합운송 </strong> " + SecondLine.replace(/복합운송 /g, "") + "<br />"; }
					if (ThirdLine != "") { ThirdLine = "<strong>해상 </strong> " + ThirdLine.replace(/국제해상 /g, "") + "<br />"; }
					if (FourthLine != "") { FourthLine = FourthLine + "<br />"; }
					ResultHtml = FirstLine + SecondLine + ThirdLine + FourthLine;
					if (ResultHtml != "") {
						ResultHtml = "<br />" + ResultHtml.substr(0, ResultHtml.length - 6);
					}
					document.getElementById("PnTransportWay").innerHTML = ResultHtml;
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function cate2change(first, second, third, obj, idx, TB_RegionPk) {
			delopt(document.getElementById(third), ":: " + form1.HArrivalLocal2.value + " ::");
			cate1 = document.getElementById(first).selectedIndex;
			if (document.getElementById(first).selectedIndex == 2 || document.getElementById(first).selectedIndex == 3) {
				if (cate3[cate1][idx]) {
					for (i = 1; i < cate3[cate1][idx].length; i++) {
						document.getElementById(third).options[i] = new Option(cate3[cate1][idx][i].value, cate3[cate1][idx][i].text);
					}
				}
			}
			document.getElementById(TB_RegionPk).value = document.getElementById(second).options[idx].value;
			//    		Request.FindOurOffice(document.getElementById(TB_RegionPk).value, SetOurBranch);
			if (TB_RegionPk == "gcodeExport") { DEPARTUREGUBUN = "Pk"; }
			else if (TB_RegionPk == "gcodeImport") { ARRIVALREGIONGUBUN = "Pk"; }
			SetUnsongWayByGcode();
		}
		function cate3change(first, second, third, obj, idx, TB_RegionPk) {
			document.getElementById(TB_RegionPk).value = document.getElementById(third).options[idx].value;
			if (TB_RegionPk == "gcodeExport") { DEPARTUREGUBUN = "Pk"; }
			else if (TB_RegionPk == "gcodeImport") { ARRIVALREGIONGUBUN = "Pk"; }
			SetUnsongWayByGcode();
		}
		function openModal(obj, file_name, width, height) {//날짜 입력 폼 띄우기
			height = height + 20;
			var rand = Math.random() * 4;
			window.showModalDialog('../Common/' + file_name + '?rand=' + rand, obj, 'dialogWidth=' + width + 'px;dialogHeight=' + height + 'px;resizable=0;status=0;scroll=0;help=0');
			CountTransportationDate();
		}
		function PopupCustomerListAdd() {	//신규고객 등록
			var DialogResult = 'TB_ImportStaffTEL0';
			var retVal = window.showModalDialog('./OwnCustomerListAdd.aspx?pk=' + document.getElementById("TB_pk").value.trim() + '&id=' + document.getElementById('TB_AccountID').value.trim() + '&g=N', DialogResult, 'dialogWidth=400px;dialogHeight=400px;resizable=1;status=0;scroll=0;help=0;');
			try {
				var ReturnArray = retVal.split('##');
				document.getElementById('TB_ImportCompanyName').value = ReturnArray[0] + ' (' + ReturnArray[1] + ')';
				document.getElementById('TB_CustomerListPk').value = ReturnArray[2];
			}
			catch (err) { return false; }
		}
		function PopupCustomerList() {	//수화인 선택
			var DialogResult = 'TB_ImportStaffTEL0';
			var retVal = window.showModalDialog('./OwnCustomerList.aspx?S=' + document.getElementById("TB_pk").value.trim(), DialogResult, 'dialogWidth=500px;dialogHeight=500px;resizable=0;status=0;scroll=0;help=0;');
			try {
				var ReturnArray = retVal.split('##');
				document.getElementById('TB_ImportCompanyName').value = ReturnArray[0].replace("$$$", " ").replace("$$$", " ").replace("$$$", " ") + ' (' + ReturnArray[1] + ')';
				document.getElementById('TB_CustomerListPk').value = ReturnArray[2];
				if (ReturnArray[3] != "") {
					form1.HConsigneePk.value = ReturnArray[3];
					form1.HConsigneeCode.value = ReturnArray[1];
				}
				else {
					form1.HConsigneePk.value = "N";
					form1.HConsigneeCode.value = "N";
				}
			}
			catch (ex)
			{ }
		}
		function moveNext(from, to, length) { if (from.value.length == length) { to.focus(); } }
		function CountBox(GroupNo, LineNo, obj) {
			var reg = /[0-9]-[0-9]/;
			var reg3 = /[0-9]~[0-9]/;
			var reg2 = /[0-9]/;
			if (reg2.test(obj.value) == true) {
				document.getElementById('Item[' + GroupNo + '][' + LineNo + '][PackedCount]').value = 1;
				BoxTotalCount(GroupNo);
			}
			if (reg.test(obj.value) == true) {
				var SplitResult = obj.value.split('-');
				var result = SplitResult[1] - SplitResult[0];
				if (result < 0) { result = SplitResult[0] - SplitResult[1]; }
				document.getElementById('Item[' + GroupNo + '][' + LineNo + '][PackedCount]').value = result + 1;
				BoxTotalCount(GroupNo);
			}
			if (reg3.test(obj.value) == true) {
				var SplitResult = obj.value.split('~');
				var result = SplitResult[1] - SplitResult[0];
				if (result < 0) { result = SplitResult[0] - SplitResult[1]; }
				document.getElementById('Item[' + GroupNo + '][' + LineNo + '][PackedCount]').value = result + 1;
				BoxTotalCount(GroupNo);
			}
		}
		function BoxTotalCount(GroupNo) {
			var RowLength = document.getElementById('ItemTable[' + GroupNo + ']').rows.length - 2;
			var totalCount = 0;
			for (i = 0; i < RowLength; i++) {
				if (document.getElementById('Item[' + GroupNo + '][' + i + '][PackedCount]').value != "") {
					totalCount += parseInt(document.getElementById('Item[' + GroupNo + '][' + i + '][PackedCount]').value);
				}
			}
			document.getElementById('total[' + GroupNo + '][PackedCount]').value = totalCount;
		}
		function QuantityXUnitCost(GroupNo, LineNo) {
			if (document.getElementById('Item[' + GroupNo + '][' + LineNo + '][Quantity]').value != "" && document.getElementById('Item[' + GroupNo + '][' + LineNo + '][UnitCost]').value != "") {
				Request.QuantityXUnitCost(document.getElementById('Item[' + GroupNo + '][' + LineNo + '][Quantity]').value, document.getElementById('Item[' + GroupNo + '][' + LineNo + '][UnitCost]').value, function (result) {
					document.getElementById('Item[' + GroupNo + '][' + LineNo + '][Price]').value = result;
					GetTotal("Quantity");
					GetTotal("Price");
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		
		function Btn_Submit_Click() {
			form1.BTN_SUBMIT.style.visibility = "hidden";
			/*	R.ShipperPk, R.ConsigneePk, R.AccountID, R.ConsigneeCCLPk, R.ShipperCode, 
			R.CompanyInDocumentPk, R.ConsigneeCode, R.DepartureDate, R.ArrivalDate, R.DepartureRegionCode, 
			R.ArrivalRegionCode, R.ShipperStaffName, R.ShipperStaffTEL, R.ShipperStaffMobile, R.TransportWayCL, 
			R.JubsuWayCL, R.PaymentWayCL, R.PaymentWhoCL, R.DocumentRequestCL, R.MonetaryUnitCL, 
			R.PickupRequestDate, R.NotifyPartyName, R.NotifyPartyAddress, R.Memo, R.RequestDate, 
			ShipperCompanyName, ConsigneeCompanyName, CCL.TargetCompanyName, DepartureRegion.Name AS DepartureName , ArrivalRegion.Name AS ArrivalName, 
			ModifyCount, R.StepCL, CIDName, CIDAddress
			*/
			var ShipperPk = BEFORE[0];
			var CompanyInDocumentPk = form1.CustomerClearancePk.value;
			var DepartureDate = ""; if (BEFORE[7] != form1.TB_DepartureDate.value) { DepartureDate = form1.TB_DepartureDate.value; }
			var ArrivalDate = ""; if (BEFORE[8] != form1.TB_ArrivalDate.value) { ArrivalDate = form1.TB_ArrivalDate.value; }

			var DepartureRegionCode = "";
			if (ISMODIFYDEPARTUREREGION && DEPARTUREGUBUN == "Pk") { DepartureRegionCode = form1.gcodeExport.value; }
			var ArrivalRegionCode = "";
			if (ISMODIFYARRIVALREGION && ARRIVALREGIONGUBUN == "Pk") { ArrivalRegionCode = form1.gcodeImport.value; }

			var TransportWayCL = "";
			if (ISMODIFYTRANSPORTWAY) { TransportWayCL = form1.UnsongWayCode.value; }


//			var DepartureRegionCode = ""; if (form1.gcodeExport.value != "" && BEFORE[9] != form1.gcodeExport.value) {
//				DepartureRegionCode = form1.gcodeExport.value 
//			}
//			var ArrivalRegionCode = ""; if (form1.gcodeImport.value != "" && BEFORE[10] != form1.gcodeExport.value) {
//				DepartureRegionCode = form1.gcodeExport.value 
			//			}

			var ShipperStaffName = ""; if (BEFORE[11] != document.getElementById("ItemStaffName[0]").value) { ShipperStaffName = document.getElementById("ItemStaffName[0]").value; }
			var ShipperStaffTEL = "";
			if (BEFORE[12] != form1.ItemStaffTEL00.value + "-" + form1.ItemStaffTEL01.value + "-" + form1.ItemStaffTEL02.value) {
				ShipperStaffTEL = form1.ItemStaffTEL00.value + "-" + form1.ItemStaffTEL01.value + "-" + form1.ItemStaffTEL02.value;
				if (ShipperStaffTEL == "--") { ShipperStaffTEL = ""; }
			}
			var ShipperStaffMobile = "";
			if (BEFORE[13] != form1.ItemStaffMobile00.value + "-" + form1.ItemStaffMobile01.value + "-" + form1.ItemStaffMobile02.value) {
				ShipperStaffMobile = form1.ItemStaffMobile00.value + "-" + form1.ItemStaffMobile01.value + "-" + form1.ItemStaffMobile02.value;
				if (ShipperStaffMobile == "--") { ShipperStaffMobile = ""; }
			}
			var temp = "";
			if (document.getElementById("ItemTransWay_Direct[0]").checked) { temp = "2"; }
			else if (document.getElementById("ItemTransWay_Pickup[0]").checked) { temp = "1" }
			else if (document.getElementById("ItemTransWay_ETC").checked) { temp = "3"; }
			var JubsuWayCL = ""; if (temp != BEFORE[15]) { JubsuWayCL = temp; }
			temp = "4"; if (form1.IsNotifyParty.value == "Y" && form1.paymenyLC.checked) { temp = "3"; }
			var PaymentWayCL = ""; if (temp != BEFORE[16]) { PaymentWayCL = temp; }
			var PickupRequestDate = "";
			if (form1.TB_PickupRequestDate0.value + form1.PickupRequestTimeH.value + form1.PickupRequestTimeM.value != BEFORE[20]) {
				PickupRequestDate = form1.TB_PickupRequestDate0.value + form1.PickupRequestTimeH.value + form1.PickupRequestTimeM.value;
			}
			temp = "";
			if (document.form1.Payment3[0].checked) { temp = "5"; }
			else if (document.form1.Payment3[1].checked) { temp = "6"; }
			else if (document.form1.Payment3[2].checked) { temp = "7"; }
			else if (document.form1.Payment3[3].checked) { temp = "8"; }
			else if (document.form1.Payment3[4].checked) { temp = "9"; }
			var PaymentWhoCL = ""; if (temp != BEFORE[17]) { PaymentWhoCL = temp; }
			temp = "";
			if (form1.CertificateOfOrigin.checked) { temp += "10!" }
			if (form1.FoodSanitation.checked) { temp += "11!"; }
			if (form1.ElectricSafety.checked) { temp += "12!"; }
			if (form1.ProductCheked.checked) { temp += "24!"; }
			if (form1.SuChec.checked) { temp += "25!"; }
			var DocumentRequestCL = ""; if (temp != BEFORE[18]) { DocumentRequestCL = temp; }
			var MonetaryUnitCL = "";if (document.getElementById("Item[0][MonetaryUnit]").value != BEFORE[24] && document.getElementById("Item[0][MonetaryUnit]").value != "0") {
				MonetaryUnitCL = document.getElementById("Item[0][MonetaryUnit]").value;
			}
			var NotifyPartyName = ""; if (form1.TB_NotifyName.value != BEFORE[21]) { NotifyPartyName = form1.TB_NotifyName.value; }
			var NotifyPartyAddress = ""; if (form1.TB_NotifyAddress.value != BEFORE[22]) { NotifyPartyAddress = form1.TB_NotifyAddress.value; }
			var Memo = ""; if (form1.TB_ItemTransWay_ETC.value + "$$$" + form1.ETCTextarea.value + "^^^" + form1.TextareaETC.value != BEFORE[23]) {
				Memo = form1.TB_ItemTransWay_ETC.value + "$$$" + form1.ETCTextarea.value + "^^^" + form1.TextareaETC.value;
				if (Memo == "$$$^^^") { Memo = ""; }
			}
			var ItemModifySUM = "";
			var RowLength0 = document.getElementById('ItemTable[0]').rows.length - 2;
			for (var i = 0; i < RowLength0; i++) {
				if (ITEMPK[i] != undefined) {
					if (ITEMBEFORE[i] != document.getElementById("Item[0][" + i + "][boxNo]").value + "#@!" + document.getElementById("Item[0][" + i + "][ItemName]").value + "#@!" + document.getElementById("Item[0][" + i + "][Brand]").value + "#@!" + document.getElementById("Item[0][" + i + "][Matarial]").value + "#@!" + document.getElementById("Item[0][" + i + "][Quantity]").value + "#@!" + document.getElementById("Item[0][" + i + "][QuantityUnit]").value + "#@!" + document.getElementById("Item[0][" + i + "][PackedCount]").value + "#@!" + document.getElementById("Item[0][" + i + "][PackingUnit]").value + "#@!" + document.getElementById("Item[0][" + i + "][UnitCost]").value + "#@!" + document.getElementById("Item[0][" + i + "][Price]").value + "#@!" + document.getElementById("Item[0][" + i + "][Weight]").value + "#@!" + document.getElementById("Item[0][" + i + "][Volume]").value) {
						ItemModifySUM += ITEMPK[i] + "#@!" +
													document.getElementById("Item[0][" + i + "][boxNo]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][ItemName]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][Brand]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][Matarial]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][Quantity]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][QuantityUnit]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][PackedCount]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][PackingUnit]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][Weight]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][Volume]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][UnitCost]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][Price]").value + "%!$@#";
						continue;
					}
					else { continue; }
				}
				else if (document.getElementById("Item[0][" + i + "][boxNo]").value == "" && document.getElementById("Item[0][" + i + "][ItemName]").value == "" && document.getElementById("Item[0][" + i + "][Brand]").value == "" && document.getElementById("Item[0][" + i + "][Matarial]").value == "" && document.getElementById("Item[0][" + i + "][Quantity]").value == "" && document.getElementById("Item[0][" + i + "][UnitCost]").value == "" && document.getElementById("Item[0][" + i + "][Price]").value == "" && document.getElementById("Item[0][" + i + "][PackedCount]").value == "" && document.getElementById("Item[0][" + i + "][Weight]").value == "" && document.getElementById("Item[0][" + i + "][Volume]").value == "") {
					continue;
				}
				else {
					ItemModifySUM += "NEW#@!" +
													document.getElementById("Item[0][" + i + "][boxNo]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][ItemName]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][Brand]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][Matarial]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][Quantity]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][QuantityUnit]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][PackedCount]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][PackingUnit]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][Weight]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][Volume]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][UnitCost]").value + "#@!" +
													document.getElementById("Item[0][" + i + "][Price]").value + "%!$@#";
				}
			}
			//alert(ItemModifySUM);
			var FCLData = "";
			form1.BTN_SUBMIT.style.visibility = "visible";
			Request.RequestFormModify(REQUESTFORMPK, ShipperPk, CompanyInDocumentPk, DepartureDate, ArrivalDate, DepartureRegionCode, ArrivalRegionCode, ShipperStaffName, ShipperStaffTEL, ShipperStaffMobile, TransportWayCL, JubsuWayCL, PaymentWayCL, PaymentWhoCL, DocumentRequestCL, MonetaryUnitCL, PickupRequestDate, NotifyPartyName, NotifyPartyAddress, Memo, ItemModifySUM, FCLData, function (result) {
				if (result == "1") {
					alert("수정완료");
					location.href = "./RequestFormView.aspx?pk=" + REQUESTFORMPK;
				}
				else {
					alert(result);
					return false;
				}
			}, function (result) { alert("ERROR : " + result) });
		}
					
		function AddNotifyParty() {		//기타배송지 추가
			var retVal = window.confirm(form1.HNotifyPartyMSG.value);
			if (retVal) {
				form1.IsNotifyParty.value = "Y";
				form1.TBSameAsAbove.style.width = "1px";
				document.getElementById("PnSameAsAbove").style.visibility = "hidden";
				document.getElementById("PnSameAsAbove").style.width = "5px";
				document.getElementById("PnSetNotifyParty").style.visibility = "Visible";
			}
			else {
				if (form1.IsNotifyParty.value == "Y") {
					form1.IsNotifyParty.value = "N";
					form1.TBSameAsAbove.style.width = "100px";
					document.getElementById("PnSameAsAbove").style.visibility = "visible";
					document.getElementById("PnSameAsAbove").style.width = "100px";
					document.getElementById("PnSetNotifyParty").style.visibility = "hidden";
				}
			}
		}
		function CountTransportationDate() {
			var DepartureDate = document.getElementById("TB_DepartureDate").value;
			var ArrivalDate = document.getElementById("TB_ArrivalDate").value;
			//alert(DepartureDate + " " + ArrivalDate);
			if (DepartureDate.length == 8 && ArrivalDate.length == 8) {
				var DYear = DepartureDate.substring(0, 4); var DMonth = DepartureDate.substring(4, 6); var DDay = DepartureDate.substring(6, 8);
				var AYear = ArrivalDate.substring(0, 4); var AMonth = ArrivalDate.substring(4, 6); var ADay = ArrivalDate.substring(6, 8);
				if (DYear == AYear) {
					if (DMonth == AMonth && DDay < ADay) { DateSpan = ADay - DDay + 1; } 	//같은달
					else if (DMonth < AMonth) {
						DateSpan = GetMonthNalsu(DYear, DMonth) - parseInt(DDay) + parseInt(ADay) + 1;
						var temp = DMonth;
						while (true) {
							temp++;
							if (temp == AMonth) { break; }
							else { DateSpan += GetMonthNalsu(DYear, temp); continue; }
						}
					}
				}
				else if (DYear < AYear) {
					DateSpan = GetMonthNalsu(DYear, DMonth) - parseInt(DDay) + parseInt(ADay) + 1;
				}
				//if (DateSpan > 0) { document.getElementById("TransportDatespan").value = "운송기간 : " + DateSpan + "일"; }
				if (DateSpan == 0) { alert(form1.HArrivalDateMustNearThanDepartureDate.value); document.getElementById("TB_ArrivalDate").value = ""; }
			}
		} 	//출발일이랑 도착일 넣으면 날자 숫자 구하기
		function GetMonthNalsu(yearString, month) {
			var year = parseInt(yearString);
			if (month == "01" || month == "03" || month == "05" || month == "07" || month == "08" || month == "10" || month == "12") { return 31; }
			else if (month == "04" || month == "06" || month == "09" || month == "11") { return 30; }
			else {
				if (year % 4 == 0) {
					if (year % 100 == 0) {
						if (year % 400 == 0) { return 29; }
						else { return 28; }
					}
					else { return 29; }
				}
				else { return 28; }
			} //나머지 4의 배수 !(100의 배수) 400의 배수
		}  // 해당 년도 달의 날짜 갯수 구하기
		function MoveNextByEnter(to) { if (window.event.keyCode == 13) { document.getElementById(to).focus(); } }
		function MoveNextByEnterAndAddRow(to) { if (window.event.keyCode == 13) { InsertRow('0'); document.getElementById(to).focus(); } }
	</script>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    	input{text-align:center; }
    	.tdSubT	{border-bottom:solid 2px #93A9B8;	padding-top:30px; }
		.td01{background-color:#f5f5f5; text-align:center; height:20px; width:130px; border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px; }
    	.td02{width:330px;border-bottom:dotted 1px #E8E8E8;	padding-top:4px;padding-bottom:4px; padding-left:10px; }
		.td023	{width:760px; padding-top:4px;padding-bottom:4px; padding-left:10px;	 border-bottom:dotted 1px #E8E8E8; }
		.title{}
   	</style>
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px; overflow:visible;" >
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Request.asmx" /></Services></asp:ScriptManager>
	<uc1:Loged ID="Loged1" runat="server" />
	<div style="width:100%;">
		<div style="background-color:White; padding:7px; color:Black; font-weight:bold; ">
			<img src="../Images/ico_arrow.gif" alt="" /> <%=GetGlobalResourceObject("qjsdur", "wjqtnwmdtnwjd")%>
		</div>
		<div style="background-color:#777777; height:1px; font-size:1px; "></div>
		<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
		<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
		<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
	    <table width="900px" style="background-color:White;" border="0" cellpadding="0" cellspacing="0" >
			<tr><td class="tdSubT" colspan="4">&nbsp;&nbsp;&nbsp;<strong><%= GetGlobalResourceObject("RequestForm", "Schedule") %></strong></td></tr>
			<tr>
				<td class="td01"><%= GetGlobalResourceObject("RequestForm", "Departure") %></td>
				<td class="td023" colspan="3">
					<input id="TB_DepartureDate" size="10" type="text" readonly="readonly" onclick="openModal(TB_DepartureDate, './calendar.html', 240, 220); return false;" />&nbsp;&nbsp;
					<input type="hidden" id="gcodeExport" value="" />
					<input type="text" id="TB_DepartureRegion" readonly="readonly" style="width:60px; border:0px; " />
					<input type="button" id="BTN_PnDepartureRegion" value="<%=GetGlobalResourceObject("qjsdur", "cnfqkfwl") %> <%=GetGlobalResourceObject("qjsdur", "tnwjd") %>"  onclick="Show('PnDepartureRegion');" />
					<span id="PnDepartureRegion" style="visibility:hidden;">
						<select id="regionCodeI1" style="width:100px;" name="country" size="1" onchange="cate1change('regionCodeI1','regionCodeI2','regionCodeI3',this.form, this.selectedIndex,'gcodeExport')">
							<option value="">:: <%= GetGlobalResourceObject("RequestForm", "DepartureCountrySelection") %> ::</option>
							<option value="1">KOREA&nbsp;한국</option>
							<option value="3">CHINA&nbsp;中國</option>
							<option value="514">Japan&nbsp;日本</option>
							<option value="590">United States</option>
							<option value="594">Mexico</option>
                            <option value="598">Vietnam</option>
                            <option value="717">Hongkong</option>
							<option value="586">Other Location</option>
						</select>
						<select id="regionCodeI2" style="width:140px;" name="office" onchange="cate2change('regionCodeI1','regionCodeI2','regionCodeI3',this.form,this.selectedIndex,'gcodeExport')">
							<option value="">:: <%= GetGlobalResourceObject("RequestForm", "DepartureLocal1") %> ::</option>
						</select>
						<select id="regionCodeI3" style="width:140px; " name="area" onchange="cate3change('regionCodeI1','regionCodeI2','regionCodeI3',this.form,this.selectedIndex,'gcodeExport')">
							<option value="">:: <%= GetGlobalResourceObject("RequestForm", "DepartureLocal2") %> ::</option>
						</select>
					</span>
				</td>
			</tr>
			<tr>
				<td class="td01"><%= GetGlobalResourceObject("RequestForm", "Arrival") %></td>
				<td class="td023" colspan="3" >
					<input id="TB_ArrivalDate" size="10" type="text" readonly="readonly" onclick="openModal(TB_ArrivalDate, './calendar.html', 240, 220); return false;" />&nbsp;&nbsp;
					<input type="hidden" id="gcodeImport" value="" />
					<input type="text" id="TB_ArrivalRegion" readonly="readonly" style="width:60px; border:0px; " />
					<input type="button" id="BTN_PnArrivalRegion" value="<%=GetGlobalResourceObject("qjsdur", "ehckrwl") %> <%=GetGlobalResourceObject("qjsdur", "tnwjd") %>" onclick="Show('PnArrivalRegion');" />
					<span id="PnArrivalRegion" style="visibility:hidden;">
						<select id="regionCodeE1" style="width:100px;" name="country" size="1" onchange="cate1change('regionCodeE1','regionCodeE2','regionCodeE3',this.form, this.selectedIndex,'gcodeImport')">
							<option value="">:: <%= GetGlobalResourceObject("RequestForm", "ArrivalCountrySelection")%> ::</option>
							<option value="1">KOREA&nbsp;한국</option>
							<option value="3">CHINA&nbsp;中國</option>
							<option value="514">Japan&nbsp;日本</option>
							<option value="590">United States</option>
							<option value="594">Mexico</option>
                            <option value="598">Vietnam</option>
                            <option value="717">Hongkong</option>
							<option value="586">Other Location</option>
						</select>
						<select id="regionCodeE2" style="width:140px;" name="office" size="1"  onchange="cate2change('regionCodeE1','regionCodeE2','regionCodeE3',this.form,this.selectedIndex,'gcodeImport')">
							<option value="">:: <%= GetGlobalResourceObject("RequestForm", "ArrivalLocal1")%> ::</option>
						</select>
						<select id="regionCodeE3" style="width:140px;" name="area" size="1"  onchange="cate3change('regionCodeE1','regionCodeE2','regionCodeE3',this.form,this.selectedIndex,'gcodeImport')">
							<option value="">:: <%= GetGlobalResourceObject("RequestForm", "ArrivalLocal2")%> ::</option>
						</select>
					</span>
				</td>
			</tr>
			<tr>
				<td class="td01"><%= GetGlobalResourceObject("RequestForm", "TransportWay") %></td>
				<td class="td023" colspan="3" >
					<span id="BeforeTransportWay"></span>
					<input type="button" onclick="Show('PnTransportWay');" value="<%=GetGlobalResourceObject("qjsdur", "dnsthdqkdqjq") %> <%=GetGlobalResourceObject("qjsdur", "tnwjd") %>"  />
					<span id="PnTransportWay" style="visibility:hidden;"></span>
					<input type="hidden" id="H_Days" value="<%= GetGlobalResourceObject("RequestForm", "Days") %>" />
					<input type="hidden" id="H_TransportWay" value="<%= GetGlobalResourceObject("RequestForm", "TransportWay") %>" />
					<input type="hidden" id="H_TransportBySpecial" value="<%= GetGlobalResourceObject("RequestForm", "TransportWay_Special") %>" />			
					<input type="hidden" id="H_TransportByMixed" value="<%= GetGlobalResourceObject("RequestForm", "TransportWay_Mixed") %>" />
					<input type="hidden" id="H_TransportByFlight" value="<%= GetGlobalResourceObject("RequestForm", "TransportWay_flight") %>" />
					<input type="hidden" id="HTransportByInCountry" value="<%=GetGlobalResourceObject("RequestForm", "TransportWay_InCountry") %>" />
					<input type="hidden" id="HCountury" value="<%= GetGlobalResourceObject("RequestForm", "ArrivalCountrySelection") %>" />
					<input type="hidden" id="HArrivalLocal1" value="<%= GetGlobalResourceObject("RequestForm", "ArrivalLocal1") %>" />
					<input type="hidden" id="HArrivalLocal2" value="<%= GetGlobalResourceObject("RequestForm", "ArrivalLocal2") %>" />
					<input type="hidden" id="UnsongWayCode" />
					<span id="FCLOnly" style="visibility:hidden; padding-left:10px;">
						<select id="FCLType"><option value="0" >Type</option><option value="20DRY" >20DRY</option><option value="40DRY">40DRY</option><option value="40HQ">40HQ</option></select>&nbsp;&nbsp;&nbsp;
						갯수 : <input id="FCLCount" type="text" size="8" />
					</span>
				</td>
			</tr>
		</table>
		<table id="Table_Shipper" width="900px" style="background-color:White;" border="0" cellpadding="0" cellspacing="0" >
			<tr><td class="tdSubT" colspan="6">&nbsp;&nbsp;&nbsp;<strong><%= GetGlobalResourceObject("RequestForm", "Shipper") %></strong></td></tr>
			<tr>
				<td class="td01"><%= GetGlobalResourceObject("RequestForm", "CompanyName") %>
					<input type="hidden" id="TB_pk" value="<%= MEMBERINFO[1] %>" />
					<input type="hidden" id="TB_AccountID" value='<%=MEMBERINFO[2] %>' />
				</td>
				<td class="td023" colspan="5" id="TDCompanyName" >&nbsp;</td>
			</tr>
			<tr>
				<td class="td01"><%= GetGlobalResourceObject("RequestForm", "Clearance") %></td>
				<td class="td023">
					<input type="text" id="CustomerClearanceCompanyName" style="text-align:left; border:0px; " readonly="readonly" size="20" />
					<input type="button" value="<%= GetGlobalResourceObject("Member", "Selection") %>" onclick="PopShipperInDocument()" />
					<input type="hidden" id="CustomerClearancePk" />
					<input type="hidden" id="CustomerClearanceAddress" />
					<input type="hidden" id="H_ClearanceSubstitute" value="<%= GetGlobalResourceObject("RequestForm", "ClearanceSubstitute") %>" />
				</td>
			</tr>
		</table>
		<table style="background-color:White;" border="0" cellpadding="0" cellspacing="0" >
			<tr>
				<td class="td01"><%= GetGlobalResourceObject("RequestForm", "StaffName") %></td>
				<td style="width:120px; border:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px; padding-left:10px;"><input type="text" id="ItemStaffName[0]" size="15" /></td>
				<td style="background-color:#f5f5f5; text-align:center; height:20px; width:120px; border:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px;  ">TEL</td>
				<td style="width:185px; border:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px; padding-left:10px;">
					<input type="text" id="ItemStaffTEL00" size="3" maxlength="4" onkeyup="moveNext(this,form1.ItemStaffTEL01,4);" onblur="OnlyNum(this)" />
					 - <input type="text" id="ItemStaffTEL01" maxlength="4" size="3" onkeyup="moveNext(this, form1.ItemStaffTEL02,4);" onblur="OnlyNum(this)" />
					  - <input type="text" id="ItemStaffTEL02" maxlength="4" size="3" onblur="OnlyNum(this)" />
				</td>
				<td style="background-color:#f5f5f5; text-align:center; height:20px; width:120px; border:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px;  "><%= GetGlobalResourceObject("RequestForm", "Mobile") %></td>
				<td style="width:185px; border:dotted 1px #E8E8E8;  padding-top:4px; padding-bottom:4px; padding-left:10px;">
					<input type="text" id="ItemStaffMobile00" size="3" maxlength="4" onkeyup="moveNext(this, form1.ItemStaffMobile01,4);" onblur="OnlyNum(this)" />
					 - <input type="text" id="ItemStaffMobile01" maxlength="4" size="3" onkeyup="moveNext(this, form1.ItemStaffMobile02,4);" onblur="OnlyNum(this)" />
					 - <input type="text" id="ItemStaffMobile02" maxlength="4" size="3" onblur="OnlyNum(this)" /></td>
			</tr>
			<tr>
				<td rowspan="2" class="td01" ><%= GetGlobalResourceObject("RequestForm", "ToOurOfficeDeliveryWay") %></td>
				<td colspan="2" style="padding-top:4px;padding-bottom:4px; padding-left:10px;	 border-bottom:dotted 1px #E8E8E8; " >
					<input type="radio" name="RB_ItemTransWay[0]" id="ItemTransWay_Direct[0]" checked="checked" onclick="Click_ItemTransWay(0,this.id);"  /> <label for="ItemTransWay_Direct[0]"><% = GetGlobalResourceObject("RequestForm", "SelfDelivery") %></label><input type="hidden" id="DepartureAreaBranchCode" />&nbsp;&nbsp;&nbsp;
					<input type="radio" name="RB_ItemTransWay[0]" id="ItemTransWay_Pickup[0]" onclick="Click_ItemTransWay(0,this.id);" /> <label for="ItemTransWay_Pickup[0]"><%= GetGlobalResourceObject("RequestForm", "PickUp") %></label>&nbsp;&nbsp;&nbsp;
					<input type="radio" name="RB_ItemTransWay[0]" id="ItemTransWay_ETC" onclick="Click_ItemTransWay(0,this.id);" /> <label for="ItemTransWay_ETC"><%= GetGlobalResourceObject("RequestForm", "ETC") %></label>
				</td>
				<td rowspan="2" colspan="3"><textarea id="TB_ItemTransWay_ETC" rows="2" cols="70" onfocus="this.select();" ></textarea></td>
			</tr>
			<tr>
				<td colspan="2" style="padding-top:4px;padding-bottom:4px; padding-left:10px;	 border-bottom:dotted 1px #E8E8E8; " >
					<span id="Pn_Pickup[0]" style="visibility:hidden;">
						<%=GetGlobalResourceObject("RequestForm", "RequestDateTime") %>
						<input id="TB_PickupRequestDate0" readonly="readonly" type="text" onclick="openModal(TB_PickupRequestDate0, './calendar.html', 240, 220);return false;" size="8" />
						&nbsp;<input type="text" id="PickupRequestTimeH" style="width:20px;" maxlength="2" onkeyup="moveNext(this,form1.PickupRequestTimeM,2);"  /> : <input type="text" id="PickupRequestTimeM" style="width:20px;" maxlength="2" /> 
					</span>
				</td>
			</tr>
		</table>
		<table id="Tb_NotifyParty" width="900px" style="background-color:White;" border="0" cellpadding="0" cellspacing="0">
			<tr><td class="tdSubT" colspan="4">&nbsp;&nbsp;&nbsp;<strong><%= GetGlobalResourceObject("RequestForm", "Consignee") %></strong></td></tr>
			<tr>
				<td class="td01"><%= GetGlobalResourceObject("RequestForm", "ConsigneeCompanyName") %></td>
				<td class="td023" colspan="3">
					<input type="text" style="text-align:left; border:0px;" readonly="readonly" id="TB_ImportCompanyName" />&nbsp;
					<input type="hidden" id="TB_CustomerListPk" />
					<input type="hidden" id="HConsigneePk" value="N" />
					<input type="hidden" id="HConsigneeCode" value="N" />
				</td>
			</tr>
			<tr>
				<td class="td01">
					<input type="button" onclick="AddNotifyParty()" value="<%= GetGlobalResourceObject("RequestForm", "NotifyParty") %>" />
					<input type="hidden" id="IsNotifyParty" value="N" />
				</td>
				<td class="td023" colspan="3" >
					<div id="PnSameAsAbove" style="float:left; width:100px; height:20px; padding:10px;">
						<input type="text" id="TBSameAsAbove" value="<%=GetGlobalResourceObject("RequestForm", "SameAsAbove") %>" style="width:100px; text-align:left; border:0px;" />
					</div>
					<div id="PnSetNotifyParty" style="visibility:hidden;">
						<table border="0" cellpadding="0" cellspacing="0">
							<tr>
								<td style="width:80px;">
									<input type="radio" name="Payment1" checked="checked" id="paymenyTT" value="1" />
									<label for="paymenyTT"><%=GetGlobalResourceObject("Member", "Company") %></label>
								</td>
								<td rowspan="2" style="width:450px;" valign="middle">
									<%=GetGlobalResourceObject("RequestForm", "Name1") %> <input type="text" id="TB_NotifyName" style="text-align:left;" size="10" />&nbsp;&nbsp;&nbsp;
									&nbsp;&nbsp;<%= GetGlobalResourceObject("RequestForm", "Address") %> <input type="text" id="TB_NotifyAddress" style="text-align:left;" size="40" />
								</td>
							</tr>
							<tr>
								<td>
									<input type="radio" name="Payment1" id="paymenyLC" value="0" />
									<label for="paymenyLC"><%=GetGlobalResourceObject("RequestForm", "Bank") %></label>
								</td>
							</tr>
						</table>
					</div>
				</td>
			</tr>
		</table>
		<table id="ItemTable[0]" style="background-color:White; width:900px; "  border="0" cellpadding="0" cellspacing="0">
			<thead>
				<tr><td class="tdSubT" colspan="10">&nbsp;&nbsp;&nbsp;<strong><%= GetGlobalResourceObject("RequestForm", "Freight") %></strong>&nbsp;&nbsp;&nbsp;&nbsp;
				<select id="Item[0][MonetaryUnit]" size="1" onchange="SelectChangeMonetaryUnit(this.selectedIndex)">
					<option value="0"><%= GetGlobalResourceObject("RequestForm", "MonetaryUnitChange") %></option>
					<option value="18">RMB ￥</option>
					<option value="19">USD $</option>
					<option value="20">KRW ￦</option>
					<option value="21">JPY Y</option>
					<option value="22">HKD HK$</option>
					<option value="23">EUR €</option>
				</select>
				<span style="float:right; padding-right:10px; " ><input type="button" onclick="InsertRow('0');" value="<%= GetGlobalResourceObject("RequestForm", "InsertItem") %>" /></span>
				</td></tr>
				<tr style="height:30px;" >
					<td bgcolor="#F5F5F5" height="20" align="center" width="50" ><%= GetGlobalResourceObject("RequestForm", "BoxNo") %></td>
					<td bgcolor="#F5F5F5" align="center" width="175" ><%= GetGlobalResourceObject("RequestForm", "Description") %></td>
					<td bgcolor="#F5F5F5" align="center" width="75" ><%= GetGlobalResourceObject("RequestForm", "Label") %></td>
					<td bgcolor="#F5F5F5" align="center" width="65" ><%= GetGlobalResourceObject("RequestForm", "Material") %></td>
					<td bgcolor="#F5F5F5" align="center" width="110"><%= GetGlobalResourceObject("RequestForm", "Count") %></td>
					<td bgcolor="#F5F5F5" align="center" width="70"><%= GetGlobalResourceObject("RequestForm", "UnitCost") %></td>
					<td bgcolor="#F5F5F5" align="center" width="90"><%= GetGlobalResourceObject("RequestForm", "Amount") %></td>
					<td bgcolor="#F5F5F5" align="center" width="110"><%= GetGlobalResourceObject("RequestForm", "PackingCount") %></td>
					<td bgcolor="#F5F5F5" align="center" width="40"><%= GetGlobalResourceObject("RequestForm", "GrossWeight") %> (Kg)</td>
					<td bgcolor="#F5F5F5" align="center" ><%= GetGlobalResourceObject("RequestForm", "Volume") %> (CBM)</td>
				</tr>
			</thead>
			<tbody>
				<tr>
					<td align="center" ><input type="text" id="Item[0][0][boxNo]" onkeydown="MoveNextByEnter('Item[0][0][ItemName]')" onkeyup="CountBox('0','0',this)" size="6" /></td>
					<td align="center" ><input type="text" id="Item[0][0][ItemName]" onkeydown="MoveNextByEnter('Item[0][0][Brand]')" size="25" /></td>
					<td align="center" ><input type="text" id="Item[0][0][Brand]" onkeydown="MoveNextByEnter('Item[0][0][Matarial]')" size="8" /></td>
					<td align="center" ><input type="text" id="Item[0][0][Matarial]" onkeydown="MoveNextByEnter('Item[0][0][Quantity]')" size="6" /></td >
					<td align="center" >
						<input type="text" id="Item[0][0][Quantity]" onkeyup="QuantityXUnitCost(0,0)" onkeydown="MoveNextByEnter('Item[0][0][UnitCost]')" size="5" />
						<select id="Item[0][0][QuantityUnit]">
							<option value="40">PCS</option><option value="41">PRS</option><option value="42">SET</option><option value="43">S/F</option>
							<option value="44">YDS</option><option value="45">M</option><option value="46">KG</option><option value="47">DZ</option>
							<option value="48">L</option><option value='49'>Box</option><option value='50'>SQM</option><option value='51'>M2</option><option value='52'>RO</option>
						</select>
					</td>
					<td align="center" >
						<input type="text" style="border:0; width:10px; " id="Item[0][0][MonetaryUnit][0]" /><input type="text" id="Item[0][0][UnitCost]" onkeydown="MoveNextByEnter('Item[0][0][PackedCount]')" onkeyup="QuantityXUnitCost(0,0); GetTotal('Price');" size="5" />
					</td>
					<td align="center" ><input type="text" style="border:0; width:10px; " id="Item[0][0][MonetaryUnit][1]" /><input type="text" id="Item[0][0][Price]" readonly="readonly"  size="9" /></td>
					<td align="center" >
						<input type="text" id="Item[0][0][PackedCount]" onkeydown="MoveNextByEnter('Item[0][0][Weight]')" onkeyup="BoxTotalCount('0');"  size="6" />
						<select id="Item[0][0][PackingUnit]" size="1" >
							<option value="15">CT</option>
							<option value="16">RO</option>
							<option value="17">PA</option>
						</select>
					</td>
					<td align="center" ><input type="text" id="Item[0][0][Weight]" onkeydown="MoveNextByEnter('Item[0][0][Volume]')" size="5" /></td>
					<td align="center" ><input type="text" id="Item[0][0][Volume]" onkeydown="MoveNextByEnterAndAddRow('Item[0][1][boxNo]');" size="5" /></td>
				</tr>
			</tbody>
		</table>
		<table border="0" style="background-color:White; width:900px; "  cellpadding="0" cellspacing="0">
			<tr>
				<td bgcolor="#F5F5F5" height="30" align="right" >&nbsp;&nbsp;&nbsp;<%= GetGlobalResourceObject("RequestForm", "Total") %>&nbsp;&nbsp;&nbsp;</td>
				<td bgcolor="#F5F5F5" width="92" align="left" ><input type="text" style="border:0; background-color:#F5F5F5; width:10px; " id="Item[0][MonetaryUnit][Total]"  /><input type="text" id="total[0][Price]" size="9" /></td>
				<td bgcolor="#F5F5F5" width="102" ><input type="text" id="total[0][PackedCount]" size="6" /></td>
				<td bgcolor="#F5F5F5" width="70" ><input type="text" id="total[0][Weight]" size="6" /></td>
				<td style="background-color:#F5F5F5; width:80px;" ><input type="text" id="total[0][Volume]" size="6" /> </td>
			</tr>
		</table>
		<table width="900px" style="background-color:White;"  border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td class="tdSubT" colspan="4">
					&nbsp;&nbsp;&nbsp;<strong><%= GetGlobalResourceObject("RequestForm", "TradeDocument") %></strong>
				</td>
			</tr>
			<tr>
				<td class="td01" ><%= GetGlobalResourceObject("RequestForm", "TradeDocumentRequest") %></td>
				<td class="td023" colspan="3" >
					<input type="checkbox" id="CertificateOfOrigin" /><label for="CertificateOfOrigin"><%= GetGlobalResourceObject("RequestForm", "certificateoforigin") %></label>&nbsp;&nbsp;&nbsp;&nbsp;
					<input type="checkbox" id="FoodSanitation" /><label for="FoodSanitation"><%= GetGlobalResourceObject("RequestForm", "FoodSanitation") %></label>&nbsp;&nbsp;&nbsp;&nbsp;
					<input type="checkbox" id="ElectricSafety" /><label for="ElectricSafety"><%= GetGlobalResourceObject("RequestForm", "ElectricSafety") %></label>&nbsp;&nbsp;&nbsp;&nbsp;
					<input type="checkbox" id="ProductCheked" /><label for="ProductCheked"><%= GetGlobalResourceObject("RequestForm", "ProductCheked") %></label>&nbsp;&nbsp;&nbsp;&nbsp;
					<input type="checkbox" id="SuChec" /><label for="SuChec"><%= GetGlobalResourceObject("RequestForm", "SuChec") %></label>
				</td>
			</tr>
			<tr><td class="tdSubT" colspan="4">&nbsp;&nbsp;&nbsp;<strong><%= GetGlobalResourceObject("RequestForm", "Payment") %></strong></td></tr>
			<tr>
				<td class="td01"><%= GetGlobalResourceObject("RequestForm", "PaymentWho") %></td>
				<td colspan="3"  style="border-bottom:dotted 1px #E8E8E8;	padding-top:4px;padding-bottom:4px; padding-left:10px; ">
					<div style="background-color:#f5f5f5; text-align:center; border-bottom:dotted 1px #E8E8E8; width:430px; height:65px;  float:right;  margin-right:10px; margin-top:5px; padding:10px;   " >
						<textarea id="ETCTextarea" style="overflow:hidden; " rows="4" cols="70" onfocus="this.select();" ></textarea>
					</div>
					<input type="radio" name="Payment3" id="EXW" value="5" />
						<a class="title" href="#" title="<%= GetGlobalResourceObject("RequestForm", "PaymentAEx") %>"> <%= GetGlobalResourceObject("RequestForm", "PaymentA") %></a> <!--<label for="EXW" ></label>--><br />
					<input type="radio" name="Payment3" id="DDP" value="6" />
						<a class="title" href="#" title="<%= GetGlobalResourceObject("RequestForm", "PaymentBEx") %>"> <%= GetGlobalResourceObject("RequestForm", "PaymentB") %></a> <br />
					<input type="radio" name="Payment3" id="CNF" value="7" />
						<a class="title" href="#" title="<%= GetGlobalResourceObject("RequestForm", "PaymentCEx") %>"> <%= GetGlobalResourceObject("RequestForm", "PaymentC") %></a><br />
					<input type="radio" name="Payment3" id="FOB" value="8" />
						<a class="title" href="#" title="<%= GetGlobalResourceObject("RequestForm", "PaymentDEx") %>"> <%= GetGlobalResourceObject("RequestForm", "PaymentD") %> </a> <br />
					<input type="radio" name="Payment3" id="ETC" value="9" onclick="document.getElementById('ETCTextarea').focus();" />
						<label for="ETC" ><% = GetGlobalResourceObject("RequestForm", "ETC") %></label>
				</td>
			</tr>
			<%--<tr><td class="tdSubT" colspan="4">&nbsp;&nbsp;&nbsp;<strong><%= GetGlobalResourceObject("RequestForm", "RequestFormETC") %></strong>  : 아래에 기재된 메세지는 물류회사 직원만 볼수 있습니다. </td></tr>--%>
			<tr><td colspan="4" style="padding:20px;"><input type="hidden" id="TextareaETC" /></td></tr>
			<tr>
				<td colspan="4" style=" padding:30px; text-align:center;">
					<input type="button" id="BTN_SUBMIT" value="  <%= GetGlobalResourceObject("RequestForm", "Submit") %>  " onclick="Btn_Submit_Click()" style="width:150px; height:40px;  " />&nbsp;&nbsp;&nbsp;&nbsp;
					<input type="button" value="  <%=GetGlobalResourceObject("Member", "Cancel") %>  " style="width:150px; height:40px;  " onclick="history.back();" />
				</td>
			</tr>
		</table>
	<input type="hidden" id="DEBUG" />
	<input type="hidden" id="HMustArrivalArea" value="<%=GetGlobalResourceObject("Alert", "MustArrivalArea")%>" />
	<input type="hidden" id="HMustArrivalDate" value="<%=GetGlobalResourceObject("Alert", "MustArrivalDate") %>" />
	<input type="hidden" id="HMustDeliveryWay" value="<%=GetGlobalResourceObject("Alert", "MustDeliveryWay") %>" />
	<input type="hidden" id="HMustDepartureArea" value="<%=GetGlobalResourceObject("Alert", "MustDepartureArea") %>" />
	<input type="hidden" id="HMustDepartureDate" value="<%=GetGlobalResourceObject("Alert", "MustDepartureDate") %>" />
	<input type="hidden" id="HMustNotifyPartyAddress" value="<%=GetGlobalResourceObject("Alert", "MustNotifyPartyAddress") %>" />
	<input type="hidden" id="HMustNotifyPartyBank" value="<%=GetGlobalResourceObject("Alert", "MustNotifyPartyBank") %>" />
	<input type="hidden" id="HMustNotifyPartyCompanyName" value="<%=GetGlobalResourceObject("Alert", "MustNotifyPartyCompanyName") %>" />
	<input type="hidden" id="HMustPickupRequestDate" value="<%=GetGlobalResourceObject("Alert", "MustPickupRequestDate") %>" />
	<input type="hidden" id="HMustShipper" value="<%=GetGlobalResourceObject("Alert", "MustShipper") %>" />
	<input type="hidden" id="HMustShipperNameInDocument" value="<%=GetGlobalResourceObject("Alert", "MustShipperNameInDocument") %>" />
	<input type="hidden" id="HMustTransportWay" value="<%=GetGlobalResourceObject("Alert", "MustTransportWay") %>" />
	<input type="hidden" id="HArrivalDateMustNearThanDepartureDate" value="<%=GetGlobalResourceObject("Alert", "ArrivalDateMustNearThanDepartureDate") %>" />
	<input type="hidden" id="HNotifyPartyMSG" value="<%=GetGlobalResourceObject("Alert", "NotifyPartyMSG") %>" />
	<input type="hidden" id="alwlwjd" value="<%=GetGlobalResourceObject("qjsdur", "alwlwjd") %>" />
</div>	
    </form>
</body>
</html>
