<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForCapture_PackingList.aspx.cs" Inherits="CustomClearance_ForCapture_PackingList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<style type="text/css">
		.Title{border:solid 1px black; width:692px; height:30px; font-size:18px; text-align:center; letter-spacing:5px; padding-top:5px; }
		.Shipper{border:solid 1px black; height:85px; margin-top:-1px; padding:5px; font-size:13px;  }
		.PortOfLoading{border:solid 1px black; height:41px; margin-top:-1px; padding:5px; font-size:13px;  }
		.FinalDestination{border:solid 1px black; height:41px; margin-top:-1px; margin-left:-1px;  padding:5px; font-size:13px;  }
		.InvoiceNo{border:solid 1px black; margin-left:-1px; padding:5px; }
		.BL{border-left:solid 1px black; }
		.BB{border-bottom:solid 1px black;}
		.BR{border-right:solid 1px black;}
		.BT{ border-top:solid 1px black;}
		.InputButton{width:110px; height:23px; margin:5px; }
		div#quick_menu {position:absolute; left:50%; margin-top:100px; margin-left:300px; border:1px solid #999999; background:#eeeeee;}
    </style>
</head>
<body style="background-color:#E4E4E4; width:900px; margin:0 auto; padding-top:10px; font-family:@Arial Unicode MS, Arial;" >
    <form id="form1" runat="server">
       <input type="hidden" id="HGubun" value="<%=gubun %>" />
    <div>
    <div>
    	<div class="ContentsTopMenu">
		<div id="PrintArea" >
         <% if(gubun=="Stamp"){ %>
					<div style=" position: absolute; width: 100px; height: 100px; margin-left: 230px; margin-top: 500px; ">
						<img src="/Images/Check_Back.png" style="width:75px; height:225px;" />
					</div>
					<div style=" position: absolute; width: 100px; height: 100px; margin-left: 230px; margin-top: 650px; ">
						<img src="/Images/Check_Stamp.png" style="width:73px; height:74px;" />
					</div>
				<% } %>


		<div style="width:694px; font-size:13px;   background-color:White;" >
			<div class="Title">PACKING LIST</div>
			<div style="width:346px; float:left;">
				<div id="Shipper" class="Shipper" >
					Shipper
					<div style="padding-left:5px; padding-top:5px; line-height:17px; font-size:13px;  letter-spacing:-1px; ">
						<div id="DivShipperName" ><%=Shipper %></div>
						<div id="DivShipperAddress"><%=ShipperAddress %></div>
					</div>
				</div>
				<div id="Consignee" class="Shipper"      >
					Consignee
					<div style="padding-left:5px; padding-top:5px; line-height:17px; font-size:13px;  letter-spacing:-1px; ">
						<div id="DivConsigneeName" ><%=Consignee %></div>
						<div id="DivConsigneeAddress" ><%=ConsigneeAddress %></div>					
					</div>
				</div>
				<div id="NotifyParty" class="Shipper"      >
					Notify Party
					<div style="padding-left:5px; padding-top:5px; line-height:17px; font-size:13px;  letter-spacing:-1px; ">
						<div id="DivNotifyPartyName" ><%=NotifyParty %></div>
						<div id="DivNotifyPartyAddress" ><%=NotifyPartyAddress %></div>					
					</div>
				</div>
				<div style="width:173px; height:104px; float:left; ">
					<div class="PortOfLoading" id="PortOfLanding">
						Port of loading
						<div id="DivPortOfLanding" style="padding:5px;"><%=PortOfLanding %></div>
					</div>
					<div class="PortOfLoading" id="Carrier">
						Carrier
						<div id="DivCarrier" style="padding:5px;"><%=Carrier %></div>	
					</div>
				</div>
				<div style="width:173px; height:104px; float:right; ">
					<div class="FinalDestination" id="FinalDestination">
						Finial destination
						<div id="DivFinialDestination" style="padding:5px;"><%=FinalDestination %></div>	
					</div>
					<div class="FinalDestination" id="SailingOnOrAbout">
						Sailing on or About
						<div id="DivSailingOnOrAbout" style="padding:5px;"><%=SailingOnOrAbout %></div>	
					</div>
				</div>
			</div>
			<div style="width:347px; float:right;">
				<div class="InvoiceNo" id="InvoiceNo" style="height:40px; margin-left:-2px; margin-top:-1px; ">
					Date & No of Invoice
					<div style="padding-left:5px; padding-top:5px;"><%=InvoiceNo %></div>
				</div>
				<div class="InvoiceNo" id="PaymentTerms" style="height:40px; margin-left:-2px; margin-top:-1px; ">
					Payment terms
					<div style="padding-left:5px; padding-top:5px;  "><%=PaymentTerms %></div>
				</div>
				<div class="InvoiceNo" id="Buyer" style="height:40px; margin-left:-2px;  margin-top:-1px; ">
					Buyer
					<div style="padding:5px; "><%=Buyer %></div>
				</div>

				<div class="InvoiceNo" id="OtherReferences"     style="margin-left:-2px;  height:228px; margin-top:-1px; ">
					Other References
					<div style="padding-left:25px; padding-top:15px;"><%=Memo %></div>
				</div>
			</div>
			<div style="width:694px; clear:both; " id="ItemTable"     >
				<table border="0" cellpadding="0" cellspacing="0" style="width:694px;">
					<thead>
						<tr style="height:30px;">
							<td class="BB BL" style="width:120px; padding-left:10px; font-weight:bold;">Marks & <br />Number of PKGS</td>
							<td class="BB BL" style="padding:5px; text-align:center; font-weight:bold; ">Description of Goods</td>
							<td class="BB BL" style="width:89px; text-align:center; font-weight:bold; ">Quantity</td>
							<td class="BB BL" style="width:85px; text-align:center; font-weight:bold; ">G-Weight</td>
							<td class="BB BL" style="width:85px; text-align:center; font-weight:bold; ">N-Weight</td>
							<td class="BB BL BR" style="width:85px; text-align:center; font-weight:bold; ">Volume</td>
						</tr>
						<tr style="height:25px;">
							<td class="BL" >&nbsp;</td>
							<td class="BL" >&nbsp;</td>
							<td class="BL" >&nbsp;</td>
							<td class="BL" >&nbsp;</td>
							<td class="BL" style="text-align:center;" ></td>
							<td class="BL BR" >&nbsp;</td>
						</tr>
					</thead>
					<tbody>
						<%=ItemList %>
						<tr style="height:23px;">
							<td class="BL BT" colspan="2"  style="text-align:center; padding-right:10px;   " >TOTAL</td>
							<td class="BT" style="text-align:right; padding-right:10px; font-weight:bold;" ><%=TotalQuantity %></td>
							<td class="BT" style="text-align:right; padding-right:10px; font-weight:bold;" ><%=TotalGrossWeight %></td>
							<td class="BT" style="text-align:right; padding-right:10px; font-weight:bold;" ><%=TotalNetWeight %></td>
							<td class="BR BT" style="text-align:right; padding-right:10px; font-weight:bold;" ><%=TotalVolume %></td>
						</tr>
					</tbody>
				</table>
			</div>
			<%--<div style="position:absolute; background-image:url('../UploadedFiles/3/Yantai.jpg'); background-repeat:no-repeat; z-index:1;"></div>--%>
			<table border="0" cellpadding="0" cellspacing="0" style="width:694px; margin-top:-1px;"  >
				<tr style="height:120px;">
					<td class="BL BB" style="text-align:center;  ">&nbsp;</td>
					<td class="BR BB" style="width:350px; text-align:center; ">&nbsp;</td>
				</tr>
			</table>
			<%=StampImg %>
		</div>
		</div>
	</div>	
    </div>    
    </div>
    </form>
</body>
</html>