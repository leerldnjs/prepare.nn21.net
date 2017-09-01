<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Taxpaid.aspx.cs" Inherits="Admin_Taxpaid" %>
<%@ Register src="../Admin/LogedWithoutRecentRequest11.ascx" tagname="Loged" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />

	<script type="text/javascript">
		function SetTaxpaid(RequestFormPk, AccountID, GubunCL, DocumentStepCL) {
			Admin.SetTaxpaid(RequestFormPk, AccountID, GubunCL, DocumentStepCL, function (result) {
				if (result == "1") {
					alert("Success");
					location.reload();
				}
			}, function (result) { alert("ERROR : " + result); });
		}
	</script>
</head>
    <body style="background-color:#E4E4E4; width:1100px; margin:0 auto; padding-top:10px;" >
    <form id="form1" runat="server">
	<asp:ScriptManager ID="SM" runat="server">
            <Services>
                <asp:ServiceReference Path="~/WebService/Admin.asmx" />
            </Services>
        </asp:ScriptManager>
	<uc1:Loged ID="Loged1" runat="server" />
    <div style="background-color:White; width:1050px; height:100%; padding:25px;">
		<p>
			<a href="/Admin/Taxpaid.aspx"><strong>세납</strong></a>&nbsp;&nbsp;|&nbsp;&nbsp;
			<a href="/Charge/CollectList.aspx">세금통장</a>
		</p>

       <%=ListHtml %>
       </div>
    </form>
</body>
</html>