<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SettlementWithCustoms_MultipleAdd.aspx.cs" Inherits="Admin_SettlementWithCustoms_MultipleAdd" %>
<%@ Register src="../Admin/LogedWithoutRecentRequest11.ascx" tagname="LogedWithoutRecentRequest11" tagprefix="uc1" %>
<%@ Register src="../CustomClearance/Loged.ascx" tagname="Loged" tagprefix="uc2" %>

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
			var SumBLNo = "";
			var SumPrice = "";
			var SumDescription = "";
			var SumDate = "";
			var SumAccountId = "";
			var Date = $("#Date").val();
			var AccountId = $("#AccountID").val();
			while (true) {
				if ($("#BLNo" + i).length > 0) {
					if ($("#BLNo" + i).val() != "") {
						SumBLNo += "#!@" + $("#BLNo" + i).val();
						SumPrice += "#!@" + $("#Price" + i).val();
						SumDescription += "#!@" + $("#Description" + i).val();
						SumAccountId += "#!@" + AccountId;
						SumDate += "#!@" + Date;
					}
					i++;
					continue;
				} else {
					break;
				}
			}
			if (SumBLNo != "") {
				SettlementWithCustoms.SetSettlement(SumBLNo, SumDate, SumPrice, SumDescription, SumAccountId, function (result) {
					if (result == "1") {
						alert("Success");
						$("#HClipBoard").val("");

						location.reload();
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
		<input type="hidden" id="HDate" name="HDate" value="<%=SelectedDate %>" />
	    <asp:ScriptManager ID="SM" runat="server" ><Services><asp:ServiceReference Path="~/WebService/SettlementWithCustoms.asmx" /></Services></asp:ScriptManager>
	    <uc2:Loged ID="Loged1" runat="server" Visible="false" />
		<uc1:LogedWithoutRecentRequest11 ID="LogedWithoutRecentRequest111" runat="server" />
		<div style="background-color:White; width:1050px; height:100%; padding:25px;">
			<% if (MemberInfo[0] == "OurBranch") { %>		<p>
			<a href="../Finance/ClearanceList.aspx"><strong>통관내역</strong></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="../Admin/SettlementWithCustoms_Date.aspx" >일별 정산</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="../Admin/SettlementWithCustoms_Container.aspx" >컨테이너별 정산</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="../Admin/SettlementWithCustoms_MultipleAdd.aspx" >엑셀 입력</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="/Finance/ClearanceEndList.aspx" >통관 내역보기</a>
		</p>

			<%} %>
			<p>
				<input type="button" value="붙여넣기" onclick="PasteTariff();" />
				<input type="button" value="저장" onclick="SetSave();" />
			</p>
			<p>Date : <input type="text" id="Date" name="Date" /></p>
			<%=Html %>
		</div>
	</form>
	<div style="background-color:White; width:1050px; height:100%; padding:25px;">
		<fieldset>
			<legend><strong>관세사 송금 입력</strong></legend>
			<p>Date : <input type="text" id="SendDate" /></p>
			<p>Price : <input type="text" id="SendPrice" /></p>
			<p>Description : <input type="text" id="SendDescription" /></p>
			<p>
				<input type="button" value="관세사 송금금액 입력" onclick="SetCustomsSend();" />
			</p>
		</fieldset>
	</div>
</body>
</html>