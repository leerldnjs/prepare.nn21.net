<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Clean.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Finance_Dialog_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" Runat="Server">


			<div class="col-xs-12 small">
				<div class="panel panel-default" id="PnTarget">
					<div class="panel-heading" id="PnTargetHeading">File upload</div>
					<div class="panel-body form-horizontal">
						<form name="file_upload" id="file_upload" method="post" enctype="multipart/form-data" accept-charset="utf-8">
							<div>
								<div class="form-group">
									<label class="col-xs-2 control-label" style="text-align:right;">Title</label>
									<div class="col-xs-4">
										<input type="text" class="form-control" name="Title" id="Title" value="<%=Request.Params["Title"] + "" %>" />
										<input type="hidden" name="Type" id="Type" value="<%=Request.Params["Type"] %>" />
										<input type="hidden" name="Count" id="Count" value="<%=Request.Params["Count"] %>" />
									</div>
								</div>
							</div>

							<div id="PnFile">
								<div class="form-group">
									<label class="col-xs-2 control-label" style="text-align:right;">File</label>
									<div class="col-xs-7">
										<input name="AttacheFile" style="width: 100%; height: 30px;" type="file"/>
									</div>
									<div class="col-xs-1" id="PnAddFile">
										<button class="btn btn-success btn-sm" id="BTN_AddFile" type="button" >
											<i class="fa fa-plus"></i>
										</button>
									</div>
								</div>
							</div>
						</form>
					</div>
				</div>
				<div class="form-group">
					<div class="col-xs-4 col-xs-offset-2">
						<button class="btn btn-primary btn-md btn-block" id="BTN_SubmitClick" type="button" onclick="file_upload.submit();" style="height: 50px;">Save</button>
					</div>
					<div class="col-xs-4">
						<button class="btn btn-warning btn-md btn-block" id="BTN_Cancel" type="button" style="height: 50px;"></button>
					</div>
				</div>
			</div>

	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" Runat="Server">
</asp:Content>