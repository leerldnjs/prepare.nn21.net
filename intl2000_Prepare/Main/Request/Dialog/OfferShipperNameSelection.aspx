<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OfferShipperNameSelection.aspx.cs" Inherits="Request_Dialog_OfferShipperNameSelection" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="~/Common/public.js" type="text/javascript"></script>
    <link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
    	function InsertShipperInDocument() {
    		if (form1.CustomerClearanceCompanyName.value == "") { alert(form1.HMustCompanyName.value); form1.CustomerClearanceCompanyName.focus(); return false; }
    		Request.InsertCompanyInDocument(form1.HSessionPk.value, form1.CustomerClearanceCompanyName.value, form1.CustomerClearanceAddress.value, InsertSuccess, AlertError);
    		//Request.InsertShipperNameInDocument(form1.SessionPk.value, form1.CustomerClearanceCompanyName.value, form1.CustomerClearanceAddress.value, InsertSuccess, AlertError);
    	}
    	function InsertSuccess(result) { window.returnValue = true; returnValue = result; self.close(); }
    	function AlertError(result) { alert(result); }
    	function Del(target) { Request.DelShipperNameInDocument(target, DelSuccess, AlertError); }
    	function DelSuccess(result) {
    		if (result == "1") { alert(form1.HDelComplete.value); window.returnValue = true; returnValue = 'D'; self.close(); }
    		else { alert(form1.HError.value); }
    	}
	</script>
</head>
<body style="padding:15px;" >
    <form id="form1" runat="server">
    <asp:ScriptManager ID="WebService" runat="server" ><Services><asp:ServiceReference Path="~/WebService/Request.asmx" /></Services></asp:ScriptManager>
    <%--<fieldset>
		<legend><strong><%=GetGlobalResourceObject("RequestForm", "Clearance") %></strong></legend>
    <div style="line-height:30px; padding:10px; ">
		<input type="hidden" id="HMustCompanyName" value="<%=GetGlobalResourceObject("Alert", "MustCompanyName") %>" />
		<input type="hidden" id="HMustAddress" value="<%=GetGlobalResourceObject("Alert", "MustAddress") %>" />
		<input type="button" style="width:92px;" value="<%=GetGlobalResourceObject("RequestForm", "ClearanceSubstitute") %>" onclick="InsertSuccess('1')" />&nbsp;&nbsp;<%=GetGlobalResourceObject("RequestForm", "ClearanceSubstituteEx") %><br />
		<%=ShipperData %>
	</div>
	</fieldset>--%>
	<%=ShipperData %>
	<fieldset>
		<legend><strong><%=GetGlobalResourceObject("RequestForm", "ClearanceSelf")%></strong>&nbsp;&nbsp;<%=GetGlobalResourceObject("RequestForm", "ClearanceSelfEx")%></legend>
		<div style="padding:10px; margin-top">
			<%= GetGlobalResourceObject("Member", "CompanyName")%> : 
			<input type="text" id="CustomerClearanceCompanyName" style="text-align:left;" size="15" />&nbsp;&nbsp;&nbsp;
			<%= GetGlobalResourceObject("Member", "Address")%> : 
			<input type="text" id="CustomerClearanceAddress" style="text-align:left;" size="40" />
			<input type="button" style="width:92px;" value="<%= GetGlobalResourceObject("Member", "Submit")%>" onclick="InsertShipperInDocument()" />
		</div>
	</fieldset>
	<input type="hidden" id="HError" value="<%= GetGlobalResourceObject("Alert", "CallError") %>" />
	<input type="hidden" id="HDelComplete" value="<%=GetGlobalResourceObject("Alert", "DeleteComplete") %>" />
	<input type="hidden" id="HSessionPk" value='<%=Request.Params["S"] %>' />
    </form>
</body>
</html>
