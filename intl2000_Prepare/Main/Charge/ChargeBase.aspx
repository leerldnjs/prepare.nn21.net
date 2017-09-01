<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Admin_Scale.master" AutoEventWireup="true" CodeFile="ChargeBase.aspx.cs" Inherits="Charge_ChargeBase" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" runat="Server">

	<input type="hidden" id="NewChargeNo" value="" />

	<div class="row">
		<div class="col-xs-2 col-xs-offset-3">
			<input type="text" class="form-control" id="Table_Name" value="TRANSPORT_HEAD" />
		</div>
		<div class="col-xs-2">
			<input type="text" class="form-control" id="Table_Pk" value="1581" />
		</div>
		<div class="col-xs-2">
			<input type="text" class="form-control" id="Type" value="Transport" />
		</div>
	</div>
	<div class="row" style="margin-top:10px;">
		<div class="col-xs-1 col-xs-offset-3">
			<input type="button" class="btn btn-default btn-sm form-control" id="Btn_Retrieve" value="조회" />
		</div>
		<div class="col-xs-1">
			<input type="button" class="btn btn-default btn-sm form-control" id="Btn_AddTransportCharge" value="매입비용 추가" onclick="PopCharge('N', $('#NewChargeNo').val())" />
		</div>
		<div class="col-xs-1">
			<input type="button" class="btn btn-default btn-sm form-control" id="Btn_ModTransportCharge0" value="매입비용 수정_0" onclick="PopCharge('Y', '0')" />
		</div>
		<div class="col-xs-1">
			<input type="button" class="btn btn-default btn-sm form-control" id="Btn_ModTransportCharge1" value="매입비용 수정_1" onclick="PopCharge('Y', '1')" />
		</div>
		<div class="col-xs-1">
			<input type="button" class="btn btn-default btn-sm form-control" id="Btn_ModTransportCharge2" value="매입비용 수정_2" onclick="PopCharge('Y', '2')" />
		</div>
		<div class="col-xs-1">
			<input type="button" class="btn btn-default btn-sm form-control" id="Btn_ModTransportCharge3" value="매입비용 수정_3" onclick="PopCharge('Y', '3')" />
		</div>
		<div class="col-xs-1">
			<input type="button" class="btn btn-default btn-sm form-control" id="Btn_ModTransportCharge4" value="매입비용 수정_4" onclick="PopCharge('Y', '4')" />
		</div>
	</div>

	<div class="row" id="CollectList" style="margin-top:100px;">

	</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" runat="Server">
	<script type="text/javascript">
		$(document).ready(function () {
			$("#Btn_Retrieve").on("click", Load_CollectList);

			Load_CollectList();
		});

		function Load_CollectList() {
			var data = {
				TableName: $("#Table_Name").val(),
				TablePk: $("#Table_Pk").val(),
				Type: $("#Type").val()
			}

			$.ajax({
				type: "POST",
				url: "/Charge/ChargeBase.aspx/LoadCollectList",
				data: JSON.stringify(data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					if (result.d[0] == "") {
						$("#NewChargeNo").val("0");
					}
					else {
						$("#NewChargeNo").val(parseInt(result.d[0]) + 1);
					}
					$("#CollectList").html(result.d[1]);
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});
		}

		function PopCharge(AlreadyCalc, ChargeNo) {
			window.open("/Charge/Dialog/TransportCharge.aspx?S=" + $("#Table_Pk").val() + "&A=" + AlreadyCalc + "&N=" + ChargeNo, "popupWindow", "width=800px, height=650px, scrollbars=yes");
		}

		function OpenCollect(TableName, TablePk, CalcHeadPk) {
			window.open('/Charge/Dialog/CollectCharge.aspx?N=' + TableName + '&P=' + TablePk + '&H=' + CalcHeadPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, height=650px; width=650px;');
		}
		
	</script>
</asp:Content>

