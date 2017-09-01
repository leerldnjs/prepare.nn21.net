<%@ Page Language="C#" AutoEventWireup="true" Debug="True" CodeFile="Intro.aspx.cs" Inherits="Intro" meta:resourcekey="PageResource1" %>

<%--<meta http-equiv="X-UA-Compatible" content="IE=10"> --%>
<%@ Register Src="../Member/LogedTopMenu.ascx" TagName="Loged" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
		.RecentRequestTHEAD {
			border-bottom: solid 2px #93A9B8;
			text-align: center;
		}
		.RecentRequestTBODY {
			text-align: center;
			border-bottom: dotted 1px #E8E8E8;
			padding-top: 4px;
			padding-bottom: 4px;
		}
	</style>
	<script type="text/javascript">
		window.onload = function () {
			//window.open("/popup/pop2017.html", "pop2017", "width=819, height=579, menubar=no, status=no, toolbal=no");
			//window.open("/popup/pop2017_04A.html", "pop2017_04A", "width=819, height=579, menubar=no, status=no, toolbal=no");
			//window.open("/popup/pop2017_04B.html", "pop2017_04B", "width=819, height=579, menubar=no, status=no, toolbal=no");
		}
		function Thanksgiving() {
			var DialogResult = "";
			var retVal = window.showModalDialog('./Thanksgiving.aspx', DialogResult, 'dialogWidth=1220px; dialogHeight=790px; resizable=0; status=0; scroll=1; help=0;');
			if (retVal == "Y") { window.document.location.reload(); }
		}
		function Attention() {
			var DialogResult = "";
			var HLanguage = form1.HLanguage.value;
			var retVal = window.showModalDialog('./Attention.aspx?H=' + HLanguage, DialogResult, 'dialogWidth=400px; dialogHeight=200px; resizable=0; status=0; scroll=0; help=0;');
			if (retVal == "Y") { window.document.location.reload(); }
		}
	</script>
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 10px;">
	<form id="form1" runat="server">
		<uc1:Loged ID="Loged1" runat="server" />
		<div class=" ContentsTopMenu">
			<div style="margin: 0 auto; width: 820px; padding: 20px 20px 20px 20px;"><%= Attention  %></div>
			<%--<div>
			<div style="position:absolute; margin-top:170px; margin-left:180px; text-align:right; font-size:16px; line-height:25px;  ">
				한 해 동안 저희 국제종합물류를 이용해 주셔서 감사합니다.<br />
				다가오는 새해에도 늘 건강하시길 기원합니다.<br />
				앞으로도 경쟁력 있는 물류서비스 제공을 위해 최선을 다하겠습니다.<br />
				<strong>- 국제종합물류 대표 이 재 구</strong>
			</div>
			<img style="width:800px; " src="../Images/20141218_114033.png" />
		</div>--%>
			<div style="margin: 0 auto; width: 820px; padding-left: 20px;"><%= RecentRequest %></div>
			<div style="clear: both;">&nbsp;</div>
			<input type="hidden" id="HLanguage" value="<%=Session["Language"] %>" />
			<div><a href="../Request/RequestList.aspx"><strong>거래내역 보기1</strong></a></div>
		</div>
	</form>
</body>
</html>