<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SettlementWithStorage_Multiple.aspx.cs" Inherits="Admin_SettlementWithStorage_Multiple" %>
<%@ Register src="../Admin/LogedWithoutRecentRequest11.ascx" tagname="LogedWithoutRecentRequest11" tagprefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />    
	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.1/themes/base/jquery-ui.css" />
	<script src="http://code.jquery.com/jquery-1.9.1.js"></script>
	<script src="http://code.jquery.com/ui/1.10.1/jquery-ui.js"></script>
	<script type="text/javascript">
		jQuery(document).ready(function () {
			$("#Date").datepicker();
			$("#Date").datepicker("option", "dateFormat", "yy/mm/dd");
			if ($("#HDate").val() == "") {
			} else {
				$("#Date").val($("#HDate").val());
			}
			$("#SendDate").datepicker();
			$("#SendDate").datepicker("option", "dateFormat", "yy/mm/dd");
		});
		function PasteTariff() {
			if (confirm("붙여넣기 합니다.")) {
				var clipboard = window.clipboardData.getData('Text');
				clipboard = clipboard.replace(/\n/gi, "%!$@#");
				clipboard = clipboard.replace(/\t/gi, "@#$");
				$("#HClipBoard").val(clipboard);
				form1.submit();
				return false;
			}
		}
		function SetSave() {
			var i = 0;
			var TBBHPk = "";
			var SumPrice = "";
			var Comment = $("#Comment").val();
			var ActDate = $("#ActDate").val();
			while (true) {
				if ($("#TBRow_" + i + "_Price").length > 0) {
					if ($("#TBRow_" + i + "_Price").val() != "" && $("#TBRow_" + i + "_Price").val() != "0") {
						TBBHPk += "#!@" + $("#TBRow_" + i + "_TBBHPk").val();
						SumPrice += "#!@" + $("#TBRow_" + i + "_Price").val();
					}
					i++;
					continue;
				} else {
					break;
				}
			}
			if (TBBHPk != "") {
				SettlementWithCustoms.SetSettlement_StorageMultiple(TBBHPk, SumPrice, Comment, ActDate, function (result) {
					if (result == "1") {
						alert("Success");
						//$("#HClipBoard").val("");

						//location.reload();
					} else {
						alert(result);
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function SetCustomsSend() {
			if ($("#SendDate").val() == "" || $("#SendPrice").val() == "") {
				alert("데이터가 없습니다.");
				return false;
			} else {
				SettlementWithCustoms.SetCustomsSend($("#SendDate").val(), $("#SendPrice").val(), $("#SendDescription").val(), function (result) {
					if (result == "1") {
						alert("Success");
						location.reload();
					} else {
						alert(result);
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
	</script>
</head>
<body style="background-color:#E4E4E4; width:1100px; margin:0 auto; padding-top:10px;" >
	<form id="form1" runat="server">
		<input type="hidden" name="HClipBoard" id="HClipBoard" value="<%=Request.Form["HClipBoard"] %>" />
		<input type="hidden" id="AccountID" value="<%=MemberInfo[2] %>" />
		<asp:ScriptManager ID="SM" runat="server" ><Services><asp:ServiceReference Path="~/WebService/SettlementWithCustoms.asmx" /></Services></asp:ScriptManager>
		<uc1:LogedWithoutRecentRequest11 ID="LogedWithoutRecentRequest111" runat="server" />
		<div style="background-color:White; width:1050px; height:100%; padding:25px;">
			<p>
				<input type="button" value="붙여넣기" onclick="PasteTariff();" />
			</p>
			<%=Html %>
		</div>
	</form>
</body>
</html>