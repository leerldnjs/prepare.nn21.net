using Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transport_Dialog_TransportDelivery :System.Web.UI.Page
{
	protected string RequestPk;
	protected string ConsigneePk;
	protected string TransportPk;
	protected string StorageORBodyPk;

	protected string DeliveryHeadPk = "";
	protected string DeliveryBodyPk = "";
	protected string PurchaseHeadPk = "";
	protected string PurchaseBodyPk = "";
	protected string DepatureBranchPk = "";
	protected string DepatureWarehousePk = "";
	protected string ArrivalWarehousePk = "";

	protected string Html_CompanyWarehouse;
	protected string Html_DeliveryWarehouse;
	protected string Html_DeliveryCar;

	protected void Page_Load(object sender, EventArgs e) {
		RequestPk = Request.Params["R"] + "";
		ConsigneePk = Request.Params["C"] + "";
		TransportPk = Request.Params["T"] + "";
		StorageORBodyPk = Request.Params["SB"] + "";

		Html_CompanyWarehouse = MakeHtml_CompanyWarehouse();
		MakeHtml_DeliveryInfo();
	}

	protected string MakeHtml_CompanyWarehouse() {
		TransportC TransC = new TransportC();
		List<sCompanyWarehouse> Warehouse = new List<sCompanyWarehouse>();
		StringBuilder ReturnValue = new StringBuilder();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		Warehouse = TransC.LoadList_CompanyWarehouse(ConsigneePk, ref DB);
		ReturnValue.Append("<table class=\"table text-xs\" id=\"WarehouseTable\">");
		for (int i = 0; i < Warehouse.Count; i++) {
			ReturnValue.Append("<tr><td style=\"display:none;\">" + Warehouse[i].WarehousePk + "</td>");
			ReturnValue.Append("<td>" + Warehouse[i].Title + "</td>");
			ReturnValue.Append("<td>" + Warehouse[i].Address + "</td>");
			ReturnValue.Append("<td>" + Warehouse[i].Tel + "</td>");
			ReturnValue.Append("<td>" + Warehouse[i].Staff + "</td>");
			ReturnValue.Append("<td>" + Warehouse[i].Mobile + "</td>");
			ReturnValue.Append("<td><input type=\"button\" class=\"btn btn-default btn-xs\" value=\"선택\" onclick=\"SelectWarehouse('" + i + "')\" /></td></tr>");
		}
		ReturnValue.Append("</table>");

		DB.DBCon.Close();
		return ReturnValue.ToString();
	}

	protected string MakeHtml_DeliveryInfo() {
		TransportC TC = new TransportC();
		ChargeC CC = new ChargeC();
		sTransportHead Head = new sTransportHead();
		sTransportBody Body = new sTransportBody();
		sStorageItem Storage = new sStorageItem();
		sRequestFormCalculateInfo RCalculate = new sRequestFormCalculateInfo();
		sRequestFormCalculateInfo TCalculate = new sRequestFormCalculateInfo();

		StringBuilder Warehouse = new StringBuilder();
		StringBuilder Car = new StringBuilder();
		string CurrentBoxCount = "";
		string DeliveryPrice = "";
		string PurchasePrice = "";
		string PurchaseBranchPk = "";
		string PurchaseCustomerPk = "";

		DBConn DB = new DBConn();
		DB.DBCon.Open();

		RCalculate = CC.Load_Calculated("RequestForm", RequestPk, "", ref DB);
		Storage = TC.Load_StorageItem(StorageORBodyPk, ref DB);
		CurrentBoxCount = Storage.Packed_Count;
		DepatureBranchPk = Storage.Warehouse_Branch_Pk;
		DepatureWarehousePk = Storage.Warehouse_Pk;

		if (TransportPk != "") {

			Head = TC.Load_TransportHeadDelivery(TransportPk, ref DB);
			TCalculate = CC.Load_Calculated("TRANSPORT_HEAD", TransportPk, "", ref DB);
			if (Head.Transport_Status == "6") {
				Body = TC.Load_TransportBody(StorageORBodyPk, ref DB);
				CurrentBoxCount = Body.Packed_Count;
				DepatureBranchPk = Body.Warehouse_Branch_Pk;
				DepatureWarehousePk = Body.Warehouse_Pk_Departure;
			}

			// 매입비용 구하기
			for (int i = 0; i < TCalculate.arrHead.Count; i++) {
				for (int j = 0; j < TCalculate.arrHead[i].arrBody.Count; j++) {
					if (TCalculate.arrHead[i].arrBody[j].Price_Code == "PurchaseDelivery") {
						PurchaseHeadPk = TCalculate.arrHead[i].arrBody[j].RequestFormCalculate_Head_Pk;
						PurchaseBodyPk = TCalculate.arrHead[i].arrBody[j].RequestFormCalculate_Body_Pk;
						PurchasePrice = TCalculate.arrHead[i].arrBody[j].Original_Price;
						PurchaseBranchPk = TCalculate.arrHead[i].Branch_Company_Pk;
						PurchaseCustomerPk = TCalculate.arrHead[i].Customer_Company_Pk;
					}
				}
			}

		}

		//Load DeliveryWarehouse Info
		ArrivalWarehousePk = Head.CompanyWarehouse_Pk;
		Warehouse.Append("<div class=\"row\">");
		Warehouse.Append("<label class=\"col-xs-4 control-label\">Box Count</label>");
		Warehouse.Append("<div class=\"col-xs-1\"><input type=\"text\" class=\"form-control\" id=\"BoxCount\" value=\"" + CurrentBoxCount + "\" /></div>");
		Warehouse.Append("<label class=\"col-xs-6 control-label\" style=\"text-align:left;\">" + "/ " + RCalculate.TotalPackedCount + " " + Common.GetPackingUnit(RCalculate.PackingUnit));
		Warehouse.Append(" / " + Common.NumberFormat(RCalculate.TotalGrossWeight) + "Kg / " + Common.NumberFormat(RCalculate.TotalVolume) + "CBM</label>");
		Warehouse.Append("</div>");
		Warehouse.Append("<div class=\"row\">");
		Warehouse.Append("<label class=\"col-xs-4 control-label\">Depature Date</label>");
		Warehouse.Append("<div class=\"col-xs-5\"><input type=\"text\" class=\"form-control\" id=\"DepatureDate\" value=\"" + Head.DateTime_From + "\" readonly=\"readonly\" /></div>");
		Warehouse.Append("</div>");
		Warehouse.Append("<div class=\"row\">");
		Warehouse.Append("<label class=\"col-xs-4 control-label\">Arrival Date</label>");
		Warehouse.Append("<div class=\"col-xs-5\"><input type=\"text\" class=\"form-control\" id=\"ArrivalDate\" value=\"" + Head.DateTime_To + "\" readonly=\"readonly\" /></div>");
		Warehouse.Append("</div>");
		Warehouse.Append("<div class=\"row\">");
		Warehouse.Append("<label class=\"col-xs-4 control-label\">Staff</label>");
		Warehouse.Append("<div class=\"col-xs-5\"><input type=\"text\" class=\"form-control\" id=\"Staff\" value=\"" + Head.CompanyWarehouse_Staff + "\" /></div>");
		Warehouse.Append("</div>");
		Warehouse.Append("<div class=\"row\">");
		Warehouse.Append("<label class=\"col-xs-4 control-label\">Mobile</label>");
		Warehouse.Append("<div class=\"col-xs-5\"><input type=\"text\" class=\"form-control\" id=\"WarehouseMobile\" value=\"" + Head.CompanyWarehouse_Mobile + "\" /></div>");
		Warehouse.Append("</div>");
		Warehouse.Append("<div class=\"row\">");
		Warehouse.Append("<label class=\"col-xs-4 control-label\">Address</label>");
		Warehouse.Append("<div class=\"col-xs-7\"><textarea class=\"form-control\" row=\"4\" id=\"Address\" value=\"" + Head.CompanyWarehouse_Address + "\"></textarea></div>");
		Warehouse.Append("</div>");
		Warehouse.Append("<div class=\"row\">");
		Warehouse.Append("<label class=\"col-xs-4 control-label\">Tel</label>");
		Warehouse.Append("<div class=\"col-xs-5\"><input type=\"text\" class=\"form-control\" id=\"WarehouseTel\" value=\"" + Head.CompanyWarehouse_Tel + "\" /></div>");
		Warehouse.Append("</div>");
		Warehouse.Append("<div class=\"row\">");

		string[] Selected = { "", "" };
		if (PurchaseBranchPk == PurchaseCustomerPk) {
			Selected[0] = "checked=\"checked\"";
			Selected[1] = "";
		}
		else {
			Selected[0] = "";
			Selected[1] = "checked=\"checked\"";
		}

		Warehouse.Append("<div class=\"col-xs-2\"><input type=\"radio\" name=\"DepositedWhere\" value=\"Depature\" " + Selected[0] + " /> 현불</div>");
		Warehouse.Append("<div class=\"col-xs-2\"><input type=\"radio\" name=\"DepositedWhere\" value=\"Arrival\" " + Selected[1] + " /> 착불</div>");
		Warehouse.Append("<div class=\"col-xs-5\"><input type=\"text\" class=\"form-control\" id=\"DeliveryPrice\" value=\"" + Head.CompanyWarehouse_Tel + "\" /></div>");
		Warehouse.Append("</div>");

		Html_DeliveryWarehouse = Warehouse.ToString();


		//Load DeliveryCar Info
		Car.Append("<div class=\"row\">");
		Car.Append("<label class=\"col-xs-4 control-label\">Type</label>");
		Car.Append("<div class=\"col-xs-5\"><input type=\"text\" class=\"form-control\" id=\"CarType\" value=\"" + Head.Voyage_No + "\" /></div>");
		Car.Append("</div>");
		Car.Append("<div class=\"row\">");
		Car.Append("<label class=\"col-xs-4 control-label\">Title</label>");
		Car.Append("<div class=\"col-xs-5\"><input type=\"text\" class=\"form-control\" id=\"CarTitle\" value=\"" + Head.Title + "\" /></div>");
		Car.Append("</div>");
		Car.Append("<div class=\"row\">");
		Car.Append("<label class=\"col-xs-4 control-label\">Tel</label>");
		Car.Append("<div class=\"col-xs-5\"><input type=\"text\" class=\"form-control\" id=\"CarTel\" value=\"" + Head.Value_String_2 + "\" /></div>");
		Car.Append("</div>");
		Car.Append("<div class=\"row\">");
		Car.Append("<label class=\"col-xs-4 control-label\">DriverName</label>");
		Car.Append("<div class=\"col-xs-5\"><input type=\"text\" class=\"form-control\" id=\"DriverName\" value=\"" + Head.VesselName + "\" /></div>");
		Car.Append("</div>");
		Car.Append("<div class=\"row\">");
		Car.Append("<label class=\"col-xs-4 control-label\">DriverMobile</label>");
		Car.Append("<div class=\"col-xs-5\"><input type=\"text\" class=\"form-control\" id=\"DriverMobile\" value=\"" + Head.Value_String_1 + "\" /></div>");
		Car.Append("</div>");
		Car.Append("<div class=\"row\">");
		Car.Append("<label class=\"col-xs-4 control-label\">CarSize</label>");
		Car.Append("<div class=\"col-xs-5\"><input type=\"text\" class=\"form-control\" id=\"CarSize\" value=\"" + Head.Value_String_3 + "\" /></div>");
		Car.Append("</div>");
		Car.Append("<div class=\"row\">");
		Car.Append("<label class=\"col-xs-4 control-label\">Price</label>");
		Car.Append("<div class=\"col-xs-5\"><input type=\"text\" class=\"form-control\" id=\"PurchasePrice\" value=\"" + PurchasePrice + "\" /></div>");
		Car.Append("</div>");

		Html_DeliveryCar = Car.ToString();


		DB.DBCon.Close();
		return "1";	
	}

}