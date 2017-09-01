<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeFile="RequestView_Logistics.aspx.cs" Inherits="Logistics_RequestView_Logistics" %>
<%@ Register src="~/Logistics/Logedsmall.ascx" tagname="Loged" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<style type="text/css">
      .DivEachGroup
        {
            margin-top: 7px;
            margin-bottom: 7px;
            padding-top: 3px;
            padding-bottom: 3px;
        }
		
    </style>
    <link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script src="../Common/RegionCode.js?version=20131029" type="text/javascript"></script>
	<script type="text/javascript">
	    function DeliverySet(pk) {
	        window.open('../Admin/Dialog/DeliverySet.aspx?P=' + pk + '&S=' + form1.HRequestFormPk.value + "&C=" + form1.HConsigneePk.value + "&O=" + form1.HOurBranchPk.value + "&A=" + form1.HAccountID.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=600px; width=800px;');
	    }
	    function DeliveryPrint(tbcpk) {
	        window.open('../Admin/Dialog/DeliveryReceipt2.aspx?G=Print&S=' + tbcpk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=500px; width=900px;');
	    }
	    function DeliveryCancel(tbcpk) {
	        Admin.CancelDeliveryOrder(tbcpk, function (result) {
	            if (result == "1") {
	                alert("SUCCESS");
	                location.reload();
	            }
	        }, function (result) { alert("ERROR : " + result); });
	    }
	</script>
</head>
<body style="background-color: #E4E4E4; width: 660px; margin: 0 auto; padding-top: 10px;">
    <form id="form1" runat="server">
        <uc1:Loged ID="Loged1" runat="server" />
        <asp:ScriptManager ID="SM" runat="server">
            <Services>
                <asp:ServiceReference Path="~/WebService/Admin.asmx" />
            </Services>
        </asp:ScriptManager>
        <div style="background-color: White; min-height: 500px;">
            <div style="width:600px; padding:20px 0px 20px 20px; clear: both;">
               <div id="DVItem" class="DivEachGroup"  <%=IsConsigneeConfirmedStyle %>><%=HtmlItem %></div>
                <div><%=HtmlDelivery %></div>
            </div>
            <input type="hidden" id="HAccountID" value="<%=Session["ID"]  %>" />
            <input type="hidden" id="HRequestFormPk" value="<%=Request.Params["pk"] %>" />
            <input type="hidden" id="HOurBranchPk" value="<%=MemberInformation[1] %>" />
            <input type="hidden" id="HConsigneePk" value="<%=ConsigneePk %>" />
        </div>
    </form>
</body>
</html>
