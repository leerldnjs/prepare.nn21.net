<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OfferSheet.aspx.cs" Inherits="Request_OfferSheet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
		.Title{width:750px; height:30px; font-size:20px; font-weight:bold; text-align:center; padding:10px; letter-spacing:12px; }
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
	<div>
		<table border="0" cellpadding="0" cellspacing="0" style="width:770px;">
			<tr>
				<td style="width:55px; font-size:1px; ">&nbsp;</td>
				<td style="width:55px; font-size:1px; ">&nbsp;</td>
				<td style="width:55px; font-size:1px; ">&nbsp;</td>
				<td style="width:55px; font-size:1px; ">&nbsp;</td>
				<td style="width:55px; font-size:1px; ">&nbsp;</td>
				<td style="width:55px; font-size:1px; ">&nbsp;</td>
				<td style="width:55px; font-size:1px; ">&nbsp;</td>
				<td style="width:55px; font-size:1px; ">&nbsp;</td>
				<td style="width:55px; font-size:1px; ">&nbsp;</td>
				<td style="width:55px; font-size:1px; ">&nbsp;</td>
				<td style="width:55px; font-size:1px; ">&nbsp;</td>
				<td style="width:55px; font-size:1px; ">&nbsp;</td>
				<td style="width:55px; font-size:1px; ">&nbsp;</td>
				<td style="width:55px; font-size:1px; ">&nbsp;</td>
			</tr>
			<tr><td colspan="14" style="font-size:30px;">INTERNATIONAL LOGISTICS CO.,LTD</td></tr>
			<tr><td colspan="14">IL BLDG 3F, 31-1, SHINHEUNG-DONG 1GA, JUNG-GU, INCHEON-CITY, KOREA.</td></tr>
			<tr>
				<td colspan="3" style="border-bottom:solid 3px black;">TEL : 032-772-8481</td>
				<td colspan="3" style="border-bottom:solid 3px black;">FAX : 032-765-8688</td>
				<td colspan="8" style="border-bottom:solid 3px black;">&nbsp;</td>
			</tr>
			<tr>
				<td colspan="2" >&nbsp;</td>
				<td colspan="4" >(Ocean InBound)</td>
				<td colspan="8" >&nbsp;</td>
			</tr>
			<tr><td colspan="14">&nbsp;</td></tr>
			<tr>
				<td colspan="4" >&nbsp;</td>
				<td colspan="6" style=" border:solid 2px black; text-align:center; font-size:25px;" >D E B I T</td>
				<td colspan="4" >&nbsp;</td>
			</tr>
			<tr><td colspan="14">&nbsp;</td></tr>
			<tr><td colspan="14">&nbsp;</td></tr>
			<tr><td colspan="14" style="border-bottom:solid 3px black; font-size:20px;  ">TO : SHANDONG JASU INTERNATIONAL LOGISTICS CO.,LTD.</td></tr>
			<tr><td colspan="14">&nbsp;</td></tr>
			<tr><td colspan="14">ADD : ROOM. 4B2, WANLONG SCIENCE AND TECHNOLOGY ZONE, NO.93 SHIFU STREET, YANTAI CHINA</td></tr>
			<tr><td colspan="14" style="border-bottom:solid 1px black;">&nbsp;</td></tr>
			<tr>
				<td colspan="8">&nbsp;</td>
				<td colspan="5">&nbsp;&nbsp;&nbsp;Ref No :6894</td>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td colspan="6">&nbsp;&nbsp;&nbsp;Vessel Name : HUADONG PEARL (V-2746)</td>
				<td colspan="1">&nbsp;</td>
				<td colspan="5">&nbsp;&nbsp;&nbsp;Issue Date : 2011/05/19</td>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td colspan="6">&nbsp;&nbsp;&nbsp;Container Type : LCL</td>
				<td colspan="1">&nbsp;</td>
				<td colspan="5">&nbsp;&nbsp;&nbsp;E.T.D : 2011/05/19</td>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td colspan="6">&nbsp;&nbsp;&nbsp;Q'TY : 536CT</td>
				<td colspan="1">&nbsp;</td>
				<td colspan="5">&nbsp;&nbsp;&nbsp;E.T.A : 2011/05/19</td>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td colspan="6">&nbsp;&nbsp;&nbsp;Weight : 22,701.50 KGS</td>
				<td colspan="1">&nbsp;</td>
				<td colspan="5">&nbsp;&nbsp;&nbsp;P.O.L : SHIDAO, CHINA</td>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td colspan="6">&nbsp;&nbsp;&nbsp;Measurment : 79.070 CBM</td>
				<td colspan="1">&nbsp;</td>
				<td colspan="5">&nbsp;&nbsp;&nbsp;P.O.D : INCHON, KOREA</td>
				<td>&nbsp;</td>
			</tr>
			<tr><td colspan="14">&nbsp;</td></tr>
			<tr>
				<td>&nbsp;</td>
				<td  colspan="8" style="border:solid 1px black;">CONTENTS</td>
				<td colspan="2" style="border-right:solid 1px black; border-top:solid 1px black;  border-bottom:solid 1px black;">PREPAID</td>
				<td colspan="2" style="border-right:solid 1px black; border-top:solid 1px black; border-bottom:solid 1px black;">COLLECT</td>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td>&nbsp;</td>
				<td colspan="3" style="border-left:solid 1px black; border-bottom:solid 1px black; ">&nbsp;&nbsp;&nbsp;&nbsp;INTL11051340381</td>
				<td  colspan="5" style="border-bottom:solid 1px black; border-right:solid 1px black; ">OCEAN FREIGHT CHARGE (LCL)</td>
				<td colspan="2" style="border-bottom:solid 1px black; border-right:solid 1px black;  ">&nbsp;</td>
				<td colspan="2" style="border-bottom:solid 1px black;  border-right:solid 1px black; ">110.00</td>
			</tr>
		</table>
	</div>
	<div>-------------------------------------------------------------------------------------</div>
	<div style="width:752px; ">
		<div class="Title">OFFER SHEET</div>
		<table border="0" cellpadding="0" cellspacing="0" style="font-size:18px; width:752px; line-height:30px;  ">
			<tr>
				<td style="width:80px;">APPLICANT : </td>
				<td style="width:100px;">&nbsp;</td>
				<td style="width:50px;">NO : </td>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td>COUNTRY OF ORIGIN : </td>
				<td style="width:100px;">&nbsp;</td>
				<td style="width:50px;">DATE : </td>
				<td>&nbsp;</td>
			</tr>
			<tr><td>TERMS OF PAYMENT : </td><td colspan="3"></td></tr>
			<tr><td>BENEFICIARY : </td><td colspan="3"></td></tr>
			<tr><td>BANK : </td><td colspan="3"></td></tr>
			<tr><td>SWIFT NO : </td><td colspan="3"></td></tr>
			<tr><td>ACCOUNT : </td><td colspan="3"></td></tr>
			<tr><td>FROM : </td><td colspan="3"></td></tr>
			<tr><td>TO : </td><td colspan="3"></td></tr>		
			<tr><td>PARTIAL SHIPMENTS : </td><td colspan="3"></td></tr>		
			<tr><td>TRANSSHIPMENT : </td><td colspan="3"></td></tr>		
			<tr><td>LASTEST DATE OF SHIPMENT : </td><td colspan="3"></td></tr>		
			<tr><td colspan="4">WE HEREBY CERTIFY THAT WE AGREE TO SUPPLY THE COMMODITY</td></tr>
			<tr><td colspan="4">AS FOLLOW</td></tr>
		</table>
		<table border="0" cellpadding="0" cellspacing="0" style="border:solid 1px black; width:750px; ">
			<tr style="height:40px;">
				<td style="border:solid 1px black; ">Description Of Goods</td>
				<td style="border:solid 1px black; ">Q'TY</td>
				<td style="border:solid 1px black; ">U/Price</td>
				<td style="border:solid 1px black; ">Amount</td>
			</tr>
			<tr style="height:40px;">
				<td style="border:solid 1px black; ">&nbsp;</td>		
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
			</tr>
			<tr style="height:40px;">
				<td style="border:solid 1px black; ">&nbsp;</td>		
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
			</tr>			
			<tr style="height:40px;">
				<td style="border:solid 1px black; ">&nbsp;</td>		
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
			</tr>			
			<tr style="height:40px;">
				<td style="border:solid 1px black; ">&nbsp;</td>		
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
			</tr>			
			<tr style="height:40px;">
				<td style="border:solid 1px black; ">&nbsp;</td>		
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
			</tr>			
			<tr style="height:40px;">
				<td style="border:solid 1px black; ">&nbsp;</td>		
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
			</tr>			
			<tr style="height:40px;">
				<td style="border:solid 1px black; ">&nbsp;</td>		
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
			</tr>			
			<tr style="height:40px;">
				<td style="border:solid 1px black; ">&nbsp;</td>		
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
				<td style="border:solid 1px black; ">&nbsp;</td>			
			</tr>			
		</table>
		</div>
		<div style=" font-size:18px; padding:30px;  margin-left:350px; " >
			BENEFICIARY ;
		</div>
    </form>
</body>
</html>
