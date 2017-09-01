<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Branch.master" AutoEventWireup="true" CodeFile="YuhanViewN.aspx.cs" Inherits="CustomClearance_YuhanViewN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<link href="../Lib/jquery-ui.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" runat="Server">
	<div class="col-sm-12">
		<section class="vbox scrollable wrapper" style="height: 288px;">
			<div class="panel panel-default" style="margin-right: 20px;">
				<div class="panel-heading" style="padding-top: 3px; padding-bottom: 3px;">
					<h5><strong>적화전송</strong></h5>
				</div>
				<div class="panel-body">
					<form id="PnMasterBL" onsubmit="return false; ">
						<div class="row" style="padding-right: 80px;">
							<div class="col-sm-12">
								<div class="col-sm-6">
									<div class="form-horizontal" role="form">
										<div class="form-group">
											<label class="col-sm-3 control-label">Master B/L No</label>
											<div class="col-sm-4">
												<input type="text" class="form-control" id="MasterBLNo" name="MasterBLNo" value="<%=Current.MasterBLNo %>" />
											</div>
											<label class="col-sm-2 control-label">BranchType</label>
											<div class="col-sm-3">
												<select name="FlagAACO" id="FlagAACO" class="form-control">
													<option value="INTL">INTL</option>
													<option value="AACO">AACO</option>
												</select>
											</div>
										</div>
										<div class="form-group">
											<label class="col-sm-3 control-label">MRN / MSN</label>
											<div class="col-sm-4">
												<input type="text" class="form-control" id="MRN" name="MRN" value="<%=Current.MRN  %>" />
											</div>
											<div class="col-sm-2">
												<input type="text" class="form-control" id="MSN" name="MSN" value="<%=Current.MSN %>" />
											</div>
										</div>
										<div class="form-group">
											<label class="col-sm-3 control-label">Pre-Carriage</label>
											<div class="col-sm-2">
												<input type="text" class="form-control" id="PreCarriageCode" name="PreCarriageCode" style="padding-left: 7px;" value="<%=Current.LineCode%>" />
											</div>
											<div class="col-sm-7">
												<input type="text" class="form-control" id="PreCarriageName" name="PreCarriageName" readonly="readonly" />
											</div>
										</div>
										<div class="form-group">
											<label class="col-sm-3 control-label">Vessel Name</label>
											<div class="col-sm-4">
												<input type="text" class="form-control" id="VesselName" name="VesselName" value="<%=Current.ShipName %>" />
											</div>
											<div class="col-sm-1">
											</div>
											<label class="col-sm-2 control-label">Voyage No</label>
											<div class="col-sm-2">
												<input type="text" class="form-control" id="VoyageNo" name="VoyageNo" value="<%=Current.VoyageNo %>" />
											</div>
										</div>
										<div class="form-group">
											<label class="col-sm-3 control-label">Final Destina</label>
											<div class="col-sm-2">
												<input type="text" class="form-control" id="FinalDate" name="FinalDate" style="padding: 0px; text-align: center;" value="<%=Current.FinalDate%>" />
											</div>
											<div class="col-sm-2">
												<input type="text" class="form-control" id="FinalPortCode" name="FinalPortCode" style="padding-left: 7px;" value="<%=Current.FinalPort%>" />
											</div>
											<div class="col-sm-5">
												<input type="text" class="form-control" id="FinalPortName" name="FinalPortName" readonly="readonly" />
											</div>
										</div>
										<br />
										<div class="form-group">
											<label class="col-sm-3 control-label" style="padding-top: 7px; text-align: right;">Container No</label>
											<div class="col-sm-4">
												<input type="text" class="form-control" id="ContainerNo" name="ContainerNo" value="<%=Current.Container.ContainerNo %>" />
											</div>
											<label class="col-sm-2 control-label" style="padding-top: 7px; text-align: right;">Seal No</label>
											<div class="col-sm-3">
												<input type="text" class="form-control" id="SealNo" name="SealNo" value="<%=Current.Container.SealNo1%>" />
												<input type="hidden" id="HContainerSize" value="<%=Current.Container.ContainerCode%>" />
											</div>
										</div>
										<div class="form-group">
											<label class="col-sm-3 control-label" style="padding-top: 7px; text-align: right;">Type</label>
											<div class="col-sm-4">
												<select name="ContainerType" id="ContainerType" class="form-control">
													<option>Container Type</option>
													<option value='45GP'>45GP</option>
													<option value='44GP'>44GP 40F JUMBO(HIGH CUBE)</option>
													<option value='42GP'>42GP 40F DRY</option>
													<option value='22GP'>22GP 20F DRY</option>
												</select>
											</div>
										</div>
										<div class="form-group">
											<label class="col-sm-3 control-label" style="padding-top: 7px; text-align: right;">Warehouse</label>
											<div class="col-sm-2">
												<input type="text" class="form-control" id="WarehouseCode" name="WarehouseCode" value="<%=Current.AssignmentWH %>" />
											</div>
											<div class="col-sm-5">
												<input type="text" class="form-control" readonly="readonly" id="WarehouseName" name="WarehouseName" />
											</div>
										</div>
										<div class="form-group">
											<label class="col-sm-3 control-label" style="padding-top: 7px; text-align: right;">Customs/Division</label>
											<div class="col-sm-1">
												<input type="text" class="form-control" id="Customs" name="Customs" value="<%=Current.Customs%>" />
											</div>
											<div class="col-sm-3">
												<input type="text" class="form-control" id="Division" name="Division" value="<%=Current.Division%>" />
											</div>
										</div>
										<div class="form-group">
											<label class="col-sm-3 control-label" style="padding-top: 7px; text-align: right;">Assignment</label>
											<div class="col-sm-1">
												<input type="text" class="form-control" id="AssignmentCode" name="AssignmentCode" value="<%=Current.AssignmentCode %>" />
											</div>
											<div class="col-sm-6">
												<input type="text" class="form-control" readonly="readonly" id="AssignmentName" name="AssignmentName" value="<%=Current.AssignmentName%>" />
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
						<br />
						<div class="table-responsive" style="padding: 10px;">
							<div style="text-align: right;">
								<input type="button" class="btn btn-primary btn-sm" id="AddHouseBL" value="Add HouseBL" />
							</div>
							<div id="SavedHouseBL" style="display: none;"><%=Saved_HouseBL %></div>
							<table class="table">
								<thead>
									<tr>
										<th style="width: 30px; text-align: center;">#</th>
										<th style="text-align: center;">HouseBL / CT / Volume / Weight</th>
										<th style="text-align: center; width: 350px;">S</th>
										<th style="text-align: center; width: 350px;">C</th>
									</tr>
								</thead>
								<tbody id="PnHouseBL"></tbody>
								<tfoot>
									<tr>
										<td colspan="4">
											<div class="form-group">
												<div class="col-sm-1" style="font-weight: bold; padding-top: 6px;">
													<span id="BTN_ReOrderHSN">HSN 정렬</span>
												</div>
												<div class="col-sm-3" style="text-align: right; font-weight: bold; padding-top: 6px;">
													<span id="BTN_Calc">TOTAL</span>
												</div>
												<div class="col-sm-2">
													<input type="text" class="form-control" id="TotalPackedCount" name="TotalPackedCount" style="padding: 3px; text-align: center;" />
												</div>
												<div class="col-sm-1">
													<input type="text" class="form-control" id="TotalPackingUnit" name="TotalPackingUnit" readonly="readonly" style="padding: 3px; text-align: center;" value="GT" />
												</div>
												<div class="col-sm-2">
													<input type="text" class="form-control" id="TotalVolume" name="TotalVolume" style="padding: 3px; text-align: center;" />
												</div>
												<div class="col-sm-2">
													<input type="text" class="form-control" id="TotalWeight" name="TotalWeight" style="padding: 3px; text-align: center;" />
												</div>
											</div>
										</td>
									</tr>
								</tfoot>
							</table>
						</div>
						<input type="hidden" name="IsSendYuhan" id="IsSendYuhan" />
						<input type="hidden" name="TBBHPk" id="TBBHPk" value="<%=Current.TBBHPk %>" />
					</form>
				</div>
			</div>
			<div class="text-center m-t-lg m-b-lg">
				<input type="button" class="btn btn-primary" id="BTN_SetSave" value="임시저장" />
				<input type="button" class="btn btn-primary" id="BTN_SetSend" value="유한 프로그램 전송" />
				<input type="button" class="btn btn-danger" id="BTN_Delete" value="삭제" />
			</div>
		</section>
	</div>
	<div class="modal" id="Modal_List" style="margin-top: 80px;" tabindex="-1" role="dialog">
		<div class="modal-dialog modal-md">
			<div class="modal-content">
				<div class="modal-header">
					<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
					<h4 class="modal-title" id="ModalHeading"></h4>
				</div>
				<div class="modal-body" id="ModalBody"></div>
				<div class="modal-footer">
					<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" runat="Server">
	<script src="../Lib/jquery-ui.js"></script>
	<script src="../Lib/Work.js"></script>
	<script type="text/javascript">
		var ModalStatus = "";
		var TargetId = "";
		var TargetId2 = "";
		var TargetId3 = "";

		jQuery(document).ready(function () {
			$("#FlagAACO").val("<%=Current.FlagAACO %>");

			$("#SavedHouseBL > p").each(function () {
				AddHouseBL($(this).data("hsn"), $(this).data("blno"), $(this).data("packedcount"), $(this).data("packingunit"), $(this).data("weight"), $(this).data("volume"), $(this).data("shippername"), $(this).data("shipperaddress"), $(this).data("consigneename"), $(this).data("consigneeaddress"), $(this).data("consigneesaupjano"), $(this).data("description"));
			});
			SetCALC();
			$("#BTN_Calc").on("click", SetCALC);
			$("#BTN_ReOrderHSN").on("click", function () {
				var HSN = 0;
				for (var i = 0; i < $("#PnHouseBL > tr").length; i++) {
					if ($("#House_" + i + "_HSN").val() == "" || $("#House_" + i + "_HSN").val() == "0") {
						$("#House_" + i + "_HSN").val("99");
					} else if ($("#House_" + i + "_BLNo").val() == "") {
						$("#House_" + i + "_HSN").val("99");
					} else {
						HSN++;
						$("#House_" + i + "_HSN").val(HSN);
					}
				}
			});
			$("#ContainerType").val($("#HContainerSize").val());
			$("#Tab_Work").addClass("active");
			$("#Nav_Work").addClass("active");
			$("#Nav_YuhanView").css("font-weight", "bold");
			$("#Nav_YuhanView").css("background-color", "#c0d4eE");

			$("#AddHouseBL").on("click", function () {
				AddHouseBL("", "", "", "", "", "", "", "", "", "", "", "");
			});
			$("#WarehouseName").on("click", function () { ModalOpen("Warehouse"); });
			$("#AssignmentName").on("click", function () { ModalOpen("Assignment"); });
			$("#PreCarriageName").on("click", function () { ModalOpen("PreCarriage"); });
			$("#FinalPortName").on("click", function () {
				ModalOpen("FinalPort");
			});

			$("#Customs").on("click", function () {
				ModalOpen("Customs");
			}); $("#Division").on("click", function () {
				ModalOpen("Division");
			});

			$("#PreCarriageCode, #FinalPortCode,#WarehouseCode,#AssignmentCode").keydown(function (e) {
				if (e.which == 13) {/* 13 == enter key@ascii */
					ModalStatus = this.id;
					switch (this.id) {
						case "PreCarriageCode":
							TargetId = "#PreCarriageName";
							SearchByCode("PreCarriage", $("#PreCarriageCode").val());
							return false;
						case "FinalPortCode":
							TargetId = "#FinalPortName";
							SearchByCode("FinalPort", $("#FinalPortCode").val());
							return false;
						case "WarehouseCode":
							TargetId = "#WarehouseName";
							SearchByCode("Warehouse", $("#WarehouseCode").val());
							return false;
						case "AssignmentCode":
							TargetId = "#AssignmentName";
							SearchByCode("Assignment", $("#AssignmentCode").val());
							return false;
					}
				}
			});
			$("#BTN_SetSend").on("click", function () {
				$("#IsSendYuhan").val("Y");
				SetSave();
			});
			$("#BTN_SetSave").on("click", function () {
				$("#IsSendYuhan").val("N");
				SetSave();
			});
			$("#BTN_Delete").on("click", function () {
				$.ajax({
					type: "POST",
					url: "/Process/YuhanNP.asmx/SetDeleteFromIntl2000",
					data: "{TBBHPk:\"" + $("#TBBHPk").val() + "\"}",
					contentType: "application/json; charset=utf-8",
					dataType: "json",
					success: function (result) {
						if (result.d == "1") {
							alert("<%=GetGlobalResourceObject("qjsdur","tjdrhd")%>");
							location.reload();
						} else {
							document.write(result.d);
						}
						return false;
					},
					error: function (msg) {
						alert('failure : ' + msg);
					}
				});
			});
		});

		function SetSave() {
			SetCALC();
			var datas = $("#PnMasterBL").serialize();
			$.ajax({
				type: "POST",
				url: "/Process/YuhanNP.asmx/SetMasterBL",
				data: "{ValueSum:\"" + datas + "\"}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (result) {
					if (result.d == "1") {
						alert("<%=GetGlobalResourceObject("qjsdur","tjdrhd")%>");
						location.reload();
					} else if (result.d == "0") {
						alert("유한 프로그램에 이미 저장된 마스터가 있습니다.삭제 하시고 저장해주세요");
						location.reload();
					} else if (result.d == "2") {
						alert("데이터를 확인해주세요");
						location.reload();
					} else {
						document.write(result.d);
					}
					return false;
				},
				error: function (msg) {
					alert('failure : ' + msg);
				}
			});
		}
		function SetCALC() {
			var datas = $("#PnMasterBL").serialize();
			$.ajax({
				type: "POST",
				url: "/Process/YuhanNP.asmx/SetCalc",
				data: "{ValueSum:\"" + datas + "\"}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (result) {
					data = result.d.split(",!");
					$("#TotalPackedCount").val(data[0]);
					$("#TotalVolume").val(data[1]);
					$("#TotalWeight").val(data[2]);
					return false;
				},
				error: function (msg) {
					alert('failure : ' + msg);
				}
			});
		}
		function ModalOpen(Type) {
			$.ajax({
				type: "POST",
				url: "/Process/YuhanNP.asmx/Load_SavedList",
				data: "{Type:'" + Type + "'}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (result) {
					switch (Type) {
						case "Customs":
							$("#ModalHeading").html("Customs");
							ModalStatus = "MasterBL/Customs";
							break;
						case "Division":
							$("#ModalHeading").html("Division");
							ModalStatus = "MasterBL/Division";
							break;
						case "FinalPort":
							$("#ModalHeading").html("FinalPort");
							ModalStatus = "MasterBL/FinalPort";
							break;
						case "Warehouse":
							$("#ModalHeading").html("Warehouse");
							ModalStatus = "MasterBL/Warehouse";
							break;
						case "PreCarriage":
							$("#ModalHeading").html("Pre-Carriage");
							ModalStatus = "MasterBL/PreCarriage";
							break;
						case "Assignment":
							$("#ModalHeading").html("Pre-Assignment");
							ModalStatus = "MasterBL/Assignment";
							break;
					}
					$("#ModalBody").html(result.d);
					$('#Modal_List').modal('show');
				},
				error: function (msg) {
					alert('failure : ' + msg);
				}
			});
		}
		function SearchByCode(Type, Code) {
			$.ajax({
				type: "POST",
				url: "/Process/YuhanNP.asmx/SearchCode",
				data: "{Type:'" + Type + "', Code:'" + Code + "'}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (result) {
					switch (ModalStatus) {
						default:
							$(TargetId).val(result.d);
							break;
					}
				},
				error: function (msg) {
					alert('failure : ' + msg);
				}
			});
		}
		function SetInModal(Code, Name) {
			switch (ModalStatus) {
				case "HouseBL/Description":
					for (var i = 0; i < $("#PnHouseBL > tr").length; i++) {
						if ($("#House_" + i + "_BLNo").val() == Code) {
							$("#House_" + i + "_Description").val(Name);
							break;
						}
					}
					ModalStatus = "";
					$('#Modal_List').modal('hide');
					break;
				case "MasterBL/Warehouse":
					$("#WarehouseCode").val(Code);
					$("#WarehouseName").val(Name);
					ModalStatus = "";
					$('#Modal_List').modal('hide');
					break;
				case "MasterBL/FinalPort":
					$("#FinalPortCode").val(Code);
					$("#FinalPortName").val(Name);
					ModalStatus = "";
					$('#Modal_List').modal('hide');
					break;
				case "MasterBL/PreCarriage":
					$("#PreCarriageCode").val(Code);
					$("#PreCarriageName").val(Name);
					ModalStatus = "";
					$('#Modal_List').modal('hide');
					break;
				case "MasterBL/Customs":
					$("#Customs").val(Code);
					ModalStatus = "";
					$('#Modal_List').modal('hide');
					break;
				case "MasterBL/Division":
					$("#Division").val(Code);
					ModalStatus = "";
					$('#Modal_List').modal('hide');
					break;
				case "MasterBL/Assignment":
					$("#AssignmentCode").val(Code);
					$("#AssignmentName").val(Name);
					ModalStatus = "";
					$('#Modal_List').modal('hide');
					break;

			}
		}
		function ModalOpen_BeforeHistory(RowCount) {
			$.ajax({
				type: "POST",
				url: "/Process/YuhanNP.asmx/LoadHistory_Description",
				data: "{HouseBLNo:\"" + $("#House_" + RowCount + "_BLNo").val() + "\"}",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (result) {
					if (result.d == "") {
						alert("지난 기록이 없습니다.");
					} else {
						$("#ModalHeading").html("지난 품명");
						ModalStatus = "HouseBL/Description";
						$("#ModalBody").html(result.d);
						$('#Modal_List').modal('show');
						return false;
					}
				},
				error: function (msg) {
					alert('failure : ' + msg);
				}
			});
		}

		function AddHouseBL(HSN, BLNo, PackedCount, PackingUnit, Weight, Volume, ShipperName, ShipperAddress, ConsigneeName, ConsigneeAddress, ConsigneeSaupjaNo, Description) {
			var RowCount = $("#PnHouseBL > tr").length;
			var ShipperReadOnly = "";
			var ConsigneeReadOnly = "";
			var rowStyle = "";
			if (RowCount % 2 == 0) {
				rowStyle = 'style="background-color:#eee;"';
			}
			var Html = '													\
							<tr '+ rowStyle + '>														\
								<td><input type="text" class="form-control" style="padding:3px; text-align:center; " id="House_' + RowCount + '_HSN" name="House_' + RowCount + '_HSN" value="' + HSN + '" placeholder="HSN" /></td>\
								<td>													\
									<div class="form-group ">									\
										<div class="col-sm-5">										\
											<input type="text" class="form-control" style="padding:3px; text-align:center; " id="House_' + RowCount + '_BLNo" name="House_' + RowCount + '_BLNo"  value="' + BLNo + '" placeholder="BLNo" />	\
										</div>																	\
										<div class="col-sm-2">										\
											<input type="text" class="form-control" style="padding:3px; text-align:right; " id="House_' + RowCount + '_PackedCount" name="House_' + RowCount + '_PackedCount" placeholder="Count" value="' + PackedCount + '" />										\
										</div>																		\
										<div class="col-sm-1">											\
											<input type="text" class="form-control" style="padding:3px; text-align:center; " id="House_' + RowCount + '_PackingUnit" placeholder="Unit" name="House_' + RowCount + '_PackingUnit" value="' + PackingUnit + '" />					\
										</div>																		\
										<div class="col-sm-2">											\
											<input type="text" class="form-control" style="padding:3px; text-align:right; " id="House_' + RowCount + '_Volume" placeholder="CBM" name="House_' + RowCount + '_Volume" value="' + Volume + '" /> \
										</div>																		\
										<div class="col-sm-2">											\
											<input type="text" class="form-control" style="padding:3px; text-align:right; " id="House_' + RowCount + '_Weight" placeholder="Kg" name="House_' + RowCount + '_Weight"  value="' + Weight + '" />		\
										</div>																		\
									</div>																			\
									<div class="form-group">										\
										<label class="col-sm-3 control-label" style="padding-top:7px; text-align:right;" onclick="ModalOpen_BeforeHistory(\''+ RowCount + '\');">Description</label>\
										<div class="col-sm-9">															\
											<textarea rows="4" id="House_' + RowCount + '_Description" placeholder="Description" name="House_' + RowCount + '_Description" class="form-control">' + Description + '</textarea>									\
										</div>																						\
									</div>																							\
									<div class="form-group" >														\
										<div class="col-sm-12">&nbsp;</div>								\
									</div>																							\
								</td>																								\
								<td>																									\
									<div class="form-group">														\
										<div class="col-sm-12">														\
											<input type="text" class="form-control" ' + ShipperReadOnly + ' placeholder="Shipper" id="House_' + RowCount + '_ShipperName" name="House_' + RowCount + '_ShipperName"  value="' + ShipperName + '" />														\
										</div>								\
									</div>									\
									<div class="form-group">		\
										<div class="col-sm-12">		\
											<textarea rows="4" ' + ShipperReadOnly + ' placeholder="Address"  id="House_' + RowCount + '_ShipperAddress" name="House_' + RowCount + '_ShipperAddress" class="form-control">' + ShipperAddress + '</textarea>									\
										</div>										\
									</div>											\
								</td>												\
								<td>													\
									<div class="form-group">		\
										<div class="col-sm-12">																										\
											<input type="text" class="form-control" ' + ConsigneeReadOnly + ' placeholder="Consignee"  id="House_' + RowCount + '_ConsigneeName" name="House_' + RowCount + '_ConsigneeName" value="' + ConsigneeName + '" />								\
										</div>																			\
									</div>																				\
									<div class="form-group">											\
										<div class="col-sm-12">											\
											<textarea rows="4" placeholder="Address" class="form-control" ' + ConsigneeReadOnly + ' id="House_' + RowCount + '_ConsigneeAddress" name="House_' + RowCount + '_ConsigneeAddress" >' + ConsigneeAddress + '</textarea>			\
										</div>																			\
									</div>																				\
									<div class="form-group">											\
										<div class="col-sm-12">											\
											<input type="text" placeholder="사업자번호"class="form-control" ' + ConsigneeReadOnly + ' id="House_' + RowCount + '_ConsigneeSaupjaNo" name="House_' + RowCount + '_ConsigneeSaupjaNo" value="' + ConsigneeSaupjaNo + '" />	\
										</div>																			\
									</div>																				\
									</td>																					\
							</tr>';
			$("#PnHouseBL").append(Html);
		}

	</script>
</asp:Content>
