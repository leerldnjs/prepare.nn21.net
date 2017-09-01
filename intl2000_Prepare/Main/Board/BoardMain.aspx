<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BoardMain.aspx.cs" Inherits="Board_BoardMain" %>

<%@ Register Src="BoardList.ascx" TagName="BoardList" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Common/IntlWeb.css" />
    <script type="text/javascript" src="../Common/jquery-1.4.2.min.js"></script>
    <script type="text/javascript">
        function GoView(boardcode, pk) {
            location.href = "C_View.aspx?C=" + boardcode + "&P=" + pk;
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

        <table align="center" valign="top" border="0" cellpadding="0" cellspacing="0" style="width: 1200px; height: 600px;">
            <tr>
                <td colspan="2" height="108" align="center"><font style="font-family: arial; font-size: 20px; font-weight: bold; color: #FFFFFF;">International Logistic Board</font></td>
            </tr>
            <tr>
                <td valign="bottom" style="height: 50px; padding-left: 10px;">
                    <img src="../Images/Board/id.gif" align="absmiddle">
                    <%=AccountID %><br />
                    <img src="../Images/Board/name.gif" align="absmiddle">
                    <%=Name %>
                </td>
                <td align="right" valign="bottom">
                    <% if (AccountID == "ilogistics" || AccountID == "ilman" || AccountID == "ilic30" || AccountID == "ilic31")
                       { %>
                    <a href="SetBoardCode.aspx">
                        <img src="../Images/Board/link_setting.gif" align="absmiddle" border="0"></a>&nbsp;&nbsp;
            <% } %>
                    <% if (CompanyPk != "10520")
                       { %>
                    <a href="../Admin/RequestList.aspx?G=5051">
                        <img src="../Images/Board/link.gif" align="absmiddle" border="0"></a>
                    <% }
                       else
                       { %>
                    <a href="../Process/Logout.aspx">
                        <img src="../Images/Board/Logout.jpg" align="absmiddle" border="0"></a>
                    <%} %>
                </td>

                
            </tr>
            <tr>
                <td valign="top" style="width: 200px; padding-top: 10px;">
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 200px; border-style: dotted; border-width: 2; border-color: #C8C8C8;">
                        <tr>
                            <td align="center" style="height: 40px;"><a href="BoardMain.aspx">
                                <img src="../Images/Board/main.gif" align="absmiddle" border="0"></a></td>
                        </tr>
                        <uc1:BoardList ID="BoardList1" runat="server" />
                        <tr>
                            <td style="height: 20px;">&nbsp;</td>
                        </tr>
                    </table>
                </td>
                <td valign="top" style="width: 1000px; padding-top: 10px;">
                    <table cellpadding="0" cellspacing="0" style="width: 990px; margin-left: 10px;">
                        <%=BoardContentsList %>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" width="1200">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 1200px; margin-top: 50px;">
                        <tr>
                            <td align="center" style="background-color: #EDEBEB; height: 50px;">Copyright(c) 2012 International Logistics CO.,LTD &nbsp;&nbsp;&nbsp;&nbsp;    All right reserved.</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
