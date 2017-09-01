<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LogedTopMenu.ascx.cs" Inherits="Member_LogedTopMenu" %>
<meta http-equiv="X-UA-Compatible" content="IE=10"> 

<!--link href="/menu_assets/styles.css" rel="stylesheet" type="text/css"-->
<script src="../Common/public.js" type="text/javascript"></script>
<%if (NowUri != "RequestWrite") { %>
  <%--<script src="../Common/jquery-1.4.2.min.js" type="text/javascript"></script>--%>
<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.1/themes/base/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.1/jquery-ui.js"></script>

<%  } %>

<link rel="stylesheet" type="text/css" href="../Common/IntlWeb.css" />

	<table border="0" cellpadding="0" cellspacing="0" style="padding:5px; width:900px; ">
		<tr style="height:30px; background-color:#E4E4E4; ">
			<td style="color:black; width:600px; text-align:center;"><a href="../Member/Intro.aspx" >
				<span style=" font-size:16px; font-weight:bold;"><%=CustomerCode %>&nbsp;&nbsp;&nbsp;<%= GetGlobalResourceObject("Member", "BusinessCenter") %></span></a>
			</td>
			<td style="color:black; vertical-align:middle; text-align:left;">
				<span style="cursor:pointer; color:black;" onclick="LanguageSet('en')" >English</span>&nbsp;&nbsp;||&nbsp;&nbsp;
				<span style="cursor:pointer; color:black;" onclick="LanguageSet('ko')" >한글</span> &nbsp;&nbsp;||&nbsp;&nbsp;
				<span style="cursor:pointer; color:black;" onclick="LanguageSet('zh')" >中文</span>
				&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:Button ID="BTN_Logout" runat="server" onclick="BTN_Click_Logout" Text="Log out" UseSubmitBehavior="False" />
			</td>
		</tr>
	</table>	

	<div id="cssmenu">
		<ul>
			<li class="NavInformation" style="float: right; margin-right: 40px;"><a href="../Member/OwnCustomerList.aspx"><span><%=GetGlobalResourceObject("qjsdur", "wjdqhrhksfl")%></span></a>
				<ul>
					<li style="width:180px;"><a href="../Member/OwnCustomerList.aspx"><%=GetGlobalResourceObject("Member", "InfoCustomer")%></a></li>
					<li style="width:180px;"><a href="../Member/MyCompanyView.aspx"><%=GetGlobalResourceObject("Member", "InfoCompany")%></a></li>
					<li style="width:180px;"><a href="../Member/MyAccountView.aspx"><%=GetGlobalResourceObject("Member", "InfoAccount")%></a></li>
					<li style="width:180px;"><a href="../Member/MyStaffView.aspx"><%=GetGlobalResourceObject("qjsdur", "djqandnjswjdqh") %></a></li>
				</ul>
			</li>

			<li class="NavDocu"><a href="../Request/newCommercialInvoiceWrite.aspx?&M=JW"><span><%=GetGlobalResourceObject("qjsdur", "anstjrhksfl")%></span></a>
				<ul>
					<li style="width:180px;"><a href="../Request/newCommercialInvoiceWrite.aspx?&M=JW"><%=GetGlobalResourceObject("qjsdur", "audtpwkrtjd")%></a></li>
					<li style="width:180px;"><a href="../Admin/CompanyInfoDocumentList.aspx"><%=GetGlobalResourceObject("qjsdur", "anstjdufrl")%></a></li>
				</ul>
			</li>
			<li class="NavRequestWrite">
				<a href="../Request/RequestWrite.aspx"><span><%=GetGlobalResourceObject("qjsdur", "anffbwjqtn")%></span></a>
			</li>
			<li class="NavRequestList">
				<a href="../Request/RequestList.aspx"><span><%=GetGlobalResourceObject("Member", "TradeHistory")%></span></a>
			</li>
			<li class="NavHSCodeInfo">
				<a href="../CustomClearance/HSCode_Tariff.aspx"><span>한중 FTA</span></a>
			</li>
			<%--<li class="Nav2014">
				<a><span>2014 결산</span></a>
				<ul>
					<li><a href="../Calculate/WithCustomer.aspx">거래처별 수출입자료</a></li>
					<li><a href="../Calculate/WithClearance.aspx">세관 통관자료</a></li>
				</ul>
			</li>--%>

			<%--<li class="NavTrade"><a href="../UploadedFiles/CommentWithFile_Member.aspx"><span><%=GetGlobalResourceObject("qjsdur", "andurthdrma")%></span></a>
 				<ul> 
					<li><a href="../UploadedFiles/CommentWithFile_Member.aspx"><%=GetGlobalResourceObject("qjsdur", "andurthdrma")%></a></li> 
                     <li><a href="../UploadedFiles/FileList.aspx"><%=GetGlobalResourceObject("qjsdur", "andurthdrma")%>list</a></li> 
				</ul> 
			</li>--%>
		</ul>
	</div>
