using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

public partial class RequestForm_RequestFormView : System.Web.UI.Page
{
	protected String Shipper;
	protected String Consignee;
	protected String Notify;
	protected String Schedule;
	protected String ItemTable;
	protected String Payment;
	protected String RequestFormAdditionalInfoCL;
	protected String ModifyCount;
	private String SorC;
	protected String TotalCount;
	protected String TotalAmount;
	protected String TotalPackedCount;
	protected String TotalWeight;
	protected String TotalVolume;
	protected String FILELIST;
	protected String CLEARANCELIST;
	protected String BTNModify;

	protected String[] MEMBERINFO;
	private Boolean IsConsigneeConfirmed;
	protected StringBuilder NAVIGATIONBUTTONS;
	protected StringBuilder HtmlDelivery;
	protected string DeliveryButton;
	private DBConn DB;
	protected string RequestPk;
	protected string companypk;
	protected string AccountID;
	protected string HtmlDeposited;
	//private List<string> RequestFormData;
	protected void Page_Load(object sender, EventArgs e) {
		DB = new DBConn();
		if (Request.Params["M"] + "" != "") {
			string[] MPARMS = Request.Params["M"].ToString().Split(Common.Splite11, StringSplitOptions.None);
			string Mmobile0 = MPARMS[0];
			string Mmobile1 = MPARMS[1];
			string Mpk0 = MPARMS[2];
			string Mpk1 = MPARMS[3];
			string Macountpk = MPARMS[4];
			RequestPk = ((Int32.Parse(Mpk0) * 99) + Int32.Parse(Mpk1)).ToString();
			McheckParam(Macountpk, Mmobile0, Mmobile1, RequestPk, out AccountID, out companypk);
		} else if (Request.Params["Q"] + "" != "") {
			if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
				Response.Redirect("../Default.aspx");
			} else {
				MEMBERINFO = (Session["MemberInfo"] + "").Split(Common.Splite11, StringSplitOptions.None);
			}
			companypk = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[1];
			AccountID = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[2];
			string[] QPARMS = Request.Params["Q"].ToString().Split(Common.Splite11, StringSplitOptions.None);
			string Qpk0 = QPARMS[0];
			string Qpk1 = QPARMS[1];
			RequestPk = ((Int32.Parse(Qpk0) * 99) + Int32.Parse(Qpk1)).ToString();
		} else {
			if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
				Response.Redirect("../Default.aspx");
			} else {
				MEMBERINFO = (Session["MemberInfo"] + "").Split(Common.Splite11, StringSplitOptions.None);
			}
			companypk = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[1];
			AccountID = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[2];
			RequestPk = Request.Params["Pk"];
		}

		string IsViewableResult = IsViewable(RequestPk, companypk);
		if (IsViewableResult != "1") {
			HttpCookie aCookie;
			string cookieName;
			int limit = Request.Cookies.Count;
			for (int i = 0; i < limit; i++) {
				cookieName = Request.Cookies[i].Name;
				aCookie = new HttpCookie(cookieName);
				aCookie.Expires = DateTime.Now.AddDays(-1);
				Response.Cookies.Add(aCookie);
			}

			Response.Cookies["IL"]["MemberInfo"] = null;
			Response.Cookies["IL"]["SubInfo"] = null;
			Response.Cookies["IL"]["Gubun"] = null;
			Response.Cookies["IL"].Expires = DateTime.Now.AddDays(-1);

			Session["MemberInfo"] = null;
			Session["SubInfo"] = null;
			Response.Redirect("../Default.aspx");
		}

		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } } catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }

		NAVIGATIONBUTTONS = new StringBuilder();
		//[1]companypk [2]id
		CustomerSignBTN(RequestPk, companypk, AccountID);
		SetRequestFormView(RequestPk);
		LoadInvoiceOrPacking(RequestPk, companypk);
		LoadFileList_Ever(RequestPk, companypk);
		LoadDeliveryLoad(RequestPk);
		LoadDeposited(RequestPk);
	}
	private void LoadDeposited(string RequestFormPk) {

		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT  RF.ShipperPk
		,RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume, 
		RFCH.[MONETARY_UNIT], RFCH.[TOTAL_PRICE], RFCH.[LAST_DEPOSITED_DATE], RFCH.[DEPOSITED_PRICE],RF.ExchangeDate
FROM   RequestForm RF 
       LEFT JOIN [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON RF.[RequestFormPk] = RFCH.[TABLE_PK] 
              WHERE RF.RequestFormPk=" + RequestFormPk + @"
		AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm' ;";
		DB.DBCon.Open();

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			decimal TotalCharge;
			decimal Deposited = 0;
			string MonetaryUnit;

			TotalCharge = RS["TOTAL_PRICE"] + "" != "" ? decimal.Parse(RS["TOTAL_PRICE"] + "") : 0;
			if (RS["DEPOSITED_PRICE"] + "" != "") {
				Deposited = decimal.Parse(RS["DEPOSITED_PRICE"] + "");
			}
			MonetaryUnit = Common.GetMonetaryUnit(RS["MONETARY_UNIT"] + "");

			switch (MonetaryUnit) {
				case "￥":
					TotalCharge = Math.Round(TotalCharge, 1, MidpointRounding.AwayFromZero);
					break;
				case "$":
					TotalCharge = Math.Round(TotalCharge, 2, MidpointRounding.AwayFromZero);
					break;
				case "￦":
					TotalCharge = Math.Round(TotalCharge, 0, MidpointRounding.AwayFromZero);
					break;
			}
			string LastminusPlus = "";
			string LastTotalCharge = "";
			string LastDeposited = "";
			if (TotalCharge > 0 && Deposited == 0) {
				LastminusPlus = "<span style=\"color:red;\">未</span>";
			} else if (TotalCharge == 0) {
				LastminusPlus = "--";
			} else {
				decimal tempminus = Deposited - TotalCharge;
				if (tempminus == 0) {
					LastminusPlus = "<span style=\"color:green;\">完</span>";
				} else if (tempminus > 0) {
					LastminusPlus = "<span style=\"color:blue;\">" + MonetaryUnit + " " + Common.NumberFormat(tempminus + "") + "</span>";
				} else {
					LastminusPlus = "<span style=\"color:red;\">" + MonetaryUnit + " " + Common.NumberFormat(tempminus + "") + "</span>";
				}
			}
			LastTotalCharge = TotalCharge == 0 ? "--" : MonetaryUnit + " " + Common.NumberFormat(TotalCharge + "");
			LastDeposited = Deposited == 0 ? "--" : MonetaryUnit + " " + Common.NumberFormat(Deposited + "");
			HtmlDeposited = "<li>" + GetGlobalResourceObject("qjsdur", "cjdrnrmador") + ": " + LastTotalCharge + "</li>" +
				 "<li>" + GetGlobalResourceObject("qjsdur", "dlqrmador") + ": " + LastDeposited + "</li>" +
				"<li>" + GetGlobalResourceObject("qjsdur", "ckdor") + ": " + LastminusPlus + "</li>";
		}

		RS.Dispose();
		DB.DBCon.Close();

	}
	private void LoadDeliveryLoad(string RequestFormPk) {
		HtmlDelivery = new StringBuilder();
		string DeliveryTable = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:580px;\">" +
			"<tr><td style=\"background-color:#E8E8E8;  width:150px; text-align:center; \">Arrival Time</td>" +
			"		<td style=\"background-color:#E8E8E8;  width:75px; text-align:center;\">Box Count</td>" +
			"		<td style=\"background-color:#E8E8E8;  width:95px; text-align:center;\"></td>" +
			"		<td style=\"background-color:#E8E8E8;  \">Deliverer</td>" +
			"		<td class='THead1' style=\"width:70px; background-color:#E8E8E8; \" rowspan=\"2\" >{0}</td></tr>" +
			"<tr><td colspan=\"2\" class='THead1'  style=\"text-align:center;\">Consignee Staff</td><td class='THead1'  colspan=\"2\" style=\"text-align:center;\">Address</td></tr>{1}</table>";
		string Row = "	<tr><td style=\"text-align:center;\" >{1}</td>" +
								"	<td style=\"text-align:center;\">{2} / {3}</td>" +
								"	<td style=\"text-align:center;\"></td>" +
								"	<td style=\"text-align:center;\">{5}</td>" +
								"	<td style=\"text-align:center;\" rowspan=\"2\" class=\"TBody1\" >{8}</td></tr>" +
							"	<tr><td class=\"TBody1\" colspan=\"2\" style=\"text-align:center;\">{6}</td>" +
								"	<td class=\"TBody1\" colspan=\"2\">{7}</td></tr>" +
					 "<tr><td class=\"TBody1\" colspan=\"4\" style=\"text-align:center;\">{11}</td></tr>";
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT 
	OBSO.OurBranchStorageOutPk, OBSO.BoxCount, CONVERT(CHAR(8), OBSO.StockedDate, 10) AS StockedDate,  OBSO.StatusCL 
	, OBSC.StorageName
	, TBC.TransportBetweenCompanyPk, TBC.Type, TBC.Title, TBC.DriverName, TBC.DriverTEL, 
	TBC.FromDate, TBC.ToDate, TBC.WarehouseInfo, TBC.WarehouseMobile, TBC.PackedCount, 
	TBC.PackingUnit, TBC.Weight, TBC.Volume, TBC.DepositWhere, TBC.Price, TBC.Memo ,OBSO.TransportBetweenBranchPk
--	TBM.Memo as MemberMemo
FROM OurBranchStorageOut AS OBSO 
	left join OurBranchStorageCode AS OBSC ON OBSO.StorageCode=OBSC.OurBranchStoragePk 
	left join TransportBC AS TBC ON OBSO.TransportBetweenCompanyPk=TBC.TransportBetweenCompanyPk 	
	--left join TransportBCMemo AS TBM ON OBSO.TransportBetweenCompanyPk=TBM.TransportBetweenCompanyPk 	
WHERE OBSO.RequestFormPk=" + RequestFormPk + " ORDER BY OBSO.StatusCL DESC ;";
		DB.DBCon.Open();
		StringBuilder TempRow = new StringBuilder();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		bool CheckIsReaded = false;

		while (RS.Read()) {
			if (!CheckIsReaded) {
				CheckIsReaded = true;
			}
			//TransportBetweenBranchPk = RS["TransportBetweenBranchPk"] + "";

			string[] RowValue = new string[12];
			RowValue[0] = RS["OurBranchStorageOutPk"] + "";
			RowValue[1] = RS["StorageName"] + "" == "" ? "미도착" : "<strong>" + RS["StorageName"] + "</strong> " + (RS["StockedDate"] + "" == "" ? "" : (RS["StockedDate"] + "").Substring(0, 5));
			RowValue[2] = RS["BoxCount"] + "";
			RowValue[3] = TotalPackedCount + Common.GetPackingUnit(RS["PackingUnit"] + "");
			//RowValue[4] = RS["Title"] + "";
			if (RS["Title"] + "" == "") {
				RowValue[4] = RS["Type"] + "";
			} else {
				RowValue[4] = RS["Title"] + "";
			}
			RowValue[5] = RS["DriverTEL"] + "" == "" ? RS["DriverName"] + "" : RS["DriverName"] + " (" + RS["DriverTEL"] + ")";

			string[] tempwarehouse = RS["WarehouseInfo"] + "" == "" ? new string[] { "", "", "" } : (RS["WarehouseInfo"] + "").Split(Common.Splite22, StringSplitOptions.None);
			RowValue[6] = RS["WarehouseMobile"] + "" == "" ? tempwarehouse[2] : tempwarehouse[2] + " (" + RS["WarehouseMobile"] + ")";
			if (RS["StatusCL"] + "" == "0" || RS["StatusCL"] + "" == "4") {
				RowValue[7] = "";
				RowValue[8] = "기사미지정";
			} else {
				RowValue[7] = "";
				//RowValue[7] = tempwarehouse[0];
				RowValue[8] = RS["StatusCL"] + "" == "6" ?
					(RS["FromDate"] + "" == "" ? "" : (RS["FromDate"] + "").Substring(4, 4)) + "<br /><span style=\"color:green;\">출고완료</span>" : "";
			}
			RowValue[9] = RS["TransportBetweenCompanyPk"] + "";
			//RowValue[10] = RS["MemberMemo"] + "" == "" ? "":"Memo:" + RS["MemberMemo"] + "";
			RowValue[11] = RS["WarehouseInfo"] + "" == "" ? "&nbsp;" : tempwarehouse[0];
			TempRow.Append(String.Format(Row, RowValue));
		}
		RS.Dispose();
		DB.DBCon.Close();

		if (CheckIsReaded) {
			HtmlDelivery.Append(String.Format(DeliveryTable, "&nbsp;", TempRow));
		} else {
			HtmlDelivery.Append("");
		}
	}
	protected Boolean McheckParam(string Macountpk, string Mmobile0, string Mmobile1, string RequestPk, out string AccountID, out string companypk) {
		string ID = "";
		string Cpk = "";
		string mobile = "";
		string Apk = "";
		string CKMobile = "";
		string CKmobilehypen = "";
		string CKCpk = "";
		DB.SqlCmd.CommandText = @"SELECT [AccountPk]
      ,[AccountID]
      ,[CompanyPk]
      ,[Name]
      ,[Mobile]
      ,[Email]
  FROM [INTL2010].[dbo].[Account_]
  where AccountPk=" + Macountpk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ID = RS["AccountID"] + "";
			Cpk = RS["CompanyPk"] + "";
			mobile = RS["Mobile"] + "";
			Apk = RS["AccountPk"] + "";
		} else {
			AccountID = "";
			companypk = "";
			RS.Dispose();
			DB.DBCon.Close();
			return true;
		}
		CKmobilehypen = Regex.Replace(mobile, @"\D", "");
		CKMobile = CKmobilehypen.Substring(CKmobilehypen.Length - 3, 3);

		int CKMobile0 = Int32.Parse(CKMobile) / 99;
		int CKMobile1 = Int32.Parse(CKMobile) % 99;
		RS.Dispose();

		DB.SqlCmd.CommandText = @"SELECT [RequestFormPk]
      ,[ConsigneePk]
  FROM [INTL2010].[dbo].[RequestForm]
