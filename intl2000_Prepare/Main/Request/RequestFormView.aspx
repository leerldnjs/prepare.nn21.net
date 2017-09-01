<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequestFormView.aspx.cs" Inherits="RequestForm_RequestFormView" Debug="true" %>

<%@ Register Src="../Member/LogedTopMenu.ascx" TagName="Loged" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
		.tdSubT {
			border-bottom: solid 2px #93A9B8;
			padding-top: 10px;
		}

		.ItemTableIn {
			border-bottom: dotted 1px #E8E8E8;
			padding-top: 4px;
			padding-bottom: 4px;
		}
	</style>
	<script type="text/javascript">
		function PopupItemModify(pk) {	//화물 수정
			var SendTo = new Array();
			SendTo[0] = document.getElementById("TB_ID").value;
			SendTo[1] = "";
			var retVal = window.showModalDialog('./ItemModify.aspx?S=' + pk + '&CL=' + document.getElementById("TB_CL").value, SendTo, "dialogHeight:600px; dialogWidth:900px; resizable:1; status:0; scroll:1; help:0; ");
			if (retVal == "Y") { window.document.location.reload(); }
		}
		function Warning() {
			alert("접수된 내용과 같이 수입서류가 확정되었습니다. \r\n 수정이 필요하신 경우 당사 통관팀 032-772-8481(내선 304) 으로 연락주시기 바랍니다.");
			return false;
		}
		function PopupItemModifyList(pk) { //수정내역 보기
			window.open('./ItemModifyList.aspx?S=' + pk + '&CL=' + document.getElementById("TB_CL").value, '', 'location=no, directories=no,resizable=no,status=no,toolbar=no,menubar=no, scrollbars=no, top=200px; left=200px; height=700px; width=600px;');
		}
		function BodyOnload() {
			var Url = location.href;
			var Cut = Url.indexOf("?");
			var PostValue = Url.substring(Cut + 1);
			var EachPostValue = PostValue.split("&");
			if (EachPostValue.length == 2) {
				if (EachPostValue[0].split("=")[1] == "sn") { Request.AskCompanyCustomerCodeSetAuto(EachPostValue[1].split("=")[1], ONSUCCESS, ONFAILED); }
				else if (EachPostValue[0].split("=")[1] == "cn") { alert("수하인이 신규업체입니다. 고객번호를 지정해주세요."); }
			}
			$(".NavRequest").addClass("active");
		}
		function ONSUCCESS(result) {
			if (result != "0") {
				resultSplit = result.split("!!");
				if (confirm(form1.HNumberAuto1.value + " : " + resultSplit[0] + "\r\n" + form1.HNumberAuto2.value)) {
					Request.SetCompanyCustomerCodeAuto(resultSplit[1], resultSplit[0], ONSUCCESS0, ONFAILED);
				}
			}
		}
		function ONSUCCESS0(result) {
			if (result != "0") {
				document.getElementById("TB_ShipperCode1").value = result.toString().substr(0, 3);
				document.getElementById("TB_ShipperCode2").value = result.toString().substr(3, result.toString().length - 3);
				document.getElementById("TB_ShipperCode2").readOnly = "readonly";
				document.getElementById("CompanyCodeSave").style.visibility = 'hidden';
				document.getElementById("TB_ShipperCode2").style.border = 0;
			}
			else { window.alert(form1.HError.value); }
		}
		function ONFAILED(result) { window.alert(result); }
		function SetShipperCompanyCodeManual() {
			var pk = document.getElementById('ShipperPk').value;
			var value = document.getElementById('TB_ShipperCode1').value + document.getElementById('TB_ShipperCode2').value;
			Request.SetCompanyCustomerCodeManual(pk, value, SuccessSetShipperCompanyCodeManual, ONFAILED);
		}
		function SuccessSetShipperCompanyCodeManual(result) {
			if (result == "0") { window.alert(form1.HUsedNumber.value); }
			else {
				document.getElementById("TB_ShipperCode1").value = result.toString().substr(0, 3);
				document.getElementById("TB_ShipperCode2").value = result.toString().substr(3, result.toString().length - 3);
				document.getElementById("TB_ShipperCode2").readOnly = "readonly";
				document.getElementById("TB_ShipperCode2").style.border = "0";
				document.getElementById("CompanyCodeSave").style.visibility = 'hidden';
			}
		}
		function CustomerSign(Pk, MemberPk, AccountID, Gubun) {
			if (confirm("지금 기재된 내용으로 통관을 진행하겠습니다.")) {
				Request.CustomerConfirm(Gubun, Pk, AccountID, function (result) {
					alert("승인완료");
					location.reload();
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function SetFileGubunCL(filepk, setvalue) {
			Request.FileGubunCLChange("", filepk, setvalue, function (result) {
				if (result == "1") {
					alert("SUCCESS");
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function GoFileupload(RequestPk, Gubun) {
			window.open('../Admin/Dialog/FileUpload.aspx?G=' + Gubun + '&S=' + RequestPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;');
		}
		function Goto(where) {
			switch (where) {
				case "Modify": location.href = "./RequestModify.aspx?M=CompanyModify&S=" + form1.HRequestFormPk.value + "&P=" + form1.HCompanyPk.value; break;
				case "ViewCalculate": window.open('../Request/FreightChargeView.aspx?S=' + form1.HRequestFormPk.value + "&G=" + form1.HCompanyPk.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=yes, top=200px; left=200px; height=700px; width=800px;');
					break;
			}
		}
		function PopWarehouseMap(StorageCode) {
			window.open('../Request/Dialog/WarehouseMap2.aspx?S=' + StorageCode, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=700px; width=800px;');
		}
		function ViewInPacking(IorP, CommercialDocumentPk) {
			switch (IorP) {
				case "I": location.href = "../CustomClearance/CommercialDocu_Invoice.aspx?S=" + CommercialDocumentPk; break;
				case "P": location.href = "../CustomClearance/CommercialDocu_PackingList.aspx?S=" + CommercialDocumentPk; break;
				case "B": location.href = "../CustomClearance/CommercialDocu_HouseBL.aspx?S=" + CommercialDocumentPk; break;
			}
		}
		function DeliverySetByMember() {
			window.open('../Admin/Dialog/DeliverySetByMember.aspx?P=N&S=' + form1.HRequestFormPk.value + "&C=" + form1.HCompanyPk.value + "&A=" + form1.HAccountID.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=600px; width=800px;');
		}
		function ShowOtherBL(requestFormPk) {
			location.href = "../UploadedFiles/FileDownload.aspx?S=" + requestFormPk;
		}
	</script>
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 20px;" onload="BodyOnload()">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Request.asmx" />
			</Services>
		</asp:ScriptManager>
		<uc1:Loged ID="Loged1" runat="server" />
		<div class=" ContentsTopMenu">
			<div style="float: right; width: 110px; text-align: center;">
				<%=NAVIGATIONBUTTONS %>
			</div>
			<div style="width: 725px;">
				<div style="width: 350px; float: left;">
					<%=Shipper %>
					<%=Consignee %>
					<%=Notify %>
				</div>
				<div style="width: 350px; float: left; margin-left: 20px;">
					<%=Schedule %>
					<p>&nbsp;</p>
					<%=FILELIST %>
					<%=CLEARANCELIST %>
				</div>
				<div style="clear: both;">&nbsp;</div>
				<div style="width: 720px;">

					<div class="tdSubT">
						<%=GetGlobalResourceObject("RequestForm", "Freight")%>
						<span style="float: right; margin-top: -10px; padding-right: 10px;">
							<strong><a href="../Images/명세수정요청서.docx">명세수정요청서(word)</a></strong>&nbsp;&nbsp;&nbsp;&nbsp;
							<strong><a href="../Images/명세수정요청서.hwp">명세수정요청서(hwp)</a></strong>&nbsp;&nbsp;&nbsp;&nbsp;
						<%=BTNModify %>&nbsp;&nbsp;<%= ModifyCount %>
						</span>
					</div>
					<table border="0" cellpadding="0" cellspacing="0" style="width: 720px;">
						<tr style="height: 30px;">
							<td bgcolor="#F5F5F5" height="20" align="center" style="width: 50px"><%=GetGlobalResourceObject("RequestForm", "BoxNo")%></td>
							<td bgcolor="#F5F5F5" align="center"><%=GetGlobalResourceObject("RequestForm", "Description")%> / <%=GetGlobalResourceObject("RequestForm", "Label")%> / <%=GetGlobalResourceObject("RequestForm", "Material")%></td>
							<td bgcolor="#F5F5F5" align="center" style="width: 75px"><%=GetGlobalResourceObject("RequestForm", "Count")%></td>
							<td bgcolor="#F5F5F5" align="center" style="width: 55px"><%=GetGlobalResourceObject("RequestForm", "UnitCost")%></td>
							<td bgcolor="#F5F5F5" align="center" style="width: 75px"><%=GetGlobalResourceObject("RequestForm", "Amount")%></td>
							<td bgcolor="#F5F5F5" align="center" style="width: 55px"><%=GetGlobalResourceObject("RequestForm", "PackingCount")%></td>
							<td bgcolor="#F5F5F5" align="center" style="width: 55px"><%=GetGlobalResourceObject("RequestForm", "GrossWeight")%></td>
							<td bgcolor="#F5F5F5" align="center" style="width: 55px"><%=GetGlobalResourceObject("RequestForm", "Volume")%></td>
						</tr>
						<%=ItemTable %>
						<tr style="height: 30px;">
							<td bgcolor="#F5F5F5" height="20" align="center">&nbsp;</td>
							<td bgcolor="#F5F5F5" align="center">&nbsp;</td>
							<td bgcolor="#F5F5F5" align="center"><%=TotalCount %></td>
							<td bgcolor="#F5F5F5" colspan="2" align="center"><%=TotalAmount %></td>
							<td bgcolor="#F5F5F5" align="center"><%=TotalPackedCount%></td>
							<td bgcolor="#F5F5F5" align="center"><%=TotalWeight%></td>
							<td bgcolor="#F5F5F5" align="center"><%=TotalVolume%></td>
						</tr>
					</table>
					<div class="tdSubT">
						<%=GetGlobalResourceObject("RequestForm", "Payment")%> / <%=GetGlobalResourceObject("RequestForm", "TradeDocument")%>
						<input type="hidden" id="TB_ID" value="<%=AccountID %>" />
						<input type="hidden" id="TB_CL" value="<%= RequestFormAdditionalInfoCL %>" />
					</div>
					<div class="tdSubT">
						<ul style="line-height: 20px;"><%=Payment %></ul>
						<ul style="line-height: 20px;"><%=HtmlDeposited %></ul>
					</div>
					<div class="tdSubT">
						배송지
						<ul style="line-height:20px;"><%=HtmlDelivery %></ul>
						<%=DeliveryButton %>
					</div>
				</div>
			</div>
		
		<input type="hidden" id="HRequestFormPk" value="<%=RequestPk%>" />
		<input type="hidden" id="HCompanyPk" value="<%=companypk %>" />
		<input type="hidden" id="HAccountID" value="<%=AccountID %>" />
		<input type="hidden" id="HError" value="<%= GetGlobalResourceObject("Alert", "CallError") %>" />
		<input type="hidden" id="HNumberAuto1" value="<%=GetGlobalResourceObject("Alert", "SetCompanyCodeAuto1") %>" />
		<input type="hidden" id="HNumberAuto2" value="<%=GetGlobalResourceObject("Alert", "SetCompanyCodeAuto2") %>" />
		<input type="hidden" id="HUsedNumber" value="<%=GetGlobalResourceObject("Alert", "CompanyCodeBeingUsed") %>" />
		<input type="hidden" id="wlwjddhksfy" value="<%=GetGlobalResourceObject("Alert", "wlwjddhksfy") %>" />
	</form>
</body>
</html>
