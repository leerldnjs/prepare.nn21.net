using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;

public partial class Admin_Dialog_DeliveryReceipt2 : System.Web.UI.Page
{
	protected String GUBUN;

	protected String BLNo;
	protected String CompanyName;
	protected String ItemName;
	protected String CompanyCode;
	protected String Year;
	protected String Month;
	protected String Day;
	protected String BoxCount;
	protected String Address;
	protected String TEL;
	protected String DriverTEL;
	protected String DriverName;
	protected String Weight;
	protected String Volumn;
	protected String CheckDepositBefore;
	protected String CheckDepositAfter;
	protected String Price;
	protected String Memo;
	protected String MemberMemo;
	protected String ContainerIntoDate;
	protected String ContainerNo;
	protected String StorageName;
	protected String DeliveryGubun;
	protected void Page_Load(object sender, EventArgs e)
	{
		GUBUN = Request.Params["G"] + "" == "" ? "View" : Request.Params["G"] + "";
		LoadDeliveryReceipt(Request.Params["S"] + "");
	}
	private void LoadDeliveryReceipt(string HBCPk)
	{
		DBConn DB = new DBConn();
		//, TBC.WarehouseInfo, TBC.WarehouseMobile, TBC.PackedCount, TBC.PackingUnit, TBC.Weight, TBC.Volume, TBC.DepositWhere, TBC.Price, TBC.Memo 
		DB.SqlCmd.CommandText = @"
SELECT TOP 1 TB.[REQUEST_PK], TH.[TRANSPORT_PK], TH.[TITLE], TH.[VESSELNAME], TH.[VALUE_STRING_1] AS DriverTEL, TH.[DATETIME_FROM], TH.[DATETIME_TO]
	, CW.[Title] AS WAREHOSE_TITLE, CW.[Address], CW.[Staff], CW.[Mobile], CW.[TEL], TB.[PACKED_COUNT], R.PackingUnit, R.TotalGrossWeight, R.TotalVolume, RFCH.[BRANCH_COMPANY_PK], RFCH.[CUSTOMER_COMPANY_PK], RFCH.[TOTAL_PRICE]
	, R.TotalPackedCount 
	, OBSC.StorageName
	, C.CompanyName , C.CompanyCode
	, RFI.Description AS ITEMS
	, CD.BLNo, CD.SailingOn, CD.ContainerNo 
	, R.ArrivalDate
	,'' AS Memo
	,'' as MemberMemo
  FROM [dbo].[TRANSPORT_HEAD] AS TH
	left join [dbo].[TRANSPORT_BODY] AS TB ON TB.[TRANSPORT_HEAD_PK]=TH.[TRANSPORT_PK] 
	left join OurBranchStorageCode AS OBSC ON TB.[WAREHOUSE_PK_DEPARTURE]=OBSC.OurBranchStoragePk 
	left join Company AS C ON C.CompanyPk=TH.[BRANCHPK_TO]
	left join RequestFormItems AS RFI ON TB.[REQUEST_PK]=RFI.RequestFormPk 
	left join CommerdialConnectionWithRequest AS CCWR On CCWR.RequestFormPk=TB.[REQUEST_PK]
	left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk 
	left join RequestForm AS R ON TB.[REQUEST_PK]=R.RequestFormPk 
	LEFT JOIN [dbo].[CompanyWarehouse] AS CW ON TH.[AREA_TO] = CAST(CW.[WarehousePk] AS VARCHAR)
	LEFT JOIN [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON RFCH.[TABLE_PK] = TB.[REQUEST_PK]
	LEFT JOIN [dbo].[REQUESTFORMCALCULATE_BODY] AS RFCB ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK] = RFCB.[REQUESTFORMCALCULATE_HEAD_PK]
 WHERE ISNULL(TH.[TRANSPORT_WAY], 'Delivery') = 'Delivery'
 AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
 AND RFCB.[PRICE_CODE] LIKE '%Delivery%'
 AND TH.[TRANSPORT_PK] = " + HBCPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		List<string> item = new List<string>();
		if (RS.Read()) {
			ContainerIntoDate = RS["SailingOn"] + "";
			ContainerNo = RS["ContainerNo"] + "";
			if (RS["BLNo"] + "" != "") {
				BLNo = (RS["BLNo"] + "").Substring(4) + (ContainerNo == "" ? "" : "-" + ContainerNo) + (ContainerIntoDate == "" ? "" : "-" + ContainerIntoDate.Substring(ContainerIntoDate.Length - 5).Replace(".", ""));
			}

			StorageName = RS["StorageName"] + "";

			Year = (RS["ArrivalDate"] + "").Substring(2, 2);
			Month = (RS["ArrivalDate"] + "").Substring(4, 2);
			Day = (RS["ArrivalDate"] + "").Substring(6, 2);
			CompanyName = RS["CompanyName"] + "";
			ItemName = RS["ITEMS"] + "";
			CompanyCode = RS["CompanyCode"] + "";
			BoxCount = RS["PACKED_COUNT"] + " / " + RS["TotalPackedCount"] + " " + Common.GetPackingUnit(RS["PackingUnit"] + "");

			Address = RS["Address"] + "";
			string tempTEL = RS["TEL"] + "" == "" ? "" : "TEL : " + RS["TEL"];
			string tempMobile = RS["Mobile"] + "" == "" ? "" : RS["Staff"] + " : " + RS["Mobile"];
			if (tempTEL == "" && tempMobile == "") {
				TEL = "&nbsp;";
			} else if (tempTEL != "" && tempMobile != "") {
				TEL = tempTEL + ", <br />" + tempMobile;
			} else {
				TEL = tempTEL + tempMobile;
			}

			DriverTEL = RS["DriverTEL"] + "";
			DriverName = RS["VESSELNAME"] + "";
			Weight = Common.NumberFormat(RS["TotalGrossWeight"] + "") + " Kg";
			Volumn = Common.NumberFormat(RS["TotalVolume"] + "") + " CBM";

			DeliveryGubun = RS["Title"] + "" == "" ? "&nbsp;" : RS["Title"] + "";

			if (RS["BRANCH_COMPANY_PK"] + "" == RS["CUSTOMER_COMPANY_PK"] + "") {
				Price = "현불 " + Common.NumberFormat(RS["TOTAL_PRICE"] + "");
			} else {
				Price = "착불 " + Common.NumberFormat(RS["TOTAL_PRICE"] + "");
			}

			Memo = RS["Memo"] + "" == "" ? "&nbsp;" : RS["Memo"] + "";
			MemberMemo = RS["MemberMemo"] + "" == "" ? "&nbsp;" : "/"+RS["MemberMemo"] + "";
		}
		RS.Dispose();
		DB.DBCon.Close();
	}
}