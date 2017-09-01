<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommercialDocu_PackingList.aspx.cs" Inherits="CustomClearance_CommercialDocu_PackingList" Debug="true" %>
<%@ Register src="../Admin/LogedWithoutRecentRequest.ascx" tagname="LogedWithoutRecentRequest" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script src="../Common/jquery-1.4.2.min.js" type="text/javascript"></script>
	
	<style type="text/css">
		.Title{border:solid 1px black; width:692px; height:30px; font-size:18px; text-align:center; letter-spacing:5px; padding-top:5px; }
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
			//IEPageSetupX.align = center;

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
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px; font-family:@Arial Unicode MS, Arial;" >
    <form id="form1" runat="server">
    <div>
    <div>
    	<div class="ContentsTopMenu">
		<div id="quick_menu">
			<p>
				<input type="button" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "dlstho") %>" onclick="RunPrint()" /><br />
				<input type="button" class="InputButton" value="Select Printer" onclick="SelectPrinter()" /><br />
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
				
		<div id="PrintArea" >
				<% if(gubun=="Stamp"){ %>
					<div style=" position: absolute; width: 100px; height: 100px; margin-left: 230px; margin-top: 500px; ">
						<img src="/Images/Check_Back.png" style="width:75px; height:225px;" />
					</div>
					<div style=" position: absolute; width: 100px; height: 100px; margin-left: 230px; margin-top: 650px; ">
						<img src="/Images/Check_Stamp.png" style="width:73px; height:74px;" />
					</div>
				<% } %>

		<div style="width:694px; font-size:13px;   background-color:White;" >
			<object id="IEPageSetupX" name="IEPageSetupX" classid="clsid:41C5BC45-1BE8-42C5-AD9F-495D6C8D7586" codebase="~/Common/IEPageSetupX.cab" width="0" height="0" >
				<param name="copyright" value="http://isulnara.com" />
			</object>
			<div class="Title">PACKING LIST</div>
			<div style="width:346px; float:left;">
				<div id="Shipper" class="Shipper" >
					Shipper
					<div style="padding-left:5px; padding-top:5px; line-height:17px; font-size:13px;  letter-spacing:-1px; ">
						<div id="DivShipperName" ><%=Shipper %></div>
						<div id="DivShipperAddress"><%=ShipperAddress %></div>
					</div>
				</div>
				<div id="Consignee" class="Shipper">
					Consignee
					<div style="padding-left:5px; padding-top:5px; line-height:17px; font-size:13px;  letter-spacing:-1px; ">
						<div id="DivConsigneeName" ><%=Consignee %></div>
						<div id="DivConsigneeAddress" ><%=ConsigneeAddress %></div>					
					</div>
				</div>
				<div id="NotifyParty" class="Shipper">
					Notify Party
					<div style="padding-left:5px; padding-top:5px; line-height:17px; font-size:13px;  letter-spacing:-1px; ">
						<div id="DivNotifyPartyName" ><%=NotifyParty %></div>
						<div id="DivNotifyPartyAddress" ><%=NotifyPartyAddress %></div>					
					</div>
				</div>
				<div style="width:173px; height:104px; float:left; ">
					<div class="PortOfLoading" id="PortOfLanding">
						Port of loading
						<div id="DivPortOfLanding" style="padding:5px;"><%=PortOfLanding %></div>
					</div>
					<div class="PortOfLoading" id="Carrier">
						Carrier
						<div id="DivCarrier" style="padding:5px;"><%=Carrier %></div>	
					</div>
				</div>
				<div style="width:173px; height:104px; float:right; ">
					<div class="FinalDestination" id="FinalDestination">
						Finial destination
						<div id="DivFinialDestination" style="padding:5px;"><%=FinalDestination %></div>	
					</div>
					<div class="FinalDestination" id="SailingOnOrAbout">
						Sailing on or About
						<div id="DivSailingOnOrAbout" style="padding:5px;"><%=SailingOnOrAbout %></div>	
					</div>
				</div>
			</div>
			<div style="width:347px; float:right;">
				<div class="InvoiceNo" id="InvoiceNo" style="height:40px; margin-left:-2px; margin-top:-1px; ">
					Date & No of Invoice
					<div style="padding-left:5px; padding-top:5px;"><%=InvoiceNo %></div>
				</div>
				<div class="InvoiceNo" id="PaymentTerms" style="height:40px; margin-left:-2px; margin-top:-1px; ">
					Payment terms
					<div style="padding-left:5px; padding-top:5px;  "><%=PaymentTerms %></div>
				</div>
				<div class="InvoiceNo" id="Buyer" style="height:40px; margin-left:-2px;  margin-top:-1px; ">
					Buyer
					<div style="padding:5px; "><%=Buyer %></div>
				</div>

				<div class="InvoiceNo" id="OtherReferences"     style="margin-left:-2px;  height:228px; margin-top:-1px; ">
					Other References
					<div style="padding-left:25px; padding-top:15px;"><%=Memo %></div>
				</div>
			</div>
			<div style="width:694px; clear:both; " id="ItemTable"     >
				<table border="0" cellpadding="0" cellspacing="0" style="width:694px;">
					<thead>
						<tr style="height:30px;">
							<td class="BB BL" style="width:120px; padding-left:10px; font-weight:bold;">Marks & <br />Number of PKGS</td>
							<td class="BB BL" style="padding:5px; text-align:center; font-weight:bold; ">Description of Goods</td>
							<td class="BB BL" style="width:89px; text-align:center; font-weight:bold; ">Quantity</td>
							<td class="BB BL" style="width:85px; text-align:center; font-weight:bold; ">G-Weight</td>
							<td class="BB BL" style="width:85px; text-align:center; font-weight:bold; ">N-Weight</td>
							<td class="BB BL BR" style="width:85px; text-align:center; font-weight:bold; ">Volume</td>
						</tr>
						<tr style="height:25px;">
							<td class="BL" >&nbsp;</td>
							<td class="BL" >&nbsp;</td>
							<td class="BL" >&nbsp;</td>
							<td class="BL" >&nbsp;</td>
							<td class="BL" style="text-align:center;" ></td>
							<td class="BL BR" >&nbsp;</td>
						</tr>
					</thead>
					<tbody>
						<%=ItemList %>
						<tr style="height:23px;">
							<td class="BL BT" colspan="2"  style="text-align:center; padding-right:10px;   " >TOTAL</td>
							<td class="BT" style="text-align:right; padding-right:10px; font-weight:bold;" ><%=TotalQuantity %></td>
							<td class="BT" style="text-align:right; padding-right:10px; font-weight:bold;" ><%=TotalGrossWeight %></td>
							<td class="BT" style="text-align:right; padding-right:10px; font-weight:bold;" ><%=TotalNetWeight %></td>
							<td class="BR BT" style="text-align:right; padding-right:10px; font-weight:bold;" ><%=TotalVolume %></td>
						</tr>
					</tbody>
				</table>
			</div>
			<%--<div style="position:absolute; background-image:url('../UploadedFiles/3/Yantai.jpg'); background-repeat:no-repeat; z-index:1;"></div>--%>
			<table border="0" cellpadding="0" cellspacing="0" style="width:694px; margin-top:-1px;"  >
				<tr style="height:120px;">
					<td class="BL BB" style="text-align:center;  ">&nbsp;</td>
					<td class="BR BB" style="width:350px; text-align:center; ">&nbsp;</td>
				</tr>
			</table>
			<%=StampImg %>
		</div>
		</div>
	</div>	
    </div>    
    </div>
    </form>
</body>
</html>