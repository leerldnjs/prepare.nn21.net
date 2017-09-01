<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PackingListWrite.aspx.cs" Inherits="Request_PackingListWrite" %>
<%@ Register src="../Member/LogedTopMenu.ascx" tagname="Loged" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Packing List Write</title>
    <link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <script src="../Common/public.js" type="text/javascript"></script>
	<script src="../Common/jquery-1.10.2.min.js"></script>
	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
	<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>

	<style type="text/css">
    	input {text-align:center; }
    	.tdSubT {border-bottom:solid 1px #93A9B8; padding-top:10px; }
		.td01 {background-color:#f5f5f5; text-align:center; height:20px; width:130px; border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px; }
    	.td02 {width:330px;border-bottom:dotted 1px #E8E8E8;	padding-top:4px;padding-bottom:4px; padding-left:10px; }
		.td023 {padding-top:4px; padding-bottom:4px; padding-left:10px;	 border-bottom:dotted 1px #E8E8E8; }
		.Title{border:solid 1px black; width:692px; height:30px; font-size:18px; font-weight:bold; text-align:center; letter-spacing:12px; padding-top:3px; }
		.Shipper{border:solid 1px black; height:90px; margin-top:-1px; padding:5px; }
		.PortOfLoading{border:solid 1px black; height:51px; margin-top:-1px; padding:5px; }
		.FinalDestination{border:solid 1px black; height:51px; margin-top:-1px; margin-left:-1px;  padding:5px; }
		.InvoiceNo{border:solid 1px black; margin-left:-1px; padding:5px; }
		.BL{border-left:solid 1px black; }
		.BB{border-bottom:solid 1px black;}
		.BR{border-right:solid 1px black;}
		#quick_menu input{width:110px; height:23px; margin:5px; }
		div#quick_menu {position:absolute; left:50%; margin-top:100px; margin-left:300px; border:1px solid #999999; background:#eeeeee;}
	</style>
	<script type="text/javascript">
		//RegionCode
		var Port = new Array();
		Port[86] = new Array(); //86	CHINA
		Port[86][0] = new Option('P86!00', 'ANY PORT OF CHINA'); 
		Port[86][1] = new Option('P86!01', 'CNBHY | 북해'); Port[86][2] = new Option('P86!02', 'CNBJS | 북경'); Port[86][3] = new Option('P86!03', 'CNDLC | 대련');
		Port[86][4] = new Option('P86!04', 'CNDDG | 단동'); Port[86][5] = new Option('P86!05', 'CNFOC | 복주'); Port[86][6] = new Option('P86!06', 'CNCAN | 광주');
		Port[86][7] = new Option('P86!07', 'CNHGH | 항주'); Port[86][8] = new Option('P86!08', 'CNHRB | 하얼빈'); Port[86][9] = new Option('P86!09', 'CNNKG | 남경');
		Port[86][10] = new Option('P86!10', 'CNNTG | 남통'); Port[86][11] = new Option('P86!11', 'CNNGB | 닝보'); Port[86][12] = new Option('P86!12', 'CNTAO | 청도');
		Port[86][13] = new Option('P86!13', 'CNSHP | 진항도'); Port[86][14] = new Option('P86!14', 'CNSHA | 상하이'); Port[86][15] = new Option('P86!15', 'CNSWA | 산토우');
		Port[86][16] = new Option('P86!16', 'CNSHE | 심양'); Port[86][17] = new Option('P86!17', 'CNSZX | 심천'); Port[86][18] = new Option('P86!18', 'CNSHD | 석도');
		Port[86][19] = new Option('P86!19', 'CNTSN | 텐진'); Port[86][20] = new Option('P86!20', 'CNWEI | 위해'); Port[86][21] = new Option('P86!21', 'CNWNZ | 온주');
		Port[86][22] = new Option('P86!22', 'CNWUH | 우한'); Port[86][23] = new Option('P86!23', 'CNXMN | 하문'); Port[86][24] = new Option('P86!24', 'CNYNT | 연태');
		Port[86][25] = new Option('P86!25', 'CNYIK | 영구'); Port[86][26] = new Option('P86!26', 'CNZSN | 중산');

		Port[81] = new Array(); //81	Japan
		Port[81][0] = new Option('P81!00', 'ANY PORT OF JAPAN'); 
		Port[81][1] = new Option('P81!01', 'JPAIK | 아이카와'); Port[81][2] = new Option('P81!02', 'JPAIO | 아이오이'); Port[81][3] = new Option('P81!03', 'JPAXT | 아키타');
		Port[81][4] = new Option('P81!04', 'JPAOJ | 아오모리'); Port[81][5] = new Option('P81!05', 'JPBPU | 벳부'); Port[81][6] = new Option('P81!06', 'JPFUK | 후쿠오카');
		Port[81][7] = new Option('P81!07', 'JPFKM | 후쿠시마'); Port[81][8] = new Option('P81!08', 'JPFKY | 후쿠야마'); Port[81][9] = new Option('P81!09', 'JPHKT | 하카타');
		Port[81][10] = new Option('P81!10', 'JPHMD | 하마다'); Port[81][11] = new Option('P81!11', 'JPHIJ | 히로시마'); Port[81][12] = new Option('P81!12', 'JPHTC | 히타치');
		Port[81][13] = new Option('P81!13', 'JPKOJ | 카고시마'); Port[81][14] = new Option('P81!14', 'JPKWS | 가와사키'); Port[81][15] = new Option('P81!15', 'JPUKB | 코베');
		Port[81][16] = new Option('P81!16', 'JPUKY | 교토'); Port[81][17] = new Option('P81!17', 'JPNNG | 나가노'); Port[81][18] = new Option('P81!18', 'JPNGO | 나고야');
		Port[81][19] = new Option('P81!19', 'JPKIJ | 니이가타'); Port[81][20] = new Option('P81!20', 'JPOIT | 오이타'); Port[81][21] = new Option('P81!21', 'JPOKA | 오키나와');
		Port[81][22] = new Option('P81!22', 'JPOSA | 오사카'); Port[81][23] = new Option('P81!23', 'JPSPK | 삿뽀로'); Port[81][24] = new Option('P81!24', 'JPSHS | 시모노세키');
		Port[81][25] = new Option('P81!25', 'JPTYO | 도쿄'); Port[81][26] = new Option('P81!26', 'JPYOK | 요코하마');

		Port[82] = new Array(); 	//82	KOREA
		Port[82][0] = new Option('P82!00', 'ANY PORT OF KOREA'); 
		Port[82][1] = new Option('P82!01', 'KRCJU | 제주공항'); Port[82][2] = new Option('P82!02', 'KRGMP  | 김포공항'); Port[82][3] = new Option('P82!03', 'KRICN | 인천공항');
		Port[82][4] = new Option('P82!04', 'KRDRS  | 도라산육로'); Port[82][5] = new Option('P82!05', 'KRGSG | 고성육로'); Port[82][6] = new Option('P82!06', 'KRCHA | 제주항');
		Port[82][7] = new Option('P82!07', 'KRINC | 인천항'); Port[82][8] = new Option('P82!08', 'KRKAN | 광양항'); Port[82][9] = new Option('P82!09', 'KRKUV | 군산항');
		Port[82][10] = new Option('P82!10', 'KRPTK  | 평택항'); Port[82][11] = new Option('P82!11', 'KRPUS | 부산항'); Port[82][12] = new Option('P82!12', 'KRSHO | 속초항');
		//BodyOnload
		window.onload = function () {
			$("#TBSailingOn").datepicker();
			$("#TBSailingOn").datepicker("option", "dateFormat", "yymmdd");

			var lis = document.getElementById('cssdropdown').getElementsByTagName('li');
			for (i = 0; i < lis.length; i++) {
				var li = lis[i];
				if (li.className == 'headlink') {
					li.onmouseover = function () { this.getElementsByTagName('ul').item(0).style.display = 'block'; document.getElementById("TopMenu").style.height = '130px'; }
					li.onmouseout = function () { this.getElementsByTagName('ul').item(0).style.display = 'none'; document.getElementById("TopMenu").style.height = '100%'; }
				}
			}
			var URI = location.href;
			var UriSpliteByQ = URI.split('?');
			var QueryString = "";
			var Mode = "JW";
			var urisplitbyand = UriSpliteByQ[1].split("&");
			for (var i = 0; i < urisplitbyand.length; i++) {
				if (urisplitbyand[i].substr(0, 1) == "M") {
					if (urisplitbyand[i].toString() == "M=" + Mode) {
						form1.HMode.value = Mode;
						break;
					}
					else {
						var Each = UriSpliteByQ[1].split("&");
						var RequestFormPk = Each[0].substr(2, Each[0].length - 2);
						Mode = Each[1].substr(2, 2);
						form1.HMode.value = Mode;
						form1.HRequestFormPk.value = RequestFormPk;
						Request.LoadInvoice(RequestFormPk, "P", LoadForOfferWriteSUCCESS, function (result) { window.alert(result); });
					}
				}
			}

			var quick_menu = $('#quick_menu');
			var quick_top = 110;
			/* quick menu initialization */
			quick_menu.css('top', $(window).height());
			$(document).ready(function () {
				quick_menu.animate({ "top": $(document).scrollTop() + quick_top + "px" }, 500);
				$(window).scroll(function () {
					quick_menu.stop();
					quick_menu.animate({ "top": $(document).scrollTop() + quick_top + "px" }, 1000);
				});
			});
		}
		
		function LoadForOfferWriteSUCCESS(result) {
			if (result == "N") {
				alert(form1.HAlertReject.value); document.location.href = "../Default.aspx"; return false;
			}
			var Mode = form1.HMode.value;
			var MonetaryUnit;
			for (var i = 0; i < result.length; i++) {
				var Each = result[i].split("#@!");
				if (i == 0) {
					MonetaryUnit = Each[5];
					form1.HStepCL.value = Each[6];
					form1.TBSailingOn.value = Each[0];
					if (Each[7] != "") {
						SPortCountrySelect(Each[7].substr(1, 2), 'StPortOfLanding2', 'HPortOfLanding');
						form1.StPortOfLanding1.value = Each[7].substr(1, 2);
						form1.StPortOfLanding2.value = Each[7];
						form1.HPortOfLanding.value = Each[7];
					}
					if (Each[8] != "") {
						SPortCountrySelect(Each[8].substr(1, 2), 'StFinialDestination2', 'HFinialDestination');
						form1.StFinialDestination1.value = Each[8].substr(1, 2);
						form1.StFinialDestination2.value = Each[8];
						form1.HFinialDestination.value = Each[8];
					}
					var tempstring = Each[9].split("@@@@");
					form1.TBBuyer.value = tempstring[0];
					form1.TBOtherReferences.value = tempstring[1];
					form1.TBCarrier.value = tempstring[2];
					form1.TBNortifyPartyName.value = Each[10];
					form1.TBNotifyPartyAddress.value = Each[11];
					form1.TBInvoiceNo.value = Each[1];
					if (Each[4] == "3") {
						document.form1.PaymentWayCL[0].checked = true;
					}
					else {
						document.form1.PaymentWayCL[1].checked = true;
					}
					form1.TBShipperName.value = Each[2];
					form1.TBConsigneeName.value = Each[3];
					form1.TBShipperAddress.value = Each[12];
					form1.TBConsigneeAddress.value = Each[13];
				}
				else {
					/*	0	RFI.RequestFormItemsPk,	1	RFI.ItemCode,	2	RFI.Description,		3	RFI.Label,	4	RFI.Material,	5	RFI.Quantity,		6	RFI.QuantityUnit,	7	RFI.UnitPrice,	8	RFI.Amount	*/
					if (i > 1) { InsertRow(0); }
					document.getElementById("Item[0][" + (i - 1) + "][SUM]").value = result[i];
					document.getElementById("Item[0][" + (i - 1) + "][RequestFormItemsPk]").value = Each[0];
					document.getElementById("Item[0][" + (i - 1) + "][BoxNo]").value = Each[1];
					document.getElementById("Item[0][" + (i - 1) + "][Brand]").value = Each[3];
					document.getElementById("Item[0][" + (i - 1) + "][ItemName]").value = Each[2];
					document.getElementById("Item[0][" + (i - 1) + "][Matarial]").value = Each[4];
					document.getElementById("Item[0][" + (i - 1) + "][Quantity]").value = NumberFormat(Each[5]);
					switch (Each[6]) {
						case "40": Each[6] = '0'; break;
						case "41": Each[6] = '1'; break;
						case "42": Each[6] = '2'; break;
						case "43": Each[6] = '3'; break;
						case "44": Each[6] = '4'; break;
						case "45": Each[6] = '5'; break;
						case "46": Each[6] = '6'; break;
						case "47": Each[6] = '7'; break;
						case "48": Each[6] = '8'; break;
					}
					document.getElementById("Item[0][" + (i - 1) + "][QuantityUnit]").selectedIndex = Each[6];
					document.getElementById("Item[0][" + (i - 1) + "][PackedCount]").value = NumberFormat(Each[7]);
					switch (Each[8]) {
						case "15": Each[8] = '0'; break;
						case "16": Each[8] = '1'; break;
						case "17": Each[8] = '2'; break;
					}
					document.getElementById("Item[0][" + (i - 1) + "][PackingUnit]").selectedIndex = Each[8];
					document.getElementById("Item[0][" + (i - 1) + "][Weight]").value = NumberFormat(Each[9]);
					document.getElementById("Item[0][" + (i - 1) + "][Volume]").value = NumberFormat(Each[10]);
				}
			}
			GetTotal("Quantity");
			GetTotal("PackedCount");
			GetTotal("Weight");
			GetTotal("Volume");
		}

		function NumberFormat(number) {
			if (number == "" || number == "0") { return number; }
			else { return parseInt(number * 10000) / 10000; }
		}

		function delopt(selectCtrl, mTxt) { if (selectCtrl.options) { for (i = selectCtrl.options.length; i >= 0; i--) { selectCtrl.options[i] = null; } } }
		function SPortCountrySelect(idx, StTarget, HRegionCode) {
			for (i = 0; i < Port[idx].length; i++) { document.getElementById(StTarget).options[i] = new Option(Port[idx][i].value, Port[idx][i].text); }
			StTarget.selectedIndex = 0;
			document.getElementById(HRegionCode).value = document.getElementById(StTarget).options[0].value;
			//document.getElementById(TB_RegionPk).value = document.getElementById(first).options[idx].value;
		}
		function SPortSelect(idx, HTarget) { document.getElementById(HTarget).value = idx; }


		function PopShipperInDocument() {	//신규고객 등록
			var DialogResult = '';
			var retVal = window.showModalDialog('./Dialog/ShipperNameSelection.aspx?S=' + form1.HCompanyPk.value, DialogResult, 'dialogWidth=700px;dialogHeight=400px;resizable=0;status=0;scroll=1;help=0;');
			try {
				if (retVal == '1') {
					form1.HCustomerClearancePk.value = "0";
					form1.CustomerClearanceCompanyName.value = form1.HClearanceSubstitute.value;
				}
				else if (retVal == 'D') { PopShipperInDocument(); }
				else {
					var ReturnArray = retVal.split('##');
					form1.HCustomerClearancePk.value = ReturnArray[0];
					form1.CustomerClearanceCompanyName.value = ReturnArray[1];
					form1.HCustomerClearanceAddress.value = ReturnArray[2];
				}
			}
			catch (err) { return false; }
		}

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


		function PopupCustomerListAdd() {	//신규고객 등록
			if (form1.HMode.value[1] == 'R') {
				alert("수입자는 수정할수 없습니다. \r\n다른분께 보낼 문서는 문서복사 기능을 이용해주세요");
				return false;
			}
			var DialogResult = '';
			var retVal = window.showModalDialog('./OwnCustomerListAdd.aspx?pk=' + +form1.HCompanyPk.value + '&id=' + form1.HAccountID.value + '&g=N', DialogResult, 'dialogWidth=400px;dialogHeight=400px;resizable=0;status=0;scroll=0;help=0;');
			try {
				var ReturnArray = retVal.split('##');
				if (ReturnArray[3] != "") {
					form1.HConsigneePk.value = ReturnArray[3];
					form1.HConsigneeCode.value = ReturnArray[1];
				}
				else {
					form1.HConsigneePk.value = "N";
					form1.HConsigneeCode.value = "N";
				}
				ShowModalApplicant(ReturnArray[2]);
			}
			catch (ex) { return false; }
		}


		function PopupCustomerList() {	//수화인 선택
			if (form1.HMode.value[1] == 'R') {
				alert("수입자는 수정할수 없습니다. \r\n다른분께 보낼 문서는 문서복사 기능을 이용해주세요");
				return false;
			}
			var retVal = window.showModalDialog('./OwnCustomerList.aspx?S=' + form1.HCompanyPk.value, '', 'dialogWidth=500px;dialogHeight=500px;resizable=0;status=0;scroll=0;help=0;');
			try {
				var ReturnArray = retVal.split('##');
				if (ReturnArray[3] != "") {
					form1.HConsigneePk.value = ReturnArray[3];
					form1.HConsigneeCode.value = ReturnArray[1];
				}
				else {
					form1.HConsigneePk.value = "N";
					form1.HConsigneeCode.value = "N";
				}
				ShowModalApplicant(ReturnArray[2]);
			}
			catch (ex) { return false; }
		}
		function ShowModalApplicant(CCLPk) {
			var retVal = window.showModalDialog('./Dialog/OfferSheetTypeApplicant.aspx?S=' + CCLPk, '', 'dialogWidth=400px;dialogHeight=400px;resizable=0;status=0;scroll=0;help=0;');
			document.getElementById('TB_CustomerListPk').value = CCLPk;
			var ReturnArray = retVal.split("!!!!");
			//alert(retVal);
			form1.TB_CustomerListPk.value = CCLPk;
			form1.TB_ImportCompanyName.value = ReturnArray[1];
			form1.TBImportCompanyAddress.value = ReturnArray[2];
		}

		function InsertRow(rowC) {		// 상품추가
			var objTable = document.getElementById('ItemTable[' + rowC + ']');
			objTable.appendChild(document.createElement("TBODY"));
			var lastRow = objTable.rows.length;
			var thisLineNo = lastRow - 1;
			var objRow1 = objTable.insertRow(lastRow);
			var objCell1 = objRow1.insertCell();
			var objCell2 = objRow1.insertCell();
			var objCell3 = objRow1.insertCell();
			var objCell4 = objRow1.insertCell();
			var objCell5 = objRow1.insertCell();
			var objCell6 = objRow1.insertCell();
			var objCell7 = objRow1.insertCell();
			var objCell8 = objRow1.insertCell();

			objCell1.align = "center"; objCell2.align = "center"; objCell3.align = "center"; objCell4.align = "center";
			objCell5.align = "center"; objCell6.align = "center"; objCell7.align = "center"; objCell8.align = "center";
			
			objCell1.innerHTML = "<input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][EachVolume]\" /> <input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][ItemCode]\" /> <input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][RequestFormItemsPk]\" value=\"N\" /> <input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][SUM]\" /> <input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][BoxNo]\" style=\"width:40px;\" />";
			objCell2.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Brand]\" style=\"width:75px;\" />";
			objCell3.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][ItemName]\" style=\"width:80px;\" />";
			objCell4.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Matarial]\" style=\"width:80px;\" />";
			objCell5.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Quantity]\" onkeyup=\"GetTotal('Quantity');\"  style=\"width:50px;\" /> <select id=\"Item[" + rowC + "][" + thisLineNo + "][QuantityUnit]\"><option value=\"40\">PCS</option><option value=\"41\">PRS</option><option value=\"42\">SET</option><option value=\"43\">S/F</option><option value=\"44\">YDS</option><option value=\"45\">M</option><option value=\"46\">KG</option><option value=\"47\">DZ</option><option value=\"48\">L</option><option value='49'>BOX</option><option value='50'>SQM</option><option value='51'>M2</option><option value='52'>RO</option></select>";
			objCell6.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][PackedCount]\" onkeyup=\"GetTotal('PackedCount');\" style=\"width:40px;\" /> <select id=\"Item[" + rowC + "][" + thisLineNo + "][PackingUnit]\" size=\"1\" ><option value=\"15\">CT</option><option value=\"16\">RO</option><option value=\"17\">PA</option></select>";
			objCell7.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Weight]\" onkeyup=\"GetTotal('Weight');\" style=\"width:50px;\" />";
			objCell8.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Volume]\" onkeyup=\"GetTotal('Volume');\"  style=\"width:50px;\" />";
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

		/*	확인버튼 */
		function Btn_Submit_Click() {
			var RequestFormPk = form1.HRequestFormPk.value;
			var MonetaryUnit = "";
			var StepCL = form1.HStepCL.value;

			switch (form1.HMode.value) {
				case "PW": StepCL = (parseInt(form1.HStepCL.value) + 2); break;
				case "JW": StepCL = 2; break;
			}

			var SessionPk = form1.HCompanyPk.value;
			var accountID = form1.HAccountID.value;
			var SailingOn = form1.TBSailingOn.value;
			var PortOfLanding = form1.HPortOfLanding.value;
			var FilnalDestination = form1.HFinialDestination.value;
			var NotifyParty = form1.TBNortifyPartyName.value;
			if (NotifyParty == "Same as Above") { NotifyParty = ""; }

			var PaymentWay = 3;
			if (document.form1.PaymentWayCL[1].checked) { PaymentWay = 4; }

			var RowTemp1;
			var RowTemp2;
			var RowTemp3;
			var RowTemp4;
			var RowTemp5;
			var itemInfoGroup = "";
			var RowLength = document.getElementById('ItemTable[0]').rows.length - 1;
			/*	0	RFI.RequestFormItemsPk,		1	RFI.ItemCode,	2	RFI.MarkNNumber,	3	RFI.Description,		4	RFI.Label,		5	RFI.Material,		6	RFI.Quantity,		7	RFI.QuantityUnit,	8	RFI.PackedCount,
			9	RFI.PackingUnit,	10	RFI.GrossWeight,		11	RFI.Volume	*/
			
			for (var i = 0; i < RowLength; i++) {
				
				RowTemp1 = document.getElementById("Item[0][" + i + "][RequestFormItemsPk]").value + "!" + document.getElementById("Item[0][" + i + "][ItemCode]").value + "!" + document.getElementById("Item[0][" + i + "][BoxNo]").value + "!" + document.getElementById("Item[0][" + i + "][ItemName]").value + "!" + document.getElementById("Item[0][" + i + "][Brand]").value + "!" + document.getElementById("Item[0][" + i + "][Matarial]").value + "!" + document.getElementById("Item[0][" + i + "][Quantity]").value + "!";
				RowTemp2 = document.getElementById("Item[0][" + i + "][QuantityUnit]").value + "!";
				RowTemp3 = document.getElementById("Item[0][" + i + "][PackedCount]").value + "!";
				RowTemp4 = document.getElementById("Item[0][" + i + "][PackingUnit]").value + "!";
				RowTemp5 = document.getElementById("Item[0][" + i + "][Weight]").value + "!" + document.getElementById("Item[0][" + i + "][Volume]").value;
				if (RowTemp1 + RowTemp2 + RowTemp3 + "!" + RowTemp5 != document.getElementById("Item[0][" + i + "][SUM]").value && RowTemp1 + RowTemp3 + RowTemp5 != "N!!!!!!!!!") {
					//alert(RowTemp5);
					itemInfoGroup += RowTemp1 + RowTemp2 + RowTemp3 + RowTemp4 + RowTemp5 + "####";
				}
			}
			Request.SaveForOfferWrite("P", SessionPk, RequestFormPk, MonetaryUnit, StepCL, accountID, SailingOn, PortOfLanding, FilnalDestination, PaymentWay, itemInfoGroup, NotifyParty, form1.TBNotifyPartyAddress.value, form1.TBInvoiceNo.value, form1.TBBuyer.value + "@@@@" + form1.TBOtherReferences.value + "@@@@" + form1.TBCarrier.value, form1.TBShipperName.value, form1.TBShipperAddress.value, form1.TBConsigneeName.value, form1.TBConsigneeAddress.value, function (result) {
				parent.location.href = "./PackingListView.aspx?S=" + result;
			}, ONFAILED);
		}
		function ONFAILED(result) { window.alert(result); }

	</script>
