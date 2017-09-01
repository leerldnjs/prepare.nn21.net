<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="ExchangeRate.aspx.cs" Inherits="Admin_ExchangeRate" %>
<%@ Register src="LogedWithoutRecentRequest.ascx" tagname="LogedWithoutRecentRequest" tagprefix="uc1" %>
<%@ Register src="../CustomClearance/Loged.ascx" tagname="Loged" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script src="../Common/jquery-1.10.2.min.js"></script>
	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
	<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>

	<script type="text/javascript">
		window.onload = function () {
			if (form1.HFrom.value == "") {
				form1.HFrom.value = "0";
				form1.St_MonetaryUnitFrom.value = "0"
			}
			else {
				form1.St_MonetaryUnitFrom.value = form1.HFrom.value;
			}
			if (form1.HTo.value == "") {
				form1.HTo.value = "0";
				form1.St_MonetaryUnitTo.value = "0";
			}
			else {
				form1.St_MonetaryUnitTo.value = form1.HTo.value;
			}
			$("#TB_DateStart").datepicker();
			$("#TB_DateStart").datepicker("option", "dateFormat", "yymmdd");

			$("#TB_DateEnd").datepicker();
			$("#TB_DateEnd").datepicker("option", "dateFormat", "yymmdd");
			$("#TB_DateEnd").val($("#H_Today").val());
			$("#TB_DateStart").val($("#H_Today").val());
		}
		function ExchangeRateInsert() {
			var datespan = form1.TB_DateStart.value;
			if (form1.TB_DateEnd.value != "" && datespan!=form1.TB_DateEnd.value ) {
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
			Admin.InsertExchangeRate(datespan, etcsettingpk, form1.ExchangeRateStandard.value, monetaryunitfrom, monetaryunitto, exchangerate, function (result) { alert("Ok"); location.reload(); }, function (result) { alert("ERROR"); });
		}
		function ExchangeRateModify(ExchageRatePk) {
			var datespan = form1.TB_DateStart.value;
			if (form1.TB_DateEnd.value != "" && datespan != form1.TB_DateEnd.value) {
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
			Admin.ExchangeRateModify(ExchageRatePk, datespan, etcsettingpk, form1.ExchangeRateStandard.value, monetaryunitfrom, monetaryunitto, exchangerate, function (result) {
				alert("OK");
				location.reload();
			}, function (result) { alert("ERROR : " + result); });
		}
		function SelectTitleNMonetaryUnit() {
			location.href = "./ExchangeRate.aspx?G=V&S=" + form1.St_ExchangeRateTitle.value + "&F=" + form1.St_MonetaryUnitFrom.value + "&T=" + form1.St_MonetaryUnitTo.value;
		}
		function BTN_Delete(ExchangeRatePk) {
			Admin.DeleteExchangeRate(ExchangeRatePk, function (result) { alert(form1.dhksfy.value); location.reload(true); }, function (result) { alert("ERROR : " + result); });
		}
		function BTN_Modify(ExchangeRatePk) {
			Admin.ExchangeRateSetModify(ExchangeRatePk, function (result) {
				form1.TB_DateStart.value = result[0].substr(0, result[0].indexOf("~"));
				form1.TB_DateEnd.value = result[0].substr(result[0].indexOf("~") + 1);
				document.getElementById("St_ExchangeRateTitle").value = result[1];
				form1.ExchangeRateStandard.value = result[2];
				form1.St_MonetaryUnitFrom.value = result[3];
				form1.St_MonetaryUnitTo.value = result[4];
				form1.TB_ExchangeRateValue.value = result[5];
				document.getElementById("PnBTN_Sumit").innerHTML = "<input type=\"button\" value=\"Modify\" onclick=\"ExchangeRateModify('"+ExchangeRatePk+"');\" />";
			}, function (result) { alert("ERROR : " + result); });
		}
	</script>
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px;" >
    <form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
	    <uc1:LogedWithoutRecentRequest ID="Loged1" runat="server" />
        <uc2:Loged ID="Loged2" runat="server" Visible="false" />
			<div style="background-color:White; width:850px; height:100%; padding:25px; ">

			<%
				if (Session["Type"] + "" != "ShippingBranch") {

				 %>


			<div><%=St_ExchangeRateTitle %></div>
			<div>
				<select id="St_MonetaryUnitFrom" size="1" onchange="SelectTitleNMonetaryUnit();">
					<option value="0">From</option>
					<option value="18">RMB ￥</option>
					<option value="19">USD $</option>
					<option value="20">KRW ￦</option>
					<option value="21">JPY Y</option>
					<option value="22">HKD HK$</option>
					<option value="23">EUR €</option>
				</select>
				&nbsp;&nbsp;&nbsp;
				<select id="St_MonetaryUnitTo" size="1" onchange="SelectTitleNMonetaryUnit();">
					<option value="0">To</option>
					<option value="18">RMB ￥</option>
					<option value="19">USD $</option>
					<option value="20">KRW ￦</option>
					<option value="21">JPY Y</option>
					<option value="22">HKD HK$</option>
					<option value="23">EUR €</option>
				</select>
			</div>
			<div>
				<input type="hidden" id="H_Today" value="<%=TODAY %>" />
				<input id="TB_DateStart" type="text" readonly="readonly" style="cursor:hand; width:75px; text-align:center;" />&nbsp;~&nbsp;<input id="TB_DateEnd" type="text" readonly="readonly" style="cursor:hand; width:75px; text-align:center;" />
			</div>
			<div>
				<input type="text" id="MonetaryUnitFrom" readonly="readonly" style="width:27px; border:0px;" />
				<input type="text" id="ExchangeRateStandard" value="1" style="width:40px; text-align:center;" /> = 
				<input type="text" id="MonetaryUnitTo" readonly="readonly" style="width:27px; border:0px;" /> 
				<input type="text" id="TB_ExchangeRateValue" style="width:89px;" />
				<span id="PnBTN_Sumit"><input type="button" value="Upload" onclick="ExchangeRateInsert();" /></span>
			</div>
			<% } %>
			<table border="0" cellpadding="0" cellspacing="0" style="width:500px;">
				<thead>
					<tr>
						<td colspan="5"  style="text-align:center; padding:10px; ">
							<%=Paging %>
						</td>
					</tr>
					<tr style="height:30px;">
						<td class="THead1" >기간</td>
						<td class="THead1" style="width:60px;" >기준단위</td>
						<td class="THead1" style="width:130px;" >환율</td>
						<td class="THead1" style="width:50px;" >Modify</td>
						<td class="THead1" style="width:50px;" >Delete</td>
					</tr>
				</thead>
				<tbody><%=ExchangeRateListTableBody %></tbody>
			</table>
		</div>
		<input type="hidden" id="HCompanyCode" value="<%=Session["SubInfo"].ToString().Split(new char[]{'!'}, StringSplitOptions.None)[0] %>" />
		<input type="hidden" id="HTitle" value="<%=Request.Params["S"] %>" />
		<input type="hidden" id="HFrom" value="<%=Request.Params["F"] %>" />
		<input type="hidden" id="HTo" value="<%=Request.Params["T"] %>" />
		<input type="hidden" id="HGubun" value="<%=Request.Params["G"] %>" />
		<input type="hidden" id="dhksfy" value="<%=GetGlobalResourceObject("qjsdur", "dhksfy") %>" />
    </form>
</body>
</html>