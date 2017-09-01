<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Clean.Master" AutoEventWireup="true" CodeFile="TransportPacked.aspx.cs" Inherits="Transport_Dialog_TransportPacked" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Contents" Runat="Server">
    <input type="hidden" id="AccountPk" value="<%=Request.Params["AccountPk"] %>" />
    <input type="hidden" id="CompanyPk" value="<%=Request.Params["CompanyPk"] %>" />
    <input type="hidden" id="Transport_Packed_Pk" value="<%=TPacked.Transport_Packed_Pk %>" />

    <div class="col-xs-12 small">
        <div class="panel panel-default">
            <div class="panel-heading" style="font-weight: bold;"><%=GetGlobalResourceObject("qjsdur", "zjsxpdlsj") %></div>
            <div class="panel-body form-horizontal">
                <div class="form-group">
                    <div class="col-xs-10">
                        <label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "zjsxpdlsjqjsgh") %></label>
                        <div class="col-xs-3">
                            <input name="ContainerNo" class="form-control" id="ContainerNo" type="text" value="<%=TPacked.No %>"/>
                        </div>
                        <!--<<label class="col-xs-3 control-label" style="text-align: right;">컨테이너회사</label>
                        div class="col-xs-3">
                            <input name="ContainerCompany" class="form-control" id="ContainerCompany" type="text" />
                        </div>-->
                        <label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "zjsxpdlsjrbrur") %></label>
                        <div class="col-xs-3">
							<select name="ContainerSize" class="form-control" id="ContainerSize">
								<option value="20GP">20GP</option>
								<option value="40GP">40GP</option>
								<option value="40HC">40HC</option>
								<option value="45GP">45GP</option>
							</select>
                        </div>
                        <label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "zjsxpdlsj") %> Type</label>
                        <div class="col-xs-3">
							<select name="ContainerType" class="form-control" id="ContainerType">
								<option value="DRY">DRY</option>
								<option value="REF">REF</option>
							</select>
                        </div>
                        <label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "wltk") %></label>
                        <div class="col-xs-3">
                            <select class="form-control" id="ContainerBranch_Own"><%=Components.Setting.SelectOurBranch %> </select>
                        </div>
                        <label class="col-xs-3 control-label" style="text-align: right;">SealNo</label>
                        <div class="col-xs-3">
                            <input name="SealNo" class="form-control" id="SealNo" type="text" value="<%=TPacked.Seal_No %>"/>
                        </div>
						<label class="col-xs-3 control-label" style="text-align: right;"><%=GetGlobalResourceObject("qjsdur", "ckdrh") %></label>
                        <div class="col-xs-3" id="st_WareHouse">

                        </div>
                    </div>
                </div>

                <hr />
                <div class="form-group">
                    <div class="col-xs-4 col-xs-offset-4">
                        <button class="btn btn-primary btn-md btn-block" id="BTN_Submit" type="button">저장</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" Runat="Server">
    <script src="/Lib/ForDialog.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            ForDialog.initPopup();  // PagePopUp 사용시 
//            ForDialog.Init("md", "Modal_Default"); // dialog 사용시

			var data = {
				"CompanyPk": "<%=pBranchPk %>"
			}
			$.ajax({
				type: "POST",
				url: "/WebService/TransportP.asmx/LoadBranchStorage",
				data: JSON.stringify(data),
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (result) {
					$("#st_WareHouse").html(result.d);
				},
				error: function (result) {
					alert('failure : ' + result);
				}
			});


			$("#BTN_Submit").on("click", SetSave);

			if ("<%=TPacked.Transport_Packed_Pk %>" != "") {
				$("#ContainerBranch_Own").val("<%=TPacked.Company_Pk_Owner %>");
				$("#ContainerSize").val("<%=TPacked.Size %>"); 
				$("#ContainerType").val("<%=TPacked.Type %>");
			}
        });

		function SetSave() {
            var data = {
				"Transport_Packed_Pk": $("#Transport_Packed_Pk").val(),
				"Seq": "<%=TPacked.Seq %>",
				"WareHouse_Pk": $("#St_Storage").val(),
                "Transport_Head_Pk": "<%=TPacked.Transport_Head_Pk %>",
                "ContainerBranch_Own": $("#ContainerBranch_Own").val(),
                "ContainerType": $("#ContainerType").val(),
                "ContainerNo": $("#ContainerNo").val(),
                "ContainerSize": $("#ContainerSize").val(),
				"SealNo": $("#SealNo").val(),
				"RealPacked_Flag": "Y"
            }
            $.ajax({
                type: "POST",
                url: "/WebService/TransportP.asmx/Set_TransportPacked",
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    alert(result.d);
                },
                error: function (result) {
                    alert('failure : ' + result);
                }
			});
			self.close();
			opener.location.reload();
        }

        /*
        function GetCompany() {
		var popup;
            //////////////////////////////// dialog 부////////////////////////
            $("#Pn" + "Modal_Default" + "_Body").html('																										\
				<form id="FormCategory" >																																asdfasdfsdaf</form>');

            $("#Pn" + "Modal_Default" + "_Footer").html('																									\
				<button type="button" class="btn green" onclick="SetSave_Category();">Save changes</button>		\
				<button type="button" class="btn dark btn-outline" data-dismiss="modal">Close</button>');
            $('#Modal_Default').modal('show');
            //////////////////////////////// dialog 부////////////////////////

            //////////////////////////////// PagePopUp 부////////////////////////
            var PopWindow = "pop_win" + "asdf";
            var Win = window.open('', PopWindow, 'width=780, height=464,menubar=no,status=no,scrollbars=yes');
            $("#ForDialog_CompanyPk").val("3157");
            $("#ForDialog_AccountPk").val("124585");

            $("#ForDialog").attr('target', PopWindow);
            $("#ForDialog").attr('action', '/Transport/Dialog/Container.aspx');
            $("#ForDialog").submit();
            //////////////////////////////// PagePopUp 부////////////////////////
        }
        */        
    </script>
</asp:Content>

