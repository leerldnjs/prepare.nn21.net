<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetTariff2.aspx.cs" Inherits="Admin_Dialog_SetTariff" %>
<%@ Register src="LogedWithoutRecentRequest11.ascx" tagname="LogedWithoutRecentRequest11" tagprefix="uc1" %>
<%@ Register src="../CustomClearance/Loged.ascx" tagname="Loged" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
	<script src="http://code.jquery.com/jquery-1.9.0.min.js"></script>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />    
	<script type="text/javascript">
		function LoadBLNo() {
			if (confirm("붙여넣기 합니다.")) {
				var clipboard = window.clipboardData.getData('Text');
				var eachRow = clipboard.replace(/\n/gi, "','");
				eachRow = "'" + eachRow.substr(0, eachRow.length - 2);
				Admin.LoadAleadyTariffByBLNo(eachRow, function (result) {
					alert(result);
					$("#TBList").html(result);
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function PasteTariff() {
			if (confirm("붙여넣기 합니다.")) {
				var clipboard = window.clipboardData.getData('Text');
				var eachRow = clipboard.split("\n");
				var Length = parseInt(document.getElementById("TariffInputCount").value);
				for (var i = 0; i < eachRow.length - 1; i++) {
					var each = eachRow[i].split("\t");
					for (var j = 0; j < Length; j++) {
						if (document.getElementById("TV[" + j + "][BLNo]").value == each[0]) {
							document.getElementById("TV[" + j + "][C]").value = "Y";
							document.getElementById("TV[" + j + "][1]").value = each[1];
							document.getElementById("TV[" + j + "][2]").value = each[2];
							document.getElementById("TV[" + j + "][3]").value = each[3];
						}
					}
				}
			}
		}

		function Goto(gubun, value) {
			switch (gubun) {
				case "CheckDescription": location.href = "./CheckDescription.aspx?S=" + value; break;
				case "RequestForm": location.href = "./RequestView.aspx?g=c&pk=" + value; break;
				case "Company": location.href = "./CompanyInfo.aspx?M=View&S=" + value; break;
				default: alert(value); break;
			}
		}


		function ConfirmTariff(CDPk, RequestFormPk, T1, T2, T3) {
			if (confirm("기존 관부가세가 교체되고 정산완료됩니다.")) {
				Admin.SetConfirmTariff(CDPk, RequestFormPk, T1, T2, T3, function (result) {
					if (result == "1") {
						alert("Success");
						location.reload();
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function CancelTariff(CDPk) {
			if (confirm("관세사 금액을 삭제하고 초기화")) {
				Admin.SetCancelTariff(CDPk, function (result) {
					if (result == "1") {
						alert("Success");
						location.reload();
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function SetSave() {
			var Length = parseInt(document.getElementById("TariffInputCount").value);
			var RPk = new Array();
			var CDPk = new Array();
			var BLNo = new Array();
			var T1 = new Array();
			var T2 = new Array();
			var T3 = new Array();
			var ValueCount = 0;
			for (var i = 0; i < Length; i++) {
				if (document.getElementById("TV[" + i + "][C]").value == "Y") {
					RPk[ValueCount] = document.getElementById("TV[" + i + "][RPk]").value;
					CDPk[ValueCount] = document.getElementById("TV[" + i + "][CDPk]").value;
					BLNo[ValueCount] = document.getElementById("TV[" + i + "][BLNo]").value;
					T1[ValueCount] = document.getElementById("TV[" + i + "][1]").value;
					T2[ValueCount] = document.getElementById("TV[" + i + "][2]").value;
					T3[ValueCount] = document.getElementById("TV[" + i + "][3]").value;
					ValueCount++;
				}
			}
			Admin.SetSaveTariffCheck(CDPk, BLNo, T1, T2, T3, function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server" ><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
    <uc2:Loged ID="Loged1" runat="server" Visible="false" />
    <uc1:LogedWithoutRecentRequest11 ID="LogedWithoutRecentRequest111" runat="server" />
    
	<div style="background-color:White; width:1050px; height:100%; padding:25px;">
		<div style="width:1050px; clear:both; ">
			<%=Header %></div>
		<div style="font-size:1px; clear:both;">&nbsp;</div>
		<p>
			<input type="button" value="Load BLNo" onclick="LoadBLNo();" />
			<input type="button" value="Paste To" onclick="PasteTariff();" />
			<input type="button" value="Set Save" onclick="SetSave();" />
			<input type="text" id="Debug" />
		</p>
		<div id="TBList"><%=TBList %></div>
    </div>
    </form>
</body>
</html>
