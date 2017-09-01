<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransportBetweenCompanyList.aspx.cs" Debug="true" Inherits="Admin_TransportBetweenCompanyList" %>

<%@ Register Src="LogedWithoutRecentRequest.ascx" TagName="LogedWithoutRecentRequest" TagPrefix="uc1" %>
<%@ Register Src="../CustomClearance/Loged.ascx" TagName="Loged" TagPrefix="uc2" %>
<%@ Register Src="~/Logistics/Loged.ascx" TagName="Loged" TagPrefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function DeliveryModify(OurBranchStorageOutPk, requstformpk, consigneepk, branchpk) {
            window.open('./Dialog/DeliverySet.aspx?P=' + OurBranchStorageOutPk + '&S=' + requstformpk + "&C=" + consigneepk + "&O=" + branchpk + "&A=" + form1.HAccountID.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=600px; width=800px;');
        }
        function Goto(gubun, value) {
        	switch (gubun) {
        		case "Storage": location.href = "./StorageOut.aspx?S=" + value; break;
        		case "TBBPk": location.href = "./TransportBetweenBranchView.aspx?S=" + value; break;
        		case "RequestForm": location.href = "./RequestView.aspx?g=c&pk=" + value; break;
        		case "Company": location.href = "./CompanyInfo.aspx?M=View&S=" + value; break;
        		case "CheckDescription": location.href = "./CheckDescription.aspx?S=" + value; break;
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
        <uc1:LogedWithoutRecentRequest ID="LogedWithoutRecentRequest1" runat="server" />
        <uc2:Loged ID="Loged1" runat="server" Visible="false" />
        <uc3:Loged ID="Loged2" runat="server" Visible="false" />
        <div style="background-color: White; width: 850px; height: 100%; padding: 25px;">
            <div><%=HTMLBody %></div>
        </div>
        <input type="hidden" id="HAccountID" value="<%=MEMBERINFO[2] %>" />
    </form>
</body>
</html>
