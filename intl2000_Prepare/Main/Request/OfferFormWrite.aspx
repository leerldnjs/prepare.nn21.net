<%@ Page Language="C#" AutoEventWireup="true" debug="true"  CodeFile="OfferFormWrite.aspx.cs" Inherits="RequestForm_OfferFormWrite" %>
<%@ Register src="../Member/LogedTopMenu.ascx" tagname="Loged" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
	<script src="../Common/jquery-1.4.2.min.js" type="text/javascript"></script>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
    	input{text-align:center; }
    	.tdSubT	{border-bottom:solid 2px #93A9B8;	}
    	.tdSubTGold	 { border-top:solid 2px #DAA520; background-color:#FFFACD; text-align:center; height:20px; border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px;  }
    	.tdSubMainGold{ background-color:#FFFACD; border-bottom:dotted 1px #E8E8E8; }
    	/* #FFFACD */
    	/* #DAA520 */
		.td01{background-color:#f5f5f5; text-align:center; height:20px; border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px; }
    	.td02{width:330px;border-bottom:dotted 1px #E8E8E8;	padding-top:4px;padding-bottom:4px; padding-left:10px; }
		.td023	{width:760px; padding-top:4px;padding-bottom:4px; padding-left:10px; border-bottom:dotted 1px #E8E8E8; }
   	</style>
	<script src="../Common/public.js" type="text/javascript"></script>
   	<script type="text/javascript">
   		//BodyOnload
   		window.onload = function () {
			$(".NavDocu").addClass("active");

   			SetMonetaryUnit('$');
   			var URI = location.href;
   			var UriSpliteByQ = URI.split('?');
   			var MainRoot = UriSpliteByQ[0].split('/');
   			var QueryString = "";
   			if (UriSpliteByQ[1] != undefined) {
   				var EachGet = UriSpliteByQ[1].split('&');
   				for (var i = 0; i < EachGet.length; i++) {
   					if (EachGet[i].split('=')[0] == 'P') {
   						var RequestFormPk = parseInt(EachGet[i].split('=')[1]);
   						LoadOfferSave(RequestFormPk);
   						break;
   					}
   				}
   			}
   		}
   		//신규고객 등록
   		function PopShipperInDocument() {	
   			var DialogResult = "";
   			var retVal = window.showModalDialog('./Dialog/OfferShipperNameSelection.aspx?S=' + form1.TBSessionPk.value, DialogResult, 'dialogWidth=700px; dialogHeight=400px; resizable=0; status=0; scroll=1; help=0;');
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
   		//품명선택
   		function PopOfferItemLibrary(RowIndex) {	
   			var vArguments = '';
   			var retVal = window.open('./Dialog/OfferItemLibrary.aspx?S=' + form1.TBSessionPk.value + '&C=' + form1.HShipperCode.value+"&R="+RowIndex, vArguments, 'Width=700px, Height=400px, resizable=0, status=0, scrollbars=1, help=0;');
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
   		//수화인 선택
   		function PopupCustomerList() {	
   			var DialogResult = 'TB_ImportStaffTEL0';
   			var retVal = window.showModalDialog('../Request/OwnCustomerList.aspx?S=' + form1.TBSessionPk.value, DialogResult, 'Width=500px;Height=500px;resizable=0;status=0;scroll=1;help=0;');
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
		//신규고객 등록
		function PopupCustomerListAdd() {	
			var DialogResult = 'TB_ImportStaffTEL0';
			var retVal = window.showModalDialog('./OwnCustomerListAdd.aspx?pk=' + form1.TBSessionPk.value + '&id=' + form1.HAccountID.value + '&g=N', DialogResult, 'dialogWidth=400px;dialogHeight=400px;resizable=0;status=0;scroll=0;help=0;');
			try {
				var ReturnArray = retVal.split('##');
				document.getElementById('TB_ImportCompanyName').value = ReturnArray[0] + ' (' + ReturnArray[1] + ')';
				document.getElementById('TB_CustomerListPk').value = ReturnArray[2];
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
		function SelectChangeMonetaryUnit(idx) {
			switch (document.getElementById('Item[0][MonetaryUnit]')[idx].value) {
				case '18': SetMonetaryUnit("￥"); break;		case '19': SetMonetaryUnit("$"); break;			case '20': SetMonetaryUnit("￦"); break;			
				case '21': SetMonetaryUnit("Y"); break;			case '22': SetMonetaryUnit("$"); break;			case '23': SetMonetaryUnit("€"); break;
			}
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
		function FloatCalcul(tmp_x, tmp_y, tmp_cal) {
			tmp_x = tmp_x.toString();
			tmp_y = tmp_y.toString();

			tmp_x_count = 1;
			if (tmp_x.indexOf(".") > -1) {
				tmpArr = tmp_x.split(".")
				for (tmp_Loop_cnt = 0; tmp_Loop_cnt < tmpArr[1].length; tmp_Loop_cnt++)
					tmp_x_count *= 10;
			}

			tmp_y_count = 1;
			if (tmp_y.indexOf(".") > -1) {
				tmpArr = tmp_y.split(".")
				for (tmp_Loop_cnt = 0; tmp_Loop_cnt < tmpArr[1].length; tmp_Loop_cnt++)
					tmp_y_count *= 10;
			}

			// 더하기나 빼기가 있을경우 
			if (tmp_cal == "+" || tmp_cal == "-") {
				if (tmp_x_count > tmp_y_count)
					tmp_y_count = tmp_x_count;
				else if (tmp_x_count < tmp_y_count)
					tmp_x_count = tmp_y_count;
			}

			tmp_x = Math.round(parseFloat(tmp_x) * tmp_x_count);
			tmp_y = Math.round(parseFloat(tmp_y) * tmp_y_count);

			tmp_rtn_value = eval("tmp_x " + tmp_cal + " tmp_y");
			tmp_rtn_value = tmp_rtn_value / tmp_x_count;
			tmp_rtn_value = tmp_rtn_value / tmp_y_count;

			return tmp_rtn_value;
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
		function SubmitClick(Which) {
			switch(Which)
			{
				case 'Save':
					if (form1.HRequestFormPk.value == "") {
						document.getElementById("SpSaveClick").style.visibility = "hidden";
						var InsertCheck = false;
						var itemInfoGroup0 = "";
						var RowLength0 = document.getElementById('ItemTable[0]').rows.length - 2;
						var i;
						for (i = 0; i < RowLength0; i++) {
							if (document.getElementById("Item[0][" + i + "][ItemName]").value != "" || document.getElementById("Item[0][" + i + "][Brand]").value != "") {
								itemInfoGroup0 += document.getElementById("Item[0][" + i + "][BoxNo]").value + "@@" +
														   document.getElementById("Item[0][" + i + "][ItemName]").value+ "@@" +
														   document.getElementById("Item[0][" + i + "][Brand]").value + "@@" +
														   document.getElementById("Item[0][" + i + "][Matarial]").value + "@@" +
														   document.getElementById("Item[0][" + i + "][Quantity]").value + "@@" +
														   document.getElementById("Item[0][" + i + "][QuantityUnit]").value + "@@" +
														   document.getElementById("Item[0][" + i + "][PackedCount]").value + "@@" +
														   document.getElementById("Item[0][" + i + "][PackingUnit]").value + "@@" +
														   document.getElementById("Item[0][" + i + "][UnitCost]").value + "@@" +
														   document.getElementById("Item[0][" + i + "][Price]").value + "@@" +
														   document.getElementById("Item[0][" + i + "][Weight]").value + "@@" +
														   document.getElementById("Item[0][" + i + "][Volume]").value + "####";
								InsertCheck = true;
							}
						} 
//						alert(form1.TBSessionPk.value + "_" + form1.HAccountID.value + "_" + form1.HShipperCode.value + "_" + document.getElementById("Item[0][MonetaryUnit]").value + "_" + '0' + "_" + itemInfoGroup0);
//						return false;
						if (InsertCheck) {
							Request.OfferSave(form1.TBSessionPk.value, form1.HAccountID.value, form1.HShipperCode.value, document.getElementById("Item[0][MonetaryUnit]").value, '0', itemInfoGroup0, function (result) {
								window.alert("Successfully saved"); 
								parent.location.href = "./OfferFormWrite.aspx"; 
							}, ONFAILED);
						}
						else {
							alert("We can't save without style no or description.");
							document.getElementById("SpSaveClick").style.visibility = "visible";
						}
					}
					else {
						var InsertCheck = false;
						var itemInfoGroup0 = "";
						var RowLength0 = document.getElementById('ItemTable[0]').rows.length - 2;
						var i;
						for (i = 0; i < RowLength0; i++) {
							if (document.getElementById("Item[0][" + i + "][ItemName]").value != "" || document.getElementById("Item[0][" + i + "][Brand]").value != "") {
								itemInfoGroup0 += document.getElementById("Item[0][" + i + "][ItemPk]").value + "@@" +
																document.getElementById("Item[0][" + i + "][ItemCode]").value + "@@" +
																document.getElementById("Item[0][" + i + "][BoxNo]").value + "@@" +
																document.getElementById("Item[0][" + i + "][ItemName]").value.replace("'", "''") + "@@" +
																document.getElementById("Item[0][" + i + "][Brand]").value + "@@" +
																document.getElementById("Item[0][" + i + "][Matarial]").value + "@@" +
																document.getElementById("Item[0][" + i + "][Quantity]").value + "@@" +
																document.getElementById("Item[0][" + i + "][QuantityUnit]").value + "@@" +
																document.getElementById("Item[0][" + i + "][PackedCount]").value + "@@" +
																document.getElementById("Item[0][" + i + "][PackingUnit]").value + "@@" +
																document.getElementById("Item[0][" + i + "][UnitCost]").value + "@@" +
																document.getElementById("Item[0][" + i + "][Price]").value + "@@" +
																document.getElementById("Item[0][" + i + "][Weight]").value + "@@" +
																document.getElementById("Item[0][" + i + "][Volume]").value + "####";
								InsertCheck = true;
							}
						}
						if (InsertCheck) {
							//여기서 팝업을 띄우고
							var vArguments = form1.HRequestFormPk.value + "$$$$$$" +
														 form1.TBSessionPk.value + "$$$$$$" +
														 form1.HAccountID.value + "$$$$$$" +
														 form1.HShipperCode.value + "$$$$$$" +
														 document.getElementById("Item[0][MonetaryUnit]").value + "$$$$$$" +
														 itemInfoGroup0;
							var retVal = window.showModalDialog('./Dialog/OfferInsertOrUpdate.aspx', vArguments, 'dialogWidth=250px; dialogHeight=120px; resizable=0; status=0; scroll=1; help=0;');
							//document.getElementById("total[0][PackedCount]").value = retVal;
							if (retVal == "M") { alert("Modify Complete"); }
							else if (retVal == "S") { alert("Save Complete"); }
							document.location.href = "./OfferFormWrite.aspx";
							//여기서 팝업을 띄우고
						}
						else {
							alert("We can't save Document without style no or description. /r/n please type style no or description");
							document.getElementById("SpSaveClick").style.visibility = "visible";
						}
					}
					break;
				case "Request":
					if (form1.HRequestFormPk.value == "") {
						alert("명세를 저장한 후에 접수가 가능합니다.");
					}
					else {
						var InsertCheck = false;
						var itemInfoGroup0 = "";
						var RowLength0 = document.getElementById('ItemTable[0]').rows.length - 2;
						var i;
						for (i = 0; i < RowLength0; i++) {
							if (document.getElementById("Item[0][" + i + "][ItemName]").value != "" || document.getElementById("Item[0][" + i + "][Brand]").value != "") {
								itemInfoGroup0 += document.getElementById("Item[0][" + i + "][ItemPk]").value + "@@" +
																document.getElementById("Item[0][" + i + "][ItemCode]").value + "@@" +
																document.getElementById("Item[0][" + i + "][BoxNo]").value + "@@" +
																document.getElementById("Item[0][" + i + "][ItemName]").value.replace("'", "''") + "@@" +
																document.getElementById("Item[0][" + i + "][Brand]").value + "@@" +
																document.getElementById("Item[0][" + i + "][Matarial]").value + "@@" +
																document.getElementById("Item[0][" + i + "][Quantity]").value + "@@" +
																document.getElementById("Item[0][" + i + "][QuantityUnit]").value + "@@" +
																document.getElementById("Item[0][" + i + "][PackedCount]").value + "@@" +
																document.getElementById("Item[0][" + i + "][PackingUnit]").value + "@@" +
																document.getElementById("Item[0][" + i + "][UnitCost]").value + "@@" +
																document.getElementById("Item[0][" + i + "][Price]").value + "@@" +
																document.getElementById("Item[0][" + i + "][Weight]").value + "@@" +
																document.getElementById("Item[0][" + i + "][Volume]").value + "####";
								InsertCheck = true;
							}
						}
						if (InsertCheck) {
							//여기서 팝업을 띄우고
							Request.OfferSaveOrUpdate('SaveAs', form1.HRequestFormPk.value, form1.TBSessionPk.value, form1.HAccountID.value, form1.HShipperCode.value, document.getElementById("Item[0][MonetaryUnit]").value, itemInfoGroup0, function (result) {
								location.href = "./RequestWrite.aspx?M=Write&P=" + form1.HRequestFormPk.value;
							}, function (result) { alert("ERROR : " + result); });
						}
					}
					break;
				case "Invoice":
					var Mode = "JW";
					var StepCL = parseInt(form1.HRequestFormStepCL.value);
					if (StepCL < 0) {
						document.location.href = "./CommercialInvoiceWrite.aspx?&M=" + Mode;
					}
					else {
						if (StepCL % 2 == 0) { Mode = "IW" }
						else if (StepCL % 2 == 1) { Mode = "IR"; }
						document.location.href = "./CommercialInvoiceWrite.aspx?S=" + form1.HRequestFormPk.value + "&M=" + Mode;
					}
					break;
				case "Packing":
					var Mode = "JW";
					var StepCL = parseInt(form1.HRequestFormStepCL.value);
					if (StepCL < 0) {
						document.location.href = "./PackingListWrite.aspx?&M=" + Mode;
					}
					else {
						if ((StepCL / 2) % 2 == 0) { Mode = "PW" }
						else if ((StepCL / 2) % 2 == 1) { Mode = "PR"; }
						document.location.href = "./PackingListWrite.aspx?S=" + form1.HRequestFormPk.value + "&M=" + Mode;
					}
					break;
				case "Offer":
					var Mode = "JW";
					var StepCL = parseInt(form1.HRequestFormStepCL.value);
					if ((StepCL  / 4) % 2 == 0) { Mode = "OW" }
					else if ((StepCL / 4) % 2 == 1) { Mode = "OR"; }
					document.location.href = "./OfferSheetWrite.aspx?S=" + form1.HRequestFormPk.value + "&M=" + Mode;
					break;
				default: break;
			}
		}
		function ONSUCCESS(result) { window.alert("Successfully saved"); parent.location.href = "./OfferFormWrite.aspx"; }
		function ONFAILED(result) { window.alert("ERROR : " + result); }
		function LoadOfferSave(RequestFormPk) { form1.HRequestFormPk.value = RequestFormPk; Request.LoadSavedOffer(RequestFormPk, LoadOfferSaveSuccess, ONFAILED); }
		function DeleteRequestForm(RequestFormPk) {
			if (confirm("This Document will be Deleted")) {
				Request.DeleteRequestForm(RequestFormPk, DeleteRequestFormItemSuccess, ONFAILED);
			}
			//Request.LoadSavedOffer(RequestFormPk, LoadOfferSaveSuccess, ONFAILED);
		}
		function DeleteRequestFormItemSuccess(result) { document.location.href = "./OfferFormWrite.aspx"; }
		var DeleteRowNo;
		function DeleteItems(RowNo) {
			if (form1.HRequestFormPk.value == "" || document.getElementById("Item[0][" + RowNo + "][ItemPk]").value == "N") {
				document.getElementById("Item[0][" + RowNo + "][ItemPk]").value = "";
				document.getElementById("Item[0][" + RowNo + "][ItemCode]").value = "";
				document.getElementById("Item[0][" + RowNo + "][BoxNo]").value = "";
				document.getElementById("Item[0][" + RowNo + "][ItemName]").value = "";
				document.getElementById("Item[0][" + RowNo + "][Brand]").value = "";
				document.getElementById("Item[0][" + RowNo + "][Matarial]").value = "";
				document.getElementById("Item[0][" + RowNo + "][Quantity]").value = "";
				document.getElementById("Item[0][" + RowNo + "][PackedCount]").value = "";
				document.getElementById("Item[0][" + RowNo + "][UnitCost]").value = "";
				document.getElementById("Item[0][" + RowNo + "][Price]").value = "";
				document.getElementById("Item[0][" + RowNo + "][Weight]").value = "";
				document.getElementById("Item[0][" + RowNo + "][Volume]").value = "";
				GetTotal('Quantity');
				GetTotal('Price');
				GetTotal('Weight');
				GetTotal('Volume');
				GetTotal('PackedCount');
			}
			else {
				var InsertCheck = false;
				var itemInfoGroup0 = "";
				var RowLength0 = document.getElementById('ItemTable[0]').rows.length - 2;
				var i;
				if (RowLength0 == 1) {
					Request.DeleteRequestForm(form1.HRequestFormPk.value, DeleteRequestFormItemSuccess, ONFAILED);
					document.location.href = "./OfferFormWrite.aspx?P=" + form1.HRequestFormPk.value;
				}
				else {
					for (i = 0; i < RowLength0; i++) {
						if (i == RowNo) {
							itemInfoGroup0 += "!@#DeleteRow@@" + document.getElementById("Item[0][" + i + "][ItemPk]").value + "####";
						}
						else if (document.getElementById("Item[0][" + i + "][ItemName]").value != "" || document.getElementById("Item[0][" + i + "][Brand]").value != "") {
							itemInfoGroup0 += document.getElementById("Item[0][" + i + "][ItemPk]").value + "@@" +
														document.getElementById("Item[0][" + i + "][ItemCode]").value + "@@" +
														document.getElementById("Item[0][" + i + "][BoxNo]").value + "@@" +
														document.getElementById("Item[0][" + i + "][ItemName]").value.replace("'", "''") + "@@" +
														document.getElementById("Item[0][" + i + "][Brand]").value + "@@" +
														document.getElementById("Item[0][" + i + "][Matarial]").value + "@@" +
														document.getElementById("Item[0][" + i + "][Quantity]").value + "@@" +
														document.getElementById("Item[0][" + i + "][QuantityUnit]").value + "@@" +
														document.getElementById("Item[0][" + i + "][PackedCount]").value + "@@" +
														document.getElementById("Item[0][" + i + "][PackingUnit]").value + "@@" +
														document.getElementById("Item[0][" + i + "][UnitCost]").value + "@@" +
														document.getElementById("Item[0][" + i + "][Price]").value + "@@" +
														document.getElementById("Item[0][" + i + "][Weight]").value + "@@" +
														document.getElementById("Item[0][" + i + "][Volume]").value + "####";
							InsertCheck = true;
						}
					}
				}
				if (InsertCheck) {
					Request.OfferSaveOrUpdate("Modify", form1.HRequestFormPk.value, form1.TBSessionPk.value, form1.HAccountID.value, form1.HShipperCode.value, document.getElementById("Item[0][MonetaryUnit]").value, itemInfoGroup0, function (result) { document.location.href = "./OfferFormWrite.aspx?P=" + form1.HRequestFormPk.value; }, ONFAILED);
				}
			}
		}
		function DeleteRequestFormItemsSuccess(result) {
			
		}
		//상품삭제

		function CountBox(LineNo, value) {
			var reg = /[0-9]-[0-9]/;
			var reg3 = /[0-9]~[0-9]/;
			var reg2 = /[0-9]/;
			if (reg2.test(value) == true) {
				document.getElementById('Item[0][' + LineNo + '][PackedCount]').value = 1;
			}
			if (reg.test(value) == true) {
				var SplitResult = value.split('-');
				var result = SplitResult[1] - SplitResult[0];
				if (result < 0) { result = SplitResult[0] - SplitResult[1]; }
				document.getElementById('Item[0][' + LineNo + '][PackedCount]').value = result + 1;
			}
			if (reg3.test(value) == true) {
				var SplitResult = value.split('~');
				var result = SplitResult[1] - SplitResult[0];
				if (result < 0) { result = SplitResult[0] - SplitResult[1]; }
				document.getElementById('Item[0][' + LineNo + '][PackedCount]').value = result + 1;
			}
			GetTotal('PackedCount');
		}

		function InsertRow(rowC) {		// 상품추가
			var objTable = document.getElementById('ItemTable[' + rowC + ']');
			objTable.appendChild(document.createElement("TBODY"));
			var lastRow = objTable.rows.length;
			var thisLineNo = lastRow - 2;
			var objRow1 = objTable.insertRow(lastRow);
			var objCell1 = objRow1.insertCell(); var objCell2 = objRow1.insertCell(); var objCell3 = objRow1.insertCell(); var objCell4 = objRow1.insertCell(); var objCell5 = objRow1.insertCell();
			var objCell6 = objRow1.insertCell(); var objCell7 = objRow1.insertCell(); var objCell8 = objRow1.insertCell(); var objCell9 = objRow1.insertCell(); var objCell10 = objRow1.insertCell();
			var objCell11 = objRow1.insertCell();
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
			objCell1.align = "center"; objCell2.align = "center"; objCell3.align = "center"; objCell4.align = "center"; objCell5.align = "center"; objCell6.align = "center"; objCell7.align = "center";
			objCell8.align = "center"; objCell9.align = "center"; objCell10.align = "center"; objCell11.align = "center";
			objCell1.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][BoxNo]\" style=\"width:40px;\" onkeyup=\"CountBox('" + thisLineNo + "',this.value)\" />";
			objCell2.innerHTML = "<input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][ItemPk]\" value=\"N\" />"+
												"<input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][ItemCode]\" />"+
												"<input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][EachVolume]\" />"+
												"<input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][EachX]\" />"+
												"<input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][EachY]\" />"+
												"<input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][EachZ]\" />"+
												"<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Brand]\" style=\"width:70px;\"  />";
			objCell3.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][ItemName]\" style=\"width:125px;\" />";
			objCell4.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Matarial]\" style=\"width:70px;\" />";
			objCell5.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Quantity]\" onkeyup=\"QuantityXUnitCost(0," + thisLineNo + "); GetTotal('Quantity'); \" style=\"width:44px;\" /> " +
												"<select id=\"Item[" + rowC + "][" + thisLineNo + "][QuantityUnit]\">" +
													"<option value=\"40\">PCS</option><option value=\"41\">PRS</option><option value=\"42\">SET</option><option value=\"43\">S/F</option>" +
													"<option value=\"44\">YDS</option><option value=\"45\">M</option><option value=\"46\">KG</option><option value=\"47\">DZ</option>" +
													"<option value=\"48\">L</option><option value=\"49\">BOX</option><option value=\"50\">SQM</option><option value=\"51\">M2</option><option value=\"52\">RO</option>" +
												"</select>";
			objCell6.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][PackedCount]\" onkeyup=\"GetTotal('PackedCount');\" style=\"width:40px;\" /> " +
												"<select id=\"Item[" + rowC + "][" + thisLineNo + "][PackingUnit]\" style=\"width:40px;\" >" +
													"<option value=\"15\">CT</option><option value=\"16\">RO</option><option value=\"17\">PA</option>" +
												"</select>";
			objCell7.innerHTML = "<input type=\"text\" style=\"border:0; width:10px; \" id=\"Item[" + rowC + "][" + thisLineNo + "][MonetaryUnit][0]\" value=\"" + MonetaryUnit + "\" /> " +
												"<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][UnitCost]\" onkeyup=\"QuantityXUnitCost(0," + thisLineNo + "); \" style=\"width:45px;\" />";

			objCell8.innerHTML = "<input type=\"text\" style=\"border:0; width:10px; \" id=\"Item[" + rowC + "][" + thisLineNo + "][MonetaryUnit][1]\" value=\"" + MonetaryUnit + "\" /> " +
												"<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Price]\" onkeyup=\"GetTotal('Price');\" style=\"width:55px;\" />";
			objCell9.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Weight]\" onkeyup=\"GetTotal('Weight');\" style=\"width:45px;\" />";
			objCell10.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Volume]\" onkeyup=\"GetTotal('Volume');\" style=\"width:45px;\" />";
			objCell11.innerHTML = "<span style=\"color:Red; cursor:hand;\" title=\"Delete Items\" onclick=\"DeleteItems('" + thisLineNo + "');\" >X</span>";
		}

		function LoadOfferSaveSuccess(result) {
			var TableRow = document.getElementById('ItemTable[0]').rows.length - 2;
			var Row = result.toString().split("@@");
			var Each = Row[0].split("!$^");
			var MonetaryUnit = Each[0];
			form1.HRequestFormStepCL.value = Each[1];
			for (var i = 0; i < Row.length - 1; i++) {
				if (i > TableRow - 1) { InsertRow(0); }
				Each = Row[i + 1].split("!$^");

				/*		0		RFI.RequestFormItemsPk, 
				*		1		RFI.ItemCode, 
				*		2		RFI.MarkNNumber, 
				*		3		RFI.Description, 
				*		4		RFI.Label, 
				*		5		RFI.Material, 
				*		6		RFI.Quantity, 
				*		7		RFI.QuantityUnit, 
				*		8		RFI.PackedCount, 
				*		9		RFI.PackingUnit, 
				*		10	RFI.GrossWeight, 
				*		11	RFI.Volume, 
				*		12	RFI.UnitPrice, 
				*		13	RFI.Amount, 
				*		14	RF.MonetaryUnitCL, 
				*		15	RF.StepCL		*/
				document.getElementById("Item[0][" + i + "][ItemPk]").value = Each[0];
				document.getElementById("Item[0][" + i + "][BoxNo]").value = Each[2];
				document.getElementById("Item[0][" + i + "][ItemCode]").value = Each[1];
				document.getElementById("Item[0][" + i + "][Brand]").value = Each[4];
				document.getElementById("Item[0][" + i + "][ItemName]").value = Each[3];
				document.getElementById("Item[0][" + i + "][Matarial]").value = Each[5];
				document.getElementById("Item[0][" + i + "][Quantity]").value = Each[6];
				document.getElementById("Item[0][" + i + "][QuantityUnit]").value = Each[7];
				document.getElementById("Item[0][" + i + "][PackedCount]").value = Each[8];
				document.getElementById("Item[0][" + i + "][PackingUnit]").value = Each[9];
				document.getElementById("Item[0][" + i + "][UnitCost]").value = Each[12];
				document.getElementById("Item[0][" + i + "][Price]").value = Each[13];
				document.getElementById("Item[0][" + i + "][Weight]").value = Each[10];
				document.getElementById("Item[0][" + i + "][Volume]").value = Each[11];
			}
			for (var j = i; j < TableRow; j++) {
				document.getElementById("Item[0][" + j + "][BoxNo]").value = "";
				document.getElementById("Item[0][" + j + "][ItemCode]").value = "";
				document.getElementById("Item[0][" + j + "][Brand]").value = "";
				document.getElementById("Item[0][" + j + "][ItemName]").value = "";
				document.getElementById("Item[0][" + j + "][Matarial]").value = "";
				document.getElementById("Item[0][" + j + "][Quantity]").value = "";
				//document.getElementById("Item[0][" + j + "][QuantityUnit]").value = "";
				document.getElementById("Item[0][" + i + "][PackedCount]").value = "";
				
				document.getElementById("Item[0][" + j + "][UnitCost]").value = "";
				document.getElementById("Item[0][" + j + "][Price]").value = "";
				document.getElementById("Item[0][" + j + "][Weight]").value = "";
				document.getElementById("Item[0][" + j + "][Volume]").value = "";
			}
			//alert(MonetaryUnit);
			switch (MonetaryUnit) {
				case '18': SetMonetaryUnit("￥"); break;
				case '19': SetMonetaryUnit("$"); break;
				case '20': SetMonetaryUnit("￦"); break;
				case '21': SetMonetaryUnit("Y"); break;
				case '22': SetMonetaryUnit("$"); break;
				case '23': SetMonetaryUnit("€"); break;
			}
			GetTotal('Quantity');
			GetTotal('Price');
			GetTotal('Weight');
			GetTotal('Volume');
			/*document.getElementById("ButtonBar").innerHTML = "<span onclick=\"SubmitClick('Modify')\" id=\"SpSaveClick\" style=\"cursor:hand;\" >Modify</span>" +
																							  "<span onclick=\"SubmitClick('Save')\" id=\"SpSaveClick\" style=\"cursor:hand; padding-left:65px;\" >Save As</span>" +
																							  "<span onclick=\"SubmitClick('Invoice')\" style=\"cursor:hand; padding-left:65px;\" >Commercial Invoice</span>" +
																							  "<span onclick=\"SubmitClick('Packing')\" style=\"cursor:hand; padding-left:65px;\" >Packing List</span>" +
																							  "<span onclick=\"SubmitClick('Offer')\" style=\"cursor:hand; padding-left:65px;\" >Offer Sheet</span>";
																							  */
			document.location.href = "#";
		}		
   	</script>
</head>
<body class="MemberBody" >
    <form id="form1" runat="server">
    <asp:ScriptManager ID="WebService" runat="server" ><Services><asp:ServiceReference Path="~/WebService/Request.asmx" /></Services></asp:ScriptManager>
	<uc1:Loged ID="Loged1" runat="server" />
	<div class="ContentsTopMenu">
		<fieldset>
			<legend><strong><%=GetGlobalResourceObject("Member", "rjfoaudtpwkrtjd") %></strong></legend>
			<input type="hidden" id="DEBUG" onclick="this.select();" />
			<table id="ItemTable[0]" style="background-color:White; width:838px; "  border="0" cellpadding="0" cellspacing="0">
				<thead>
					<tr><td class="tdSubT" colspan="10">
						<span style="float:right; padding-right:10px; " >
							<select id="Item[0][MonetaryUnit]" size="1" onchange="SelectChangeMonetaryUnit(this.selectedIndex)">
								<option value="19"><%=GetGlobalResourceObject("RequestForm", "MonetaryUnitChange") %></option>
								<option value="18">RMB ￥</option><option value="19">USD $</option><option value="20">KRW ￦</option>
								<option value="21">JPY Y</option><option value="22">HKD HK$</option><option value="23">EUR €</option>
							</select>
							<%--<input type="button" onclick="InsertRow('0');" value="<%=GetGlobalResourceObject("RequestForm", "InsertItem") %>" />--%>
							<input type="button" onclick="InsertRow('0');" value="<%=GetGlobalResourceObject("RequestForm", "InsertItem") %>" />
						</span>
					</td></tr>
					<tr style="height:30px;" >
						<td bgcolor="#F5F5F5" align="center" >Box No</td>
						<td bgcolor="#F5F5F5" align="center" ><%=GetGlobalResourceObject("RequestForm", "Label")%></td>
						<td bgcolor="#F5F5F5" align="center" ><%= GetGlobalResourceObject("RequestForm", "Description")%></td>
						<td bgcolor="#F5F5F5" align="center" ><%=GetGlobalResourceObject("RequestForm", "Material") %></td>
						<td bgcolor="#F5F5F5" align="center" ><%=GetGlobalResourceObject("RequestForm", "Count") %></td>
						<td bgcolor="#F5F5F5" align="center" ><%=GetGlobalResourceObject("RequestForm", "PackingCount") %></td>
						<td bgcolor="#F5F5F5" align="center" ><%=GetGlobalResourceObject("RequestForm", "UnitCost") %></td>
						<td bgcolor="#F5F5F5" align="center" ><%=GetGlobalResourceObject("Member", "rmador")%></td>
						<td bgcolor="#F5F5F5" align="center" ><%=GetGlobalResourceObject("RequestForm", "GrossWeight") %>Kg</td>
						<td bgcolor="#F5F5F5" align="center"  >CBM</td>
						<td bgcolor="#F5F5F5" align="center"  >&nbsp;</td>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td align="center" ><input type="text" id="Item[0][0][BoxNo]" style="width:40px;" onkeyup="CountBox('0',this.value)" /></td>
						<td align="center" >
							<input type="hidden" id="Item[0][0][ItemPk]" value="N" />
							<input type="hidden" id="Item[0][0][ItemCode]" />
							<input type="hidden" id="HRequestFormStepCL" value="-1" />
							<input type="hidden" id="Item[0][0][EachVolume]" />
							<input type="hidden" id="Item[0][0][EachX]" />
							<input type="hidden" id="Item[0][0][EachY]" />
							<input type="hidden" id="Item[0][0][EachZ]" />
							<input type="text" id="Item[0][0][Brand]" style="width:70px;"  />
						</td>
						<td align="center" ><input type="text" id="Item[0][0][ItemName]" style="width:125px;" /></td>
						<td align="center" ><input type="text" id="Item[0][0][Matarial]" style="width:70px;" /></td>
						<td align="center" >
							<input type="text" id="Item[0][0][Quantity]" onkeyup="QuantityXUnitCost(0,0);" style="width:44px;" />
							<select id="Item[0][0][QuantityUnit]">
								<option value="40">PCS</option><option value="41">PRS</option><option value="42">SET</option><option value="43">S/F</option>
								<option value="44">YDS</option><option value="45">M</option><option value="46">KG</option><option value="47">DZ</option>
								<option value="48">L</option><option value='49'>BOX</option><option value='50'>SQM</option><option value='51'>M2</option><option value='52'>RO</option>
							</select>
						</td>
						<td align="center" >
							<input type="text" id="Item[0][0][PackedCount]" onkeyup="GetTotal('PackedCount');" style="width:40px;" />
							<select id="Item[0][0][PackingUnit]" style="width:40px;" >
								<option value="15">CT</option>
								<option value="16">RO</option>
								<option value="17">PA</option>
							</select>
						</td>
						<td align="center" >
							<input type="text" style="border:0; width:10px; " id="Item[0][0][MonetaryUnit][0]" />
							<input type="text" id="Item[0][0][UnitCost]" onkeyup="QuantityXUnitCost(0,0);" style="width:45px;" />
						</td>
						<td align="center" >
							<input type="text" style="border:0; width:10px; " id="Item[0][0][MonetaryUnit][1]" />
							<input type="text" id="Item[0][0][Price]" style="width:55px;" onkeyup="GetTotal('Price');" />
						</td>
						<td align="center" ><input type="text" id="Item[0][0][Weight]" onkeyup="GetTotal('Weight');" style="width:45px;" /></td>
						<td align="center" ><input type="text" id="Item[0][0][Volume]" onkeyup="GetTotal('Volume');" style="width:45px;" /></td>
						<td align="center" ><span style="color:Red; cursor:hand;" title="Delete Items" onclick="DeleteItems('0');" >X</span></td>
					</tr>
					<tr>
						<td align="center" ><input type="text" id="Item[0][1][BoxNo]" style="width:40px;" onkeyup="CountBox('1',this.value)" /></td>
						<td align="center" >
							<input type="hidden" id="Item[0][1][ItemPk]" value="N" />
							<input type="hidden" id="Item[0][1][ItemCode]" />
							<input type="hidden" id="Hidden1" value="-1" />
							<input type="hidden" id="Item[0][1][EachVolume]" />
							<input type="hidden" id="Item[0][1][EachX]" />
							<input type="hidden" id="Item[0][1][EachY]" />
							<input type="hidden" id="Item[0][1][EachZ]" />
							<input type="text" id="Item[0][1][Brand]" style="width:70px;"  />
						</td>
						<td align="center" ><input type="text" id="Item[0][1][ItemName]" style="width:125px;" /></td>
						<td align="center" ><input type="text" id="Item[0][1][Matarial]" style="width:70px;" /></td>
						<td align="center" >
							<input type="text" id="Item[0][1][Quantity]" onkeyup="QuantityXUnitCost(0,1);" style="width:44px;" />
							<select id="Item[0][1][QuantityUnit]">
								<option value="40">PCS</option><option value="41">PRS</option><option value="42">SET</option><option value="43">S/F</option>
								<option value="44">YDS</option><option value="45">M</option><option value="46">KG</option><option value="47">DZ</option>
								<option value="48">L</option><option value='49'>BOX</option><option value='50'>SQM</option><option value='51'>M2</option><option value='52'>RO</option>
							</select>
						</td>
						<td align="center" >
							<input type="text" id="Item[0][1][PackedCount]" onkeyup="GetTotal('PackedCount');" style="width:40px;" />
							<select id="Item[0][1][PackingUnit]" style="width:40px;" >
								<option value="15">CT</option>
								<option value="16">RO</option>
								<option value="17">PA</option>
							</select>
						</td>
						<td align="center" >
							<input type="text" style="border:0; width:10px; " id="Item[0][1][MonetaryUnit][0]" />
							<input type="text" id="Item[0][1][UnitCost]" onkeyup="QuantityXUnitCost(0,1);" style="width:45px;" />
						</td>
						<td align="center" >
							<input type="text" style="border:0; width:10px; " id="Item[0][1][MonetaryUnit][1]" />
							<input type="text" id="Item[0][1][Price]" onkeyup="GetTotal('Price');" style="width:55px;" />
						</td>
						<td align="center" ><input type="text" id="Item[0][1][Weight]" onkeyup="GetTotal('Weight');" style="width:45px;" /></td>
						<td align="center" ><input type="text" id="Item[0][1][Volume]" onkeyup="GetTotal('Volume');" style="width:45px;" /></td>
						<td align="center" ><span style="color:Red; cursor:hand;" title="Delete Items" onclick="DeleteItems('1');" >X</span></td>
					</tr>
					<tr>
						<td align="center" ><input type="text" id="Item[0][2][BoxNo]" style="width:40px;" onkeyup="CountBox('2',this.value)" /></td>
						<td align="center" >
							<input type="hidden" id="Item[0][2][ItemPk]" value="N" />
							<input type="hidden" id="Item[0][2][ItemCode]" />
							<input type="hidden" id="Hidden2" value="-1" />
							<input type="hidden" id="Item[0][2][EachVolume]" />
							<input type="hidden" id="Item[0][2][EachX]" />
							<input type="hidden" id="Item[0][2][EachY]" />
							<input type="hidden" id="Item[0][2][EachZ]" />
							<input type="text" id="Item[0][2][Brand]" style="width:70px;"  />
						</td>
						<td align="center" ><input type="text" id="Item[0][2][ItemName]" style="width:125px;" /></td>
						<td align="center" ><input type="text" id="Item[0][2][Matarial]" style="width:70px;" /></td>
						<td align="center" >
							<input type="text" id="Item[0][2][Quantity]" onkeyup="QuantityXUnitCost(0,2);" style="width:44px;" />
							<select id="Item[0][2][QuantityUnit]">
								<option value="40">PCS</option><option value="41">PRS</option><option value="42">SET</option><option value="43">S/F</option>
								<option value="44">YDS</option><option value="45">M</option><option value="46">KG</option><option value="47">DZ</option>
								<option value="48">L</option><option value='49'>BOX</option><option value='50'>SQM</option><option value='51'>M2</option><option value='52'>RO</option>
							</select>
						</td>
						<td align="center" >
							<input type="text" id="Item[0][2][PackedCount]" onkeyup="GetTotal('PackedCount');" style="width:40px;" />
							<select id="Item[0][2][PackingUnit]" style="width:40px;" ><option value="15">CT</option><option value="16">RO</option><option value="17">PA</option></select>
						</td>
						<td align="center" >
							<input type="text" style="border:0; width:10px; " id="Item[0][2][MonetaryUnit][0]" />
							<input type="text" id="Item[0][2][UnitCost]" onkeyup="QuantityXUnitCost(0,2);" style="width:45px;" />
						</td>
						<td align="center" >
							<input type="text" style="border:0; width:10px; " id="Item[0][2][MonetaryUnit][1]" />
							<input type="text" id="Item[0][2][Price]" onkeyup="GetTotal('Price');" style="width:55px;" />
						</td>
						<td align="center" ><input type="text" id="Item[0][2][Weight]" onkeyup="GetTotal('Weight');" style="width:45px;" /></td>
						<td align="center" ><input type="text" id="Item[0][2][Volume]" onkeyup="GetTotal('Volume');" style="width:45px;" /></td>
						<td align="center" ><span style="color:Red; cursor:hand;" title="Delete Items" onclick="DeleteItems('2');" >X</span></td>
					</tr>
				</tbody>
			</table>
			<table border="0" style="background-color:White; width:837px; "  cellpadding="0" cellspacing="0">
				<tr>
					<td style="background-color:#F5F5F5; height:30px; width:355px; text-align:right;">
						<input type="button" value="<%=GetGlobalResourceObject("Member", "wldnrl") %>" onclick="document.location.href='./OfferFormWrite.aspx';" />
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						&nbsp;&nbsp;&nbsp;&nbsp;<%=GetGlobalResourceObject("RequestForm", "Total") %>&nbsp;&nbsp;&nbsp;</td>
					<td bgcolor="#F5F5F5" align="left" >
						<input type="text" style="width:50px;" id="total[0][Quantity]" />
						<span style="padding-left:52px;"><input type="text" style="width:45px;" id="total[0][PackedCount]"  /></span>
						<span style="padding-left:118px;">
							<input type="text" style="border:0; background-color:#F5F5F5; width:10px; " id="Item[0][MonetaryUnit][Total]"  />
							<input type="text" id="total[0][Price]" style="width:55px;" />
							<input type="text" id="total[0][Weight]" style="width:45px;" />
							<input type="text" id="total[0][Volume]" style="width:45px;" />
						</span>
					</td>
				</tr>
			</table>
			<div style="background-color:#777777; height:1px; font-size:1px; "></div>
			<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
			<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
			<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
			<div id="ButtonBar" style="width:818px; padding:10px; text-align:center;">
				<span onclick="SubmitClick('Save')" id="SpSaveClick" style="cursor:hand; font-weight:bold;" ><%=GetGlobalResourceObject("qjsdur", "wjwkd") %></span>
				<span onclick="SubmitClick('Request')" style="cursor:hand; padding-left:65px;" ><%=GetGlobalResourceObject("Member", "rnrwpanffbwjqtn") %></span>
				<span onclick="SubmitClick('Invoice')" style="cursor:hand; padding-left:65px;" ><%=GetGlobalResourceObject("qjsdur", "Invoice") %></span>
				<span onclick="SubmitClick('Packing')" style="cursor:hand; padding-left:65px;" ><%=GetGlobalResourceObject("qjsdur", "PackingList")%></span>
				<%--<span onclick="SubmitClick('Offer')" style="cursor:hand; padding-left:65px;" >Offer Sheet</span>--%>
			</div>
			<input type="hidden" id="CustomerClearanceCompanyName" />
			<input type="hidden" id="CustomerClearancePk" />
			<input type="hidden" id="CustomerClearanceAddress" />
			<input type="hidden" id="TBSessionPk" value="<%=MemberInfo[1] %>" />
			<input type="hidden" id="HAccountID" value="<%=MemberInfo[2] %>" />
			<input type="hidden" id="TB_CustomerListPk" />
			<input type="hidden" id="HConsigneePk" />
			<input type="hidden" id="HConsigneeCode" />
			<input type="hidden" id="HShipperCode" value="<%=SubInfo[0] %>" />
			<input type="hidden" id="HRequestFormPk" />
		</fieldset>
		<p>&nbsp;</p>
		<%=OfferList %>
	</div>
    </form>
</body>
</html>