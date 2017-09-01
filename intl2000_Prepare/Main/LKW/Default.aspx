<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="LKW_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

    <div id="HTML_TEST">
		<input type="text" name="TEST" id="TB_TEST"  />    
		<input type="button" id="BTN_TEST" />
		<span id="SP_AAAA">클릭</span>
		<span id="SP_CCCC">클릭2</span>
		<input type="button" id="BTN_SUBMIT" value="서브밋" />

    </div>
    </form>

	<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$("#BTN_TEST").on("click", AAAA);
			$("#SP_AAAA").on("click", BBBB);
			$("#SP_CCCC").on("click", CCCC);
			$("#BTN_SUBMIT").on("click", function () {
				alert($("#form1").serialize());
				return false; 
			});
		});
		function CCCC() {
			$("#HTML_TEST").html("1");
			alert($("#HTML_TEST").html());
			$("#HTML_TEST").append("3");

		}
		function BBBB() {
			$("#TB_TEST").val("BBBB");
		}
		function AAAA() {
			alert($("#TB_TEST").val());
		}

	</script>
</body>
</html>
