<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetWarehouse.aspx.cs" Inherits="Admin_Dialog_SetWarehouse" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script src="../../Common/jquery-1.10.2.min.js"></script>
	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
	<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />    
	<script>
		jQuery(document).ready(function () {
			$("#BTN_GetPath").on("click", GetPath);
			$("#BTN_Submit").on("click", SetSave);
		});
		function SetSave() {
			Admin.AddOurBranchStorage($("#HOurBranchStoragePk").val(), $("#HOurBranchCode").val(), $("#StorageName").val(), $("#StorageCode").val(), $("#StorageAddress").val(), $("#TEL").val(), $("#FAX").val(), $("#NaverMapPathX").val(), $("#NaverMapPathY").val(), function (result) {
				if (result == "1") {
					alert("SUCCESS");
					window.close();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function GetPath() {
			var EachValue = $("#NaverURI").val().split("&");
			for (var i = 0; i < EachValue.length; i++) {
				if (EachValue[i].substr(0, 4) == "lat=") {
					$("#NaverMapPathX").val(EachValue[i].substr(4));
					alert(EachValue[i]);
				}
				if (EachValue[i].substr(0, 4) == "lng=") {
					$("#NaverMapPathY").val(EachValue[i].substr(4));
					alert(EachValue[i]);
				}
			}
		}
	</script>
</head>
<body style="background-color:#999999; overflow-x:hidden; ">
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>

	<div style="width:570px;   padding-left:5px; padding-right:5px; padding-top:5px; padding-bottom:5px; background-color:white;">
		<input type="hidden" id="HOurBranchStoragePk" value="<%=Request.Params["S"] %>" />
		<input type="hidden" id="HOurBranchCode" value="<%=Request.Params["BranchPk"] %>" />
		<p>창고명 : <input type="text" id="StorageName" value="<%=TITLE %>" /></p>    
		<p>창고코드 : <input type="text" id="StorageCode" value="<%=StorageCode %>" /></p>    
		<p>주소 : <input type="text" id="StorageAddress" style="width:450px; " value="<%=Address %>" /></p>    
		<p>전화번호 : <input type="text" id="TEL" value="<%=TEL %>" /></p>    
		<p>FAX : <input type="text" id="FAX" value="<%=FAX %>" /></p>    
		<p>Naver 지도 URL : <input type="text" id="NaverURI" /><input type="button" id="BTN_GetPath" value="좌표추출" /></p>    
		<p>좌표 X : <input type="text" id="NaverMapPathX" value="<%=X %>" /></p>    
		<p>좌표 Y : <input type="text" id="NaverMapPathY" value="<%=Y %>" /></p>    
		<p style="text-align:center;"><input type="button" id="BTN_Submit" value="저장" /></p>
    </div>
    </form>
</body>
</html>