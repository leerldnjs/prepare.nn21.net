<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyInfoDocumentList.aspx.cs" Inherits="Admin_CompanyInfoDocumentList" Debug="true" %>
<meta http-equiv="X-UA-Compatible" content="IE=10"> 
<%@ Register Src="../Member/LogedTopMenu.ascx" TagName="Loged" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DocumentList</title>
    <style type="text/css">
        .HeaderInfo
        {
            position: absolute;
            border: dotted 1px #93A9B8;
            font-weight: bold;
            padding-top: 5px;
            text-align: center;
            letter-spacing: 3px;
            width: 100px;
        }
    </style>
    <link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function PrintPOP(pk)
        {
            window.open('../Request/DocumentView.aspx?S=' + pk , '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=yes, top=200px; left=200px; height=700px; width=800px;');
        }
        function GoClearance(Gubun, Value) {
            switch (Gubun) {
                case "ShowTrade": location.href = "../CustomClearance/newCommercialDocu_TradingSchedule.aspx?S=" + Value; break;
                case "ShowInvoice": location.href = "../CustomClearance/newCommercialDocu_Invoice.aspx?S=" + Value; break;
                case "ShowPacking": location.href = "../CustomClearance/newCommercialDocu_PackingList.aspx?S=" + Value; break;
            }
        }

		function GoModify(Gubun, Value) {
			switch (Gubun) {
				case "InvoiceModify": location.href = "../Request/newCommercialInvoiceWrite.aspx?S=" + Value + "&M=IW"; break;
				case "PackingModify": location.href = "../Request/newPackingListWrite.aspx?S=" + Value + "&M=IW"; break;
			}
		}

        function DeleteDocument(Value) {
            Request.DeleteDocument(Value, function (result) {
                if (result == "1") {
                    alert("SUCCESS");
                    parent.location.href = "../Admin/CompanyInfoDocumentList.aspx";
                }
            }, function (result) { alert("ERROR : " + result); });
        }
    </script>
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 10px;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="SM" runat="server">
            <Services>
                <asp:ServiceReference Path="~/WebService/Request.asmx" />
            </Services>
        </asp:ScriptManager>
        <uc1:Loged ID="Loged1" runat="server" />
        <div style="background-color: White; width: 850px; height: 100%; padding: 25px;">
            <%=HtmlList %>
            <input type="hidden" id="HAccountID" value="<%=MEMBERINFO[2] %>" />
            <input type="hidden" id="HCompanyPk" value="<%=CompanyPk %>" />
        </div>
    </form>
</body>
</html>
