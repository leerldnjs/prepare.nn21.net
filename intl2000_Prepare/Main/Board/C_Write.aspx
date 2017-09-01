<%@ Page Language="C#" AutoEventWireup="true" CodeFile="C_Write.aspx.cs" Inherits="Board_C_Write" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="BoardList.ascx" TagName="BoardList" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Common/IntlWeb.css" />
    <meta http-equiv="X-UA-Compatible" content="IE=9">
    <script type="text/javascript" src="../Common/jquery-1.4.2.min.js"></script>
    <script type="text/javascript">
        var FileCount = 0;
        function AddFile() {
            if (FileCount == 10) {
                alert("한 게시물에는 10개 미만의 파일만 추가할수 있습니다.");
                return false;
            }
            $("#PnFile" + FileCount).attr("style", "visibility:visible; margin-top:10px; ");
            FileCount++;
        }
        function FileDelete(filePk) {
            Board.DeleteFile(filePk, function (result) {
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
<body style="margin: 0 auto; background-repeat: repeat-x; background-color: #FFFFFF;" background="../Images/Board/top_bg.gif">
    <form id="form1" runat="server">
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
                    <img src="../Images/Board/id.gif" align="absmiddle"/>
                    <%=AccountID %>
                    <br />
                    <img src="../Images/Board/name.gif" align="absmiddle"/>
                    <%=Name %>
                </td>
                <td align="right" valign="bottom">
                    <% if (CompanyPk != "10520")
                       { %>
                    <a href="../Admin/RequestList.aspx?G=5051">
                        <img src="../Images/Board/link.gif" align="absmiddle" border="0"/></a>
                    <% }else{ %>
                    <a href="../Process/Logout.aspx">
                        <img src="../Images/Board/Logout.jpg" align="absmiddle" border="0"/></a>
                    <%} %>
                </td>
            </tr>
            <tr>
                <td valign="top" style="width: 200px; padding-top: 10px;">
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 200px; border-style: dotted; border-width: 2; border-color: #C8C8C8;">
                        <tr>
                            <td align="center" style="height: 40px;"><a href="BoardMain.aspx">
                                <img src="../Images/Board/main.gif" align="absmiddle" border="0"/></a></td>
                        </tr>
                        <uc1:BoardList ID="BoardList2" runat="server" />
                        <tr>
                            <td style="height: 20px;">&nbsp;</td>
                        </tr>
                    </table>
                </td>
                <td valign="top" style="width: 1000px; padding-top: 10px;">
                    <table cellpadding="0" cellspacing="0" style="width: 990px; margin-left: 10px;">
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 990px; border-collapse: collapse;">
                                    <tr>
                                        <td style="height: 3px; background-color: #F6F6F6;"></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 50px; padding-left: 20px;">제목 :
                                            <asp:DropDownList ID="HTMLHeader" runat="server" Width="100"></asp:DropDownList>&nbsp;&nbsp;
										<asp:TextBox ID="HTMLTitle" runat="server" Width="400" Style="ime-mode: active;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px; background-repeat: repeat-x;" background="../Images/Board/dot.gif"></td>
                                    </tr>
                                    <tr>
                                        <td valign="top" style="padding-left: 20px; padding-right: 20px;"><%=AttachedFiles %></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 45px; padding-left: 20px; padding-bottom: 10px;">파일첨부 <span onclick="AddFile()">
                                            <img src="../Images/Board/add.gif" border="0" align="absmiddle" /></span>
                                            <div id="PnFile0" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File0" runat="server" style="width: 550px;" /></div>
                                            <div id="PnFile1" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File1" runat="server" style="width: 550px;" /></div>
                                            <div id="PnFile2" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File2" runat="server" style="width: 550px;" /></div>
                                            <div id="PnFile3" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File3" runat="server" style="width: 550px;" /></div>
                                            <div id="PnFile4" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File4" runat="server" style="width: 550px;" /></div>
                                            <div id="PnFile5" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File5" runat="server" style="width: 550px;" /></div>
                                            <div id="PnFile6" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File6" runat="server" style="width: 550px;" /></div>
                                            <div id="PnFile7" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File7" runat="server" style="width: 550px;" /></div>
                                            <div id="PnFile8" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File8" runat="server" style="width: 550px;" /></div>
                                            <div id="PnFile9" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File9" runat="server" style="width: 550px;" /></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <CKEditor:CKEditorControl ID="CKEditor1" runat="server"></CKEditor:CKEditorControl>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="height: 50px;">
                                            <asp:Button ID="Button1" runat="server" Text="저장" OnClick="BTN_Save" Width="80" Height="30" />
                                            <input type="button" value="취소" onclick="javascript: history.back(-1);" style="width: 80px; height: 30px;" />
                                        </td>
                                    </tr>
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
</body>
</html>
