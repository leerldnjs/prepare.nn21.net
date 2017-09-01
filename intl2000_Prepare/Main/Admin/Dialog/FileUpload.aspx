<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileUpload.aspx.cs" Inherits="Admin_Dialog_FileUpload" Debug="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script type="text/javascript">
		var DA;
		window.onload = function () {
			if (form1.HCheckPostBack.value == "Y") {
				alert("Success");
				opener.location.reload();
				self.close();
				return false;
			}
			else {
				if (form1.HGubun.value != "2") {
					for (var i = 0; i < 4; i++) {
						document.getElementById("CHK" + i).value = form1.HGubun.value;
					}
				}
				if (form1.HGubun.value == "2") {
					for (var i = 0; i < 4; i++) {
						document.getElementById("SP_CHK" + i).innerHTML = GetSPCHK(i);
						document.getElementById("CHK" + i).value = "11";
					}
				}
			}
		}
		function GetSPCHK(Count) {
			return "<input type=\"checkbox\" id=\"CHK" + Count + "_A\" checked=\"checked\" onclick=\"GetCHKValue('"+Count+"')\" /><label for=\"CHK" + Count + "_A\">Admin</label>" +
					"&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"checkbox\" id=\"CHK" + Count + "_S\" onclick=\"GetCHKValue('" + Count + "')\" /><label for=\"CHK" + Count + "_S\">Shipper</label>" +
					"&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"checkbox\" id=\"CHK" + Count + "_C\" onclick=\"GetCHKValue('" + Count + "')\" /><label for=\"CHK" + Count + "_C\">Consignee</label>";
		}
		function GetCHKValue(Count) {
			var ReturnValue = 10;
			if (document.getElementById("CHK" + Count + "_A").checked) {
				ReturnValue += 1;
			}
			if (document.getElementById("CHK" + Count + "_S").checked) {
				ReturnValue += 2;
			}
			if (document.getElementById("CHK" + Count + "_C").checked) {
				ReturnValue += 4;
			}
			document.getElementById("CHK" + Count).value = ReturnValue;
		}
	</script>
	<link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
</head>
<body style="padding:5px;">
    <form id="form1" runat="server">
		<fieldset style="padding-right:20px; " >
			<legend><strong>FILE UPLOAD</strong></legend>
			<ul style="line-height:23px;">
				<li>File 1 : 
					<input type="text" name="TB0" style="width:100px;" />&nbsp;&nbsp;<input type="file" name="FILE0" id="FILE0" runat="server" />
					<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<span id="SP_CHK0"></span>
					<input type="hidden" id="CHK0" name="CHK0" value="" />
					</li>
				<li>File 2 : 
					<input type="text" name="TB1" style="width:100px;" />&nbsp;&nbsp;<input type="file" name="FILE0" id="FILE1" runat="server" />
					<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<span id="SP_CHK1"></span>
					<input type="hidden" id="CHK1" name="CHK1" value="" />
					</li>
				<li>File 3 : 
					<input type="text" name="TB2" style="width:100px;" />&nbsp;&nbsp;<input type="file" name="FILE0" id="FILE2" runat="server" />
					<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<span id="SP_CHK2"></span>
					<input type="hidden" id="CHK2" name="CHK2" value="" />
					</li>
				<li>File 4 : 
					<input type="text" name="TB3" style="width:100px;" />&nbsp;&nbsp;<input type="file" name="FILE0" id="FILE3" runat="server" />
					<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<span id="SP_CHK3"></span>
					<input type="hidden" id="CHK3" name="CHK3" value="" />
					</li>
			</ul>
			<div style="padding-top:20px; padding-bottom:20px;  padding-left:200px; "><asp:Button ID="Button1" runat="server" Text="SAVE" onclick="Button1_Click" /></div>
			<input type="hidden" id="HCheckPostBack" value="<%=IsUpload %>" />
			<input type="hidden" id="HGubun" value="<%=Gubun %>" />
		</fieldset>
    </form>
</body>
</html>