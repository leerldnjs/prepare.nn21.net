<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyList_SearchAddress.aspx.cs" Inherits="Admin_CompanyList_SearchAddress" %>
<%@ Register Src="~/Admin/LogedWithoutRecentRequest11.ascx" TagPrefix="uc1" TagName="LogedWithoutRecentRequest11" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
	<link href="/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #E4E4E4; width: 1100px; margin: 0 auto; padding-top: 10px;">
	<form id="form1" runat="server">
		<uc1:LogedWithoutRecentRequest11 runat="server" ID="LogedWithoutRecentRequest11" />
		<div style="background-color: White; width: 1050px; height: 100%; padding: 25px;">
			<input type="hidden" id="HAccountID" value="<%=ACCOUNTID %>" />
			<div><%=Txt_companyaddinfo199_3157  %><%=BTN_companyaddinfo199_3157 %></div>
			<%=NAVIGATION %>
			<%=COMPANYLIST %>
		</div>
    </form>
	<script src="/Common/public.js" type="text/javascript"></script>
	<script src="/Lib/jquery-1.10.2.min.js" type="text/javascript"></script>
	<script>
		function Goto(pk) {
			window.open('./Dialog/TalkBusinessforBusiness.aspx?S=' + pk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=700px; width=600px;');
		}
		function companyaddinfo199_3157() {
			Admin.companyaddinfo199_3157(form1.TB_MEMO.value, function (result) {
				if (result = "1") {
					alert("OK");
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function SetSearch() {
			location.href = "/Admin/CompanyList_SearchAddress.aspx?SearchValue=" + $("#SearchAddress").val();
		}
	</script>
</body>
</html>
