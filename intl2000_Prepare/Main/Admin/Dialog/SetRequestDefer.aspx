<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetRequestDefer.aspx.cs" Inherits="Admin_Dialog_SetRequestDefer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />    
    <script type="text/javascript">
    	function BTN_Submit_Click() {
    		Admin.SetRequestDefer(form1.HRequestFormPk.value, form1.HAccountID.value, form1.TBComment.value.replace("'", "''"), SetRequestDeferSuccess, ONFAILED);
    	}
    	function SetRequestDeferSuccess(result) {
    		if (result == "Y") {
    			window.returnValue = true;
    			returnValue = result;
    			self.close();
    		}
    		else { alert(result); }
    	}
    	function ONFAILED(result) { alert(result); }
    </script>
</head>
<body style="padding:10px;" onload="document.getElementById('HAccountID').value = dialogArguments; ">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="WebService" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
    <fieldset>
		<legend><%=GetGlobalResourceObject("qjsdur", "wjqtnqhfbtkdb")%></legend>
		<textarea id="TBComment" cols="40" rows="10"></textarea> 
		<input type="hidden" id="HAccountID" />
		<input type="hidden" id="HRequestFormPk" value="<%=Request.Params["S"] %>" />
    </fieldset>
    <div style="text-align:center; padding-top:20px; ">
		<input type="button" onclick="BTN_Submit_Click()" value="<%=GetGlobalResourceObject("RequestForm", "Submit") %>" />
	</div>
    </form>
</body>
</html>