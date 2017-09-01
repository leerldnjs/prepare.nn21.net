<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StorageOut_Logistics.aspx.cs" Debug="true" Inherits="Logistics_StorageOut_Logistics" %>
<%@ Register Src="~/Logistics/Loged.ascx" TagName="Loged" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function DeliveryModify(OurBranchStorageOutPk, requstformpk, consigneepk, branchpk) {
            window.open('../Logistics/Dialog/DeliverySet_Logistics.aspx?P=' + OurBranchStorageOutPk + '&S=' + requstformpk + "&C=" + consigneepk + "&O=" + branchpk + "&A=" + form1.HAccountID.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=600px; width=800px;');
        }
        function DeliveryListExcelDown(DeliveryType, BranchPk) {
            location.href = "../UploadedFiles/FileDownloadWithExcel.aspx?G=OKChulgo&S=" + escape(DeliveryType) + "&P=" + BranchPk;
        }
        function DeliveryPrint(pk) {
            window.open('../Admin/Dialog/DeliveryReceipt2.aspx?G=Print&S=' + pk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=500px; width=900px;');
        }
        function GoDeliveryOrder(tbcpk, requestformpk, accountid, isDeposited) {
            if (confirm("출고지시하시겠습니까?")) {
                Admin.GoDeliveryOrder(tbcpk, requestformpk, accountid, isDeposited, function (result) {
                    if (result == "1") { alert("출고지시완료"); location.reload(); }
                }, function (result) { alert("ERROR : " + result); });
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
            <div>
                <%=HTMLDriver %>
            </div>        
            
            <input type="hidden" id="HAccountID" value="<%=MEMBERINFO[2] %>" />
            <input type="hidden" id="HCHKCount" value="<%=ChkCount %>" />
            <input type="hidden" id="HDebug" />
        </div>
    </form>
</body>
</html>
