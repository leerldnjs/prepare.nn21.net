<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="FileList.aspx.cs" Inherits="UploadedFiles_FileList" %>

<%@ Register Src="../Admin/LogedWithoutRecentRequest.ascx" TagName="LogedWithoutRecentRequest" TagPrefix="uc1" %>
<%@ Register Src="../Member/LogedTopMenu.ascx" TagName="Loged" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function ImgDown(pk) {
            location.href = "../UploadedFiles/FileDownload.aspx?S=" + pk + "&A=" + form1.HAccountID.value;
        }
        function ImgCommentAdd(pk) {
            window.open('./CommentWithFile.aspx?G=4&S=' + pk + "&A=" + form1.HAccountID.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;');
        }
        function ImgDelete(Pk) {
            if (confirm("Will be Deleted. Right?")) {
                Admin.FileDelete(Pk, "", function (result) {
                    if (result == "1") {
                        alert("SUCCESS");
                        location.reload();
                    }
                }, function (result) { alert("ERROR : " + result); });
            }
        }
        function CommentDelete(pk) {
            if (confirm("this comment will DELETED")) {
                Admin.DeleteComment("F", pk, function (result) {
                    if (result == "1") {
                        alert("SUCCESS");
                        location.reload();
                    }
                }, function (result) { alert("ERROR : " + result); });
            }
        }
        function BTN_Serch_Click() {
            location.href = "./FileList.aspx?G=" + form1.G.value + "&T=CompanyCode&V=" + form1.TBSerchValue.value + "";
        }
        function SendEmail(FilePk) {
            var Email = document.getElementById("TBEmail[" + FilePk + "]").value;
            alert(Email);
            return false;
            Admin.EmailSend("korea@nn21.com", "국제물류 아이엘", document.getElementById("TBEmail").value, "", "국제물류 아이엘 || 무역송금 결과", PrintArea.innerHTML, function (result) {
                if (result == "1") {
                    alert("OK");
                }
            }, function (result) { alert("ERROR : " + result); });
        }
        function SendMailByCafe24_Document(MemberInfo, EmailPhysicalPath, Companypk, RelationPk) {
        	var FromEmail = "";
        	var FromName = "";
        	if (MemberInfo + "" == "3157") {
        		FromEmail = "korea@nn21.com";
        		FromName = "(주)국제종합물류";
        	} else {
        		FromEmail = "ilsd@nn21.com";
        		FromName = "International Logistics CO.,LTD";
        	}
        	Admin.SendMailByCafe24_Document(FromEmail, FromName, EmailPhysicalPath,Companypk,RelationPk, function (result) {
        		if (result == "1") {
        			alert("OK");
        		} else if (result == "0") {
        			alert("지정된 수입서류담당자가 없습니다");
        		}
        	}, function (result) { alert("ERROR : " + result); });
        }
    </script>
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 10px;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="SM" runat="server">
            <Services>
                <asp:ServiceReference Path="~/WebService/Admin.asmx" />
            </Services>
        </asp:ScriptManager>
        <div>
            <uc1:LogedWithoutRecentRequest ID="LogedWithoutRecentRequest1" runat="server" />
            <uc2:Loged ID="Loged1" runat="server" Visible="false" />
            <div style="background-color: White; width: 850px; height: 100%; padding: 25px;">
				<p><a href="../Admin/Management_ShipperName.aspx">발화인 통관명 설정</a>&nbsp;&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;&nbsp;<a href="../Document/DebitCredit_List.aspx">DebitCredit</a></p>

                <div style="text-align: right; padding-bottom: 10px;">
                    <%=HtmlButton %>
                </div>
                <%=FileListHTML %>
                <input type="hidden" id="HAccountID" value="<%=MemberInfo[2] %>" />
                <input type="hidden" id="G" value="<%=G %>" />
                <input type="hidden" id="T" value="<%=T %>" />
                <input type="hidden" id="V" value="<%=V %>" />
            </div>
        </div>
    </form>
</body>
</html>
