<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnCustomerView.aspx.cs" Inherits="Member_OwnCustomerView" Debug="true" %>
<%@ Register src="../Member/LogedTopMenu.ascx" tagname="Loged" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
		.UnderLineBlue{ border-bottom:solid 2px #93A9B8; }
		.UnderLineDotted{ border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px; }
		.RecentRequestTHEAD{border-bottom:solid 2px #93A9B8; text-align:center; background-color:#E8E8E8; }
		.RecentRequestTBODY{text-align:center; border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px; }
    </style>
    
	<script type="text/javascript">
		function Btn_UpdateClick() {
			var Memo = FormNow.TB_Memo.value;
			if (FormNow.HGubunCL.value == "0") {
				Member.UpdateCompanyRelationMemo(FormNow.Hidden_Pk.value, Memo, function (result) {
					if (result == "1") { alert(FormNow.dhksfy.value); location.reload(); }
					else { alert(FormNow.HError.value); }
				}, callError);
			}
			else {
				var TEL = FormNow.TB_TEL.value;
				var FAX = FormNow.TB_FAX.value;
				var Email= FormNow.TB_Email.value;
				var PresidentName = FormNow.TB_PresidentName.value;

				Member.UpdateCompanyRelationInformateion(FormNow.Hidden_Pk.value, FormNow.HTargetPk.value, TEL,	FAX, Email, PresidentName, Memo, function (result) {
					if (result == "1") { alert(FormNow.dhksfy.value); location.reload(); }
					else { alert(FormNow.HError.value); }
				}, callError);
			}
    	}
    	function callError(result) { window.alert(FormNow.HError.value); }
	</script>
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px; ">
    <form id="FormNow" runat="server">
    <asp:ScriptManager runat="server"><Services><asp:ServiceReference Path="~/WebService/Member.asmx" /></Services></asp:ScriptManager>
	<uc1:Loged ID="Loged1" runat="server" />
    <div class=" ContentsTopMenu" style=" width:775px;  padding-left:100px;">
		<p>
			<a href="OwnCustomerList.aspx"><strong><%=GetGlobalResourceObject("Member", "InfoCustomer")%></strong></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="MyCompanyView.aspx"><%=GetGlobalResourceObject("Member", "InfoCompany")%></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="MyAccountView.aspx"><%=GetGlobalResourceObject("Member", "InfoAccount")%></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="MyStaffView.aspx"><%=GetGlobalResourceObject("qjsdur", "djqandnjswjdqh") %></a>
		</p>
		<div style="width:300px; height:185px; float:left" >
			<%=CompanyInformation %>
		</div>
		<div style="padding-top:20px;">
			<p><%=GetGlobalResourceObject("Member", "Memo") %></p><textarea id="TB_Memo" rows="9" cols="60" ><%= Memo %></textarea>
		</div>
		<div style="clear:both; padding-left:300px; padding-top:20px; padding-bottom:40px; ">
			<input type="button" value="<%=GetGlobalResourceObject("Member", "Modify") %>" style="width:80px; height:35px;" onclick="Btn_UpdateClick()" />
		</div>
        <%=RequestList %>
	</div>
	<input type="hidden" id="Hidden_Pk" value="<%= Request["pk"] %>" />
	<input type="hidden" id="HTargetPk" value="<%=TargetPk %>" />
	<input type="hidden" id="HGubunCL" value="<%=GubunCL %>"/>
	<input type="hidden" id="HError" value="<%= GetGlobalResourceObject("Alert", "CallError") %>" />
	<input type="hidden" id="dhksfy" value="<%=GetGlobalResourceObject("qjsdur", "dhksfy") %>" />
    </form>
</body>
</html>