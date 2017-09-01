<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WarehouseMap.aspx.cs" Inherits="Request_Dialog_WarehouseMap" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<title>OpenAPI 2.0 - 지도 생성</title>
		<!-- prevent IE6 flickering -->
		<script type="text/javascript">
			try { document.execCommand('BackgroundImageCache', false, true); } catch (e) { }
		</script>
	    <link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />

<script type="text/javascript" src="http://openapi.map.naver.com/openapi/naverMap.naver?ver=2.0&key=301dac5bfb9f6e08fa31345a4a82da18"></script>
</head>

<body>

<div style="margin:20px;">
	<p><%=Title %></p>
	TEL : <%=TEL %><br />
	FAX : <%=FAX %><br />
	주소 : <%=Address %><br /><br />

	<div id = "testMap" style="border:1px solid #000; "></div>
	<form id="form1" runat="server">
		<input type="hidden" name="S" value="<%=Request.Params["S"] %>">
		<input type="hidden" name="Title" value="<%=Title %>">
		<input type="hidden" name="TEL" value="<%=TEL %>">
		<input type="hidden" name="FAX" value="<%=FAX %>">
		<input type="hidden" id="Address" value="<%=Address %>">
	</form>
</div>

		<script type="text/javascript">
			var S=form1.S.value;
			switch(S){
				case "2": 
					var Y=126.5956809;
					var X=37.4480169; 
					var Title="동방창고";
					break; 
				case "58": 
					var Y=126.6216439;
					var X=37.4692021; 
					var Title="구내1 신창고";
					break; 
				case "61": 
					var Y=126.6052706;
					var X=37.4586735; 
					var Title="구내4";
					break; 
				case "65": 
					var Y=126.6088054;
					var X=37.4566620; 
					var Title="극동";
					break; 
				case "69": 
					var Y=126.6170345;
					var X=37.4540276; 
					var Title="흥아창고";
					break; 
				case "71": 
					var Y=126.5956809;
					var X=37.4480169; 
					var Title="선광창고";
					break; 
				case "76": 
					var Y=126.6260727;
					var X=37.4529501; 
					var Title="한진국제창고";
					break; 
				case "77": 
					var Y=126.5983298;
					var X=37.4434135; 
					var Title="브이지로지스틱스창고";
					break; 
				case "79": 
					var Y=126.5956809;
					var X=37.4480169; 
					var Title="그린창고";
					break; 
				case "125": 
					var Y=126.5983298;
					var X=37.4434135; 
					var Title="인천콘테이너(본사)";
					break;
					/*
				case "125": 
					var Y=126.5983298;
					var X=37.4434135; 
					var Title="대아 (인천컨테이너보세)";
					break; 
					*/
				case "130": 
					var Y=126.6241119;
					var X=37.4524784; 
					var Title="세계로물류 보세창고";
					break; 
				case "132": 
					var Y=126.6148899;
					var X=37.4502671; 
					var Title="희창물산㈜인천물류지점";
					break; 
				case "133": 
					var Y=126.6180369;
					var X=37.4357295; 
					var Title="대우로지스틱 보세창고";
					break;
			    case "153":
			        var Y = 126.6218993;
			        var X = 37.4496052;
			        var Title = "대호보세창고";
			        break;
			    case "156":
			        var Y = 126.6217644;
			        var X = 37.4444624;
			        var Title = "대흥창고";
			        break;
			    case "157":
			        var Y = 126.6143969;
			        var X = 37.4848298;
			        var Title = "대보창고";
			        break;
			}

			var oPoint = new nhn.api.map.LatLng(parseFloat(X), Y);
			nhn.api.map.setDefaultPoint('LatLng');
			oMap = new nhn.api.map.Map('testMap', {
				point: oPoint,
				zoom: 8,
				enableWheelZoom: true,
				enableDragPan: true,
				enableDblClickZoom: false,
				mapMode: 0,
				activateTrafficMap: false,
				activateBicycleMap: false,
				minMaxLevel: [1, 14],
				size: new nhn.api.map.Size(700, 600)
			});
			var oSize = new nhn.api.map.Size(28, 37);
			var oOffset = new nhn.api.map.Size(14, 37);
			var oIcon = new nhn.api.map.Icon('http://static.naver.com/maps2/icons/pin_spot2.png', oSize, oOffset);
			   
			var oMarker = new nhn.api.map.Marker(oIcon, { title : Title });  //마커를 생성한다 
			oMarker.setPoint(oPoint); //마커의 좌표를 oPoint 에 저장된 좌표로 지정한다
			oMap.addOverlay(oMarker); //마커를 네이버 지도위에 표시한다
			 
			var oLabel = new nhn.api.map.MarkerLabel(); // 마커 라벨를 선언한다. 
			oMap.addOverlay(oLabel); // - 마커의 라벨을 지도에 추가한다. 
			oLabel.setVisible(true, oMarker); // 마커의 라벨을 보이게 설정한다.
		</script>
</body>
</html>