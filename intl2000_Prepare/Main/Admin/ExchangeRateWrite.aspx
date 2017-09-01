<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExchangeRateWrite.aspx.cs" Inherits="Admin_ExchangeRateWrite" %>
<%@ Register src="Loged.ascx" tagname="Loged" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />

	<script type="text/javascript">
		function OpenCalendar(obj, width, height) {
			height = height + 20;
			var rand = Math.random() * 4;
			window.showModalDialog('../Common/calendar.html?rand=' + rand, obj, 'dialogWidth=' + width + 'px;dialogHeight=' + height + 'px;resizable=0;status=0;scroll=0;help=0');
		}
		function ExchangeRateInsert() {
			var datespan = form1.TB_DateStart.value;
			if (form1.TB_DateEnd.value != "") {
				datespan += "~" + form1.TB_DateEnd.value;
			}

			var etcsettingpk = form1.St_ExchangeRateTitle.value;
			if (etcsettingpk == "") {
				alert("Please select exchange's type");
				return false;
			}
			var monetaryunitfrom = form1.St_MonetaryUnitFrom.value;
			var monetaryunitto = form1.St_MonetaryUnitTo.value;
			if (monetaryunitfrom == monetaryunitto) {
				alert("two monetary unit can't be equal");
				return false;
			}
			var exchangerate = form1.TB_ExchangeRateValue.value;
			if (exchangerate == "") {
				alert("please set exchange rate");
				return false;
			}
			Admin.InsertExchangeRate(datespan, etcsettingpk, form1.ExchangeRateStandard.value, monetaryunitfrom, monetaryunitto, exchangerate, function (result) { alert(result); }, function (result) { alert("ERROR"); });
		}
	</script>

</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px;" >
    <form id="form1" runat="server">
	    <uc1:Loged ID="Loged1" runat="server" />
		<div class="Contents">
			<div><%=St_ExchangeRateTitle %></div>
			<div>
				<input id="TB_DateStart" type="text" readonly="readonly" style="cursor:hand; width:75px; text-align:center;" onclick="OpenCalendar(TB_DateStart, 240, 220);" />&nbsp;~&nbsp;<input id="TB_DateEnd" type="text" readonly="readonly" style="cursor:hand; width:75px; text-align:center;" onclick="OpenCalendar(TB_DateEnd, 240, 220);" />
			</div>
			<div>
				<input type="text" id="MonetaryUnitFrom" readonly="readonly" style="width:27px; border:0px;" />
				<input type="text" id="ExchangeRateStandard" value="1" style="width:40px; text-align:center;" /> = 
				<input type="text" id="MonetaryUnitTo" readonly="readonly" style="width:27px; border:0px;" /> <input type="text" id="TB_ExchangeRateValue" style="width:89px;" />
				<input type="button" id="BTN_ExchangeRateInsert" value="Upload" onclick="ExchangeRateInsert();" />
			</div>
			<p>&nbsp;</p>
		</div>
		<input type="hidden" id="HCompanyCode" value="<%=Session["SubInfo"].ToString().Split(new char[]{'!'}, StringSplitOptions.None)[0] %>" />
		<input type="hidden" id="HTitle" value="<%=Request.Params["S"] %>" />
		<input type="hidden" id="HFrom" value="<%=Request.Params["F"] %>" />
		<input type="hidden" id="HTo" value="<%=Request.Params["T"] %>" />
	</form>
</body>
</html>
