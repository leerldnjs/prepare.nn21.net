<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BranchList.aspx.cs" Inherits="Admin_BranchList" Debug="true" %>
<%@ Register src="LogedWithoutRecentRequest.ascx" tagname="LogedWithoutRecentRequest" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script src="../Common/public.js" type="text/javascript"></script>
<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    var _gaq = _gaq || [];
    _gaq.push(['_setAccount', 'UA-20110231-1']);
    _gaq.push(['_setDomainName', 'none']);
    _gaq.push(['_setAllowLinker', true]);
    _gaq.push(['_trackPageview']);

    (function () {
        var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
        ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
    })();

    function BranchAdd() {
        var DialogResult = form1.logedAccountID.value + "!!!";
        var retVal = window.showModalDialog('../Admin/Dialog/BranchAdd.aspx', DialogResult, 'dialogWidth=1100px;dialogHeight=650px;resizable=0;status=0;scroll=1;help=0;');
        if (retVal != undefined) {
            location.href = "../Admin/BranchInfo.aspx?M=View&S=" + retVal;
        }
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 10px;">
    <form id="form1" runat="server">
        <uc2:LogedWithoutRecentRequest ID="LogedWithoutRecentRequest1" runat="server" />
        <div style="background-color: White; width: 850px; height: 100%; padding: 25px;">
            <input type="hidden" id="HAccountID" value="<%=ACCOUNTID %>" />
            <p>
                <input type="button" value="지사등록" onclick="BranchAdd();" />
                <input type="button" value="지역관리" onclick="location.href = '../Admin/Dialog/SetRegionCode.aspx';" />
            </p>
            <%= BranchLIST%>
        </div>
    </form>
</body>
</html>
