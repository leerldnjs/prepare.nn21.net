<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Admin_Scale.master" AutoEventWireup="true" CodeFile="TransportView.aspx.cs" Inherits="Transport_TransportView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" runat="Server">

	<div class="panel panel-warning col-xs-10 col-xs-offset-1">
		<div class="panel-body form-horizontal">
			<div class="col-xs-6">
				<section class="panel b-a">
					<div class="panel-heading no-border bg-default lt">
						<h5 class="padder">Transport Info</h5>
					</div>
					<div class="panel-body">
						<%=Html_TransportView %>
					</div>
				</section>
			</div>

			<div class="col-xs-6">
				<section class="panel b-a">
					<div class="panel-heading no-border bg-default lt">
						<h5 class="padder">Function</h5>
					</div>
					<div class="panel-body">
						<div class="row" style="margin-bottom: 10px;">
							<div class="col-xs-2 col-xs-offset-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="운송정보수정" id="Btn_ModifyHead" />
							</div>
							<div class="col-xs-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="컨테이너수정" id="Btn_ModifyPacked" />
							</div>
							<div class="col-xs-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="File Upload" id="Btn_FileUpload" />
							</div>
							<div class="col-xs-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="Excel Down" id="Btn_ExcelDown" />
							</div>
						</div>
						<div class="row" style="margin-bottom: 10px;">
							<div class="col-xs-2 col-xs-offset-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="입고확인" id="Btn_ToStep_2" />
							</div>
							<div class="col-xs-4" id="d_StorageList">
							</div>
							<div class="col-xs-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="창고지정" id="Btn_Assign_Warehouse" />
							</div>
							<div class="col-xs-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="입고완료" id="Btn_ToStep_3" />
							</div>
						</div>
						<div class="row" style="margin-bottom: 10px;">
							<div class="col-xs-2 col-xs-offset-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="선적데이터 전송" id="Btn_SendDataToCommercialDocu" />
							</div>
							<div class="col-xs-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="창고정보" id="Btn_WareHouseInfo" onclick="location.href = '/Admin/ManagementStorage.aspx?S=3157'"/>
							</div>
						</div>
						<div class="row" style="margin-bottom: 10px;">
							<div class="col-xs-2 col-xs-offset-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="겉지" id="Btn_Abji" />
							</div>
							<div class="col-xs-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="적화전송" id="Btn_JukWha" />
							</div>
							<div class="col-xs-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="2" id="Btn_JukWhaSend" />
							</div>
							<div class="col-xs-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="적화전송2" id="Btn_JukWha2" />
							</div>
						</div>
						<div class="row">
							<div class="col-xs-2 col-xs-offset-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="DebitCredit" id="Btn_DebitCredit" />
							</div>
							<div class="col-xs-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="매입비용 입력" id="Btn_Cost" />
							</div>
							<div class="col-xs-2">
								<input type="button" class="btn btn-default btn-sm form-control" value="유한 전화전송N" id="Btn_Yuhan" />
							</div>
						</div>


					</div>
				</section>
			</div>

			<div class="col-xs-3">
				<section class="panel b-a">
					<div class="panel-heading no-border bg-default lt">
						<h5 class="padder">Charge</h5>
					</div>
					<div class="panel-body">
						<ul class="list-group">
							<%=Html_ChargeList %>
						</ul>
					</div>
				</section>
			</div>

			<div class="col-xs-3">
				<section class="panel b-a">
					<div class="panel-heading no-border bg-default lt">
						<h5 class="padder">FileList</h5>
					</div>
					<div class="panel-body">
						<ul class="list-group">
							<%=Html_FileList %>
						</ul>
					</div>
				</section>
			</div>

			<div class="col-xs-6">
				<section class="panel b-a">
					<div class="panel-heading no-border bg-default lt">
						<h5 class="padder">Comment</h5>
					</div>
					<div class="panel-body">
						<ul class="list-group">
							<%=Html_Comment %>
						</ul>
					</div>
					<div class="clearfix panel-footer">
						<div class="col-xs-10 col-xs-offset-1">
							<input type="text" class="form-control" id="CommentText"/>
						</div>
						<div class="col-xs-1">
							<input type="button" class="btn btn-primary btn-sm" id="Btn_AddComment" value="입력" />
						</div>
					</div>
				</section>
			</div>

			
		</div>
		<div class="panel panel-default">
			<div class="panel-body form-horizontal">
				<table class="table b-t b-light">
					<thead class="bg-primary">
						<tr>
							<th style="text-align: left;">Shipper</th>
							<th style="text-align: left;">Consignee</th>
							<th style="text-align: left;">From</th>
							<th style="text-align: left;">To</th>
							<th style="text-align: left;">Count</th>
							<th style="text-align: left;">CBM</th>
							<th style="text-align: left;">Weight</th>
							<th style="text-align: left;">Description</th>
						</tr>
					</thead>
					<tbody id="ViewBody">
					</tbody>
				</table>
			</div>
		</div>
	</div>

	<input type="hidden" id="pHeadPk" value="<%=pHeadPk %>" />
	<input type="hidden" id="pBlNo" value="<%=Head.Value_String_0 %>" />
	<input type="hidden" id="pStatus" value="<%=pStatus %>" />
	<input type="hidden" id="pWarehouse" value="<%=pWarehouse %>" />
	<input type="hidden" id="pPackedCount" value="<%=pPackedCount %>" />
	<input type="hidden" id="pSessionId" value="<%=MemberInformation[2] %>" />
	<input type="hidden" id="pBranchPk" value="<%=MemberInformation[1] %>" />
	<input type="hidden" id="NewChargeNo" value="" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" runat="Server">
	<script src="/Lib/ForDialog.js"></script>
	<script type="text/javascript">

		var TransportHeadPk = $("#pHeadPk").val();
		var TransportPackedPk = "";
		var Status = $("#pStatus").val();
		var Warehouse = $("#pWarehouse").val();
		var PackedCount = $("#ppackedCount").val();

		$(document).ready(function () {
			$("#Btn_ToStep_2").on("click", ToStep);
			$("#Btn_ToStep_3").on("click", ToStep);
			$("#Btn_Assign_Warehouse").on("click", Assign_Warehouse);
			$("#Btn_FileUpload").on("click", function () { GoFileupload("File") });
			$("#Btn_Cost").on("click", function () { GoFileupload("Cost") });
			$("#Btn_ExcelDown").on("click", function () { GoExcelDown("Excel") });
			$("#Btn_Abji").on("click", function () { GoExcelDown("Abji") });
			$("#Btn_JukWha").on("click", function () { GoExcelDown("JukWha") });
			$("#Btn_JukWhaSend").on("click", function () { GoExcelDown("JukWhaSend") });
			$("#Btn_JukWha2").on("click", function () { GoExcelDown("JukWha2") });
			$("#Btn_DebitCredit").on("click", function () { Goto("DebitCredit_Detail") });
			$("#Btn_Yuhan").on("click", function () { Goto("YuhanDetailN") });
			
			$("#Btn_SendDataToCommercialDocu").on("click", SendDataToCommercialDocu)
			$("#Btn_ModifyHead").on("click", function () {
				Modify_Modal("Head");
			})
			$("#Btn_ModifyPacked").on("click", function () {
				Modify_Modal("Packed");
			})
			$("#Btn_AddComment").on("click", Add_Comment)

			if (Status == "1") {
				$("#Btn_ToStep_3").hide();
			}
			else if (Status == "2") {
				$("#Btn_ToStep_2").hide();
			}
			else { //Status == "3"
				$("#Btn_ToStep_2").hide();
				$("#Btn_ToStep_3").hide();
			}

			Call_Ajax("StorageList", "/WebService/TransportP.asmx/LoadBranchStorage", "{CompanyPk:" + "'" + "<%=MemberInformation[1] %>" + "'" + "}");
			$("#St_Storage").val("<%=pWarehouse %>");

		});

		function DO_Insert() {
			if ($("#MRN").val() != "") {
				Call_Ajax("DO_Insert", "/WebService/Admin.asmx/AddDO", "{BBHeadPk:'" + $("#pHeadPk").val() + "', " + "HDOBLNo:'" + $("#pBlNo").val() + "', " + "MRN:'" + $("#MRN").val() + "', " + "MSN:'" + $("#MSN").val() + "', " + "AccountID:'" + $("#pSessionId").val() + "'}");
			}
		}
		function DO_Update() {
			if ($("#MSN").val() != "") {
				Call_Ajax("DO_Update", "/WebService/Admin.asmx/UpdateDO", "{BBHeadPk:'" + $("#pHeadPk").val() + "', " + "HDOBLNo:'" + $("#pBlNo").val() + "', " + "MRN:'" + $("#MRN").val() + "', " + "MSN:'" + $("#MSN").val() + "', " + "AccountID:'" + $("#pSessionId").val() + "'}");
			}
		}
		function GoFileupload(Flag) {
			switch (Flag) {
				case "File":
					window.open('/Admin/Dialog/FileUpload.aspx?G=0&S=' + TransportHeadPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;');
					break;
				case "Cost":
					var data = {
						TableName: "TRANSPORT_HEAD",
						TablePk: $("#pHeadPk").val(),
						Type: "Purchase"
					}
					$.ajax({
						type: "POST",
						url: "/Process/ChargeP.asmx/GetChargeNo",
						data: JSON.stringify(data),
						dataType: "json",
						contentType: "application/json; charset=utf-8",
						async: false,
						success: function (result) {
							if (result.d == "") {
								$("#NewChargeNo").val("0");
							}
							else {
								$("#NewChargeNo").val(parseInt(result.d) + 1);
							}
						},
						error: function (result) {
							alert('failure : ' + result);
						}
					});
					window.open("/Charge/Dialog/TransportCharge.aspx?S=" + $("#pHeadPk").val() + "&A=N&N=" + $("#NewChargeNo").val() + "&G=31", "popupWindow", "width=800px, height=650px, scrollbars=yes");
					break;
			}
		}
		function GoExcelDown(Flag) {
			switch (Flag) {
				case "Excel":
					location.href = "/UploadedFiles/FileDownloadWithExcel.aspx?S=" + TransportHeadPk;
					break;
				case "Abji":
				case "JukWha":
				case "JukWhaSend":
				case "JukWha2":
					location.href = "/UploadedFiles/FileDownloadWithExcel.aspx?G=" + Flag + "&S=" + TransportHeadPk;
					break;
			}
		}
		function Goto(Type) {
			switch (Type) {
				case "YuhanDetail":
					location.href = "/CustomClearance/YuhanView.aspx?S=" + TransportHeadPk;
					break;
				case "YuhanDetailN":
					location.href = "/CustomClearance/YuhanViewN.aspx?S=" + TransportHeadPk;
					break;
				case "DebitCredit_Detail":
					window.open('/Document/DebitCredit_Detail.aspx?S=' + TransportHeadPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, top=20px; left=200px; height=700px; width=800px;');
					break;
				case "Credit_View":
					window.open('/Document/DebitCredit_View.aspx?PageType=Credit&S=' + TransportHeadPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, top=20px; left=200px; height=700px; width=850px;');
					break;
				case "Debit_View":
					window.open('/Document/DebitCredit_View.aspx?PageType=Debit&S=' + TransportHeadPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, top=20px; left=200px; height=700px; width=850px;');
					break;
			}
		}

		function SendDataToCommercialDocu() {
			Call_Ajax("SendDataToCommercialDocu", "/WebService/Admin.asmx/SendDateToCommercialDocu", "{BBHPk:" + "'" + TransportHeadPk + "'}");
		}

		function Add_Comment() {
			Call_Ajax("Add_Comment", "/Process/HistoryP.asmx/Set_Comment", "{Table_Name:'TRANSPORT_HEAD', Table_Pk:'" + TransportHeadPk + "', Category:'HeadComment', Contents:'" + $("#CommentText").val() + "', Account_Id:'" + "<%=MemberInformation[2] %>" +"'}");
		}

		function DeleteTransportBBCharge(ChargePk) {
			Call_Ajax("DeleteTransportBBCharge", "/WebService/Admin.asmx/DeleteTransportBBCharge", "{Pk:" + "'" + ChargePk + "'}");
		}

		function Print(Gubun, Value, OBPK) {
			switch (Gubun) {
				case "DO": location.href = "/CustomClearance/CommercialDocu_DeliveryOrder.aspx?S=" + Value + "&B=" + form1.HBBHPk.value + "&G=Print"; break;
				case "B":
					if (OBPK != "") {
						location.href = "/UploadedFiles/FileDownload.aspx?S=" + OBPK;
						break;
					} else {
						location.href = "/CustomClearance/CommercialDocu_HouseBL.aspx?S=" + Value + "&G=Print";
						break;
					}
				case "OB": location.href = "/UploadedFiles/FileDownload.aspx?S=" + Value; break;
				case "B_YT": location.href = "/CustomClearance/CommercialDocu_HouseBL.aspx?S=" + Value + "&YT=YT"; break;
				case "B_YT2": location.href = "/CustomClearance/CommercialDocu_HouseBL_YT.aspx?S=" + Value; break;
				case "I": location.href = "/CustomClearance/CommercialDocu_Invoice.aspx?S=" + Value + "&G=Print"; break;
				case "P": location.href = "/CustomClearance/CommercialDocu_PackingList.aspx?S=" + Value + "&G=Print"; break;
				case "D": window.open('/Admin/Dialog/AutoPrint.aspx?G=DeliveryReceipt&S=' + Value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=	300px; width=600px;'); break;
			}
		}

		function Modify_Modal(ModFlag) {
			if (ModFlag == "Packed" && TransportPackedPk=="") {
				alert("컨테이너가 선택되지 않았습니다.")
				return false;
			}

			ForDialog.initPopup();
			var PopWindow = "pop_win" + "asdf";
			var Win = window.open('', PopWindow, 'width=780, height=464,menubar=no,status=no,scrollbars=yes');

			$("#ForDialog_AccountPk").val($("#pSessionId").val());

			if (ModFlag == "Head") {
				$("#ForDialog_Type").val("TransportHeadPk");
				$("#ForDialog_TypePk").val(TransportHeadPk);
				$("#ForDialog").attr('target', PopWindow);
				$("#ForDialog_CompanyPk").val($("#pBranchPk").val());
				$("#ForDialog").attr('action', '/Transport/Dialog/TransportHead.aspx');
			}
			else {
				$("#ForDialog_Type").val("TransportPackedPk");
				$("#ForDialog_TypePk").val(TransportPackedPk);
				$("#ForDialog_CompanyPk").val($("#pBranchPk").val());
				$("#ForDialog").attr('target', PopWindow);
				$("#ForDialog").attr('action', '/Transport/Dialog/TransportPacked.aspx');
			}
			$("#ForDialog").submit();
		}

		/*
		function Modify_ViewData() {
			var DataSum = TransportHeadPk;
			DataSum += ",!" + $("#Vessel_name").val();
			DataSum += ",!" + $("#BL").val();
			DataSum += ",!" + $("#Area_From").val();
			DataSum += ",!" + $("#Date_From").val();
			DataSum += ",!" + $("#Area_To").val();
			DataSum += ",!" + $("#Date_To").val();
			DataSum += ",!" + $("#Value1").val();
			DataSum += ",!" + $("#Value2").val();
			DataSum += ",!" + $("#Value3").val();
			DataSum += ",!" + $("#Voyage_No").val();
			for (var j = 0; j < PackedCount; j++) {
				DataSum += ",!@" + $("#PackedPk_" + j).val();
				DataSum += ",!" + $("#Owner_" + j).val();
				DataSum += ",!" + $("#PackedNo_" + j).val();
				DataSum += ",!" + $("#Seal_" + j).val();
			}

			Call_Ajax("Modify_ViewData", "TransportView.aspx/Modify_ViewData", "{DataSum:" + "'" + DataSum + "'" + "}");

		}
		*/

		function ToStep() {
			switch (Status) {
				case "1":
					var r = confirm("입고를 '확인' 하시겠습니까?");
					if (r == false) {
						return false;
					}
					break;
				case "2":
					if (Warehouse == "") {
						alert("창고가 지정되지 않았습니다.");
						return false;
					}
					var r = confirm("입고를 '확정' 하시겠습니까?");
					if (r == false) {
						return false;
					}
					break;
				case "3":
					alert("이미 입고가 확정되었습니다.");
					return false;
			}

			Call_Ajax("ToStep", "TransportView.aspx/Transport_ToStep", "{TransportHeadPk:" + "'" + TransportHeadPk + "'" + ", " + "TransportStatus:" + "'" + Status + "'" + "}");

		}

		function Assign_Warehouse() {
			if ($("#Transport_Status").val() < 2) {
				alert("입고가 '확인' 되지 않았습니다.");
				return false;
			}
			if ($("#St_Storage").val() == 0) {
				alert("창고가 '선택' 되지 않았습니다.");
				return false;
			}
			var r = confirm("창고를 지정하시겠습니까?");
			if (r == false) {
				return false;
			}

			Call_Ajax("Assign_Warehouse", "TransportView.aspx/Assign_Stroage", "{TransportHeadPk:" + "'" + TransportHeadPk + "'" + ", " + "WarehousePk:" + "'" + $("#St_Storage").val() + "'" + "}");

		}
		
		function Retrieve_Body(PackedPk) {
			TransportPackedPk = PackedPk;
			Call_Ajax("Retrieve_Body", "TransportView.aspx/MakeHtml_RetrieveBody", "{TransportPackedPk:" + PackedPk + "}");
		}

		function Call_Ajax(Type, Url, Data) {
			$.ajax({
				type: "POST",
				url: Url,
				data: Data,
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				async: false,	
				success: function (result) {
					switch (Type) {
						case "Retrieve_Body":
							$("#ViewBody").html(result.d);
							break;
						case "Assign_Warehouse":
							location.reload();
							break;
						case "Modify_ViewData":
							alert(result.d);
							break;
						case "ToStep":
							location.reload();
							break;
						case "StorageList":
							$("#d_StorageList").html(result.d);
							break;
						case "Add_Comment":
							location.reload();
							break;
						case "SendDataToCommercialDocu":
							alert("성공");
							break;
						case "DO_Insert":
							location.reload();
							break;
						case "DO_Update":
							location.reload();
							break;
						case "DeleteTransportBBCharge":
							location.reload();
							break;
					}
				},
				error: function (result) {
					alert(result);
				}
			});
		}

	</script>
</asp:Content>

