<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Admin_Scale.master" AutoEventWireup="true" CodeFile="ChargeList.aspx.cs" Inherits="Charge_ChargeList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" runat="Server">
	<div class="panel panel-warning col-xs-10 col-xs-offset-1">
		<div class="panel-body form-horizontal">
			<table class="table text-xs">
				<thead>
					<tr>
						<th style="text-align:left;">수량</th>
						<th style="text-align:left;">지불회사코드</th>
						<th style="text-align:left;">지불회사명</th>
						<!--
						<th style="text-align:left;">청구일</th>
						<th style="text-align:left;">최근입금일</th>
						-->
						<th style="text-align:left;">입금계좌</th>
						<th style="text-align:left;">청구비용</th>
						<th style="text-align:left;">지불비용</th>
						<th style="text-align:left;">차액</th>
						<th style="text-align:left;">청구상태</th>
						<th style="text-align:left;">화물상태</th>
					</tr>
				</thead>
				<tbody id="ListBody">
					<%=Html_ChargeBody %>
				</tbody>

			</table>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" runat="Server">

	<script type="text/javascript">

		function OpenCollect(TableName, TablePk, CalcHeadPk) {
			window.open('/Charge/Dialog/CollectCharge.aspx?N=' + TableName + '&P=' + TablePk + '&H=' + CalcHeadPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, height=650px; width=650px;');
		}


	</script>

</asp:Content>

