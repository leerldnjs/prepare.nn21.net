 <%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyInfoRequestList.aspx.cs" Inherits="Admin_CompanyInfoRequestList" Debug="true" %>
<%@ Register src="LogedWithoutRecentRequest.ascx" tagname="LogedWithoutRecentRequest" tagprefix="uc1" %>
<%@ Register Src="../CustomClearance/Loged.ascx" TagName="Loged" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Company Info : Customer RequestList</title>
	<style type="text/css">
		.HeaderInfo{position:absolute; border:dotted 1px #93A9B8; font-weight:bold; padding-top:5px; text-align:center; letter-spacing:3px; width:100px; }
	</style>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
		<script type="text/javascript">
			function SetPrice(RequestFormPk) {
				var dialogArgument = form1.HAccountID.value;
				var retVal = window.showModalDialog('./Dialog/FixCharge.aspx?S=' + RequestFormPk, dialogArgument, "dialogHeight:500px; dialogWidth:750px; resizable:0; status:0; scroll:1; help:0; ");
				if (retVal == "Y") { window.document.location.reload(); }
			}
			function GotoEmailSend(RequestFormPk, StepCL) { }
			function Goto(value) {
				switch (value) {
					case "back": history.back(); break;
					case "basic": location.href = "CompanyInfo.aspx?M=View&S=" + form1.HCompanyPk.value; break;
					case "NewBasic": location.href = "CompanyView.aspx?S=" + COMPANYPK; break;
					case "customer": location.href = "CompanyInfoCustomer.aspx?S=" + form1.HCompanyPk.value; break;
					case "request": location.href = "CompanyInfoRequestList.aspx?G=CI&S=" + form1.HCompanyPk.value; break;
					case "talkBusiness":
						window.open('./Dialog/TalkBusiness.aspx?S=' + form1.HCompanyPk.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=1, top=200px; left=200px; height=700px; width=600px;');
						break;
					case "PrepareGoOut": location.href = "PrepareTransport.aspx"; break;
				}
			}
			function ShowFreightChargeView(RequestFormPk, SorC) {
				window.open('../Request/FreightChargeView.aspx?S=' + RequestFormPk + "&G=" + SorC.toUpperCase(), '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=yes, top=200px; left=200px; height=700px; width=800px;');
			}
			function DownWithExcel() {
				location.href = "../UploadedFiles/FileDownloadWithExcel.aspx?G=CompanyRequestList&S=" + form1.HCompanyPk.value;
			}

			function ShowDeposited(RequestFormPk, SorC, CompanyPk) {
				var dialogArgument = new Array();
				dialogArgument[0] = RequestFormPk;
				dialogArgument[1] = form1.HAccountID.value;
				dialogArgument[2] = SorC.toUpperCase();
				dialogArgument[3] = CompanyPk;
				var retVal = window.showModalDialog('./Dialog/CollectPayment.aspx', dialogArgument, "dialogHeight:500px; dialogWidth:480px; resizable:1; status:0; scroll:1; help:0; ");
				if (retVal == "Y") { window.document.location.reload(); }
			}

//			function SetCalc() {
//				Admin.DepositMinusCharge(form1.HCompanyPk.value, function (result) {
//					var ResultHtml = "";
//					for (var i = 0; i < result.length; i++) {
//						ResultHtml += result[i] + "<br />";
//					}
//					document.getElementById("PnCalcresult").innerHTML = ResultHtml;
//				}, function (result) { alert("ERROR : " + result); });
//			}
		</script>
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px; ">
	<form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
    <uc1:LogedWithoutRecentRequest ID="LogedWithoutRecentRequest1" runat="server" />
        <uc2:Loged ID="Loged2" runat="server" Visible="false" />
    <div style="background-color:White; width:850px; height:100%; padding:25px;">
		<div style="float:right; ">
			<input type="button" value="상담내역" onclick="Goto('talkBusiness');" />&nbsp;&nbsp;&nbsp;
			<%--<input type="button" value="<%=GetGlobalResourceObject("Admin", "wjqtngkrl") %>" onclick="GoRequestWrite();" />--%>
		</div>
		<div style="padding-bottom:20px; "> 
			<span onclick="Goto('back');" style="cursor:hand;" ><%=GetGlobalResourceObject("qjsdur", "enlfh")%></span>&nbsp;&nbsp;||&nbsp;&nbsp;
			<span onclick="Goto('NewBasic');" style="cursor:hand;" >Total</span>&nbsp;&nbsp;||&nbsp;&nbsp;
			<span onclick="Goto('basic');" style="cursor:hand;" ><%=GetGlobalResourceObject("qjsdur", "rlqhswjdqh")%></span>&nbsp;&nbsp;||&nbsp;&nbsp;
			<span onclick="Goto('request');" style="cursor:hand; font-weight:bold; font-weight:bold;" >RequestList</span>
			<%--&nbsp;&nbsp;||&nbsp;&nbsp;
			<span onclick="Goto('customer');" style="cursor:hand; " ><%=GetGlobalResourceObject("qjsdur", "rjfocjwjdqhqhrl")%></span>&nbsp;&nbsp;||&nbsp;&nbsp;
			--%>
		</div>
		<div style="background-color:#E4E4E4; width:400px; height:82px; margin-left:220px;  margin-top:10px; margin-bottom:20px;">
			<div style="position:absolute; margin-left:20px; margin-top:10px; width:250px; height:50px;" >
				<%=CompanyName %> <%--<input type="button" onclick="SetCalc();" value="<%=GetGlobalResourceObject("RequestForm", "setoff")%>" />--%>
				<input type="button" value="<%=GetGlobalResourceObject("qjsdur", "rjfosodurekdnsqkerl") %>" onclick="DownWithExcel();">
				<%--<div id="PnCalcresult" style="text-align:center; width:230px; padding:10px;"></div>--%>
			</div>
			<div class="HeaderInfo" style="background-color:#FFFACD; margin-left:292px; margin-top:5px;" ><%=GetGlobalResourceObject("RequestForm", "transportinprogress")%></div>
			<div class="HeaderInfo" style="background-color:#F0F8FF; margin-left:292px; margin-top:30px;" ><%=GetGlobalResourceObject("RequestForm", "waitingforrelease")%></div>
			<div class="HeaderInfo" style="background-color:white; margin-left:292px; margin-top:55px; " ><%=GetGlobalResourceObject("qjsdur", "dhksfy")%></div>
		</div>
		<%=HtmlList %>
		<input type="hidden" id="HAccountID" value="<%=MemberInfo[2] %>" />
		<input type="hidden" id="HCompanyPk" value="<%=CompanyPk %>" />
	</div>
    </form>
</body>
</html>