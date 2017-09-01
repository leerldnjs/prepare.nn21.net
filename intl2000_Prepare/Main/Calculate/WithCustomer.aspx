<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WithCustomer.aspx.cs" Inherits="Calculate_WithCustomer" %>

<%@ Register Src="~/Member/LogedTopMenu.ascx" TagPrefix="uc1" TagName="LogedTopMenu" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px;  ">
    <form id="form1" runat="server">
		<uc1:LogedTopMenu runat="server" ID="LogedTopMenu" />
	    <div class="ContentsTopMenu" >
			<%=TBList %>
		</div>
	</form>
	<script>
		jQuery(document).ready(function () {
			$(".Nav2014").addClass("active");
		});
	</script>
</body>
</html>
