<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SettlementWithCustoms_Date.aspx.cs" Inherits="Admin_SettlementWithCustoms_Date" %>
<%@ Register src="LogedWithoutRecentRequest11.ascx" tagname="LogedWithoutRecentRequest11" tagprefix="uc1" %>
<%@ Register src="../CustomClearance/Loged.ascx" tagname="Loged" tagprefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
	<script>
		function View(ListPk) {
			location.href = "../Admin/SettlementWithCustoms_Date.aspx?Date=" + ListPk + "&Range=" + document.getElementById("Range").value;
		}
		function Goto(gubun, value) {
			switch (gubun) {
				case "Storage": location.href = "./PrepareDelivery.aspx?S=" + value; break;
				case "TBBPk": location.href = "./TransportBetweenBranchView.aspx?S=" + value; break;
				case "RequestForm": location.href = "./RequestView.aspx?g=c&pk=" + value; break;
				case "Company": location.href = "./CompanyInfo.aspx?M=View&S=" + value; break;
				case "CheckDescription": location.href = "./CheckDescription.aspx?S=" + value; break;
			}
		}
		function DelThis(SettlementWithCustomsPk) {
			if (confirm("삭제합니다.")) {
				SettlementWithCustoms.DelSettlement(SettlementWithCustomsPk, function (result) {
					if (result == "1") {
						alert("Success");
						location.reload();
					} else {
						alert(result);
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function DelThis2(SettlementWithCustomsPk) {
			if (confirm("삭제합니다.")) {
				SettlementWithCustoms.DelSettlement2(SettlementWithCustomsPk, function (result) {
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
<body>
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/SettlementWithCustoms.asmx" /></Services></asp:ScriptManager>

	<uc2:Loged ID="Loged1" runat="server" Visible="false" />
	<uc1:LogedWithoutRecentRequest11 ID="LogedWithoutRecentRequest111" runat="server" />
	<input type="hidden" id="AccountID" value="<%=MemberInfo[2] %>" />
	<% if (MemberInfo[0] == "OurBranch") { %>		<p>
			<a href="../Finance/ClearanceList.aspx"><strong>통관내역</strong></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="../Admin/SettlementWithCustoms_Date.aspx" >일별 정산</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="../Admin/SettlementWithCustoms_Container.aspx" >컨테이너별 정산</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="../Admin/SettlementWithCustoms_MultipleAdd.aspx" >엑셀 입력</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="/Finance/ClearanceEndList.aspx" >통관 내역보기</a>
		</p>


	<%} %>
	<div style="padding-top:10px; float:left; ">
		<%=TBRange %><br />
		<%=TBList %></div>
	<div style="padding-top:10px; "><%=TBView %></div>
    </form>
</body>
</html>