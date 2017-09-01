<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransportBetweenBranchList.aspx.cs" Inherits="Admin_TransportBetweenBranchList" Debug="true" %>
<%@ Register src="LogedWithoutRecentRequest.ascx" tagname="LogedWithoutRecentRequest" tagprefix="uc1" %>
<%@ Register src="../CustomClearance/Loged.ascx" tagname="Loged" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />    
	<script type="text/javascript">
		function GoExcelDown(BBHPk) {
			var AccountID = document.getElementById("HAccountID").value;
			if (AccountID == "ilyt1" || AccountID == "ilyt2" || AccountID == "ilyt3" || AccountID == "ilyt4" || AccountID == "ilyt0" || AccountID == "ilic31") {
				location.href = "../UploadedFiles/FileDownloadWithExcel.aspx?S=" + BBHPk + "&G=YT_Clearance";
			} else {
				location.href = "../UploadedFiles/FileDownloadWithExcel.aspx?S=" + BBHPk;
			}
		}
		function GotoSelect(Gubun, Country) {
			var selected = document.getElementById("OptionStorageHistory").value;
			location.href = "./TransportBetweenBranchList.aspx?G=" + Gubun + "&C=" + Country + "&S=" + selected;
		}
		function gotoDifferantGubun(Gubun, Country) { location.href = "./TransportBetweenBranchList.aspx?G=" + Gubun + "&C=" + Country; }
		function GoAbjiDown(date, branchPk, InorOut) { location.href = "../UploadedFiles/FileDownloadWithExcel.aspx?G=TodayAbji&S=" + branchPk + "&D=" + date + "&G2=" + InorOut; }
		function GoAbjiDown2(date, branchPk, InorOut,Type) { location.href = "../UploadedFiles/FileDownloadWithExcel.aspx?G=TodayAbjiForJaemu&S=" + branchPk + "&D=" + date + "&G2=" + InorOut+"&G3=" + Type; }
	</script>
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px;" >
	<input type="hidden" id="HAccountID" value="<%=MemberInfo[2] %>" />
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server" ><Services><asp:ServiceReference Path="~/WebService/Admin.asmx" /></Services></asp:ScriptManager>
	<uc1:LogedWithoutRecentRequest ID="LogedWithoutRecentRequest1" runat="server" />
     <uc2:Loged ID="Loged1" runat="server" Visible="false" />
    <div style="background-color:White; width:850px; height:100%; padding:25px; ">
		<p><%=FromRegion %></p>
		<%=InnerHtml %>		
    </div>
    </form>
</body>
</html>