<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeliveryCharge.aspx.cs" Inherits="Finance_DeliveryCharge" Debug="true" %>
<%@ Register src="../Admin/LogedWithoutRecentRequest11.ascx" tagname="Loged" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Common/jquery-1.10.2.min.js"></script>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />

	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
	<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
	<script type="text/javascript">
		window.onload = function () {
			form1.St_Gubun.value = form1.Gubun.value;
		    //document.getElementById("St_Month").value = form1.Month.value;
			var TB_DepartureDate = document.getElementById("TB_DepartureDate").value;
			var TB_ArrivalDate = document.getElementById("TB_ArrivalDate").value;
		}
		$(document).ready(function () {
		    $("#TB_DepartureDate").datepicker();
		    $("#TB_DepartureDate").datepicker("option", "dateFormat", "yymmdd");
		    $("#TB_DepartureDate").val($("#HTB_DepartureDate").val());
		    $("#TB_ArrivalDate").datepicker();
		    $("#TB_ArrivalDate").datepicker("option", "dateFormat", "yymmdd");
		    $("#TB_ArrivalDate").val($("#HTB_ArrivalDate").val());

		});
		function St_Gubun_Change() {
		    //location.href = "./DeliveryCharge.aspx?AB=" + form1.ArrivalBranch.value + "&M=" + document.getElementById("St_Month").value + "&G=" + form1.St_Gubun.value;
			//document.getElementById("SelectedType")
		    location.href = "./DeliveryCharge.aspx?AB=" + form1.ArrivalBranch.value + "&SD=" + form1.TB_DepartureDate.value + "&ED=" + form1.TB_ArrivalDate.value + "&G=" + form1.St_Gubun.value + "&SelectedType=" + form1.SelectedType.options[form1.SelectedType.selectedIndex].value;		    
		}
		function St_Storage_Change() {
		    location.href = "./DeliveryCharge.aspx?AB=" + form1.ArrivalBranch.value + "&SD=" + form1.TB_DepartureDate.value + "&ED=" + form1.TB_ArrivalDate.value + "&G=" + form1.St_Gubun.value + "&SelectedType=" + form1.SelectedType.options[form1.SelectedType.selectedIndex].value + "&SelectedStorage=" + form1.SelectedStorage.options[form1.SelectedStorage.selectedIndex].value;

		}
		function TBCConfirm(TBCPk) {
			if (form1.Gubun.value == "After") {
				alert("착불은 승인이 필요없습니다.");
			}
			else {
				Finance.TransportBetweenCompanyConfirm(TBCPk, function (result) {
					if (result == "1") {
						alert("Success");
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
	</script>
</head>
<body style="background-color:#E4E4E4; width:1100px; margin:0 auto; padding-top:10px;" >
    <form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="Server"><Services><asp:ServiceReference Path="~/WebService/Finance.asmx" /></Services></asp:ScriptManager>
   		<uc1:Loged ID="Loged1" runat="server" />
		<div style="background-color:White; width:1050px; height:100%; padding:25px;">
			<p>
				도착지사 : 
				<%--<select id="ArrivalBranch" onchange="St_Gubun_Change();">--%>
                <select id="ArrivalBranch" >
					<option value="3157">Korea</option>
					<option value="3095">Japan</option>
					<option value="3843">广州</option>
					<option value="2888">义乌</option>
					<option value="2886">烟台</option>
					<option value="2887" >瀋陽</option>
					<option value="3388">青岛</option>
					<option value="11456">杭州</option>
                    <option value="7898">衢州</option>
					<option value="3798">OtherLocation</option>
				</select>
				<%--<%=SelectMonth %>--%>
				<select id="St_Gubun" >
                    <%--<select id="Select1" onchange="St_Gubun_Change();">--%>
					<option value="Before">현불</option>
					<option value="After">착불</option>
				</select>
            
			<input type="hidden" id="HTB_DepartureDate" value="<%=StartDate %>" />
			<input type="hidden" id="HTB_ArrivalDate" value="<%=EndDate %>" />

			<input id="TB_DepartureDate" size="10" type="text" style="text-align:center;" /> ~ 
			<input id="TB_ArrivalDate" size="10" type="text" style="text-align:center;"  />
		
            <input type="button" value="Search" onclick="St_Gubun_Change();" />
			</p>
			<div><%=HTMLList %></div>
			<input type="hidden" id="Gubun" value="<%=Gubun %>" />
            
			<%--<input type="hidden" id="Month" value="<%=Month %>"/>--%>
		</div>
    </form>
</body>
</html>
