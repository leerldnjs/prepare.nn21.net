<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommercialItemHistory.aspx.cs" Inherits="Admin_Dialog_CommercialItemHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
	<script type="text/javascript">
		var DA;
		window.onload = function () {
			DA = dialogArguments;
			document.getElementById("TBDescription").value = DA[2];
			document.getElementById("TBMaterial").value = DA[3];
			//form1.SSS.focus();
		}
		function Modify(ItemCode, HSCode, Description, RANDescription, RANTradingDescription, Material, TarriffRate, AdditionalTaxRate, FCN1,E1,C,Law_Nm) {
			document.getElementById("HItemCode").value = ItemCode;
			document.getElementById("TBHSCode").value = HSCode;
			document.getElementById("TBDescription").value = Description;
			document.getElementById("TBRANDescription").value = RANDescription;
			document.getElementById("TBRANTradingDescription").value = RANTradingDescription;
			document.getElementById("TBMaterial").value = Material;
			document.getElementById("TBGaunse").value = TarriffRate;
			document.getElementById("TBBugase").value = AdditionalTaxRate;
			document.getElementById("TBLaw_Nm").value = Law_Nm;
			if (FCN1 != "") {
				document.getElementById("FCN1Check").checked = true;
			} else {
				document.getElementById("FCN1Check").checked = false;
			}

			if (E1 != "") {
				document.getElementById("E1Check").checked = true;
			} else {
				document.getElementById("E1Check").checked = false;
			}
			if (C != "") {
				document.getElementById("CCheck").checked = true;
			} else {
				document.getElementById("CCheck").checked = false;
			}
			document.getElementById("TBHSCode").focus();
		}

		function Copy(ItemCode, HSCode, Description, RANDescription, RANTradingDescription, Material, TarriffRate, AdditionalTaxRate,FCN1, E1, C, Law_Nm) {
			//ItemCode 쓰지 않고 초기화 시킨다
			document.getElementById("HItemCode").value = "";
			document.getElementById("TBHSCode").value = HSCode;
			//document.getElementById("TBDescription").value = Description;
			document.getElementById("TBRANDescription").value = RANDescription;
			document.getElementById("TBRANTradingDescription").value = RANTradingDescription;
			document.getElementById("TBMaterial").value = Material;
			document.getElementById("TBGaunse").value = TarriffRate;
			document.getElementById("TBBugase").value = AdditionalTaxRate;
			document.getElementById("TBLaw_Nm").value = Law_Nm;
			if (FCN1 != "") {
				document.getElementById("FCN1Check").checked = true;
			} else {
				document.getElementById("FCN1Check").checked = false;
			}
			if (E1 != "") {
				document.getElementById("E1Check").checked = true;
			} else {
				document.getElementById("E1Check").checked = false;
			}
			if (C != "") {
				document.getElementById("CCheck").checked = true;
			} else {
				document.getElementById("CCheck").checked = false;
			}
			document.getElementById("TBHSCode").focus();
		}

		function Insert() {
			if (document.getElementById("TBDescription").value == "") {
				alert("품명은 필수입니다.");
				return false;
			}
			var FCN1Checked;
			if (document.getElementById("FCN1Check").checked + "" == "true") {
				FCN1Checked = "1";
			} else {
				FCN1Checked = "";
			}
			var E1Checked;
			if (document.getElementById("E1Check").checked + "" == "true") {
				E1Checked = "1";
			} else {
				E1Checked = "";
			}
			var CChecked;
			if (document.getElementById("CCheck").checked + "" == "true") {
				CChecked = "1";
			} else {
				CChecked = "";
			}

			Admin.InsertClearanceItemCodeKOR_HSCode(document.getElementById("HItemCode").value, document.getElementById("TBHSCode").value, document.getElementById("TBDescription").value, document.getElementById("TBRANDescription").value, document.getElementById("TBRANTradingDescription").value, document.getElementById("TBMaterial").value, document.getElementById("TBGaunse").value, document.getElementById("TBBugase").value, FCN1Checked, E1Checked, CChecked, document.getElementById("TBLaw_Nm").value, form1.HCompanyPk.value, function (result) {
				alert("Success");

				//SelectThis2(result + "#@!" + document.getElementById("TBHSCode").value + "#@!" + document.getElementById("TBDescription").value + "#@!" + document.getElementById("TBTradingDescription").value + "#@!" + document.getElementById("TBStandardModel").value + "#@!" + document.getElementById("TBMaterial").value + "#@!" + document.getElementById("TBGaunse").value + "#@!" + document.getElementById("TBBugase").value + "#@!" + E1Checked);


				SelectThis2(result + "#@!" + document.getElementById("TBDescription").value + "#@!" + document.getElementById("TBMaterial").value + "#@!" + document.getElementById("TBGaunse").value + "#@!" + document.getElementById("TBBugase").value);


			}, function (result) { alert("ERROR : " + result); });
		}
		function SelectThis(stringSum, HSCode, ItemCode) {
			
			Admin.readyTOintl2000_Law(HSCode, ItemCode, function (result) {
				alert(result);
				if (result == "1") {
					
				}
			}, function (result) { alert("ERROR : " + result); });

			window.returnValue = true;
			returnValue = document.getElementById("SUM[" + stringSum + "]").value;
			self.close();
		}
		function SelectThis2(stringSum) {
			window.returnValue = true;
			returnValue = stringSum;
			self.close();
		}
		//function SSSKD() {
		//	switch (window.event.keyCode) {
		//		case 49: SelectThis('1'); break;
		//		case 50: SelectThis('2'); break;
		//		case 51: SelectThis('3'); break;
		//		case 52: SelectThis('4'); break;
		//		case 53: SelectThis('5'); break;
		//		case 54: SelectThis('6'); break;
		//		case 55: SelectThis('7'); break;
		//		case 56: SelectThis('8'); break;
		//		case 57: SelectThis('9'); break;
		//	}
	   //}
		function DeleteAll() {
		   if (confirm("이 회사의 아이템 히스토리가 삭제됩니다.")) {
		      Admin.DeleteClearanceItemCodeKOR_All(form1.HCompanyPk.value, function (result) {
		         if (result == "1") {
		            alert("삭제완료");
		            window.returnValue = true;
		            returnValue = "N";
		            self.close();
		         }
		      }, function (result) { alert("ERROR : " + result); });
		   }
		}

		function Delete(pk) {
		   Admin.DeleteClearanceItemCodeKOR(pk, function (result) {
				if (result == "1") {										
					document.getElementById("BTNDelete[" + pk + "]").disabled = "disabled";
				}
			}, function (result) { alert("ERROR : " + result); });
		}
	</script>
	<link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
    	BODY	{	font-family:돋움,굴림,Verdana, Arial, Helvetica, geneva,sans-serif;	font-size: 14px;	color:#4F4F4F;	line-height:16px;	}
    	.tdSubT	{	border-bottom:solid 2px #93A9B8;	padding-top:10px;	}
		.td01	{	background-color:#f5f5f5; text-align:center; height:20px; width:150px;	border-bottom:dotted 1px #E8E8E8;	padding-top:2px;	padding-bottom:2px;	}
    	.td02	{	border-bottom:dotted 1px #E8E8E8;		padding-top:2px;	padding-bottom:2px;	}
		input{text-align:center; }
    </style>
</head>
<body style="background-color:#E4E4E4; width:1200px; margin:0 auto; padding-top:20px; padding-bottom:20px;" >
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
    <div style="width:1150px;   padding-left:10px; padding-right:10px;  background-color:white;">
		<%=ItemHistory %>
		<input type="hidden" id="HCompanyPk" value="<%=CompanyPk %>" />
		<input type="hidden" id="HItemCode" />
		<%--<input type="text" style="width:1px; height:1px; font-size:1px; border:0px;"  id="SSS" onkeydown="SSSKD();" />--%>
	</div>
    </form>
</body>
</html>

