<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Clean.master" AutoEventWireup="true" CodeFile="DebitCredit_View.aspx.cs" Inherits="Document_DebitCredit_View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" runat="Server">
	<div style="position:absolute; margin-left:720px; ">
		<input type="button" value="인쇄" id="BTN_Print" class="btn btn-sm btn-primary" />
		<input type="hidden" value="<%=Request.Params["Mode"] %>" id="Mode" />
	</div>

	<div style="background-color: white; width: 700px; padding: 10px; float: left;" id="ForPrint" >
		<% if (PageType == "Debit") { %>
			<table border="0" style="width:694px; " >
				<tr>
					<td colspan="3" style="font-size: 24px;"><%=Current.ShipperName %></td>
				</tr>
				<tr>
					<td colspan="3"><%=Current.ShipperAddress %></td>
				</tr>
				<tr>
					<td style="border-bottom: solid 3px black; width:200px; ">TEL : <%=Current.ShipperTEL %></td>
					<td style="border-bottom: solid 3px black;">FAX : <%=Current.ShipperFAX %></td>
					<td style="border-bottom: solid 3px black;">&nbsp;</td>
				</tr>
			</table>
			<br />
			<br />
			<table border="0" style="width:694px; " >
				<tr>
					<td style="width:220px; ">&nbsp;</td>
					<td style="border: solid 2px black; text-align: center; font-size: 25px;">D E B I T</td>
					<td style="width:220px; ">&nbsp;</td>
				</tr>
			</table>
			<br />
			<br />
			<table border="0" style="width:694px; " >
				<tr>
					<td colspan="3" style="border-bottom: solid 3px black; font-size: 20px;">TO : <%=Current.ConsigneeName %></td>
				</tr>
				<tr>
					<td colspan="3"><%=Current.ConsigneeAddress %></td>
				</tr>
				<tr>
					<td style="border-bottom: solid 1px black; width:200px; ">TEL : <%=Current.ConsigneeTEL %></td>
					<td style="border-bottom: solid 1px black;">FAX : <%=Current.ConsigneeFAX %></td>
					<td style="border-bottom: solid 1px black;">&nbsp;</td>
				</tr>
			</table>

		<% } else {%>
			<table border="0" style="width:694px; " >
				<tr>
					<td colspan="3" style="font-size: 24px;"><%=Current.ConsigneeName %></td>
				</tr>
				<tr>
					<td colspan="3"><%=Current.ConsigneeAddress %></td>
				</tr>
				<tr>
					<td style="border-bottom: solid 3px black; width:200px; ">TEL : <%=Current.ConsigneeTEL %></td>
					<td style="border-bottom: solid 3px black;">FAX : <%=Current.ConsigneeFAX %></td>
					<td style="border-bottom: solid 3px black;">&nbsp;</td>
				</tr>
			</table>
			<br />
			<br />
			<table border="0" style="width:694px; " >
				<tr>
					<td style="width:220px; ">&nbsp;</td>
					<td style="border: solid 2px black; text-align: center; font-size: 25px;">C R E D I T</td>
					<td style="width:220px; ">&nbsp;</td>
				</tr>
			</table>
			<br />
			<br />
			<table border="0" style="width:694px; " >
				<tr>
					<td colspan="3" style="border-bottom: solid 3px black; font-size: 20px;">TO : <%=Current.ShipperName %></td>
				</tr>
				<tr>
					<td colspan="3"><%=Current.ShipperAddress %></td>
				</tr>
				<tr>
					<td style="border-bottom: solid 1px black; width:200px; ">TEL : <%=Current.ShipperTEL %></td>
					<td style="border-bottom: solid 1px black;">FAX : <%=Current.ShipperFAX %></td>
					<td style="border-bottom: solid 1px black;">&nbsp;</td>
				</tr>
			</table>

		<% } %>


		<br />
		<table border="0" style="width:634px; margin:0 auto; " >
			<tr>
				<td>&nbsp;&nbsp;&nbsp;Vessel Name :  <%=Current.VesselName %></td>
				<td>&nbsp;&nbsp;&nbsp;Issue Date : <%=Current.IssueDate %></td>
			</tr>
			<tr>
				<td>&nbsp;&nbsp;&nbsp;Container : <%=Current.Container %></td>
				<td>&nbsp;&nbsp;&nbsp;E.T.D : <%=Current.ETD %></td>
			</tr>
			<tr>
				<td>&nbsp;&nbsp;&nbsp;Q'TY : <%=Current.Quantity %> CT</td>
				<td>&nbsp;&nbsp;&nbsp;E.T.A : <%=Current.ETA %></td>
			</tr>
			<tr>
				<td>&nbsp;&nbsp;&nbsp;Weight : <%=Current.Weight %> KGS</td>
				<td>&nbsp;&nbsp;&nbsp;P.O.L : <%=Current.POL %></td>
			</tr>
			<tr>
				<td>&nbsp;&nbsp;&nbsp;Measurment : <%=Current.Measurment %> CBM</td>
				<td>&nbsp;&nbsp;&nbsp;P.O.D : <%=Current.POD %></td>
			</tr>
		</table>
		<br />
		<table border="0" style="width:634px; margin:0 auto; " >
			<tr>
				<td colspan="3" style="border: solid 1px black; text-align: center; font-size: 15px; font-weight: bold;">CONTENTS</td>
				<td style="border-right: solid 1px black; border-top: solid 1px black; border-bottom: solid 1px black; text-align: center; font-size: 15px; font-weight: bold;">PREPAID</td>
				<td style="border-right: solid 1px black; border-top: solid 1px black; border-bottom: solid 1px black; text-align: center; font-size: 15px; font-weight: bold;">COLLECT</td>
			</tr>
			<%=Html_InnerPrice %>
		</table>
	</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" runat="Server">
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