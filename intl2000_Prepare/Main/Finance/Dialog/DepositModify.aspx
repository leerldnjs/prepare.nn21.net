<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DepositModify.aspx.cs" Inherits="Finance_Dialog_DepositModify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script type="text/javascript">
		var BEFOREDATA;

		var ACCOUNTID;
		var SORC;
		var DEPOSITEDPK;
		var REQUESTFORMPK;
		window.onload = function () {
			DEPOSITEDPK = dialogArguments[0];
			REQUESTFORMPK = dialogArguments[1];
			SORC = dialogArguments[2];
			ACCOUNTID = dialogArguments[3];
			LoadBankAccount();
			alert("Modify Load");
			LoadDeposited(DEPOSITEDPK);
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

		function LoadBankAccount() {
			form1.st_Deposited.options[0] = new Option('기타', '0');
			Admin.LoadOurStaffSBankAccount(REQUESTFORMPK, SORC, function (result) {
				for (var i = 0; i < result.length; i++) {
					var Each = result[i].split("!");
					form1.st_Deposited.options[i + 1] = new Option(Each[4] + " " + Each[1] + " " + Each[3] + " " + Each[2], Each[0]);
				}
			}, function (result) { alert(result); });
		}
		function LoadDeposited(DepositedPk) {
			Admin.LoadDepositedData(DepositedPk, function (result) {
				if (result[0] == "N") {
					alert("error");
					return false;
				}
				BEFOREDATA = result;
				//alert(BEFOREDATA[0]);
				document.getElementById("st_Deposited").value = BEFOREDATA[0];
				document.getElementById("SP_MonetaryUnit").innerHTML = BEFOREDATA[1];
				form1.TB_DepositedValue.value = BEFOREDATA[2];
				//alert(DepositedPk);
				form1.TB_DepositedD.value = BEFOREDATA[3].substr(0, 8);
				if (BEFOREDATA[3].length > 8) { form1.TB_DepositedH.value = BEFOREDATA[3].substr(8, 2); }
				if (BEFOREDATA[3].length > 10) { form1.TB_DepositedM.value = BEFOREDATA[3].substr(10, 2); }
				 form1.TB_Comment.value = BEFOREDATA[4];
			}, function (result) { alert("ERROR : " + result); });
		}

		function DepositedModifyOK() {
			var DepositedDate = "";
			if (form1.TB_DepositedD.value == "") { alert("입금일은 필수입니다."); return false; }
			else { DepositedDate = form1.TB_DepositedD.value; }
			if (form1.TB_DepositedH.value != "") {
				if (form1.TB_DepositedM.value == "") { DepositedDate = DepositedDate + form1.TB_DepositedH.value; }
				else { DepositedDate = DepositedDate + form1.TB_DepositedH.value + form1.TB_DepositedM.value; }
			}

			//alert(BEFOREDATA[0]);

			var ChangedValue = new Array();
			if (document.getElementById("st_Deposited").value != BEFOREDATA[0]) {
				ChangedValue[0] = document.getElementById("st_Deposited").value;
			}
			else {
				ChangedValue[0] = "NN";
			}

			if (form1.TB_DepositedValue.value != BEFOREDATA[2]) {
				ChangedValue[1] = form1.TB_DepositedValue.value;
			}
			else {
				ChangedValue[1] = "NN";
			}

			if (DepositedDate != BEFOREDATA[3]) {
				ChangedValue[2] = DepositedDate;
			}
			else {
				ChangedValue[2] = "NN";
			}

			if (form1.TB_Comment.value != BEFOREDATA[4]) {
				ChangedValue[3] = form1.TB_Comment.value;
			}
			else {
				ChangedValue[3] = "NN";
			}
			var gubuncl="0";
			if (SORC=="C")
			{
				gubuncl="1";
			}
			Admin.DepositModifyOK(DEPOSITEDPK, ChangedValue, BEFOREDATA[2], ACCOUNTID, gubuncl, REQUESTFORMPK, function (result) {
				if (result == "0") {
					alert("변경사항이 없습니다.");
				}
				else if (result == "1") {
					alert("SUCCESS");
					this.close();
				}
				else {
					alert("ERROR");
				}

			}, function (result) { alert("ERROR : " + result); });
		}
	</script>
</head>
<body>
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
    <div style="padding:20px; ">
		<select id="st_Deposited"></select><br /><br />
		<input id="TB_DepositedD" type="text" size="8" maxlength="8" />&nbsp;
		<input type="text" id="TB_DepositedH" style="width:20px;" maxlength="2" /> : <input type="text" id="TB_DepositedM" style="width:20px;" maxlength="2" />
		<br /><br />
		<textarea rows="5" cols="50" id="TB_Comment"></textarea><br /><br />
		<span id="SP_MonetaryUnit"></span><input type="text" id="TB_DepositedValue" style="width:100px;" />&nbsp;&nbsp;<input type="button" value="수정" onclick="DepositedModifyOK();" /> 		    
    </div>
    </form>
</body>
</html>
