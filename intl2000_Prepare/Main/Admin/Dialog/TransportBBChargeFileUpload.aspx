<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransportBBChargeFileUpload.aspx.cs" Inherits="Admin_Dialog_TransportBBChargeFileUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../Common/jquery-1.10.2.min.js"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script type="text/javascript">

        window.onload = function () {
            var now = new Date();
            $("#Date").datepicker();
            $("#Date").datepicker("option", "dateFormat", "yymmdd");
            $("#Date").val(now.format("yyyyMMdd"));

            if (form1.HCheckPostBack.value == "Y") {
                alert("Success");
                opener.location.reload();
                self.close();
                return false;
            }
        }
    </script>
    <link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="SM" runat="server">
            <Services>
                <asp:ServiceReference Path="~/WebService/Admin.asmx" />
            </Services>
        </asp:ScriptManager>
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>Date</td>
                <td>Title</td>
                <td>Unit</td>
                <td>Price</td>
            </tr>
            <tr>
                <td>
                    <input type="text" id="Date" name="Date" runat="server" style="width: 100px;" /></td>
                <td>
                    <input type="text" id="Title" name="Title" runat="server" style="width: 100px;" /></td>
                <td>
                    <select id="MonetaryUnit" name="MonetaryUnit" size="1">
                        <option value="18">RMB ￥</option>
                        <option value="19">USD $</option>
                        <option value="20" selected="selected">KRW ￦</option>
                        <option value="21">JPY Y</option>
                        <option value="22">HKD HK$</option>
                        <option value="23">EUR €</option>
                    </select></td>
                <td>
                    <input type="text" id="Price" name="Price" runat="server" style="width: 100px;" /></td>
            </tr>
        </table>
        <p>
            Memo
        </p>
        <p>
            <textarea rows="5" cols="60" id="TB_Comment" name="TB_Comment"></textarea>
        </p>
        <fieldset style="padding-right: 20px;">
            <legend><strong>FILE UPLOAD</strong></legend>
            <ul style="line-height: 23px;">
                <li>File 1 : 
					<input type="text" name="TB0" style="width: 100px;" />&nbsp;&nbsp;<input type="file" name="FILE0" id="FILE0" runat="server" />
                </li>
                <li>File 2 : 
					<input type="text" name="TB1" style="width: 100px;" />&nbsp;&nbsp;<input type="file" name="FILE0" id="FILE1" runat="server" />
                </li>
                <li>File 3 : 
					<input type="text" name="TB2" style="width: 100px;" />&nbsp;&nbsp;<input type="file" name="FILE0" id="FILE2" runat="server" />
                </li>
                <li>File 4 : 
					<input type="text" name="TB3" style="width: 100px;" />&nbsp;&nbsp;<input type="file" name="FILE0" id="FILE3" runat="server" />
                </li>
            </ul>
            <div style="padding-top: 20px; padding-bottom: 20px; padding-left: 200px;">
                <asp:Button ID="Button1" runat="server" Text="SAVE" OnClick="Button1_Click" />
            </div>
            <input type="hidden" id="HCheckPostBack" value="<%=IsUpload %>" />
            <input type="hidden" id="HeadPk" name="HeadPk" value="<%=Request.Params["S"] + ""%>" />
        </fieldset>
    </form>
</body>
</html>
