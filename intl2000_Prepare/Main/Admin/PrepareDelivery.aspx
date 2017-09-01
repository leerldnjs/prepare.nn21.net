<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrepareDelivery.aspx.cs" Inherits="Admin_PrepareDelivery" Debug="true" %>
<%@ Register src="LogedWithoutRecentRequest11.ascx" tagname="LogedWithoutRecentRequest11" tagprefix="uc1" %>
<%@ Register src="../CustomClearance/Loged.ascx" tagname="Loged" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />    
	<script type="text/javascript">
		function GotoDifferantStorage(Storage) {
			location.href = "PrepareDelivery.aspx?S=" + Storage;
		}
		function InvoiceExcelDown(CDPk) {
			location.href = "../UploadedFiles/FileDownloadWithExcel.aspx?G=InvoiceItem&S=" + CDPk;
		}
		function CollectPayment(requestformpk, accountid, gubun, companypk)
		{
			var dialogArgument = new Array();
			dialogArgument[0] = requestformpk;
			dialogArgument[1] = accountid;
			dialogArgument[2] = gubun;
			dialogArgument[3] = companypk;
			var retVal = window.showModalDialog('./Dialog/CollectPayment.aspx', dialogArgument, "dialogHeight:500px; dialogWidth:480px; resizable:1; status:0; scroll:1; help:0; ");
			if (retVal == "Y") { window.document.location.reload(); }
		}
		function Goto(gubun, value) {
			switch (gubun) {
				case "Storage": location.href = "./PrepareDelivery.aspx?S=" + value; break;
				case "TBBPk": location.href = "./TransportBetweenBranchView.aspx?S=" + value; break;
				case "RequestForm": location.href = "./RequestView.aspx?g=c&pk=" + value; break;
				case "Company": location.href = "./CompanyInfo.aspx?M=View&S=" + value; break;
				case "CheckDescription": location.href = "./CheckDescription.aspx?S=" + value; break;
			}
		}
		//onclick=\"AddDelivery('" + RS["RequestFormPk"] + "', '" + RS["ConsigneePk"] + "', '" + CompanyPk + "');\" 
		function AddDelivery(requestformpk, consigneepk, branchpk) {
			window.open('./Dialog/DeliverySet.aspx?P=N&S=' + requestformpk + "&C=" + consigneepk + "&O=" + branchpk + "&A=" + form1.HAccountID.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=600px; width=800px;');
		}
		function DeliverySet(ourbranchstorageoutpk, requestformpk, consigneepk, branchpk) {
			window.open('./Dialog/DeliverySet.aspx?P=' + ourbranchstorageoutpk + '&S=' + requestformpk + "&C=" + consigneepk + "&O=" + branchpk + "&A=" + form1.HAccountID.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=600px; width=800px;');
		}

		function DeliveryModify(OurBranchStorageOutPk, requstformpk, consigneepk, branchpk) {
			window.open('./Dialog/DeliverySet.aspx?P=' + OurBranchStorageOutPk + '&S=' + requstformpk + "&C=" + consigneepk + "&O=" + branchpk + "&A=" + form1.HAccountID.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=600px; width=800px;');
		}
		function DeliveryPrint(pk) {
			window.open('./Dialog/DeliveryReceipt2.aspx?G=Print&S=' + pk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=500px; width=900px;');
		}
		function DeliveryCancel(tbcpk) {
			Admin.CancelDeliveryOrder(tbcpk, function (result) {
				if (result == "1") {
					alert("SUCCESS");
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function DocumentStepCLTo(btnID,toStepCL, pk) {
			var confirmcomment;
			switch (toStepCL) {
				case "6": confirmcomment = "통관지시하시겠습니까?"; break;
				case "7": confirmcomment = "검사생략 입니다."; break;
				case "8": confirmcomment = "서류제출 입니다."; break;
				case "9": confirmcomment = "심물검사 입니다."; break;
				case "10": confirmcomment = "세금납부 지시합니다."; break;
				case "11": confirmcomment = "세금납부 지시합니다."; break;
				case "12": confirmcomment = "세금납부 지시합니다."; break;
				case "13": confirmcomment = "면허났습니다."; break;
				case "14": confirmcomment = "면허났습니다."; break;
				case "15": confirmcomment = "면허났습니다."; break;
			}
			if (confirm(confirmcomment)) {
				Admin.SetDocumentStepCLTo(pk, toStepCL, form1.HAccountID.value, function (result) {
					if (result == "1") {
						alert("SUCCESS");

						switch (toStepCL) {
							case "7":
								document.getElementById(btnID).disabled = "disabled";
								document.getElementById(btnID + "0").disabled = "disabled";
								document.getElementById(btnID + "00").disabled = "disabled";
								break;
							case "8":
								document.getElementById(btnID).disabled = "disabled";
								document.getElementById(btnID.toString().substr(0, btnID.toString().length - 1)).disabled = "disabled";
								document.getElementById(btnID + "0").disabled = "disabled";
								break;
							case "9":
								document.getElementById(btnID).disabled = "disabled";
								document.getElementById(btnID.toString().substr(0, btnID.toString().length - 1)).disabled = "disabled";
								document.getElementById(btnID.toString().substr(0, btnID.toString().length - 2)).disabled = "disabled";
								break;
							default: document.getElementById(btnID).disabled = "disabled"; break;
						}
					}

				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function GoDeliveryOrder(tbcpk, requestformpk, accountid, isDeposited) {
			if (confirm("출고지시하시겠습니까?")) {
				Admin.GoDeliveryOrder(tbcpk, requestformpk, accountid, isDeposited, function (result) {
					if (result == "1") { alert("출고지시완료"); location.reload(); }
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function Print(Gubun, Value) {
			switch (Gubun) {
				case "B": location.href = "../CustomClearance/CommercialDocu_HouseBL.aspx?S=" + Value + "&G=Print"; break;
				case "I": location.href = "../CustomClearance/CommercialDocu_Invoice.aspx?S=" + Value + "&G=Print"; break;
				case "P": location.href = "../CustomClearance/CommercialDocu_PackingList.aspx?S=" + Value + "&G=Print"; break;
				case "D": window.open('./Dialog/AutoPrint.aspx?G=DeliveryReceipt&S=' + Value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;'); break;
			}
		}
		function setHighlight(GubunCL, gubun, value) {
			Admin.SetHighlight(GubunCL, gubun, value, "1", function (result) {
				if (result == "1") {
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
	</script>
</head>
<body style="background-color:#E4E4E4; width:1100px; margin:0 auto; padding-top:10px;" >
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server" ><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
	<uc2:Loged ID="Loged1" runat="server" Visible="false" />
	<uc1:LogedWithoutRecentRequest11 ID="LogedWithoutRecentRequest111" runat="server" />
    <div style="background-color:White; width:1050px; height:100%; padding:25px;">
		<div><%=HTMLHeader %></div>
		<div><%=HTMLBody %></div>
		<input type="hidden" id="HAccountID" value="<%=MEMBERINFO[2] %>"   />
    </div>
    </form>
</body>
</html>