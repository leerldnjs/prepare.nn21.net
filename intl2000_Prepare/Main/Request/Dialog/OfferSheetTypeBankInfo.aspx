<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OfferSheetTypeBankInfo.aspx.cs" Inherits="Request_Dialog_OfferSheetTypeBankInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		function BodyOnload() {form1.TBCompanyName.value= dialogArguments;}
		function BTNSubmitClick() {
			var BankName = form1.TBBankName.value;
			var BankAddress = form1.TBBankAddress.value;
			var SwiftCode = form1.TBSwiftCode.value;
			var AccountNo = form1.TBAccountNo.value;

			if (BankName == "" || BankAddress == "" || SwiftCode == "" || AccountNo == "") { alert("Type Bank Information please"); return false; }
			else if (form1.TBBankName.value + "!!!!" + form1.TBBankAddress.value + "!!!!" + form1.TBSwiftCode.value + "!!!!" + form1.TBAccountNo.value == form1.HSum.value) {
				window.returnValue = true;
				returnValue = form1.HBankPk.value + "!!!!" + form1.HSum.value;
				self.close();
			}
			else if (form1.HBankPk.value == "") {
				Request.SaveOfferSheetBankInfo("I", form1.HCompanyInDocumentPk.value, form1.TBBankName.value, form1.TBBankAddress.value, form1.TBSwiftCode.value, form1.TBAccountNo.value, SaveOfferSheetBankInfoSuccess, ONFAILED);
			}
			else {
				Request.SaveOfferSheetBankInfo("U", form1.HBankPk.value, form1.TBBankName.value, form1.TBBankAddress.value, form1.TBSwiftCode.value, form1.TBAccountNo.value, SaveOfferSheetBankInfoSuccess, ONFAILED);
			}
		}
		function SaveOfferSheetBankInfoSuccess(result) {
			window.returnValue = true;
			returnValue = result + "!!!!" + form1.TBBankName.value + "!!!!" + form1.TBBankAddress.value + "!!!!" + form1.TBSwiftCode.value + "!!!!" + form1.TBAccountNo.value;
			self.close();
		}
		function ONFAILED(result) { window.alert(result); return false; }
	</script>
</head>
<body onload="BodyOnload();">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="WebService" runat="server" ><Services><asp:ServiceReference Path="~/WebService/Request.asmx" /></Services></asp:ScriptManager>
    <div>
		<input type="hidden" id="HCompanyInDocumentPk" value="<%=Request.Params["S"] %>" />
		<input type="hidden" id="HBankPk" value="<%=BankInfo[0] %>" />
		<input type="hidden" id="HSum" value="<%=BankInfo[1]+"!!!!"+BankInfo[2]+"!!!!"+BankInfo[3]+"!!!!"+BankInfo[4] %>" />
		
		<p><input type="text" id="TBCompanyName" readonly="readonly" /></p>
		<p>BANK : <input type="text" id="TBBankName" value="<%=BankInfo[1] %>" /></p>
		<p>BANK ADDRESS : <input type="text" id="TBBankAddress" value="<%=BankInfo[2] %>" /></p>
		<p>SWIFT CODE : <input type="text" id="TBSwiftCode" value="<%=BankInfo[3] %>" /></p>
		<p>ACCOUNT NO : <input type="text" id="TBAccountNo" value="<%=BankInfo[4] %>" /></p>
		<p><input type="button" value="확인" onclick="BTNSubmitClick();" /></p>
    </div>
    </form>
</body>
</html>
