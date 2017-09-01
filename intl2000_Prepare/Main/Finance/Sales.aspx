<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sales.aspx.cs" Inherits="Finance_Sales" %>
<%@ Register src="../Admin/LogedWithoutRecentRequest11.ascx" tagname="Loged" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finance :: Sales</title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script src="../Common/jquery-1.10.2.min.js"></script>
	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
	<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>

	<script type="text/javascript">
		function SerchThis() {
			location.href = "./Sales.aspx?DB=" + form1.DepartureBranch.value + "&AB=" + form1.ArrivalBranch.value + "&SD=" + form1.TB_DepartureDate.value + "&ED=" + form1.TB_ArrivalDate.value;
		}
		window.onload = function () {
			form1.DepartureBranch.value = form1.HDepartureBranch.value;
			form1.ArrivalBranch.value = form1.HArrivalBranch.value;
			$("#TB_DepartureDate").datepicker();
			$("#TB_DepartureDate").datepicker("option", "dateFormat", "yymmdd");
			$("#TB_DepartureDate").val($("#HTB_DepartureDate").val());
			$("#TB_ArrivalDate").datepicker();
			$("#TB_ArrivalDate").datepicker("option", "dateFormat", "yymmdd");
			$("#TB_ArrivalDate").val($("#HTB_ArrivalDate").val());

		}
	</script>
</head>
<body style="background-color:#E4E4E4; width:1100px; margin:0 auto; padding-top:10px;" >
    <form id="form1" runat="server">
	<uc1:Loged ID="Loged1" runat="server" />
    	<div style="background-color:White; height:100%; padding:25px;">
		<input type="hidden" id="HDepartureBranch" value="<%=DepartureBranch %>" />
		<input type="hidden" id="HArrivalBranch" value="<%=ArrivalBranch %>" />
		<div>
			<p><a href="Sales2.aspx">들어온 선박명 출력작업</a></p>
			<p><a href="Sales3.aspx">이우-인천</a></p>
			출발지사 : 
			<select id="DepartureBranch">
				<option value="">ALL</option>
				<option value="2886">烟台国际物流</option>
				<option value="2887" >瀋陽国际物流</option>
				<option value="2888">义乌国际物流</option>
				<option value="3095">SUNCOLOR SHIPPING CO.,LTD</option>
				<option value="3157">INTERNATIONAL LOGISTICS CO.,LTD</option>
				<option value="3388">青岛国际物流</option>
				<option value="3798">OtherLocation</option>
				<option value="3843">广州爱尔国际物流</option>
				<option value="11456">杭州爱尔国际物流</option>
                <option value="7898">衢州国际物流</option>
			</select>
		</div>
		<div>
			도착지사 : 
			<select id="ArrivalBranch">
				<option value="">ALL</option>
				<option value="2886">烟台国际物流</option>
				<option value="2887" >瀋陽国际物流</option>
				<option value="2888">义乌国际物流</option>
				<option value="3095">SUNCOLOR SHIPPING CO.,LTD</option>
				<option value="3157">INTERNATIONAL LOGISTICS CO.,LTD</option>
				<option value="3388">青岛国际物流</option>
				<option value="3798">OtherLocation</option>
				<option value="3843">广州爱尔国际物流</option>
				<option value="11456">杭州爱尔国际物流</option>
                <option value="7898">衢州国际物流</option>
			</select>
		</div>
			<input type="hidden" id="HTB_DepartureDate" value="<%=StartDate %>" />
			<input type="hidden" id="HTB_ArrivalDate" value="<%=EndDate %>" />
		
		<input id="TB_DepartureDate" size="10" type="text" style="text-align:center;" readonly="readonly" /> ~ 
		<input id="TB_ArrivalDate" size="10" type="text" style="text-align:center;" readonly="readonly" />
		<input type="button" onclick="SerchThis();" value="SERCH" />
		<div id="ResultHTML">
			<%=HTML %>
		</div>
	</div>
    </form>
</body>
</html>
