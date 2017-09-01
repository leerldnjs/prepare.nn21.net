<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WarehouseMap2.aspx.cs" Inherits="Request_Dialog_WarehouseMap2" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
	<% if (TEL!=""){ %>
		TEL : <%=TEL %><br />
	<% } %>
	주소 : <%=Address %><br /><br />

	<div id = "testMap" style="border:1px solid #000; "></div>
	<form id="form1" runat="server">
		<input type="hidden" name="S" value="<%=Request.Params["S"] %>">
		<input type="hidden" name="Title" value="<%=Title %>">
		<input type="hidden" name="TEL" value="<%=TEL %>">
		<input type="hidden" name="Address" value="<%=Address %>">
		<input type="hidden" name="Y" value="<%=NaverY %>" />
		<input type="hidden" name="X" value="<%=NaverX %>" />
	</form>
</div>

		<script type="text/javascript">
			var Y = form1.Y.value;
			var X = form1.X.value;
			var Title = form1.Title.value;

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

			var oMarker = new nhn.api.map.Marker(oIcon, { title: Title });  //마커를 생성한다 
			oMarker.setPoint(oPoint); //마커의 좌표를 oPoint 에 저장된 좌표로 지정한다
			oMap.addOverlay(oMarker); //마커를 네이버 지도위에 표시한다

			var oLabel = new nhn.api.map.MarkerLabel(); // 마커 라벨를 선언한다. 
			oMap.addOverlay(oLabel); // - 마커의 라벨을 지도에 추가한다. 
			oLabel.setVisible(true, oMarker); // 마커의 라벨을 보이게 설정한다.
		</script>
</body>
</html>