<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ItemModifyList.aspx.cs" Inherits="RequestForm_ItemModifyList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   	<style type="text/css">
		.tdSubT	{border-bottom:solid 2px #93A9B8;	padding-top:10px; }
		.ItemTableIn{border-bottom:dotted 1px #E8E8E8; padding-top:4px; padding-bottom:4px; }
	</style>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table>
		<tr style="height:30px;" >
			<td bgcolor="#F5F5F5" height="20" align="center" width="60" ><%=GetGlobalResourceObject("Member", "Modify") %> ID</td>
			<td bgcolor="#F5F5F5" align="center" width="400" >내용</td>
			<td bgcolor="#F5F5F5" align="center" width="140">수정일시</td>
		</tr>
		<%=ItemModifyList %>
		</table>
    </div>
    </form>
</body>
</html>
