<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeliverySet_Logistics.aspx.cs" Inherits="Logistics_Dialog_DeliverySet_Logistics" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Delivery Set</title>
    <link href="../../Common/IntlWeb.css" rel="stylesheet" type="text/css" />
    <script src="../../Common/jquery-1.10.2.min.js"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
	<script type="text/javascript">
		var TOTALBOXCOUNT;
		var BOXCOUNTLIMIT;
		var NOTSELECTEDBOXCOUNT;
		var StorageName;
		var CompanyName;
		var PackingUnit;
		var IsSelecetedDelivery = false;
		window.onload = function () {
			form1.DepositWhereAfter.checked = true;
			form1.Text_DepositWhere.value = "착불";
			LoadDeliveryDefaultSetting();
			LoadSavedDelivery();
		}

		function LoadTransportBCHistory() {
			Admin.LoadTransportBCHistory(form1.HTransportBetweenCompanyPk.value, function (result) {
				document.getElementById("SPTransportBCHistory").innerHTML = result;
			}, function (result) { alert("ERROR : " + result); });
		}

		function LoadDeliveryDefaultSetting() {
			Admin.AddDeliveryPlaceOnload(form1.HConsigneePk.value, function (result) {
				for (var i = 0; i < result.length; i++) {
					switch (i) {
						case 2:
							if (result[i] == "") { continue; }
							var each = result[i].split("@@");
							document.getElementById("RegisterdInfo").innerHTML = "RegisterdInfo:  <strong>" + each[0] + " / " + each[2] + "</strong>";
							form1.TBContents.value = each[1];
							break;
					}
				}
			}, function (result) { alert("ERROR : " + result); });
		}

		function LoadSavedDelivery() {
			Admin.ModifyDeliveryPlaceOnLoad(form1.HOurBranchStorageOutPk.value, form1.HRequestFormPk.value, function (result) {
				if (result[0] == "N") {
					alert("ERROR"); opener.location.reload(); self.close(); return false;
				}
				if (form1.HOurBranchStorageOutPk.value == "N") {
					form1.HTransportBetweenCompanyPk.value = "N";
					form1.HStorageCode.value = "0";
					var ArrivalTime = result[0];
					if (ArrivalTime != "") {
						form1.ArrivalDate.value = ArrivalTime.substr(0, 8);
						if (ArrivalTime.length > 8) { form1.ArrivalHour.value = ArrivalTime.substr(8, 2); }
						if (ArrivalTime.length > 10) { form1.ArrivalMin.value = ArrivalTime.substr(10); }
					}
					form1.FromDate.value = result[0];

					TOTALBOXCOUNT = result[1];
					BOXCOUNTLIMIT = result[1];
					NOTSELECTEDBOXCOUNT = 0;
					document.getElementById("TotalBoxCount").innerHTML = result[1];
					document.getElementById("PackingUnit").innerHTML = result[2];
					document.getElementById("WeightNVolume").innerHTML = result[3] + "Kg " + result[4] + "CBM";
					form1.HWeight.value = result[3];
					form1.HVolume.value = result[4];
					form1.LeftBoxCount.value = BOXCOUNTLIMIT;

					if (result[6] == "") {
						form1.DepositWhereAfter.checked = true;
						form1.Text_DepositWhere.value = "착불";
					}
					else {
						form1.DepositWhereBefore.checked = true;
						form1.Text_DepositWhere.value = "현불";
						form1.Price.value = result[6];
						form1.MonetaryUnitCL.value = result[5];
					}
					form1.HPackingUnit.value = result[7];
					LoadTransportBCHistory();
				}
				else {
					if (result[0] != "") { document.getElementById("SPStorageName").innerHTML = " : " + result[0]; }
					StorageName = result[0];
					form1.HTransportBetweenCompanyPk.value = result[2];

					TOTALBOXCOUNT = result[5];
					BOXCOUNTLIMIT = parseInt(result[4]) + parseInt(result[1]);
					NOTSELECTEDBOXCOUNT = parseInt(result[4]);
					document.getElementById("TotalBoxCount").innerHTML = BOXCOUNTLIMIT;
					document.getElementById("PackingUnit").innerHTML = " / " + result[5] + " " + result[6];
					PackingUnit = result[6];

					document.getElementById("WeightNVolume").innerHTML = result[7] + "Kg " + result[8] + "CBM";
					form1.LeftBoxCount.value = result[1];
					form1.HWeight.value = result[7];
					form1.HVolume.value = result[8];

					form1.Delivery_Type.value = result[9];
					form1.Delivery_Title.value = result[10];
					form1.Delivery_DriverName.value = result[11];
					form1.Delivery_DriverMobile.value = result[12];
					form1.Delivery_TEL.value = result[13];
					form1.Delivery_CarSize.value = result[14];
					form1.Delivery_Price.value = result[25];
					/*
					0		RS["StorageName"]+"",		
					1		RS["BoxCount"]+"", 
					2		RS["TransportBetweenCompanyPk"]+"", 
					3		RS["StatusCL"]+"", 
					4		LeftCount, 
					5		RS["TotalPackedCount"]+"", 
					6		Common.GetPackingUnit(RS["PackingUnit"]+""), 
					7		Common.NumberFormat(RS["TotalGrossWeight"]+""), 
					8		Common.NumberFormat(RS["TotalVolume"]+""), 
					9		RS["Type"]+"", 
					10	RS["Title"]+"", 
					11	RS["DriverName"]+"", 
					12	RS["DriverTEL"]+"", 
					13	RS["TEL"]+"", 
					14	RS["CarSize"]+"", 
					15	RS["FromDate"]+"", 
					16	RS["ToDate"]+"", 
					17	RS["WarehouseInfo"]+"", 
					18	RS["WarehouseMobile"]+"", 
					19	RS["DepositWhere"]+"", 
					19	Common.NumberFormat(RS["Price"]+""), 
					20	RS["Memo"]+""
					*/
					form1.FromDate.value = result[15];
					var ArrivalTime = result[16];
					if (ArrivalTime != "") {
						form1.ArrivalDate.value = ArrivalTime.substr(0, 8);
						if (ArrivalTime.length > 8) { form1.ArrivalHour.value = ArrivalTime.substr(8, 2); }
						if (ArrivalTime.length > 10) { form1.ArrivalMin.value = ArrivalTime.substr(10); }
					}

					var WarehouseInfo
					if (result[17] == "") {
						WarehouseInfo = new Array('', '', '');
					}
					else {
						WarehouseInfo = result[17].split("@@");
					}

					form1.ToAddress.value = WarehouseInfo[0];
					form1.ToTEL.value = WarehouseInfo[1];
					form1.StaffName.value = WarehouseInfo[2];

					form1.StaffMobile.value = result[18];
					if (result[19] == "0") {
						form1.DepositWhereBefore.checked = true;
						form1.Text_DepositWhere.value = "현불";
					}
					else {
						form1.DepositWhereAfter.checked = true;
						form1.Text_DepositWhere.value = "착불";
					}
					form1.Price.value = result[20];
					form1.Memo.value = result[21];
					form1.HPackingUnit.value = result[22];
					form1.HStorageCode.value = result[23];
					form1.HTransportBetweenBranchPk.value = result[24];
					//form1.MemberMemo.value = result[25];
					LoadTransportBCHistory();

				}
			}, function (result) { alert("ERROR : " + result); });

		}
        function SelThisDriver(type, title, TEL, drivername, drivermobile, carsize) {
            form1.Delivery_Type.value = type;
            form1.Delivery_Title.value = title;
            form1.Delivery_TEL.value = TEL;
            form1.Delivery_DriverName.value = drivername;
            form1.Delivery_DriverMobile.value = drivermobile;
            form1.Delivery_CarSize.value = carsize;
        }
        function SelThisWarehouse(address, tel, staff, staffmobile) {
            form1.ToAddress.value = address;
            form1.ToTEL.value = tel;
            form1.StaffName.value = staff;
            form1.StaffMobile.value = staffmobile;
        }


        function BTN_Submit_Click() {
            form1.BTNSubmit.style.visibility = "hidden";
            if (form1.LeftBoxCount.value == "0" || form1.LeftBoxCount.value == "") {
                if (parseInt(form1.LeftBoxCount.value) > parseInt(BOXCOUNTLIMIT)) {
                    alert("배차할수 있는 수량보다 많습니다. 배차할수 없습니다.");
                    form1.BTNSubmit.style.visibility = "visible";
                    return false;
                }
                alert("박스수량이 정확하지 않습니다. 배차할수 없습니다.");
                form1.BTNSubmit.style.visibility = "visible";
                return false;
            }
            if (IsSelecetedDelivery) {
            	if (form1.ArrivalDate.value == "") {
            		alert("출고예정일을 넣어주세요.");
            		form1.BTNSubmit.style.visibility = "visible";
            		return false;
            	}
            }
            if (form1.ToAddress.value == "") {
                alert("주소를 넣어주세요");
                form1.BTNSubmit.style.visibility = "visible";
                return false;
            }
            var ArrivalTime = form1.ArrivalDate.value + form1.ArrivalHour.value + form1.ArrivalMin.value;
            var WarehouseInfo = form1.ToAddress.value + "@@" + form1.ToTEL.value + "@@" + form1.StaffName.value;

            var DepositWhere = "1";
            if (form1.DepositWhereBefore.checked == true) { DepositWhere = "0"; }

            //Admin.SetDeliveryPlace(form1.HOurBranchStorageOutPk.value, form1.HRequestFormPk.value, form1.HTransportBetweenBranchPk.value, form1.HTransportBetweenCompanyPk.value, form1.HConsigneePk.value, form1.HStorageCode.value, form1.LeftBoxCount.value, NOTSELECTEDBOXCOUNT, BOXCOUNTLIMIT, TOTALBOXCOUNT, form1.HPackingUnit.value, form1.FromDate.value, ArrivalTime,
            //WarehouseInfo, form1.StaffMobile.value, form1.Memo.value, DepositWhere, form1.Price.value, form1.Delivery_Type.value, form1.Delivery_Title.value, form1.Delivery_TEL.value, form1.Delivery_DriverName.value,
            //form1.Delivery_DriverMobile.value, form1.Delivery_CarSize.value,form1.MemberMemo.value, form1.HAccountID.value, function (result) {
            Admin.SetDeliveryPlace(form1.HOurBranchStorageOutPk.value, form1.HRequestFormPk.value, form1.HTransportBetweenBranchPk.value, form1.HTransportBetweenCompanyPk.value, form1.HConsigneePk.value, form1.HStorageCode.value, form1.LeftBoxCount.value, NOTSELECTEDBOXCOUNT, BOXCOUNTLIMIT, TOTALBOXCOUNT, form1.HPackingUnit.value, form1.FromDate.value, ArrivalTime,
			WarehouseInfo, form1.StaffMobile.value, form1.Memo.value, DepositWhere, form1.Price.value, form1.Delivery_Type.value, form1.Delivery_Title.value, form1.Delivery_TEL.value, form1.Delivery_DriverName.value,
			form1.Delivery_DriverMobile.value, form1.Delivery_CarSize.value, form1.Delivery_Price.value, form1.HAccountID.value, function (result) {
			    if (result == "1") {
			        if (form1.Delivery_Type.value != "") {
			            if (confirm("배차기사 지정 완료되었습니다. 출고완료지시하시겠습니까? ")) {
			                Admin.GoDeliveryOrder(form1.HTransportBetweenCompanyPk.value, "", form1.HAccountID.value, '', function (result) { }, function (result) { alert("ERROR : " + result); });
			            }
			        }
			        /*
					if (form1.Delivery_DriverMobile.value != "" && form1.StaffMobile.value != "") {
						if (confirm("SMS 보내시겠습니까? ")) {
							//Admin.SendDeliveryMSG(form1.HTransportBetweenCompanyPk.value, form1.HAccountID.value, form1.HOurBranchPk.value, form1.HConsigneePk.value, function (result) { }, function (result) { alert("ERROR : " + result); });
							SMSShow();
						}
						else {
							opener.location.reload();
							self.close();
							return false;
						}
					}
					*/
			        alert("Success");
			        opener.location.reload();
			        self.close();
			        return false;
			    }
			    else {
			        alert(result);
			        form1.BTNSubmit.style.visibility = "visible";
			        form1.DEBUG.value = result;
			        return false;
			    }
			}, function (result) {
			    form1.BTNSubmit.style.visibility = "visible";
			    alert("ERROR1 : " + result);
			});
        }
        function GoDeliveryOrder() {
            if (confirm("출고지시하시겠습니까?")) {
                Admin.GoDeliveryOrder(form1.HTransportBetweenCompanyPk.value, "", form1.HAccountID.value, "", function (result) {
                    if (result == "1") { alert("출고지시완료"); location.reload(); }
                    else {
                        alert(result);
                    }
                }, function (result) { alert("ERROR : " + result); });
            }
        }
        function SMSAutoSend() {
            SMSShow();            
            //SMSSend('DrivererSMSSend');
            SMSSend('CompanySMSSend');
            
        }
        function SMSShow() {
        	var CompanyMobile = form1.StaffMobile.value;
        	var DelivererMobile = form1.Delivery_DriverMobile.value;

        	var TempBoxCout;
        	if (TOTALBOXCOUNT == form1.LeftBoxCount.value) {
        		TempBoxCout = form1.LeftBoxCount.value + PackingUnit;
        	}
        	else {
        		TempBoxCout = form1.LeftBoxCount.value + PackingUnit + "(총 " + TOTALBOXCOUNT + PackingUnit + "중 분할)";
        	}

        	var TempArrivalTime;
        	if (form1.ArrivalDate.value == "") {

        	}
        	else {
        		TempArrivalTime = form1.ArrivalDate.value.substr(4, 2) + "/" + form1.ArrivalDate.value.substr(6, 2);
        	}
        	if (form1.ArrivalHour.value != "") { TempArrivalTime += form1.ArrivalHour.value; }
        	if (form1.ArrivalMin.value != "") { TempArrivalTime += ":" + form1.ArrivalMin.value; }


        	var TempPrice;
        	if (form1.DepositWhereBefore.checked == true) {
        		TempPrice = "현불 " + form1.Price.value;
        	}
        	else {
        		TempPrice = "착불 " + form1.Price.value;
        	}

        	var CompanyContents = TempBoxCout + "화물이 " + form1.Delivery_DriverName.value + "기사(" + DelivererMobile + ") 편으로 배차되었습니다. " + TempArrivalTime + " 도착예정, " + TempPrice + " 입니다.";

        	var DelivererContents = "물건 출발할때 화주분(" + CompanyMobile + ")께 전화주세요! " + StorageName + " " + CompanyName + " " + TempBoxCout + " 배차돼었습니다. : " + form1.ToAddress.value + " (" + form1.StaffName.value + ") 앞 " + TempPrice;

        	var Html =

			"<div style=\"width:300px; padding:10px;\">" +
			"	<fieldset style=\"padding:10px;\">" +
			"		<legend><strong>For Company</strong></legend>" +
			"		<textarea id=\"CompanyContents\" rows=\"10\" cols=\"40\">" + CompanyContents + "</textarea>" +
			"		<p>FROM : <input type=\"text\" id=\"CompanyFromNo\" value=\"" + DelivererMobile + "\"  /></p>" +
			"		<p>" +
			"			&nbsp;&nbsp;&nbsp;&nbsp;TO : <input type=\"text\" id=\"CompanyToNo\" value=\"" + CompanyMobile + "\"  />&nbsp;" +
			"			<input type=\"button\" id=\"CompanySMSSend\"  value=\"Send\" onclick=\"SMSSend('CompanySMSSend');\"  />" +
			"		</p>" +
			"	</fieldset>" +
			"</div>";
        	document.getElementById("SMSPrepare").innerHTML = Html;
        }
        function SMSSend(BTNID) {
        	var from;
        	var to;
        	var contents;
        	var GubunCL = "";
        	if (BTNID == "CompanySMSSend") {
        		from = document.getElementById("CompanyFromNo").value;
        		to = document.getElementById("CompanyToNo").value;
        		contents = document.getElementById("CompanyContents").value;
        		GubunCL = "2";
        	}
        	else {
        		from = document.getElementById("DelivererFromNo").value;
        		to = document.getElementById("DelivererToNo").value;
        		contents = document.getElementById("DelivererContents").value;
        		GubunCL = "3";
        	}

        	if (from == "" || to == "") {
        		alert("SMS를 보낼수 없습니다. 번호를 확인해주세요");
        		return false;
        	} else if (contents == "") {
        		alert("SMS를 보낼수 없습니다. 내용이 없습니다");
        		return false;
        	}

        	Admin.SendSMS(from, to, contents, function (result) {
        		if (result == "1") {
        			if (form1.HTransportBetweenCompanyPk.value == "N" || form1.HTransportBetweenCompanyPk.value == "") {
        				alert("Success");
        			} else {
        				Admin.AddTransportBCHistory(form1.HTransportBetweenCompanyPk.value, "3", form1.HAccountID.value, '', function (result) {
        					if (result == "1") {
        						alert("Success");
        					}
        				}, function (result) { alert("ERROR : " + result); });
        			}
        		}
        	}, function (result) { alert("ERROR : " + result); });
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="SM" runat="server">
            <Services>
                <asp:ServiceReference Path="~/WebService/Admin.asmx" />
            </Services>
        </asp:ScriptManager>
        <div style="width: 680px;" id="SMSPrepare"></div>

        <div style="width: 680px;">
            <div style="width: 300px; padding: 10px; float: right;">
                <fieldset style="padding: 10px;">
                    <legend><strong><%=GetGlobalResourceObject("qjsdur", "qock") %></strong></legend>
                    <p>
                        TYPE :
						<input type="text" id="Delivery_Type" disabled="disabled" />
                    </p>
                    <p>
                        TITLE :
						<input type="text" id="Delivery_Title" disabled="disabled" />
                    </p>
                    <p>
                        TEL :
						<input type="text" id="Delivery_TEL" disabled="disabled" />
                    </p>
                    <p>
                        Driver Name :
						<input type="text" id="Delivery_DriverName" />
                    </p>
                    <p>
                        Driver Mobile :
						<input type="text" id="Delivery_DriverMobile" />
                    </p>
                    <p>
                        Car Size :
						<input type="text" id="Delivery_CarSize" />
                    </p>
                    <p>
                        Price :
						<input type="text" id="Delivery_Price" />
                    </p>
                </fieldset>
                <input type="button" value="SUBMIT" id="BTNSubmit" onclick="BTN_Submit_Click();" />
                <%--<input type="button" value="PRINT" onclick="DeliveryPrint();" />--%>
                <%--<input type="button" value="SEND MSG" onclick="SENDSMS();" />--%>
                <%--<input type="button" value="Show SMS" onclick="SMSShow();" />--%>
                <input type="button" value="SMS AutoSend" onclick="SMSAutoSend();" />
            </div>
            <div style="width: 300px; padding: 10px;">
                <fieldset style="padding: 10px;">
                    <legend><strong><%=GetGlobalResourceObject("qjsdur", "cnfrhwl") %></strong><span id="SPStorageName"></span></legend>
                    <p>
                        BOX COUNT :
						<input type="text" style="width: 30px; text-align: right;" id="LeftBoxCount" onclick="this.select();" disabled="disabled" />
                        / <span id="TotalBoxCount"></span><span id="PackingUnit"></span>
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
					<span id="WeightNVolume"></span>
                    </p>
                    <p>
                        Start Date :
						<input id="FromDate" size="10" style="text-align: center;" type="text" disabled="disabled" />
                    </p>
                    <p>
                        Arrival Date :
						<input id="ArrivalDate" size="10" style="text-align: center;" type="text" disabled="disabled" />&nbsp;&nbsp;
					<input id="ArrivalHour" maxlength='2' type="text" style="width: 18px; text-align: center;" disabled="disabled" />
                        :
						<input id="ArrivalMin" maxlength='2' type="text" style="width: 18px; text-align: center;" disabled="disabled" />
                    </p>
                    <p>
                        <%=GetGlobalResourceObject("qjsdur", "ekaekdwkaud") %> :
						<input type="text" id="StaffName" disabled="disabled" />
                    </p>
                    <p>
                        Mobile :
						<input type="text" id="StaffMobile" disabled="disabled" />
                    </p>
                    <p>
                        Address :
						<textarea rows="3" cols="33" id="ToAddress" style="overflow: auto;" disabled="disabled"></textarea>
                    </p>
                    <p>
                        TEL :
						<input type="text" id="ToTEL" disabled="disabled" />
                    </p>
                    <p>
                        MEMO :
                        <textarea rows="2" cols="33" id="Memo" style="overflow: auto;" disabled="disabled"></textarea>
                        <%--<input type="text" id="Memo" />--%>
                    </p>
                    <%--<p>MemberMemo : <input type="text" id="MemberMemo" /></p>--%>
                    <p>

                        <input type="hidden" name="DepositWhere" id="DepositWhereBefore" value="0" />
                        <input type="hidden" name="DepositWhere" id="DepositWhereAfter" value="1" checked="checked" />
                        <input type="text" id="Text_DepositWhere" style="width: 50px;" disabled="disabled" />
                        <input type="text" id="MonetaryUnitCL" disabled="disabled" style="border: 0px; width: 10px;" /><input type="text" id="Price" style="width: 100px;" disabled="disabled" />
                    </p>
                </fieldset>
            </div>
        </div>
        <div style="width: 680px;">
            <fieldset style="padding: 10px;">
                <legend><strong>Attention</strong></legend>
                <span id="RegisterdInfo"></span>
                <br />
                <textarea id="TBContents" rows="5" cols="90" disabled="disabled"></textarea>
            </fieldset>
        </div>
        <div style="width: 680px;">
            <fieldset style="padding: 10px;">
                <legend><strong>History</strong></legend>
                <span id="SPTransportBCHistory"></span>
            </fieldset>
        </div>
        <input type="hidden" id="HTransportBBCLPk" />
        <input type="hidden" id="HTransportBetweenCompanyPk" />
        <input type="hidden" id="HOurBranchStorageOutPk" value="<%=OurBranchStorageOutPk %>" />
        <input type="hidden" id="HRequestFormPk" value="<%=RequestFormPk %>" />
        <input type="hidden" id="HConsigneePk" value="<%=CONSIGNEEPK %>" />
        <input type="hidden" id="HOurBranchPk" value="<%=OURBRANCHPK %>" />
        <input type="hidden" id="HAccountID" value="<%=ACCOUNTID %>" />
        <input type="hidden" id="HPackingUnit" />
        <input type="hidden" id="HStorageCode" />
        <input type="hidden" id="HWeight" />
        <input type="hidden" id="HVolume" />
        <input type="hidden" id="HTransportBetweenBranchPk" />
        <input type="hidden" id="DEBUG" onclick="this.select();" />
    </form>
</body>
</html>
