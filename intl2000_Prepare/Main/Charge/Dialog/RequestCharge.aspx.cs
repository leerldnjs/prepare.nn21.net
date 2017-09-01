using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Charge_Dialog_RequestCharge :System.Web.UI.Page
{
	protected String RequestFormPk = "";
	protected String Already = "";
	protected String Html_ChargeHeader;
	protected String Html_ChargeBody;
	protected String TransportWayCL, DepartureRegionCode, ArrivalRegionCode, DepartureDate;
	protected String GrossWeight;
	protected String OverWeight;
	protected String Volume;
	protected String PaymentWho;
	protected String SelectOurBranch = Common.SelectOurBranch;
	protected String IsEstimation;
	protected static String Monetary = ChargeC.Get_MonetaryUnit("0");
	protected static sStandardPriceList StandardPrice;
	protected sRequestFormCalculateInfo Calculated;
	protected StringBuilder Calculated_Head = new StringBuilder();
	protected StringBuilder Calculated_Item = new StringBuilder();
	protected StringBuilder Calculated_Total = new StringBuilder();
	protected static String ShipperPk, ConsigneePk;
	protected string[] MemberInformation;

	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("/Default.aspx");
		}
		MemberInformation = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		RequestFormPk = Request.Params["S"] + "";
		Already = Request.Params["A"] + "";
		ChargeP CP = new ChargeP();
		Html_ChargeHeader = MakeHtml_ChargeHeader(RequestFormPk);
		Html_ChargeBody = CP.MakeHtml_ChargeBody("Request", PaymentWho, ShipperPk);
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT StepCL FROM RequestForm WHERE RequestFormPk=" + RequestFormPk + ";";
		IsEstimation = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();

		if (Already == "Y") {
			Load_Calculated();
		}
	}

	private string MakeHtml_ChargeHeader(string RequestFormPk) {
		StringBuilder ReturnValue = new StringBuilder();
		ChargeC ChargeC = new ChargeC();
		List<sStandardPriceList> PriceList = new List<sStandardPriceList>();
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = @"SELECT [ShipperPk], [ConsigneePk], [TransportWayCL], [DepartureDate] FROM [dbo].[RequestForm] WHERE [RequestFormPk] = " + RequestFormPk;
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ShipperPk = RS["ShipperPk"] + "";
			ConsigneePk = RS["ConsigneePk"] + "";
			TransportWayCL = RS["TransportWayCL"] + "";
			DepartureDate = RS["DepartureDate"] + "";
		}
		RS.Close();
		PriceList = ChargeC.LoadList_StandardPriceList(ShipperPk, TransportWayCL, "M", ref DB);

		DB.SqlCmd.CommandText = @"SELECT RF.ShipperCode, RF.ConsigneeCode, RF.DepartureDate, RF.ArrivalDate, DRC.RegionCodePk AS DepartureRegionCode, 
			ARC.RegionCodePk AS ArrivalRegionCode, RF.TransportWayCL, RF.JubsuWayCL, RF.PaymentWayCL, RF.PaymentWhoCL, 
			RF.DocumentRequestCL, RF.MonetaryUnitCL, RF.Memo , RF.StockedDate, 
			RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume, DRC.Name AS DRCName, ARC.Name AS ARCName, DRC.OurBranchCode ,RF.ShipperPk , RF.ConsigneePk
		FROM RequestForm AS RF 
		Left outer join RegionCode As DRC on RF.DepartureRegionCode=DRC.RegionCode 
		Left outer join RegionCode As ARC on RF.ArrivalRegionCode=ARC.RegionCode 
		WHERE RF.RequestFormPk=" + RequestFormPk + @";";

		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			PaymentWho = RS["PaymentWhoCL"] + "";
			GrossWeight = RS["TotalGrossWeight"] + "";
			Volume = RS["TotalVolume"] + "";
			Decimal tempGWeight = GrossWeight == "" ? 0 : Decimal.Parse(GrossWeight);
			Decimal tempVolume = Volume == "" ? 0 : Decimal.Parse(Volume);

			switch (RS["OurBranchCode"] + "") {
				case "3843":
					if (tempVolume * 250 < tempGWeight) {
						OverWeight = Math.Round(tempGWeight / 250, 2).ToString();
					}
					else {
						OverWeight = Volume;
					}
					break;
				case "2888":
					if (tempVolume * 400 < tempGWeight) {
						OverWeight = Math.Round(tempGWeight / 400, 2).ToString();
					}
					else {
						OverWeight = Volume;
					}
					break;
				case "11456":
					if (tempVolume * 400 < tempGWeight) {
						OverWeight = Math.Round(tempGWeight / 400, 2).ToString();
					}
					else {
						OverWeight = Volume;
					}
					break;
				case "7898":
					if (tempVolume * 400 < tempGWeight) {
						OverWeight = Math.Round(tempGWeight / 400, 2).ToString();
					}
					else {
						OverWeight = Volume;
					}
					break;
				case "2887":
					if (tempVolume * 350 < tempGWeight) {
						OverWeight = Math.Round(tempGWeight / 350, 2).ToString();
					}
					else {
						OverWeight = Volume;
					}
					break;
				case "3388":
					if (tempVolume * 350 < tempGWeight) {
						OverWeight = Math.Round(tempGWeight / 350, 2).ToString();
					}
					else {
						OverWeight = Volume;
					}
					break;
				case "2886":
					if (tempVolume * 350 < tempGWeight) {
						OverWeight = Math.Round(tempGWeight / 350, 2).ToString();
					}
					else {
						OverWeight = Volume;
					}
					break;
				case "3157":
					OverWeight = Volume;
					break;
			}

			ReturnValue.Append("<div class=\"row\"><label class=\"control-label col-xs-4 col-xs-offset-1\">" + GetGlobalResourceObject("qjsdur", "dnsthdqkdqjq") + ": ");
			ReturnValue.Append(Common.GetTransportWay(RS["TransportWayCL"] + "") + "</label></div>");
			ReturnValue.Append("<div class=\"row\"><label class=\"control-label col-xs-8 col-xs-offset-1\" >" + GetGlobalResourceObject("qjsdur", "cnfqkfwl") + ": ");
			ReturnValue.Append(RS["DepartureDate"] + "(" + RS["DRCName"] + ")" + " - " + RS["DepartureDate"] + "(" + RS["ARCName"] + ")" + "</label></div>");
			ReturnValue.Append("<div class=\"row\"><label class=\"control-label col-xs-3 col-xs-offset-1\">" + GetGlobalResourceObject("qjsdur", "chdqkrtm") + ": ");
			ReturnValue.Append(RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "") + "</label>");
			ReturnValue.Append("<label class=\"control-label col-xs-3\">" + GetGlobalResourceObject("qjsdur", "chdcpwjr") + ": ");
			ReturnValue.Append(Common.NumberFormat(RS["TotalVolume"] + "") + "CBM</label>");
			ReturnValue.Append("<label class=\"control-label col-xs-3\">" + GetGlobalResourceObject("qjsdur", "chdwndfid") + ": ");
			ReturnValue.Append(Common.NumberFormat(RS["TotalGrossWeight"] + "") + "Kg</label></div>");
			ReturnValue.Append("<div class=\"well m-t\">");
			ReturnValue.Append("<div class=\"row\"><label class=\"control-label col-xs-2\" style=\"text-align:right; margin-top:9px;\">단가표: </label>");
			ReturnValue.Append("<div class=\"col-xs-2\"><select id=\"st_StandardPrice\" class=\"form-control\"><option value=\"0\">수기입력</option>");
			for (int i = 0; i < PriceList.Count; i++) {
				ReturnValue.Append("<option value=\"" + PriceList[i].StandardPrice_Pk + "\">" + PriceList[i].Title + "</option>");
			}
			ReturnValue.Append("</select></div>");
			ReturnValue.Append("<div class=\"col-xs-2\"><input type=\"button\" class=\"btn btn-default btn-xs\" style=\"margin-top:6px;\" value=\"Load\" onclick=\"LoadStandardPriceItem()\" /></div></div>");
			ReturnValue.Append("<div class=\"row\"><label class=\"control-label col-xs-2\" style=\"text-align:right; margin-top:9px;\">단가기준: </label>");
			ReturnValue.Append("<div class=\"col-xs-2\"><input class=\"form-control\" type=\"text\" id=\"CriterionValue\" onkeyup=\"javascript: {$('#OverWeightValue').val(this.value);}\" value=\"" + Common.NumberFormat(RS["TotalVolume"] + "") + "\"/></div><div class=\"col-xs-1\" id=\"OverUnit\" style=\"margin-top:9px;\">CBM</div>");
			ReturnValue.Append("<label class=\"control-label col-xs-2\" style=\"text-align:right; margin-top:9px;\" id=\"OverValue\">과중량: </label>");
			ReturnValue.Append("<div class=\"col-xs-2\"><input class=\"form-control\" type=\"text\" id=\"OverWeightValue\" value=\"" + Math.Round(Decimal.Parse(OverWeight), 2) + "\" /></div>");
			ReturnValue.Append("<div class=\"col-xs-2\"><input type=\"button\" class=\"btn btn-default btn-xs\" value=\"Confirm\" style=\"margin-top:6px;\" onclick=\"LoadStandardPriceValue()\" /></div></div>");
			ReturnValue.Append("</div>");

		}
		RS.Close();

		DB.DBCon.Close();
		return ReturnValue + "";
	}

	protected string Load_Calculated() {
		ChargeC CC = new ChargeC();
		Calculated = new sRequestFormCalculateInfo();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		Calculated = CC.Load_Calculated("RequestForm", RequestFormPk, "", ref DB);

		for (int i = 0; i < Calculated.arrHead.Count; i++) {
			Calculated_Head.Append("<th><table><tr><td style=\"display:none;\"><input type=\"hidden\" id=\"Branch_" + i + "\" value=\"" + Calculated.arrHead[i].Branch_Company_Pk + "\" /></td>");
			Calculated_Head.Append("<td style=\"display:none\"><input type=\"hidden\" id=\"BranchBank_" + i + "\" value=\"" + Calculated.arrHead[i].Branch_Bank_Pk + "\" /></td>");
			Calculated_Head.Append("<td><input type=\"button\" id=\"BtnBranch_" + i + "\" class=\"btn btn-default btn-xs\" value=\"" + Calculated.arrHead[i].Branch_Code + "\" onclick=\"ForModal(Type, this.id)\" /></td></tr>");
			Calculated_Head.Append("<tr><td style=\"display:none;\"><input type=\"hidden\" id=\"Customer_" + i + "\" value=\"" + Calculated.arrHead[i].Customer_Company_Pk + "\" /></td>");
			Calculated_Head.Append("<td><input type=\"button\" id=\"BtnCustomer_" + i + "\" class=\"btn btn-default btn-xs\" value=\"" + Calculated.arrHead[i].Customer_Code + "\" onclick=\"ForModal(Type, this.id)\" /></td></tr></table></th>");

			Calculated_Total.Append("<tr><td style=\"display:none;\"><input type=\"text\"  id=\"ChargeHeadPk_" + i + "\" value=\"" + Calculated.arrHead[i].RequestFormCalculate_Head_Pk + "\" /></td>");
			Calculated_Total.Append("<td id=\"TotalBranch_" + i + "\">" + Calculated.arrHead[i].Branch_Code + "</td>");
			Calculated_Total.Append("<td id=\"TotalCustomer_" + i + "\">" + Calculated.arrHead[i].Customer_Code + "</td>");
			Calculated_Total.Append("<td><select class=\"form-control\" id=\"TotalMonetary_" + i + "\">" + ChargeC.Get_MonetaryUnit(Calculated.arrHead[i].Monetary_Unit) + "</select></td>");
			Calculated_Total.Append("<td><input class=\"form-control\" id=\"TotalPrice_" + i + "\" type=\"text\" value=\"" + Common.NumberFormat(Calculated.arrHead[i].Total_Price) + "\" /></td></tr>");


			for (int j = 0; j < Calculated.arrHead[i].arrBody.Count; j++) {
				Calculated_Item.Append("<tr><td style=\"display:none;\"><input type=\"text\" id=\"ChargeBodyPk_" + j + "\" value=\"" + Calculated.arrHead[i].arrBody[j].RequestFormCalculate_Body_Pk + "\" /></td>");
				Calculated_Item.Append("<td style=\"display:none;\"><input type=\"hidden\" id=\"StandardPriceItem_" + j + "\" value=\"" + Calculated.arrHead[i].arrBody[j].StandardPrice_Item_Pk + "\" /></td>");
				Calculated_Item.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_OverWeightFlag_" + j + "\" value=\"" + ChargeC.Get_StandardPriceItem_Column(Calculated.arrHead[i].arrBody[j].StandardPrice_Item_Pk, "[OVERWEIGHT_FLAG]", ref DB) + "\"/></td>");
				Calculated_Item.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_5_" + j + "\" value=\"" + ChargeC.Get_StandardPriceItem_Column(Calculated.arrHead[i].arrBody[j].StandardPrice_Item_Pk, "[EXW]", ref DB) + "\" /></td>");
				Calculated_Item.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_6_" + j + "\" value=\"" + ChargeC.Get_StandardPriceItem_Column(Calculated.arrHead[i].arrBody[j].StandardPrice_Item_Pk, "[DDP]", ref DB) + "\" /></td>");
				Calculated_Item.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_7_" + j + "\" value=\"" + ChargeC.Get_StandardPriceItem_Column(Calculated.arrHead[i].arrBody[j].StandardPrice_Item_Pk, "[CNF]", ref DB) + "\" /></td>");
				Calculated_Item.Append("<td style=\"display:none;\"><input type=\"text\" id=\"PriceItem_8_" + j + "\" value=\"" + ChargeC.Get_StandardPriceItem_Column(Calculated.arrHead[i].arrBody[j].StandardPrice_Item_Pk, "[FOB]", ref DB) + "\" /></td>");
				Calculated_Item.Append("<td><select class=\"form-control\" id=\"SettlementBranch_" + j + "\">" + ChargeC.Get_OurBranch(Calculated.arrHead[i].arrBody[j].Settlement_Company_Pk) + "</select></td>");
				string[] Selected = { "", "", "" };
				switch (Calculated.arrHead[i].arrBody[j].Category) {
					case "해운비":
						Selected[0] = "selected=\"selected\"";
						break;
					case "대행비":
						Selected[1] = "selected=\"selected\"";
						break;
					case "제세금":
						Selected[2] = "selected=\"selected\"";
						break;
				}
				Calculated_Item.Append("<td><select class=\"form-control\" id=\"PriceCategory_" + j + "\"><option value=\"해운비\" " + Selected[0] + ">해운비 (영세)</option><option value=\"대행비\" " + Selected[1] + ">대행비 (과세)</option><option value=\"제세금\" " + Selected[2] + ">제세금 (관.부가세)</option></select></td>");
				Calculated_Item.Append("<td><input type=\"text\" class=\"form-control\" id=\"Title_" + j + "\" value=\"" + Calculated.arrHead[i].arrBody[j].Title + "\" /></td>");
				Calculated_Item.Append("<td><select class=\"form-control\" id=\"Item_MonetaryUnit_" + j + "\">" + ChargeC.Get_MonetaryUnit(Calculated.arrHead[i].arrBody[j].Original_Monetary_Unit) + "</select></td>");
				Calculated_Item.Append("<td><input type=\"text\" class=\"form-control\" id=\"PriceItemValue_" + j + "\" value=\"" + Common.NumberFormat(Calculated.arrHead[i].arrBody[j].Original_Price) + "\" /></td>");
				for (int x = 0; x < Calculated.arrHead.Count; x++) {
					var Checked = "";
					if (Calculated.arrHead[i].arrBody[j].RequestFormCalculate_Head_Pk == Calculated.arrHead[x].RequestFormCalculate_Head_Pk) {
						Checked = "checked=\"checked\"";
					}
					Calculated_Item.Append("<td style=\"text-align:center;\"><input type=\"radio\" id=\"RadioCharge_" + j + "_" + x + "\" name=\"Radio_" + Calculated.arrHead[i].arrBody[j].StandardPrice_Item_Pk + "\" " + Checked + " /></td>");
				}
				Calculated_Item.Append("</tr>");
			}
		}

		DB.DBCon.Close();
		return "1";
	}

}	