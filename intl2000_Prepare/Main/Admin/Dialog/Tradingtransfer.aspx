<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Tradingtransfer.aspx.cs" Inherits="Admin_Dialog_Tradingtransfer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
	<link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
</head>
<body style="padding: 5px;">
	<form id="form1" runat="server">
		<fieldset style="padding-right: 20px;">
			<legend><strong>TRADINGTRANSFER FILE UPLOAD</strong></legend>
			<ul style="line-height: 23px;">
				<li>
					<input type="text" name="TB0" style="width: 100px;" value="무역송금" readonly="readonly"/>&nbsp;&nbsp;
					<select id="RelationPk" name="RelationPk">
						<%=LoadOptionRelationCompany() %>
					</select>
					<input type="file" name="FILE0" id="FILE0" runat="server" />
				</li>
			</ul>
			<div style="padding-top: 20px; padding-bottom: 20px; padding-left: 200px;">
				<asp:Button ID="Button1" runat="server" Text="SAVE" OnClick="Button1_Click" /></div>
			<input type="hidden" id="HCheckPostBack" value="<%=IsUpload %>" />
			<input type="hidden" id="HGubun" value="<%=Gubun %>" />
		</fieldset>
	</form>
</body>
</html>
