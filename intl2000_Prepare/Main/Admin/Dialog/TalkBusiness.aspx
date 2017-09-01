<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TalkBusiness.aspx.cs" Inherits="Admin_Dialog_TalkBusiness" Debug="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   	<style type="text/css">
		.tdSubT	{border-bottom:solid 2px #93A9B8;	padding-top:10px; }
		
	</style>
	<link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script src="../../Common/jquery-1.10.2.min.js"></script>
	<script type="text/javascript">

		function InsertContents() {
			var Gubun = 'Basic'
			if (form1.HGubunCL.value == "F") { Gubun = "Basic_F"; }
			var data = {
				Table_Name: "Company",
				Table_Pk: form1.HCompanyPk.value,
				Category: Gubun,
				Contents: form1.TBContents.value,
				Account_Id: opener.document.getElementById("HAccountID").value
			}
			$.ajax({
				type: "POST",
				url: "/Process/HistoryP.asmx/Set_Comment",
				data: JSON.stringify(data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					alert("성공");
					location.reload();
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});
		}

		function SetGubunCL(TalkBusinessPk, GubunCL) {
			var Gubun = "";
			if (GubunCL == "Basic") {
				Gubun = "Basic_Important";
			}
			else {
				Gubun = "Basic";
			}
			var data = {
				Comment_Pk: TalkBusinessPk,
				Category: Gubun
			}
			$.ajax({
				type: "POST",
				url: "/Process/HistoryP.asmx/ChangeCategory_FromComment",
				data: JSON.stringify(data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					alert("성공");
					location.reload();
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});
		}

		function CommentDelete(TalkBusinessPk) {
			if (confirm("this comment will be deleted")) {
				var data = {
					Comment_Pk: TalkBusinessPk
				}
				$.ajax({
					type: "POST",
					url: "/Process/HistoryP.asmx/Delete_Comment",
					data: JSON.stringify(data),
					dataType: "json",
					contentType: "application/json; charset=utf-8",
					success: function (result) {
						alert("성공");
						location.reload();
					},
					error: function (result) {
						alert('failure : ' + result);
					}
				});
			}
		}
	</script>
</head>
<body>
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
	<div>
		<table>
			<tr style="height:30px;" >
				<td bgcolor="#F5F5F5" height="20" align="center" width="60" >Write ID</td>
				<td bgcolor="#F5F5F5" align="center" width="520" >Comment</td>
				<td bgcolor="#F5F5F5" align="center" width="20" >I</td>

			</tr>
			<%=TalkList %>
			<tr>
				<td class="ItemTableIn" align="center" colspan="2" >
					<textarea id="TBContents" rows="5" cols="82" ></textarea>
					<input type="hidden" id="HCompanyPk" value="<%=Request.Params["S"] %>" />
					<input type="hidden" id="HGubunCL" value="<%=Request.Params["G"] %>" />
					&nbsp;<input type="button" value="등록" onclick="InsertContents();" />
				</td>
			</tr>
		</table>
	</div>
    </form>
</body>
</html>