where RequestFormPk=" + RequestPk + ";";

		SqlDataReader RS1 = DB.SqlCmd.ExecuteReader();
		while (RS1.Read()) {
			CKCpk = RS1["ConsigneePk"] + "";
		}
		RS1.Dispose();
		DB.DBCon.Close();

		if (Apk == Macountpk && CKMobile0.ToString() == Mmobile0 && CKMobile1.ToString() == Mmobile1 && Cpk == CKCpk) {
			//성공
			AccountID = ID;
			companypk = Cpk;
		} else {
			AccountID = "";
			companypk = "";
			Response.Redirect("../Default.aspx");
		}
		return true;
	}
	private String IsViewable(string RequestFormPk, string CompanyPk) {
		string shipperpk = "";
		string consigneepk = "";
		DB.SqlCmd.CommandText = "SELECT ShipperPk , ConsigneePk FROM RequestForm where RequestFormPk=" + RequestFormPk;
		DB.DBCon.Open();
		SqlDataReader SDR = DB.SqlCmd.ExecuteReader();
		SDR.Read();
		string ReturnData = "0";
		shipperpk = SDR[0] + "";
		consigneepk = SDR[1] + "";
		if (shipperpk == CompanyPk) {
			SorC = "S";
			ReturnData = "1";
		} else if (consigneepk == CompanyPk) {
			SorC = "C";
			ReturnData = "1";
		}
		SDR.Dispose();
		if (ReturnData == "1") {
			DB.DBCon.Close();
			return ReturnData;
		} else {
			if (shipperpk != "") {
				DB.SqlCmd.CommandText = "SELECT [MainCompanyPk] FROM [CompanyRelation] WHERE TargetCompanyPk=" + shipperpk + " and GubunCL=0";
				SDR = DB.SqlCmd.ExecuteReader();
				while (SDR.Read()) {
					if (SDR[0] + "" == CompanyPk) {
						SorC = "S";
						ReturnData = "1";
						break;
					}
				}
				SDR.Dispose();
				if (ReturnData == "1") {
					DB.DBCon.Close();
					return ReturnData;
				}
			}
			if (consigneepk != "") {
				DB.SqlCmd.CommandText = "SELECT [MainCompanyPk] FROM [CompanyRelation] WHERE TargetCompanyPk=" + consigneepk + " and GubunCL=0";
				SDR = DB.SqlCmd.ExecuteReader();
				while (SDR.Read()) {
					if (SDR[0] + "" == CompanyPk) {
						SorC = "C";
						ReturnData = "1";
						break;
					}
				}
				SDR.Dispose();
				DB.DBCon.Close();
			}
			if (ReturnData != "1") {
				DB.SqlCmd.CommandText = @"
					SELECT 
						COUNT(*)
					FROM [dbo].[HISTORY] AS RFH 
						left join Account_ AS A ON RFH.[ACCOUNT_ID]=A.AccountId
					  WHERE RFH.[TABLE_NAME] = 'RequestForm' AND RFH.[TABLE_PK]=" + RequestFormPk + @" and RFH.[CODE]='50' and CompanyPk=" + CompanyPk + @"
					";
				DB.DBCon.Open();
				string Count = DB.SqlCmd.ExecuteScalar().ToString();
				DB.DBCon.Close();
				if (Count != "0") {
					SorC = "S";
					ReturnData = "1";
				}
			}


		}
		return ReturnData;
	}
	private void SetRequestFormView(string pk) //Gubun 70=업체 71=지사
	{
		//RequestFormData = new List<string>();
		//RP = new RequestP();
		//string Item = RP.RequestFormView(pk, ref RequestFormData);
		string MonetaryUnit = string.Empty;
		DB.SqlCmd.CommandText = @"
DECLARE @StorageCodePk int; 
DECLARE @TransportBetweenCompanyPk int; 
DECLARE @NaverX varchar(50);
SELECT TOP 1 @StorageCodePk =StorageOut.[storagecode] , @NaverX =Code.NaverMapPathX, @TransportBetweenCompanyPk = StorageOut.TransportBetweenCompanyPk
        FROM   [ourbranchstorageout] AS StorageOut
        left join [OurBranchStorageCode] AS Code ON StorageOut.[storagecode] =Code.OurBranchStoragePk 
        WHERE  requestformpk = " + pk + @"; 
SELECT 
	R.ConsigneePk, R.ShipperCode, R.ConsigneeCode, R.DepartureDate, R.ArrivalDate	
	, R.ShipperStaffName, R.ShipperStaffTEL, R.ShipperStaffMobile, R.TransportWayCL, R.JubsuWayCL 
	, R.PaymentWayCL, R.PaymentWhoCL, R.DocumentRequestCL, R.MonetaryUnitCL, R.StepCL  
	, R.PickupRequestDate, R.NotifyPartyName, R.NotifyPartyAddress, R.Memo, R.DocumentStepCL
	, ShipperC.CompanyName AS ShipperCompanyName, ShipperC.CompanyTEL AS ShipperCompanyTEL, ShipperC.CompanyFAX AS ShipperCompanyFAX
	, ConsigneeC.CompanyName AS ConsigneeCompanyName, ConsigneeC.CompanyTEL AS ConsigneeCompanyTEL, ConsigneeC.CompanyFAX AS ConsigneeCompanyFAX 
	, DepartureRegion.Name AS DepartureName , ArrivalRegion.Name AS ArrivalName, 
	(
		SELECT count(*) FROM [dbo].[HISTORY] WHERE TABLE_NAME = 'RequestFrom' AND TABLE_PK=" + pk + @" AND (CODE='70' or CODE='71')
	) AS ModifyCount
	, @StorageCodePk    AS StorageCode 
    , @NaverX AS NaverX 
,@TransportBetweenCompanyPk AS TransportBetweenCompanyPk
, TBBHead.[VALUE_STRING_0], TBBHead.[VESSELNAME], TBBHead.[VOYAGE_NO], TBBHead.[VALUE_STRING_1], TBBHead.[VALUE_STRING_2], TBBHead.[VALUE_STRING_3], TBBHead.[AREA_FROM], TBBHead.[AREA_TO]
FROM RequestForm AS R 
	left join Company AS ShipperC ON R.ShipperPk=ShipperC.CompanyPk 
	left join Company AS ConsigneeC ON R.ConsigneePk=ConsigneeC.CompanyPk
	left join RegionCode AS DepartureRegion ON R.DepartureRegionCode=DepartureRegion.RegionCode 
	left join RegionCode AS ArrivalRegion ON R.ArrivalRegionCode=ArrivalRegion.RegionCode
    left join (select RequestFormpk,TransportBetweenBranchPk from TransportBBHistory 
               union all
               select [REQUEST_PK],[TRANSPORT_HEAD_PK] from [dbo].[STORAGE]) as TBBPk on R.RequestFormPk=TBBPk.RequestFormPk 
    left join (select * from [dbo].[TRANSPORT_HEAD] where [BRANCHPK_TO]=3157) AS TBBHead ON TBBPk.[TransportBetweenBranchPk]=TBBHead.[TRANSPORT_PK]
    WHERE R.RequestFormPk=" + pk + " order by TBBHead.[TRANSPORT_PK] desc;";
		//Response.Write(DB.SqlCmd.CommandText);

		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		if (RS.Read()) {
			Int32 stepcl = Int32.Parse(RS["StepCL"] + "");
			//if (stepcl < 54) { NAVIGATIONBUTTONS.Append("<p><input type=\"button\" onclick=\"Goto('Modify');\" value=\"" + GetGlobalResourceObject("qjsdur", "wjqtnwmdtnwjd") + "\" /></p>"); }
			if (stepcl > 57) {
				NAVIGATIONBUTTONS.Append("<p><input type=\"button\" onclick=\"Goto('ViewCalculate');\" value=\"운임비용 확인\" /></p>");
			}
			string StorageCode = RS["StorageCode"] + "";
			string TransportBetweenCompanyPk = RS["TransportBetweenCompanyPk"] + "";
			string NaverX = RS["NaverX"] + "";
			//if (StorageCode == "2" || StorageCode == "58" || StorageCode == "61" || StorageCode == "65" || StorageCode == "69" || StorageCode == "71" || StorageCode == "76" || StorageCode == "77" || StorageCode == "79" || StorageCode == "125" || StorageCode == "130" || StorageCode == "132" || StorageCode == "133" || StorageCode == "153" || StorageCode == "156"||StorageCode == "157") {
			//	NAVIGATIONBUTTONS.Append("<p><input type=\"button\" onclick=\"PopWarehouseMap('" + RS["StorageCode"] + "');\" value=\"창고약도 보기\" /></p>");
			//}
			if (NaverX != "") {
				NAVIGATIONBUTTONS.Append("<p><input type=\"button\" onclick=\"PopWarehouseMap('" + RS["StorageCode"] + "');\" value=\"창고약도 보기\" /></p>");
			}

			Int32 DocumentStep = RS["DocumentStepCL"] + "" == "" ? 0 : Int32.Parse(RS["DocumentStepCL"] + "");
			//20140806 명세수정 버튼 수정

			if (StorageCode == "" && stepcl > 56) {
				if (Request.Params["M"] + "" != "" || Request.Params["Q"] + "" != "") {
					DeliveryButton = "<p><input type=\"button\" style=\"width: 350px; height: 75px; font-weight: bold; letter-spacing: 15px;  font-size: 30px;\" onclick=\"DeliverySetByMember();\" value=\"배송지 입력\" /></p>";
				} else {
					DeliveryButton = "<p><input type=\"button\" onclick=\"DeliverySetByMember();\" value=\"배송지 입력\" /></p>";
				}
			}

			if (DocumentStep < 5 && DocumentStep != 3) {
				BTNModify = "<input type=\"button\" value=" + GetGlobalResourceObject("qjsdur", "audtptnwjd") + " onclick=\"PopupItemModify('" + RequestPk + "')\" style=\"width:100px; height:20px;\" />";
			} else {
				BTNModify = "<input type=\"button\" value=" + GetGlobalResourceObject("qjsdur", "audtptnwjd") + " onclick=\"Warning()\" style=\"width:100px; height:20px;\" />";
				//BTNModify = "<input type=\"button\" value=" + GetGlobalResourceObject("qjsdur", "audtptnwjd") + " onclick=\"PopupItemModify('" + RequestPk + "')\" style=\"width:100px; height:20px;\" />";
			}

			if (RS["ModifyCount"] + "" != "0") {
				ModifyCount = "<input type=\"button\" value=\"수정내역 (" + RS["ModifyCount"] + ")\" onclick=\"PopupItemModifyList('" + RequestPk + "')\" style=\"width:100px; height:20px;\" />";
			} else {
				ModifyCount = string.Empty;
			}

			Shipper = "<div class=\"tdSubT\">" + GetGlobalResourceObject("RequestForm", "Shipper") + "</div>" +
				"<div><ul style=\"line-height:20px;\"><li>" + GetGlobalResourceObject("Member", "CompanyCode") + " : " + RS["ShipperCode"] + "</li>" +
				"<li>" + GetGlobalResourceObject("Member", "CompanyName") + " : " + RS["ShipperCompanyName"] + "</li>" +
				"<li>" + GetGlobalResourceObject("Member", "TEL") + " : " + RS["ShipperCompanyTEL"] + "</li><li>FAX : " + RS["ShipperCompanyFAX"] + "</li></ul></div>";

			Consignee = "<div class=\"tdSubT\" >" + GetGlobalResourceObject("RequestForm", "Consignee") + "</div>" +
				"<div><ul style=\"line-height:20px;\"><li>" + GetGlobalResourceObject("Member", "CompanyCode") + " : " + RS["ConsigneeCode"] + "</li>" +
				"<li>" + GetGlobalResourceObject("Member", "CompanyName") + " : " + RS["ConsigneeCompanyName"] + "</li>" +
				"<li>TEL : " + RS["ConsigneeCompanyTEL"] + "</li>" +
				"<li>FAX : " + RS["ConsigneeCompanyFAX"] + "</li></ul></div>";

			switch (RS["PaymentWayCL"] + "") {
				case "0":
					Notify = string.Empty;
					break;
				case "3":
					Notify = "<li>" + GetGlobalResourceObject("RequestForm", "Bank") + " : " + RS["NotifyPartyName"] + "</li><li>" + GetGlobalResourceObject("RequestForm", "Address") + " : " + RS["NotifyPartyAddress"] + "</li>";
					break;
				case "4":
					if (RS["NotifyPartyName"] + "" == "") {
						Notify = "<li>" + GetGlobalResourceObject("RequestForm", "SameAsAbove") + "</li>";
					} else {
						Notify = "<li>" + GetGlobalResourceObject("Member", "CompanyName") + " : " + RS["NotifyPartyName"] + "</li><li>" + GetGlobalResourceObject("RequestForm", "Address") + " : " + RS["NotifyPartyAddress"] + "</li>";
					}
					break;
			}
			if (Notify != string.Empty) {
				Notify = "<div class=\"tdSubT\">" + GetGlobalResourceObject("RequestForm", "NotifyParty") + "</div>" +
					"<div><ul style=\"line-height:20px;\">" + Notify + "</ul></div>";
			}
			
			string TempArrivalPort = "";
			if (RS["VESSELNAME"].ToString() != "") {
				if (RS["AREA_TO"].ToString().Trim().Length > 5) {
					if (RS["AREA_TO"].ToString().Trim().Substring(0, 6) == "PYONGT") {
						TempArrivalPort = "PYEONGTAEK";
					} else if (RS["AREA_TO"].ToString().Trim().Substring(0, 4) == "INCH") {
						TempArrivalPort = "INCHEON";
					} else {
						TempArrivalPort = "";
					}
				}
			}

			Schedule = "<div class=\"tdSubT\">" + GetGlobalResourceObject("RequestForm", "Schedule") + "</div>" +
				"<div><ul style=\"line-height:20px;\"><li>" + Common.GetTransportWay(RS["TransportWayCL"] + "") + "</li><li>" + RS["DepartureDate"] + " ~ " + RS["ArrivalDate"] + "</li><li>" + RS["DepartureName"] + " ~ " + RS["ArrivalName"] + "</li><li>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjd") + " : " + TempArrivalPort + "</li></ul></div>";
			switch (RS["PaymentWhoCL"] + "") {
				case "0":
					Payment = string.Empty;
					break;
				case "5":
					Payment = "<li>" + GetGlobalResourceObject("RequestForm", "PaymentA") + "</li>";
					break;
				case "6":
					Payment = "<li>" + GetGlobalResourceObject("RequestForm", "PaymentB") + "</li>";
					break;
				case "7":
					Payment = "<li>" + GetGlobalResourceObject("RequestForm", "PaymentC") + "</li>";
					break;
				case "8":
					Payment = "<li>" + GetGlobalResourceObject("RequestForm", "PaymentD") + "</li>";
					break;
				case "9":
					Payment = "<li>" + GetGlobalResourceObject("RequestForm", "ETC");
					string temp = "";
					if (RS["Memo"] + "" != "") {
						//temp = (RS["Memo"] + "").Substring((RS["Memo"] + "").IndexOf("$$$") + 3, (RS["Memo"] + "").IndexOf("^^^") - (RS["Memo"] + "").IndexOf("$$$") - 3);
						temp = RS["Memo"] + "";

					}
					if (temp != "") {
						Payment += " : " + temp;
					}
					break;
			}

			string[] documentRequest = (RS["DocumentRequestCL"] + "").Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
			foreach (string documentCL in documentRequest) {
				switch (documentCL) {
					case "10":
						Payment += "<li>원산지 증명</li>";
						break;
					case "11":
						Payment += "<li>식품위생허가증</li>";
						break;
					case "12":
						Payment += "<li>전기안전승인</li>";
						break;
					case "24":
						Payment += "<li>상검</li>";
						break;
					case "25":
						Payment += "<li>수책</li>";
						break;
					case "31":
						Payment += "<li>화주원산지제공(客户提供产地证)</li>";
						break;
					case "32":
						Payment += "<li>원산지신청(申请产地证)</li>";
						break;
					case "33":
						Payment += "<li>단증보관(单证报关)</li>";
						break;
				}
			}
			MonetaryUnit = Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "");
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"
SELECT 
	Items.RequestFormItemsPk, Items.ItemCode, Items.MarkNNumber, Items.Description, Items.Label
	, Items.Material, Items.Quantity, Items.QuantityUnit, Items.PackedCount, Items.PackingUnit
	, Items.GrossWeight, Items.Volume, Items.UnitPrice, Items.Amount, Items.LastModify
