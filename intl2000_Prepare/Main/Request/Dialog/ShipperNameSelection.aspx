<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="ShipperNameSelection.aspx.cs" Inherits="Request_Dialog_ShipperNameSelection" meta:resourcekey="PageResource1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<script src="../../Common/public.js" type="text/javascript"></script>
	<title><%=GetGlobalResourceObject("RequestForm", "Clearance")%></title>
	<link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		var IsAdmin;
		var DR1;
		var DR2;
		var DR3;
		var DR4;
		var ShipperCode;
		var ConsigneeCode;
		var clearChk = true;
		window.onload = function () {
			DR1 = form1.D1.value;
			DR2 = form1.D2.value;
			DR3 = form1.D3.value;
			DR4 = form1.D4.value;
			S1 = form1.S1.value;
			ShipperCode = form1.ShipperCode.value;
			ConsigneeCode = form1.ConsigneeCode.value;

			if (DR1 == "Admin") {
				IsAdmin = "Y";
				Request.LoadAdminDefault(form1.HConsigneePk.value, function (result) {
					var html = "";
					if (result[0] != "0") {
						for (var i = 0; i < result.length; i++) {
							var each = result[i].split("##");
							var Sajin = new Image();
							Sajin.src = "../../UploadedFiles/" + each[5];
							var File = "";
							
							if (each[3] != "") {
								if (DR4 == "ilyt0" || DR4 == "ilic30") {
									File = "<a href='../../UploadedFiles/FileDownload.aspx?S=" + each[3] + "' ><img src=\"" + Sajin.src + "\" width=\"45\" height=\"45\" style=\"border:0px;\" /></a>" +
										   "&nbsp;<span onclick=\"FileDelete('" + each[3] + "');\" style='color:red;'>X</span>";
								}
								else {
									File = "<a href='../../UploadedFiles/FileDownload.aspx?S=" + each[3] + "' ><img src=\"" + Sajin.src + "\" width=\"45\" height=\"45\" style=\"border:0px;\" /></a>";
								}
							}
							else {
								File = "<input type=\"button\" value=\"img\" onclick=\"GoFileupload('" + each[0] + "')\" />";
							}
							if (each[6] != "0") {
								if (form1.SessionPk.value == form1.HConsigneePk.value) {
									html += "<tr >";
								} else {
									html += "<tr style='color: #fff; background-color: #428bca;'>";
								}
							} else {
								if (DR2 == "S") {
									if (ConsigneeCode.substr(-1, 1) <= 0 && ConsigneeCode.substr(-1, 1) < 3 && each[0] == "6760") {
										html += "<tr style='color: #fff; background-color: #928bca;'>";
									}
									else if (ConsigneeCode.substr(-1, 1) >= 3 && ConsigneeCode.substr(-1, 1) < 5 && each[0] == "7966") {
										html += "<tr style='color: #fff; background-color: #928bca;'>";
									}
									else if (ConsigneeCode.substr(-1, 1) >= 5 && each[0] == "5790") {
										html += "<tr style='color: #fff; background-color: #928bca;'>";
									}
									else {
										html += "<tr >"
									}
								}
								else {
									//if (ConsigneeCode.substr(-1, 1) < 5 && each[0] == "5880") {
									//	html += "<tr style='color: #fff; background-color: #928bca;'>";
									//}
									//else if (ConsigneeCode.substr(-1, 1) >= 5 && each[0] == "5790") {
									//	html += "<tr style='color: #fff; background-color: #928bca;'>";
									//}
									//else {
									html += "<tr >"
									//}
								}
							}

							html += "	<td style='text-align:center; border-bottom:dotted 1px #93A9B8; '>" + File + "</td>" +
									"	<td style='border-bottom:dotted 1px #93A9B8; '>&nbsp;&nbsp;<strong>CompanyCode:</strong>" + each[9] + "<br />&nbsp;&nbsp;<strong>Name:</strong><strong>" + each[1] + "</strong><br />&nbsp;&nbsp;<strong>Addr:</strong>" + each[2] + "<br />&nbsp;&nbsp;<strong>Name_KOR:</strong><strong>" + each[12] + "</strong><br />&nbsp;&nbsp;<strong>Address_KOR:</strong>" + each[13] + "<br />&nbsp;&nbsp;<strong>CompanyNo</strong>:" + each[7] + "<br />&nbsp;&nbsp;<strong>CutomsCode:</strong>" + each[8] + "<br />&nbsp;&nbsp;<strong>ZipCode:</strong>" + each[10] + "-" + each[11] + "</td>" +
									"	<td style='text-align:center; border-bottom:dotted 1px #93A9B8; '><input type=\"button\" style='width:60px;' value=\"select\" onclick=\"InsertSuccess2('" + result[i] + "')\" />";

							if (DR4 == "ilyt0" || DR4 == "ilic30") {
								html += "<br />" +
										"	<input type=\"button\" style='width:60px; color:red;' value=\"delete\" onclick=\"Del('" + each[0] + "')\" />" + "<br />" +
										"	<input type=\"button\" style='width:60px; color:#1DDB16;' value=\"modify\" onclick=\"modify('Admin','" + each[0] + "','" + each[1] + "','" + each[2] + "','" + each[7] + "','" + each[8] + "','" + each[9] + "','" + each[10] + "','" + each[11] + "','" + each[12] + "','" + each[13] + "')\" />";
							}
							if (each[6] != "0") {
								html += "</td></tr>";
							} else {
								if (DR4 == "ilman" || DR4 == "ilogistics" || DR4 == "ilic01" || DR4 == "ilic32" || DR4 == "ilic30" || DR4 == "ilic66" || DR4 == "ilic31" || DR4 == "ilyt0" || DR4 == "ilyw0" || DR4 == "ilgz0") {
									html += "<br />" +
	"	<input type=\"button\" style='width:60px; padding-left:0px; padding-right:0px; text-align:center; color:blue ' value=\"primary\" onclick=\"SetPrimary('" + each[0] + "')\" /><br />" +
	"	</td></tr>";
								} else {
									html += "</td></tr>";
								}
							}
						}
					}

					if ( DR4 == "ilic30" || DR4 == "ilyt0") {
						html += "	<tr><td style='text-align:center;'><strong>Common</strong></td>" +
		"	<td>&nbsp;&nbsp;CompanyCode : <input type=\"text\" id=\"Admin_CompanyCode\" style=\"text-align:left; width:165px;\" /><br/>" +
		"&nbsp;&nbsp;Name : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"text\" id=\"Admin_CompanyName\" style=\"text-align:left; width:450px;\" /><br/>" +
		"&nbsp;&nbsp;Address : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"text\" id=\"Admin_Address\" style=\"text-align:left; width:450px;\" /><br/>" +
		"&nbsp;&nbsp;Name_KOR: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"text\" id=\"Admin_Name_KOR\" style=\"text-align:left; width:450px;\" /><br/>" +
		"&nbsp;&nbsp;Address_KOR : &nbsp;&nbsp<input type=\"text\" id=\"Admin_Address_KOR\" style=\"text-align:left; width:450px;\" /><br/>" +
		"&nbsp;&nbsp;CompanyNo : &nbsp;&nbsp;&nbsp;<input type=\"text\" id=\"Admin_CompanyNo\" style=\"text-align:left; width:150px;\" />" +
		"&nbsp;&nbsp;CustomsCode : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"text\" id=\"Admin_CustomsCode\" style=\"text-align:left; width:150px;\" /><br/>" +
		"&nbsp;&nbsp;Zipcode : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"text\" id=\"Admin_ZipCode1\" style=\"text-align:left; width:50px;\" />&nbsp;-&nbsp;<input type=\"text\" id=\"Admin_ZipCode2\" style=\"text-align:left; width:50px;\" /></td>" +
		"<td ><input type=\"button\" style=\"width:70px;\" value=\"insert\" onclick=\"InsertShipperInDocument_CustomsCode('6', " + DR3 + ", form1.Admin_CompanyName.value, form1.Admin_Address.value,form1.Admin_CompanyNo.value,form1.Admin_CustomsCode.value,form1.Admin_ZipCode1.value,form1.Admin_ZipCode2.value,form1.Admin_Name_KOR.value,form1.Admin_Address_KOR.value,form1.Admin_CompanyCode.value,form1.AdminPk.value);\" /></td></tr>";
					}

					document.getElementById("PnClearanceHead").innerHTML = "<table style=\"width:710px;\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
					"<tr><td style='border-bottom:dotted 1px #93A9B8; ' >&nbsp;</td><td style='border-bottom:dotted 1px #93A9B8; '>&nbsp;</td>" +
                    "<td style='border-bottom:dotted 1px #93A9B8; '></td></tr>" + html + "</table>";
				}, function (result) { alert("ERROR : " + result); });
			}

			else {
				document.getElementById("PnClearanceHead").innerHTML = "" +
					"<input type=\"button\" style=\"width:92px;\" value=" + form1.HBTNClearanceSubstitute.value + " onclick=\"InsertSuccess('1')\" />&nbsp;&nbsp;" + form1.HDescriptionClearanceSubstitute.value;
			}
		}
		function FileDelete(filepk) {
			if (confirm('삭제하시겠습니까? \r\n 복구되지 않습니다.')) {
				Request.FileDeleteWithGubunPk(filepk, function (result) {
					if (result == "1") {
						alert("Success");
						window.returnValue = true;
						returnValue = "D";
						self.close();
					}
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function SetTalkBusiness() {
			//40 배달
			//41 통관명
			//42 정산서 
			var GubunCL = 'ShipperSelection'
			Request.SetTalkBusiness(form1.S1.value, form1.D4.value, form1.TBContents.value, GubunCL, function (result) {
				if (result == "1") {
					alert("SUCCESS");
					location.reload();
				}
			}, function (result) { return false; });
		}
		function GoFileupload(pk) {
			window.open('../../Admin/Dialog/FileUpload.aspx?G=3&S=' + pk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;');
		}
		function InsertShipperInDocument(gubunCL, pk, name, addr) {
			if (name == "") { alert(form1.HMustCompanyName.value); return false; }
			if (addr == "") { alert(form1.HMustAddress.value); return false; }
			Request.InsertShipperNameInDocument(gubunCL, pk, name, addr, InsertSuccess, function (result) { alert("ERROR : " + result); });
		}
		function checkByte(frm, limitByte) {
			clearChk = true;
			var totalByte = 0;

			for (var i = 0; i < frm.length; i++) {
				var currentByte = frm.charCodeAt(i);
				if (currentByte > 128) totalByte += 2;
				else totalByte++;
			}

			if (totalByte > limitByte) {
				alert(limitByte + "바이트까지 전송가능합니다.");
				clearChk = false;
			}
		}
		function InsertShipperInDocument_CustomsCode(gubunCL, pk, name, addr, CompanyNo, CustomsCode, ZipCode1, ZipCode2, Name_KOR, Addr_KOR, CompanyCode, HiddenPk) {
			if (name == "") { alert(form1.HMustCompanyName.value); return false; }
			if (addr == "") { alert(form1.HMustAddress.value); return false; }

			name = name.replace(/'/gi, "`");
			addr = addr.replace(/'/gi, "`");
			Name_KOR = Name_KOR.replace(/'/gi, "`");
			Addr_KOR = Addr_KOR.replace(/'/gi, "`");

			checkByte(Addr_KOR,'70');
			if (!clearChk) {
				return false;
			}

			Request.InsertShipperNameInDocument_CustomsCode(gubunCL, pk, name, addr, CompanyNo, CustomsCode, ZipCode1, ZipCode2, Name_KOR, Addr_KOR, CompanyCode, HiddenPk, InsertSuccess, function (result) { alert("ERROR : " + result); });
		}

		function InsertSuccess2(result) {
			var each = result.split("##");
			Request.CompanyInDocument_CompanyNoAdd(each[0], function (result) {
				if (result != "") {
					opener.SelectCompanyInDocumentOpen(result, DR2)
					self.close();
				}
			}, function (result) { alert("ERROR : " + result); });
		}

		function InsertSuccess(result) {
			opener.SelectCompanyInDocumentOpen(result, DR2)
			self.close();
		}

		function SetPrimary(TargetPk) {
			if (confirm("really Set Primary this?")) {
				Request.SetPrimaryShipperNameInDocument(form1.HConsigneePk.value, TargetPk, function (result) {
					if (result == "1") {
						alert("OK"); window.returnValue = true; returnValue = 'D'; self.close();
					}
					else { alert(form1.HError.value); }
				}, function (result) { alert("ERROR : " + result); });
			}
		}

		function Del(target) {
			if (confirm("really delete this?")) {
				Request.DelShipperNameInDocument(target, function (result) {
					if (result == "1") { alert(form1.HDelComplete.value); window.returnValue = true; returnValue = 'D'; self.close(); }
					else { alert(form1.HError.value); }
				}, function (result) { alert("ERROR : " + result); });
			}
		}
		function modify(TYPE, target, Name, Address, CompanyNo, CustomsCode, CompanyCode, ZipCode1, ZipCode2, Name_KOR, Address_KOR) {
			if (TYPE + "" == "Admin") {
				form1.AdminPk.value = target;
				form1.Admin_CompanyName.value = Name;
				document.getElementById("Admin_CompanyName").disabled = true;
				form1.Admin_Address.value = Address;
				document.getElementById("Admin_Address").disabled = true;
				form1.Admin_CompanyNo.value = CompanyNo;
				form1.Admin_CustomsCode.value = CustomsCode;
				form1.Admin_CompanyCode.value = CompanyCode;
				form1.Admin_ZipCode1.value = ZipCode1;
				form1.Admin_ZipCode2.value = ZipCode2;
				form1.Admin_Name_KOR.value = Name_KOR;
				form1.Admin_Address_KOR.value = Address_KOR;
			} else {
				form1.CustomerClearancePk.value = target;
				form1.CustomerClearanceCompanyName.value = Name;
				document.getElementById("CustomerClearanceCompanyName").disabled = true;
				form1.CustomerClearanceAddress.value = Address
				document.getElementById("CustomerClearanceAddress").disabled = true;
				form1.CustomerClearanceCompanyNo.value = CompanyNo;
				form1.CustomerClearanceCustomsCode.value = CustomsCode;
				form1.CustomerClearanceCompanyCode.value = CompanyCode;
				form1.CustomerClearanceZipCode1.value = ZipCode1;
				form1.CustomerClearanceZipCode2.value = ZipCode2;
				form1.CustomerClearanceName_KOR.value = Name_KOR;
				form1.CustomerClearanceAddress_KOR.value = Address_KOR;
			}
		}
	</script>
</head>
<body style="padding: 15px; width: 750px;">
	<form id="form1" runat="server">
		<asp:ScriptManager ID="RequestFormService" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Request.asmx" />
			</Services>
		</asp:ScriptManager>
		<fieldset>
			<legend><strong><%=GetGlobalResourceObject("RequestForm", "Clearance") %></strong></legend>
			<div style="line-height: 20px; padding: 10px;">
				<input type="hidden" id="SessionPk" value='<%=Request.Params["S"] %>' />
				<input type="hidden" id="HConsigneePk" value='<%=ConsigneePk %>' />

				<input type="hidden" id="HMustCompanyName" value="<%=GetGlobalResourceObject("Alert", "MustCompanyName") %>" />
				<input type="hidden" id="HMustAddress" value="<%=GetGlobalResourceObject("Alert", "MustAddress") %>" />
				<input type="hidden" id="HBTNClearanceSubstitute" value="<%=GetGlobalResourceObject("RequestForm", "ClearanceSubstitute")  %>" />
				<input type="hidden" id="HDescriptionClearanceSubstitute" value="<%=GetGlobalResourceObject("RequestForm", "ClearanceSubstituteEx")%>" />
				<input type="hidden" id="qjsdurCompanyName" value="<%=GetGlobalResourceObject("Member", "CompanyName")%>" />
				<input type="hidden" id="qjsdurCompanyAddress" value="<%=GetGlobalResourceObject("Member", "Address")%>" />
				<div id="PnClearanceHead"></div>
			</div>
		</fieldset>
		<p></p>
		<fieldset>
			<legend><strong><%= GetGlobalResourceObject("RequestForm", "ClearanceSelf")%></strong>&nbsp;&nbsp;<%= GetGlobalResourceObject("RequestForm", "ClearanceSelfEx")%></legend>
			<div style="padding: 10px; line-height: 20px;">
				<%=ShipperData %>
			</div>
		</fieldset>
		<fieldset>
			<legend><strong>Attention</strong></legend>
			<span><%=Registerd %></span><br />
			<textarea id="TBContents" rows="5" cols="90"><%=Contents %></textarea>
			&nbsp;<input type="button" value="Save" onclick="SetTalkBusiness();" />
		</fieldset>
		<input type="hidden" id="HError" value="<%= GetGlobalResourceObject("Alert", "CallError") %>" />
		<input type="hidden" id="HDelComplete" value="<%=GetGlobalResourceObject("Alert", "DeleteComplete") %>" />
		<input type="hidden" id="D1" value="<%=Request.Params["D1"] %>" />
		<input type="hidden" id="D2" value="<%=Request.Params["D2"] %>" />
		<input type="hidden" id="D3" value="<%=Request.Params["D3"] %>" />
		<input type="hidden" id="D4" value="<%=Request.Params["D4"] %>" />
		<input type="hidden" id="S1" value="<%=Request.Params["S"] %>" />
		<input type="hidden" id="ShipperCode" value="<%=ShipperCode %>" />
		<input type="hidden" id="ConsigneeCode" value="<%=ConsigneeCode %>" />
		<input type="hidden" id="AdminPk" />
		<input type="hidden" id="CustomerClearancePk" />
	</form>
</body>
</html>
