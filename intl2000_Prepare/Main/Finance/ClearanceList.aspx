<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClearanceList.aspx.cs" Inherits="Finance_ClearanceList" Debug="true" %>
<%@ Register src="../Admin/LogedWithoutRecentRequest11.ascx" tagname="LogedWithoutRecentRequest11" tagprefix="uc2" %>

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
			form1.ArrivalBranch.value = form1.HArrivalBranch.value;
			form1.ShipperSelect.value = form1.HShipperSelect.value;
			$("#StartDate").datepicker();
			$("#StartDate").datepicker("option", "dateFormat", "yymmdd");
			$("#StartDate").val($("#HStartDate").val());
			$("#EndDate").datepicker();
			$("#EndDate").datepicker("option", "dateFormat", "yymmdd");
			$("#EndDate").val($("#HEndDate").val());

		}
		function SelectThisShipper(PK) {
			location.href = "./ClearanceList.aspx?G=S&V=" + PK + "&AB=" + form1.ArrivalBranch.value + "&SD=" + form1.StartDate.value + "&ED=" + form1.EndDate.value;
		}
		function SelectThisCompany() {
			location.href = "./ClearanceList.aspx?G=C&V=" + form1.SerchCompanyPk.value + "&AB=" + form1.ArrivalBranch.value + "&SD=" + form1.StartDate.value + "&ED=" + form1.EndDate.value;
		}
	</script>
</head>
<body style="background-color:#E4E4E4; width:1100px; margin:0 auto; padding-top:10px;" >
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="Server"><Services><asp:ServiceReference Path="~/WebService/Finance.asmx" /></Services></asp:ScriptManager>
   	<uc2:LogedWithoutRecentRequest11 ID="LogedWithoutRecentRequest111" runat="server" />
   	
    <div style="background-color:White; width:1050px; height:100%; padding:25px;">
		<p>
			<a href="../Finance/ClearanceList.aspx"><strong>통관내역</strong></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="../Admin/SettlementWithCustoms_Date.aspx" >일별 정산</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="../Admin/SettlementWithCustoms_Container.aspx" >컨테이너별 정산</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="../Admin/SettlementWithCustoms_MultipleAdd.aspx" >엑셀 입력</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="/Finance/ClearanceEndList.aspx" >통관 내역보기</a>
		</p>
		<div style="float:right; width:550px; padding-right:100px; padding-bottom:10px;  ">
			<%=Summary %>
		</div>
		<div>
			도착지사 : 
			<select id="ArrivalBranch">
				<option value="">ALL</option>
				<option value="2886">烟台</option>
				<option value="2887" >瀋陽</option>
				<option value="2888">义乌</option>
				<option value="3095">Japan</option>
				<option value="3157">Korea</option>
				<option value="3388">青岛</option>
				<option value="3798">OtherLocation</option>
				<option value="3843">广州</option>
				<option value="11456">杭州</option>
                <option value="7898">衢州 </option>
			</select>
			<input type="hidden" id="HStartDate" value="<%=StartDate %>" />
			<input type="hidden" id="HEndDate" value="<%=EndDate %>" />
			<input id="StartDate" size="10" type="text" style="text-align:center;" readonly="readonly" /> ~ 
			<input id="EndDate" size="10" type="text" style="text-align:center;" readonly="readonly" />
			<input type="hidden" id="HArrivalBranch" value="<%=ArrivalBranch %>" />
			<input type="hidden" id="HShipperSelect" value="<%=Value %>" />
			<p>
				<select id="ShipperSelect" onchange="SelectThisShipper(this.value)">
					<option value="0">ALL</option>
					<%=InnerSelect %>
				</select>
			</p>
			<p>고객번호 : <input type="text" id="SerchCompanyPk" /> <input type="button" onclick="SelectThisCompany();" value="Serch" /></p>
		</div>
		<%=ListHtml %>
    </div>
    </form>
</body>
</html>
