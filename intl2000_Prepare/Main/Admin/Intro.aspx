<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Intro.aspx.cs" Inherits="Admin_Intro" %>

<%@ Register Src="Loged.ascx" TagName="Loged" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 10px;">
    <form id="form1" runat="server">
        <uc1:Loged ID="Loged1" runat="server" />
        <div class="Contents" style="height: 500px;">
            <div style="padding: 10px; width: 480px; height: 500px; position: absolute; background-color: #E6E6FA; font-weight: bold;"><%=GetGlobalResourceObject("qjsdur", "djqandusfkr") %></div>
            <div style="padding: 10px; width: 180px; height: 230px; margin-left: 520px; position: absolute; background-color: #F0E68C; font-weight: bold;"><%=GetGlobalResourceObject("qjsdur", "djqanwltl") %></div>
            <div style="padding: 10px; width: 180px; height: 230px; margin-top: 270px; margin-left: 520px; position: absolute; background-color: #FFDEAD;">
                <div style="font-weight: bold;"><%=GetGlobalResourceObject("qjsdur", "djqanqhrh") %></div>
            </div>
        </div>
    </form>
</body>
</html>
