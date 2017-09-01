<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Admin_Scale.master" AutoEventWireup="true" CodeFile="Storage.aspx.cs" Inherits="Transport_Storage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<html class="app">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" runat="Server">

	<input type="hidden" id="HBranchPk" value="<%=BranchPk %>" />
	<input type="hidden" id="HAccountID" value="<%=AccountID %>" />

	<div class="col-xs-11 col-xs-offset-1">
	<section class="hbox stretch">
		<section id="StorageContent">
			<section class="vbox">
				<section class="scrollable">
					<div class="slim-scroll" data-height="auto" data-disable-fade-out="true" data-distance="0" data-size="10px" data-railopacity="0.2">
						<!--content-->
						<!-- 메뉴상단 Tab Menu TRANSPORT_HEAD -->
						<div class="panel panel-warning">
							<div class="panel-body form-horizontal">
								<%=Html_TransportHead_WithPacked %>
							</div>
						</div>
						<!-- 메뉴상단 Tab Menu TRANSPORT_HEAD -->

						<!-- 메뉴하단 Tab Menu_STORAGE RETRIEVE TRANSPORT_PACKED -->
						<div class="panel panel-warning">
							<div class="panel-body form-horizontal">
								
								<!--
								<p style="font-weight: bold;">CONTAINER</p>
								<div class="panel-body form-horizontal">
									<%=Html_TransportPacked %>
								</div>
								-->

								<p style="font-weight: bold; margin-top: 5px;">STORAGE</p>

								<%=Html_StorageTab %>

								<div class="tab-content" id="tab_StorageContent">
									<div class="tab-pane" id="tab_3">
										asfasdfsadf
									</div>
								</div>

							</div>
						</div>
					</div>
					<!-- 메뉴하단 Tab Menu_STORAGE RETRIEVE TRANSPORT_PACKED -->
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
				</section>
			</section>
		</section>
		<!-- .aside -->
		<aside class="aside-md" id="nav" style="background-color: #ffffff; width: 160px;">

			<div class="panel panel-warning">
				<div class="panel-body">
					<div style="margin-bottom: 5px; margin-top: 130px;">
						<div class="col-xs-6"><span>Box</span></div><div class="col-xs-6"><span id="SumBox" class="badge bg-danger text-lg">0</span></div>
					</div>
					<div style="margin-bottom: 5px;">
						<div class="col-xs-6"><span>Volume</span></div><div class="col-xs-6"><span id="SumCbm" class="badge bg-danger text-lg">0</span></div>
					</div>
					<div>
						<div class="col-xs-6"><span>Weight</span></div><div class="col-xs-6"><span id="SumKg" class="badge bg-danger text-lg">0</span></div>
					</div>

					<input type="button" id="BTN_PopHead" class="btn btn-dark btn-sm" style="width: 125px; margin-bottom: 2px; margin-top:70px;" value="<%=GetGlobalResourceObject("qjsdur", "dnsthddPdir") %>" />
					<input type="button" id="BTN_PopPacked" class="btn btn-dark btn-sm" style="width: 125px; margin-bottom: 2px;" value="<%=GetGlobalResourceObject("qjsdur", "zjsxpdlsjtodtjd") %>" />
					<input type="button" id="BTN_EditStorage" class="btn btn-dark btn-sm" style="width: 125px; margin-bottom: 50px;" value="<%=GetGlobalResourceObject("qjsdur", "worhtnwjd") %>" />
					<input type="button" id="BTN_ToHead" class="btn btn-dark btn-sm" style="width: 125px; margin-bottom: 2px;" value="-> <%=GetGlobalResourceObject("qjsdur", "dPdir") %>" />
					<input type="button" id="BTN_ToPacked" class="btn btn-dark btn-sm" style="width: 125px; margin-bottom: 2px;" value="-> <%=GetGlobalResourceObject("qjsdur", "zjsxpdlsj") %>" />
					<input type="button" id="BTN_ToStorage" class="btn btn-dark btn-sm" style="width: 125px; margin-bottom: 30px;" value="-> <%=GetGlobalResourceObject("qjsdur", "ckdrh") %>" />
				</div>
			</div>

		</aside>
		<!-- /.aside -->
	</section>
		</div>

	<input type="hidden" id="BranchPk" name="BranchPk" value="<%=MemberInformation[1] %>"/>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" runat="Server">
	<script src="/Lib/ForDialog.js"></script>
	<script type="text/javascript">
		var SumBox;
		var SumCbm;
		var SumKg;
		var FromTypeChk;
		var FromTypeChk_FullName;
		var FromType;
		var FromTypePk_s;
		var FromTypePk_Array = new Array();
		var Count_s; 
		var Count_Array = new Array();
		var EditStoragePk;
		var EditCount;

		$(document).ready(function () {

			$("#BTN_PopHead").on("click", PopHead);
			$("#BTN_PopPacked").on("click", PopPacked);
			$("#BTN_ToHead").on("click", function () {
				if (FromTypeChk != "ChkWaitPacked" && FromTypeChk != "ChkStorage") {
					alert("컨테이너 또는 화물이 선택되지 않았습니다.");
					return false;
				}
				ForModal("DialogHead");
			});
			$("#BTN_ToPacked").on("click", function () {
				if ((FromTypeChk != "ChkHeadPacked" && FromTypeChk != "ChkStorage")) {
					alert("'Head의 컨테이너' 또는 'Storage의 재고가' 선택되지 않았습니다.");
					return false;
				}
				if (FromTypeChk == "ChkHeadPacked") {
					var r = confirm("Packed To Wait?")
					if (r == false) {
						return false;
					}
					else {
						FromType = "Packed";
						Target = "ToPacked";
						Url = "/WebService/TransportP.asmx/TransportToPacked";
						data = {
							PackedPk: ",!",
							FromType: FromType,
							FromTypePk_s: FromTypePk_s,
							Count_s: Count_s
						}
						Call_Ajax("ToPacked", Url, data);
					}
				}
				else {
					ForModal("DialogPacked");
				}
			});
			$("#BTN_ToStorage").on("click", function () {
				if ((FromTypeChk != "ChkPackedBody" && FromTypeChk != "ChkStorage")) {
					alert("'Packed의 재고' 또는 '창고를 이동 할 Storage의 재고가' 선택되지 않았습니다.");
					return false;
				}
				ForModal("DialogStorage");
			});
			$("#BTN_EditStorage").on("click", function () {
				Edit_Storage();
			});

		});

		function Edit_Storage() {
			Chk_Onclick("ChkStorage");
			var data = {
				StoragePk: EditStoragePk,
				EditCount: EditCount
			}
			if ($('input:checkbox:checked').length != 1) {
				alert("재고를 수정할 1개의 화물만 선택 하십시오.");
				return false;
			}
			Call_Ajax("EditStorage", "/WebService/TransportP.asmx/TransportEditStorage", data);
		}

		function Delete_Packed(PackedPk) {
			var r = confirm("컨테이너를 삭제하시겠습니까?");
			if (r == false) {
				return false;
			}

			var data = {
				Transport_Packed_Pk: PackedPk
			}
			Call_Ajax("DeletePacked", "/WebService/TransportP.asmx/Delete_TrarnsportPacked", data);
		}

		function ForModal(ToTarget) {
			var data = {
				Type: ToTarget
			}
			Call_Ajax(ToTarget, "/Transport/Storage.aspx/MakeHtml_Modal", data);
			$("#ToModal").modal("show");
		}

		function Click_Dialog(Type, TypePk) {
			var Target;
			var Url;
			var data;
			Chk_Onclick(FromTypeChk_FullName);

			switch (Type) {
				case "ToHead":
					var r = confirm(Type + "_" + TypePk + " ?");
					if (r == false) {
						return false;
					}

					if (FromTypeChk == "ChkWaitPacked") {
						FromType = "Packed";
					}
					else {
						FromType = "Storage";
					}

					Target = "ToHead";
					Url = "/WebService/TransportP.asmx/TransportToHead";
					data = {
						HeadPk: TypePk,
						FromType: FromType,
						FromTypePk_s: FromTypePk_s,
						Count_s: Count_s
					}
					break;
				case "ToPacked":
					var r = confirm(Type + "_" + TypePk + " ?");
					if (r == false) {
						return false;
					}

					FromType = "Storage";
					Target = "ToPacked";
					Url = "/WebService/TransportP.asmx/TransportToPacked";
					data = {
						PackedPk: TypePk,
						FromType: FromType,
						FromTypePk_s: FromTypePk_s,
						Count_s: Count_s
					}
					break;
				case "ToStorage":
					var r = confirm(Type + "_" + TypePk + " ?");
					if (r == false) {
						return false;
					}
					if (FromTypeChk == "ChkPackedBody") {
						FromType = "Packed";
					}
					else {
						FromType = "Storage";
					}
					Target = "ToStorage";
					Url = "/WebService/TransportP.asmx/TransportToStorage";
					data = {
						WarehousePk: TypePk,
						FromType: FromType,
						FromTypePk_s: FromTypePk_s,
						Count_s: Count_s
					}
					break;
			}
			Call_Ajax(Target, Url, data);

		}

		function PopHead() {
			window.open('/Transport/Dialog/TransportHead.aspx', '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=400px; width=800px;')
		}

		function PopPacked() {
			ForDialog.initPopup();

			var PopWindow = "pop_win" + "asdf";
			var Win = window.open('', PopWindow, 'width=780, height=464,menubar=no,status=no,scrollbars=yes');
			$("#ForDialog_Type").val("null");
			$("#ForDialog_TypePk").val("null");
			$("#ForDialog_CompanyPk").val($("#BranchPk").val());

			$("#ForDialog").attr('target', PopWindow);
			$("#ForDialog").attr('action', '/Transport/Dialog/TransportPacked.aspx');
			$("#ForDialog").submit();
		}

		function Chk_Onclick(Which) {
			var WhichData = Which.split("_");
			FromTypePk_s = "";
			Count_s = "";
			Count_Array = new Array();
			FromTypePk_Array = new Array();
			SumBox = 0;
			SumCbm = 0;
			SumKg = 0;
			$("#SumBox").val(SumBox);
			$("#SumCbm").val(SumCbm);
			$("#SumKg").val(SumKg);
			$('input:checkbox').each(function () {
				if ((this.name == WhichData[0] && this.checked == true) || (this.name == Which && this.checked == true)) {
					FromTypePk_Array.push(this.value);
					FromTypePk_s = FromTypePk_Array.join(",!");
					FromTypeChk = WhichData[0];
					FromTypeChk_FullName = Which;

					if (WhichData[0] == "ChkStorage" || WhichData[0] == "ChkHeadBody" || WhichData[0] == "ChkPackedBody") {
						Count_Array.push($("#" + this.id + "_Count").val());
						Count_s = Count_Array.join(",!");

						SumBox += parseInt($("#" + this.id + "_Count").val());
						SumCbm += parseInt($("#" + WhichData[0] + "_" + WhichData[1] + "_Volume").text());
						SumKg += parseInt($("#" + WhichData[0] + "_" + WhichData[1] + "_Weight").text());
						$("#SumBox").text(SumBox);
						$("#SumCbm").text(SumCbm);
						$("#SumKg").text(SumKg);

						if (WhichData[0] == "ChkStorage") {
							EditStoragePk = $("#" + this.id).val();
							EditCount = $("#" + this.id + "_Count").val();
						}
					}
					if (WhichData[0] == "ChkHeadPacked" || WhichData[0] == "ChkWaitPacked") {
						RetrieveBody(parseInt(FromTypePk_s))
					}
				}
				else {
					this.checked = false;
				}
			});
			
		}

		function RetrieveBody(TransportPackedPk) {
			var data = {
				TransportPackedPk: TransportPackedPk,
				Type: FromTypeChk
			}
			Call_Ajax("RetrieveBody", "/Transport/Storage.aspx/MakeHtml_TransportBody", data);
		}

		function RetrieveItem(WarehousePk) {
			var data = {
				WarehousePk: WarehousePk
			}
			Call_Ajax("RetrieveItem", "/Transport/Storage.aspx/MakeHtml_StorageItem", data);
		}

		function Comment(HeadPk) {
			var data = {
				Table_Name: "TRANSPORT_HEAD",
				Table_Pk: HeadPk,
				Category: "TransportHead",
				Contents: $("#Comment_" + HeadPk).val(),
				Account_Id: $("#HAccountID").val()
			}
			Call_Ajax("Set_Comment", "/Process/HistoryP.asmx/Set_Comment", data);
		}

		function Transport_Send(HeadPk) {
			var data = {
				TransportHeadPk: HeadPk
			}
			Call_Ajax("Transport_Send", "/Transport/Storage.aspx/Transport_Send", data);

		}

		function Transport_Cancle(HeadPk) {
			var data = {
				TransportHeadPk: HeadPk
			}
			Call_Ajax("Transport_Cancle", "/Transport/Storage.aspx/Transport_Cancle", data);

		}

		function Call_Ajax(Type, Url, Data) {

			$.ajax({
				type: "POST",
				url: Url,
				data: JSON.stringify(Data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					switch (Type) {
						case "DialogHead":
							$("#ToModal_Body").html(result.d);
							break;
						case "DialogPacked":
							$("#ToModal_Body").html(result.d);
							break;
						case "DialogStorage":
							$("#ToModal_Body").html(result.d);
							break;
						case "ToHead":
							location.reload();
							break;
						case "ToPacked":
							location.reload();
							break;
						case "ToStorage":
							location.reload();
							break;
						case "RetrieveBody":
							var PackedPk = JSON.stringify(Data).split(":");
							if (FromTypeChk == "ChkHeadPacked") {
								$("#Body_" + parseInt(PackedPk[1])).html(result.d);
							}
							else if (FromTypeChk == "ChkWaitPacked") {
								$("#Pn_PackedBody").html(result.d);
							}
							break;
						case "RetrieveItem":
							$("#tab_StorageContent").html(result.d);
							break;
						case "EditStorage":
							location.reload();
							break;
						case "DeletePacked":
							if (result.d == "-1") {
								alert("컨테이너 내에 화물이 존재합니다.");
							}
							else {
								location.reload();
							}
							break;
						case "Set_Comment":
							location.reload();
							break;
						case "Transport_Send":
							alert("Success");
							location.reload();
							break;
						case "Transport_Cancle":
							if (result.d == "1") {
								alert("Success");
								location.reload();
								break;
							}
							else {
								alert("화물이 존재합니다. 화물을 모두 재고로 이동 후 삭제하십시오.");
								break;
							}
					}
				},
				error: function (result) {
					alert('failure : ' + result)
				}
			});
		}

	</script>
</asp:Content>

