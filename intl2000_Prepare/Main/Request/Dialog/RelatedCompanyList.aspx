<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RelatedCompanyList.aspx.cs" Inherits="Request_Dialog_RelatedCompanyList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script type="text/javascript">
		function BTNSelectClick(CompanyRelationPk, TargetCompanyPk, TargetCompanyNick, CompanyCode, RegionCode, BranchPk) {
			//window.returnValue = true;
			//returnValue = new Array();
			//returnValue[0] = CompanyRelationPk;
			//returnValue[1] = TargetCompanyPk;
			//returnValue[2] = TargetCompanyNick;
			//returnValue[3] = CompanyCode;
			//returnValue[4] = RegionCode;
			//returnValue[5] = BranchPk;
			var SorC = form1.SorC.value;

			opener.SelectWhoResult(SorC, TargetCompanyPk, TargetCompanyNick, CompanyCode, RegionCode, BranchPk);
			self.close();
		}
	</script>
	<link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	 <style type="text/css">
    	.tdSubT { border-bottom:solid 2px #93A9B8; padding-top:10px; }
		.td01 { background-color:#f5f5f5; text-align:center; height:20px; width:150px; border-bottom:dotted 1px #E8E8E8;	 padding-top:2px; padding-bottom:2px; }
    	.td02 { border-bottom:dotted 1px #E8E8E8; padding-top:2px; padding-bottom:2px; }
		input { text-align:center; }
    </style>
</head>
<body style="background-color:#999999; padding:10px; width:480px; ">
    <form id="form1" runat="server" >
    <div style="width:440px;   padding-left:5px; padding-right:5px;  background-color:white;">
		<table border='0' cellpadding='0' cellspacing='0' style="width:440px; ">
			<%=CustomerList %>
		</table>		
        <input type="hidden" id="SorC" value="<%=Request.Params["C"] %>" />
    </div>
        
    </form>
</body>
</html>