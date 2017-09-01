<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommercialDocu_HouseBL.aspx.cs" Inherits="CustomClearance_CommercialDocu_HouseBL" Debug="true" %>

<%@ Register Src="../Admin/LogedWithoutRecentRequest.ascx" TagName="LogedWithoutRecentRequest" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=BLNo %></title>
    <script src="http://nn21.net/Common/jquery-1.4.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        .Title {
            border: solid 1px black;
            width: 692px;
            height: 30px;
            font-size: 18px;
            text-align: center;
            letter-spacing: 5px;
            padding-top: 5px;
        }

        .Shipper {
            border: solid 1px black;
            height: 85px;
            margin-top: -1px;
            padding: 5px;
            font-size: 13px;
        }

        .PortOfLoading {
            border: solid 1px black;
            height: 41px;
            margin-top: -1px;
            padding: 5px;
            font-size: 13px;
        }

        .FinalDestination {
            border: solid 1px black;
            height: 41px;
            margin-top: -1px;
            margin-left: -1px;
            padding: 5px;
            font-size: 13px;
        }

        .InvoiceNo {
            border: solid 1px black;
            margin-left: -1px;
            padding: 5px;
        }

        .BL {
            border-left: solid 1px black;
        }

        .BB {
            border-bottom: solid 1px black;
        }

        .BR {
            border-right: solid 1px black;
        }

        .BT {
            border-top: solid 1px black;
        }

        .InputButton {
            width: 110px;
            height: 23px;
            margin: 5px;
        }

        div#quick_menu {
            position: absolute;
            left: 50%;
            margin-top: 100px;
            margin-left: 300px;
            border: 1px solid #999999;
            background: #eeeeee;
        }
    </style>
    <script type="text/javascript">
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
    </script>
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 10px; font-family: @Arial Unicode MS, Arial;">
    <form id="form1" runat="server">
        <div>
            <div class="ContentsTopMenu">
                <div id="quick_menu">
                    <p>
                        <input type="button" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "dlstho") %>" onclick="RunPrint()" /><br />
                        <input type="button" class="InputButton" value="Select Printer" onclick="SelectPrinter()" /><br />
                        <%--<input type="button" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "tnwjd") %>" onclick="Modify()" /><br />--%>
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

                <input type="hidden" id="HRequestFormPk" value="<%=Request.Params["S"] %>" />
                <input type="hidden" id="HAccountID" value="<%=MemberInfo[2] %>" />
                <input type="hidden" id="HCompanyPk" value="<%=MemberInfo[1] %>" />
                <input type="hidden" id="HGubun" value="<%=gubun %>" />

                <div id="PrintArea">
                    <div style="position: absolute; margin-top: -20px;">
                        <img src="http://nn21.net/UploadedFiles/3/EmptyBL.jpg" style="width: 710px; height: 1050px;" />
                    </div>
                    <div style="width: 694px; font-size: 13px; height: 1000px; background-color: White;">
                        <object id="IEPageSetupX" name="IEPageSetupX" classid="clsid:41C5BC45-1BE8-42C5-AD9F-495D6C8D7586" codebase="~/Common/IEPageSetupX.cab" width="0" height="0">
                            <param name="copyright" value="http://isulnara.com" />
                        </object>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 30px; margin-left: 30px; width: 320px;">
                            <div><%=Shipper %></div>
                            <div><%=ShipperAddress %></div>
                        </div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 120px; margin-left: 30px; width: 320px;">
                            <div><%=Consignee %></div>
                            <div><%=ConsigneeAddress %></div>
                        </div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 210px; margin-left: 30px; width: 320px;">
                            <div><%=NotifyParty %></div>
                            <div><%=NotifyPartyAddress %></div>
                        </div>
                        <% if (gubun == "Stamp")
                           { %>
                        <div style="position: absolute; width: 100px; height: 100px; margin-left: 500px; margin-top: 430px;">
                            <img src="/Images/Check_Back.png" style="width: 75px; height: 225px;" />
                        </div>
                        <div style="position: absolute; width: 100px; height: 100px; margin-left: 500px; margin-top: 580px;">
                            <img src="/Images/Check_Stamp.png" style="width: 73px; height: 74px;" />
                        </div>
                        <% } %>
                        <div style="position: absolute; line-height: 14px; font-size: 14px; margin-top: 30px; margin-left: 500px; width: 150px;"><%=BLNo %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 410px; margin-left: 50px; width: 240px;"><%=NM %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 410px; margin-left: 250px; width: 240px;"><%=ItemList.ToString().ToUpper() %></div>
                        <div style="position: absolute; line-height: 14px; text-align: center; font-size: 11px; margin-top: 410px; margin-left: 180px; width: 60px;"><%=TotalQuantity %></div>
                        <div style="position: absolute; line-height: 14px; text-align: center; font-size: 11px; margin-top: 410px; margin-left: 510px; width: 80px;"><%=TotalGrossWeight %></div>
                        <div style="position: absolute; line-height: 14px; text-align: center; font-size: 11px; margin-top: 410px; margin-left: 600px; width: 80px;"><%=TotalVolume %></div>
                        <div style="position: absolute; line-height: 14px; text-align: center; font-size: 11px; margin-top: 670px; margin-left: 320px; width: 200px;"><%=FOBNCNF %></div>
                        <div style="position: absolute; line-height: 14px; text-align: left; font-size: 11px; margin-top: 570px; margin-left: 510px; width: 200px;"><%=LCNo %></div>
                        <div style="position: absolute; line-height: 14px; text-align: left; font-size: 11px; margin-top: 640px; margin-left: 250px; width: 200px;"><%=CFSNCY %></div>
                        <div style="position: absolute; line-height: 14px; text-align: left; font-size: 11px; margin-top: 640px; margin-left: 510px; width: 150px;">SHIPPED ON BOARD</div>
                        <div style="position: absolute; line-height: 14px; text-align: left; font-size: 11px; margin-top: 360px; margin-left: 30px; width: 150px;"><%=FinalDestination %></div>
                        <div style="position: absolute; line-height: 14px; text-align: left; font-size: 11px; margin-top: 300px; margin-left: 30px;"><%=VoyageCompany %></div>
                        <div style="position: absolute; line-height: 14px; text-align: left; font-size: 11px; margin-top: 330px; margin-left: 30px; width: 150px;"><%=Carrier %></div>
                        <div style="position: absolute; line-height: 14px; text-align: left; font-size: 11px; margin-top: 330px; margin-left: 160px; width: 150px;"><%=VoyageNo %></div>
                        <div style="position: absolute; line-height: 14px; text-align: left; font-size: 11px; margin-top: 600px; margin-left: 30px; width: 150px;"><%=ContainerNo %> / <%=SealNo %></div>
                        <div style="position: absolute; line-height: 14px; text-align: left; font-size: 11px; margin-top: 330px; margin-left: 250px; width: 150px;"><%=PortOfLanding %></div>
                        <div style="position: absolute; line-height: 14px; text-align: left; font-size: 11px; margin-top: 875px; margin-left: 50px; width: 220px;">DESTINATION</div>
                        <div style="position: absolute; line-height: 14px; text-align: left; font-size: 11px; margin-top: 875px; margin-left: 250px; width: 220px;">THREE</div>
                        <div style="position: absolute; line-height: 14px; text-align: left; font-size: 11px; margin-top: 875px; margin-left: 450px; width: 220px;"><%=SailingOnOrAbout %></div>
                        <div style="position: absolute; line-height: 14px; text-align: left; font-size: 11px; margin-top: 700px; margin-left: 200px; width: 300px;"><%=LastTotalQuantity %></div>
                        <% if (Request.Params["YT"] + "" == "YT")
                           { %>
                        <div style="position: absolute; line-height: 14px; text-align: left; font-size: 11px; margin-top: 660px; margin-left: 530px; width: 220px;"><%=SailingOnOrAbout %></div>
                        <% } %>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
