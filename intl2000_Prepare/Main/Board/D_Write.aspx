<%@ Page Language="C#" AutoEventWireup="true" CodeFile="D_Write.aspx.cs" Inherits="Board_D_Write" ValidateRequest="false" %>


<%@ Register Src="~/Board/BoardList.ascx" TagPrefix="uc1" TagName="BoardList" %>
<%@ Register Src="~/Board/daumeditor/Editor1.ascx" TagPrefix="uc2" TagName="Editor1" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Common/IntlWeb.css" />   
    <link href="../Lib/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../Common/jquery-1.4.2.min.js"></script>

    <link rel="stylesheet" href="daumeditor/css/editor.css" type="text/css" charset="utf-8" />
    <script src="daumeditor/js/editor_loader.js" type="text/javascript" charset="utf-8"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            
        });

        EditorJSLoader.ready(function (Editor) {
            var config = {
                txHost: '', /* 런타임 시 리소스들을 로딩할 때 필요한 부분으로, 경로가 변경되면 이 부분 수정이 필요. ex) http://localhost: http://xxx.xxx.com */
                txPath: '/Board/daumeditor/', /* 런타임 시 리소스들을 로딩할 때 필요한 부분으로, 경로가 변경되면 이 부분 수정이 필요. ex) /xxx/xxx/ */
                txService: 'sample', /* 수정필요없음. */
                txProject: 'sample', /* 수정필요없음. 프로젝트가 여러개일 경우만 수정한다. */
                initializedId: "", /* 대부분의 경우에 빈문자열 */
                wrapper: "tx_trex_container", /* 에디터를 둘러싸고 있는 레이어 이름(에디터 컨테이너) */
                form: 'tx_editor_form' + "", /* 등록하기 위한 Form 이름 */
                txIconPath: "/Board/daumeditor/images/icon/editor/", /*에디터에 사용되는 이미지 디렉터리, 필요에 따라 수정한다. */
                txDecoPath: "/Board/daumeditor/images/deco/contents/", /*본문에 사용되는 이미지 디렉터리, 서비스에서 사용할 때는 완성된 컨텐츠로 배포되기 위해 절대경로로 수정한다. */
                canvas: {
                    exitEditor: {
                        /*
                        desc:'빠져 나오시려면 shift+b를 누르세요.',
                        hotKey: {
                            shiftKey:true,
                            keyCode:66
                        },
                        nextElement: document.getElementsByTagName('button')[0]
                        */
                    },
                    styles: {
                        color: "#222", /* 기본 글자색 */
                        fontFamily: "굴림", /* 기본 글자체 */
                        fontSize: "10pt", /* 기본 글자크기 */
                        backgroundColor: "#fff", /*기본 배경색 */
                        lineHeight: "1.5", /*기본 줄간격 */
                        padding: "8px" /* 위지윅 영역의 여백 */
                    },
                    showGuideArea: false
                },
                events: {
                    preventUnload: false
                },
                sidebar: {
                    attachbox: {
                        show: true,
                        confirmForDeleteAll: true
                    }
                },
                size: {
                    //contentWidth: 700 /* 지정된 본문영역의 넓이가 있을 경우에 설정 */
                }
            };
            //var editor = new Editor(config);

            if ("<%=Mode%>" == "Modify") {
              loadContent();
            }
        });
        function validForm(editor) {
            var validator = new Trex.Validator();
            var content = editor.getContent();7
            //if (!validator.exists(content)) {
            //    alert("Content is not write");
            //    return false;
            //}
            if ($("#HTMLTitle").val() + "" == "") {
                alert("Title is not write");
                $("#Title").select();
                return false;
            }
            return true;
        }
        function saveContent() {
            Editor.save(); // 이 함수를 호출하여 글을 등록하면 된다.
        }
        function Set_E() {
            tx_editor_form.submit();
            alert("Success");
        }
        function setForm(editor) {
            var i, input;
            var form = editor.getForm();
            var content = editor.getContent();
            //홑따옴표 처리 
            content = content.replace(/'/gi, '&#39;');
            //\r\n 처리 
            content = content.replace('\r\n', '<br />');
            // 본문 내용을 필드를 생성하여 값을 할당하는 부분
            var textarea = document.createElement('textarea');
            textarea.id = 'daumeditor_content';
            textarea.name = 'content';            
            textarea.value = content;
            form.createField(textarea);
            Set_E();
        }
        
        function loadContent() {
            /* 저장된 컨텐츠를 불러오기 위한 함수 호출 */            
            var attachments = {};
            attachments['image'] = [];
            attachments['file'] = [];
            Editor.modify({
                "attachments": function () { /* 저장된 첨부가 있을 경우 배열로 넘김, 위의 부분을 수정하고 아래 부분은 수정없이 사용 */
                    var allattachments = [];
                    for (var i in attachments) {
                        allattachments = allattachments.concat(attachments[i]);
                    }
                    return allattachments;
                }(),   
                "content": '<%=content.Replace("\r\n","'+'")%>' 
            });
        }

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
            $.ajax({
                type: "POST",
                url: "/WebService/Board.asmx/DeleteFile",
                data: "{FilePk:\"" + filePk + "\"}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.d == "1") {
                        alert("성공");
                        location.reload();
                    }
                },
                error: function (msg) {
                    alert('failure : ' + msg);
                    console.log(msg);
                }
            });
        }

    </script>
