<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Clean.Master" AutoEventWireup="true" CodeFile="Logistics_Route.aspx.cs" Inherits="Board_Logistics_Route" %>

<%@ Register Src="/Board/BoardList.ascx" TagName="BoardList" TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
	<link rel="stylesheet" type="text/css" href="/Common/IntlWeb.css" />
	<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
	<link rel="stylesheet" href="/resources/demos/style.css">
	<style>
		* RouteBody{
			font-size:9px !important;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" runat="Server">

	<body style="margin: 0 auto; background-repeat: repeat-x; background-color: #FFFFFF;" background="/Images/Board/top_bg.gif">
		<form id="form2" runat="server">
			<asp:ScriptManager ID="SM" runat="server">
				<Services>
					<asp:ServiceReference Path="~/WebService/Board.asmx" />
				</Services>
			</asp:ScriptManager>

			<table align="center" border="0" cellpadding="0" cellspacing="0" style="width: 1200px; height: 600px;">
				<tr>
					<td colspan="2" height="108" align="center"><font style="font-family: arial; font-size: 20px; font-weight: bold; color: #FFFFFF;">International Logistic Board</font></td>
				</tr>
				<tr>
					<td valign="bottom" style="height: 50px; padding-left: 10px;">
						<input type="hidden" id="HBoardCode" value="<%=Request.Params["C"] + "" %>" />
						<img src="../Images/Board/id.gif" align="absmiddle">
						<%=MemberInfo[2] + "" %>
						<br />
						<img src="../Images/Board/name.gif" align="absmiddle">
						<%=MemberInfo[3] + "" %>
					</td>
					<td align="right" valign="bottom">
						<% if (Request.Params["CompanyPk"] + "" != "10520") { %>
						<a href="/Admin/RequestList.aspx?G=5051">
							<img src="../Images/Board/link.gif" align="absmiddle" border="0"></a>
						<% }
							else { %>
						<a href="/Process/Logout.aspx">
							<img src="/Images/Board/Logout.jpg" align="absmiddle" border="0"></a>
						<%} %>
					</td>
				</tr>
				<tr>
					<td valign="top" style="width: 200px; padding-top: 10px;">
						<table align="center" cellpadding="0" cellspacing="0" style="width: 200px; border-style: dotted; border-width: 2; border-color: #C8C8C8;">
							<tr>
								<td align="center" style="height: 40px;"><a href="BoardMain.aspx">
									<img src="../Images/Board/main.gif" align="absmiddle" border="0"></a></td>
							</tr>
							<uc1:BoardList ID="BoardList1" runat="server" />
							<tr>
								<td style="height: 20px;">&nbsp;</td>
							</tr>
						</table>
					</td>
					<td valign="top" style="width: 1000px; padding-top: 10px;">
						<table cellpadding="0" cellspacing="0" style="width: 990px; margin-left: 10px;">
							<tr>
								<td align="left" style="width: 990px; height: 40px; padding-left: 20px;">
									<img src="../Images/Board/bar.gif" align="absmiddle">&nbsp;
                                <font style="font-family: Vernada; font-size: 16px; font-weight: bold;">노선관리</font>
								</td>
							</tr>
							<tr>
								<td>
									<table border="1" cellpadding="0" cellspacing="0" style="width: 990px; border-collapse: collapse;">



										<!--***********************************************************Logistics_Route****************************************************************-->

										<div class="col-xs-12 small">
											<div class="panel panel-default">
												<div class="panel-heading" style="font-weight: bold;">노선등록</div>
												<div class="panel-body form-horizontal">

													<input name="SavedPk_Branch_From" class="form-control" id="SavedPk_Branch_From" type="hidden" />
													<input name="SavedPk_Branch_To" class="form-control" id="SavedPk_Branch_To" type="hidden" />
													<input name="BranchPk" class="form-control" id="BranchPk" type="hidden" value="<%=Request.Params["CompanyPk"] %>" />

													<div class="form-group">
														<div class="row">
															<div class="col-xs-1 control-label font-bold">출발</div>
															<div class="col-xs-2 col-xs-offset-1">
																<input type="text" readonly="readonly" value="" id="From_Continent" class="form-control" placeholder="대륙" />
															</div>
															<div class="col-xs-2">
																<input type="text" readonly="readonly" value="" id="From_Country" class="form-control" placeholder="국가" />
															</div>
															<div class="col-xs-2">
																<input type="text" readonly="readonly" value="" id="From_Area" class="form-control" placeholder="지역" />
															</div>
															<div class="col-xs-2">
																<input type="text" readonly="readonly" value="" id="From_Branch" class="form-control" placeholder="지점" />
															</div>
														</div>
													</div>

													<div class="form-group">
														<div class="row">
															<div class="col-xs-1 control-label font-bold">도착</div>
															<div class="col-xs-2 col-xs-offset-1">
																<input type="text" readonly="readonly" value="" id="To_Continent" class="form-control" placeholder="대륙" />
															</div>
															<div class="col-xs-2">
																<input type="text" readonly="readonly" value="" id="To_Country" class="form-control" placeholder="국가" />
															</div>
															<div class="col-xs-2">
																<input type="text" readonly="readonly" value="" id="To_Area" class="form-control" placeholder="지역" />
															</div>
															<div class="col-xs-2">
																<input type="text" readonly="readonly" value="" id="To_Branch" class="form-control" placeholder="지점" />
															</div>
														</div>
													</div>

													<div class="form-group">
														<div class="row">
															<div class="col-xs-1 control-label font-bold">구분</div>
															<div class="col-xs-2 col-xs-offset-7">
																<select id="st_TransportCategory" class="form-control">
																	<option value="해상운송">해상운송</option>
																	<option value="복합운송">복합운송</option>
																	<option value="항공운송">항공운송</option>
																	<option value="육상운송">육상운송</option>
																</select>
															</div>
														</div>
													</div>
													<div>

													</div>

													<div class="form-group" style="padding-top: 20px;">
														<div class="col-xs-2 col-xs-offset-4">
															<input type="button" name="BTN_Retrieve" id="BTN_Retrieve" class="btn btn-success btn-sm btn-block" value="노선일정조회" style="display:none;"/>
														</div>
														<div class="col-xs-2">
															<input type="button" name="BTN_Submit" id="BTN_Submit" class="btn btn-primary btn-sm btn-block" value="노선저장" />
														</div>
														<div class="col-xs-2">
															<input type="button" name="BTN_Delete" id="BTN_Delete" class="btn btn-danger btn-sm btn-block" value="노선삭제" style="display:none;"/>
														</div>
													</div>

													<hr />
													<hr />

													<div class="form-group" id="Schedule">
													</div>

												</div>
												<div class="panel-footer">
													<a class="text-xs"> - 물류노선을 선택 후 노선일정을 조회하면 노선의 수정 및 일정추가가 가능합니다.<br /></a>
												</div>
											</div>
										</div>
										<div class="col-xs-12 small">
											<div class="panel panel-default">
												<div class="panel-heading" style="font-weight: bold;">물류노선</div>
												<div class="panel-body form-horizontal">
													<div id="Route_Body">

													</div>
												</div>
											</div>
										</div>

										<!--***********************************************************Logistics_Route****************************************************************-->
									</table>


								</td>
							</tr>

						</table>
					</td>
				</tr>
				<tr>
					<td colspan="2" width="1200">
						<table border="0" cellpadding="0" cellspacing="0" style="width: 1200px; margin-top: 20px;">
							<tr>
								<td align="center" style="background-color: #EDEBEB; height: 50px;">Copyright(c) 2012 International Logistics CO.,LTD &nbsp;&nbsp;&nbsp;&nbsp;    All right reserved.</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>

		</form>



		<form id='Form_SavedAdd'>
			<input type='hidden' name='BranchPk' id='Modal_SavedAdd_BranchPk' />
			<input type='hidden' name='Code' id='Modal_SavedAdd_Code' />
			<input type='hidden' name='Value_0' id='Modal_SavedAdd_Value_0' />
			<input type='hidden' name='Value_1' id='Modal_SavedAdd_Value_1' />
			<input type='hidden' name='Value_2' id='Modal_SavedAdd_Value_2' />
			<input type='hidden' name='Value_3' id='Modal_SavedAdd_Value_3' />
			<input type='hidden' name='Value_4' id='Modal_SavedAdd_Value_4' />
			<input type='hidden' name='Value_Int_0' id='Modal_SavedAdd_Value_Int_0' />
		</form>
		

	</body>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" runat="Server">
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
	<script src="/Lib/ForDialog.js"></script>
	<script type="text/javascript">
		var ModalStatus = "";
		var RoutePk_Main = "";
		var VesselId = "";
		var ShipWayId = "";
		var ContainerId = "";
		var AddContainer_Vessel = new Array();
		$(document).ready(function () {
			
			var data = {
				QueryWhere:""
			}
			$.ajax({
				type: "POST",
				url: "Logistics_Route.aspx/MakeHtml_RouteAll",
				data: JSON.stringify(data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					$("#Route_Body").html(result.d);
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});

			$("#BTN_Submit").on("click", function () {
				Set_Document("Route", "", RoutePk_Main, "");
			});
			$("#BTN_Delete").on("click", function () {
				Delete_Route("");
			});

			$("#From_Continent").on("click", function () {
				ModalStatus = "Modal_From_Continent";
				ModalOpen_Saved("nn21com_Route_Continent", "");
			});
			$("#From_Country").on("click", function () {
				ModalStatus = "Modal_From_Country";
				ModalOpen_Saved("nn21com_Route_Country", $("#From_Continent").val());
			});
			$("#From_Area").on("click", function () {
				ModalStatus = "Modal_From_Area";
				ModalOpen_Saved("nn21com_Route_Area", $("#From_Continent").val() + ",!" + $("#From_Country").val());
			});
			$("#From_Branch").on("click", function () {
				ModalStatus = "Modal_From_Branch";
				ModalOpen_Saved("nn21com_Route_Branch", $("#From_Continent").val() + ",!" + $("#From_Country").val() + ",!" + $("#From_Area").val());
			});
			$("#To_Continent").on("click", function () {
				ModalStatus = "Modal_To_Continent";
				ModalOpen_Saved("nn21com_Route_Continent", "");
			});
			$("#To_Country").on("click", function () {
				ModalStatus = "Modal_To_Country";
				ModalOpen_Saved("nn21com_Route_Country", $("#To_Continent").val());
			});
			$("#To_Area").on("click", function () {
				ModalStatus = "Modal_To_Area";
				ModalOpen_Saved("nn21com_Route_Area", $("#To_Continent").val() + ",!" + $("#To_Country").val());
			});
			$("#To_Branch").on("click", function () {
				ModalStatus = "Modal_To_Branch";
				ModalOpen_Saved("nn21com_Route_Branch", $("#To_Continent").val() + ",!" + $("#To_Country").val() + ",!" + $("#To_Area").val());
			});

			$("#BTN_Retrieve").on("click", Retrieve_Route);

			ForDialog.Init("sm", "Modal_Default");
		});

		function Dom_Connect() {
			for (var i = 0; i < 50; i++) {
				$("#VesselName_" + i).on("click", function () {
					ModalStatus = "Modal_VesselName";
					VesselId = this.id;
					ModalOpen_Saved("nn21com_Route_VesselName", "");
				});
				$("#ShipWay_" + i).on("click", function () {
					ModalStatus = "Modal_ShipWay";
					ShipWayId = this.id;
					ModalOpen_Saved("nn21com_Route_ShipWay", $("#st_TransportCategory").val());
				});
				$("#Container_" + i).on("click", function () {
					ModalStatus = "Modal_Container";
					ContainerId = this.id;
					ModalOpen_Saved("nn21com_Route_Container", $("#st_TransportCategory").val());
				});
				$("#Datetime_Dead_" + i).datepicker({
					dayNames: ['일요일', '월요일', '화요일', '수요일', '목요일', '금요일', '토요일'], 
					dayNamesShort: ['일', '월', '화', '수', '목', '금', '토'], 
					dateFormat: "(D) yy.mm.dd"
				});
				$("#Datetime_From_" + i).datepicker({
					dayNames: ['일요일', '월요일', '화요일', '수요일', '목요일', '금요일', '토요일'],
					dayNamesShort: ['일', '월', '화', '수', '목', '금', '토'],
					dateFormat: "(D) yy.mm.dd"
				});
				$("#Datetime_To_" + i).datepicker({
					dayNames: ['일요일', '월요일', '화요일', '수요일', '목요일', '금요일', '토요일'],
					dayNamesShort: ['일', '월', '화', '수', '목', '금', '토'],
					dateFormat: "(D) yy.mm.dd"
				});
			}
		}

		function ChooseSaved(ValueSum) {
			if (ValueSum != "") {
				var Saved_Data = ValueSum.split(",!");
				switch (ModalStatus) {
					case "Modal_From_Continent":
						$("#From_Continent").val(Saved_Data[1]);
						break;
					case "Modal_From_Country":
						$("#From_Country").val(Saved_Data[1]);
						break;
					case "Modal_From_Area":
						$("#From_Area").val(Saved_Data[1]);
						break;
					case "Modal_From_Branch":
						$("#From_Branch").val(Saved_Data[1]);
						break;
					case "Modal_To_Continent":
						$("#To_Continent").val(Saved_Data[1]);
						break;
					case "Modal_To_Country":
						$("#To_Country").val(Saved_Data[1]);
						break;
					case "Modal_To_Area":
						$("#To_Area").val(Saved_Data[1]);
						break;
					case "Modal_To_Branch":
						$("#To_Branch").val(Saved_Data[1]);
						break;
					case "Modal_VesselName":
						$("#" + VesselId).val(Saved_Data[1]);
						break;
					case "Modal_ShipWay":
						$("#" + ShipWayId).val(Saved_Data[1]);
						break;
					case "Modal_Container":
						$("#" + ContainerId).val(Saved_Data[1]);
						break;
				}
			}
			$('#Modal_Default').modal('hide');
		}

		function Delete_Saved(ValueSum) {
			if (ValueSum != "") {
				var Saved_Data = ValueSum.split(",!");
				var data = {
					SavedPk: Saved_Data[0]
				}

				$.ajax({
					type: "POST",
					url: "/WebService/DocumentP.asmx/Delete_Saved",
					data: JSON.stringify(data),
					dataType: "json",
					contentType: "application/json; charset=utf-8",
					success: function (result) {
						alert("삭제완료.");
						$('#Modal_Default').modal('hide');
					},
					error: function (result) {
						alert('failure : ' + result);
					}
				});
			}
		}

		function Set_Document(Flag, Row, RoutePk, Container) {
			if ($("#From_Branch").val() == "" || $("#To_Branch").val() == "" || $("#st_TransportCategory").val() == "") {
				alert("출발지점 | 도착지점 | 운송구분은 반드시 입력되어야 합니다.");
				return false;
			}
			var DupFlag = "N";

			if (Flag == "Route") {
				var Status = "0";
				var VesselName = "";
				var ShipWay = "";
				var Container = "";
				var Datetime_Dead = "";
				var Datetime_From = "";
				var Datetime_Take = "";
				var Datetime_To = "";

				var Dupdata = {
					Value0: $("#From_Continent").val(),
					Value1: $("#From_Country").val(),
					Value2: $("#From_Area").val(),
					Value3: $("#From_Branch").val(),
					Value4: $("#To_Continent").val(),
					Value5: $("#To_Country").val(),
					Value6: $("#To_Area").val(),
					Value7: $("#To_Branch").val(),
					Value8: $("#st_TransportCategory").val()
				}

				$.ajax({
					type: "POST",
					url: "Logistics_Route.aspx/Duplicate_Check",
					data: JSON.stringify(Dupdata),
					dataType: "json",
					contentType: "application/json; charset=utf-8",
					success: function (result) {
						if (result.d != "0") {
							DupFlag = "Y";
						}
					},
					error: function (result) {
						alert('failure : ' + result);
					}
				});
			}
			else if (Flag == "Schedule") {
				var Status = "1";
				var VesselName = $("#VesselName_" + Row).val();
				var ShipWay = $("#ShipWay_" + Row).val();
				var Container = $("#Container_" + Row).val();
				var Datetime_Dead = $("#Datetime_Dead_" + Row).val();
				var Datetime_From = $("#Datetime_From_" + Row).val();
				var Datetime_Take = $("#Datetime_Take_" + Row).val();
				var Datetime_To = $("#Datetime_To_" + Row).val();
			}
			else if (Flag == "Vessel") {
				var Status = "1";
				var VesselName = $("#VesselName_" + Row).val();
				var ShipWay = $("#ShipWay_" + Row).val();
				var Container = "";
				var Datetime_Dead = "";
				var Datetime_From = "";
				var Datetime_Take = "";
				var Datetime_To = "";
			}
			else { //Flag = "AddContainer""
				var Status = "1";
				var VesselName = AddContainer_Vessel[0];
				var ShipWay = AddContainer_Vessel[1];
				var Container = Container;
				var Datetime_Dead = "";
				var Datetime_From = "";
				var Datetime_Take = "";
				var Datetime_To = "";
			}

			var data = {
				DocumentPk: RoutePk,
				Type: "NN21.COM_ROUTE",
				TypePk: "0",
				Status: Status,
				Value0: $("#From_Continent").val(),
				Value1: $("#From_Country").val(),
				Value2: $("#From_Area").val(),
				Value3: $("#From_Branch").val(),
				Value4: $("#To_Continent").val(),
				Value5: $("#To_Country").val(),
				Value6: $("#To_Area").val(),
				Value7: $("#To_Branch").val(),
				Value8: $("#st_TransportCategory").val(),
				Value9: VesselName,
				Value10: ShipWay,
				Value11: Container,
				Value12: Datetime_Dead,
				Value13: Datetime_From,
				Value14: Datetime_Take,
				Value15: Datetime_To,
				Value16: "",
				Value17: "",
				Value18: "",
				Value19: ""
			}

			setTimeout(function () {
				if (DupFlag == "Y") {
					alert("노선이 이미 존재합니다.");
					return false;
				} else {
					$.ajax({
						type: "POST",
						url: "/WebService/DocumentP.asmx/Set_Document",
						data: JSON.stringify(data),
						dataType: "json",
						contentType: "application/json; charset=utf-8",
						success: function (result) {
							if (Flag == "Route") {
								location.reload();
							}
							else {
								$('#Modal_AddVessel').modal('hide');
								Retrieve_Route();
							}

						},
						error: function (result) {
							alert('failure : ' + result);
						}
					});
				}
			}, 500);
		}

		function ModalOpen_Saved(code, value) {
			var data = {
				BranchPk: "3157",//$("#From_BranchPk").val(),
				Code: code,
				Value: value
			};

			$.ajax({
				type: "POST",
				url: "/WebService/DocumentP.asmx/MakeHtml_ModalSaved",
				data: JSON.stringify(data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					var Html_DirectSet = "";
					Html_DirectSet = "<table><tr><td style=\"padding:2px;\"><input type=\"text\" class=\"form-control\" id=\"DirectSet0\" style=\"width:135px;\"/></td></tr></table>";

					$("#Pn" + "Modal_Default" + "_Body").html('<form id="FormCategory" >'	+ result.d + Html_DirectSet + '</form>');
					$("#Pn" + "Modal_Default" + "_Footer").html('<button type="button" class="btn green" onclick="Set_Saved();">Save Changes</button>		\
				<button type="button" class="btn dark btn-outline" data-dismiss="modal">Close</button>');
					$('#Modal_Default').modal('show');
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});
		}

		function Onclick_Route(DataRow, RoutePk) {
			$("#BTN_Delete").show();
			$("#BTN_Retrieve").show();
			$(window).scrollTop(0);

			RoutePk_Main = RoutePk;
			$("#From_Continent").val($("#RouteTable tr:eq(" + DataRow + ") td:eq(0)").text());
			$("#From_Country").val($("#RouteTable tr:eq(" + DataRow + ") td:eq(1)").text());
			$("#From_Area").val($("#RouteTable tr:eq(" + DataRow + ") td:eq(2)").text());
			$("#From_Branch").val($("#RouteTable tr:eq(" + DataRow + ") td:eq(3)").text());
			$("#To_Continent").val($("#RouteTable tr:eq(" + DataRow + ") td:eq(4)").text());
			$("#To_Country").val($("#RouteTable tr:eq(" + DataRow + ") td:eq(5)").text());
			$("#To_Area").val($("#RouteTable tr:eq(" + DataRow + ") td:eq(6)").text());
			$("#To_Branch").val($("#RouteTable tr:eq(" + DataRow + ") td:eq(7)").text());
			$("#st_TransportCategory").val($("#RouteTable tr:eq(" + DataRow + ") td:eq(8)").text());
		}

		function Retrieve_Route() {
			var data = {
				RoutePk: RoutePk_Main
			}
			
			$.ajax({
				type: "POST",
				url: "Logistics_Route.aspx/MakeHtml_Schedule",
				data: JSON.stringify(data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					$("#Schedule").html("<table class=\"table\"><thead><th>" + $("#From_Branch").val() + "&nbsp&nbsp&nbsp&nbsp&nbsp >>>>> &nbsp&nbsp&nbsp&nbsp&nbsp" + $("#To_Branch").val() + " [" + $("#st_TransportCategory").val() + "]" + "</th>"
						+ "<th>" + "<input type=\"button\" onclick=\"Add_Vessel()\" id=\"BTN_AddVessel\" class=\"btn btn-dark btn-xs btn-block\" value=\"선사추가\" /></th>" + 
						"<th><input type=\"button\" id=\"BTN_AddContainer\" class=\"btn btn-dark btn-xs btn-block\" onclick=\"Add_Container()\" value=\"규격추가\"/></th></thead></table>");
					$("#Schedule").append(result.d);
					Dom_Connect();
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});
		}

		function Delete_Route(RoutePk) {
			if (RoutePk == "") { // 운송노선 삭제시 
				var data = {
					DocumentPk: RoutePk_Main
				}
			}
			else {	// 운손일정 삭제시 
				var data = {
					DocumentPk: RoutePk
				}
			}
			
			$.ajax({
				type: "POST",
				url: "/WebService/DocumentP.asmx/Delete_Document",
				data: JSON.stringify(data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					alert("삭제완료");
					if (RoutePk == "") {
						location.reload();
					}
					else {
						Retrieve_Route();
					}
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});
		}

		function Set_Saved() {
			$("#Modal_SavedAdd_BranchPk").val("3157");
			switch (ModalStatus) {
				case "Modal_From_Continent":
					$("#Modal_SavedAdd_Code").val("nn21com_Route_Continent");
					$("#Modal_SavedAdd_Value_0").val($("#DirectSet0").val());
					$("#From_Continent").val($("#DirectSet0").val());
					break;
				case "Modal_From_Country":
					$("#Modal_SavedAdd_Code").val("nn21com_Route_Country");
					$("#Modal_SavedAdd_Value_0").val($("#DirectSet0").val());
					$("#Modal_SavedAdd_Value_4").val($("#From_Continent").val());
					$("#From_Country").val($("#DirectSet0").val());
					break;
				case "Modal_From_Area":
					$("#Modal_SavedAdd_Code").val("nn21com_Route_Area");
					$("#Modal_SavedAdd_Value_0").val($("#DirectSet0").val());
					$("#Modal_SavedAdd_Value_4").val($("#From_Continent").val() + ",!" + $("#From_Country").val());
					$("#From_Area").val($("#DirectSet0").val());
					break;
				case "Modal_From_Branch":
					$("#Modal_SavedAdd_Code").val("nn21com_Route_Branch");
					$("#Modal_SavedAdd_Value_0").val($("#DirectSet0").val());
					$("#Modal_SavedAdd_Value_4").val($("#From_Continent").val() + ",!" + $("#From_Country").val() + ",!" + $("#From_Area").val());
					$("#From_Branch").val($("#DirectSet0").val());
					break;
				case "Modal_To_Continent":
					$("#Modal_SavedAdd_Code").val("nn21com_Route_Continent");
					$("#Modal_SavedAdd_Value_0").val($("#DirectSet0").val());
					$("#To_Continent").val($("#DirectSet0").val());
					break;
				case "Modal_To_Country":
					$("#Modal_SavedAdd_Code").val("nn21com_Route_Country");
					$("#Modal_SavedAdd_Value_0").val($("#DirectSet0").val());
					$("#Modal_SavedAdd_Value_4").val($("#To_Continent").val());
					$("#To_Country").val($("#DirectSet0").val());
					break;
				case "Modal_To_Area":
					$("#Modal_SavedAdd_Code").val("nn21com_Route_Area");
					$("#Modal_SavedAdd_Value_0").val($("#DirectSet0").val());
					$("#Modal_SavedAdd_Value_4").val($("#To_Continent").val() + ",!" + $("#To_Country").val());
					$("#To_Area").val($("#DirectSet0").val());
					break;
				case "Modal_To_Branch":
					$("#Modal_SavedAdd_Code").val("nn21com_Route_Branch");
					$("#Modal_SavedAdd_Value_0").val($("#DirectSet0").val());
					$("#Modal_SavedAdd_Value_4").val($("#To_Continent").val() + ",!" + $("#To_Country").val() + ",!" + $("#To_Area").val());
					$("#To_Branch").val($("#DirectSet0").val());
					break;
				case "Modal_VesselName":
					$("#Modal_SavedAdd_Code").val("nn21com_Route_VesselName");
					$("#Modal_SavedAdd_Value_0").val($("#DirectSet0").val());
					$("#VesselName").val($("#DirectSet0").val());
					break;
				case "Modal_ShipWay":
					$("#Modal_SavedAdd_Code").val("nn21com_Route_ShipWay");
					$("#Modal_SavedAdd_Value_0").val($("#DirectSet0").val());
					$("#Modal_SavedAdd_Value_4").val($("#st_TransportCategory").val());
					$("#ShipWay").val($("#DirectSet0").val());
					break;
				case "Modal_Container":
					$("#Modal_SavedAdd_Code").val("nn21com_Route_Container");
					$("#Modal_SavedAdd_Value_0").val($("#DirectSet0").val());
					$("#Modal_SavedAdd_Value_4").val($("#st_TransportCategory").val());
					$("#Container").val($("#DirectSet0").val());
					break;
			}
			var datas = $("#Form_SavedAdd").serialize();
			$.ajax({
				type: "POST",
				url: "/WebService/DocumentP.asmx/Set_Saved",
				data: "{ValueSum:\"" + datas + "\"}",
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					//alert(result.d);
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});

			$('#Modal_Default').modal('hide');
		}

		function Add_Vessel() {

			var data = {
				TransportCategory: $("#st_TransportCategory").val()
			}

			$.ajax({
				type: "POST",
				url: "Logistics_Route.aspx/MakeHtml_AddVessel",
				data: JSON.stringify(data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					ForDialog.InitZ("sm", "Modal_AddVessel");
					$("#Pn" + "Modal_AddVessel" + "_Body").html(result.d);
					$("#Pn" + "Modal_AddVessel" + "_Footer").html("<button type=\"button\" class=\"btn green\" onclick=\"Set_Document('Vessel', '49', '', '');\">Save Changes</button><button type=\"button\" class=\"btn dark btn-outline\" data-dismiss=\"modal\">Close</button>");
					Dom_Connect();
					$('#Modal_AddVessel').modal('show');
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});
		}

		function Add_Container() {
			var data = {
				RoutePk: RoutePk_Main,
				RouteCategory: $("#st_TransportCategory").val()
			};

			$.ajax({
				type: "POST",
				url: "Logistics_Route.aspx/MakeHtml_Container",
				data: JSON.stringify(data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					$("#Pn" + "Modal_Default" + "_Body").html(result.d);
					$("#Pn" + "Modal_Default" + "_Footer").html("<button type=\"button\" class=\"btn green\" onclick=\"Set_Container();\">Save Changes</button><button type=\"button\" class=\"btn dark btn-outline\" data-dismiss=\"modal\">Close</button>");
					$('#Modal_Default').modal('show');
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});

		}

		function Set_Container() {
			if (AddContainer_Vessel[0] == "") {
				alert("선사를 선택하세요.");
				return false;
			}

			var Containers = new Array();
			$('input:checkbox').each(function () {
				if (this.checked == true) {
					Containers.push(this.value);
				}
			});

			var data = {
				FromBranch: $("#From_Branch").val(),
				ToBranch: $("#To_Branch").val(),
				RouteCategory: $("#st_TransportCategory").val(),
				Vessel: AddContainer_Vessel[0],
				ShipWay: AddContainer_Vessel[1]
			}

			$.ajax({
				type: "POST",
				url: "Logistics_Route.aspx/AddContainer_Check",
				data: JSON.stringify(data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});


			for (var i = 0; i < Containers.length; i++) {
				Set_Document("AddContainer", "", "", Containers[i]);
			}
			$('#Modal_Default').modal('hide');

		}


	</script>
</asp:Content>

