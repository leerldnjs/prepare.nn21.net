<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header_ShippingBranch.ascx.cs" Inherits="ViewShare_Header_ShippingBranch" %>

<script src="/Common/public.js" type="text/javascript"></script>
<script type="text/javascript">
	function SerchOptionChange() {
		document.getElementById("HSerchOption").value = document.getElementById("SerchOption").value;
	}
	function GoSerch() {
		if (window.event.keyCode == 13) {
			<%= Page.GetPostBackEventReference( BTN_GoSerch ) %>
		}
	}
</script>

<div style="width:400px; float:right; margin-right:10px;">
	<div style="float:right;"><asp:Button ID="LogOut" runat="server" OnClick="Button1_Click" TabIndex="99" UseSubmitBehavior="False" class="btn btn-default" style="width: 70px; height: 30px; font-size: 12px; margin-left:5px;" /></div>
		<% if (Session["Type"] + "" != "ShippingBranch") {%>
			<div style="float:right;"><input type="button" style="width: 70px; height: 30px; font-size: 12px;" class="btn btn-default" value="<%=GetGlobalResourceObject("qjsdur", "rptlvks") %>" onclick="location.href = '../Board/BoardMain.aspx';" /></div>
		<%} %>
	<div style="color:white;">
		<span style="cursor: pointer; " onclick="LanguageSet('en')">english</span>&nbsp;&nbsp;||&nbsp;&nbsp;
		<span style="cursor: pointer; " onclick="LanguageSet('ko')">한글</span> &nbsp;&nbsp;||&nbsp;&nbsp;
		<span style="cursor: pointer; " onclick="LanguageSet('zh')">中文</span>&nbsp;&nbsp;
		<span style="visibility: hidden;"><asp:Button runat="server" ID="BTN_GoSerch" OnClick="Btn_GoSerch_Click" /></span>
	</div>
	<div style="clear:both; text-align:right; padding-top:10px; padding-bottom:10px; ">
		<%if (MEMBERINFO[1] + "" == "3157") { %>
		<input type="button" class="btn btn-default btn-sm" value="세납" onclick="location.href='/Admin/Taxpaid.aspx'" style="height: 28px; font-size: 12px;" />
		<%} %>
		<input type="button" class="btn btn-danger btn-sm" value="未" onclick="location.href='/Finance/SimpleMisu.aspx';" /> 
		<input type="button" class="btn btn-default btn-sm" value="환율정보" onclick="location.href = '/Admin/ExchangeRate.aspx?G=V';" style=" height:28px; font-size:12px; ">
		<input type="button" class="btn btn-default btn-sm" value="거래처 정보" onclick="location.href = '/Admin/CompanyList.aspx';" style="height:28px; font-size:12px; ">
		<input type="button" class="btn btn-default btn-sm" value="업체등록" style=" height:28px;font-size:12px;" onclick="Goto('NewCompany', '<%=Session["CompanyPk"]%>');">
	</div>
	<div style="text-align:right;">
		<select id="SerchOption" onchange="SerchOptionChange();">
			<option value="Serch"><%=GetGlobalResourceObject("qjsdur", "tkdgh") %> & <%=GetGlobalResourceObject("qjsdur", "rhrorqjsgh") %></option>
			<option value="SerchItem">Major Item</option>
			<option value="SerchAll"><%=GetGlobalResourceObject("qjsdur", "wjscp") %></option>
		</select>
		<input id="HSerchOption" name="HSerchOption" type="hidden" value="Serch" />
		<input type="text" name="SerchValue" onkeydown="GoSerch();" style="width: 120px; ime-mode:active;" />
	</div>
</div>
<table border="0" style="width: 680px;">
	<tr>
		<td colspan="2" style="text-align: center; color: WindowText; font-size: 18px;"><span style="font-size: 18px; font-weight: bold; color: #7F0000;"><%=MEMBERINFO[2] %></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; I LOGISTICS NETWORK SYSTEM
			<asp:DropDownList ID="SelectBranch" Visible="false" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SelectBranch_SelectedIndexChanged">
				<asp:ListItem Value="3157">KRIC</asp:ListItem>
				<asp:ListItem Value="3843">CNGZ</asp:ListItem>
				<asp:ListItem Value="2887">CNSY</asp:ListItem>
				<asp:ListItem Value="2888">CNYW</asp:ListItem>
				<asp:ListItem Value="2886">CNYT</asp:ListItem>
				<asp:ListItem Value="3388">CNQD</asp:ListItem>
				<asp:ListItem Value="11456">CNHZ</asp:ListItem>
				<asp:ListItem Value="7898">CNSX</asp:ListItem>
				<asp:ListItem Value="12437">VTHM</asp:ListItem>
				<asp:ListItem Value="12438">VTHN</asp:ListItem>
				<asp:ListItem Value="12527">VTDN</asp:ListItem>
				<asp:ListItem Value="12464">MMYG</asp:ListItem>
				<asp:ListItem Value="3798">Other</asp:ListItem>
			</asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td style="color: White; font-weight: bold; font-size: 20px; text-align: right; width: 200px; height: 35px; vertical-align: middle;">In Bound </td>
		<td style="width: 480px;">&nbsp;&nbsp;
			<input type="button" class="btn btn-default btn-sm" value="<%=GetGlobalResourceObject("qjsdur", "ehckrdPwjd") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '../Admin/RequestList.aspx?G=Arrival';" />
			<input type="button" class="btn btn-default btn-sm" value="<%=GetGlobalResourceObject("qjsdur", "wjqtngusghkd") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '/Transport/TransportList.aspx?G=in';" />
			<input type="button" class="btn btn-default btn-sm" value="<%=GetGlobalResourceObject("qjsdur", "qothdwnsql") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '../Admin/StorageOut.aspx';" />
			<input type="button" class="btn btn-default btn-sm" value="<%=GetGlobalResourceObject("qjsdur", "cnfrhgusghkd") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '../Admin/TransportBetweenCompanyList.aspx';" />
		</td>
	</tr>
	<tr>
		<td style="color: White; font-weight: bold; font-size: 20px; text-align: right; height: 35px;">Out Bound</td>
		<td>&nbsp;&nbsp;
			<input type="button" class="btn btn-default btn-sm" value="<%=GetGlobalResourceObject("qjsdur", "wjqtndPdir") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '../Admin/RequestList.aspx?G=5051';" />
			<input type="button" class="btn btn-default btn-sm" value="<%=GetGlobalResourceObject("qjsdur", "wjqtngusghkd") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '../Admin/RequestList.aspx?G=5455';" />
			<input type="button" class="btn btn-default btn-sm" value="<%=GetGlobalResourceObject("qjsdur", "qkfthdwnsql") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '/Transport/Storage.aspx';" />
			<input type="button" class="btn btn-default btn-sm" value="<%=GetGlobalResourceObject("qjsdur", "cnfrhgusghkd") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '/Transport/TransportList.aspx?G=out';" />
		</td>
	</tr>
	<%=FinanceOnly %>
</table>
