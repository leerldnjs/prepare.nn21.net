<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FixCharge_old.aspx.cs" Inherits="Admin_Dialog_FixCharge_old" Debug="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		var MaxCriterionValue;
		var GuideLine;
		var Criterion;
		var STANDARDPRICEHEADPKnCOLUMN = new Array();
		var OVERWEIGHT = new Array();
		var SUGICHECK = new Array();
		var DESCRIPTIONROW = new Array();
		var PAYMENTWHO = new Array();
		var ACCOUNTID;
		var ISMODIFY = false;
		var ISCALC = false;
		var CHECKPERSONALLY = false;
		var EXCHANGERATESUM = "";
		var EmptyCostListHTML = "<table id=\"TB_StandardPrice\" border=\"1\" cellpadding=\"0\" cellspacing=\"0\" style='width:570px; '  >" +
									"<thead>" +
										"<tr style=\"height:20px;\" >" +
											"<td class='THead1' style=\"text-align:center;\" >Description</td>" +
											"<td class='THead1' style=\"text-align:center; width:160px; \" >Shipper Charge</td>" +
											"<td class='THead1' style=\"text-align:center; width:160px; \" >Consignee Charge</td>" +
										"</tr>" +
									"</thead>" +
									"<tbody>" +
										"<tr>" +
											"<td style=\"text-align:center;\">" +
												"<input type=\"hidden\" id=\"StandardPrice[0][ColumnName]\" value='SUGI'  />" +
												"<input type=\"hidden\" id=\"StandardPrice[0][ShipperColumnPk]\" />" +
												"<input type=\"hidden\" id=\"StandardPrice[0][ConsigneeColumnPk]\" />" +
												//"<input type=\"text\" id=\"StandardPrice[0][Price]\" />" +
												"<input type=\"text\" id=\"StandardPrice[0][Description]\" style=\"width:220px;\" onchange=\"SetSugi('0');\"/>" +
											"</td>" +
											"<td style=\"text-align:center;\">" +
												"<select id=\"StandardPrice[0][ShipperMonetaryUnit]\" style=\"width:70px;\" >" +
												"<option value=\"18\">RMB ￥</option><option value=\"19\">USD $</option><option value=\"20\">KRW ￦</option>" +
												"<option value=\"21\">JPY Y</option><option value=\"22\">HKD HK$</option><option value=\"23\">EUR €</option>" +
												"</select>" +
												"<input type=\"text\" id=\"StandardPrice[0][ShipperPrice]\" style=\"width:80px;\" /></td>" +
											"<td style=\"text-align:center;\">" +
												"<select id=\"StandardPrice[0][ConsigneeMonetaryUnit]\"style=\"width:70px;\" >" +
												"<option value=\"18\">RMB ￥</option><option value=\"19\">USD $</option><option value=\"20\">KRW ￦</option>" +
												"<option value=\"21\">JPY Y</option><option value=\"22\">HKD HK$</option><option value=\"23\">EUR €</option>" +
												"</select>" +
												"<input type=\"text\" id=\"StandardPrice[0][ConsigneePrice]\" style=\"width:80px;\" /></td>" +
										"</tr>" +
									"</tbody>" +
									"</table>";
		window.onload = function () {
			
			if (dialogArguments.substr(0, 12) == "ModifyCharge") {
				ISMODIFY = true;
				ACCOUNTID = dialogArguments.substr(12);
				LoadOurStaff();
				/*
				Admin.LoadCalculated(form1.HRequestFormPk.value, function (result) {

					var Each = result[0].split("#@!");
					var sugicheckforLoad = "";
					if (Each[0] == "") { Each[0] = "0"; sugicheckforLoad = "0"; }
					document.getElementById("st_StandardPrice").value = Each[0];
					StandardPriceSelectChange(Each[0]);

					EXCHANGERATESUM = Each[3];
					alert("Loaded Complite");
					document.getElementById("ST_CriterionValue").value = Each[2].substr(0, Each[2].indexOf("!"));
					document.getElementById("ST_OverWeight").value = Each[2].substr(Each[2].indexOf("!") + 1);
					document.getElementById("st_TotalShipperMonetaryUnit").value = Each[4];
					document.getElementById("TotalShipperAmount").value = NumberFormat(Each[5]);
					document.getElementById("st_TotalConsigneeMonetaryUnit").value = Each[6];
					document.getElementById("TotalConsigneeAmount").value = NumberFormat(Each[7]);
					if (Each[8] + "" != "") { form1.st_ReceiveBankShipper.value = Each[8]; }
					if (Each[9] + "" != "") { form1.st_ReceiveBankConsignee.value = Each[9]; }
					document.getElementById("st_WhoWillPayTariff").value = Each[10];
					form1.TBComment_Shipper.value = Each[11] + "";
					form1.TBComment_Consignee.value = Each[12] + "";


					var lastRow = document.getElementById('TB_StandardPrice').rows.length - 1;
					for (var i = 1; i < result.length; i++) {
						Each = result[i].split("#@!");
						var gubunCL = Each[1];
						//도착지 배달비
						if (Each[5] == "D") {
							if (gubunCL == "200") {
								form1.TBDeleveryChargeShipper.value = NumberFormat(Each[3]);
								form1.HDeliveryChargeShipperPk.value = Each[0];
							}
							else {
								form1.TBDeleveryChargeConsignee.value = NumberFormat(Each[3]);
								form1.HDeliveryChargeConsigneePk.value = Each[0];
							}
							continue;
						}
						//도착지 배달비
						//	if (Each[5] != "" && sugicheckforLoad == "") {

						var CheckIsIn = false;
						for (var j = 0; j < lastRow; j++) {
							if (document.getElementById("StandardPrice[" + j + "][ColumnName]").value == Each[5]) {
								if (gubunCL == "200") {
									document.getElementById("StandardPrice[" + j + "][ShipperColumnPk]").value = Each[0];
									document.getElementById("StandardPrice[" + j + "][ShipperMonetaryUnit]").value = Each[4];
									document.getElementById("StandardPrice[" + j + "][ShipperPrice]").value = NumberFormat(Each[3]);
									CheckIsIn = true;
									break;
								}
								else {
									document.getElementById("StandardPrice[" + j + "][ConsigneeColumnPk]").value = Each[0];
									document.getElementById("StandardPrice[" + j + "][ConsigneeMonetaryUnit]").value = Each[4];
									document.getElementById("StandardPrice[" + j + "][ConsigneePrice]").value = NumberFormat(Each[3]);
									CheckIsIn = true;
									break;
								}
							}
						}
						if (!CheckIsIn) {
							if (i > 1) { InsertRow(); lastRow++; }
							document.getElementById("StandardPrice[" + (lastRow - 1) + "][Description]").value = Each[2];
							if (gubunCL == "200") {
								document.getElementById("StandardPrice[" + (lastRow - 1) + "][ShipperColumnPk]").value = Each[0];
								document.getElementById("StandardPrice[" + (lastRow - 1) + "][ShipperMonetaryUnit]").value = Each[4];
								document.getElementById("StandardPrice[" + (lastRow - 1) + "][ShipperPrice]").value = NumberFormat(Each[3]);
							}
							else {
								document.getElementById("StandardPrice[" + (lastRow - 1) + "][ConsigneeColumnPk]").value = Each[0];
								document.getElementById("StandardPrice[" + (lastRow - 1) + "][ConsigneeMonetaryUnit]").value = Each[4];
								document.getElementById("StandardPrice[" + (lastRow - 1) + "][ConsigneePrice]").value = NumberFormat(Each[3]);
							}
						}
					}
				}, function (result) { alert("ERROR : " + result); });
				*/
			}
			else {
				ACCOUNTID = dialogArguments;
				LoadOurStaff();
			}
		}
		function StandardPriceSelectChange(value) {
			ISCALC = false;
			document.getElementById("PnCostList").innerHTML = EmptyCostListHTML;
			document.getElementById("PnAddRow").innerHTML = " <input type=\"button\" value=\"+\" onclick=\"InsertRow();\" />";
			if (value == "0") {
				CHECKPERSONALLY = true;
				document.getElementById("PnDangakijun").innerHTML = "	<input type=\"hidden\" id=\"ST_CriterionValue\" />" +
																										"	<input type=\"hidden\" id=\"ST_WeightOrVolume\" />" +
																										"	<input type=\"hidden\" id=\"ST_OverWeight\" />";
				return false;
			}
			CHECKPERSONALLY = false;
			document.getElementById("PnDangakijun").innerHTML = "단가기준 : <input type=\"text\" id=\"ST_CriterionValue\" style=\"width:50px; text-align:right; \" onfocus=\"this.select();\" onkeyup=\"form1.ST_OverWeight.value=this.value;\" >" +
																										"<input type=\"text\" style=\"width:30px; border:0px; \" id=\"ST_WeightOrVolume\" readonly=\"readonly\"  />" +
																										"&nbsp;&nbsp;&nbsp;&nbsp;" +
																										"<input type=\"text\" style=\"width:40px; border:0px;\" id=\"OverWeight\" value=\"" + form1.rhkwndfid.value + "\" />" +
																										"<input type=\"text\" id=\"ST_OverWeight\" onfocus=\"this.select();\" style=\"width:50px; text-align:right; \"  />" +
																										"<input type=\"button\" value=\"Confirm\" onclick=\"StandardPriceGetValue();\"  />";
			Admin.LoadStandardPrice(value, function (result) {
				//var Standard = result.substr(0, result.indexOf("%%%%"));
				//0.5000####39@@0~5!19@운송비#运费$O/F C%^####39@@0~5!19@운임#运费$O/F CHARGE%うんそうひ^####39@@0~0!20@창고료#$storageCharge%^@@0~15!20@통관수수료#$%^
				var StandardPriceGroup = result.split("####");
				//alert(StandardPriceGroup);
				GuideLine = StandardPriceGroup[0].split("!")[0];
				MaxCriterionValue = 66;
				//		    	delopt(form1.ST_CriterionValue, "CriterionValue");
				//		    	delopt(form1.ST_OverWeight, "CriterionValue");
				//		    	var CriterionTemp = 0;
				//		    	var count = 0;
				//		    	while (CriterionTemp < MaxCriterionValue) {
				//		    		CriterionTemp = CriterionTemp + parseFloat(GuideLine);
				//		    		form1.ST_CriterionValue.options[count] = new Option(CriterionTemp, CriterionTemp);
				//		    		form1.ST_OverWeight.options[count] = new Option(CriterionTemp, CriterionTemp);
				//		    		count++;
				//		    	}

				var CountRow = 0;
				for (var i = 1; i < StandardPriceGroup.length; i++) {
					var EachRow = StandardPriceGroup[i].split("@@");

					if (EachRow[0] == "39") {
						form1.ST_WeightOrVolume.value = "CBM";
						form1.OverWeight.value = form1.rhkwndfid.value;
						form1.HCriterionValue.value = form1.HVolume.value;
					}
					else {
						form1.ST_WeightOrVolume.value = "Kg";
						form1.OverWeight.value = form1.rhkcpwjr.value;
						form1.HCriterionValue.value = form1.HGrossWeight.value;
					}
					for (var j = 1; j < EachRow.length; j++) {
					   OVERWEIGHT[CountRow] = EachRow[j].substr(0, 1);
					   SUGICHECK[CountRow] = EachRow[j].substr(EachRow[j].indexOf('+') + 1, EachRow[j].indexOf('~') - EachRow[j].indexOf('+') - 1);
						//한자리 숫자 0: 국제운송비 1:출발지 비용 2: 도착지 비용 + 컬럼 피케이 + 컬럼 네임
						STANDARDPRICEHEADPKnCOLUMN[CountRow] = EachRow[j].substr(EachRow[j].indexOf('*') + 1, EachRow[j].length - EachRow[j].indexOf('*') + 1);
						//alert(STANDARDPRICEHEADPKnCOLUMN[CountRow]);
						var MonetaryUnit = EachRow[j].substr(EachRow[j].indexOf('!') + 1, EachRow[j].indexOf('@') - EachRow[j].indexOf('!') - 1);
						var DescriptionKOR = EachRow[j].substr(EachRow[j].indexOf('@') + 1, EachRow[j].indexOf('#') - EachRow[j].indexOf('@') - 1);
						var DescriptionENG = EachRow[j].substr(EachRow[j].indexOf('$') + 1, EachRow[j].indexOf('%') - EachRow[j].indexOf('$') - 1);
						if (DescriptionENG == "") {
							DescriptionENG = DescriptionKOR;
						}
						DESCRIPTIONROW[CountRow] = EachRow[j].substr(EachRow[j].indexOf('@') + 1);
						if (CountRow >= document.getElementById('TB_StandardPrice').rows.length - 1) {
							InsertRow();
						}
						PAYMENTWHO[CountRow] = EachRow[j].substr(EachRow[j].indexOf('~') + 1, EachRow[j].indexOf('!') - EachRow[j].indexOf('~') - 1);
						var ConsigneePercent;
						var ShipperPercent
						var temp = parseInt(PAYMENTWHO[CountRow]);

						if (form1.EXW.checked == true) {
							if (parseInt(temp / 8) == 1) { ShipperPercent = "0"; ConsigneePercent = "100"; } else {
								ShipperPercent = "100"; ConsigneePercent = "0";
							}
						}
						else if (form1.DDP.checked == true) {
							if (parseInt((temp / 4)) % 2 == 1) { ShipperPercent = "0"; ConsigneePercent = "100"; } else {
								ShipperPercent = "100"; ConsigneePercent = "0";
							}
						}
						else if (form1.CNF.checked == true) {
							if (parseInt((temp / 2)) % 2 == 1) { ShipperPercent = "0"; ConsigneePercent = "100"; } else {
								ShipperPercent = "100"; ConsigneePercent = "0";
							}
						}
						else if (form1.FOB.checked == true) {
							if (temp % 2 == 1) { ShipperPercent = "0"; ConsigneePercent = "100"; } else {
								ShipperPercent = "100"; ConsigneePercent = "0";
							}
						}
						else { ShipperPercent = "0"; ConsigneePercent = "0"; }

						//document.getElementById("StandardPrice[" + CountRow + "][ShipperPercent]").value = ShipperPercent;
						//document.getElementById("StandardPrice[" + CountRow + "][ConsigneePercent]").value = ConsigneePercent;
						document.getElementById("StandardPrice[" + CountRow + "][Description]").value = DescriptionENG;
						document.getElementById("StandardPrice[" + CountRow + "][ShipperMonetaryUnit]").value = MonetaryUnit;
						document.getElementById("StandardPrice[" + CountRow + "][ConsigneeMonetaryUnit]").value = MonetaryUnit;
						document.getElementById("StandardPrice[" + CountRow + "][ColumnName]").value = STANDARDPRICEHEADPKnCOLUMN[CountRow];
						//document.getElementById("StandardPrice[" + CountRow + "][ShipperPrice]").value = OVERWEIGHT[CountRow];
						CountRow++;
					}
				}
				if (form1.ST_WeightOrVolume.value == "CBM") {
					form1.ST_CriterionValue.value = NumberFormat(form1.HVolume.value);
					form1.ST_OverWeight.value = NumberFormat(form1.HOverWeight.value);
				}
				else {
					form1.ST_CriterionValue.value = NumberFormat(form1.HGrossWeight.value);
					form1.ST_OverWeight.value = NumberFormat(form1.HGrossWeight.value);
				}
			}, function (result) { alert("ERROR : " + result); });
			//alert("123");
			var TotalValue;
			if (form1.ST_WeightOrVolume.value == "CBM") { TotalValue = form1.HVolume.value; }
			else { TotalValue = form1.HGrossWeight.value; }
			form1.ST_CriterionValue.value = TotalValue;
			//alert(value +" "+ TotalValue);
		}
		function NumberFormat(number) { if (number == "" || number == "0") { return number; } else { return parseInt(number * 10000) / 10000; } }
		function LoadOurStaff() {
			Admin.LoadOurStaffSBankAccountForFixCharge(form1.HRequestFormPk.value, function (result) {
				var Account = result.split("##");
				//selectCtrl.options[0] = new Option(mTxt, "")
				var OptionCountS = -1;
				var OptionCountC = -1;
				var Count = 0;
				while (true) {
					if (Count == Account.length) {
						break;
					}
					var Each = Account[Count].split("!");
					if (Each[0] == "10") {
						OptionCountS++;
						form1.st_ReceiveBankShipper.options[OptionCountS] = new Option(Each[5] + " " + Each[2] + " " + Each[4] + " " + Each[3], Each[1]);
					}
					else {
						OptionCountC++;
						form1.st_ReceiveBankConsignee.options[OptionCountC] = new Option(Each[5] + " " + Each[2] + " " + Each[4] + " " + Each[3], Each[1]);
					}
					Count++;
					continue;
				}
			}, function (result) { alert(result); });
		}
		function delopt(selectCtrl, mTxt) {
			if (selectCtrl.options) {
				for (i = selectCtrl.options.length; i >= 0; i--) { selectCtrl.options[i] = null; }
			}
			selectCtrl.options[0] = new Option(mTxt, "")
			selectCtrl.selectedIndex = 0;
		}

		function StandardPriceGetValue() {
			StandardPricePk = document.getElementById('st_StandardPrice').value;
			TotalValue = document.getElementById('ST_CriterionValue').value;
			OverWeightValue = document.getElementById('ST_OverWeight').value;

			//alert(StandardPricePk + " " + TotalValue + " " + OverWeightValue);
			//return false;
			Admin.LoadStandardPriceEachValue(StandardPricePk, TotalValue, OverWeightValue, function (result) {
				var RowCount = document.getElementById('TB_StandardPrice').rows.length - 1;
				for (var i = 0; i < result.length; i++) {
					var Each = result[i].split("*");
					for (var j = 0; j < RowCount; j++) {
						if (Each[0] == STANDARDPRICEHEADPKnCOLUMN[j]) {
							var EachPrice;
							if (TotalValue != OverWeightValue && OVERWEIGHT[j] == "1") {
								EachPrice = Each[2];
							}
							else {
								EachPrice = Each[1];
							}							
							var temp = parseInt(PAYMENTWHO[j]);
							if (form1.EXW.checked == true) {
								if (parseInt(temp / 8) == 1) {
									document.getElementById("StandardPrice[" + j + "][ShipperPrice]").value = 0;
									document.getElementById("StandardPrice[" + j + "][ConsigneePrice]").value = EachPrice;
								} else {
									document.getElementById("StandardPrice[" + j + "][ShipperPrice]").value = EachPrice;
									document.getElementById("StandardPrice[" + j + "][ConsigneePrice]").value = 0;
								}
							}
							else if (form1.DDP.checked == true) {
								if (parseInt((temp / 4)) % 2 == 1) {
									document.getElementById("StandardPrice[" + j + "][ShipperPrice]").value = 0;
									document.getElementById("StandardPrice[" + j + "][ConsigneePrice]").value = EachPrice;
								} else {
									document.getElementById("StandardPrice[" + j + "][ShipperPrice]").value = EachPrice;
									document.getElementById("StandardPrice[" + j + "][ConsigneePrice]").value = 0;
								}
							}
							else if (form1.CNF.checked == true) {
								if (parseInt((temp / 2)) % 2 == 1) {
									document.getElementById("StandardPrice[" + j + "][ShipperPrice]").value = 0;
									document.getElementById("StandardPrice[" + j + "][ConsigneePrice]").value = EachPrice;
								} else {
									document.getElementById("StandardPrice[" + j + "][ShipperPrice]").value = EachPrice;
									document.getElementById("StandardPrice[" + j + "][ConsigneePrice]").value = 0;
								}
							}
							else if (form1.FOB.checked == true) {
								if (temp % 2 == 1) {
									document.getElementById("StandardPrice[" + j + "][ShipperPrice]").value = 0;
									document.getElementById("StandardPrice[" + j + "][ConsigneePrice]").value = EachPrice;
								} else {
									document.getElementById("StandardPrice[" + j + "][ShipperPrice]").value = EachPrice;
									document.getElementById("StandardPrice[" + j + "][ConsigneePrice]").value = 0;
								}
							}
							else {
								document.getElementById("StandardPrice[" + j + "][ShipperPrice]").value = 0;
								document.getElementById("StandardPrice[" + j + "][ConsigneePrice]").value = EachPrice;
							}
						}
					}
				}

				if (TotalValue != OverWeightValue) {
					resultSplit = result[1].split("####");
					var RowCount = 0;
					for (var i = 0; i < resultSplit.length - 1; i++) {
						var Each = resultSplit[i].split("@@");
						form1.ST_OverWeight.value = Each[1];
						for (var j = 2; j < Each.length; j++) {
							//alert(document.getElementById("StandardPrice[" + RowCount + "][HSugi]").value);
							var StandardPriceHeadPkNColumn = Each[j].substr(Each[j].indexOf('*') + 1, Each[j].length - Each[j].indexOf('*') - 1);
							if (StandardPriceHeadPkNColumn == STANDARDPRICEHEADPKnCOLUMN[RowCount]) {
								if (OVERWEIGHT[RowCount] == "1") {
									var EachPrice = Each[j].substr(0, Each[j].indexOf("*"));

									//document.getElementById("StandardPrice[" + RowCount + "][Price]").value = EachPrice;
									//									var ShipperP = parseInt(document.getElementById("StandardPrice[" + RowCount + "][ShipperPercent]").value);
									//									var ConsigneeP = parseInt(document.getElementById("StandardPrice[" + RowCount + "][ConsigneePercent]").value);
									if (document.getElementById("StandardPrice[" + RowCount + "][ColumnName]").value != "SUGI") {
										var temp = parseInt(PAYMENTWHO[RowCount]);
										if (form1.EXW.checked == true) {
											if (parseInt(temp / 8) == 1) {
												document.getElementById("StandardPrice[" + RowCount + "][ShipperPrice]").value = 0;
												document.getElementById("StandardPrice[" + RowCount + "][ConsigneePrice]").value = EachPrice;
											} else {
												document.getElementById("StandardPrice[" + RowCount + "][ShipperPrice]").value = EachPrice;
												document.getElementById("StandardPrice[" + RowCount + "][ConsigneePrice]").value = 0;
											}
										}
										else if (form1.DDP.checked == true) {
											if (parseInt((temp / 4)) % 2 == 1) {
												document.getElementById("StandardPrice[" + RowCount + "][ShipperPrice]").value = 0;
												document.getElementById("StandardPrice[" + RowCount + "][ConsigneePrice]").value = EachPrice;
											} else {
												document.getElementById("StandardPrice[" + RowCount + "][ShipperPrice]").value = EachPrice;
												document.getElementById("StandardPrice[" + RowCount + "][ConsigneePrice]").value = 0;
											}
										}
										else if (form1.CNF.checked == true) {
											if (parseInt((temp / 2)) % 2 == 1) {
												document.getElementById("StandardPrice[" + RowCount + "][ShipperPrice]").value = 0;
												document.getElementById("StandardPrice[" + RowCount + "][ConsigneePrice]").value = EachPrice;
											} else {
												document.getElementById("StandardPrice[" + RowCount + "][ShipperPrice]").value = EachPrice;
												document.getElementById("StandardPrice[" + RowCount + "][ConsigneePrice]").value = 0;
											}
										}
										else if (form1.FOB.checked == true) {
											if (temp % 2 == 1) {
												document.getElementById("StandardPrice[" + RowCount + "][ShipperPrice]").value = 0;
												document.getElementById("StandardPrice[" + RowCount + "][ConsigneePrice]").value = EachPrice;
											} else {
												document.getElementById("StandardPrice[" + RowCount + "][ShipperPrice]").value = EachPrice;
												document.getElementById("StandardPrice[" + RowCount + "][ConsigneePrice]").value = 0;
											}
										}
									}
								}
							}
							else { j--; }
							RowCount++;
						}
					}
				}
				RowCount = document.getElementById('TB_StandardPrice').rows.length - 1;
				for (i = 0; i < RowCount; i++) {				   
					if (SUGICHECK[i] == "1") {
						document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value = 0;
						document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value = 0;
					}
				}
				form1.DEBUG.value = result;
			}, function (result) { alert(result); });
		}
		function InsertRow() {
			var objTable = document.getElementById('TB_StandardPrice');
			objTable.appendChild(document.createElement("TBODY"));
			var lastRow = objTable.rows.length;
			var thisLineNo = lastRow - 1;
			var objRow1 = objTable.insertRow(lastRow);
			var objCell1 = objRow1.insertCell();
			var objCell2 = objRow1.insertCell();
			var objCell3 = objRow1.insertCell();

			objCell1.align = "center";
			objCell2.align = "center";
			objCell3.align = "center";

			objCell1.innerHTML = "<input type=\"hidden\" id=\"StandardPrice[" + thisLineNo + "][ColumnName]\" value='SUGI' />" +
												"<input type=\"hidden\" id=\"StandardPrice[" + thisLineNo + "][ShipperColumnPk]\" />" +
												"<input type=\"hidden\" id=\"StandardPrice[" + thisLineNo + "][ConsigneeColumnPk]\" />" +
												//"<input type=\"hidden\" id=\"StandardPrice[" + thisLineNo + "][Price]\" />" +
												"<input type=\"text\" id=\"StandardPrice[" + thisLineNo + "][Description]\" style=\"width:220px;\" onchange=\"SetSugi('" + thisLineNo + "');\"   />";
			objCell2.innerHTML = "<select id=\"StandardPrice[" + thisLineNo + "][ShipperMonetaryUnit]\" style=\"width:70px;\" >" +
													"<option value=\"18\">RMB ￥</option><option value=\"19\">USD $</option><option value=\"20\">KRW ￦</option>" +
													"<option value=\"21\">JPY Y</option><option value=\"22\">HKD HK$</option><option value=\"23\">EUR €</option>" +
												"</select><input type=\"text\" id=\"StandardPrice[" + thisLineNo + "][ShipperPrice]\" style=\"width:80px;\" />";
			objCell3.innerHTML = "<select id=\"StandardPrice[" + thisLineNo + "][ConsigneeMonetaryUnit]\" style=\"width:70px;\" >" +
													"<option value=\"18\">RMB ￥</option><option value=\"19\">USD $</option><option value=\"20\">KRW ￦</option>" +
													"<option value=\"21\">JPY Y</option><option value=\"22\">HKD HK$</option><option value=\"23\">EUR €</option>" +
												"</select><input type=\"text\" id=\"StandardPrice[" + thisLineNo + "][ConsigneePrice]\" style=\"width:80px;\" />";
		}
		function SetSugi(thisLineNo) {
			document.getElementById("StandardPrice[" + thisLineNo + "][ColumnName]").value = "SUGI";
		}
		function PaymentWhoChange(value) {
			var RowCount = document.getElementById('TB_StandardPrice').rows.length - 1;
			var temp;
			switch (value) {
				case "EXW":
					for (i = 0; i < RowCount; i++) {
						temp = PAYMENTWHO[i];
						var sp = 0;
						if (document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value != "") { sp = parseFloat(document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value); }
						var cp = 0; if (document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value != "") { cp = parseFloat(document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value); }

						if (cp > 0 && sp > 0) {
							continue;
						}
						else {
							var EachPrice = sp + cp;
							if (parseInt(temp / 8) == 1) {
								document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value = 0;
								document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value = EachPrice;
							} else {
								document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value = EachPrice;
								document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value = 0;
							}
						}
					}
					break;
				case "DDP":
					for (i = 0; i < RowCount; i++) {
						temp = PAYMENTWHO[i];
						var sp = 0;
						if (document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value != "") { sp = parseFloat(document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value); }
						var cp = 0; if (document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value != "") { cp = parseFloat(document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value); }

						if (cp > 0 && sp > 0) {
							continue;
						}
						else {
							var EachPrice = sp + cp;
							if (parseInt((temp / 4)) % 2 == 1) {
								document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value = 0;
								document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value = EachPrice;
							} else {
								document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value = EachPrice;
								document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value = 0;
							}
						}
					}
					break;
				case "CNF":
					for (i = 0; i < RowCount; i++) {
						temp = PAYMENTWHO[i];

						var sp = 0;
						if (document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value != "") { sp = parseFloat(document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value); }
						var cp = 0; if (document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value != "") { cp = parseFloat(document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value); }

						if (cp > 0 && sp > 0) {
							continue;
						}
						else {
							var EachPrice = sp + cp;
							if (parseInt((temp / 2)) % 2 == 1) {
								document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value = 0;
								document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value = EachPrice;
							} else {
								document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value = EachPrice;
								document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value = 0;
							}
						}
					}
					break;
				case "FOB":
					for (i = 0; i < RowCount; i++) {
						temp = PAYMENTWHO[i];
						var sp = 0;
						if (document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value != "") { sp = parseFloat(document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value); }
						var cp = 0; if (document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value != "") { cp = parseFloat(document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value); }

						if (cp > 0 && sp > 0) {
							continue;
						}
						else {
							var EachPrice = sp + cp;
							if (temp % 2 == 1) {
								document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value = 0;
								document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value = EachPrice;
							} else {
								document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value = EachPrice;
								document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value = 0;
							}
						}
					}
					break;
				default: return false;
			}
		}
		function PaymentSetting() {
			var RowCount = document.getElementById('TB_StandardPrice').rows.length - 1;
			var CalculateBodySum = "";
			for (i = 0; i < RowCount; i++) {
				//StandardPrice[0][Price]
				if (parseInt(document.getElementById("StandardPrice[" + i + "][ShipperPercent]").value) + parseInt(document.getElementById("StandardPrice[" + i + "][ConsigneePercent]").value) != 100) {
					document.getElementById("StandardPrice[" + i + "][ConsigneePercent]").value = 100 - parseInt(document.getElementById("StandardPrice[" + i + "][ShipperPercent]").value);
				}
				if (document.getElementById("StandardPrice[" + i + "][ShipperPercent]").value == "0") {
					document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value = "0";
					//document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value = document.getElementById("StandardPrice[" + i + "][Price]").value;
				}
				else if (document.getElementById("StandardPrice[" + i + "][ShipperPercent]").value == "100") {
					//document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value = document.getElementById("StandardPrice[" + i + "][Price]").value;
					document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value = "0";
				}
				else {
					document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value = parseFloat(document.getElementById("StandardPrice[" + i + "][Price]").value) / parseInt(document.getElementById("StandardPrice[" + i + "][ShipperPercent]").value) * 100;
					document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value = parseFloat(document.getElementById("StandardPrice[" + i + "][Price]").value) - parseFloat(document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value);
				}
			}
		}

		var SHIPPERTOTALAMOUNT = 0;
		var CONSIGNEETOTALAMOUNT = 0;
		var ShipperAccount = new Array();
		ShipperAccount[18] = 0;
		ShipperAccount[19] = 0;
		ShipperAccount[20] = 0;
		ShipperAccount[21] = 0;
		ShipperAccount[22] = 0;
		ShipperAccount[23] = 0;
		var ConsigneeAccount = new Array();
		ConsigneeAccount[18] = 0;
		ConsigneeAccount[19] = 0;
		ConsigneeAccount[20] = 0;
		ConsigneeAccount[21] = 0;
		ConsigneeAccount[22] = 0;
		ConsigneeAccount[23] = 0;

		var ExchangeRate = new Array();
		ExchangeRate[18] = new Array();
		ExchangeRate[18][19] = -1; ExchangeRate[18][20] = -1; ExchangeRate[18][21] = -1; ExchangeRate[18][22] = -1; ExchangeRate[18][23] = -1;
		ExchangeRate[19] = new Array();
		ExchangeRate[19][18] = -1; ExchangeRate[19][20] = -1; ExchangeRate[19][21] = -1; ExchangeRate[19][22] = -1; ExchangeRate[19][23] = -1;
		ExchangeRate[20] = new Array();
		ExchangeRate[20][18] = -1; ExchangeRate[20][19] = -1; ExchangeRate[20][21] = -1; ExchangeRate[20][22] = -1; ExchangeRate[20][23] = -1;
		ExchangeRate[21] = new Array();
		ExchangeRate[21][18] = -1; ExchangeRate[21][19] = -1; ExchangeRate[21][20] = -1; ExchangeRate[21][22] = -1; ExchangeRate[21][23] = -1;
		ExchangeRate[22] = new Array();
		ExchangeRate[22][18] = -1; ExchangeRate[22][19] = -1; ExchangeRate[22][20] = -1; ExchangeRate[22][21] = -1; ExchangeRate[22][23] = -1;
		ExchangeRate[23] = new Array();
		ExchangeRate[23][18] = -1; ExchangeRate[23][19] = -1; ExchangeRate[23][20] = -1; ExchangeRate[23][21] = -1; ExchangeRate[23][22] = -1;

		function CalcSum() {
			//PaymentSetting();
			var SMonetaryUnit = form1.st_TotalShipperMonetaryUnit.value;
			var CMonetaryUnit = form1.st_TotalConsigneeMonetaryUnit.value;
			var RowCount = document.getElementById('TB_StandardPrice').rows.length - 1;
			var ShipperSUM = "";
			var ConsigneeSUM = "";
			for (i = 0; i < RowCount; i++) {
				if (document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value != "0" && document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value != "") {
					ShipperSUM += document.getElementById("StandardPrice[" + i + "][ShipperMonetaryUnit]").value + "!" +
										document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value.toString().replace(/,/g, '') + "@@";
				}
				if (document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value != "0" && document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value != "") {
					ConsigneeSUM += document.getElementById("StandardPrice[" + i + "][ConsigneeMonetaryUnit]").value + "!" +
										document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value.toString().replace(/,/g, '') + "@@";
				}
			}
			//alert(ShipperSUM + " " + ConsigneeSUM);
			//return false;
			//alert(WhichMonetaryUnitNeed);
			//alert(SMonetaryUnit + " " + ShipperSUM + " " + CMonetaryUnit + " " + ConsigneeSUM); return false;


			var ExchangedDate = document.getElementById("HDepartureDate").value;
			var isEstimation = "";
			if (document.getElementById("IsEstimation").value == "33") {
				isEstimation = "Y";
			}
			if (parseInt(ExchangedDate) < 20130724) {
				if (EXCHANGERATESUM != "") {
					var EachExchange = EXCHANGERATESUM.split("@@");

					for (var i = 0; i < EachExchange.length; i++) {
						if (EachExchange[i].substr(6, 8) > ExchangedDate) {
							ExchangedDate = EachExchange[i].substr(6, 8);
						}
					}
					//ExchangedDate = EXCHANGERATESUM.substr(6, 8);
				}
			}

			//alert(SMonetaryUnit + "__" + ShipperSUM + "__" + CMonetaryUnit + "__" + ConsigneeSUM + "__" + ExchangedDate);
			Admin.GetTotalPrice(SMonetaryUnit, ShipperSUM, CMonetaryUnit, ConsigneeSUM, ExchangedDate, isEstimation, function (result) {
				if (result[0] == "-1") {
					alert("발화인이 사용할 환율정보가 없습니다. ");
					return false;
				}
				else {
					form1.TotalShipperAmount.value = result[1];
				}
				if (result[0] == "-2") {
					alert("수하인이 사용할 환율정보가 없습니다.");
					return false;
				}
				else {
					form1.TotalConsigneeAmount.value = result[2];
				}
				EXCHANGERATESUM = result[0];
				//alert(EXCHANGERATESUM);
				ISCALC = true;
				alert(form1.dhksfy.value);
			}, function (result) { alert(result); });
		}

		function BTN_Submit_Click() {
			if (!ISCALC) { alert("please click calcuate button before submit click"); return false; }
			var GubunCL = '42'
			var ShipperContentsSum = "";
			var ConsigneeContentsSum = "";

			if (form1.HShipperTBContents.value != form1.ShipperTBContents.value + "") {
				ShipperContentsSum = form1.HShipperPk.value + "%!$@#" + ACCOUNTID + "%!$@#" + form1.ShipperTBContents.value + "%!$@#" + GubunCL;
			}
			if (form1.HConsigneeTBContents.value != form1.ConsigneeTBContents.value + "") {
				ConsigneeContentsSum = form1.HConsigneePk.value + "%!$@#" + ACCOUNTID + "%!$@#" + form1.ConsigneeTBContents.value + "%!$@#" + GubunCL;
			}
			if (ISMODIFY) {
				var PaymentWhoCL = 9;
				if (form1.EXW.checked) { PaymentWhoCL = 5; }
				else if (form1.DDP.checked) { PaymentWhoCL = 6; }
				else if (form1.CNF.checked) { PaymentWhoCL = 7; }
				else if (form1.FOB.checked) { PaymentWhoCL = 8; }
				
				var RowCount = document.getElementById('TB_StandardPrice').rows.length - 1;
				var CalculateBodySum = "";
				var ColumnPk = "";
				var percent = "";
				var IsSorC = new Array();
				var ColumnStandardpriceN = "";

				IsSorC[0] = "Shipper";
				IsSorC[1] = "Consignee";
				for (i = 0; i < RowCount; i++) {
					for (j = 0; j < IsSorC.length; j++) {
						if (document.getElementById("StandardPrice[" + i + "][" + IsSorC[j] + "Price]").value != "" && document.getElementById("StandardPrice[" + i + "][" + IsSorC[j] + "Price]").value != "0") {
							if (IsSorC[j] == "Shipper") {
								percent = "200";
							}
							else {
								percent = "300";
							}
							ColumnPk = "N"; if (document.getElementById("StandardPrice[" + i + "][" + IsSorC[j] + "ColumnPk]").value != "") { ColumnPk = document.getElementById("StandardPrice[" + i + "][" + IsSorC[j] + "ColumnPk]").value; }
							var ColumnName = document.getElementById("StandardPrice[" + i + "][ColumnName]").value;

							if (ColumnName.substr(0, 4) == "Misu") {
								ColumnStandardpriceN = ColumnName;
							} else if (document.getElementById("StandardPrice[" + i + "][ColumnName]").value == "SUGI") {
								ColumnStandardpriceN = "";
							}
							else {
								ColumnStandardpriceN = STANDARDPRICEHEADPKnCOLUMN[i];
							}
							CalculateBodySum += percent + "!" + document.getElementById("StandardPrice[" + i + "][Description]").value + "!" +
																document.getElementById("StandardPrice[" + i + "][" + IsSorC[j] + "Price]").value + "!" +
																document.getElementById("StandardPrice[" + i + "][" + IsSorC[j] + "MonetaryUnit]").value +
																"!" + ColumnStandardpriceN + "!" + ColumnPk + "@@";
						}
					}
				}

				if (form1.TBDeleveryChargeShipper.value != "") { CalculateBodySum += "200!DELIVERY CHARGE!" + form1.TBDeleveryChargeShipper.value + "!" + form1.st_TotalShipperMonetaryUnit.value + "!D!" + form1.HDeliveryChargeShipperPk + "@@"; }
				if (form1.TBDeleveryChargeConsignee.value != "") { CalculateBodySum += "300!DELIVERY CHARGE!" + form1.TBDeleveryChargeConsignee.value + "!" + form1.st_TotalConsigneeMonetaryUnit.value + "!D!" + form1.HDeliveryChargeConsigneePk + "@@"; }

				var WeightOrVolume = "46";
				if (form1.ST_WeightOrVolume.value == "CBM") { WeightOrVolume = "39"; }
				form1.DEBUG.value = CalculateBodySum;

				Admin.ModifyFixCharge(PaymentWhoCL, form1.HRequestFormPk.value, document.getElementById("st_StandardPrice").value, WeightOrVolume, form1.ST_CriterionValue.value + "!" + form1.ST_OverWeight.value, EXCHANGERATESUM, form1.st_TotalShipperMonetaryUnit.value, form1.TotalShipperAmount.value, form1.st_TotalConsigneeMonetaryUnit.value, form1.TotalConsigneeAmount.value, CalculateBodySum, ACCOUNTID, form1.st_ReceiveBankShipper.value, form1.st_ReceiveBankConsignee.value, form1.st_WhoWillPayTariff.value, form1.TBComment_Shipper.value, form1.TBComment_Consignee.value, ShipperContentsSum, ConsigneeContentsSum, function (result) {
					if (result == "1") {
						alert(form1.dhksfy.value); window.returnValue = true; returnValue = "Y"; self.close();
					}
					else {
						alert(result);
						document.getElementById("ShipperTBContents").value = result;
					}
				}, function (result) { alert(result); });
			}
			else {
				var PaymentWhoCL = 9;
				if (form1.EXW.checked) { PaymentWhoCL = 5; }
				else if (form1.DDP.checked) { PaymentWhoCL = 6; }
				else if (form1.CNF.checked) { PaymentWhoCL = 7; }
				else if (form1.FOB.checked) { PaymentWhoCL = 8; }

				var RowCount = document.getElementById('TB_StandardPrice').rows.length - 1;
				var CalculateBodySum = "";
				for (i = 0; i < RowCount; i++) {
					if (document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value != "" && document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value != "0") {
						var percent = "200";
						if (CHECKPERSONALLY || document.getElementById("StandardPrice[" + i + "][ColumnName]").value == "SUGI") {
							CalculateBodySum += percent + "!" + document.getElementById("StandardPrice[" + i + "][Description]").value + "!" + document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value + "!" + document.getElementById("StandardPrice[" + i + "][ShipperMonetaryUnit]").value + "!@@";
						}
						else {
							CalculateBodySum += percent + "!" + document.getElementById("StandardPrice[" + i + "][Description]").value + "!" + document.getElementById("StandardPrice[" + i + "][ShipperPrice]").value + "!" + document.getElementById("StandardPrice[" + i + "][ShipperMonetaryUnit]").value + "!" + STANDARDPRICEHEADPKnCOLUMN[i] + "@@";
						}
					}
					if (document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value != "" && document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value != "0") {
						var percent = "300";
						if (CHECKPERSONALLY || document.getElementById("StandardPrice[" + i + "][ColumnName]").value == "SUGI") {
							CalculateBodySum += percent + "!" + document.getElementById("StandardPrice[" + i + "][Description]").value + "!" + document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value + "!" + document.getElementById("StandardPrice[" + i + "][ConsigneeMonetaryUnit]").value + "!@@";
						}
						else {
							CalculateBodySum += percent + "!" + document.getElementById("StandardPrice[" + i + "][Description]").value + "!" + document.getElementById("StandardPrice[" + i + "][ConsigneePrice]").value + "!" + document.getElementById("StandardPrice[" + i + "][ConsigneeMonetaryUnit]").value + "!" + STANDARDPRICEHEADPKnCOLUMN[i] + "@@";
						}
					}
				}
				if (form1.TBDeleveryChargeShipper.value != "") { CalculateBodySum += "200!DELIVERY CHARGE!" + form1.TBDeleveryChargeShipper.value + "!" + form1.st_TotalShipperMonetaryUnit.value + "!D@@"; }
				if (form1.TBDeleveryChargeConsignee.value != "") { CalculateBodySum += "300!DELIVERY CHARGE!" + form1.TBDeleveryChargeConsignee.value + "!" + form1.st_TotalConsigneeMonetaryUnit.value + "!D@@"; }
				var WeightOrVolume = "46";
				if (form1.ST_WeightOrVolume.value == "CBM") { WeightOrVolume = "39"; }
				Admin.SetFixCharge(PaymentWhoCL, form1.HRequestFormPk.value, document.getElementById("st_StandardPrice").value, WeightOrVolume, form1.ST_CriterionValue.value + "!" + form1.ST_OverWeight.value, EXCHANGERATESUM, form1.st_TotalShipperMonetaryUnit.value, form1.TotalShipperAmount.value, form1.st_TotalConsigneeMonetaryUnit.value, form1.TotalConsigneeAmount.value, CalculateBodySum, ACCOUNTID, form1.st_ReceiveBankShipper.value, form1.st_ReceiveBankConsignee.value, form1.st_WhoWillPayTariff.value, form1.TBComment_Shipper.value, form1.TBComment_Consignee.value, ShipperContentsSum, ConsigneeContentsSum, function (result) {
					if (result == "1") { alert(form1.dhksfy.value); window.returnValue = true; returnValue = "Y"; self.close(); }
					else { alert(result); }
				}, function (result) { alert(result); });
			}
		}
		function CheckMisu() {
			var dialogArgument = ACCOUNTID;
			var retVal = window.showModalDialog('./FixCharge_Checkmisu.aspx?S=' + form1.HRequestFormPk.value, dialogArgument, "dialogHeight:600px; dialogWidth:760px; resizable:0; status:0; scroll:1; help:0; ");
			try {
				var Row = retVal.split("%%$$##");
				for (var i = 0; i < Row.length; i++) {
					var Each = Row[i].split("!@#");

					var objTable = document.getElementById('TB_StandardPrice');
					objTable.appendChild(document.createElement("TBODY"));
					var lastRow = objTable.rows.length;
					var thisLineNo = lastRow - 1;
					var objRow1 = objTable.insertRow(lastRow);
					var objCell1 = objRow1.insertCell();
					//var objCell2 = objRow1.insertCell();
					var objCell3 = objRow1.insertCell();
					//var objCell4 = objRow1.insertCell();
					var objCell5 = objRow1.insertCell(); z

					objCell1.align = "center";
					objCell3.align = "center";
					objCell5.align = "center";

					objCell1.innerHTML = "<input type=\"hidden\" id=\"StandardPrice[" + thisLineNo + "][ColumnName]\" value='Misu" + Each[1] + "' />" +
                                                        "<input type=\"hidden\" id=\"StandardPrice[" + thisLineNo + "][ShipperColumnPk]\" />" +
                                                        "<input type=\"hidden\" id=\"StandardPrice[" + thisLineNo + "][ConsigneeColumnPk]\" />" +
                                                        "<input type=\"text\" id=\"StandardPrice[" + thisLineNo + "][Description]\" style=\"width:220px;\" value='" + Each[2] + "' />";
					if (Each[0] == "S") {
						var Selected = Array("", "", "", "", "", "");
						switch (Each[3]) {
							case "18":
								Selected = Array(" selected=\"selected\" ", "", "", "", "", "");
								break;
							case "19":
								Selected = Array("", " selected=\"selected\" ", "", "", "", "");
								break;
							case "20":
								Selected = Array("", "", " selected=\"selected\" ", "", "", "");
								break;
							case "21":
								Selected = Array("", "", "", " selected=\"selected\" ", "", "");
								break;
							case "22":
								Selected = Array("", "", "", "", " selected=\"selected\" ", "");
								break;
							case "23":
								Selected = Array("", "", "", "", "", " selected=\"selected\" ");
								break;
						}

						objCell3.innerHTML = "<select id=\"StandardPrice[" + thisLineNo + "][ShipperMonetaryUnit]\" style=\"width:70px;\" >" +
                                                                "<option value=\"18\" " + Selected[0] + ">RMB ￥</option><option value=\"19\" " + Selected[1] + ">USD $</option><option value=\"20\" " + Selected[2] + ">KRW ￦</option>" +
                                                                "<option value=\"21\" " + Selected[3] + ">JPY Y</option><option value=\"22\" " + Selected[4] + ">HKD HK$</option><option value=\"23\" " + Selected[5] + ">EUR €</option>" +
                                                            "</select><input type=\"text\" id=\"StandardPrice[" + thisLineNo + "][ShipperPrice]\" style=\"width:80px;\" value=\"" + NumberFormat(Each[4]) + "\" />";
					} else {
						objCell3.innerHTML = "<select id=\"StandardPrice[" + thisLineNo + "][ShipperMonetaryUnit]\" style=\"width:70px;\" >" +
                                                                "<option value=\"18\">RMB ￥</option><option value=\"19\">USD $</option><option value=\"20\">KRW ￦</option>" +
                                                                "<option value=\"21\">JPY Y</option><option value=\"22\">HKD HK$</option><option value=\"23\">EUR €</option>" +
                                                            "</select><input type=\"text\" id=\"StandardPrice[" + thisLineNo + "][ShipperPrice]\" style=\"width:80px;\" />";
					}

					if (Each[0] == "C") {
						var Selected = Array("", "", "", "", "", "");
						switch (Each[3]) {
							case "18":
								Selected = Array(" selected=\"selected\" ", "", "", "", "", "");
								break;
							case "19":
								Selected = Array("", " selected=\"selected\" ", "", "", "", "");
								break;
							case "20":
								Selected = Array("", "", " selected=\"selected\" ", "", "", "");
								break;
							case "21":
								Selected = Array("", "", "", " selected=\"selected\" ", "", "");
								break;
							case "22":
								Selected = Array("", "", "", "", " selected=\"selected\" ", "");
								break;
							case "23":
								Selected = Array("", "", "", "", "", " selected=\"selected\" ");
								break;
						}

						objCell5.innerHTML = "<select id=\"StandardPrice[" + thisLineNo + "][ConsigneeMonetaryUnit]\" style=\"width:70px;\" >" +
                                                                "<option value=\"18\" " + Selected[0] + ">RMB ￥</option><option value=\"19\" " + Selected[1] + ">USD $</option><option value=\"20\" " + Selected[2] + ">KRW ￦</option>" +
                                                                "<option value=\"21\" " + Selected[3] + ">JPY Y</option><option value=\"22\" " + Selected[4] + ">HKD HK$</option><option value=\"23\" " + Selected[5] + ">EUR €</option>" +
                                                            "</select><input type=\"text\" id=\"StandardPrice[" + thisLineNo + "][ConsigneePrice]\" style=\"width:80px;\" value=\"" + NumberFormat(Each[4]) + "\" />";
					} else {
						objCell5.innerHTML = "<select id=\"StandardPrice[" + thisLineNo + "][ConsigneeMonetaryUnit]\" style=\"width:70px;\" >" +
                                                                "<option value=\"18\">RMB ￥</option><option value=\"19\">USD $</option><option value=\"20\">KRW ￦</option>" +
                                                                "<option value=\"21\">JPY Y</option><option value=\"22\">HKD HK$</option><option value=\"23\">EUR €</option>" +
                                                            "</select><input type=\"text\" id=\"StandardPrice[" + thisLineNo + "][ConsigneePrice]\" style=\"width:80px;\" />";
					}

				}
			} catch (error) {
			}
		}
	</script>
