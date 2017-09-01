<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequestWrite.aspx.cs" Inherits="Request_RequestWrite" %>

<meta http-equiv="X-UA-Compatible" content="IE=10">
<%@ Register Src="../Member/LogedTopMenu.ascx" TagName="Loged" TagPrefix="uc1" %>
<%@ Register Src="../Admin/LogedWithoutRecentRequest.ascx" TagName="LogedWithoutRecentRequest" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title><%=GetGlobalResourceObject("RequestForm", "RequestFormWrite") %></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<link rel="stylesheet" href="/Lib/jquery-ui.css" />

	<script src="../Common/public.js" type="text/javascript"></script>
	<script src="../Common/jquery-1.10.2.min.js"></script>
	<script src="/Lib/jquery-ui.js"></script>

	<script type="text/javascript">
		var CheckCalc = false;
		var ITEMBEFORE = new Array();
		var ITEMPK = new Array();
		var HaveCalcuatedHead = false;
		var Weight_Before;
		var Volume_Before;
		window.onload = function () {
			LoadRegionCodeCountry("regionCodeI1");
			LoadRegionCodeCountry("regionCodeE1");

			if (form1.HGubun.value != "Admin") {
				$(".NavRequestWrite").addClass("active");
			}
			if (form1.HMode.value == "Write") {
				InsertRow('0');
				if (form1.HRequestFormPk.value != "") {
					Request.RequestLoad(form1.HRequestFormPk.value, function (result) {
						var ITEMCOUNT = 0;
						for (var i = 0; i < result.length; i++) {
							var each = result[i].split("#@!");
							if (each[0] == "I") {
								InsertRow('0');
								if (each[2] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][boxNo]").value = each[2]; }
								if (each[3] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][ItemName]").value = each[3]; }
								if (each[4] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][Brand]").value = each[4]; }
								if (each[5] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][Matarial]").value = each[5]; }
								if (each[6] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][Quantity]").value = NumberFormat(each[6]); }
								if (each[7] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][QuantityUnit]").value = each[7]; }
								if (each[8] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][PackedCount]").value = NumberFormat(each[8]); }
								if (each[9] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][PackingUnit]").value = each[9]; }
								if (each[12] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][UnitCost]").value = NumberFormat(each[12]); }
								if (each[13] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][Price]").value = NumberFormat(each[13]); }
								if (each[10] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][Weight]").value = NumberFormat(each[10]); }
								if (each[11] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][Volume]").value = NumberFormat(each[11]); }
								ITEMPK[ITEMCOUNT] = each[1];
								ITEMBEFORE[ITEMCOUNT] = document.getElementById("Item[0][" + ITEMCOUNT + "][boxNo]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][ItemName]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][Brand]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][Matarial]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][Quantity]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][QuantityUnit]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][PackedCount]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][PackingUnit]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][UnitCost]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][Price]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][Weight]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][Volume]").value;
								ITEMCOUNT++;
							}
						}
						document.getElementById("Item[0][MonetaryUnit]").value = TempMonetary;
						switch (TempMonetary) {
							case '18': SetMonetaryUnit("￥"); break;
							case '19': SetMonetaryUnit("$"); break;
							case '20': SetMonetaryUnit("￦"); break;
							case '21': SetMonetaryUnit("Y"); break;
							case '22': SetMonetaryUnit("$"); break;
							case '23': SetMonetaryUnit("€"); break;
							default: SetMonetaryUnit("$");
								document.getElementById("Item[0][MonetaryUnit]").value = "19";
								break;

						}
					}, function (result) { alert("ERROR : " + result); });
				}
			}
			else {
				var TempMonetary = "";
				Request.RequestLoad(form1.HRequestFormPk.value, function (result) {
					var ITEMCOUNT = 0;
					for (var i = 0; i < result.length; i++) {
						var each = result[i].split("#@!");
						switch (each[0]) {
							case "C":
								form1.HCompanyPk.value = each[1];
								SelectWhoResult("S", each[1], each[2], each[3], each[4], each[5]);
								SelectWhoResult("C", each[6], each[7], each[8], each[9], each[10]);
								form1.TB_DepartureDate.value = each[11];
								form1.TB_ArrivalDate.value = each[12];
								//alert(each[24]);
								form1.TB_ShipmentDate.value = each[24];
								if (each[13] != "") {
									if (each[13] == "0") {
										document.getElementById("ShipperClearanceNamePk").value = each[13];
										document.getElementById("PnShipperClearanceName").innerHTML = "<strong><%=GetGlobalResourceObject("RequestForm", "ClearanceSubstitute") %></strong>";

									}
									else {
										document.getElementById("ShipperClearanceNamePk").value = each[13];
										document.getElementById("PnShipperClearanceName").innerHTML = "<strong>" + each[14] + "</strong><br />" + each[15];
									}
								}
								if (each[16] != "") {
									if (each[16] == "0") {
										document.getElementById("ConsigneeClearanceNamePk").value = each[16];
										document.getElementById("PnConsigneeClearanceName").innerHTML = "<strong><%=GetGlobalResourceObject("RequestForm", "ClearanceSubstitute") %></strong>";
									}
									else {
										document.getElementById("ConsigneeClearanceNamePk").value = each[16];
										document.getElementById("PnConsigneeClearanceName").innerHTML = "<strong>" + each[17] + "</strong><br />" + each[18];
									}
								}
								document.getElementById("UnsongWayCode").value = each[19];
								switch (each[20]) {
									case "5": document.form1.Payment3[0].checked = "checked"; break;
									case "6": document.form1.Payment3[1].checked = "checked"; break;
									case "7": document.form1.Payment3[2].checked = "checked"; break;
									case "8": document.form1.Payment3[3].checked = "checked"; break;
									case "9": document.form1.Payment3[4].checked = "checked"; break;
								}
								var DocumentRequest = each[21].split("!");
								for (var j = 0; j < DocumentRequest.length; j++) {
									switch (DocumentRequest[j]) {
										//case "10": document.getElementById('CertificateOfOrigin').checked = "checked"; break;
										//case "24": document.getElementById('ProductCheked').checked = "checked"; break;
										//case "25": document.getElementById('SuChec').checked = "checked"; break;
										case "31": document.getElementById('CertificateOfOrigin').checked = "checked"; break;
										case "32": document.getElementById('ProductCheked').checked = "checked"; break;
										case "33": document.getElementById('SuChec').checked = "checked"; break;
										case "34": document.getElementById('FTACheked').checked = "checked"; break;
									}
								}

								TempMonetary = each[22];
								form1.PaymentETC.value = each[23].replace("<br />", "\n").replace("<br />", "\r\n");
								break;
							case "I":
								InsertRow('0');
								if (each[2] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][boxNo]").value = each[2]; }
								if (each[3] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][ItemName]").value = each[3]; }
								if (each[4] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][Brand]").value = each[4]; }
								if (each[5] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][Matarial]").value = each[5]; }
								if (each[6] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][Quantity]").value = NumberFormat(each[6]); }
								if (each[7] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][QuantityUnit]").value = each[7]; }
								if (each[8] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][PackedCount]").value = NumberFormat(each[8]); }
								if (each[9] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][PackingUnit]").value = each[9]; }
								if (each[12] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][UnitCost]").value = NumberFormat(each[12]); }
								if (each[13] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][Price]").value = NumberFormat(each[13]); }
								if (each[10] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][Weight]").value = NumberFormat(each[10]); }
								if (each[11] != "") { document.getElementById("Item[0][" + ITEMCOUNT + "][Volume]").value = NumberFormat(each[11]); }
								ITEMPK[ITEMCOUNT] = each[1];
								ITEMBEFORE[ITEMCOUNT] = document.getElementById("Item[0][" + ITEMCOUNT + "][boxNo]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][ItemName]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][Brand]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][Matarial]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][Quantity]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][QuantityUnit]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][PackedCount]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][PackingUnit]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][UnitCost]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][Price]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][Weight]").value + "#@!" +
														document.getElementById("Item[0][" + ITEMCOUNT + "][Volume]").value;
								ITEMCOUNT++;
								break;
							case "T":
								document.getElementById("total[0][PackedCount]").value = each[1];
								document.getElementById("total[0][PackingUnit]").value = each[2];
								document.getElementById("total[0][Weight]").value = NumberFormat(each[3]);
								document.getElementById("total[0][Volume]").value = NumberFormat(each[4]);
								HaveCalcuatedHead = true;
								break;
						}
					}
					document.getElementById("Item[0][MonetaryUnit]").value = TempMonetary;
					switch (TempMonetary) {
						case '18': SetMonetaryUnit("￥"); break;
						case '19': SetMonetaryUnit("$"); break;
						case '20': SetMonetaryUnit("￦"); break;
						case '21': SetMonetaryUnit("Y"); break;
						case '22': SetMonetaryUnit("$"); break;
						case '23': SetMonetaryUnit("€"); break;
						default: SetMonetaryUnit("$");
							document.getElementById("Item[0][MonetaryUnit]").value = "19";
							break;

					}
				}, function (result) {
					alert("ERROR : " + result);
				});
			}
			$("#TB_DepartureDate").datepicker();
			$("#TB_DepartureDate").datepicker("option", "dateFormat", "yymmdd");
			$("#TB_ArrivalDate").datepicker();
			$("#TB_ArrivalDate").datepicker("option", "dateFormat", "yymmdd");
			$("#TB_ShipmentDate").datepicker();
			$("#TB_ShipmentDate").datepicker("option", "dateFormat", "yymmdd");

			setTimeout(function () {
				Weight_Before = document.getElementById("total[0][Weight]").value;
				Volume_Before = document.getElementById("total[0][Volume]").value;
			}, 800);

		}
		function NumberFormat(number) { if (number == "" || number == "0") { return number; } else { return parseInt(number * 10000) / 10000; } }
		function SelectWhoResult(SorC, TargetCompanyPk, TargetCompanyNick, CompanyCode, RegionCode, BranchPk) {
			if (SorC == "S") {
				form1.ShipperPk.value = TargetCompanyPk;
				form1.ShipperCode.value = CompanyCode;
				document.getElementById("PnShipperName").innerHTML = "<strong>" + CompanyCode + "</strong>&nbsp;" + decodeURI(TargetCompanyNick);
				document.getElementById("DepartureBranch").value = BranchPk;
			}
			else {
				form1.ConsigneePk.value = TargetCompanyPk;
				form1.ConsigneeCode.value = CompanyCode;
				document.getElementById("PnConsigneeName").innerHTML = "<strong>" + CompanyCode + "</strong>&nbsp;" + decodeURI(TargetCompanyNick);
				document.getElementById("ArrivalBranch").value = BranchPk;
			}
			var IsFoundRegion = false;
			var RegionArray = RegionCode.split("|"); // 0 = RegionCode, 1 = RegionCodePk
			if (RegionArray[0].length == 4) {
				if (SorC == "S") {
					Admin.LoadSuperRegionCode(RegionArray[0].substr(0, 1), function (result) {
						form1.regionCodeI1.value = result;
						var Idx1 = $("#regionCodeI1 option").index($("#regionCodeI1 option:selected"));
						cate1change('regionCodeI1', 'regionCodeI2', 'regionCodeI3', Idx1, 'gcodeFrom');
					});
					setTimeout(function () {
						form1.regionCodeI2.value = RegionCode;
						var Idx2 = $("#regionCodeI2 option").index($("#regionCodeI2 option:selected"));
						cate2change('regionCodeI1', 'regionCodeI2', 'regionCodeI3', Idx2, 'gcodeFrom');
					}, 300);
				}
				else {
					Admin.LoadSuperRegionCode(RegionArray[0].substr(0, 1), function (result) {
						form1.regionCodeE1.value = result;
						var Idx1 = $("#regionCodeE1 option").index($("#regionCodeE1 option:selected"));
						cate1change('regionCodeE1', 'regionCodeE2', 'regionCodeE3', Idx1, 'gcodeTo');
					});
					setTimeout(function () {
						form1.regionCodeE2.value = RegionCode;
						var Idx2 = $("#regionCodeE2 option").index($("#regionCodeE2 option:selected"));
						cate2change('regionCodeE1', 'regionCodeE2', 'regionCodeE3', Idx2, 'gcodeTo');
					}, 300);
				}
			}
			else if (RegionArray[0].length == 7) {
				if (SorC == "S") {
					Admin.LoadSuperRegionCode(RegionArray[0].substr(0, 1), function (result) {
						form1.regionCodeI1.value = result;
						var Idx1 = $("#regionCodeI1 option").index($("#regionCodeI1 option:selected"));
						cate1change('regionCodeI1', 'regionCodeI2', 'regionCodeI3', Idx1, 'gcodeFrom');
					});
					setTimeout(function () {
						Admin.LoadSuperRegionCode(RegionArray[0].substr(0, 4), function (result) {
							form1.regionCodeI2.value = result;
							var Idx2 = $("#regionCodeI2 option").index($("#regionCodeI2 option:selected"));
							cate2change('regionCodeI1', 'regionCodeI2', 'regionCodeI3', Idx2, 'gcodeFrom');
						});
					}, 300);
					setTimeout(function () {
						form1.regionCodeI3.value = RegionCode;
						var Idx3 = $("#regionCodeI3 option").index($("#regionCodeI3 option:selected"));
						cate3change('regionCodeI1', 'regionCodeI2', 'regionCodeI3', Idx3, 'gcodeFrom');
					}, 600);
					
				}
				else {
					Admin.LoadSuperRegionCode(RegionArray[0].substr(0, 1), function (result) {
						form1.regionCodeE1.value = result;
						var Idx1 = $("#regionCodeE1 option").index($("#regionCodeE1 option:selected"));
						cate1change('regionCodeE1', 'regionCodeE2', 'regionCodeE3', Idx1, 'gcodeTo');
					});
					setTimeout(function () {
						Admin.LoadSuperRegionCode(RegionArray[0].substr(0, 4), function (result) {
							form1.regionCodeE2.value = result;
							var Idx2 = $("#regionCodeE2 option").index($("#regionCodeE2 option:selected"));
							cate2change('regionCodeE1', 'regionCodeE2', 'regionCodeE3', Idx2, 'gcodeTo');
						});
					}, 300);
					setTimeout(function () {
						form1.regionCodeE3.value = RegionCode;
						var Idx3 = $("#regionCodeE3 option").index($("#regionCodeE3 option:selected"));
						cate3change('regionCodeE1', 'regionCodeE2', 'regionCodeE3', Idx3, 'gcodeTo');
					}, 600);
				}
			}

			/*
			for (var i = 1; i < cate2[1].length; i++) {
				if (cate2[1][i].text == RegionCode) {
					IsFoundRegion = true;
					if (SorC == "S") {
						form1.regionCodeI1.value = "1";
						cate1change('regionCodeI1', 'regionCodeI2', 'regionCodeI3', 1, 'gcodeFrom');
						form1.regionCodeI2.value = cate2[1][i].text;
						cate2change('regionCodeI1', 'regionCodeI2', 'regionCodeI3', i, 'gcodeFrom');
					}
					else {
						form1.regionCodeE1.value = "1";
						cate1change('regionCodeE1', 'regionCodeE2', 'regionCodeE3', 1, 'gcodeTo');
						form1.regionCodeE2.value = cate2[1][i].text;
						cate2change('regionCodeE1', 'regionCodeE2', 'regionCodeE3', i, 'gcodeTo');
					}
					break;
				}
			}
			if (!IsFoundRegion) {
				for (var j = 1; j < cate3[2].length; j++) {
					for (var k = 1; k < cate3[2][j].length; k++) {
						if (cate3[2][j][k].text == RegionCode) {
							IsFoundRegion = true;
							if (SorC == "S") {
								form1.regionCodeI1.value = "3";
								cate1change('regionCodeI1', 'regionCodeI2', 'regionCodeI3', 2, 'gcodeFrom');
								form1.regionCodeI2.value = cate2[2][j].text;
								cate2change('regionCodeI1', 'regionCodeI2', 'regionCodeI3', j, 'gcodeFrom');
								form1.regionCodeI3.value = cate3[2][j][k].text;
								cate3change('regionCodeI1', 'regionCodeI2', 'regionCodeI3', k, 'gcodeFrom');
							}
							else {
								form1.regionCodeE1.value = "3";
								cate1change('regionCodeE1', 'regionCodeE2', 'regionCodeE3', 2, 'gcodeTo');
								form1.regionCodeE2.value = cate2[2][j].text;
								cate2change('regionCodeE1', 'regionCodeE2', 'regionCodeE3', j, 'gcodeTo');
								form1.regionCodeE3.value = cate3[2][j][k].text;
								cate3change('regionCodeE1', 'regionCodeE2', 'regionCodeE3', k, 'gcodeTo');
							}
							break;
						}
					}
				}
			}
			
			if (!IsFoundRegion) {
				if (SorC == "S") {
					alert("출발지를 자동설정하지 못했습니다. 직접 선택해 주세요");
					form1.regionCodeI1.value = "";
					cate1change('regionCodeI1', 'regionCodeI2', 'regionCodeI3', 0, 'gcodeFrom');
				}
				else {
					alert("도착지를 자동설정하지 못했습니다. 직접 선택해 주세요");
					form1.regionCodeE1.value = "";
					cate1change('regionCodeE1', 'regionCodeE2', 'regionCodeE3', 0, 'gcodeTo');
				}
			}
			*/
		}
		function SelectWho(SorC) {
			//var retVal = window.showModalDialog('./Dialog/RelatedCompanyList.aspx?S=' + form1.HCompanyPk.value, "", 'dialogWidth=500px;dialogHeight=500px;resizable=0;status=0;scroll=1;help=0;');
			if (SorC == "S") {
				window.open('./Dialog/RelatedCompanyList.aspx?C=S&S=' + form1.HCompanyPk.value, "", 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=500px,height=500px');
			}
			else {
				window.open('./Dialog/RelatedCompanyList.aspx?C=C&S=' + form1.HCompanyPk.value, "", 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=500px,height=500px');
			}
			//try {
			//	SelectWhoResult(SorC, retVal[1], retVal[2], retVal[3], retVal[4], retVal[5]);
			//	//if (form1.HGubun.value == "Admin") {					SelectCompanyInDocument(SorC);}
			//}
			//catch (ex)
			//{ }
		}

		function SelectCompanyInDocument(SorC) {
			var CompanyPk;
			if (form1.ShipperPk.value == "" || form1.ConsigneePk.value == "") {
				alert("수화인 / 발화인을 먼저 선택 해주세요");
				return false;
			}
			if (SorC == "S") {
				CompanyPk = form1.ShipperPk.value;
			}
			else {
				CompanyPk = form1.ConsigneePk.value;
			}


			//var DialogResult = new Array();
			DialogResult1 = form1.HGubun.value;
			DialogResult2 = SorC;
			// 20140106 부장님 요청 수정 통관명 구분
			DialogResult3 = "3157";
			DialogResult4 = form1.HAccountID.value;

			//var retVal = window.showModalDialog('../Request/Dialog/ShipperNameSelection.aspx?S=' + CompanyPk, DialogResult, 'dialogWidth=800px;dialogHeight=400px;resizable=0;status=0;scroll=1;help=0;');
			//window.open('../Request/Dialog/ShipperNameSelection.aspx?S=' + CompanyPk+'&D1='+DialogResult1+'&D2='+DialogResult2+'&D3='+DialogResult3, '', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=800px,height=400px');
			window.open('../Request/Dialog/ShipperNameSelection.aspx?S=' + CompanyPk + '&D1=' + DialogResult1 + '&D2=' + DialogResult2 + '&D3=' + DialogResult3 + '&D4=' + DialogResult4 + '&Shipper=' + form1.ShipperPk.value + '&Consignee=' + form1.ConsigneePk.value, '', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=800px,height=400px');
			//window.open('./Dialog/DeliverySet.aspx?P=' + OurBranchStorageOutPk + '&S=' + requstformpk + "&C=" + consigneepk + "&O=" + branchpk + "&A=" + form1.HAccountID.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=600px; width=800px;');
			//try {
			//	if (retVal == 'D') { SelectCompanyInDocument(SorC); }
			//	else if (retVal == "1") {
			//		if (SorC == "S") {
			//			form1.ShipperClearanceNamePk.value = "0";
			//			document.getElementById("PnShipperClearanceName").innerHTML = "<strong>" + form1.H_ClearanceSubstitute.value + "</strong>";
			//		} else {
			//			form1.ConsigneeClearanceNamePk.value = "0";
			//			document.getElementById("PnConsigneeClearanceName").innerHTML = "<strong>" + form1.H_ClearanceSubstitute.value + "</strong>";
			//		}
			//	} else if (retVal == "2") {
			//		if (SorC == "S") {
			//			form1.ShipperClearanceNamePk.value = "";
			//			document.getElementById("PnShipperClearanceName").innerHTML = "<br />";
			//		} else {
			//			form1.ConsigneeClearanceNamePk.value = "";
			//			document.getElementById("PnConsigneeClearanceName").innerHTML = "<br />";
			//		}
			//	} else {
			//		var ReturnArray = retVal.split('##');
			//		if (SorC == "S") {
			//			document.getElementById("ShipperClearanceNamePk").value = ReturnArray[0];
			//			document.getElementById("PnShipperClearanceName").innerHTML = "<strong>" + ReturnArray[1] + "</strong><br />" + ReturnArray[2];
			//		} else {
			//			document.getElementById("ConsigneeClearanceNamePk").value = ReturnArray[0];
			//			document.getElementById("PnConsigneeClearanceName").innerHTML = "<strong>" + ReturnArray[1] + "</strong><br />" + ReturnArray[2];
			//		}
			//	}
			//}
			//catch (err) { return false; }
		}
		function SelectCompanyInDocumentOpen(retVal, SorC) {
			try {
				if (retVal == 'D') { SelectCompanyInDocument(SorC); }
				else if (retVal == "1") {
					if (SorC == "S") {
						form1.ShipperClearanceNamePk.value = "0";
						document.getElementById("PnShipperClearanceName").innerHTML = "<strong>" + form1.H_ClearanceSubstitute.value + "</strong>";
					} else {
						form1.ConsigneeClearanceNamePk.value = "0";
						document.getElementById("PnConsigneeClearanceName").innerHTML = "<strong>" + form1.H_ClearanceSubstitute.value + "</strong>";
					}
				} else if (retVal == "2") {
					if (SorC == "S") {
						form1.ShipperClearanceNamePk.value = "";
						document.getElementById("PnShipperClearanceName").innerHTML = "<br />";
					} else {
						form1.ConsigneeClearanceNamePk.value = "";
						document.getElementById("PnConsigneeClearanceName").innerHTML = "<br />";
					}
				} else {
					var ReturnArray = retVal.split('##');
					if (SorC == "S") {
						document.getElementById("ShipperClearanceNamePk").value = ReturnArray[0];
						document.getElementById("PnShipperClearanceName").innerHTML = "<strong>" + ReturnArray[1] + "</strong><br />" + ReturnArray[2];
					} else {
						document.getElementById("ConsigneeClearanceNamePk").value = ReturnArray[0];
						document.getElementById("PnConsigneeClearanceName").innerHTML = "<strong>" + ReturnArray[1] + "</strong><br />" + ReturnArray[2];
					}
				}
			}
			catch (err) { return false; }
		}
		function delopt(selectCtrl, mTxt) {
			if (selectCtrl.options) {
				for (i = selectCtrl.options.length; i >= 0; i--) { selectCtrl.options[i] = null; }
			}
			selectCtrl.options[0] = new Option(mTxt, "")
			selectCtrl.selectedIndex = 0;
		}
		function LoadRegionCodeCountry(selectID) {
			delopt(document.getElementById(selectID), ":: Country ::");
			Admin.LoadReginoCodeCountry_ReturnWithPk(function (result) {
				for (var i = 0; i < result.length; i++) {
					var thisR = result[i].split("$");
					document.getElementById(selectID).options[i + 1] = new Option(thisR[1], thisR[0]);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function cate1change(first, second, third, idx, TB_RegionPk) {
			delopt(document.getElementById(second), ":: Region ::");
			delopt(document.getElementById(third), ":: Local ::");

			var FirstValue = document.getElementById(first).options[idx].value.split("|");
			Admin.LoadRegionCode_ReturnWithPk(FirstValue[0], function (result) {
				for (var i = 1; i < result.length; i++) {
					var thisR = result[i].split("$");
					document.getElementById(second).options[i] = new Option(thisR[1], thisR[0]);
				}
			});

			document.getElementById(TB_RegionPk).value = FirstValue[1];
			document.getElementById("PnUnSongWaySelect").innerHTML = "";
			SetUnsongWayByGcode();
		}
		function cate2change(first, second, third, idx, TB_RegionPk) {
			delopt(document.getElementById(third), ":: Local ::");
			cate1 = document.getElementById(first).selectedIndex;

			var SecondValue = document.getElementById(second).options[idx].value.split("|");
			Admin.LoadRegionCode_ReturnWithPk(SecondValue[0], function (result) {
				for (var i = 1; i < result.length; i++) {
					var thisR = result[i].split("$");
					document.getElementById(third).options[i] = new Option(thisR[1], thisR[0]);
				}
			});

			document.getElementById(TB_RegionPk).value = SecondValue[1];
			SetUnsongWayByGcode();
		}
		function cate3change(first, second, third, idx, TB_RegionPk) {
			var ThirdValue = document.getElementById(third).options[idx].value.split("|");
			document.getElementById(TB_RegionPk).value = ThirdValue[1];
			SetUnsongWayByGcode();
		}
		function SetUnsongWayByGcode() {
			var from = document.getElementById("gcodeFrom").value;
			var to = document.getElementById("gcodeTo").value;
			if (from != '' && to != '') {
				Request.LoadTransportWayCL(from, to, function (result) {
					var resultSplit = result.split("@@");
					var ResultHtml = "";
					var FirstLine = "";
					var SecondLine = "";
					var ThirdLine = "";
					var SixthLine = "";
					var SeverthLine = "";
					var EighthLine = "";
					var Temp;
					for (var i = 0; i < resultSplit.length - 1; i++) {
						var EachValue = resultSplit[i].split("!");
						Temp = "<input type=\"radio\" name=\"TransportWayCL\" id=\"TransportWayCL" + EachValue[0] + "\" onclick=\"SetTransportWayCL(this.value)\" value=\"" + EachValue[0] + "\" /><label for=\"TransportWayCL" + EachValue[0] + "\">" + EachValue[1].toString() + "</label>&nbsp;&nbsp;&nbsp;&nbsp;";
						switch (EachValue[2].substr(0, 1)) {
							case "1": FirstLine += Temp; break;
							case "2": SecondLine += Temp; break;
							case "3": ThirdLine += Temp; break;
							case "6": SixthLine += Temp; break;
							case "7": SeverthLine += Temp; break;
							case "8": EighthLine += Temp; break;
						}
					}
					if (FirstLine != "") {
						FirstLine = "<strong>AIR </strong> " + FirstLine.replace(/항공 /g, "") + "<br />";
					}
					if (SecondLine != "") {
						SecondLine = "<strong>COMBINED </strong> " + SecondLine.replace(/복합운송 /g, "") + "<br />";
					}
					if (ThirdLine != "") {
						ThirdLine = "<strong>SEA </strong> " + ThirdLine.replace(/국제해상 /g, "") + "<br />";
					}
					if (SixthLine != "") {
						SixthLine = "<strong>내륙항공운송 </strong> " + SixthLine.replace(/국내항공 /g, "") + "<br />";
					}
					if (SeverthLine != "") {
						SeverthLine = "<strong>내륙해상운송 </strong> " + SeverthLine.replace(/국내해상 /g, "") + "<br />";
					}
					if (EighthLine != "") {
						EighthLine = "<strong>육로운송 </strong> " + EighthLine.replace(/국내육로 /g, "") + "<br />";
					}

					ResultHtml = FirstLine + SecondLine + ThirdLine + SixthLine + SeverthLine + EighthLine;
					if (ResultHtml != "") { ResultHtml = ResultHtml.substr(0, ResultHtml.length - 6); }
					document.getElementById("PnUnSongWaySelect").innerHTML = ResultHtml;

				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function SetTransportWayCL(idx) {
			document.getElementById("UnsongWayCode").value = idx;
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

			objCell1.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][boxNo]' onkeyup='CountBox(" + rowC + "," + thisLineNo + ",this)' size='6' />";
			objCell2.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][ItemName]' size='25' />";
			objCell3.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Brand]' size='8'  />";
			objCell4.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Matarial]' size='6' />";
			objCell5.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Quantity]' onkeyup=\"QuantityXUnitCost('" + rowC + "', '" + thisLineNo + "');\" style=\"width:45px; \"  />" +
				"<select id='Item[" + rowC + "][" + thisLineNo + "][QuantityUnit]'><option value='40'>PCS</option><option value='41'>PRS</option><option value='42'>SET</option><option value='43'>S/F</option><option value='44'>YDS</option><option value='45'>M</option><option value='46'>KG</option><option value='47'>DZ</option><option value='48'>L</option><option value='49'>Box</option><option value='50'>SQM</option><option value='51'>M2</option><option value='52'>RO</option></select>";
			objCell6.innerHTML = "<input type='text' style='border:0; width:10px;' id='Item[" + rowC + "][" + thisLineNo + "][MonetaryUnit][0]' value='" + MonetaryUnit + "' /><input type='text' id='Item[" + rowC + "][" + thisLineNo + "][UnitCost]' onkeyup=\"QuantityXUnitCost('" + rowC + "', '" + thisLineNo + "');\" size='5' />";
			objCell7.innerHTML = "<input type='text' style='border:0; width:10px;' id='Item[" + rowC + "][" + thisLineNo + "][MonetaryUnit][1]'  value='" + MonetaryUnit + "' /><input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Price]' readonly='readonly' size='9' />";
			objCell8.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][PackedCount]' size='6' /><select id='Item[" + rowC + "][" + thisLineNo + "][PackingUnit]' size='1'><option value='15'>CT</option><option value='16'>RO</option><option value='17'>PA</option></select>";
			objCell9.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Weight]' onkeyup=\"GetTotal('Weight');\" size='5' />";
			objCell10.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Volume]' size='5' onkeyup=\"GetTotal('Volume');\" />";
		}
		function SelectChangeMonetaryUnit(idx) {
			switch (document.getElementById('Item[0][MonetaryUnit]')[idx].value) {
				case '18': SetMonetaryUnit("￥"); break;
				case '19': SetMonetaryUnit("$"); break;
				case '20': SetMonetaryUnit("￦"); break;
				case '21': SetMonetaryUnit("Y"); break;
				case '22': SetMonetaryUnit("$"); break;
				case '23': SetMonetaryUnit("€"); break;
				default: SetMonetaryUnit("$");
					document.getElementById("Item[0][MonetaryUnit]").value = "19";
					break;
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

		function QuantityXUnitCost(GroupNo, LineNo) {
			if (document.getElementById('Item[' + GroupNo + '][' + LineNo + '][Quantity]').value != "" && document.getElementById('Item[' + GroupNo + '][' + LineNo + '][UnitCost]').value != "") {
				document.getElementById('Item[' + GroupNo + '][' + LineNo + '][Price]').value = parseFloat(document.getElementById('Item[' + GroupNo + '][' + LineNo + '][Quantity]').value) * parseFloat(document.getElementById('Item[' + GroupNo + '][' + LineNo + '][UnitCost]').value);
				GetTotal("Quantity");
				GetTotal("Price");
			}
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
				}
			}
			var FloatTotalValue = parseInt(TotalValue + 0.00000001) / ToInt;
			if (TotalValue != 0) { document.getElementById('total[0][' + Which + ']').value = FloatTotalValue; }
		}

		function CountBox(GroupNo, LineNo, obj) {
			var reg = /[0-9]-[0-9]/;
			var reg3 = /[0-9]~[0-9]/;
			var reg2 = /[0-9]/;
			if (reg2.test(obj.value) == true) {
				document.getElementById('Item[' + GroupNo + '][' + LineNo + '][PackedCount]').value = 1;
			}
			if (reg.test(obj.value) == true) {
				var SplitResult = obj.value.split('-');
				var result = SplitResult[1] - SplitResult[0];
				if (result < 0) { result = SplitResult[0] - SplitResult[1]; }
				document.getElementById('Item[' + GroupNo + '][' + LineNo + '][PackedCount]').value = result + 1;
			}
			if (reg3.test(obj.value) == true) {
				var SplitResult = obj.value.split('~');
				var result = SplitResult[1] - SplitResult[0];
				if (result < 0) { result = SplitResult[0] - SplitResult[1]; }
				document.getElementById('Item[' + GroupNo + '][' + LineNo + '][PackedCount]').value = result + 1;
			}
			GetTotal("PackedCount");
		}
		function Paste(Which) {
			if (confirm("기존데이터가 사라지고 복사한 데이터로 대체됩니다.")) {
				var RowLength = document.getElementById('ItemTable[0]').rows.length - 2;
				var clipboard = window.clipboardData.getData("Text");
				var each = clipboard.split("\n");
				for (var i = 0; i < each.length; i++) {
					if (i >= RowLength) {
						InsertRow('0');
					}
					document.getElementById('Item[0][' + i + '][' + Which + ']').value = each[i];
				}
			}
		}
		function ChangeCount_All(SetValue) {
			var RowLength0 = document.getElementById('ItemTable[0]').rows.length - 2;

			for (i = 0; i < RowLength0; i++) {
				document.getElementById("Item[0][" + i + "][QuantityUnit]").value = SetValue;
			}
		}
		function ItemCALC(gubun) {
			var i;
			var SQuantity = "";
			var SPrice = "";
			var SPackingCount = "";
			var SWeight = "";
			var SVolume = "";

			var RowLength0 = document.getElementById('ItemTable[0]').rows.length - 2;

			for (i = 0; i < RowLength0; i++) {
				if (document.getElementById("Item[0][" + i + "][Quantity]").value != "") { SQuantity += document.getElementById("Item[0][" + i + "][Quantity]").value + "#@!"; }
				SPrice += document.getElementById("Item[0][" + i + "][Quantity]").value + "*" + document.getElementById("Item[0][" + i + "][UnitCost]").value + "#@!";
				if (document.getElementById("Item[0][" + i + "][PackedCount]").value != "") { SPackingCount += document.getElementById("Item[0][" + i + "][PackedCount]").value + "#@!"; }
				if (document.getElementById("Item[0][" + i + "][Weight]").value != "") { SWeight += document.getElementById("Item[0][" + i + "][Weight]").value + "#@!"; }
				if (document.getElementById("Item[0][" + i + "][Volume]").value != "") { SVolume += document.getElementById("Item[0][" + i + "][Volume]").value + "#@!"; }
			}
			Request.ItemCALC(SQuantity, SPrice, SPackingCount, SWeight, SVolume, function (result) {
				document.getElementById("total[0][Quantity]").value = result[0];
				document.getElementById("total[0][PackedCount]").value = result[2];
				document.getElementById("total[0][Weight]").value = result[3];
				document.getElementById("total[0][Volume]").value = result[4];
				var priceRow = result[1].split("!");
				for (i = 0; i < RowLength0; i++) {
					document.getElementById("Item[0][" + i + "][Price]").value = priceRow[i];
				}
				document.getElementById("total[0][Price]").value = priceRow[priceRow.length - 1];
				CheckCalc = true;
				if (gubun != "") {
					alert(gubun);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function AddNewCCL(which) {
			var DialogResult = form1.HCompanyPk.value + '!' + form1.HAccountID.value + "!" + $("#AccountType").val() + "!" + $("#BranchPk").val();
			var retVal = window.showModalDialog('./Dialog/OwnCustomerAdd.aspx', DialogResult, 'dialogWidth=900px;dialogHeight=650px;resizable=0;status=0;scroll=1;help=0;');
			try {
				var ReturnArray = retVal.split('##');
				document.getElementById('TB_ImportCompanyName').value = ReturnArray[0] + ' (' + ReturnArray[1] + ')';
				document.getElementById('TB_CustomerListPk').value = ReturnArray[2];
			}
			catch (err) { return false; }
		}


		function Btn_Submit_Click() {
			form1.BTN_SUBMIT.style.visibility = "hidden";
			if (document.getElementById("gcodeFrom").value == "") { alert(form1.HMustDepartureArea.value); form1.BTN_SUBMIT.style.visibility = "visible"; return false; }
			if (form1.ShipperPk.value == "") { alert(form1.HMustShipper.value); form1.BTN_SUBMIT.style.visibility = "visible"; return false; }
			if (!CheckCalc) { ItemCALC(''); }
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			var SHIPPERPK = form1.ShipperPk.value;
			var SHIPPERCODE = form1.ShipperCode.value;
			var CONSIGNEEPK = form1.ConsigneePk.value;
			var CONSIGNEECODE = form1.ConsigneeCode.value;
			var SHIPPERCLEARANCE = form1.ShipperClearanceNamePk.value;
			var CONSIGNEECLEARANCE = form1.ConsigneeClearanceNamePk.value;
			var ACCOUNTID = form1.HAccountID.value;

			var FROM = form1.gcodeFrom.value;
			var TO = form1.gcodeTo.value;

			var DEPARTUREDATE = form1.TB_DepartureDate.value;
			var ARRIVALDATE = form1.TB_ArrivalDate.value;
			var SHIPMENTDATE = form1.TB_ShipmentDate.value;
			var MONETARYUNIT = document.getElementById("Item[0][MonetaryUnit]").value;
			if (MONETARYUNIT == "0") { MONETARYUNIT = "19"; }

			var paymentWho;
			if (document.form1.Payment3[0].checked) { paymentWho = "5"; }
			else if (document.form1.Payment3[1].checked) { paymentWho = "6"; }
			else if (document.form1.Payment3[2].checked) { paymentWho = "7"; }
			else if (document.form1.Payment3[3].checked) { paymentWho = "8"; }
			else if (document.form1.Payment3[4].checked) { paymentWho = "9"; }
			else { paymentWho = "0"; }

			var TransportWay = document.getElementById("UnsongWayCode").value;
			var PAYMENTETC = form1.PaymentETC.value;

			var DocumentRequest = "";
			if (document.getElementById('CertificateOfOrigin').checked) { DocumentRequest += "31!"; }
			if (document.getElementById('ProductCheked').checked) { DocumentRequest += "32!"; }
			if (document.getElementById('SuChec').checked) { DocumentRequest += "33!"; }
			if (document.getElementById('FTACheked').checked) { DocumentRequest += "34!"; }
			var ONLYOURSTAFFETC = form1.OnlyBranchStaff.value;

			var ITEMSUM = "";
			var RowLength0 = document.getElementById('ItemTable[0]').rows.length - 2;
			var BranchPk_Departure = document.getElementById("DepartureBranch").value;
			if ($("#AccountType").val() == "ShippingBranch") {
				BranchPk_Departure = $("#BranchPk").val();
			}
			
			if (form1.HMode.value == "Write") {

				for (var i = 0; i < RowLength0; i++) {
					if (document.getElementById("Item[0][" + i + "][boxNo]").value != "" ||
						document.getElementById("Item[0][" + i + "][ItemName]").value != "" ||
						document.getElementById("Item[0][" + i + "][Brand]").value != "" ||
						document.getElementById("Item[0][" + i + "][Matarial]").value != "" ||
						document.getElementById("Item[0][" + i + "][Quantity]").value != "" ||
						document.getElementById("Item[0][" + i + "][UnitCost]").value != "" ||
						document.getElementById("Item[0][" + i + "][Price]").value != "" ||
						document.getElementById("Item[0][" + i + "][PackedCount]").value != "" ||
						document.getElementById("Item[0][" + i + "][Weight]").value != "" ||
						document.getElementById("Item[0][" + i + "][Volume]").value != "") {
						ITEMSUM += document.getElementById("Item[0][" + i + "][boxNo]").value + "#@!" +
										document.getElementById("Item[0][" + i + "][ItemName]").value.replace("'", "''") + "#@!" +
										document.getElementById("Item[0][" + i + "][Brand]").value + "#@!" +
										document.getElementById("Item[0][" + i + "][Matarial]").value + "#@!" +
										document.getElementById("Item[0][" + i + "][Quantity]").value + "#@!" +
										document.getElementById("Item[0][" + i + "][QuantityUnit]").value + "#@!" +
										document.getElementById("Item[0][" + i + "][UnitCost]").value + "#@!" +
										document.getElementById("Item[0][" + i + "][Price]").value + "#@!" +
										document.getElementById("Item[0][" + i + "][PackedCount]").value + "#@!" +
										document.getElementById("Item[0][" + i + "][PackingUnit]").value + "#@!" +
										document.getElementById("Item[0][" + i + "][Weight]").value + "#@!" +
										document.getElementById("Item[0][" + i + "][Volume]").value + "%!$@#";
					}
				}
				var isEstimation = "0";
				if ($("#AccountType").val() != "ShippingBranch") {
					isEstimation = $("#IsEstimation :selected").val();
				}

				/*
				var isElecronicCommerce = "0";
				if ($("#ElectonicCommerce").is(":checked")) {
					isElecronicCommerce = "1";
				}
				*/

				Request.RequestWrite(form1.HCompanyPk.value, SHIPPERPK, CONSIGNEEPK, ACCOUNTID, SHIPPERCODE, CONSIGNEECODE, SHIPPERCLEARANCE, CONSIGNEECLEARANCE, DEPARTUREDATE, ARRIVALDATE, SHIPMENTDATE, FROM, TO, TransportWay, paymentWho, DocumentRequest, MONETARYUNIT, PAYMENTETC, ONLYOURSTAFFETC, BranchPk_Departure, document.getElementById("ArrivalBranch").value, ITEMSUM, isEstimation, function (result) {
					if (result == "1") {
						window.alert("SAVE OK");
						form1.BTN_SUBMIT.style.visibility = "visible";
						parent.location.href = "../default.aspx";
					}
					else {
						form1.BTN_SUBMIT.style.visibility = "visible";
						alert("ERROR : " + result);
						form1.DEBUG.value = result;
					}
				}, function (result) {
					form1.BTN_SUBMIT.style.visibility = "visible";
					alert("ERROR : " + result);
					form1.DEBUG.value = result;
				});
			}
			else {
				var RowLength0 = document.getElementById('ItemTable[0]').rows.length - 2;
				for (var i = 0; i < RowLength0; i++) {
					if (ITEMPK[i] != undefined) {
						if (ITEMBEFORE[i] != document.getElementById("Item[0][" + i + "][boxNo]").value + "#@!" + document.getElementById("Item[0][" + i + "][ItemName]").value + "#@!" + document.getElementById("Item[0][" + i + "][Brand]").value + "#@!" + document.getElementById("Item[0][" + i + "][Matarial]").value + "#@!" + document.getElementById("Item[0][" + i + "][Quantity]").value + "#@!" + document.getElementById("Item[0][" + i + "][QuantityUnit]").value + "#@!" + document.getElementById("Item[0][" + i + "][PackedCount]").value + "#@!" + document.getElementById("Item[0][" + i + "][PackingUnit]").value + "#@!" + document.getElementById("Item[0][" + i + "][UnitCost]").value + "#@!" + document.getElementById("Item[0][" + i + "][Price]").value + "#@!" + document.getElementById("Item[0][" + i + "][Weight]").value + "#@!" + document.getElementById("Item[0][" + i + "][Volume]").value) {
							ITEMSUM += ITEMPK[i] + "#@!" +
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
					else if (document.getElementById("Item[0][" + i + "][boxNo]").value != "" ||
								document.getElementById("Item[0][" + i + "][ItemName]").value != "" ||
								document.getElementById("Item[0][" + i + "][Brand]").value != "" ||
								document.getElementById("Item[0][" + i + "][Matarial]").value != "" ||
								document.getElementById("Item[0][" + i + "][Quantity]").value != "" ||
								document.getElementById("Item[0][" + i + "][UnitCost]").value != "" ||
								document.getElementById("Item[0][" + i + "][Price]").value != "" ||
								document.getElementById("Item[0][" + i + "][PackedCount]").value != "" ||
								document.getElementById("Item[0][" + i + "][Weight]").value != "" ||
								document.getElementById("Item[0][" + i + "][Volume]").value != "") {
						ITEMSUM += "NEW#@!" +
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
				if (Weight_Before != document.getElementById("total[0][Weight]").value || Volume_Before != document.getElementById("total[0][Volume]").value) {
					Request.ResetRequestFormCalculate(form1.HRequestFormPk.value, function (result) {
						if (result == "1") {
							alert("운임 초기화");
						}

						Request.RequestModify(form1.HRequestFormPk.value, SHIPPERPK, CONSIGNEEPK, ACCOUNTID, SHIPPERCODE, CONSIGNEECODE, SHIPPERCLEARANCE, CONSIGNEECLEARANCE, DEPARTUREDATE, ARRIVALDATE, SHIPMENTDATE, FROM, TO, TransportWay, paymentWho, DocumentRequest, MONETARYUNIT, PAYMENTETC, ONLYOURSTAFFETC, BranchPk_Departure, document.getElementById("ArrivalBranch").value, ITEMSUM, function (result) {
							if (result == "1") {
								if (form1.HGubun.value == "Admin" && HaveCalcuatedHead == true) {
									Request.RequestFormCalculateHeadModify(form1.HRequestFormPk.value, document.getElementById("total[0][PackingUnit]").value, document.getElementById("total[0][PackedCount]").value, document.getElementById("total[0][Weight]").value, document.getElementById("total[0][Volume]").value, function (result) {
									}, function (result) { alert("ERROR : " + result); });
								}
								window.alert("SAVE OK");
								form1.BTN_SUBMIT.style.visibility = "visible";
								if (form1.HGubun.value == "Admin") {
									window.returnValue = true; returnValue = "Y"; self.close();
								}
								else {
									history.back();
								}
							}
							else {
								form1.BTN_SUBMIT.style.visibility = "visible";
								alert("ERROR : " + result);
								form1.DEBUG.value = result;
							}
						}, function (result) {
							form1.BTN_SUBMIT.style.visibility = "visible";
							alert("ERROR : " + result);
							form1.DEBUG.value = result;
						});

					}, function (result) { alert("ERROR : " + result); });
				} else {

					Request.RequestModify(form1.HRequestFormPk.value, SHIPPERPK, CONSIGNEEPK, ACCOUNTID, SHIPPERCODE, CONSIGNEECODE, SHIPPERCLEARANCE, CONSIGNEECLEARANCE, DEPARTUREDATE, ARRIVALDATE, SHIPMENTDATE, FROM, TO, TransportWay, paymentWho, DocumentRequest, MONETARYUNIT, PAYMENTETC, ONLYOURSTAFFETC, BranchPk_Departure, document.getElementById("ArrivalBranch").value, ITEMSUM, function (result) {
						if (result == "1") {
							if (form1.HGubun.value == "Admin" && HaveCalcuatedHead == true) {
								Request.RequestFormCalculateHeadModify(form1.HRequestFormPk.value, document.getElementById("total[0][PackingUnit]").value, document.getElementById("total[0][PackedCount]").value, document.getElementById("total[0][Weight]").value, document.getElementById("total[0][Volume]").value, function (result) {
								}, function (result) { alert("ERROR : " + result); });
							}
							window.alert("SAVE OK");
							form1.BTN_SUBMIT.style.visibility = "visible";
							if (form1.HGubun.value == "Admin") {
								window.returnValue = true; returnValue = "Y"; self.close();
							}
							else {
								history.back();
							}
						}
						else {
							form1.BTN_SUBMIT.style.visibility = "visible";
							alert("ERROR : " + result);
							form1.DEBUG.value = result;
						}
					}, function (result) {
						form1.BTN_SUBMIT.style.visibility = "visible";
						alert("ERROR : " + result);
						form1.DEBUG.value = result;
					});
				}
			}
		}
		function PopClearancedItemHistory() {
			var ShipperPk = document.getElementById("ShipperPk").value;
			var ConsigneePk = document.getElementById("ConsigneePk").value;
			if (ShipperPk != "" && ConsigneePk != "") {
				window.open("../Admin/Dialog/RequestClearanceItemHistory.aspx?S=" + ShipperPk + "&C=" + ConsigneePk + "&L=7", '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=yes, top=200px; left=200px; height=500px; width=400px;');
			}
			else {
				alert("Must select shipper and consignee");
			}
		}

	</script>
	<style type="text/css">
		input {
			text-align: center;
		}

		.tdSubT {
			border-bottom: solid 2px #93A9B8;
			padding-top: 30px;
		}

		.td01 {
			background-color: #f5f5f5;
			text-align: center;
			height: 20px;
			width: 130px;
			border-bottom: dotted 1px #E8E8E8;
			padding-top: 4px;
			padding-bottom: 4px;
		}

		.td02 {
			width: 330px;
			border-bottom: dotted 1px #E8E8E8;
			padding-top: 4px;
			padding-bottom: 4px;
			padding-left: 10px;
		}

		.td023 {
			width: 760px;
			padding-top: 4px;
			padding-bottom: 4px;
			padding-left: 10px;
			border-bottom: dotted 1px #E8E8E8;
		}
	</style>
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 10px; overflow: visible;">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Request.asmx" />
				<asp:ServiceReference Path="~/WebService/Admin.asmx" />
			</Services>
		</asp:ScriptManager>
		<uc2:LogedWithoutRecentRequest ID="LogedWithoutRecentRequest1" runat="server" />
		<uc1:Loged ID="Loged1" runat="server" />
		<div style="background-color: White; padding: 15px; color: Black;">
			<input type="hidden" id="DEBUG" onclick="this.select();" />
			<input type="hidden" id="AccountType" value="<%=Session["Type"] %>" />
			<input type="hidden" id="BranchPk" value="<%=Session["CompanyPk"] %>" />
			<input type="hidden" id="gcodeFrom" value="" />
			<input type="hidden" id="gcodeTo" value="" />
			<input type="hidden" id="ShipperPk" />
			<input type="hidden" id="ShipperCode" />
			<input type="hidden" id="ShipperRelatedPk" />
			<input type="hidden" id="ConsigneePk" />
			<input type="hidden" id="ConsigneeCode" />
			<input type="hidden" id="ConsigneeRelatedPk" />
			<input type="hidden" id="ShipperClearanceNamePk" />
			<input type="hidden" id="ConsigneeClearanceNamePk" />

			<div>
				<% if (Session["Type"] + "" != "ShippingBranch") {%>
					<div style="float: right;">
						<select id="IsEstimation">
							<option value="0">일반</option>
							<option value="1">견적</option>
						</select>
					</div>				
				<% } %>
				<p>&nbsp;&nbsp;&nbsp;<img src="../Images/ico_arrow.gif" alt="" /><strong> <%= GetGlobalResourceObject("RequestForm", "Schedule") %></strong></p>
			</div>
			<div style="float: right; background-color: #C0C0C0; width: 410px; padding: 10px;">
				<div>
					<div style="float: right;">
						<input type="button" value="select" onclick="SelectWho('C');" />
						<input type="button" value="new" style="color: Red;" onclick="AddNewCCL('C');" />
					</div>
					<strong>Consignee</strong>&nbsp;&nbsp;
				<span id="PnConsigneeName"></span>
				</div>
				<div style="padding-top: 20px;">
					<div style="float: left;">
						<input type="button" value="<%= GetGlobalResourceObject("RequestForm", "xhdrhksauddn") %>" onclick="SelectCompanyInDocument('C');" />
					</div>
					<div id="PnConsigneeClearanceName">
						<%--<strong>YANTAI BINGCHENG INTERNATIONAL TRADE CO.,LTD</strong><br />
					<span>NO.213 NANDAJIE STREET YANTAI CHINA</span>--%>
						<br />
					</div>
				</div>
				<div style="padding-top: 20px;">
					<select id="regionCodeE1" style="width: 100px;" name="country" size="1" onchange="cate1change('regionCodeE1','regionCodeE2','regionCodeE3',this.selectedIndex,'gcodeTo')">
						<option value="">:: Country ::</option>
					</select>
					<select id="regionCodeE2" style="width: 140px;" name="office" size="1" onchange="cate2change('regionCodeE1','regionCodeE2','regionCodeE3',this.selectedIndex,'gcodeTo')">
						<option value="">:: Area ::</option>
					</select>
					<select id="regionCodeE3" style="width: 140px; visibility: visible;" name="area" size="1" onchange="cate3change('regionCodeE1','regionCodeE2','regionCodeE3',this.selectedIndex,'gcodeTo')">
						<option value="">:: Local ::</option>
					</select>
				</div>
				<%=SelectArrivalBranch %>
			</div>
			<div style="background-color: #FAEBD7; width: 410px; padding: 10px;">
				<div>
					<div style="float: right;">
						<input type="button" value="select" onclick="SelectWho('S');" />
						<input type="button" value="new" style="color: Red;" onclick="AddNewCCL('S');" />
					</div>
					<strong>Shipper</strong>&nbsp;&nbsp;
				<span id="PnShipperName"></span>
				</div>
				<div style="padding-top: 20px;">
					<div style="float: left;">
						<input type="button" value="<%= GetGlobalResourceObject("RequestForm", "xhdrhksaudwhk") %>" onclick="SelectCompanyInDocument('S');" />
					</div>
					<div id="PnShipperClearanceName">
						<br />
					</div>
				</div>
				<div style="padding-top: 20px;">
					<select id="regionCodeI1" style="width: 100px;" name="country" size="1" onchange="cate1change('regionCodeI1','regionCodeI2','regionCodeI3',this.selectedIndex,'gcodeFrom')">
						<option value="">:: Country ::</option>
					</select>
					<select id="regionCodeI2" style="width: 140px;" name="office" onchange="cate2change('regionCodeI1','regionCodeI2','regionCodeI3',this.selectedIndex,'gcodeFrom')">
						<option value="">:: Area ::</option>
					</select>
					<select id="regionCodeI3" style="width: 140px; visibility: visible;" name="area" onchange="cate3change('regionCodeI1','regionCodeI2','regionCodeI3',this.selectedIndex,'gcodeFrom')">
						<option value="">:: Local ::</option>
					</select>
				</div>
				<%=SelectDepartureBranch %>
			</div>
			<table width="870px" style="background-color: White;" border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td class="td01">Date</td>
					<td class="td023">
						<input id="TB_DepartureDate" size="10" type="text" readonly="readonly" />&nbsp;~&nbsp;
					<input id="TB_ArrivalDate" size="10" type="text" readonly="readonly" />
					</td>
				</tr>
				<tr>
					<td class="td01">Date of shipment</td>
					<td class="td023">
						<input id="TB_ShipmentDate" size="10" type="text" readonly="readonly" />
					</td>
				</tr>
				<tr>
					<td class="td01">Type</td>
					<td class="td023">
						<span id="PnUnSongWaySelect"></span>
						<input type="hidden" id="UnsongWayCode" />
						<span id="FCLOnly" style="visibility: hidden;">
							<select id="FCLType">
								<option value="0">Type</option>
								<option value="20DRY">20DRY</option>
								<option value="40DRY">40DRY</option>
								<option value="40HQ">40HQ</option>
							</select>&nbsp;&nbsp;&nbsp;
						갯수 :
							<input id="FCLCount" type="text" size="8" />
						</span>
					</td>
				</tr>
				<tr>
					<td class="td01">&nbsp;</td>
					<td class="td023">
						<input type="checkbox" id="ElectonicCommerce" />&nbsp;<label for="ElectonicCommerce">전자상거래</label>
					</td>
				</tr>

				<tr>
					<td class="td01"><% =GetGlobalResourceObject("RequestForm", "PaymentWho") %></td>
					<td colspan="3" style="border-bottom: dotted 1px #E8E8E8; padding-top: 4px; padding-bottom: 4px; padding-left: 10px;">
						<div style="background-color: #f5f5f5; text-align: center; border-bottom: dotted 1px #E8E8E8; width: 430px; height: 65px; float: right; margin-right: 10px; margin-top: 5px; padding: 10px;">
							<textarea id="PaymentETC" style="overflow: hidden;" rows="4" cols="70" onfocus="this.select();"></textarea>
						</div>
						<input type="radio" name="Payment3" id="EXW" value="5" /><label for="EXW"><%=GetGlobalResourceObject("RequestForm", "PaymentA") %></label><br />
						<input type="radio" name="Payment3" id="DDP" value="6" /><label for="DDP"><%=GetGlobalResourceObject("RequestForm", "PaymentB") %></label><br />
						<input type="radio" name="Payment3" id="CNF" value="7" /><label for="CNF"><%=GetGlobalResourceObject("RequestForm", "PaymentC") %></label><br />
						<input type="radio" name="Payment3" id="FOB" value="8" /><label for="FOB"><%=GetGlobalResourceObject("RequestForm", "PaymentD") %></label><br />
						<input type="radio" name="Payment3" id="ETC" value="9" onclick="document.getElementById('PaymentETC').focus();" /><label for="ETC"><%=GetGlobalResourceObject("RequestForm", "ETC") %></label>
					</td>
				</tr>
			</table>
		</div>
		<div style="background-color: White; color: Black; padding-left: 15px; padding-right: 15px; padding-bottom: 15px;">
			<table id="ItemTable[0]" style="background-color: White; width: 870px;" border="0" cellpadding="0" cellspacing="0">
				<thead>
					<tr>
						<td class="tdSubT" colspan="10">&nbsp;&nbsp;&nbsp;<img src="../Images/ico_arrow.gif" alt="" /><strong> <%=GetGlobalResourceObject("RequestForm", "Freight") %></strong>
							&nbsp;&nbsp;&nbsp;
							<input type="button" onclick="Paste('boxNo');" value="<%=GetGlobalResourceObject("RequestForm", "BoxNo") %>" />
							<input type="button" onclick="Paste('ItemName');" value="<%=GetGlobalResourceObject("RequestForm", "Description") %>" />
							<input type="button" onclick="Paste('Brand');" value="<%=GetGlobalResourceObject("RequestForm", "Label") %>" />
							<input type="button" onclick="Paste('Matarial');" value="<%=GetGlobalResourceObject("RequestForm", "Material") %>" />
							<input type="button" onclick="Paste('Quantity');" value="<%=GetGlobalResourceObject("RequestForm", "Count") %>" />
							<input type="button" onclick="Paste('UnitCost');" value="<%=GetGlobalResourceObject("RequestForm", "UnitCost") %>" />
							<input type="button" onclick="Paste('PackedCount');" value="<%=GetGlobalResourceObject("RequestForm", "PackingCount") %>" />
							<input type="button" onclick="Paste('Weight');" value="<%=GetGlobalResourceObject("RequestForm", "GrossWeight") %>" />
							<input type="button" onclick="Paste('Volume');" value="<%=GetGlobalResourceObject("RequestForm", "Volume") %>" />
							<span style="float: right; padding-right: 10px;">
								<select id="Item[0][MonetaryUnit]" size="1" onchange="SelectChangeMonetaryUnit(this.selectedIndex)">
									<option value="0"><%=GetGlobalResourceObject("RequestForm", "MonetaryUnitChange") %></option>
									<option value="18">RMB ￥</option>
									<option value="19">USD $</option>
									<option value="20">KRW ￦</option>
									<option value="21">JPY Y</option>
									<option value="22">HKD HK$</option>
									<option value="23">EUR €</option>
								</select>
								<input type="button" onclick="InsertRow('0');" value="<%=GetGlobalResourceObject("RequestForm", "InsertItem") %>" />
							</span>
						</td>
					</tr>
					<tr style="height: 30px;">
						<td bgcolor="#F5F5F5" height="20" align="center" width="50"><%=GetGlobalResourceObject("RequestForm", "BoxNo") %></td>
						<td bgcolor="#F5F5F5" align="center" width="175"><%=GetGlobalResourceObject("RequestForm", "Description") %></td>
						<td bgcolor="#F5F5F5" align="center" width="75"><%=GetGlobalResourceObject("RequestForm", "Label") %></td>
						<td bgcolor="#F5F5F5" align="center" width="65"><%=GetGlobalResourceObject("RequestForm", "Material") %></td>
						<td bgcolor="#F5F5F5" align="center" width="110">
							<select onchange="ChangeCount_All(this.value);" ><option value='40'>PCS</option><option value='41'>PRS</option><option value='42'>SET</option><option value='43'>S/F</option><option value='44'>YDS</option><option value='45'>M</option><option value='46'>KG</option><option value='47'>DZ</option><option value='48'>L</option><option value='49'>Box</option><option value='50'>SQM</option><option value='51'>M2</option><option value='52'>RO</option></select>
						</td>
						<td bgcolor="#F5F5F5" align="center" width="70"><%=GetGlobalResourceObject("RequestForm", "UnitCost") %></td>
						<td bgcolor="#F5F5F5" align="center" width="90"><%=GetGlobalResourceObject("RequestForm", "Amount") %></td>
						<td bgcolor="#F5F5F5" align="center" width="110"><%=GetGlobalResourceObject("RequestForm", "PackingCount") %></td>
						<td bgcolor="#F5F5F5" align="center" width="40"><%=GetGlobalResourceObject("RequestForm", "GrossWeight") %> (Kg)</td>
						<td bgcolor="#F5F5F5" align="center"><%=GetGlobalResourceObject("RequestForm", "Volume") %> (CBM)</td>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
			<table style="background-color: White; width: 870px;" border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td bgcolor="#F5F5F5" height="30" align="right">
						<input type="button" value="Clearanced Item History" onclick="PopClearancedItemHistory();" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<%=GetGlobalResourceObject("RequestForm", "Total") %>&nbsp;&nbsp;&nbsp;<input type="button" value="CALC" onclick="ItemCALC('Done');" />&nbsp;&nbsp;&nbsp;</td>
					<td bgcolor="#F5F5F5" style="width: 110px;">&nbsp;&nbsp;<input type="text" readonly="readonly" id="total[0][Quantity]" size="6" /></td>
					<td bgcolor="#F5F5F5" width="80" align="right">&nbsp;</td>
					<td bgcolor="#F5F5F5" style="width: 95px;" align="left">
						<input type="text" style="border: 0; background-color: #F5F5F5; width: 10px;" id="Item[0][MonetaryUnit][Total]" readonly="readonly" />
						<input type="text" id="total[0][Price]" style="width: 70px;" readonly="readonly" />
					</td>
					<td bgcolor="#F5F5F5" style="width: 105px;">
						<input type="text" id="total[0][PackedCount]" size="6" readonly="readonly" /><select id='total[0][PackingUnit]' size='1'><option value='15'>CT</option>
							<option value='16'>RO</option>
							<option value='17'>PA</option>
							<option value='18'>M2</option>
						</select></td>
					<td bgcolor="#F5F5F5" style="width: 55px; text-align: right;">
						<input type="text" id="total[0][Weight]" style="width: 47px;" readonly="readonly" /></td>
					<td style="background-color: #F5F5F5; width: 62px;">&nbsp;<input type="text" id="total[0][Volume]" style="width: 50px;" readonly="readonly" />
					</td>
				</tr>
			</table>
			<table width="870px" style="background-color: White;" border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td class="td01" style="border-top: solid 1px #93A9B8; border-bottom: solid 2px #93A9B8;"><%=GetGlobalResourceObject("RequestForm", "TradeDocumentRequest") %></td>
					<td class="td023" colspan="3" style="border-top: solid 1px #93A9B8; border-bottom: solid 2px #93A9B8;">
						<input type="checkbox" id="CertificateOfOrigin" /><label for="CertificateOfOrigin">화주원산지제공(客户提供产地证)</label>&nbsp;&nbsp;&nbsp;&nbsp;
					<input type="checkbox" id="ProductCheked" /><label for="ProductCheked">원산지대리신청(代理申请产地证)</label>&nbsp;&nbsp;&nbsp;&nbsp;
					<input type="checkbox" id="FTACheked" /><label for="FTACheked">FTA원산지대리신청(FTA代理申请产地证)</label>&nbsp;&nbsp;&nbsp;&nbsp;<br />
						<input type="checkbox" id="SuChec" /><label for="SuChec">단증보관(单证报关)</label>
					</td>
				</tr>
			</table>
		</div>
		<div style="background-color: White; padding: 15px; color: Black;">
			<p>&nbsp;&nbsp;&nbsp;<img src="../Images/ico_arrow.gif" alt="" /><strong> <%=GetGlobalResourceObject("RequestForm", "RequestFormETC") %></strong>  : <%=GetGlobalResourceObject("qjsdur", "dkfodprlwoehlsaptpwlsmsanffbghltkwlrdnjsaksqhftndlTtmqslek")%></p>
			<div style="text-align: center;">
				<textarea id="OnlyBranchStaff" style="overflow: hidden;" rows="5" cols="140"></textarea>
			</div>
			<%=OnlyStorcedIn %>
		</div>

		<div style="background-color: White; padding: 10px; line-height: 20px; text-align: left;">
			<p>
				&nbsp;&nbsp;&nbsp;<img src="../Images/ico_arrow.gif" alt="" /><strong> <%=GetGlobalResourceObject("RequestForm", "dksso") %></strong> : <%=GetGlobalResourceObject("RequestForm", "dkssorhkdehdtjddptj")%>
			</p>
		</div>

		<div style="background-color: White; padding-top: 5px; padding-bottom: 25px; text-align: center;">
			<input type="button" id="BTN_SUBMIT" value="  <%=GetGlobalResourceObject("RequestForm", "Submit") %>  " onclick="Btn_Submit_Click()" style="width: 150px; height: 40px;" />&nbsp;&nbsp;&nbsp;&nbsp;
		<input type="button" value="  <%=GetGlobalResourceObject("Member", "Cancel") %>  " style="width: 150px; height: 40px;" onclick="history.back();" />
		</div>
		<input type="hidden" id="HCompanyPk" value="<%=CompanyPk %>" />
		<input type="hidden" id="HRequestFormPk" value="<%=RequestFormPk %>" />
		<input type="hidden" id="HAccountID" value="<%=AccountID %>" />
		<input type="hidden" id="HGubun" value="<%=Request.Params["G"] %>" />
		<input type="hidden" id="HMustDepartureArea" value="<%=GetGlobalResourceObject("Alert", "MustDepartureArea") %>" />
		<input type="hidden" id="HMustShipper" value="<%=GetGlobalResourceObject("Alert", "MustShipper") %>" />
		<input type="hidden" id="H_ClearanceSubstitute" value="<%=GetGlobalResourceObject("RequestForm", "ClearanceSubstitute") %>" />
		<input type="hidden" id="HMode" value="<%=Mode %>" />
	</form>
</body>
</html>