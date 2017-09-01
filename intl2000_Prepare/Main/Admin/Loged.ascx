<%@ Control Language="C#" AutoEventWireup="true" Debug="true" CodeFile="Loged.ascx.cs" Inherits="Admin_Loged" %>
<script src="/Common/public.js" type="text/javascript"></script>
<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
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

<div style="background-color: #999999; padding: 10px;">
   <table border="0" cellpadding="0" cellspacing="0" style="width: 880px;">
      <tr>
         <td colspan="4" style="text-align: center; color: WindowText; font-size: 18px;"><span style="font-size: 18px; font-weight: bold; color: #7F0000;">
            <%=MEMBERINFO[2] %></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; I LOGISTICS NETWORK SYSTEM
				<asp:DropDownList ID="SelectBranch" Visible="false" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SelectBranch_SelectedIndexChanged">
               <asp:ListItem Value="3157">KRIC</asp:ListItem>
               <asp:ListItem Value="3843">CNGZ</asp:ListItem>
               <asp:ListItem Value="2887">CNSY</asp:ListItem>
               <asp:ListItem Value="2888">CNYW</asp:ListItem>
               <asp:ListItem Value="2886">CNYT</asp:ListItem>
               <asp:ListItem Value="3388">CNQD</asp:ListItem>
			   <asp:ListItem Value="11456">CNHZ</asp:ListItem>
               <asp:ListItem Value="7898">CNSX</asp:ListItem>
               <asp:ListItem Value="3798">Other</asp:ListItem>
            </asp:DropDownList>
            
         </td>
         <td style="text-align: right; color: White;">
            <span style="cursor: pointer; color: White;" onclick="LanguageSet('en')">english</span>&nbsp;&nbsp;||&nbsp;&nbsp;
				<span style="cursor: pointer; color: White;" onclick="LanguageSet('ko')">한글</span> &nbsp;&nbsp;||&nbsp;&nbsp;
				<span style="cursor: pointer; color: White;" onclick="LanguageSet('zh')">中文</span>&nbsp;&nbsp;
				<span style="visibility: hidden;">
               <asp:Button runat="server" ID="BTN_GoSerch" OnClick="Btn_GoSerch_Click" /></span>
            <asp:Button ID="LogOut" runat="server" OnClick="Button1_Click" TabIndex="99" UseSubmitBehavior="False" />
            <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "rptlvks") %>" onclick="location.href = '../Board/BoardMain.aspx';" />
         </td>
      </tr>
      <tr>
         <td style="color: White; font-weight: bold; font-size: 20px; text-align: right; width: 200px; height: 35px; vertical-align: middle;">In Bound </td>
         <td style="width: 156px;">&nbsp;&nbsp;<input type="button" value="<%=GetGlobalResourceObject("qjsdur", "ehckrdPwjd") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '../Admin/RequestList.aspx?G=Arrival';" />
            <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "wjqtngusghkd") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '../Admin/TransportBetweenBranchList.aspx?G=in';" />
         </td>
         <td rowspan="2" style="width: 74px;">
            <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "qkfthdwnsql") %>" style="width: 70px; height: 65px; font-size: 12px;" onclick="location.href = '../Admin/PrepareTransport.aspx';" />
         </td>
         <td>
            <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "cnfrhgusghkd") %>" style="width: 70px; height: 30px; font-size: 12px;" />
         </td>
         <td style="padding-right: 10px; text-align: right;">

            <%=ButtonRight %>
         </td>
      </tr>
      <tr>
         <td style="color: White; font-weight: bold; font-size: 20px; text-align: right; height: 35px;">Out Bound</td>
         <td>&nbsp;&nbsp;<input type="button" value="<%=GetGlobalResourceObject("qjsdur", "wjqtndPdir") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '../Admin/RequestList.aspx?G=5051';" />
            <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "wjqtngusghkd") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '../Admin/RequestList.aspx?G=5455';" />
         </td>
         <td>
            <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "cnfrhgusghkd") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '../Admin/TransportBetweenBranchList.aspx?G=out';" />
         </td>
         <td style="padding-right: 10px; text-align: right;">
            <select id="SerchOption" onchange="SerchOptionChange();">
               <option value="Serch"><%=GetGlobalResourceObject("qjsdur", "tkdgh") %> & <%=GetGlobalResourceObject("qjsdur", "rhrorqjsgh") %></option>
               <option value="SerchItem">Major Item</option>
               <%--<option value="SerchBL">BLNo</option>--%>
               <%=SerchBL+"" %>
               <option value="SerchAll"><%=GetGlobalResourceObject("qjsdur", "wjscp") %></option>
            </select>
            <input id="HSerchOption" name="HSerchOption" type="hidden" value="Serch" />
            <input id="logedAccountID" type="hidden" value="<%=MEMBERINFO[2] %>" />
            <input type="text" name="SerchValue" onkeydown="GoSerch();" style="width: 120px; ime-mode:active;" />
         </td>
      </tr>
   </table>
</div>
<div style="background-color: #777777; height: 1px; font-size: 1px;"></div>
<div style="background-color: #BBBBBB; height: 1px; font-size: 1px;"></div>
<div style="background-color: #CCCCCC; height: 1px; font-size: 1px;"></div>
<div style="background-color: #DDDDDD; height: 1px; font-size: 1px;"></div>

<div style="float: left; background-color: #f5f5f5; text-align: center; width: 150px; padding-top: 4px; padding-bottom: 4px;">
   <div style="padding: 20px; vertical-align: middle;">
      <a href="../Admin/Intro.aspx"><span style="font-size: 16px; font-weight: bold;"><%=CustomerCode %></span></a>
   </div>
   <table id="RequestFormNew" style="width: 130px; margin: 10px;" border="0" cellpadding="0" cellspacing="0">
      <tr>
         <td><%=GetGlobalResourceObject("qjsdur", "wjqtndlftl") %></td>
         <td style="width: 70px;"><%=GetGlobalResourceObject("Member", "CompanyCode") %></td>
      </tr>
      <%=RecentRequest %>
   </table>
   <div style="padding: 10px;"></div>
</div>
