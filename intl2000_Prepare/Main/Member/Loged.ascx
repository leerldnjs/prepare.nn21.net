<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Loged.ascx.cs" Inherits="Member_Loged" %>
<meta http-equiv="X-UA-Compatible" content="IE=10"> 
<script src="../Common/public.js" type="text/javascript"></script>
<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<table border="0" cellpadding="0" cellspacing="0" style="background-color:#999999; padding:10px; width:900px; ">
		<tr style="height:30px;">
			<td style="color:White; font-weight:bold; font-size:20px; width:700px; text-align:center;"><%= GetGlobalResourceObject("Member", "BusinessCenter") %></td>
			<td style="color:White;">
				<span style="cursor:hand; color:White;" onclick="LanguageSet('en')" >english</span>&nbsp;&nbsp;||&nbsp;&nbsp;
				<span style="cursor:hand; color:White;" onclick="LanguageSet('ko')" >한글</span> &nbsp;&nbsp;||&nbsp;&nbsp;
				<span style="cursor:hand; color:White;" onclick="LanguageSet('zh')" >中文</span></td>
			</tr>
	</table>
	<div style="background-color:#777777; height:1px; font-size:1px; "></div>
	<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
	<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
	<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>
	<div  style="float:left;">	
		<div style="background-color:#f5f5f5; text-align:center; width:150px; padding-top:4px; padding-bottom:4px;" >
			<div style="padding:20px; vertical-align:middle;"><a href="../Member/Intro.aspx" ><span style=" font-size:16px; font-weight:bold;"><%=CustomerCode %></span></a></div>
			<div style="padding:5px;"><asp:Button ID="Button2" runat="server" Text="문서관리" Width="80px" PostBackUrl="~/Request/OfferFormWrite.aspx" /></div>
			<div style="padding:5px;"><asp:Button ID="RequestFormL" runat="server" Text="물류관리" Width="80px" PostBackUrl="~/Request/RequestList.aspx" /></div>
			<%--<div style="padding:10px;"><asp:Button ID="BTN_CustomsClearance" runat="server" Text="자료관리" PostBackUrl="~/ForProvideDocuments/CustomsClearanceList.aspx" /></div>--%>
			<div style="padding:5px;"><asp:Button ID="BTN_Warehouse" runat="server" Text="창고관리" Width="80px" /></div>
			<div style="padding:5px;"><asp:Button ID="BTN_Order" runat="server" Text="오더관리" Width="80px" /></div>
			<div style="padding:5px;"><asp:Button ID="OwnCustomerList" runat="server" Text="정보관리" Width="80px" PostBackUrl="~/Member/OwnCustomerList.aspx" /></div>
			<div style="padding:5px;"><asp:Button ID="BTN_Fianacial" runat="server" Text="재무관리" Width="80px" /></div>
			<div style="padding:5px;"><asp:Button ID="Button1" runat="server" onclick="Button1_Click" Width="80px" Text="로그아웃" /></div>
		</div>
	</div>