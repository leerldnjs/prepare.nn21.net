<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnCustomerAdd.aspx.cs" Inherits="Request_Dialog_OwnCustomerAdd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=GetGlobalResourceObject("RequestForm", "ConsigneeAdd") %></title>
	<style type="text/css">
		.tdSubT { border-bottom-width:2px; border-bottom-style:solid; border-bottom-color:#93A9B8; padding-top:30px; padding-bottom:3px; }
		.td01{background-color:#f5f5f5; text-align:center; height:25px; width:150px;padding-top:4px; padding-bottom:4px; border-bottom:1px dotted #E8E8E8;}
		.td02{width:250px; padding-top:4px;padding-bottom:4px; padding-left:10px; border-bottom:1px dotted #E8E8E8; background-color:White;	}
		.td023{width:740px; padding-top:4px;padding-bottom:4px; padding-left:10px;border-bottom:1px dotted #E8E8E8;background-color:White;}
		.tdStaffBody{text-align:center; padding:5px; border:dotted 1px #E8E8E8; background-color:White;}
    </style>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script src="../../Common/RegionCode.js?version=20131029" type="text/javascript"></script>
	<script type="text/javascript">
		var isAdmin;
		var TARGETCOMPANYPK;
		var COMPANYPK;
		var GUBUNCL;
		var ACCOUNTID;
		var AccountType;
		var BranchPk;
		window.onload = function () {
			var uri = location.href;
			if (uri.indexOf("?") == -1) {
				isAdmin = "N";
				var DA = new Array();
				DA = dialogArguments.toString().split("!");
				COMPANYPK = DA[0];
				ACCOUNTID = DA[1];
				if (DA.length > 2) {
					AccountType = DA[2];
					BranchPk = DA[3];
				}
			}
			else {
				document.getElementById("PnSubmit").style.visibility = "hidden";
				document.getElementById("PnAdminOnly").innerHTML = "고객번호 : <input type=\"text\" id=\"ACompanyCode\" /><input type=\"button\" value=\"검색\" onclick=\"SerchForAdmin(form1.ACompanyCode.value);\" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"button\" value=\"검색된 고객을 연결합니다.\" onclick=\"CustomerAddSubmit();\" />";
				isAdmin = "Y";
				DA = new Array();
				var querystring = uri.split("?");
				var eachQuerystring = querystring[1].split('&');
				for (var i = 0; i < eachQuerystring.length; i++) {
					switch (eachQuerystring[i].substr(0, 1)) {
						case 'S': COMPANYPK = eachQuerystring[i].substr(2); break;
						case 'A': ACCOUNTID = eachQuerystring[i].substr(2); break;
					}
				}
			}
		}
		function CustomerAddSubmit() {
			if (isAdmin == "Y") {
				if (confirm("기존화주와 검색된 화주가 서로 연결됩니다. 서로의 거래처에 추가됩니다.")) {
					var companytype;
					if (form1.CompanyType0.checked) {
						companytype = "0";
					}
					else {
						companytype = "1";
					}
					Request.OwnCustomerAddWithNoRequest(companytype, COMPANYPK, TARGETCOMPANYPK, function (result) {
						if (result == "1") {
							alert("완료");
							self.close();
						}
						else {
							alert(result);
						}
					}, function (result) { window.alert(result); });
				}
				return false;
			}
			var companytype;
			if (form1.CompanyType0.checked) {
				companytype = "0";
			}
			else {
				companytype = "1";
			}
			
			var companyName = form1.TB_CompanyName.value;
			var regionCode = form1.gcodeExport.value;
			var companyAddress = form1.TB_CompanyAddress.value;
			var TEL = form1.TB_CompanyTEL.value;
			var FAX = form1.TB_CompanyFAX.value;
			var Email = form1.TB_CompanyEmail.value;
			var Homepage = form1.TB_CompanyHomepage.value;
			var TabStaffRowLength = document.getElementById("TabStaff").rows.length - 2;
			var StaffSum = "";
			for (var i = 0; i < TabStaffRowLength; i++) {
				if (document.getElementById("TBStaffName[" + i + "]").value != "" ||
					document.getElementById("TBStaffTEL[" + i + "]").value != "" ||
					document.getElementById("TBStaffEmail[" + i + "]").value != "" ||
					document.getElementById("TBStaffMobile0[" + i + "]").value != "") {
					StaffSum += document.getElementById("TBStaffDuties[" + i + "]").value + "#@!" +
										document.getElementById("TBStaffName[" + i + "]").value + "#@!" +
										document.getElementById("TBStaffTEL[" + i + "]").value + "#@!" +
										document.getElementById("TBStaffMobile0[" + i + "]").value + "#@!" +
										document.getElementById("TBStaffEmail[" + i + "]").value + "%!$@#";
				}
			}
			var Memo = form1.TBMemo.value;
			if (Memo == "MEMO") { Memo = ""; }
			Request.OwnCustomerAdd(companytype, COMPANYPK, ACCOUNTID, companyName, regionCode, companyAddress, TEL, FAX, Email, form1.TB_PresidentName.value, Homepage, Memo, StaffSum, function (result) {
				if (result == "1") {
					alert("완료");
					window.returnValue = true;
					returnValue = companyName + "##New##" + result;
					self.close();
				}
				else {
					alert(result);
					form1.DEBUG.value = result;
					return false
				}
			}, function (result) { alert("ERROR : " + result); });

		}
		function AutoCompanyCodeSuccess(result) {
			form1.CompanyCode2.value = result;
			form1.CompanyCode2.select();
			form1.BTN_Submit.style.visibility = "visible";
			//form1.CompanyCode2.focus();
		}
		function CheckAutoNum(which, value) {
			if (value.length == 4) {
				Admin.AutoCompanyCode(value, function (result) {
					document.getElementById(which).value = result;
					document.getElementById(which).select();
					document.getElementById(which).focus();
					//alert(result);
				}, function (result) {
					alert(result);
				});
			}
			else { return false; }
		}
		function SerchForAdmin(value) {
			Request.LoadCompanyInfForCompanyCode(value, function (result) {
				if (result[0] == "N") {
					alert("해당고객번호는 비어있습니다."); return false;
				}
				var StaffRowCount = 0;
				for (var i = 0; i < result.length; i++) {
					var each = result[i].split("#@!");
					if (i == 0) {
						TARGETCOMPANYPK = each[0];
						form1.TB_CompanyName.value = each[1];
						form1.TB_CompanyAddress.value = each[2];
						form1.TB_CompanyTEL.value = each[3];
						form1.TB_CompanyFAX.value = each[4];
						form1.TB_PresidentName.value = each[5];
						form1.TB_CompanyEmail.value = each[6];
						GUBUNCL = each[7];
						if (GUBUNCL == "0") {
							document.getElementById("CompanyType0").disabled = "disable";
							document.getElementById("CompanyType1").checked = "checked";
						}
						else {
							document.getElementById("CompanyType0").disabled = "";
							document.getElementById("CompanyType1").checked = "checked";
						}
						document.getElementById("spRegionName").innerHTML = "";
						form1.TB_CompanyHomepage.value = each[10];
					}
					else {
						document.getElementById("TBStaffDuties[" + StaffRowCount + "]").value = each[0];
						document.getElementById("TBStaffName[" + StaffRowCount + "]").value = each[1];
						document.getElementById("TBStaffTEL[" + StaffRowCount + "]").value = each[2];
						if (each[3] != "") {
							document.getElementById("TBStaffMobile0[" + StaffRowCount + "]").value = each[3];
						}
						document.getElementById("TBStaffEmail[" + StaffRowCount + "]").value = each[4];
						StaffRowCount++;
					}
				}
			}, function (result) { alert("ERROR : " + result); });
		}
	</script>
</head>
<body style="background-color:#E4E4E4; width:800px; margin:0 auto; padding-top:20px; padding-bottom:20px;"  >
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Request.asmx" /></Services></asp:ScriptManager>
		<div id="PnAdminOnly"></div>
		<input type="hidden" id="DEBUG" onclick="this.select();" />
		<div style="background-color:#999999; padding:7px;">
			<span style="color:White; font-weight:bold"><img src="../../Images/ico_arrow.gif" alt="" /><%=GetGlobalResourceObject("RequestForm", "ConsigneeAdd") %></span>
		</div>
		<div style="background-color:#777777; height:1px; font-size:1px; "></div>
		<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
		<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
		<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
		<div style="background-color:#EEEEEE; height:1px; font-size:1px; "></div>
		<table border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td style="background-color:#f5f5f5; text-align:center; height:25px; width:150px;padding-top:4px; padding-bottom:4px;" >
					Type
				</td>
				<td colspan="3" class="td023">&nbsp; 
					<input type="radio" name="CompanyType" id="CompanyType1" value="1" checked="checked" /><label for="CompanyType1"><%= GetGlobalResourceObject("qjsdur", "rjfocj")%></label> 
					&nbsp;&nbsp;&nbsp;
					<input type="radio" name="CompanyType" id="CompanyType0" value="0" /><label for="CompanyType0"><%= GetGlobalResourceObject("qjsdur", "rhksfldjqcp")%></label>
				</td>
			</tr>
			<tr>
				<td class="td01"><%= GetGlobalResourceObject("Member", "CompanyName")%></td>
				<td class="td02"><input type="text" id="TB_CompanyName" style="width:200px;" /></td>
				<td class="td01"><%= GetGlobalResourceObject("Member", "PresidentName")%></td>
				<td class="td02"><input type="text" id="TB_PresidentName" style="width:100px;" /></td>
			</tr>
			<tr>
				<td style="background-color:#f5f5f5; text-align:center; height:25px; width:150px;padding-top:4px; padding-bottom:4px;" >
					<%=GetGlobalResourceObject("Member", "CompanyAddress") %></td>
				<td colspan="3" class="td023">
					<input type="hidden" id="gcodeExport" value="" />
					<span id="spRegionName">
					<select id="regionCodeI1" style="width:100px;" name="country" size="1" onchange="cate1change('regionCodeI1','regionCodeI2','regionCodeI3',this.form, this.selectedIndex,'gcodeExport')">
						<option value="">:: <%=GetGlobalResourceObject("Member", "SelectionCountry2") %> ::</option>
					</select>
					<select id="regionCodeI2" style="width:140px;" name="office" onchange="cate2change('regionCodeI1','regionCodeI2','regionCodeI3',this.form,this.selectedIndex,'gcodeExport')">
						<option value="">:: <%=GetGlobalResourceObject("Member", "SelectionArea") %> ::</option>
					</select>
					<select id="regionCodeI3" style="width:140px;" name="area" onchange="cate3change('regionCodeI1','regionCodeI2','regionCodeI3',this.form,this.selectedIndex,'gcodeExport')">
						<option value="">:: <%=GetGlobalResourceObject("Member", "SelectionDetailArea") %> ::</option>
					</select>
					<input type="hidden" id="HSelectionArea" value=":: <%=GetGlobalResourceObject("Member", "SelectionArea") %> ::" />
					<input type="hidden" id="HSelectionDetailArea" value=":: <%=GetGlobalResourceObject("Member", "SelectionDetailArea") %> ::" />
					</span>
					<div style="padding-top:4px;"><input type="text" id="TB_CompanyAddress" style="width:500px;" /></div>
				</td>
			</tr>
			<tr>
				<td class="td01">TEL</td><td class="td02"><input type="text" id="TB_CompanyTEL" /></td>
				<td class="td01">FAX</td><td class="td02"><input type="text" id="TB_CompanyFAX" /></td>
			</tr>
			<tr>
				<td class="td01">E-mail</td><td class="td02"><input type="text" id="TB_CompanyEmail" size="30"  /></td>
				<td class="td01"><%=GetGlobalResourceObject("Member", "Homepage") %></td><td class="td02"><input type="text" id="TB_CompanyHomepage" style="width:230px;" /></td>
			</tr>
		</table>
		<div style="background-color:#777777; height:1px; font-size:1px; "></div>
		<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
		<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
		<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
		<div style="background-color:#EEEEEE; height:1px; font-size:1px; "></div>
		<div style="background-color:#FFFFFF; height:1px; font-size:1px; "></div>
		<table id="TabStaff" style="background-color:White;width:800px;"  border="0" cellpadding="0" cellspacing="0">
			<thead>
				<tr><td class="tdSubT" colspan="5">&nbsp;&nbsp;&nbsp;<strong>Staff</strong></td></tr>
				<tr style="height:30px;" >
					<td bgcolor="#F5F5F5" height="20" align="center" ><%= GetGlobalResourceObject("Member", "Duties") %></td>
					<td bgcolor="#F5F5F5" align="center" ><%=GetGlobalResourceObject("Member", "Name") %></td>
					<td bgcolor="#F5F5F5" align="center" >TEL</td >
					<td bgcolor="#F5F5F5" align="center" >Mobile</td>
					<td bgcolor="#F5F5F5" align="center" >E-mail</td >
				</tr>
			</thead>
			<tbody>
				<tr style="height:30px;">
					<td align="center"><input type="text" id="TBStaffDuties[0]" style="width:80px; text-align:center;" /></td>
					<td align="center"><input type="text" id="TBStaffName[0]" style="width:100px;" /></td>
					<td align="center"><input type="text" id="TBStaffTEL[0]" style="width:120px;" /></td>
					<td align="center"><input type="text" id="TBStaffMobile0[0]" /></td>
					<td align="center" ><input type="text" id="TBStaffEmail[0]" style="width:175px;" /></td>
				</tr>
				<tr style="height:30px;">
					<td align="center"><input type="text" id="TBStaffDuties[1]" style="width:80px; text-align:center;" /></td>
					<td align="center"><input type="text" id="TBStaffName[1]" style="width:100px;" /></td>
					<td align="center"><input type="text" id="TBStaffTEL[1]" style="width:120px;" /></td>
					<td align="center"><input type="text" id="TBStaffMobile0[1]" /></td>
					<td align="center" ><input type="text" id="TBStaffEmail[1]" style="width:175px;" /></td>				
				</tr>
				<tr style="height:30px;">
					<td align="center"><input type="text" id="TBStaffDuties[2]" style="width:80px; text-align:center;" /></td>
					<td align="center"><input type="text" id="TBStaffName[2]" style="width:100px;" /></td>
					<td align="center"><input type="text" id="TBStaffTEL[2]" style="width:120px;" /></td>
					<td align="center"><input type="text" id="TBStaffMobile0[2]" /></td>
					<td align="center" ><input type="text" id="TBStaffEmail[2]" style="width:175px;" /></td>
				</tr>
			</tbody>
		</table>
		<div style="background-color:#777777; height:1px; font-size:1px; "></div>
		<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
		<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
		<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
		<div style="background-color:#EEEEEE; height:1px; font-size:1px; "></div>
		<div style="background-color:#FFFFFF; height:1px; font-size:1px; "></div>
		<div id="PnSubmit" style="width:800px; background-color:#FFFFFF; padding-top:20px; padding-bottom:15px; text-align:center; ">
			<div style="float:left; margin-left:50px; "><textarea id="TBMemo" cols="60" rows="3" onfocus="this.select();" style="overflow:hidden;" >MEMO</textarea></div>
			<input type="button" id="BTN_Submit" style="width:200px; height:50px; " value="<%=GetGlobalResourceObject("Member", "Submit") %>" onclick="CustomerAddSubmit()" />
		</div>
		<input type="hidden" id="apdlfgudtlrdlakwwldksgtmqslek" value="<%=GetGlobalResourceObject("qjsdur", "apdlfgudtlrdlakwwldksgtmqslek") %>" />
    </form>
</body>
</html>