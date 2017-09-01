<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="MyAccountView.aspx.cs" Inherits="Member_MyAccountView" %>
<meta http-equiv="X-UA-Compatible" content="IE=10"> 
<%@ Register src="../Member/LogedTopMenu.ascx" tagname="Loged" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
		.UnderLineBlue{ border-bottom:solid 2px #93A9B8; }
		.UnderLineDotted{ border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px; }
    </style>
    <script type="text/javascript">
    	window.onload = function () {
    		$(".NavInformation").addClass("active");
    	}

    	function LanguageSet(Lang) {
    		form1.Tb_LanguageSetInfo.value = Lang;
    		__doPostBack('Btn_ChangeLanguage', '');
    	}
    	function DoModify() {
    		var TEL = form.TB_TEL1.value;
    		var Mobile = form.TB_Mobile1.value;
    		Member.UpdateCompanyAccount(form.Hidden_ID.value, form.TB_Duties.value, form.TB_Name.value, TEL, Mobile, form.TB_Email.value, function (result) { if (result == "1") { alert("SUCCESS"); } }, callError);
    	}
    	function callError(result) { window.alert(form.HError.value); }
    	function PwdChangeCancel() {
    		form.OldPWD.value = "";
    		form.NewPWD.value = "";
    		form.NewPWD2.value = "";
    	}
    	function ChangePWD() {
    		if (form.OldPWD.value == "") { alert(form.HPWDMust.value); return false; }
    		if (form.NewPWD.value != form.NewPWD2.value) {
    			alert(form.HPWDDifferent.value);
    			form.NewPWD.select();form.NewPWD.focus();
    			return false;
    		}
    		Member.ChangePassword(form.Hidden_ID.value, form.OldPWD.value, form.NewPWD.value, SuccessChangePWD, callError);
    	}
    	function SuccessChangePWD(result) {
    		switch (result) {
    			case "0":
    				alert(form.HWrongPWD.value);
    				form.OldPWD.select();
    				form.OldPWD.focus();
    				break;
    			case "1":
    				alert(form.HiddenModifySuccess.value);
    				PwdChangeCancel();
    				break;
    		}
    	}
	</script>
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px; ">
    <form id="form" runat="server">
    <asp:ScriptManager ID="Member" runat="server"><Services><asp:ServiceReference Path="~/WebService/Member.asmx" /></Services></asp:ScriptManager>
	<uc1:Loged ID="Loged1" runat="server" />
    <div class="ContentsTopMenu" style="height:300px;">
		<p>
			<a href="OwnCustomerList.aspx"><%=GetGlobalResourceObject("Member", "InfoCustomer")%></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="MyCompanyView.aspx"><%=GetGlobalResourceObject("Member", "InfoCompany")%></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="MyAccountView.aspx"><strong><%=GetGlobalResourceObject("Member", "InfoAccount")%></strong></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="MyStaffView.aspx"><%=GetGlobalResourceObject("qjsdur", "djqandnjswjdqh") %></a>
		</p>
		<div style="width:300px;  float:left; z-index:5;">
			<ul style="line-height:30px;">
				<li><%=GetGlobalResourceObject("Member", "Name") %> : <input type="text" id="TB_Name" value="<%= Name %>" /> </li>
				<li><%=GetGlobalResourceObject("Member", "Duties") %> : <input type="text" id="TB_Duties" value="<%= Duties %>" /></li>
				<li>TEL : <input type="text" id="TB_TEL1" value="<%= TEL %>" /></li>
				<li>Mobile : <input type="text" id="TB_Mobile1" value="<%= Mobile %>" /></li>
				<li>E-Mail : <input type="text" id="TB_Email" onblur="onlyEmail(this)" value="<%= Email %>" /></li>
			</ul>
			<input type="button" id="BTN_Modify" value="<%=GetGlobalResourceObject("Member", "Modify") %>" style="width:98px; height:30px; margin-top:10px; margin-left:80px;" onclick="DoModify()" />
		</div>
		<div style="padding-top:15px;">
		<div id="Pn_PWD" style=" line-height:30px; ">
			<%=GetGlobalResourceObject("Member", "PWDOld") %> : <input type="password" id="OldPWD" /><br />
			<%=GetGlobalResourceObject("Member", "PWD") %> : <input type="password" id="NewPWD" /><br />
			<%=GetGlobalResourceObject("Member", "PWD2") %> : <input type="password" id="NewPWD2" /><br />
			<input type="button" style="margin-left:50px;width:98px; height:30px; margin-top:20px; " onclick="ChangePWD()" value="<%=GetGlobalResourceObject("Member", "Modify") %>" />
		</div>
		
		</div>
		<input type="hidden" id="Hidden_ID" value="<%= AccountID %>" />
		<input type="hidden" id="HiddenModifySuccess" value="<%= GetGlobalResourceObject("Alert", "ModifySuccess") %>" />
		<input type="hidden" id="HError" value="<%= GetGlobalResourceObject("Alert", "CallError") %>" />
		<input type="hidden" id="HPWDDifferent" value="<%=GetGlobalResourceObject("Alert", "PWDDifferent") %>" />		
		<input type="hidden" id="HPWDMust" value="<%=GetGlobalResourceObject("Alert", "PWDMust") %>" />
		<input type="hidden" id="HWrongPWD" value="<%=GetGlobalResourceObject("Alert", "WrongPWD") %>" />
	</div>
    </form>
</body>
</html>