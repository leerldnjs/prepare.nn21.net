<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SettlementWithCustoms.aspx.cs" Inherits="Admin_Dialog_SettlementWithCustoms" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.1/themes/base/jquery-ui.css" />
	<script src="http://code.jquery.com/jquery-1.9.1.js"></script>
	<script src="http://code.jquery.com/ui/1.10.1/jquery-ui.js"></script>
	<script type="text/javascript">
		jQuery(document).ready(function () {
			$("#Date").datepicker();
			$("#Date").datepicker("option", "dateFormat", "yy/mm/dd");
			$("#Date").val("2014/01/02");
			$("#BTN_Save").on("click", SetSave);
		});
		function SetSave() {
			var BLNo, Date, Price, Description, AccountId;
			BLNo = $("#BLNo").val();
			Date = $("#Date").val();
			Price = $("#Price").val();
			Description = $("#Description").val();
			AccountId = $("#AccountId").val();

			SettlementWithCustoms.SetSettlement(BLNo, Date, Price, Description, AccountId, function (result) {
				if (result == "1") {
					alert("Success");
					this.close();
				} else {
					alert(result);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
	</script>
</head>
<body style="background-color:#999999;">
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/SettlementWithCustoms.asmx" /></Services></asp:ScriptManager>
    <div style=" margin:10px;  padding-left:10px; padding-right:10px;  background-color:white;">
        <div style="line-height:20px; padding:10px;" >
			<p>BLNo : <input type="text" id="BLNo" value="<%=BLNo %>" /></p>
			<p>Date : <input type="text" id="Date" /></p>
			<p>Price : <input type="text" id="Price" /></p>
			<p>Description : <input type="text" id="Description" /><input type="hidden" id="AccountId" value="<%=AccountId %>" /></p>
			<p><input type="button" id="BTN_Save" value="SetSave" /></p>
		</div>
    </div>
    </form>
</body>
</html>