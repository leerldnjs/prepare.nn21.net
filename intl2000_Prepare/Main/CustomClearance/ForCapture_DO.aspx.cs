using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class CustomClearance_ForCapture_DO : System.Web.UI.Page
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
	protected String ArrivalDate;
	protected String PaymentTerms;
	protected StringBuilder ItemList;
	protected String TotalQuantity;
	protected String TotalGrossWeight;
	protected String TotalNetWeight;
	protected String TotalVolume;
	protected String[] MemberInfo;
	protected String BLNo;
	protected String Buyer;
	protected String Memo;
	protected String FOBNCNF;
	protected String gubun;
	protected String VoyageNo;
	protected String VoyageCompany;
	protected String ContainerNo;
	protected String SealNo;
	protected String ContainerSize;
	protected String ClearanceDate;
	protected String MasterBL;
	protected String StorageName;
	protected String Description;
	protected String MRN;
	protected String MSN;

	private DBConn DB;
	protected void Page_Load(object sender, EventArgs e)
	{
		DB = new DBConn("PoolingNo");
		LocdMasterNStorage(Request.Params["S"]);
		LoadCommercialInvoice(Request.Params["S"]);
		LoadCommercialInvoiceItems(Request.Params["S"]);
		LoadDeliveryOrder(Request.Params["B"]);
	}
	private bool LocdMasterNStorage(string commercialDocumentPk)
	{
		DB.SqlCmd.CommandText = @"

DECLARE @RequestFormPk int;

SELECT TOP 1 @RequestFormPk=[RequestFormPk]
  FROM [dbo].[CommerdialConnectionWithRequest]
 WHERE [CommercialDocumentPk]=" + commercialDocumentPk + @"

SELECT TBBH.[VALUE_STRING_0], OBSC.StorageName ,TBBH.[VESSELNAME], TBBH.[VOYAGE_NO], TBBH.[VALUE_STRING_1], TBBH.[VALUE_STRING_2], TBBH.[VALUE_STRING_3], TBP.[NO]
  FROM [dbo].[TRANSPORT_HEAD] AS TBBH
	left join [dbo].[TransportBBHistory] AS TBBHistory ON TBBH.[TRANSPORT_PK]=TBBHistory.[TransportBetweenBranchPk]
	left join OurBranchStorageCode AS OBSC ON TBBHistory.StorageCode=OBSC.OurBranchStoragePk 
	LEFT JOIN [dbo].[TRANSPORT_PACKED] AS TBP ON TBBH.[TRANSPORT_PK] = TBP.[TRANSPORT_HEAD_PK]
 WHERE RequestFormPk=@RequestFormPk  and 	OBSC.OurBranchCode=3157;";
		//Response.Write(DB.SqlCmd.CommandText);
		return true;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		if (RS.Read()) {
			MasterBL = RS[0] + "";
			StorageName = RS["StorageName"] + "";
			Description = RS["VESSELNAME"] + (RS["NO"] + "" == "" ? "" : "(" + RS["NO"] + ")");
		}
		RS.Dispose();
		DB.DBCon.Close();
		return true;
	}
	private void LoadCommercialInvoice(string CommercialDocumentHeadPk)
	{
		DB.SqlCmd.CommandText = "SELECT		[BLNo], [InvoiceNo], [Shipper], [ShipperAddress], [Consignee], [ConsigneeAddress], [NotifyParty], [NotifyPartyAddress], [PortOfLoading], [FinalDestination], [Carrier], [SailingOn], [PaymentTerms] " +
													"				,	[OtherReferences], [StampImg], [FOBorCNF], [VoyageNo], [VoyageCompany], [ContainerNo], [SealNo], [ContainerSize], [Registerd], [ClearanceDate], [StepCL] " +
													"	FROM CommercialDocument " +
													"	WHERE CommercialDocumentHeadPk=" + CommercialDocumentHeadPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		if (RS.Read()) {
			BLNo = RS["BLNo"] + "";
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
			Memo = "" + RS["OtherReferences"];
			VoyageNo = "" + RS["VoyageNo"];
			VoyageCompany = "" + RS["VoyageCompany"];
			ContainerNo = "" + RS["ContainerNo"];
			SealNo = "" + RS["SealNo"];
			ContainerSize = "" + RS["ContainerSize"];
			ClearanceDate = "" + RS["ClearanceDate"] == "" ? "" : string.Format("{0}/{1}/{2}", ("" + RS["ClearanceDate"]).Substring(0, 4), ("" + RS["ClearanceDate"]).Substring(4, 2), ("" + RS["ClearanceDate"]).Substring(6, 2));
			string tempFOBNCNF = RS["FOBorCNF"] + "" == "" ? "" : (RS["FOBorCNF"] + "").Substring(0, 3);
			FOBNCNF = tempFOBNCNF + "" == "FOB" ? "FREIGHT COLLECT" : "FREIGHT PREPAID";

		}
		RS.Dispose();
		DB.DBCon.Close();
	}
	private void LoadCommercialInvoiceItems(string CommercialDocumentHeadPk)
	{
		DB.SqlCmd.CommandText = @"	SELECT CICK.Description, CICK.Material 
															FROM CommerdialConnectionWithRequest as CCWR 
																left join RequestFormItems AS RFI ON CCWR.RequestFormPk =RFI.RequestFormPk  
																left join ClearanceItemCodeKOR AS CICK ON RFI.ItemCode=CICK.ItemCode  
															WHERE CCWR.CommercialDocumentPk=" + CommercialDocumentHeadPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		List<string> TempItemSum = new List<string>();
		while (RS.Read()) {
			bool CheckIsIn = false;
			for (int i = 0; i < TempItemSum.Count; i++) { if (TempItemSum[i] == (RS["Description"] + "").Trim()) { CheckIsIn = true; } }
			if (!CheckIsIn) {
				TempItemSum.Add((RS["Description"] + "").Trim());
			}
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"	SELECT RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume 
															FROM CommerdialConnectionWithRequest AS CCWR
																left join [dbo].[RequestForm] AS RF ON CCWR.RequestFormPk=RF.RequestFormPk	
															WHERE CCWR.CommercialDocumentPk=" + CommercialDocumentHeadPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		string BeforePacking = "";
		Decimal TempTotalGweight = 0;
		Decimal TempTotalVolume = 0;
		Decimal TempTotalPackingCount = 0;

		while (RS.Read()) {
			if (RS[0] + "" != "") { TempTotalPackingCount += Decimal.Parse(RS[0] + ""); }
			if (RS[1] + "" != "") {
				if (BeforePacking == "") {
					BeforePacking = Common.GetPackingUnit(RS[1] + "");
				} else if (BeforePacking != Common.GetPackingUnit(RS[1] + "")) {
					BeforePacking = "GT";
				}
			}
			if (RS[2] + "" != "") { TempTotalGweight += Decimal.Parse(RS[2] + ""); }
			if (RS[3] + "" != "") { TempTotalVolume += Decimal.Parse(RS[3] + ""); }
		}

		RS.Dispose();
		DB.DBCon.Close();
		ItemList = new StringBuilder();
		for (int i = 0; i < TempItemSum.Count; i++) {
			if (i == 0) {
				ItemList.Append(TempItemSum[i]);
			} else {
				if (TempItemSum[(i - 1)].Length > 50) {
					ItemList.Append(", <br /> " + TempItemSum[i]);
				} else {
					ItemList.Append(", " + TempItemSum[i]);
				}
			}
		}
		TotalQuantity = string.Empty;

		TotalGrossWeight = TempTotalGweight == 0 ? "" : Common.NumberFormat(TempTotalGweight + "") + " Kg";
		TotalVolume = TempTotalVolume == 0 ? "" : Common.NumberFormat(TempTotalVolume + "") + " CBM";
		TotalQuantity = TempTotalPackingCount + " " + BeforePacking;
	}
	private void LoadDeliveryOrder(string TransportBBPk)
	{
		DB.SqlCmd.CommandText = "SELECT [MRN], [MSN] FROM [CommercialDocumentDO] " +
													"	WHERE [TransportBBPk]=" + TransportBBPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		if (RS.Read()) {
			MRN = RS["MRN"] + "";
			MSN = "" + RS["MSN"];
		}
		RS.Dispose();
		DB.DBCon.Close();
	}

}
