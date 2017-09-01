<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CollectPayment.aspx.cs" Inherits="Admin_Dialog_CollectPayment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script src="../../Common/jquery-1.10.2.min.js"></script>
	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
	<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>

	<script type="text/javascript">
		var ACCOUNTID;
		var SORC;
		var COMPANYPK;

		window.onload = function () {
			REQUESTFORMPK = dialogArguments[0];
			ACCOUNTID = dialogArguments[1];
			SORC = dialogArguments[2];
			COMPANYPK = dialogArguments[3];
			Admin.LoadCollectPayment(SORC, REQUESTFORMPK, COMPANYPK, function (result) {
				var SelectedBankAccount;
				var PaymentHTML = "";
				var DefaultM = "";
				var IsConfirmed = false;
				if (result[0] == "N") { alert("운임확정이 안됐습니다"); return false; }
				for (var i = 0; i < result.length; i++) {
					var EachRow = result[i].split("%!$@#");
					switch (i) {
						case 0:
							var eachline1 = EachRow[0].split("#@!");
								SelectedBankAccount = eachline1[1];
								LoadBankAccount();

							if (EachRow[1] != "N") { PaymentHTML += "<p>물류비 : " + EachRow[1] + "</p>"; }
							if (EachRow[2] != "N") { PaymentHTML += "<p>통관비 : " + EachRow[2] + "</p>"; }
							if (EachRow[3] != "N") { PaymentHTML += "<p>배달비 : " + EachRow[3] + "</p>"; }
							//							if (EachRow[4] != "N") { PaymentHTML += "<p>미수/과입 : " + EachRow[4] + "</p>"; }
							if (PaymentHTML != "") {
								PaymentHTML += "----------------------------<p>계 : " + EachRow[4] + "</p>----------------------------";
								
								DefaultM = EachRow[4].substr(0, 1);
								if (DefaultM == "H") {
									DefaultM = EachRow[4].substr(0, 3);
								}

								if (!IsConfirmed) {
									document.getElementById("SP_MonetaryUnit").innerHTML = EachRow[4].substr(0, 1);
								}
								var monetaryUnitCL = "";
								switch (EachRow[4].substr(0, 1)) {
									case "￥": monetaryUnitCL = "18"; break;
									case "$": monetaryUnitCL = "19"; break;
									case "￦": monetaryUnitCL = "20"; break;
									case "Y": monetaryUnitCL = "21"; break;
									case "H": monetaryUnitCL = "22"; break;
									case "€": monetaryUnitCL = "23"; break;
								}
								form1.HMonetaryUnit.value = monetaryUnitCL;
								form1.HTotalAmount.value = EachRow[4].substr(2).replace(/,/g, "");
							}
							break;
						case 1:
							for (var j = 0; j < EachRow.length; j++) {
								var each = EachRow[j].split("#@!");
								if (each[0] != "aleady") {
									PaymentHTML += "<p>" + each[4] + each[1] + " <strong>" + each[2] + "</strong>&nbsp" + each[5] + "</p>";
								}
								else {
									form1.HAleadyDeposited.value = each[1];
								}
							}
							break;
						case 2:
							var temptitle = "잔액";
							if (result[2].substr(0, 1) == "-") {
								temptitle = "과입";
							}
							PaymentHTML += "<p>" + temptitle + " : " + DefaultM + " " + result[2] + "</p>";
							form1.HLastLeft.value = result[2];
							try { form1.TB_DepositedValue.value = result[2]; } catch (e) { }
							break;
					}
					document.getElementById("PaymentHTML").innerHTML = PaymentHTML;
				}
				alert("Load complete");

				if (!IsConfirmed) {
					for (var j = 0; j < form1.st_Deposited.options.length; j++) {
						if (form1.st_Deposited.options[j].value == SelectedBankAccount) {
							form1.st_Deposited.selectedIndex = j;
							break;
						}
					}
				}
			}, function (result) { alert("ERROR : " + result); });
			var now = new Date();
			var min = parseInt(now.getMinutes().toString());
			if (min < 10) {
				var minute = "0" + min;
			}
			else {
				var minute = min;
			}

			$("#TB_DepositedD").datepicker();
			$("#TB_DepositedD").datepicker("option", "dateFormat", "yymmdd");
			$("#TB_DepositedD").val(now.format("yyyyMMdd"));

			form1.TB_DepositedH.value = now.getHours();
			form1.TB_DepositedM.value = minute;
			form1.TB_DepositedValue.focus();
		}
		function LoadBankAccount() {
			form1.st_Deposited.options[0] = new Option('기타', '0');
			Admin.LoadOurStaffSBankAccount(REQUESTFORMPK, SORC, function (result) {
				for (var i = 0; i < result.length; i++) {
					var Each = result[i].split("!");
					form1.st_Deposited.options[i + 1] = new Option(Each[4] + " " + Each[1] + " " + Each[3] + " " + Each[2], Each[0]);
				}
			}, function (result) { alert(result); });
		}
		function DepositedOK() {
			document.getElementById("BTN_OK").style.display = "none";
			if (form1.TB_DepositedValue.value == "") { alert("Have no Value"); }
			var GubunCL = "0"; if (SORC == "C") { GubunCL = "1"; }
			var DepositedDate = "";
			if (form1.TB_DepositedD.value == "") { alert("입금일은 필수입니다."); return false; }
			else { DepositedDate = form1.TB_DepositedD.value; }
			if (form1.TB_DepositedH.value != "") {
				if (form1.TB_DepositedM.value == "") { DepositedDate = DepositedDate + form1.TB_DepositedH.value; }
				else { DepositedDate = DepositedDate + form1.TB_DepositedH.value + form1.TB_DepositedM.value; }
			}
			Admin.DepositedOK(REQUESTFORMPK, GubunCL, COMPANYPK, form1.st_Deposited.value, form1.HMonetaryUnit.value, form1.TB_DepositedValue.value, DepositedDate, ACCOUNTID, form1.TB_Comment.value, function (result) {
				if (result == "1") {
					DepositedConfirm();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function DepositedConfirm() {
			if (confirm("입금이 완료되었습니까? 재고에 수금이 완료된것으로 표시됩니다.")) {
				alert("123");
				var GubunCL = "0"; if (SORC == "C") { GubunCL = "1"; }
				Admin.DepositedConfirm(GubunCL, ACCOUNTID, REQUESTFORMPK, form1.HMonetaryUnit.value + "#@!" + form1.HLastLeft.value, COMPANYPK, function (result) {
					if (result == "1") {
						alert("SUCCESS");
						this.close();
					}
					else {
						alert(result);
					}
				}, function (result) { alert("ERROR : " + result); });
			} 

		}
		function DepositedCancel() {
			var GubunCL = "0"; if (SORC == "C") { GubunCL = "1"; }
			Admin.DepositedCancel(GubunCL, REQUESTFORMPK, function (result) {
				if (result == "1") {
					alert("SUCCESS");
					this.close();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function openModal(obj, file_name, width, height) {//날짜 입력 폼 띄우기
			height = height + 20;
			var rand = Math.random() * 4;
			window.showModalDialog('../../Common/' + file_name + '?rand=' + rand, obj, 'dialogWidth=' + width + 'px;dialogHeight=' + height + 'px;resizable=0;status=0;scroll=0;help=0');
		}
		function moveNext(from, to, length) { if (from.value.length == length) { to.focus(); } }
	</script>
</head>
<body>
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
    <div id="PaymentHTML"></div>
	<div id="HistoryHTML"></div>
	<div id="DepositedHTML" >
		<select id="st_Deposited"></select><br />
		<input id="TB_DepositedD" readonly="readonly" type="text" size="8" />&nbsp;
		<input type="text" id="TB_DepositedH" style="width:20px;" maxlength="2" onkeyup="moveNext(this,form1.TB_DepositedM,2);" /> : <input type="text" id="TB_DepositedM" style="width:20px;" maxlength="2" />
		<span id="SP_MonetaryUnit"></span><input type="text" id="TB_DepositedValue" style="width:200px;" /><input type="button" id="BTN_OK" value="입금" onclick="DepositedOK();" /> 
		<textarea rows="5" cols="50" id="TB_Comment"></textarea>
		<input type="button" value="완료" onclick="DepositedConfirm();" />
	</div>
	<input type="hidden" id="HTotalAmount" />
	<input type="hidden" id="HMonetaryUnit" />
	<input type="hidden" id="HAleadyDeposited" value="0" />
	<input type="hidden" id="HLastLeft" />
    </form>
</body>
</html>