</head>
<body class="MemberBody" >
    <form id="form1" runat="server">
    <asp:ScriptManager ID="WebService" runat="server" ><Services><asp:ServiceReference Path="~/WebService/Request.asmx" /></Services></asp:ScriptManager>
    <uc1:Loged ID="Loged1" runat="server" />
    <div class="ContentsTopMenu">
		<div id="quick_menu">
			<p>
				<input type="button" id="BTN_Submit" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "wjwkd") %>" onclick="Btn_Submit_Click();"/><br />
				<input type="button" id="BTN_Cancel" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "cnlth") %>" onclick="history.back();" />
			</p>
		</div>
		<div style="width:694px; background-color:White;" >
			<div class="Title">PACKING LIST</div>
			<div style="width:346px; float:left;">
				<div id="Shipper" class="Shipper" >
					<strong>Shipper</strong>
					<div style="padding-left:25px; padding-top:10px; line-height:20px; font-size:12px;  letter-spacing:-1px; ">
						<div id="DivShipperName" ><input type="text" id="TBShipperName" style="font-weight:bold; text-align:left; width:288px;" /></div>
						<div id="DivShipperAddress"><textarea rows="2" cols="48" style="overflow:hidden;" id="TBShipperAddress" ></textarea></div>
					</div>
				</div>
				<div id="Consignee" class="Shipper" >
					<strong>Consignee</strong>
					<div style="padding-left:25px; padding-top:10px; line-height:20px; font-size:12px;  letter-spacing:-1px; ">
						<div id="DivConsigneeName" style="font-weight:bold; "><input type="text" style=" font-weight:bold; text-align:left; width:288px;" id="TBConsigneeName" /></div>
						<div id="DivConsigneeAddress" ><textarea rows="2" cols="48" style="overflow:hidden; " id="TBConsigneeAddress" ></textarea></div>		
					</div>
				</div>
				<div id="NotifyParty" class="Shipper" >
					<strong>Notify Party</strong>
					<div style="padding-left:25px; padding-top:10px; line-height:20px; font-size:12px;  letter-spacing:-1px; ">
						<div id="DivNotifyPartyName" style="font-weight:bold; "><input type="text" id="TBNortifyPartyName" style="text-align:left; width:288px; font-weight:bold;" onclick="this.select();" value="Same as Above" size="20" /></div>
						<div id="DivNotifyPartyAddress" ><textarea rows="2" cols="48" style="overflow:hidden;" id="TBNotifyPartyAddress" ></textarea></div>					
					</div>
				</div>
				<div style="width:173px; height:123px; float:left; ">
					<div class="PortOfLoading" id="PortOfLanding" >
						<strong>Port of loading</strong>
						<div id="DivPortOfLanding" style="padding-left:10px; padding-top:3px;">
							<input type="hidden" id="HPortOfLanding" value="" />
							<select id="StPortOfLanding1"  style="width:80px;" size="1" onchange="SPortCountrySelect(this.value,'StPortOfLanding2', 'HPortOfLanding')">
								<option value="">Country</option>
								<option value="82">KOREA&nbsp;한국</option>
								<option value="86">CHINA&nbsp;中國</option>
								<option value="81">Japan&nbsp;日本</option>
							</select>
							<select id="StPortOfLanding2" style="width:140px;" onchange="SPortSelect(this.value,'HPortOfLanding')"><option value="">:: Port ::</option></select>
						</div>
					</div>
					<div class="PortOfLoading" id="Carrier" >
						<strong>Carrier</strong>
						<div id="DivCarrier" style="padding:10px;"><input id="TBCarrier" type="text" style="width:135px; text-align:left; " /></div>	
					</div>
				</div>
				<div style="width:173px; height:123px; float:right;">
					<div class="FinalDestination" id="FinalDestination" >
						<strong>Finial destination</strong>
						<div id="DivFinialDestination" style="padding-left:10px; padding-top:3px;">
							<input type="hidden" id="HFinialDestination" value="" />
							<select id="StFinialDestination1" style="width:80px;" size="1" onchange="SPortCountrySelect(this.value,'StFinialDestination2' ,'HFinialDestination')">
								<option value="">Country</option>
								<option value="82">KOREA&nbsp;한국</option>
								<option value="86">CHINA&nbsp;中國</option>
								<option value="81">Japan&nbsp;日本</option>
							</select>
							<select id="StFinialDestination2" style="width:140px;" onchange="SPortSelect(this.value,'HFinialDestination')"><option value="">:: Port ::</option></select>
						</div>	
					</div>
					<div class="FinalDestination" id="SailingOnOrAbout" >
						<strong>Sailing on or About</strong>
						<div id="DivSailingOnOrAbout" style="padding:10px;">
							<input id="TBSailingOn" type="text" readonly="readonly" style="cursor:hand; width:100px; " />
						</div>	
					</div>
				</div>
			</div>
			<div style="width:347px; float:right;">
				<div class="InvoiceNo" id="InvoiceNo" style="height:57px; margin-left:-2px; margin-top:-1px; ">
					<strong>Date & No of Invoice</strong>
					<div style="padding-left:25px; padding-top:15px;"><input type="text" id="TBInvoiceNo" style="overflow:hidden; width:287px; text-align:left;" /></div>
				</div>
				<div class="InvoiceNo" id="PaymentTerms" style="height:57px; margin-left:-2px; margin-top:-1px; ">
					<strong>Payment terms</strong>
					<div style="padding-left:25px; padding-top:15px;  ">
						<input type="radio" name="PaymentWayCL" id="TT" value="4" checked="checked" /> <label for="TT">T / T</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="radio" name="PaymentWayCL" id="LC" value="3" /> <label for="LC">L / C</label>
					</div>
				</div>
				<div class="InvoiceNo" id="Buyer" style="height:55px; margin-left:-2px;  margin-top:-1px; ">
					<strong>Buyer</strong>
					<div style="padding-top:5px; padding-left:25px;"><textarea id="TBBuyer"  cols="48" rows="2" style="overflow:hidden;" ></textarea></div>
				</div>
				<div class="InvoiceNo" id="OtherReferences" style="margin-left:-2px;  height:214px; margin-top:-1px; ">
					<strong>Other References</strong>
					<div style="padding-left:25px; padding-top:15px;"><textarea id="TBOtherReferences" cols="48" rows="12" style="overflow:hidden; line-height:20px;" ></textarea></div>
				</div>
			</div>
			<div style="width:694px; clear:both; " id="ItemTable">
				<table border="0" cellpadding="0" cellspacing="0" style="width:694px;">
					<thead>
						<tr style="height:40px;">
							<td class="BB BL" style="width:120px; padding-left:10px; font-weight:bold;">Marks & <br />Number of PKGS</td>
							<td class="BB BL" style="padding:5px; text-align:center; font-weight:bold; ">Description of Goods</td>
							<td class="BB BL" style="width:89px; text-align:center; font-weight:bold; ">Quantity</td>
							<td class="BB BL" style="width:85px; text-align:center; font-weight:bold; ">G-Wright</td>
							<td class="BB BL" style="width:85px; text-align:center; font-weight:bold; ">N-Weight</td>
							<td class="BB BL BR" style="width:85px; text-align:center; font-weight:bold; ">Volume</td>
						</tr>
						<tr style="height:30px;">
							<td class="BL" >&nbsp;</td>
							<td class="BL" >&nbsp;</td>
							<td class="BL" >&nbsp;</td>
							<td class="BL" >&nbsp;</td>
							<td class="BL" style="text-align:center;" ></td>
							<td class="BL BR" style="text-align:center;" >
								<span style="padding-right:10px;" >
									<input type="button" onclick="InsertRow('0');" value="<%=GetGlobalResourceObject("RequestForm", "InsertItem") %>" />
								</span>
							</td>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td class="BL BB BR" colspan="6" style="padding-bottom:10px;">
								<table id="ItemTable[0]" style="background-color:White; width:683px; "  border="0" cellpadding="0" cellspacing="0">
									<thead>
										<tr style="height:30px;" >
											<td style="background-color:#F5F5F5; text-align:center;" ><%=GetGlobalResourceObject("RequestForm", "BoxNo") %></td>
											<td style="background-color:#F5F5F5; text-align:center;" ><%=GetGlobalResourceObject("RequestForm", "Label") %></td>
											<td style="background-color:#F5F5F5; text-align:center;" ><%=GetGlobalResourceObject("RequestForm", "Description") %></td>
											<td style="background-color:#F5F5F5; text-align:center;" ><%=GetGlobalResourceObject("RequestForm", "Material") %></td>
											<td style="background-color:#F5F5F5; text-align:center;" ><%=GetGlobalResourceObject("RequestForm", "Count") %></td>
											<td style="background-color:#F5F5F5; text-align:center;" ><%=GetGlobalResourceObject("RequestForm", "PackingCount") %></td>
											<td style="background-color:#F5F5F5; text-align:center;" ><%=GetGlobalResourceObject("RequestForm", "GrossWeight") %></td>
											<td style="background-color:#F5F5F5; text-align:center;" ><%=GetGlobalResourceObject("RequestForm", "Volume") %></td>
										</tr>
									</thead>
									<tbody>
										<tr>
											<td align="center" >
												<input type="hidden" id="Item[0][0][EachVolume]" />
												<input type="hidden" id="Item[0][0][ItemCode]" />
												<input type="hidden" id="Item[0][0][RequestFormItemsPk]" value="N" />
												<input type="hidden" id="Item[0][0][SUM]" />
												<input type="text" id="Item[0][0][BoxNo]" style="width:40px;" />
											</td>
											<td align="center" ><input type="text" id="Item[0][0][Brand]" style="width:75px;" /></td>
											<td align="center" ><input type="text" id="Item[0][0][ItemName]" style="width:80px;" /></td>
											<td align="center" ><input type="text" id="Item[0][0][Matarial]" style="width:80px;" /></td >
											<td align="center" >
												<input type="text" id="Item[0][0][Quantity]" onkeyup="GetTotal('Quantity');" style="width:50px;" />
												<select id="Item[0][0][QuantityUnit]">
													<option value="40">PCS</option><option value="41">PRS</option><option value="42">SET</option><option value="43">S/F</option>
													<option value="44">YDS</option><option value="45">M</option><option value="46">KG</option><option value="47">DZ</option>
													<option value="48">L</option><option value='49'>Box</option><option value='50'>SQM</option><option value='51'>M2</option><option value='52'>RO</option>
												</select>
											</td>
											<td align="center" >
												<input type="text" id="Item[0][0][PackedCount]" onkeyup="GetTotal('PackedCount');" style="width:40px;" />
												<select id="Item[0][0][PackingUnit]" size="1" >
													<option value="15">CT</option>
													<option value="16">RO</option>
													<option value="17">PA</option>
												</select>
										</td>
										<td align="center" ><input type="text" id="Item[0][0][Weight]" onkeyup="GetTotal('Weight');" style="width:50px;" /></td>
										<td align="center" ><input type="text" id="Item[0][0][Volume]" onkeyup="GetTotal('Volume');" style="width:50px;" /></td>
										</tr>
									</tbody>
								</table>
								<table border="0" style="background-color:White; width:683px;" cellpadding="0" cellspacing="0">
									<tr>
										<td bgcolor="#F5F5F5" height="30" align="right" style="width:340px;" >&nbsp;&nbsp;&nbsp;<%=GetGlobalResourceObject("RequestForm", "Total") %>&nbsp;&nbsp;&nbsp;</td>
										<td bgcolor="#F5F5F5" align="left" >
											<input type="text" id="total[0][Quantity]" style="width:50px;" />
											<span style="padding-left:61px;"><input type="text" id="total[0][PackedCount]" style="width:40px;" /></span>
											<span style="padding-left:51px;"><input type="text" id="total[0][Weight]" style="width:50px;" /></span>
											<span style="padding-left:2px;"><input type="text" id="total[0][Volume]" style="width:50px;" /></span>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</tbody>
				</table>
			</div>
			<table border="0" cellpadding="0" cellspacing="0" style="width:694px; margin-top:-1px;  ">
				<tr style="height:150px;">
					<td class="BL BB" style="text-align:center; font-weight:bold;">&nbsp;</td>
					<td class="BR BB" style="width:350px; text-align:center; font-weight:bold; ">&nbsp;</td>
				</tr>
			</table>
		</div>
		<input type="hidden" id="HCompanyPk" value="<%=MemberInfo[1] %>" />
		<input type="hidden" id="HAccountID" value="<%=MemberInfo[2] %>" />
		<input type="hidden" id="HMode" />
		<input type="hidden" id="HRequestFormPk" />
		<input type="hidden" id="HStepCL" />
		<input type="hidden" id="HSum" />
    </div>
    </form>
</body>
</html>