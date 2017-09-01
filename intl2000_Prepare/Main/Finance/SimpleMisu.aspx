<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SimpleMisu.aspx.cs" Inherits="Finance_SimpleMisu" %>

<%@ Register Src="../Admin/LogedWithoutRecentRequest.ascx" TagName="Loged" TagPrefix="uc1" %>
<%@ Register Src="../CustomClearance/Loged.ascx" TagName="Loged" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script src="/Common/public.js" type="text/javascript"></script>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />

   <script type="text/javascript">
   	function GoExcel(Type) {
   		location.href = "../UploadedFiles/FileDownloadWithExcel.aspx?G=SimpleMisu&D=Now&T=" + Type;
   	}
   </script>
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 10px;">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="Server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Finance.asmx" />
			</Services>
		</asp:ScriptManager>
		<uc1:Loged ID="Loged1" runat="server" />
		<uc2:Loged ID="Loged2" runat="server" Visible="false" />
		<div style="background-color: White; width: 850px; height: 100%; padding: 25px;">
			<p><a href="SimpleMisu_2015Before.aspx">2015년 이전</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href="SimpleMisu.aspx"><strong>2015년 이후</strong></a></p>
			<p>
				<span onclick="GoExcel('03');" style="cursor: pointer;">(012)</span>
				<span onclick="GoExcel('46');" style="cursor: pointer;">(3456)</span>
				<span onclick="GoExcel('79');" style="cursor: pointer;">(789)</span>
			</p>
			<div style="float: left; padding-right: 30px; background-color: white;"><%=LeftMenu %></div>
			<%=ListHtml %>
		</div>
	</form>
</body>
</html>		