FROM RequestFormItems AS Items WHERE Items.RequestFormPk=" + pk + "order by MarkNNumber desc,Description asc";
		RS = DB.SqlCmd.ExecuteReader();

		StringBuilder TempITEM = new StringBuilder();
		Decimal tempTotalCount = 0;
		Decimal tempTotalAmount = 0;
		Decimal tempTotalPackedCount = 0;
		Decimal tempTotalWeight = 0;
		Decimal tempTotalVolume = 0;

		string ItemRow = "<tr><td align='center' class='ItemTableIn'>{0}</td>" +
			"<td align='left' style='padding-left:5px;' class='ItemTableIn'>{1}</td>" +
			"<td align='center' class='ItemTableIn'>{2}</td>" +
			"<td align='center' class='ItemTableIn'>{3}</td>" +
			"<td align='center' class='ItemTableIn' style=\"text-align:right; padding-right:10px; \" >{4}</td>" +
			"<td align='center' class='ItemTableIn'>{5}</td>" +
			"<td align='center' class='ItemTableIn'>{6}</td>" +
			"<td align='center' class='ItemTableIn'>{7}</td></tr>";
		while (RS.Read()) {
			if (RS["Quantity"] + "" != "") { tempTotalCount += decimal.Parse(RS["Quantity"] + ""); }
			if (RS["Amount"] + "" != "") { tempTotalAmount += decimal.Parse(RS["Amount"] + ""); }
			if (RS["PackedCount"] + "" != "") { tempTotalPackedCount += decimal.Parse(RS["PackedCount"] + ""); }
			if (RS["GrossWeight"] + "" != "") { tempTotalWeight += decimal.Parse(RS["GrossWeight"] + ""); }
			if (RS["Volume"] + "" != "") { tempTotalVolume += decimal.Parse(RS["Volume"] + ""); }

			string[] RowData = new string[] {
				RS["MarkNNumber"] +"",
				""+RS["Description"]+(RS["Label"]+""==""?"&nbsp;":" / "+RS["Label"])+(RS["Material"]+""==""?"&nbsp;":" / "+RS["Material"]),
				RS["Quantity"] + "" == "" ? "&nbsp;" : Common.NumberFormat(RS["Quantity"] + "") + " " + Common.GetQuantityUnit(RS["QuantityUnit"] + "") ,
				MonetaryUnit + " " + Common.NumberFormat(RS["UnitPrice"] + "") ,
				MonetaryUnit + " " + Common.NumberFormat(RS["Amount"] + "") ,
				RS["PackedCount"] + "" == "" ? "&nbsp;" : Common.NumberFormat(RS["PackedCount"] + "") + " " + Common.GetPackingUnit(RS["PackingUnit"] + ""),
				RS["GrossWeight"]+ "" == "" ? "&nbsp;" : Common.NumberFormat(RS["GrossWeight"] + "") + " Kg" ,
				RS["Volume"]+ "" == "" ? "&nbsp;" : Common.NumberFormat(RS["Volume"] + "") + " CBM"};
			TempITEM.Append(String.Format(ItemRow, RowData));
		}
		RS.Dispose();
		DB.DBCon.Close();
		ItemTable = TempITEM + "";
		TotalCount = Common.NumberFormat(tempTotalCount + "");
		TotalAmount = Common.NumberFormat(tempTotalAmount + "");
		TotalPackedCount = Common.NumberFormat(tempTotalPackedCount + "");
		TotalWeight = Common.NumberFormat(tempTotalWeight + "");
		TotalVolume = Common.NumberFormat(tempTotalVolume + "");
	}   //접수증 내용 빼오기
	private void CustomerSignBTN(string pk, string MemberPk, string AccountID) {
		DB.SqlCmd.CommandText = "  SELECT COUNT(*) FROM [dbo].[HISTORY] WHERE [CODE]='41' and [TABLE_NAME] = 'RequestForm' AND [TABLE_PK]=" + pk;
		DB.DBCon.Open();
		string Count = DB.SqlCmd.ExecuteScalar() + "";
		if (Count == "0") {
			IsConsigneeConfirmed = false;
			NAVIGATIONBUTTONS.Append("<p><input type=\"button\" onclick=\"CustomerSign('" + pk + "', '" + MemberPk + "', '" + AccountID + "', 'C');\" value=\"Document OK\" /></p>");
		} else {
			IsConsigneeConfirmed = true;
		}

		if (SorC == "") {
			DB.SqlCmd.CommandText = "SELECT ShipperPk, ConsigneePk, ShipperSignID, ShipperSignDate, ConsigneeSignID, ConsigneeSignDate FROM RequestForm WHERE RequestFormPk=" + pk + ";";
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				if (RS["ConsigneePk"] + "" == MemberPk) {
					SorC = "C";
				} else {
					SorC = "S";
				}
			}
			RS.Dispose();
		}

		DB.DBCon.Close();
	}
	private void LoadInvoiceOrPacking(string RequestFormPk, string MemberPk) {
		StringBuilder filelist = new StringBuilder();

		DB.SqlCmd.CommandText = "SELECT [CommercialDocumentPk] FROM CommerdialConnectionWithRequest WHERE RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		string CommercialDocumentPk = DB.SqlCmd.ExecuteScalar() + "";

		DB.SqlCmd.CommandText = "select [FilePk] from [dbo].[File] where [GubunCL]= 18  and [GubunPk]=" + RequestFormPk + ";";
		string BL_UploadPk = DB.SqlCmd.ExecuteScalar() + "";

		string ShowBl = "";
		if (BL_UploadPk == "") {
			ShowBl = "<a onclick=\"ViewInPacking('B' , '" + CommercialDocumentPk + "');\" ><span style=\"cursor:hand;\">HOUSE BL</span></a>";
		} else {
			ShowBl = "<a onclick=\"ShowOtherBL('" + BL_UploadPk + "');\" ><span style=\"cursor:hand;\">HOUSE BL</span></a>";
		}

		if (CommercialDocumentPk != "") {
			filelist.Append(String.Format("<tr style=\"height:20px; \"><td colspan='5' class='{0}' style=\"text-align:left; padding-left:20px; \" >{1}</td></tr>" +
				"<tr style=\"height:20px; \"><td colspan='5' class='{0}' style=\"text-align:left; padding-left:20px; \" >{2}</td></tr>" +
				"<tr style=\"height:20px; \"><td colspan='5' class='{0}' style=\"text-align:left; padding-left:20px; \" >{3}</td></tr>"
				, "TBody1G"
				, ShowBl
				, "<a onclick=\"ViewInPacking('I' , '" + CommercialDocumentPk + "');\" ><span style=\"cursor:hand;\">COMMERCIAL INVOICE</span></a>"
				, "<a onclick=\"ViewInPacking('P' , '" + CommercialDocumentPk + "');\" ><span style=\"cursor:hand;\">PACKING LIST</span></a>"));
		}
		SqlDataReader RS;
		if (SorC == "C") {
			DB.SqlCmd.CommandText = "SELECT [ClearancedFilePk], [GubunCL], [PhysicalPath] FROM ClearancedFile WHERE RequestFormPk=" + RequestFormPk + " and GubunCL<100 ORDER BY GubunCL;";
			RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				string tempstring;
				switch (RS["GubunCL"] + "") {
					case "0":
						tempstring = "모음";
						break;
					case "1":
						tempstring = "수입신고필증";
						break;
					case "2":
						tempstring = "관부가세 납부영수증";
						break;
					case "3":
						tempstring = "관세사비 세금계산서";
						break;
					case "4":
						tempstring = "수입 세금계산서";
						break;
					default:
						tempstring = "기타";
						break;
				}
				int gubuncl = Int32.Parse(RS["GubunCL"] + "");
				string[] temp = new string[] {
				RS["ClearancedFilePk"]+"",
				"ClearancedFile",
				tempstring
			};
				filelist.Append(string.Format("<tr style=\"height:20px; \">" +
					"<td class='TBody1G' colspan='5' style=\"text-align:left; padding-left:20px; \" ><a href='../UploadedFiles/FileDownload.aspx?S={0}&T={1}' ><span style=\"cursor:hand;\">{2}</a></a></td></tr>", temp));
			}
			RS.Dispose();
		}

		string QueryGubun = SorC == "S" ? "12, 13, 16, 17" : "14, 15, 16, 17";
		string deletebtn = "";

		string fileROW = "<tr style=\"height:20px; \">" +
				"<td class='{7}' >{2}</td>" +
				"<td class='{7}' style=\"text-align:left;\"><a href='../UploadedFiles/FileDownload.aspx?S={0}&T={1}' >{3}</a></td>" +
				"<td class='{7}' >{4}</td>" +
				"<td class='{7}' >{5}</td>" +
				"<td class='{7}' >{6}</td></tr>";


		DB.SqlCmd.CommandText = "SELECT [FilePk], [Title], [GubunCL], [FileName], [AccountID], [Registerd] FROM [INTL2010].[dbo].[File] WHERE GubunCL in (" + QueryGubun + ") and GubunPk=" + RequestFormPk + ";";
		RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			int gubuncl = Int32.Parse(RS["GubunCL"] + "") - 10;
			if (SorC == "S") {
				deletebtn = "<span onclick=\"SetFileGubunCL('" + RS["FilePk"] + "', '" + (gubuncl + 8) + "')\" style='color:red;'>X</span>";
			} else {
				deletebtn = "<span onclick=\"SetFileGubunCL('" + RS["FilePk"] + "', '" + (gubuncl + 6) + "')\" style='color:red;'>X</span>";
			}

			string[] temp = new string[] {
				RS["FilePk"]+"",
				"File",
				(RS["Title"]+""==""?"&nbsp;":RS["Title"]+""),
				RS["FileName"]+"",
				RS["AccountID"]+"",
				(RS["Registerd"]+"").Substring(2, (RS["Registerd"]+"").Length-5),
				deletebtn,
				"TBody1"
			};
			filelist.Append(string.Format(fileROW, temp));
		}
		RS.Dispose();
		DB.DBCon.Close();

		FILELIST = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:350px;\">" +
