<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommercialItemHistory_A.aspx.cs" Inherits="Admin_Dialog_CommercialItemHistory_A" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
	<script type="text/javascript">
		var DA;
		window.onload = function () {
			
		}
		function SelectThis(stringSum, HSCode, ItemCode) {
			alert("OK");
			var each = document.getElementById("SUM[" + stringSum + "]").value.split("#@!");
			Admin.InsertClearanceItemCodeKOR_HSCode("", each[5], "김경민", each[9], each[9], each[2], each[3], each[4], each[6], each[7], each[8], "", form1.HCompanyPk.value, function (result) {
				self.close();

			}, function (result) { alert("ERROR : " + result); });			
		}
		function SelectThis2(stringSum) {
			opener.document.getElementById("CommercialItemHistory_A").value = stringSum;
			self.close();
		}
		function Delete(pk) {
			Admin.DeleteClearanceItemCodeKOR(pk, function (result) {
				if (result == "1") {
					alert("Success");
					window.returnValue = true;
					returnValue = "N";
					self.close();
				}
			}, function (result) { alert("ERROR : " + result); });
		}

		//function onKeyDown()
		//{	
		//	if (window.event.keyCode == 13)
		//	{
		//		GoSerch();
				
		//	}
		//}


		function GoSerch() {
			
			if(document.getElementById("SerchValue").value+""!=""){
				location.href = "../Dialog/CommercialItemHistory_A.aspx?S=" + document.getElementById("HCompanyPk").value + "&Search=" + document.getElementById("SerchValue").value;
				
			}
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
<body style="background-color:#E4E4E4; width:1150px; margin:0 auto; padding-top:20px; padding-bottom:20px;" >
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
    <div style="width:1150px;   padding-left:10px; padding-right:10px;  background-color:white;">
		<input type="text" name="SerchValue" id="SerchValue"  style="width:120px;" onkeypress="if(window.event.keyCode==13) {GoSerch(); return false;}"/><input type="button" onclick="	GoSerch();" value="Search" />
		<%=ItemHistory %>
	</div>
		<input type="hidden" id="HCompanyPk" value="<%=CompanyPk %>" />
    </form>
</body>
</html>
