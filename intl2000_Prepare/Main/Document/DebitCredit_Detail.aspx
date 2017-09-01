<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Clean.master" AutoEventWireup="true" CodeFile="DebitCredit_Detail.aspx.cs" Inherits="Document_DebitCredit_Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<link href="/Lib/jquery-ui.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" Runat="Server">
<div class="col-xs-12 small">
	<div class="panel panel-default" id="PnTarget">
		<div class="panel-heading" id="PnTargetHeading" style="font-weight:bold;">Debit Credit</div>
		<div class="panel-body form-horizontal">
			<form id="Form_Document">

			<div class="form-group">
				<label class="col-xs-1 control-label" style="text-align:right;">수출자</label>
				<div class="col-xs-4">
					<input type="hidden" name="DocumentPk" id="DocumentPk" value="<%=Current.DocumentPk %>" />				
					<input type="hidden" name="TBBHPk" value="<%=Current.TBBHPk %>" />				
					<input type="hidden" name="Status" value="<%=Current.Status %>" />
					<input type="text" class="form-control" name="ShipperName" id="ShipperName" value="<%=Current.ShipperName %>" />
				</div>
				<div class="col-xs-1">
					<input type="button" id="BTN_ModalOpen_ChooseShipper" class="btn btn-success btn-sm" value="선택" />
				</div>
				<div class="col-xs-5">
					<label class="col-xs-2 control-label" style="text-align:right;">TEL</label>
					<div class="col-xs-4">
						<input type="text" class="form-control" name="ShipperTEL" id="ShipperTEL" value="<%=Current.ShipperTEL %>" />
					</div>
					<label class="col-xs-2 control-label" style="text-align:right;">FAX</label>
					<div class="col-xs-4">
						<input type="text" class="form-control" name="ShipperFAX" id="ShipperFAX" value="<%=Current.ShipperFAX %>" />
					</div>

				</div>
			</div>
			<div class="form-group">
				<label class="col-xs-1 control-label" style="text-align:right;">&nbsp;</label>
				<div class="col-xs-10">
					<input type="text" class="form-control" name="ShipperAddress" id="ShipperAddress" value="<%=Current.ShipperAddress %>" />
				</div>
			</div>

			<div class="form-group">
				<label class="col-xs-1 control-label" style="text-align:right;">수입자</label>
				<div class="col-xs-4">
					<input type="text" class="form-control" name="ConsigneeName" id="ConsigneeName" value="<%=Current.ConsigneeName %>" />
				</div>
				<div class="col-xs-1">
					<input type="button" id="BTN_ModalOpen_ChooseConsignee" class="btn btn-success btn-sm" value="선택" />
				</div>

				<div class="col-xs-5">
					<label class="col-xs-2 control-label" style="text-align:right;">TEL</label>
					<div class="col-xs-4">
						<input type="text" class="form-control" name="ConsigneeTEL" id="ConsigneeTEL" value="<%=Current.ConsigneeTEL %>" />
					</div>
					<label class="col-xs-2 control-label" style="text-align:right;">FAX</label>
					<div class="col-xs-4">
						<input type="text" class="form-control" name="ConsigneeFAX" id="ConsigneeFAX" value="<%=Current.ConsigneeFAX %>" />
					</div>
				</div>
			</div>
			<div class="form-group">
				<label class="col-xs-1 control-label" style="text-align:right;">&nbsp;</label>
				<div class="col-xs-10">
					<input type="text" class="form-control" name="ConsigneeAddress" id="ConsigneeAddress" value="<%=Current.ConsigneeAddress %>" />
				</div>
			</div>
			<hr />
			<div class="form-group">
				<label class="col-xs-2 control-label" style="text-align:right;">Vessel Name</label>
				<div class="col-xs-3">
					<input type="text" class="form-control" name="VesselName" id="VesselName" value="<%=Current.VesselName %>" />
				</div>
				<label class="col-xs-2 control-label" style="text-align:right;">Issue Date</label>
				<div class="col-xs-3">
					<input type="text" class="form-control" name="IssueDate" id="IssueDate" value="<%=Current.IssueDate %>" />
				</div>
			</div>
			<div class="form-group">
				<label class="col-xs-2 control-label" style="text-align:right;">Container</label>
				<div class="col-xs-3">
					<input type="text" class="form-control" name="Container" id="Container" value="<%=Current.Container %>" />
				</div>
				<label class="col-xs-2 control-label" style="text-align:right;">E.T.D</label>
				<div class="col-xs-3">
					<input type="text" class="form-control" name="ETD" id="ETD" value="<%=Current.ETD %>" />
				</div>
			</div>
			<div class="form-group">
				<label class="col-xs-2 control-label" style="text-align:right;">Q'TY</label>
				<div class="col-xs-3">
					<input type="text" class="form-control" name="Quantity" id="Quantity" value="<%=Components.Common.NumberFormat(Current.Quantity) %>" />
				</div>
				<label class="col-xs-2 control-label" style="text-align:right;">E.T.A</label>
				<div class="col-xs-3">
					<input type="text" class="form-control" name="ETA" id="ETA" value="<%=Current.ETA %>" />
				</div>
			</div>
			<div class="form-group">
				<label class="col-xs-2 control-label" style="text-align:right;">Weight</label>
				<div class="col-xs-3">
					<input type="text" class="form-control" name="Weight" id="Weight" value="<%=Components.Common.NumberFormat(Current.Weight) %>" />
				</div>
				<label class="col-xs-2 control-label" style="text-align:right;">P.O.L</label>
				<div class="col-xs-3">
					<input type="text" class="form-control" name="POL" id="POL" value="<%=Current.POL %>" />
				</div>
			</div>
			<div class="form-group">
				<label class="col-xs-2 control-label" style="text-align:right;">Measurment</label>
				<div class="col-xs-3">
					<input type="text" class="form-control" name="Measurment" id="Measurment" value="<%=Components.Common.NumberFormat(Current.Measurment) %>" />
				</div>
				<label class="col-xs-2 control-label" style="text-align:right;">P.O.D</label>
				<div class="col-xs-3">
					<input type="text" class="form-control" name="POD" id="POD" value="<%=Current.POD %>" />
				</div>
			</div>

			<hr />
			<table class="table">
				<thead>
					<tr>
						<th colspan="2" class="text-center">Contents</th>
						<th class="text-center">Prepaid</th>
						<th class="text-center">Collect</th>
						<th class="text-center">&nbsp;</th>
					</tr>
				</thead>
				<%=Html_InnerPrice %>
				<tfoot>
					<tr>
						<th colspan="3" class="text-right" style="background-color:#eaeef1; padding-top:6px; padding-right:6px;  " >TOTAL</th>
						<th>
							<input type="text" class="form-control text-center" name="InnerPrice_Total" />
						</th>
						<th style="background-color:#eaeef1; padding-top:6px; padding-right:6px;  " >&nbsp;</th>
					</tr>

				</tfoot>
			</table>
			</form>

			<div class="col-xs-3 col-xs-offset-3">
				<button class="btn btn-primary btn-md btn-block" id="BTN_SetSave_Document" type="button" >Save</button>
			</div>
			<div class="col-xs-2">
				<button class="btn btn-danger btn-md btn-block" id="BTN_SetDelete_Document" type="button" >Delete</button>
			</div>
			<div class="col-xs-2 ">
				<button class="btn btn-warning btn-md btn-block" type="button" >Cancel</button>
			</div>

		</div>

	</div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" Runat="Server">
	<script src="/Lib/jquery-ui.js"></script>
	<script src="/Lib/ForModal.js"></script>
	<script type="text/javascript">
		jQuery(document).ready(function () {
			$("#IssueDate").datepicker({
				dateFormat: "yy/mm/dd"
			});
			$("#ETA").datepicker({
				dateFormat: "yy/mm/dd"
			});
			$("#ETD").datepicker({
				dateFormat: "yy/mm/dd"
			});
			$("#BTN_SetSave_Document").on("click", SetSave_Document);
			$("#BTN_SetDelete_Document").on("click", SetDelete_Document);

			ForModal.Init("md", "ModalChoose_DebitCredit");
			$("#BTN_ModalOpen_ChooseShipper").on("click", function () {
				ForModal.Open("ModalChoose_DebitCredit", "S");
			});
			$("#BTN_ModalOpen_ChooseConsignee").on("click", function () {
				ForModal.Open("ModalChoose_DebitCredit", "C");
			});

		});
		function SetDelete_Document() {
			$.ajax({
				type: "POST",
				url: "/Process/DocumentP.asmx/SetDelete_Document",
				data: "{DocumentPk:\"" + $("#DocumentPk").val() + "\"}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (result) {
					if (result.d == "1") {
						alert("성공");
						window.close();
						opener.location.reload();
					}
				},
				error: function (msg) {
					alert('failure : ' + msg);
					console.log(msg);
				}
			});
		}
		function ChooseCompany(SorC, Title, Address, TEL, FAX) {
			if (SorC == "S") {
				$("#ShipperName").val(Title);
				$("#ShipperTEL").val(TEL);
				$("#ShipperFAX").val(FAX);
				$("#ShipperAddress").val(Address);
			} else {
				$("#ConsigneeName").val(Title);
				$("#ConsigneeTEL").val(TEL);
				$("#ConsigneeFAX").val(FAX);
				$("#ConsigneeAddress").val(Address);
			}
			$("#ModalChoose_DebitCredit").modal("hide");
		}
		function SetSave_Document() {
			var datas = $("#Form_Document").serialize();
			$.ajax({
				type: "POST",
				url: "/Process/DocumentP.asmx/SetSave_Document",
				data: "{ValueSum:\"" + datas + "\"}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (result) {
					if (result.d == "1") {
						alert("성공");
						window.close();
					}
				},
				error: function (msg) {
					alert('failure : ' + msg);
					console.log(msg);
				}
			});
		}
		function DeleteRow(rowcount) {
			$("#InnerPrice_" + rowcount + "_BLNo").val("");
			$("#InnerPrice_" + rowcount + "_Title").val("");
			$("#InnerPrice_" + rowcount + "_Collect").val("");
		}
	</script>
</asp:Content>

