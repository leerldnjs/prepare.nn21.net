<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Clean.Master" AutoEventWireup="true" CodeFile="CollectCharge.aspx.cs" Inherits="Charge_Dialog_CollectCharge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" runat="Server">

	<input type="hidden" id="Table_Name" value="<%=Head.Table_Name %>" />
	<input type="hidden" id="Table_Pk" value="<%=Head.Table_Pk %>" />
	<input type="hidden" id="CalcHeadPk" value="<%=CalcHeadPk %>" />
	<input type="hidden" id="BBankPk" value="<%=Head.Branch_Bank_Pk %>" />
	<input type="hidden" id="AccountID" value="<%=MemberInformation[2] %>" />
	<input type="hidden" id="CustomerCode" value="<%=Head.Customer_Code %>" />
	<input type="hidden" id="CustomerName" value="<%=Head.Customer_Name %>" />
	<input type="hidden" id="TotalCount" value="<%=Head.TotalPackedCount %>" />


	<div class="panel panel-warning">
		<div class="panel-heading" style="font-weight: bold;">수금</div>
		<div class="panel-body form-horizontal">
			<div class="form-group">
				<div class="col-xs-5 col-xs-offset-1">
					<label style="font-weight:bold;">운임비용</label>
				</div>
			</div>
			<div class="form-group" id="Pn_ChargePrice">
					<%=Html_ChargePrice %>
			</div>
			<hr />
			<div class="form-group">
				<div class="col-xs-5 col-xs-offset-1">
					<label style="font-weight:bold;">운임비용 합계</label>
				</div>
			</div>
			<div class="form-group" id="Pn_ChargeTotal">
				<%=Html_ChargeTotal %>
			</div>
			<hr />
			<div class="form-group">
				<div class="col-xs-5 col-xs-offset-1">
					<label style="font-weight:bold;">잔액</label>
				</div>
			</div>
			<div class="form-group" id="Pn_ChargeBalance">
				<%=Html_ChargeBalance %>
			</div>
			<hr />
			<div class="form-group">
				<div class="col-xs-10 col-xs-offset-1">
					<label style="font-weight:bold;">입금정보</label>
				</div>
			</div>
			<div class="form-group">
				<%=Html_CollectInfo %>
			</div>
			<hr />
			<div class="form-group">
				<div class="col-xs-10 col-xs-offset-1">
					<textarea id="Comment" class="form-control" rows="5" placeholder="Comment"></textarea>
				</div>
			</div>
		</div>
		<div class="panel-footer">
			<div class="form-group">
				<input type="button" class="btn btn-primary btn-sm col-xs-offset-5" value="입금" onclick="Set_Deposited()" />
				<input type="button" class="btn btn-warning btn-sm" value="취소" onclick="self.close()" />
			</div>
		</div>
	</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" runat="Server">
	<script type="text/javascript">

		function Set_Deposited() {
			
			var data = {
				Table_Name: $("#Table_Name").val(),
				Table_Pk: $("#Table_Pk").val(),
				CalculateHeadPk: $("#CalcHeadPk").val(),
				BankPk: $("#BBankPk").val(),
				MonetaryUnit: $("#MonetaryUnit").text(),
				Price: $("#Price").val().replace(/,/g, ""),
				AccountID: $("#AccountID").val(),
				DepositedDate: $("#Dateymd").val().substr(0, 4) + "-" + $("#Dateymd").val().substr(4, 2) + "-" + $("#Dateymd").val().substr(6, 2) + " " + $("#Datehh").val() + ":" + $("#Datemm").val(),
				Description: $("#CustomerName").val() + "[" + $("#CustomerCode").val() + "] " + $("#TotalCount").val() + "GT",
				Comment: $("#Comment").val()
			}
			$.ajax({
				type: "POST",
				url: "/Process/ChargeP.asmx/Set_Deposited",
				data: JSON.stringify(data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					alert("성공");
					opener.location.reload();
					self.close();
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});
			
			
		}



	</script>
</asp:Content>

