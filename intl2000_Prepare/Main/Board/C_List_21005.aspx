<%@ Page Language="C#" AutoEventWireup="true" CodeFile="C_List.aspx.cs" Inherits="Board_C_List" %>

<%@ Register Src="BoardList.ascx" TagName="BoardList" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
	<link rel="stylesheet" type="text/css" href="../Common/IntlWeb.css" />
</head>
<body style="margin: 0 auto; background-repeat: repeat-x; background-color: #FFFFFF;" background="../Images/Board/top_bg.gif">
	<form id="form2" runat="server">
		<asp:ScriptManager ID="SM" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Board.asmx" />
			</Services>
		</asp:ScriptManager>
		<table align="center" border="0" cellpadding="0" cellspacing="0" style="width: 1200px; height: 600px;">
			<tr>
				<td colspan="2" height="108" align="center"><font style="font-family: arial; font-size: 20px; font-weight: bold; color: #FFFFFF;">International Logistic Board</font></td>
			</tr>
			<tr>
				<td valign="bottom" style="height: 50px; padding-left: 10px;">
					<input type="hidden" id="HBoardCode" value="<%=Request.Params["C"] + "" %>" />
					<img src="../Images/Board/id.gif" align="absmiddle" />
					<%=AccountID %>
					<br />
					<img src="../Images/Board/name.gif" align="absmiddle" >
					<%=Name %>
				</td>
				<td align="right" valign="bottom">
					<% if (CompanyPk != "10520") { %>
						<a href="../Admin/RequestList.aspx?G=5051">
							<img src="../Images/Board/link.gif" align="absmiddle" border="0"></a>
					<% } else { %>
						<a href="../Process/Logout.aspx">
							<img src="../Images/Board/Logout.jpg" align="absmiddle" border="0"></a>
					<% } %>
				</td>
			</tr>
			<tr>
				<td valign="top" style="width: 200px; padding-top: 10px;">
					<table align="center" cellpadding="0" cellspacing="0" style="width: 200px; border-style: dotted; border-width: 2; border-color: #C8C8C8;">
						<tr>
							<td align="center" style="height: 40px;"><a href="BoardMain.aspx">
								<img src="../Images/Board/main.gif" align="absmiddle" border="0"></a>
							</td>
						</tr>
						<uc1:BoardList ID="BoardList1" runat="server" />
						<tr>
							<td style="height: 20px;">&nbsp;</td>
						</tr>
					</table>
				</td>
				<td valign="top" style="width: 1000px; padding-top: 10px;">
					<table cellpadding="0" cellspacing="0" style="width: 990px; margin-left: 10px;">
						<tr>
							<td align="left" style="width: 990px; height: 40px; padding-left: 20px;">
								<img src="../Images/Board/bar.gif" align="absmiddle">&nbsp;
								<font style="font-family: Vernada; font-size: 16px; font-weight: bold;"><%=BoardTitle %></font>
							</td>
						</tr>
						<tr>
							<td>
								<table border="1" style="width: 590px; border-collapse: collapse;">
									<tr>
										<td style="height:25px; width:50px;  text-align:center; background-color: #D1E3E5;">구분</td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="운송구분" /></td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="운송방법" /></td>
									</tr>
								</table>
								<table border="1" style="width: 590px; border-collapse: collapse;">
									<tr>
										<td style="height:25px; width:50px;  text-align:center; background-color: #D1E3E5;">출발</td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="대륙" /></td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="국가" /></td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="지역" /></td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="지점" /></td>
									</tr>
								</table>
								<table border="1" style="width: 590px; border-collapse: collapse;">
									<tr>
										<td style="height:25px; width:50px;  text-align:center; background-color: #D1E3E5;">도착</td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="대륙" /></td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="국가" /></td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="지역" /></td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="지점" /></td>
									</tr>
								</table>
								<table border="1" style="width: 990px; border-collapse: collapse;">
									<tr>
										<td style="height:25px; width:50px;  text-align:center; background-color: #D1E3E5;">일정</td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="선사" /></td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="구분" /></td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="마감일" /></td>

										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="출항일" /></td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="운항일" /></td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="도착일" /></td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="컨테이너 규격" /></td>
										<td style="background-color: #D1E3E5;"><input type="text" id="Area" style="width:90%; " placeholder="컨테이너 사이즈" /></td>
									</tr>
								</table>

								<table border="1" cellpadding="0" cellspacing="0" style="width: 990px; border-collapse: collapse;">
									<%=BoardContentsList %>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
                <td colspan="2" width="1200">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 1200px; margin-top: 20px;">
                        <tr>
                            <td align="center" style="background-color: #EDEBEB; height: 50px;">Copyright(c) 2012 International Logistics CO.,LTD &nbsp;&nbsp;&nbsp;&nbsp;    All right reserved.</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>

	<button id="create">Create new user</button>
	<div id="dialog" title="Download complete">
		<p>
			<span class="ui-icon ui-icon-circle-check" style="float:left; margin:0 7px 50px 0;"></span>
			Your files have downloaded successfully into the My Downloads folder.
		</p>
		<p>
			Currently using <b>36% of your storage space</b>.
		</p>
	</div>


	<script src="https://code.jquery.com/jquery-1.12.4.js"></script>
	<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
	<script type="text/javascript">
		$(function () {
			$("#dialog").dialog({
				autoOpen: false
			});
			$("#create").on("click", function () {
				$("#dialog").dialog("open");
			});
		});
		
		function BTN_Write(boardcode) {
			//location.href = "C_Write.aspx?C=" + boardcode;
			location.href = "D_Write.aspx?C=" + boardcode;
		}
        function GoView(boardcode, pk) {
            location.href = "C_View.aspx?C=" + boardcode + "&P=" + pk;
        }
        function GoSerch() {
        	var value = $("#SerchValue").val();
        	var type = 'All';
        	if (value == "") {
        		return false;
        	}
        	else {
        		location.href = "C_List.aspx?C=" + form2.HBoardCode.value + "&SerchValue=" + escape(value) + "&SerchType=" + escape(type);
        	}
        }
    </script>
</body>
</html>
