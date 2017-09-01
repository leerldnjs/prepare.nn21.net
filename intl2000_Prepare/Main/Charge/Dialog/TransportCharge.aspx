<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Clean.Master" AutoEventWireup="true" CodeFile="TransportCharge.aspx.cs" Inherits="Charge_Dialog_TransportCharge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" Runat="Server">

	<div class="panel panel-default">
		<header class="panel-heading font-bold">매입비용</header>
		<div class="panel-body">

			<%=Html_ChargeBody %>

		</div>
		<div class="panel-footer col-xs-12">
			<div class="col-xs-2 col-xs-offset-4">
				<input type="button" id="Btn_Submit" class="btn btn-primary btn-md" value="Submit" onclick="Submit_Charge();" />
			</div>
			<div class="col-xs-2">
				<input type="button" id="Btn_Cancel" class="btn btn-warning btn-md" value="Cancel" onclick="self.close();"/>
			</div>
		</div>
	</div>

	<!--Modal HTML-->
	<div class="modal" id="ToModal" role="dialog">
		<div class="modal-dialog modal-md">
			<div class="modal-content">
				<div class="modal-body" id="ToModal_Body">
				</div>
				<div class="modal-footer">
					<button type="button" data-dismiss="modal" id="ModalClose">Close</button>
				</div>
			</div>
		</div>
	</div>
	<!--Modal HTML-->

	<input type="hidden" id="TablePk" value="<%=TransportPk %>" />
	<input type="hidden" id="ChargeNo" value="<%=ChargeNo %>" />
	<input type="hidden" id="SelectOurBranch" value="<%=SelectOurBranch %>" />
	<input type="hidden" id="MonetaryDefault" value="<%=Monetary.Replace("\"", "") %>" />
	<input type="hidden" id="Datetime_From" value="<%=Datetime_From %>" />

	<input type="hidden" id="AlreadyCalc" value="<%=Already %>" />
	<input type="hidden" id="AccountID" value="<%=MemberInformation[2] %>" />

	<input type="hidden" id="BranchPk_From" value="<%=BranchPk_From %>" />
	<input type="hidden" id="BranchPk_To" value="<%=BranchPk_To %>" />


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" Runat="Server">
	<script type="text/javascript">

		var OverWeight = new Array();
		var ModalStatus;
		var THeadCount = 1;
		var THeadTemp = "";
		var TBodyTemp = "";
		var TotalTemp = "";
		var TotalPriceTemp = "";
		var ExchangeDate = "";
		var ExchangeCheck = "";
		var ExchangeRate = new Array();
		var CalcCheck = false;
		var Type = "Purchase";

		$(document).ready(function () {
			if ($("#AlreadyCalc").val() == "Y") {
				$("#ChargeBody").html("<%=Calculated_Item.Replace("\"", "'") %>");
				$("#ChargeTotal").html("<%=Calculated_Total.Replace("\"", "'") %>");
				$("#ChargeTable > thead > tr > th:eq(5)").remove();
				$("#ChargeTable > thead > tr > th:eq(5)").remove();
				$("#ChargeTable > thead > tr").append("<%=Calculated_Head.Replace("\"", "'") %>");
				THeadCount = $("#ChargeTable > thead > tr > th").length - 6;
			}

		});

		function AddChargeBody() {

			var RowCount = $("#ChargeTable > tbody > tr").length;
			var NewRow = "<tr><td><select class=\"form-control\" id=\"SettlementBranch_" + RowCount + "\">" + $("#SelectOurBranch").val() + "</select></td>";
			NewRow += "<td><select class=\"form-control\" id=\"PriceCategory_" + RowCount + "\"><option value=\"해운비\" selected=\"selected\">해운비 (영세)</option><option value=\"대행비\">대행비 (과세)</option><option value=\"제세금\">제세금 (관.부가세)</option></select></td>"
			NewRow += "<td><input type=\"text\" class=\"form-control\" id=\"Title_" + RowCount + "\" /></td>";
			NewRow += "<td><select class=\"form-control\" id=\"Item_MonetaryUnit_" + RowCount + "\">" + $("#MonetaryDefault").val() + "</select></td>";
			NewRow += "<td><input type=\"text\" class=\"form-control\" id=\"PriceItemValue_" + RowCount + "\" /></td>";
			for (var i = 0; i <= THeadCount; i++) {
				var Bdata = {
					Type: Type,
					Row: RowCount,
					Col: i,
					Group: RowCount
				}
				Call_Ajax("TBody", "/Process/ChargeP.asmx/AddChargeTable_TBody", Bdata);
				NewRow += TBodyTemp;
			}
			NewRow += "</tr>";
			$("#ChargeBody").append(NewRow);
		}

		function AddChargeCol() {
			var RowLength = $("#ChargeTable > tbody > tr").length;
			var Hdata = {
				Type: Type,
				Count: ++THeadCount
			}
			Call_Ajax("THead", "/Process/ChargeP.asmx/AddChargeTable_THead", Hdata);

			for (var i = 0; i < RowLength; i++) {
				var Head = $("#ChargeTable > thead > tr").eq(i);
				var Body = $("#ChargeTable > tbody > tr").eq(i);

				var RadioGroup = $("#ChargeTable > tbody > tr:eq(" + i + ") > td:last > input").prop("name");
				var Count = $("#ChargeTable > tbody > tr:eq(" + i + ") > td:last > input").prop("id");

				var Bdata = {
					Type: Type,
					Row: Count.split("_")[1],
					Col: THeadCount,
					Group: RadioGroup.split("_")[1]
				}
				Call_Ajax("TBody", "/Process/ChargeP.asmx/AddChargeTable_TBody", Bdata);

				var AddHead = THeadTemp;
				var AddBody = TBodyTemp;
				Head.append(AddHead);
				Body.append(AddBody);
			}
			var Tdata = {
				Type: Type,
				Count: THeadCount
			}
			Call_Ajax("Total", "/Process/ChargeP.asmx/AddChargeTotal", Tdata);

			var AddTotal = TotalTemp;
			$("#ChargeTotal").append(AddTotal);

		}

		function LoadStandardPriceItem() {
			if ($("#st_StandardPrice").val() == "0") {
				alert("Load Seccess");
				$("#ChargeBody").html("");
				AddChargeBody();
				return false;
			}

			var data = {
				StandardPricePk: $("#st_StandardPrice").val()
			}
			Call_Ajax("PriceItem", "/Process/ChargeP.asmx/MakeHtml_StandardPriceItem", data);
		}

		function LoadStandardPriceItemSub() {
			var data = {
				StandardPricePk: $("#st_StandardPriceSub").val(),
				StandardItemPk: $("#st_StandardItemSub").val(),
				Col: THeadCount,
				Row: $("#ChargeTable > tbody > tr").length
			}
			Call_Ajax("PriceItemSub", "/Process/ChargeP.asmx/MakeHtml_StandardPriceItemSub", data);
		}

		function LoadStandardPriceValue() {
			var data = {
				StandardPricePk: $("#st_StandardPrice").val(),
				Criterion: $("#CriterionValue").val(),
				OverWeight: $("#OverWeightValue").val()
			}
			Call_Ajax("PriceValue", "/Process/ChargeP.asmx/Load_StrandardPriceValue", data);
		}

		function Call_Ajax(Target, Url, Data) {
			$.ajax({
				type: "POST",
				url: Url,
				data: JSON.stringify(Data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				async: false,
				success: function (result) {
					switch (Target) {
						case "PriceItem":
							alert("Load Success");
							$("#ChargeBody").html(result.d);
							break;
						case "PriceItemSub":
							alert("Add Success");
							$("#ChargeBody").append(result.d);
							break;
						case "PriceValue":
							alert("Load Success");
							var AllPrice = result.d.split("@@");
							var CriterionPrice_s = AllPrice[0];
							var OverWeightPrice_s = AllPrice[1];
							var CriterionPrice = CriterionPrice_s.split(",!");
							var OverWeightPrice = OverWeightPrice_s.split(",!");

							for (var i = 0; i < CriterionPrice.length; i++) {
								var CriterionValue = CriterionPrice[i].split(":");
								$("#PriceItemValue_" + i).val(CriterionValue[1]);
							}
							for (var i = 0; i < OverWeightPrice.length; i++) {
								var OverWeightValue = OverWeightPrice[i].split(":");
								var OverWeightFlag = $("#PriceItem_OverWeightFlag_" + i).val();
								if (OverWeightFlag == "Y") {
									$("#PriceItemValue_" + i).val(OverWeightValue[1]);
								}
							}
							PaymentWay($("input[name=Payment]:checked").val());
							break;
						case "Modal":
							$("#ToModal_Body").html(result.d);
							$("#ToModal").modal("show");
							break;
						case "THead":
							THeadTemp = result.d;
							break;
						case "TBody":
							TBodyTemp = result.d;
							break;
						case "Total":
							TotalTemp = result.d;
							break;
						case "RetrieveBank":
							$("#St_BranchBank").html(result.d);
							break;
						case "TotalCalc":
							if (result.d[0] == "-1" || result.d[0] == "-2") {
								alert("환율정보가 없습니다.");
								ExchangeCheck = "N";
							}
							ExchangeDate = result.d[0].split("!")[2];
							if (ExchangeDate == undefined) {
								ExchangeDate = "null";
							}
							TotalPriceTemp = result.d[1];
							var Rate = "";
							var Result = result.d[0].split("@@");
							for (var i = 0; i < Result.length - 1; i++) {
								Rate = Result[i].split("!")[3];
								if (Result[i].split("!")[3] == undefined) {
									Rate = "null";
								}
								ExchangeRate.push(Rate);
							}
							ExchangeCheck = "Y";
							break;
						case "Submit":
							alert("Success");
							opener.location.reload();
							self.close();
							break;
						case "ChangedStandardPriceSub":
							$("#st_StandardItemSub").html(result.d);
							break;
						case "ChangedStandardItemSub":
							$("#st_StandardValueSub").html(result.d);
							break;
						case "ChangedStandardValueSub":
							$("#PriceItemValue_" + ($("#ChargeTable > tbody > tr").length - 1) + "").val(NumberFormat(result.d));
							break;
					}
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});
		}

		function PaymentWay(Payment) {
			var RowLength = $("#ChargeTable > tbody > tr").length;

			for (var i = 0; i < RowLength; i++) {
				var Who = $("#PriceItem_" + Payment + "_" + i).val();
				if (Who == "S") {
					$("#RadioCharge_" + i + "_0").prop("checked", true);
				}
				else if (Who == "C") {
					$("#RadioCharge_" + i + "_1").prop("checked", true);
				}
			}
		}

		function ForModal(Status, ID) {
			ModalStatus = ID.split("_")[1];
			var data = {
				Type: Status,
				ShipperPk: $("#BranchPk_From").val(),
				ConsigneePk: $("#BranchPk_To").val()
			}
			Call_Ajax("Modal", "/Process/ChargeP.asmx/MakeHtml_Modal", data)
		}

		function RetrieveBank(BranchPk) {
			var data = {
				BranchPk: BranchPk
			}
			Call_Ajax("RetrieveBank", "/Process/ChargeP.asmx/Load_CompanyBank", data);
		}

		function ChoosedCustomer(CustomerPk, CustomerCode) {
			$("#Branch_" + ModalStatus).val($("#St_Branch option:selected").val());
			$("#BranchBank_" + ModalStatus).val($("#St_BranchBank option:selected").val());
			$("#BtnBranch_" + ModalStatus).val($("#St_Branch option:selected").text());
			$("#TotalBranch_" + ModalStatus).text($("#St_Branch option:selected").text());
			$("#Customer_" + ModalStatus).val(CustomerPk);
			$("#BtnCustomer_" + ModalStatus).val(CustomerCode);
			$("#TotalCustomer_" + ModalStatus).text(CustomerCode);

			$("#ToModal").modal("hide");
		}

		function TotalCalc() {
			var RowLength = $("#ChargeTable > tbody > tr").length;
			var ChargeLength = THeadCount;
			var PriceSum = new Array();
			var ExchangedSetDate = $("#Datetime_From").val();
			var IsEstimation = "";
			if ($("#IsEstimation").val() == "33") {
				IsEstimation = "Y";
			}

			var ChargeCol = new Array();

			for (var i = 0; i <= ChargeLength; i++) {
				ChargeCol[i] = new Array();
				PriceSum[i] = "";	// undefined때문에 초기화 
				for (var j = 0; j < RowLength; j++) {
					if ($("#RadioCharge_" + j + "_" + i).prop("checked") == true) {
						PriceSum[i] += $("#Item_MonetaryUnit_" + j).val() + "!" + $("#PriceItemValue_" + j).val() + "@@";
					}
				}
			}

			ExchangeRate = new Array();
			for (var i = 0; i <= ChargeLength; i++) {
				var data = {
					ShipperMonetaryUnit: $("#TotalMonetary_" + i).val(),
					ShipperSUM: PriceSum[i],
					ConsigneeMonetaryUnit: "",
					ConsigneeSUM: "",
					ExchangeSetDate: ExchangedSetDate,
					IsEstimation: IsEstimation
				}
				Call_Ajax("TotalCalc", "/WebService/Admin.asmx/GetTotalPrice", data);
				if (ExchangeCheck == "N") {
					return false;
				}
				$("#TotalPrice_" + i).val(TotalPriceTemp);
			}
			CalcCheck = true;
		}

		function Submit_Charge() {
			if (CalcCheck == false) {
				alert("Total Calc가 필요합니다.");
				return false;
			}
			for (var i = 0; i < 1; i++) {
				if ($("#Branch_" + i).val() == "" || $("#Branch_" + i).val() == null) {
					alert("청구지사와 지불회사를 모두 선택하여 주십시오.");
					return false;
				}
			}
			
			var TableName = "TRANSPORT_HEAD";
			var TablePk = $("#TablePk").val();
			var ChargeNo = $("#ChargeNo").val();
			var StandardPricePk = "";
			var CriterionValue = "";
			var OverWeightValue = "";
			var PaymentWay = "";
			
			var RowLength = $("#ChargeTable > tbody > tr").length;
			var ChargeBodyPk = new Array();
			var SettlementBranch = new Array();
			var Category = new Array();
			var StandardPriceItem = new Array();
			var Title = new Array();
			var Item_MonetaryUnit = new Array();
			var Price = new Array();
			var ChargeHeadPk = new Array();
			var ChargeBranch = new Array();
			var ChargeBranchBank = new Array();
			var ChargeCustomer = new Array();
			var Charge_MonetaryUnit = new Array();
			var ChargeTotalPrice = new Array();
			var ChargeCol = new Array();

			if ($("#BtnBranch_1").val() == "청구회사") {
				THeadCount = 0;
			}
			for (var i = 0; i <= THeadCount; i++) {
				ChargeHeadPk.push($("#ChargeHeadPk_" + i).val());
				ChargeBranch.push($("#Branch_" + i).val());
				ChargeBranchBank.push($("#BranchBank_" + i).val());
				ChargeCustomer.push($("#Customer_" + i).val());
				Charge_MonetaryUnit.push($("#TotalMonetary_" + i).val());
				ChargeTotalPrice.push($("#TotalPrice_" + i).val().replace(/,/g, ''));
			}

			for (var i = 0; i < RowLength; i++) {
				ChargeBodyPk.push($("#ChargeBodyPk_" + i).val());
				SettlementBranch.push($("#SettlementBranch_" + i).val());
				Category.push($("#PriceCategory_" + i).val());
				StandardPriceItem.push($("#StandardPriceItem_" + i).val());
				Title.push($("#Title_" + i).val());
				Item_MonetaryUnit.push($("#Item_MonetaryUnit_" + i).val());
				Price.push($("#PriceItemValue_" + i).val().replace(/,/g, ''));
				ChargeCol[i] = new Array();
				for (var j = 0; j <= THeadCount; j++) {
					if ($("#RadioCharge_" + i + "_" + j).prop("checked") == true) {
						ChargeCol[i][j] = "Y";
					}
					else {
						ChargeCol[i][j] = "N";
					}
				}
			}

			var data = {
				Table_Name: TableName,
				Table_Pk: TablePk,
				Type: Type,
				Charge_No: ChargeNo,
				AlreadyCalc: $("#AlreadyCalc").val(),
				StandardPricePk: StandardPricePk,
				CriterionValue: CriterionValue,
				OverWeightValue: OverWeightValue,
				PaymentWay: PaymentWay,
				ExchangeDate: ExchangeDate,
				ChargeBodyPk: ChargeBodyPk,
				SettlementBranch: SettlementBranch,
				Category: Category,
				StandardPriceItem: StandardPriceItem,
				Title: Title,
				Item_MonetaryUnit: Item_MonetaryUnit,
				Price: Price,
				ChargeHeadPk: ChargeHeadPk,
				ChargeBranch: ChargeBranch,
				ChargeBranchBank: ChargeBranchBank,
				ChargeCustomer: ChargeCustomer,
				Charge_MonetaryUnit: Charge_MonetaryUnit,
				ChargeTotalPrice: ChargeTotalPrice,
				ExchangeRate: ExchangeRate,
				ChargeCol: ChargeCol,
				AccountID: $("#AccountID").val()
			}
			Call_Ajax("Submit", "/Process/ChargeP.asmx/Set_FixCharge", data);
		}

		function Changed_StandardPriceSub(Which) {
			switch (Which) {
				case "List":
					var data = {
						PriceListPk: $("#st_StandardPriceSub").val()
					}
					Call_Ajax("ChangedStandardPriceSub", "/Process/ChargeP.asmx/LoadList_StPriceItem", data);
					break;
				case "Item":
					var data = {
						PriceItemPk: $("#st_StandardItemSub").val()
					}
					Call_Ajax("ChangedStandardItemSub", "/Process/ChargeP.asmx/LoadList_StPriceValue", data);
					break;
				case "Value":
					var data = {
						PriceValuePk: $("#st_StandardValueSub").val()
					}
					Call_Ajax("ChangedStandardValueSub", "/Process/ChargeP.asmx/Get_PriceValueSub", data);
					break;
			}

		}

		function NumberFormat(number) {
			if (number == "" || number == "0") {
				return number;
			}
			else {
				return parseInt(number * 10000) / 10000;
			}
		}

	</script>
</asp:Content>

