<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommercialDocu_DeliveryOrder.aspx.cs" Inherits="CustomClearance_CommercialDocu_DeliveryOrder" %>
<%@ Register src="../Admin/LogedWithoutRecentRequest.ascx" tagname="LogedWithoutRecentRequest" tagprefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DeliveryOrder</title>
	<script src="../Common/jquery-1.4.2.min.js" type="text/javascript"></script>
	
	<style type="text/css">
	    .body, table, th, td, p, div, span, a { font-size:9pt; color:#000000;  font-family: "Arial";}
	    .Shipper{border:solid 1px black; height:85px; margin-top:-1px; padding:5px; font-size:13px;  }
		.PortOfLoading{border:solid 1px black; height:41px; margin-top:-1px; padding:5px; font-size:13px;  }
		.FinalDestination{border:solid 1px black; height:41px; margin-top:-1px; margin-left:-1px;  padding:5px; font-size:13px;  }
		.InvoiceNo{border:solid 1px black; margin-left:-1px; padding:5px; }
		.BL{border-left:solid 1px black; }
		.BB{border-bottom:solid 1px black;}
		.BR{border-right:solid 1px black;}
		.BT{ border-top:solid 1px black;}
		.InputButton{width:110px; height:23px; margin:5px; }
		div#quick_menu {position:absolute; left:50%; margin-top:100px; margin-left:300px; border:1px solid #999999; background:#eeeeee;}
        .style1
        {
            height: 35px;
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
	        IEPageSetupX.leftMargin = 5;
	        IEPageSetupX.rightMargin = 0;
	        IEPageSetupX.topMargin = 10;
	        IEPageSetupX.bottomMargin = 0;
	        IEPageSetupX.PrintBackground = true; // 배경색 및 이미지 인쇄
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
	        IEPageSetupX.leftMargin = 5;
	        IEPageSetupX.rightMargin = 0;
	        IEPageSetupX.topMargin = 10;
	        IEPageSetupX.bottomMargin = 0;
	        IEPageSetupX.PrintBackground = true; // 배경색 및 이미지 인쇄
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


<body style="background-color:#FFFFFF; margin:0; padding-top:10px;padding-bottom:10px;" >
<form id="form1" runat="server">
<div class="ContentsTopMenu">
    <div id="quick_menu">
	    <p>
	    <input type="button" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "dlstho") %>" onclick="RunPrint()" /><br />
	    <input type="button" class="InputButton" value="Select Printer" onclick="SelectPrinter()" /><br />
        </p>
    </div>

    		<script type="text/javascript">
    		    var quick_menu = $('#quick_menu');
    		    var quick_top = 150;
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
            <input type="hidden" id="HBBHPk" value="<%=Request.Params["B"] %>" />

    <div id="PrintArea" >
        <object id="IEPageSetupX" name="IEPageSetupX" classid="clsid:41C5BC45-1BE8-42C5-AD9F-495D6C8D7586" codebase="~/Common/IEPageSetupX.cab" width="0" height="0" >
		    <param name="copyright" value="http://isulnara.com" />
		</object>

    <table border="0"  cellpadding="0" cellspacing="0" bordercolor="#000000" style="border-collapse:collapse;width:700px;background-color:#FFFFFF;"  align="center">
	<tr>
		<td  height="100"  valign="top" >
		<table id=""  width="700" border="0"  cellpadding="0" cellspacing="0"  align="center">
			<tr>
				<td colspan="2" width="700" height="50">
					<font style="font-size:30px; letter-spacing:3px; "><b>INTERNATIONAL LOGISTICS CO.,LTD</b></font><br> 
					<font style="font-size:15px; ">IL BLDG., 31-1, SHINHEUNGDONG 1GA, JUNG-GU, INCHEON-CITY, KOREA</font>
				</td>
			</tr>
			<tr>
				<td width="350" height="50"><font style="font-size:15px;">TEL.   032-772-8481</font></td>
				<td width="350" height="50"><font style="font-size:15px;">FAX.   032-765-8688</font></td>
			</tr>
			<tr>
				<td colspan="2"  width="700" height="3" style="background-color:#000000;"></td>
			</tr>
			<tr>
				<td colspan="2" width="700" height="20" ><font style="font-size:12px;">(Ocean InBound)</font></td>
			</tr>
		</table>
		</td>
	</tr>
	<tr>
		<td>
		<table border="1"  cellpadding="0" cellspacing="0" bordercolor="#000000" style="border-collapse:collapse;width:700px;background-color:#FFFFFF;"  align="center">
		<tr>	
			<td colspan="4" height="50" align="center"  style="font-size:20px;"><b><u>CARGO DELIVERY ORDER</u></b></td>
		</tr>
		<tr >	
		<tr>	
			<td colspan="2" width="350" height="80" valign="top" align="left"> 
				<table cellpadding="0" cellspacing="0">
					<tr rowspan="2" >
						<td width="350" valign="top">
						<table border="1" cellpadding="0" cellspacing="0" bordercolor="#000000" style="border-collapse:collapse;width:140px;background-color:#FFFFFF;">
							<tr><td  height="20" align="center"><b>Shipper</b></td>	</tr>
						</table>
						</td>
					</tr>
					<tr><td width="350" height="35" style="padding:5"><font style="font-size:11px;"><%=Shipper %><br /><%=ShipperAddress %></font></td></tr>                  
				</table>
		    </td>
			<td  colspan="2" rowspan="3" width="350" valign="top" align="center">
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td height="30" align="center" valign="top">
						<table cellpadding="0" cellspacing="0" border="1" bordercolor="#000000"  style="border-collapse:collapse;width:350px;background-color:#FFFFFF;" >
							<tr>							
								<td width="150" align="center" style="padding:10" class="style1"><b>House B/L No</b></td>
								<td style="padding:10" class="style1"><%=BLNo%></td>
							</tr>
						</table>
						</td>
					</tr>
					<tr>
						<td  colspan="2">
						<table width="350" cellpadding="0" cellspacing="0" >
							<tr><td colspan="2" height="12">&nbsp;</td></tr>
							<tr>							
								<td width="170" height="18" align="right" style="padding:5"><b>Master B/L No</b></td>
								<td width="170" height="18" align="left" style="padding:5"><%=MasterBL %></td>
							</tr>
							<tr>							
								<td width="170" height="18" align="right" style="padding:5"><b> MRN</b></td>
								<td width="170" height="18" align="left" style="padding:5"><%=MRN %></td>
							</tr>
							<tr>							
								<td width="170" height="18" align="right" style="padding:5"><b>MSN</b></td>
								<td width="170" height="18" align="left" style="padding:5"><%=MSN %></td>
							</tr>
							<tr>							
								<td width="170" height="18" align="right" style="padding:5"><b>HSN</b></td>
								<td width="170" height="18" align="left" style="padding:5"><%=HSN %></td>
							</tr>

							<tr>							
								<td width="170" height="18" align="right" style="padding:5"><b>Ware House</b></td>
								<td width="170" height="18" align="left" style="padding:5"><%=StorageName %></td>
							</tr>
							<tr>							
								<td width="170" height="80" align="right" style="padding:5" valign="bottom"><b>D/O Issue Date</b></td>
								<td width="170" height="80" align="left" style="padding:5" valign="bottom"><%=ClearanceDate %></td>
							</tr>
						</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>	
			<td  colspan="2" width="350"  height="80" valign="top"  align="left" > 
				<table cellpadding="0" cellspacing="0">
					<tr rowspan="2" >
						<td width="350" valign="top">
						<table  border="1" cellpadding="0" cellspacing="0" bordercolor="#000000"  style="border-collapse:collapse;width:140px;background-color:#FFFFFF;" >
							<tr><td height="20" align="center"><b>Consignee</b></td></tr>
						</table>
						</td>
					</tr>
					<tr><td height="35" style="padding:5"><font style="font-size:11px;"><%=Consignee %><br /><%=ConsigneeAddress %></font></td></tr>
				</table>
		    </td>
		</tr>
		<tr>	
			<td colspan="2"   width="350"  height="80" valign="top" align="left" > 
				<table cellpadding="0" cellspacing="0">
					<tr rowspan="2" >
						<td width="350" valign="top">
						<table  border="1" cellpadding="0" cellspacing="0" bordercolor="#000000"  style="border-collapse:collapse;width:140px;background-color:#FFFFFF;" >
							<tr><td height="20" align="center"><b>Notify Party</b></td></tr>
						</table>
						</td>
					</tr>
					<tr><td height="35" style="padding:5"><font style="font-size:11px;"><%=NotifyParty %><br /><%=NotifyPartyAddress %></font></td></tr>
				</table>

		    </td>
		</tr>
		<tr>	
			<td  width="175"  height="25" align="center"  style="padding:2"><b>Vessel Name</td>
			<td colspan="3"  height="25"  style="padding:2"><%=Description %></td>
		</tr>
		<tr>	
			<td  width="175"  height="25" align="center" style="padding:2"><b>Date of Arrival</b></td>
			<td  width="175"  height="25"  style="padding:2"><%=ClearanceDate %></td>
			<td  width="175"  height="25" align="center" style="padding:2"><b>Place of Receipt</b></td>
			<td  width="175"  height="25"  style="padding:2">&nbsp;</td>
		</tr>
		<tr>	
			<td  width="175"  height="25" align="center" style="padding:2"><b>Port of Loading</b></td>
			<td  width="175"  height="25"  style="padding:2"><%=PortOfLanding %></td>
			<td  width="175"  height="25" align="center" style="padding:2"><b>Port of Discharge</b></td>
			<td  width="175"  height="25"  style="padding:2">&nbsp;</td>
		</tr>
		<tr>	
			<td  width="175"  height="25" align="center" style="padding:2"><b>Place of Delivey</b></td>
			<td  width="175"  height="25"  style="padding:2">&nbsp;</td>
			<td  width="175"  height="25" align="center" style="padding:2"><b>Final Destination</b></td>
			<td  width="175"  height="25"  style="padding:2"><%=FinalDestination %></td>
		</tr>
		<tr>	
			<td  colspan="4" valign="top">
				<table cellpadding="0" cellspacing="0">
					<tr>	
						<td  width="175"  height="25" style="padding:2"><b>Mark & Number</b></td>
						<td  width="175"  height="25" style="padding:2"><b>No of package</b></td>
						<td  width="175"  height="25" style="padding:2"><b>Weight</b></td>
						<td  width="175"  height="25" style="padding:2"><b>Measur ement</b></td>
					</tr>
					<tr>	
						<td  width="175"  height="25" style="padding:2">&nbsp;</td>
						<td  width="175"  height="25" style="padding:2">&nbsp;</td>
						<td  width="175"  height="25" style="padding:2"><b>Description of good</b></td>
						<td  width="175"  height="25" style="padding:2">&nbsp;</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>	
			<td  colspan="4" height="250" valign="top">
				<table cellpadding="0" cellspacing="0">
					<tr>	
						<td  width="175"  height="35" style="padding:2">&nbsp;</td>
						<td  width="175"  height="35" style="padding:2"><%=TotalQuantity %></td>
						<td  width="175"  height="35" style="padding:2"><%=TotalGrossWeight %></td>
						<td  width="175"  height="35" style="padding:2"><%=TotalVolume %></td>
					</tr>
					<tr>	
						<td  width="175"  height="35" style="padding:2">&nbsp;</td>
						<td  width="175"  height="35" style="padding:2">&nbsp;</td>
						<td  width="350"  colspan="2" height="35" style="padding:2"><%=ItemList %></td>
						
					</tr>
                    <tr>	
						<td  colspan="4" width="700" height="60" style="padding:2">&nbsp;</td>
					</tr>
					<tr>	
						<td  colspan="4" width="700" style="padding:10" valign="top">*Container No:<br><br /><%=ContainerNo %> / <%=SealNo %></td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>	
			<td colspan="4"  height="160" valign="top"  style="padding:10">
				<table width="670"  cellpadding="0" cellspacing="0" style="background-image: url(../images/do_sajin_130.jpg);  background-repeat: no-repeat; background-position:100% 95%">
					<tr>	
						<td width="670" colspan="3" height="40" align="center"><b>본 D/O 를 제출한 수하인에게 화물인도를 승락합니다.</b></font></td>
					</tr>
					<tr>	
						<td width="670" colspan="3" height="10" align="right">INTERNATIONAL LOGISTICS CO.,LTD</td>
					</tr>
					<tr >	
						<td width="230" height="110">&nbsp;</td>
                        <td width="200" height="110">&nbsp;</td>
						<td width="240" height="110" valign="bottom" ><hr></hr></td>
					</tr>
				</table>
			</td>
		</tr>
		</table>
    </td>
    </tr>
    </table>
    </div>
</div>
</form>
</body>
</html>