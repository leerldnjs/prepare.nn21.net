<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BankDeposit_inpyung.aspx.cs" Inherits="Finance_BankDeposit_inpyung" %>
<%@ Register src="../Admin/LogedWithoutRecentRequest11.ascx" tagname="Loged" tagprefix="uc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
		<title></title>
		<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	</head>
	<body style="background-color:#E4E4E4; width:1100px; margin:0 auto; padding-top:10px;" >
		<form id="form1" runat="server" method="post">
			<uc1:Loged ID="Loged1" runat="server" />
			<div style="background-color:White; width:1050px; height:100%; padding:25px;">
				<p>
					<a href="/Admin/Taxpaid.aspx">세납</a>&nbsp;&nbsp;|&nbsp;&nbsp;
					<a href="/Finance/BankDeposit_inpyung.aspx"><strong>세금통장</strong></a>
				</p>
				<p>
					<input id="BankDeposit_Date" name="Date" type="text" style="width:95px; text-align:center;" value="<%=Date %>" />
					<input type="submit" value="조회"  />
				</p>
				<p>
					<input id="BankDeposit_Time" type="text" style="width:45px; text-align:center; " value="<%=DateTime.Now.ToString("HH:mm") %>" />
					<input id="BankDeposit_Description" type="text" placeholder="적요" style="width:200px; " />
					<select id="BankDeposit_InOut">
						<option value="+">입금</option>
						<option value="-">출금</option>
					</select>
					<input id="BankDeposit_Monetary" type="text" style="width:15px; text-align:center; border:0px;  " value="￦" />
					<input id="BankDeposit_Price" type="text" style="width:120px; " />
					<input type="button" id="BTN_Save" value="Save" />
				</p>
				<%=ListHtml %>
			</div>
		</form>

		


		<script src="/Lib/Scale/js/jquery.min.js"></script>
		<script type="text/javascript">
			$(document).ready(function () {
				$("#BTN_Save").on("click", SetSave);
			});
			function Delete_BankDeposit(BANK_DEPOSIT_PK) {
				if (confirm("삭제하시겠습니까?")) {
					var data = {
						BankDepositPk: BANK_DEPOSIT_PK
					}

					$.ajax({
						type: "POST",
						url: "/WebService/Finance.asmx/SetDelete_BankDeposit",
						data: JSON.stringify(data),
						dataType: "json",
						contentType: "application/json; charset=utf-8",
						success: function (result) {
							if (result.d == "1") {
								$("#form1").submit();
							}
						},
						error: function (result) {
							alert('failure : ' + result)
						}
					});
				}
			}
			function SetSave() {
				if ($("#BankDeposit_Price").val() == "") {
					alert("금액을 넣어주세요.");
					return false;
				}

				var datetime = $("#BankDeposit_Date").val().trim() + " " + $("#BankDeposit_Time").val().trim();
				var data = {
					BankPk: 1,
					Type: "",
					TypePk: "",
					Description: $("#BankDeposit_Description").val(),
					MonetaryUnit: $("#BankDeposit_Monetary").val(),
					Price: $("#BankDeposit_InOut").val() + $("#BankDeposit_Price").val(),
					DateTime: datetime
				}

				$.ajax({
					type: "POST",
					url: "/WebService/Finance.asmx/SetSave_BankDeposit",
					data: JSON.stringify(data),
					dataType: "json",
					contentType: "application/json; charset=utf-8",
					success: function (result) {
						if (result.d == "1") {
							$("#form1").submit();
						}
					},
					error: function (result) {
						alert('failure : ' + result)
					}
				});
			}
		</script>
	</body>
</html>