</head>
<body style="margin: 0 auto; background-repeat: repeat-x; background-color: #FFFFFF;" background="../Images/Board/top_bg.gif">
    <form id="tx_editor_form" enctype="multipart/form-data" method="post" action="/Board/SetBoard.aspx" runat="server">
        <table align="center" border="0" cellpadding="0" cellspacing="0" style="width: 1200px; height: 600px;">
            <tr>
                <td colspan="2" height="108" align="center"><font style="font-family: arial; font-size: 20px; font-weight: bold; color: #FFFFFF;">International Logistic Board</font></td>
            </tr>
            <tr>
                <td valign="bottom" style="height: 50px; padding-left: 10px;">
                    <img src="../Images/Board/id.gif" align="absmiddle" />
                    <%=AccountID %>
                    <br />
                    <img src="../Images/Board/name.gif" align="absmiddle" />
                    <%=Name %>
                </td>
                <td align="right" valign="bottom">
                    <% if (CompanyPk != "10520")
                       { %>
                    <a href="../Admin/RequestList.aspx?G=5051">
                        <img src="../Images/Board/link.gif" align="absmiddle" border="0" /></a>
                    <% }
                       else
                       { %>
                    <a href="../Process/Logout.aspx">
                        <img src="../Images/Board/Logout.jpg" align="absmiddle" border="0" /></a>
                    <%} %>
                </td>
            </tr>
            <tr>
                <td valign="top" style="width: 200px; padding-top: 10px;">
                    <table align="center" cellpadding="0" cellspacing="0" style="width: 200px; border-style: dotted; border-width: 2; border-color: #C8C8C8;">
                        <tr>
                            <td align="center" style="height: 40px;"><a href="BoardMain.aspx">
                                <img src="../Images/Board/main.gif" align="absmiddle" border="0" /></a></td>
                        </tr>
                        <uc1:BoardList runat="server" ID="BoardList" />
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
                                            <asp:DropDownList ID="HTMLHeader" name="HTMLHeader" runat="server" Width="100"></asp:DropDownList>&nbsp;&nbsp;
										<asp:TextBox ID="HTMLTitle" name="HTMLTitle" runat="server" Width="400" Style="ime-mode: active;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 10px; background-repeat: repeat-x;" background="../Images/Board/dot.gif"></td>
                                    </tr>
                                    <tr>
                                        <td valign="top" style="padding-left: 20px; padding-right: 20px;"><%=AttachedFiles %></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 45px; padding-left: 20px; padding-bottom: 10px;">파일첨부 
                                            <span onclick="AddFile()">
                                                <img src="../Images/Board/add.gif" border="0" align="absmiddle" /></span>
                                            <div id="PnFile0" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File0" name="File0" runat="server" style="width: 550px;" />
                                            </div>
                                            <div id="PnFile1" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File1" name="File1" runat="server" style="width: 550px;" />
                                            </div>
                                            <div id="PnFile2" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File2" name="File2" runat="server" style="width: 550px;" />
                                            </div>
                                            <div id="PnFile3" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File3" name="File3" runat="server" style="width: 550px;" />
                                            </div>
                                            <div id="PnFile4" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File4" name="File4" runat="server" style="width: 550px;" />
                                            </div>
                                            <div id="PnFile5" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File5" name="File5" runat="server" style="width: 550px;" />
                                            </div>
                                            <div id="PnFile6" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File6" name="File6" runat="server" style="width: 550px;" />
                                            </div>
                                            <div id="PnFile7" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File7" name="File7" runat="server" style="width: 550px;" />
                                            </div>
                                            <div id="PnFile8" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File8" name="File8" runat="server" style="width: 550px;" />
                                            </div>
                                            <div id="PnFile9" style="position: absolute; visibility: hidden;">
                                                <input type="file" id="File9" name="File9" runat="server" style="width: 550px;" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc2:Editor1 runat="server" ID="Editor1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="height: 50px;">
                                            <input type="button" value="저장" onclick="saveContent();" style="width: 80px; height: 30px;" />
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
        <input type="hidden" name="FileUpload_Type" id="FileUpload_Type" value="<%=Mode%>" />
        <input type="hidden" name="AccountID" id="AccountID" value="<%=AccountID%>" />
        <input type="hidden" name="Name" id="Name" value="<%=Name%>" />
        <input type="hidden" name="BoardCode" id="BoardCode" value="<%=BoardCode%>" />
        <input type="hidden" name="Pk" id="Pk" value="<%=Pk%>" />
        <input type="hidden" id="H_content" name="H_content" value="<%=content%>" />
    </form>
</body>
</html>
