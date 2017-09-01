using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Admin_Dialog_FixCharge_old : System.Web.UI.Page
{
	private DBConn DB;
	protected String Head;
	protected StringBuilder st_StandardPrice;
	protected String GrossWeight;
	protected String OverWeight;
	protected String Volume;
	protected String[] PaymmentWhoCL;
	protected String RequestFormPk;
	protected String departureDate;
	protected String IsEstimation;
	private String TransportWayCL;
	private String DepartureRegionCode;
	private String ArrivalRegionCode;
	public String Estimation_DIV;
	protected String ShipperPk, ConsigneePk;
	protected String ShipperRegisterd, ShipperContents;
	protected String ConsigneeRegisterd, ConsigneeContents;

	protected void Page_Load(object sender, EventArgs e) {
		RequestFormPk = Request.Params["S"];
		DefaultSetting(RequestFormPk);
		LoadSelectListStandardPrice(RequestFormPk);
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");

		if (IsEstimation == "33") {
			Estimation_DIV = "<div style=\"padding-top:10px; \">COMMENT for Shipper<br /><textarea id=\"TBComment_Shipper\" rows=\"5\" cols=\"92\"></textarea></div>" +
							"<div style=\"padding-top:10px; \">COMMENT for Consignee<br /><textarea id=\"TBComment_Consignee\" rows=\"5\" cols=\"92\"></textarea></div>";
		} else {
			Estimation_DIV = "<div style=\"padding-top:10px; \">COMMENT for Shipper<br /><textarea id=\"TBComment_Shipper\" rows=\"5\" cols=\"92\">위와 같이 예상 산출된 운임 / 통관 부대비용을 청구하오니 상기 계좌번호로 입금시켜 주시기 바랍니다.</textarea></div>" +
							"<div style=\"padding-top:10px; \">COMMENT for Consignee<br /><textarea id=\"TBComment_Consignee\" rows=\"5\" cols=\"92\">위와 같이 예상 산출된 운임 / 통관 부대비용을 청구하오니 상기 계좌번호로 입금시켜 주시기 바랍니다.</textarea></div>";
		}
	}
	private void DefaultSetting(string RequestFormPk) {
		string arrivalDate;
		DB = new DBConn();
		DB.SqlCmd.CommandText = "EXEC SP_SelectFixChargeLoad @RequestFormPk=" + RequestFormPk;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		/*	RF.ShipperCode, RF.ConsigneeCode, RF.DepartureDate, RF.ArrivalDate, RF.DepartureRegionCode, 
		 *	RF.ArrivalRegionCode, RF.TransportWayCL, RF.JubsuWayCL, RF.PaymentWayCL, RF.PaymentWhoCL, 
		 *	RF.DocumentRequestCL, RF.MonetaryUnitCL, RF.Memo ,
		 *	RFCH.TotalPackedCount, RFCH.PackingUnit, RFCH.TotalGrossWeight, RFCH.TotalVolume, DStaff.Value, AStaff.Value		*/

		if (RS.Read()) {
			ShipperPk = RS["ShipperPk"] + "";
			ConsigneePk = RS["ConsigneePk"] + "";
			TransportWayCL = RS["TransportWayCL"] + "";
			DepartureRegionCode = RS["DepartureRegionCode"] + "";
			ArrivalRegionCode = RS["ArrivalRegionCode"] + "";
			departureDate = RS["DepartureDate"] + "";
			arrivalDate = RS["ArrivalDate"] + "";
			string TransportWay;
			TransportWay = Common.GetTransportWay(TransportWayCL);

			PaymmentWhoCL = new string[5] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
			switch (RS["PaymentWhoCL"] + "") {
				case "5":
					PaymmentWhoCL[0] = "checked=\"checked\"";
					break;
				case "6":
					PaymmentWhoCL[1] = "checked=\"checked\"";
					break;
				case "7":
					PaymmentWhoCL[2] = "checked=\"checked\"";
					break;
				case "8":
					PaymmentWhoCL[3] = "checked=\"checked\"";
					break;
				case "9":
					PaymmentWhoCL[4] = "checked=\"checked\"";
					break;
			}

			GrossWeight = RS["TotalGrossWeight"] + "";
			Volume = RS["TotalVolume"] + "";
			Decimal tempGWeight = GrossWeight == "" ? 0 : Decimal.Parse(GrossWeight);
			Decimal tempVolume = Volume == "" ? 0 : Decimal.Parse(Volume);

			switch (RS["OurBranchCode"] + "") {
				case "3843":
					if (tempVolume * 250 < tempGWeight) {
						OverWeight = Math.Round(tempGWeight / 250, 2).ToString();
					} else {
						OverWeight = Volume;
					}
					break;
				case "2888":
					if (tempVolume * 400 < tempGWeight) {
						OverWeight = Math.Round(tempGWeight / 400, 2).ToString();
					} else {
						OverWeight = Volume;
					}
					break;
				case "11456":
					if (tempVolume * 400 < tempGWeight) {
						OverWeight = Math.Round(tempGWeight / 400, 2).ToString();
					} else {
						OverWeight = Volume;
					}
					break;
				case "7898":
					if (tempVolume * 400 < tempGWeight) {
						OverWeight = Math.Round(tempGWeight / 400, 2).ToString();
					} else {
						OverWeight = Volume;
					}
					break;
				case "2887":
					if (tempVolume * 350 < tempGWeight) {
						OverWeight = Math.Round(tempGWeight / 350, 2).ToString();
					} else {
						OverWeight = Volume;
					}
					break;
				case "3388":
					if (tempVolume * 350 < tempGWeight) {
						OverWeight = Math.Round(tempGWeight / 350, 2).ToString();
					} else {
						OverWeight = Volume;
					}
					break;
				case "2886":
					if (tempVolume * 350 < tempGWeight) {
						OverWeight = Math.Round(tempGWeight / 350, 2).ToString();
					} else {
						OverWeight = Volume;
					}
					break;
				case "3157":
					OverWeight = Volume;
					break;
			}

			Head = "<div>" + GetGlobalResourceObject("qjsdur", "dnsthdqkdqjq") + " : " + TransportWay + "</div>" +
						"<div>" + GetGlobalResourceObject("qjsdur", "cnfqkfwl") + " : <strong>" + departureDate + "</strong> (" + RS["DRCName"] + ") ~ <strong>" + arrivalDate + "</strong> (" + RS["ARCName"] + ")</div>" +
						"<div>" + GetGlobalResourceObject("qjsdur", "chdqkrtm") + " : <strong>" + RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "") + "</strong>&nbsp;&nbsp;" + GetGlobalResourceObject("qjsdur", "chdcpwjr") + " : <strong>" + Common.NumberFormat(RS["TotalVolume"] + "") + "CBM</strong>&nbsp;&nbsp;" + GetGlobalResourceObject("qjsdur", "chdwndfid") + " : <strong>" + Common.NumberFormat(RS["TotalGrossWeight"] + "") + "Kg</strong></div>";
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = "SELECT StepCL FROM RequestForm WHERE RequestFormPk=" + RequestFormPk + ";";
		IsEstimation = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		Html_Contents(ShipperPk, ConsigneePk);

		//Response.Write(DB.SqlCmd.CommandText);
	}
	private String Html_Contents(string ShipperPk, string ConsigneePk) {
		string ShipperReturnValue = new Admin().LoadTalkBusiness(ShipperPk, "42");
		if (ShipperReturnValue == "") {
			var Each = ShipperReturnValue.Split(new string[] { "@@" }, StringSplitOptions.None);
			ShipperRegisterd = "";
			ShipperContents = "";
		} else {
			var Each = ShipperReturnValue.Split(new string[] { "@@" }, StringSplitOptions.None);
			ShipperRegisterd = "Registerd Info: <strong>" + Each[0] + " / " + Each[2] + "</strong>";
			ShipperContents = Each[1];
		}

		string ConsigneeReturnValue = new Admin().LoadTalkBusiness(ConsigneePk, "42");
		if (ConsigneeReturnValue == "") {
			var Each = ConsigneeReturnValue.Split(new string[] { "@@" }, StringSplitOptions.None);
			ConsigneeRegisterd = "";
			ConsigneeContents = "";
		} else {
			var Each = ConsigneeReturnValue.Split(new string[] { "@@" }, StringSplitOptions.None);
			ConsigneeRegisterd = "Registerd Info: <strong>" + Each[0] + " / " + Each[2] + "</strong>";
			ConsigneeContents = Each[1];
		}

		return "1";
	}
	private void LoadSelectListStandardPrice(string requestformpk) {
		//DepartureRegionCode = "2!01!04";
		//ArrivalRegionCode = "1!01";
		DB.DBCon.Open();
		st_StandardPrice = new StringBuilder();
		st_StandardPrice.Append("<select id=\"st_StandardPrice\" ><option value=\"0\">수기입력</option>");
		SqlDataReader RS;

		if (TransportWayCL == "31") {
			DB.SqlCmd.CommandText = @"
				SELECT [StandardPriceListPk], [Title]
				FROM [INTL2010].[dbo].[StandardPrice] 
				WHERE ArrivalBranchPk is null and TransportWayCL=31";
			RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				st_StandardPrice.Append("<option value=\"" + RS["StandardPriceListPk"] + "\">" + RS["Title"] + "</option>");
			}
			RS.Close();
		}

		DB.SqlCmd.CommandText = @"
			SELECT TCLS.StandardPriceListPk 
			FROM TransportWayCLSetting AS TCLS 
			WHERE isnull(TCLS.StandardPriceListPk, '0') <> '0' and DepartureRegionCodePk = " + Common.StringToDB(DepartureRegionCode, false, false) + " and ArrivalRegionCodePk = " + Common.StringToDB(ArrivalRegionCode, false, false) + " and TransportWayCL = " + Common.StringToDB(TransportWayCL, false, false) + ";";
		
		//Response.Write(DB.SqlCmd.CommandText);
		string StandardPricePk = DB.SqlCmd.ExecuteScalar() + "";
		if (StandardPricePk != "") {
			DB.SqlCmd.CommandText = @"SELECT [StandardPriceListPk], [TransportWayCL], [Title], [FreightCharge], [DepartureCharge], [ArrivalCharge] 
														FROM StandardPrice 
														WHERE StandardPriceListPk in (" + StandardPricePk + ") ORDER BY TransportWayCL ASC;";
			RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				//[StandardPriceListPk], [TransportWayCL], [Title], [FreightCharge], [DepartureCharge], [ArrivalCharge] 
				st_StandardPrice.Append("<option value=\"" + RS["StandardPriceListPk"] + "\">" + RS["Title"] + "</option>");
			}
			RS.Dispose();
		}

		DB.SqlCmd.CommandText = @"
			SELECT 
				[StandardPriceListPk],[Title] 
			FROM 
				[INTL2010].[dbo].[StandardPrice] 
			WHERE BranchPk_Own=" + Session["CompanyPk"] + " " + (StandardPricePk != "" ? " and StandardPriceListPk not in (" + StandardPricePk + ") " : "") + " ;";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			st_StandardPrice.Append("<option value=\"" + RS["StandardPriceListPk"] + "\">" + RS["Title"] + "</option>");
		}
		RS.Close();

		if (ConsigneePk == "7597") {
			st_StandardPrice.Append("<option value=\"" + "215" + "\" style='background-color:#FFFACD;'>" + "이우-인천 (산동항) G (65000)" + "</option>");
		}
		DB.DBCon.Close();
		st_StandardPrice.Append("</select>");
	}
}