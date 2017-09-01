<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForCapture_FreightCharge.aspx.cs" Inherits="CustomClearance_ForCapture_FreightCharge" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
	<link href="~/Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
		.OurLogo{ border-bottom:solid 2px black; width:692px; height:75px; text-align:center; padding-top:3px; }
		.Shipper{border:solid 1px black; height:90px; margin-top:-1px; padding:5px; }
		.PortOfLoading{border:solid 1px black; height:51px; margin-top:-1px; padding:5px; }
		.FinalDestination{border:solid 1px black; height:51px; margin-top:-1px; margin-left:-1px;  padding:5px; }
		.InvoiceNo{border:solid 1px black; margin-left:-1px; padding:5px; }
		.BL{border-left:solid 1px black; }
		.BB{border-bottom:solid 1px black;}
		.BR{border-right:solid 1px black;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="PrintArea" >
		<div style="width:694px; background-color:White;" >
			<%=Title %>
			<div style="padding:10px; width:150px; height:20px; text-align:center; margin:0 auto; border:2px solid black; margin-top:20px; margin-bottom:20px; font-size:17px; letter-spacing:3px;   ">INVOICE</div>
			<%=Header %>
			<br />
			<%=Item %>
			<%=Freight %>
			<%=BankInfo %>
            <table border="0" style="width:694px; margin-top:10px; margin-left:5px; ">
                <tr>
                    <td align="left"><strong>◆ 화남지구</strong> : 廣州,中山,東莞,佛山,順德,山头,朝州,江門,</td>
                    <td align="left"><strong>◆ 화동지구</strong> : 上海,溫州,义乌,杭州,寧波,晋江,廈門,紹興,蘇州</td>
                </tr>
                <tr>
                    <td align="left" style="padding-left:82px;">深川,香港</td>
                    <td align="left" style="padding-left:82px;">상담전화: 400-708-1600/ 186-58787077</td>
                </tr>
                <tr>
                    <td align="left" style="padding-left:82px;">상담전화: 400-880-8300/ 138-26214498</td>
                    <td align="left" style="padding-left:82px;">E-Mail: ilwz@nn21.com</td>
                </tr>
                <tr>
                    <td align="left" style="padding-left:82px;">E-Mail: ilgz@nn21.com</td>
                    <td align="left" style="padding-left:82px;">&nbsp;</td>
                </tr>
                <tr>
                    <td height="5">&nbsp;</td>
                </tr>
                <tr>
                    <td align="left"><strong>◆ 화북지구</strong> : 靑島,橋州,橋南,煙台,威海,北京,濟南.天津,</td>
                    <td align="left"><strong>◆ 동북지구</strong> : 沈陽,大連,丹東,營口,長春,延吉</td>
                </tr>
                <tr>
                    <td align="left" style="padding-left:82px;">衡水,鄭州</td>
                    <td align="left" style="padding-left:82px;">(신의주,개성공단)</td>
                </tr>
                <tr>
                    <td align="left" style="padding-left:82px;">상담전화: 400-708-0060/ 186-06319989</td>
                    <td align="left" style="padding-left:82px;">상담전화: 400-708-0060/ 138-04039968</td>
                </tr>
                <tr>
                    <td align="left" style="padding-left:82px;">E-Mail: ilsd@nn21.com</td>
                    <td align="left" style="padding-left:82px;">E-Mail: ildb@nn21.com</td>
                </tr>
                </table>
		</div>
	</div>
    </form>
</body>
</html>