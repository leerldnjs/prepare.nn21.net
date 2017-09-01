<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommentWithFile_Member.aspx.cs" Inherits="UploadedFiles_CommentWithFile_Member" %>
<%@ Register Src="../Member/LogedTopMenu.ascx" TagName="Loged" TagPrefix="uc1" %>
<%@ Register Src="~/Admin/LogedWithoutRecentRequest.ascx" TagName="LogedWithoutRecentRequest" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
    </script>
    <link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 10px; overflow: visible;">
    <form id="form1" runat="server">
        <uc1:Loged ID="Loged1" runat="server" />
        <uc2:LogedWithoutRecentRequest ID="Loged2" runat="server" Visible="false"/>
        <div style="background-color: White; width: 850px; height: 100%; padding: 25px;">
            <fieldset style="padding-right: 20px; background-color: white;">
                <legend><strong><%=GetGlobalResourceObject("qjsdur", "andurthdrma")%></strong></legend>
                <div style="padding: 20px;">
                    <p>&nbsp;<img src="../Images/ico_arrow.gif" alt="" /><strong> <%=GetGlobalResourceObject("qjsdur", "andurthdrmaETC") %></strong></p>
                    <textarea id="TBComment" name="TBComment" rows="10" cols="80"></textarea><br />
                    <p>&nbsp;<img src="../Images/ico_arrow.gif" alt="" />
                        <strong><%=GetGlobalResourceObject("qjsdur", "wjwkdqkdqjq") %></strong></p>
                    <p>File 1 : &nbsp;&nbsp;<input type="file" name="FILE0" id="FILE0" runat="server" /></p>
                </div>
                <div style="padding-bottom: 20px; padding-left: 200px;">
                    <asp:Button ID="Button1" runat="server" Text="SAVE" OnClick="Button1_Click" UseSubmitBehavior="false"/>
                    <%--<input type="button" value="save" onclick="startest();" style="width:100px;" />--%>
                </div>
            </fieldset>
        </div>
    </form>
</body>
</html>
