using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Finance_ClearanceList : System.Web.UI.Page
{
	protected String InnerSelect;
	private DBConn DB;
	protected String ArrivalBranch;
	protected String StartDate;
	protected String EndDate;
	protected String Value;
	protected String Gubun;
	protected String ListHtml;
	protected String Summary;

	protected void Page_Load(object sender, EventArgs e) {
		ArrivalBranch = Request.Params["AB"] + "" == "" ? "3157" : Request.Params["AB"] + "";
		StartDate = Request.Params["SD"] + "";
		EndDate = Request.Params["ED"] + "";
		Value = Request.Params["V"] + "";
		Gubun = Request.Params["G"] + "";
		DB = new DBConn();
		DB.DBCon.Open();
		InnerSelect = LoadInnerSelect(ArrivalBranch);

		switch (Gubun) {
			case "S": ListHtml = LoadClearanceList(ArrivalBranch, Value, StartDate, EndDate); break;
			case "C": ListHtml = LoadClearanceListASCompanyCode(ArrivalBranch, Value, StartDate, EndDate); break;
			default: ListHtml = ""; break;
		}
		DB.DBCon.Close();
	}

	private String LoadInnerSelect(string BranchPk) {
		DB.SqlCmd.CommandText = @"
SELECT CID.CompanyInDocumentPk, CID.Name, CID.Address 
FROM CompanyInDocument AS CID 
WHERE CID.GubunPk=" + BranchPk + "  and CID.GubunCL = 6;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		StringBuilder Tempinnerselect = new StringBuilder();
		Tempinnerselect.Append("<option value='4732'>WORLD EVERGREEN IMPORT AND EXPORT CO.，LTD.</option>");
		while (RS.Read()) {
			string IsSelected = "";
			Tempinnerselect.Append("<option " + IsSelected + " value='" + RS[0] + "'>" + RS[1] + "</option>");
		}
		RS.Dispose();
		return Tempinnerselect + "";
	}
	private String LoadClearanceList(string ArrivalBranch, string ShipperCompanyPk, string StartDate, string EndDate) {
		StringBuilder ReturnValue = new StringBuilder();
		StringBuilder Query = new StringBuilder();
		List<string[]> SumShipper = new List<string[]>();
		List<decimal[]> SumAmount = new List<decimal[]>();

		List<string> TotalShipperNMonetary = new List<string>();
		List<decimal> TotalAmount = new List<decimal>();
		List<decimal> TotalPackingCount = new List<decimal>();

		if (ShipperCompanyPk != "0" && ShipperCompanyPk != "") {
			DB.SqlCmd.CommandText = "SELECT [Name] FROM CompanyInDocument WHERE [CompanyInDocumentPk]=" + ShipperCompanyPk + "; ";
			Query.Append(" and CID.Shipper='" + DB.SqlCmd.ExecuteScalar() + "'");
		}

		if (StartDate != "") {
			Query.Append(" and ClearanceDate>='" + StartDate + "'");
		}

		if (EndDate != "") {
			Query.Append(" and ClearanceDate<='" + EndDate + "'");
		}
		DB.SqlCmd.CommandText = @"
SELECT CID.CommercialDocumentHeadPk, CID.BLNo, CID.Shipper, 
	Total.Amount, Total.PackedCount, Total.GrossWeight, Total.Volume, 
	RF.MonetaryUnitCL, 
	C.CompanyName, C.CompanyCode , ClearanceDate
  FROM CommercialDocument AS CID
	left join CommerdialConnectionWithRequest AS CCWR ON CID.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
	inner join (
		SELECT CCWR.CommercialDocumentPk, SUM(RFI.Amount) AS Amount, SUM(RF.TotalPackedCount) as PackedCount, SUM(RF.TotalGrossWeight) AS GrossWeight, SUM(RF.TotalVolume ) AS Volume
		FROM RequestFormItems AS RFI	
			left join CommerdialConnectionWithRequest AS CCWR ON CCWR.RequestFormPk=RFI.RequestFormPk
			left join [dbo].[RequestForm] AS RF ON RF.RequestFormPk=RFI.RequestFormPk 
		WHERE ISNULL(RFI.Amount, 0)>0
		Group By CCWR.CommercialDocumentPk
	) AS Total ON Total.CommercialDocumentPk=CID.CommercialDocumentHeadPk 
	left join RequestForm AS RF On RF.RequestFormPk=CCWR.RequestFormPk
	left join Company AS C On RF.ConsigneePk=C.CompanyPk
	left join RegionCode AS ARC ON RF.ArrivalRegionCode=ARC.RegionCode
WHERE OurBranchCode=" + ArrivalBranch + Query +
" order by ClearanceDate;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			bool CheckAdd = true;
			for (int i = 0; i < SumShipper.Count; i++) {
				if (SumShipper[i][0] == RS["Shipper"] + "" && SumShipper[i][1] == RS["MonetaryUnitCL"] + "") {
					SumAmount[i][0] += decimal.Parse("" + RS["Amount"]);
					SumAmount[i][1] += decimal.Parse("" + RS["PackedCount"]);
					SumAmount[i][2] += "" + RS["GrossWeight"] == "" ? 0 : decimal.Parse("" + RS["GrossWeight"]);
					SumAmount[i][3] += decimal.Parse("" + RS["Volume"]);
					CheckAdd = false;
				}
			}
			if (CheckAdd) {
				SumShipper.Add(new string[] { RS["Shipper"] + "", RS["MonetaryUnitCL"] + "" });
				SumAmount.Add(new decimal[] { decimal.Parse("" + RS["Amount"]), decimal.Parse("" + RS["PackedCount"]), decimal.Parse("" + RS["GrossWeight"]), decimal.Parse("" + RS["Volume"]) });
			}


			if (ShipperCompanyPk != "0" && ShipperCompanyPk != "") {
				ReturnValue.Append("<tr><td class='TBody1' >" +
					RS["ClearanceDate"] + "</td><td class='TBody1' >" +
					RS["BLNo"] + "</td><td class='TBody1' >" +
					RS["CompanyCode"] + "</td><td class='TBody1' >" +
					RS["CompanyName"] + "</td><td class='TBody1' >" +
					Common.GetMonetaryUnit("" + RS["MonetaryUnitCL"]) + "</td><td class='TBody1' >" +
					Common.NumberFormat("" + RS["Amount"]) + "</td><td class='TBody1' >" +
					Common.NumberFormat("" + RS["PackedCount"]) + "</td><td class='TBody1' >" +
					Common.NumberFormat("" + RS["GrossWeight"]) + "</td><td class='TBody1' >" +
					Common.NumberFormat("" + RS["Volume"]) + "</td><tr>");
			} else {
				ReturnValue.Append("<tr><td class='TBody1' >" +
					RS["ClearanceDate"] + "</td><td class='TBody1' >" +
					RS["BLNo"] + "</td><td class='TBody1' >" +
					RS["CompanyCode"] + "</td><td class='TBody1' >" +
					RS["CompanyName"] + "</td><td class='TBody1' >" +
					RS["Shipper"] + "</td><td class='TBody1' >" +
					Common.GetMonetaryUnit("" + RS["MonetaryUnitCL"]) + "</td><td class='TBody1' >" +
					Common.NumberFormat("" + RS["Amount"]) + "</td><td class='TBody1' >" +
					Common.NumberFormat("" + RS["PackedCount"]) + "</td><td class='TBody1' >" +
					Common.NumberFormat("" + RS["GrossWeight"]) + "</td><td class='TBody1' >" +
					Common.NumberFormat("" + RS["Volume"]) + "</td><tr>");
			}
		}
		RS.Dispose();
		DB.DBCon.Close();

		StringBuilder TempSummery = new StringBuilder();
		for (int i = 0; i < SumShipper.Count; i++) {
			TempSummery.Append("<fieldset><legend style=\"font-weight:bold; padding:10px; \">" + SumShipper[i][0] + "</legend><strong>" +
					"총금액</strong> : " + Common.GetMonetaryUnit(SumShipper[i][1]) + " " + Common.NumberFormat(SumAmount[i][0] + "") + "&nbsp;&nbsp;<strong>" +
					"포장수량</strong> : " + Common.NumberFormat(SumAmount[i][1] + "") + "&nbsp;&nbsp;<strong>" +
					"중량</strong> : " + Common.NumberFormat(SumAmount[i][2] + "") + "&nbsp;&nbsp;<strong>" +
					"체적</strong> : " + Common.NumberFormat(SumAmount[i][3] + "") +
				"</fieldset>");
		}
		Summary = TempSummery + "";

		if (ShipperCompanyPk != "0" && ShipperCompanyPk != "") {
			return "<table border='0' cellpadding='0' cellspacing='0' style='width:1050px;' ><thead><tr style=\"height:40px;\"><td class='THead1' >통관일</td><td class='THead1' >BLNo</td><td class='THead1' >고객번호</td><td class='THead1' >상호</td><td colspan=\"2\" class='THead1' >물품대금</td><td class='THead1' >포장수량</td><td class='THead1' >중량</td><td class='THead1' >체적</td><tr>" + ReturnValue + "</table>";
		} else {
			return "<table border='0' cellpadding='0' cellspacing='0' style='width:1050px;' ><thead><tr style=\"height:40px;\"><td class='THead1' >통관일</td><td class='THead1' >BLNo</td><td class='THead1' >고객번호</td><td class='THead1' >상호</td><td class='THead1' >Shipper</td><td colspan=\"2\" class='THead1' >물품대금</td><td class='THead1' >포장수량</td><td class='THead1' >중량</td><td class='THead1' >체적</td><tr>" + ReturnValue + "</table>";
		}
	}
	private String LoadClearanceListASCompanyCode(string ArrivalBranch, string CompanyCode, string StartDate, string EndDate) {
		StringBuilder ReturnValue = new StringBuilder();
		StringBuilder Query = new StringBuilder();
		List<string[]> SumShipper = new List<string[]>();
		List<decimal[]> SumAmount = new List<decimal[]>();

		DB.SqlCmd.CommandText = "SELECT [CompanyPk] FROM Company WHERE CompanyCode='" + CompanyCode + "';";
		string CompanyPk = DB.SqlCmd.ExecuteScalar() + "";

		if (CompanyPk == "") {
			return "없는 고객번호입니다.";
		}

		if (StartDate != "") {
			Query.Append(" and ClearanceDate>='" + StartDate + "'");
		}

		if (EndDate != "") {
			Query.Append(" and ClearanceDate<='" + EndDate + "'");
		}
		DB.SqlCmd.CommandText = @"
SELECT CID.CommercialDocumentHeadPk, CID.BLNo, CID.Shipper, 
	Total.Amount, Total.PackedCount, Total.GrossWeight, Total.Volume, 
	RF.MonetaryUnitCL, 
	C.CompanyName, C.CompanyCode , ClearanceDate
  FROM CommercialDocument AS CID
	left join CommerdialConnectionWithRequest AS CCWR ON CID.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
	inner join (
		SELECT CCWR.CommercialDocumentPk, SUM(RFI.Amount) AS Amount, SUM(RF.TotalPackedCount) as PackedCount, SUM(RF.TotalGrossWeight) AS GrossWeight, SUM(RF.TotalVolume ) AS Volume
		FROM RequestFormItems AS RFI	
			left join CommerdialConnectionWithRequest AS CCWR ON CCWR.RequestFormPk=RFI.RequestFormPk
			left join [dbo].[RequestForm] AS RF ON RF.RequestFormPk=RFI.RequestFormPk 
		WHERE ISNULL(RFI.Amount, 0)>0
		Group By CCWR.CommercialDocumentPk
	) AS Total ON Total.CommercialDocumentPk=CID.CommercialDocumentHeadPk 
	left join RequestForm AS RF On RF.RequestFormPk=CCWR.RequestFormPk
	left join Company AS C On RF.ConsigneePk=C.CompanyPk
	left join RegionCode AS ARC ON RF.ArrivalRegionCode=ARC.RegionCode
WHERE RF.ConsigneePk=" + CompanyPk + " and isnull(Shipper, '0')<>'0' and OurBranchCode=" + ArrivalBranch + Query +
" order by ClearanceDate;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			bool CheckAdd = true;
			for (int i = 0; i < SumShipper.Count; i++) {
				if (SumShipper[i][0] == RS["Shipper"] + "" && SumShipper[i][1] == RS["MonetaryUnitCL"] + "") {
					SumAmount[i][0] += decimal.Parse("" + RS["Amount"]);
					SumAmount[i][1] += decimal.Parse("" + RS["PackedCount"]);
					SumAmount[i][2] += decimal.Parse("" + RS["GrossWeight"]);
					SumAmount[i][3] += decimal.Parse("" + RS["Volume"]);
					CheckAdd = false;
				}
			}
			if (CheckAdd) {
				SumShipper.Add(new string[] { RS["Shipper"] + "", RS["MonetaryUnitCL"] + "" });
				SumAmount.Add(new decimal[] { decimal.Parse("" + RS["Amount"]), decimal.Parse("" + RS["PackedCount"]), decimal.Parse("" + RS["GrossWeight"]), decimal.Parse("" + RS["Volume"]) });
			}

			ReturnValue.Append("<tr><td class='TBody1' >" +
	RS["ClearanceDate"] + "</td><td class='TBody1' >" +
	RS["BLNo"] + "</td><td class='TBody1' >" +
	RS["CompanyCode"] + "</td><td class='TBody1' >" +
	RS["CompanyName"] + "</td><td class='TBody1' >" +
	RS["Shipper"] + "</td><td class='TBody1' >" +
	Common.GetMonetaryUnit("" + RS["MonetaryUnitCL"]) + "</td><td class='TBody1' >" +
	Common.NumberFormat("" + RS["Amount"]) + "</td><td class='TBody1' >" +
	Common.NumberFormat("" + RS["PackedCount"]) + "</td><td class='TBody1' >" +
	Common.NumberFormat("" + RS["GrossWeight"]) + "</td><td class='TBody1' >" +
	Common.NumberFormat("" + RS["Volume"]) + "</td><tr>");
		}
		RS.Dispose();
		DB.DBCon.Close();

		StringBuilder TempSummery = new StringBuilder();
		for (int i = 0; i < SumShipper.Count; i++) {
			TempSummery.Append("<fieldset><legend style=\"font-weight:bold;\">" + SumShipper[i][0] + "</legend><p>" +
					"총금액 : " + Common.GetMonetaryUnit(SumShipper[i][1]) + " " + Common.NumberFormat(SumAmount[i][0] + "") + "</p><p>" +
					"포장수량 : " + Common.NumberFormat(SumAmount[i][1] + "") + "</p><p>" +
					"중량 : " + Common.NumberFormat(SumAmount[i][2] + "") + "</p><p>" +
					"체적 : " + Common.NumberFormat(SumAmount[i][3] + "") + "</p>" +
				"</fieldset>");
		}
		Summary = TempSummery + "";
		return "<table border='0' cellpadding='0' cellspacing='0' style='width:1050px;' ><thead><tr style=\"height:40px;\"><td class='THead1' >통관일</td><td class='THead1' >BLNo</td><td class='THead1' >고객번호</td><td class='THead1' >상호</td><td class='THead1' >Shipper</td><td colspan=\"2\" class='THead1' >물품대금</td><td class='THead1' >포장수량</td><td class='THead1' >중량</td><td class='THead1' >체적</td><tr>" + ReturnValue + "</table>";
	}
}