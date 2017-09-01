<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompanyJoinFromEmail.aspx.cs" Inherits="Member_CompanyJoinFromEmail" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px; ">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" runat="server" ><Services><asp:ServiceReference Path="~/WebService/Member.asmx" /></Services> </asp:ScriptManager>
    <div style="width:900px;">
		<div style="background-color:White; padding:20px; ">
			<p>안녕하세요. 저희 아이엘 국제물류를 이용해 주셔서 감사합니다. </p>
			<p>현재 접수된 물건의 상세내역을 확인하시기 위해 아래 버튼으로 들어가 신규가입을 해주세요. </p>
			<p>앞으로도 고객님의 원할한 수입ㆍ수출업무를 위해 최선을 다하겠습니다. <asp:Button ID="Button1" runat="server" Text="신규가입" PostBackUrl="CompanyJoinStep1.aspx" /></p>
		</div>
	</div>
    </form>
</body>
</html>