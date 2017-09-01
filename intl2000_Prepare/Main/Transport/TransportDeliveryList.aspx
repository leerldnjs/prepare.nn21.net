<%@ Page Title="" Language="C#" MasterPageFile="~/ViewShare/Admin_Scale.master" AutoEventWireup="true" CodeFile="TransportDeliveryList.aspx.cs" Inherits="Transport_TransportDeliveryList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contents" Runat="Server">

	<div class="panel panel-warning col-xs-10 col-xs-offset-1">
		<div class="panel-body form-horizontal">

			<%=Html_ResponsibleStaff %>

			<%=Html_DeliveryList %>
		</div>
	</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Javascript" Runat="Server">
	<script type="text/javascript">

		$(document).ready(function () {
			
		});

		function RetrieveStaff(ID) {
			location.href("/Transport/TransportDeliveryList.aspx?W=" + ID);
		}



	</script>
</asp:Content>

