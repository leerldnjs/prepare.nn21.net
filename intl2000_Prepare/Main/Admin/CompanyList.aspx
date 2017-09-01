<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyList.aspx.cs" Inherits="Admin_CompanyList" Debug="true" %>
<%@ Register Src="../CustomClearance/Loged.ascx" TagName="Loged" TagPrefix="uc2" %>
<%@ Register Src="~/Admin/LogedWithoutRecentRequest11.ascx" TagPrefix="uc1" TagName="LogedWithoutRecentRequest11" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
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
	</script>
</head>
<body style="background-color: #E4E4E4; width: 1100px; margin: 0 auto; padding-top: 10px;">
	<form id="form1" runat="server">
		<uc1:LogedWithoutRecentRequest11 runat="server" ID="LogedWithoutRecentRequest11" />
		<uc2:Loged ID="Loged1" runat="server" Visible="false" />
      <asp:ScriptManager ID="SM" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Admin.asmx" />
			</Services>
		</asp:ScriptManager>
      		<div style="background-color: White; width: 1050px; height: 100%; padding: 25px;">
			<input type="hidden" id="HAccountID" value="<%=ACCOUNTID %>" />
         <div><%=Txt_companyaddinfo199_3157  %><%=BTN_companyaddinfo199_3157 %></div>
			<%=NAVIGATION %>
			<%=COMPANYLIST %>
		</div>
	</form>
</body>
</html>