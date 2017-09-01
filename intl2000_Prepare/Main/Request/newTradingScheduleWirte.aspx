<%@ Page Language="C#" AutoEventWireup="true" CodeFile="newTradingScheduleWirte.aspx.cs" Inherits="Request_newTradingScheduleWirte" %>
<meta http-equiv="X-UA-Compatible" content="IE=10"> 

<%@ Register Src="../Member/LogedTopMenu.ascx" TagName="Loged" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>거래명세표</title>
    <link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <script src="../Common/public.js" type="text/javascript"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.1/themes/base/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.1/jquery-ui.js"></script>
    <style type="text/css">
        input
        {
            text-align: center;
        }

        .tdSubT
        {
            border-bottom: solid 1px #93A9B8;
            padding-top: 10px;
        }

        .td01
        {
            text-align: center;
            height: 25px;
            /*width: 120px;*/
            border-bottom: dotted 1px #E8E8E8;
        }

        .td02
        {
            background-color: #f5f5f5;
            text-align: center;
            height: 20px;
            width: 150px;
            border-bottom: dotted 1px #E8E8E8;
        }

        .td03
        {
            text-align: center;
            height: 25px;
            width: 190px;
            border-bottom: dotted 1px #E8E8E8;
        }

        .td04
        {
            background-color: #F5F5F5;
            text-align: center;
            border-bottom: dotted 1px #E8E8E8;
        }

        .Title
        {
            border: solid 1px black;
            width: 692px;
            height: 30px;
            font-size: 18px;
            font-weight: bold;
            text-align: center;
            letter-spacing: 12px;
            padding-top: 3px;
        }

        .left_top
        {
            border: solid 1px black;
            margin-top: -1px;
            padding: 5px;
        }

        .BL
        {
            border-left: solid 1px black;
        }

        .BB
        {
            border-bottom: solid 1px black;
        }

        .BR
        {
            border-right: solid 1px black;
        }

        #quick_menu input
        {
            width: 110px;
            height: 23px;
            margin: 5px;
        }

        div#quick_menu
        {
            position: absolute;
            left: 50%;
            margin-top: 100px;
            margin-left: 300px;
            border: 1px solid #999999;
            background: #eeeeee;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#TBDate").datepicker();
            $("#TBDate").datepicker("option", "dateFormat", "yymmdd");
            $("#TBDate").val(<%=TODAY %>)
            $("#Item\\[0\\]\\[0\\]\\[Date\\]").datepicker();
            $("#Item\\[0\\]\\[0\\]\\[Date\\]").datepicker("option", "dateFormat", "yymmdd");
            $("#Item\\[0\\]\\[0\\]\\[Date\\]").val(<%=TODAY %>)
        });

        window.onload = function () {
            var URI = location.href;
            var UriSpliteByQ = URI.split('?');
            var QueryString = "";
            var Mode = "JW";
            var urisplitbyand = UriSpliteByQ[1].split("&");
            for (var i = 0; i < urisplitbyand.length; i++) {
                if (urisplitbyand[i].substr(0, 1) == "M") {
                    if (urisplitbyand[i].toString() == "M=" + Mode) {
                        form1.HMode.value = Mode;
                        break;
                    }
                    else {
                        var Each = UriSpliteByQ[1].split("&");
                        var TradingScheduleWirteHeadPk = Each[0].substr(2, Each[0].length - 2);
                        Mode = Each[1].substr(2, 2);
                        form1.HMode.value = Mode;
                        form1.HTradingScheduleWirteHeadPk.value = TradingScheduleWirteHeadPk;
                        Request.LoadTradingScheduleDocument(TradingScheduleWirteHeadPk, LoadForOfferWriteSUCCESS, function (result) { window.alert(result); });
                    }
                }
            }

            var quick_menu = $('#quick_menu');
            var quick_top = 110;
            /* quick menu initialization */
            quick_menu.css('top', $(window).height());
            $(document).ready(function () {
                quick_menu.animate({ "top": $(document).scrollTop() + quick_top + "px" }, 500);
                $(window).scroll(function () {
                    quick_menu.stop();
                    quick_menu.animate({ "top": $(document).scrollTop() + quick_top + "px" }, 1000);
                });
            });
        }

        function LoadForOfferWriteSUCCESS(result) {
            if (result == "N") {
                alert(form1.HAlertReject.value); document.location.href = "../Default.aspx"; return false;
            }
            var Mode = form1.HMode.value;

            for (var i = 0; i < result.length; i++) {
                var Each = result[i].split("#@!");
                if (i == 0) {
                    form1.TBDate.value = Each[0];
                    form1.TBBusinessNumber.value = Each[2];
                    form1.TBName.value = Each[1];
                    form1.TBCompanyName.value = Each[3];
                    form1.TBPresidentName.value = Each[4];
                    form1.TBCompanyAddress.value = Each[5];
                    form1.TBUpjong.value = Each[6];
                    form1.TBUptae.value = Each[7];
                    form1.TotalAmount1.value = Each[12];
                    form1.TBTotalQuantity.value = Each[8];
                    form1.TBTotalPrice.value = Each[9];
                    form1.TBTotalAmount.value = Each[10];
                    form1.TBTotalTax.value = Each[11];
                    form1.TBMisuAmout.value = Each[13];
                    form1.TBTotalAmount2.value = Each[12];
                }
                else {
                    if (i > 1) { InsertRow(0); }
                    //document.getElementById("Item[0][" + (i - 1) + "][SUM]").value = result[i];
                    document.getElementById("Item[0][" + (i - 1) + "][TradingScheduleItemsPk]").value = Each[0];
                    document.getElementById("Item[0][" + (i - 1) + "][Date]").value = Each[1];
                    document.getElementById("Item[0][" + (i - 1) + "][Description]").value = Each[2];
                    document.getElementById("Item[0][" + (i - 1) + "][Volume]").value = Each[3];
                    document.getElementById("Item[0][" + (i - 1) + "][Quantity]").value = Each[4];
                    document.getElementById("Item[0][" + (i - 1) + "][Price]").value = Each[5];
                    document.getElementById("Item[0][" + (i - 1) + "][Amount]").value = Each[6];
                    document.getElementById("Item[0][" + (i - 1) + "][Tax]").value = Each[7];
                }
            }
            GetTotal("Quantity");
            GetTotal("PackedCount");
            GetTotal("Weight");
            GetTotal("Volume");
        }

        function NumberFormat(number) {
            if (number == "" || number == "0") { return number; }
            else { return parseInt(number * 10000) / 10000; }
        }

        function GetTotal(Which) {
            var RowLength = document.getElementById('ItemTable[0]').rows.length - 2;
            var MaxCountUnder1 = 0;
            for (i = 0; i < RowLength; i++) {
                var ThisValue = document.getElementById('Item[0][' + i + '][' + Which + ']').value.toString();
                if (ThisValue.indexOf(".") != -1 && ThisValue.length - ThisValue.indexOf(".") - 1 > MaxCountUnder1) {
                    MaxCountUnder1 = ThisValue.length - ThisValue.indexOf(".") - 1;
                }
            }

            var TotalValue = 0;
            var ToInt = Math.pow(10, MaxCountUnder1);
            for (i = 0; i < RowLength; i++) {
                var ThisValue = document.getElementById('Item[0][' + i + '][' + Which + ']').value.toString().replace(/,/g, '');
                if (ThisValue != "") {
                    TotalValue += parseInt(parseFloat(ThisValue) * ToInt);
                }
            }
            var FloatTotalValue = parseInt(TotalValue + 0.00000001) / ToInt;
            if (TotalValue != 0) { document.getElementById('TBTotal' + Which + '').value = FloatTotalValue; }
        }

        function QuantityXUnitCost(GroupNo, LineNo) {
            if (document.getElementById('Item[' + GroupNo + '][' + LineNo + '][Quantity]').value != "" && document.getElementById('Item[' + GroupNo + '][' + LineNo + '][UnitCost]').value != "") {
                Request.QuantityXUnitCostDocument(document.getElementById('Item[' + GroupNo + '][' + LineNo + '][Quantity]').value, document.getElementById('Item[' + GroupNo + '][' + LineNo + '][UnitCost]').value, function (result) {
                    document.getElementById('Item[' + GroupNo + '][' + LineNo + '][Price]').value = result;
                    GetTotal("Quantity");
                    GetTotal("Price");
                }, function (result) { alert("ERROR : " + result); });
            }
        }

        function InsertRow(rowC) {		// 상품추가

            var objTable = document.getElementById('ItemTable[' + rowC + ']');
            objTable.appendChild(document.createElement("TBODY"));
            var lastRow = objTable.rows.length;
            var thisLineNo = lastRow - 2;
            var objRow1 = objTable.insertRow(lastRow);
            var objCell1 = objRow1.insertCell();
            var objCell2 = objRow1.insertCell();
            var objCell3 = objRow1.insertCell();
            var objCell4 = objRow1.insertCell();
            var objCell5 = objRow1.insertCell();
            var objCell6 = objRow1.insertCell();
            var objCell7 = objRow1.insertCell();

            objCell1.align = "center"; objCell2.align = "center"; objCell3.align = "center"; objCell4.align = "center";
            objCell5.align = "center"; objCell6.align = "center"; objCell7.align = "center";

            objCell1.innerHTML = "<input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][TradingScheduleItemsPk]\" value=\"N\" /><input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Date]\" style=\"width:65px;\" />";
            objCell2.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Description]\" style=\"width:179px;\" />";
            objCell3.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Volume]\" style=\"width:65px;\" />";
            objCell4.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Quantity]\" onkeyup=\"GetTotal('Quantity');\" style=\"width:65px;\" />";
            objCell5.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Price]\" onkeyup=\"GetTotal('Price');\" style=\"width:65px;\" />";
            objCell6.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Amount]\" onkeyup=\"GetTotal('Amount');\" style=\"width:155px;\" />";
            objCell7.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Tax]\"  onkeyup=\"GetTotal('Tax');\" style=\"width:65px;\" />";

            $(function () {
                $("#Item\\[0\\]\\[" + thisLineNo + "\\]\\[Date\\]").datepicker();
                $("#Item\\[0\\]\\[" + thisLineNo + "\\]\\[Date\\]").datepicker("option", "dateFormat", "yymmdd");
                $("#Item\\[0\\]\\[" + thisLineNo + "\\]\\[Date\\]").val(<%=TODAY %>)
            });
        }
        /*	확인버튼 */
        function Btn_Submit_Click() {
            var TradingScheduleWirteHeadPk = form1.HTradingScheduleWirteHeadPk.value;
            var CompanyPk = form1.HCompanyPk.value;
            var AccountID = form1.HAccountID.value;
            var Date = form1.TBDate.value;
            var BusinessNumber = form1.TBBusinessNumber.value;
            var Name = form1.TBName.value;
            var CompanyName = form1.TBCompanyName.value;
            var PresidentName = form1.TBPresidentName.value;
            var CompanyAddress = form1.TBCompanyAddress.value;
            var Upjong = form1.TBUpjong.value;
            var Uptae = form1.TBUptae.value;
            var TotalQuantity = form1.TBTotalQuantity.value;
            var TotalPrice = form1.TBTotalPrice.value;
            var TotalAmount = form1.TBTotalAmount.value;
            var TotalTax = form1.TBTotalTax.value;
            var MisuAmout = form1.TBMisuAmout.value;
            var TotalAmount2 = form1.TBTotalAmount2.value;
            //var GubunCL = 2;

            var RowTemp1;
            var RowTemp2;
            var RowTemp3;
            var RowTemp4;
            var RowTemp5;
            var RowTemp6;
            var RowTemp7;
            var RowTemp8;

            var itemInfoGroup = "";
            var RowLength = document.getElementById('ItemTable[0]').rows.length - 2;

            for (var i = 0; i < RowLength; i++) {

                RowTemp1 = document.getElementById("Item[0][" + i + "][TradingScheduleItemsPk]").value + "!";
                RowTemp2 = document.getElementById("Item[0][" + i + "][Date]").value + "!";
                RowTemp3 = document.getElementById("Item[0][" + i + "][Description]").value + "!";
                RowTemp4 = document.getElementById("Item[0][" + i + "][Volume]").value + "!";
                RowTemp5 = document.getElementById("Item[0][" + i + "][Quantity]").value + "!";
                RowTemp6 = document.getElementById("Item[0][" + i + "][Price]").value + "!";
                RowTemp7 = document.getElementById("Item[0][" + i + "][Amount]").value + "!";
                RowTemp8 = document.getElementById("Item[0][" + i + "][Tax]").value + "!";

                itemInfoGroup += RowTemp1 + RowTemp2 + RowTemp3 + RowTemp4 + RowTemp5 + RowTemp6 + RowTemp7 + RowTemp8 + "####";
            }

            if (itemInfoGroup == "") {
                alert('명세를 입력하세요')
                return;
            }

            Request.SaveTradingScheduleDocument(TradingScheduleWirteHeadPk, CompanyPk, AccountID, Date, BusinessNumber, Name, CompanyName, PresidentName, CompanyAddress, Upjong, Uptae, TotalQuantity, TotalPrice, TotalAmount, TotalTax, MisuAmout, TotalAmount2, itemInfoGroup, function (result) {
                parent.location.href = "../Admin/CompanyInfoDocumentList.aspx";
            }, ONFAILED);
        }
        function ONFAILED(result) { window.alert(result); }


    </script>
