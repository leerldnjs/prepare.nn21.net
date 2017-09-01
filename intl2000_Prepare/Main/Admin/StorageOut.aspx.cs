using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Admin_StorageOut : System.Web.UI.Page
{
	protected String[] MEMBERINFO;
	protected String Gubun;
	protected Int32 PageNo;
	protected Int32 TotalRecord;
	protected String StorageCode;
	protected String HTMLHeader;
	protected String HTMLBody;
	protected String HTMLDriver;
	private DBConn DB;
	protected Int32 ChkCount = -1;
	protected String TodayAll;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("../Default.aspx");
		}
		try {
			if (Request["Language"].Length == 2) {
				Session["Language"] = Request["Language"];
			}
		} catch (Exception) {
		}
		switch (Session["Language"] + "") {
			case "en":
				Page.UICulture = "en";
				break;
			case "ko":
				Page.UICulture = "ko";
				break;
			case "zh":
				Page.UICulture = "zh-cn";
				break;
		}
		MEMBERINFO = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		if (MEMBERINFO[2] == "ilic31" || MEMBERINFO[2] == "ilic06" || MEMBERINFO[2] == "ilic07" || MEMBERINFO[2] == "ilic08") {
			new Ready().Check_ClearanceStatus_FromReadyKorea();
		}

		StorageCode = Request.Params["S"] + "" == "" ? "0" : Request.Params["S"] + "";
		PageNo = Request.Params["PageNo"] + "" == "" ? 1 : Int32.Parse(Request.Params["PageNo"] + "");
		TodayAll = Request.Params["TD"];
		DB = new DBConn();

		if (MEMBERINFO[0] == "Customs") {
			LogedWithoutRecentRequest111.Visible = false;
			Loged1.Visible = true;
		} else if (MEMBERINFO[0] == "OurBranchOut") {
			LogedWithoutRecentRequest111.Visible = false;
			Loged2.Visible = true;
		}

		HTMLHeader = LoadHeader(StorageCode, MEMBERINFO[1]);
		LoadAleadySetDriver(StorageCode, MEMBERINFO[1], TodayAll);
		HTMLBody = LoadBody(StorageCode, MEMBERINFO[1], PageNo);
	}
	private String LoadHeader(string StorageCode, string CompanyPk) {
		string EachHeadBTN = "<input type=\"button\" {0} value=\"{1}\" onclick=\"GotoDifferantStorage('{2}');\" />&nbsp;&nbsp;";
		StringBuilder ReturnValue = new StringBuilder();
		string BTNToday = string.Format(EachHeadBTN, StorageCode == "0" ? "style=\"font-weight:bold;\"" : "", "TOTAL", "0");

		DB.SqlCmd.CommandText = @"	SELECT OBSC.OurBranchStoragePk, OBSC.StorageName, Ct.C 
															FROM OurBranchStorageCode AS OBSC left join ( 
																SELECT [WAREHOUSE_PK], count(*) AS C 
																FROM [dbo].[STORAGE]  
																group by [WAREHOUSE_PK] 
															) AS Ct ON OBSC.OurBranchStoragePk=Ct.[WAREHOUSE_PK]
															WHERE OBSC.OurBranchCode=" + CompanyPk + " and isnull(Ct.C, 0)<>0  ;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string tempStyle = StorageCode == RS[0] + "" ? "style=\"font-weight:bold;\"" : "";
			ReturnValue.Append(string.Format(EachHeadBTN, tempStyle, RS[1] + "(" + RS[2] + ")", RS[0]));
		}
		RS.Dispose();
		DB.DBCon.Close();
		return "<div>" + BTNToday + ReturnValue + "</div>";
	}
	private Boolean LoadAleadySetDriver(string StorageCode, string BranchPk, string TodayAll) {
		string InnerTableRowFormat = "<tr>" +
			"<td class='TBody1' >{0}</td>" +
			"<td class='TBody1' >{1}</td>" +
			"<td class='TBody1' style=\"text-align:left; padding-left:10px; \" >{2}</td>" +
			"<td class='TBody1' >{5}</td>" +

			"<td class='TBody1' >{8}</td>" +
			"<td class='TBody1' >{9}</td>" +
			"<td class='TBody1' >{10}</td>" +
			"<td class='TBody1' >{11}</td>" +

			"<td class='TBody1' >{3}</td>" +
			"<td class='TBody1' >{4}</td>" +
			"<td class='TBody1' >{6}</td>" +
			"<td class='TBody1' >{7}</td></tr>";
		string InnerTableFormat = "<fieldset style=\"width:960px; margin-left:10px; margin-right:10px; \">" +
					"<legend><strong>{0}</strong>&nbsp;&nbsp;&nbsp;<input type=\"button\" value=\"List Down\" onclick=\"DeliveryListExcelDown('{0}', '" + BranchPk + "');\"  /></legend>" +
					"<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"padding:3px; width:960px; \"><tr>" +
					"	<td style=\"width:80px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:80px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:90px; font-size:1px; \">&nbsp;</td>" +

					"	<td style=\"width:70px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:70px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:70px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:70px; font-size:1px; \">&nbsp;</td>" +

					"	<td style=\"width:30px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:30px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:90px; font-size:1px; \">&nbsp;</td>" +
					"	<td style=\"width:50px; font-size:1px; \">&nbsp;</td></tr>{1}</table></fieldset>";
		string tempStorageCode = StorageCode == "0" ? "" : " and OBS.StorageCode=" + StorageCode + " ";
		if (TodayAll == "2") {
			tempStorageCode = tempStorageCode + " and TBC.ToDate='" + DateTime.Today.ToString("yyyyMMdd") + "'";
		}

		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"	
DECLARE @BranchCode int;
SET @BranchCode=" + BranchPk + @";
SELECT 
	OBS.[STORAGE_PK] AS STORAGE_PK, OBS.[REQUEST_PK], OBS.[PACKED_COUNT], TBH.[TRANSPORT_STATUS]
	, OBSC.StorageName, Consignee.CompanyPk  
	, TBH.[DATETIME_TO]
	, Consignee.CompanyName AS ConsigneeName, Consignee.CompanyNamee AS ConsigneeNamee
	, R.ShipperPk, R.ConsigneePk, R.ConsigneeCode, R.StepCL, R.DocumentStepCL
	, R.TotalPackedCount, R.PackingUnit, R.[ExchangeDate], RFCH.[MONETARY_UNIT], RFCH.[TOTAL_PRICE], (isnull(RFCH.[DEPOSITED_PRICE], 0)-isnull(RFCH.[TOTAL_PRICE], 0)) AS LeftCharge
	, RFCH.[BRANCH_BANK_PK], RFCH.[CUSTOMER_BANK_PK]
	, RFCH.[CUSTOMER_BANK_PK], R.[TotalGrossWeight],R.[TotalVolume]
	, CD.CommercialDocumentHeadPk
	, TBH.[TRANSPORT_PK], TBH.[VOYAGE_NO], TBH.[TITLE], TBH.[VESSELNAME], TBH.[VALUE_STRING_1], TBH.[DATETIME_FROM], OBS.[PACKED_COUNT], OBS.[PACKING_UNIT], '' AS Memo 
FROM [dbo].[STORAGE] AS OBS 
	left join OurBranchStorageCode AS OBSC ON OBS.[WAREHOUSE_PK]=OBSC.OurBranchStoragePk
	left join [dbo].[TRANSPORT_HEAD] AS TBH ON TBH.[TRANSPORT_PK]=OBS.[TRANSPORT_HEAD_PK]
	left join RequestForm AS R ON OBS.[REQUEST_PK]=R.RequestFormPk 
	left join Company AS Consignee ON Consignee.CompanyPk=R.ConsigneePk 
	--left join (SELECT [RequestFormPk], [Price], [StandardPriceHeadPkNColumn] FROM [RequestFormCalculateBody] where [StandardPriceHeadPkNColumn]='D') AS RFCB ON RFCB.RequestFormPk=OBS.[REQUEST_PK]
	left join [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON RFCH.[TABLE_PK]=OBS.[REQUEST_PK]
	--left join (SELECT [GubunPk], SUM([Value]) AS TSum FROM [CommercialDocumentTariff] Group By [GubunPk]) AS Tariff ON Tariff.GubunPk=OBS.[REQUEST_PK]
	left join CommerdialConnectionWithRequest AS CCWR ON CCWR.RequestFormPk=OBS.[REQUEST_PK]
	left join CommercialDocument AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
WHERE OBSC.OurBranchCode=@BranchCode " + tempStorageCode + @" 
AND TBH.[TRANSPORT_WAY] = 'Delivery'
AND RFCH.[TABLE_NAME] = 'RequestForm'
AND ( R.DocumentStepCL in (5, 6, 7, 8, 9, 10, 11, 12) or isnull(TBH.[TRANSPORT_STATUS], 0)<6 ) 
AND isnull(TBH.[VOYAGE_NO], '')<>'' 
UNION 
SELECT 
	OBS.[TRANSPORT_BODY_PK] AS STORAGE_PK, OBS.[REQUEST_PK], OBS.[PACKED_COUNT], TBH.[TRANSPORT_STATUS]
	, OBSC.StorageName, Consignee.CompanyPk 
	, TBH.[DATETIME_TO]
	, Consignee.CompanyName AS ConsigneeName, Consignee.CompanyNamee AS ConsigneeNamee
	, R.ShipperPk, R.ConsigneePk, R.ConsigneeCode, R.StepCL, R.DocumentStepCL
	, R.TotalPackedCount, R.PackingUnit, R.[ExchangeDate], RFCH.[MONETARY_UNIT], RFCH.[TOTAL_PRICE], (isnull(RFCH.[DEPOSITED_PRICE], 0)-isnull(RFCH.[TOTAL_PRICE], 0)) AS LeftCharge
	, RFCH.[BRANCH_BANK_PK], RFCH.[CUSTOMER_BANK_PK]
	, RFCH.[CUSTOMER_BANK_PK], R.[TotalGrossWeight],R.[TotalVolume]
	, CD.CommercialDocumentHeadPk
	, TBH.[TRANSPORT_PK], TBH.[VOYAGE_NO], TBH.[TITLE], TBH.[VESSELNAME], TBH.[VALUE_STRING_1], TBH.[DATETIME_FROM], OBS.[PACKED_COUNT], OBS.[PACKING_UNIT], '' AS Memo 
FROM [dbo].[TRANSPORT_BODY] AS OBS 
	left join OurBranchStorageCode AS OBSC ON OBS.[WAREHOUSE_PK_DEPARTURE]=OBSC.OurBranchStoragePk
	left join [dbo].[TRANSPORT_HEAD] AS TBH ON TBH.[TRANSPORT_PK]=OBS.[TRANSPORT_HEAD_PK]
	left join RequestForm AS R ON OBS.[REQUEST_PK]=R.RequestFormPk 
	left join Company AS Consignee ON Consignee.CompanyPk=R.ConsigneePk 
	--left join (SELECT [RequestFormPk], [Price], [StandardPriceHeadPkNColumn] FROM [RequestFormCalculateBody] where [StandardPriceHeadPkNColumn]='D') AS RFCB ON RFCB.RequestFormPk=OBS.[REQUEST_PK]
	left join [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON RFCH.[TABLE_PK]=OBS.[REQUEST_PK]
	--left join (SELECT [GubunPk], SUM([Value]) AS TSum FROM [CommercialDocumentTariff] Group By [GubunPk]) AS Tariff ON Tariff.GubunPk=OBS.[REQUEST_PK]
	left join CommerdialConnectionWithRequest AS CCWR ON CCWR.RequestFormPk=OBS.[REQUEST_PK]
	left join CommercialDocument AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
WHERE OBSC.OurBranchCode=@BranchCode " + tempStorageCode + @" 
AND TBH.[TRANSPORT_WAY] = 'Delivery'
AND RFCH.[TABLE_NAME] = 'RequestForm'
AND ( R.DocumentStepCL in (5, 6, 7, 8, 9, 10, 11, 12) or isnull(TBH.[TRANSPORT_STATUS], 0)<6 ) 
AND isnull(TBH.[VOYAGE_NO], '')<>'' 
ORDER BY TBH.[VOYAGE_NO] ASC, TBH.[TITLE] ASC, Consignee.CompanyPk, TBH.[TRANSPORT_STATUS] ASC;";

		//Response.Write(DB.SqlCmd.CommandText);
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder returnvalue = new StringBuilder();
		StringBuilder innertable = new StringBuilder();
		string[] innertableData;
		string TempType = "#@!";
		string TempTitle = "#@!";

		while (RS.Read()) {

			if (TempType == "#@!") {
				TempType = RS["VOYAGE_NO"] + "";
			} else if (TempType != RS["VOYAGE_NO"] + "") {
				returnvalue.Append(string.Format(InnerTableFormat, TempType, innertable));
				innertable = new StringBuilder();
				TempType = RS["VOYAGE_NO"] + "";
			}

			if (TempTitle != RS["TITLE"] + "") {
				TempTitle = RS["TITLE"] + "";
				innertable.Append("<tr><td colspan=\"12\" class='THead1' style=\"text-align:left; font-weight:bold; padding:5px; \" >&nbsp;&nbsp;" + TempTitle + "</td></tr>");
			}

			innertableData = new string[12];
			innertableData[0] = RS["DATETIME_TO"] + "" == "" ? "" : RS["DATETIME_TO"].ToString().Substring(0, RS["DATETIME_TO"].ToString().LastIndexOf("."));
			innertableData[1] = RS["StorageName"] + "";
			innertableData[2] = "<a onclick=\"Goto('RequestForm', '" + RS["REQUEST_PK"] + "');\"><span style=\"font-weight:bold; cursor:hand;\">" + RS["ConsigneeCode"] + "</span> " + RS["ConsigneeName"] + "/" + RS["ConsigneeNamee"] + "</a>";
			
			if (RS["TOTAL_PRICE"] + "" == "0.0000") {
				innertableData[3] = "--";
			} else {
				//----------------------------------------------------
				if (RS["TOTAL_PRICE"] + "" == "0.0000") {
					string ShipperMonetaryUnit = RS["MONETARY_UNIT"] + "";
					string ExchangeDate = RS["ExchangeDate"] + "";
					string tempExchangedDate = "";
					if (ExchangeDate != "") {
						tempExchangedDate = ExchangeDate;
					}
					string temp = "";
				}


				decimal templeft = RS["LeftCharge"] + "" == "" ? 0 : decimal.Parse(RS["LeftCharge"] + "");

				if (templeft < 0) {
					templeft *= -1;
				}
				int limit;
				switch (RS["MONETARY_UNIT"] + "") {
					case "20":
						limit = 1000;
						break;
					default:
						limit = 10;
						break;
				}


				if (templeft < limit) {
					innertableData[3] = "<span onclick=\"CollectPayment('" + RS["REQUEST_PK"] + "', '" + MEMBERINFO[2] + "', 'S', '" + RS["ShipperPk"] + "');\" style=\"cursor:hand;\" >" +
									"		<img src=\"../Images/CheckTrue.jpg\" style=\"width:20px;\" alt=\"\" /></span>";
				} else {
					innertableData[3] = "<span onclick=\"CollectPayment('" + RS["REQUEST_PK"] + "', '" + MEMBERINFO[2] + "', 'S', '" + RS["ShipperPk"] + "');\" style=\"cursor:hand;\" >" +
									"		<img src=\"../Images/CheckFalse.jpg\" style=\"width:20px;\" alt=\"\" />" +
									"	</span>";
				}
			}

			if (RS["TOTAL_PRICE"] + "" == "0.0000") {
				innertableData[4] = "--";
			} else {
				decimal templeft = RS["LeftCharge"] + "" == "" ? 0 : decimal.Parse(RS["LeftCharge"] + "");
				if (templeft < 0) {
					templeft *= -1;
				}

				int limit;
				switch (RS["MONETARY_UNIT"] + "") {
					case "20":
						limit = 1000;
						break;
					default:
						limit = 10;
						break;
				}
				if (templeft < limit) {
					innertableData[4] = "<span onclick=\"CollectPayment('" + RS["REQUEST_PK"] + "', '" + MEMBERINFO[2] + "', 'C', '" + RS["ConsigneePk"] + "');\" style=\"cursor:hand;\" >" +
									"		<img src=\"../Images/CheckTrue.jpg\" style=\"width:20px;\" alt=\"\" /></span>";
				} else {
					innertableData[4] = "<span onclick=\"CollectPayment('" + RS["REQUEST_PK"] + "', '" + MEMBERINFO[2] + "', 'C', '" + RS["ConsigneePk"] + "');\" style=\"cursor:hand;\" >" +
									"		<img src=\"../Images/CheckFalse.jpg\" style=\"width:20px;\" alt=\"\" />" +
									"	</span>";
				}
			}

			innertableData[5] = "<span style=\"cursor:hand;\" onclick=\"DeliveryModify('" + RS["STORAGE_PK"] + "','" + RS["REQUEST_PK"] + "' , '" + RS["ConsigneePk"] + "', '" + MEMBERINFO[1] + "');\" >  " +
						RS["PACKED_COUNT"] + " / " + RS["TotalPackedCount"] + " " + Common.GetPackingUnit(RS["PackingUnit"] + "") + "</span>";

			string tempdocument;
			switch (RS["DocumentStepCL"] + "") {
				case "1":
					tempdocument = "BL생성";
					break;
				case "2":
					tempdocument = "세금책정중";
					break;  //통관비 확정
				case "3":
					tempdocument = "자가통관";
					break;
				case "4":
					tempdocument = "Sample";
					break;
				case "5":
					tempdocument = "<input type=\"button\" value=\"통관지시\" id=\"BTN" + RS["CommercialDocumentHeadPk"] + "\" onclick=\"DocumentStepCLTo(this.id, '6', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
					break;  //통관비 확정
				case "6":
					tempdocument = "<input type=\"button\" value=\"략\" style=\"padding:0px;\" id=\"BTN" + RS["CommercialDocumentHeadPk"] + "\" onclick=\"DocumentStepCLTo(this.id, '7', '" + RS["CommercialDocumentHeadPk"] + "')\" />" +
			"<input type=\"button\" value=\"서\" style=\"padding:0px;\" id=\"BTN" + RS["CommercialDocumentHeadPk"] + "0\" onclick=\"DocumentStepCLTo(this.id, '8', '" + RS["CommercialDocumentHeadPk"] + "')\" />" +
			"<input type=\"button\" value=\"검\" style=\"padding:0px;\" id=\"BTN" + RS["CommercialDocumentHeadPk"] + "00\" onclick=\"DocumentStepCLTo(this.id, '9', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
					break;  //통관지시
				case "7":
					tempdocument = "생략 <input type=\"button\" value=\"세납\" id=\"BTN" + RS["CommercialDocumentHeadPk"] + "\" onclick=\"DocumentStepCLTo(this.id, '10', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
					break;
				case "8":
					tempdocument = "서류 <input type=\"button\" value=\"세납\" id=\"BTN" + RS["CommercialDocumentHeadPk"] + "\" onclick=\"DocumentStepCLTo(this.id, '11', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
					break;
				case "9":
					tempdocument = "검사 <input type=\"button\" value=\"세납\" id=\"BTN" + RS["CommercialDocumentHeadPk"] + "\" onclick=\"DocumentStepCLTo(this.id, '12', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
					break;
				case "10":
					tempdocument = "세납 <input type=\"button\" value=\"면허\" id=\"BTN" + RS["CommercialDocumentHeadPk"] + "\" onclick=\"DocumentStepCLTo(this.id,'13', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
					break;
				case "11":
					tempdocument = "세납 <input type=\"button\" value=\"면허\" id=\"BTN" + RS["CommercialDocumentHeadPk"] + "\" onclick=\"DocumentStepCLTo(this.id,'14', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
					break;
				case "12":
					tempdocument = "세납 <input type=\"button\" value=\"면허\" id=\"BTN" + RS["CommercialDocumentHeadPk"] + "\" onclick=\"DocumentStepCLTo(this.id,'15', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
					break;
				case "13":
					tempdocument = "면허완료";
					break;
				case "14":
					tempdocument = "면허완료";
					break;
				case "15":
					tempdocument = "면허완료";
					break;
				default:
					tempdocument = "미확정";
					break;
			}
			innertableData[6] = tempdocument;
			innertableData[7] = RS["TRANSPORT_STATUS"] + "" == "6" ?
				"&nbsp;<span style=\"color:green;\">출고끝</span>" :
				"<input type=\"button\" value=\"出\" style=\"padding:0px; width:30px; \" id='BTN_GoDeliveryOrder" + RS["TRANSPORT_PK"] + "' onclick=\"GoDeliveryOrder('" + RS["TRANSPORT_PK"] + "' , '" + RS["REQUEST_PK"] + "', '" + MEMBERINFO[2] + "', '" + MEMBERINFO[1] + "');\" />";
			innertableData[8] = Common.NumberFormat(RS["TotalGrossWeight"].ToString()) + " Kg";
			innertableData[9] = Common.NumberFormat(RS["TotalVolume"].ToString()) + " CBM";
			innertableData[10] = RS["VESSELNAME"] + "";
			innertableData[11] = RS["VALUE_STRING_1"] + "";
			innertable.Append(String.Format(InnerTableRowFormat, innertableData));
			if (RS["Memo"] + "" != "") {
				innertable.Append("<tr>" +
			"<td class='TBody1' colspan='2' >&nbsp;</td>" +
			"<td class='TBody1' style=\"text-align:left; padding-left:10px; color:darkblue;\" colspan='10' >" + RS["Memo"] + "</td></tr>");
			}
		}
		if (innertable + "" != "") {
			returnvalue.Append(string.Format(InnerTableFormat, TempType, innertable));
		}
		RS.Dispose();
		DB.DBCon.Close();
		HTMLDriver = returnvalue + "";
		return true;
	}
	private String LoadBody(string StorageCode, string CompanyPk, int PageNo) {
		int EachPageRowCount = 15;
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("	<table border='0' cellpadding='0' cellspacing='0' style='width:1050px;' ><thead><tr style=\"height:40px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; width:40px; \" >F</td>" +
										"		<td class='THead1' style='width:125px;'>BLNo</td>" +
										"		<td class='THead1' style='width:55px;' >s</td>" +
										"		<td class='THead1' style='width:55px;' >c</td>" +
										"		<td class='THead1' style='width:43px;'>Arrival</td>" +
										"		<td class='THead1' style='width:200px;'>CompanyName</td>" +
										"		<td class='THead1' style='width:100px;'>CT / Total</td>" +
										"		<td class='THead1' style='width:50px;'>Weight</td>" +
										"		<td class='THead1' style='width:50px;'>Volume</td>" +
										"		<td class='THead1' style='width:25px;'>SP</td>" +
										"		<td class='THead1' >CP</td>" +
										"		<td class='THead1' style='width:180px;'>출고지</td>" +
										"		<td class='THead1' style='width:75px;'>세관</td>" +
										"	</tr></thead>");
		string EachGroup = "	<tr style=\"height:20px;\"><td class='TBody1' style=\"text-align:left; \" colspan='13'  >{0}</td></tr>";
		string EachRow = "	<tr style=\"height:20px;\">" +
									"		<td class='TBody1' style=\"text-align:center; \" ><a onclick=\"Goto('CheckDescription','{2}');\">{4}</a></td>" +
									"		<td class='TBody1' ><a onclick=\"Goto('CheckDescription','{2}');\">{8}</a></td>" +
									"		<td class='TBody1' ><a onclick=\"Goto('Company', '{0}');\">{5}</a></td>" +
									"		<td class='TBody1' ><a onclick=\"Goto('Company', '{1}');\">{6}</a></td>" +
									"		<td class='TBody1' >{7}</td>" +
									"		<td class='TBody1' ><a onclick=\"Goto('RequestForm', '{3}');\"><span style=\"cursor:hand; font-weight:bold;\">{9}</span></a></td>" +
									"		<td class='TBody1' {17} >{10}</td>" +
									"		<td class='TBody1' {17} >{11}</td>" +
									"		<td class='TBody1' {17} >{12}</td>" +
									"		<td class='TBody1' >{13}</td>" +
									"		<td class='TBody1' >{14}</td>" +
									"		<td class='TBody1' style=\"text-align:left;\" >{16}</td>" +
									"		<td class='TBody1' >{15}</td>" +
									"	</tr>";
		if (StorageCode == "0") {
			DB.SqlCmd.CommandText = @"
	declare @AA int;
SELECT @AA=count(*)
FROM OurBranchStorageOut AS OBS 
	left join OurBranchStorageCode AS OBSC ON OBS.StorageCode=OBSC.OurBranchStoragePk
	left join TransportBBMain AS TBH ON TBH.TBBPk=OBS.TransportBetweenBranchPk 
	left join RequestForm AS R On OBS.RequestFormPk=R.RequestFormPk
WHERE OBS.StatusCL>3 and OBSC.OurBranchCode=" + CompanyPk + @" and OBS.BoxCount>0 and ( R.DocumentStepCL in (5, 6, 7, 8, 9, 10, 11, 12) or isnull(OBS.StatusCL, 0)<6 )
group by TBH.TBBPk; 
SELECT @@RowCount;";
		} else {
			DB.SqlCmd.CommandText = @"
	declare @aa int; 
	SELECT @aa=count(*)
	FROM OurBranchStorageOut AS OBS 
		left join OurBranchStorageCode AS OBSC ON OBS.StorageCode=OBSC.OurBranchStoragePk 
		left join TransportBBMain AS TBH ON TBH.TBBPk=OBS.TransportBetweenBranchPk 
		left join RequestForm AS R On OBS.RequestFormPk=R.RequestFormPk
	WHERE OBS.StatusCL>3 and OBS.StorageCode=" + StorageCode + @" and OBS.BoxCount>0 and ( R.DocumentStepCL in (5, 6, 7, 8, 9, 10, 11, 12) or isnull(OBS.StatusCL, 0)<6 )
	GROUP BY OBS.TransportBetweenBranchPk ;	
	SELECT @@ROWCOUNT; ";
		}
		DB.DBCon.Open();
		TotalRecord = Int32.Parse(DB.SqlCmd.ExecuteScalar() + "");
		//Response.Write(TotalRecord + "_" + StorageCode);
		//if (StorageCode == "0") {
		//    //DB.SqlCmd.CommandText = "EXEC SP_PrepareDeliveryListByBranchPk @BranchPk=" + CompanyPk + ";";
		//} else {
		//    DB.SqlCmd.CommandText = "EXEC SP_PrepareDeliveryListByStorageCode @StorageCode=" + StorageCode + ";";
		//}
		string tempStorageCode = StorageCode == "0" ? "" : " and OBS.StorageCode=" + StorageCode + " ";
		DB.SqlCmd.CommandText = @"	
DECLARE @BranchPk int;
SET @BranchPk=" + CompanyPk + @";
SELECT OBS.OurBranchStorageOutPk, OBS.StorageCode, OBS.RequestFormPk, OBS.BoxCount, OBS.StockedDate, OBS.TransportBetweenBranchPk, OBS.StatusCL, OBS.Comment  
		, OBSC.OurBranchStoragePk, OBSC.OurBranchCode, OBSC.StorageName, OBSC.StorageAddress, OBSC.TEL, OBSC.FAX 
		, TBH.[TRANSPORT_PK], TBH.[TRANSPORT_WAY], TBH.[VALUE_STRING_0], TBH.[BRANCHPK_FROM], TBH.[BRANCHPK_TO], TBH.[DATETIME_FROM], TBH.[DATETIME_TO], TBH.[VESSELNAME]
		, TBH.[VALUE_STRING_0], TBH.[VOYAGE_NO], TBH.[VALUE_STRING_1], TBH.[VALUE_STRING_2], TBH.[VALUE_STRING_3], TBP.[SEAL_NO]
		, FromBranch.CompanyName AS FromBranchName, Consignee.CompanyName AS ConsigneeName, Consignee.CompanyNamee AS ConsigneeNamee
		, R.ShipperPk, R.ConsigneePk, R.ShipperCode, R.ConsigneeCode, R.DepartureDate, R.ArrivalDate, R.DepartureRegionCode, R.ArrivalRegionCode, R.TransportWayCL, R.DocumentRequestCL, R.MonetaryUnitCL, R.StepCL, R.DocumentStepCL, R.ShipperSignID, R.ShipperSignDate, R.ConsigneeSignID, R.ConsigneeSignDate 
		, DRC.Name AS DRCName
		, R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume, RFCH.[ShipperMonetaryUnit], RFCH.[ExchangeRate], RFCH.[ShipperCharge], (isnull(RFCH.[ShipperDeposited], 0)-RFCH.[ShipperCharge]-isnull(RFCB.[Price], 0)) AS Shipperleft, RFCH.[ConsigneeMonetaryUnit], RFCH.[ConsigneeCharge], (isnull(RFCH.[ConsigneeDeposited], 0)-RFCH.[ConsigneeCharge]-isnull(RFCB.[Price], 0)) AS Consigneeleft, RFCH.[ConsigneeBankAccountPk], RFCH.[WillPayTariff]
		, RFCB.Price AS DCPrice, RFCB.StandardPriceHeadPkNColumn
		, Tariff.TSum
		, CD.CommercialDocumentHeadPk, CD.BLNo AS HOUSEBL, CD.InvoiceNo, CD.Shipper, CD.ShipperAddress, CD.Consignee, CD.ConsigneeAddress, CD.NotifyParty, CD.NotifyPartyAddress, CD.PortOfLoading, CD.FinalDestination, CD.Carrier, CD.SailingOn, CD.PaymentTerms, CD.OtherReferences, CD.StampImg, CD.FOBorCNF, CD.VoyageNo, CD.VoyageCompany, CD.ContainerNo, CD.SealNo, CD.ContainerSize, CD.Registerd, CD.ClearanceDate, CD.StepCL AS CDStepCL
		, TBC.TransportBetweenCompanyPk, TBC.CompanyPk, TBC.Type, TBC.Title, TBC.DriverName, TBC.DriverTEL, TBC.FromDate, TBC.WarehouseInfo, TBC.PackedCount, TBC.PackingUnit, TBC.DepositWhere, TBC.Price, TBC.Memo, H.HighlighterPk, H.Color , DaeNap.AccountID AS DaeNapID
	FROM OurBranchStorageOut AS OBS 
		left join OurBranchStorageCode AS OBSC ON OBS.StorageCode=OBSC.OurBranchStoragePk
		left join [dbo].[TRANSPORT_HEAD] AS TBH ON TBH.[TRANSPORT_PK]=OBS.TransportBetweenBranchPk 
		left join Company AS FromBranch ON FromBranch.CompanyPk=TBH.[BRANCHPK_FROM] 
		left join RequestForm AS R ON OBS.RequestFormPk=R.RequestFormPk 
		left join Company AS Consignee ON Consignee.CompanyPk=R.ConsigneePk 
		left join (SELECT [RequestFormPk], [Price], [StandardPriceHeadPkNColumn] FROM [RequestFormCalculateBody] where [StandardPriceHeadPkNColumn]='D') AS RFCB ON OBS.RequestFormPk=RFCB.RequestFormPk 
		left join RegionCode AS DRC ON R.DepartureRegionCode=DRC.RegionCode 
		left join RequestFormCalculateHead AS RFCH ON RFCH.RequestFormPk=OBS.RequestFormPk 
		left join (SELECT [GubunPk], SUM([Value]) AS TSum FROM [CommercialDocumentTariff] Group By [GubunPk]) AS Tariff ON Tariff.GubunPk=OBS.RequestFormPk 
		left join CommerdialConnectionWithRequest AS CCWR ON CCWR.RequestFormPk=OBS.RequestFormPk
		left join CommercialDocument AS CD ON CCWR.CommercialDocumentPk=CD.CommercialDocumentHeadPk 
		left join TransportBC AS TBC ON TBC.TransportBetweenCompanyPk=OBS.TransportBetweenCompanyPk 
		left join [dbo].[TRANSPORT_PACKED] AS TBP ON TBH.[TRANSPORT_PK] = TBP.[TRANSPORT_HEAD_PK]
		left join (
				SELECT [HighlighterPk], [GubunPk], [Color] FROM Highlighter WHERE GubunCL=1 
			) AS H ON CCWR.CommercialDocumentPk=H.GubunPk 
		left join RequestFormHold AS DaeNap ON R.RequestFormPk=DaeNap.RequestFormPk 
	WHERE OBSC.OurBranchCode=@BranchPk 
	AND OBS.BoxCount>0 " + tempStorageCode + @" 
	AND ( R.DocumentStepCL in (5, 6, 7, 8, 9, 10, 11, 12) or isnull(OBS.StatusCL, 0)<6 )	
	AND isnull(TBC.Type, '')=''
	ORDER BY TBH.[DATETIME_TO] DESC, TBH.[TRANSPORT_PK] ASC, CD.BLNo ASC , StaTusCL ASC;";

		tempStorageCode = "";
		string tempTransportBetweenBranchPk = "";
		object[] rowValue = new object[18];
		string tempRequestFormPk = "";
		string tempCommercialDocumentHeadPk = "";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		int rowcount = 0;

		StringBuilder DeliveryList = new StringBuilder();

		string tempStorageTitle = "";
		string tempContainerTitle = "";
		bool isDeposited = true;
		bool isContinue = true;
		int nowTempPage = 1;

		while (true) {
			if (PageNo == nowTempPage) {
				break;
			}

			int temprowcount = 0;
			while (RS.Read()) {


				if (tempTransportBetweenBranchPk != RS["TransportBetweenBranchPk"] + "") {
					tempTransportBetweenBranchPk = RS["TransportBetweenBranchPk"] + "";
					temprowcount++;

					if (temprowcount == EachPageRowCount) {
						nowTempPage++;
						if (PageNo == nowTempPage) {
							while (RS.Read()) {
								if (tempTransportBetweenBranchPk == RS["TransportBetweenBranchPk"] + "") {
									continue;
								}
								break;
							}
							isContinue = false;
						} else {
							isContinue = true;
						}
						break;
					}
				}
			}
			if (isContinue) {
				continue;
			} else {
				break;
			}
		}

		bool check = false;
		if (PageNo == 1) {
			if (RS.Read()) {
				check = true;
			}
		} else {
			check = true;
		}

		while (check) {
			//창고 타이틀
			if (StorageCode == "0") {
				if (tempStorageCode != RS["StorageCode"] + "") {
					tempStorageCode = RS["StorageCode"] + "";
					tempStorageTitle = String.Format(EachGroup, "<a onclick=\"Goto('Storage','" + tempStorageCode + "');\"><span style=\"font-size:20px; font-weight:bold; cursor:hand; padding-left:130px; \">" + RS["StorageName"] + "</span></a> " + RS["StorageAddress"] + " TEL : " + RS["TEL"] + " FAX : " + RS["FAX"]);
				} else {
					tempStorageTitle = "";
				}
			}
			//컨테이너 타이틀
			if (tempTransportBetweenBranchPk != RS["TransportBetweenBranchPk"] + "") {
				rowcount++;
				if (StorageCode != "0") {
					if (rowcount > EachPageRowCount) {
						break;
					}
				}

				string TempTitle;
				tempTransportBetweenBranchPk = RS["TransportBetweenBranchPk"] + "";
				switch (RS["TRANSPORT_WAY"] + "") {
					case "Air":
						TempTitle = "<a onclick=\"Goto('TBBPk','" + tempTransportBetweenBranchPk + "');\"><span style=\"font-size:15px; font-weight:bold; cursor:hand; padding-left:50px; \">" +
							"AIR From " + RS["FromBranchName"] + " " + (RS["DATETIME_FROM"] + "").Substring(5, 5) + "~" + (RS["DATETIME_TO"] + "").Substring(5, 5) + "</span></a>";
						break;
					case "Car":
						TempTitle = "<a onclick=\"Goto('TBBPk','" + tempTransportBetweenBranchPk + "');\"><span style=\"font-size:15px; font-weight:bold; cursor:hand; padding-left:50px; \">" +
							"Car From " + RS["FromBranchName"] + " " + (RS["DATETIME_FROM"] + "").Substring(5, 5) + "~" + (RS["DATETIME_TO"] + "").Substring(5, 5) + "</span></a>";
						break;
					case "Ship":
						string companyname = RS["FromBranchName"] + "" == "" ? "" : (RS["FromBranchName"] + "").Substring(0, 2);
						TempTitle = "<a onclick=\"Goto('TBBPk','" + tempTransportBetweenBranchPk + "');\"><span style=\"font-size:15px; font-weight:bold; cursor:hand; padding-left:50px; \">" +
							"Ship From " + companyname + " " + (RS["DATETIME_TO"] + "").Substring(5, 5) + "</span></a>&nbsp;" + RS["VALUE_STRING_0"] + " " + RS["VOYAGE_NO"] + " " + RS["SEAL_NO"] + "";
						break;
					case "OtherCompany":
						TempTitle = "<a onclick=\"Goto('TBBPk','" + tempTransportBetweenBranchPk + "');\"><span style=\"font-size:15px; font-weight:bold; cursor:hand; padding-left:50px; \">" +
							"OtherCompany From " + RS["FromBranchName"] + " " + (RS["DATETIME_FROM"] + "").Substring(5, 5) + "~" + (RS["DATETIME_TO"] + "").Substring(5, 5) + "</span></a>";
						break;
					default:
						TempTitle = "??";
						break;
				}

				tempContainerTitle = String.Format(EachGroup, TempTitle);
			} else {
				tempContainerTitle = "";
			}
			//컨테이너 타이틀

			if (tempRequestFormPk != RS["RequestFormPk"] + "") {
				isDeposited = true;
				if (tempRequestFormPk != "") {
					rowValue[16] = DeliveryList + "";
					ReturnValue.Append(String.Format(EachRow, rowValue));
				}
				ReturnValue.Append(tempStorageTitle + tempContainerTitle);
				DeliveryList = new StringBuilder();

				tempRequestFormPk = RS["RequestFormPk"] + "";
				rowValue = new object[18];
				rowValue[0] = RS["ShipperPk"] + "";
				rowValue[1] = RS["ConsigneePk"] + "";
				rowValue[2] = RS["CommercialDocumentHeadPk"] + "";
				rowValue[3] = tempRequestFormPk;
				rowValue[4] = RS["DRCName"] + "";
				rowValue[5] = RS["ShipperCode"] + "";
				rowValue[6] = RS["ConsigneeCode"] + "";
				rowValue[7] = RS["ArrivalDate"] + "" == "" ? "미지정" : string.Format("{0}/{1}", (RS["ArrivalDate"] + "").Substring(4, 2), (RS["ArrivalDate"] + "").Substring(6, 2));
				rowValue[8] = RS["HOUSEBL"] + "";
				rowValue[9] = RS["ConsigneeName"] + (RS["ConsigneeNamee"] + "" == "" ? "&nbsp;" : "/" + RS["ConsigneeNamee"]);
				rowValue[10] = RS["BoxCount"] + " / " + RS["TotalPackedCount"] + " " + Common.GetPackingUnit(RS["PackingUnit"] + "");
				rowValue[11] = Common.NumberFormat(RS["TotalGrossWeight"] + "");
				rowValue[12] = Common.NumberFormat(RS["TotalVolume"] + "");
				string Tsum = RS["TSum"] + "";
				string tempWillPayTariff = RS["WillPayTariff"] + "";
				if (RS["ShipperCharge"] + "" == "0.0000") {
					rowValue[13] = "--";
				} else {
					//----------------------------------------------------
					if (RS["ConsigneeCharge"] + "" == "0.0000") {
						string TariffMonetaryUnit = "20";
						string ShipperMonetaryUnit = RS["ShipperMonetaryUnit"] + "";
						string ExchangeRate = RS["ExchangeRate"] + "";
						decimal TariffS = 0;
						if (Tsum != "") {

							TariffS = Decimal.Parse(RS["TSum"] + "");
						}
						string tempExchangedDate = "";
						if (ExchangeRate != "") {
							string[] RowExchangeRate = ExchangeRate.Split(Common.Splite22, StringSplitOptions.RemoveEmptyEntries);
							for (int i = 0; i < RowExchangeRate.Length; i++) {
								if (tempExchangedDate == "") {
									tempExchangedDate = RowExchangeRate[i].Substring(6, 8);
									continue;
								}
								if (Int32.Parse(RowExchangeRate[i].Substring(6, 8)) > Int32.Parse(tempExchangedDate)) {
									tempExchangedDate = RowExchangeRate[i].Substring(6, 8);
								}
							}
						}
						string temp = "";
						decimal tempTariffS = new Admin().GetExchangeRated(TariffMonetaryUnit, ShipperMonetaryUnit, TariffS, out temp, tempExchangedDate);
						TariffS = Math.Round(tempTariffS, 1, MidpointRounding.AwayFromZero);
						//Response.Write(TariffS);

						Tsum = TariffS + "";

					}


					decimal templeft = RS["Shipperleft"] + "" == "" ? 0 : decimal.Parse(RS["Shipperleft"] + "");

					if (tempWillPayTariff == "S" && Tsum + "" != "") {
						templeft -= decimal.Parse(Tsum + "");
					}
					if (templeft < 0) {
						templeft *= -1;
					}
					int limit;
					switch (RS["ShipperMonetaryUnit"] + "") {
						case "20":
							limit = 1000;
							break;
						default:
							limit = 10;
							break;
					}

					if (templeft < limit) {
						rowValue[13] = "<span onclick=\"CollectPayment('" + RS["RequestFormPk"] + "', '" + MEMBERINFO[2] + "', 'S', '" + RS["ShipperPk"] + "');\" style=\"cursor:hand;\" >" +
										"		<img src=\"../Images/CheckTrue.jpg\" style=\"width:20px;\" alt=\"\" /></span>";
					} else {
						rowValue[13] = "<span onclick=\"CollectPayment('" + RS["RequestFormPk"] + "', '" + MEMBERINFO[2] + "', 'S', '" + RS["ShipperPk"] + "');\" style=\"cursor:hand;\" >" +
										"		<img src=\"../Images/CheckFalse.jpg\" style=\"width:20px;\" alt=\"\" />" +
										"	</span>";
					}
				}

				if (RS["ConsigneeCharge"] + "" == "0.0000") {
					rowValue[14] = "--";
				} else {
					decimal templeft = RS["Consigneeleft"] + "" == "" ? 0 : decimal.Parse(RS["Consigneeleft"] + "");
					if (tempWillPayTariff == "C" && RS["TSum"] + "" != "") {
						templeft -= decimal.Parse(RS["TSum"] + "");
					}
					if (templeft < 0) {
						templeft *= -1;
					}
					int limit;
					switch (RS["ConsigneeMonetaryUnit"] + "") {
						case "20":
							limit = 5000;
							break;
						default:
							limit = 10;
							break;
					}

					if (templeft < limit) {
						rowValue[14] = "<span onclick=\"CollectPayment('" + RS["RequestFormPk"] + "', '" + MEMBERINFO[2] + "', 'C', '" + RS["ConsigneePk"] + "');\" style=\"cursor:hand;\" >" +
										"		<img src=\"../Images/CheckTrue.jpg\" style=\"width:20px;\" alt=\"\" /></span>";
					} else {
						rowValue[14] = "<span onclick=\"CollectPayment('" + RS["RequestFormPk"] + "', '" + MEMBERINFO[2] + "', 'C', '" + RS["ConsigneePk"] + "');\" style=\"cursor:hand;\" >" +
										"		<img src=\"../Images/CheckFalse.jpg\" style=\"width:20px;\" alt=\"\" />" +
										"	</span>";
					}
				}


				string CDStepCL;
				if (tempCommercialDocumentHeadPk != RS["CommercialDocumentHeadPk"] + "") {
					tempCommercialDocumentHeadPk = RS["CommercialDocumentHeadPk"] + "";
					switch (RS["DocumentStepCL"] + "") {
						case "1":
							CDStepCL = "BL생성";
							break;
						case "2":
							CDStepCL = "세금책정중";
							break;  //통관비 확정
						case "3":
							CDStepCL = "자가통관";
							break;
						case "4":
							CDStepCL = "Sample";
							break;
						case "5":
							CDStepCL = "<input type=\"button\" value=\"통관지시\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id, '6', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;  //통관비 확정
						case "6":
							CDStepCL = "<input type=\"button\" value=\"략\" style=\"padding:0px;\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id, '7', '" + RS["CommercialDocumentHeadPk"] + "')\" />" +
					"<input type=\"button\" value=\"서\" style=\"padding:0px;\" id=\"BTN" + tempCommercialDocumentHeadPk + "0\" onclick=\"DocumentStepCLTo(this.id, '8', '" + RS["CommercialDocumentHeadPk"] + "')\" />" +
					"<input type=\"button\" value=\"검\" style=\"padding:0px;\" id=\"BTN" + tempCommercialDocumentHeadPk + "00\" onclick=\"DocumentStepCLTo(this.id, '9', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;  //통관지시
						case "7":
							CDStepCL = "생략 <input type=\"button\" value=\"세납\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id, '10', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;
						case "8":
							CDStepCL = "서류 <input type=\"button\" value=\"세납\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id, '11', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;
						case "9":
							CDStepCL = "검사 <input type=\"button\" value=\"세납\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id, '12', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;
						case "10":
							CDStepCL = "세납 <input type=\"button\" value=\"면허\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id,'13', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;
						case "11":
							CDStepCL = "세납 <input type=\"button\" value=\"면허\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id,'14', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;
						case "12":
							CDStepCL = "세납 <input type=\"button\" value=\"면허\" id=\"BTN" + tempCommercialDocumentHeadPk + "\" onclick=\"DocumentStepCLTo(this.id,'15', '" + RS["CommercialDocumentHeadPk"] + "')\" />";
							break;
						case "13":
							CDStepCL = "면허완료";
							break;
						case "14":
							CDStepCL = "면허완료";
							break;
						case "15":
							CDStepCL = "면허완료";
							break;
						default:
							CDStepCL = "미확정";
							break;
					}
				} else {
					CDStepCL = "``";
				}
				if (RS["DaeNapID"] + "" != "") {
					CDStepCL = "<span style=\"color:red;\">대납지시</span>";
				}
				rowValue[15] = CDStepCL;
				if (RS["HighlighterPk"] + "" == "") {
					rowValue[17] = "onclick=\"setHighlight('1', 'N', '" + RS["CommercialDocumentHeadPk"] + "');\"";
				} else {
					switch (RS["Color"] + "") {
						case "0":
							rowValue[17] = "onclick=\"setHighlight('1', 'D', '" + RS["HighlighterPk"] + "');\" style=\"background-color:#87CEFA;\" ";
							break;
						case "1":
							rowValue[17] = "onclick=\"setHighlight('1', 'D', '" + RS["HighlighterPk"] + "');\" style=\"background-color:#E9967A;\" ";
							break;
					}
				}
			}

			if (RS["TransportBetweenCompanyPk"] + "" != "") {
				if (RS["Type"] + "" == "") {
					DeliveryList.Append("<input type=\"button\" value=\"P\" style=\"padding:0px;\" onclick=\"DeliveryPrint(" + RS["TransportBetweenCompanyPk"] + ");\" />&nbsp;" +
						"<span style=\"cursor:hand;\" onclick=\"DeliveryModify('" + RS["OurBranchStorageOutPk"] + "','" + RS["RequestFormPk"] + "' , '" + RS["ConsigneePk"] + "', '" + MEMBERINFO[1] + "');\" >  " +
						RS["PackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "") + " : " +
						((RS["WarehouseInfo"] + "").Substring(0, (RS["WarehouseInfo"] + "").IndexOf("@@")).Length > 18 ? (RS["WarehouseInfo"] + "").Substring(0, 18) + "..." : (RS["WarehouseInfo"] + "").Substring(0, (RS["WarehouseInfo"] + "").IndexOf("@@"))) + "</span>	<br />");
				} else if (RS["StatusCL"] + "" == "6") {
					DeliveryList.Append("&nbsp;<span style=\"color:green;\">출고완료</span>");
				} else {
					string tempisdeposited = isDeposited ? "T" : "F";
					DeliveryList.Append("&nbsp;<span style=\"cursor:hand;\" onclick=\"DeliveryModify('" + RS["OurBranchStorageOutPk"] + "','" + RS["RequestFormPk"] + "' , '" + RS["ConsigneePk"] + "', '" + MEMBERINFO[1] + "');\" >  " +
													RS["PackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "") + " : " +
													"<strong>" + (RS["FromDate"] + "" == "" ? "" : (RS["FromDate"] + "").Substring(4, 4)) + "</strong> " + ((RS["Title"] + "").Length > 9 ? (RS["Title"] + "").Substring(0, 8) : RS["Title"] + "") +
													"</span>" +
													"	<input type=\"button\" value=\"出\" style=\"padding:0px;\" id='BTN_GoDeliveryOrder" + RS["TransportBetweenCompanyPk"] + "'  onclick=\"GoDeliveryOrder('" + RS["TransportBetweenCompanyPk"] + "' , '" + RS["RequestFormPk"] + "', '" + RS["ConsigneePk"] + "', '" + MEMBERINFO[1] + "');\" /><br />");
				}
			} else {
				DeliveryList.Append("&nbsp;<input type=\"button\" value=\"add\" onclick=\"DeliverySet('" + RS["OurBranchStorageOutPk"] + "' , '" + RS["RequestFormPk"] + "', '" + RS["ConsigneePk"] + "', '" + MEMBERINFO[1] + "');\" /><br />");
			}

			if (RS.Read()) {
				continue;
			} else {
				break;
			}
		}
		rowValue[16] = DeliveryList + "";
		ReturnValue.Append(String.Format(EachRow, rowValue));
		RS.Dispose();
		DB.DBCon.Close();
		ReturnValue.Append("<TR Height='20px'><td colspan='13' style='background-color:#F5F5F5; text-align:center; padding:20px; '>" + new Common().SetPageListByNo(EachPageRowCount, PageNo, TotalRecord, "PrepareDelivery.aspx", "?S=" + StorageCode + "&") + "</TD></TR></Table>");
		return ReturnValue + "";
	}
	
}