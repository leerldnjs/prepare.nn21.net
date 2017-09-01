<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransportBetweenBranchView.aspx.cs" Inherits="Admin_TransportBetweenBranchView" Debug="true" %>

<%@ Register Src="LogedWithoutRecentRequest.ascx" TagName="LogedWithoutRecentRequest" TagPrefix="uc1" %>
<%@ Register Src="../CustomClearance/Loged.ascx" TagName="Loged" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		function MsgSendFromTransport() {
			var dialogSum = "";
			switch (form1.HTransportCL.value) {
				case "1":
					dialogSum = document.getElementById("TBMasterBL").value + "#@!" + document.getElementById("TBAirCompanyName").value;
					break;
				case "3":
					dialogSum = document.getElementById("TBMasterBL").value + "#@!" + document.getElementById("TBShipCompanyName").value;
					break;
				case "4":
					dialogSum = document.getElementById("TBMasterBL").value + "#@!" + document.getElementById("TBHandCarryCompanyName").value;
					break;
				case "5":
					dialogSum = document.getElementById("TBMasterBL").value + "#@!" + document.getElementById("TBShipCompanyName").value;
					break;
				case "6":
					dialogSum = document.getElementById("TBMasterBL").value + "#@!" + document.getElementById("TBShipCompanyName").value;
					break;
				case "7":
					dialogSum = document.getElementById("TBMasterBL").value + "#@!" + document.getElementById("TBCarCompanyName").value;
					break;
			}
			var dialogArgument = new Array();
			dialogArgument[0] = form1.HBBHPk.value;
			dialogArgument[1] = form1.HAccountID.value;
			dialogArgument[2] = form1.HBranchPk.value;
			dialogArgument[3] = form1.Hstepcl.value;
			dialogArgument[4] = dialogSum;
			var retVal = window.showModalDialog('./Dialog/MsgSendFromTransport.aspx', dialogArgument, "dialogHeight:600px; dialogWidth:480px; resizable:1; status:0; scroll:1; help:0; ");
			if (retVal == "Y") { window.document.location.reload(); }
		}
		function DeleteTransportBBCharge(ChargePk) {
			Admin.DeleteTransportBBCharge(ChargePk, function (result) {
				if (result == "1") {
					alert("SUCCESS");
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); })
		}
		function PackingSend(Gubun, BBHPk) {
			Admin.PackingSend(Gubun, BBHPk, form1.HAccountID.value, function (result) {
				if (result == "0") {
					alert("보낼 화물을 추가하지 않았습니다. 발송할수 없습니다.");
					return false;
				}
				else if (result == "1") {
					alert("SUCCESS");
					location.reload();
				}
				else {
					form1.DEBUG.value = result;
					alert("error");
				}
			}, function (result) { alert("ERROR : " + result); })
		}
		function FileDelete(filepk) {
			Admin.FileDelete(filepk, "", function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function GoFileupload() {
			window.open('./Dialog/FileUpload.aspx?G=0&S=' + form1.HBBHPk.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;');
		}
		function GoFileupload2() {
			window.open('./Dialog/TransportBBChargeFileUpload.aspx?P=' + document.getElementById("HBranchPk").value + '&G=31&S=' + form1.HBBHPk.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	500px; width=600px;');
		}
		function PackingModify() {
			var FromDateTime = document.getElementById("TBDepartureDate").value.substr(0, 4) + "." + document.getElementById("TBDepartureDate").value.substr(4, 2) + "." + document.getElementById("TBDepartureDate").value.substr(6, 2) + ". " + document.getElementById("TBDepartureHour").value + ":" + document.getElementById("TBDepartureMin").value;
			var ToDateTime = document.getElementById("TBArrivalDate").value.substr(0, 4) + "." + document.getElementById("TBArrivalDate").value.substr(4, 2) + "." + document.getElementById("TBArrivalDate").value.substr(6, 2) + ". " + document.getElementById("TBArrivalHour").value + ":" + document.getElementById("TBArrivalMin").value;
			var InfoSum = "";
			var BLNo = "";
			switch (form1.HTransportCL.value) {
				case "1":
					BLNo = document.getElementById("TBMasterBL").value;
					InfoSum = document.getElementById("TBAirCompanyName").value + "#@!" +
						document.getElementById("TBDepartureRegion").value + "#@!" +
						document.getElementById("TBArrivalRegion").value + "#@!" +
						document.getElementById("TBAirName").value + "#@!" +
						document.getElementById("TBAirDepartureTEL").value + "#@!" +
						document.getElementById("TBAirArrivalTEL").value;
					break;
				case "3":
					BLNo = document.getElementById("TBMasterBL").value;
					InfoSum = document.getElementById("TBShipCompanyName").value + "#@!" +
						document.getElementById("TBDepartureRegion").value + "#@!" +
						document.getElementById("TBArrivalRegion").value + "#@!" +
						document.getElementById("TBShipName").value + "#@!" +
						document.getElementById("TBContainerSize").value + "#@!" +
						document.getElementById("TBShippingTime").value + "#@!" +
						document.getElementById("TBContainerNo").value + "#@!" +
						document.getElementById("TBSealNo").value;
					break;
				case "4":
					BLNo = document.getElementById("TBMasterBL").value;
					InfoSum = document.getElementById("TBHandCarryCompanyName").value + "#@!" +
						document.getElementById("TBDepartureRegion").value + "#@!" +
						document.getElementById("TBArrivalRegion").value + "#@!" +
						document.getElementById("TBHandCarryDepartureTEL").value + "#@!" +
						document.getElementById("TBHandCarryArrivalTEL").value;
					break;
				case "5":
					BLNo = document.getElementById("TBMasterBL").value;
					InfoSum = document.getElementById("TBShipCompanyName").value + "#@!" +
						document.getElementById("TBDepartureRegion").value + "#@!" +
						document.getElementById("TBArrivalRegion").value + "#@!" +
						 document.getElementById("TBShipName").value + "#@!" +
						 document.getElementById("TBFCLTitle").value + "#@!" +
						document.getElementById("TBContainerSize").value + "#@!" +
						 document.getElementById("TBShippingTime").value + "#@!" +
						document.getElementById("TBContainerNo").value + "#@!" +
						document.getElementById("TBSealNo").value;
					break;
				case "6":
					BLNo = document.getElementById("TBMasterBL").value;
					InfoSum = document.getElementById("TBShipCompanyName").value + "#@!" +
						document.getElementById("TBDepartureRegion").value + "#@!" +
						document.getElementById("TBArrivalRegion").value + "#@!" +
						document.getElementById("TBShipName").value + "#@!" +
						document.getElementById("TBContainerSize").value + "#@!" +
						document.getElementById("TBContainerNo").value + "#@!" +
						document.getElementById("TBSealNo").value + "#@!" +
						document.getElementById("TBDepartureStaff").value + "#@!" +
						document.getElementById("TBArrivalStaff").value;
					break;
				case "7":
					BLNo = document.getElementById("TBMasterBL").value;
					InfoSum = document.getElementById("TBCarCompanyName").value + "#@!" +
						document.getElementById("TBDepartureRegion").value + "#@!" +
						document.getElementById("TBArrivalRegion").value + "#@!" +
						document.getElementById("TBCarNo").value + "#@!" +
						document.getElementById("TBCarSize").value + "#@!" +
						document.getElementById("TBDriverTEL").value + "#@!" +
						document.getElementById("TBDriverName").value;
					break;
			}
			Admin.UpdateTransportBBHead(form1.HBBHPk.value, BLNo, FromDateTime, ToDateTime, InfoSum, function (result) {
				if (result == "1") {
					alert("Success");
					return false;
				}
				else {
					alert("ERROR");
					return false;
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function CommentAdd() {
			if (form1.TBHeadComment.value != "") {
				Admin.AddComment(form1.HBBHPk.value, form1.HAccountID.value, form1.TBHeadComment.value, function (result) { alert("완료"); location.reload(); }, function (result) { alert("ERROR : " + result); });
			}
		}
		function DO_insert() {
			if (form1.MRN.value != "") {
				Admin.AddDO(form1.HBBHPk.value, form1.HDOBLNo.value, form1.MRN.value, form1.MSN.value, form1.HAccountID.value, function (result) { alert("입력완료"); location.reload(); }, function (result) { alert("입력오류 : " + result); });
			}
		}
		function DO_update() {
			if (form1.MRN.value != "") {
				Admin.UpdateDO(form1.HBBHPk.value, form1.HDOBLNo.value, form1.MRN.value, form1.MSN.value, form1.HAccountID.value, function (result) { alert("수정완료"); location.reload(); }, function (result) { alert("수정오류 : " + result); });
			}
		}

		function GoExcelDown() {
			location.href = "../UploadedFiles/FileDownloadWithExcel.aspx?S=" + form1.HBBHPk.value;
		}
		function BTN_SendDataToCommercialDocu(BBHPk) {
			Admin.SendDateToCommercialDocu(BBHPk, function (result) {
				if (result == "1") {
					alert("SUCCESS");
				}
				else {
					alert("FAIL" + result);
					form1.DEBUG.value = result;
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function Goto(Type, TBBHPk) {
			switch (Type) {
				case "YuhanDetail":
					location.href = "/CustomClearance/YuhanView.aspx?S=" + TBBHPk;
					break;
				case "YuhanDetailN":
					location.href = "/CustomClearance/YuhanViewN.aspx?S=" + TBBHPk;
					break;
				case "DebitCredit_Detail":
					window.open('/Document/DebitCredit_Detail.aspx?S=' + TBBHPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, top=20px; left=200px; height=700px; width=800px;');
					break;
				case "Credit_View":
					window.open('/Document/DebitCredit_View.aspx?PageType=Credit&S=' + TBBHPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, top=20px; left=200px; height=700px; width=850px;');
					break;
				case "Debit_View":
					window.open('/Document/DebitCredit_View.aspx?PageType=Debit&S=' + TBBHPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, top=20px; left=200px; height=700px; width=850px;');
					break;
			}
		}

		function BTN_ExcelDown(gubun, bbhpk) {
			location.href = "../UploadedFiles/FileDownloadWithExcel.aspx?G=" + gubun + "&S=" + form1.HBBHPk.value;
		}
		function Print(Gubun, Value, OBPK) {
			switch (Gubun) {
				case "DO": location.href = "../CustomClearance/CommercialDocu_DeliveryOrder.aspx?S=" + Value + "&B=" + form1.HBBHPk.value + "&G=Print"; break;
				case "B":
					if (OBPK != "") {
						location.href = "../UploadedFiles/FileDownload.aspx?S=" + OBPK;
						break;
					} else {
						location.href = "../CustomClearance/CommercialDocu_HouseBL.aspx?S=" + Value + "&G=Print"; break;
						break;
					}
				case "OB": location.href = "../UploadedFiles/FileDownload.aspx?S=" + Value; break;
				case "B_YT": location.href = "../CustomClearance/CommercialDocu_HouseBL.aspx?S=" + Value + "&YT=YT"; break;
				case "B_YT2": location.href = "../CustomClearance/CommercialDocu_HouseBL_YT.aspx?S=" + Value; break;
				case "I": location.href = "../CustomClearance/CommercialDocu_Invoice.aspx?S=" + Value + "&G=Print"; break;
				case "P": location.href = "../CustomClearance/CommercialDocu_PackingList.aspx?S=" + Value + "&G=Print"; break;
				case "D": window.open('./Dialog/AutoPrint.aspx?G=DeliveryReceipt&S=' + Value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;'); break;
			}
		}

		function BTN_Submit_Click() {
			document.getElementById("BTN_SetStorageIn").disabled = "disabled";

			if (form1.STStorage.value == "0") { alert("창고를 지정해주세요."); return false; }
			Admin.ReceiveTransportBB(form1.HBBHPk.value, form1.HBranchPk.value, form1.STStorage.value, form1.HAccountID.value, function (result) {
				if (result == "1") { alert("Success"); location.reload(); }
				else { alert(result); form1.DEBUG.value = result; }
			}, function (result) { alert("ERROR : " + result); });
		}

		function StorageChange() {
			if (form1.STStorage.value == "0") { alert("창고를 지정해주세요."); return false; }
			Admin.ChangeStorage(form1.HBBHPk.value, form1.HBranchPk.value, form1.STStorage.value, form1.HAccountID.value, function (result) {
				if (result == "1") {
					alert("Success"); location.reload();
				}
				else {
					alert(result); form1.DEBUG.value = result;
				}
			}, function (result) { alert("ERROR : " + result); });
		}

		function TransportBBCommentDELETE(pk) {
			if (confirm("삭제하시겠습니까?")) {
				Admin.TransportBBCommentDELETE(pk, function (result) {
					if (result == "1") {
						alert("COMMENT DELETED");
						location.reload();
					} else {
						alert(result);
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}

		function receipt_confirm16_All(stepcl, BBPk) {
			if (confirm("계산서 발행합니다.")) {
				Admin.receipt_confirm16_All(stepcl, BBPk, form1.HAccountID.value, function (result) {
					if (result == "1") {
						alert("Success");
						location.reload();
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}

		function StepBack(BBPk) {
			if (confirm("예약중으로 돌리시겠습니까?")) {
				Admin.StepBack(BBPk, function (result) {
					if (result == "1") {
						alert("Success");
						location.reload();
					}
				}, function (result) { alert("ERROR : " + result); });
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
		<div style="background-color: white; width: 850px; height: 100%; padding: 25px;">
			<div id="PnDescription" style="float: left;">
				<%=Description %>
				<%=DeliveryOrder %>
				<%=CommentList %>
				<%=TransportBB_ChargeList %>
			</div>
			<div style="width: 350px; float: left; margin-left: 20px;">
				<%=BUTTON %><br />
				<%=FileList %>
			</div>
			<br />
			<div id="PnPackedList"><%=PackedList %></div>
		</div>
		<input type="hidden" id="HGubun" value="<%=Request.Params["G"] %>" />
		<input type="hidden" id="HBBHPk" value="<%=Request.Params["S"] %>" />
		<input type="hidden" id="Hstepcl" value="<%=stepcl %>" />
		<input type="hidden" id="HTransportCL" value="<%=TransportCL %>" />
		<input type="hidden" id="HDOBLNo" value="<%=DOBLNo %>" />
		<input type="hidden" id="HAccountID" value="<%=MemberInformation[2] %>" />
		<input type="hidden" id="HBranchPk" value="<%=MemberInformation[1] %>" />
		<input type="hidden" id="DEBUG" onclick="this.select();" />
		<input type="hidden" id="dhksfy" value="<%=GetGlobalResourceObject("qjsdur", "ckdrhfmfwlwjdgowntpdy") %>" />
	</form>
</body>
</html>
