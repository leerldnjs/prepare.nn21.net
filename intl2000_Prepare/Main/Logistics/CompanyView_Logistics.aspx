<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeFile="CompanyView_Logistics.aspx.cs" Inherits="Logistics_CompanyView_Logistics" %>
<%@ Register src="~/Logistics/Logedsmall.ascx" tagname="Loged" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<style type="text/css">
		.tdSubT { border-bottom:2px solid #93A9B8; padding-top:30px; padding-bottom:3px; }
		.td01{ width:90px;  background-color:#f5f5f5;  text-align:center; height:25px; border-bottom:1px dotted #E8E8E8;}
		.td02{ width:200px;  padding-left:10px; border-bottom:1px dotted #E8E8E8; background-color:White;	}
		.td03{ width:110px;  padding-left:10px; border-bottom:1px dotted #E8E8E8; background-color:White;	}
		.td023{width:410px;  padding-bottom:4px; padding-left:10px;border-bottom:1px dotted #E8E8E8;background-color:White;}
		.tdStaffBody{text-align:center; padding:5px; border:dotted 1px #E8E8E8; background-color:White;}
		.BB{ border-bottom:1px solid #93A9B8;}
		.BR{ border-right:1px solid #93A9B8;}
    </style>
    <link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<script src="../Common/RegionCode.js?version=20131029" type="text/javascript"></script>
	<script type="text/javascript">
		function Goto(value) {
			switch (value) {
				case "back": history.back(); break;
			    case "talkBusiness": window.open('../Admin/Dialog/TalkBusiness.aspx?S=' + form1.HCompanyPk.value, '', 'location=no, directories=no, resizable=no, status=no, toolbar=1, menubar=no, scrollbars=1, top=200px; left=200px; height=700px; width=600px;'); break;
			    case "basic": location.href = "Warehouse_Logistics.aspx?M=View&S=" + form1.HCompanyPk.value; break;
			}
		}
	</script>
</head>
<body style="background-color: #E4E4E4; width: 660px; margin: 0 auto; padding-top: 10px;">
    <form id="form1" runat="server">
        <uc1:Loged ID="Loged1" runat="server" />
        <asp:ScriptManager ID="SM" runat="server">
            <Services>
                <asp:ServiceReference Path="~/WebService/Admin.asmx" />
            </Services>
        </asp:ScriptManager>
        <div style="background-color: White; min-height: 500px;">
            <div style="padding-right: 20px; padding-top: 20px; text-align: right;">
                <%=HtmlButton %>
            </div>
            <div style="width:600px; padding:0px 20px 20px 20px; clear: both;">
                <%=CompanyInfo %>
                <%=StaffInfo %>
                <%=WarehouseInfo %>
            </div>
            <div style="width: 840px; padding: 20px; clear: both;">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr style="height: 1px;">
                        <td style="width: 180px; font-size: 1px;">&nbsp;</td>
                        <td style="width: 25px; font-size: 1px;">&nbsp;</td>
                        <td style="width: 25px; font-size: 1px;">&nbsp;</td>
                        <td style="width: 180px; font-size: 1px;">&nbsp;</td>
                        <td style="width: 90px; font-size: 1px;">&nbsp;</td>
                        <td style="width: 90px; font-size: 1px;">&nbsp;</td>
                        <td style="width: 25px; font-size: 1px;">&nbsp;</td>
                        <td style="width: 25px; font-size: 1px;">&nbsp;</td>
                        <td style="width: 180px; font-size: 1px;">&nbsp;</td>
                    </tr>
                    <%--<%=CompanyRelatedInfo%>--%>
                </table>
            </div>
            <input type="hidden" id="HCompanyPk" value="<%=CompanyPk %>" />
            <input type="hidden" id="HAccountID" value="<%=Session["ID"]  %>" />
        </div>
    </form>
</body>
</html>