</head>
<body class="MemberBody">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="WebService" runat="server">
            <Services>
                <asp:ServiceReference Path="~/WebService/Request.asmx" />
            </Services>
        </asp:ScriptManager>
        <uc1:Loged ID="Loged1" runat="server" />
        <div class="ContentsTopMenu">
            <div id="quick_menu">
                <p>
                    <input type="button" id="BTN_Submit" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "wjwkd") %>" onclick="Btn_Submit_Click();" /><br />
                    <input type="button" id="BTN_Cancel" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "cnlth") %>" onclick="history.back();" />
                </p>
            </div>
            <div style="width: 694px; background-color: White;">
                <div style="padding: 0px 0px 0px 5px">
            <strong>
                <p>
                    <a href="newCommercialInvoiceWrite.aspx?&M=JW"><strong>COMMERCIAL INVOICE</strong></a>
                    &nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href="newPackingListWrite.aspx?&M=JW">PACKING LIST </a>
                    &nbsp;&nbsp;&nbsp;||&nbsp;&nbsp;&nbsp;<a href="newTradingScheduleWirte.aspx?&M=JW"><%=GetGlobalResourceObject("qjsdur", "rjfoaudtpvy") %></a>
                </p>
            </strong>
        </div>
                <div style="padding: 10px; width: 250px;  text-align: center; margin: 0 auto; border: 2px solid black; margin-top: 20px; margin-bottom: 20px; font-size: 17px; letter-spacing: 3px; font-weight: bold;"><%=GetGlobalResourceObject("qjsdur", "rjfoaudtpvy") %></div>
                <table cellpadding="0" cellspacing="0" style="width: 694px; border: solid 1px black;">
                    <tr>
                        <td class="td03">
                            <input id="TBDate" type="text" readonly="readonly" style="width: 100px;" /></td>
                        <td class="td02" style="font-weight: bold;"><%=GetGlobalResourceObject("Member", "SaupjaNo")%></td>
                        <td class="td01" colspan="3">
                            <input type="text" id="TBBusinessNumber" type="text" readonly="readonly" value="<%=CompanyNo %>" style="width: 405px; text-align: left;" /></td>
                    </tr>
                    <tr>
                        <td class="td03">
                            <input type="text" id="TBName" type="text" style="width: 78px;" />
                            <%=GetGlobalResourceObject("RequestForm", "rnlgk") %></td>
                        <td class="td02" style="font-weight: bold;"><%=GetGlobalResourceObject("Member", "CompanyName")%></td>
                        <td class="td01">
                            <input type="text" id="TBCompanyName" type="text" readonly="readonly" value="<%=CompanyName %>" style="width: 130px; text-align: left;" /></td>
                        <td class="td02" style="font-weight: bold;"><%=GetGlobalResourceObject("Member", "Name")%></td>
                        <td class="td01">
                            <input type="text" id="TBPresidentName" type="text" readonly="readonly" value="<%=PresidentName %>" style="width: 130px; text-align: left;" /></td>
                    </tr>
                    <tr>
                        <td class="td03" rowspan="2"><%=GetGlobalResourceObject("qjsdur", "rPtksgkqslek")%></td>
                        <td class="td02" style="font-weight: bold;"><%=GetGlobalResourceObject("Member", "CompanyAddress")%></td>
                        <td class="td01" colspan="3">
                            <input type="text" id="TBCompanyAddress" type="text" readonly="readonly" value="<%=CompanyAddress %>" style="width: 410px; text-align: left;" /></td>
                    </tr>
                    <tr>
                        <td class="td02" style="font-weight: bold;"><%=GetGlobalResourceObject("Member", "Upjong")%></td>
                        <td class="td01">
                            <input type="text" id="TBUpjong" type="text" value="<%=upjong %>" readonly="readonly" style="width: 130px; text-align: left;" /></td>
                        <td class="td02" style="font-weight: bold;"><%=GetGlobalResourceObject("Member", "Uptae")%></td>
                        <td class="td01">
                            <input type="text" id="TBUptae" type="text" value="<%=uptae %>" readonly="readonly" style="width: 130px; text-align: left;" /></td>
                    </tr>
                    <tr>
                        <td class="td02" colspan="2" style="font-weight: bold;"><%=GetGlobalResourceObject("RequestForm", "Amount")%></td>
                        <td class="td03" colspan="3">
                            <input type="text" id="TotalAmount1" type="text" style="width: 390px;" />
                            </td>
                    </tr>
                </table>
                <table id="ItemTable[0]" cellpadding="0" cellspacing="0" style="width: 694px; border-left: solid 1px black; border-right: solid 1px black;">
                    <tr style="height: 25px;">
                        <td colspan="7" align="right">
                            <input type="button" value="+" onclick="InsertRow('0');" /></td>
                    </tr>
                    <thead>
                        <tr style="height: 25px;">
                            <td class="td04" style="width: 70px;"><%=GetGlobalResourceObject("RequestForm", "skfWk") %></td>
                            <td class="td04" style="width: 184px;"><%=GetGlobalResourceObject("RequestForm", "Description") %></td>
                            <td class="td04" style="width: 70px;"><%=GetGlobalResourceObject("RequestForm", "rbrur") %></td>
                            <td class="td04" style="width: 70px;"><%=GetGlobalResourceObject("RequestForm", "Count") %></td>
                            <td class="td04" style="width: 70px;"><%=GetGlobalResourceObject("RequestForm", "UnitCost") %></td>
                            <td class="td04" style="width: 160px;"><%=GetGlobalResourceObject("RequestForm", "rhdrmqrkdor") %></td>
                            <td class="td04" style="width: 70px;"><%=GetGlobalResourceObject("RequestForm", "tpdor") %></td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td align="center">
                                <input type="hidden" id="Item[0][0][TradingScheduleItemsPk]" value="N" />
                                <input type="text" id="Item[0][0][Date]" style="width: 65px;" /></td>
                            <td align="center">
                                <input type="text" id="Item[0][0][Description]" style="width: 179px;" /></td>
                            <td align="center">
                                <input type="text" id="Item[0][0][Volume]" style="width: 65px;" /></td>
                            <td align="center">
                                <input type="text" id="Item[0][0][Quantity]" style="width: 65px;" onkeyup="GetTotal('Quantity');" /></td>
                            <td align="center">
                                <input type="text" id="Item[0][0][Price]" style="width: 65px;" onkeyup="GetTotal('Price');" /></td>
                            <td align="center">
                                <input type="text" id="Item[0][0][Amount]" style="width: 155px;" onkeyup="GetTotal('Amount');" /></td>
                            <td align="center">
                                <input type="text" id="Item[0][0][Tax]" style="width: 65px;" onkeyup="GetTotal('Tax');" /></td>
                        </tr>
                    </tbody>
                </table>
                <table cellpadding="0" cellspacing="0" style="width: 694px; border: solid 1px black;">
                    <tr>
                        <td align="center" class="td04" colspan="3" style="width: 309px; font-weight: bold;"><%=GetGlobalResourceObject("RequestForm", "Total") %></td>
                        <td align="center">
                            <input type="text" id="TBTotalQuantity" style="width: 65px;" /></td>
                        <td align="center">
                            <input type="text" id="TBTotalPrice" style="width: 65px;" /></td>
                        <td align="center">
                            <input type="text" id="TBTotalAmount" style="width: 155px;" /></td>
                        <td align="center">
                            <input type="text" id="TBTotalTax" style="width: 65px;" /></td>
                    </tr>
                    <tr>
                        <td align="center" class="td04" style="width: 65px; font-weight: bold;"><%=GetGlobalResourceObject("RequestForm", "wjswksrma") %></td>
                        <td align="center" colspan="2" style="width: 244px;">
                            <input type="text" id="TBMisuAmout" style="width: 250px;" /></td>
                        <td align="center" class="td04" style="font-weight: bold;"><%=GetGlobalResourceObject("RequestForm", "Amount")%></td>
                        <td align="center" colspan="3">
                            <input type="text" id="TBTotalAmount2" style="width: 293px;" /></td>
                    </tr>
                </table>
            </div>
            <input type="hidden" id="HCompanyPk" value="<%=MemberInfo[1] %>" />
            <input type="hidden" id="HAccountID" value="<%=MemberInfo[2] %>" />
            <input type="hidden" id="HMode" />
            <input type="hidden" id="HTradingScheduleWirteHeadPk" value="0" />
            <input type="hidden" id="HStepCL" />
            <input type="hidden" id="HSum" />           
            <input type="hidden" id="HGubun" value="<%=Request.Params["G"] %>" />
            <input type="hidden" id="ShipperClearanceNamePk" />
            <input type="hidden" id="ConsigneeClearanceNamePk" />
        </div>
    </form>
</body>
</html>
