<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequestList.aspx.cs" Inherits="Request_RequestList" %>
<meta http-equiv="X-UA-Compatible" content="IE=10"> 
<%@ Register src="../Member/LogedTopMenu.ascx" tagname="Loged" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<style type="text/css">
		.HeaderInfo {
			position: absolute;
			border: dotted 1px #93A9B8;
			font-weight: bold;
			padding-top: 5px;
			text-align: center;
			letter-spacing: 3px;
			width: 100px;
		}
	</style>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
		<script type="text/javascript">
			window.onload = function () {
				$(".NavRequestList").addClass("active");
			}

			function Goto(value) {
				switch (value) {
					case "back": history.back(); break;
					case "basic": location.href = "CompanyInfo.aspx?M=View&S=" + form1.HCompanyPk.value; break;
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

//			function SetCalc() {
//				Admin.DepositMinusCharge(form1.HCompanyPk.value, function (result) {
//					var ResultHtml = "";
//					for (var i = 0; i < result.length; i++) {
//						ResultHtml += result[i] + "<br />";
//					}
//					document.getElementById("PnCalcresult").innerHTML = ResultHtml;
//				}, function (result) { alert("ERROR : " + result); });
//			}
			function Goto(value) {
				location.href = "./RequestList.aspx?G=" + value;
			}
			function GoToRelation(pk) {
				location.href = "./RequestList.aspx?S=" + pk;				
			}
			function DownWithExcel() {
				location.href = "../UploadedFiles/FileDownloadWithExcel.aspx?G=MemberRequestList&S=" + form1.HCompanyPk.value;
				//location.href = "../UploadedFiles/FileDownloadWithExcel.aspx?G=CompanyRequestList&S=" + form1.HCompanyPk.value;
			}
		</script>

</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px;  ">
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
	<uc1:Loged ID="Loged1" runat="server" />
    <div class="ContentsTopMenu">
		<%=CompanyRelation %>
		<input type="button" value="All" onclick="Goto('A');" />
		<input type="button" value="In Bound" onclick="Goto('I');" />
		<input type="button" value="Out Bound" onclick="Goto('O');" />
		<div style="background-color:#E4E4E4; width:400px; height:82px; margin-left:220px;  margin-top:10px; margin-bottom:20px;">
			<div style="position:absolute; margin-left:20px; margin-top:10px; width:250px; height:50px;" >
				<strong><%=SubInfo[0] %></strong> <%=SubInfo[1] %>
				<input type="button" value="<%=GetGlobalResourceObject("qjsdur", "rjfosodurekdnsqkerl") %>" onclick="DownWithExcel();">
			</div>
			<div class="HeaderInfo" style="background-color:#FFFACD; margin-left:292px; margin-top:5px;" ><%=GetGlobalResourceObject("qjsdur", "dnsthdwlsgodwnd") %></div>
			<div class="HeaderInfo" style="background-color:#F0F8FF; margin-left:292px; margin-top:30px;" ><%=GetGlobalResourceObject("qjsdur", "cnfrheorl") %></div>
			<div class="HeaderInfo" style="background-color:white; margin-left:292px; margin-top:55px; " ><%=GetGlobalResourceObject("qjsdur", "dhksfy") %></div>
		</div>

		<%=HtmlList %>
		<input type="hidden" id="HCompanyPk" value="<%=MemberInfo[1] %>" />
    </div>
    </form>
</body>
</html>