"	<tr>" +
"		<td class='THead1' style=\"width:100px; text-align:center;\" colspan=\"2\"><input type=\"button\" value=\"File Upload\" onclick=\"GoFileupload('" + RequestPk + "', '2');\" /></td>" +
"		<td class='THead1' style=\"width:50px; text-align:center;\">ID</td>" +
"		<td class='THead1' style=\"width:130px; text-align:center;\">Registerd</td>" +
"		<td class='THead1' style=\"width:18px; text-align:center;\">D</td>" +
"	</tr><tr>" + filelist + "</table>";
	}

	private void LoadFileList_Ever(string RequestFormPk, string MemberPk) {
		StringBuilder filelist = new StringBuilder();

		string deletebtn = "";
		string fileROW = "<tr style=\"height:20px; \">" +
				"<td class='{7}' >{2}</td>" +
				"<td class='{7}' style=\"text-align:left;\"><a href='../UploadedFiles/FileDownload.aspx?S={0}&T={1}' >{3}</a></td>" +
				"<td class='{7}' >{4}</td>" +
				"<td class='{7}' >{5}</td>" +
				"<td class='{7}' >{6}</td></tr>";

		DB.SqlCmd.CommandText = "SELECT [FilePk], [Title], [GubunCL], [FileName], [AccountID], [Registerd] FROM [INTL2010].[dbo].[File] WHERE GubunCL in (99) and GubunPk=" + companypk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			int gubuncl = Int32.Parse(RS["GubunCL"] + "") - 10;
			if (SorC == "S") {
				deletebtn = "<span onclick=\"SetFileGubunCL('" + RS["FilePk"] + "', '" + (gubuncl + 8) + "')\" style='color:red;'>X</span>";
			}
			else {
				deletebtn = "<span onclick=\"SetFileGubunCL('" + RS["FilePk"] + "', '" + (gubuncl + 6) + "')\" style='color:red;'>X</span>";
			}

			string[] temp = new string[] {
				RS["FilePk"]+"",
				"File",
				(RS["Title"]+""==""?"&nbsp;":RS["Title"]+""),
				RS["FileName"]+"",
				RS["AccountID"]+"",
				(RS["Registerd"]+"").Substring(2, (RS["Registerd"]+"").Length-5),
				deletebtn,
				"TBody1"
			};
			filelist.Append(string.Format(fileROW, temp));
		}
		RS.Dispose();
		DB.DBCon.Close();

		CLEARANCELIST = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:350px; margin-top:10px;\">" +
"	<tr>" +
"		<td class='THead1' style=\"width:100px; text-align:center;\" colspan=\"2\"><input type=\"button\" value=\"ClearanceDocu Upload\" onclick=\"GoFileupload('" + companypk + "', '99');\" /></td>" +
"		<td class='THead1' style=\"width:50px; text-align:center;\">ID</td>" +
"		<td class='THead1' style=\"width:130px; text-align:center;\">Registerd</td>" +
"		<td class='THead1' style=\"width:18px; text-align:center;\">D</td>" +
"	</tr><tr>" + filelist + "</table>";
	}
}