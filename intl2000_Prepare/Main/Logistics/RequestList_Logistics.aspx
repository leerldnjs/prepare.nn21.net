<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="RequestList_Logistics.aspx.cs" Inherits="Logistics_RequestList_Logistics" %>

<%@ Register Src="~/Logistics/Loged.ascx" TagName="Loged" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function Goto(gubun, value) {
            switch (gubun) {
                case "CompanyInfo":
                    if (value == "") {
                        alert("회사정보가 아직 등록되지 않았습니다");
                    }
                    else {
                        location.href = "../Logistics/CompanyView_Logistics.aspx?M=View&S=" + value; break;
                    }
                case "RequestForm": location.href = "../Logistics/RequestView_Logistics.aspx?g=c&pk=" + value; break;
            }
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
        <uc1:loged id="Loged1" runat="server" />
        <div style="background-color: White; width: 850px; height: 100%; padding: 25px;">
            <%=HtmlList %>
            <input type="hidden" id="HAccountID" value="<%=MemberInfo[2] %>" />
        </div>
    </form>
</body>
</html>
