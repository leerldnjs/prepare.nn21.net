<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FreightChargeView_ASECO.aspx.cs" Debug="true" Inherits="Request_FreightChargeView_ASECO" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
		.OurLogo{ border-bottom:solid 2px black; width:692px; height:75px; text-align:center; padding-top:3px; }
		.Shipper{border:solid 1px black; height:90px; margin-top:-1px; padding:5px; }
		.PortOfLoading{border:solid 1px black; height:51px; margin-top:-1px; padding:5px; }
		.FinalDestination{border:solid 1px black; height:51px; margin-top:-1px; margin-left:-1px;  padding:5px; }
		.InvoiceNo{border:solid 1px black; margin-left:-1px; padding:5px; }
		.BL{border-left:solid 1px black; }
		.BB{border-bottom:solid 1px black;}
		.BR{border-right:solid 1px black;}
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

			IEPageSetupX.Print(false); // 인쇄하기
			// self.close();
			// PrintTest(); // 컨트롤설치여부 테스트
			// IEPageSetupX.RollBack(); // 수정 이전 값으로 되돌림(한 단계 이전만 지원)
			// IEPageSetupX.Clear(); // 여백은 0으로, 머리글/바닥글은 모두 제거, 배경색 및 이미지 인쇄 안함
			// IEPageSetupX.Print(true); // 인쇄대화상자 띄우기
			document.body.innerHTML = initBody;
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
		function ActiveXDown() {
			location.href = "../Common/IEPageSetupX_en.exe";
		}
		function ShowEmail() {
			document.getElementById("PnEmail").innerHTML = "";
			document.getElementById("TBEmail").focus();
		}
		function SendEmail() {
			//alert(document.getElementById("TBEmail").value);
			Admin.EmailSend("korea@nn21.com", "국제물류 아이엘", document.getElementById("TBEmail").value, "", "국제물류 아이엘 || 요청하신 정산서 보내드립니다. ", PrintArea.innerHTML, function (result) {
				if (result == "1") {
					alert("OK");
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function PnNotCalculatedTariff_hide() {
			document.getElementById("PnNotCalculatedTariff").innerHTML= "";
		}
	</script>
</head>
<body>
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
	<input type="button" class="InputButton" value="<%=GetGlobalResourceObject("qjsdur", "dlstho") %>" onclick="RunPrint()" />	<%=BTN_FAX %> <input type="button" onclick="ActiveXDown()" value="인쇄 프로그램" /> 
	<% if (MEMBERINFO[2] == "ilic66") {%>
<input type="button" onclick="PnNotCalculatedTariff_hide();" value="관부가세 책정중 감추기" />
		<%} %>
 <br />
	<%=OnlyAdmin %>
    <div id="PrintArea" >
		<% if(Request.Params["Stamp"]=="Yes"){ %>
			<div style=" position: absolute; width: 100px; height: 100px; margin-left: 590px; margin-top: 200px; ">
				<img src="/Images/Check_Back.png" style="width:75px; height:225px;" />
			</div>
			<div style=" position: absolute; width: 100px; height: 100px; margin-left: 590px; margin-top: 350px; ">
				<img src="/Images/Check_Stamp.png" style="width:73px; height:74px;" />
			</div>
		<% } %>

		<object id="IEPageSetupX" name="IEPageSetupX" classid="clsid:41C5BC45-1BE8-42C5-AD9F-495D6C8D7586" codebase="~/Common/IEPageSetupX.cab" width="0" height="0" >
			<param name="copyright" value="http://isulnara.com" />
		</object>
		<div style="width:694px; background-color:White;" >
			<%=Title %>
			<div style="padding:10px; width:150px; height:20px; text-align:center; margin:0 auto; border:2px solid black; margin-top:20px; margin-bottom:20px; font-size:17px; letter-spacing:3px;   ">
				<%if (IsEstimation) {
		  Response.Write("견적 (报价)");
	  } else {
		  Response.Write("INVOICE");
	  } %>
				</div>
			<%=Header %>
			<br />
			<!--<%//=Item %> -->
			<%=Freight %>
			<%=BankInfo %>
            <table border="0" style="width: 694px; margin-top: 10px; margin-left: 5px;">
				<tr>
					<td align="left">상기와 같이 선임 및 부대비용을 청구하오니 상기 계좌번호로 입금시켜 주시기 바라며,<br />
						송금 후 송금내역을 FAX로 넣어 주시기 바랍니다.</td>
				</tr>
			</table>
		</div>
	</div>
    </form>
</body>
</html>