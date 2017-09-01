<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckDescriptionList.aspx.cs" Inherits="Admin_CheckDescriptionList" Debug="true" %>

<%@ Register Src="LogedWithoutRecentRequest11.ascx" TagName="LogedWithoutRecentRequest11" TagPrefix="uc1" %>
<%@ Register Src="../CustomClearance/Loged.ascx" TagName="Loged" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
		.Date {
			float: left;
			border: 1px solid black;
			padding: 5px;
			margin: 3px;
			text-align: center;
		}

		.DateInnerDate {
			font-weight: bold;
		}

		.Shipper {
			border: solid 1px black;
			height: 90px;
			margin-top: -1px;
			padding: 5px;
		}
	</style>
	<script type="text/javascript">
		function Goto(gubun, value) {
			switch (gubun) {
				case "CheckDescription": location.href = "./CheckDescription.aspx?S=" + value; break;
				case "RequestForm": location.href = "./RequestView.aspx?g=c&pk=" + value; break;
				case "Company": location.href = "./CompanyInfo.aspx?M=View&S=" + value; break;
				default: alert(value); break;
			}
		}

		function Print(Gubun, Value) {
			switch (Gubun) {
				case "B": location.href = "../CustomClearance/CommercialDocu_HouseBL.aspx?S=" + Value + "&G=Print"; break;
				case "I": location.href = "../CustomClearance/CommercialDocu_Invoice.aspx?S=" + Value + "&G=Print"; break;
				case "P": location.href = "../CustomClearance/CommercialDocu_PackingList.aspx?S=" + Value + "&G=Print"; break;
				case "D": window.open('./Dialog/AutoPrint.aspx?G=DeliveryReceipt&S=' + Value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;'); break;
			}
		}
	</script>
</head>
<body style="background-color: #E4E4E4; width: 1100px; margin: 0 auto; padding-top: 10px;">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Admin.asmx" />
			</Services>
		</asp:ScriptManager>
		<uc2:Loged ID="Loged1" runat="server" Visible="false" />
		<uc1:LogedWithoutRecentRequest11 ID="LogedWithoutRecentRequest111" runat="server" />
		<div style="background-color: White; width: 1050px; height: 100%; padding: 25px;">
			<div style="width: 1050px; clear: both;">
				<%=Header %>
				<%=PageDate %>
			</div>
			<div style="font-size: 1px; clear: both;">&nbsp;</div>
			<div><%=RequestList %></div>
		</div>
	</form>
</body>
</html>
