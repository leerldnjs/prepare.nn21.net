<%@ Page Language="C#" AutoEventWireup="true" CodeFile="C_View.aspx.cs" Inherits="Board_C_View" %>

<%@ Register src="BoardList.ascx" tagname="BoardList" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link rel="stylesheet" type="text/css" href="../Common/IntlWeb.css" />
	<script type="text/javascript" src="../Common/jquery-1.4.2.min.js"></script>
	<script type="text/javascript">
		function SaveReply(commentID, position) {
			Board.SaveComment(form1.ContentsPk.value, position,$("#" + commentID).val(), form1.AccountID.value, form1.Name.value, function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				}
				else {
					alert(result);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function ModifyReply(commentID, CommentPk) {
			Board.ModifyComment(CommentPk, $("#" + commentID).val(), function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				}
				else {
					alert(result);
				}
			}, function (result) { alert("ERROR : " + result); });
		}

		function OpenComment(position, id) {
			if ($("#" + id).html() == "") {
			    $("#" + id).html("<table bgcolor=\"#F0F5F6\" style=\"width:940px;\"><tr><td align=\"center\" style=\"width:700px; height:90px; padding-left:10px;\"><textarea rows=\"4\" cols=\"125\" id=\"Comment_" + position + "\"></textarea></td>" +
                                        "<td align=\"left\" ><input type=\"button\" style=\"width:60px; height:50px; \" value=\"Reply\" onclick=\"SaveReply('Comment_" + position + "', '" + position + "');\" /></td></tr></table>");
			}
		}
		function OpenCommentForModify(position, id, Pk, Value) {
			Value = Value.replace(/<br \/>/g, "\r\n");
			$("#" + id).html("<table bgcolor=\"#F0F5F6\" style=\"width:940px;\"><tr><td align=\"center\" style=\"width:700px; height:90px; padding-left:10px;\">"+
				"<textarea rows=\"4\" cols=\"125\" id=\"Comment_" + position + "\">"+Value+"</textarea></td>" +
									"<td align=\"left\" ><input type=\"button\" style=\"width:60px; height:50px; \" value=\"Modify\" onclick=\"ModifyReply('Comment_" + position + "', '"+Pk+"');\" /></td></tr></table>");
		}
		function Goto(which) {
			switch (which) {
				case "list":
					location.href = "C_List.aspx?C=" + form1.BoardCode.value;
					break;
			    case "Modify":
			        //location.href = "C_Write.aspx?C=" + form1.BoardCode.value + "&M=Modify&P=" + form1.ContentsPk.value;
			        location.href = "D_Write.aspx?C=" + form1.BoardCode.value + "&M=Modify&P=" + form1.ContentsPk.value;
					break;
			    case "Reply":
			        //location.href = "C_Write.aspx?C=" + form1.BoardCode.value + "&M=Reply&P=" + form1.ContentsPk.value;
					location.href = "D_Write.aspx?C=" + form1.BoardCode.value + "&M=Reply&P=" + form1.ContentsPk.value;
					break;
				case "Delete":
					if (confirm("삭제하시겠습니까?")) {
						Board.DeleteContents(form1.ContentsPk.value, form1.AccountID.value, function (result) {
							if (result == "1") {
								alert("Success");
								location.href = "C_List.aspx?C=" + form1.BoardCode.value;
							}
							else {
								alert(result);
							}
						}, function (result) { alert("ERROR : " + result); });
					}
					break;
				default:
					Board.SerchUpperOrUnderPk(form1.ContentsPk.value, which, function (result) {
						if (result == "") {
							alert("자료가 없습니다. ");
							return false;
						}
						else {
							location.href = "C_View.aspx?C=" + form1.BoardCode.value + "&P=" + result;
						}
					}, function (result) { alert("ERROR : " + result); });
			}
		}
		function GoView(boardcode, pk) {
			location.href = "C_View.aspx?C=" + boardcode + "&P=" + pk;
		}
		function DeleteComment(pk) {
		    if (confirm("삭제하시겠습니까?")) {
		        Board.DeleteComment(pk, function (result) {
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
		function MoveBoardCode() {
			Board.MoveBoardCode(form1.ContentsPk.value, $("#BoardCodeForMove").val(), function (result) {
				if (result == "1") {
					alert("Success");
					location.href = "C_View.aspx?C=" + $("#BoardCodeForMove").val() + "&P=" + form1.ContentsPk.value;
				}
				else {
					alert(result);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
	</script>
</head>
<body style="margin:0 auto; background-repeat:repeat-x; background-color:#FFFFFF; "  background="../Images/Board/top_bg.gif">
    <form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Board.asmx" /></Services></asp:ScriptManager>
		<input type="hidden" id="ContentsPk" value="<%=Request.Params["P"] %>" />
		<input type="hidden" id="AccountID" value="<%=AccountID %>" />
		<input type="hidden" id="Name" value="<%=Name %>" />

		<input type="hidden" id="BoardCode" value="<%=Request.Params["C"] %>" />
        <table align="center"  border="0" cellpadding="0" cellspacing="0" style=" width:1200px; height:600px;" >
            <tr> 
                <td colspan="2" height="108" align="center"><font style="font-family:arial; font-size:20px;font-weight:bold; color:#FFFFFF;">International Logistic Board</font></td>
            </tr>
            <tr>
                <td valign="bottom" style="height:50px; padding-left:10px;">
                    <img src="../Images/Board/id.gif" align="absmiddle"> <%=AccountID %> <br /><img src="../Images/Board/name.gif" align="absmiddle"> <%=Name %>
                </td>
                <td align="right" valign="bottom" >
                    <% if (CompanyPk != "10520")
                       { %>
                    <a href="../Admin/RequestList.aspx?G=5051"><img src="../Images/Board/link.gif" align="absmiddle" border="0"></a>
                    <% }else{ %>
                    <a href="../Process/Logout.aspx">
                        <img src="../Images/Board/Logout.jpg" align="absmiddle" border="0"></a>
                    <%} %>
                </td>
            </tr>
            <tr>
                <td valign="top" style="width:200px; padding-top:10px;">
                    <table  align="center" cellpadding="0" cellspacing="0" style="width:200px; border-style:dotted ; border-width:2; border-color:#C8C8C8;">
                        <tr><td align="center" style="height:40px;"><a href="BoardMain.aspx"><img src="../Images/Board/main.gif" align="absmiddle" border="0"></a></td></tr>
                        <uc1:BoardList ID="BoardList1" runat="server" />
                        <tr><td style="height:20px;">&nbsp;</td></tr>
                    </table>
                </td>
                <td valign="top" style="width:1000px;padding-top:10px;">
			        <table cellpadding="0" cellspacing="0" style="width:990px; margin-left:10px;" >
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" style="width:990px; border-collapse:collapse;">
                                    <tr><td colspan="2" style="width:990px; height:3px;  background-color:#F6F6F6;"></td></tr>
				                    <tr>
                                        <td style="width:700px; height:50px; padding-left:10px;"><b><%=Title %></b> || <%=BoardTitle %></td>
                                        <td style="width:290px;" align="center"><span onclick="Goto('list');">목록</span> | <span onclick="Goto('upper');">▲ 윗글</span> | <span onclick="Goto('under');">▼ 아랫글</span></td>
                                    </tr>
                                    <tr><td colspan="2" style="width:990px; height:10px; background-repeat:repeat-x; " background="../Images/Board/dot.gif"></td></tr>
                                    <tr><td colspan="2" style="height:20px; padding-left:20px;"><font color="#FF0000"><%=Writer %></font> | 조회: <%=ReadCount %> | <%=RegisterdDate %></td></tr>
                                    <tr><td colspan="2" valign="top" style="height:25px; padding-left:20px;  padding-right:20px; "><%=AttachedFiles %></td></tr>
                                    <tr><td colspan="2" valign="top" style="height:100px; padding-left:20px;  padding-right:20px; padding-top:20px; padding-bottom:10px;"><%=Contents %></td></tr>
                                    <tr><%=Reply %></tr>
                                    <tr>
                                        <td colspan="2">
                                            <table align="center" border="1" cellpadding="0" cellspacing="0" style="width:970px; border-collapse:collapse; margin-top:20px; margin-bottom:20px;">
				                                <tr>
					                                <td align="center" style="width:50px; height:30px; background-color:#D1E3E5; ">No</td>
					                                <td align="center" style="width:80px; background-color:#D1E3E5;">Header</td>
					                                <td align="center" style="width:550px; background-color:#D1E3E5;" >Title</td>
					                                <td align="center" style="width:100px; background-color:#D1E3E5;">ID</td>
					                                <td align="center" style="width:150px; background-color:#D1E3E5;">Registerd</td>
					                                <td align="center" style="width:50px; background-color:#D1E3E5;">C</td>
				                                </tr>
                                                <%=BoardContentsList %>
			                                </table>
                                        </td>
                                    </tr>
                                    <tr><td colspan="2" style="height:5px; background-repeat:repeat-x; " background="../Images/Board/dot.gif"></td></tr>
                                    <tr>
                                        <td style="width:700px; padding-left:10px; height:30px; ">
                                            <%=BTNModify %>
				                            <%=BTNDelete %>
				                            <%=BTNReply %>
                                        </td>
                                        <td style="width:290px;" align="center"><span onclick="Goto('list');">목록</span> | <span onclick="Goto('upper');">▲ 윗글</span> | <span onclick="Goto('under');">▼ 아랫글</span></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td> 
            </tr>
            <tr>
		        <td colspan="2" width="1200" >
			        <table border="0" cellpadding="0" cellspacing="0" style="width:1200px; margin-top:50px;" >
                        <tr>
                            <td align="center" style="background-color:#EDEBEB; height:50px;"> Copyright(c) 2012 International Logistics CO.,LTD &nbsp;&nbsp;&nbsp;&nbsp;    All right reserved.</td>
                        </tr>
                    </table>
                </td>
	        </tr>
        </table>
    </form>
</body>
</html>



