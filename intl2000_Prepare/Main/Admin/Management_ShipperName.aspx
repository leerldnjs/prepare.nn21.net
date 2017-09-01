<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Management_ShipperName.aspx.cs" Inherits="Admin_Management_ShipperName" %>

<%@ Register Src="../Admin/LogedWithoutRecentRequest.ascx" TagName="LogedWithoutRecentRequest" TagPrefix="uc1" %>
<%@ Register Src="../Member/LogedTopMenu.ascx" TagName="Loged" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
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
                <input type="hidden" id="HAccountID" value="<%=MemberInfo[2] %>" />
				<%=ListHeader %>
				<%=ListBody %>
            </div>
        </div>
    </form>
</body>
</html>
