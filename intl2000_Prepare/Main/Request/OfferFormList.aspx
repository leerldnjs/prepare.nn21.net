<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="OfferFormList.aspx.cs" Inherits="Request_OfferFormList" %>
<%@ Register src="../Member/LogedTopMenu.ascx" tagname="Loged" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    	input{text-align:center; }
    	.tdSubT{border-bottom:solid 2px #93A9B8;	}
    	.tdSubTGold	 { border-top:solid 2px #DAA520; background-color:#FFFACD; text-align:center; height:20px; border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px;  }
    	.tdSubMainGold{ background-color:#FFFACD; border-bottom:dotted 1px #E8E8E8; }
    	/*#FFFACD #DAA520*/
		.td01{background-color:#f5f5f5; text-align:center; height:20px; border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px; }
    	.td02{width:330px;border-bottom:dotted 1px #E8E8E8;	padding-top:4px;padding-bottom:4px; padding-left:10px; }
		.td023	{width:760px; padding-top:4px;padding-bottom:4px; padding-left:10px;	 border-bottom:dotted 1px #E8E8E8; }
   	</style>
	<script src="../Common/public.js" type="text/javascript"></script>
	<script type="text/javascript">
		function ViewDocu(Gubun, RequestFormPk) {
			switch (Gubun) {
				case "I": location.href = "CommercialInvoiceView.aspx?S=" + RequestFormPk; break;
				case "P": location.href = "PackingListView.aspx?S=" + RequestFormPk; break;
				case "D": location.href = "OfferFormWrite.aspx?P=" + RequestFormPk; break;
			}
		}
		function WriteDocu(Gubun, RequestFormPk) {
			switch (Gubun) {
				case "I": location.href = "CommercialInvoiceWrite.aspx?S=" + RequestFormPk + "&M=IW"; break;
				case "P": location.href = "PackingListWrite.aspx?S=" + RequestFormPk + "&M=PW"; break;
			}
		}
		function DeleteDocu(RequestFormPk) {
			if (confirm("This Document will be Deleted")) {
				Request.DeleteRequestForm(RequestFormPk, function (result) { document.location.href = "./OfferFormList.aspx"; }, function (result) { alert("Error"); });
			}
		}

	</script>
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px; overflow:visible;" >
    <form id="form1" runat="server">
	<asp:ScriptManager ID="WebService" runat="server" ><Services><asp:ServiceReference Path="~/WebService/Request.asmx" /></Services></asp:ScriptManager>
    <uc1:Loged ID="Loged1" runat="server" />
    <div class="ContentsTopMenu">
		<div style="width:783px; margin:0 auto; " >
			<div style="width:500px; padding:10px; float:left; "><%=PageNoHtml %></div>
			<div style="text-align:right; padding:10px; "><%=GetGlobalResourceObject("qjsdur", "anstjrhksfl")%>&nbsp;&nbsp;>>&nbsp;&nbsp;<strong><%=GetGlobalResourceObject("qjsdur", "anstjdufrl")%></strong></div>
			<table id="ItemTable[0]" style="background-color:White; width:783px; "  border="0" cellpadding="0" cellspacing="0">
			<thead>
				<tr><td class="tdSubT" colspan="10">&nbsp;</td></tr>
				<tr style="height:30px;" >
					<td style="background-color:#F5F5F5; text-align:center; width:80px;" class="Line1E8E8E8" >Date</td>
					<td style="background-color:#F5F5F5; text-align:center; width:130px;" class="Line1E8E8E8" >Docu No</td>
					<td style="background-color:#F5F5F5; text-align:center;" class="Line1E8E8E8" >Shipper</td>
					<td style="background-color:#F5F5F5; text-align:center; width:70px;" class="Line1E8E8E8" >Consignee</td>
					<td style="background-color:#F5F5F5; text-align:center; width:130px;" class="Line1E8E8E8" >Description & Summary</td>
					<td style="background-color:#F5F5F5; text-align:center; width:50px;" class="Line1E8E8E8" >IV</td>
					<td style="background-color:#F5F5F5; text-align:center; width:50px;" class="Line1E8E8E8" >PL</td>
					<%--<td style="background-color:#F5F5F5; text-align:center; width:50px;" class="Line1E8E8E8" >오퍼시트</td>--%>
					<td style="background-color:#F5F5F5; text-align:center; width:50px;" class="Line1E8E8E8" >Item</td>
					<td style="background-color:#F5F5F5; text-align:center; width:50px;" class="Line1E8E8E8" >Del</td>
				</tr>
			</thead>
			<tbody>
				<%=OfferList %>
			</tbody>
		</table>
		</div>
    </div>
    </form>
</body>
</html>