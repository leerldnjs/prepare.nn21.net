<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyList_Search.aspx.cs" Inherits="Admin_CompanyList_Search" %>

<%@ Register Src="~/Admin/LogedWithoutRecentRequest11.ascx" TagPrefix="uc1" TagName="LogedWithoutRecentRequest11" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script src="../Common/jquery-1.10.2.min.js"></script>
	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
	<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
	<script type="text/javascript">
		window.onload = function () {
			form1.BranchCode.value = form1.HBranchCode.value;
			$("#Criteriadate").datepicker();
			$("#Criteriadate").datepicker("option", "dateFormat", "yymmdd");
			$("#Criteriadate").val($("#HCriteriadate").val());


			form1.monthago.value = form1.Hmonthago.value;
		}
		function SerchThis() {
			location.href = "./CompanyList_Search.aspx?M=" + form1.monthago.value + "&B=" + form1.BranchCode.value + "&D=" + form1.Criteriadate.value;
		}
		function Goto(pk) {
			window.open('./Dialog/TalkBusinessforBusiness.aspx?S=' + pk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=700px; width=600px;');
		}
	</script>
</head>
<body style="background-color: #E4E4E4; width: 1100px; margin: 0 auto; padding-top: 10px;">
	<form id="form1" runat="server">
		<uc1:LogedWithoutRecentRequest11 runat="server" ID="LogedWithoutRecentRequest11" />
		<asp:ScriptManager ID="SM" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Admin.asmx" />
			</Services>
		</asp:ScriptManager>
		<div style="background-color: White; width: 1050px; height: 100%; padding: 25px;">
			<div>
				지사:
                <select id="BranchCode">
					<option value="KR">KR</option>
					<option value="JP">JP</option>
					<option value="YT">YT</option>
					<option value="SY">SY</option>
					<option value="YW">YW</option>
					<option value="QD">QD</option>
					<option value="SX">SX</option>
					<option value="OT">OT</option>
				</select>
				<br />
				기준:                    
                <input id="Criteriadate" size="10" type="text" style="text-align: center;" readonly="readonly" />
				<br />
				최근:   
                <select id="monthago">
					<option value="1">1달</option>
					<option value="2">2달</option>
					<option value="3">3달</option>
				</select>
				동안 거래내역이 없다
                
                
                
			<input type="button" onclick="SerchThis();" value="SERCH" />
			</div>

			<%=NAVIGATION %>
			<%=COMPANYLIST %>
		</div>
		<input type="hidden" id="HAccountID" value="<%=ACCOUNTID %>" />
		<input type="hidden" id="HBranchCode" value="<%=BranchCode %>" />
		<input type="hidden" id="HCriteriadate" value="<%=Criteriadate %>" />
		<input type="hidden" id="Hmonthago" value="<%=Hmonthago %>" />
	</form>
</body>
</html>
