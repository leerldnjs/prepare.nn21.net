<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Loged.ascx.cs" Inherits="Logistics_Loged" %>
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
    <table border="0" cellpadding="0" cellspacing="0" style="width: 1050px;">
        <tr>
            <td colspan="2" style="text-align: center; color: WindowText; font-size: 18px;"><span style="font-size: 18px; font-weight: bold; color: #7F0000;">
                <%=MEMBERINFO[2] %></span>&nbsp;&nbsp;&nbsp;&nbsp;I LOGISTICS NETWORK SYSTEM</td>
            <td>
                <asp:Button ID="LogOut" runat="server" OnClick="Button1_Click" TabIndex="99" UseSubmitBehavior="False" />
            </td>
        </tr>
        <tr>
            <td colspan="1" style="color: White; font-weight: bold; font-size: 18px; text-align: left; width: 200px; height: 35px; vertical-align: middle;"></td>
            <td colspan="2">&nbsp;				
                <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "qothdwnsql") %>" style="width: 70px; height: 30px; font-size: 12px;" onclick="location.href = '../Logistics/StorageOut_Logistics.aspx';" />
                <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "cnfrhgusghkd") %>" onclick="location.href = '../Logistics/TransportBetweenCompanyList_Logistics.aspx';" style="width: 70px; height: 30px; font-size: 12px;" />
            </td>

        </tr>
    </table>
</div>
<div style="background-color: #777777; height: 1px; font-size: 1px;"></div>
<div style="background-color: #BBBBBB; height: 1px; font-size: 1px;"></div>
<div style="background-color: #CCCCCC; height: 1px; font-size: 1px;"></div>
<div style="background-color: #DDDDDD; height: 1px; font-size: 1px;"></div>
