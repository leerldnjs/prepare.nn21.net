<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="CustomerAdd.aspx.cs" Inherits="Admin_CustomerAdd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=GetGlobalResourceObject("Admin", "CustomerAddTitle")%></title>
	<style type="text/css">
		.tdSubT { border-bottom-width:2px; border-bottom-style:solid; border-bottom-color:#93A9B8; padding-top:30px; padding-bottom:3px; }
		.td01{background-color:#f5f5f5; text-align:center; height:25px; width:150px;padding-top:4px; padding-bottom:4px; border-bottom:1px dotted #E8E8E8;}
		.td02{width:250px; padding-top:4px;padding-bottom:4px; padding-left:10px; border-bottom:1px dotted #E8E8E8; background-color:White;	}
		.td023{width:740px; padding-top:4px;padding-bottom:4px; padding-left:10px;border-bottom:1px dotted #E8E8E8;background-color:White;}
		.tdStaffBody{text-align:center; padding:5px; border:dotted 1px #E8E8E8; background-color:White;}
    </style>
    <link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		var RESPONSIBLESTAFF;
		var IsThirdRegion = false;
		var GUBUNCL = "1";


		window.onload = function () {
			if (document.getElementById("PageMode").value == "ShippingBranch") {
				if (document.getElementById("BranchPk").value == "11773") {
					document.getElementById("CompanyCode1").value = "B1";
					document.getElementById("CompanyCode1").readOnly = "readOnly";
				}
			} else {
				var DA = dialogArguments.toString().split("!");
				form1.HAccountID.value = DA[0];
				RESPONSIBLESTAFF = DA[0];
			}
			LoadRegionCodeCountry("RegionCode[0]");
		}

		//0	본인이 가입 
		//1	관리자 임의등록
		//2	SUB
		function delopt(selectCtrl, mTxt) {
			if (selectCtrl.options) {
				for (i = selectCtrl.options.length; i >= 0; i--) {
					selectCtrl.options[i] = null;
				}
			}
			selectCtrl.options[0] = new Option(mTxt, "")
			selectCtrl.selectedIndex = 0;
		}

		function LoadRegionCodeCountry(selectID) {
			delopt(document.getElementById(selectID), ":: Country ::");
			Admin.LoadReginoCodeCountry(function (result) {
				for (var i = 0; i < result.length; i++) {
					var thisR = result[i].split("$");
					document.getElementById(selectID).options[i + 1] = new Option(thisR[1], thisR[0]);
				}
			}, function (result) { alert("ERROR : " + result); });
		}

		function LoadRegionCode(thisC) {
			if (thisC == "0") {
				delopt(document.getElementById("RegionCode[1]"), ":: Area ::");
				delopt(document.getElementById("RegionCode[2]"), ":: Detail ::");
			}
			else if (thisC == "1" && IsThirdRegion == true) {
				delopt(document.getElementById("RegionCode[2]"), ":: Detail ::");
			}

			if (thisC == "0" || IsThirdRegion == true) {
				Admin.LoadRegionCode(document.getElementById("RegionCode[" + thisC + "]").value, function (result) {
					if (thisC == "0") {
						if (result[0].substr(0, 1) == "7") {
							document.getElementById("RegionCode[1]").style.visibility = "visible";
							document.getElementById("RegionCode[2]").style.visibility = "visible";
							IsThirdRegion = true;
						}
						else {
							document.getElementById("RegionCode[1]").style.visibility = "visible";
							document.getElementById("RegionCode[2]").style.visibility = "hidden";
							IsThirdRegion = false;
						}
					}
					for (var i = 1; i < result.length; i++) {
						var thisR = result[i].split("$");
						document.getElementById("RegionCode[" + (parseInt(thisC) + 1) + "]").options[i] = new Option(thisR[1], thisR[0]);
					}
				}, function (result) { alert("ERROR : " + result); });
			}
			form1.RegionCode.value = document.getElementById("RegionCode[" + thisC + "]").value;
		}

		function InsertWarehouse() {		// 상품추가
			var objTable = document.getElementById("TabWarehouse");
			objTable.appendChild(document.createElement("TBODY"));
			var lastRow = objTable.rows.length;
			var thisLineNo = lastRow - 2;
			var objRow1 = objTable.insertRow(lastRow);
			var objCell1 = objRow1.insertCell(); var objCell2 = objRow1.insertCell(); var objCell3 = objRow1.insertCell(); var objCell4 = objRow1.insertCell(); var objCell5 = objRow1.insertCell();
			objCell1.align = "center"; objCell2.align = "center"; objCell3.align = "center"; objCell4.align = "center"; objCell5.align = "center";
			objCell1.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][0]\" style=\"width:80px;\" />";
			objCell2.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][1]\" style=\"width:380px;\" />";
			objCell3.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][2]\" style=\"width:110px;\" />";
			objCell4.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][3]\" style=\"width:60px;\" />";
			objCell5.innerHTML = "<input type=\"text\" id=\"TBWarehouse[" + thisLineNo + "][4]\" style=\"width:110px;\" />";
		}

		function InsertStaff() {
			var objTable = document.getElementById("TabStaff");
			objTable.appendChild(document.createElement("TBODY"));
			var lastRow = objTable.rows.length;
			var thisLineNo = lastRow - 2;
			var objRow1 = objTable.insertRow(lastRow);
			var objCell1 = objRow1.insertCell();
			var objCell2 = objRow1.insertCell();
			var objCell3 = objRow1.insertCell();
			var objCell4 = objRow1.insertCell();
			var objCell5 = objRow1.insertCell();
			var objCell6 = objRow1.insertCell();

			objCell1.align = "center";
			objCell2.align = "center";
			objCell3.align = "center";
			objCell4.align = "center";
			objCell5.align = "center";
			objCell6.align = "center";

			objCell1.innerHTML = "<td align=\"center\"><input type=\"text\" id=\"TBStaffDuties[" + thisLineNo + "]\" style=\"width:90px; text-align:center;\" /></td>";
			objCell2.innerHTML = "<td align=\"center\"><input type=\"text\" id=\"TBStaffName[" + thisLineNo + "]\" style=\"width:90px;\" /></td>";
			objCell3.innerHTML = "<td align=\"center\"><input type=\"text\" id=\"TBStaffTEL[" + thisLineNo + "]\" /></td>";
			objCell4.innerHTML = "<td align=\"center\" style=\"line-height:25px;\">" +
				"<select id=\"IsEmailSMS[" + thisLineNo + "]\" ><option value=\"3\">둘다</option><option value=\"2\">Email</option><option value=\"1\">SMS</option><option value=\"0\">X</option></select></td>";
			objCell5.innerHTML = "<td align=\"center\" style=\"line-height:25px;\"><input type=\"text\" id=\"TBStaffEmail[" + thisLineNo + "]\" style=\"width:145px;\" onblur=\"onlyEmail(this)\" /></td>";
			objCell6.innerHTML = "<td style=\"text-align:center;\"><input type=\"text\" id=\"TBStaffMobile[" + thisLineNo + "]\" /></td>";
		}

		function CustomerAddSubmit() {
			if (form1.RegionCode.value == "") {
				alert("please choose this Comapny's Area. At least country");
				return false;
			}

			if (form1.CompanyCode1.value == "" || form1.CompanyCode2.value == "") {
				alert("고객번호를 지정해주세요");
				return false;
			}
			var CompanyCode = form1.CompanyCode1.value + form1.CompanyCode2.value;
			Admin.AskCompanyCodeUsed_(CompanyCode, function (result) {
				if (result != "N") {
					if (result.length > 7) {
						if (result.substr(0, 7) == "Warning") {
							var arrResult = result.split("#!@");
							if (confirm(arrResult[1] + "지역의 고객번호는 " + arrResult[2] + " ~ " + arrResult[3] + "입니다. 무시하고 진행하시겠습니까?")) {
								AddCustomer();
							}
						}
					} else {
						alert("이미 사용중인 고객번호입니다.");
						return false;
					}
				} else {
					AddCustomer();
				}
			}, function (result) { alert("ERROR : " + result); return false; });
		}

		function AddCustomer() {
			var CompanyCode = form1.CompanyCode1.value + form1.CompanyCode2.value;
			var CompanyValue = new Array();
			CompanyValue[0] = GUBUNCL;
			CompanyValue[1] = CompanyCode;
			CompanyValue[2] = form1.TB_CompanyName.value;
			CompanyValue[3] = form1.RegionCode.value;
			CompanyValue[4] = form1.TB_CompanyAddress.value;
			CompanyValue[5] = form1.TB_CompanyTEL.value;
			CompanyValue[6] = form1.TB_CompanyFAX.value;
			CompanyValue[7] = form1.TB_PresidentName.value;
			CompanyValue[8] = form1.TB_CompanyEmail.value;
			CompanyValue[9] = form1.TB_SaupjaNo.value;
			CompanyValue[10] = form1.TB_CompanyNamee.value;

			var businessType = form1.BusinessType.value;
			var AdditionalInformationSum = "";
			if (document.getElementById("TB_CompanyHomepage").value != "") { AdditionalInformationSum += "%!$@#62#@!" + document.getElementById("TB_CompanyHomepage").value; }
			var CompanyUpjong = "";
			if (document.getElementById("Upjong1").checked) { CompanyUpjong += "57"; }
			if (document.getElementById("Upjong2").checked) { CompanyUpjong += "!58"; }
			if (document.getElementById("Upjong3").checked) { CompanyUpjong += "!59"; }
			if (CompanyUpjong != "") { AdditionalInformationSum += "%!$@#63#@!" + CompanyUpjong; }
			if (document.getElementById("TB_CategoryofBusiness").value != "" || document.getElementById("TB_BusinessConditions").value != "") {
				AdditionalInformationSum += "%!$@#64#@!" + document.getElementById("TB_CategoryofBusiness").value + "!" + document.getElementById("TB_BusinessConditions").value;
			}
			if (form1.TBMajorItem.value != "") { AdditionalInformationSum += "%!$@#65#@!" + form1.TBMajorItem.value; }

			var TabStaffRowLength = document.getElementById("TabStaff").rows.length - 2;
			var StaffSum = "";
			for (var i = 0; i < TabStaffRowLength; i++) {
				if (document.getElementById("TBStaffDuties[" + i + "]").value == "" && document.getElementById("TBStaffName[" + i + "]").value == "" && document.getElementById("TBStaffTEL[" + i + "]").value == "" && document.getElementById("TBStaffEmail[" + i + "]").value == "") {
				}
				else {
					StaffSum += document.getElementById("TBStaffDuties[" + i + "]").value + "#@!" +
										document.getElementById("TBStaffName[" + i + "]").value + "#@!" +
										document.getElementById("TBStaffTEL[" + i + "]").value + "#@!" +
										document.getElementById("TBStaffMobile[" + i + "]").value + "#@!" +
										document.getElementById("TBStaffEmail[" + i + "]").value + "#@!" +
										document.getElementById("IsEmailSMS[" + i + "]").value + "%!$@#";
				}
			}

			var TabWarehouseRowLength = document.getElementById('TabWarehouse').rows.length - 2;
			var warehouseSum = "";
			for (var i = 0; i < TabWarehouseRowLength; i++) {
				if (document.getElementById("TBWarehouse[" + i + "][0]").value == "" && document.getElementById("TBWarehouse[" + i + "][1]").value == "" && document.getElementById("TBWarehouse[" + i + "][3]").value == "" && document.getElementById("TBWarehouse[" + i + "][4]").value == "") { }
				else {
					warehouseSum += document.getElementById("TBWarehouse[" + i + "][0]").value + "@@" +
													  document.getElementById("TBWarehouse[" + i + "][1]").value + "@@" +
													  document.getElementById("TBWarehouse[" + i + "][2]").value + "@@" +
													  document.getElementById("TBWarehouse[" + i + "][3]").value + "@@" +
													  document.getElementById("TBWarehouse[" + i + "][4]").value + "####";
				}
			}

			var Identity = "";
			if (form1.HAccountID.value == "") {
				if (document.getElementById("PageMode").value == "ShippingBranch") {
					Identity = document.getElementById("BranchPk").value + "S!h@i#p$p%i^n&g*B(r)a_n+ch";
				}
			} else {
				Identity = form1.HAccountID.value;
			}
			Admin.InsertCompanyByAdmin_(Identity, CompanyValue, StaffSum, warehouseSum, AdditionalInformationSum, businessType, function (result) {
				//alert(result);
				//form1.TB_CompanyAddress.value = result;
				window.returnValue = true;
				returnValue = result;
				self.close();
				return false;
			}, ONFAILED);
		}

		function ONFAILED(result) { window.alert(result); }
		
		function onlyEmail(obj) {
			if (obj.value.length > 0) {
				re = /^[0-9a-zA-Z]([-_\.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_\.]?[0-9a-zA-Z])*.[a-zA-Z]{2,3}$/i;
				if (obj.value.length < 6 || !re.test(obj.value)) {
					alert("메일형식이 맞지 않습니다.\n"); obj.select(); obj.focus(); return false;
				}
			}
		}

		function CheckCode() {
			if (form1.CompanyCode1.value == "") {
				alert("please type area code");
				return false;
			}
			var CompanyCode = form1.CompanyCode1.value + form1.CompanyCode2.value;
			Admin.CheckCompanyCode(CompanyCode, function (result) {
				if (result != "") {
					form1.CompanyCode1.value = result.substr(0, 2);
					form1.CompanyCode2.value = result.substr(2);
				}
			}
			, function (result) {
				alert(result);
			});
		}

		function CheckAutoNum(which, value) {
			if (value.length == 2) {
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
	</script>
</head>
<body style="background-color:#E4E4E4; width:800px; margin:0 auto; padding-top:20px; padding-bottom:20px;"  >
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
		<input type="hidden" id="PageMode" value="<%=Request.Params["Mode"] + "" %>" />
		<input type="hidden" id="BranchPk" value="<%=Request.Params["OwnCPk"]+"" %>"
		<div style="background-color:#999999; padding:7px;">
			<span style="font-weight:bold"><img src="../../Images/ico_arrow.gif" alt="" /> <%=GetGlobalResourceObject("Admin", "CustomerAddTitle")%></span> : <%=GetGlobalResourceObject("Admin", "CustomerAddEx") %> 
			<input type="hidden" id="HCCLPK" value="<%=Request.Params["CCLPK"]+"" %>" />
			<input type="hidden" id="HAccountID" />
		</div>
		<div style="background-color:#777777; height:1px; font-size:1px; "></div>
		<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
		<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
		<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
		<div style="background-color:#EEEEEE; height:1px; font-size:1px; "></div>
		<table border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td class="td01"><%= GetGlobalResourceObject("Member", "CompanyName")%></td>
				<td class="td02"><input type="text" id="TB_CompanyName" style="width:200px;" /></td>
				<td class="td01">PresidentName</td>
				<td class="td02"><input type="text" id="TB_PresidentName" /></td>
			</tr>
            <tr>
				<td class="td01"><%= GetGlobalResourceObject("Member", "CompanyNameE")%></td>
				<td class="td02" colspan="3"><input type="text" id="TB_CompanyNamee" style="width:200px;" /></td>
			</tr>
			<tr>
				<td style="background-color:#f5f5f5; text-align:center; height:25px; width:150px;padding-top:4px; padding-bottom:4px;" >
					<%=GetGlobalResourceObject("Member", "CompanyAddress") %></td>
				<td colspan="3" class="td023">
					<input type="hidden" id="RegionCode" />
					<input type="hidden" id="ResponsibleBranch" />
					<select id="RegionCode[0]" style="width:100px;" size="1" onchange="LoadRegionCode('0');"></select>
					<select id="RegionCode[1]" style="width:140px;" name="office" onchange="LoadRegionCode('1');" ><option value="">:: <%=GetGlobalResourceObject("Member", "SelectionArea") %> ::</option></select>
					<select id="RegionCode[2]" style="width:140px;" name="area" onchange="LoadRegionCode('2');" ><option value="">:: <%=GetGlobalResourceObject("Member", "SelectionDetailArea") %> ::</option></select>
					<div style="padding-top:4px;"><input type="text" id="TB_CompanyAddress" style="width:500px;" /></div>
				</td>
			</tr>
			<tr>
				<td class="td01">TEL</td><td class="td02"><input type="text" id="TB_CompanyTEL" /></td>
				<td class="td01"><%=GetGlobalResourceObject("Member", "SaupjaNo") %></td>
				<td class="td02">
					<select id="BusinessType">
						<option value="54"><%=GetGlobalResourceObject("qjsdur", "rlxk")%></option>
						<option value="55"><%=GetGlobalResourceObject("qjsdur", "qjqdls") %></option>
						<option value="56"><%=GetGlobalResourceObject("qjsdur", "rodlstkdjqwk")%></option>
					</select>
					<input type="text" id="TB_SaupjaNo" maxlength="15" /></td>
			</tr>
			<tr>
				<td class="td01">FAX</td><td class="td02"><input type="text" id="TB_CompanyFAX" /></td>	
				<td class="td01">Homepage</td><td class="td02"><input type="text" id="TB_CompanyHomepage" style="width:230px;" /></td>				
			</tr>
			<tr>
				<td class="td01">E-mail</td><td class="td02"><input type="text" id="TB_CompanyEmail" size="30"  onblur="onlyEmail(this)" /></td>
				<td class="td01"><%=GetGlobalResourceObject("Member", "Upjong") %> / <%=GetGlobalResourceObject("Member", "Uptae") %></td>
				<td class="td02"><input type="text" id="TB_CategoryofBusiness" size="8" /> / <input type="text" id="TB_BusinessConditions" size="8" /></td>
			</tr>
			<tr>
				<td class="td01">Major Item</td><td class="td02"><input type="text" id="TBMajorItem" size="30"  /></td>
				
				<td class="td01"><%=GetGlobalResourceObject("Member", "Upmu") %></td>
				<td class="td02">
					<input type="checkbox" id="Upjong1" /> <label for="Upjong1"><%=GetGlobalResourceObject("Member", "Production") %></label> &nbsp;&nbsp;
					<input type="checkbox" id="Upjong2" /> <label for="upjong2"><%=GetGlobalResourceObject("Member", "Distribution")%></label> &nbsp;&nbsp;
					<input type="checkbox" id="Upjong3" /> <label for="upjong3"><%=GetGlobalResourceObject("Member", "Saler") %></label>
				</td>
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
				<tr><td class="tdSubT" colspan="6">&nbsp;&nbsp;&nbsp;<strong>staff</strong> <input type="button" value="add" onclick="InsertStaff();" /></td></tr>
				<tr style="height:30px;" >
					<td style="background-color:#F5F5F5; text-align:center;" height="20" ><%= GetGlobalResourceObject("Member", "Duties") %></td>
					<td style="background-color:#F5F5F5; text-align:center;" ><%=GetGlobalResourceObject("Member", "Name") %></td>
					<td style="background-color:#F5F5F5; text-align:center;" >TEL</td >
					<td style="background-color:#F5F5F5; text-align:center;" >MSG</td>
					<td style="background-color:#F5F5F5; text-align:center;" >E-mail</td >
					<td style="background-color:#F5F5F5; text-align:center;" >Mobile</td>
				</tr>
			</thead>
			<tbody>
				<tr>
					<td align="center"><input type="text" id="TBStaffDuties[0]" style="width:90px; text-align:center;" /></td>
					<td align="center"><input type="text" id="TBStaffName[0]" style="width:90px;" /></td>
					<td align="center"><input type="text" id="TBStaffTEL[0]" /></td>
					<td align="center" style="line-height:25px;">
						<select id="IsEmailSMS[0]" ><option value="3">둘다</option><option value="2">Email</option><option value="1">SMS</option><option value="0">X</option></select>
					</td>
					<td align="center" style="line-height:25px;"><input type="text" id="TBStaffEmail[0]" style="width:145px;" onblur="onlyEmail(this)" /></td>
					<td style="text-align:center;"><input type="text" id="TBStaffMobile[0]" /></td>
				</tr>
			</tbody>
		</table>
		<div style="background-color:#FFFFFF; height:10px; font-size:10px; "></div>
		<div style="background-color:#777777; height:1px; font-size:1px; "></div>
		<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
		<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
		<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
		<div style="background-color:#EEEEEE; height:1px; font-size:1px; "></div>
		<div style="background-color:#FFFFFF; height:1px; font-size:1px; "></div>
		<table id="TabWarehouse" style="background-color:White; width:800px;" border="0" cellpadding="0" cellspacing="0">
		<thead>
			<tr><td class="tdSubT" colspan="5">&nbsp;&nbsp;&nbsp;<strong>Warehouse</strong> <input type="button" value="add" onclick="InsertWarehouse();" /></td></tr>
			<tr style="height:30px;" >
				<td bgcolor="#F5F5F5" height="20" align="center" style="width:90px;" >상호</td>
				<td bgcolor="#F5F5F5" align="center" ><%=GetGlobalResourceObject("Member", "Address") %></td>
				<td bgcolor="#F5F5F5" align="center" style="width:120px;" >TEL</td>
				<td bgcolor="#F5F5F5" align="center" style="width:70px;" >담당자명</td>
				<td bgcolor="#F5F5F5" align="center" style="width:120px;" >Mobile</td >
			</tr>
		</thead>
		<tbody>
			<tr>
				<td align="center"><input type="text" id="TBWarehouse[0][0]" style="width:80px;" /></td>
				<td align="center"><input type="text" id="TBWarehouse[0][1]" style="width:380px;" /></td>
				<td align="center"><input type="text" id="TBWarehouse[0][2]" style="width:110px;" /></td>
				<td align="center" ><input type="text" id="TBWarehouse[0][3]" style="width:60px;" /></td>
				<td align="center" ><input type="text" id="TBWarehouse[0][4]" style="width:110px;" /></td>
			</tr>
		</tbody>
	</table>
		<div style="background-color:#FFFFFF; height:10px; font-size:10px; "></div>
		<div style="background-color:#777777; height:1px; font-size:1px; "></div>
		<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
		<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
		<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
		<div style="background-color:#EEEEEE; height:1px; font-size:1px; "></div>
		<div style="background-color:#FFFFFF; height:1px; font-size:1px; "></div>
		<div style="width:800px; background-color:#FFFFFF; padding-top:20px; padding-bottom:15px; text-align:center; ">
			<span style="font-size:20px; font-weight:bold;"> <%=GetGlobalResourceObject("Member", "CompanyCode") %></span>
			&nbsp;
			<input type="text" id="CompanyCode1" maxlength="2" style="width:60px; text-align:right; font-size:20px;" />
			<input type="text" id="CompanyCode2" style="width:80px; font-size:20px; " />
			<input type="button" value="Check Code" onclick="CheckCode();" />
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

			<input type="button" id="BTN_Submit" style="width:100px; height:37px; " value="<%=GetGlobalResourceObject("Member", "Submit") %>" onclick="CustomerAddSubmit()" />
			<input type="hidden" id="dhksfy" value="<%=GetGlobalResourceObject("qjsdur", "dhksfy") %>" />
		</div>
		<div style="background-color:#FFFFFF; height:10px; font-size:10px; "></div>
		<div style="background-color:#777777; height:1px; font-size:1px; "></div>
		<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
		<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
		<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
		<div style="background-color:#EEEEEE; height:1px; font-size:1px; "></div>
    </form>
</body>
</html>