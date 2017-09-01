<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Clean.Master" AutoEventWireup="true" CodeFile="TransportHead.aspx.cs" Inherits="Transport_Dialog_TransportHead" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" runat="Server">
    <input type="hidden" id="AccountPk" value="<%=Request.Params["AccountPk"] %>" />
    <div class="col-xs-12 small">
        <div class="panel panel-default">
            <div class="panel-heading" style="font-weight: bold;" ><%=GetGlobalResourceObject("qjsdur", "dnsthdqkdqjq") %></div>
            <div class="panel-body form-horizontal">
                <div class="form-group">
                    <label class="col-xs-5 control-label" style="text-align: right;">운송방법 선택</label>
                    <div class="col-xs-3">
						<select id="st_TransportWay" class="form-control">
							<option value=''>선택하세요</option>
							<option value='Air'><%=GetGlobalResourceObject("qjsdur", "gkdrhd") %></option>
							<option value='Car'><%=GetGlobalResourceObject("qjsdur", "ckfid") %></option>
							<option value='Ship'><%=GetGlobalResourceObject("qjsdur", "tjsqkr") %></option>
							<option value='Sub'>SubCompany</option>
						</select>
					</div>
                </div>
                <input name="SavedPk_VesselName" class="form-control" id="SavedPk_VesselName" type="hidden" value="" />
                <input name="SavedPk_Area_From" class="form-control" id="SavedPk_Area_From" type="hidden" value="" />
                <input name="SavedPk_Area_To" class="form-control" id="SavedPk_Area_To" type="hidden" value="" />
                <div class="form-group">
                    <div class="col-xs-11" id="Pn_Air" style="display: none;">
                        <div class="row">
                            <label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "gkdrhdaud") %></label>
                            <div class="col-xs-3">
                                <input name="Air_Company" class="form-control" id="Air_Company" type="text" value="<%=Head.Title %>" readonly="readonly"  />
                            </div>
                            <label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "gkdck") %></label>
                            <div class="col-xs-3">
                                <input name="Air_FlightNo" class="form-control" id="Air_FlightNo" type="text" value="<%=Head.Voyage_No %>" />
                            </div>
                        </div>
						<div class="row">
							<label class="col-xs-3 control-label" style="text-align: right;">MASTER B/L</label>
                            <div class="col-xs-3">
                                <input name="Air_BlNo" class="form-control" id="Air_BlNo" type="text" value="<%=Head.Value_String_0 %>" />
                            </div>
						</div>
                    </div>

                    <div class="col-xs-11" id="Pn_Car" style="display: none;">
                        <div class="row">
							<label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "ckfidghltkaud") %></label>
                            <div class="col-xs-3">
                                <input name="Car_Company" class="form-control" id="Car_Company" type="text" value="<%=Head.Title %>" readonly="readonly" />
                            </div>
                            <label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "ckfid") %></label>
                            <div class="col-xs-3">
                                <input name="Car_Driver" class="form-control" id="Car_Driver" type="text" value="<%=Head.VesselName %>" readonly="readonly" />
                            </div>
                        </div>
                        <div class="row">
							<label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "ckfiddusfkrcj") %></label>
                            <div class="col-xs-3">
                                <input name="Car_DriverTel" class="form-control" id="Car_DriverTel" type="text" value="<%=Head.Value_String_1 %>" readonly="readonly" />
                            </div>
                            <label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "ckfidqjsgh") %></label>
                            <div class="col-xs-3">
                                <input name="Car_No" class="form-control" id="Car_No" type="text" value="<%=Head.Voyage_No %>" readonly="readonly" />
                            </div>
                        </div>
						<div class="row">
							<label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "ckfidrbrur") %></label>
                            <div class="col-xs-3">
                                <input name="Car_Size" class="form-control" id="Car_Size" type="text" value="<%=Head.Value_String_3 %>" readonly="readonly" />
                            </div>
							<label class="col-xs-3 control-label" style="text-align: right;">MASTER B/L</label>
                            <div class="col-xs-3">
                                <input name="Car_BlNo" class="form-control" id="Car_BlNo" type="text" value="<%=Head.Value_String_0 %>" />
                            </div>
						</div>
                    </div>

                    <div class="col-xs-11" id="Pn_Ship" style="display: none;">
                        <div class="row">
							<label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "tjsqkrghltkaud") %></label>
                            <div class="col-xs-3">
                                <input name="Ship_Company" class="form-control" id="Ship_Company" type="text" value="<%=Head.Title %>" readonly="readonly" />
                            </div>
                            <label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "tjsqkraud") %></label>
                            <div class="col-xs-3">
                                <input name="Ship_VesselName" class="form-control" id="Ship_VesselName" type="text" value="<%=Head.VesselName %>" readonly="readonly" />
                            </div>
                        </div>
						<div class="row">
							 <label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "gkdck") %></label>
                            <div class="col-xs-3">
                                <input name="Ship_Shipping" class="form-control" id="Ship_Shipping" type="text" value="<%=Head.Voyage_No %>" />
                            </div>
							<label class="col-xs-3 control-label" style="text-align: right;">MASTER B/L</label>
                            <div class="col-xs-3">
                                <input name="Ship_BlNo" class="form-control" id="Ship_BlNo" type="text" value="<%=Head.Value_String_0 %>" />
                            </div>
						</div>
                    </div>

                    <div class="col-xs-11" id="Pn_Sub" style="display: none;">
                        <div class="row">
                            <label class="col-xs-3 control-label" style="text-align: right;">Company</label>
                            <div class="col-xs-3">
                                <input name="Sub_Company" class="form-control" id="Sub_Company" type="text" value="<%=Head.Title %>" readonly="readonly" />
                            </div>
                            <label class="col-xs-3 control-label" style="text-align: right;">From TEL</label>
                            <div class="col-xs-3">
                                <input name="Sub_FromTel" class="form-control" id="Sub_FromTel" type="text" value="<%=Head.Value_String_1 %>" readonly="readonly" />
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-xs-3 control-label" style="text-align: right;">To TEL</label>
                            <div class="col-xs-3">
                                <input name="Sub_ToTel" class="form-control" id="Sub_ToTel" type="text" value="<%=Head.Value_String_2 %>" readonly="readonly" />
                            </div>
							<label class="col-xs-3 control-label" style="text-align: right;">MASTER B/L</label>
                            <div class="col-xs-3">
                                <input name="Sub_BlNo" class="form-control" id="Sub_BlNo" type="text" value="<%=Head.Value_String_0 %>" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-xs-11" id="Pn_ComVar" style="display:none;">
                        <div class="row">
                            <label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "cnfqkfwl") %></label>
                            <div class="col-xs-3">
                                <select id="From_BranchPk" class="form-control" name="From_BranchPk" >
                                    <%=Components.Setting.SelectOurBranch %>
                                </select>
                            </div>
                            <label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "ehckrwltk") %></label>
                            <div class="col-xs-3">
                                <select id="To_BranchPk" class="form-control" name="To_BranchPk">
                                    <%=Components.Setting.SelectOurBranch %>
                                </select>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "cnfqkfwl") %></label>
                            <div class="col-xs-3">
                                <input name="From_Reg" class="form-control" id="From_Reg" type="text" value="<%=Head.Area_From %>" readonly="readonly" />
                            </div>
                            <label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "ehckrwl") %></label>
                            <div class="col-xs-3">
                                <input name="To_Reg" class="form-control" id="To_Reg" type="text" value="<%=Head.Area_To %>" readonly="readonly" />
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-xs-2 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") %></label>
                            <div class="col-xs-2">
                                <input name="From_Date" class="form-control" id="From_Date" type="text" value="<%=pDateTime_From[0] %>" readonly="readonly" placeholder="년월일" />
                            </div>
                            <div class="col-xs-1">
                                <input name="From_Hour" class="form-control" id="From_Hour" type="text" value="<%=pDateTime_From[1] %>" maxlength="2" placeholder="시" />
                            </div>
                            <div class="col-xs-1">
                                <input name="From_Min" class="form-control" id="From_Min" type="text" value="<%=pDateTime_From[2] %>" maxlength="2" placeholder="분" />
                            </div>
                            <label class="col-xs-2 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") %></label>
                            <div class="col-xs-2">
                                <input name="To_Date" class="form-control" id="To_Date" type="text" value="<%=pDateTime_To[0] %>" readonly="readonly" placeholder="년월일" />
                            </div>
                            <div class="col-xs-1">
                                <input name="To_Hour" class="form-control" id="To_Hour" type="text" value="<%=pDateTime_To[1] %>" maxlength="2" placeholder="시" />
                            </div>
                            <div class="col-xs-1">
                                <input name="To_Min" class="form-control" id="To_Min" type="text" value="<%=pDateTime_To[2] %>" maxlength="2"  placeholder="분" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group" id="TransportHistory">
                </div>

                <hr/>
                <div class="col-xs-3 col-xs-offset-3">
                    <button class="btn btn-primary btn-md btn-block" id="BTN_Submit" type="button">Submit</button>
                </div>
                <div class="col-xs-3 ">
                    <button class="btn btn-warning btn-md btn-block" type="button" onclick="self.close()">Cancel</button>
                </div>
            </div>
        </div>
    </div>

	<input type="hidden" id="HeadPk" value="<%=Head.Transport_Head_Pk %>" />

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" Runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.1/themes/base/jquery-ui.css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>
    <link rel="stylesheet" href="/resources/demos/style.css"/>
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="/Lib/ForDialog.js"></script>

    <script type="text/javascript">
        var Category = "";
		var dialog = ""
		var Status = "0";
		$(document).ready(function () {

			if ("<%=Head.Transport_Head_Pk %>" != ""){
				$("#st_TransportWay").val("<%=Head.Transport_Way %>");
				$("#st_TransportWay").attr("disabled", true);
				$("#Pn_" + $("#st_TransportWay").val()).show();
				$("#Pn_ComVar").show()
				$("#From_BranchPk").val(<%=Head.BranchPk_From %>);
				$("#To_BranchPk").val(<%=Head.BranchPk_To %>);
				Category = "<%=Head.Transport_Way %>";
				Status = "<%=Head.Transport_Status %>";
			}

			$("#st_TransportWay").on("change", function () {
                $("#Pn_Air").hide();
                $("#Pn_Car").hide();
                $("#Pn_Ship").hide();
                $("#Pn_Sub").hide();
                $("#Pn_ComVar").hide();

                if ($("#st_TransportWay").val() != "") {
                    $("#Pn_" + $("#st_TransportWay").val()).show();
                    $("#Pn_ComVar").show()
                }
                Category = $("#st_TransportWay").val();
            });

            $("#From_Date").datepicker({
                dateFormat: "yymmdd"
            });
            $("#To_Date").datepicker({
                dateFormat: "yymmdd"
            });

            $("#BTN_Submit").on("click", SetSave);         

            $("#Air_Company").on("click", function () { MakeHtml("Air"); });
            $("#Car_Company").on("click", function () { MakeHtml("Car"); });
            $("#Ship_Company").on("click", function () { MakeHtml("Ship"); });
            $("#Sub_Company").on("click", function () { MakeHtml("Sub"); });
            $("#From_Reg").on("click", function () { MakeHtml("FromReg"); });
            $("#To_Reg").on("click", function () { MakeHtml("ToReg"); });

            dialog = $("#TransportHistory").dialog({
                autoOpen: false,
                title: "운송정보 입력",
                modal: true,
                width: "500",
                height: "400",
                border: "1px",
            });
            
        });
        

		function SetSave() {
			if ($("#To_Date").val() == "" || $("#To_Date").val() == null) {
				alert("날짜를 입력하십시오.");
				return false;
			}
			var data = "";
			var Title = "";
            var VesselName = "";
            var Voyage_No = "";
            var Add_Value0 = "";
            var Add_Value1 = "";
            var Add_Value2 = "";
            var Add_Value3 = "";
            var Add_Value4 = "";
            var Add_Value5 = "";
			var FromDate = $("#From_Date").val() + " " + $("#From_Hour").val() + ":" + $("#From_Min").val();
			var ToDate = $("#To_Date").val() + " " + $("#To_Hour").val() + ":" + $("#To_Min").val();

            switch (Category) {
				case "Air":  //항공
					Title = $("#Air_Company").val();
					Voyage_No = $("#Air_FlightNo").val();
					Add_Value0 = $("#Air_BlNo").val();
                    break;
				case "Car":  //차량
					Title = $("#Car_Company").val();
                    VesselName = $("#Car_Driver").val();
					Voyage_No = $("#Car_No").val();
					Add_Value0 = $("#Car_BlNo").val();
                    Add_Value1 = $("#Car_DriverTel").val();
                    Add_Value3 = $("#Car_Size").val();
                    break;
				case "Ship":  //선박
					Title = $("#Ship_Company").val();
                    VesselName = $("#Ship_VesselName").val();
					Voyage_No = $("#Ship_Shipping").val();
					Add_Value0 = $("#Ship_BlNo").val();
                    break;
				case "Sub":  //외주 
					Title = $("#Sub_Company").val();
                    Add_Value1 = $("#Sub_FromTel").val();
					Add_Value2 = $("#Sub_ToTel").val();
					Add_Value0 = $("#Sub_BlNo").val();
                    break;
            }
            var data = {
				"Transport_Head_Pk": $("#HeadPk").val(),
                "Transport_Way": Category,
				"Transport_Status": Status,
                "BranchPk_From": $("#From_BranchPk").val(),
                "BranchPk_To": $("#To_BranchPk").val(),
                "Area_From": $("#From_Reg").val(),
                "Area_To": $("#To_Reg").val(),
                "DateTime_From": FromDate,
				"DateTime_To": ToDate,
				"Title": Title,
                "VesselName": VesselName,
                "Voyage_No": Voyage_No,
                "Value_String_0": Add_Value0,
                "Value_String_1": Add_Value1,
                "Value_String_2": Add_Value2,
                "Value_String_3": Add_Value3,
                "Value_String_4": Add_Value4,
                "Value_String_5": Add_Value5,
                "SavedPk_VesselName": $("#SavedPk_VesselName").val(),
                "SavedPk_Area_From": $("#SavedPk_Area_From").val(),
                "SavedPk_Area_To": $("#SavedPk_Area_To").val()
			}
             $.ajax({
                 type: "POST",
				 url: "/WebService/TransportP.asmx/Set_TransportHead",
                 data: JSON.stringify(data),
                 dataType: "json",
                 contentType: "application/json; charset=utf-8",
                 success: function (result) {
					 self.close();
					 opener.location.reload();
                 },
                 error: function (result) {
                     alert('failure : ' + result);
                 }
             });
        }

        function MakeHtml(Flag) {
            var RetVal = "";
            var data = {
                "BranchPk": $("#From_BranchPk").val(),
                "Category": Flag
            }
            $.ajax({
                type: "POST",
                url: "/WebService/TransportP.asmx/MakeHtml_TransportHistory",
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    //alert(result.d);
                    RetVal = result.d;
                    TransportHistory.innerHTML = RetVal;
                    dialog.dialog("open");
                },
                error: function (result) {
                    alert('failure : ' + result);
                }
            });
        }

		function GetHistory(HistoryReturn, HistoryFlag) {
            var HistoryValue = HistoryReturn.split(",!");
            if (HistoryReturn == "Direct") {
                HistoryValue[0] = "";
                HistoryValue[1] = $("#DirectSet0").val();
                HistoryValue[2] = $("#DirectSet1").val();
                HistoryValue[3] = $("#DirectSet2").val();
				HistoryValue[4] = $("#DirectSet3").val();
				HistoryValue[5] = $("#DirectSet4").val();
			}
            switch (HistoryFlag) {
                case "Air":
                    $("#SavedPk_VesselName").val(HistoryValue[0]);
                    $("#Air_Company").val(HistoryValue[1]);
                    break;
                case "Car":
					$("#SavedPk_VesselName").val(HistoryValue[0]);
					$("#Car_Company").val(HistoryValue[1]);
                    $("#Car_Driver").val(HistoryValue[2]);
                    $("#Car_DriverTel").val(HistoryValue[3]);
                    $("#Car_No").val(HistoryValue[4]);
                    $("#Car_Size").val(HistoryValue[5]);
                    break;
                case "Ship":
                    $("#SavedPk_VesselName").val(HistoryValue[0]);
					$("#Ship_Company").val(HistoryValue[1]);
					$("#Ship_VesselName").val(HistoryValue[2]);
                    break;
                case "Sub":
                    $("#SavedPk_VesselName").val(HistoryValue[0]);
                    $("#Sub_Company").val(HistoryValue[1]);
                    $("#Sub_FromTel").val(HistoryValue[2]);
                    $("#Sub_ToTel").val(HistoryValue[3]);
                    break;
                case "FromReg":
                    $("#SavedPk_Area_From").val(HistoryValue[0]);
                    $("#From_Reg").val(HistoryValue[1]);
                    break;
                case "ToReg":
                    $("#SavedPk_Area_To").val(HistoryValue[0]);
                    $("#To_Reg").val(HistoryValue[1]);
                    break;
            }
            dialog.dialog("close");
        }

    </script>
</asp:Content>

