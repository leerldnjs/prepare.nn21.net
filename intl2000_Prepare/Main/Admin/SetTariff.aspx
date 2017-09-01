<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetTariff.aspx.cs" Inherits="Admin_Dialog_SetTariff" %>
<%@ Register src="LogedWithoutRecentRequest11.ascx" tagname="LogedWithoutRecentRequest11" tagprefix="uc1" %>
<%@ Register src="../CustomClearance/Loged.ascx" tagname="Loged" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />    
    <script src="../Common/jquery-1.4.2.min.js"></script>

	<script type="text/javascript">
		function PasteTariff() {
			if (confirm("붙여넣기 합니다.")) {
				var clipboard = window.clipboardData.getData('Text');
				clipboard=clipboard.replace(/\n/gi, "%!$@#");
				clipboard = clipboard.replace(/\t/gi, "@#$");
				$("#HClipBoard").val(clipboard);
				form1.submit();
				return false;

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

		function SetSave() {
/*
			var Length = parseInt(document.getElementById("TariffInputCount").value);
			var CDPk = new Array();
			var ValueCount = 0;
			for (var i = 0; i < Length; i++) {
			    if (document.getElementById("chk_CDPk[" + i + "]").checked) {
			        CDPk[ValueCount] = document.getElementById("chk_CDPk[" + i + "]").value;
			        ValueCount++;
			    }
			}
			Admin.SetCDStepCL(CDPk, "2", $("#AccountID").val(), function (result) {
			    if (result == "1") {
			        alert("Success");
			        location.reload();
			    } else {
			    	alert(result);
			    }
			}, function (result) { alert("ERROR : " + result); });
			*/
			var Length = parseInt(document.getElementById("TariffInputCount").value);
			var CDPk = new Array();
			var ValueCount = 0;
			for (var i = 0; i < Length; i++) {
				if (document.getElementById("chk_CDPk[" + i + "]").checked) {
					CDPk[ValueCount] = document.getElementById("chk_CDPk[" + i + "]").value;
					ValueCount++;
				} 
			}
			Admin.ChargeTariff(CDPk, function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				} else {
					$("#Debug").val(result);
					alert(result);
				}
			}, function (result) { alert("ERROR : " + result); });
		}

		function TariffChange(CDHPk, T1, T2, T3) {
			Admin.TariffModify(CDHPk, T1, T2, T3, function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				} else {
					alert(result);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
	</script>
</head>
<body>
    <form name="form1" id="form1" method="post" runat="server">
        <input type="hidden" name="HClipBoard" id="HClipBoard" value="<%=Request.Form["HClipBoard"] %>" />
		<input type="hidden" id="AccountID" value="<%=MemberInfo[2] %>" />
    <asp:ScriptManager ID="SM" runat="server" ><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
    <uc2:Loged ID="Loged1" runat="server" Visible="false" />
    <uc1:LogedWithoutRecentRequest11 ID="LogedWithoutRecentRequest111" runat="server" />
    
	<div style="background-color:White; width:1050px; height:100%; padding:25px;">
		<input type="hidden" id="Debug" />
		<div style="width:1050px; clear:both; ">
			<%=Header %></div>
		<div style="font-size:1px; clear:both;">&nbsp;</div>
		<p>
			<input type="button" value="붙여넣기" onclick="PasteTariff();" />
			<input type="button" value="세금 송금" onclick="SetSave();" />
		</p>
		<div><%=TBList %></div>
    </div>
    </form>
</body>
</html>
