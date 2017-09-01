using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Components;
using System.Data.SqlClient;

public partial class CustomClearance_newCommercialDocu_PackingList : System.Web.UI.Page
{
	protected String Shipper;
	protected String ShipperAddress;
	protected String Consignee;
	protected String ConsigneeAddress;
	protected String NotifyParty;
	protected String NotifyPartyAddress;
	protected String PortOfLanding;
	protected String Carrier;
	protected String FinalDestination;
	protected String SailingOnOrAbout;
	protected String PaymentTerms;
	protected StringBuilder ItemList;
	protected String TotalQuantity;
	protected String TotalGrossWeight;
	protected String TotalNetWeight;
	protected String TotalVolume;
	protected String[] MemberInfo;
	protected String InvoiceNo;
	protected String Buyer;
	protected String Memo;
	protected String StampImg;
	protected String FOBNCNF;
	protected String gubun;

	protected void Page_Load(object sender, EventArgs e) {
		try { MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None); } catch (Exception) { Response.Redirect("../Default.aspx"); }
		//자기문서아니면 안보이게 만들기!!!!!!

		gubun = Request.Params["G"] + "" == "" ? "View" : Request.Params["G"] + "";
		if (Request.Params["S"] + "" != "") {
			LoadCommercialInvoice(Request.Params["S"]);
			LoadCommercialInvoiceItems(Request.Params["S"]);
		}
	}
	private void LoadCommercialInvoice(string CommercialDocumentHeadPk) {
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"	
SELECT 
	[BLNo], [InvoiceNo], [Shipper], [ShipperAddress]
	, [Consignee], [ConsigneeAddress], [NotifyParty], [NotifyPartyAddress], [PortOfLoading]
	, [FinalDestination], [Carrier], [SailingOn], [PaymentTerms] ,[OtherReferences]
	, [StampImg], [FOBorCNF], [Registerd], [ClearanceDate], [StepCL] 
FROM newCommercialDocument 
WHERE CommercialDocumentHeadPk=" + CommercialDocumentHeadPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		if (RS.Read()) {
			InvoiceNo = RS["InvoiceNo"] + "";
			Shipper = "" + RS["Shipper"];
			ShipperAddress = "" + RS["ShipperAddress"];
			Consignee = "" + RS["Consignee"];
			ConsigneeAddress = "" + RS["ConsigneeAddress"];
			NotifyParty = "" + RS["NotifyParty"];
			NotifyPartyAddress = "" + RS["NotifyPartyAddress"];
			PortOfLanding = "" + RS["PortOfLoading"];
			FinalDestination = "" + RS["FinalDestination"];
			Carrier = "" + RS["Carrier"];
			SailingOnOrAbout = "" + RS["SailingOn"];
			PaymentTerms = "" + RS["PaymentTerms"];
			if (RS["StampImg"] + "" != "" && (RS["StampImg"] + "").IndexOf(".") > 0) {
				StampImg = "<div style=\"margin-top:-125px; margin-left:430px; \"><img alt=\"\" src=\"../UploadedFiles" + RS["StampImg"] + "\" /></div>";
			} else {
				StampImg = "";
			}
			Memo = "" + RS["OtherReferences"];
		}
		RS.Dispose();
		DB.DBCon.Close();
	}
	private void LoadCommercialInvoiceItems(string CommercialDocumentHeadPk) {
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"	SELECT CCWR.MonetaryUnitCL,RFI.MarkNNumber, RFI.Label, RFI.PackedCount, RFI.PackingUnit, RFI.GrossWeight, RFI.NetWeight, RFI.Volume, CICK.Description, CICK.Material, Row.C  
															FROM newCommercialDocument as CCWR 
															left join DocumentFormItems AS RFI ON CCWR.CommercialDocumentHeadPk =RFI.DocumentFormPk  
															left join ClearanceItemCodeKOR AS CICK ON RFI.ItemCode=CICK.ItemCode  
															left join (select count(*) AS C, DocumentFormPk FROM DocumentFormItems GROUP BY DocumentFormPk ) AS Row ON CCWR.CommercialDocumentHeadPk =Row.DocumentFormPk  
															WHERE CCWR.CommercialDocumentHeadPk=" + CommercialDocumentHeadPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string PackingCount;
		string GWeight;
		string NWeight;
		string Volume;
		int RowLength = 0;
		Decimal TempTotalGweight = 0;
		Decimal TempTotalNweight = 0;
		Decimal TempTotalVolume = 0;

		ItemList = new StringBuilder();
		Decimal[] TempTotalPackingCount = new Decimal[3] { 0, 0, 0 };
		TotalQuantity = string.Empty;

		bool isFirstRow = true;
		string TRHeight = "";

		while (RS.Read()) {
			if (isFirstRow) {
				int temprowcount = Int32.Parse(RS["C"] + "");
				if (temprowcount > 20 && temprowcount < 30) {
					TRHeight = "30px";
				} else {
					TRHeight = "20px";
				}
				isFirstRow = false;
			}


			RowLength++;
			if (RS["PackedCount"] + "" != "") {
				PackingCount = Common.NumberFormat(RS["PackedCount"] + "") + " " + Common.GetPackingUnit(RS["PackingUnit"] + "");
				TempTotalPackingCount[(Int32.Parse(RS["PackingUnit"] + "") - 15)] += decimal.Parse(RS["PackedCount"] + "");
			} else { PackingCount = ""; }
			if (RS["GrossWeight"] + "" != "") {
				GWeight = Common.NumberFormat(RS["GrossWeight"] + "") + " Kg";
				TempTotalGweight += Decimal.Parse(RS["GrossWeight"] + "");
			} else {
				GWeight = "";
			}
			if (RS["NetWeight"] + "" != "") {
				NWeight = Common.NumberFormat(RS["NetWeight"] + "") + " Kg";
				TempTotalNweight += Decimal.Parse(RS["NetWeight"] + "");
			} else {
				NWeight = "";
			}
			if (RS["Volume"] + "" != "") {
				Volume = Common.NumberFormat(RS["Volume"] + "") + " CBM";
				TempTotalVolume += Decimal.Parse(RS["Volume"] + "");
			} else {
				Volume = "";
			}

			ItemList.Append("<tr style=\"height:" + TRHeight + ";\"><td class=\"BL\" style=\"text-align:center; font-size:12px; \" >" + RS["MarkNNumber"] + "&nbsp;</td>" +
	 //"<td style=\"padding-left:20px; font-size:12px; \" >" + RS["Description"] + (RS["Material"] + "" == "" ? "" : " (" + RS["Material"] + ") ") + "</td>" +
	 "<td style=\"padding-left:20px; font-size:12px; \" >" + (RS["Label"] + "" == "" ? "" : RS["Label"] + " : ") + RS["Description"] + (RS["Material"] + "" == "" ? "" : " (" + RS["Material"] + ") ") + "</td>" +
																				"<td style=\"text-align:right; padding-right:10px; font-size:12px;  \" >" + PackingCount + "</td>" +
																				"<td style=\"text-align:right; padding-right:10px; font-size:12px;  \" >" + GWeight + "</td>" +
																				"<td style=\"text-align:right; padding-right:10px; font-size:12px;  \" >" + NWeight + "</td>" +
																				"<td class=\"BR\" style=\"text-align:right; padding-right:10px; font-size:12px;  \" >" + Volume + "&nbsp;</td></tr>");
		}
		//TotalAmount = TempTotalAmount == 0 ? "" : TempMonetaryUnit + " " + Common.NumberFormat(TempTotalAmount + "");
		TotalGrossWeight = TempTotalGweight == 0 ? "" : Common.NumberFormat(TempTotalGweight + "") + " Kg";

		TotalNetWeight = TempTotalNweight == 0 ? "" : Common.NumberFormat(TempTotalNweight + "") + " Kg";
		TotalVolume = TempTotalVolume == 0 ? "" : Common.NumberFormat(TempTotalVolume + "") + " CBM";

		for (int i = 0; i < TempTotalPackingCount.Length; i++) {
			if (TempTotalPackingCount[i] != 0) {
				TotalQuantity += Common.NumberFormat(TempTotalPackingCount[i] + "") + " " + Common.GetPackingUnit((i + 15) + "") + "<br />";
			}
		}

		for (; RowLength < 17; RowLength++) {
			ItemList.Append("<tr style=\"height:23px;\"><td class=\"BL\" >&nbsp;</td><td >&nbsp;</td><td >&nbsp;</td><td >&nbsp;</td><td >&nbsp;</td><td class=\"BR\" >&nbsp;</td></tr>");
		}
	}
}