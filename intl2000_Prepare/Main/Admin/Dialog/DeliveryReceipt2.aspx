<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="DeliveryReceipt2.aspx.cs" Inherits="Admin_Dialog_DeliveryReceipt2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script src="../../Common/jquery-1.4.2.min.js" type="text/javascript"></script>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
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
				/*
				if (form1.HGubun.value == "Print") {
					RunPrint();
					self.close();
				}
				*/
			}
			function RunPrint() {
				/*
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
				*/
			}
			function RunFAX() {
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
<body>
    <form id="form1" runat="server">
    <div>
	    <div class="ContentsTopMenu">
			<input type="hidden" id="HGubun" value="<%=GUBUN %>"  />
			<!--
			<input type="button" onclick="RunPrint()" value="인쇄" />
			-->
			<div id="PrintArea" >
				<div style="width:694px; font-size:13px;   background-color:White;" >
					<object id="IEPageSetupX" name="IEPageSetupX" classid="clsid:41C5BC45-1BE8-42C5-AD9F-495D6C8D7586" codebase="~/Common/IEPageSetupX.cab" width="0" height="0" >
						<param name="copyright" value="http://isulnara.com" />
					</object>
						<div>
						<div style="float:left; width:70px; height:60px; padding:5px;  "><img alt="" src="http://nn21.net/images/LogoBlack.jpg" style="width:70px; height:60px;" /></div>
						<div style="font-size:18px; font-weight:bold;  text-align:left; letter-spacing:3px;">INTERNATIONAL LOGISTICS CO LTD</div>
						<div style="font-size:12px; text-align:left; padding-top:5px; letter-spacing:1px;">IL BLDG 3F, 31-1, SHINHEUNG-DONG 1GA, JUNG-GU, INCHEON-CITY, KOREA.</div>
						<div style="font-size:12px; text-align:left; padding-left:300px;  padding-top:5px; letter-spacing:1px;">TEL : 032-772-8481              FAX : 032-765-8688</div>
					</div>
					<div style="clear:both;">
						<div style="float:right;">No : <%=BLNo %> / 창고 : <%=StorageName %> / 출고 : '<%=Year %>. <%=Month %>. <%=Day %></div>
					</div>
					<table border="0" cellpadding="0" cellspacing="0" style="width:694px;" >
						<tr style="height:50px;">
							<td class="BB BT BL " style="width:174px; text-align:center; font-size:18px; font-weight:bold; "><%=CompanyName %> (<%=CompanyCode %>)</td>
							<td class="BB BT BL " colspan="2" style="text-align:left; padding-left:10px; "><%=Address %></td>
							<td class="BB BT BL BR" style="width:174px; padding-left:10px; line-height:18px;  " ><%=TEL %></td>
						</tr>
						<tr style="height:30px;">
							<td class="BB BL" style="width:174px; text-align:center; ">품명</td>
							<td class="BB BL" style="width:174px; text-align:center; " >수량</td>
							<td class="BB BL" style="width:174px; text-align:center; " >중량</td>
							<td class="BB BL BR" style="width:174px; text-align:center; " >체적</td>
						</tr>
						<tr style="height:30px;">
							<td class="BB BL" style="width:174px; text-align:center; "><%=ItemName %></td>
							<td class="BB BL" style="width:174px; text-align:center; " ><%=BoxCount %></td>
							<td class="BB BL" style="width:174px; text-align:center; " ><%=Weight %></td>
							<td class="BB BL BR" style="width:174px; text-align:center; " ><%=Volumn %></td>
						</tr>
						<tr style="height:30px;">
							<td class="BB BL" style="width:174px; text-align:center; "><%=DeliveryGubun %></td>
							<td class="BB BL" style="width:174px; text-align:center; " ><%=DriverName %> (<%=DriverTEL %>)</td>
							<td class="BB BL BR" colspan="2" style="text-align:center; " ><%=Price %></td>
						</tr>
						<tr style="height:50px;">
							<td class="BB BL" style="width:174px; text-align:center; ">기 타 사 항</td>
							<td class="BB BL BR" colspan="3" style="text-align:center; " ><%=Memo %><%=MemberMemo %></td>
						</tr>
					</table>

					<div style="clear:both; width:694px; text-align:center; padding:10px; ">--------------------------------------------------------------------------------------------------------------</div>
					
					<div>
						<div style="float:left; width:70px; height:60px; padding:5px;  "><img alt="" src="http://nn21.net/images/LogoBlack.jpg" style="width:70px; height:60px;" /></div>
						<div style="font-size:18px; font-weight:bold;  text-align:left; letter-spacing:3px;">INTERNATIONAL LOGISTICS CO LTD</div>
						<div style="font-size:12px; text-align:left; padding-top:5px; letter-spacing:1px;">IL BLDG 3F, 31-1, SHINHEUNG-DONG 1GA, JUNG-GU, INCHEON-CITY, KOREA.</div>
						<div style="font-size:12px; text-align:left; padding-left:300px;  padding-top:5px; letter-spacing:1px;">TEL : 032-772-8481              FAX : 032-765-8688</div>
					</div>
					<div style="clear:both;">
						<div style="float:right;">No : <%=BLNo %> / 창고 : <%=StorageName %> / 출고 : '<%=Year %>. <%=Month %>. <%=Day %></div>
					</div>
					<table border="0" cellpadding="0" cellspacing="0" style="width:694px;" >
						<tr style="height:50px;">
							<td class="BB BT BL " style="width:174px; text-align:center; font-size:18px; font-weight:bold; "><%=CompanyName %> (<%=CompanyCode %>)</td>
							<td class="BB BT BL " colspan="2" style="text-align:left; padding-left:10px; "><%=Address %></td>
							<td class="BB BT BL BR" style="width:174px; padding-left:10px; line-height:18px;  " ><%=TEL %></td>
						</tr>
						<tr style="height:30px;">
							<td class="BB BL" style="width:174px; text-align:center; ">품명</td>
							<td class="BB BL" style="width:174px; text-align:center; " >수량</td>
							<td class="BB BL" style="width:174px; text-align:center; " >중량</td>
							<td class="BB BL BR" style="width:174px; text-align:center; " >체적</td>
						</tr>
						<tr style="height:30px;">
							<td class="BB BL" style="width:174px; text-align:center; "><%=ItemName %></td>
							<td class="BB BL" style="width:174px; text-align:center; " ><%=BoxCount %></td>
							<td class="BB BL" style="width:174px; text-align:center; " ><%=Weight %></td>
							<td class="BB BL BR" style="width:174px; text-align:center; " ><%=Volumn %></td>
						</tr>
						<tr style="height:30px;">
							<td class="BB BL" style="width:174px; text-align:center; "><%=DeliveryGubun %></td>
							<td class="BB BL" style="width:174px; text-align:center; " ><%=DriverName %> (<%=DriverTEL %>)</td>
							<td class="BB BL BR" colspan="2" style="text-align:center; " ><%=Price %></td>
						</tr>
						<tr style="height:50px;">
							<td class="BB BL" colspan="2" style="width:174px; text-align:center; "><%=Memo %></td>
							<td class="BB BL" style="width:174px; text-align:center; " >인수자 서명</td>
							<td class="BB BL BR" style="text-align:right; font-size:10px;  " >(인)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
						</tr>
					</table>

					<div style="clear:both; width:694px; text-align:center; padding:10px; ">--------------------------------------------------------------------------------------------------------------</div>

					<div>
						<div style="float:left; width:70px; height:60px; padding:5px;  "><img alt="" src="http://nn21.net/images/LogoBlack.jpg" style="width:70px; height:60px;" /></div>
						<div style="font-size:18px; font-weight:bold;  text-align:left; letter-spacing:3px;">INTERNATIONAL LOGISTICS CO LTD</div>
						<div style="font-size:12px; text-align:left; padding-top:5px; letter-spacing:1px;">IL BLDG 3F, 31-1, SHINHEUNG-DONG 1GA, JUNG-GU, INCHEON-CITY, KOREA.</div>
						<div style="font-size:12px; text-align:left; padding-left:300px;  padding-top:5px; letter-spacing:1px;">TEL : 032-772-8481              FAX : 032-765-8688</div>
					</div>
					<div style="clear:both;">
						<div style="float:right;">No : <%=BLNo %> / 창고 : <%=StorageName %> / 출고 : '<%=Year %>. <%=Month %>. <%=Day %></div>
					</div>
					<table border="0" cellpadding="0" cellspacing="0" style="width:694px;" >
						<tr style="height:50px;">
							<td class="BB BT BL " style="width:174px; text-align:center; font-size:18px; font-weight:bold; "><%=CompanyName %> (<%=CompanyCode %>)</td>
							<td class="BB BT BL " colspan="2" style="text-align:left; padding-left:10px; "><%=Address %></td>
							<td class="BB BT BL BR" style="width:174px; padding-left:10px; line-height:18px;  " ><%=TEL %></td>
						</tr>
						<tr style="height:30px;">
							<td class="BB BL" style="width:174px; text-align:center; ">품명</td>
							<td class="BB BL" style="width:174px; text-align:center; " >수량</td>
							<td class="BB BL" style="width:174px; text-align:center; " >중량</td>
							<td class="BB BL BR" style="width:174px; text-align:center; " >체적</td>
						</tr>
						<tr style="height:30px;">
							<td class="BB BL" style="width:174px; text-align:center; "><%=ItemName %></td>
							<td class="BB BL" style="width:174px; text-align:center; " ><%=BoxCount %></td>
							<td class="BB BL" style="width:174px; text-align:center; " ><%=Weight %></td>
							<td class="BB BL BR" style="width:174px; text-align:center; " ><%=Volumn %></td>
						</tr>
						<tr style="height:30px;">
							<td class="BB BL" style="width:174px; text-align:center; "><%=DeliveryGubun %></td>
							<td class="BB BL" style="width:174px; text-align:center; " ><%=DriverName %> (<%=DriverTEL %>)</td>
							<td class="BB BL BR" colspan="2" style="text-align:center; " ><%=Price %></td>
						</tr>
						<tr style="height:50px;">
							<td class="BB BL" style="width:174px; text-align:center; ">기 타 사 항</td>
							<td class="BB BL BR" colspan="3" style="text-align:center; " ><%=Memo %></td>
						</tr>
					</table>
					<div style="padding-top:4px;   ">
						<div style="position:absolute; ">
							<div style="font-weight:bold; font-size:11px; color:#4B0082; padding-left:3px;  ">☞화남지역 : TEL:400-880-8300  HP:138-26214498  </div>
							<div style="padding-left:20px; padding-bottom:2px; font-size:11px; ">광주, 불산, 중산, 순덕, 동관, 산토</div>
						</div>
						<div style="position:absolute; margin-left:360px; ">
							<div style="font-weight:bold; font-size:11px; color:#4B0082; padding-left:3px;  ">☞화동지역 : TEL:400-708-1600  HP:135-66229451  </div>
							<div style="padding-left:20px; padding-bottom:2px; font-size:11px; ">상해, 온주, 이우, 항주, 소흥</div>
						</div>
						<div style="position:absolute; margin-top:35px; ">
							<div style="font-weight:bold; font-size:11px; color:#4B0082; padding-left:3px;  ">☞화북지역 : TEL:400-708-0060  HP:186-06319989  </div>
							<div style="padding-left:20px; padding-bottom:2px; font-size:11px; ">연태, 청도, 위해, 천진, 북경</div>
						</div>
						<div style="position:absolute; margin-top:35px; margin-left:360px; ">
							<div style="font-weight:bold; font-size:11px; color:#4B0082; padding-left:3px;  ">☞동북지역 : TEL:400-708-0060  HP:138-04039968  </div>
							<div style="padding-left:20px; padding-bottom:2px; font-size:11px; ">심양, 대련, 단동, 장춘, 하얼빈, 길림</div>
						</div>
					</div>
					<div style="clear:both; padding-top:50px; ">&nbsp;</div>
				
				</div>
			</div>
	</div>	
    </div>
    </form>
</body>
</html>