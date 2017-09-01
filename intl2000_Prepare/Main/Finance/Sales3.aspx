<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sales3.aspx.cs" Inherits="Finance_Sales3" %>

<%@ Register Src="../Admin/LogedWithoutRecentRequest11.ascx" TagName="Loged" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Finance :: Sales</title>
	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.1/themes/base/jquery-ui.css" />
	<script src="http://code.jquery.com/jquery-1.9.1.js"></script>
	<script src="http://code.jquery.com/ui/1.10.1/jquery-ui.js"></script>

	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		$(function () {
			$("#TB_DepartureDate").datepicker();
			$("#TB_DepartureDate").datepicker("option", "dateFormat", "yymmdd");
			$("#TB_ArrivalDate").datepicker();
			$("#TB_ArrivalDate").datepicker("option", "dateFormat", "yymmdd");
			//form1.DepartureBranch.value = form1.HDepartureBranch.value;
			//form1.ArrivalBranch.value = form1.HArrivalBranch.value;
			form1.TransportWay.value = form1.HTransportWay.value;
			$("#TB_DepartureDate").val(form1.HStartDate.value);
			$("#TB_ArrivalDate").val(form1.HEndDate.value);



			if (form1.HDepartureBranch.value != "") {
				form1.DepartureBranch.value = form1.HDepartureBranch.value;
			}
			if (form1.HArrivalBranch.value != "") {
				form1.ArrivalBranch.value = form1.HArrivalBranch.value;
			}
		});

		function SerchThis() {

			

			location.href = "./Sales3.aspx?DB=" + form1.DepartureBranch.value + "&AB=" + form1.ArrivalBranch.value + "&SD=" + form1.TB_DepartureDate.value + "&ED=" + form1.TB_ArrivalDate.value + "&TW=" + form1.TransportWay.value;
		}
		window.onload = function () {
		}
	</script>
</head>
<body style="background-color: #E4E4E4; width: 1100px; margin: 0 auto; padding-top: 10px;">
	<form id="form1" runat="server">
		<uc1:Loged ID="Loged1" runat="server" />
		<div style="background-color: White; height: 100%; padding: 25px;">
			<input type="hidden" id="HDepartureBranch" value="<%=DepartureBranch %>" />
			<input type="hidden" id="HArrivalBranch" value="<%=ArrivalBranch %>" />
			<input type="hidden" id="HTransportWay" value="<%=TransportWay %>" />
			<input type="hidden" id="HStartDate" value="<%=StartDate %>" />
			<input type="hidden" id="HEndDate" value="<%=EndDate %>" />
			<input type="hidden" id="DepartureBranch" name="DepartureBranch" value="2888" />
			<input type="hidden" id="ArrivalBranch" name="ArrivalBranch" value="3157" />
			<div>
				운송방법 : 
			<select id="TransportWay">
				<option value="">ALL</option>
				<option value="Air">AIR</option>
				<option value="Car">CAR</option>
				<option value="Ship">SHIP</option>
				<option value="Sub">Hand carry</option>
			</select>
			</div>
			<input id="TB_DepartureDate" size="10" type="text" value="<%=StartDate %>" style="text-align: center;"/>
			~ 
		<input id="TB_ArrivalDate" size="10" type="text" value="<%=EndDate %>" style="text-align: center;"/>
			<input type="button" onclick="SerchThis();" value="Search" />
			<div id="ResultHTML">
				<%=HTML %>
			</div>
		</div>
	</form>
</body>
</html>
