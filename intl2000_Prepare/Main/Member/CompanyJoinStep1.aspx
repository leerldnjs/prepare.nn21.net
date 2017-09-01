<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyJoinStep1.aspx.cs" Inherits="Member_CompanyJoinStep1" meta:resourcekey="PageResource1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script src="../Common/public.js" type="text/javascript"></script>
    <script type="text/javascript">
    	var idCheckbool = "0";
    	function CountrySelection(idx, target) {
    		if (idx == 1) {
    			document.getElementById(target).value = "1"; // 한국선택시
    			/*<span id="PnJuminChn" style="visibility:hidden;" ><input type="text" id="TB_JuminChn" style="width:150px;" maxlength="22" onblur="OnlyNum(this)" /> </span>*/
    			MemberJoinStep1.TB_JuminChn.style.width = "1px";
    			document.getElementById("PnJuminChn").style.visibility = "hidden";
    			document.getElementById("PnJuminKor").style.visibility = "hidden";
    			MemberJoinStep1.JuminNoTEXT.value = MemberJoinStep1.JuminNoKOR.value;
    		} else if (idx == 2) {
    			document.getElementById(target).value = "2"; // 중국선택시
    			document.getElementById("PnJuminChn").style.visibility = "hidden";
    			document.getElementById("PnJuminKor").style.visibility = "hidden";
    			MemberJoinStep1.JuminNoTEXT.value = MemberJoinStep1.JuminNoCHN.value;
    		} else {
    			document.getElementById(target).value = "3"; // 중국선택시
    			document.getElementById("PnJuminChn").style.visibility = "hidden";
    			MemberJoinStep1.TB_JuminChn.style.width = "1px";
    			document.getElementById("PnJuminKor").style.visibility = "hidden";
    			MemberJoinStep1.JuminNoTEXT.value = "";
    		}
    	}
		
    	function CheckID(f) {
    		regA1 = new RegExp(/^[0-9a-zA-Z]+$/);
    		if (!regA1.test(f)) {
    			MemberJoinStep1.CheckIDResult.style.color = "red";
    			MemberJoinStep1.CheckIDResult.value = MemberJoinStep1.Hidden_IDMustAlphaNNumber.value;
    			MemberJoinStep1.TB_ID.select();
    			document.getElementById("TB_ID").select();
    			document.getElementById("TB_ID").focus();
    			idCheckbool = "0";
    			return false;
    		}
    		if (f.length < 5) {
    			document.getElementById("CheckIDResult").style.color = "red";
    			document.getElementById("CheckIDResult").value = MemberJoinStep1.Hidden_IDMustMore4.value;
    			idCheckbool = "0";
    		}
    		else { Member.UniqueCheck('AccountID', f, CheckIDSuccess, callError); }
    	}
    	function CheckIDSuccess(result) {
    		if (result == 1) {
    			document.getElementById("CheckIDResult").style.color = "red";
    			document.getElementById("CheckIDResult").value = MemberJoinStep1.Hidden_IDBeenUsed.value;
    			idCheckbool = "0";
    		}
    		else {
    			document.getElementById("CheckIDResult").style.color = "black";
    			document.getElementById("CheckIDResult").value = MemberJoinStep1.Hidden_IDOK.value;
    			idCheckbool = "1";
    		}
    	}
    	function callError(result) { window.alert(result); }
    	function BTN_Submit_Click() {
    		if (idCheckbool != "1") {
    			alert(MemberJoinStep1.IDCHECK.value);
    			return false;
    		}
    		if (document.getElementById("Chk_YakGuan").checked == false) {	
    			alert(MemberJoinStep1.HYakguanMust.value);
    			document.getElementById("Chk_YakGuan").focus();
    			return false;
    		}
    		var temp;
    		temp = document.getElementById("TB_ID");
    		if (temp.value.trim() == "") {	
    			alert(MemberJoinStep1.HIdMust.value);
    			temp.focus();
    			return false;
    		}
    		var id = temp.value.trim();
    		temp = document.getElementById("TB_PWD");
    		if (temp.value.trim() == "") {
    			alert(MemberJoinStep1.HPWDMust.value);
    			temp.focus();
    			return false;
    		}
    		if (temp.value.length < 4) {
    			alert("비밀번호는 4자리 이상이어야 합니다.");
    			temp.focus();
    			return false;
    		}
    		if (temp.value.trim() != document.getElementById("TB_PWD2").value.trim()) {
    			alert(MemberJoinStep1.HPWDDifferent.value);
    			temp.focus();
    			return false;
    		}
    		var pwd = temp.value.trim();
    		////////////////////////////////
    		temp = document.getElementById("TB_WriterName");
    		if (temp.value == "") { alert(MemberJoinStep1.Hidden_MustWriterName.value); temp.focus(); return false; }
    		var WriterName = temp.value;
    		temp = document.getElementById("TB_Duties");
    		if (temp.value == "") { alert(MemberJoinStep1.Hidden_MustDuties.value); temp.focus(); return false; }
    		var WriterDuties = temp.value;

    		if (document.getElementById("regionCodeI1").selectedIndex == 0) {
    			alert(MemberJoinStep1.HCountryMust.value);
    			document.getElementById("regionCodeI1").focus();
    			return false;
    		}
    		var JuminNo = "N";
    		if (MemberJoinStep1.gcodeExport.value == "1" && MemberJoinStep1.TB_Jumin1.value != "" && MemberJoinStep1.TB_Jumin2.value != "") {
    			temp = document.getElementById("TB_Jumin1");
    			if (temp.value.length < 6) { alert(MemberJoinStep1.Hidden_MustJuminNo.value); temp.focus(); temp.select(); return false; }
    			JuminNo = temp.value;
    			temp = document.getElementById("TB_Jumin2");
    			if (temp.value.length < 7) { alert(MemberJoinStep1.Hidden_MustJuminNo.value); temp.focus(); temp.select(); return false; }
    			JuminNo += "-" + temp.value;
    			var jumin1 = document.getElementById("TB_Jumin1");
    			var jumin2 = document.getElementById("TB_Jumin2");
    			var jumin = jumin1.value + jumin2.value;

    			//주민번호 체크
    			var total = 0;
    			for (i = 0; i < jumin.length; i++) {
    				if (i <= 7) { total += parseInt(jumin.charAt(i)) * (i + 2); }
    				else if (i >= 8 && i <= 11) { total += parseInt(jumin.charAt(i)) * (i - 6); }
    			}
    			var check = (11 - (total % 11)) % 10;
    			if (parseInt(check) != parseInt(jumin.charAt(12))) {
    				alert(MemberJoinStep1.Hidden_NotRightJuminNo.value);
    				jumin1.focus(); jumin1.select();
    				return false;
    			}
    		}
    		else if (MemberJoinStep1.gcodeExport.value == "2" && MemberJoinStep1.TB_JuminChn.value != "") {
    			JuminNo = MemberJoinStep1.TB_JuminChn.value;
    		}
    		MemberJoinStep1.HJumin.value = JuminNo;
    		
    		temp = document.getElementById("TB_TEL");
    		if (temp.value.length < 8) {
    			alert(MemberJoinStep1.Hidden_WrongTEL.value); temp.focus(); temp.select(); return false; 
			}
    		var WriterTEL = temp.value;
    		
			temp = document.getElementById("TB_Mobile");
    		if (temp.value.length < 8) {
    			alert(MemberJoinStep1.Hidden_WrongMobile.value); temp.focus(); temp.select(); return false; 
			}
    		var WriterMobile = temp.value;

    		temp = document.getElementById("TB_Email");
    		if (temp.value.length < 7) { alert(MemberJoinStep1.Hidden_WrongEmail.value); temp.focus(); temp.select(); return false; }
    		var WriterEmail = temp.value;
    		var regionCode = document.getElementById("gcodeExport").value;
    		MemberJoinStep1.Session1.value = id + "#@!" + pwd + "#@!" + WriterName + "#@!" + WriterDuties + "#@!" + WriterTEL + "#@!" + WriterMobile + "#@!" + WriterEmail + "#@!" + MemberJoinStep1.HJumin.value + "#@!" + ISMSGRECEIVE;
    		
    		__doPostBack('btn_Submit', '');
    	}
    	var ISMSGRECEIVE = "3";
    	function Rd_IsMsgReceiveClick(which, value) { ISMSGRECEIVE = value; }
    </script>
   	<style type="text/css">
		.tdSubT { border-bottom-width: 2px;	border-bottom-style: solid;	border-bottom-color: #93A9B8;	padding-top:30px; width:900px; padding-bottom:3px; }
		.td01{text-align:center; height:25px; width:150px; padding-top:4px; padding-bottom:4px; background-color:#f5f5f5; }
		.td023{width:740px; padding-top:4px;padding-bottom:4px; padding-left:10px;background-color:White;}
		.td02 { width:300px; padding-top:4px;padding-bottom:4px; padding-left:10px; background-color:White;}
    </style>
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px; ">
    <form id="MemberJoinStep1" runat="server" >
    <asp:ScriptManager ID="ScriptManager1" runat="server">
		<Services><asp:ServiceReference Path="~/WebService/Member.asmx" /></Services>
	</asp:ScriptManager>
	<div style="width:900px;">
		<div style="background-color:#999999; padding:7px; ">
			<span style="color:White; font-weight:bold"><img src="../Images/ico_arrow.gif" alt="" /> <% Response.Write(GetLocalResourceObject("MainTitle"));%></span>
			<span style="color:White; padding-left:550px; ">
				<span style="cursor:pointer; color:White;" onclick="LanguageSet('en')" >english</span>&nbsp;&nbsp;||&nbsp;&nbsp;
				<span style="cursor:pointer; color:White;" onclick="LanguageSet('ko')" >한글</span> &nbsp;&nbsp;||&nbsp;&nbsp;
				<span style="cursor:pointer; color:White;" onclick="LanguageSet('zh')" >中文</span>
			</span>
		</div>
		<div style="background-color:#777777; height:1px; font-size:1px; "></div>
		<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
		<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
		<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
		<div style="background-color:#EEEEEE; height:1px; font-size:1px; "></div>
		<div style="text-align:center; padding:10px; ">
			<textarea style="padding:10px;" cols="140" rows="30" readonly="readonly"><%= GetLocalResourceObject("Yakguan") %></textarea>
		<div style="padding:10px;"><input id="Chk_YakGuan" type="checkbox" /><label for="Chk_YakGuan"> <%=GetLocalResourceObject("YakguanCHK") %></label></div>
		</div>

		<table border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td class="td01 Line1E8E8E8">*<% Response.Write(GetLocalResourceObject("ID"));%></td>
				<td class="td023 Line1E8E8E8" colspan="3" >
					<input type="text" id="TB_ID" name="TB_ID" onkeyup="CheckID(this.value)" onblur="CheckID(this.value)"  />
					<input id="CheckIDResult"  style="border:0; color:Red; width:300px;" tabindex="99" readonly="readonly" type="text" />
				</td>
			</tr>
			<tr>
				<td class="td01 Line1E8E8E8">*<% Response.Write(GetLocalResourceObject("PWD"));%></td>
				<td class="td023 Line1E8E8E8" colspan="3" ><input type="password" id="TB_PWD" /></td>
			</tr>
			<tr>
				<td class="td01 Line1E8E8E8">*<% Response.Write(GetLocalResourceObject("PWD2"));%></td>
				<td class="td023 Line1E8E8E8" colspan="3" ><input type="password" id="TB_PWD2" /></td>
			</tr>
			<tr>
                <td class="td01 Line1E8E8E8">*<%= GetGlobalResourceObject("Member", "PresidentName")%></td>
				<td class="td023 Line1E8E8E8" colspan="3" >
					<input type="text" id="TB_WriterName" name="TB_WriterName" />
					<input type="hidden" id="TB_Duties" name="TB_Duties" value="President" />
				</td>
			</tr>
			<tr>
                <td class="td01 Line1E8E8E8">*<%= GetGlobalResourceObject("Member", "SelectionCountry") %></td>
				<td class="td02 Line1E8E8E8">
					<input type="hidden" id="gcodeExport" name="gcodeExport" value="" />
					<select id="regionCodeI1" style="width:100px;" name="country" size="1" onchange="CountrySelection(this.selectedIndex,'gcodeExport')">
						<option value="">:: <%= GetGlobalResourceObject("Member", "SelectionCountry") %> ::</option>
						<option value="1">KOR&nbsp;한국</option>
						<option value="2">CHN&nbsp;中國</option>
						<option value="3">JPN&nbsp;日本</option>
					</select>
				</td>
				<td class="td01">
					<input type="text" id="JuminNoTEXT" readonly="readonly" style="border:0px; background-color:#f5f5f5; text-align:center;" />
					<input type="hidden" id="JuminNoKOR" value="" />
					<input type="hidden" id="JuminNoCHN" value="" />
				</td>
				<td class="td02">
					<span id="PnJuminChn" style="visibility:hidden;" ><input type="text" id="TB_JuminChn" style="width:150px;" maxlength="22" onblur="OnlyNum(this)" /> </span>
					<span id="PnJuminKor" style="visibility:hidden;" >
						<input type="text" id="TB_Jumin1" size="5" maxlength="6" onkeyup="moveNext(this, 'TB_Jumin2' ,6);" onblur="OnlyNum(this)"  /> - 
						<input type="text" id="TB_Jumin2" size="6" maxlength="7" onblur="OnlyNum(this)" />
					</span>
					<input type="hidden" id="HJumin" name="HJumin" />
				</td>
			</tr>
			<tr>
				<td class="td01">*<%= GetGlobalResourceObject("Member", "TEL")%></td>
				<td colspan="3" class="td023"><input type="text" id="TB_TEL" size="20" maxlength="14" /></td>
			</tr>
			<tr>
				<td class="td01">*<%= GetGlobalResourceObject("Member", "Mobile")%></td>
				<td class="td02"><input type="text" id="TB_Mobile" size="20" maxlength="14" /></td>
				<td colspan="2" rowspan="2" style="text-align:left; padding-top:4px; padding-bottom:4px; background-color:#f5f5f5; padding-left:15px; padding-top:4px; ">
					<%=GetGlobalResourceObject("qjsdur", "ghkanfehckrdprhksgksdkfflaaptpwlfmfqkedmtlrpTtmqslRk") %> <br />
					<input type="radio" name="Damdangja[0]IsMsgReceive" id="Damdangja[0]All" onclick="Rd_IsMsgReceiveClick('0', '3');" checked="checked" />&nbsp;<label for="Damdangja[0]All">All</label>&nbsp;&nbsp;
					<input type="radio" name="Damdangja[0]IsMsgReceive" id="Damdangja[0]Email" onclick="Rd_IsMsgReceiveClick('0', '2');"   />&nbsp;<label for="Damdangja[0]Email">E-mail</label>&nbsp;&nbsp;
					<input type="radio" name="Damdangja[0]IsMsgReceive" id="Damdangja[0]SMS" onclick="Rd_IsMsgReceiveClick('0', '1');"  />&nbsp;<label for="Damdangja[0]SMS">SMS</label>&nbsp;&nbsp;
					<input type="radio" name="Damdangja[0]IsMsgReceive" id="Damdangja[0]Never" onclick="Rd_IsMsgReceiveClick('0', '0');"  />&nbsp;<label for="Damdangja[0]Never">Never</label>
				</td>
			</tr>
			<tr>
				<td class="td01">*<%= GetGlobalResourceObject("Member", "Email")%></td>
                <td class="td02"><input type="text" id="TB_Email" size="30" /></td>
			</tr>
		</table>
		<div style="background-color:#999999; height:1px; font-size:1px; "></div>
		<div style="background-color:#AAAAAA; height:1px; font-size:1px; "></div>
		<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
		<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
		<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
		<div style="background-color:#E4E4E4; height:1px; font-size:1px; "></div>
		<div style="text-align:center; padding:30px; ">
			<input  type="button" value="<%=GetLocalResourceObject("ToNext") %>" style="width:550px; height:40px;" onclick="BTN_Submit_Click()" />
			<span style="visibility:hidden;"><asp:Button runat="server" ID="btn_Submit" onclick="btn_Submit_Click" /></span>
		</div>
	</div>
	<input type="hidden" id="Hidden_IDMustAlphaNNumber" value="<% Response.Write(GetLocalResourceObject("IDMustAlphabetNNumber")); %>" />
	<input type="hidden" id="Hidden_IDMustMore4" value="<% Response.Write(GetLocalResourceObject("IDMustMore4")); %>" />
	<input type="hidden" id="Hidden_IDBeenUsed" value="<% Response.Write(GetLocalResourceObject("IDBeenUsed")); %>" />
	<input type="hidden" id="HIdMust" value="<%=GetLocalResourceObject("IDMust") %>" />
	<input type="hidden" id="HPWDMust" value="<%=GetLocalResourceObject("PWDMust")%>" />
	<input type="hidden" id="HYakguanMust" value="<%=GetLocalResourceObject("YakguanMust")%>" />
	<input type="hidden" id="HPWDDifferent" value="<%=GetLocalResourceObject("PWDDifferent")%>" />
	<input type="hidden" id="HCountryMust" value="<%=GetGlobalResourceObject("Alert","CountryMust")%>" />
	<input type="hidden" id="Hidden_IDOK" value="<% Response.Write(GetLocalResourceObject("IDOK")); %>" />
	<input type="hidden" id="Hidden_IDCHECK" value="<% Response.Write(GetLocalResourceObject("IDCHECK")); %>" />
	<input type="hidden" id="Hidden_MustWriterName" value="<%= GetGlobalResourceObject("Alert","MustPresidentName") %>" />
	<input type="hidden" id="Hidden_MustDuties" value="<%= GetGlobalResourceObject("Alert", "MustDuties") %>" />
	<input type="hidden" id="Hidden_MustJuminNo" value="<%= GetGlobalResourceObject("Alert", "MustJuminNo") %>" />
	<input type="hidden" id="Hidden_NotRightJuminNo" value="<%= GetGlobalResourceObject("Alert", "NotRightJuminNo") %>" />
	<input type="hidden" id="Hidden_WrongTEL" value="<%= GetGlobalResourceObject("Alert", "WrongTEL") %>" />
	<input type="hidden" id="Hidden_WrongMobile" value="<%= GetGlobalResourceObject("Alert", "WrongMobile") %>" />
	<input type="hidden" id="Hidden_WrongEmail" value="<%= GetGlobalResourceObject("Alert", "WrongEmail") %>" />
	<input type="hidden" id="Session1" name="SESSION1" />
    </form>
</body>
</html>