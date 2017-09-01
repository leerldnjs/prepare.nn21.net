<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LogedTopMenuWithoutScript.ascx.cs" Inherits="Member_LogedTopMenuWithoutScript" %>
<meta http-equiv="X-UA-Compatible" content="IE=10"> 
<style type="text/css"> 
 	/* General */
	#cssdropdown, #cssdropdown ul { list-style: none; }
	#cssdropdown, #cssdropdown * { padding: 0; margin: 0; }
	/* Head links */
	#cssdropdown li.headlink { width: 145px; height:34px;  float: left; margin-left: -1px; background-color:#999999; color:White; border-right:1px solid white; font-weight:bold; border-left:1px solid white; text-align: center; }
	#cssdropdown li.headlink span { display: block; padding: 10px; }
	#cssdropdown li.headlinkNoChild { width: 145px; height:34px;  float: left; margin-left: -1px; background-color:#999999; color:White; border-right:1px solid white; font-weight:bold; border-left:1px solid white; text-align: center; }
	#cssdropdown li.headlinkNoChildEmpty { width: 437px; height:34px;  float: left; margin-left: -1px; background-color:#999999; color:White; border-right:1px solid white; font-weight:bold; border-left:1px solid white; text-align: center; }
	#cssdropdown li.headlinkNoChild span { display: block; padding: 10px; }
	/* Child lists and links */
	#cssdropdown li.headlink ul { display:none; border-bottom:2px #999999 solid; border-left:2px #999999 solid; border-right:2px #999999 solid; border-top:2px #FFFFFF solid; padding-top:6px; padding-bottom:6px;   text-align:center; background-position:top; background-color:#999999; line-height:25px;   }
	#cssdropdown li.headlink:hover ul { display: block; background-color:White;  }
	#cssdropdown li.headlink ul li a:hover { background-color: #cccccc; }
	/* Pretty styling */
	#cssdropdown ul li a:hover { text-decoration: none; }
</style>
<link rel="stylesheet" type="text/css" href="../Common/IntlWeb.css" />
	<table border="0" cellpadding="0" cellspacing="0" style="padding:5px; width:900px; ">
		<tr style="height:30px; background-color:#E4E4E4; ">
			<td style="color:black; width:600px; text-align:center;"><a href="../Member/Intro.aspx" >
				<span style=" font-size:16px; font-weight:bold;"><%=CustomerCode %>&nbsp;&nbsp;&nbsp;<%= GetGlobalResourceObject("Member", "BusinessCenter") %></span></a>
			</td>
			<td style="color:black; vertical-align:middle; text-align:left;">
				<span style="cursor:hand; color:black;" onclick="LanguageSet('en')" >english</span>&nbsp;&nbsp;||&nbsp;&nbsp;
				<span style="cursor:hand; color:black;" onclick="LanguageSet('ko')" >한글</span> &nbsp;&nbsp;||&nbsp;&nbsp;
				<span style="cursor:hand; color:black;" onclick="LanguageSet('zh')" >中文</span>
				&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:Button ID="BTN_Logout" runat="server" onclick="BTN_Click_Logout" 
					Text="Log out" UseSubmitBehavior="False" />
			</td>
		</tr>
		<tr >
			<td colspan="2" style="padding-left:7px; ">
				<div id="TopMenu" >
				<ul id="cssdropdown"> 
 					<li class="headlink"> 
						<span><%=GetGlobalResourceObject("qjsdur", "anstjrhksfl")%></span>
 						<ul> 
							<li><a href="../Request/OfferFormWrite.aspx"><%=GetGlobalResourceObject("qjsdur", "audtpwkrtjd")%></a></li> 
							<li><a href="../Request/OfferFormList.aspx"><%=GetGlobalResourceObject("qjsdur", "anstjdufrl")%></a></li> 
						</ul> 
					</li> 
 					<li class="headlink"> 
						<span><%=GetGlobalResourceObject("qjsdur", "anffbrhksfl")%></span>
 						<ul> 
							<li><a href="../Request/RequestWrite.aspx"><%=GetGlobalResourceObject("qjsdur", "anffbwjqtn")%></a></li>
							 <li><a href="../Request/RequestList.aspx"><%=GetGlobalResourceObject("Member", "TradeHistory")%></a></li> 
						</ul> 
					</li> 
					<li class="headlink"> 
						<span><%=GetGlobalResourceObject("qjsdur", "wjdqhrhksfl")%></span>
 						<ul> 
							<li><a href="../Member/OwnCustomerList.aspx"><%=GetGlobalResourceObject("Member", "InfoCustomer")%></a></li> 
							<li><a href="../Member/MyCompanyView.aspx"><%=GetGlobalResourceObject("Member", "InfoCompany")%></a></li> 
							<li><a href="../Member/MyAccountView.aspx"><%=GetGlobalResourceObject("Member", "InfoAccount")%></a></li> 
							<li><a href="../Member/MyStaffView.aspx"><%=GetGlobalResourceObject("qjsdur", "djqandnjswjdqh") %></a></li> 
						</ul> 
					</li> 
					<li class="headlinkNoChildEmpty"> 
						<span>&nbsp;</span>
						<ul><li>&nbsp;</li></ul>
					</li>
 				</ul> 
			</div>
			</td>
		</tr>
	</table>
	<div style="background-color:#777777; height:1px; font-size:1px; "></div>
	<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
	<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
	<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>