<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="RequestList.aspx.cs" Inherits="Admin_RequestList" %>
<%@ Register src="LogedWithoutRecentRequest.ascx" tagname="LogedWithoutRecentRequest" tagprefix="uc1" %>
<%@ Register src="../CustomClearance/Loged.ascx" tagname="Loged" tagprefix="uc2" %>
<%@ Register src="~/Logistics/Loged.ascx" tagname="Loged" tagprefix="uc3" %>
<%@ Register Src="~/ViewShare/Header_ShippingBranch.ascx" TagPrefix="uc4" TagName="Header_ShippingBranch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script src="../Common/public.js" type="text/javascript"></script>
	<script type="text/javascript">
		function PopMsgSend(RequestFormPk) {
			window.open('../Request/Dialog/MsgSendedList.aspx?S=' + RequestFormPk, '', 'location=no, directories=no, resizable=no, status=no, toolbar=no, menubar=no, scrollbars=yes, top=200px; left=200px; height=700px; width=800px;');
		}
		function WriteMeasurement(RequestFormPk) {
			var dialogArgument = form1.HAccountID.value;
			var retVal = window.showModalDialog('./Dialog/StocedInOurBranch.aspx?S=' + RequestFormPk + '&CL=72', dialogArgument, "dialogHeight:600px; dialogWidth:900px; resizable:1; status:0; scroll:0; help:0; ");
			if (retVal == "Y") { window.document.location.reload(); }
		}
		function SetPrice(RequestFormPk) {
			var dialogArgument = form1.HAccountID.value;
			var retVal = window.showModalDialog('./Dialog/FixCharge.aspx?S=' + RequestFormPk, dialogArgument, "dialogHeight:500px; dialogWidth:750px; resizable:0; status:0; scroll:1; help:0; ");
			if (retVal == "Y") { window.document.location.reload(); }		
		}
		function Goto(gubun, value) {
			switch (gubun) {
				case "CompanyInfo":
					if (value == "") {
						alert("회사정보가 아직 등록되지 않았습니다");
					}
					else {
						location.href = "../Logistics/CompanyView_Logistics.aspx?M=View&S=" + value; break;
					}
			}
		}
	</script>
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px;" >
    <form id="form1" runat="server">
		<asp:ScriptManager ID="SM" runat="server"><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
		<uc1:LogedWithoutRecentRequest ID="LogedWithoutRecentRequest1" runat="server"/>
        <uc2:Loged ID="Loged1" runat="server" Visible="false" />
        <uc3:Loged ID="Loged2" runat="server" Visible="false" />
		<div style="background-color:White; width:850px; height:100%; padding:25px; ">
            <asp:Panel Height="40px" ID="P5051" Visible="false" runat="server" ><asp:Button ID="Button1" Text="CNYW"  runat="server" OnClick="Button1_Click"  UseSubmitBehavior="False"  style="width:100px;"/><asp:Button ID="Button2" Text="CNSX"  runat="server" OnClick="Button2_Click"  UseSubmitBehavior="False"  style="width:100px;"/></asp:Panel>
            <asp:Panel Height="40px" ID="P5455" Visible="false" runat="server" ><asp:Button ID="Button3" Text="CNYW"  runat="server" OnClick="Button3_Click"  UseSubmitBehavior="False"  style="width:100px;"/><asp:Button ID="Button4" Text="CNSX"  runat="server" OnClick="Button4_Click"  UseSubmitBehavior="False"  style="width:100px;"/></asp:Panel>
			<%=HtmlList %>
			<input type="hidden" id="HAccountID" value="<%=MemberInfo[2] %>" />
		</div>
    </form>
</body>
</html>