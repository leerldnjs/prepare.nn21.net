<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForCapture_HouseBL_YT.aspx.cs" Inherits="CustomClearance_ForCapture_HouseBL_YT" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
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
</head>
<body style="background-color: #E4E4E4; width: 900px; margin: 0 auto; padding-top: 10px; font-family: @Arial Unicode MS, Arial;">
    <form id="form1" runat="server">
        <div>
            <div class="ContentsTopMenu">              
                <input type="hidden" id="HRequestFormPk" value="<%=Request.Params["S"] %>" />                
                <input type="hidden" id="HGubun" value="<%=gubun %>" />

                <div id="PrintArea">
                    <div style="position: absolute; margin-top: -20px;">
                        <img src="http://nn21.net/UploadedFiles/3/EmptyBL_YT.jpg" style="width: 710px; height: 1050px;" /></div>
                    <div style="width: 694px; font-size: 13px; height: 1000px; background-color: White;">                        
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 40px; margin-left: 30px; width: 320px;">
                            <div><%=Shipper %></div>
                            <div><%=ShipperAddress %></div>
                        </div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 130px; margin-left: 30px; width: 320px;">
                            <div><%=Consignee %></div>
                            <div><%=ConsigneeAddress %></div>
                        </div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 215px; margin-left: 30px; width: 320px;">
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
                        <div style="position: absolute; line-height: 14px; font-size: 14px; margin-top: 35px; margin-left: 380px; width: 150px;"><%=BLNo %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 420px; margin-left: 50px; width: 240px;"><%=NM %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 420px; margin-left: 250px; width: 240px;"><%=ItemList %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 420px; margin-left: 170px; width: 60px; text-align: center;"><%=TotalQuantity %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 420px; margin-left: 500px; width: 80px; text-align: center;"><%=TotalGrossWeight %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 420px; margin-left: 600px; width: 80px; text-align: center;"><%=TotalVolume %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 645px; margin-left: 320px; width: 200px; text-align: center;"><%=FOBNCNF %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 630px; margin-left: 250px; width: 200px; text-align: left;">CFS/CFS</div>
                        <%--<div style=" position:absolute; line-height:14px; text-align:left; font-size:11px;  margin-top:630px; margin-left:380px; width:150px; ">SHIPPED ON BOARD</div>--%>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 355px; margin-left: 30px; width: 150px; text-align: left;"><%=FinalDestination %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 295px; margin-left: 30px; text-align: left;"><%=VoyageCompany %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 325px; margin-left: 30px; width: 150px; text-align: left;"><%=Carrier %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 325px; margin-left: 160px; width: 150px; text-align: left;"><%=VoyageNo %></div>
                        <div style=" position:absolute; line-height:14px; text-align:left; font-size:11px;  margin-top:600px; margin-left:30px; width:150px; "><%=ContainerNo %> / <%=SealNo %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 325px; margin-left: 230px; width: 150px; text-align: left;"><%=PortOfLanding %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 910px; margin-left: 280px; width: 220px; text-align: left;">DESTINATION</div>
                        <%--<div style=" position:absolute; line-height:14px; text-align:left; font-size:11px;  margin-top:875px; margin-left:250px; width:220px; ">THREE</div>--%>
                        <div style=" position:absolute; line-height:14px; text-align:left; font-size:11px;  margin-top:930px; margin-left:540px; width:220px; "><%=SailingOnOrAbout %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 690px; margin-left: 200px; width: 300px; text-align: left;"><%=LastTotalQuantity %></div>
                        <div style="position: absolute; line-height: 14px; font-size: 11px; margin-top: 905px; margin-left: 540px; width: 150px; text-align: left;"><%=PortOfLanding %></div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
