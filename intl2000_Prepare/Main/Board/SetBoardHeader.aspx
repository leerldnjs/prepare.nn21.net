<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetBoardHeader.aspx.cs" Inherits="Board_SetBoardHeader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link rel="stylesheet" type="text/css" href="../Common/IntlWeb.css" />
	<script type="text/javascript" src="../Common/jquery-1.4.2.min.js"></script>

	<script type="text/javascript">
		function Submit_Click(BTN_Value) {
			var BoardCode = $("#BoardCode").val();
			var Header = $("#TBHeader").val();
			var Pk = $("#SelectHeader option:selected").val();
			Board.SaveNewBoardHeader(Pk, BoardCode, Header, function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				}
				else {
					alert(result);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function SelectHeaderChange(value) {
			if (value == "N") {
				$("#BTN_Submit").val("Add");
				$("#TBHeader").val("");
				$("#Pn_Delete").attr("style", "visibility:hidden");
			}
			else {
				$("#BTN_Submit").val("Modify");
				$("#TBHeader").val($("#SelectHeader option:selected").text());
				$("#Pn_Delete").attr("style", "visibility:visible");

			}
		}
		function HeaderUpOrDown(UporDown) {
			var BoardCode = $("#BoardCode").val();
			var Pk = $("#SelectHeader option:selected").val();
			Board.ChangeBoardHeaderOrder(UporDown, BoardCode, Pk, function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				}
				else {
					alert(result);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function Delete_Click()
		{
			var BoardCode = $("#BoardCode").val();
			var Pk = $("#SelectHeader option:selected").val();
			if (confirm("삭제하시겠습니까?")) {
				Board.DeleteBoardHeader(BoardCode, Pk, function (result) {
					if (result == "1") {
						alert("Success");
						location.reload();
					}
					else {
						alert(result);
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
	</script>
</head>
<body style="margin:0 auto; background-color:#FFFFFF; " >
    <form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Board.asmx" /></Services></asp:ScriptManager>
	    <input type="hidden" id="BoardCode" value="<%=Request.Params["C"] %>" />
        <table align="center" style="width:450px; margin-top:20px;">
            <tr><td style="height:25px; padding-left:10px;"><img src="../Images/Board/bul_org.gif" /><img src="../Images/Board/bul_org.gif" />&nbsp;<b>Board Header Setting</b></td></tr>
            <tr>
                <td style="padding-bottom:10px; ">
                    <select size="6" style="width:450px; height:200px;" id="SelectHeader" onchange="SelectHeaderChange(this.value);" ><%=OptionValue+"" %></select>
                </td>
            </tr>
            <tr>
                <td style="height:40px; padding-left:10px; background-color:#D1E3E5;" >
                    <input type="text" id="TBHeader" style="width:150px; height:18px; border:1px solid #767676; color:#000000;" />
			        <input type="button" value="Add" id="BTN_Submit" onclick="Submit_Click(this.value);" />
			        <span id="Pn_Delete" style="visibility:hidden;">
				        <input type="button" value="Delete" id="BTN_Delete" onclick="Delete_Click();"/>
				        <input type="button" value="↑ up" onclick="HeaderUpOrDown('Up');" />
				        <input type="button" value="↓ down" onclick="HeaderUpOrDown('Down');" />
			        </span>
                </td>
            </tr>
            <tr>
                <td align="center" style="padding-top:50px; padding-bottom:20px;"><a href='javascript:window.close();'><img src="../Images/Board/close.gif" border="0"></a></td>
            </tr>  
        </table>
    </form>
</body>
</html>