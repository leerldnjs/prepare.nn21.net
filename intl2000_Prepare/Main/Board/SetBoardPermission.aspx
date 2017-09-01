<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetBoardPermission.aspx.cs" Inherits="Board_SetBoardPermission" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link rel="stylesheet" type="text/css" href="../Common/IntlWeb.css" />
	<script type="text/javascript" src="../Common/jquery-1.10.2.min.js"></script>
	<script type="text/javascript">
		function PermissionAdd() {
			var BoardCode = $("#BoardCode").val();
			var mode = $("#InsertMode").val();
			var Value;
			var Permission = $("#PermissionType").val();
			if (mode == "Branch") {
				Value = $("#TargetBranch").val();
			}
			else {
				Value = $("#TargetID").val();
			}
			Board.InsertBoardPermission(BoardCode, mode, Value, Permission, function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				}
				else if (result == "0") {
					alert("Can't Save");
					return false;
				}
				else {
					alert(result);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function InsertModeChange(ModeValue)
		{
			if (ModeValue == "Branch") {
				$("#PnInsertMode1").attr("style", "visibility:visible; height:100%; ");
				$("#PnInsertMode2").attr("style", "visibility:hidden; height:0px; ");
			}
			else {
				$("#PnInsertMode1").attr("style", "visibility:hidden; height:0px; ");
				$("#PnInsertMode2").attr("style", "visibility:visible; height:100%; ");

			}
		}
		function DeleteThis(Pk) {
			Board.DeleteBoardPermission(Pk, function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				}
				else {
					alert(result);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
	</script>
</head>
<body style="margin:0 auto; background-color:#FFFFFF; overflow:auto; " >
    <form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Board.asmx" /></Services></asp:ScriptManager>
		<input type="hidden" id="BoardCode" value="<%=Request.Params["C"] %>" />

		<table align="center" style="margin-top:20px;">
            <tr><td style="height:25px; padding-left:10px;"><img src="../Images/Board/bul_org.gif" /><img src="../Images/Board/bul_org.gif" />&nbsp;<b>Permission Setting</b></td></tr>
            <tr>
                <td>
			        <table border="1" align="center"  cellpadding="0" cellspacing="0" style="border-collapse:collapse;width:600px;" >
				        <tr>
					        <td style="width:240px; height:30px; background-color:#C8C8C8;" align="center"><b>지사명</b></td>
					        <td style="width:90px;background-color:#C8C8C8;" align="center"><b>직위</b></td>
					        <td style="width:100px; background-color:#C8C8C8;" align="center"><b>Name</b></td>
					        <td style="width:70px; background-color:#C8C8C8;" align="center"><b>ID</b></td>
					        <td style="width:70px; background-color:#C8C8C8;" align="center"><b>권한</b></td>
					        <td style="width:30px; background-color:#C8C8C8;" align="center"><b>Del</b></td>
				        </tr>
				        <%=TBBody %>
			        </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table style=" width:600px; height:50px; margin-top:10px; background-color:#F0F5F6;">
                        <tr>
                            <td style="width:200px;" align="right">
                                <select id="InsertMode" onchange="InsertModeChange(this.value);">
                                    <option value="Branch">지사별 추가</option>
                                    <option value="AccountID">아이디별 추가</option>
                                </select>
                            </td>
                            <td  style="width:150px;" align="center">
                                <div id="PnInsertMode1" style="visibility:visible; "><select id="TargetBranch"><option>지사선택</option><%=Components.Common.SelectOurBranch_HAHAHA %></select></div>
                                <div id="PnInsertMode2" style="visibility:hidden; height:0px;  ">ID : <input type="text" id="TargetID"  style="width:100px; height:16px; border:1px solid #767676; color:#000000;" /></div>
                            </td>
                            <td  style="width:100px;">
                                <select id="PermissionType">
                                    <option value="A">읽기 / 쓰기</option>
                                    <option value="W">쓰기</option>
                                    <option value="R">읽기</option>
                                </select>
                            </td>
                            <td style="width:150px;" align="left">
                                <input type="button" value="추가" onclick="PermissionAdd();" />
                            </td>
                        </tr> 
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" style="padding-top:50px; padding-bottom:20px;"><a href='javascript:window.close();'><img src="../Images/Board/close.gif" border="0"></a></td>
            </tr>  
        </table>
    </form>
</body>
</html>
