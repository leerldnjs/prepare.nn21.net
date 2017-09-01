<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="CommercialInvoiceView.aspx.cs" Inherits="Request_CommercialInvoiceView" %>
<%@ Register src="../Member/LogedTopMenu.ascx" tagname="Loged" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
	<script src="../Common/jquery-1.4.2.min.js" type="text/javascript"></script>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
		.Title{border:solid 1px black; width:692px; height:30px; font-size:18px; font-weight:bold; text-align:center; letter-spacing:12px; padding-top:3px; }
		.Shipper{border:solid 1px black; height:90px; margin-top:-1px; padding:5px; }
		.PortOfLoading{border:solid 1px black; height:51px; margin-top:-1px; padding:5px; }
		.FinalDestination{border:solid 1px black; height:51px; margin-top:-1px; margin-left:-1px;  padding:5px; }
		.InvoiceNo{border:solid 1px black; margin-left:-1px; padding:5px; }
		.BL{border-left:solid 1px black; }
		.BB{border-bottom:solid 1px black;}
		.BR{border-right:solid 1px black;}
		.InputButton{width:110px; height:23px; margin:5px; }
		div#quick_menu {position:absolute; left:50%; margin-top:100px; margin-left:300px; border:1px solid #999999; background:#eeeeee;}
    </style>
	<script type="text/javascript">
		var initBody;
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

			IEPageSetupX.Print(true); // 인쇄하기
			// self.close();
			// PrintTest(); // 컨트롤설치여부 테스트
			// IEPageSetupX.RollBack(); // 수정 이전 값으로 되돌림(한 단계 이전만 지원)
			// IEPageSetupX.Clear(); // 여백은 0으로, 머리글/바닥글은 모두 제거, 배경색 및 이미지 인쇄 안함
			// IEPageSetupX.Print(true); // 인쇄대화상자 띄우기
			document.body.innerHTML = initBody;
		}

		function Modify() { location.href = "./CommercialInvoiceWrite.aspx?S=" + form1.HRequestFormPk.value + "&M=IM"; }
		//function Delete() { 	}
		function RequestFormWrite() {
			if (confirm("이 문서의 내용으로 국제물류 접수를 합니다. \r\n일정과 결제조건 등을 입력해주세요.")) {
				location.href = "./RequestWrite.aspx?M=Write&P=" + form1.HRequestFormPk.value;
			}
			else {
				return false;
			}
		}

		function Delete() {
			if (confirm("해당문서가 완전히 삭제됩니다.")) {
				Request.DeleteRequestForm(form1.HRequestFormPk.value, function (result) { alert("Delete Success"); location.href = "./OfferFormList.aspx" }, function (result) { alert(result); });
			}
			else {
				return false;
			}
		}
	</script>
