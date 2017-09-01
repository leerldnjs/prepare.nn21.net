<%@ Page Language="C#" AutoEventWireup="true" CodeFile="file.aspx.cs" Inherits="Board_daumeditor_pages_trex_file" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../js/popup.js" type="text/javascript" charset="utf-8"></script>
    <link rel="stylesheet" href="../../css/popup.css" type="text/css" charset="utf-8" />
    <script type="text/javascript">
        // <![CDATA[

        function done() {
            if (typeof (execAttach) == 'undefined') { //Virtual Function
                return;
            }

            var _mockdata = {
                'attachurl': document.getElementById("SavedFile").value,
                'filename': document.getElementById("FileName").value,
                'filesize': document.getElementById("FileSize").value,
                'originalurl': document.getElementById("SavedFile").value,
                'thumburl': document.getElementById("SavedFile").value
            };
            execAttach(_mockdata);
            closeWindow();
        }

        function initUploader() {
            var _opener = PopupUtil.getOpener();
            if (!_opener) {
                alert('잘못된 경로로 접근하셨습니다.');
                return;
            }

			<% if (files == null)
      { %>
	        document.getElementById("opnerinfo").value = _opener.location;
			<% } %>

	        var _attacher = getAttacher('file', _opener);
	        registerAction(_attacher);
	    }

    </script>
</head>
<body onload="initUploader();">
    <div class="wrapper">
        <% if (files == null)
           { %>
        <form name="file_upload" id="file_upload" method="post" enctype="multipart/form-data" accept-charset="utf-8">
            <div class="header">
                <h1>파일 첨부</h1>
            </div>
            <div class="body">
                <dl class="alert">
                    <dt>파일 첨부 확인</dt>
                    <dd>
                        <input type="file" name="UploadFile" />
                    </dd>
                </dl>
            </div>
            <div class="footer">
                <p><a href="#" onclick="closeWindow();" title="닫기" class="close">닫기</a></p>
                <ul>
                    <li class="submit"><a href="#" onclick="file_upload.submit();" title="등록" class="btnlink">등록</a></li>
                    <li class="cancel"><a href="#" onclick="closeWindow();" title="취소" class="btnlink">취소</a></li>
                </ul>
                <input type="hidden" id="opnerinfo" name="opnerinfo" />
            </div>

        </form>

        <% }
           else
           { %>
        <div class="header">
            <h1>파일 첨부</h1>
        </div>
        <div class="body">
            <dl class="alert">
                <dt>파일 첨부 확인</dt>
                <dd>
                    <input type="hidden" id="SavedFile" value="<%="/UploadedFiles/Temp/"+yyyyMM+"/" + TempFileFullName%>" />
                    <input type="hidden" id="FileName" value="<%=TempFileFullName %>" />
                    <%=TempFileFullName%>
                    <input type="hidden" id="FileSize" value="<%=FileSize%>" />
                </dd>
            </dl>
        </div>
        <div class="footer">
            <p><a href="#" onclick="closeWindow();" title="닫기" class="close">닫기</a></p>
            <ul>
                <li class="submit"><a href="#" onclick="done();" title="등록" class="btnlink">등록</a> </li>
                <li class="cancel"><a href="#" onclick="closeWindow();" title="취소" class="btnlink">취소</a></li>
            </ul>
        </div>
        <% } %>
    </div>
</body>

</html>
