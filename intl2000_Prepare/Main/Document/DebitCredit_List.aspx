<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Branch.master" AutoEventWireup="true" CodeFile="DebitCredit_List.aspx.cs" Inherits="Document_DebitCredit_List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<link href="/Lib/jquery-ui.css" rel="stylesheet" />
	<style type="text/css">
		.bg_selected {
			background-color: #c0d4eE;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" Runat="Server">
		<div class="col-sm-12">
			<section class="vbox scrollable wrapper" style="height: 288px;">
				<div class="panel panel-default" style="margin-right: 20px;">
					<div class="panel-heading" style="padding-top:3px; padding-bottom:3px;  ">
						<div id="Pn_ForSendMoney" class=" form-inline" style="float:right; padding-top:3px; ">
							<input type="text" class="form-control" style="width:80px; text-align:center;" id="IssueDate" />
							<input type="text" class="form-control" style="width:100px; text-align:right;" id="TotalAmount" />
							<input type="button" class="btn btn-sm btn-primary" value="선택된 문서 송금" id="BTN_SetSave_Transfer_SelectedRow" />
						</div>
						<h5><strong>History</strong></h5>
					</div>
					<div class="panel-body" >
						<%=Html_List%>
					</div>
				</div>
				<div class="text-center m-t-lg m-b-lg">
					<ul class="pagination pagination-md"></ul>
				</div>
			</section>
		</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" Runat="Server">
	<script src="/Lib/jquery-ui.js"></script>
	<script src="../Lib/Work.js"></script>
	<script type="text/javascript">
		jQuery(document).ready(function () {
			$("#Tab_Work").addClass("active");
			$("#Nav_Work").addClass("active");
			$("#Nav_DebitCredit").css("font-weight", "bold");
			$("#Nav_DebitCredit").css("background-color", "#c0d4eE");
			$("#BTN_SetSave_Transfer_SelectedRow").on("click", SetSave_Transfer);
			$("#IssueDate").datepicker({
				dateFormat: "yy/mm/dd"
			});

			var fullDate = new Date();
			var twoDigitMonth = (fullDate.getMonth()+1) + "";
			if (twoDigitMonth.length == 1) twoDigitMonth = "0" + twoDigitMonth;
			var twoDigitDate = fullDate.getDate() + ""; if (twoDigitDate.length == 1) twoDigitDate = "0" + twoDigitDate;
			$("#IssueDate").val(fullDate.getFullYear() + "/" + twoDigitMonth + "/" + twoDigitDate);
			$("#Pn_ForSendMoney").hide();

			$("#Pn_EachRow > tr").each(function (){
				if ($(this).data("transferid") != "" && $(this).data("transferid")!=undefined) {
					$(this).hide();
				}
			});
		});
		function Row_ShowHide(TransferId) {
			if ($("#BTN_Transfer" + TransferId).val() == "열기") {
				$("#Pn_EachRow > tr").each(function () {
					if ($(this).data("transferid") == TransferId) {
						$(this).show();
					}
				});
				$("#BTN_Transfer" + TransferId).val("닫기");
				$("#BTN_CancelTransfer" + TransferId).show();
			} else {
				$("#Pn_EachRow > tr").each(function () {
					if ($(this).data("transferid") == TransferId) {
						$(this).hide();
					}
				});
				$("#BTN_Transfer" + TransferId).val("열기");
				$("#BTN_CancelTransfer" + TransferId).hide();
			}
		}
		function SetSave_Transfer() {
			var Sum_DocumentPk = "";
			for (var i = 0; i < $("#Pn_EachRow > tr").length; i++) {
				if ($("#Pn_EachRow" + i).hasClass("bg_selected")) {
					Sum_DocumentPk += ", " + $("#Pn_EachRow" + i).data("documentpk");
				}
			}

			$.ajax({
				type: "POST",
				url: "/Process/DocumentP.asmx/SetSave_Transfer",
				data: "{IssueDate:\"" + $("#IssueDate").val() + "\", SumDocumentPk:\"" + Sum_DocumentPk.substr(1) + "\" }",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (result) {
					alert("성공");
					location.reload();
				},
				error: function (msg) {
					alert('failure : ' + msg);
					console.log(msg);
				}
			});
		}
		function ChooseRow(SelectedId) {
			$("#" + SelectedId).toggleClass("bg_selected");
			Calc_SelectedAmount();
		}
		function Calc_SelectedAmount() {
			var TotalAmount = 0;
			for (var i = 0; i < $("#Pn_EachRow > tr").length; i++) {
				if ($("#Pn_EachRow" + i).hasClass("bg_selected")) {
					TotalAmount += parseFloat($("#Pn_EachRow" + i).data("amount"));
				}
			}
			if (TotalAmount == 0) {
				$("#Pn_ForSendMoney").hide();
			} else {
				$("#Pn_ForSendMoney").show();
				$("#TotalAmount").val(TotalAmount);
			}
		}

		function Goto(Type, TBBHPk) {
			switch (Type) {
				case "DebitCredit_Detail":
					window.open('/Document/DebitCredit_Detail.aspx?S=' + TBBHPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, top=20px; left=200px; height=700px; width=800px;');
					break;
				case "Credit_View":
					window.open('/Document/DebitCredit_View.aspx?PageType=Credit&S=' + TBBHPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, top=20px; left=200px; height=700px; width=850px;');
					break;
				case "Credit_Print":
					window.open('/Document/DebitCredit_View.aspx?Mode=Print&PageType=Credit&S=' + TBBHPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, top=20px; left=200px; height=700px; width=850px;');
					break;
				case "Debit_View":
					window.open('/Document/DebitCredit_View.aspx?PageType=Debit&S=' + TBBHPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, top=20px; left=200px; height=700px; width=850px;');
					break;
				case "Debit_Print":
					window.open('/Document/DebitCredit_View.aspx?Mode=Print&PageType=Debit&S=' + TBBHPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, top=20px; left=200px; height=700px; width=850px;');
					break;
				case "DebitCredit_First":
					window.open('/Document/DebitCredit_FirstPage.aspx?Mode=Print&S=' + TBBHPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=yes, top=20px; left=200px; height=700px; width=850px;');
					break;
			}
		}
		function Cancel_Transfer(TransferId) {
			if (confirm("송금 취소하시겠습니까?")) {
				$.ajax({
					type: "POST",
					url: "/Process/DocumentP.asmx/SetCancel_Transfer",
					data: "{TransferId:\"" + TransferId + "\"}",
					contentType: "application/json; charset=utf-8",
					dataType: "json",
					success: function (result) {
						if (result.d == "1") {
							alert("성공");
							location.reload();
						}
					},
					error: function (msg) {
						alert('failure : ' + msg);
						console.log(msg);
					}
				});

			}
		}
		function OpenAll_Transfer(Type, TransferId) {
			$("#Pn_EachRow > tr").each(function () {
				if ($(this).data("transferid") == TransferId) {
					if (Type == "Debit") {
						Goto("Debit_View", $(this).data("typepk"));
					} else {
						Goto("Credit_View", $(this).data("typepk"));
					}
				}
			});
		}
	</script>
</asp:Content>