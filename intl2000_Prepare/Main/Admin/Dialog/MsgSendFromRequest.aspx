<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MsgSendFromRequest.aspx.cs" Inherits="Admin_Dialog_MsgSendFromRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script type="text/javascript">
		var REQUESTFORMPK;
		var ACCOUNTID;
		var SHIPPERPK;
		var CONSIGNEEPK;
		var CountShipperStaff = 0;
		var CountConsigneeStaff = 0;
		var OURBRANCHPK;
		var GUBUN;
		var MSGSchedule = "";
		var ArrivalPort = "";
		var K_Notice1 = "[안내] \r\n 당사의 베트남 지사가 설립되었습니다. \r\n 전화: 097-230-9990 \r\n";
		var K_Notice2 = "[안내] \r\n 당사의 광주사무실과 창고를 확장 이전하였습니다. \r\n 전화:020-8609-1668 / 138-2621-4498, \r\n 창고전화:137-1086-5533, \r\n 주소:广州市白云区嘉禾街弘森国际物流中E1栋101-103号. \r\n";
		var C_Notice1 = "[提示] \r\n 我司成立越南分公司 \r\n 电话: 097-2309990 \r\n";
		var C_Notice2 = "[提示] \r\n 广州公司办公室和仓库扩张乔迁. \r\n 电话: 020-8609-1668 / 138-2621-4498, \r\n 仓库手机: 137-1086-5533, \r\n 地址: 广州市白云区嘉禾街弘森国际物流中E1栋101-103号. \r\n";
		var OriginSMS = "";
		window.onload = function () {
			REQUESTFORMPK = dialogArguments[0];
			ACCOUNTID = dialogArguments[1];
			OURBRANCHPK = dialogArguments[2];
			GUBUN = dialogArguments[3];
			Admin.PrepareSendMessageFromRequest(REQUESTFORMPK, function (result) {
				var DepartureStaff = "";
				var ArrivalStaff = "";
				var Schedule;
				var Owner;
				var CheckedWeight;
				var Fright;
				var ShipperStaff = "";

				var ConsigneeStaff = "";
				var CheckedS = "";
				if (GUBUN == "s") {
					CheckedS = "checked = \"checked\"";
				}
				var CheckedC = "";
				if (GUBUN == "c") {
					CheckedC = "checked = \"checked\"";
				}
				for (var i = 0; i < result.length; i++) {
					var Row = result[i].split("#@!");
					switch (Row[0]) {
						case "DepartureStaff":
							DepartureStaff = Row[1];
							break;
						case "ArrivalStaff":
							ArrivalStaff = Row[1];
							break;
						case "ShipperPk":
							SHIPPERPK = Row[1];
							break;
						case "ConsigneePk":
							CONSIGNEEPK = Row[1];
							break;
						case "Schedule":
							Schedule = Row[1];
							break;
						case "ArrivalPort":
							ArrivalPort = Row[1];
							break;
						case "CompanyCode":
							Owner = Row[1];
							break;
						case "Fright":
							Fright = Row[1];
							break;
						case "ShipperStaff":
							var TEMP = "";
							var Email = false;
							var SMS = false;
							TEMP += "<tr><td>" + Row[1] + " " + Row[2] +
														"<input type=\"hidden\" id=\"ShipperStaff[" + CountShipperStaff + "]Name\" value=\"" + Row[1] + " " + Row[2] + "\" />" +
														"<input type=\"hidden\" id=\"ShipperStaff[" + CountShipperStaff + "]SorC\" value=\"S\" /></td>";
							if (Row[5] == "2" || Row[5] == "3") {
								if (Row[4] != "") {
									TEMP += "<td><input type=\"hidden\" id=\"ShipperIsEmail[" + CountShipperStaff + "]\" value=\"Y\" />" +
																	"<input type=\"checkbox\" " + CheckedS + " id=\"ShipperStaff[" + CountShipperStaff + "]E\" value=\"" + Row[4] + "\" /><label for=\"ShipperStaff[" + CountShipperStaff + "]E\">" + Row[4] + "</label></td>";
									Email = true;
								}
							}
							if (!Email) { TEMP += "<td><input type=\"hidden\" id=\"ShipperIsEmail[" + CountShipperStaff + "]\" value=\"N\" /></td>"; }

							if (Row[5] == "3" || Row[5] == "1") {
								if (Row[3] != "") {
									TEMP += "<td><input type=\"hidden\" id=\"ShipperIsSMS[" + CountShipperStaff + "]\" value=\"Y\" />" +
																	"<input type=\"checkbox\" " + CheckedS + " id=\"ShipperStaff[" + CountShipperStaff + "]S\" value=\"" + Row[3] + "\" /><label for=\"ShipperStaff[" + CountShipperStaff + "]S\">" + Row[3] + "</label></td></tr>";
									SMS = true;
								}
							}
							if (!SMS) {
								TEMP += "<td><input type=\"hidden\" id=\"ShipperIsSMS[" + CountShipperStaff + "]\" value=\"N\" /></td></tr>";
							}

							if (Email | SMS) {
								ShipperStaff += TEMP;
								CountShipperStaff++;
							}
							break;
						case "ConsigneeStaff":
							var TEMP = "";
							var Email = false;
							var SMS = false;
							TEMP += "<tr><td>" + Row[1] + " " + Row[2] +
														"<input type=\"hidden\" id=\"ConsigneeStaff[" + CountConsigneeStaff + "]Name\" value=\"" + Row[1] + " " + Row[2] + "\" />" +
														"<input type=\"hidden\" id=\"ConsigneeStaff[" + CountConsigneeStaff + "]SorC\" value=\"C\" /></td>";
							if (Row[5] == "2" || Row[5] == "3") {
								if (Row[4] != "") {
									TEMP += "<td><input type=\"hidden\" id=\"ConsigneeIsEmail[" + CountConsigneeStaff + "]\" value=\"Y\" />" +
																	"<input type=\"checkbox\" " + CheckedC + " id=\"ConsigneeStaff[" + CountConsigneeStaff + "]E\" value=\"" + Row[4] + "\" /><label for=\"ConsigneeStaff[" + CountConsigneeStaff + "]E\">" + Row[4] + "</label></td>";
									Email = true;
								}
							}
							if (!Email) { TEMP += "<td><input type=\"hidden\" id=\"ConsigneeIsEmail[" + CountConsigneeStaff + "]\" value=\"N\" /></td>"; }

							if (Row[5] == "3" || Row[5] == "1") {
								if (Row[3] != "") {
									TEMP += "<td><input type=\"hidden\" id=\"ConsigneeIsSMS[" + CountConsigneeStaff + "]\" value=\"Y\" />" +
													"<input type=\"checkbox\" " + CheckedC + " id=\"ConsigneeStaff[" + CountConsigneeStaff + "]S\" value=\"" + Row[3] + "\" /><label for=\"ConsigneeStaff[" + CountConsigneeStaff + "]S\">" + Row[3] + "</label>" +
													"<input type=\"hidden\" id=\"Maccountpk" + CountConsigneeStaff + "\" value=\"" + Row[6] + "\" />" +
													"<input type=\"hidden\" id=\"Maccoutnmobile" + CountConsigneeStaff + "\" value=\"" + Row[3] + "\" />" +
																	"</td></tr>";
									SMS = true;
								}
							}
							if (!SMS) {
								TEMP += "<td><input type=\"hidden\" id=\"ConsigneeIsSMS[" + CountConsigneeStaff + "]\" value=\"N\" /></td></tr>";
							}
							if (Email | SMS) {
								ConsigneeStaff += TEMP;
								CountConsigneeStaff++;
							}

							break;
					}
				}
				document.getElementById("PnMsgMain").innerHTML = "<ul type=\"disc\" style=\"line-height:20px; \">" + Owner + Schedule + Fright + "</ul>";
				document.getElementById("PnShipper").innerHTML = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td colspan=\"3\">담당자 <input type=\"text\" id=\"StaffS\" style='width:300px;' value=\"" + DepartureStaff + "\"  /> </td></tr><tr><td>이름</td><td>Email</td><td>SMS</td></tr>" + ShipperStaff + "</table>";
				document.getElementById("PnConsignee").innerHTML = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td colspan=\"3\">담당자 <input type=\"text\" id=\"StaffC\" style='width:300px;' value=\"" + ArrivalStaff + "\"   /> </td></tr><tr><td>이름</td><td>Email</td><td>SMS</td></tr>" + ConsigneeStaff + "</table>";
				if (GUBUN == "s") {
					SMSDefaultLoad("C1");
				}
				else if (GUBUN == "c") {
					SMSDefaultLoad("K1");
				}

			}, function (result) { alert("ERROR : " + result); });
		}

		function BTNSubmitClick() {
			var EmailList = "";
			var SMSList = "";

			for (var i = 0; i < CountShipperStaff; i++) {
				if (document.getElementById("ShipperIsEmail[" + i + "]").value == "Y" && document.getElementById("ShipperStaff[" + i + "]E").checked) {
					EmailList += document.getElementById("ShipperStaff[" + i + "]SorC").value + "#@!" + document.getElementById("ShipperStaff[" + i + "]Name").value + "#@!" + document.getElementById("ShipperStaff[" + i + "]E").value + "%!$@#";
				}
				if (document.getElementById("ShipperIsSMS[" + i + "]").value == "Y" && document.getElementById("ShipperStaff[" + i + "]S").checked) {
					SMSList += document.getElementById("ShipperStaff[" + i + "]SorC").value + "#@!" + document.getElementById("ShipperStaff[" + i + "]Name").value + "#@!" + document.getElementById("ShipperStaff[" + i + "]S").value + "%!$@#";
				}
			}
			for (var i = 0; i < CountConsigneeStaff; i++) {
				if (document.getElementById("ConsigneeIsEmail[" + i + "]").value == "Y" && document.getElementById("ConsigneeStaff[" + i + "]E").checked) {
					EmailList += document.getElementById("ConsigneeStaff[" + i + "]SorC").value + "#@!" + document.getElementById("ConsigneeStaff[" + i + "]Name").value + "#@!" + document.getElementById("ConsigneeStaff[" + i + "]E").value + "%!$@#";
				}
				if (document.getElementById("ConsigneeIsSMS[" + i + "]").value == "Y" && document.getElementById("ConsigneeStaff[" + i + "]S").checked) {
					SMSList += document.getElementById("ConsigneeStaff[" + i + "]SorC").value + "#@!" + document.getElementById("ConsigneeStaff[" + i + "]Name").value + "#@!" + document.getElementById("ConsigneeStaff[" + i + "]S").value + "%!$@#";
				}
			}
			var MainContents = document.getElementById("PnMsgMain").innerHTML + "<div style=\"font-size:14px; line-height:25px; padding:10px; background-color:#DDDDDD;\">" + form1.TBMSG.value;
			var ShipperStaff = document.getElementById("StaffS").value;
			var ConsigneeStaff = document.getElementById("StaffC").value;
			
			Admin.MsgSendFromRequest(ACCOUNTID, MainContents, EmailList, SMSList, ShipperStaff, ConsigneeStaff, SHIPPERPK, CONSIGNEEPK, REQUESTFORMPK, OURBRANCHPK, form1.TBSMS.value, form1.K1Check.value, function (result) {
				if (result != "1") {
					alert(result);
					return false;
				}
				alert(form1.dhksfy.value); window.returnValue = true; returnValue = "Y"; self.close();
			}, function (result) { alert("ERROR : " + result); });
		}
		function SMSDefaultLoad(thisid) {
			
			var text;
			var MPARAM;
			switch (thisid) {
				case "K_Notice1":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					/*
					text = "[안내]" + "\r\n"
					text += "당사의 베트남 지사가 설립되었습니다." + "\r\n";
					text += "전화: 097-230-9990" + "\r\n";
					*/
					text = K_Notice1;


					form1.K1Check.value = "";
					break;
				case "K_Notice2":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}
					/*
					text = "[안내]" + "\r\n"
					text += "당사의 광주사무실과 창고를 확장 이전하였습니다." + "\r\n";
					text += "전화:020-8609-1668 / 138-2621-4498," + "\r\n"
					text += "창고전화:137-1086-5533," + "\r\n";
					text += "주소:广州市白云区嘉禾街弘森国际物流中E1栋101-103号." + "\r\n";
					*/
					text = K_Notice2;

					form1.K1Check.value = "";
					break;
				case "K_DescriptionS1":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					var Qcheck;
					for (var i = 0; i < CountConsigneeStaff; i++) {
						try {
							if (document.getElementById("Maccoutnmobile" + i + "").value + "" != "") {
								var pattern = /[^(0-9)]/gi;
								var REQUESTFORMPKEncryption0 = parseInt(parseInt(REQUESTFORMPK) / 99);
								var REQUESTFORMPKEncryption1 = parseInt(parseInt(REQUESTFORMPK) % 99);

								var QPARAM = REQUESTFORMPKEncryption0 + "!" + REQUESTFORMPKEncryption1;

								text = "안녕하세요? 귀하의 화물 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 이 출발예정입니다." + "\r\n";
								text += "nn21.net/Request/RequestFormView.aspx?Q=" + QPARAM + "\r\n"
								text += "화물명세와 도착지를 확인하시기 바랍니다." + "\r\n";
								text += "접수내용에 따라 수출입 통관 및 제반 물류업무가 실행되오니 유의하시기 바랍니다." + "\r\n";
								text += "* 국제종합물류 *";

								Qcheck = "99";
							}
						} catch (e) {

						}

						if (Qcheck == "99") {
							break;
						}
					}
					form1.K1Check.value = QPARAM;
					break;
				case "K_DescriptionS2":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					text = "안녕하세요? 귀하의 화물 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 이 출발예정입니다." + "\r\n";
					text += "발송인으로부터 화물명세를 받지 못하여 물류업무를 실행할 수 없는바 운송일정에 차질이 우려됩니다." + "\r\n";
					text += "이에 귀사의 조속한 조치를 바랍니다." + "\r\n";
					text += "* 국제종합물류 *";

					form1.K1Check.value = "";
					break;
				case "K_DescriptionC1":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					var Qcheck;
					for (var i = 0; i < CountConsigneeStaff; i++) {
						try {
							if (document.getElementById("Maccoutnmobile" + i + "").value + "" != "") {
								var pattern = /[^(0-9)]/gi;
								var REQUESTFORMPKEncryption0 = parseInt(parseInt(REQUESTFORMPK) / 99);
								var REQUESTFORMPKEncryption1 = parseInt(parseInt(REQUESTFORMPK) % 99);

								var QPARAM = REQUESTFORMPKEncryption0 + "!" + REQUESTFORMPKEncryption1;

								text = "안녕하세요? 귀하의 화물 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 이 도착예정입니다." + "\r\n";
								text += "nn21.net/Request/RequestFormView.aspx?Q=" + QPARAM + "\r\n"
								text += "화물명세와 도착지를 확인하시기 바랍니다." + "\r\n";
								text += "접수내용에 따라 수출입 통관 및 제반 물류업무가 실행되오니 유의하시기 바랍니다." + "\r\n";
								text += "* 국제종합물류 *";

								Qcheck = "99";
							}
						} catch (e) {

						}

						if (Qcheck == "99") {
							break;
						}
					}
					form1.K1Check.value = QPARAM;
					break;
				case "K_DescriptionC2":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					text = "안녕하세요? 귀하의 화물 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 이 도착예정입니다." + "\r\n";
					text += "발송인으로부터 화물명세를 받지 못하여 물류업무를 실행할 수 없는바 운송일정에 차질이 우려됩니다." + "\r\n";
					text += "이에 귀사의 조속한 조치를 바랍니다." + "\r\n";
					text += "* 국제종합물류 *";

					form1.K1Check.value = "";
					break;
				case "K_ChargeS1":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					Admin.LoadChargeForMSG_Carryover(REQUESTFORMPK, "S", function (result) {
						var DateTemp = "";
						if (document.getElementById("HSchedule").value != "" || document.getElementById("HSchedule").value != null) {
							DateTemp = document.getElementById("HSchedule").value.substr(0, 2) + "-" + document.getElementById("HSchedule").value.substr(2, 2);
						}
						else {
							DateTemp = document.getElementById("HSchedule").value;
						}

						text = "안녕하세요? 귀사의 화물 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 이 발송되었으니 " + DateTemp + "일 14시 까지 물류비용을 지급하여 주시기 바랍니다." + "\r\n";
						text += "물류비 총액은 " + result[3] + " 이고, 자세한 사항은 nn21.net 고객페이지 에서 확인하실 수 있습니다." + "\r\n";
						text += "입금하실 계좌는 " + result[4] + " 입니다." + "\r\n";
						text += "지정된 기간내 물류비용이 입금되지 않으면 물류업무는 보류됩니다." + "\r\n";
						text += "* 국제종합물류 *";

						form1.TBSMS.value = text;

					}, function (result) { alert("ERROR : " + result); });

					form1.K1Check.value = "";
					break;
				case "K_ChargeS2":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					Admin.LoadChargeForMSG_Carryover(REQUESTFORMPK, "S", function (result) {
						var DateTemp = "";
						if (document.getElementById("HSchedule").value != "" || document.getElementById("HSchedule").value != null) {
							DateTemp = document.getElementById("HSchedule").value.substr(0, 2) + "-" + document.getElementById("HSchedule").value.substr(2, 2);
						}
						else {
							DateTemp = document.getElementById("HSchedule").value;
						}

						text = "귀사의 화물 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 이 통관예정이오니 " + DateTemp + "일 14시 까지 통관자금을 지급하여 주시기 바랍니다." + "\r\n";
						text += "물류비 총액은 " + result[3] + " 이고, 자세한 사항은 nn21.net 고객페이지 에서 확인하실 수 있습니다." + "\r\n";
						text += "입금하실 계좌는 " + result[4] + " 입니다." + "\r\n";
						text += "지정된 기간내 통관자금이 입금되지 않을경우 통관의사가 없는것으로 간주하여 통관은 보류됩니다." + "\r\n";
						text += "* 국제종합물류 *";

						form1.TBSMS.value = text;

					}, function (result) { alert("ERROR : " + result); });

					form1.K1Check.value = "";
					break;
				case "K_ChargeC1":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					Admin.LoadChargeForMSG_Carryover(REQUESTFORMPK, "C", function (result) {
						var DateTemp = "";
						if (document.getElementById("HSchedule").value != "" || document.getElementById("HSchedule").value != null) {
							DateTemp = document.getElementById("HSchedule").value.substr(0, 2) + "-" + document.getElementById("HSchedule").value.substr(2, 2);
						}
						else {
							DateTemp = document.getElementById("HSchedule").value;
						}

						text = "안녕하세요? 귀사의 화물 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 이 발송되었으니 " + DateTemp + "일 14시 까지 물류비용을 지급하여 주시기 바랍니다." + "\r\n";
						text += "물류비 총액은 " + result[3] + " 이고, 자세한 사항은 nn21.net 고객페이지 에서 확인하실 수 있습니다." + "\r\n";
						text += "입금하실 계좌는 " + result[4] + " 입니다." + "\r\n";
						text += "지정된 기간내 물류비용이 입금되지 않으면 물류업무는 보류됩니다." + "\r\n";
						text += "* 국제종합물류 *";

						form1.TBSMS.value = text;

					}, function (result) { alert("ERROR : " + result); });

					form1.K1Check.value = "";
					break;
				case "K_ChargeC2":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					Admin.LoadChargeForMSG_Carryover(REQUESTFORMPK, "C", function (result) {
						var DateTemp = "";
						if (document.getElementById("HSchedule").value != "" || document.getElementById("HSchedule").value != null) {
							DateTemp = document.getElementById("HSchedule").value.substr(0, 2) + "-" + document.getElementById("HSchedule").value.substr(2, 2);
						}
						else {
							DateTemp = document.getElementById("HSchedule").value;
						}

						text = "귀사의 화물 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 이 통관예정이오니 " + DateTemp + "일 14시 까지 통관자금을 지급하여 주시기 바랍니다." + "\r\n";
						text += "물류비 총액은 " + result[3] + " 이고, 자세한 사항은 nn21.net 고객페이지 에서 확인하실 수 있습니다." + "\r\n";
						text += "입금하실 계좌는 " + result[4] + " 입니다." + "\r\n";
						text += "지정된 기간내 통관자금이 입금되지 않을경우 통관의사가 없는것으로 간주하여 통관은 보류됩니다." + "\r\n";
						text += "* 국제종합물류 *";

						form1.TBSMS.value = text;

					}, function (result) { alert("ERROR : " + result); });

					form1.K1Check.value = "";
					break;

				case "K_Madein1":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					text = "귀사의 상품은 한국통관시 관세 감면을 받을 수 있는 원산지증명 적용대상 품목입니다." + "\r\n";
					text += "원산지 증명의 발급신청은 통관전일(정상근무일) 오전 12시까지 입니다." + "\r\n";
					text += "아직까지도 화물명세를 받지 못하여 원산지증명서 발급신청을 할 수 없으니 참고하시기 바랍니다." + "\r\n";
					text += "* 국제종합물류 *";

					form1.K1Check.value = "";
					break;
				case "C_Notice1":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					/*
					text = "[안내]" + "\r\n"
					text += "당사의 베트남 지사가 설립되었습니다." + "\r\n";
					text += "전화: 097-230-9990" + "\r\n";
					*/
					text = C_Notice1;


					form1.K1Check.value = "";
					break;
				case "C_Notice2":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}
					/*
					text = "[안내]" + "\r\n"
					text += "당사의 광주사무실과 창고를 확장 이전하였습니다." + "\r\n";
					text += "전화:020-8609-1668 / 138-2621-4498," + "\r\n"
					text += "창고전화:137-1086-5533," + "\r\n";
					text += "주소:广州市白云区嘉禾街弘森国际物流中E1栋101-103号." + "\r\n";
					*/
					text = C_Notice2;

					form1.K1Check.value = "";
					break;
				case "C_DescriptionS1":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					var Qcheck;
					for (var i = 0; i < CountConsigneeStaff; i++) {
						try {
							if (document.getElementById("Maccoutnmobile" + i + "").value + "" != "") {
								var pattern = /[^(0-9)]/gi;
								var REQUESTFORMPKEncryption0 = parseInt(parseInt(REQUESTFORMPK) / 99);
								var REQUESTFORMPKEncryption1 = parseInt(parseInt(REQUESTFORMPK) % 99);

								var QPARAM = REQUESTFORMPKEncryption0 + "!" + REQUESTFORMPKEncryption1;

								text = "您好！ 贵司 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 件货物已接收, " + "\r\n";
								text += "nn21.net/Request/RequestFormView.aspx?Q=" + QPARAM + "\r\n"
								text += "网址确认接收内容 (货物明细，目的地)." + "\r\n";
								text += "进出口通关业务按照接收内容实行, 因此请注意确认接收内容." + "\r\n";
								text += "* 国际物流 *";

								Qcheck = "99";
							}
						} catch (e) {

						}

						if (Qcheck == "99") {
							break;
						}
					}
					form1.K1Check.value = QPARAM;
					break;
				case "C_DescriptionS2":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					text = "您好! 贵司的 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 件货物已确人接收. " + "\r\n";
					text += "因没有货物明细, 能影响货物无法进行及延误运送日程." + "\r\n";
					text += "望贵司及时安排处理." + "\r\n";
					text += "* 国际物流 *";

					form1.K1Check.value = "";
					break;
				case "C_DescriptionC1":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					var Qcheck;
					for (var i = 0; i < CountConsigneeStaff; i++) {
						try {
							if (document.getElementById("Maccoutnmobile" + i + "").value + "" != "") {
								var pattern = /[^(0-9)]/gi;
								var REQUESTFORMPKEncryption0 = parseInt(parseInt(REQUESTFORMPK) / 99);
								var REQUESTFORMPKEncryption1 = parseInt(parseInt(REQUESTFORMPK) % 99);

								var QPARAM = REQUESTFORMPKEncryption0 + "!" + REQUESTFORMPKEncryption1;

								text = "您好！ 贵司 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 件货物已接收, " + "\r\n";
								text += "nn21.net/Request/RequestFormView.aspx?Q=" + QPARAM + "\r\n"
								text += "网址确认接收内容 (货物明细，目的地)." + "\r\n";
								text += "进出口通关业务按照接收内容实行, 因此请注意确认接收内容." + "\r\n";
								text += "* 国际物流 *";

								Qcheck = "99";
							}
						} catch (e) {

						}

						if (Qcheck == "99") {
							break;
						}
					}
					form1.K1Check.value = QPARAM;
					break;
				case "C_DescriptionC2":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					text = "您好! 贵司的 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 件货物已确人接收. " + "\r\n";
					text += "因没有货物明细, 能影响货物无法进行及延误运送日程." + "\r\n";
					text += "望贵司及时安排处理." + "\r\n";
					text += "* 国际物流 *";

					form1.K1Check.value = "";
					break;
				case "C_ChargeS1":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					Admin.LoadChargeForMSG_Carryover(REQUESTFORMPK, "S", function (result) {
						var DateTemp = "";
						if (document.getElementById("HSchedule").value != "" || document.getElementById("HSchedule").value != null) {
							DateTemp = document.getElementById("HSchedule").value.substr(0, 2) + "-" + document.getElementById("HSchedule").value.substr(2, 2);
						}
						else {
							DateTemp = document.getElementById("HSchedule").value;
						}

						text = "您好! 贵司 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 件货物已发送, 望在" + DateTemp + "日 14点之前支付物流费用." + "\r\n";
						text += "物流费用总额 " + result[3] + " , 详细明细在我司网址 nn21.net 上确认." + "\r\n";
						text += "汇款账户信息 " + result[4] + "\r\n";
						text += "请在指定时间内支付物流费用, 以免影响物流业务进程." + "\r\n";
						text += "* 国际物流 *";

						form1.TBSMS.value = text;

					}, function (result) { alert("ERROR : " + result); });

					form1.K1Check.value = "";
					break;
				case "C_ChargeS2":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					Admin.LoadChargeForMSG_Carryover(REQUESTFORMPK, "S", function (result) {
						var DateTemp = "";
						if (document.getElementById("HSchedule").value != "" || document.getElementById("HSchedule").value != null) {
							DateTemp = document.getElementById("HSchedule").value.substr(0, 2) + "-" + document.getElementById("HSchedule").value.substr(2, 2);
						}
						else {
							DateTemp = document.getElementById("HSchedule").value;
						}

						text = "贵司 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 件货物预计清关, 请在" + DateTemp + "日 14点之前支付如下清关费用." + "\r\n";
						text += "物流费用总额 " + result[3] + " , 详细明细在我司网址 nn21.net 上确认." + "\r\n";
						text += "汇款账户信息 " + result[4] + "\r\n";
						text += "请在指定时间内支付物流费用, 以免影响清关业务进程." + "\r\n";
						text += "* 国际物流 *";

						form1.TBSMS.value = text;

					}, function (result) { alert("ERROR : " + result); });

					form1.K1Check.value = "";
					break;
				case "C_ChargeC1":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					Admin.LoadChargeForMSG_Carryover(REQUESTFORMPK, "C", function (result) {
						var DateTemp = "";
						if (document.getElementById("HSchedule").value != "" || document.getElementById("HSchedule").value != null) {
							DateTemp = document.getElementById("HSchedule").value.substr(0, 2) + "-" + document.getElementById("HSchedule").value.substr(2, 2);
						}
						else {
							DateTemp = document.getElementById("HSchedule").value;
						}

						text = "您好! 贵司 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 件货物已发送, 望在" + DateTemp + "日 14点之前支付物流费用." + "\r\n";
						text += "物流费用总额 " + result[3] + " , 详细明细在我司网址 nn21.net 上确认." + "\r\n";
						text += "汇款账户信息 " + result[4] + "\r\n";
						text += "请在指定时间内支付物流费用, 以免影响物流业务进程." + "\r\n";
						text += "* 国际物流 *";

						form1.TBSMS.value = text;

					}, function (result) { alert("ERROR : " + result); });

					form1.K1Check.value = "";
					break;
				case "C_ChargeC2":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					Admin.LoadChargeForMSG_Carryover(REQUESTFORMPK, "C", function (result) {
						var DateTemp = "";
						if (document.getElementById("HSchedule").value != "" || document.getElementById("HSchedule").value != null) {
							DateTemp = document.getElementById("HSchedule").value.substr(0, 2) + "-" + document.getElementById("HSchedule").value.substr(2, 2);
						}
						else {
							DateTemp = document.getElementById("HSchedule").value;
						}

						text = "贵司 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + " 件货物预计清关, 请在" + DateTemp + "日 14点之前支付如下清关费用." + "\r\n";
						text += "物流费用总额 " + result[3] + " , 详细明细在我司网址 nn21.net 上确认." + "\r\n";
						text += "汇款账户信息 " + result[4] + "\r\n";
						text += "请在指定时间内支付物流费用, 以免影响清关业务进程." + "\r\n";
						text += "* 国际物流 *";

						form1.TBSMS.value = text;

					}, function (result) { alert("ERROR : " + result); });

					form1.K1Check.value = "";
					break;
				case "C_Madein1":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}

					text = "贵司的产品在韩国清关时适用减免税的原产地证明." + "\r\n";
					text += "原产地证明申请时间为清关前日（正常工作日）上午12点之前." + "\r\n";
					text += "到目前为止尚未提供货物明细, 可能影响申请产地证，望参考!!" + "\r\n";
					text += "* 国际物流 *";

					form1.K1Check.value = "";
					break;


				case "C1":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					}
					for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}
					text = "您好 : " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + "箱货物已入库，详情请上 nn21.net 网站上确认--[国际物流]";

					form1.K1Check.value = "";
					break;
				case "C2":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					}
					for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}
					text = "您好 : " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + "您好 : 5CT箱货物已入库，详情请上 nn21.net 网站上确认--[国际物流]各位客户大家好，因公司业务发展需要，自2017年4月1日起，将搬迁至新的办公地址：广州市白云区嘉禾街弘森国际物流中E1栋101-103号. 电话:020-8609-1668 / 138-2621-4498 仓库手机：137-1086-5533，如因公司搬迁给您与贵司带来的不便，我们深表歉意";

					form1.K1Check.value = "";
					break;
				case "K1":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").checked = false;
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "disabled";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").checked = false;
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "disabled";
						}
					}
					for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}
					var Qcheck;
					for (var i = 0; i < CountConsigneeStaff; i++) {
						try {
							if (document.getElementById("Maccoutnmobile" + i + "").value + "" != "") {
								var pattern = /[^(0-9)]/gi;
								//var accountmobile = document.getElementById("Maccoutnmobile" + i + "").value.replace(pattern, "");
								//var Checkaccountmobile = accountmobile.substr(accountmobile.length - 3, 3);
								//var accoutnmobileEncryption0 = parseInt(parseInt(Checkaccountmobile) / 99);
								//var accoutnmobileEncryption1 = parseInt(parseInt(Checkaccountmobile) % 99);
								var REQUESTFORMPKEncryption0 = parseInt(parseInt(REQUESTFORMPK) / 99);
								var REQUESTFORMPKEncryption1 = parseInt(parseInt(REQUESTFORMPK) % 99);
								//var accountpk = document.getElementById("Maccountpk" + i + "").value;

								//var MPARAM = accoutnmobileEncryption0 + "!" + accoutnmobileEncryption1 + "!" + REQUESTFORMPKEncryption0 + "!" + REQUESTFORMPKEncryption1 + "!" + accountpk;
								var QPARAM = REQUESTFORMPKEncryption0 + "!" + REQUESTFORMPKEncryption1;

								text = "귀하의 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + "화물이 " + document.getElementById("HArrivalName").value;
								/*
								if (ArrivalPort == "") {
									text += "한국";
								} else {
									text += "한국(" + ArrivalPort + ")";s
								}
								*/
								var DateTemp = "";
								if (document.getElementById("HSchedule").value != "" || document.getElementById("HSchedule").value != null) {
									DateTemp = document.getElementById("HSchedule").value.substr(0, 2) + "-" + document.getElementById("HSchedule").value.substr(2, 2);
								}
								else {
									DateTemp = document.getElementById("HSchedule").value;
								}

								text += "으로 도착예정(" + DateTemp + ") 입니다. nn21.net/Request/RequestFormView.aspx?Q=" + QPARAM + " 에서 화물명세를 확인하시고 도착지를 지정해주시기 바랍니다. -국제물류 아이엘";
								Qcheck = "99";
							}
						} catch (e) {

						}

						if (Qcheck == "99") {
							break;
						}
					}
					//핸드폰 체크
					if (Qcheck != "99") {
						alert("<주의> 수화인의 핸드폰 정보가 없습니다.(没有 收货人的 电话)");
						text = "<주의> 수화인의 핸드폰 정보가 없습니다.(没有 收货人的 电话)";
					}
					form1.K1Check.value = QPARAM;
					break;
				case "ChargeLoad":
					Admin.LoadChargeForMSG(REQUESTFORMPK, "C", function (result) {
						for (var i = 0; i < CountShipperStaff; i++) {
							if (document.getElementById("ShipperStaff[" + i + "]E")) {
								document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
							}
							if (document.getElementById("ShipperStaff[" + i + "]S")) {
								document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
							}
						}
						for (var i = 0; i < CountConsigneeStaff; i++) {
							if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
								document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
							}
						}
						text = "귀사의 화물 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + "가 접수되었습니다. " + document.getElementById("HSchedule").value + "일 도착예정이오니 프로그램 확인 후 Document OK 바라며 아래 물류비를 출고당시 2시전까지 입금바랍니다.";
						if (result[0] != "0") {
							text += " 물류비 : " + result[0] + " ";
						}
						if (result[1] != "0") {
							text += " 통관비 : " + result[1] + " ";
						}

						if (result[2] != "0") {
							text += " 배달비 : " + result[2] + " ";
						}
						text += " 총 입금하실 금액은 : " + result[3] + " 입니다.";
						text += " 계좌번호 " + result[4] + " 로 입금바랍니다.";
						text += " - 국제물류 아이엘(nn21.net)";
						form1.TBSMS.value = text;
						
					}, function (result) { alert("ERROR : " + result); });
					
					form1.K1Check.value = "";
					break;
			   case "ChargeLoad_Carryover":
			      Admin.LoadChargeForMSG_Carryover(REQUESTFORMPK, "C", function (result) {
			         for (var i = 0; i < CountShipperStaff; i++) {
			            if (document.getElementById("ShipperStaff[" + i + "]E")) {
			               document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
			            }
			            if (document.getElementById("ShipperStaff[" + i + "]S")) {
			               document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
			            }
			         }
			         for (var i = 0; i < CountConsigneeStaff; i++) {
			            if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
			               document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
			            }
			         }
			         text = "귀사의 화물 " + document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + "가 접수되었습니다. " + document.getElementById("HSchedule").value + "일 도착예정이오니 프로그램 확인 후 Document OK 바라며 아래 물류비를 출고당시 2시전까지 입금바랍니다.";
			         if (result[0] != "0") {
			            text += " 물류비 : " + result[0] + " ";
			         }
			         if (result[1] != "0") {
			            text += " 통관비 : " + result[1] + " ";
			         }

			         if (result[2] != "0") {
			            text += " 배달비 : " + result[2] + " ";
			         }
			         if (result[5] != "0") {
			            text += " 전잔 : " + result[5] + " ";
			         }
			         text += " 총 입금하실 금액은 : " + result[3] + " 입니다.";
			         text += " 계좌번호 " + result[4] + " 로 입금바랍니다.";
			         text += " - 국제물류 아이엘(nn21.net)";
			         form1.TBSMS.value = text;

			      }, function (result) { alert("ERROR : " + result); });

			      form1.K1Check.value = "";
			      break;
				case "K3":
					Admin.LoadSelfgeForMSG(REQUESTFORMPK, function (result) {
						for (var i = 0; i < CountShipperStaff; i++) {
							if (document.getElementById("ShipperStaff[" + i + "]E")) {
								document.getElementById("ShipperStaff[" + i + "]E").checked = false;
								document.getElementById("ShipperStaff[" + i + "]E").disabled = "disabled";
							}
							if (document.getElementById("ShipperStaff[" + i + "]S")) {
								document.getElementById("ShipperStaff[" + i + "]S").checked = false;
								document.getElementById("ShipperStaff[" + i + "]S").disabled = "disabled";
							}
						}
						for (var i = 0; i < CountConsigneeStaff; i++) {
							if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
								document.getElementById("ConsigneeStaff[" + i + "]E").checked = false;
								document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "disabled";
							}
						}
						if (result[0] + "" == "" || result[0] + "" == "null") {
							alert("B/L 정보가 없습니다");
							text = "B/L 정보가 없습니다";
							return false;
						}
						if (result[1] + "" == "" || result[1] + "" =="null") {
							alert("창고정보가 없습니다");
							text = "창고정보가 없습니다";
							return false;
						}
						text = "귀사의 수입화물 정보는 다음과 같사오니 참고하여 주시기 바랍니다 \r\n";
						text += "B/L번호:" + result[0] + "\r\n";
						text += result[2] + "\r\n";
						text += "창고:" + result[1] + "\r\n";
						text += " - 국제물류주식회사";
						form1.TBSMS.value = text;

					}, function (result) { alert("ERROR : " + result); });

					form1.K1Check.value = "";
					break;
					
				case "K4":
					for (var i = 0; i < CountShipperStaff; i++) {
						if (document.getElementById("ShipperStaff[" + i + "]E")) {
							document.getElementById("ShipperStaff[" + i + "]E").disabled = "";
						}
						if (document.getElementById("ShipperStaff[" + i + "]S")) {
							document.getElementById("ShipperStaff[" + i + "]S").disabled = "";
						}
					} for (var i = 0; i < CountConsigneeStaff; i++) {
						if (document.getElementById("ConsigneeStaff[" + i + "]E")) {
							document.getElementById("ConsigneeStaff[" + i + "]E").disabled = "";
						}
					}
					//text = "화물접수되었습니다. 프로그램에서 확인후 DOCUMENT OK 눌러주세요 - 국제물류 아이엘";
					text = document.getElementById("HPackedCount").value + document.getElementById("HPackingUnit").value + "화물이 도착예정입니다. nn21.net 에서 접수명세를 확인하시기 바랍니다.-국제물류 아이엘";

					form1.K1Check.value = "";
					break;
				
			}
			form1.TBSMS.value = text;
			OriginSMS = thisid;
		}
		function AddNotice(NoticeID) {
			SMSDefaultLoad(OriginSMS)
			var BeforeMSG = form1.TBSMS.value;
			var NoticeFlag1 = "";
			var NoticeFlag2 = "";

			if (OriginSMS.substr(0, 1) == "K") {
				NoticeFlag1 = K_Notice1;
				NoticeFlag2 = K_Notice2;
			} else {
				NoticeFlag1 = C_Notice1;
				NoticeFlag2 = C_Notice2;
			}

			if (document.getElementById("Chk_Notice1").checked == true && document.getElementById("Chk_Notice2").checked == true) {
				form1.TBSMS.value = NoticeFlag1 + NoticeFlag2 + BeforeMSG;
			}
			else if (document.getElementById("Chk_Notice1").checked == true && document.getElementById("Chk_Notice2").checked == false) {
				form1.TBSMS.value = NoticeFlag1 + BeforeMSG;
			}
			else if (document.getElementById("Chk_Notice1").checked == false && document.getElementById("Chk_Notice2").checked == true) {
				form1.TBSMS.value = NoticeFlag2 + BeforeMSG;
			}
			else {
				SMSDefaultLoad(OriginSMS);
			}
		}
	</script>
	<link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #999999; padding-left: 10px; padding-right: 10px; width: 480px; overflow-x: hidden;">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Admin.asmx" />
			</Services>
		</asp:ScriptManager>
		<div style="width: 420px; padding-left: 10px; padding-right: 10px; background-color: white;">
			<div id="PnMsgMain"></div>
			SMS <br />
			<input type="button" value="공지1" id="K_Notice1" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="공지2" id="K_Notice2" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="公告1" id="C_Notice1" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="公告2" id="C_Notice2" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" /><br />

			SMS Shipper <br />
			<input type="button" value="명세1" id="K_DescriptionS1" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="명세2" id="K_DescriptionS2" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="비용1" id="K_ChargeS1" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="비용2" id="K_ChargeS2" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="산지1" id="K_Madein1" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" /><br />

			<input type="button" value="明细1" id="C_DescriptionS1" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="明细2" id="C_DescriptionS2" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="费用1" id="C_ChargeS1" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="费用2" id="C_ChargeS2" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="原产地1" id="C_Madein1" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" /><br />

			SMS Consignee <br />
			<input type="button" value="명세1" id="K_DescriptionC1" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="명세2" id="K_DescriptionC2" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="비용1" id="K_ChargeC1" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="비용2" id="K_ChargeC2" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" /><br />

			<input type="button" value="明细1" id="C_DescriptionC1" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="明细2" id="C_DescriptionC2" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="费用1" id="C_ChargeC1" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" />
			<input type="button" value="费用2" id="C_ChargeC2" onclick="SMSDefaultLoad(this.id)" style="font-size:10px;" /><br />

			<input type="checkbox" id="Chk_Notice1" onclick="AddNotice(this.id)" />Add Notice1
			<input type="checkbox" id="Chk_Notice2" onclick="AddNotice(this.id)" />Add Notice2

			<!--
			SMS
			
			<input type="button" value="中1" id="C1" onclick="SMSDefaultLoad(this.id)" />
			<input type="button" value="中2" id="C2" onclick="SMSDefaultLoad(this.id)" />
			<input type="button" value="韓1" id="K1" onclick="SMSDefaultLoad(this.id)" />
			<input type="button" value="韓2" id="ChargeLoad" onclick="   SMSDefaultLoad(this.id)" />
			<input type="button" value="韓3" id="K3" onclick="   SMSDefaultLoad(this.id)" />
			<input type="button" value="韓4" id="ChargeLoad_Carryover" onclick="SMSDefaultLoad(this.id)" /><br />
			-->
			
			<textarea id="TBSMS" cols="50" rows="7"></textarea>
			<br />
			<br />
			E-mail<br />
			<textarea id="TBMSG" cols="50" rows="5"></textarea>
			<div id="PnShipper" style="padding: 10px;"></div>
			<div id="PnConsignee" style="padding: 10px;"></div>
			<p style="text-align: center; padding: 10px;">
				<input type="button" value="확인" style="width: 100px; height: 50px;" onclick="BTNSubmitClick();" />
			</p>
			<input type="hidden" id="dhksfy" value="<%=GetGlobalResourceObject("qjsdur", "dhksfy") %>" />
			<input type="hidden" id="K1Check" />
			
		</div>
	</form>
</body>
</html>
