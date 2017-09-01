<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HSCode_Tariff.aspx.cs" Inherits="CustomClearance_HSCode_Tariff" %>

<%@ Register Src="../Member/LogedTopMenu.ascx" TagName="Loged" TagPrefix="uc1" %>
<%@ Register Src="~/Admin/LogedWithoutRecentRequest11.ascx" TagPrefix="uc1" TagName="LogedWithoutRecentRequest" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.1/themes/base/jquery-ui.css" />
	<script src="http://code.jquery.com/jquery-1.9.1.js"></script>
	<script src="http://code.jquery.com/ui/1.10.1/jquery-ui.js"></script>
	<link rel="stylesheet" href="../Lib/Scale/css/bootstrap.css" type="text/css" />
	<link rel="stylesheet" href="../Lib/Scale/css/animate.css" type="text/css" />
	<link rel="stylesheet" href="../Lib/Scale/css/font-awesome.min.css" type="text/css" />
	<link rel="stylesheet" href="../Lib/Scale/css/icon.css" type="text/css" />
	<link rel="stylesheet" href="../Lib/Scale/css/font.css" type="text/css" />
	<link rel="stylesheet" href="../Lib/Scale/css/app.css" type="text/css" />
	<script type="text/javascript">
		window.onload = function () {
			if ("<%=MemberInfo[0]%>" == "OurBranch") {
				$("#ShowTab").show();
			} else { $("#ShowTab").hide(); }

		}
		function OnlyNum_Calc(val) {
			re = /[^0-9]/gi;
			if (re.test(val)) {
				alert("숫자만 입력가능합니다");
				return false;
			} else {
				return true;
			}
		}
		function NumberFormat(number) {
			var number, nArr;
			number = number.replace(/\,/g, "");
			nArr = String(number).split('').join(',').split('');
			for (var i = nArr.length - 1, j = 1; i >= 0; i--, j++) if (j % 6 != 0 && j % 2 == 0) nArr[i] = '';
			return nArr.join('');
		}


		function Calc_click() {
			if ($("#HsCode").val().length != 10) {
				alert("Hscode를 정확히 입력해주세요");
				$("#HsCode").select();
				$("#HsCode").focus();
				return false;
			}
			if ($("#HsCode").val() == "") {
				alert("Hscode를 입력해주세요");
				$("#HsCode").select();
				$("#HsCode").focus();
				return false;
			} if ($("#Charge").val() == "") {
				alert("물품금액을 입력해주세요");
				$("#Charge").select();
				$("#Charge").focus();
				return false;
			}
			if (!OnlyNum_Calc($("#HsCode").val())) {
				$("#HsCode").select();
				$("#HsCode").focus();
				return false;
			}
			if (!OnlyNum_Calc($("#Charge").val())) {
				$("#Charge").select();
				$("#Charge").focus();
				return false;
			}

			Admin.CalcTarriffKOR_FTA($("#HsCode").val(), $("#MonetaryUnit").val(), $("#Charge").val(), function (result) {
				var arr = result.split("#@!");
				$("#DefalutGuanse").val(NumberFormat(arr[0]));
				$("#DefalutBugase").val(NumberFormat(arr[1]));
				$("#WTOGuanse").val(NumberFormat(arr[2]));
				$("#WTOBugase").val(NumberFormat(arr[3]));
				$("#COGuanse").val(NumberFormat(arr[4]));
				$("#COBugase").val(NumberFormat(arr[5]));
				$("#2016Guanse").val(NumberFormat(arr[6]));
				$("#2016Bugase").val(NumberFormat(arr[7]));
				$("#2017Guanse").val(NumberFormat(arr[8]));
				$("#2017Bugase").val(NumberFormat(arr[9]));
				$("#2018Guanse").val(NumberFormat(arr[10]));
				$("#2018Bugase").val(NumberFormat(arr[11]));
				$("#2019Guanse").val(NumberFormat(arr[12]));
				$("#2019Bugase").val(NumberFormat(arr[13]));

				if (arr[0] == "x" && arr[1] == "x") {
					$("#DefalutTotal").val("x");
				} else {
					$("#DefalutTotal").val(NumberFormat((parseInt(arr[0]) + parseInt(arr[1])).toString()));
				}
				if (arr[2] == "x" && arr[3] == "x") {
					$("#WTOTotal").val("x");
				} else {
					$("#WTOTotal").val(NumberFormat((parseInt(arr[2]) + parseInt(arr[3])).toString()));
				}
				if (arr[4] == "x" && arr[5] == "x") {
					$("#COTotal").val("x");
				} else {
					$("#COTotal").val(NumberFormat((parseInt(arr[4]) + parseInt(arr[5])).toString()));
				} if (arr[6] == "x" && arr[7] == "x") {
					$("#2016Total").val("x");
				} else {
					$("#2016Total").val(NumberFormat((parseInt(arr[6]) + parseInt(arr[7])).toString()));
				} if (arr[8] == "x" && arr[9] == "x") {
					$("#2017Total").val("x");
				} else {
					$("#2017Total").val(NumberFormat((parseInt(arr[8]) + parseInt(arr[9])).toString()));
				} if (arr[10] == "x" && arr[11] == "x") {
					$("#2018Total").val("x");
				} else {
					$("#2018Total").val(NumberFormat((parseInt(arr[10]) + parseInt(arr[11])).toString()));
				} if (arr[12] == "x" && arr[13] == "x") {
					$("#2019Total").val("x");
				} else {
					$("#2019Total").val(NumberFormat((parseInt(arr[12]) + parseInt(arr[13])).toString()));
				}
			}, function (result) { alert("ERROR : " + result); });
		}
		function GoPage(PageNo) {
			$("#HPageNo").val(PageNo);
			$("#FormSearch").submit();
		}
		function popDescription(item) {
			if (item == "") {
				item = item;
			} else {
				item = item.replace(/,/gi, "<br />")
			}
			$("#ModalInner").html(item);
			$("#Modal").show();
		}
		function modal_Close() {
			$("#Modal").hide();
		}
	</script>
