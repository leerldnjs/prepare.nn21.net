<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="CompanyJoinStep2.aspx.cs" Inherits="Member_CompanyJoinStep2" meta:resourcekey="PageResource1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script src="../Common/public.js" type="text/javascript"></script>
	<script src="../Common/RegionCode.js?version=20131029" type="text/javascript"></script>
	<script type="text/javascript">
		function cate1change(first, second, third, obj, idx, TB_RegionPk) {
			delopt(document.getElementById(second), CompanyJoinStep2.HSelectionArea.value);
			delopt(document.getElementById(third), CompanyJoinStep2.HSelectionDetailArea.value);
			if (idx == 1) {
				document.getElementById(third).style.visibility = "hidden"; // 한국선택시
			}
			else {
				document.getElementById(third).style.visibility = "visible"; // 중국선택시
			}
			if (cate2[idx]) {
				for (i = 1; i < cate2[idx].length; i++) {
					document.getElementById(second).options[i] = new Option(cate2[idx][i].value, cate2[idx][i].text);
				}
			}
			document.getElementById(TB_RegionPk).value = document.getElementById(first).options[idx].value;
		}
		function cate2change(first, second, third, obj, idx, TB_RegionPk) {
			delopt(document.getElementById(third), CompanyJoinStep2.HSelectionDetailArea.value);
			cate1 = document.getElementById(first).selectedIndex;
			if (document.getElementById(first).selectedIndex != 1) {
				if (cate3[cate1][idx]) {
					for (i = 1; i < cate3[cate1][idx].length; i++) {
						document.getElementById(third).options[i] = new Option(cate3[cate1][idx][i].value, cate3[cate1][idx][i].text);
					}
				}
			}
			document.getElementById(TB_RegionPk).value = document.getElementById(second).options[idx].value;
		}
		function cate3change(first, second, third, obj, idx, TB_RegionPk) {
			document.getElementById(TB_RegionPk).value = document.getElementById(third).options[idx].value;
		}
		
		function BTN_Submit_Click() {
			var temp;
			temp = document.getElementById("TB_CompanyName");
			if (temp.value == "") { alert(CompanyJoinStep2.Hidden_MustCompanyName.value); temp.focus(); temp.select(); return false; }
			var CompanyName = temp.value;
			temp = document.getElementById("TB_CompanyNamee");
			if (temp.value == "") { alert(CompanyJoinStep2.Hidden_MustCompanyNamee.value); temp.focus(); temp.select(); return false; }
			var CompanyNamee = temp.value;
			var PresidentName = CompanyJoinStep2.SessionStep1.value.split("#@!")[2];
			temp = document.getElementById("gcodeExport");
			if (temp.value == "") { alert(CompanyJoinStep2.Hidden_MustSelectLocal.value); return false; }
			var CompanyRegionCodePk = temp.value;
			temp = document.getElementById("TB_CompanyAddress");
			if (temp.value == "") { alert(CompanyJoinStep2.Hidden_MustAddress.value); temp.focus(); temp.select(); return false; }
			var CompanyAddress = temp.value;

			var SaupjaGubun = "";
			if (document.getElementById("BusinessType_Bubin").checked == true) { SaupjaGubun = "55"; }
			if (document.getElementById("BusinessType_Personal").checked == true) { SaupjaGubun = "56"; }
			if (document.getElementById("BusinessType_ETC").checked == true) { SaupjaGubun = "54"; }
			var SaupjaNo = CompanyJoinStep2.TB_SaupjaNo.value;

			temp = document.getElementById("TB_CompanyTEL[0]");
			if (temp.value.length < 8) { alert(CompanyJoinStep2.Hidden_WrongCompanyTEL.value); temp.focus(); temp.select(); return false; }
			var CompanyTEL = temp.value;

			temp = document.getElementById("TB_CompanyFAX[0]");
			if (temp.value.length < 8) { alert(CompanyJoinStep2.Hidden_WrongCompanyFAX.value); temp.focus(); temp.select(); return false; }
			var CompanyFAX = temp.value;
			
			temp = document.getElementById("TB_CompanyEmail");
			if (temp.value.length < 7) { alert(CompanyJoinStep2.Hidden_WrongCompanyEmail.value); temp.focus(); temp.select(); return false; }
			var CompanyEmail = temp.value;

			var CompanyUpjong = "";
			if (document.getElementById("Upjong1").checked) { CompanyUpjong += "57"; }
			if (document.getElementById("Upjong2").checked) { CompanyUpjong += "!58"; }
			if (document.getElementById("Upjong3").checked) { CompanyUpjong += "!59"; }

			var AdditionalInformationSum = "";
			//if (PresidentNameByEnglish != "") { AdditionalInformationSum += "@@67###'" + PresidentNameByEnglish + "'"; }
			if (SaupjaGubun != "") {
				AdditionalInformationSum += "@@61###'" + SaupjaGubun + "'";
			}
			if (document.getElementById("TB_CompanyHomepage").value != "") {
				AdditionalInformationSum += "@@62###'" + document.getElementById("TB_CompanyHomepage").value + "'";
			}
			if (CompanyUpjong != "") { AdditionalInformationSum += "@@63###'" + CompanyUpjong + "'"; }
			if (document.getElementById("TB_CategoryofBusiness").value != "" || document.getElementById("TB_BusinessConditions").value != "") {
				AdditionalInformationSum += "@@64###'" + document.getElementById("TB_CategoryofBusiness").value + "!" + document.getElementById("TB_BusinessConditions").value + "'";
			}
			//alert(CompanyName + " " + CompanyAddress + " " + CompanyTEL + " " + CompanyFAX + " " + PresidentName + " " + CompanyEmail + " " + SaupjaNo + " " + CompanyNameByEnglish + " " + CompanyAddressByEnglish + " " + CompanyRegionCodePk + " " + AdditionalInformationSum);
			/////////////////////////////////////////STAFF

			var staffsum ="";
			if (document.getElementById("Damdangja[0]Name").value != "") {
				staffsum +=
				STAFFID[0]+ "@@" +
				document.getElementById("Damdangja[0]PWD").value + "@@" +
				document.getElementById("Damdangja[0]JikWi").value + "@@" +
				document.getElementById("Damdangja[0]Name").value + "@@" +
				document.getElementById("Damdangja[0]TEL").value + "@@" +
				document.getElementById("Damdangja[0]Mobile").value + "@@" +
				document.getElementById("Damdangja[0]Email2").value + "@@1@@" +
				document.getElementById("Damdangja[0]MSG").value;
			}
			staffsum += "####";
			if (document.getElementById("Damdangja[1]Name").value != "") {
				staffsum +=
				STAFFID[1]+ "@@" +
				document.getElementById("Damdangja[1]PWD").value + "@@" +
				document.getElementById("Damdangja[1]JikWi").value + "@@" +
				document.getElementById("Damdangja[1]Name").value + "@@" +
				document.getElementById("Damdangja[1]TEL").value + "@@" +
				document.getElementById("Damdangja[1]Mobile").value + "@@" +
				document.getElementById("Damdangja[1]Email2").value + "@@0@@" +
				document.getElementById("Damdangja[1]MSG").value;
			}
			/////////////////////////////////////////STAFF
			if (CompanyJoinStep2.HJFECompanyPk.value == "N") {
			    Member.CompanyJoinStep2Submit(CompanyName,CompanyNamee, CompanyAddress, CompanyTEL, CompanyFAX, PresidentName, CompanyEmail, SaupjaNo, CompanyRegionCodePk, AdditionalInformationSum, CompanyJoinStep2.SessionStep1.value, staffsum, function success(result) {
					document.getElementById("CompanyPk").value = result.toString();
					alert(CompanyJoinStep2.Hidden_Welcome.value);
					__doPostBack('Button1', '');
				}, callError);
			}
			else {
			    Member.JoinFromEmailStep2Submit(CompanyJoinStep2.HJFECompanyPk.value, CompanyName ,CompanyNamee, CompanyAddress, CompanyTEL, CompanyFAX, PresidentName, CompanyEmail, SaupjaNo, CompanyRegionCodePk, AdditionalInformationSum, CompanyJoinStep2.SessionStep1.value, staffsum, function success(result) {
					alert(CompanyJoinStep2.Hidden_Welcome.value);
					__doPostBack('Button1', '');
				}, callError);
			}
			//Member.CompanyJoinStep2Submit(CompanyName, CompanyAddress, CompanyTEL, CompanyFAX, PresidentName, CompanyEmail, SaupjaNo, CompanyNameByEnglish, CompanyAddressByEnglish, CompanyRegionCodePk, AdditionalInformationSum, 
		}
		function callError(result) { window.alert(result); }
		var STAFFID = new Array();

		function CheckID(f, count) {
			regA1 = new RegExp(/^[0-9a-zA-Z]+$/);
			if (f.value != "") {
				if (CompanyJoinStep2.HCompanyJoin1Account.value == f.value) {
					alert(CompanyJoinStep2.anffbekaekdwkdkdlelrkeovydkdleldhkrkxtmqslek.value);
					f.value = "";
					return false;
				}
				if (document.getElementById("Damdangja[0]ID").value != "" && document.getElementById("Damdangja[1]ID").value != "" && document.getElementById("Damdangja[0]ID").value == document.getElementById("Damdangja[1]ID").value) {
					alert("담당자 아이디는 같을수 없습니다");
					f.value = "";
					return false;
				}

				if (!regA1.test(f.value)) {
					alert("아이디에는 영문자와 숫자만 사용가능합니다.");
					f.value = "";
					return false;
				}
				if (f.value.length < 5) {
					alert("아이디는 네자 이상만 가능합니다.");
					f.value = "";
					return false;
				}
				Member.UniqueCheck('AccountID', f.value, function (result) {
					if (result == 1) {
						alert("이미사용중인 아이디입니다. ");
						f.value = "";
						return false;
					}
					else {
						STAFFID[count] = f.value;
						//alert(STAFFID[0]);
					}
				}, function (result) { window.alert(result); });
			}
		}
	</script>
   	<style type="text/css">
		.tdSubT	{ border-bottom-width: 2px; border-bottom-style: solid; border-bottom-color: #93A9B8;	padding-top:30px; width:900px; padding-bottom:3px; }
		.td01
		{
			background-color:#f5f5f5; text-align:center; height:25px; width:150px;padding-top:4px; padding-bottom:4px;
			border-bottom-style:dotted; border-bottom-color:#E8E8E8; border-bottom-width:1px;
		}
		.td02
		{
			padding-top:4px;padding-bottom:4px; padding-left:10px;
			border-bottom-style:dotted; border-bottom-color:#E8E8E8; border-bottom-width:1px;
			background-color:White;
		}
		.td023
		{
			width:740px; padding-top:4px;padding-bottom:4px; padding-left:10px;
			border-bottom-style:dotted; border-bottom-color:#E8E8E8; border-bottom-width:1px;
			background-color:White;
		}
		.tdStaffBody{text-align:center; padding:5px; border:dotted 1px #E8E8E8; background-color:White;}
    </style>
    <link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px; ">
    <form id="CompanyJoinStep2" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"><Services><asp:ServiceReference Path="~/WebService/Member.asmx" /></Services></asp:ScriptManager>
    <div style="width:100%;">
		<div style="background-color:#999999; padding:7px; ">
			<div>
				<div style="color:White; float:right; padding-right:10px;">
					<a href="./CompanyJoinStep2.aspx?Language=en"><span style="color:White;">english</span></a>&nbsp;&nbsp;||&nbsp;&nbsp;
					<a href="./CompanyJoinStep2.aspx?Language=ko"><span style="color:White;">한글</span></a> &nbsp;&nbsp;||&nbsp;&nbsp;
					<a href="./CompanyJoinStep2.aspx?Language=zh"><span style="color:White;">中文</span></a>
				</div>
				<span style="color:White; font-weight:bold"><img src="../Images/ico_arrow.gif" alt="" /> <%=GetGlobalResourceObject("Member", "InfoCompany") %></span>
			</div>
		</div>
		<div style="background-color:#777777; height:1px; font-size:1px; "></div>
		<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
		<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
		<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
		<div style="background-color:#EEEEEE; height:1px; font-size:1px; "></div>
		<table border="0" cellpadding="0" cellspacing="0" style="width:900px;">
			<tr>
				<td class="td01">*<%= GetGlobalResourceObject("Member", "CompanyName")%></td>
				<td class="td02" colspan="3"><input type="text" id="TB_CompanyName" style="width:280px;" /></td>
			</tr>
            <tr>
				<td class="td01">*<%= GetGlobalResourceObject("Member", "CompanyNamee")%></td>
				<td class="td02" colspan="3"><input type="text" id="TB_CompanyNamee" style="width:280px;" /></td>
			</tr>
			<tr>
				<td class="td01">*<%=GetGlobalResourceObject("Member", "CompanyAddress") %></td>
				<td class="td02" colspan="3">
					<input type="hidden" id="gcodeExport" value="" />
					<input type="hidden" id="CompanyPk" name="CompanyPk" value="" />
                    <input type="hidden" id="HJFECompanyPk" value="<%=JFECompanyPk %>" />
					<input type="hidden" id="SessionStep1" value="<%=Session["CompanyJoinStep1"].ToString()%>" />					
					<select id="regionCodeI1" style="width:100px;" name="country" size="1" onchange="cate1change('regionCodeI1','regionCodeI2','regionCodeI3',this.form, this.selectedIndex,'gcodeExport')">
						<option value="">:: <%=GetGlobalResourceObject("Member", "SelectionCountry2") %> ::</option>
						<option value="1">KOREA&nbsp;한국</option>
						<option value="2">CHINA&nbsp;中國</option>
						<option value="3">JAPAN&nbsp;日本</option>
					</select>
					<select id="regionCodeI2" style="width:140px;" name="office" onchange="cate2change('regionCodeI1','regionCodeI2','regionCodeI3',this.form,this.selectedIndex,'gcodeExport')">
						<option value="">:: <%=GetGlobalResourceObject("Member", "SelectionArea") %> ::</option>
					</select>
					<input type="hidden" id="HSelectionArea" value=":: <%=GetGlobalResourceObject("Member", "SelectionArea") %> ::" />
					<input type="hidden" id="HSelectionDetailArea" value=":: <%=GetGlobalResourceObject("Member", "SelectionDetailArea") %> ::" />
					<select id="regionCodeI3" style="width:140px;" name="area" onchange="cate3change('regionCodeI1','regionCodeI2','regionCodeI3',this.form,this.selectedIndex,'gcodeExport')">
						<option value="">:: <%=GetGlobalResourceObject("Member", "SelectionDetailArea") %> ::</option>
					</select>
					<div style="padding-top:4px;"><input type="text" id="TB_CompanyAddress" size="60" /></div>
				</td>
			</tr>
			<tr>
				<td class="td01">*<%= GetLocalResourceObject("SaupjaGubun")%></td>
				<td class="td02">
					<input type="radio" name="BusinessType" id="BusinessType_Bubin" style="text-align:left;" />
					<label for="BusinessType_Bubin"><%= GetLocalResourceObject("BusinessType1") %></label>&nbsp;&nbsp;&nbsp;
					<input type="radio" name="BusinessType" id="BusinessType_Personal" style="text-align:left;" />
					<label for="BusinessType_Personal"><%= GetLocalResourceObject("BusinessType2") %></label>&nbsp;&nbsp;&nbsp;
					<input type="radio" name="BusinessType" id="BusinessType_ETC" style="text-align:left;" />
					<label for="BusinessType_ETC"><%= GetGlobalResourceObject("RequestForm", "ETC") %></label>
				</td>
				<td class="td01"><%= GetLocalResourceObject("SaupjaNo") %></td>
				<td class="td02"><input type="text" id="TB_SaupjaNo" maxlength="15" size="25" /></td>
			</tr>
			<tr>
				<td class="td01">*<%=GetGlobalResourceObject("Member", "CompanyTEL") %></td>
				<td class="td02"><input type="text" id="TB_CompanyTEL[0]" value="<%=CompanyJoinStep1[4] %>" /></td>
				<td class="td01">*FAX</td>
				<td class="td02"><input type="text" id="TB_CompanyFAX[0]" /></td>
			</tr>
			<tr>
				<td class="td01">*<%=GetGlobalResourceObject("Member", "Email") %></td>
				<td colspan="3" class="td023"><input type="text" id="TB_CompanyEmail" size="30" value="<%=CompanyJoinStep1[6] %>" /></td>
			</tr>
			<tr>
				<td class="td01"><%=GetGlobalResourceObject("Member", "Homepage") %></td>
				<td colspan="3" class="td023"><input type="text" id="TB_CompanyHomepage" size="50" /></td>
			</tr>
			<tr>
				<td class="td01"><%= GetLocalResourceObject("Upmu") %></td>
				<td colspan="3" class="td023">
					<input type="checkbox" id="Upjong1" /> <label for="Upjong1"><%= GetLocalResourceObject("Production") %></label> &nbsp;&nbsp;
					<input type="checkbox" id="Upjong2" /> <label for="upjong2"><%= GetLocalResourceObject("Distribution")%></label> &nbsp;&nbsp;
					<input type="checkbox" id="Upjong3" /> <label for="upjong3"><%= GetLocalResourceObject("Saler") %></label>
				</td>
			</tr>
			<tr>
				<td class="td01"><%= GetLocalResourceObject("Upjong") %> / <%= GetLocalResourceObject("Uptae") %></td>
				<td colspan="3" class="td023"><input type="text" id="TB_CategoryofBusiness" size="8" /> / <input type="text" id="TB_BusinessConditions" size="8" /></td>
			</tr>
		</table>
		<div style="background-color:#999999; height:1px; font-size:1px; "></div>
		<div style="background-color:#AAAAAA; height:1px; font-size:1px; "></div>
		<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
		<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
		<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
		<div style="background-color:#E4E4E4; height:1px; font-size:1px; "></div>
		<br />
		<div style="background-color:#999999; padding:7px; ">
			<div>
				<span style="color:White; font-weight:bold"><img src="../Images/ico_arrow.gif" alt="" />&nbsp; Staff Information : 추가 업무담당자가 있는경우 등록해주세요. </span>
			</div>
		</div>
		<table border="0" cellpadding="0" cellspacing="0" style="width:900px;">
			<tr>
				<td class="Line1E8E8E8" rowspan="4" style="width:90px; background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px; font-weight:bold;">
					<%= GetGlobalResourceObject("Member", "Staff_FreightMan") %></td>
				<td class="Line1E8E8E8" style="background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">
					<%=GetGlobalResourceObject("Member","JikWi") %></td>
				<td class="Line1E8E8E8" style=" background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">
					<%=GetGlobalResourceObject("Member", "Name") %></td>
				<td class="Line1E8E8E8" style="background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">TEL</td>
				<td class="Line1E8E8E8" style="background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">MSG</td>
				<td class="Line1E8E8E8" style="background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">Mobile</td>
				<td class="Line1E8E8E8" style="background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">E-Mail</td>
			</tr>
			<tr>
                <td class="tdStaffBody"><input type="text" id="Damdangja[0]JikWi" style="width:60px;" /></td>
				<td class="tdStaffBody"><input type="text" id="Damdangja[0]Name" style="width:80px;" /></td>
				<td class="tdStaffBody"><input type="text" id="Damdangja[0]TEL" style="width:120px;" /></td>
				<td class="tdStaffBody"><select id="Damdangja[0]MSG" ><option value="3">All</option><option value="2">Email</option><option value="1">SMS</option><option value="0">X</option> </select></td>	
				<td class="tdStaffBody"><input type="text" id="Damdangja[0]Mobile" style="width:120px;" /></td>	
				<td class="tdStaffBody"><input type="text" id="Damdangja[0]Email2" style="width:145px;" /></td>
			</tr>
			<tr>
				<td class="tdStaffBody" colspan="6" style="text-align:left;">
					&nbsp;
					필요한경우 ID / PW를 추가신청해서 여러곳에서 로그인할수 있습니다. 
					<span style="padding-left:40px;">ID : </span><input type="text" id="Damdangja[0]ID" onblur="CheckID(this, 0);" style="width:100px;" />
					<span style="padding-left:40px;">PW : </span><input type="text" id="Damdangja[0]PWD" style="width:100px;" />
				</td>
			</tr>
			
		</table>
		<div style="background-color:#999999; height:1px; font-size:1px; "></div>
		<table border="0" cellpadding="0" cellspacing="0" style="width:900px;">
			<tr>
				<td class="Line1E8E8E8" rowspan="4" style="width:90px; background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px; font-weight:bold;">
					<%= GetGlobalResourceObject("Member", "Staff_FreightMan") %></td>
				<td class="Line1E8E8E8" style="background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">
					<%=GetGlobalResourceObject("Member","JikWi") %></td>
				<td class="Line1E8E8E8" style=" background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">
					<%=GetGlobalResourceObject("Member", "Name") %></td>
				<td class="Line1E8E8E8" style="background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">TEL</td>
				<td class="Line1E8E8E8" style="background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">MSG</td>
				<td class="Line1E8E8E8" style="background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">Mobile</td>
				<td class="Line1E8E8E8" style="background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">E-Mail</td>
			</tr>
			<tr>
				<td class="tdStaffBody"><input type="text" id="Damdangja[1]JikWi" style="width:60px;" /></td>
				<td class="tdStaffBody"><input type="text" id="Damdangja[1]Name" style="width:80px;" /></td>
				<td class="tdStaffBody"><input type="text" id="Damdangja[1]TEL" style="width:120px;" /></td>
				<td class="tdStaffBody"><select id="Damdangja[1]MSG" ><option value="3">All</option><option value="2">Email</option><option value="1">SMS</option><option value="0">X</option> </select></td>
				<td class="tdStaffBody"><input type="text" id="Damdangja[1]Mobile" style="width:120px;" /></td>	
				<td class="tdStaffBody"><input type="text" id="Damdangja[1]Email2" style="width:145px;" /></td>
			</tr>
			<tr>
				<td class="tdStaffBody" colspan="6" style="text-align:left;">
					&nbsp;
					필요한경우 ID / PW를 추가신청해서 여러곳에서 로그인할수 있습니다. 
					<span style="padding-left:40px;">ID : </span><input type="text" id="Damdangja[1]ID" onblur="CheckID(this, 1);" style="width:100px;" />
					<span style="padding-left:40px;">PW : </span><input type="text" id="Damdangja[1]PWD" style="width:100px;" />
				</td>
			</tr>
		</table>
		<div style="background-color:#999999; height:1px; font-size:1px; "></div>
		<div style="background-color:#AAAAAA; height:1px; font-size:1px; "></div>
		<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
		<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
		<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
		<div style="background-color:#E4E4E4; height:1px; font-size:1px; "></div>
		<p></p>
		<div style="text-align:center; padding-top:20px;">
			<input  type="button" value="<%=GetGlobalResourceObject("Member", "Submit")%>" style="width:550px; height:40px;" onclick="BTN_Submit_Click()" />
			<input type="hidden" id="Debug" onclick="this.select();" />
		</div>
	</div>
	<span style="visibility:hidden;"><asp:Button ID="Button1" runat="server" Text="Button" onclick="BTN_Submit_Click" /></span>
	<input type="hidden" id="HCompanyJoin1Account" value="<%=CompanyJoinStep1[0]%>" />	
	<input type="hidden" id="Hidden_BusinessType1" value="<%= GetGlobalResourceObject("Member","BusinessType1") %>" />
	<input type="hidden" id="Hidden_BusinessType2" value="<%= GetGlobalResourceObject("Member","BusinessType2") %>" />
	<input type="hidden" id="Hidden_MustCompanyName" value="<%= GetGlobalResourceObject("Alert","MustCompanyName") %>" />
    <input type="hidden" id="Hidden_MustCompanyNamee" value="<%= GetGlobalResourceObject("Alert","MustCompanyNamee") %>" />
	<input type="hidden" id="Hidden_MustPresidentName" value="<%= GetGlobalResourceObject("Alert","MustPresidentName") %>" />
	<input type="hidden" id="Hidden_WrongCompanyTEL" value="<%= GetGlobalResourceObject("Alert","WrongTEL") %>" />
	<input type="hidden" id="Hidden_WrongCompanyFAX" value="<%= GetLocalResourceObject("WrongCompanyFAX") %>" />
	<input type="hidden" id="Hidden_WrongCompanyEmail" value="<%= GetLocalResourceObject("WrongCompanyEmail") %>" />
	<input type="hidden" id="Hidden_MustSelectLocal" value="<%= GetLocalResourceObject("MustSelectLocal") %>" />
	<input type="hidden" id="Hidden_MustAddress" value="<%= GetLocalResourceObject("MustAddress") %>" />
	<input type="hidden" id="Hidden_Welcome" value="<%=GetGlobalResourceObject("Member", "Welcome") %>" />
	<input type="hidden" id="anffbekaekdwkdkdlelrkeovydkdleldhkrkxtmqslek" value="<%=GetGlobalResourceObject("qjsdur", "anffbekaekdwkdkdlelrkeovydkdleldhkrkxtmqslek") %>" />
	<input type="hidden" id="anffbekaekdwksmsvlftntkgkddlqslek" value="<%=GetGlobalResourceObject("qjsdur", "anffbekaekdwksmsvlftntkgkddlqslek") %>" />
	<input type="hidden" id="anffbekaekdwktjdauddmsvlftntkgkddlqslek" value="<%=GetGlobalResourceObject("qjsdur", "anffbekaekdwktjdauddmsvlftntkgkddlqslek") %>" />
	<input type="hidden" id="anffbekaekdwkvotmdnjemsmsvlftntkgkddlqslek" value="<%=GetGlobalResourceObject("qjsdur", "anffbekaekdwkvotmdnjemsmsvlftntkgkddlqslek") %>" />
	
    </form>
</body>
</html>