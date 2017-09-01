<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TalkBusinessforRelatedCompany.aspx.cs" Inherits="Admin_Dialog_TalkBusinessforRelatedCompany" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
   <style type="text/css">
		.tdSubT	{border-bottom:solid 2px #93A9B8;	padding-top:10px; }
      </style>
   <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.1/themes/base/jquery-ui.css" />
  <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
  <script src="http://code.jquery.com/ui/1.10.1/jquery-ui.js"></script>
   <link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
	   function InsertContents() {
	      var CheckSum = form1.HCompanyPk.value+"!";
	      $("#checktd").find("input[type=checkbox]").each(function () {
	         if ($(this).is(":checked")) {
	            CheckSum += $(this).val() + "!";
	         }
	      });
	      
	      Admin.InsertTalkBusiness_RelatedCompany(CheckSum, opener.document.getElementById("HAccountID").value, form1.TBContents.value, "Basic_Important", function (result) {
	         if (result == "1") {
	            alert("SUCCESS");
	            window.close();
	            opener.location.reload();
	         }
		  }, function (result) { return false; });

	   }
	   </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
	<div>
		<table>
			<tr style="height:30px;" >
				<td bgcolor="#F5F5F5" height="20" align="center" width="580" >Main</td>
			</tr>
			<%=Html %>
			<tr>
				<td class="ItemTableIn" >
					<textarea id="TBContents" rows="5" cols="82" ></textarea>
					<input type="hidden" id="HCompanyPk" value="<%=Request.Params["S"] %>" />
					&nbsp;<input type="button" value="Write" onclick="InsertContents();" />
				</td>
			</tr>
		</table>
	</div>
    </form>
</body>
</html>
