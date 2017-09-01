<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Loged.ascx.cs" Inherits="CustomClearance_Loged" %>
<script src="../Common/public.js" type="text/javascript"></script>
<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    function SerchOptionChange() {
        document.getElementById("HSerchOption").value = document.getElementById("SerchOption").value;
    }

    //function GoSerch() {
        //if (window.event.keyCode == 13) {
		    //< %= Page.GetPostBackEventReference( BTN_GoSerch ) %>
		//}
	//}
</script>
<div style="background-color:#999999; padding:10px;">
	<table border="0" cellpadding="0" cellspacing="0"  >
		<tr>
			<td style="text-align:center; color:WindowText; font-size:18px; ">
				<span style=" font-size:18px; font-weight:bold; color:#7F0000;">
				<%=MEMBERINFO[2] %></span>
				&nbsp;&nbsp;&nbsp;&nbsp;I LOGISTICS NETWORK SYSTEM&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
			<td style="text-align:right; color:White; padding-right:10px; ">
				<%--<input type="button" value="관부가세" onclick="location.href='../Admin/CheckDescriptionList.aspx'" />
				<input type="button" value="세금맞추기" onclick="location.href = '../Admin/SetTariff.aspx'" />
				<input type="button" value="세금송금리스트" onclick="location.href = '../Admin/AccountTariffList.aspx'" />--%>
				<input type="button" value="통관업무" onclick="location.href = '../Admin/StorageOutForCustoms.aspx'" />
                <input type="button" value="게시판" onclick="location.href='../Board/BoardMain.aspx' ;" />
				<asp:Button ID="LogOut" runat="server" onclick="Button1_Click" TabIndex="99" Text="로그아웃" UseSubmitBehavior="False"  />
			</td>
		</tr>
       
	</table>
</div>
<div style="background-color:#777777; height:1px; font-size:1px; "></div>
<div style="background-color:#BBBBBB; height:1px; font-size:1px; "></div>
<div style="background-color:#CCCCCC; height:1px; font-size:1px; "></div>
<div style="background-color:#DDDDDD; height:1px; font-size:1px; "></div>