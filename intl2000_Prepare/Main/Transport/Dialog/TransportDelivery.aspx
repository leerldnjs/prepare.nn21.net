<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Clean.Master" AutoEventWireup="true" CodeFile="TransportDelivery.aspx.cs" Inherits="Transport_Dialog_TransportDelivery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" Runat="Server">

	<div class="col-xs-12">
		<div class="panel panel-default">

			<div class="panel-body">

				<div class="col-xs-12">
					<section class="panel b-a">
						<div class="panel-heading" style="font-weight: bold;">Company Warehouse</div>
						<div class="panel-body" id="SavedWarehouse">
							<%=Html_CompanyWarehouse %>
						</div>
					</section>
				</div>

				<div class="col-xs-6">
					<section class="panel b-a">
						<div class="panel-heading" style="font-weight: bold;">Delivery Info</div>
						<div class="panel-body form-horizontal" id="DeliveryWarehouse">
							<%=Html_DeliveryWarehouse %>
						</div>
					</section>
				</div>

				<div class="col-xs-6">
					<section class="panel b-a">
						<div class="panel-heading" style="font-weight: bold;">Car Info</div>
						<div class="panel-body form-horizontal" id="DeliveryCar">
							<%=Html_DeliveryCar %>
						</div>
					</section>
				</div>

			</div>
			<div class="panel-footer col-xs-12">
				<div class="col-xs-2 col-xs-offset-3">
					<input type="button" class="btn btn-primary btn-sm form-control" id="Submit" value="Submit" />
				</div>
				<div class="col-xs-2">
					<input type="button" class="btn btn-warning btn-sm form-control" value="Close" />
				</div>
				<div class="col-xs-2">
					<input type="button" class="btn btn-default btn-sm form-control" value="Print" />
				</div>
			</div>

		</div>
	</div>

	<input type="hidden" id="TransportHeadPk" value="<%=TransportPk %>" />
	<input type="hidden" id="RequestPk" value="<%=RequestPk %>" />
	<input type="hidden" id="ConsigneePk" value="<%=ConsigneePk %>" />
	<input type="hidden" id="StorageORBodyPk" value="<%=StorageORBodyPk %>" />
	<input type="hidden" id="DeliveryHeadPk" value="<%=DeliveryHeadPk %>" />
	<input type="hidden" id="DeliveryBodyPk" value="<%=DeliveryBodyPk %>" />
	<input type="hidden" id="PurchaseHeadPk" value="<%=PurchaseHeadPk %>" />
	<input type="hidden" id="PurchaseBodyPk" value="<%=PurchaseBodyPk %>" />
	<input type="hidden" id="DepatureBranchPk" value="<%=DepatureBranchPk %>" />
	<input type="hidden" id="DepatureWarehousePk" value="<%=DepatureWarehousePk %>" />
	<input type="hidden" id="ArrivalWarehousePk" value="<%=ArrivalWarehousePk %>" />


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" Runat="Server">
	<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
	<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

	<script type="text/javascript">
		$(document).ready(function () {
			$("#DepatureDate").datepicker({
				dateFormat: "yymmdd"
			});
			$("#ArrivalDate").datepicker({
				dateFormat: "yymmdd"
			});

			$("#Submit").on("click", SubmitDelivery);
		});

		function SelectWarehouse(Row) {
			var WarehousePk = $("#WarehouseTable tr:eq(" + Row + ") td:eq(0)").text();
			var Staff = $("#WarehouseTable tr:eq(" + Row + ") td:eq(4)").text();
			var Mobile = $("#WarehouseTable tr:eq(" + Row + ") td:eq(5)").text();
			var Address = $("#WarehouseTable tr:eq(" + Row + ") td:eq(1)").text() + " : " + $("#WarehouseTable tr:eq(" + Row + ") td:eq(2)").text();
			var Tel = $("#WarehouseTable tr:eq(" + Row + ") td:eq(3)").text();

			$("#ArrivalWarehousePk").val(WarehousePk);
			$("#Staff").val(Staff);
			$("#WarehouseMobile").val(Mobile);
			$("#Address").val(Address);
			$("#WarehouseTel").val(Tel);
		}

		function SelectCar() {
			//$(opener.document).find("#TEST").val("value");
		}

		function SubmitDelivery() {
			var data = {
				TransportHeadPk: $("#TransportHeadPk").val(),
				TransportStatus: "5", 
				BranchPk_From: $("#DepatureBranchPk").val(),
				CompanyPk_To: $("#ConsigneePk").val(), 
				WarehousePk_Arrival: $("#DepatureWarehousePk").val(), 
				Area_From: $("#DepatureWarehousePk").val(), 
				Area_To: $("#ArrivalWarehousePk").val(), 
				Datetime_From: $("#DepatureDate").val(), 
				Datetime_To: $("#ArrivalDate").val(), 
				Title: $("#CarTitle").val(), 
				VesselName: $("#DriverName").val(), 
				Voyage_No: $("#CarType").val(), 
				Value_String_1: $("#DriverMobile").val(), 
				Value_String_2: $("#CarTel").val(), 
				Value_String_3: $("#CarSize").val(),
				StoragePk: $("#StorageORBodyPk").val(),
				DeliveryHeadPk: $("#DeliveryHeadPk").val(), 
				DeliveryBodyPk: $("#DeliveryBodyPk").val(), 
				DeliveryPrice: $("#DeliveryPrice").val(), 
				PurchaseHeadPk: $("#PurchaseHeadPk").val(), 
				PurchaseBodyPk: $("#PurchaseBodyPk").val(), 
				PurchasePrice: $("#PurchasePrice").val()
			}
			$.ajax({
				type: "POST",
				url: "/WebService/TransportP.asmx/Set_TransportDelivery",
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

	</script>
</asp:Content>

