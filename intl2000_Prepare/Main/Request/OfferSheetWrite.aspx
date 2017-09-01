<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OfferSheetWrite.aspx.cs" Inherits="Request_OfferSheetWrite" %>
<%@ Register src="../Member/LogedTopMenu.ascx" tagname="Loged" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <script src="../Common/public.js" type="text/javascript"></script>
    	<script type="text/javascript">
    		var Port = new Array();
    		Port[86] = new Array(); //86	CHINA
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
    		Port[82][1] = new Option('P82!01', 'KRCJU | 제주공항'); Port[82][2] = new Option('P82!02', 'KRGMP  | 김포공항'); Port[82][3] = new Option('P82!03', 'KRICN | 인천공항');
    		Port[82][4] = new Option('P82!04', 'KRDRS  | 도라산육로'); Port[82][5] = new Option('P82!05', 'KRGSG | 고성육로'); Port[82][6] = new Option('P82!06', 'KRCHA | 제주항');
    		Port[82][7] = new Option('P82!07', 'KRINC | 인천항'); Port[82][8] = new Option('P82!08', 'KRKAN | 광양항'); Port[82][9] = new Option('P82!09', 'KRKUV | 군산항');
    		Port[82][10] = new Option('P82!10', 'KRPTK  | 평택항'); Port[82][11] = new Option('P82!11', 'KRPUS | 부산항'); Port[82][12] = new Option('P82!12', 'KRSHO | 속초항');


    		function delopt(selectCtrl, mTxt) {
    			if (selectCtrl.options) {
    				for (i = selectCtrl.options.length; i >= 0; i--) { selectCtrl.options[i] = null; }
    			}
    			selectCtrl.options[0] = new Option(mTxt, "")
    			selectCtrl.selectedIndex = 0;
    		}

    		function SPortCountrySelect(idx, StTarget) {
    			delopt(document.getElementById(StTarget), ":: Port ::");
    			for (i = 1; i < Port[idx].length; i++) {
    				document.getElementById(StTarget).options[i] = new Option(Port[idx][i].value, Port[idx][i].text);
    			}
    			//document.getElementById(TB_RegionPk).value = document.getElementById(first).options[idx].value;
    		}
    		function SPortSelect(idx, HTarget) { document.getElementById(HTarget).value = idx; }

    		function openModal(obj, file_name, width, height) {//날짜 입력 폼 띄우기
    			height = height + 20;
    			var rand = Math.random() * 4;
    			window.showModalDialog('../Common/' + file_name + '?rand=' + rand, obj, 'dialogWidth=' + width + 'px;dialogHeight=' + height + 'px;resizable=0;status=0;scroll=0;help=0');
    		}

    		function PopupCustomerListAdd() {	//신규고객 등록
    			var DialogResult = '';
    			var retVal = window.showModalDialog('./OwnCustomerListAdd.aspx?pk=' + +form1.HCompanyPk.value + '&id=' + form1.HAccountID.value + '&g=N', DialogResult, 'dialogWidth=400px;dialogHeight=400px;resizable=0;status=0;scroll=0;help=0;');
    			try {
    				var ReturnArray = retVal.split('##');
    				ShowModalApplicant(ReturnArray[2]);
    			}
    			catch (ex) { return false; }
    		}
    		function ShowModalApplicant(CCLPk) {
    			var retVal = window.showModalDialog('./Dialog/OfferSheetTypeApplicant.aspx?S=' + CCLPk, '', 'dialogWidth=400px;dialogHeight=400px;resizable=0;status=0;scroll=0;help=0;');
    			document.getElementById('TB_CustomerListPk').value = CCLPk;
    			var ReturnArray = retVal.split("!!!!");
    			//alert(retVal);
    			form1.HCompanyInDocumentPk.value = ReturnArray[0];
    			form1.TBApplicantName.value = ReturnArray[1];
    			form1.TBApplicantAddress.value = ReturnArray[2];
    		}
    		function ShowModalBankInfo(CompanyInDocumentPk, CompanyName) {
    			if (CompanyInDocumentPk == "" || CompanyName == "") {
    				alert("Please Select BENEFICIARY First");
    				return false;
    			}
    			var retVal = window.showModalDialog('./Dialog/OfferSheetTypeBankInfo.aspx?S=' + CompanyInDocumentPk, CompanyName, 'dialogWidth=400px;dialogHeight=400px;resizable=0;status=0;scroll=0;help=0;');
    			try {
    				var ReturnArray = retVal.split("!!!!");
    				form1.HBankPk.value = ReturnArray[0];
    				form1.TBBankName.value = ReturnArray[1];
    				form1.TBBankAddress.value = ReturnArray[2];
    				form1.TBSwiftCode.value = ReturnArray[3];
    				form1.TBAccountNo.value = ReturnArray[4];
    			}
    			catch (Ex) { }
    		}

    		function PopShipperInDocument() {
    			var DialogResult = '';
    			form1.HBankPk.value = '';
    			form1.TBBankName.value = '';
    			form1.TBBankAddress.value = '';
    			form1.TBSwiftCode.value = '';
    			form1.TBAccountNo.value = '';
    			var retVal = window.showModalDialog('./Dialog/ShipperNameSelection.aspx?S=' + form1.HCompanyPk.value, DialogResult, 'dialogWidth=700px;dialogHeight=400px;resizable=0;status=0;scroll=1;help=0;');
    			try {
    				if (retVal == '1') {
    					form1.HCustomerClearancePk.value = "0";
    					form1.CustomerClearanceCompanyName.value = form1.HClearanceSubstitute.value;
    					form1.HCustomerClearanceAddress.value = "";
    				}
    				else if (retVal == 'D') { PopShipperInDocument(); }
    				else {
    					var ReturnArray = retVal.split('##');
    					form1.HCustomerClearancePk.value = ReturnArray[0];
    					form1.CustomerClearanceCompanyName.value = ReturnArray[1];
    					form1.HCustomerClearanceAddress.value = ReturnArray[2];
    					ShowModalBankInfo(ReturnArray[0], ReturnArray[1]);
    				}
    			}
    			catch (err) {
    				form1.HCustomerClearancePk.value = "";
    				form1.CustomerClearanceCompanyName.value = "";
    				form1.HCustomerClearanceAddress.value = ""; return false;
    			}
    		}

    		function PopupCustomerList() {	//수화인 선택
    			var retVal = window.showModalDialog('./OwnCustomerList.aspx?S=' + form1.HCompanyPk.value, '', 'dialogWidth=500px;dialogHeight=500px;resizable=0;status=0;scroll=0;help=0;');
    			try {
    				var ReturnArray = retVal.split('##');
    				ShowModalApplicant(ReturnArray[2]);
    			}
    			catch (ex) { return false; }
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

    			var MonetaryUnit;
    			switch (document.getElementById('Item[0][MonetaryUnit]')[document.getElementById("Item[0][MonetaryUnit]").selectedIndex].value) {
    				case '18': MonetaryUnit = "￥"; break;
    				case '19': MonetaryUnit = "$"; break;
    				case '20': MonetaryUnit = "￦"; break;
    				case '21': MonetaryUnit = "Y"; break;
    				case '22': MonetaryUnit = "$"; break;
    				case '23': MonetaryUnit = "€"; break;
    				default: MonetaryUnit = ""; break;
    			}
    			objCell1.align = "center";
    			objCell2.align = "center";
    			objCell3.align = "center";
    			objCell4.align = "center";
    			objCell5.align = "center";
    			objCell6.align = "center";

    			objCell1.innerHTML = "<input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][ItemCode]\" /><input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][RequestFormItemsPk]\" value=\"N\" /><input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][SUM]\" /><input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][EachVolume]\" /><input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][EachX]\" /><input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][EachY]\" /><input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][EachZ]\" /><input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Brand]\" style=\"width:95px;\" onclick=\"PopOfferItemLibrary('" + thisLineNo + "')\" />";
    			objCell2.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][ItemName]\" style=\"width:135px;\" onclick=\"PopOfferItemLibrary('" + thisLineNo + "')\" />";
    			objCell3.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Matarial]\" style=\"width:115px;\" />";
    			objCell4.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Quantity]\" onkeyup=\"QuantityXUnitCost(0,0)\" size=\"5\" /> <select id=\"Item[" + rowC + "][" + thisLineNo + "][QuantityUnit]\"><option value=\"40\">PCS</option><option value=\"41\">PRS</option><option value=\"42\">SET</option><option value=\"43\">S/F</option><option value=\"44\">YDS</option><option value=\"45\">M</option><option value=\"46\">KG</option><option value=\"47\">DZ</option><option value=\"48\">L</option><option value=\"49\">BOX</option><option value=\"50\">SQM</option><option value=\"51\">M2</option><option value=\"52\">RO</option></select>";
    			objCell5.innerHTML = "<input type=\"text\" style=\"border:0; width:10px; \" id=\"Item[" + rowC + "][" + thisLineNo + "][MonetaryUnit][0]\" value=\"" + MonetaryUnit + "\" /><input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][UnitCost]\" onkeyup=\"QuantityXUnitCost(0,0)\" style=\"width:55px;\" />";
    			objCell6.innerHTML = "<input type=\"text\" style=\"border:0; width:10px; \" id=\"Item[" + rowC + "][" + thisLineNo + "][MonetaryUnit][1]\" value=\"" + MonetaryUnit + "\"/><input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Price]\" readonly=\"readonly\"  style=\"width:70px;\" />";
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
    			if (form1.HMode.value == "PW") {
    				StepCL = (parseInt(form1.HStepCL.value) + 2);
    			}
    			var accountID = form1.HAccountID.value;
    			var CustomerListPk = form1.TB_CustomerListPk.value;
    			var ConsigneeCode = form1.HConsigneeCode.value;
    			if (ConsigneeCode == "N") {
    				ConsigneeCode = "";
    			}
    			var ConsigneePk = form1.HConsigneePk.value;
    			if (ConsigneePk == "N") {
    				ConsigneePk = "";
    			}
    			var SailingOn = form1.TBSailingOn.value;
    			var PortOfLanding = form1.HPortOfLanding.value;
    			var FilnalDestination = form1.HFinialDestination.value;
    			var NotifyParty = form1.TBNortifyParty.value;
    			if (NotifyParty == "Same as Above") {
    				NotifyParty = "";
    			}
    			var CompanyInDocumentPk = form1.HCustomerClearancePk.value;
    			var paymentWho;
    			if (document.form1.Payment3[0].checked) { paymentWho = "5"; }
    			else if (document.form1.Payment3[1].checked) { paymentWho = "6"; }
    			else if (document.form1.Payment3[2].checked) { paymentWho = "7"; }
    			else if (document.form1.Payment3[3].checked) { paymentWho = "8"; }
    			else { paymentWho = "0"; }

    			var RowTemp1;
    			var RowTemp2;
    			var RowTemp3;
    			var itemInfoGroup = "";
    			var RowLength = document.getElementById('ItemTable[0]').rows.length - 2;
    			/*	0	RFI.RequestFormItemsPk,		1	RFI.ItemCode,	2	RFI.MarkNNumber,	3	RFI.Description,		4	RFI.Label,		5	RFI.Material,		6	RFI.Quantity,		7	RFI.QuantityUnit,	8	RFI.PackedCount,
    			9	RFI.PackingUnit,	10	RFI.GrossWeight,		11	RFI.Volume	*/
    			for (var i = 0; i < RowLength; i++) {
    				RowTemp1 = document.getElementById("Item[0][" + i + "][RequestFormItemsPk]").value + "!" + document.getElementById("Item[0][" + i + "][ItemCode]").value + "!" + document.getElementById("Item[0][" + i + "][BoxNo]").value + "!" + document.getElementById("Item[0][" + i + "][ItemName]").value + "!" + document.getElementById("Item[0][" + i + "][Brand]").value + "!" + document.getElementById("Item[0][" + i + "][Matarial]").value + "!" + document.getElementById("Item[0][" + i + "][Quantity]").value + "!";
    				RowTemp2 = document.getElementById("Item[0][" + i + "][QuantityUnit]").value + "!";
    				RowTemp3 = document.getElementById("Item[0][" + i + "][PackedCount]").value + "!";
    				RowTemp4 = document.getElementById("Item[0][" + i + "][PackingUnit]").value + "!";
    				RowTemp5 = document.getElementById("Item[0][" + i + "][Weight]").value + "!" + document.getElementById("Item[0][" + i + "][Volume]").value;
    				if (RowTemp1 + RowTemp2 + RowTemp3 + "!" + RowTemp5 != document.getElementById("Item[0][" + i + "][SUM]").value && RowTemp1 + RowTemp3 + RowTemp5 != "N!!!!!!!!!") {
    					itemInfoGroup += RowTemp1 + RowTemp2 + RowTemp3 + RowTemp4 + RowTemp5 + "####";
    				}
    			}
    			Request.SaveForOfferWrite("P", RequestFormPk, MonetaryUnit, StepCL, accountID, CustomerListPk, ConsigneeCode, ConsigneePk, SailingOn, PortOfLanding, FilnalDestination, paymentWho, itemInfoGroup, NotifyParty, CompanyInDocumentPk, ONSUCCESS, ONFAILED);
    		}
    		function ONSUCCESS(result) {
    			//window.alert(result);
    			//form1.TBNortifyParty.value = result;
    			parent.location.href = "./CommercialInvoice.aspx?S=" + form1.HRequestFormPk.value;
    		}
    		function ONFAILED(result) { window.alert(result); }

    		function PopOfferItemLibrary(RowIndex) {	//품명선택
    			var vArguments = '';
    			var retVal = window.open('./Dialog/OfferItemLibrary.aspx?S=' + form1.HCompanyPk.value + '&C=' + form1.HCompanyCode.value + "&R=" + RowIndex, vArguments, 'Width=700px, Height=400px, resizable=0, status=0, scrollbars=1, help=0;');
    			try {
    				if (retVal == '1') {
    					form1.CustomerClearancePk.value = "0";
    					form1.CustomerClearanceCompanyName.value = form1.H_ClearanceSubstitute.value;
    				}
    				else if (retVal == 'D') { PopShipperInDocument(); }
    				else {
    					retVal = retVal.replace('!!', '@@');
    					var ReturnArray = retVal.split('@@');
    					form1.CustomerClearancePk.value = ReturnArray[0];
    					form1.CustomerClearanceCompanyName.value = ReturnArray[1];
    					form1.CustomerClearanceAddress.value = ReturnArray[2];
    					document.getElementById("CompanyNameInDocument").innerHTML = ReturnArray[1];
    				}
    			}
    			catch (err) { return false; }
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

    		function BodyOnload() {
    			var URI = location.href;
    			var UriSpliteByQ = URI.split('?');
    			var QueryString = "";
    			var Mode = "JW";
    			if (UriSpliteByQ[1] != undefined) {
    				var Each = UriSpliteByQ[1].split("&");
    				var RequestFormPk = Each[0].substr(2, Each[0].length - 2);
    				Mode = Each[1].substr(2, 2);
    				if (Mode == "JW") {
    					form1.HMode.value = Mode;
    					SetMonetaryUnit("$");
    					return false;
    				}
    				form1.HMode.value = Mode;
    				form1.HRequestFormPk.value = RequestFormPk;
    			}
    			else {
    				form1.HMode.value = Mode;
    				SetMonetaryUnit("$");
    			}
    			//alert(form1.HCompanyPk.value);
    		}
    		function LoadForOfferWriteSUCCESS(result) {
    			//alert(result);
    			//form1.TBApplicantName.value = result;
    			//return false;
    			if (result == "N") {
    				alert(form1.HAlertReject.value);
    				document.location.href = "../Default.aspx";
    				return false;
    			}
    			/*					Packing List
    			* R															W
    			* 0		RF.ShipperPk,								0
    			* 1		RF.MonetaryUnitCL,						1
    			* 2		RF.StepCL ,									2
    			* 3		RF.ConsigneeCCLPk,						
    			* 4		RF.CompanyInDocumentPk, 
    			* 5		RF.ConsigneeCode, 
    			*	6		RF.DepartureDate, 
    			*	7		RF.DepartureRegionCode, 
    			*	8		RF.ArrivalRegionCode, 
    			*	9		RF.PaymentWhoCL, 
    			*	10	RF.NotifyPartyName,
    			*	11	RF.NotifyPartyAddress, 
    			*	12	CID.Name, 
    			*	13	CCL.TargetCompanyName, 
    			*	14	DP.Name as DPName, 
    			*	15	AP.Name as APName , 
    			*	16	RFI.RequestFormItemsPk,				3
    			*	17	RFI.ItemCode,								4
    			*	18	RFI.MarkNNumber,						5
    			*	19	RFI.Description,								6
    			*	20	RFI.Label,										7
    			*	21	RFI.Material,									8
    			*	22	RFI.Quantity,									9
    			*	23	RFI.QuantityUnit,							10
    			*	24	RFI.PackedCount,							11
    			*	25	RFI.PackingUnit,							12
    			*	26	RFI.GrossWeight,							13
    			*	27	RFI.Volume									14	*/

    			var Mode = form1.HMode.value;
    			var Row = result.toString().split("@@");

    			var Each = Row[0].split("!");
    			var MonetaryUnit = Each[0];
    			form1.HSum.value = Row[0];
    			form1.HStepCL.value = Each[1];
    			if (Mode[1] == "R") {
    				/*
    				* 3		RF.ConsigneeCCLPk,						
    				* 4		RF.CompanyInDocumentPk, 
    				* 5		RF.ConsigneeCode, 
    				*	6		RF.DepartureDate, 
    				*	7		RF.DepartureRegionCode, 
    				*	8		RF.ArrivalRegionCode, 
    				*	9		RF.PaymentWhoCL, 
    				*	10	RF.NotifyPartyName,
    				*	11	RF.NotifyPartyAddress, 
    				*	12	CID.Name, 
    				*	13	CCL.TargetCompanyName, 
    				*	14	DP.Name as DPName, 
    				*	15	AP.Name as APName , 
    				*/
    			}
    			for (var i = 0; i < Row.length - 1; i++) {
    				/*	0	RFI.RequestFormItemsPk,	1	RFI.ItemCode,	2	RFI.Description,		3	RFI.Label,	4	RFI.Material,	5	RFI.Quantity,		6	RFI.QuantityUnit,	7	RFI.UnitPrice,	8	RFI.Amount	*/
    				if (i > 0) { InsertRow(0); }
    				document.getElementById("Item[0][" + i + "][SUM]").value = Row[i + 1];
    				Each = Row[i + 1].split("!");
    				document.getElementById("Item[0][" + i + "][RequestFormItemsPk]").value = Each[0];
    				document.getElementById("Item[0][" + i + "][ItemCode]").value = Each[1];
    				document.getElementById("Item[0][" + i + "][Brand]").value = Each[3];
    				document.getElementById("Item[0][" + i + "][ItemName]").value = Each[2];
    				document.getElementById("Item[0][" + i + "][Matarial]").value = Each[4];
    				document.getElementById("Item[0][" + i + "][Quantity]").value = Each[5];
    				document.getElementById("Item[0][" + i + "][UnitCost]").value = Each[7];
    				document.getElementById("Item[0][" + i + "][Price]").value = Each[8];
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
    				document.getElementById("Item[0][" + i + "][QuantityUnit]").selectedIndex = Each[6];
    			}
    			switch (MonetaryUnit) {
    				case '18': SetMonetaryUnit("￥"); break;
    				case '19': SetMonetaryUnit("$"); break;
    				case '20': SetMonetaryUnit("￦"); break;
    				case '21': SetMonetaryUnit("Y"); break;
    				case '22': SetMonetaryUnit("$"); break;
    				case '23': SetMonetaryUnit("€"); break;
    			}
    		}
	</script>
    <style type="text/css">
    	input{text-align:center; }
    	.tdSubT	{border-bottom:solid 2px #93A9B8;	padding-top:10px; }
		.td01{background-color:#f5f5f5; text-align:center; height:20px; width:130px; border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px; }
    	.td02{width:330px;border-bottom:dotted 1px #E8E8E8;	padding-top:4px;padding-bottom:4px; padding-left:10px; }
		.td023	{padding-top:4px; padding-bottom:4px; padding-left:10px;	 border-bottom:dotted 1px #E8E8E8; }
	</style>
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px; overflow:visible;" onload="BodyOnload();" >
    <form id="form1" runat="server">
    <asp:ScriptManager ID="WebService" runat="server" ><Services><asp:ServiceReference Path="~/WebService/Request.asmx" /></Services></asp:ScriptManager>
    <uc1:Loged ID="Loged1" runat="server" />
	<div class="Contents">
		<div style="text-align:center; letter-spacing:2px; font-weight:bold;  font-size:21px; width:600px;" >Offer Sheet</div>
		<table id="Table_Shipper" width="683px" style="background-color:White;" border="0" cellpadding="0" cellspacing="0" >
			<tr><td class="tdSubT" colspan="2">&nbsp;</td></tr>
			<tr style="height:50px; line-height:20px; ">
				<td class="td01" >APPLICANT</td>
				<td class="td023">
					<input type="hidden" id="HCompanyInDocumentPk" />
					<input type="text" id="TBApplicantName" style="text-align:left; border:0px; width:300px; font-weight:bold;  " />
					<input type="text" id="TBApplicantAddress" style="text-align:left; border:0px; width:300px;" />
					<input type="button" value="선택" onclick="PopupCustomerList()" style="width:37px; padding:0px; "  />
					<input type="button" value="등록" onclick="PopupCustomerListAdd()" style="width:37px; padding:0px; " />
					<input type="hidden" id="TB_CustomerListPk" />
					<input type="hidden" id="HConsigneePk" value="N" />
					<input type="hidden" id="HConsigneeCode" value="N" /></td>
			</tr>
			<tr style="line-height:22px; ">
				<td class="td01" rowspan="2" >BENEFICIARY</td>
				<td class="td023">
					<input type="text" id="CustomerClearanceCompanyName" value="<%=Shipper %>" style="text-align:left; border:0px; width:300px; font-weight:bold;" readonly="readonly" /><br />
					<input type="text" id="HCustomerClearanceAddress" style="text-align:left; border:0px; width:300px;" />
					<input type="hidden" id="HCustomerClearancePk" />
					<input type="hidden" id="HClearanceSubstitute" value="<%=GetGlobalResourceObject("RequestForm", "ClearanceSubstitute") %>" />
					<input type="button" onclick="PopShipperInDocument()" style="width:79px; padding:0px; " value="선택" />
				</td>
			</tr>
			<tr style="line-height:22px; ">
				<td class="td023">
					<input type="hidden" id="HBankPk" />
					BANK : <input type="text" id="TBBankName" style="text-align:left; border:0px; width:258px; font-weight:bold;" /><input type="button" style="width:79px; padding:0px;" onclick="ShowModalBankInfo(form1.HCustomerClearancePk.value, form1.CustomerClearanceCompanyName);" value="입력" /><br />
					BANK Address : <input type="text" id="TBBankAddress" style="text-align:left; border:0px; width:300px; font-weight:bold;" /><br />
					SWIFT CODE : <input type="text" id="TBSwiftCode" style="text-align:left; border:0px; width:300px; font-weight:bold;" /><br />
					ACCOUNT NO : <input type="text" id="TBAccountNo" style="text-align:left; border:0px; width:300px; font-weight:bold;" />
				</td>
			</tr>
		</table>
		<table id="ItemTable[0]" style="background-color:White; width:683px; "  border="0" cellpadding="0" cellspacing="0">
			<thead>
				<tr><td class="tdSubT" colspan="6">
					<span style="float:right; padding-right:10px;" >
						<select id="Item[0][MonetaryUnit]" size="1" onchange="SelectChangeMonetaryUnit(this.selectedIndex)">
							<option value="19"><%=GetGlobalResourceObject("RequestForm", "MonetaryUnitChange") %></option>
							<option value="18">RMB ￥</option>
							<option value="19">USD $</option>
							<option value="20">KRW ￦</option>
							<option value="21">JPY Y</option>
							<option value="22">HKD HK$</option>
							<option value="23">EUR €</option>
						</select>
						&nbsp;&nbsp;
						<input type="button" onclick="InsertRow('0');" value="<%=GetGlobalResourceObject("RequestForm", "InsertItem") %>" />
					</span>
				</td></tr>
				<tr style="height:30px;" >
					<td bgcolor="#F5F5F5" align="center" width="110" ><%=GetGlobalResourceObject("RequestForm", "Label") %></td>
					<td bgcolor="#F5F5F5" align="center" ><%=GetGlobalResourceObject("RequestForm", "Description") %></td>
					<td bgcolor="#F5F5F5" align="center" width="130" ><%=GetGlobalResourceObject("RequestForm", "Material") %></td>
					<td bgcolor="#F5F5F5" align="center" width="110"><%=GetGlobalResourceObject("RequestForm", "Count") %></td>
					<td bgcolor="#F5F5F5" align="center" width="80"><%=GetGlobalResourceObject("RequestForm", "UnitCost") %></td>
					<td bgcolor="#F5F5F5" align="center" width="90"><%=GetGlobalResourceObject("RequestForm", "Amount") %></td>
				</tr>
			</thead>
			<tbody>
				<tr>
					<td align="center" >
						<input type="hidden" id="Item[0][0][EachVolume]" />
						<input type="hidden" id="Item[0][0][EachX]" />
						<input type="hidden" id="Item[0][0][EachY]" />
						<input type="hidden" id="Item[0][0][EachZ]" />
						<input type="hidden" id="Item[0][0][ItemCode]" />
						<input type="hidden" id="Item[0][0][RequestFormItemsPk]" value="N" />
						<input type="hidden" id="Item[0][0][SUM]" />
						<input type="text" id="Item[0][0][Brand]" onclick="PopOfferItemLibrary('0')"  style="width:95px;" />
					</td>
					<td align="center" ><input type="text" id="Item[0][0][ItemName]" onclick="PopOfferItemLibrary('0')"  style="width:135px;" /></td>
					<td align="center" ><input type="text" id="Item[0][0][Matarial]" style="width:115px;" /></td >
					<td align="center" >
						<input type="text" id="Item[0][0][Quantity]" onkeyup="QuantityXUnitCost(0,0)" size="5" />
						<select id="Item[0][0][QuantityUnit]">
							<option value="40">PCS</option><option value="41">PRS</option><option value="42">SET</option><option value="43">S/F</option>
							<option value="44">YDS</option><option value="45">M</option><option value="46">KG</option><option value="47">DZ</option>
							<option value="48">L</option><option value='49'>BOX</option><option value='50'>SQM</option><option value='51'>M2</option><option value='52'>RO</option>
						</select>
					</td>
					<td align="center" >
						<input type="text" style="border:0; width:10px; " id="Item[0][0][MonetaryUnit][0]" /><input type="text" id="Item[0][0][UnitCost]" onkeyup="QuantityXUnitCost(0,0)" style="width:55px;" />
					</td>
					<td align="center" >
						<input type="text" style="border:0; width:10px; " id="Item[0][0][MonetaryUnit][1]" /><input type="text" id="Item[0][0][Price]" readonly="readonly"  style="width:70px;" />
					</td>
				</tr>
			</tbody>
		</table>
		<table border="0" style="background-color:White; width:683px;" cellpadding="0" cellspacing="0">
			<tr>
				<td bgcolor="#F5F5F5" height="30" align="right" style="width:403px;" >&nbsp;&nbsp;&nbsp;<%=GetGlobalResourceObject("RequestForm", "Total") %>&nbsp;&nbsp;&nbsp;</td>
				<td bgcolor="#F5F5F5" align="left" >
					<input type="text" id="total[0][Quantity]" style="width:50px;" />
					<span style="padding-left:120px;">
						<input type="text" style="border:0; background-color:#F5F5F5; width:10px; " id="Item[0][MonetaryUnit][Total]"  /><input type="text" id="total[0][Price]" style="width:80px;" />
					</span>
				</td>
			</tr>
		</table>
		<table width="683px" style="background-color:White;" border="0" cellpadding="0" cellspacing="0" >
			<tr><td class="tdSubT" colspan="4">&nbsp;</td></tr>
			<tr>
				<td style="background-color:#f5f5f5; text-align:center; height:20px; border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px;" >Port of loading</td>
				<td style="text-align:center; height:20px; width:215px; border-bottom:dotted 1px #E8E8E8;" >
					<input type="hidden" id="HPortOfLanding" value="" />
					<select style="width:80px;" size="1" onchange="SPortCountrySelect(this.value,'StPortOfLanding2')">
						<option value="">Country</option>
						<option value="82">KOREA&nbsp;한국</option>
						<option value="86">CHINA&nbsp;中國</option>
						<option value="81">Japan&nbsp;日本</option>
					</select>
					<select id="StPortOfLanding2" style="width:120px;" onchange="SPortSelect(this.value,'HPortOfLanding')"><option value="">:: Port ::</option></select>
				</td>
				<td style="background-color:#f5f5f5; text-align:center; height:20px; border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px;" >Destination</td>
				<td style="text-align:center; height:20px; width:215px; border-bottom:dotted 1px #E8E8E8;" >
					<input type="hidden" id="HFinialDestination" value="" />
					<select style="width:80px;" size="1" onchange="SPortCountrySelect(this.value,'StFinialDestination2' ,'HFinialDestination')">
						<option value="">Country</option>
						<option value="82">KOREA&nbsp;한국</option>
						<option value="86">CHINA&nbsp;中國</option>
						<option value="81">Japan&nbsp;日本</option>
					</select>
					<select id="StFinialDestination2" style="width:120px;" onchange="SPortSelect(this.value,'HFinialDestination')"><option value="">:: Port ::</option></select>
				</td>
			</tr>
			<tr>
				<td style="background-color:#f5f5f5; text-align:center; height:20px; border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px;" >LASTEST DATE<br /> OF SHIPMENT</td>
				<td class="Line1E8E8E8" colspan="3" style="height:20px; text-align:left; padding-left:4px; ">
					<input id="TBSailingOn" type="text" readonly="readonly" style="cursor:hand; width:100px; " onclick="openModal(TBSailingOn, './calendar.html', 240, 220); return false;" />
				</td>				
			</tr>
			<tr>
				<td style="background-color:#f5f5f5; text-align:center; height:20px; border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px;" >PARTIAL SHIPMENTS</td>
				<td style="height:20px; width:215px; border-bottom:dotted 1px #E8E8E8; padding-left:4px; " >
					<select style="width:110px;" id="StPartialShipments" size="1">
						<option value="1">ALLOWED</option>
						<option value="0">PROHIBITED</option>
					</select>
				</td>
				<td style="background-color:#f5f5f5; text-align:center; height:20px; border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px;" >TRANSSHIPMENT</td>
				<td style="height:20px; width:215px; border-bottom:dotted 1px #E8E8E8; padding-left:4px; " >
					<select style="width:110px;" id="StTransshipment" size="1">
						<option value="1">ALLOWED</option>
						<option value="0">PROHIBITED</option>
					</select>
				</td>
			</tr>
		</table>
		<input type="button" id="BTN_SUBMIT" value="오퍼시트 만들기 " onclick="document.location.href='./offersheet.aspx';" style="width:130px; height:30px;  " />
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
		<input type="hidden" id="HCompanyPk" value="<%=MemberInfo[1] %>" />
		<input type="hidden" id="HCompanyCode" value="<%=SubInfo[0] %>" />
		<input type="hidden" id="HAccountID" value="<%=MemberInfo[2] %>" />
		<input type="hidden" id="HMode" />
		<input type="hidden" id="HRequestFormPk" />
		<input type="hidden" id="HStepCL" />
		<input type="hidden" id="HSum" />
    </div>
    </form>
</body>
</html>
