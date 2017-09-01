<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyStaffView.aspx.cs" Inherits="Member_MyStaffView" Debug="true" %>
<meta http-equiv="X-UA-Compatible" content="IE=10"> 
<%@ Register src="../Member/LogedTopMenu.ascx" tagname="Loged" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />

	<style type="text/css">
		.tdSubT	{ border-bottom-width: 2px; border-bottom-style: solid; border-bottom-color: #93A9B8;	padding-top:30px; width:900px; padding-bottom:3px; }
		.td01
		{
			background-color:#f5f5f5; text-align:center; height:25px; width:150px;padding-top:4px; padding-bottom:4px;
			border-bottom-style:dotted; border-bottom-color:#E8E8E8; border-bottom-width:1px;
		}
		.td02
		{
			width:300px; padding-top:4px;padding-bottom:4px; padding-left:10px;
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
	<script src="../Common/public.js" type="text/javascript"></script>
	<script type="text/javascript">
		var ISMSGRECEIVE = new Array();
		ISMSGRECEIVE[0] = "3";
		ISMSGRECEIVE[1] = "3";
		var IsIDCheck = new Array();
		IsIDCheck[0] = 0;
		IsIDCheck[1] = 0;
		window.onload = function () {
			$(".NavInformation").addClass("active");
		}

		function IDCheck(which) {
			var target = document.getElementById("Damdangja[" + which + "]ID").value;
			if (target.length > 3) {
				regA1 = new RegExp(/^[0-9a-zA-Z]+$/);
				if (regA1.test(target)) {
					Member.UniqueCheck('AccountID', target, function (result) {
						if (result == "0") {
							alert("This ID is available.");
							document.getElementById("Damdangja[" + which + "]IsChecked").value = "1";
						}
						else {
							alert("This ID already used.");
							document.getElementById("Damdangja[" + which + "]IsChecked").value = "0";
						}
					}, function (result) { alert("ERROR : " + result); });
				}
			}
		}

		function BTN_Submit_Click() {
			var staffsum = "";
			var staffrow = parseInt(form1.HStaffCount.value);
			for (var i = 0; i < staffrow; i++) {
				var Pk = document.getElementById("Damdangja[" + i + "]Pk").value;
				var Duties = document.getElementById("Damdangja[" + i + "]Duties").value;
				var Name = document.getElementById("Damdangja[" + i + "]Name").value;
				var ID = document.getElementById("Damdangja[" + i + "]ID").value;
				var HID = document.getElementById("Damdangja[" + i + "]HID").value;
				var IsChecked = document.getElementById("Damdangja[" + i + "]IsChecked").value;
				var PWD = document.getElementById("Damdangja[" + i + "]PWD").value;
				var TEL = document.getElementById("Damdangja[" + i + "]TEL").value;
				var MSG = document.getElementById("Damdangja[" + i + "]MSG").value;
				var Mobile = document.getElementById("Damdangja[" + i + "]Mobile").value;
				var Email = document.getElementById("Damdangja[" + i + "]Email").value;
				var Authority = document.getElementById("Damdangja[" + i + "]Authority").value;

				if (ID != HID) {
					if (IsChecked != "1") {
						alert("아이디를 변경하려면 아이디 체크를 해주셔야 합니다. "); return false;
					}
				}
				else {
					ID = "N0tCh@nged";
				}

				if (PWD == "******") {
					PWD = "N0tCh@nged";
				}

				staffsum+= Pk + "@@" + ID + "@@" + PWD + "@@" + Duties + "@@" + Name + "@@" + TEL + "@@" + Mobile + "@@" + Email + "@@" + Authority + "@@" + MSG + "####";
			}
			if (staffsum == "") {
				alert("수정할 내용이 없습니다");
				return false;
			}
			
			Member.MyStaffModify(staffsum, function (result) {
				if (result == "1") {
					alert("SUCCESS");
					document.location.reload();
				}
			}, function (result) { alert(result); });
		}
	</script>
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px; ">
    <form id="form1" runat="server">
	<asp:ScriptManager ID="Member" runat="server"><Services><asp:ServiceReference Path="~/WebService/Member.asmx" /></Services></asp:ScriptManager>
	<uc1:Loged ID="Loged1" runat="server" />
    <div class="ContentsTopMenu">
		<p>
			<a href="OwnCustomerList.aspx"><%=GetGlobalResourceObject("Member", "InfoCustomer")%></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="MyCompanyView.aspx"><%=GetGlobalResourceObject("Member", "InfoCompany")%></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="MyAccountView.aspx"><%=GetGlobalResourceObject("Member", "InfoAccount")%></a>&nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;
			<a href="MyStaffView.aspx"><strong><%=GetGlobalResourceObject("qjsdur", "djqandnjswjdqh") %></strong></a>
		</p>
		<p>&nbsp;</p>
		<input type="hidden" id="HCompanyPk" value="<%=MEMBERINFO[1] %>" />
		<input type="hidden" id="HDebug" onclick="this.select();" />
		<%=StaffList %>
	<%--	<table border="0" cellpadding="0" cellspacing="0" style="width:850px;">
			<tr>
				<td class="Line1E8E8E8" rowspan="4" style="width:90px; background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px; font-weight:bold;">
					* <%= GetGlobalResourceObject("Member", "Staff_FreightMan") %><input type="hidden" value="1" id="Damdangja1[0]"/></td>
				<td class="Line1E8E8E8" style="width:90px; background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">
					<%=GetGlobalResourceObject("Member","Buseo") %> or <%=GetGlobalResourceObject("Member","JikWi") %></td>
				<td class="Line1E8E8E8" style="width:140px; background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">
					<%=GetGlobalResourceObject("Member", "Name") %></td>
				<td class="Line1E8E8E8" style="background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">ID</td>
				<td class="Line1E8E8E8" style="width:160px; background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">Password</td>
			</tr>
			<tr>
				<td class="tdStaffBody"><input type="text" id="Damdangja2[0]" style="width:70px;" /></td>
				<td class="tdStaffBody"><input type="text" id="Damdangja4[0]" style="width:110px;" /></td>
				<td class="tdStaffBody">
					<input type="text" id="Damdangja[0]ID" style="width:110px;" />
					<input type="hidden" id="H[0]ID" />					
					<input type="button" value="ID Check" onclick="IDCheck('0');" /> </td>
				<td class="tdStaffBody"><input type="password" id="Damdangja[0]PWD" style="width:130px;" /></td>	
			</tr>
			<tr>
				<td class="tdStaffBody" colspan="5" style="text-align:left;">
					TEL : <input type="text" id="Damdangja5[0]" />
					<span style="padding-left:40px;">Mobile : <input type="text" id="Damdangja8[0]" /></span>
					<span style="padding-left:40px;">E-mail : <input type="text" id="Damdangja11[0]" size="27" onblur="onlyEmail(this)" /></span>
				</td>
			</tr>
			<tr>
				<td class="tdStaffBody" colspan="5" style="text-align:left;">
					<%=GetGlobalResourceObject("qjsdur", "ghkanfehckrdprhksgksdkfflaaptpwlfmfqkedmtlrpTtmqslRk") %> 
					<input type="radio" name="Damdangja[0]IsMsgReceive" id="Damdangja[0]All" onclick="Rd_IsMsgReceiveClick('0', '3');" checked="checked" />
						&nbsp;<label for="Damdangja[0]All">All</label>&nbsp;&nbsp;
					<input type="radio" name="Damdangja[0]IsMsgReceive" id="Damdangja[0]Email" onclick="Rd_IsMsgReceiveClick('0', '2');"   />
						&nbsp;<label for="Damdangja[0]Email">E-mail</label>&nbsp;&nbsp;
					<input type="radio" name="Damdangja[0]IsMsgReceive" id="Damdangja[0]SMS" onclick="Rd_IsMsgReceiveClick('0', '1');"  />
						&nbsp;<label for="Damdangja[0]SMS">SMS</label>&nbsp;&nbsp;
					<input type="radio" name="Damdangja[0]IsMsgReceive" id="Damdangja[0]Never" onclick="Rd_IsMsgReceiveClick('0', '0');"  />
						&nbsp;<label for="Damdangja[0]Never">Never</label>
				</td>
			</tr>
		</table>
		<div style="background-color:#999999; height:1px; font-size:1px; "></div>
		<table border="0" cellpadding="0" cellspacing="0" style="width:850px;">
			<tr>
				<td class="Line1E8E8E8" rowspan="4" style="width:90px; background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px; font-weight:bold;">
					<%= GetGlobalResourceObject("Member", "Staff_Accountancy") %><input type="hidden" value="0" id="Damdangja1[1]" /></td>
				<td class="Line1E8E8E8" style="width:90px; background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">
					<%=GetGlobalResourceObject("Member","Buseo") %> or <%=GetGlobalResourceObject("Member","JikWi") %></td>
				<td class="Line1E8E8E8" style="width:140px; background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">
					<%=GetGlobalResourceObject("Member", "Name") %></td>
				<td class="Line1E8E8E8" style="background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">ID</td>
				<td class="Line1E8E8E8" style="width:160px; background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;">Password</td>
			</tr>
			<tr>
				<td class="tdStaffBody"><input type="text" id="Damdangja2[1]" style="width:70px;" /></td>
				<td class="tdStaffBody"><input type="text" id="Damdangja4[1]" style="width:110px;" /></td>
				<td class="tdStaffBody">
					<input type="text" id="Damdangja[1]ID" style="width:110px;" />
					<input type="hidden" id="H[1]ID" />
					<input type="button" value="ID Check" onclick="IDCheck('1');" /> </td>
				<td class="tdStaffBody">
					<input type="password" id="Damdangja[1]PWD" style="width:130px;" />
				</td>	
			</tr>
			<tr>
				<td class="tdStaffBody" colspan="5" style="text-align:left;">
					TEL : <input type="text" id="Damdangja5[1]" />
					<span style="padding-left:40px;">Mobile : <input type="text" id="Damdangja8[1]" /></span>
					<span style="padding-left:40px;">E-mail : <input type="text" id="Damdangja11[1]" size="27" onblur="onlyEmail(this)" /></span>
				</td>
			</tr>
			<tr>
				<td class="tdStaffBody" colspan="5" style="text-align:left;">
					<%=GetGlobalResourceObject("qjsdur", "ghkanfehckrdprhksgksdkfflaaptpwlfmfqkedmtlrpTtmqslRk") %> 
					<input type="radio" name="Damdangja[1]IsMsgReceive" id="Damdangja[1]All" checked="checked" onclick="Rd_IsMsgReceiveClick('1', '3');" />
						&nbsp;<label for="Damdangja[1]All">All</label>&nbsp;&nbsp;
					<input type="radio" name="Damdangja[1]IsMsgReceive" id="Damdangja[1]Email" onclick="Rd_IsMsgReceiveClick('1', '2');"  />
						&nbsp;<label for="Damdangja[1]Email">E-mail</label>&nbsp;&nbsp;
					<input type="radio" name="Damdangja[1]IsMsgReceive" id="Damdangja[1]SMS" onclick="Rd_IsMsgReceiveClick('1', '1');"  />
						&nbsp;<label for="Damdangja[1]SMS">SMS</label>&nbsp;&nbsp;
					<input type="radio" name="Damdangja[1]IsMsgReceive" id="Damdangja[1]Never" onclick="Rd_IsMsgReceiveClick('1', '0');"  />
						&nbsp;<label for="Damdangja[1]Never">Never</label>
				</td>
			</tr>
		</table>--%>
		<div style="text-align:center; padding-top:40px; ">
			<%=BTNSUBMIT %>
			<input type="hidden" id="wjwkddhksfy" value="<%=GetGlobalResourceObject("Alert", "wjwkddhksfy") %>" />
			<input type="hidden" id="HStaffCount" value="<%=RowCount %>" />
		</div>
	</div>
    </form>
</body>
</html>