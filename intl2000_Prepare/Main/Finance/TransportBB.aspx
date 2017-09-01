<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransportBB.aspx.cs" Inherits="Finance_TransportBB" %>

<%@ Register Src="../Admin/LogedWithoutRecentRequest11.ascx" TagName="Loged" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="../Common/IntlWeb.css" rel="stylesheet" type="text/css" />

    <script src="../Common/jquery-1.10.2.min.js"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>

    <script type="text/javascript">
    	function serch() {
    		var DepartureBranch = document.getElementById("DepartureBranch").value;
    		var ArrivalBranch = document.getElementById("ArrivalBranch").value;
    		var TB_DepartureDate = document.getElementById("TB_DepartureDate").value;
    		var TB_ArrivalDate = document.getElementById("TB_ArrivalDate").value;
    		var TransportWayCL = document.getElementById("TransportWayCL").value;
    		var Warehouse = document.getElementById("Warehouse").value;
    		var ArrivalPortchecked = "";
    		if (form1.ArrivalPort.checked == true) {
    			ArrivalPortchecked = "Y";
    		} else if (form1.NotArrivalPort.checked == true) {
    			ArrivalPortchecked = "N";
    		}
    		location.href("TransportBB.aspx?DB=" + DepartureBranch + "&AB=" + ArrivalBranch + "&SD=" + TB_DepartureDate + "&ED=" + TB_ArrivalDate + "&T=" + TransportWayCL + "&W=" + Warehouse + "&Y=" + ArrivalPortchecked);
    	}
    	function ArrivalPortClick(Gubun) {
    		if (Gubun == "Y") {
    			$("#ArrivalBranch").val("3157");
    			$("#ArrivalBranch").hide();
    			form1.ArrivalPort.checked = true;
    			form1.NotArrivalPort.checked = false;
    		} else if (Gubun == "N") {
    			$("#ArrivalBranch").val("3157");
    			$("#ArrivalBranch").hide();
    			form1.ArrivalPort.checked = false;
    			form1.NotArrivalPort.checked = true;
    		}
    		else {
    			$("#ArrivalBranch").show();
    			form1.ArrivalPort.checked = false;
    			form1.NotArrivalPort.checked = false;
    		}
    	}
    	$(document).ready(function () {
    		$("#TB_DepartureDate").datepicker();
    		$("#TB_DepartureDate").datepicker("option", "dateFormat", "yymmdd");
    		$("#TB_DepartureDate").val($("#HTB_DepartureDate").val());
    		$("#TB_ArrivalDate").datepicker();
    		$("#TB_ArrivalDate").datepicker("option", "dateFormat", "yymmdd");
    		$("#TB_ArrivalDate").val($("#HTB_ArrivalDate").val());

    		ArrivalPortClick($("#HArrivalPortchecked").val());
    	});
    </script>
</head>
<body style="background-color: #E4E4E4; width: 1100px; margin: 0 auto; padding-top: 10px;">
	<form id="form1" runat="server">
		<uc1:Loged ID="Loged1" runat="server" />
<div style="background-color: White; width: 1050px; height: 100%; padding: 25px;">
            <div>
                Branch : 
			<select id="DepartureBranch">
                <option value="">ALL</option>
                <%=OptionDB %>
                </select>
                ~ 
			<select id="ArrivalBranch"><%=OptionAB %></select>
                <div style="float: right;">
					<a href="/Admin/SettlementWithStorage_Multiple.aspx" >매입비용 일괄입력</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                    <input type="checkbox" id="ArrivalPort" onclick="ArrivalPortClick('Y');" />
                    <label for="ArrivalPort">평택 도착</label>
                    <input type="checkbox" id="NotArrivalPort" onclick="ArrivalPortClick('N');" />
                    <label for="NotArrivalPort">평택 도착 제외</label>
                </div>
                
            </div>
            <p>
                <input type="hidden" id="HTB_DepartureDate" value="<%=StartDate %>" />
                <input type="hidden" id="HTB_ArrivalDate" value="<%=EndDate %>" />
                Data :
            <input id="TB_DepartureDate" size="10" type="text" style="text-align: center;" />
                ~ 
			<input id="TB_ArrivalDate" size="10" type="text" style="text-align: center;" />
            </p>
            <p>
                Type : 
			<select id="TransportWayCL">
                <%=OptionT %>
            </select>
            </p>
            <p>
                Warehouse : 
			<select id="Warehouse">
                <option value="">ALL</option>
                <%=OptionWarehouse %>
            </select>
            </p>
            <p>
                <input type="button" value="Serch" onclick="serch();" />
                <input type="hidden" id="HArrivalPortchecked" value="<%=Request.Params["Y"] + ""%>" />
            </p>
            <div><%=TransportBBList %></div>
        </div>
    </form>
</body>
</html>