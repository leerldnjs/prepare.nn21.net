<%@ Page Language="C#" AutoEventWireup="true" CodeFile="newCommercialDocu_TradingSchedule.aspx.cs" Inherits="CustomClearance_newCommercialDocu_TradingSchedule" Debug="true" %>
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
            width: 120px;
            border-bottom: dotted 1px #E8E8E8;
        }

        .td02
        {
            background-color: #f5f5f5;
            text-align: center;
            height: 20px;
            width: 80px;
            border-bottom: dotted 1px #E8E8E8;
        }

        .td03
        {
            text-align: center;
            height: 25px;
            width: 175px;
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
            $("#Item\\[0\\]\\[0\\]Date").datepicker();
            $("#Item\\[0\\]\\[0\\]Date").datepicker("option", "dateFormat", "yymmdd");
            $("#Item\\[0\\]\\[0\\]Date").val(<%=TODAY %>)
            $("#Item\\[0\\]\\[1\\]Date").datepicker();
            $("#Item\\[0\\]\\[1\\]Date").datepicker("option", "dateFormat", "yymmdd");
            $("#Item\\[0\\]\\[1\\]Date").val(<%=TODAY %>)
        });
        var initBody;
        window.onload = function () {
            if (form1.HGubun.value == "Print") {
                RunPrint();
                history.back();
            }
        }
        function RunPrint() {
            initBody = document.body.innerHTML;
            document.body.innerHTML = PrintArea.innerHTML;
            IEPageSetupX.header = ""; // 헤더설정
            IEPageSetupX.footer = ""; // 푸터설정
            IEPageSetupX.Orientation = 1; // 가로 출력은 0 세로출력은 1
            IEPageSetupX.leftMargin = 13;
            IEPageSetupX.rightMargin = 0;
            IEPageSetupX.topMargin = 10;
            IEPageSetupX.bottomMargin = 0;
            IEPageSetupX.PrintBackground = false; // 배경색 및 이미지 인쇄
            //IEPageSetupX..align = center;

            IEPageSetupX.Print(false); // 인쇄하기
            // self.close();
            // PrintTest(); // 컨트롤설치여부 테스트
            // IEPageSetupX.RollBack(); // 수정 이전 값으로 되돌림(한 단계 이전만 지원)
            // IEPageSetupX.Clear(); // 여백은 0으로, 머리글/바닥글은 모두 제거, 배경색 및 이미지 인쇄 안함
            // IEPageSetupX.Print(true); // 인쇄대화상자 띄우기
            document.body.innerHTML = initBody;
        }
        function SelectPrinter() {
            initBody = document.body.innerHTML;
            document.body.innerHTML = PrintArea.innerHTML;
            IEPageSetupX.header = ""; // 헤더설정
            IEPageSetupX.footer = ""; // 푸터설정
            IEPageSetupX.Orientation = 1; // 가로 출력은 0 세로출력은 1
            IEPageSetupX.leftMargin = 13;
            IEPageSetupX.rightMargin = 0;
            IEPageSetupX.topMargin = 10;
            IEPageSetupX.bottomMargin = 0;
            IEPageSetupX.PrintBackground = false; // 배경색 및 이미지 인쇄
            //IEPageSetupX..align = center;

            IEPageSetupX.Print(true); // 인쇄하기
            // self.close();
            // PrintTest(); // 컨트롤설치여부 테스트
            // IEPageSetupX.RollBack(); // 수정 이전 값으로 되돌림(한 단계 이전만 지원)
            // IEPageSetupX.Clear(); // 여백은 0으로, 머리글/바닥글은 모두 제거, 배경색 및 이미지 인쇄 안함
            // IEPageSetupX.Print(true); // 인쇄대화상자 띄우기
            document.body.innerHTML = initBody;
        }

        function NumberFormat(number) {
            if (number == "" || number == "0") { return number; }
            else { return parseInt(number * 10000) / 10000; }
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

            objCell1.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Date]\" style=\"width:65px;\" />";
            objCell2.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Description]\" style=\"width:179px;\" />";
            objCell3.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Volume]\" style=\"width:65px;\" />";
            objCell4.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Quantity]\" onkeyup=\"GetTotal('Quantity');\" style=\"width:65px;\" />";
            objCell5.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Price]\" onkeyup=\"GetTotal('Price');\" style=\"width:65px;\" />";
            objCell6.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Amount]\" onkeyup=\"GetTotal('Amount');\" style=\"width:155px;\" />";
            objCell7.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Tax]\"  onkeyup=\"GetTotal('Tax');\" style=\"width:65px;\" />";
            //objCell7.innerHTML = "<input type=\"text\" id=\"Item[" + rowC + "][" + thisLineNo + "][Tax]\" onkeyup=\"GetTotal('Weight');\" style=\"width:50px;\" />";


        }
        /*	확인버튼 */
        function Btn_Submit_Click() {
            var CommercialDocumentHeadPk = form1.HCommercialDocumentHeadPk.value;
            //var MonetaryUnit = "";
            var SessionPk = form1.HCompanyPk.value;
            var accountID = form1.HAccountID.value;
            var PortOfLanding = form1.TBPortOfLanding.value;
            var FilnalDestination = form1.FinialDestination.value;
            var NotifyParty = form1.TBNortifyPartyName.value;
            var PaymentWay = form1.PaymentWayCL.value;
            var GubunCL = 2;
            //var PaymentWay = 3;
            //if (document.form1.PaymentWayCL[1].checked) { PaymentWay = 4; }

            var RowTemp1;
            var RowTemp2;
            var RowTemp3;
            var RowTemp4;
            var RowTemp5;
            var itemInfoGroup = "";
            var RowLength = document.getElementById('ItemTable[0]').rows.length - 1;

            for (var i = 0; i < RowLength; i++) {

                RowTemp1 = document.getElementById("Item[0][" + i + "][DocumentFormItemsPk]").value + "!" + document.getElementById("Item[0][" + i + "][ItemCode]").value + "!" + document.getElementById("Item[0][" + i + "][BoxNo]").value + "!" + document.getElementById("Item[0][" + i + "][ItemName]").value + "!" + document.getElementById("Item[0][" + i + "][Brand]").value + "!" + document.getElementById("Item[0][" + i + "][Matarial]").value + "!" + document.getElementById("Item[0][" + i + "][Quantity]").value + "!";
                RowTemp2 = document.getElementById("Item[0][" + i + "][QuantityUnit]").value + "!";
                RowTemp3 = document.getElementById("Item[0][" + i + "][PackedCount]").value + "!";
                RowTemp4 = document.getElementById("Item[0][" + i + "][PackingUnit]").value + "!";
                RowTemp5 = document.getElementById("Item[0][" + i + "][Weight]").value + "!" + document.getElementById("Item[0][" + i + "][Volume]").value;
                if (RowTemp1 + RowTemp2 + RowTemp3 + "!" + RowTemp5 != document.getElementById("Item[0][" + i + "][SUM]").value && RowTemp1 + RowTemp3 + RowTemp5 != "N!!!!!!!!!") {
                    //alert(RowTemp5);
                    itemInfoGroup += RowTemp1 + RowTemp2 + RowTemp3 + RowTemp4 + RowTemp5 + "####";
                }
            }
            if (itemInfoGroup == "") {
                alert('명세를 입력하세요')
                return;
            }
            Request.SaveForOfferWriteDocument("P", SessionPk, CommercialDocumentHeadPk, MonetaryUnit, StepCL, accountID, SailingOn, PortOfLanding, FilnalDestination, PaymentWay, itemInfoGroup, NotifyParty, form1.TBNotifyPartyAddress.value, form1.TBInvoiceNo.value, form1.TBBuyer.value, form1.TBOtherReferences.value, form1.TBCarrier.value, form1.TBShipperName.value, form1.TBShipperAddress.value, form1.TBConsigneeName.value, form1.TBConsigneeAddress.value, GubunCL, function (result) {
                parent.location.href = "../Admin/CompanyInfoDocumentList.aspx";
            }, ONFAILED);
        }
        function ONFAILED(result) { window.alert(result); }

        function GoModify(Gubun, Value) {
            switch (Gubun) {
                case "Modify": location.href = "../Request/newTradingScheduleWirte.aspx?S=" + Value + "&M=IW"; break;
            }
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
					<input type="button" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "dlstho") %>" onclick="RunPrint()" /><br />
					<input type="button" class="InputButton" value="Select Printer" onclick="SelectPrinter()" /><br />
                    <%--<input type="button" class="InputButton" value="Modify" onclick="GoModify('Modify', HRequestFormPk.value)" /><br />--%>
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
            <div id="PrintArea" >
            <div style="width: 694px; background-color: White;">
                <object id="IEPageSetupX" name="IEPageSetupX" classid="clsid:41C5BC45-1BE8-42C5-AD9F-495D6C8D7586" codebase="~/Common/IEPageSetupX.cab" width="0" height="0" >
						<param name="copyright" value="http://isulnara.com" />
					</object>
                <div style="padding: 10px; width: 250px;  text-align: center; margin: 0 auto; border: 2px solid black; margin-top: 20px; margin-bottom: 20px; font-size: 17px; letter-spacing: 3px; font-weight: bold;"><%=GetGlobalResourceObject("qjsdur", "rjfoaudtpvy") %></div>
                <table cellpadding="0" cellspacing="0" style="width: 694px; border: solid 1px black;">
                    <tr>
                        <td class="td03" style=" border-right:solid 1px black; border-bottom:solid 1px black;">
                            <%=Date %></td>
                        <td class="td02" style="font-weight: bold; border-right:solid 1px black; border-bottom:solid 1px black;"><%=GetGlobalResourceObject("Member", "SaupjaNo")%></td>
                        <td class="td01" colspan="3" style=" border-bottom:solid 1px black;">
                            <%=CompanyNo %>
                    </tr>
                    <tr>
                        <td class="td03" style=" border-right:solid 1px black; border-bottom:solid 1px black;">
                            <%=Name %>
                            <%=GetGlobalResourceObject("RequestForm", "rnlgk") %></td>
                        <td class="td02" style="font-weight: bold; border-right:solid 1px black; border-bottom:solid 1px black;"><%=GetGlobalResourceObject("Member", "CompanyName")%></td>
                        <td class="td01" style=" border-right:solid 1px black; border-bottom:solid 1px black;">
                            <%=CompanyName%></td>
                        <td class="td02" style="font-weight: bold;border-right:solid 1px black; border-bottom:solid 1px black;"><%=GetGlobalResourceObject("Member", "Name")%></td>
                        <td class="td01" style="border-bottom:solid 1px black;">
                            <%=PresidentName%></td>
                    </tr>
                    <tr>
                        <td class="td03" rowspan="2" style="border-right:solid 1px black; border-bottom:solid 1px black;"><%=GetGlobalResourceObject("qjsdur", "rPtksgkqslek")%></td>
                        <td class="td02" style="font-weight: bold; border-right:solid 1px black; border-bottom:solid 1px black;"><%=GetGlobalResourceObject("Member", "CompanyAddress")%></td>
                        <td class="td01" colspan="3" style="border-bottom:solid 1px black;">
                            <%=CompanyAddress%></td>
                    </tr>
                    <tr>
                        <td class="td02" style="font-weight: bold; border-right:solid 1px black; border-bottom:solid 1px black;"><%=GetGlobalResourceObject("Member", "Upjong")%></td>
                        <td class="td01"  style="border-right:solid 1px black; border-bottom:solid 1px black;">
                            <%=upjong%></td>
                        <td class="td02" style="font-weight: bold; border-right:solid 1px black; border-bottom:solid 1px black;"><%=GetGlobalResourceObject("Member", "Uptae")%></td>
                        <td class="td01" style="border-bottom:solid 1px black;">
                            <%=uptae%></td>
                    </tr>
                    <tr>
                        <td class="td02" colspan="2" style="font-weight: bold; border-right:solid 1px black; "><%=GetGlobalResourceObject("RequestForm", "Amount")%></td>
                        <td class="td03" colspan="3">
                            <%=TotalAmount1 %>
                            </td>
                    </tr>
                </table>
                <%=Item %>
                    <table  cellpadding="0" cellspacing="0" style="width: 694px; border: solid 1px black;">
                    <tr style="height: 25px;">
                        <td align="center" class="td04" style="width:329px; font-weight: bold;"><%=GetGlobalResourceObject("RequestForm", "Total") %></td>
                        <td align="center" style="width:70px; border-left:solid 1px black; border-right:solid 1px black; " ><%=TotalQuantity%></td>
                        <td align="center" style="width:70px; border-right:solid 1px black; "><%=TotalPrice%></td>
                        <td align="center"  style="width:160px; border-right:solid 1px black; "><%=TotalAmount%></td>
                        <td align="center" ><%=TotalTax%></td>
                    </tr>
                     </table>
                <table  cellpadding="0" cellspacing="0" style="width: 694px; ">
                    <tr style="height: 25px;">
                        <td align="center" class="td04" style="width:160px; font-weight: bold; border-bottom:solid 1px black;border-left:solid 1px black; border-right:solid 1px black; "><%=GetGlobalResourceObject("RequestForm", "wjswksrma") %></td>
                        <td align="center" style="border-bottom:solid 1px black;border-right:solid 1px black;">
                            <%=MisuAmout%></td>
                        <td align="center" class="td04" style="width:160px; font-weight: bold;border-bottom:solid 1px black;border-right:solid 1px black;"><%=GetGlobalResourceObject("RequestForm", "Amount")%></td>
                        <td align="center" style="border-bottom:solid 1px black;border-right:solid 1px black;">
                            <%=TotalAmount1%></td>
                    </tr>
                    <%--<tr>--%>
                        <%--					<td align="center" class="td04" style="font-weight: bold;">입금</td>
					<td align="center"><input type="text" id="Text23" style="width:179px;" /></td>
					<td align="center" class="td04" style="font-weight: bold;">잔금</td>
					<td align="center" ><input type="text" id="Text25" style="width:65px;"/></td>
					<td align="center" class="td04"style="font-weight: bold;">인수자</td>
					<td align="center" colspan="2"><input type="text" id="Text27" style="width:230px;"/></td>
				</tr>--%>
                </table>
            </div>
                </div>
            <input type="hidden" id="HCompanyPk" value="<%=MemberInfo[1] %>" />
            <input type="hidden" id="HAccountID" value="<%=MemberInfo[2] %>" />
            <input type="hidden" id="HMode" />
            <input type="hidden" id="HCommercialDocumentHeadPk" value="0" />
            <input type="hidden" id="HStepCL" />
            <input type="hidden" id="HRequestFormPk" value="<%=Request.Params["S"] %>" />
            <input type="hidden" id="HGubun" value="<%=Request.Params["G"] %>" />
        </div>
    </form>
</body>
</html>
