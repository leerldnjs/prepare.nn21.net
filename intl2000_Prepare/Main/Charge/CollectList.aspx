<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Admin_Scale.master" AutoEventWireup="true" CodeFile="CollectList.aspx.cs" Inherits="Charge_CollectList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" Runat="Server">

	<input type="hidden" id="BranchPk" value="<%=MEMBERINFO[1] %>" />

	<div class="panel panel-warning col-xs-10 col-xs-offset-1">
		<div class="panel-body form-horizontal">
			<div class="row">
				<label class="col-xs-1 control-label" style="text-align: right;">통장</label>
				<div class="col-xs-4">
					<select class="form-control" id="St_BankList"></select>
				</div>
			</div>
			<div class="row" style="margin-top:5px;">
				<label class="col-xs-1 control-label" style="text-align: right;">일자</label>
				<div class="col-xs-2">
					<input type="text" class="form-control" style="text-align:center;" readonly="readonly" id="RetrieveDate" value="<%=DateTime.Now.ToString("yyyy-MM-dd") %>" />
				</div>
				<div class="col-xs-2">
					<input type="button" class="form-control btn btn-sm btn-success" id="Btn_Retrieve" value="조회" />
				</div>
			</div>
			<div class="row" style="margin-top:5px;">
				<label class="col-xs-1 control-label" style="text-align: right;">적요</label>
				<div class="col-xs-1">
					<input type="text" class="form-control" style="text-align:center;" id="InputDate" value="<%=DateTime.Now.ToString("HH:mm") %>" />
				</div>
				<div class="col-xs-2">
					<input type="text" class="form-control" id="Description" placeholder="적요" />
				</div>
				<div class="col-xs-1">
					<select class="form-control" id="BankInOut">
						<option value="+">입금</option>
						<option value="-">출금</option>
					</select>
				</div>
				<label class="col-xs-1 control-label" style="text-align: right; width:30px;" id="MonetaryUnit">￦</label>
				<div class="col-xs-2">
					<input type="text" class="form-control" id="Price"/> 
				</div>
				<div class="col-xs-1">
					<input type="button" class="form-control btn btn-sm btn-primary" id="Btn_Submit" value="저장" />

				</div>
			</div>

		</div>

		<div class="panel panel-default">
			<div class="panel-body form-horizontal">
				<table class="table b-t b-light text-sm">
					<thead class="bg-primary">
						<tr>
							<th style="text-align: left;">거래일시</th>
							<th style="text-align: left;">적요</th>
							<th style="text-align: left;">입금</th>
							<th style="text-align: left;">출금</th>
							<th style="text-align: left;">잔액</th>
						</tr>
					</thead>
					<tbody id="DepositHis">
					</tbody>
				</table>
			</div>
		</div>




	</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" Runat="Server">
	<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
	<script type="text/javascript">
		
		$(document).ready(function () {

			$("#Btn_Retrieve").on("click", LoadBank);
			$("#Btn_Submit").on("click", SubmitBank);

			$("#RetrieveDate").datepicker({
				dateFormat: "yy-mm-dd"
			});

			var data = {
				BranchPk: $("#BranchPk").val()
			}
			Call_Ajax("LoadBankList", "/Process/ChargeP.asmx/Load_CompanyBank", data);

		});

		function LoadBank() {
			var data = {
				BankPk: $("#St_BankList").val(),
				RetrieveDate: $("#RetrieveDate").val()
			}
			Call_Ajax("LoadBank", "/Charge/CollectList.aspx/MakeHtml_DepositHis", data);
		}

		function SubmitBank() {
			var InOut = $("#BankInOut").val();
			
			if ($("#Price").val() == "") {
				alert("금액을 넣어주세요.");
				return false;
			}

			var data = {
				BankPk: $("#St_BankList").val(),
				Type: "",
				TypePk: "",
				Description: $("#Description").val(),
				MonetaryUnit: $("#MonetaryUnit").text(),
				Price: $("#BankInOut").val() + $("#Price").val(),
				DateTime: $("#RetrieveDate").val().trim() + " " + $("#InputDate").val().trim()
			}
			Call_Ajax("Submit", "/WebService/Finance.asmx/SetSave_BankDeposit", data);
			
		}

		function DelDeposit(DepositPk) {
			if (confirm("삭제하시겠습니까?")) {
				var data = {
					BankDepositPk: DepositPk
				}
				Call_Ajax("Delete", "/WebService/Finance.asmx/SetDelete_BankDeposit", data);
			}
		}

		function Call_Ajax(TarGet, Url, Data) {
			$.ajax({
				type: "POST",
				url: Url,
				data: JSON.stringify(Data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					switch (TarGet) {
						case "LoadBankList":
							$("#St_BankList").html(result.d);
							break;
						case "LoadBank":
							$("#DepositHis").html(result.d);
							break;
						case "Submit":
							LoadBank();
							break;
						case "Delete":
							LoadBank();
							break;
					}
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});
		}

	</script>

</asp:Content>

