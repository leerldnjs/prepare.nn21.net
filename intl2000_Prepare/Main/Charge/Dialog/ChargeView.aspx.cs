using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Charge_Dialog_ChargeView :System.Web.UI.Page
{
	protected String[] MEMBERINFO;
	protected String Title;
	protected String Header;
	protected String Item;
	protected String Freight;
	protected String BankInfo;
	protected String OnlyAdmin;
	protected String StampFlag;
	protected String Stamp = "";
	protected Boolean IsEstimation;
	private DBConn DB;
	private String CalcHeadPk;
	private String CompanyPk;
	private String OurBranchPk;
	private String CompanyTEL;
	private String PresidentEmail;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("/Default.aspx");
		}
		else {
			MEMBERINFO = (Session["MemberInfo"] + "").Split(Common.Splite11, StringSplitOptions.None);
		}
		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } } catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }


		string gubun = Request.Params["T"] + "" == "" ? "" : "FreightInvoice";
		StampFlag = Request.Params["Stamp"] + "";
		DB = new DBConn();
		DB.DBCon.Open();
		
		if (Request.Params["G"] == "" || Request.Params["G"] == null || Request.Params["G"] == "0") {
			Title = "<strong style=\"font-size:30px;\">청구내역이 없습니다.</strong>";
		}
		else {
			SorCLoad(Request.Params["S"], Request.Params["G"]);
			Title = TitleLoad(Request.Params["S"]);
			Header = RequestLoad(Request.Params["S"]);
			Item = ItemLoad(Request.Params["S"]);
			Freight = FreightLoad2015(Request.Params["S"], CalcHeadPk);
		}
		DB.DBCon.Close();
	}

	private void SorCLoad(string RequestFormPk, string CalculateHeadPk) {
		CalcHeadPk = CalculateHeadPk;

		DB.SqlCmd.CommandText = "SELECT [StepCL] FROM RequestForm WHERE RequestFormPk=" + RequestFormPk;
		IsEstimation = DB.SqlCmd.ExecuteScalar() + "" == "33" ? true : false;

		if (MEMBERINFO[0] == "OurBranch") {
			asAdmin asAdmin = new asAdmin();
			if (MEMBERINFO[2] == "ilic66" || MEMBERINFO[2] == "ilic32") {
				OnlyAdmin = "<input type=\"button\" onclick=\"PnNotCalculatedTariff_hide(); \" value=\"관부가세 책정중 감추기\" />";
			}
			OnlyAdmin += asAdmin.Get_SelectEmail(RequestFormPk, CalcHeadPk);
		}
		else {
			DB.SqlCmd.CommandText = "SELECT [CUSTOMER_COMPANY_PK] FROM [dbo].[REQUESTFORMCALCULATE_HEAD] WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK] = " + RequestFormPk + " AND [REQUESTFORMCALCULATE_HEAD_PK] = " + CalcHeadPk;
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				CalcHeadPk = CalculateHeadPk;
				CompanyPk = RS["CUSTOMER_COMPANY_PK"] + "";
			}
			RS.Dispose();
		}

		if (StampFlag == "Yes") {
			Stamp = "<div style=\"position: absolute; width: 100px; height: 100px; margin-left: 590px; margin-top: 200px;\"><img src=\"/Images/Check_Back.png\" style=\"width: 75px; height: 225px;\" /></div><div style=\"position: absolute; width: 100px; height: 100px; margin-left: 590px; margin-top: 350px;\"><img src=\"/Images/Check_Stamp.png\" style=\"width: 73px; height: 74px;\" /></div>";
		}
		

	}
	private String RequestLoad(string RequestFormPk) {
		StringBuilder Html = new StringBuilder();
		DB.SqlCmd.CommandText = @"SELECT R.ShipperPk, R.ConsigneePk, R.ConsigneeCCLPk, R.ShipperCode, R.ConsigneeCode, 
		CUSTOMERC.[CompanyPk] AS CUSTOMERPK, CUSTOMERC.[CompanyCode] AS CUSTOMERCODE,
		R.DepartureDate, R.ArrivalDate, R.DepartureRegionCode, R.ArrivalRegionCode, R.TransportWayCL, R.PaymentWayCL, R.DocumentRequestCL, 
		R.MonetaryUnitCL, R.PickupRequestDate, R.NotifyPartyName, R.NotifyPartyAddress, R.Memo, 
		R.RequestDate, R.StockedDate, 
		HEAD.[BRANCH_COMPANY_PK], HEAD.[CUSTOMER_COMPANY_PK],
		ShipperC.CompanyName AS ShipperCompanyName, ShipperC.CompanyTEL AS ShipperComapnyTEL, ShipperC.CompanyFAX AS ShipperCompanyFAX,  
		ConsigneeC.CompanyName AS ConsigneeCompanyName, ConsigneeC.CompanyTEL AS ConsigneeCompanyTEL, ConsigneeC.CompanyFAX AS ConsigneeCompanyFAX,
		CUSTOMERC.CompanyName AS CUSTOMERNAME, CUSTOMERC.CompanyTEL AS CUSTOMERTEL, CUSTOMERC.CompanyFAX AS CUSTOMERFAX,  
		DepartureRegion.Name AS DepartureName , ArrivalRegion.Name AS ArrivalName, 
		(SELECT count(*) FROM RequestFormAdditionalInfo where RequestFormPk = " + RequestFormPk + @" and (GubunCL=70 or GubunCL=71)) AS ModifyCount, R.StepCL, CID.Name AS CIDName, CID.Address AS CIDAddress, 
		CD.BLNo, CD.Registerd AS InvoiceDate, 
		R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume
	FROM RequestForm AS R 
		LEFT JOIN [dbo].[REQUESTFORMCALCULATE_HEAD] AS HEAD ON R.[RequestFormPk] = HEAD.[TABLE_PK]
		left join Company AS ShipperC ON R.ShipperPk=ShipperC.CompanyPk 
		left join Company AS ConsigneeC ON R.ConsigneePk=ConsigneeC.CompanyPk
		left join Company AS CUSTOMERC ON HEAD.[CUSTOMER_COMPANY_PK]=CUSTOMERC.[CompanyPk]
		left join RegionCode AS DepartureRegion ON R.DepartureRegionCode=DepartureRegion.RegionCode 
		left join RegionCode AS ArrivalRegion ON R.ArrivalRegionCode=ArrivalRegion.RegionCode 
		left join CompanyInDocument AS CID ON R.CompanyInDocumentPk=CID.CompanyInDocumentPk 
		left join CommerdialConnectionWithRequest AS CCWR ON R.RequestFormPk=CCWR.RequestFormPk
		left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
	WHERE HEAD.[TABLE_NAME] = 'RequestForm'
	AND R.RequestFormPk = " + RequestFormPk + @"
	AND HEAD.[REQUESTFORMCALCULATE_HEAD_PK] = " + CalcHeadPk;

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			if (IsEstimation) {
				Html.Append("<div class=\"Title\">" +
	"<div style=\"border:1px black solid; padding:10px;\"><strong>" + RS["CUSTOMERNAME"] + "(" + RS["CUSTOMERCODE"] + ")</strong>&nbsp;&nbsp;&nbsp; &nbsp;TEL : " + RS["CUSTOMERTEL"] + "&nbsp;&nbsp;FAX : " + RS["CUSTOMERFAX"] + "</div><br />" +
	"<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:695px; height:50px; \">" +
	"	<tr><td>Schedule</td><td>" +
			(RS["DepartureDate"] + "" == "" ? "" : string.Format("{0}.{1}.{2}", ("" + RS["DepartureDate"]).Substring(0, 4), ("" + RS["DepartureDate"]).Substring(4, 2), ("" + RS["DepartureDate"]).Substring(6, 2))) + " - " +
			(RS["ArrivalDate"] + "" == "" ? "" : string.Format("{0}.{1}.{2}", ("" + RS["ArrivalDate"]).Substring(0, 4), ("" + RS["ArrivalDate"]).Substring(4, 2), ("" + RS["ArrivalDate"]).Substring(6, 2))) + "</td><td>" + GetGlobalResourceObject("qjsdur", "dnsthddlfwjd") + "</td><td>" + RS["DepartureName"] + " - " + RS["ArrivalName"] + " " + Common.GetTransportWay(RS["TransportWayCL"] + "") + "</td></tr>" +
	"	<tr><td>WEIGHT</td><td>" + Common.NumberFormat("" + RS["TotalGrossWeight"]) + " KG</td><td>MEASURMENT</td><td>" + Common.NumberFormat(RS["TotalVolume"] + "") + " CBM</td></tr>" +
	"	</table></div>");
			}
			else {
				Html.Append("<div class=\"Title\">" +
	"<div style=\"border:1px black solid; padding:10px;\"><strong>" + RS["ConsigneeCompanyName"] + "(" + RS["ConsigneeCode"] + ")</strong>&nbsp;&nbsp;&nbsp; &nbsp;TEL : " + RS["ConsigneeCompanyTEL"] + "&nbsp;&nbsp;FAX : " + RS["ConsigneeCompanyFAX"] + "</div><br />" +
	"<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:695px; height:70px; \">" +
	"	<tr><td colspan=\"2\">Shipper. " + RS["ShipperCompanyName"] + " (" + RS["ShipperCode"] + ")</td><td>TEL : " + RS["ShipperComapnyTEL"] + "</td><td>FAX : " + RS["ShipperCompanyFAX"] + "</td></tr>" +
	"	<tr><td>B / L NO</td><td>" + RS["BLNo"] + "</td><td>" + GetGlobalResourceObject("qjsdur", "dnsthddlfwjd") + "</td><td>" + RS["DepartureName"] + " - " + RS["ArrivalName"] + " " + Common.GetTransportWay(RS["TransportWayCL"] + "") + "</td></tr>" +
	"	<tr><td>QTY</td><td>" + RS["TotalPackedCount"] + " " + Common.GetPackingUnit(RS["PackingUnit"] + "") + "</td><td>&nbsp;</td><td>" +
			(RS["DepartureDate"] + "" == "" ? "" : string.Format("{0}.{1}.{2}", ("" + RS["DepartureDate"]).Substring(0, 4), ("" + RS["DepartureDate"]).Substring(4, 2), ("" + RS["DepartureDate"]).Substring(6, 2))) + " - " +
			(RS["ArrivalDate"] + "" == "" ? "" : string.Format("{0}.{1}.{2}", ("" + RS["ArrivalDate"]).Substring(0, 4), ("" + RS["ArrivalDate"]).Substring(4, 2), ("" + RS["ArrivalDate"]).Substring(6, 2))) + "</td></tr>" +
	"	<tr><td>WEIGHT</td><td>" + Common.NumberFormat("" + RS["TotalGrossWeight"]) + " KG</td><td>MEASURMENT</td><td>" + Common.NumberFormat(RS["TotalVolume"] + "") + " CBM</td></tr>" +
	"	</table></div>");
			}
		}
		RS.Dispose();
		return Html + "";
	}
	
	private String ItemLoad(string RequestFormPk) {
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:695px; \"><tr><td colspan='8' style=\"font-size:15px; font-weight:bold; border-bottom:solid 1px black;\" >" + GetGlobalResourceObject("qjsdur", "wjqtnaudtp") + "</td></tr><tr>" +
										"<td style=\"height:25px; text-align:center; border-left:solid 1px black; border-right:solid 1px black; border-bottom:solid 1px black;\" >" + GetGlobalResourceObject("qjsdur", "vnaaudwowlf") + "</td>" +
										"<td style=\"width:85px; text-align:center; border-right:solid 1px black; border-bottom:solid 1px black;\" >" + GetGlobalResourceObject("qjsdur", "tnfid") + "</td>" +
										"<td style=\"width:60px; text-align:center; border-right:solid 1px black; border-bottom:solid 1px black;\" >" + GetGlobalResourceObject("qjsdur", "eksrk") + "</td>" +
										"<td style=\"width:85px; text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >" + GetGlobalResourceObject("qjsdur", "rmador") + "</td>" +
										"<td style=\"width:50px; text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >" + GetGlobalResourceObject("qjsdur", "vhwkdtnfid") + "</td>" +
										"<td style=\"width:60px; text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >" + GetGlobalResourceObject("qjsdur", "wndfid") + "</td>" +
										"<td style=\"width:65px; text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >" + GetGlobalResourceObject("qjsdur", "cpwjr") + "</td></tr>");
		DB.SqlCmd.CommandText = @"SELECT 
			RFI.RequestFormItemsPk, RFI.ItemCode, RFI.MarkNNumber, RFI.Description, RFI.Label, 
			RFI.Material, RFI.Quantity, RFI.QuantityUnit, RFI.PackedCount, RFI.PackingUnit, 
			RFI.GrossWeight, RFI.Volume, RFI.UnitPrice, RFI.Amount, RFI.LastModify, 
			RF.MonetaryUnitCL ,RF.ConsigneePk
		FROM RequestForm AS RF
		left join RequestFormItems AS RFI On RF.RequestFormPk = RFI.RequestFormPk
		WHERE RF.RequestFormPk = " + RequestFormPk + ";";
		string EachRow = "<tr><td style=\"height:20px; text-align:left; padding-left:5px;  border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black; font-size:11px; \" >{0}</td>" +
								"<td style=\"text-align:right; padding-right:6px;  border-right:solid 1px black;border-bottom:solid 1px black;font-size:11px;\" >{1}</td>" +
								"<td style=\"text-align:right; padding-right:6px;  border-right:solid 1px black;border-bottom:solid 1px black;font-size:11px;\" >{2}</td>" +
								"<td style=\"text-align:right; padding-right:6px;  border-right:solid 1px black;border-bottom:solid 1px black;font-size:11px;\" >{3}</td>" +
								"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;font-size:11px;\" >{4}</td>" +
								"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;font-size:11px;\" >{5}</td>" +
								"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;font-size:11px;\" >{6}</td></tr>";

		string EachRowTotal = "<tr><td style=\"font-weight:bold; height:20px; text-align:center; border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black; padding:2px; \" >{0}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{1}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{2}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{3}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{4}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{5}</td>" +
										"<td style=\"text-align:center; border-right:solid 1px black;border-bottom:solid 1px black;\" >{6}</td></tr>";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		decimal quantity = 0;
		decimal amount = 0;
		string monetaryunit = "";
		string quantityunit = "";
		while (RS.Read()) {
			string squantity = "&nbsp;";
			string samount = "&nbsp;";

			if (RS["Quantity"] + "" != "") {
				quantityunit = Common.GetQuantityUnit(RS["QuantityUnit"] + "");
				squantity = Common.NumberFormat(RS["Quantity"] + "") + " " + Common.GetQuantityUnit(RS["QuantityUnit"] + "");
				quantity += Decimal.Parse(RS["Quantity"] + "");
			}
			if (RS["Amount"] + "" != "") {
				monetaryunit = Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "");
				samount = Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "") + " " + Common.NumberFormat("" + RS["Amount"]);
				amount += Decimal.Parse("" + RS["Amount"]);
			}

			string description;
			if (RS["Description"] + "" != "" && RS["Label"] + "" != "") {
				description = RS["Label"] + " : " + RS["Description"];
			}
			else {
				description = RS["Label"] + "" + RS["Description"];
			}

			string[] RowData = new string[]{
				description + ((RS["Material"] + "") == "" ? "" : "(" + RS["Material"] + ")") ,
				((RS["Quantity"] + "") == "" ? "&nbsp;" : Common.NumberFormat(RS["Quantity"] + "") + " " + Common.GetQuantityUnit(RS["QuantityUnit"] + "")) ,
				((RS["UnitPrice"] + "") == "" ? "&nbsp;" : Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "") + " " + Common.NumberFormat("" + RS["UnitPrice"])) ,
				((RS["Amount"] + "") == "" ? "&nbsp;" : Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "") + " " + Common.NumberFormat("" + RS["Amount"])),
				((RS["PackedCount"] + "") == "" ? "&nbsp;" : Common.NumberFormat(RS["PackedCount"] + "") + " " + Common.GetPackingUnit(RS["PackingUnit"] + "")) ,
				((RS["GrossWeight"] + "") == "" ? "&nbsp;" : Common.NumberFormat(RS["GrossWeight"] + "") + " Kg") ,
				((RS["Volume"] + "") == "" ? "&nbsp;" : Common.NumberFormat(RS["Volume"] + "") + " CBM")
			};

			ReturnValue.Append(String.Format(EachRow, RowData));
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = @"SELECT	
			[TotalPackedCount], [PackingUnit], [TotalGrossWeight], [TotalVolume] 
		FROM [dbo].[RequestForm]
		WHERE [RequestFormPk]=" + RequestFormPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue.Append(String.Format(EachRowTotal, "Total",
				Common.NumberFormat(quantity.ToString()) + " " + quantityunit,
				"&nbsp;",
				monetaryunit + " " + Common.NumberFormat(amount.ToString()),
				((RS["TotalPackedCount"] + "") == "" ? "" : Common.NumberFormat(RS["TotalPackedCount"] + "") + " " + Common.GetPackingUnit(RS["PackingUnit"] + "")),
				((RS["TotalGrossWeight"] + "") == "" ? "" : Common.NumberFormat(RS["TotalGrossWeight"] + "") + " Kg"),
				((RS["TotalVolume"] + "") == "" ? "" : Common.NumberFormat(RS["TotalVolume"] + "") + " CBM")));
		}
		RS.Dispose();
		ReturnValue.Append("</table>");
		return ReturnValue + "";
	}
	private String TitleLoad(string RequestFormPk) {
		string ReturnValue = "";
		DB.SqlCmd.CommandText = @"SELECT 
			C.[CompanyPk]
			, C.[CompanyName]
			, C.[CompanyAddress]
			, C.[CompanyTEL]
			, C.[CompanyFAX]
			, C.[PresidentEmail] 
		FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS HEAD
		LEFT JOIN [dbo].[Company] AS C ON HEAD.[BRANCH_COMPANY_PK] = C.[CompanyPk]
		WHERE HEAD.[TABLE_NAME] = 'RequestForm'
		AND HEAD.[TABLE_PK] = " + RequestFormPk + @" 
		AND [REQUESTFORMCALCULATE_HEAD_PK] = " + CalcHeadPk;
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		if (RS.Read()) {
			OurBranchPk = RS["CompanyPk"] + "";
			CompanyTEL = RS["CompanyTEL"] + "";
			PresidentEmail = RS["PresidentEmail"] + "";
			ReturnValue = "<div style=\"border-bottom:solid 2px black; width:692px; height:95px; text-align:center; padding-top:3px; \">" +
				"<div style=\"font-size:18px; font-weight:bold; text-align:center; letter-spacing:3px;\">" + RS["CompanyName"] + "</div>" +
				"<div style=\"font-size:12px; text-align:center; padding-top:15px; letter-spacing:1px;   \">" + RS["CompanyAddress"] + "</div>" +
				"<div style=\"font-size:12px; text-align:center; padding-top:5px; letter-spacing:1px;   \">TEL : " + RS["CompanyTEL"] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;FAX : " + RS["CompanyFAX"] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;E-mail : " + RS["PresidentEmail"] + "</div>" +
			"</div>";
			if (IsEstimation) {
				ReturnValue += "<div style=\"padding: 10px; width: 150px; height: 50px; text-align: center; margin: 0 auto; border: 2px solid black; margin-top: 20px; margin-bottom: 20px; font-size: 17px; letter-spacing: 3px;\">견적 (报价)</div>";
			}
			else {
				ReturnValue += "<div style=\"padding: 10px; width: 150px; height: 50px; text-align: center; margin: 0 auto; border: 2px solid black; margin-top: 20px; margin-bottom: 20px; font-size: 17px; letter-spacing: 3px;\">INVOICE</div>";
			}

			
			
		}
		RS.Dispose();
		return ReturnValue;
	}

	public Decimal LoadCarryOver(string SorC, string RequestFormPk) {

		DB.SqlCmd.CommandText = @"SELECT RFCH.[CUSTOMER_COMPANY_PK], RF.[DepartureRegionCode], RF.[ArrivalRegionCode], RF.[ArrivalDate] 
		FROM[dbo].[RequestForm] AS RF
		LEFT JOIN[dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON RF.[RequestFormPk] = RFCH.[TABLE_PK] 
		WHERE RFCH.[TABLE_NAME] = 'RequestForm' AND RequestFormPk=" + RequestFormPk + @" AND RFCH.[REQUESTFORMCALCULATE_HEAD_PK] = " + SorC;
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string CompanyPk = "";
		string ArrivalDate = "";
		if (RS.Read()) {
			CompanyPk = RS[0] + "";
			ArrivalDate = RS["ArrivalDate"] + "" == "" ? "29991231" : RS["ArrivalDate"] + "";
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"	
SELECT 
	RF.RequestFormPk, RF.ShipperPk, ConsigneePk , RF.ShipperCode, RF.ConsigneeCode, RF.DepartureDate, RF.ArrivalDate, RF.StepCL, RF.DocumentStepCL, RF.RequestDate
	, RF.[TotalPackedCount], RF.[PackingUnit], RF.[TotalGrossWeight], RF.[TotalVolume], RCH.[MONETARY_UNIT], RCH.[TOTAL_PRICE], RCH.[LAST_DEPOSITED_DATE], RCH.[DEPOSITED_PRICE], RF.[ExchangeDate]
	, RFCC.[AttachedRequestFormPk]
FROM RequestForm AS RF 
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RCH on RF.RequestFormPk=RCH.[TABLE_PK] 
	left join [RequestFormCalculateCarryover] AS RFCC ON RF.RequestFormPk=RFCC.[OriginalRequestFormPk] 
WHERE RCH.[TABLE_NAME] = 'RequestForm' 
AND StepCL>58 
AND ( ShipperPk=" + CompanyPk + " or ConsigneePk=" + CompanyPk + @" ) 
AND isnull(RF.ArrivalDate,20191231) <" + ArrivalDate + @" 
AND isnull(RFCC.SorC, '" + SorC + @"')='" + SorC + @"'
Order by RF.ArrivalDate DESC, RF.RequestFormPk DESC;";
		RS = DB.SqlCmd.ExecuteReader();
		String TempRequestFormPk = string.Empty;
		StringBuilder Temp = new StringBuilder();

		decimal TotalMinus = 0;

		while (RS.Read()) {
			if (TempRequestFormPk != RS[0] + "") {
				TempRequestFormPk = RS[0] + "";
				string[] TableInnerData = new string[13];
				if (RS["ShipperPk"] + "" == CompanyPk) {
					TableInnerData[1] = "s";
				}
				else {
					TableInnerData[1] = "c";
				}
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

				TableInnerData[10] = TotalCharge == 0 ? "--" : MonetaryUnit + " " + Common.NumberFormat(TotalCharge + "");
				TableInnerData[11] = Deposited == 0 ? "--" : MonetaryUnit + " " + Common.NumberFormat(Deposited + "");

				decimal tempminus = Deposited - TotalCharge;
				if (tempminus == 0) {
					continue;
				}

				if (RS["AttachedRequestFormPk"] + "" == RequestFormPk) {
					TotalMinus += tempminus;
				}
				continue;
			}
		}
		RS.Dispose();
		return TotalMinus * -1;
	}
	private String FreightLoad2015(string RequestFormPk, string SorC) {
		Decimal CarryOver = LoadCarryOver(SorC, RequestFormPk);

		DB.SqlCmd.CommandText = @"SELECT RF.[TotalPackedCount], RF.[PackingUnit], RF.[TotalGrossWeight], RF.[TotalVolume], RF.[ExchangeDate],
			RF.[CriterionValue], RFCB.[TO_EXCHANGE_RATE], RFCB.[ORIGINAL_MONETARY_UNIT], RFCB.[ORIGINAL_PRICE],
			RFCB.[EXCHANGED_MONETARY_UNIT], RFCB.[EXCHANGED_PRICE], RFCB.[TITLE], 
			BBANK.[BankName] AS BBankName, BBANK.[OwnerName] AS BBankOwnerName, 
			BBANK.[AccountNo] AS BBankAccountNo, BBANK.[BankMemo] AS BBankMemo , 
			CBANK.[BankName] AS CBankName, CBANK.[OwnerName] AS CBankOwnerName, 
			CBANK.[AccountNo] AS CBankAccountNo, CBANK.[BankMemo] AS CBankMemo 
		FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH 
		LEFT JOIN [dbo].[REQUESTFORMCALCULATE_BODY] AS RFCB ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK] = RFCB.[REQUESTFORMCALCULATE_HEAD_PK]
		LEFT JOIN [dbo].[RequestForm] AS RF ON RFCH.[TABLE_PK] = RF.[RequestFormPk]
		left join [dbo].[STANDARDPRICE_LIST] AS SP on RF.[StandardPricePk]=SP.[STANDARDPRICE_PK] 
		left join CompanyBank AS BBANK on RFCH.[BRANCH_COMPANY_PK]=BBANK.[CompanyBankPk]
		left join CompanyBank AS CBANK on RFCH.[CUSTOMER_BANK_PK]=CBANK.[CompanyBankPk]
		WHERE RFCH.[TABLE_NAME] = 'RequestForm'
		AND RFCH.[TABLE_PK] = " + RequestFormPk + @"
		AND RFCH.[REQUESTFORMCALCULATE_HEAD_PK] = " + SorC;
		string ExchangeDate, Original_Monetary_Unit, Original_Price, Exchanged_Monetary_Unit, Exchanged_Price, BBankName, BBankOwnerName, BBankAccount, CBankName, CBankOwnerName, CBankAccount;

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string monetary = "";
		if (RS.Read()) {
			ExchangeDate = RS["ExchangeDate"] + "";
			Original_Monetary_Unit = RS["ORIGINAL_MONETARY_UNIT"] + "";
			Original_Price = RS["ORIGINAL_PRICE"] + "";
			Exchanged_Monetary_Unit = RS["EXCHANGED_MONETARY_UNIT"] + "";
			Exchanged_Price = RS["EXCHANGED_PRICE"] + "";
			BBankName = RS["BBankName"] + "";
			BBankOwnerName = RS["BBankOwnerName"] + "";
			BBankAccount = RS["BBankAccountNo"] + "";
			CBankName = RS["CBankName"] + "";
			CBankOwnerName = RS["CBankOwnerName"] + "";
			CBankAccount = RS["CBankAccountNo"] + "";
			monetary = Common.GetMonetaryUnit(Exchanged_Monetary_Unit);
		}
		else {
			RS.Dispose();
			DB.DBCon.Close();
			return "0";
		}

		RS.Dispose();
		string mainmonetaryunit = Exchanged_Monetary_Unit;

		DB.SqlCmd.CommandText = "SELECT [TITLE], [ORIGINAL_PRICE], [ORIGINAL_MONETARY_UNIT] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [REQUESTFORMCALCULATE_HEAD_PK] = " + SorC + " ;";
		RS = DB.SqlCmd.ExecuteReader();

		List<string[]> ItemRowSource = new List<string[]>();
		////////////////////////여기부터 
		while (RS.Read()) {
			string CUR, ExRate, AmountBefore, AmountAfter;
			if (mainmonetaryunit != RS["ORIGINAL_MONETARY_UNIT"] + "") {
				string temp = RS["ORIGINAL_MONETARY_UNIT"] + "";
				switch (temp) {
					case "18":
						CUR = "CNY";
						break;
					case "19":
						CUR = "USD";
						break;
					case "20":
						CUR = "KRW";
						break;
					case "21":
						CUR = "JPY";
						break;
					case "22":
						CUR = "HKD";
						break;
					case "23":
						CUR = "EUR";
						break;
					default:
						CUR = "?";
						break;
				}

				ExRate = "N";
				AmountBefore = RS["ORIGINAL_PRICE"] + "";
				AmountAfter = "N";
			}
			else {
				CUR = "&nbsp;";
				ExRate = "&nbsp;";
				AmountBefore = "&nbsp;";
				AmountAfter = RS["ORIGINAL_PRICE"] + "";
			}
			string Title = RS["TITLE"] + "";
			string Header = "물류비";
			if (mainmonetaryunit == "20") {
				if (Title.IndexOf("VAT포함") > 0 || Title.IndexOf("부가세포함") > 0) {
					Header = "대행비";
				}
			}
			string[] innerData = new string[10];
			innerData[0] = Header;
			innerData[1] = Title;
			innerData[2] = CUR;
			innerData[3] = ExRate;
			innerData[4] = RS["ORIGINAL_MONETARY_UNIT"] + "";
			innerData[5] = AmountBefore;
			innerData[6] = mainmonetaryunit;
			innerData[7] = AmountAfter;
			ItemRowSource.Add(innerData);
		}
		RS.Dispose();

		bool isFirst;
		//Response.Write("WillPayTariff : " + WillPayTariff + " / SorC :" + SorC);
		string isCalculatedTariff = "";

		DB.SqlCmd.CommandText = "SELECT DocumentStepCL FROM RequestForm WHERE RequestFormPk=" + RequestFormPk + ";";
		string DocumentStepCL = DB.SqlCmd.ExecuteScalar() + "";
		if (DocumentStepCL == "0" || DocumentStepCL == "1" || DocumentStepCL == "2") {
			isCalculatedTariff = "0";
		}
		else {
			isCalculatedTariff = "1";
			
			isFirst = true;

			/* 제세금 body로 들어감으로 주석처리
			 DB.SqlCmd.CommandText = @"	
			SELECT CDT.CommercialDocumentHeadPk, CDT.Title, CDT.MonetaryUnitCL, CDT.Value 
			FROM CommercialDocumentTariff AS CDT
			WHERE CDT.GubunPk=" + RequestFormPk + " and CDT.GubunCL=0 and CDT.Value>0 ORDER BY  CDT.Title;";
			 RS = DB.SqlCmd.ExecuteReader();
			 
			while (RS.Read()) {
				string[] innerData = new string[10];

				innerData[1] = RS["Title"] + "";
				if (RS["Title"] + "" == "관세사비" && mainmonetaryunit == "20") {
					innerData[0] = "대행비";
					innerData[1] = "통관수수료";
				}
				else {
					innerData[0] = "제세금";
				}

				if (mainmonetaryunit != "20") {
					string temp = RS["MonetaryUnitCL"] + "";
					switch (temp) {
						case "18": innerData[2] = "CNY"; break;
						case "19": innerData[2] = "USD"; break;
						case "20": innerData[2] = "KRW"; break;
						case "21": innerData[2] = "JPY"; break;
						case "22": innerData[2] = "HKD"; break;
						case "23": innerData[2] = "EUR"; break;
						default: innerData[2] = "?"; break;
					}
					innerData[3] = "N";
					innerData[4] = RS["MonetaryUnitCL"] + "";
					innerData[5] = RS["Value"] + "";
					innerData[6] = mainmonetaryunit;
					innerData[7] = "N";
				}
				else {
					innerData[2] = "&nbsp;";
					innerData[3] = "&nbsp;";
					innerData[4] = "";
					innerData[5] = "";
					innerData[6] = mainmonetaryunit;
					innerData[7] = RS["Value"] + "";
				}
				ItemRowSource.Add(innerData);

			}
			RS.Dispose();
			*/

		}

		for (int i = 0; i < ItemRowSource.Count; i++) {
			if (ItemRowSource[i][3] == "N") {
				string tempExchangedDate = "";
				decimal tempExchangedAmount = 0;

				tempExchangedDate = ExchangeDate;
				string temp = "";
				//tempExchangedAmount = Math.Truncate(new Admin().GetExchangeRated(ItemRowSource[i][4], ItemRowSource[i][6], decimal.Parse(ItemRowSource[i][5]), out temp, tempExchangedDate));
				tempExchangedAmount = Math.Round(new Admin().GetExchangeRated(ItemRowSource[i][4], ItemRowSource[i][6], decimal.Parse(ItemRowSource[i][5]), out temp, tempExchangedDate));
				if (temp != "") {
					string[] temparray = temp.Split(Common.Splite11, StringSplitOptions.None);
					ItemRowSource[i][3] = Common.NumberFormat(temparray[3].Replace("@@", ""));
					ItemRowSource[i][7] = tempExchangedAmount + "";
				}
			}
		}

		StringBuilder tempstringbuilder = new StringBuilder();
		tempstringbuilder.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:695px; \"	><tr><td colspan='7' style=\"font-size:15px; font-weight:bold; padding-top:20px; border-bottom:solid 1px black;\" >청구내역</td></tr>" +
													"<tr><td style=\"height:25px; text-align:center;border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black;\" >TYPE</td>" +
													"<td style=\"height:25px; text-align:center;border-bottom:solid 1px black;border-right:solid 1px black;\" >ITEM</td>" +
													"<td style=\"width:40px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >CUR</td>" +
													"<td style=\"width:60px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >ExRate</td>" +
													"<td style=\"width:100px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >AMOUNT</td>" +
													"<td style=\"width:100px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >AMOUNT[" + Common.GetMonetaryUnit(mainmonetaryunit) + "]</td>" +
													"<td style=\"width:100px; text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" >TOTAL</td></tr>");

		int[] Summary_Colspan = new int[] { 0, 0, 0 };
		decimal[] Summary_Total = new decimal[] { 0, 0, 0 };
		for (int i = 0; i < ItemRowSource.Count; i++) {
			var Cursor = 0;
			switch (ItemRowSource[i][0]) {
				case "물류비": Cursor = 0; break;
				case "대행비": Cursor = 1; break;
				case "제세금": Cursor = 2; break;
			}
			Summary_Colspan[Cursor]++;
			Summary_Total[Cursor] += decimal.Parse(ItemRowSource[i][7]);
		}
		StringBuilder[] html_TBList = new StringBuilder[] { new StringBuilder(), new StringBuilder(), new StringBuilder() };
		for (int i = 0; i < ItemRowSource.Count; i++) {
			var Cursor = 0;
			switch (ItemRowSource[i][0]) {
				case "물류비": Cursor = 0; break;
				case "대행비": Cursor = 1; break;
				case "제세금": Cursor = 2; break;
			}
			if (html_TBList[Cursor].ToString() == "") {
				if (Cursor == 1) {
					if (ItemRowSource[i][1].LastIndexOf("(") > 0) {
						ItemRowSource[i][1] = ItemRowSource[i][1].Substring(0, ItemRowSource[i][1].LastIndexOf("("));
					}
					html_TBList[Cursor].Append("<tr>" +
							"<td style=\"height:22px; text-align:center; font-weight:bold; border-bottom:solid 1px black;border-right:solid 1px black;border-left:solid 1px black;\" rowspan=\"" + Summary_Colspan[Cursor] + "\" >" + ItemRowSource[i][0] + "</td>" +
							"<td style=\"height:22px; text-align:left; padding-left:10px;  border-bottom:solid 1px black;border-right:solid 1px black;\" >" + ItemRowSource[i][1] + "</td>" +
							"<td style=\"text-align:center;border-bottom:solid 1px black;border-right:solid 1px black;\" colspan='2'>VAT포함</td>" +
							"<td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">" +
										(ItemRowSource[i][4] == ItemRowSource[i][6] ? "&nbsp;" : Common.GetMonetaryUnit(ItemRowSource[i][4]) + " " + Common.NumberFormat(ItemRowSource[i][5])) + "</td>" +
							"<td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">" + Common.GetMonetaryUnit(ItemRowSource[i][6]) + " " + Common.NumberFormat(ItemRowSource[i][7]) + "</td>" +
							"<td rowspan=\"" + Summary_Colspan[Cursor] + "\" style=\"text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\">" + Common.GetMonetaryUnit(ItemRowSource[i][6]) + " " + Common.NumberFormat(Summary_Total[Cursor].ToString()) + "</td></tr>");
				}
				else if (Cursor == 2 && ItemRowSource[i][6] == "20") {
					html_TBList[Cursor].Append("<tr>" +
							"<td style=\"height:22px; text-align:center; font-weight:bold; border-bottom:solid 1px black;border-right:solid 1px black;border-left:solid 1px black;\" rowspan=\"" + Summary_Colspan[Cursor] + "\" >" + ItemRowSource[i][0] + "</td>" +
							"<td style=\"height:22px; text-align:left; padding-left:10px;  border-bottom:solid 1px black;border-right:solid 1px black;\" colspan='3' >" + ItemRowSource[i][1] + "</td>" +
							"<td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\" colspan='2' >" +
										Common.GetMonetaryUnit(ItemRowSource[i][6]) + " " + Common.NumberFormat(ItemRowSource[i][7]) + "</td>" +
							"<td rowspan=\"" + Summary_Colspan[Cursor] + "\" style=\"text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\">" +
										Common.GetMonetaryUnit(ItemRowSource[i][6]) + " " + Common.NumberFormat(Summary_Total[Cursor].ToString()) + "</td></tr>");
				}
				else {
					html_TBList[Cursor].Append("<tr>" +
							"<td style=\"height:22px; text-align:center; font-weight:bold; border-bottom:solid 1px black;border-right:solid 1px black;border-left:solid 1px black;\" rowspan=\"" + Summary_Colspan[Cursor] + "\" >" + ItemRowSource[i][0] + "</td>" +
							"<td style=\"height:22px; text-align:left; padding-left:10px;  border-bottom:solid 1px black;border-right:solid 1px black;\" >" + ItemRowSource[i][1] + "</td>" +
							"<td style=\"text-align:center;border-bottom:solid 1px black;border-right:solid 1px black;\">" + ItemRowSource[i][2] + "</td>" +
							"<td style=\"text-align:center;border-bottom:solid 1px black;border-right:solid 1px black;\">" + ItemRowSource[i][3] + "</td>" +
							"<td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">" +
										(ItemRowSource[i][4] == ItemRowSource[i][6] ? "&nbsp;" : Common.GetMonetaryUnit(ItemRowSource[i][4]) + " " + Common.NumberFormat(ItemRowSource[i][5])) + "</td>" +
							"<td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">" + Common.GetMonetaryUnit(ItemRowSource[i][6]) + " " + Common.NumberFormat(ItemRowSource[i][7]) + "</td>" +
							"<td rowspan=\"" + Summary_Colspan[Cursor] + "\" style=\"text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\">" + Common.GetMonetaryUnit(ItemRowSource[i][6]) + " " + Common.NumberFormat(Summary_Total[Cursor].ToString()) + "</td></tr>");
				}
			}
			else {
				if (Cursor == 1) {
					if (ItemRowSource[i][1].LastIndexOf("(") > 0) {
						ItemRowSource[i][1] = ItemRowSource[i][1].Substring(0, ItemRowSource[i][1].LastIndexOf("("));
					}
					html_TBList[Cursor].Append("<tr>" +
								"<td style=\"height:22px; text-align:left; padding-left:10px;  border-bottom:solid 1px black;border-right:solid 1px black; \" >" + ItemRowSource[i][1] + "</td>" +
								"<td style=\"text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\" colspan='2' >VAT포함</td>" +
								"<td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">" +
									(ItemRowSource[i][4] == ItemRowSource[i][6] ? "&nbsp;" : Common.GetMonetaryUnit(ItemRowSource[i][4]) + " " + Common.NumberFormat(ItemRowSource[i][5])) + "</td>" +
								"<td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">" +
									Common.GetMonetaryUnit(ItemRowSource[i][6]) + " " + Common.NumberFormat(ItemRowSource[i][7]) + "</td></tr>");
				}
				else if (Cursor == 2 && ItemRowSource[i][6] == "20") {
					html_TBList[Cursor].Append("<tr>" +
								"<td style=\"height:22px; text-align:left; padding-left:10px;  border-bottom:solid 1px black;border-right:solid 1px black; \" colspan='3' >" + ItemRowSource[i][1] + "</td>" +
								"<td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\" colspan='2' >" +
									Common.GetMonetaryUnit(ItemRowSource[i][6]) + " " + Common.NumberFormat(ItemRowSource[i][7]) + "</td></tr>");
				}
				else {
					html_TBList[Cursor].Append("<tr>" +
								"<td style=\"height:22px; text-align:left; padding-left:10px;  border-bottom:solid 1px black;border-right:solid 1px black; \" >" + ItemRowSource[i][1] + "</td>" +
								"<td style=\"text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\">" + ItemRowSource[i][2] + "</td>" +
								"<td style=\"text-align:center; border-bottom:solid 1px black;border-right:solid 1px black;\">" + ItemRowSource[i][3] + "</td>" +
								"<td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">" +
									(ItemRowSource[i][4] == ItemRowSource[i][6] ? "&nbsp;" : Common.GetMonetaryUnit(ItemRowSource[i][4]) + " " + Common.NumberFormat(ItemRowSource[i][5])) + "</td>" +
								"<td style=\"text-align:right;padding-right:10px; border-bottom:solid 1px black;border-right:solid 1px black;\">" +
									Common.GetMonetaryUnit(ItemRowSource[i][6]) + " " + Common.NumberFormat(ItemRowSource[i][7]) + "</td></tr>");
				}
			}
		}
		tempstringbuilder.Append(html_TBList[0] + "" + html_TBList[1] + "" + html_TBList[2]);

		string Html_NotCalculatedTariff = "";
		if (isCalculatedTariff == "0" && SorC == "C") {
			Html_NotCalculatedTariff = "			<div style=\"position: absolute; margin-top:-100px; margin-left:200px; \" id=\"PnNotCalculatedTariff\"><img src=\"/Images/NotCalculatedTariff.png\" /></div>";
		}
		if (Summary_Colspan[0] == 0 && Summary_Colspan[1] == 0 && Summary_Colspan[2] == 0) {
			tempstringbuilder.Append("<tr><td colspan='7' style=\"height:30px; font-weight:bold; text-align:center; border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black;\" >청구내역이 없습니다. </tr>");
		}
		else {
			string TotalString = "";
			if (Summary_Total[0] > 0) {
				TotalString += "물류비";
			}
			if (Summary_Total[1] > 0) {
				if (TotalString != "") {
					TotalString += " + ";
				}
				TotalString += "대행비";
			}
			if (Summary_Total[2] > 0) {
				if (TotalString != "") {
					TotalString += " + ";
				}
				TotalString += "제세금";
			}

			tempstringbuilder.Append("<tr  style=\"height:30px;\"><td colspan='4' style=\"height:30px; font-weight:bold; text-align:center; border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black;\" >TOTAL (" + TotalString + ")</td>" +
														"	<td colspan='3' style=\"text-align:center;padding-right:10px; font-weight:bold; border-bottom:solid 1px black;border-right:solid 1px black;\">" + monetary + " " + Common.NumberFormat((Summary_Total[0] + Summary_Total[1] + Summary_Total[2]).ToString()) + "</td></tr>");
		}
		if (CarryOver != 0) {
			tempstringbuilder.Append("<tr  style=\"height:30px;\"><td colspan='4' style=\"height:30px; font-weight:bold; text-align:center; border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black;\" >전잔</td>" +
														"	<td colspan='3' style=\"text-align:center;padding-right:10px; font-weight:bold; border-bottom:solid 1px black;border-right:solid 1px black;\">" + monetary + " " + Common.NumberFormat(CarryOver.ToString()) + "</td></tr>");
			tempstringbuilder.Append("<tr  style=\"height:30px;\"><td colspan='4' style=\"height:30px; font-weight:bold; text-align:center; border-left:solid 1px black;border-right:solid 1px black;border-bottom:solid 1px black;\" >청구합계</td>" +
														"	<td colspan='3' style=\"text-align:center;padding-right:10px; font-weight:bold; border-bottom:solid 1px black;border-right:solid 1px black;\">" + monetary + " " + Common.NumberFormat((Summary_Total[0] + Summary_Total[1] + Summary_Total[2] + CarryOver) + "") + "</td></tr>");
		}

		tempstringbuilder.Append("</table>");
		tempstringbuilder.Append(Html_NotCalculatedTariff);

		if (IsEstimation) {
			string comment = "";

			DB.SqlCmd.CommandText = @"SELECT A.Duties, A.Name, A.TEL, A.Mobile, A.Email , RFAI.[DESCRIPTION]
			FROM Company AS C
			left join Account_ AS A on C.ResponsibleStaff=A.AccountID
			left join [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON RFCH.[BRANCH_COMPANY_PK]=C.CompanyPk
			LEFT JOIN [dbo].[RequestForm] AS R ON RFCH.[TABLE_PK] = R.RequestFormPk 
			left join (SELECT [TABLE_PK], [DESCRIPTION] FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [CODE]='5') AS RFAI ON RFAI.[TABLE_PK]=R.RequestFormPk 
			WHERE RFCH.[TABLE_NAME]  = 'RequestForm'
			AND RFCH.[REQUESTFORMCALCULATE_HEAD_PK] = " + SorC + "; ";

			RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				if (RS["DESCRIPTION"] + "" != "") {
					comment = "<p>" + RS["DESCRIPTION"] + "</p>";
				}
			}
			BankInfo = "<div style=\"font-size:17px; margin-top:10px; padding-top:10px; line-height:20px; text-align:left;\" >" +
						"<div></br></div></div>" +
				"	<div style=\"border-top:1px dotted black; border-bottom:1px dotted black; margin-top:10px; padding-top:10px; line-height:20px; text-align:right;\">" +
"<div><strong>상기 금액은 견적용으로 환율과 같은 기타사항의 변동으로 실제 청구금액이 변동될수 있습니다.</strong></div>" +
comment.Replace("\r", "<br/>") +
						"</div>";
		}
		else {
			string bankaccount;
			bankaccount = BBankName + " : " + BBankAccount + " " + BBankOwnerName;
			DB.SqlCmd.CommandText = @"SELECT A.Duties, A.Name, A.TEL, A.Mobile, A.Email , RFAI.[DESCRIPTION]
			FROM Company AS C
			left join Account_ AS A on C.ResponsibleStaff=A.AccountID
			left join [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON RFCH.[BRANCH_COMPANY_PK]=C.CompanyPk
			LEFT JOIN [dbo].[RequestForm] AS R ON RFCH.[TABLE_PK] = R.RequestFormPk 
			left join (SELECT [TABLE_PK], [DESCRIPTION] FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [CODE]='5') AS RFAI ON RFAI.[TABLE_PK]=R.RequestFormPk 
			WHERE RFCH.[TABLE_NAME] = 'RequestForm'
			AND RFCH.[REQUESTFORMCALCULATE_HEAD_PK] = " + SorC + ";";

			//Response.Write(DB.SqlCmd.CommandText);
			RS = DB.SqlCmd.ExecuteReader();
			string staff = "";
			string comment = "";
			if (RS.Read()) {
				if (RS["DESCRIPTION"] + "" != "") {
					comment = "<p>" + RS["Value"] + "</p>청구금액의 입금기한은 출고일 PM 1:40전까지, 토요일은11:00전까지 입니다. <br />마감시간내 미입금 및 이후 입금시 익일 업무진행됩니다. 양해바랍니다.";
					/*
					comment += "<p>위와 같이 운임/통관 대행비을 청구하오니<br>" +
						 "상기 계좌번호로 <br>" +
						  "</p>";
					 */
				}
				staff = RS["Duties"] + " " + RS["Name"] + " ( TEL : " + CompanyTEL + "  E-mail : " + PresidentEmail + ")";
			}

			string tradecommnet = "<strong>* : 무역송금 통장이 변경 되오니 본사에 반드시 확인 후 무역송금 바랍니다</strong>";
			RS.Dispose();
			string Html_BankInfo = "";
			MakeHtml_BankInfo(OurBranchPk, out Html_BankInfo);


			if (OurBranchPk == "3157") {
				bankaccount = @"KEB 하나은행(구.외환은행) (주)국제종합물류 630-004796-321 ";

				BankInfo = "	<div style=\"border-top:1px dotted black; border-bottom:1px dotted black; margin-top:10px; padding-top:10px; line-height:20px; text-align:right;\">" +
   "<div>" + staff + "&nbsp;&nbsp;&nbsp;<strong><br/>" + bankaccount + "</strong></div>" +
								comment.Replace("\r", "<br/>") +
							"</div>";
			}
			else {
				BankInfo = "<div style=\"font-size:17px; margin-top:10px; padding-top:10px; line-height:20px; text-align:left;\">" +
								"<div>" + tradecommnet + "</br></div></div>" +
								"	<div style=\"border-top:1px dotted black; border-bottom:1px dotted black; margin-top:10px; padding-top:10px; line-height:20px; text-align:right;\">" +
					   "<div>" + staff + "<strong><br/>" + Html_BankInfo + "</strong></div>" +
													comment.Replace("\r", "<br/>") +
						"</div>";
			}


		}
		return tempstringbuilder + "";
	}
	private string MakeHtml_BankInfo(string OurBranchPk, out string Html_BankInfo) {
		DB.SqlCmd.CommandText = string.Format(@"
select BankName,OwnerName,AccountNo from CompanyBank
where isdel = 0 and GubunPk={0}", OurBranchPk);
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		Html_BankInfo = "";
		while (RS.Read()) {
			Html_BankInfo += RS["BankName"].ToString() + " " + RS["OwnerName"].ToString() + " " + RS["AccountNo"].ToString() + "<br />";
		}
		RS.Dispose();

		return "1";
	}
}