</head>

<body style="width: 1100px; margin: 0 auto; padding-top: 10px;">
	<div class="modal" id="Modal" style="margin-top: 80px;" tabindex="-1" role="dialog">
		<div class="modal-dialog modal-sm">
			<div class="modal-content">
				<div class="modal-body">
					<div id="ModalInner" class="panel-body form-horizontal">
					</div>
				</div>
				<div class="modal-footer">
					<button type="button" class="btn btn-default" data-dismiss="modal" onclick='modal_Close();'>Close</button>
				</div>
			</div>
		</div>
	</div>
	<form runat="server">
		<uc1:Loged ID="Loged1" runat="server" />
		<uc1:LogedWithoutRecentRequest runat="server" ID="LogedWithoutRecentRequest" Visible="false" />
		<asp:ScriptManager ID="SM" runat="server">
			<Services>
				<asp:ServiceReference Path="~/WebService/Admin.asmx" />
			</Services>
		</asp:ScriptManager>
	</form>

	<form role="search" method="post" id="FormSearch" onsubmit="GoPage('1');">
		<div class="wrapper col-sm-12" style="position: inherit;">
			<div class="panel panel-default">
				<div class="panel-heading">
					<div class="row">
						<div class="col-sm-10" style="position: inherit;">
							<strong>&nbsp;&nbsp;&nbsp;&nbsp;기존에 통관하셨던 아이템 히스토리</strong>
						</div>
						<div class="col-sm-2" style="position: inherit;">
							<strong><a href="../Images/양허표.png">*양허표 다운로드</a></strong>
						</div>
					</div>
				</div>
				<div class="panel-body form-horizontal">
					<div style="color: red;">이 자료는 관세청 배포 자료를 인용하였으면 법적 효력이 없음을 알려드립니다.</div>
					<div>
						<table class="table">
							<%=MakeHtml_HscodeHistory %>
						</table>
					</div>
				</div>
				<div class="panel panel-default">

					<div class="panel-body form-horizontal">
						<div>
							<%=HtmlList %>
						</div>
					</div>
					<div class="text-center m-t-lg m-b-lg">
						<%=Html_Paging %>
					</div>
					<div class="panel-heading" id="ShowTab">
						<div class="row">
							<div class="form-group m-b-none col-sm-offset-5 col-sm-4" style="float: right; position: inherit;">
								<div class="input-group" style="position: inherit;">
									<input type="text" class="form-control" style="position: inherit;" name="SearchValue" value="<%=SearchValue %>" placeholder="HSCode를 입력하세요" />
									<input type="hidden" id="HPageNo" name="PageNo" value="<%=Request.Params["PageNo"] %>" />
									<span class="input-group-btn" style="position: inherit;">
										<button type="submit" class="btn btn-default btn-icon btn-sm" style="position: inherit;"><i class="fa fa-search" style="position: inherit;"></i></button>
									</span>
								</div>
							</div>
						</div>
					</div>
					<div class="panel-body form-horizontal">
						<div>
							<%=Search_HtmlList %>
						</div>
					</div>
					<div class="text-center m-t-lg m-b-lg">
						<%=Search_Html_Paging %>
					</div>
					<div class="text-center m-t-lg m-b-lg">
						<table class="table">
							<thead>
								<tr>
									<th>HSCode</th>
									<th style="width: 80px;">화폐단위</th>
									<th>물품총액</th>
									<th></th>
									<th style="width: 60px;"></th>
									<th>기본</th>
									<th>WTO</th>
									<th>아태-CO</th>
									<th>FTA 2016</th>
									<th>FTA 2017</th>
									<th>FTA 2018</th>
									<th>FTA 2019</th>
								</tr>
							</thead>
							<tbody>
								<tr>
									<td>
										<input type="text" class="form-control" id="HsCode" name="HsCode" /></td>
									<td>
										<select id="MonetaryUnit" name="MonetaryUnit" class="form-control">
											<option value="18">RMB ￥</option>
											<option value="19">USD $</option>
											<option value="20">KRW ￦</option>
											<option value="21">JPY Y</option>
											<option value="22">HKD HK$</option>
											<option value="23">EUR €</option>
										</select>
									</td>
									<td>
										<input type="text" class="form-control" id="Charge" name="Charge" /></td>
									<td>
										<input type="button" id="Calc" value="Calc" class="btn btn-success btn-icon btn-sm" onclick="Calc_click();" /></td>
									<td>관세</td>
									<td>
										<input type="text" class="form-control" id="DefalutGuanse" name="DefalutGuanse" /></td>
									<td>
										<input type="text" class="form-control" id="WTOGuanse" name="WTOGuanse" /></td>
									<td>
										<input type="text" class="form-control" id="COGuanse" name="COGuanse" /></td>
									<td>
										<input type="text" class="form-control" id="2016Guanse" name="2016Guanse" /></td>
									<td>
										<input type="text" class="form-control" id="2017Guanse" name="2017Guanse" /></td>
									<td>
										<input type="text" class="form-control" id="2018Guanse" name="2018Guanse" /></td>
									<td>
										<input type="text" class="form-control" id="2019Guanse" name="2019Guanse" /></td>
								</tr>
								<tr>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
									<td>부가세</td>
									<td>
										<input type="text" class="form-control" id="DefalutBugase" name="DefalutBugase" /></td>
									<td>
										<input type="text" class="form-control" id="WTOBugase" name="WTOBugase" /></td>
									<td>
										<input type="text" class="form-control" id="COBugase" name="COBugase" /></td>
									<td>
										<input type="text" class="form-control" id="2016Bugase" name="2016Bugase" /></td>
									<td>
										<input type="text" class="form-control" id="2017Bugase" name="2017Bugase" /></td>
									<td>
										<input type="text" class="form-control" id="2018Bugase" name="2018Bugase" /></td>
									<td>
										<input type="text" class="form-control" id="2019Bugase" name="2019Bugase" /></td>
								</tr>
								<tr>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
									<td>&nbsp;</td>
									<td>합계</td>
									<td>
										<input type="text" class="form-control" id="DefalutTotal" name="DefalutTotal" /></td>
									<td>
										<input type="text" class="form-control" id="WTOTotal" name="WTOTotal" /></td>
									<td>
										<input type="text" class="form-control" id="COTotal" name="COTotal" /></td>
									<td>
										<input type="text" class="form-control" id="2016Total" name="2016Total" /></td>
									<td>
										<input type="text" class="form-control" id="2017Total" name="2017Total" /></td>
									<td>
										<input type="text" class="form-control" id="2018Total" name="2018Total" /></td>
									<td>
										<input type="text" class="form-control" id="2019Total" name="2019Total" /></td>
								</tr>
							</tbody>
						</table>
					</div>
					<input type="hidden" id="HCompanyPk" value="<%=CompanyPk %>" />
				</div>
			</div>
		</div>
	</form>
</body>
</html>
