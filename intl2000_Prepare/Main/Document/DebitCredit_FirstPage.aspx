<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Clean.master" AutoEventWireup="true" CodeFile="DebitCredit_FirstPage.aspx.cs" Inherits="Document_DebitCredit_FirstPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" Runat="Server">
	<div style="position:absolute; margin-left:720px; ">
		<input type="button" value="인쇄" id="BTN_Print" class="btn btn-sm btn-primary" />
		<input type="hidden" value="<%=Request.Params["Mode"] %>" id="Mode" />
	</div>

	<div style="background-color: white; width: 700px; padding: 10px; float: left;" id="ForPrint" >
		<table border="0" style="width:694px; " >
			<tr>
				<td></td>
			</tr>
		</table>
		<%=Html_List %>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" Runat="Server">
	<script type="text/javascript">
		jQuery(document).ready(function () {
			$("#BTN_Print").on("click", RunPrint);
			if ($("#Mode").val() == "Print") {
				RunPrint();
			}
		});
		function RunPrint() {
			var initBody;
			window.onbeforeprint = function () {
				initBody = document.body.innerHTML;
				document.body.innerHTML = document.getElementById("ForPrint").innerHTML;
			};
			window.onafterprint = function () {
				document.body.innerHTML = initBody;
			};
			window.print();
			return false;
		}

	</script>
</asp:Content>