<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Clean.Master" AutoEventWireup="true" CodeFile="ChargeView.aspx.cs" Inherits="Charge_Dialog_ChargeView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" runat="Server">
	<input type="hidden" id="IsEstimation" value="<%=IsEstimation %>" />

	<input type="button" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "dlstho") %>" onclick="RunPrint()" />
	<%=OnlyAdmin %>
	<div id="PrintArea">
		<%=Stamp %>
		<div style="width: 694px; background-color: White;">
			<%=Title %>
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





</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" runat="Server">
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
			var data = {
				FromEmail: "korea@nn21.com",
				FromName: "국제물류 아이엘",
				ToEmail: $("#TBEmail").val(),
				ToName: "",
				Title: "국제물류 아이엘 || 요청하신 정산서 보내드립니다. ",
				Body: $("#PrintArea").html()
			}
			$.ajax({
				type: "POST",
				url: Url,
				data: JSON.stringify(Data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					alert("OK");
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});
		}
		function PnNotCalculatedTariff_hide() {
			$("#PnNotCalculatedTariff").html("");
		}

	</script>
</asp:Content>

