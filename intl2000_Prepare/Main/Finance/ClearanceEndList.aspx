<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClearanceEndList.aspx.cs" Inherits="Finance_ClearanceEndList" %>
<%@ Register src="~/Admin/LogedWithoutRecentRequest11.ascx" tagname="LogedWithoutRecentRequest" tagprefix="uc1" %>
<%@ Register src="../CustomClearance/Loged.ascx" tagname="Loged" tagprefix="uc2" %>
<%@ Register src="~/Logistics/Loged.ascx" tagname="Loged" tagprefix="uc3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		function Print(Gubun, Value) {
			switch (Gubun) {
				case "DO": location.href = "../CustomClearance/CommercialDocu_DeliveryOrder.aspx?S=" + Value + "&B=" + form1.HBBHPk.value + ""; break;
				case "B": location.href = "../CustomClearance/CommercialDocu_HouseBL.aspx?S=" + Value + ""; break;
				case "B_YT": location.href = "../CustomClearance/CommercialDocu_HouseBL.aspx?S=" + Value + "&YT=YT"; break;
				case "I": location.href = "../CustomClearance/CommercialDocu_Invoice.aspx?S=" + Value + ""; break;
				case "P": location.href = "../CustomClearance/CommercialDocu_PackingList.aspx?S=" + Value + ""; break;
				case "D": window.open('./Dialog/AutoPrint.aspx?G=DeliveryReceipt&S=' + Value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;'); break;
			}
		}
		function LoadClearanceEnd() {
			location.href = '/Finance/ClearanceEndList.aspx?D=' + document.getElementById("Date").value;
		}
		function ExcelDown() {
			location.href = "/UploadedFiles/FileDownloadWithExcel.aspx?G=ClearanceEndList&D=" + document.getElementById("Date").value;
		}
		function Goto(pk) {
			location.href = "/Admin/RequestView.aspx?g=s&pk=" + pk;
			
		}
	</script>

</head>
<body style="background-color:#E4E4E4; width:1100px; margin:0 auto; padding-top:10px;" >
    <form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
		<uc1:LogedWithoutRecentRequest ID="LogedWithoutRecentRequest1" runat="server" />
        <uc2:Loged ID="Loged1" runat="server" Visible="false" />
        <uc3:Loged ID="Loged2" runat="server" Visible="false" />
				<div style="background-color: White; width: 1050px; height: 100%; padding: 25px;">
					<p>
			<a href="../Finance/ClearanceList.aspx"><strong>통관내역</strong></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="../Admin/SettlementWithCustoms_Date.aspx" >일별 정산</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="../Admin/SettlementWithCustoms_Container.aspx" >컨테이너별 정산</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="../Admin/SettlementWithCustoms_MultipleAdd.aspx" >엑셀 입력</a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="/Finance/ClearanceEndList.aspx" ><strong>통관 내역보기</strong></a>
		</p>

			<p style="text-align:right;">
				조회일자 : 
				<input type="text" style="width:90px; text-align:center;" id="Date" value="<%=Date %>" />
				<input type="button" onclick="LoadClearanceEnd();" value="확인" />
				<input type="button" onclick="ExcelDown();" value="엑셀다운로드" />
			</p>
			<p>
				<%=MSG %>
			</p>
			<%=Html %>
		</div>
    </form>
</body>
</html>
