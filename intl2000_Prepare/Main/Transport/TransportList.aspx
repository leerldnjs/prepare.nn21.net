<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Admin_Scale.master" AutoEventWireup="true" CodeFile="TransportList.aspx.cs" Inherits="Transport_TransportList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" Runat="Server">

	<input type="hidden" id="HAccountID" value="<%=AccountID %>" />
	<input type="hidden" id="HTotalPage" value="<%=TotalPage %>" />

	<div class="panel panel-warning col-xs-10 col-xs-offset-1">
		<div class="panel-body form-horizontal">
			<table class="table table-hover b-t b-light">
				<thead>
					<tr>
						<th style="text-align: left">Description</th>
						<th style="text-align: left">Title</th>
						<th style="text-align: left">Date</th>
						<th style="text-align: left">Area</th>
						<th><select id="st_Storage" class="form-control"><%=Html_SelectStorage %></select></th>
						<th></th>
					</tr>
				</thead>

				<tbody id="TransportList">
				</tbody>

			</table>
			 <ul id="Paging" class="pagination-sm"></ul>
		</div>
	</div>

	<form name="parameter">
		<input type="hidden" id="HeadPk" name="HeadPk" value=""/>
	</form>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" Runat="Server">
	<script src="/Common/jquery.twbsPagination.js"></script>
	<script type="text/javascript">

		$(document).ready(function () {
			$('#Paging').twbsPagination({
				totalPages: $("#HTotalPage").val(),
				visiblePages: 5,
				onPageClick: function (event, page) {
					var data = {
						PageNo: page
					}
					$.ajax({
						type: "POST",
						url: "/Transport/TransportList.aspx/MakeHtml_TransportList",
						data: JSON.stringify(data),
						dataType: "json",
						contentType: "application/json; charset=utf-8",
						success: function (result) {
							$("#TransportList").html(result.d);
						},
						error: function (result) {
							alert('failure : ' + result);
						}
					});
				}
			});

		});

		function GoView(HeadPk) {
			$("#HeadPk").val(HeadPk);

			//document.parameter.action = "/Transport/TransportView.aspx";
			//document.parameter.method = "post";
			//document.parameter.submit();
			location.href = "/Transport/TransportView.aspx?S=" + HeadPk;
		}

		function GoExcelDown(BBHPk) {
			var AccountID = $("#HAccountID").val();
			if (AccountID == "ilyt1" || AccountID == "ilyt2" || AccountID == "ilyt3" || AccountID == "ilyt4" || AccountID == "ilyt0" || AccountID == "ilic31") {
				location.href = "/UploadedFiles/FileDownloadWithExcel.aspx?S=" + BBHPk + "&G=YT_Clearance";
			} else {
				location.href = "/UploadedFiles/FileDownloadWithExcel.aspx?S=" + BBHPk;
			}
		}
		function GoAbjiDown(date, branchPk, InorOut) { location.href = "../UploadedFiles/FileDownloadWithExcel.aspx?G=TodayAbji&S=" + branchPk + "&D=" + date + "&G2=" + InorOut; }
		function GoAbjiDown2(date, branchPk, InorOut, Type) { location.href = "../UploadedFiles/FileDownloadWithExcel.aspx?G=TodayAbjiForJaemu&S=" + branchPk + "&D=" + date + "&G2=" + InorOut + "&G3=" + Type; }

	</script>

</asp:Content>

