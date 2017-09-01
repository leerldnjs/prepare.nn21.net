<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Logedsmall.ascx.cs" Inherits="Logistics_Logedsmall" %>
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
<div style="background-color: #999999; padding: 10px;">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 650px;">
        <tr>
            <td colspan="4" style="text-align: center; color: WindowText; font-size: 18px;"><span style="font-size: 18px; font-weight: bold; color: #7F0000;">
                <%=MEMBERINFO[2] %></span>&nbsp;&nbsp;&nbsp;&nbsp;I LOGISTICS NETWORK SYSTEM</td>
            <td style="text-align: right; color: White; padding-right: 10px;">
                <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "rptlvks") %>" onclick="location.href = '../Board/BoardMain.aspx';" />
                <asp:Button ID="LogOut" runat="server" OnClick="Button1_Click" TabIndex="99"  UseSubmitBehavior="False" />
            </td>
        </tr>
        <tr>
            <td style="color: White; font-weight: bold; font-size: 18px; text-align: left; width: 100px; height: 35px; vertical-align: middle;">In Bound </td>
            <td colspan="3">&nbsp;
				<input type="button" value="<%=GetGlobalResourceObject("qjsdur", "ehckrdPwjd") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '../Logistics/RequestList_Logistics.aspx';" />
                <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "qothdwnsql") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '../Logistics/StorageOut_Logistics.aspx';" />
                <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "cnfrhgusghkd") %>" onclick="location.href = '../Logistics/TransportBetweenCompanyList_Logistics.aspx';" style="width: 70px; height: 30px; font-size: 12px;" />
            </td>
            <td style="padding-right: 10px; text-align: right;">

                <%--<span style="visibility:hidden;"><asp:Button runat="server" ID="BTN_GoSerch" onclick="Btn_GoSerch_Click" /></span>
                <select id="SerchOption" onchange="SerchOptionChange();">
					<option value="Serch"><%=GetGlobalResourceObject("qjsdur", "tkdgh") %> & <%=GetGlobalResourceObject("qjsdur", "rhrorqjsgh") %></option>
					<option value="SerchItem">Major Item</option>
					<option value="SerchBL">BLNo</option>
					<option value="SerchAll"><%=GetGlobalResourceObject("qjsdur", "wjscp") %></option>
				</select>
				<input id="HSerchOption" name="HSerchOption" type="hidden" value="Serch" />
				<input id="logedAccountID" type="hidden" value="<%=MEMBERINFO[2] %>" />
				<input type="text" name="SerchValue" onkeydown="GoSerch();" style="width:120px;" />
				<asp:Button ID="ExchangeRate" runat="server" Width="70" Height="25" PostBackUrl="~/Admin/ExchangeRate.aspx?G=V" UseSubmitBehavior="False" />
				<asp:Button ID="CustomerView" runat="server" Width="80" Height="25" PostBackUrl="~/Admin/CompanyList.aspx" UseSubmitBehavior="False" />--%>
            </td>
        </tr>
    </table>
</div>
<div style="background-color: #777777; height: 1px; font-size: 1px;"></div>
<div style="background-color: #BBBBBB; height: 1px; font-size: 1px;"></div>
<div style="background-color: #CCCCCC; height: 1px; font-size: 1px;"></div>
<div style="background-color: #DDDDDD; height: 1px; font-size: 1px;"></div>
