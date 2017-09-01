<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManagementStorage.aspx.cs" Inherits="Admin_ManagementStorage" %>
<%@ Register src="LogedWithoutRecentRequest.ascx" tagname="LogedWithoutRecentRequest" tagprefix="uc1" %>
<%@ Register src="../CustomClearance/Loged.ascx" tagname="Loged" tagprefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
		<title></title>
		<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />    
		<script type="text/javascript">
			function PopWarehouseMap(StorageCode) {
				window.open('../Request/Dialog/WarehouseMap2.aspx?S=' + StorageCode, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=700px; width=800px;');
			}
			function PopWarehouseModify(StorageCode) {
				window.open('../Admin/Dialog/SetWarehouse.aspx?BranchPk=' + document.getElementById("HCompanyPk").value + '&S=' + StorageCode, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=no, top=200px; left=200px; height=700px; width=800px;');
			}
			function WarehouseUseNo(StorageCodePk) {
			   if (confirm("사용안함으로 처리 됩니다")) {
			      Admin.WarehouseUseNo(StorageCodePk, function (result) {
			         if (result == "1") {
			            alert("SUCCESS");
			            location.reload();
			         }
			      }, function (result) { alert("ERROR : " + result); });
			   }
			}
		</script>
	</head>
	<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px;" >
		<form id="form1" runat="server">
         <asp:ScriptManager ID="SM" runat="server">
            <Services>
                <asp:ServiceReference Path="~/WebService/Admin.asmx" />
            </Services>
        </asp:ScriptManager>
			<uc1:LogedWithoutRecentRequest ID="LogedWithoutRecentRequest1" runat="server" />
			<uc2:Loged ID="Loged1" runat="server" Visible="false" />
			<div style="background-color:white; width:850px; height:100%; padding:25px; ">
				<div style="float:right;">
					<input type="hidden" id="HCompanyPk" value="<%=MemberInfo[1] %>" />
				</div>
				<div>
					<input type="button"  value="New" onclick="PopWarehouseModify('New');" />
				</div>
				<%=Html_Warehouse %>
			</div>
		</form>
	</body>
</html>