<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FreightChargeView.aspx.cs" Debug="true" Inherits="Request_FreightChargeView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
		.OurLogo {
			border-bottom: solid 2px black;
			width: 692px;
			height: 75px;
			text-align: center;
			padding-top: 3px;
		}
		.Shipper {
			border: solid 1px black;
			height: 90px;
			margin-top: -1px;
			padding: 5px;
		}
		.PortOfLoading {
			border: solid 1px black;
			height: 51px;
			margin-top: -1px;
			padding: 5px;
		}
		.FinalDestination {
			border: solid 1px black;
			height: 51px;
			margin-top: -1px;
			margin-left: -1px;
			padding: 5px;
		}
		.InvoiceNo {
			border: solid 1px black;
			margin-left: -1px;
			padding: 5px;
		}
		.BL {
			border-left: solid 1px black;
		}
		.BB {
			border-bottom: solid 1px black;
		}
		.BR {
			border-right: solid 1px black;
		}
	</style>
	<script src="/Common/jquery-1.10.2.min.js"></script>
	<script src="/Common/printThis.js"></script>
	<script type="text/javascript">
		var initBody;

		function RunPrint() {
			$("#PrintArea").printThis();
		}

		function ShowEmail() {
			document.getElementById("PnEmail").innerHTML = "";
			document.getElementById("TBEmail").focus();
		}
		function SendEmail() {
			//alert(document.getElementById("TBEmail").value);
			Admin.EmailSend("korea@nn21.com", "국제물류 아이엘", document.getElementById("TBEmail").value, "", "국제물류 아이엘 || 요청하신 정산서 보내드립니다. ", PrintArea.innerHTML, function (result) {
				if (result == "1") {
					alert("OK");
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function PnNotCalculatedTariff_hide() {
			document.getElementById("PnNotCalculatedTariff").innerHTML = "";
		}
	</script>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Admin.asmx" />
			</Services>
		</asp:ScriptManager>
		<input type="button" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "dlstho") %>" onclick="RunPrint()" />
		<% if (MEMBERINFO[2] == "ilic66") {%>
		<input type="button" onclick="PnNotCalculatedTariff_hide();" value="관부가세 책정중 감추기" />
		<%} %>
		<br />
		<%=OnlyAdmin %>
		<div id="PrintArea">
			<% if (Request.Params["Stamp"] == "Yes") { %>
			<div style="position: absolute; width: 100px; height: 100px; margin-left: 590px; margin-top: 200px;">
				<img src="/Images/Check_Back.png" style="width: 75px; height: 225px;" />
			</div>
			<div style="position: absolute; width: 100px; height: 100px; margin-left: 590px; margin-top: 350px;">
				<img src="/Images/Check_Stamp.png" style="width: 73px; height: 74px;" />
			</div>
			<% } %>

			<div style="width: 694px; background-color: White;">
				<%=Title %>
				<div style="padding: 10px; width: 150px; height: 20px; text-align: center; margin: 0 auto; border: 2px solid black; margin-top: 20px; margin-bottom: 20px; font-size: 17px; letter-spacing: 3px;">
					<%if (IsEstimation) {
							Response.Write("견적 (报价)");
						}
						else {
							Response.Write("INVOICE");
						} %>
				</div>
				<%=Header %>
				<br />
				<%=Item %>
				<%=Freight %>
				<%=BankInfo %>
				<table border="0" style="width: 694px; margin-top: 10px; margin-left: 5px;">
					<tr>
						<td align="left"><strong>◈ China Office 中國事務室</strong> : 热线(代表) : 400 708 1600</td>
					</tr>
					<tr>
						<td height="5">&nbsp;</td>
					</tr>
					<tr>
						<td align="left"><strong>◆ 화남지역 [华南地区]</strong> : (广州/ 中山/ 东莞/ 弗山/ 厦门)</td>
						<td align="left"><strong>◆ 화동지역 [华东地区]</strong> : (上海/ 温州/ 义乌/ 杭州/ 苏州)</td>
					</tr>
					<tr>
						<td align="left" style="padding-left: 140px;">E-Mail : ilgz@nn21.com</td>
						<td align="left" style="padding-left: 140px;">E-Mail : ilzz@nn21.com</td>
					</tr>
					<tr>
						<td align="left" style="padding-left: 140px;">H.P : 138 2621 4498</td>
						<td align="left" style="padding-left: 140px;">H.P : 136 5650 8768</td>
					</tr>
					<tr>
						<td height="5">&nbsp;</td>
					</tr>
					<tr>
						<td align="left"><strong>◆ 화북지역 [华北地区]</strong> : (北京/ 天津/ 烟台/ 青岛/ 威海)</td>
						<td align="left"><strong>◆ 동북지역 [东北地区]</strong> : (沈阳/ 大连/ 丹东/ 吉林/ 长春)</td>
					</tr>
					<tr>
						<td align="left" style="padding-left: 140px;">E-Mail : ilsd@nn21.com</td>
						<td align="left" style="padding-left: 140px;">E-Mail : ildb@nn21.com</td>
					</tr>
					<tr>
						<td align="left" style="padding-left: 140px;">H.P : 186 0631 9989</td>
						<td align="left" style="padding-left: 140px;">H.P : 138 0403 9968</td>
					</tr>
				</table>
			</div>
		</div>
	</form>
</body>
</html>
