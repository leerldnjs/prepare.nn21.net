<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WithClearance.aspx.cs" Inherits="Calculate_WithClearance" %>

<%@ Register Src="~/Member/LogedTopMenu.ascx" TagPrefix="uc1" TagName="LogedTopMenu" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title></title>
	<script src="http://code.jquery.com/jquery-1.9.1.js"></script>
	<script type="text/javascript">
		function Print(Gubun, Value) {
			switch (Gubun) {
				case "DO": location.href = "../CustomClearance/CommercialDocu_DeliveryOrder.aspx?S=" + Value + "&B=" + form1.HBBHPk.value + ""; break;
				case "B": location.href = "../CustomClearance/CommercialDocu_HouseBL.aspx?S=" + Value + ""; break;
				case "B_YT": location.href = "../CustomClearance/CommercialDocu_HouseBL.aspx?S=" + Value + "&YT=YT"; break;
				case "I": location.href = "../CustomClearance/CommercialDocu_Invoice.aspx?S=" + Value + ""; break;
				case "P": location.href = "../CustomClearance/CommercialDocu_PackingList.aspx?S=" + Value + ""; break;
				case "D": window.open('./Dialog/AutoPrint.aspx?G=DeliveryReceipt&S=' + Value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;'); break;
			}
		}
	</script>
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 10px;">
	<form id="form1" runat="server">
		<uc1:LogedTopMenu runat="server" ID="LogedTopMenu" />
		<div class="ContentsTopMenu" >
			<%=TBList %>
		</div	>
	</form>
	<script>
		jQuery(document).ready(function () {
			$(".Nav2014").addClass("active");
		});
	</script>
</body>
</html>
