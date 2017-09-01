<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ItemModify.aspx.cs" Inherits="RequestForm_ItemModify" Debug="true" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
   <style type="text/css">
   		.tdSubT{border-bottom:solid 2px #93A9B8;	padding-top:10px; }
   		input{ text-align:center;}
   	</style>
   	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript">
		var OURBRANCHPK;
		var Weight_Before;
		var Volume_Before;
		window.onload = function () {
			if (dialogArguments[1] == "A") {
				form1.HIsAdmin.value = "Y";
				//form1.BTNSubmit.value = "  MODIFY  ";
				document.getElementById("SPComment").style.visibility = "visible";
				OURBRANCHPK = dialogArguments[2];
			}
			else {
				OURBRANCHPK = "";
				//form1.BTNSubmit.value = "  MODIFY  ";
			}
			document.getElementById("Item[0][MonetaryUnit]").value = form1.BeforeMonetaryUnitCL.value;
			document.getElementById('TB_ID').value = dialogArguments[0];

			Weight_Before = document.getElementById("total[0][Weight]").value;
			Volume_Before = document.getElementById("total[0][Volume]").value;
		}
		function CountBox(GroupNo, LineNo, obj) {
			if (form1.HIsAdmin.value == "Y") {
				var reg = /[0-9]-[0-9]/;
				var reg2 = /[0-9]/;
				if (reg2.test(obj.value) == true) {
					document.getElementById('Item[' + GroupNo + '][' + LineNo + '][PackedCount]').value = 1;
					var RowLength = document.getElementById('ItemTable[' + GroupNo + ']').rows.length - 2;
					var totalCount = 0;
					for (i = 0; i < RowLength; i++) {
						if (document.getElementById('Item[' + GroupNo + '][' + i + '][PackedCount]').value != "") {
							totalCount += parseInt(document.getElementById('Item[' + GroupNo + '][' + i + '][PackedCount]').value);
						}
					}
					document.getElementById('total[' + GroupNo + '][PackedCount]').value = totalCount;
				}
				if (reg.test(obj.value) == true) {
					var SplitResult = obj.value.split('-');
					var result = SplitResult[1] - SplitResult[0];
					if (result < 0) {
						result = SplitResult[0] - SplitResult[1];
					}
					document.getElementById('Item[' + GroupNo + '][' + LineNo + '][PackedCount]').value = result + 1;
					var i;
					var RowLength = document.getElementById('ItemTable[' + GroupNo + ']').rows.length - 2;
					var totalCount = 0;
					for (i = 0; i < RowLength; i++) {
						if (document.getElementById('Item[' + GroupNo + '][' + i + '][PackedCount]').value != "") {
							totalCount += parseInt(document.getElementById('Item[' + GroupNo + '][' + i + '][PackedCount]').value);
						}
					}
					document.getElementById('total[' + GroupNo + '][PackedCount]').value = totalCount;
				}
			}
		}
		function QuantityXUnitCost(GroupNo, LineNo) {
			if (document.getElementById('Item[' + GroupNo + '][' + LineNo + '][Quantity]').value != "" && document.getElementById('Item[' + GroupNo + '][' + LineNo + '][UnitCost]').value != "") {
				Request.QuantityXUnitCost(document.getElementById('Item[' + GroupNo + '][' + LineNo + '][Quantity]').value, document.getElementById('Item[' + GroupNo + '][' + LineNo + '][UnitCost]').value, function (result) {
					document.getElementById('Item[' + GroupNo + '][' + LineNo + '][Price]').value = result;
					GetTotal("Quantity");
					GetTotal("Price");
				}, function (result) { });
			}
			/*
			var quantity = parseInt(document.getElementById('Item[' + GroupNo + '][' + LineNo + '][Quantity]').value * 100000);
			var unitCost = parseInt(document.getElementById('Item[' + GroupNo + '][' + LineNo + '][UnitCost]').value * 100000);

			
			alert(unitCost);
			if (quantity > 0 && unitCost > 0) {
			document.getElementById('Item[' + GroupNo + '][' + LineNo + '][Price]').value = quantity * unitCost / 10000000000;
			var i;
			var RowLength = document.getElementById('ItemTable[' + GroupNo + ']').rows.length - 2;
			var totalPrice = 0;
			for (i = 0; i < RowLength; i++) {
			if (document.getElementById('Item[' + GroupNo + '][' + i + '][Price]').value != "") {
			totalPrice += parseInt(document.getElementById('Item[' + GroupNo + '][' + i + '][Price]').value);
			}
			}
			document.getElementById('total[' + GroupNo + '][Price]').value = totalPrice;
			}
			*/
		}
		function InsertRow(rowC) {		// 상품추가
			var objTable = document.getElementById('ItemTable[' + rowC + ']');
			objTable.appendChild(document.createElement("TBODY"));
			var lastRow = objTable.rows.length;
			var thisLineNo = lastRow - 2;
			var objRow1 = objTable.insertRow(lastRow);
			var objCell1 = objRow1.insertCell();
			var objCell2 = objRow1.insertCell();
			var objCell3 = objRow1.insertCell();
			var objCell4 = objRow1.insertCell();
			var objCell5 = objRow1.insertCell();
			var objCell6 = objRow1.insertCell();
			var objCell7 = objRow1.insertCell();
			var objCell8 = objRow1.insertCell();

			var MonetaryUnit;
			switch (document.getElementById('Item[0][MonetaryUnit]').value) {
				case '18': MonetaryUnit = "￥"; break;
				case '19': MonetaryUnit = "$"; break;
				case '20': MonetaryUnit = "￦"; break;
				case '21': MonetaryUnit = "Y"; break;
				case '22': MonetaryUnit = "$"; break;
				case '23': MonetaryUnit = "€"; break;
				default: MonetaryUnit = ""; break;
			}
			objCell1.align = "center";
			objCell2.align = "center";
			objCell3.align = "center";
			objCell4.align = "center";
			objCell5.align = "center";
			objCell6.align = "center";
			objCell7.align = "center";
			objCell8.align = "center";

			var disableIfNotAdmin = "";
			if (form1.HMemberGubun.value != "OurBranch") {
				disableIfNotAdmin = "disabled=\"disabled\"";
			}

			objCell1.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][boxNo]' onkeyup='CountBox(" + rowC + "," + thisLineNo + ",this)' size='3' />";
			objCell2.innerHTML = "<input type=\"hidden\" id=\"Item[0][" + thisLineNo + "][BeforeItem]\" value=\"N\" /><input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Brand]' size='7' /> / <input type='text' id='Item[" + rowC + "][" + thisLineNo + "][ItemName]' size='12' /> / <input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Matarial]' size='12' />";
			objCell3.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Quantity]' onkeyup='QuantityXUnitCost(" + rowC + "," + thisLineNo + ")' size='4' />" +
												"<select id='Item[" + rowC + "][" + thisLineNo + "][QuantityUnit]'>" +
													"<option value='40'>PCS</option>" +
													"<option value='41'>PRS</option>" +
													"<option value='42'>SET</option>" +
													"<option value='43'>S/F</option>" +
													"<option value='44'>YDS</option>" +
													"<option value='45'>M</option>" +
													"<option value='46'>KG</option>" +
													"<option value='47'>DZ</option>" +
													"<option value='48'>L</option>" +
													"<option value='49'>BOX</option>" +
													"<option value='50'>SQM</option>" +
                                                    "<option value='51'>M2</option>" +
													"<option value='52'>RO</option>" +
												"</select>";
			objCell4.innerHTML = "<input type='text' style='border:0;' id='Item[" + rowC + "][" + thisLineNo + "][MonetaryUnit][0]' size='1' value='" + MonetaryUnit + "' />" +
												"<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][UnitCost]' onkeyup='QuantityXUnitCost(" + rowC + "," + thisLineNo + ")' size='2' />";
			objCell5.innerHTML = "<input type='text' style='border:0;' id='Item[" + rowC + "][" + thisLineNo + "][MonetaryUnit][1]'  value='" + MonetaryUnit + "'size='1' />" +
												"<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Price]' readonly='readonly' size='8' />";
			objCell6.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][PackedCount]' "+disableIfNotAdmin+" size='2' />" +
													"<select id='Item[" + rowC + "][" + thisLineNo + "][PackingUnit]' size='1'>" +
														"<option value='15'>CT</option>" +
														"<option value='16'>RO</option>" +
														"<option value='17'>PA</option>" +
													"</select>";
			objCell7.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Weight]' " + disableIfNotAdmin + " size='6' />";
			objCell8.innerHTML = "<input type='text' id='Item[" + rowC + "][" + thisLineNo + "][Volume]' " + disableIfNotAdmin + " size='6' />";
		}
		function Btn_Submit_Click() {
			form1.BTNSubmit.style.visibility = "hidden";
			var NewMonetaryUnit = "";
			var ModifyComment = "";
			if (form1.TBComment.value != "") {
				ModifyComment = "* " + form1.TBComment.value + " || ";
			}
			var ChangeSum = "";
			if (document.getElementById('Item[0][MonetaryUnit]').value != form1.BeforeMonetaryUnitCL.value) {
				NewMonetaryUnit = document.getElementById('Item[0][MonetaryUnit]').value;
				ModifyComment += "* MonetaryUnit : " + form1.BeforeMonetaryUnit.value + " → " + document.getElementById("Item[0][MonetaryUnit][Total]").value + " || ";
			}
			var Title = new Array();
			Title[0] = "MarkNNumber";
			Title[1] = "Description";
			Title[2] = "Style No";
			Title[3] = "Material";
			Title[4] = "Quantity";
			Title[5] = "QuantityUnit";
			Title[6] = "PackedCount";
			Title[7] = "PackingUnit";
			Title[8] = "GrossWeight";
			Title[9] = "Volume";
			Title[10] = "UnitPrice";
			Title[11] = "Amount";

			var RowLength0 = document.getElementById('ItemTable[0]').rows.length - 2;
			for (i = 0; i < RowLength0; i++) {
				var Temp = new Array();
				Temp[0] = document.getElementById("Item[0][" + i + "][boxNo]").value;
				Temp[1] = document.getElementById("Item[0][" + i + "][ItemName]").value;

				Temp[2] = document.getElementById("Item[0][" + i + "][Brand]").value;
				Temp[3] = document.getElementById("Item[0][" + i + "][Matarial]").value;
				Temp[4] = document.getElementById("Item[0][" + i + "][Quantity]").value;
				Temp[5] = document.getElementById("Item[0][" + i + "][QuantityUnit]").value;
				Temp[6] = document.getElementById("Item[0][" + i + "][PackedCount]").value;
				Temp[7] = document.getElementById("Item[0][" + i + "][PackingUnit]").value;
				Temp[8] = document.getElementById("Item[0][" + i + "][Weight]").value;
				Temp[9] = document.getElementById("Item[0][" + i + "][Volume]").value;
				Temp[10] = document.getElementById("Item[0][" + i + "][UnitCost]").value;
				Temp[11] = document.getElementById("Item[0][" + i + "][Price]").value;

				if (document.getElementById("Item[0][" + i + "][BeforeItem]").value == "N") {
					if (Temp[0] == "" && Temp[1] == "" && Temp[2] == "" && Temp[3] == "" && Temp[4] == "" && Temp[10] == "" && Temp[11] == "" && Temp[6] == "" && Temp[8] == "" && Temp[9] == "") { }
					else {
						ModifyComment += "*" + (i + 1) + " ADD || ";
						ChangeSum += Temp[0] + "#@!" + Temp[1] + "#@!" + Temp[2] + "#@!" + Temp[3] + "#@!" + Temp[4] + "#@!" + Temp[5] + "#@!" + Temp[6] + "#@!" + Temp[7] + "#@!" + Temp[8] + "#@!" + Temp[9] + "#@!" + Temp[10] + "#@!" + Temp[11] + "#@!N%!$@#";
					}
				}
				else {
					//string Temp = RS["MarkNNumber"] + "#@!" + RS["Description"] + "#@!" + RS["Label"] + "#@!" + RS["Material"] + "#@!" + RS["Quantity"] + "#@!" + RS["QuantityUnit"] + "#@!" + RS["PackedCount"] + "#@!" + RS["PackingUnit"] + "#@!" + RS["GrossWeight"] + "#@!" + RS["Volume"] + "#@!" + RS["UnitPrice"] + "#@!" + RS["Amount"] + "#@!" + RS["RequestFormItemsPk"];
					var TempEach = document.getElementById("Item[0][" + i + "][BeforeItem]").value;
					var Each = TempEach.replace(/##%%$$^^/gi, "\"").split("#@!");
					var IsChanged = false;
					var TempChange = "";
					for (var j = 0; j < Temp.length; j++) {
						if (Each[j] == Temp[j]) {
							TempChange += "*NC*#@!";
						}
						else {
							if (IsChanged == false) {
								ModifyComment += "*" + (i + 1) + " || ";
								IsChanged = true;
							}
							if (Temp[j] == "") {
								TempChange += "#@!";
								ModifyComment += Title[j] + " : " + Each[j] + " → X ||";
							}
							else {
								TempChange += Temp[j] + "#@!";
								ModifyComment += Title[j] + " : " + Each[j] + " → " + Temp[j] + " ||";
							}
						}
					}
					if (IsChanged) {
						ChangeSum += TempChange + Each[12] + "%!$@#";
					}
				}
			}
			var CalcuatedSum = new Array();
			CalcuatedSum[0] = document.getElementById("total[0][PackedCount]").value;
			CalcuatedSum[1] = document.getElementById("Item[0][0][PackingUnit]").value;
			CalcuatedSum[2] = document.getElementById("total[0][Weight]").value;
			CalcuatedSum[3] = document.getElementById("total[0][Volume]").value;

			//			alert(NewMonetaryUnit + " " + ChangeSum + " " + form1.TB_ID.value + " " + form1.TB_Pk.value + " " + CalcuatedSum + " " + ModifyComment + " " + form1.HMemberGubun.value);
			//			form1.BTNSubmit.style.visibility = "visible";
			//			return false;
			if (Weight_Before != document.getElementById("total[0][Weight]").value || Volume_Before != document.getElementById("total[0][Volume]").value) {
				Request.ResetRequestFormCalculate(form1.TB_Pk.value, function (result) {
					if (result == "1") {
						alert("운임 초기화");
					}

					Request.UpdateItemModify2(NewMonetaryUnit, ChangeSum, form1.TB_ID.value, form1.TB_Pk.value, CalcuatedSum, ModifyComment, form1.HMemberGubun.value, function (result) {
						if (result == "1") {
							alert("SUCCESS");
							window.returnValue = true;
							returnValue = "Y";
							self.close();
						}
						else {
							alert(result);
							form1.DEBUG.value = result;
						}
					}, function (result) { alert("ERROR : " + result); });

				}, function (result) { alert("ERROR : " + result); });
			} else {
				Request.UpdateItemModify2(NewMonetaryUnit, ChangeSum, form1.TB_ID.value, form1.TB_Pk.value, CalcuatedSum, ModifyComment, form1.HMemberGubun.value, function (result) {
					if (result == "1") {
						alert("SUCCESS");
						window.returnValue = true;
						returnValue = "Y";
						self.close();
					}
					else {
						alert(result);
						form1.DEBUG.value = result;
					}
				}, function (result) { alert("ERROR : " + result); });
			}

			form1.BTNSubmit.style.visibility = "visible";
		}
		function callError(result) { window.alert(form1.HError.value); }
		//function callError(result) { alert(result); }
		function SelectChangeMonetaryUnit(idx) {
			switch (document.getElementById('Item[0][MonetaryUnit]')[idx].value) {
				case '18': SetMonetaryUnit("￥"); break;
				case '19': SetMonetaryUnit("$"); break;
				case '20': SetMonetaryUnit("￦"); break;
				case '21': SetMonetaryUnit("Y"); break;
				case '22': SetMonetaryUnit("$"); break;
				case '23': SetMonetaryUnit("€"); break;
			}
		}
		function SetMonetaryUnit(value) {
			var i;
			var RowLength = document.getElementById('ItemTable[0]').rows.length - 2;
			var totalCount = 0;
			for (i = 0; i < RowLength; i++) {
				document.getElementById('Item[0][' + i + '][MonetaryUnit][0]').value = value;
				document.getElementById('Item[0][' + i + '][MonetaryUnit][1]').value = value;
			}
			document.getElementById('Item[0][MonetaryUnit][Total]').value = value;
		}
		function BoxTotalCount(GroupNo) {
			var RowLength = document.getElementById('ItemTable[' + GroupNo + ']').rows.length - 2;
			var totalCount = 0;
			for (i = 0; i < RowLength; i++) {
				if (document.getElementById('Item[' + GroupNo + '][' + i + '][PackedCount]').value != "") {
					totalCount += parseInt(document.getElementById('Item[' + GroupNo + '][' + i + '][PackedCount]').value);
				}
			}
			document.getElementById('total[' + GroupNo + '][PackedCount]').value = totalCount;
		}

		function GetTotal(Which) {
			var RowLength = document.getElementById('ItemTable[0]').rows.length - 2;
			var MaxCountUnder1 = 0;
			for (i = 0; i < RowLength; i++) {
				var ThisValue = document.getElementById('Item[0][' + i + '][' + Which + ']').value.toString();
				if (ThisValue.indexOf(".") != -1 && ThisValue.length - ThisValue.indexOf(".") - 1 > MaxCountUnder1) {
					MaxCountUnder1 = ThisValue.length - ThisValue.indexOf(".") - 1;
				}
			}
			var TotalValue = 0;
			var ToInt = Math.pow(10, MaxCountUnder1);
			for (i = 0; i < RowLength; i++) {
				var ThisValue = document.getElementById('Item[0][' + i + '][' + Which + ']').value.toString().replace(/,/g, '');
				if (ThisValue != "") {
					TotalValue += parseInt(parseFloat(ThisValue) * ToInt);
				}
			}
			var FloatTotalValue = parseInt(TotalValue + 0.00000001) / ToInt;
			if (TotalValue != 0) { document.getElementById('total[0][' + Which + ']').value = FloatTotalValue; }
		}
		function BoxTotalCount(GroupNo) {
			var RowLength = document.getElementById('ItemTable[' + GroupNo + ']').rows.length - 2;
			var totalCount = 0;
			for (i = 0; i < RowLength; i++) {
				if (document.getElementById('Item[' + GroupNo + '][' + i + '][PackedCount]').value != "") {
					totalCount += parseInt(document.getElementById('Item[' + GroupNo + '][' + i + '][PackedCount]').value);
				}
			}
			document.getElementById('total[' + GroupNo + '][PackedCount]').value = totalCount;
		}
		function DeleteAll() {
			Request.DeleteRequestFormItems_All(form1.TB_Pk.value, form1.TB_ID.value, function (result) {
				alert("삭제완료");
				window.returnValue = true; returnValue = "Y"; self.close();
			}, function (result) { alert("ERROR : " + result); });
		}
		function DelThisRow(RequestFormItemsPk, count) {
			Request.DeleteRequestFormItemsPk(form1.TB_Pk.value, RequestFormItemsPk, count, form1.TB_ID.value, function (result) {
				alert("삭제완료");
				window.returnValue = true; returnValue = "Y"; self.close();
			}, function (result) { alert("ERROR : " + result); });
		}
	</script>
</head>
<body style="padding:10px;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Request.asmx" /></Services></asp:ScriptManager>
	<fieldset>
	<legend><strong><%=GetGlobalResourceObject("RequestForm", "Freight") %></strong></legend>
	<table id="ItemTable[0]" style="background-color:white;"  border="0 " cellpadding="0" cellspacing="0">
		<thead>
			<tr><td class="tdSubT" colspan="9">&nbsp;&nbsp;&nbsp;
				<span style="float:right;">
					<select id="Item[0][MonetaryUnit]"  size="1" onchange="SelectChangeMonetaryUnit(this.selectedIndex)">
						<option value="19"><%=GetGlobalResourceObject("RequestForm", "MonetaryUnitChange") %></option>
						<option value="18">RMB ￥</option><option value="19">USD $</option><option value="20">KRW ￦</option>
						<option value="21">JPY Y</option><option value="22">HKD HK$</option><option value="23">EUR €</option>
					</select>
					<input type="button" onclick="DeleteAll();" style="color:red;" value="DELETE ALL" />
					<input type="button" onclick="InsertRow('0');" value="<%=GetGlobalResourceObject("qjsdur", "gkdahrcnrk") %>" />
				</span>
			</td></tr>
			<tr style="height:30px;" >
				<td bgcolor="#F5F5F5" height="20" align="center" width="50px" >BOX No</td>
				<td bgcolor="#F5F5F5" align="center" width="300px" ><%=GetGlobalResourceObject("RequestForm", "Label") %> / <%=GetGlobalResourceObject("RequestForm", "Description")%> / <%=GetGlobalResourceObject("RequestForm", "Material")%></td>
				<td bgcolor="#F5F5F5" align="center" width="110px"><%=GetGlobalResourceObject("RequestForm", "count")%></td>
				<td bgcolor="#F5F5F5" align="center" width="70px"><%=GetGlobalResourceObject("RequestForm", "unitcost")%></td>
				<td bgcolor="#F5F5F5" align="center" width="110px"><%=GetGlobalResourceObject("RequestForm", "amount")%></td>
				<td bgcolor="#F5F5F5" align="center" width="85px"><%=GetGlobalResourceObject("RequestForm", "PackingCount")%></td>
				<td bgcolor="#F5F5F5" align="center" width="65px"><%=GetGlobalResourceObject("qjsdur", "wndfid")%>(Kg)</td>
				<td bgcolor="#F5F5F5" align="center" width="65px"><%=GetGlobalResourceObject("qjsdur", "cpwjr")%>(CBM)</td>
				<td bgcolor="#F5F5F5" align="center" width="15px">&nbsp;</td>
			</tr>
		</thead>
		<tbody>
			<%=ItemInformationLoad %>
		</tbody>
	</table>
	<table border="0" style="background-color:White; width:855px;"  cellpadding="0" cellspacing="0">
		<tr>
			<td bgcolor="#F5F5F5" height="30" width="342" align="right" ><%=GetGlobalResourceObject("RequestForm", "Total")%>&nbsp;&nbsp;&nbsp;</td>
			<td bgcolor="#F5F5F5" ><input type="text" id="total[0][Quantity]" style="width:47px;" value="<%=QuantitySum %>" /></td>
			<td bgcolor="#F5F5F5" style="width:120px;" align="center" >
				<input type="text" style="border:0; background-color:#F5F5F5;" id="Item[0][MonetaryUnit][Total]" value="<%=MonetaryUnit %>" size="1" />
				<input type="text" id="total[0][Price]" style="width:70px; " value="<%=PriceSum %>" readonly="readonly" />
			</td>
			<td bgcolor="#F5F5F5" style="width:89px;">
				<input type="text" id="total[0][PackedCount]" style="width:32px;" value="<%=PackedCountSum %>" readonly="readonly" />
				<select id='total[0][PackingUnit]' size='1'>
					<option value='15'>CT</option>
					<option value='16'>RO</option>
					<option value='17'>PA</option>
				</select>
			</td>
			<td bgcolor="#F5F5F5" style="width:126px; padding-right:1px; " >
				<input type="text" id="total[0][Weight]" size="6" value="<%=WeightSum %>" readonly="readonly" />
				<input type="text" id="total[0][Volume]" size="6" value="<%=VolumeSum %>" readonly="readonly" />
			</td>
		</tr>
	</table>
	<div style="padding:20px; ">
		<input type="hidden" id="TB_ID" />
        <input type="hidden" id="HStepCL" value="<%=StepCL %>" />
		<input type="hidden" id="HIsAdmin" value="N" />
		<input type="hidden" id="TB_Pk" value="<%= Request.Params["S"] %>" />
		<input type="hidden" id="TB_GubunCL" value="<%= Request.Params["CL"] %>" />
		<input type="hidden" id="HError" value="<%= GetGlobalResourceObject("Alert", "CallError") %>" />
		<input type="hidden" id="BeforeMonetaryUnitCL" value="<%=MonetaryUnitCL %>" />
		<input type="hidden" id="BeforeMonetaryUnit" value="<%=MonetaryUnit %>" />
		<input type="hidden" id="wjqtn" value="<%=GetGlobalResourceObject("qjsdur", "wjqtn") %>" />
			<span id="SPComment" style="padding-right:10px; visibility:hidden;" >Comment : <input type="text"  id="TBComment" style="width:200px; text-align:left; " /></span>
		<input type="button" id="BTNSubmit" value="  <%=GetGlobalResourceObject("Member", "Modify") %>  " style="width:100px; height:30px;" onclick="Btn_Submit_Click()" />
		&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
		<input type="button" value="  <%=GetGlobalResourceObject("member", "cancel")%>  " style="width:100px; height:30px;" onclick="window.returnValue = true; returnValue = 'N'; self.close();" />
		<input type="hidden" id="DEBUG" />
		<input type="hidden" value="<%=MemberGubun %>" id="HMemberGubun" />
	</div>
	</fieldset>
    </form>
</body>
</html>