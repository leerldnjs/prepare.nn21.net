<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeliveryCharge.aspx.cs" Inherits="Admin_Dialog_DeliveryCharge" Debug="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var ACCOUNTID;
        window.onload = function () {

        }

        function NumberFormat(number) { if (number == "" || number == "0") { return number; } else { return parseInt(number * 10000) / 10000; } }

        function BTN_Submit_Click() {
            var CalculateBodySum = "";
            ACCOUNTID = dialogArguments.substr(14);
            if (form1.TBDeleveryChargeShipper.value != "")
            {
                CalculateBodySum += "200!DELIVERY CHARGE!" + form1.TBDeleveryChargeShipper.value + "!" + form1.st_DeliveryChargeMonetaryShipper.value + "!D!" + form1.HDeliveryChargeShipperPk + "@@";
            }
            if (form1.TBDeleveryChargeConsignee.value != "")
            {
                CalculateBodySum += "300!DELIVERY CHARGE!" + form1.TBDeleveryChargeConsignee.value + "!" + form1.st_DeliveryChargeMonetaryConsignee.value + "!D!" + form1.HDeliveryChargeConsigneePk + "@@";
            }

            Admin.ModifyDeliveryCharge(form1.HRequestFormPk.value, CalculateBodySum, ACCOUNTID, function (result) {
                if (result == "1") { alert(form1.dhksfy.value); window.returnValue = true; returnValue = "Y"; self.close(); }
                else { alert(result); }
            }, function (result) { alert(result); });
        }

    </script>
</head>
<body style="background-color: #999999;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
        <div style="width:350px; margin: 10px; padding-left: 10px; padding-right: 10px; padding-top:10px; background-color: white;">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 350px;">
                <tr>
                <td>Delivery Charge</td>
                    </tr>
                <tr>
                    <td>
                        <input type="hidden" id="HDeliveryChargeShipperPk" />
                        <select id="st_DeliveryChargeMonetaryShipper" style="width: 70px;" disabled="disabled">
                            <option value="18">RMB ￥</option>
                            <option value="19">USD $</option>
                            <option value="20">KRW ￦</option>
                            <option value="21">JPY Y</option>
                            <option value="22">HKD HK$</option>
                            <option value="23">EUR €</option>
                        </select>
                        <input type="text" id="TBDeleveryChargeShipper" value="" style="width: 80px;" /></td>
                    <td>
                        <input type="hidden" id="HDeliveryChargeConsigneePk" />
                        <select id="st_DeliveryChargeMonetaryConsignee" style="width: 70px;" disabled="disabled">
                            <option value="20">KRW ￦</option>
                            <option value="18">RMB ￥</option>
                            <option value="19">USD $</option>
                            <option value="21">JPY Y</option>
                            <option value="22">HKD HK$</option>
                            <option value="23">EUR €</option>
                        </select>
                        <input type="text" id="TBDeleveryChargeConsignee" value="" style="width: 80px;" />
                    </td>
                </tr>
            </table>
            <div style="padding: 20px;">
                <input type="button" id="BTN_Submit" value="Submit" onclick="BTN_Submit_Click();" style="width: 300px; height: 30px;" /></div>
        </div>
        <input type="hidden" id="HRequestFormPk" value="<%=Request.Params["S"] %>" />
        <input type="hidden" id="dhksfy" value="<%=GetGlobalResourceObject("qjsdur", "dhksfy") %>" />
        <input type="hidden" id="DEBUG" onfocus="this.select();" />
    </form>
</body>
</html>
