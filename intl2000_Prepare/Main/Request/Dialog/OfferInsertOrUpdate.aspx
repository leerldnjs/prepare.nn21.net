<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OfferInsertOrUpdate.aspx.cs" Inherits="Request_Dialog_OfferInsertOrUpdate" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ask Update or Insert</title>
	<link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		var RequestFormPk;
		var SessionPk;
		var AccountID;
		var ShipperCode;
		var MonetaryUnit;
		var itemInfo;
		window.onload = function () {
			//alert(dialogArguments);
			var DA = dialogArguments.toString().split("$$$$$$");
			RequestFormPk = DA[0];
			SessionPk = DA[1];
			AccountID = DA[2];
			ShipperCode = DA[3];
			MonetaryUnit = DA[4];
			itemInfo = DA[5];
		}
		function BTNSubmitClick(Gubun) {
			//alert(itemInfo);
			Request.OfferSaveOrUpdate(Gubun, RequestFormPk, SessionPk, AccountID, ShipperCode, MonetaryUnit, itemInfo, function (result) {
				window.returnValue = true;
				returnValue = result[0];
				self.close();
			}, ONFAILED);
		}
		function ONFAILED(result) { window.alert(result); return false; }
	</script>
</head>
<body style="padding:10px;">
    <form id="form1" runat="server">
	<asp:ScriptManager ID="WebService" runat="server" ><Services><asp:ServiceReference Path="~/WebService/Request.asmx" /></Services></asp:ScriptManager>
    <fieldset style="padding:10px; line-height:25px;text-align:center;">
		이 문서를 수정하시겠습니까? <br />
		아니면 새로저장하시겠습니까? <br />
		<input type="button" value="<%=GetGlobalResourceObject("qjsdur", "anstjtnwjd")%>" onclick="BTNSubmitClick('Modify');" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		<input type="button" value="새로저장" onclick="BTNSubmitClick('SaveAs');" />
    </fieldset>
    </form>
</body>
</html>