<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="MyCompanyView.aspx.cs" Inherits="Member_MyCompanyView" meta:resourcekey="PageResource1" %>
<meta http-equiv="X-UA-Compatible" content="IE=10"> 
<%@ Register src="../Member/LogedTopMenu.ascx" tagname="Loged" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script src="../Common/public.js" type="text/javascript"></script>
	<script type="text/javascript">
		window.onload = function () {
			$(".NavInformation").addClass("active");
		}

		function GoModify() {
			form1.BTN_Submit.style.visibility = "hidden";
			var CompanyTEL = CompanyTEL = document.getElementById("TB_CompanyTEL").value;
			var CompanyFAX = document.getElementById("TB_CompanyFAX").value;
			var CompanyEmail = form1.TB_Email.value;
		
			var AdditionalInfo = "@@@@62##" + form1.TB_Homepage.value;
			var CompanyUpjong = "";
			if (document.getElementById("Upjong1").checked) { CompanyUpjong += "57"; }
			if (document.getElementById("Upjong2").checked) { CompanyUpjong += "!58"; }
			if (document.getElementById("Upjong3").checked) { CompanyUpjong += "!59"; }
			AdditionalInfo += "@@@@63##" + CompanyUpjong;
			AdditionalInfo += "@@@@64##" + form1.TB_CategoryofBusiness.value + "!" + form1.TB_BusinessConditions.value;
			Member.ModifyCompanyInfo(form1.HCompanyPk.value, CompanyTEL, CompanyFAX, CompanyEmail, AdditionalInfo, ModifySuccess, CallError);
		}
		function ModifySuccess(result) {
			switch (result) {
				case "0": alert("변경사항 없습니다?"); break;
				case "1": alert("성공"); break;
				default: alert(result); break;
			}
			form1.BTN_Submit.style.visibility = "visible";
		}
		function CallError(result) {alert(result);  alert(form1.HError.value); }
		function MaybeModify(type, target) {
			if (type == "1") { document.getElementById(target).value = "1"; }
			else { document.getElementById('Check' + target).value = "1"; }
		}
	</script>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
		.TableTop{background-color:#f5f5f5; border-right:dotted 1px #BBBBBB;  text-align:center; height:25px; border-bottom:dotted 1px #E8E8E8; padding:4px;}
		.TableBody{text-align:center; padding:5px; border:dotted 1px #E8E8E8; background-color:White;}
	</style>
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px; ">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"><Services><asp:ServiceReference Path="~/WebService/Member.asmx" /></Services></asp:ScriptManager>
    <uc1:Loged ID="Loged1" runat="server" />
    <div class=" ContentsTopMenu">
		<p>
			<a href="OwnCustomerList.aspx"><%=GetGlobalResourceObject("Member", "InfoCustomer")%></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="MyCompanyView.aspx"><strong><%=GetGlobalResourceObject("Member", "InfoCompany")%></strong></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="MyAccountView.aspx"><%=GetGlobalResourceObject("Member", "InfoAccount")%></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="MyStaffView.aspx"><%=GetGlobalResourceObject("qjsdur", "djqandnjswjdqh") %></a>
		</p>
		<input type="hidden" id="HCompanyPk" value="<%=CompanyPk %>" />
		<div style="float:right; width:450px; line-height:23px;  ">
			<ul>
			<li>TEL : <input type="text" id="TB_CompanyTEL" value="<%=TEL %>" /></li>
			<li>FAX : <input type="text" id="TB_CompanyFAX" value="<%=FAX %>" /></li>
			<li>E-mail : <input type="text" id="TB_Email" value="<%=Email %>" style="width:200px;" /></li>
			<li>Homepage : <input type="text" id="TB_Homepage" value="<%=Homepage %>" /></li>
			<li>
				<%=GetGlobalResourceObject("Member", "Upjong")%> / <%=GetGlobalResourceObject("Member", "Uptae")%> : 
				<input type="text" id="TB_CategoryofBusiness" size="8" value="<%=upjong %>" style="text-align:center;" /> / 
				<input type="text" id="TB_BusinessConditions" size="8" value="<%=uptae %>" style="text-align:center;" />
			</li>
			<li>
				<%=GetGlobalResourceObject("Member", "Upmu")%> : 
				<input type="checkbox" id="Upjong1" <%=Upmu57 %> /> <label for="Upjong1"><%=GetGlobalResourceObject("Member", "Production")%></label> &nbsp;&nbsp;
				<input type="checkbox" id="Upjong2" <%=Upmu58 %> /> <label for="upjong2"><%=GetGlobalResourceObject("Member", "Distribution")%></label> &nbsp;&nbsp;
				<input type="checkbox" id="Upjong3" <%=Upmu59 %> /> <label for="upjong3"><%=GetGlobalResourceObject("Member", "Saler")%></label> 
			</li>
			</ul>
		</div>
		<ul style="line-height:30px; width:400px; ">
			<li><%= GetGlobalResourceObject("Member", "Country") %> : <%=Country %></li>
			<li><%=GetGlobalResourceObject("Member", "CompanyName") %> : <%=CompanyName %></li>
			<li><%=GetGlobalResourceObject("Member", "Address") %> : <%=CompanyAddress %></li>
			<li><%= GetGlobalResourceObject("Member", "SaupjaNo")%> : (<%=SaupjaGubun %>) <%=CompanyNo %></li>
			<li><%=GetGlobalResourceObject("Member", "PresidentName")%> : <%=PresidentName %></li>
		</ul>
		<div style="text-align:center; padding-top:20px; ">
			<%=BTNSUBMIT %>
		</div>
	</div>
	<input type="hidden" id="HError" value="<%= GetGlobalResourceObject("Alert", "CallError") %>" />
	<input type="hidden" id="HDuties" value="<%=MEMBERINFO[4] %>" />
    </form>
</body>
</html>
