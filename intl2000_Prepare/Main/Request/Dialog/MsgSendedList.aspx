<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MsgSendedList.aspx.cs" Inherits="Request_Dialog_MsgSendedList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
    	.tdSubT	{	border-bottom:solid 2px #93A9B8;	padding-top:10px;	}
		.td01	{	background-color:#f5f5f5; text-align:center; height:20px; width:150px;	border-bottom:dotted 1px #E8E8E8;	padding-top:2px;	padding-bottom:2px;	}
    	.td02	{	border-bottom:dotted 1px #E8E8E8;		padding-top:2px;	padding-bottom:2px;	}
		input{text-align:center; }
    </style>
</head>
<body style="background-color:#999999; padding:10px; width:480px; overflow-x:hidden; ">
    <form id="form1" runat="server">
    <div style="width:850px;   padding-left:10px; padding-right:10px;  background-color:white;">
		<%=SendedHtml %>
    </div>
    </form>
</body>
</html>
