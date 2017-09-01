<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommentWithFile.aspx.cs" Inherits="UploadedFiles_CommentWithFile" %>

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
			}
		</script>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <fieldset style="padding-right:20px; " >
			<legend><strong>FILE UPLOAD</strong></legend>
			<div style="padding:20px;">
			<textarea id="TBComment" name="TBComment" rows="10" cols="80"></textarea><br />
			<p>File 1 : <input type="text" name="TB0" style="width:100px;" />&nbsp;&nbsp;<input type="file" name="FILE0" id="FILE0" runat="server" /></p>
			</div>
			<div style="padding-bottom:20px;  padding-left:200px; "><asp:Button ID="Button1" runat="server" Text="SAVE" onclick="Button1_Click" /></div>
			<input type="hidden" id="HCheckPostBack" value="<%=IsUpload %>" />
			<input type="hidden" id="HGubun" value="<%=Gubun %>" />
		</fieldset>
    </form>
</body>
</html>