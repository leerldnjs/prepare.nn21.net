<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attention.aspx.cs" Inherits="Member_Attention" %>
<meta http-equiv="X-UA-Compatible" content="IE=10"> 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    </style>
    <script type="text/javascript">
        window.onload = function () {
            
        }

        </script>
</head>
<body style="background-color: white; padding: 10px; width: 400px; overflow-x: hidden;">
    <form id="form1" runat="server">
        <div style="width:400px;   padding-left:10px; padding-right:10px;  background-color:white;">
            <p>
                <%=Image %>
            </p>
        </div>
        <input type="hidden" id="HLanguage" />
    </form>
</body>
</html>
