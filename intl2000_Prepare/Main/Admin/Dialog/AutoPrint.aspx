<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoPrint.aspx.cs" Inherits="Admin_Dialog_AutoPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script type="text/javascript">
		window.onload = function () {
			if (form1.hgubun.value == "DeliveryReceipt") {
				Admin.LoadDeliveryReceiptPk(form1.hpk.value, function (result) {
					if (result.length == 0) {
						alert("인쇄할 출고증이 없습니다.");
					}
					else {
						for (var i = 0; i < result.length; i++) {
							window.open('./DeliveryReceipt2.aspx?G=Print&S=' + result[i], '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=500px; width=900px;');
						}
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
	</script>
</head>
<body>
    <form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
    <div>
		<input type="hidden" id="hgubun" value="<%=gubun %>" />
		<input type="hidden" id="hpk" value="<%=pk %>" />
    </div>
    </form>
</body>
</html>