</head>
<body style="background-color: #999999;">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Admin.asmx" />
			</Services>
		</asp:ScriptManager>
		<div style="margin: 10px; padding-left: 10px; padding-right: 10px; background-color: white;">

			<!--div style="float:right; margin-top:10px; margin-right:10px; ">
            <input type="button" value="미수확인" onclick="CheckMisu();" />
		</!--div-->
			<div style="line-height: 20px; padding: 10px;">
				<input type="hidden" id="IsEstimation" value="<%=IsEstimation %>" />
				<% if (IsEstimation == "33") { %>
				<div style="float: right; font-weight: bold; font-size: 17px;">견적용</div>
				<% } %>
				<%=Head %>
			Standard Price<%=st_StandardPrice %>
				<input type="button" value="Load" onclick="StandardPriceSelectChange(document.getElementById('st_StandardPrice').value);" />
				<div>
					<div id="PnDangakijun"></div>
					<div>
						Who will pay Tariff&nbsp;&nbsp;&nbsp;&nbsp;<select id="st_WhoWillPayTariff"><option value="C">Consignee</option>
							<option value="S">Shipper</option>
						</select>
					</div>
				</div>
			</div>
			<div>
				결제방식 
			<input type="radio" name="PaymentWhoCL" id="EXW" value="5" onclick="PaymentWhoChange(this.id);" <%=PaymmentWhoCL[0] %> /><label for="EXW">EXW (A)</label>
				<input type="radio" name="PaymentWhoCL" id="DDP" value="6" onclick="PaymentWhoChange(this.id);" <%=PaymmentWhoCL[1] %> /><label for="DDP">DDP (B)</label>
				<input type="radio" name="PaymentWhoCL" id="CNF" value="7" onclick="PaymentWhoChange(this.id);" <%=PaymmentWhoCL[2] %> /><label for="CNF">CNF (C)</label>
				<input type="radio" name="PaymentWhoCL" id="FOB" value="8" onclick="PaymentWhoChange(this.id);" <%=PaymmentWhoCL[3] %> /><label for="FOB">FOB (D)</label>
				<input type="radio" name="PaymentWhoCL" id="ETC" value="9" onclick="PaymentWhoChange(this.id);" <%=PaymmentWhoCL[4] %> /><label for="ETC">ETC</label>
			</div>
			<div>
				<%=GetGlobalResourceObject("qjsdur", "cnfqkfwl") %> :
				<select id="st_ReceiveBankShipper"></select><br />
				<%=GetGlobalResourceObject("qjsdur", "ehckr") %> :
				<select id="st_ReceiveBankConsignee"></select>
			</div>
			<div id="PnAddRow" style="padding-top: 10px;"></div>
			<div id="PnCostList" style="width: 575px; padding-bottom: 10px;"></div>
			<table border="0" cellpadding="0" cellspacing="0" style="width: 570px;">
				<tr>
					<td>Total
						<input type="button" id="BTN_Calc" onclick="CalcSum();" value="CALC" /></td>
					<td style="width: 160px;">
						<select id="st_TotalShipperMonetaryUnit" style="width: 70px;" onchange="document.getElementById('st_DeliveryChargeMonetaryShipper').value=this.value;">
							<option value="18">RMB ￥</option>
							<option value="19">USD $</option>
							<option value="20">KRW ￦</option>
							<option value="21">JPY Y</option>
							<option value="22">HKD HK$</option>
							<option value="23">EUR €</option>
						</select>
						<input type="text" id="TotalShipperAmount" style="width: 80px;" />
					</td>
					<td style="width: 160px;">
						<select id="st_TotalConsigneeMonetaryUnit" style="width: 70px;" onchange="document.getElementById('st_DeliveryChargeMonetaryConsignee').value=this.value;">
							<option value="20">KRW ￦</option>
							<option value="18">RMB ￥</option>
							<option value="19">USD $</option>
							<option value="21">JPY Y</option>
							<option value="22">HKD HK$</option>
							<option value="23">EUR €</option>
						</select>
						<input type="text" id="TotalConsigneeAmount" style="width: 80px;" />
					</td>
				</tr>
				<tr>
					<td>Delivery Charge</td>
					<td>
						<input type="hidden" id="HDeliveryChargeShipperPk" />
						<select id="st_DeliveryChargeMonetaryShipper" style="width: 70px;" disabled="disabled">
							<option value="18">RMB ￥</option>
							<option value="19">USD $</option>
							<option value="20">KRW ￦</option>
							<option value="21">JPY Y</option>
							<option value="22">HKD HK$</option>
							<option value="23">EUR €</option>
						</select>
						<input type="text" id="TBDeleveryChargeShipper" value="" style="width: 80px;" /></td>
					<td>
						<input type="hidden" id="HDeliveryChargeConsigneePk" />
						<select id="st_DeliveryChargeMonetaryConsignee" style="width: 70px;" disabled="disabled">
							<option value="20">KRW ￦</option>
							<option value="18">RMB ￥</option>
							<option value="19">USD $</option>
							<option value="21">JPY Y</option>
							<option value="22">HKD HK$</option>
							<option value="23">EUR €</option>
						</select>

						<input type="text" id="TBDeleveryChargeConsignee" value="" style="width: 80px;" />
					</td>
				</tr>
			</table>
			<%=Estimation_DIV %>
			<div style="width: 580px; padding: 30px 0px 10px 0px;">
				<fieldset style="padding: 0px 10px 10px 10px;">
					<legend><strong>Shipper Attention</strong></legend>
					<span><%=ShipperRegisterd %></span><br />
					<textarea id="ShipperTBContents" rows="5" cols="85"><%=ShipperContents %></textarea>
				</fieldset>
			</div>
			<div style="width: 580px; padding: 10px 0px 10px 0px;">
				<fieldset style="padding: 0px 10px 10px 10px;">
					<legend><strong>Consignee Attention</strong></legend>
					<span><%=ConsigneeRegisterd %></span><br />
					<textarea id="ConsigneeTBContents" rows="5" cols="85"><%=ConsigneeContents %></textarea>
				</fieldset>
			</div>
			<div style="padding: 20px;">
				<input type="button" id="BTN_Submit" value="Submit" onclick="BTN_Submit_Click();" style="width: 200px; height: 30px;" /></div>
		</div>
		<input type="hidden" id="HShipperTBContents" value="<%=ShipperContents %>" />
		<input type="hidden" id="HConsigneeTBContents" value="<%=ConsigneeContents %>" />
		<input type="hidden" id="HCriterionValue" />
		<input type="hidden" id="HGrossWeight" value="<%=GrossWeight %>" />
		<input type="hidden" id="HVolume" value="<%=Volume %>" />
		<input type="hidden" id="HOverWeight" value="<%=OverWeight %>" />
		<input type="hidden" id="HRequestFormPk" value="<%=Request.Params["S"] %>" />
		<input type="hidden" id="HDepartureDate" value="<%=departureDate %>" />
		<input type="hidden" id="HShipperPk" value="<%=ShipperPk %>" />
		<input type="hidden" id="HConsigneePk" value="<%=ConsigneePk %>" />
		<%--<input type="text" id="HDepartureRegion" value="<%=DepartureRegionCode %>" />
	<input type="text" id="HArrivalRegion" value="<%=ArrivalRegionCode %>" />--%>
		<input type="hidden" id="rhkwndfid" value="<%=GetGlobalResourceObject("qjsdur", "rhkwndfid")%>" />
		<input type="hidden" id="rhkcpwjr" value="<%=GetGlobalResourceObject("qjsdur", "rhkcpwjr")%>" />
		<input type="hidden" id="dhksfy" value="<%=GetGlobalResourceObject("qjsdur", "dhksfy") %>" />
		<input type="hidden" id="DEBUG" onfocus="this.select();" />
	</form>
</body>
</html>