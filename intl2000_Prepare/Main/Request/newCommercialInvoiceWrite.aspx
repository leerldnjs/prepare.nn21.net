<%@ Page Language="C#" AutoEventWireup="true" CodeFile="newCommercialInvoiceWrite.aspx.cs" Inherits="Request_newCommercialInvoiceWrite" %>
<meta http-equiv="X-UA-Compatible" content="IE=10"> 
<%@ Register Src="../Member/LogedTopMenu.ascx" TagName="Loged" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Commercial Invoice Write</title>
    <%--<script src="../Common/jquery-1.4.2.min.js" type="text/javascript"></script>--%>
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
            background-color: #f5f5f5;
            text-align: center;
            height: 20px;
            width: 130px;
            border-bottom: dotted 1px #E8E8E8;
            padding-top: 4px;
            padding-bottom: 4px;
        }

        .td02
        {
            width: 330px;
            border-bottom: dotted 1px #E8E8E8;
            padding-top: 4px;
            padding-bottom: 4px;
            padding-left: 10px;
        }

        .td023
        {
            padding-top: 4px;
            padding-bottom: 4px;
            padding-left: 10px;
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

        .Shipper
        {
            border: solid 1px black;
            height: 90px;
            margin-top: -1px;
            padding: 5px;
        }

        .PortOfLoading
        {
            border: solid 1px black;
            height: 51px;
            margin-top: -1px;
            padding: 5px;
        }

        .FinalDestination
        {
            border: solid 1px black;
            height: 51px;
            margin-top: -1px;
            margin-left: -1px;
            padding: 5px;
        }

        .InvoiceNo
        {
            border: solid 1px black;
            margin-left: -1px;
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
            $("#TBSailingOn").datepicker();
            $("#TBSailingOn").datepicker("option", "dateFormat", "yymmdd");
            $("#TBSailingOn").val(<%=TODAY %>)
        });
        window.onload = function () {
            SetMonetaryUnit("$");
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
                        var CommercialDocumentHeadPk = Each[0].substr(2, Each[0].length - 2);
                        Mode = Each[1].substr(2, 2);
                        form1.HMode.value = Mode;
                        form1.HCommercialDocumentHeadPk.value = CommercialDocumentHeadPk;
                        Request.LoadInvoiceDocument(CommercialDocumentHeadPk, "C", LoadForOfferWriteSUCCESS, function (result) { window.alert(result); });
                    }
                }
            }
        }

        function LoadForOfferWriteSUCCESS(result) {
            if (result[0] == "N") {
                alert(form1.HAlertReject.value); document.location.href = "../Default.aspx"; return false;
            }
            var Mode = form1.HMode.value;
            var MonetaryUnit;
            for (var i = 0; i < result.length; i++) {
                var Each = result[i].split("#@!");
                if (i == 0) {
                    MonetaryUnit = Each[14];
                    //form1.HStepCL.value = Each[6];
                    form1.TBSailingOn.value = Each[10];
                    if (Each[7] != "") {
                        form1.TBPortOfLanding.value = Each[7];
                    }
                    if (Each[8] != "") {
                        form1.FinialDestination.value = Each[8];
                    }
                    //var tempstring = Each[9].split("@@@@");
                    form1.TBBuyer.value = Each[13];
                    form1.TBOtherReferences.value = Each[12];
                    form1.TBCarrier.value = Each[9];
                    form1.TBNortifyPartyName.value = Each[5];
                    form1.TBNotifyPartyAddress.value = Each[6];
                    form1.TBInvoiceNo.value = Each[0];
                    form1.PaymentWayCL.value = Each[11];

                    form1.TBShipperName.value = Each[1];
                    form1.TBShipperAddress.value = Each[2];
                    form1.TBConsigneeName.value = Each[3];
                    form1.TBConsigneeAddress.value = Each[4];
                }
                else {
                    /*	0	RFI.DocumentFormItemsPk,	1	RFI.ItemCode,	2	RFI.Description,		3	RFI.Label,	4	RFI.Material,	5	RFI.Quantity,		6	RFI.QuantityUnit,	7	RFI.UnitPrice,	8	RFI.Amount	*/
                    if (i > 1) { InsertRow(0); }
                    document.getElementById("Item[0][" + (i - 1) + "][SUM]").value = result[i];
                    document.getElementById("Item[0][" + (i - 1) + "][DocumentFormItemsPk]").value = Each[0];
                    document.getElementById("Item[0][" + (i - 1) + "][Brand]").value = Each[2];
                    document.getElementById("Item[0][" + (i - 1) + "][ItemName]").value = Each[1];
                    document.getElementById("Item[0][" + (i - 1) + "][Matarial]").value = Each[3];
                    document.getElementById("Item[0][" + (i - 1) + "][Quantity]").value = NumberFormat(Each[4]);
                    document.getElementById("Item[0][" + (i - 1) + "][UnitCost]").value = NumberFormat(Each[6]);
                    document.getElementById("Item[0][" + (i - 1) + "][Price]").value = NumberFormat(Each[7]);
                    switch (Each[5]) {
                        case "40": Each[5] = '0'; break;
                        case "41": Each[5] = '1'; break;
                        case "42": Each[5] = '2'; break;
                        case "43": Each[5] = '3'; break;
                        case "44": Each[5] = '4'; break;
                        case "45": Each[5] = '5'; break;
                        case "46": Each[5] = '6'; break;
                        case "47": Each[5] = '7'; break;
                        case "48": Each[5] = '8'; break;
                    }
                    document.getElementById("Item[0][" + (i - 1) + "][QuantityUnit]").selectedIndex = Each[5];
                }
            }
            switch (MonetaryUnit) {
                case '18': SetMonetaryUnit("￥"); break;
                case '19': SetMonetaryUnit("$"); break;
                case '20': SetMonetaryUnit("￦"); break;
                case '21': SetMonetaryUnit("Y"); break;
                case '22': SetMonetaryUnit("$"); break;
                case '23': SetMonetaryUnit("€"); break;
            }
            GetTotal("Quantity");
            GetTotal("Price");
        }

        function NumberFormat(number) {
            if (number == "" || number == "0") { return number; }
            else { return parseInt(number * 10000) / 10000; }
        }

        //합계구하기
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
            if (TotalValue != 0) { document.getElementById('total[0][' + Which + ']').value = FloatTotalValue; }
        }

        function SelectChangeMonetaryUnit(idx) {
            switch (document.getElementById('Item[0][MonetaryUnit]')[idx].value) {
                case '18': SetMonetaryUnit("￥"); break;
                case '19': SetMonetaryUnit("$"); break;
                case '20': SetMonetaryUnit("￦"); break;
                case '21': SetMonetaryUnit("Y"); break;
                case '22': SetMonetaryUnit("$"); break;
                case '23': SetMonetaryUnit("€"); break;
            }
        }
        function SetMonetaryUnit(value) {
            var i;
            var RowLength = document.getElementById('ItemTable[0]').rows.length - 2;
            var totalCount = 0;
            for (i = 0; i < RowLength; i++) {
                document.getElementById('Item[0][' + i + '][MonetaryUnit][0]').value = value;
                document.getElementById('Item[0][' + i + '][MonetaryUnit][1]').value = value;
            }
            document.getElementById('Item[0][MonetaryUnit][Total]').value = value;
        }

        //수화인 선택 / 등록
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

            var MonetaryUnit;
            switch (document.getElementById('Item[0][MonetaryUnit]')[document.getElementById("Item[0][MonetaryUnit]").selectedIndex].value) {
                case '18': MonetaryUnit = "￥"; break;
                case '19': MonetaryUnit = "$"; break;
                case '20': MonetaryUnit = "￦"; break;
                case '21': MonetaryUnit = "Y"; break;
                case '22': MonetaryUnit = "$"; break;
                case '23': MonetaryUnit = "€"; break;
                default: MonetaryUnit = ""; break;
            }
            objCell1.align = "center";
            objCell2.align = "center";
            objCell3.align = "center";
            objCell4.align = "center";
            objCell5.align = "center";
            objCell6.align = "center";

            objCell1.innerHTML = "<input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][ItemCode]\" /><input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][DocumentFormItemsPk]\" value=\"N\" /><input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][SUM]\" /><input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][EachVolume]\" /><input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][EachX]\" /><input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][EachY]\" /><input type=\"hidden\" id=\"Item[" + rowC + "][" + thisLineNo + "][EachZ]\" /><input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Brand]\" style=\"width:95px;\" />";
            objCell2.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][ItemName]\" style=\"width:135px;\" />";
            objCell3.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Matarial]\" style=\"width:115px;\" />";
            objCell4.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Quantity]\" onkeyup=\"QuantityXUnitCost(0," + thisLineNo + "); GetTotal('Quantity'); GetTotal('Price');\" size=\"5\" /> <select id=\"Item[" + rowC + "][" + thisLineNo + "][QuantityUnit]\"><option value=\"40\">PCS</option><option value=\"41\">PRS</option><option value=\"42\">SET</option><option value=\"43\">S/F</option><option value=\"44\">YDS</option><option value=\"45\">M</option><option value=\"46\">KG</option><option value=\"47\">DZ</option><option value=\"48\">L</option><option value=\"49\">BOX</option><option value=\"50\">SQM</option><option value=\"51\">M2</option><option value=\"52\">RO</option></select>";
            objCell5.innerHTML = "<input type=\"text\" style=\"border:0; width:10px; \" id=\"Item[" + rowC + "][" + thisLineNo + "][MonetaryUnit][0]\" value=\"" + MonetaryUnit + "\" /><input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][UnitCost]\" onkeyup=\"QuantityXUnitCost(0," + thisLineNo + "); GetTotal('Quantity'); GetTotal('Price');\" style=\"width:55px;\" />";
            objCell6.innerHTML = "<input type=\"text\" style=\"border:0; width:10px; \" id=\"Item[" + rowC + "][" + thisLineNo + "][MonetaryUnit][1]\" value=\"" + MonetaryUnit + "\"/><input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Price]\" readonly=\"readonly\"  style=\"width:70px;\" />";
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

        function SelectWho(SorC) {
            if (SorC == "S") {
                window.open('./Dialog/RelatedCompanyList.aspx?C=S&S=' + form1.HCompanyPk.value, "", 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=500px,height=500px');
            }
            else if (SorC == "C") {
                window.open('./Dialog/RelatedCompanyList.aspx?C=C&S=' + form1.HCompanyPk.value, "", 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=500px,height=500px');
            }
            else {
                window.open('./Dialog/RelatedCompanyList.aspx?C=N&S=' + form1.HCompanyPk.value, "", 'toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=500px,height=500px');
            }
        }
        function SelectWhoResult(SorC, TargetCompanyPk, TargetCompanyNick, CompanyCode, RegionCode, BranchPk, CompanyAddress) {
            if (SorC == "S") {
                form1.ShipperPk.value = TargetCompanyPk;
                document.getElementById("TBShipperName").value = TargetCompanyNick;
                document.getElementById("TBShipperAddress").value = CompanyAddress;
            }
            else if (SorC == "C") {
                form1.ShipperPk.value = TargetCompanyPk;
                document.getElementById("TBConsigneeName").value = TargetCompanyNick;
                document.getElementById("TBConsigneeAddress").value = CompanyAddress;
            }
            else {
                form1.ConsigneePk.value = TargetCompanyPk;
                document.getElementById("TBNortifyPartyName").value = TargetCompanyNick;
                document.getElementById("TBNotifyPartyAddress").value = CompanyAddress;
            }
        }
        function AddNewCCL(which) {
            var DialogResult = form1.HCompanyPk.value + '!' + form1.HAccountID.value;
            var retVal = window.showModalDialog('./Dialog/OwnCustomerAdd.aspx', DialogResult, 'dialogWidth=900px;dialogHeight=650px;resizable=0;status=0;scroll=1;help=0;');
            try {
                var ReturnArray = retVal.split('##');
                document.getElementById('TB_ImportCompanyName').value = ReturnArray[0] + ' (' + ReturnArray[1] + ')';
                document.getElementById('TB_CustomerListPk').value = ReturnArray[2];
            }
            catch (err) { return false; }
        }
        function SelectCompanyInDocument(SorC) {
            var CompanyPk;
            if (SorC == "S") {
                if (form1.ShipperPk.value == "") {
                    alert("먼저 발화인 선택을 해주세요");
                    return false;
                }
                CompanyPk = form1.ShipperPk.value;
            }
            else {
                if (form1.ConsigneePk.value == "") {
                    alert("먼저 수하인 선택을 해주세요");
                    return false;
                }
                CompanyPk = form1.ConsigneePk.value;
            }
            var DialogResult = new Array();
            DialogResult[0] = form1.HGubun.value;
            DialogResult[1] = SorC;
            DialogResult[2] = "3157";

            var retVal = window.showModalDialog('../Request/Dialog/ShipperNameSelection.aspx?S=' + CompanyPk, DialogResult, 'dialogWidth=800px;dialogHeight=400px;resizable=0;status=0;scroll=1;help=0;');
            try {
                if (retVal == 'D') { SelectCompanyInDocument(SorC); }
                else if (retVal == "1") {
                    if (SorC == "S") {
                        form1.ShipperClearanceNamePk.value = "0";
                        document.getElementById("TBShipperName").value = form1.H_ClearanceSubstitute.value;
                    } else {
                        form1.ConsigneeClearanceNamePk.value = "0";
                        document.getElementById("TBConsigneeName").value = form1.H_ClearanceSubstitute.value;
                    }
                } else if (retVal == "2") {
                    if (SorC == "S") {
                        form1.ShipperClearanceNamePk.value = "";
                        document.getElementById("TBShipperName").value = "";
                    } else {
                        form1.ConsigneeClearanceNamePk.value = "";
                        document.getElementById("TBConsigneeName").value = "";
                    }
                } else {
                    var ReturnArray = retVal.split('##');
                    if (SorC == "S") {
                        document.getElementById("ShipperClearanceNamePk").value = ReturnArray[0];
                        document.getElementById("TBShipperName").value = ReturnArray[1];
                        document.getElementById("TBShipperAddress").value = ReturnArray[2];
                    } else {
                        document.getElementById("ConsigneeClearanceNamePk").value = ReturnArray[0];
                        document.getElementById("TBConsigneeName").value = ReturnArray[1];
                        document.getElementById("TBConsigneeAddress").value = ReturnArray[2];
                    }
                }
            }
            catch (err) { return false; }
        }
        /*	확인버튼 */
        function Btn_Submit_Click() {
            var CommercialDocumentHeadPk = form1.HCommercialDocumentHeadPk.value;
            var MonetaryUnit = document.getElementById("Item[0][MonetaryUnit]").value;
            var StepCL = "";

            var SessionPk = form1.HCompanyPk.value;
            var accountID = form1.HAccountID.value;
            var SailingOn = form1.TBSailingOn.value;
            var PortOfLanding = form1.TBPortOfLanding.value;
            var FilnalDestination = form1.FinialDestination.value;
            var NotifyParty = form1.TBNortifyPartyName.value;
            var PaymentWay = form1.PaymentWayCL.value;
            var GubunCL = 1;
            //var PaymentWay = 3;
            //if (document.form1.PaymentWayCL[1].checked) { PaymentWay = 4; }

            var RowTemp1;
            var RowTemp2;
            var RowTemp3;
            var itemInfoGroup = "";
            var RowLength = document.getElementById('ItemTable[0]').rows.length - 2;
            for (var i = 0; i < RowLength; i++) {
                RowTemp1 = document.getElementById("Item[0][" + i + "][DocumentFormItemsPk]").value + "!" + document.getElementById("Item[0][" + i + "][ItemCode]").value + "!" + document.getElementById("Item[0][" + i + "][ItemName]").value + "!" + document.getElementById("Item[0][" + i + "][Brand]").value + "!" + document.getElementById("Item[0][" + i + "][Matarial]").value + "!" + document.getElementById("Item[0][" + i + "][Quantity]").value + "!";
                RowTemp2 = document.getElementById("Item[0][" + i + "][QuantityUnit]").value + "!";
                RowTemp3 = document.getElementById("Item[0][" + i + "][UnitCost]").value + "!" + document.getElementById("Item[0][" + i + "][Price]").value;
                if (RowTemp1 + RowTemp2 + RowTemp3 != document.getElementById("Item[0][" + i + "][SUM]").value && RowTemp1 + RowTemp3 != "N!!!!!!!") {
                    itemInfoGroup += RowTemp1 + RowTemp2 + RowTemp3 + "####";
                }
            }
            if (itemInfoGroup == "") {
                alert('명세를 입력하세요')
                return;
            }
            Request.SaveForOfferWriteDocument("C", SessionPk, CommercialDocumentHeadPk, MonetaryUnit, StepCL, accountID, SailingOn, PortOfLanding, FilnalDestination, PaymentWay, itemInfoGroup, NotifyParty, form1.TBNotifyPartyAddress.value, form1.TBInvoiceNo.value, form1.TBBuyer.value, form1.TBOtherReferences.value, form1.TBCarrier.value, form1.TBShipperName.value, form1.TBShipperAddress.value, form1.TBConsigneeName.value, form1.TBConsigneeAddress.value, GubunCL, function (result) {
                parent.location.href = "../Admin/CompanyInfoDocumentList.aspx"
            }, function (result) { window.alert(result); });
        }
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
            <script type="text/javascript">
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
            </script>
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
                <div class="Title">COMMERCIAL INVOICE</div>
                <div style="width: 346px; float: left;">
                    <div id="Shipper" class="Shipper">
                        <strong>Shipper</strong>
                        <div style="float: right;">
                            <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "tjsxor") %>" onclick="SelectWho('S');" />
                            <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "emdfhr") %>" style="color: Red;" onclick="AddNewCCL('S');" />
                            <%--<input type="button" value="통관명" onclick="SelectCompanyInDocument('S');" />--%>
                        </div>
                        <div style="padding-left: 25px; padding-top: 10px; line-height: 20px; font-size: 12px; letter-spacing: -1px;">
                            <div id="DivShipperName">
                                <input type="text" id="TBShipperName" style="font-weight: bold; text-align: left; width: 288px;" />
                            </div>
                            <div id="DivShipperAddress">
                                <textarea rows="2" cols="48" style="overflow: hidden;" id="TBShipperAddress"></textarea>
                            </div>
                        </div>
                    </div>
                    <div id="Consignee" class="Shipper">
                        <strong>Consignee</strong>
                        <div style="float: right;">
                            <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "tjsxor") %>" onclick="SelectWho('C');" />
                            <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "emdfhr") %>" style="color: Red;" onclick="AddNewCCL('C');" />
                            <%--<input type="button" value="통관명" onclick="SelectCompanyInDocument('C');" />--%>
                        </div>
                        <div style="padding-left: 25px; padding-top: 10px; line-height: 20px; font-size: 12px; letter-spacing: -1px;">
                            <div id="DivConsigneeName" style="font-weight: bold;">
                                <input type="text" style="font-weight: bold; text-align: left; width: 288px;" id="TBConsigneeName" />
                            </div>
                            <div id="DivConsigneeAddress">
                                <textarea rows="2" cols="48" style="overflow: hidden;" id="TBConsigneeAddress"></textarea>
                            </div>
                        </div>
                    </div>
                    <div id="NotifyParty" class="Shipper">
                        <strong>Notify Party</strong>
                        <div style="float: right;">
                            <input type="button" value="<%=GetGlobalResourceObject("qjsdur", "tjsxor") %>" onclick="SelectWho('N');" />
                            <%--<input type="button" value="<%=GetGlobalResourceObject("qjsdur", "emdfhr") %>" style="color: Red;" onclick="AddNewCCL('N');" />--%>
                            <%--<input type="button" value="통관명" onclick="SelectCompanyInDocument('C');" />--%>
                        </div>
                        <div style="padding-left: 25px; padding-top: 10px; line-height: 20px; font-size: 12px; letter-spacing: -1px;">
                            <div id="DivNotifyPartyName" style="font-weight: bold;">
                                <input type="text" id="TBNortifyPartyName" style="text-align: left; width: 288px; font-weight: bold;" onclick="this.select();" value="Same as Above" size="20" />
                            </div>
                            <div id="DivNotifyPartyAddress">
                                <textarea rows="2" cols="48" style="overflow: hidden;" id="TBNotifyPartyAddress"></textarea>
                            </div>
                        </div>
                    </div>
                    <div style="width: 173px; height: 123px; float: left;">
                        <div class="PortOfLoading" id="PortOfLanding">
                            <strong>Port of loading</strong>
                            <div id="DivPortOfLanding" style="padding-left: 10px; padding-top: 3px;">
                                <input id="TBPortOfLanding" type="text" style="width: 135px; text-align: left;" />
                            </div>
                        </div>
                        <div class="PortOfLoading" id="Carrier">
                            <strong>Carrier</strong>
                            <div id="DivCarrier" style="padding: 10px;">
                                <input id="TBCarrier" type="text" style="width: 135px; text-align: left;" />
                            </div>
                        </div>
                    </div>
                    <div style="width: 173px; height: 123px; float: right;">
                        <div class="FinalDestination" id="FinalDestination">
                            <strong>Final destination</strong>
                            <div id="DivFinialDestination" style="padding-left: 10px; padding-top: 3px;">
                                <input id="FinialDestination" type="text" style="width: 135px; text-align: left;" />
                            </div>
                        </div>
                        <div class="FinalDestination" id="SailingOnOrAbout">
                            <strong>Sailing on or About</strong>
                            <div id="DivSailingOnOrAbout" style="padding: 10px;">
                                <input id="TBSailingOn" type="text" readonly="readonly" style="width: 100px;" />
                            </div>
                        </div>
                    </div>
                </div>
                <div style="width: 347px; float: right;">
                    <div class="InvoiceNo" id="InvoiceNo" style="height: 57px; margin-left: -2px; margin-top: -1px;">
                        <strong>Date & No of Invoice</strong>
                        <div style="padding-left: 25px; padding-top: 15px;">
                            <input type="text" id="TBInvoiceNo" style="overflow: hidden; width: 287px; text-align: left;" />
                        </div>
                    </div>
                    <div class="InvoiceNo" id="PaymentTerms" style="height: 57px; margin-left: -2px; margin-top: -1px;">
                        <strong>Payment terms</strong>
                        <div style="padding-left: 25px; padding-top: 15px;">
                            <input type="text" id="PaymentWayCL" style="font-weight: bold; text-align: left; width: 288px;" value="T / T" onclick="this.select();" />
                        </div>
                    </div>
                    <div class="InvoiceNo" id="Buyer" style="height: 55px; margin-left: -2px; margin-top: -1px;">
                        <strong>Buyer</strong>
                        <div style="padding-top: 5px; padding-left: 25px;">
                            <textarea id="TBBuyer" cols="48" rows="2" style="overflow: hidden;"></textarea>
                        </div>
                    </div>
                    <div class="InvoiceNo" id="OtherReferences" style="margin-left: -2px; height: 214px; margin-top: -1px;">
                        <strong>Other References</strong>
                        <div style="padding-left: 25px; padding-top: 15px;">
                            <textarea id="TBOtherReferences" cols="48" rows="12" style="overflow: hidden; line-height: 20px;"></textarea>
                        </div>
                    </div>
                </div>
                <div style="width: 694px; clear: both;" id="ItemTable">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 694px;">
                        <thead>
                            <tr style="height: 40px;">
                                <td class="BB BL" style="width: 120px; padding-left: 10px; font-weight: bold;">Marks &
                                    <br />
                                    Number of PKGS</td>
                                <td class="BB BL" style="padding: 5px; text-align: center; font-weight: bold;">Description of Goods</td>
                                <td class="BB BL" style="width: 100px; text-align: center; font-weight: bold;">Quantity</td>
                                <td class="BB BL" style="width: 95px; text-align: center; font-weight: bold;">Unit Price</td>
                                <td class="BB BL BR" style="width: 150px; text-align: center; font-weight: bold;">Amount</td>
                            </tr>
                            <tr style="height: 30px;">
                                <td class="BL">&nbsp;</td>
                                <td class="BL">&nbsp;</td>
                                <td class="BL">&nbsp;</td>
                                <td class="BL" style="text-align: center;">
                                    <select id="Item[0][MonetaryUnit]" size="1" onchange="SelectChangeMonetaryUnit(this.selectedIndex)">
                                        <option value="19"><%=GetGlobalResourceObject("RequestForm", "MonetaryUnitChange") %></option>
                                        <option value="18">RMB ￥</option>
                                        <option value="19">USD $</option>
                                        <option value="20">KRW ￦</option>
                                        <option value="21">JPY Y</option>
                                        <option value="22">HKD HK$</option>
                                        <option value="23">EUR €</option>
                                    </select>
                                </td>
                                <td class="BL BR" style="text-align: center;">
                                    <input type="button" onclick="InsertRow('0');" value="<%=GetGlobalResourceObject("RequestForm", "InsertItem") %>" /></td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="BL BR" colspan="5">
                                    <table id="ItemTable[0]" style="background-color: White; width: 683px;" border="1" cellpadding="0" cellspacing="0">
                                        <thead>
                                            <tr>
                                                <td class="tdSubT" colspan="6"></td>
                                            </tr>
                                            <tr style="height: 30px;">
                                                <td bgcolor="#F5F5F5" align="center" width="110"><%=GetGlobalResourceObject("RequestForm", "Label") %></td>
                                                <td bgcolor="#F5F5F5" align="center"><%=GetGlobalResourceObject("RequestForm", "Description") %></td>
                                                <td bgcolor="#F5F5F5" align="center" width="130"><%=GetGlobalResourceObject("RequestForm", "Material") %></td>
                                                <td bgcolor="#F5F5F5" align="center" width="115"><%=GetGlobalResourceObject("RequestForm", "Count") %></td>
                                                <td bgcolor="#F5F5F5" align="center" width="80"><%=GetGlobalResourceObject("RequestForm", "UnitCost") %></td>
                                                <td bgcolor="#F5F5F5" align="center" width="90"><%=GetGlobalResourceObject("RequestForm", "Amount") %></td>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td align="center">
                                                    <input type="hidden" id="Item[0][0][EachVolume]" />
                                                    <input type="hidden" id="Item[0][0][EachX]" />
                                                    <input type="hidden" id="Item[0][0][EachY]" />
                                                    <input type="hidden" id="Item[0][0][EachZ]" />
                                                    <input type="hidden" id="Item[0][0][ItemCode]" />
                                                    <input type="hidden" id="Item[0][0][DocumentFormItemsPk]" value="N" />
                                                    <input type="hidden" id="Item[0][0][SUM]" />
                                                    <input type="text" id="Item[0][0][Brand]" style="width: 95px;" />
                                                </td>
                                                <td align="center">
                                                    <input type="text" id="Item[0][0][ItemName]" style="width: 135px;" /></td>
                                                <td align="center">
                                                    <input type="text" id="Item[0][0][Matarial]" style="width: 115px;" /></td>
                                                <td align="center">
                                                    <input type="text" id="Item[0][0][Quantity]" onkeyup="QuantityXUnitCost(0,0); GetTotal('Quantity'); GetTotal('Price');" size="5" />
                                                    <select id="Item[0][0][QuantityUnit]">
                                                        <option value="40">PCS</option>
                                                        <option value="41">PRS</option>
                                                        <option value="42">SET</option>
                                                        <option value="43">S/F</option>
                                                        <option value="44">YDS</option>
                                                        <option value="45">M</option>
                                                        <option value="46">KG</option>
                                                        <option value="47">DZ</option>
                                                        <option value="48">L</option>
                                                        <option value='49'>Box</option>
                                                        <option value='50'>SQM</option>
														<option value='51'>M2</option>
														<option value='52'>RO</option>
                                                    </select>
                                                </td>
                                                <td align="center">
                                                    <input type="text" style="border: 0; width: 10px;" id="Item[0][0][MonetaryUnit][0]" /><input type="text" id="Item[0][0][UnitCost]" onkeyup="QuantityXUnitCost(0,0); GetTotal('Quantity'); GetTotal('Price');" style="width: 55px;" />
                                                </td>
                                                <td align="center">
                                                    <input type="text" style="border: 0; width: 10px;" id="Item[0][0][MonetaryUnit][1]" /><input type="text" id="Item[0][0][Price]" readonly="readonly" style="width: 70px;" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <table border="0" style="background-color: White; width: 683px;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td bgcolor="#F5F5F5" height="30" align="right" style="width: 403px;">&nbsp;&nbsp;&nbsp;<%=GetGlobalResourceObject("RequestForm", "Total") %>&nbsp;&nbsp;&nbsp;</td>
                                            <td bgcolor="#F5F5F5" align="left">
                                                <input type="text" id="total[0][Quantity]" style="width: 50px;" />
                                                <span style="padding-left: 120px;">
                                                    <input type="text" style="border: 0; background-color: #F5F5F5; width: 10px;" id="Item[0][MonetaryUnit][Total]" /><input type="text" id="total[0][Price]" style="width: 80px;" />
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="height: 40px;">
                                <td class="BL BB">&nbsp;</td>
                                <td class="BB" colspan="2" style="text-align: right; padding-right: 10px; font-weight: bold;">&nbsp;</td>
                                <td class="BR BB" colspan="2" style="text-align: right; padding-right: 30px; font-weight: bold;">&nbsp;</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 694px; margin-top: -1px;">
                    <tr style="height: 150px;">
                        <td class="BL BB" style="text-align: center; font-weight: bold;">&nbsp;</td>
                        <td class="BR BB" style="width: 350px; text-align: center; font-weight: bold;">&nbsp;</td>
                    </tr>
                </table>
            </div>
            <input type="hidden" id="HCompanyPk" value="<%=MemberInfo[1] %>" />
            <input type="hidden" id="HCompanyCode" value="<%=SubInfo[0] %>" />
            <input type="hidden" id="HAccountID" value="<%=MemberInfo[2] %>" />
            <input type="hidden" id="HAlertReject" value="<%=GetGlobalResourceObject("Alert", "Rejection") %>" />
            <input type="hidden" id="HMode" />
            <input type="hidden" id="HCommercialDocumentHeadPk" value="0" />
            <input type="hidden" id="HStepCL" />
            <input type="hidden" id="ShipperPk" />
            <input type="hidden" id="ConsigneePk" />
            <input type="hidden" id="HGubun" value="<%=Request.Params["G"] %>" />
            <input type="hidden" id="ShipperClearanceNamePk" />
            <input type="hidden" id="ConsigneeClearanceNamePk" />
        </div>
    </form>
</body>
</html>
