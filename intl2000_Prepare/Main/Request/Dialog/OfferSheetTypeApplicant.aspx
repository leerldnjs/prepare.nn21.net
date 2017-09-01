<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OfferSheetTypeApplicant.aspx.cs" Inherits="Request_Dialog_OfferSheetTypeApplicant" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
    	function BTNSubmitClick() {
    		var CIDName = document.getElementById("CIDName").value;
    		var CIDAddress = document.getElementById("CIDAddress").value;
    		var CIDPk = document.getElementById("CIDPk").value;
    		if (CIDName == "" || CIDAddress == "") {
    			alert("Type Applicant Company name & Address please");
    			return false;
    		}
    		else {
    			if (CIDName + "!!!!" + CIDAddress == document.getElementById("HBefore").value) {
    				window.returnValue = true;
    				returnValue = document.getElementById("CIDPk").value + "!!!!" + document.getElementById("HBefore").value;
    				self.close();
    			}
    			else { Request.SaveApplicantUsingCCL(CIDPk, form1.HCCLPK.value, CIDName, CIDAddress, SaveApplicantUsingCCLSuccess, ONFAILED); }
    		}
    	}
    	function SaveApplicantUsingCCLSuccess(result) {
    		window.returnValue = true;
    		returnValue = result + "!!!!" + document.getElementById("CIDName").value + "!!!!" + document.getElementById("CIDAddress").value;
    		self.close();
    	}
    	function ONFAILED(result) { window.alert(result); return false; }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="WebService" runat="server" ><Services><asp:ServiceReference Path="~/WebService/Request.asmx" /></Services></asp:ScriptManager>
    <div style="padding:10px;">
		<%=CCLInfo %>
		<input type="hidden" id="HCCLPK" value="<%=Request.Params["S"] %>" />		
		<input type="button" onclick="BTNSubmitClick();" value="확인" />
    </div>
    </form>
</body>
</html>