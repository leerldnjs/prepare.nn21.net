<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequestStorcedIn.aspx.cs" Inherits="Admin_Dialog_RequestStorcedIn" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script src="/Lib/Branch/js/jquery.min.js"></script>
	<script type="text/javascript">
		function BTN_SUBMIT_CLICK() {
			var PackedCount = document.getElementById("PackedCount").value;
			if (PackedCount == "" || PackedCount == "0") {
				alert("Please Type Storced count");
				return false;
			}
			var StorageCode = document.getElementById("STStorage").options[document.getElementById("STStorage").selectedIndex].value;
			if (StorageCode == "0" || StorageCode == "") {
				alert("Please Select warehouse");
				return false;
			}
			var RequestFormPk = form1.HRequestFormPk.value;
			var AccountID = form1.HAccountID.value;
			var PackingUnitCL = document.getElementById("PackingUnit").value;
			var Weight = document.getElementById("Weight").value;
			var Volume = document.getElementById("Volume").value;
			var StorageName = document.getElementById("STStorage").options[document.getElementById("STStorage").selectedIndex].text;
			var StorcedDate = document.getElementById("StorcedD").value + document.getElementById("StorcedH").value + document.getElementById("StorcedM").value;
			var Comment = form1.Comment.value;

			var IsAleadyStorecd = "Y";
			if (document.getElementById("HTotalPackedCount").value == "" && document.getElementById("HTotalGrossWeight").value == "" && document.getElementById("HTotalVolume").value == "") {
				IsAleadyStorecd = "N";
			}
			var HTotalPackedCount = parseInt(PackedCount);
			if (document.getElementById("HTotalPackedCount").value != "") { HTotalPackedCount += parseInt(document.getElementById("HTotalPackedCount").value); }

			var HTotalGrossWeight;
			if (Weight == "") { HTotalGrossWeight = 0; }
			else { HTotalGrossWeight = parseFloat(Weight); }
			if (document.getElementById("HTotalGrossWeight").value != "") { HTotalGrossWeight += parseFloat(document.getElementById("HTotalGrossWeight").value); }

			var HTotalVolume;
			if (Volume == "") { HTotalVolume = 0; }
			else { HTotalVolume = parseFloat(Volume); }
			if (document.getElementById("HTotalVolume").value != "") { HTotalVolume += parseFloat(document.getElementById("HTotalVolume").value); }

			var data = {
				RequestFormPk: RequestFormPk,
				AccountID: AccountID,
				PackedCount: PackedCount,
				PackingUnitCL: PackingUnitCL,
				Weight: Weight,
				Volume: Volume,
				StorageCode: StorageCode,
				StorageName: StorageName,
				StorcedDate: StorcedDate,
				CommentText: Comment,
				TotalPackedCount: HTotalPackedCount,
				TotalGrossWeight: HTotalGrossWeight,
				TotalVolume: HTotalVolume,
				IsAleadyCalculated: IsAleadyStorecd
			}
			$.ajax({
				type: "POST",
				url: "/WebService/Admin.asmx/RequestStorcedIn",
				data: JSON.stringify(data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					if (result.d == "1") {
						alert("성공");
						window.returnValue = true;
						returnValue = "Y";
						self.close();
					}
					else {
						alert(result);
						form1.DEBUG.value = result;
						return false;
					}
					
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});

		}
	</script>
</head>
<body style="padding:10px; ">
    <form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
    <fieldset style="width:400px; height:200px; padding-left:10px;  ">
		<legend><strong>Storced In</strong></legend>
		<p><%=TITLE %></p>
		<%=StorcedIn%>
		<p>Comment : <input type="text" id="Comment" style="width:290px; "  /></p>
		<div style="padding:10px; text-align:center;">
			<input type="button" value="SUBMIT" onclick="BTN_SUBMIT_CLICK();" />&nbsp;&nbsp;&nbsp;
			<input type="button" value="CANCLE" />
		</div>
		<input type="hidden" id="HRequestFormPk" value="<%=RequestFormPk %>" />
		<input type="hidden" id="HAccountID" value="<%=Session["ID"] %>" />
		<input type="hidden" id="DEBUG" onclick="this.select();" />
	</fieldset>
    </form>
</body>
</html>