<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransportBetweenCompanyList_Logistics.aspx.cs" Debug="true" Inherits="Logistics_TransportBetweenCompanyList_Logistics" %>
<%@ Register Src="~/Logistics/Loged.ascx" TagName="Loged" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function DeliveryModify(OurBranchStorageOutPk, requstformpk, consigneepk, branchpk) {
            window.open('../Logistics/Dialog/DeliverySet_Logistics.aspx?P=' + OurBranchStorageOutPk + '&S=' + requstformpk + "&C=" + consigneepk + "&O=" + branchpk + "&A=" + form1.HAccountID.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=600px; width=800px;');
        }
        function Goto(gubun, value) {
            switch (gubun) {
                case "Storage": location.href = "../Admin/StorageOut.aspx?S=" + value; break;
                case "TBBPk": location.href = "../Admin/TransportBetweenBranchView.aspx?S=" + value; break;
                case "RequestForm": location.href = "../Logistics/RequestView_Logistics.aspx?g=c&pk=" + value; break;
                case "Company": location.href = "../Admin/CompanyInfo.aspx?M=View&S=" + value; break;
                case "CompanyInfo": location.href = "../Logistics/CompanyView_Logistics.aspx?M=View&S=" + value; break;
                case "CheckDescription": location.href = "../Admin/CheckDescription.aspx?S=" + value; break;
            }
        }
    </script>
</head>
<body style="background-color: #E4E4E4; width: 1000px; margin: 0 auto; padding-top: 10px;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="SM" runat="server">
            <Services>
                <asp:ServiceReference Path="~/WebService/Admin.asmx" />
            </Services>
        </asp:ScriptManager>
        <uc1:Loged ID="Loged1" runat="server" />
        <div style="background-color: White; width: 950px; height: 100%; padding: 25px;">
            <div><%=HTMLBody %></div>
        </div>
        <input type="hidden" id="HAccountID" value="<%=MEMBERINFO[2] %>" />
    </form>
</body>
</html>