</head>
<body class="MemberBody">
    <form id="form1" runat="server">
	<asp:ScriptManager ID="WebService" runat="server" ><Services><asp:ServiceReference Path="~/WebService/Request.asmx" /></Services></asp:ScriptManager>
	<uc1:Loged ID="Loged1" runat="server" />
    <div class="ContentsTopMenu">
		<div id="quick_menu">
			<p>
				<input type="button" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "dlstho") %>" onclick="RunPrint()" /><br />
				<input type="button" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "tnwjd") %>" onclick="Modify()" /><br />
				<input type="button" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "tkrwp") %>" onclick="Delete()" /><br />
				<input type="button" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "anffbwjqtn")%>" onclick="RequestFormWrite()" />
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
		
		<div id="PrintArea" >
		<div style="width:694px; background-color:White;" >
			<object id="IEPageSetupX" name="IEPageSetupX" classid="clsid:41C5BC45-1BE8-42C5-AD9F-495D6C8D7586" codebase="~/Common/IEPageSetupX.cab" width="0" height="0" >
				<param name="copyright" value="http://isulnara.com" />
			</object>
			<div class="Title">COMMERCIAL INVOICE</div>
			<div style="width:346px; float:left;">
				<div id="Shipper" class="Shipper"     >
					<strong>Shipper</strong>
					<div style="padding-left:25px; padding-top:10px; line-height:20px; font-size:12px;  letter-spacing:-1px; ">
						<div id="DivShipperName" style="font-weight:bold; "><%=Shipper %></div>
						<div id="DivShipperAddress"><%=ShipperAddress %></div>
					</div>
				</div>
				<div id="Consignee" class="Shipper" >
					<strong>Consignee</strong>
					<div style="padding-left:25px; padding-top:10px; line-height:20px; font-size:12px;  letter-spacing:-1px; ">
						<div id="DivConsigneeName" style="font-weight:bold; "><%=Consignee %></div>
						<div id="DivConsigneeAddress" ><%=ConsigneeAddress %></div>					
					</div>
				</div>
				<div id="NotifyParty" class="Shipper"      >
					<strong>Notify Party</strong>
					<div style="padding-left:25px; padding-top:10px; line-height:20px; font-size:12px;  letter-spacing:-1px; ">
						<div id="DivNotifyPartyName" style="font-weight:bold; "><%=NotifyParty %></div>
						<div id="DivNotifyPartyAddress" ><%=NotifyPartyAddress %></div>					
					</div>
				</div>
				<div style="width:173px; height:123px; float:left; ">
					<div class="PortOfLoading" id="PortOfLanding"><strong>Port of loading</strong><div id="DivPortOfLanding" style="padding:10px;"><%=PortOfLanding %></div></div>
					<div class="PortOfLoading" id="Carrier"><strong>Carrier</strong><div id="DivCarrier" style="padding:10px;"><%=Carrier %></div>	</div>
				</div>
				<div style="width:173px; height:123px; float:right; ">
					<div class="FinalDestination" id="FinalDestination"><strong>Finial destination</strong><div id="DivFinialDestination" style="padding:10px;"><%=FinalDestination %></div></div>
					<div class="FinalDestination" id="SailingOnOrAbout"><strong>Sailing on or About</strong><div id="DivSailingOnOrAbout" style="padding:10px;"><%=SailingOnOrAbout %></div></div>
				</div>
			</div>
			<div style="width:347px; float:right;">
				<div class="InvoiceNo" id="InvoiceNo" style="height:57px; margin-left:-2px; margin-top:-1px; ">
					<strong>Date & No of Invoice</strong><div style="padding-left:25px; padding-top:15px;"><%=InvoiceNo %></div>
				</div>
				<div class="InvoiceNo" id="PaymentTerms" style="height:57px; margin-left:-2px; margin-top:-1px; ">
					<strong>Payment terms</strong><div style="padding-left:25px; padding-top:15px;  "><%=PaymentTerms %></div>
				</div>
				<div class="InvoiceNo" id="Buyer" style="height:55px; margin-left:-2px;  margin-top:-1px; ">
					<strong>Buyer</strong><div style="padding-left:25px; padding-top:15px;  "><%=Buyer %></div>
				</div>
				<div class="InvoiceNo" id="OtherReferences"     style="margin-left:-2px;  height:214px; margin-top:-1px; ">
					<strong>Other References</strong><div style="padding-left:25px; padding-top:15px;"><%=Memo %></div>
				</div>
			</div>
			<div style="width:694px; clear:both; " id="ItemTable"     >
				<table border="0" cellpadding="0" cellspacing="0" style="width:694px;">
					<thead>
						<tr style="height:40px;">
							<td class="BB BL" style="width:120px; padding-left:10px; font-weight:bold;">Marks & <br />Number of PKGS</td>
							<td class="BB BL" style="padding:5px; text-align:center; font-weight:bold; ">Description of Goods</td>
							<td class="BB BL" style="width:100px; text-align:center; font-weight:bold; ">Quantity</td>
							<td class="BB BL" style="width:95px; text-align:center; font-weight:bold; ">Unit Price</td>
							<td class="BB BL BR" style="width:150px; text-align:center; font-weight:bold; ">Amount</td>
						</tr>
						<tr style="height:30px;">
							<td class="BL" >&nbsp;</td>
							<td class="BL" >&nbsp;</td>
							<td class="BL" >&nbsp;</td>
							<td class="BL" style="text-align:center;" ><%=MonetaryUnit %></td>
							<td class="BL BR" >&nbsp;</td>
						</tr>
					</thead>
					<tbody>
						<%=ItemList %>
						<tr style="height:40px;">
							<td class="BL BB" >&nbsp;</td>
							<td class="BB" colspan="2" style="text-align:right; padding-right:10px; font-weight:bold;" ><%=TotalQuantity %></td>
							<td class="BR BB" colspan="2" style="text-align:right; padding-right:30px; font-weight:bold;" ><%=TotalAmount %></td>
						</tr>
					</tbody>
				</table>
			</div>
			<table border="0" cellpadding="0" cellspacing="0" style="width:694px; margin-top:-1px;  ">
				<tr style="height:150px;">
					<td class="BL BB" style="text-align:center; font-weight:bold;">&nbsp;</td>
					<td class="BR BB" style="width:350px; text-align:center; font-weight:bold; ">&nbsp;</td>
				</tr>
			</table>
		</div>
		</div>
	</div>	
    </form>
</body>
</html>