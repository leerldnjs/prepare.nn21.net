<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetBoardCode.aspx.cs" Inherits="Board_SetBoardCode" Buffer="False" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link rel="stylesheet" type="text/css" href="../Common/IntlWeb.css" />
	<script type="text/javascript" src="../Common/jquery-1.4.2.min.js"></script>
	<script type="text/javascript">
		//$(document).ready(function () {
		//	alert("123");
		//});
		function AddNew(Type) {
			var Code, Title, Mode;
			if (Type == "Category") {
				Code = $("#NewCategoryCode").val();
				Title = $("#NewCategoryTitle").val();
				if ($("#CategoryAdd").val() == "카테고리 수정") {
					Mode = "Modify";
				}
				else {
					Mode = "Add";
				}
				if (Code.length < 2 || Title == "") {
					alert("필수항목을 채워넣치 않았습니다.");
					return false;
				}
			}
			else {
				Code = $("#NewBoardCodeFirst2").val() + $("#NewBoardCodeLast3").val();
				Title = $("#NewBoardTitle").val();
				if ($("#BoardAdd").val() == "게시판 수정") {
					Mode = "Modify";
				}
				else {
					Mode = "Add";
				}
				if (Code.length != 5 || Title == "") {
					alert("필수항목을 채워넣치 않았습니다.");
					return false;
				}
			}
			Board.SaveNewBoardCode(Mode, Code, Title, function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				}
				else if (result == "0") {
					alert(Code + "는 이미 사용중입니다");
					return false;
				}
				else {
					alert(result);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function ChageOrder(type, boardcode) {
			Board.ChangeBoardCodeOrder(type, boardcode, function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				}
				else if (result == "0") {
					alert("변경이 불가능합니다.");
					return false;
				}
				else {
					alert(result);
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function PopOpen(type, boardcode) {
			window.open("SetBoard" + type + ".aspx?C=" + boardcode, "", 'Width=700px, Height=700px, resizable=0, status=0, scrollbars=1, help=0');
		}
		function DeleteBoard(mode, boardcode) {
			if (confirm("삭제하시겠습니까?")) {
				Board.DeleteBoard(mode, boardcode, function (result) {
					if (result == "1") {
						alert("Success");
						location.reload();
					}
					else if (result == "0") {
						alert("삭제할수 없습니다");
						return false;
					}
					else {
						alert(result);
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function SetModify(mode, boardcode, boardtitle) {
			if (mode == "MainCode") {
				$("#NewCategoryCode").val(boardcode);
				document.getElementById("NewCategoryCode").disabled = "disabled";
				$("#NewCategoryTitle").val(boardtitle);
				$("#CategoryAdd").val("카테고리 수정");
			}
			else {
				$("#NewBoardTitle").val(boardtitle);
				$("#NewBoardCodeFirst2").val(boardcode.substr(0, 2));
				$("#NewBoardCodeLast3").val(boardcode.substr(2, 3));
				document.getElementById("NewBoardCodeFirst2").disabled = "disabled";
				document.getElementById("NewBoardCodeLast3").disabled = "disabled";
				$("#BoardAdd").val("게시판 수정");
			}
			//alert(mode + " " + boardcode + " " + boardtitle);
		}
	</script>
</head>
<body style="margin: 0 auto; background-repeat: repeat-x; background-color: #FFFFFF;" background="../Images/Board/top_bg.gif">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Board.asmx" />
			</Services>
		</asp:ScriptManager>
		<table width="900" align="center" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td colspan="2" height="108" align="center"><font style="font-family: arial; font-size: 20px; font-weight: bold; color: #FFFFFF;">International Logistic Board</font></td>
			</tr>
			<tr>
				<td colspan="2" align="right" valign="middle" style="height: 50px;">
					<a href="BoardMain.aspx">
						<img src="../Images/Board/link_board.gif" align="absmiddle" border="0"></a>&nbsp;&nbsp;
                    <a href="../Admin/RequestList.aspx?G=5051">
						<img src="../Images/Board/link.gif" align="absmiddle" border="0"></a>
				</td>
			</tr>
			<tr>
				<td valign="top" style="width: 500px;">
					<table style="width: 500px; margin-top: 20px;">
						<tr>
							<td style="height: 25px; padding-left: 10px;">
								<img src="../Images/Board/bul_org.gif" /><img src="../Images/Board/bul_org.gif" />&nbsp;<b>Board Management</b></td>
						</tr>
						<tr>
							<td><%=BoardList %></td>
						</tr>
					</table>
				</td>
				<td valign="top" style="width: 400px;">
					<table style="width: 400px; margin-top: 20px;">
						<tr>
							<td style="height: 25px; padding-left: 10px;">
								<img src="../Images/Board/bul_org.gif" /><img src="../Images/Board/bul_org.gif" />&nbsp;<b>Category Setting</b></td>
						</tr>
						<tr>
							<td>
								<table border="1" align="center" cellpadding="0" cellspacing="0" style="border-collapse: collapse; width: 400px; margin-bottom: 10px;">
									<tr>
										<td colspan="2" align="center" style="background-color: #708090; width: 400px; height: 35px;"><font color="#FFFFFF"><b>카테고리</b></font></td>
									</tr>
									<tr>
										<td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">제목</td>
										<td style="width: 300px; height: 30px; padding-left: 10px;">
											<input type="text" id="NewCategoryTitle" style="width: 250px; height: 18px; border: 1px solid #767676; color: #000000;" /></td>
									</tr>
									<tr>
										<td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">코드</td>
										<td style="width: 300px; height: 30px; padding-left: 10px;">
											<input type="text" id="NewCategoryCode" maxlength="2" style="width: 80px; height: 18px; border: 1px solid #767676; color: #000000;" /></td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td align="right" valign="top" style="width: 400px; height: 40px;">
								<input type="button" id="CategoryAdd" value="카테고리 추가" onclick="AddNew('Category');" style="width: 120px; height: 30px;" />
							</td>
						</tr>
						<tr>
							<td style="height: 25px; padding-left: 10px;">
								<img src="../Images/Board/bul_org.gif" /><img src="../Images/Board/bul_org.gif" />&nbsp;<b>Board Setting</b></td>
						</tr>
						<tr>
							<td>
								<table border="1" align="center" cellpadding="0" cellspacing="0" style="border-collapse: collapse; width: 400px; margin-bottom: 10px;">
									<tr>
										<td colspan="2" align="center" style="background-color: #708090; width: 400px; height: 35px;"><font color="#FFFFFF"><b>게시판</b></font></td>
									</tr>
									<tr>
										<td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">제목</td>
										<td style="width: 300px; height: 30px; padding-left: 10px;">
											<input type="text" id="NewBoardTitle" style="width: 250px; height: 18px; border: 1px solid #767676; color: #000000;" /></td>
									</tr>
									<tr>
										<td align="center" style="width: 100px; height: 30px; background-color: #F0F5F6;">코드</td>
										<td style="width: 300px; height: 30px; padding-left: 10px;">
											<select id="NewBoardCodeFirst2">
												<option value="0">Category</option>
												<%=CategoryOption+"" %></select>&nbsp;
                                        <input type="text" id="NewBoardCodeLast3" maxlength="3" style="width: 80px; height: 18px; border: 1px solid #767676; color: #000000;" />
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td align="right" valign="top" style="width: 400px; height: 40px;">
								<input type="button" id="BoardAdd" value="게시판 추가" onclick="AddNew('Board');" style="width: 120px; height: 30px;" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</form>
</body>
</html>
