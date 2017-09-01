using System;
using System.Web.UI;
using Components;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

public partial class Admin_RequestView : System.Web.UI.Page
{
	protected String Contents;
	protected String HtmlSchedule;
	protected StringBuilder HtmlOwnerOfGoods;
	protected StringBuilder HtmlItem;
	protected StringBuilder HtmlJubsuWayCL;
	protected StringBuilder HtmlPayment;
	protected StringBuilder HtmlDeposited;
	protected String HtmlDocumentRequest;
	protected String HtmlButton;
	protected String HtmlButtonPayment;
	protected String[] MemberInformation;
	protected StringBuilder HtmlOurStaff;
	protected StringBuilder HtmlDelivery;
	protected String HtmlFileList;
	protected String HtmlFileList_Ever;
	protected String Gubun;
	protected String BBHPk;

	protected String Html_SettlementWithCustoms;
	protected String ShipperPk;
	protected String ConsigneePk;
	private String RequestFormPk;
	private Int32 StepCL;
	private String DocumentStepCL;
	private String CommercialDocumentPk;
	private String BL_UploadPk;
	private String SorC;
	protected Decimal ShipperTotalCharge;
	protected String PaymentShipperMonetary = "";
	protected Decimal ConsigneeTotalCharge;
	protected String PaymentConsigneeMonetary = "";
	protected String DaeNap;
	private DBConn DB;
	protected String HtmlCommentList;
	protected String HtmlCommentListColored;
	protected String HtmlRequestHistory;
	protected String HtmlMemo;
	protected String IsConsigneeConfirmedStyle;

	private String TotalPackedCount;
	private String PackingUnit;
	protected String RecentClearanceHtml;
	protected String TransportBetweenBranchPk;

	protected String ConsigeeInDocument;
	protected String Html_HistoryShipper;
	protected String Html_HistoryConsignee;
	protected String BLCHECKCommercialDocumentPk;
	protected String CalcHeadPk;

	private bool IsOnlyilic66BTN;
	protected string BTN_Onlyilic66;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("../Default.aspx");
		}
		MemberInformation = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		if (MemberInformation[0] == "Customs") {
			Response.Write("<script type='text/javascript'>alert(\"권한이 없습니다\"); history.back();</script>");
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

		Gubun = Request.Params["g"];
		RequestFormPk = Request.Params["Pk"];

		// 회계팀요청 계산서 완료체크 기능
		//2014. 02. 13. 김상수 		
		IsOnlyilic66BTN = Onlyilic66BTN(RequestFormPk);

		if (MemberInformation[2] == "ilic55" || MemberInformation[2] == "ilic66" || MemberInformation[2] == "ilic77") {
			BTN_Onlyilic66 = MakeOnlyilic66BTN(IsOnlyilic66BTN);
			//2014. 02. 13. 김상수
			ConsigeeInDocument = "<br />" + LoadConsgieeInDocument(Request.Params["Pk"]);
			// 회계팀요청 계산서 완료체크 기능
			//2014. 03. 06. 김상수 		

		} else {
			ConsigeeInDocument = "";
			BTN_Onlyilic66 = "";
		}
		RequestFormLoad(Request.Params["pk"], Gubun, MemberInformation[2]);

		decimal CarryOverS = 0;
		decimal CarryOverC = 0;
		if (ShipperPk != "") {
			Html_HistoryShipper = RequestListLoadByAdmin(ShipperPk, "S", RequestFormPk, out CarryOverS);
		}
		if (ConsigneePk != "") {
			Html_HistoryConsignee = RequestListLoadByAdmin(ConsigneePk, "C", RequestFormPk, out CarryOverC);
		}


		if (StepCL > 54 || StepCL != 56) {
			LoadAdditionalData(Request.Params["pk"], CarryOverS, CarryOverC);
		}
		LoadRequestComment(Request.Params["pk"], MemberInformation[2]);
		if (StepCL > 57) {
			LoadCompanyDepositList();
		}
		// 회계팀요청 계산서 완료체크 기능
		//2014. 02. 13. 김상수 		
		if (StepCL < 57) {
			BTN_Onlyilic66 = "";
		}

		LoadDeliveryLoad(Request.Params["Pk"], MemberInformation[2]);
		LoadRequestHistory(Request.Params["Pk"]);

		RecentClearanceHtml = new RequestP().HTMLClearanceItemHistory(ShipperPk, ConsigneePk, 7);
		LoadDaenap();
		LoadDO();


	}

	public String RequestListLoadByAdmin(string CompanyPk, string SorC, string RequestFormPk, out decimal Carryover) {
		string ArrivalDate = "";
		DB.SqlCmd.CommandText = string.Format(@"
select ArrivalDate from RequestForm
where RequestFormPk='{0}'", RequestFormPk);

		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ArrivalDate = RS["ArrivalDate"] + "" == "" ? "29991231" : RS["ArrivalDate"] + "";
		}
		RS.Dispose();


		string TableInnerRow = "<tr>" +
		   "<td class='{0}'><a href=\"RequestView.aspx?g={1}&pk={2} \">{4}</a></td>" +
		   "<td class='{0}' ><a href=\"CompanyInfo.aspx?M=View&S={3} \">{5}</a></td>" +
		   "<td class='{0}'>{7}</td>" +
		   "<td class='{0}'\">{10}</td>" +
		   "<td class='{0}'>{11}</td>" +
		   "<td class='{0}'>{12}</td>" +
		   "<td class='{0}'>{6}</td>" +
		"</tr>";
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"	
SELECT 
	RF.RequestFormPk, RF.ShipperPk, ConsigneePk , RF.ShipperCode, RF.ConsigneeCode, RF.DepartureDate, RF.ArrivalDate, RF.StepCL, RF.DocumentStepCL, RF.RequestDate
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume, RCH.[MONETARY_UNIT], RCH.[TOTAL_PRICE], RCH.[DEPOSITED_PRICE], RCH.[LAST_DEPOSITED_DATE], RF.ExchangeDate
	, RFCC.[AttachedRequestFormPk]
FROM RequestForm AS RF 
Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RCH on RF.RequestFormPk=RCH.[TABLE_PK] 
left join [RequestFormCalculateCarryover] AS RFCC ON RF.RequestFormPk=RFCC.[OriginalRequestFormPk] 
WHERE RF.ArrivalDate>'20130000' 
AND ISNULL(RCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
AND StepCL>58 and( ShipperPk=" + CompanyPk + " or ConsigneePk=" + CompanyPk + @" ) 
AND isnull(RF.ArrivalDate,20191231) <" + ArrivalDate + @" 
AND isnull(RFCC.SorC, '" + SorC + @"')='" + SorC + @"'

	Order by RF.ArrivalDate DESC, RF.RequestFormPk DESC;";
		RS = DB.SqlCmd.ExecuteReader();
		String StyleID;
		String TempRequestFormPk = string.Empty;
		StringBuilder Temp = new StringBuilder();
		bool IsFirst = true;
		int k = 0;

		decimal TotalMinus = 0;
		Carryover = 0;

		while (RS.Read()) {
			/*
			 *				RF.RequestFormPk, RF.ShipperPk, ConsigneePk , RF.ShipperCode, RF.ConsigneeCode, 
			 *				RF.DepartureDate, RF.ArrivalDate, RF.StepCL, RF.RequestDate, C.CompanyName, 
			 *				CC.CompanyName as CCompanyName, CCL.TargetCompanyName, Departure.NameE, Arrival.NameE, RI.Description, 
			 *				RCH.TotalPackedCount, RCH.PackingUnit, RCH.TotalGrossWeight, RCH.TotalVolume, RII.itemCount
			 */
			if (SorC == "S") {
				if (RS["ShipperPk"] + "" != CompanyPk) {
					SorC = "C";
				}
			} else {
				if (RS["ShipperPk"] + "" == CompanyPk) {
					SorC = "S";
				}
			}
			if (TempRequestFormPk != RS[0] + "") {
				TempRequestFormPk = RS[0] + "";
				string[] TableInnerData = new string[13];
				TableInnerData[0] = "TBody1";

				if (SorC == "S") {
					TableInnerData[1] = "s";
					TableInnerData[5] = "<strong>" + RS["ShipperCode"] + "</strong> ";
				} else {
					TableInnerData[1] = "c";
					TableInnerData[5] = "<strong>" + RS["ConsigneeCode"] + "</strong> ";
				}

				TableInnerData[2] = TempRequestFormPk;
				TableInnerData[3] = CompanyPk;


				TableInnerData[4] = (RS["DepartureDate"] + "" == "" ? "" : (RS["DepartureDate"] + "").Substring(2)) + " ~ " + (RS["ArrivalDate"] + "" == "" ? "" : (RS["ArrivalDate"] + "").Substring(2));


				TableInnerData[7] = RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "");
				TableInnerData[8] = Common.NumberFormat(RS["TotalGrossWeight"] + "");
				TableInnerData[9] = Common.NumberFormat(RS["TotalVolume"] + "");
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
				TotalMinus += tempminus;
				if (tempminus == 0) {
					continue;
				} else if (tempminus > 0) {
					TableInnerData[12] = "<span style=\"color:blue;\">" + MonetaryUnit + " " + Common.NumberFormat(tempminus + "") + "</span>";
				} else {
					TableInnerData[12] = "<span style=\"color:red;\">" + MonetaryUnit + " " + Common.NumberFormat(tempminus + "") + "</span>";
				}
				if (RS["AttachedRequestFormPk"] + "" == "") {
					TableInnerData[6] = "<input type='button' id='BTN_CarryOver" + TableInnerData[2] + "' value='이월' onclick=\"SetCarryover('Set', '" + RequestFormPk + "', '" + TableInnerData[2] + "', '" + SorC + "', '" + (tempminus * -1).ToString() + "');\" />";
				} else if (RS["AttachedRequestFormPk"] + "" == RequestFormPk) {
					TableInnerData[6] = "<input type='button' id='BTN_CarryOver" + TableInnerData[2] + "' value='취소' style='color:red;' onclick=\"SetCarryover('Remove', '" + RequestFormPk + "', '" + TableInnerData[2] + "', '" + SorC + "', '" + (tempminus * -1).ToString() + "');\" />";
					Carryover += tempminus * -1;
				} else {
					TableInnerData[6] = "<input type='button' id='BTN_CarryOver" + TableInnerData[2] + "' value='재이월' style='color:red;' onclick=\"SetCarryover('reset', '" + RequestFormPk + "', '" + TableInnerData[2] + "', '" + SorC + "', '" + (tempminus * -1).ToString() + "');\" />";
				}

				ReturnValue.Append(string.Format(TableInnerRow, TableInnerData));
				continue;
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		string TempString = "";
		if (TotalMinus > 0) {
			TempString = "<span style=\"color:blue;\">" + Common.NumberFormat(TotalMinus + "") + "</span>";
		} else {
			TempString = "<span style=\"color:red;\">" + Common.NumberFormat(TotalMinus + "") + "</span>";
		}
		return "<table border='0' cellpadding='0' cellspacing='0' style='width:580px;' ><thead><tr height='30px'>" +
					"<td class='THead1' style='width:120px;' >" + GetGlobalResourceObject("Member", "CompanyCode") + "</td>" +
					"<td class='THead1' style='width:80px;' >Name</td>" +
					"<td class='THead1' style='width:45px;'>Box</td>" +
					"<td class='THead1' style='width:95px;'>청구금액</td>" +
					"<td class='THead1' style='width:95px;'>입금액</td>" +
					"<td class='THead1' style='width:95px;'>차액</td>" +
					"<td class='THead1' style='' >BTN</td>" +
				 "</tr></thead>" + ReturnValue + "<tr height='10px'><td colspan='5' >&nbsp;</td><td style='text-align:center;'>" + TempString + "</td></tr></Table>";
	}


	private String LoadConsgieeInDocument(string RequetFormPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"SELECT [Consignee]
	FROM [INTL2010].[dbo].CommercialDocument 
	WHERE CommercialDocumentHeadPk=(SELECT TOP 1 [CommercialDocumentPk]
  FROM [INTL2010].[dbo].[CommerdialConnectionWithRequest]
  WHERE RequestFormPk=" + RequestFormPk + ");";
		DB.DBCon.Open();
		string ReturnValue = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		return ReturnValue;
	}
	private bool Onlyilic66BTN(string RequestFormPk) {
		DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = string.Format(@"
SELECT [CODE]
  FROM [dbo].[HISTORY]
  WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK]={0} AND [CODE] in ('16', '17') ORDER BY [CODE];", RequestFormPk);
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		int Count = 0;
		while (RS.Read()) {
			if (RS[0] + "" == "16") {
				Count++;
			} else {
				Count--;
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		Boolean ReturnValue = false;
		if (Count > 0) {
			ReturnValue = true;
		}
		return ReturnValue;
	}
	private string MakeOnlyilic66BTN(bool isOnlyilic66BTN) {
		if (!isOnlyilic66BTN) {
			return "<input type=\"button\" style='background-color:#FAEBD7;' onclick=\"SetRequestFormStep('receipt_confirm16')\" value=\"계산서 발행완료\" />";
		} else {
			return "<input type=\"button\" style='background-color:#FAEBD7;' onclick=\"SetRequestFormStep('receipt_confirm17')\" value=\"계산서 발행취소\" />";
		}
	}

	private void LoadDaenap() {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [AccountID], [Registerd] FROM [RequestFormHold] WHERE RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		if (MemberInformation[0] == "OurBranch") {
			if (RS.Read()) {
				if (RS["AccountID"] + "" == MemberInformation[2]) {
					DaeNap = "<div><strong>" + RS["AccountID"] + " : " + RS["Registerd"] + " " + GetGlobalResourceObject("qjsdur", "DaeNapHold") + "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "DaeNapDone") + "\" onclick=\"SetDaeNap('done');\" /></strong></div>";
				} else {
					DaeNap = "<div><strong>" + RS["AccountID"] + " : " + RS["Registerd"] + " " + GetGlobalResourceObject("qjsdur", "DaeNapHold") + "</strong></div>";

				}
			} else {
				if (StepCL == 57 || StepCL == 58) {
					DaeNap = "<div><input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "DaeNapHold") + "\" onclick=\"SetDaeNap('Hold');\" /></div>";
				} else {
					DaeNap = "";
				}
			}
		} else {
			DaeNap = "";
		}
		RS.Dispose();
		DB.DBCon.Close();
	}
	private void RequestFormLoad(string RequestFormPk, string Gubun, string ID) {
		DB = new DBConn();
		string tempstring = string.Empty;
		ShipperPk = string.Empty;
		ConsigneePk = string.Empty;
		string cclpk = string.Empty;
		string transportwaycl = string.Empty;
		string jubsuwaycl = string.Empty;
		string paymentwaycl = string.Empty;
		string paymentwhocl = string.Empty;
		string documentrequestcl = string.Empty;
		string monetaryunitcl = string.Empty;
		string stepcl = string.Empty;
		string memo = string.Empty;
		string itemmodifycount = string.Empty;
		string pickuprequestdate = string.Empty;

		string departurename = string.Empty;
		string arrivalname = string.Empty;
		string departuredate = string.Empty;
		string arrivaldate = string.Empty;
		string shippercode = string.Empty;
		string consigneecode = string.Empty;
		string notifypartyname = string.Empty;
		string shippercompanyname = string.Empty;
		string consigneecompanyname = string.Empty;
		string documentstepcl = string.Empty;
		string history = string.Empty;
		GetQuery GQ = new GetQuery();
		HtmlOwnerOfGoods = new StringBuilder();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT [REQUESTFORMCALCULATE_HEAD_PK] FROM [dbo].[REQUESTFORMCALCULATE_HEAD] WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK] = " + RequestFormPk + @" AND [BRANCH_COMPANY_PK] = " + MemberInformation[1];
		CalcHeadPk = DB.SqlCmd.ExecuteScalar() + "";
		if (CalcHeadPk == "") {
			CalcHeadPk = "0";
		}
		DB.SqlCmd.CommandText = "EXECUTE SP_SelectRequestViewLoad @RequestFormPk=" + RequestFormPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		if (RS.Read()) {
			ShipperPk = RS["ShipperPk"] + "";
			ConsigneePk = RS["ConsigneePk"] + "";
			cclpk = RS["ConsigneeCCLPk"] + "";
			transportwaycl = RS["TransportWayCL"] + "";
			jubsuwaycl = RS["JubsuWayCL"] + "";
			paymentwaycl = RS["PaymentWayCL"] + "";
			paymentwhocl = RS["PaymentWhoCL"] + "";
			documentrequestcl = RS["DocumentRequestCL"] + "";
			monetaryunitcl = RS["MonetaryUnitCL"] + "";
			stepcl = RS["StepCL"] + "";
			memo = RS["Memo"] + "";
			if (memo != "") {
				HtmlMemo = "<p style=\"width:430px; font-weight:bold; color:red;\">" + memo + "</p>";
			}


			pickuprequestdate = RS["PickupRequestDate"] + "";
			departurename = RS["DepartureName"] + "";
			arrivalname = RS["ArrivalName"] + "";
			departuredate = RS["DepartureDate"] + "";
			arrivaldate = RS["ArrivalDate"] + "";
			shippercode = RS["ShipperCode"] + "";
			consigneecode = RS["ConsigneeCode"] + "";
			notifypartyname = RS["NotifyPartyName"] + "";
			shippercompanyname = RS["ShipperCompanyName"] + "";
			consigneecompanyname = RS["ConsigneeCompanyName"] + "";
			//targetcompanyname = RS["TargetCompanyName"] + "";
			//targetcompanytel = RS["TargetCompanyTEL"] + "";
			//targetcompanyfax = RS["TargetCompanyFAX"] + "";
			//targetpresidentname = RS["TargetPresidentName"] + "";
			//targetemail = RS["TargetEmail"] + "";
			itemmodifycount = RS["ModifyCount"] + "";
			documentstepcl = RS["DocumentStepCL"] + "";
			DocumentStepCL = documentstepcl;
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = "SELECT [DESCRIPTION] FROM [dbo].[HISTORY] WHERE [TABLE_PK]=" + RequestFormPk + " AND [CODE]='49';";

		history = DB.SqlCmd.ExecuteScalar() + "";

		DB.SqlCmd.CommandText = "select [FilePk] from [dbo].[File] where [GubunCL]= 18  and [GubunPk]=" + RequestFormPk + ";";
		BL_UploadPk = DB.SqlCmd.ExecuteScalar() + "";

		//최초읽은거 표시
		if (stepcl == "50") {
			DB.SqlCmd.CommandText = "UPDATE RequestForm SET [StepCL] ='51' WHERE RequestFormPk=" + RequestFormPk + ";" +
			   GQ.AddRequestHistory(RequestFormPk, "51", MemberInformation[2], string.Empty);
			DB.SqlCmd.ExecuteNonQuery();
		}
		DB.SqlCmd.CommandText = "declare @aa int; SELECT @aa=count(*) FROM MsgSendedHistory WHERE GubunPk=" + RequestFormPk + " and GubunCL<6 group by SendedTime; select @@rowcount ";
		string msgsendcount = DB.SqlCmd.ExecuteScalar() + "";
		//최초읽은거 표시

		LoadFileList();
		LoadFileList_Ever();

		DB.SqlCmd.CommandText = "SELECT CommercialDocumentPk FROM CommerdialConnectionWithRequest WHERE RequestFormPk=" + RequestFormPk + ";";
		BLCHECKCommercialDocumentPk = DB.SqlCmd.ExecuteScalar() + "";

		if (documentstepcl != "" && documentstepcl != "0") {
			DB.SqlCmd.CommandText = "SELECT CommercialDocumentPk FROM CommerdialConnectionWithRequest WHERE RequestFormPk=" + RequestFormPk + ";";
			CommercialDocumentPk = DB.SqlCmd.ExecuteScalar() + "";


			//관세사와 정산 추가 140412 김상수
			if (CommercialDocumentPk != "") {
				DB.SqlCmd.CommandText = string.Format(@"
SELECT [BLNo], [Date], [Price], [Description] 
FROM [dbo].[SettlementWithCustoms] 
WHERE [BLNo] in (
	SELECT [BLNo] 
	FROM [dbo].[CommercialDocument] 
	WHERE [CommercialDocumentHeadPk]={0}
) 
ORDER BY [Date] ASC", CommercialDocumentPk);
				RS = DB.SqlCmd.ExecuteReader();

				StringBuilder TempSettlementWithCustoms = new StringBuilder();

				decimal TempTotal_SettlementWithCustoms = 0;
				while (RS.Read()) {
					TempSettlementWithCustoms.Append("<tr style=\"height:30px;\"><td style=\"text-align:center; \">" + RS["Date"].ToString().Substring(2) + "</td>" +
					   "<td style=\"text-align:right; padding-right:5px; \">" + Common.NumberFormat(RS["Price"] + "") + "</td>" +
					   "<td style=\"text-align:center; \">" + RS["Description"] + "</td></tr>");
					TempTotal_SettlementWithCustoms += decimal.Parse(RS["Price"] + "");
				}
				RS.Dispose();

				Html_SettlementWithCustoms = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:250px; border:1px black solid; \">" +
				   "<tr style=\"height:30px;\"><td colspan=\"3\" style=\"text-align:center; font-weight:bold;\">관세사와 정산</td></tr>" +
				   "<tr style=\"height:30px;\">" +
				   "	<td style=\"width:70px; text-align:center;\" >DATE</td>" +
				   "	<td style=\"width:90px; text-align:center;\" >Price</td>" +
				   "	<td >Comment</td></tr>" + TempSettlementWithCustoms +
				   "<tr style=\"height:30px;\">" +
				   "	<td style=\"text-align:right; padding-right:20px; background-color:#DDDDDD;\">Total</td>" +
				   "	<td style=\"text-align:right; padding-right:5px; \" >" + Common.NumberFormat(TempTotal_SettlementWithCustoms.ToString()) + "</td>" +
				   "	<td>&nbsp;</td></tr>" +
				   "</table>";
				TempSettlementWithCustoms = null;
			} else {
				Html_SettlementWithCustoms = "";
			}
			//관세사와 정산 추가 140412 김상수


		} else {
			CommercialDocumentPk = "";
		}

		DB.SqlCmd.CommandText = @"
SELECT DRC.OurBranchCode, ARC.OurBranchCode 
  FROM RequestForm AS R 
	left join RegionCode AS DRC ON R.DepartureRegionCode=DRC.RegionCode 
	left join RegionCode AS ARC ON R.ArrivalRegionCode=ARC.RegionCode 
WHERE RequestFormPk=" + RequestFormPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			if (RS[0] + "" == MemberInformation[1]) {
				SorC = "S";
			} else if (RS[1] + "" == MemberInformation[1]) {
				SorC = "C";
			} else {
				SorC = "N";
			}
		} else {
			SorC = "N";
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = @"
DECLARE @SPk int; 
DECLARE @CPk int; 
DECLARE @SName varchar(100); 
DECLARE @SAddress varchar(255);
DECLARE @CName varchar(100); 
DECLARE @CAddress varchar(255);

SELECT @SPk=[ShipperClearanceNamePk], @CPk=[ConsigneeClearanceNamePk] FROM [RequestForm] WHERE RequestFormPk=" + RequestFormPk + @";
if(@SPk='0')
SELECT @SName='명의대행',@SAddress=''
else
SELECT @SName=[Name], @SAddress=[Address] FROM [CompanyInDocument] WHERE [CompanyInDocumentPk]=@SPk;
if(@CPk='0')
SELECT @CName='명의대행',@CAddress=''
else
SELECT @CName=[Name], @CAddress=[Address] FROM [CompanyInDocument] WHERE [CompanyInDocumentPk]=@CPk;

SELECT @SName AS SN, @SAddress AS SA, @CName AS CN, @CAddress AS CA; ";
		string SN = "";
		string SA = "";
		string CN = "";
		string CA = "";
		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			SN = ((RS["SN"] + "").Length > 50 ? (RS["SN"] + "").Substring(0, 47) + "<br />" + (RS["SN"] + "").Substring(47) : RS["SN"] + "");
			SA = ((RS["SA"] + "").Length > 50 ? (RS["SA"] + "").Substring(0, 47) + "<br />" + (RS["SA"] + "").Substring(47) : RS["SA"] + "");
			CN = ((RS["CN"] + "").Length > 50 ? (RS["CN"] + "").Substring(0, 47) + "<br />" + (RS["CN"] + "").Substring(47) : RS["CN"] + "");
			CA = ((RS["CA"] + "").Length > 50 ? (RS["CA"] + "").Substring(0, 47) + "<br />" + (RS["CA"] + "").Substring(47) : RS["CA"] + "");
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"
  	SELECT 
		COUNT(*) 
	FROM 
		RequestForm AS RF
		left join RequestFormItems AS RFI On RF.RequestFormPk=RFI.RequestFormPk 
		left join ItemCertificate AS IC ON IC.StyleNo=RFI.Label 
	WHERE RF.RequestFormPk=" + RequestFormPk + @" 
		and ISNULL(IC.CompanyPk, RF.ConsigneePk)=RF.ConsigneePk 
		and IC.CertificateNo is not null ";

		string CertificateCount = DB.SqlCmd.ExecuteScalar() + "";

		DB.DBCon.Close();

		//Schedule
		HtmlSchedule = "<div style=\"font-size:18px; font-weight:bold; \"><span onclick=\"history.back();\" style=\"color:Blue; cursor:hand;\" ><-back</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
		   (departuredate == "" ? "" : departuredate.Substring(4, 4)) + departurename + " - " +
		   (arrivaldate == "" ? "" : arrivaldate.Substring(4, 4)) + arrivalname + " : " + Common.GetTransportWay(transportwaycl) + "</div>";
		//Schedule
		//Owner
		HtmlOwnerOfGoods.Append("	<div style=\"background-color:#FAEBD7;cursor:hand; padding:5px; \"  onclick=\"CompanyViewLight('s')\" >SHIPPER : " + shippercompanyname + (shippercode == "" ? " <span style=\"color:red;\">[NEW]</span>" : " [" + shippercode + "]") + "<br /><strong>" + SN + "</strong><br />" + SA + "</div>");

		tempstring = consigneecode == "" ? " <span style=\"color:red;\">[NEW]</span>" : " [" + consigneecode + "]";

		HtmlOwnerOfGoods.Append("	<div style=\"background-color:#C0C0C0;cursor:hand; padding:5px; margin-top:5px;\" onclick=\"CompanyViewLight('c')\"  >CONSIGNEE : " + consigneecompanyname + tempstring + "&nbsp;&nbsp;&nbsp;<input type=\"hidden\" id=\"HCCLPk\" value=\"" + cclpk + "\" ><br /><strong>" + CN + "</strong><br />" + CA + "</div>");
		//}
		if (notifypartyname + "" != "") {
			HtmlOwnerOfGoods.Append("<div>NOTIFY PARTY : " + notifypartyname + "</div>");
		}
		//Owner

		//////JubsuWayCL
		HtmlJubsuWayCL = new StringBuilder();

		//switch (jubsuwaycl)
		//{
		//    case "1":
		//        HtmlJubsuWayCL.Append("<div>" + GetGlobalResourceObject("RequestForm", "PickUp") + " : ");
		//        try { HtmlJubsuWayCL.Append(pickuprequestdate.Substring(4, 2) + "-" + pickuprequestdate.Substring(6, 2)); }
		//        catch (Exception) { }
		//        try { HtmlJubsuWayCL.Append(" " + pickuprequestdate.Substring(8, 2) + ":" + pickuprequestdate.Substring(10, 2)); }
		//        catch (Exception) { }
		//        break;
		//    case "2": HtmlJubsuWayCL.Append("<div>" + GetGlobalResourceObject("RequestForm", "SelfDelivery")); break;
		//    case "3": HtmlJubsuWayCL.Append("<div>" + GetGlobalResourceObject("RequestForm", "ETC") + " : "); break;
		//}
		//string Memo = memo != "" ? memo.Substring(0, memo.IndexOf("$$$")) : String.Empty;
		//HtmlJubsuWayCL.Append("&nbsp;&nbsp;&nbsp;" + Memo +
		//                                        "<input type=\"hidden\" id=\"ShipperCompanyPk\" value=\"" + ShipperPk + "\" />" +
		//                                        "<input type=\"hidden\" id=\"HPickUp\" value=\"" + transportwaycl + "##" + pickuprequestdate + "##" + Memo + "##" + ShipperPk + "\" /></div>");

		//////JubsuWayCL
		//////DocumentRequestCL
		if (documentrequestcl == "0" || documentrequestcl == "") {
			HtmlDocumentRequest = string.Empty;
		} else {
			HtmlDocumentRequest = "<div>" + GetGlobalResourceObject("RequestForm", "TradeDocument") + " : ";
			string[] Documents = documentrequestcl.Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
			foreach (string T in Documents) {
				switch (T) {
					case "10":
						HtmlDocumentRequest += GetGlobalResourceObject("RequestForm", "certificateoforigin") + " ";
						break;
					case "11":
						HtmlDocumentRequest += GetGlobalResourceObject("RequestForm", "FoodSanitation") + " ";
						break;
					case "12":
						HtmlDocumentRequest += GetGlobalResourceObject("RequestForm", "ElectricSafety") + " ";
						break;

					case "24":
						HtmlDocumentRequest += GetGlobalResourceObject("RequestForm", "ProductCheked") + " ";
						break;
					case "25":
						HtmlDocumentRequest += GetGlobalResourceObject("RequestForm", "SuChec") + " ";
						break;
					case "31":
						HtmlDocumentRequest += "화주원산지제공(客户提供产地证) ";
						break;
					case "32":
						HtmlDocumentRequest += "원산지신청(申请产地证) ";
						break;
					case "33":
						HtmlDocumentRequest += "단증보관(单证报关) ";
						break;
					case "34":
						HtmlDocumentRequest += "FTA원산지대리신청(FTA代理申请产地证)";
						break;

				}
			}
			HtmlDocumentRequest += "</div>";
		}
		//////DocumentRequestCL
		//////Payment
		HtmlPayment = new StringBuilder();
		switch (paymentwhocl) {
			case "5":
				HtmlPayment.Append("<div>" + GetGlobalResourceObject("RequestForm", "PaymentA") + "</div>");
				break;
			case "6":
				HtmlPayment.Append("<div>" + GetGlobalResourceObject("RequestForm", "PaymentB") + "</div>");
				break;
			case "7":
				HtmlPayment.Append("<div>" + GetGlobalResourceObject("RequestForm", "PaymentC") + "</div>");
				break;
			case "8":
				HtmlPayment.Append("<div>" + GetGlobalResourceObject("RequestForm", "PaymentD") + "</div>");
				break;
			default:
				HtmlPayment.Append("<div>" + GetGlobalResourceObject("RequestForm", "ETC") + "</div>");
				break;
		}

		//////Payment

		//////ETC
		//HtmlETC = memo != "" ? "<div> Memo : " + memo.Substring(memo.IndexOf("^^^") + 3, memo.Length - memo.IndexOf("^^^") - 3) + "</div>" : string.Empty;
		//////ETC
		//BTNList

		//발송지에서 하는거
		//BTNList
		StepCL = Int32.Parse(stepcl);
		LoadItemTable(itemmodifycount, CertificateCount, RequestFormPk);
		LoadBTNSUM(msgsendcount);
	}
	private void LoadFileList() {
		string fileROW = "<tr style=\"height:20px; \"><td class='{7}' >{1}</td><td class='{7}' style=\"text-align:left;\"><a href='../UploadedFiles/FileDownload.aspx?S={0}&T={8}' >{2}</a></td><td class='{7}' >{3}</td><td class='{7}' >{4}</td><td class='{7}' >{5}</td><td class='{7}' >{6}</td><td class='{7}' ><span onclick=\"FileDelete('{0}', '{8}');\" style='color:red;'>X</span></td></tr>";

		DB.SqlCmd.CommandText = "SELECT [ClearancedFilePk], [GubunCL], [PhysicalPath] FROM ClearancedFile WHERE RequestFormPk=" + RequestFormPk + " ORDER BY GubunCL;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder filelist = new StringBuilder();
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
				case "100":
					tempstring = "모음";
					break;
				case "101":
					tempstring = "수입신고필증";
					break;
				case "102":
					tempstring = "관부가세 납부영수증";
					break;
				case "103":
					tempstring = "관세사비 세금계산서";
					break;
				default:
					tempstring = "기타";
					break;
			}
			int gubuncl = Int32.Parse(RS["GubunCL"] + "");
			string[] temp = new string[] {
				RS["ClearancedFilePk"]+"",
				"&nbsp;",
				tempstring,
				"&nbsp;",
				gubuncl<100?
					"<img src=\"../Images/CheckTrue.jpg\" width=\"15\" height=\"15\" onclick=\"SetFileGubunCL2('"+RS["ClearancedFilePk"]+"', '"+(gubuncl+100)+"')\" />":
					"<img src=\"../Images/CheckFalse.jpg\" width=\"15\" height=\"15\" onclick=\"SetFileGubunCL2('"+RS["ClearancedFilePk"]+"', '"+(gubuncl-100)+"')\"  />",
				"&nbsp;",
				"&nbsp;",
				"TBody1G",
				"ClearancedFile"
			};
			filelist.Append(string.Format(fileROW, temp));
		}
		RS.Dispose();



		DB.SqlCmd.CommandText = "SELECT [FilePk], [Title], [GubunCL], [FileName], [AccountID], [Registerd] FROM [INTL2010].[dbo].[File] WHERE GubunCL in (11, 13, 15, 17) and GubunPk=" + RequestFormPk + ";";
		RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			int gubuncl = Int32.Parse(RS["GubunCL"] + "") - 10;
			string[] temp = new string[] {
				RS["FilePk"]+"",
				(RS["Title"]+""==""?"&nbsp;":RS["Title"]+""),
				((RS["FileName"] + "").Length > 30 ? (RS["FileName"] + "").Substring(0, 29) + "..." : RS["FileName"] + ""),
				//gubuncl%2==1?"<img src=\"../Images/CheckTrue.jpg\" width=\"15\" height=\"15\" onclick=\"SetFileGubunCL('"+RS["FilePk"]+"', '')\" />":"<img src=\"../Images/CheckFalse.jpg\" width=\"15\" height=\"15\" />",
				(gubuncl/2)%2==1?
					"<img src=\"../Images/CheckTrue.jpg\" width=\"15\" height=\"15\" onclick=\"SetFileGubunCL('"+RS["FilePk"]+"', '"+(gubuncl+8)+"')\"  />":
					"<img src=\"../Images/CheckFalse.jpg\" width=\"15\" height=\"15\" onclick=\"SetFileGubunCL('"+RS["FilePk"]+"', '"+(gubuncl+12)+"')\"  />",
				gubuncl>3?
					"<img src=\"../Images/CheckTrue.jpg\" width=\"15\" height=\"15\" onclick=\"SetFileGubunCL('"+RS["FilePk"]+"', '"+(gubuncl+6)+"')\" />":
					"<img src=\"../Images/CheckFalse.jpg\" width=\"15\" height=\"15\" onclick=\"SetFileGubunCL('"+RS["FilePk"]+"', '"+(gubuncl+14)+"')\"  />",
				RS["AccountID"]+"",
				(RS["Registerd"]+"").Substring(2, (RS["Registerd"]+"").Length-5),
				"TBody1",
				"File"
			};
			filelist.Append(string.Format(fileROW, temp));
			//filelist.Append(String.Format("<a href='../UploadedFiles/FileDownload.aspx?S={0}' >ㆍ{1} : {2} </a><span onclick=\"FileDelete('{0}');\" style='color:red;'>X</span><br />", RS[0] + "", RS[1] + "", RS[2] + ""));
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = "SELECT [FilePk], [Title], [GubunCL], [FileName], [AccountID], [Registerd] FROM [INTL2010].[dbo].[File] WHERE GubunCL in (18) and GubunPk=" + RequestFormPk + ";";
		RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			string[] temp = new string[] {
				RS["FilePk"]+"",
				(RS["Title"]+""==""?"&nbsp;":RS["Title"]+""),
				((RS["FileName"] + "").Length > 30 ? (RS["FileName"] + "").Substring(0, 29) + "..." : RS["FileName"] + ""),
			"<img src=\"../Images/CheckTrue.jpg\" width=\"15\" height=\"15\" />",
			"<img src=\"../Images/CheckTrue.jpg\" width=\"15\" height=\"15\" />",
				RS["AccountID"]+"",
				(RS["Registerd"]+"").Substring(2, (RS["Registerd"]+"").Length-5),
				"TBody1",
				"File"
			};
			filelist.Append(string.Format(fileROW, temp));
		}
		RS.Dispose();
		string BTN_FileUpload = "<input type=\"button\" onclick=\"FileUpload('" + RequestFormPk + "', '2')\" value=\"" + GetGlobalResourceObject("qjsdur", "fileupload") + "\" />";

		string BTN_OtherBLUpload = BL_UploadPk != "" ? "" : "&nbsp;&nbsp;&nbsp;<input type =\"button\" id=\"BTNUploadBLOK\" value=\"BL Upload\" onclick=\"SetRequestFormStep('OtherBL_Upload');\" />";
		//HtmlFileList = filelist + "" == "" ? BTN_FileUpload : "<fieldset><legend><strong>Attached File " + BTN_FileUpload + "</strong></legend>" + filelist + "</fieldset>";
		HtmlFileList = filelist + "" == "" ? BTN_FileUpload + BTN_OtherBLUpload :
		   "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:580px;\">" +
		   "	<tr>" +
		   "		<td class='THead1' style=\"width:100px; text-align:center;\" colspan=\"2\">Attached File " + BTN_FileUpload + BTN_OtherBLUpload + "</td>" +
		   //			"		<td style=\"width:18px; text-align:center;\">A</td>" +
		   "		<td class='THead1' style=\"width:18px; text-align:center;\">S</td>" +
		   "		<td class='THead1' style=\"width:18px; text-align:center;\">C</td>" +
		   "		<td class='THead1' style=\"width:50px; text-align:center;\">ID</td>" +
		   "		<td class='THead1' style=\"width:130px; text-align:center;\">Registerd</td>" +
		   "		<td class='THead1' style=\"width:18px; text-align:center;\">D</td>" +
		   "	</tr><tr>" + filelist + "</table>";
	}
	private void LoadFileList_Ever() {
		StringBuilder filelist = new StringBuilder();

		string fileROW = "<tr style=\"height:20px; \"><td class='{7}' >{1}</td><td class='{7}' style=\"text-align:left;\"><a href='../UploadedFiles/FileDownload.aspx?S={0}&T={8}' >{2}</a></td><td class='{7}' >{5}</td><td class='{7}' >{6}</td><td class='{7}' ><span onclick=\"FileDelete('{0}', '{8}');\" style='color:red;'>X</span></td></tr>";

		string TempConsigneePk = ConsigneePk == "" ? "0" : ConsigneePk;
		DB.SqlCmd.CommandText = "SELECT [FilePk], [Title], [GubunCL], [FileName], [AccountID], [Registerd] FROM [INTL2010].[dbo].[File] WHERE GubunCL in (99) and GubunPk=" + TempConsigneePk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			string[] temp = new string[] {
				RS["FilePk"]+"",
				(RS["Title"]+""==""?"&nbsp;":RS["Title"]+""),
				((RS["FileName"] + "").Length > 30 ? (RS["FileName"] + "").Substring(0, 29) + "..." : RS["FileName"] + ""),
			"<img src=\"../Images/CheckTrue.jpg\" width=\"15\" height=\"15\" />",
			"<img src=\"../Images/CheckTrue.jpg\" width=\"15\" height=\"15\" />",
				RS["AccountID"]+"",
				(RS["Registerd"]+"").Substring(2, (RS["Registerd"]+"").Length-5),
				"TBody1",
				"File"
			};
			filelist.Append(string.Format(fileROW, temp));
		}
		RS.Dispose();
		string BTN_FileUpload = "<input type=\"button\" onclick=\"FileUpload('" + ConsigneePk + "', '99')\" value=\"" + GetGlobalResourceObject("qjsdur", "fileupload") + "\" />";

		HtmlFileList_Ever =
		   "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:440px;\">" +
		   "	<tr>" +
		   "		<td class='THead1' style=\"width:100px; text-align:center;\" colspan=\"2\">Attached File By Ever " + BTN_FileUpload + "</td>" +
		   //			"		<td style=\"width:18px; text-align:center;\">A</td>" +
		   "		<td class='THead1' style=\"width:50px; text-align:center;\">ID</td>" +
		   "		<td class='THead1' style=\"width:130px; text-align:center;\">Registerd</td>" +
		   "		<td class='THead1' style=\"width:18px; text-align:center;\">D</td>" +
		   "	</tr><tr>" + filelist + "</table>";
	}
	private void LoadBTNSUM(string MSGCount) {
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = "SELECT COUNT(*) FROM [RequestModifyHistory] AS H WHERE H.ModifyWhich='Charge' and H.RequestFormPk=" + RequestFormPk + ";";
		string tempcount = DB.SqlCmd.ExecuteScalar() + "";
		string btnView_chargemodifyhistory = "";
		if (tempcount != "0") {
			btnView_chargemodifyhistory = "<input type=\"button\" onclick=\"SetRequestFormStep('ChargeHistory')\" value=\"CHARGE HISTORY(" + tempcount + ")\" />";
		}
		DB.DBCon.Close();
		string BTN_Clearance = "";

		if (DocumentStepCL == "") {
			BTN_Clearance = "<input type=\"button\" onclick=\"SetRequestFormStep('DepartureOK')\" value=\"Document OK\" />" +
			   "<input type=\"button\" onclick=\"SetRequestFormStep('Stoced')\" value=\"" + GetGlobalResourceObject("Admin", "dlqrhdhksfy") + "\" /></br>";
		} else if (BLCHECKCommercialDocumentPk != "") {
			string ShowBl = "";
			if (BL_UploadPk == "") {
				ShowBl = "<input type=\"button\" onclick=\"GoClearance('ShowBL', '" + BLCHECKCommercialDocumentPk + "')\" value=\"BL\" />";
			} else {
				ShowBl = "<input type=\"button\" onclick=\"ShowOtherBL('" + BL_UploadPk + "')\" value=\"BL\" />";
				//< a href = '../UploadedFiles/FileDownload.aspx?S="+ RequestFormPk + "' >

			}

			BTN_Clearance = "<input type=\"button\" onclick=\"GoClearance('Clearance', '" + BLCHECKCommercialDocumentPk + "')\" value=\"" + GetGlobalResourceObject("qjsdur", "xhdrhks") + "\" /> " +
ShowBl +
								 "<input type=\"button\" onclick=\"GoClearance('ShowInvoice', '" + BLCHECKCommercialDocumentPk + "')\" value=\"Invoice\" /> " +
								 "<input type=\"button\" onclick=\"GoClearance('ShowPacking', '" + BLCHECKCommercialDocumentPk + "')\" value=\"Packing\" /> " +
								 "<input type=\"button\" onclick=\"GoClearance('DO', '" + BLCHECKCommercialDocumentPk + "')\" value=\"DO\" /> ";
		} else if (MemberInformation[0] == "OurBranch" && BLCHECKCommercialDocumentPk == "" && DocumentStepCL == "0") {
			BTN_Clearance = "<input type=\"button\" id=\"BTNMakeBL\" onclick=\"SetRequestFormStep('LoadBLNo')\" value=\"Make BL\" /></br>";
		} else if (MemberInformation[1] == "3157" && SorC == "S" && StepCL == 57) {
			BTN_Clearance = "<input type=\"button\" id=\"BTNMakeBL\" onclick=\"SetRequestFormStep('LoadBLNo')\" value=\"Make BL\" /></br>";
		}


		if (DocumentStepCL != "" && StepCL == 58) {
			BTN_Clearance += "<input type=\"button\" onclick=\"DocumentStepCLTo()\" value=\"통관지시\" /></br>";
		} else {
			BTN_Clearance += "<br />";
		}

		string BTN_Modify = "<input type=\"button\" onclick=\"SetRequestFormStep('Modify')\" value=\"" + GetGlobalResourceObject("qjsdur", "wjqtnwmdtnwjd") + "\" />";
		string BTN_Stoced = "<input type=\"button\" onclick=\"SetRequestFormStep('Stoced')\" value=\"" + GetGlobalResourceObject("Admin", "dlqrhdhksfy") + "\" />";
		string BTN_Defer = "<input type=\"button\" onclick=\"SetRequestFormStep('defer')\" value=\"" + GetGlobalResourceObject("qjsdur", "qhfb") + "\" />";
		//string BTN_MsgSend = "<input type=\"button\" id=\"BTN_MsgPrepare\" onclick=\"SetRequestFormStep('SendAuto')\" value=\"메세지보내기\" />";
		string BTN_MsgSend = "<input type=\"button\" id=\"BTN_MsgPrepare\" onclick=\"SetRequestFormStep('Send2')\" value=\"" + GetGlobalResourceObject("qjsdur", "aptpwlqhsorl") + "\" />" +
		   (MSGCount == "0" ? "" : "<input type=\"button\" onclick=\"SetRequestFormStep('MsgHistory')\" value=\"" + GetGlobalResourceObject("qjsdur", "aptpwlqkfthdsodur") + "(" + MSGCount + ")\" />");
		string BTN_DeferRestore = "<input type=\"button\" onclick=\"SetRequestFormStep('deferRestore')\" value=\"" + GetGlobalResourceObject("qjsdur", "qhfbgowp") + "\" />";
		string BTN_RequestDelete = "<input type=\"button\" onclick=\"DeleteRequest('" + RequestFormPk + "')\" value=\"Delete\" />";

		string BTN_FixCharge = "<input type=\"button\" onclick=\"SetRequestFormStep('FixCharge')\" value=\"" + GetGlobalResourceObject("qjsdur", "dnsdlaghkrwjd") + "\" />";

		string BTN_ModifyCharge = "";
		if (!IsOnlyilic66BTN) {
			BTN_ModifyCharge = "<input type=\"button\" onclick=\"SetRequestFormStep('ModifyCharge')\" value=\"" + GetGlobalResourceObject("qjsdur", "dnsdlatnwjd") + "\" />";
		}

		string BTN_DeliveryCharge = "<input type=\"button\" onclick=\"SetRequestFormStep('DeliveryCharge')\" value=\"배달비추가\" />";


		if (MemberInformation[0] == "OurBranch" || MemberInformation[0] == "ShippingBranch") {
			switch (StepCL.ToString()) {
				case "50":
					HtmlButton = BTN_Modify + " " + BTN_Stoced + " " + BTN_RequestDelete;
					break;      //접수완료 관리자 읽지않음
				case "51":
					HtmlButton = BTN_Modify + " " + BTN_Stoced + " " + BTN_RequestDelete;
					break;  //접수완료 관리자 읽음
				case "52":
					HtmlButton = BTN_Modify + " " + BTN_DeferRestore + " " + BTN_RequestDelete;
					break;      //보류
				case "57":
					if (MemberInformation[1] == "3157" && SorC == "S") {
						HtmlButton = BTN_Clearance;
					}
					HtmlButton += BTN_Defer + " " + BTN_Modify + " " + BTN_MsgSend + " " + BTN_RequestDelete + "<br />" + BTN_FixCharge + BTN_Stoced;
					break;  //입고완료
				case "58":
					if (MemberInformation[2] == "ilic66") {
						HtmlButton = BTN_Clearance + " " + BTN_Defer + " " + BTN_Modify + " " + BTN_MsgSend + " " + BTN_RequestDelete + "<br />" + BTN_ModifyCharge;
					} else if (DocumentStepCL == "" || SorC == "C") {
						HtmlButton = BTN_Clearance + " " + BTN_Defer + " " + BTN_Modify + " " + BTN_MsgSend + " " + BTN_RequestDelete + "<br />" + BTN_ModifyCharge;
					} else {
						HtmlButton = BTN_Clearance + " " + BTN_MsgSend;
					}
					break;//운임확정
				case "63":
					HtmlButton = BTN_Clearance + "<br />" + BTN_MsgSend + (SorC == "C" ? "<br />" + BTN_ModifyCharge : "");
					IsConsigneeConfirmedStyle = "style=\" border:2px solid black; background-color:#FFFACD;\"";
					break;  //통관지시
				case "65":
					if (MemberInformation[2] == "ilic30" || MemberInformation[2] == "ilic31" || MemberInformation[2] == "ilic55" || MemberInformation[2] == "ilic66" || MemberInformation[2] == "ilic77" || MemberInformation[2] == "ilic01") {
						HtmlButton = BTN_Clearance + "<br />" + BTN_ModifyCharge + BTN_MsgSend;
					} else {
						HtmlButton = BTN_Clearance + BTN_MsgSend;
					}
					IsConsigneeConfirmedStyle = "style=\" border:2px solid black; background-color:#CDFACD;\"";
					break;
				default:
					HtmlButton = "";
					break;
			}
		} else {
			HtmlButton = BTN_MsgSend + BTN_DeliveryCharge;
		}

		HtmlButton += btnView_chargemodifyhistory;
	}
	private void LoadItemTable(string ModifyCount, string CertificateCount, string RequestFormPk) {
		HtmlItem = new StringBuilder();
		if (Int32.Parse(ModifyCount) != 0) {
			ModifyCount = "<input type=\"button\" value=\"수정내역 (" + ModifyCount + ")\" onclick=\"PopupItemModifyList('" + RequestFormPk + "')\" style=\"width:100px; height:20px;\" />";
		} else {
			ModifyCount = "";
		}
		if (CertificateCount != "") {
			CertificateCount = "<input type=\"button\" value=\"인증서\" onclick=\"PopupCertificateList('" + RequestFormPk + "')\" style=\"height:20px;\" />";
		}

		if (StepCL < 58) {
			ModifyCount = "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "audtptnwjd") + "\" onclick=\"PopupItemModify('" + RequestFormPk + "', '" + MemberInformation[2] + "')\" style=\"width:100px; height:20px;\" /> " + ModifyCount;
		} else if (StepCL == 58) {
			if (MemberInformation[2] == "ilic66" || MemberInformation[2] == "ilman" || MemberInformation[2] == "ilic01" || MemberInformation[2] == "ilic00" || MemberInformation[2] == "ilic03") {
				ModifyCount = "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "audtptnwjd") + "\" onclick=\"PopupItemModify('" + RequestFormPk + "', '" + MemberInformation[2] + "')\" style=\"width:100px; height:20px;\" /> " + ModifyCount;
			} else if (DocumentStepCL == "" || SorC == "C") {
				ModifyCount = "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "audtptnwjd") + "\" onclick=\"PopupItemModify('" + RequestFormPk + "', '" + MemberInformation[2] + "')\" style=\"width:100px; height:20px;\" /> " + ModifyCount;
			}
		} else {
			if (MemberInformation[2] == "ilic66" || MemberInformation[2] == "ilic03" || MemberInformation[2] == "ilic06") {
				ModifyCount = "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "audtptnwjd") + "\" onclick=\"PopupItemModify('" + RequestFormPk + "', '" + MemberInformation[2] + "')\" style=\"width:100px; height:20px;\" /> " + ModifyCount;
			}

		}
		HtmlItem.Append("	<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:580px; \" >" +
							 "		<tr><td align=\"right\" colspan='8'>" +
							 CertificateCount + ModifyCount + "</td></tr>" +
							 "		<tr style=\"height:30px;\" >" +
							 "			<td bgcolor=\"#F5F5F5\" height=\"20\" align=\"center\" width=\"45px\" >" + GetGlobalResourceObject("RequestForm", "BoxNo") + "</td>" +
							 "			<td bgcolor=\"#F5F5F5\" align=\"center\" >" +
										 GetGlobalResourceObject("RequestForm", "Description") + " / " +
										 GetGlobalResourceObject("RequestForm", "Label") + " / " +
										 GetGlobalResourceObject("RequestForm", "Material") + "</td>" +
							 "			<td bgcolor=\"#F5F5F5\" align=\"center\" width=\"65\">" + GetGlobalResourceObject("RequestForm", "Count") + "</td>" +
							 "			<td bgcolor=\"#F5F5F5\" align=\"center\" width=\"50\">" + GetGlobalResourceObject("RequestForm", "UnitCost") + "</td>" +
							 "			<td bgcolor=\"#F5F5F5\" align=\"center\" width=\"75\">" + GetGlobalResourceObject("RequestForm", "Amount") + "</td>" +
							 "			<td bgcolor=\"#F5F5F5\" align=\"center\" width=\"50\">" + GetGlobalResourceObject("RequestForm", "PackingCount") + "</td>" +
							 "			<td bgcolor=\"#F5F5F5\" align=\"center\" width=\"45\">kg</td>" +
							 "			<td bgcolor=\"#F5F5F5\" align=\"center\" width=\"30\">cbm</td>" +
							 "		</tr>");

		DB.SqlCmd.CommandText = "EXEC SP_SelectItemWithRequestFormPk @RequestFormPk = " + RequestFormPk + ";";
		Decimal PriceSum = 0;
		Decimal weightSum = 0;
		Decimal volumeSum = 0;
		int CTSum = 0;
		int ROSum = 0;
		int PASum = 0;
		string monetaryunit = string.Empty;
		bool flagfirst = true;
		/*
	 SELECT 
		RFI.RequestFormItemsPk, RFI.ItemCode, RFI.MarkNNumber, RFI.Description, RFI.Label, 
		RFI.Material, RFI.Quantity, RFI.QuantityUnit, RFI.PackedCount, RFI.PackingUnit, 
		RFI.GrossWeight, RFI.Volume, RFI.UnitPrice, RFI.Amount, RFI.LastModify, 
		RF.MonetaryUnitCL 
	 FROM 
		RequestForm AS RF
		left join RequestFormItems AS RFI On RF.RequestFormPk=RFI.RequestFormPk 
	 WHERE RF.RequestFormPk=@RequestFormPk
		 */
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			if (flagfirst) {
				monetaryunit = Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "");
				flagfirst = false;
			}
			HtmlItem.Append("<tr style=\"height:25px; \"><td align='center' class='ItemTableIn'>" + RS["MarkNNumber"] + "</td>");

			if (RS["Label"] + "" != "" || RS["Material"] + "" != "") {
				HtmlItem.Append("<td align='left' style='padding-left:5px;' class='ItemTableIn'>" + RS["Description"] + " / " + RS["Label"] + " / " + RS["Material"] + "</td>");
			} else {
				HtmlItem.Append("<td align='left' style='padding-left:5px;' class='ItemTableIn'>" + RS["Description"] + "</td>");
			}

			if (RS["Quantity"] + "" != "") {
				switch (RS["QuantityUnit"] + "") {
					case "40":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " PCS" + "</td>");
						break;
					case "41":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " PRS" + "</td>");
						break;
					case "42":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " SET" + "</td>");
						break;
					case "43":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " S/F" + "</td>");
						break;
					case "44":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " YDS" + "</td>");
						break;
					case "45":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " M" + "</td>");
						break;
					case "46":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " KG" + "</td>");
						break;
					case "47":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " DZ" + "</td>");
						break;
					case "48":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " L" + "</td>");
						break;
					case "49":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " BOX" + "</td>");
						break;
					case "50":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " SQM" + "</td>");
						break;
					case "51":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " M2" + "</td>");
						break;
					case "52":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " RO" + "</td>");
						break;
					default:
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + Common.NumberFormat(RS["Quantity"] + "") + " " + "</td>");
						break;
				}
			} else {
				HtmlItem.Append("<td align='center' class='ItemTableIn'>&nbsp;</td>");
			}

			HtmlItem.Append("<td align='center' class='ItemTableIn'>" + monetaryunit + " " + Common.NumberFormat(RS["UnitPrice"] + "") + "</td>");


			HtmlItem.Append("<td align='center' class='ItemTableIn'>" + (RS["Amount"] + "" == "" ? "&nbsp;" : monetaryunit + " " + Common.NumberFormat(RS["Amount"] + "")) + "</td>");

			if (RS["PackedCount"] + "" != "") {
				switch (RS["PackingUnit"] + "") {
					case "15":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + RS["PackedCount"] + " CT" + "</td>");
						CTSum += int.Parse(RS["PackedCount"] + "");
						break;
					case "16":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + RS["PackedCount"] + " RO" + "</td>");
						ROSum += int.Parse(RS["PackedCount"] + "");
						break;
					case "17":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + RS["PackedCount"] + " PA" + "</td>");
						PASum += int.Parse(RS["PackedCount"] + "");
						break;
					case "18":
						HtmlItem.Append("<td align='center' class='ItemTableIn'>" + RS["PackedCount"] + " M2" + "</td>");
						PASum += int.Parse(RS["PackedCount"] + "");
						break;
				}
			} else {
				HtmlItem.Append("<td align='center' class='ItemTableIn'>&nbsp;</td>");
			}

			HtmlItem.Append("<td align='center' class='ItemTableIn'>" + (RS["GrossWeight"] + "" == "" ? "&nbsp;" : Common.NumberFormat(RS["GrossWeight"] + "")) + "</td>");
			HtmlItem.Append("<td align='center' class='ItemTableIn'>" + (RS["Volume"] + "" == "" ? "&nbsp;" : Common.NumberFormat(RS["Volume"] + "")) + "</td></tr>");

			if (RS["Amount"] + "" != "") {
				PriceSum += decimal.Parse(RS["Amount"] + "");
			}
			if (RS["GrossWeight"] + "" != "") {
				weightSum += decimal.Parse(RS["GrossWeight"] + "");
			}
			if (RS["Volume"] + "" != "") {
				volumeSum += decimal.Parse(RS["Volume"] + "");
			}
		}
		RS.Dispose();
		DB.DBCon.Close();

		string tempCT = CTSum != 0 ? CTSum + "CT " : string.Empty;
		string tempRO = ROSum != 0 ? ROSum + "RO " : string.Empty;
		string tempPA = PASum != 0 ? PASum + "PA " : string.Empty;

		//Response.Write(StepCL + "");
		if (StepCL < 55 || StepCL == 56) {
			HtmlItem.Append("<tr><td bgcolor=\"#F5F5F5\" height=\"30\" align=\"right\" colspan='8'>" + GetGlobalResourceObject("qjsdur", "chdqkrtm") + " : <strong>" + tempCT + tempRO + tempPA + "</strong> " + GetGlobalResourceObject("qjsdur", "chdwndfid") + " : <strong>" + Common.NumberFormat(weightSum + "") + "Kg</strong> " + GetGlobalResourceObject("qjsdur", "chdcpwjr") + " : <strong>" + Common.NumberFormat(volumeSum + "") + "CBM</strong></td></tr></table>");
		} else {
			HtmlItem.Append("<tr><td bgcolor=\"#F5F5F5\" height=\"30\" align=\"right\" colspan='8'>");
		}
	}
	private void LoadAdditionalData(string RequestFormPk, decimal CarryOverS, decimal CarryOverC) {
		DB.SqlCmd.CommandText = @"SELECT	RF.[TotalPackedCount], RF.[PackingUnit], RF.[TotalGrossWeight], RF.[TotalVolume], 
			RF.[CriterionValue], RF.[ExchangeDate], RFCH.[MONETARY_UNIT], RFCH.[TOTAL_PRICE], 
			RFCH.[LAST_DEPOSITED_DATE], RFCH.[DEPOSITED_PRICE],
			SP.[TITLE], 
			BranchBank.BankName AS BBankName, BranchBank.OwnerName AS BBankOwnerName, 
			BranchBank.AccountNo AS BBankAccountNo, BranchBank.BankMemo AS BBankMemo, 
			CustomerBank.BankName AS CBankName, CustomerBank.OwnerName AS CBankOwnerName, 
			CustomerBank.AccountNo AS CBankAccountNo, CustomerBank.BankMemo AS CBankMemo 
	FROM	[dbo].[RequestForm] AS RF
				LEFT JOIN [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON RF.[RequestFormPk] = RFCH.[TABLE_PK]
				left join [dbo].[STANDARDPRICE_LIST] AS SP on RF.[StandardPricePk]=SP.[STANDARDPRICE_PK]
				left join CompanyBank AS BranchBank on RFCH.[BRANCH_BANK_PK]=BranchBank.CompanyBankPk
				left join CompanyBank AS CustomerBank on RFCH.[CUSTOMER_BANK_PK]=CustomerBank.CompanyBankPk
	WHERE RF.[RequestFormPk] = " + RequestFormPk + @"
	AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm' ";
		string TotalGrossWeight, TotalVolume, CriterionValue, ExchangeDate, MonetaryUnit, Charge, Title, BBankName, BBankOwnerName, BBankAccount, BBankMemo, CBankName, CBankOwnerName, CBankAccount, CBankMemo;

		string PaymentRowS = "<tr style=\"height:25px; text-align:right;\"><td >{0}</td><td style=\"text-align:right; padding-right:10px; \">{1}</td><td style=\"text-align:right; padding-right:10px; \">&nbsp;</td></tr>";
		string PaymentRowC = "<tr style=\"height:25px; text-align:right; \"><td >{0}</td><td style=\"text-align:right; padding-right:10px; \">&nbsp;</td><td style=\"text-align:right; padding-right:10px; \">{1}</td></tr>";
		string PaymentRowTotal = "	<tr  style=\"height:30px;\">" +
									  "		<td style=\"background-color:{0}; text-align:right;\" >{1}</td><td style=\"background-color:{0}; text-align:right; padding-right:10px; \" >{2}</td>" +
									  "		<td style=\"background-color:{0}; text-align:right; padding-right:10px; \" >{3}</td></tr>";
		string DeliveryRow = "";
		decimal DeliveryChargeS = 0;
		decimal DeliveryChargeC = 0;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		
		if (RS.Read()) {
			TotalPackedCount = RS["TotalPackedCount"] + "";
			PackingUnit = RS["PackingUnit"] + "";
			TotalGrossWeight = RS["TotalGrossWeight"] + "";
			TotalVolume = RS["TotalVolume"] + "";
			CriterionValue = RS["CriterionValue"] + "";
			ExchangeDate = RS["ExchangeDate"] + "";
			MonetaryUnit = RS["MONETARY_UNIT"] + "";
			PaymentShipperMonetary = MonetaryUnit;
			Charge = RS["TOTAL_PRICE"] + "";
			PaymentConsigneeMonetary = MonetaryUnit;
			Title = RS["Title"] + "";
			BBankName = RS["BBankName"] + "";
			BBankOwnerName = RS["BBankOwnerName"] + "";
			BBankAccount = RS["BBankAccountNo"] + "";
			BBankMemo = RS["BBankMemo"] + "";
			CBankName = RS["CBankName"] + "";
			CBankOwnerName = RS["CBankOwnerName"] + "";
			CBankAccount = RS["CBankAccountNo"] + "";
			CBankMemo = RS["CBankMemo"] + "";
		} else {
			RS.Dispose();
			DB.DBCon.Close();
			HtmlItem.Append("</td></tr></table>");
			return;
		}
		RS.Dispose();
		HtmlItem.Append(" &nbsp;&nbsp;&nbsp;&nbsp;<strong>" + GetGlobalResourceObject("qjsdur", "chdqkrtm") + " : " + Common.NumberFormat(TotalPackedCount) + " " + Common.GetPackingUnit(PackingUnit) + " " + GetGlobalResourceObject("qjsdur", "chdwndfid") + " : " + Common.NumberFormat(TotalGrossWeight) + "Kg " + GetGlobalResourceObject("qjsdur", "chdcpwjr") + " : " + Common.NumberFormat(TotalVolume) + "CBM " + "</strong></td></tr></table>");

		DB.SqlCmd.CommandText = "SELECT [CATEGORY], [TITLE], [ORIGINAL_PRICE], [ORIGINAL_MONETARY_UNIT] FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE [REQUESTFORMCALCULATE_HEAD_PK]=" + CalcHeadPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		StringBuilder tempstringbuilder = new StringBuilder();
		while (RS.Read()) {
			string tempstring = RS["TITLE"] + "";
			if (RS["CATEGORY"] + "" != "대행비") {
				tempstringbuilder.Append(string.Format(PaymentRowS, tempstring, Common.GetMonetaryUnit(RS["ORIGINAL_MONETARY_UNIT"] + "") + " " + Common.NumberFormat(RS["ORIGINAL_PRICE"] + "")));
			} else {
				DeliveryRow = String.Format(PaymentRowTotal, (IsOnlyilic66BTN ? "#FAEBD7" : "#D3D3D3"), tempstring, Common.GetMonetaryUnit(RS["ORIGINAL_MONETARY_UNIT"] + "") + " " + Common.NumberFormat(RS["ORIGINAL_PRICE"] + ""));
					DeliveryChargeS = decimal.Parse(RS["ORIGINAL_PRICE"] + "");
					DeliveryChargeC = decimal.Parse(RS["ORIGINAL_PRICE"] + "");
			}
		}
		RS.Dispose();
		HtmlPayment.Append("<div>" + Title + "</div>" +
					"	<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:430px; border:1px solid #333333;\" >" +
					"		<tr style=\"height:30px; \">" +
					"			<td style=\"text-align:center; font-weight:bold; border-bottom:1px solid #333333;\">Item</td>" +
					"			<td style=\"text-align:center; font-weight:bold; width:110px; border-bottom:1px solid #333333;\" >Charge</td>" +
					"		</tr>" + tempstringbuilder +
					String.Format(PaymentRowTotal, (IsOnlyilic66BTN ? "#FAEBD7" : "#D3D3D3"), "운임 계", Charge == "0.0000" ? "&nbsp" : Common.GetMonetaryUnit(MonetaryUnit) + " " + Common.NumberFormat(Charge), Charge == "0.0000" ? "&nbsp" : Common.GetMonetaryUnit(MonetaryUnit) + " " + Common.NumberFormat(Charge)));

		DB.SqlCmd.CommandText = "SELECT [CommercialDocumentHeadPk], [Title], [MonetaryUnitCL], [Value] FROM CommercialDocumentTariff WHERE GubunPk=" + RequestFormPk + " and GubunCL=0 and Value<>0 ;";
		DB.SqlCmd.CommandText = @"SELECT  RFCB.[TITLE] AS Title, RFCB.[ORIGINAL_MONETARY_UNIT] AS MonetaryUnitCL, RFCB.[ORIGINAL_PRICE] AS Value
			FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH
			LEFT JOIN ( 
						SELECT [TITLE], [ORIGINAL_MONETARY_UNIT], [ORIGINAL_PRICE] 
						FROM [dbo].[REQUESTFORMCALCULATE_BODY] 
						WHERE [CATEGORY] = '제세금' 
						) AS RFCB ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK] = RFCH.[REQUESTFORMCALCULATE_HEAD_PK]
						WHERE ISNULL(RFCB.[ORIGINAL_PRICE], 0) <> 0 
						AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
						AND RFCH.[TABLE_PK] = " + RequestFormPk;
		RS = DB.SqlCmd.ExecuteReader();
		decimal TariffS = 0;
		decimal TariffC = 0;
		bool CheckTariff = false;
		string TariffMonetaryUnit = "";
		while (RS.Read()) {
			HtmlPayment.Append(string.Format(PaymentRowC, RS["Title"] + "", Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "") + " " + Common.NumberFormat(RS["Value"] + "")));
			TariffC += Decimal.Parse(RS["Value"] + "");
			TariffMonetaryUnit = RS["MonetaryUnitCL"] + "";
			CheckTariff = true;
		}
		RS.Dispose();

		DB.DBCon.Close();

		if (CheckTariff) {
			if (TariffS != 0 && MonetaryUnit != TariffMonetaryUnit) {
				//19!18!20110602!6.4700@@20!18!20110602!60.2000@@20!18!20110602!60.2000@@20!18!20110602!60.2000@@20!18!20110602!60.2000@@ 
				string tempExchangedDate = ExchangeDate;
				string temp = "";
				decimal tempTariffS = new Admin().GetExchangeRated(TariffMonetaryUnit, MonetaryUnit, TariffS, out temp, tempExchangedDate);
				//Response.Write(tempTariffS);

				switch (MonetaryUnit) {
					case "18":
						TariffS = Math.Round(tempTariffS, 1, MidpointRounding.AwayFromZero);
						break;
					case "19":
						TariffS = Math.Round(tempTariffS, 2, MidpointRounding.AwayFromZero);
						break;
					case "20":
						TariffS = Math.Round(tempTariffS, 0, MidpointRounding.AwayFromZero);
						break;
					default:
						TariffS = tempTariffS;
						break;
				}
			}

			HtmlPayment.Append(String.Format(PaymentRowTotal, "#F0E68C", "통관비 계", TariffS == 0 ? "&nbsp;" : Common.GetMonetaryUnit(MonetaryUnit) + " " + Common.NumberFormat(TariffS + ""), TariffC == 0 ? "&nbsp;" : Common.GetMonetaryUnit(MonetaryUnit) + " " + Common.NumberFormat(TariffC + "")));
		}
		HtmlPayment.Append(DeliveryRow);

		ShipperTotalCharge = decimal.Parse(Charge == "" ? "0" : Charge) + TariffS + DeliveryChargeS;
		ConsigneeTotalCharge = decimal.Parse(Charge == "" ? "0" : Charge) + TariffC + DeliveryChargeC;
		HtmlPayment.Append(String.Format(PaymentRowTotal,
		   "#FFFFFF",
		   "TOTAL",
		   ShipperTotalCharge == 0 ? "&nbsp;" : Common.GetMonetaryUnit(MonetaryUnit) + " " + Common.NumberFormat("" + ShipperTotalCharge),
		   ConsigneeTotalCharge == 0 ? "&nbsp;" : Common.GetMonetaryUnit(MonetaryUnit) + " " + Common.NumberFormat("" + ConsigneeTotalCharge)));
		string TempHtml = string.Empty;

		HtmlPayment.Append("</table>");
		HtmlDeposited = new StringBuilder();	
		string BTN_CollectPaymentByS = "<input type=\"button\" onclick=\"SetRequestFormStep('CollectByS');\" style=\"padding:0px; \" value=\"Collect By Shipper\" />";
		string BTN_CollectPaymentByC = "<input type=\"button\" onclick=\"SetRequestFormStep('CollectByC');\" style=\"padding:0px; \" value=\"Collect By Consignee\" />";
		string BTN_ShipperCharge = "<input type=\"button\" onclick=\"SetRequestFormStep('ShipperCharge')\" style=\"padding:0px; \" value=\"" + GetGlobalResourceObject("qjsdur", "qkfghkdlsdnsdla") + "\" />";
		string BTN_ConsigneeCharge = "<input type=\"button\" onclick=\"SetRequestFormStep('ConsigneeCharge')\" style=\"padding:0px; \" value=\"" + GetGlobalResourceObject("qjsdur", "tngkdlsdnsdla") + "\" />";
		string BTN_ConsigneeCharge_ASECO = "<input type=\"button\" onclick=\"SetRequestFormStep('ConsigneeCharge_ASECO')\" style=\"padding:0px; \" value=\"수하인운임_ASECO\" />";
		HtmlButtonPayment = "";
		if (MemberInformation[0] == "OurBranch") {
			HtmlButtonPayment += BTN_ShipperCharge + " " + BTN_ConsigneeCharge;
			if (Session["ID"] +"" == "ilic03" || Session["ID"] + "" == "ilic06" || Session["ID"] + "" == "ilic07" || Session["ID"] + "" == "ilic08" || Session["ID"] + "" == "ilic31" || Session["ID"] + "" == "ilic32" || Session["ID"] + "" == "ilic66") {
				HtmlButtonPayment += BTN_ConsigneeCharge_ASECO;
			}
			if (ShipperTotalCharge > 0) {
				HtmlButtonPayment += " " + BTN_CollectPaymentByS;
			}
			if (ConsigneeTotalCharge > 0) {
				HtmlButtonPayment += " " + BTN_CollectPaymentByC;
			}
			if (Session["ID"] + "" == "ilic30" || Session["ID"] + "" == "ilic01") {
				HtmlButtonPayment += "<input type=\"button\" onclick=\"SetRequestFormStep('ConsigneeChargeFI')\" style=\"padding:0px; \" value=\"FI\" />";
			}			
		} else {
			HtmlButtonPayment += BTN_ShipperCharge + " " + BTN_ConsigneeCharge;
		}

		if (ShipperTotalCharge > 0) {
			HtmlPayment.Append("<div style=\"margin-top:10px; width:430px;\"><span style=\"font-weight:bold;\">shipper bank</span> " + BBankOwnerName + " " + BBankName + " " + BBankAccount + "</div>");
		}
		if (ConsigneeTotalCharge > 0) {
			HtmlPayment.Append("<div style=\"margin-top:10px; width:430px;\"><span style=\"font-weight:bold;\">consign bank</span> " + CBankOwnerName + " " + CBankName + " " + CBankAccount + "</div>");
		}
	}

	private void LoadRequestComment(string RequestFormPk, string AccountID) {
		StringBuilder temp = new StringBuilder();
		DB = new DBConn();
		DB.DBCon.Open();
		HistoryC HisC = new HistoryC();
		List<sComment> Shipper = new List<sComment>();
		List<sComment> Consignee = new List<sComment>();
		if (ConsigneePk == "") {
			Shipper = HisC.LoadList_Comment("Company", ShipperPk, "'Basic_Important'", ref DB);
		}
		else {
			Shipper = HisC.LoadList_Comment("Company", ShipperPk, "'Basic_Important'", ref DB);
			Consignee = HisC.LoadList_Comment("Company", ConsigneePk, "'Basic_Important'", ref DB);
		}
		for (int i = 0; i < Shipper.Count; i++) {
			string deleteButton = "";
			string bgcolor = "#FAEBD7";
			if (Shipper[i].Account_Id == AccountID || AccountID == "ilman") {
				deleteButton = "<span style=\"color:red; cursor:pointer;\" onclick=\"CommentDelete('" + Shipper[i].Comment_Pk + "')\">X</span>";
			}
			temp.Append("<div class=\"Line1E8E8E8\" style=\"padding:3px; background-color:" + bgcolor + ";\" >" + (Shipper[i].Contents).Replace("\r\n", "<br />") + " <span style=\"color:gray;\">" + (Shipper[i].Registerd) + "</span> <span style=\"color:Blue;\">" + Shipper[i].Account_Id + "</span>" + deleteButton + "</div>");
		}
		for (int i = 0; i < Consignee.Count; i++) {
			string deleteButton = "";
			string bgcolor = "#C0C0C0";
			if (Consignee[i].Account_Id == AccountID || AccountID == "ilman") {
				deleteButton = "<span style=\"color:red; cursor:pointer;\" onclick=\"CommentDelete('" + Consignee[i].Comment_Pk + "')\">X</span>";
			}
			temp.Append("<div class=\"Line1E8E8E8\" style=\"padding:3px; background-color:" + bgcolor + ";\" >" + (Consignee[i].Contents).Replace("\r\n", "<br />") + " <span style=\"color:gray;\">" + (Consignee[i].Registerd) + "</span> <span style=\"color:Blue;\">" + Consignee[i].Account_Id + "</span>" + deleteButton + "</div>");
		}
		HtmlCommentListColored = temp + "";

		temp = new StringBuilder();
		List<sComment> Request = new List<sComment>();
		Request = HisC.LoadList_Comment("RequestForm", RequestFormPk, "'Request', 'Request_Confirm'", ref DB);
		for (int i = 0; i < Request.Count; i++) {
			string deleteButton = "";
			string AddCmt = "";
			if (Request[i].Account_Id == AccountID || AccountID == "ilman") {
				deleteButton = "<span style=\"color:red; cursor:pointer;\" onclick=\"CommentDelete('" + Request[i].Comment_Pk + "')\">X</span>";
			}
			if (Request[i].Category == "Request_Confirm") {
				AddCmt = "[입고확인 Comment] " + Request[i].Contents;
			}
			else {
				AddCmt = Request[i].Contents;
			}
			temp.Append("<div class=\"Line1E8E8E8\" style=\"padding:3px;\">" + (AddCmt).Replace("\r\n", "<br />") + " <span style=\"color:gray;\">" + (Request[i].Registerd) + "</span> <span style=\"color:Blue;\">" + Request[i].Account_Id + "</span>" + deleteButton + "</div>");
		}
		DB.DBCon.Close();
		HtmlCommentList = temp + "";
	}

	private String LoadDeposied(string SorC, string RequestFormPk, ref decimal Aleady) {
		StringBuilder ReturnValue = new StringBuilder();
		string WhereGubun = SorC == "S" ? "RFD.GubunCL=0" : "RFD.GubunCL=1";
		DB.SqlCmd.CommandText = @"	SELECT RFD.RequestFormDepositedPk, RFD.MonetaryUnitCL, RFD.Charge, RFD.DepositedDate, RFD.Confirmed, CB.BankName, CB.OwnerName 
															FROM RequestFormDeposited AS RFD 
																left join CompanyBank AS CB ON RFD.BankAccountPk=CB.CompanyBankPk 
															WHERE RFD.RequestFormPk=" + RequestFormPk + " and " + WhereGubun +
											   " ORDER BY RFD.DepositedDate ASC ";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			Aleady += decimal.Parse(RS["Charge"] + "");
			string tempIsinorout = "入";
			string tempStyle = RS["Confirmed"] + "" == "" ? "" : "font-weight:bold;";
			if ((RS[3] + "").Substring(0, 1) == "-") {
				tempIsinorout = "出";
			}

			ReturnValue.Append("<tr style=\"height:30px;\"><td style=\"text-align:center; " + tempStyle + "\">" + (RS["DepositedDate"] + "").Substring(4, 2) + " / " + (RS["DepositedDate"] + "").Substring(6, 2) + "</td><td style=\"text-align:center; " + tempStyle + "\">" +
						   RS["BankName"] + "</td><td style=\"text-align:center; " + tempStyle + "\">" +
						   tempIsinorout + "</td><td style=\"text-align:right; padding-right:5px; \" >" +
						   Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "") + " " + Common.NumberFormat(RS["Charge"] + "") + "</td></tr>");
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"SELECT [AttachedRequestFormPk]
      ,[Price]
  FROM [dbo].[RequestFormCalculateCarryover]
  WHERE [OriginalRequestFormPk]=" + RequestFormPk + " and SorC='" + SorC + "';";
		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue.Append("<tr style=\"height:30px;\"><td colspan='3' style='text-align:right;' ><a href='../Admin/RequestView.aspx?g=c&pk=" + RS["AttachedRequestFormPk"] + "' ><span style='color:red; font-weight:bold; '>이월됨</span></a></td><td style=\"text-align:right; padding-right:5px; \" >" + Common.NumberFormat(RS["Price"] + "") + "</td></tr>");
		}
		RS.Dispose();

		DB.DBCon.Close();
		return ReturnValue + "";
	}
	private void LoadCompanyDepositList() {
		StringBuilder Temp = new StringBuilder();

		Temp.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:300px; border:1px black solid; \"><tr style=\"height:30px;\"><td colspan=\"4\" style=\"text-align:center; font-weight:bold;\">SHIPPER</td></tr>");
		Temp.Append("<tr style=\"height:30px;\"><td style=\"width:55px; text-align:center;\" >DATE</td><td style=\"width:100px; text-align:center;\" >BANK</td><td style=\"width:20px; text-align:center;\" >T</td><td style=\"text-align:right; padding-right:5px; \" >" + Common.GetMonetaryUnit(PaymentShipperMonetary) + " " + Common.NumberFormat(ShipperTotalCharge + "") + "</td></tr>");
		decimal aleady = 0;
		Temp.Append(LoadDeposied("S", RequestFormPk, ref aleady));
		Temp.Append("<tr style=\"height:30px;\"><td colspan=\"3\" style=\"text-align:right; padding-right:20px; background-color:#DDDDDD;\">left</td><td style=\"text-align:right; padding-right:5px; \" >" + Common.GetMonetaryUnit(PaymentShipperMonetary) + " " + Common.NumberFormat("" + (aleady - ShipperTotalCharge)) + "</td></tr>");
		Temp.Append("</table>");
		string styleTop = "";

		if (aleady != 0 || ShipperTotalCharge != 0) {
			HtmlDeposited.Append(Temp.ToString());
			styleTop = "margin-top:20px; ";
		}
		Temp = new StringBuilder();

		aleady = 0;
		Temp.Append("	<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:300px; border:1px black solid; " + styleTop + " \"><tr style=\"height:30px;\"><td colspan=\"4\" style=\"text-align:center; font-weight:bold;\">CONSIGNEE</td></tr>" +
								   "	<tr style=\"height:30px;\"><td style=\"width:55px; text-align:center;\" >DATE</td><td style=\"width:100px; text-align:center;\" >BANK</td><td style=\"width:20px; text-align:center;\" >T</td><td style=\"text-align:right; padding-right:5px; \" >" + Common.GetMonetaryUnit(PaymentConsigneeMonetary) + " " + Common.NumberFormat("" + ConsigneeTotalCharge) + "</td></tr>");
		Temp.Append(LoadDeposied("C", RequestFormPk, ref aleady));
		Temp.Append("<tr style=\"height:30px;\"><td colspan=\"3\" style=\"text-align:right; padding-right:20px; background-color:#DDDDDD;\">left</td><td style=\"text-align:right; padding-right:5px; \" >" + Common.GetMonetaryUnit(PaymentConsigneeMonetary) + " " + Common.NumberFormat((aleady - ConsigneeTotalCharge) + "") + "</td></tr>");
		Temp.Append("</table>");

		if (aleady != 0 || ConsigneeTotalCharge != 0) {
			HtmlDeposited.Append(Temp.ToString());
		}
	}
	//private void LoadCompanyDepositList()
	//{
	//    if (ShipperTotalCharge > 0)
	//    {
	//        HtmlDeposited.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:300px; border:1px black solid; \"><tr style=\"height:30px;\"><td colspan=\"4\" style=\"text-align:center; font-weight:bold;\">SHIPPER</td></tr>");
	//        HtmlDeposited.Append("<tr style=\"height:30px;\"><td style=\"width:55px; text-align:center;\" >DATE</td><td style=\"width:100px; text-align:center;\" >BANK</td><td style=\"width:20px; text-align:center;\" >T</td><td style=\"text-align:right; padding-right:5px; \" >" + Common.GetMonetaryUnit(PaymentShipperMonetary) + " " + Common.NumberFormat(ShipperTotalCharge + "") + "</td></tr>");
	//        decimal aleady = 0;
	//        HtmlDeposited.Append(LoadDeposied("S", RequestFormPk, ref aleady));
	//        HtmlDeposited.Append("<tr style=\"height:30px;\"><td colspan=\"3\" style=\"text-align:right; padding-right:20px; background-color:#DDDDDD;\">left</td><td style=\"text-align:right; padding-right:5px; \" >" + Common.GetMonetaryUnit(PaymentShipperMonetary) + " " + Common.NumberFormat("" + (aleady - ShipperTotalCharge)) + "</td></tr>");
	//        HtmlDeposited.Append("</table>");
	//    }
	//    if (ConsigneeTotalCharge > 0)
	//    {
	//        string styleTop = "";
	//        if (ShipperTotalCharge > 0)
	//        {
	//            styleTop = "margin-top:20px; ";
	//        }
	//        HtmlDeposited.Append("	<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:300px; border:1px black solid; " + styleTop + " \"><tr style=\"height:30px;\"><td colspan=\"4\" style=\"text-align:center; font-weight:bold;\">CONSIGNEE</td></tr>" +
	//                                   "	<tr style=\"height:30px;\"><td style=\"width:55px; text-align:center;\" >DATE</td><td style=\"width:100px; text-align:center;\" >BANK</td><td style=\"width:20px; text-align:center;\" >T</td><td style=\"text-align:right; padding-right:5px; \" >" + Common.GetMonetaryUnit(PaymentConsigneeMonetary) + " " + Common.NumberFormat("" + ConsigneeTotalCharge) + "</td></tr>");
	//        decimal aleady = 0;
	//        HtmlDeposited.Append(LoadDeposied("C", RequestFormPk, ref aleady));
	//        HtmlDeposited.Append("<tr style=\"height:30px;\"><td colspan=\"3\" style=\"text-align:right; padding-right:20px; background-color:#DDDDDD;\">left</td><td style=\"text-align:right; padding-right:5px; \" >" + Common.GetMonetaryUnit(PaymentConsigneeMonetary) + " " + Common.NumberFormat((aleady - ConsigneeTotalCharge) + "") + "</td></tr>");
	//        HtmlDeposited.Append("</table>");
	//    }
	//}




	private void LoadDeliveryLoad(string RequestFormPk, string AccountID) {
		HtmlDelivery = new StringBuilder();
		string DeliveryTable = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:580px;\">" +
		   "<tr><td style=\"background-color:#E8E8E8;  width:150px; text-align:center; \">Arrival Time</td>" +
		   "		<td style=\"background-color:#E8E8E8;  width:75px; text-align:center;\">Box Count</td>" +
		   "		<td style=\"background-color:#E8E8E8;  width:95px; text-align:center;\">Type</td>" +
		   "		<td style=\"background-color:#E8E8E8;  \">Deliverer</td>" +
		   "		<td class='THead1' style=\"width:70px; background-color:#E8E8E8; \" rowspan=\"2\" >{0}</td>" +
		   "		<td class='THead1' style=\"width:20px; background-color:#E8E8E8; \" rowspan=\"2\" >P</td></tr>" +
		   "<tr><td colspan=\"2\" class='THead1'  style=\"text-align:center;\">Consignee Staff</td><td class='THead1'  colspan=\"2\" style=\"text-align:center;\">Address</td></tr>{1}</table>";
		string Row = "	<tr><td style=\"text-align:center;\" >{1}</td>" +
						  "	<td style=\"text-align:center;\">{2} / {3}</td>" +
						  "	<td style=\"text-align:center;\">{4}</td>" +
						  "	<td style=\"text-align:center;\">{5}</td>" +
						  "	<td style=\"text-align:center;\" rowspan=\"2\" class=\"TBody1\" >{8}</td>" +
						  "	<td style=\"text-align:center;\" rowspan=\"2\" class=\"TBody1\" ><input type=\"button\" value=\"P\" onclick=\"DeliveryPrint('{9}')\" style=\"height:40px; width:15px; padding:0px;  \" /></td></tr>" +
					   "	<tr><td class=\"TBody1\" colspan=\"2\"  style=\"text-align:center;\">{6}</td>" +
						  "	<td class=\"TBody1\" colspan=\"2\">{7}</td></tr>" +
						  "	<tr>" +
						  "	<td class=\"TBody1\" colspan=\"5\">{11}</td></tr>" +
		"<tr><td class=\"TBody1\" colspan=\"4\" style=\"text-align:center;\">{10}</td></tr>";
		DB = new DBConn();
		/*
		DB.SqlCmd.CommandText = @"
SELECT 
	OBSO.OurBranchStorageOutPk, OBSO.BoxCount, CONVERT(CHAR(8), OBSO.StockedDate, 10) AS StockedDate,  OBSO.StatusCL 
	, OBSC.StorageName
	, TBC.TransportBetweenCompanyPk, TBC.Type, TBC.Title, TBC.DriverName, TBC.DriverTEL, 
	TBC.FromDate, TBC.ToDate, TBC.WarehouseInfo, TBC.WarehouseMobile, TBC.PackedCount, 
	TBC.PackingUnit, TBC.Weight, TBC.Volume, TBC.DepositWhere, TBC.Price, TBC.Memo ,OBSO.TransportBetweenBranchPk
FROM OurBranchStorageOut AS OBSO 
	left join OurBranchStorageCode AS OBSC ON OBSO.StorageCode=OBSC.OurBranchStoragePk 
	left join TransportBC AS TBC ON OBSO.TransportBetweenCompanyPk=TBC.TransportBetweenCompanyPk 	
WHERE OBSO.RequestFormPk=" + RequestFormPk + " ORDER BY OBSO.StatusCL DESC ;";
*/
		DB.SqlCmd.CommandText = @"SELECT 
			SR.[STORAGE_PK], SR.[PACKED_COUNT], CONVERT(CHAR(8), SR.[STOCKED_DATE], 10) AS StockedDate
			, OBSC.StorageName
			, TH.[TRANSPORT_PK] , TH.[VOYAGE_NO], TH.[TITLE], TH.[VESSELNAME], TH.[VALUE_STRING_1], '' AS Memo,
			TH.[DATETIME_FROM], TH.[DATETIME_TO], CW.[Title], CW.[Address], CW.[Staff], CW.[Mobile], 
			SR.[PACKING_UNIT], SR.[WEIGHT], SR.[VOLUME], SR.[LAST_TRANSPORT_HEAD_PK], TH.[TRANSPORT_STATUS]
		FROM [dbo].[STORAGE] AS SR 
		left join OurBranchStorageCode AS OBSC ON SR.[WAREHOUSE_PK]=OBSC.OurBranchStoragePk 
		left join [dbo].[TRANSPORT_HEAD] AS TH ON SR.[TRANSPORT_HEAD_PK]=TH.[TRANSPORT_PK]
		LEFT JOIN [dbo].[CompanyWarehouse] AS CW ON TH.[AREA_TO] = CAST(CW.[WarehousePk] AS VARCHAR)
		WHERE SR.[REQUEST_PK]=" + RequestFormPk + @" 
		AND ISNULL(TH.[TRANSPORT_WAY], 'Delivery') = 'Delivery'
		ORDER BY TH.[TRANSPORT_STATUS] DESC ";
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
			RowValue[0] = RS["STORAGE_PK"] + "";
			RowValue[1] = RS["StorageName"] + "" == "" ? "미도착" : "<strong>" + RS["StorageName"] + "</strong> " + (RS["StockedDate"] + "" == "" ? "" : (RS["StockedDate"] + "").Substring(0, 5));
			RowValue[2] = RS["PACKED_COUNT"] + "";
			RowValue[3] = TotalPackedCount + Common.GetPackingUnit(PackingUnit);
			//RowValue[4] = RS["Title"] + "";
			if (RS["Title"] + "" == "") {
				RowValue[4] = RS["VOYAGE_NO"] + "";
			} else {
				RowValue[4] = RS["TITLE"] + "";
			}
			RowValue[5] = RS["VALUE_STRING_1"] + "" == "" ? RS["VESSELNAME"] + "" : RS["VESSELNAME"] + " (" + RS["VALUE_STRING_1"] + ")";

			RowValue[6] = RS["Mobile"] + "" == "" ? "&nbsp" : "(" + RS["Mobile"] + ")";
			if (RS["TRANSPORT_STATUS"] + "" != "6") {
				RowValue[7] = "<input type=\"button\" value=\"출고지 / 배송기사 지정\" onclick=\"DeliverySet('" + RS["STORAGE_PK"] + "', '" + RS["TRANSPORT_PK"] + "')\" />";
				RowValue[8] = "미지정";
				RowValue[11] = RS["Title"] + "" == "" ? "&nbsp;" : RS["Title"] + "";
			} else {
				RowValue[7] = "";
				RowValue[11] = RS["Title"] + "" == "" ? "&nbsp;" : "<span onclick=\"DeliverySet('" + RS["STORAGE_PK"] + "', '" + RS["TRANSPORT_PK"] + "')\" style=\"cursor:hand;\"  >" + RS["Title"] + "</span>";
				RowValue[8] = RS["TRANSPORT_STATUS"] + "" == "6" ?
				   (RS["DATETIME_FROM"] + "" == "" ? "" : (RS["DATETIME_TO"] + "").Substring(4, 4)) + "<br /><span style=\"color:green;\">출고완료</span>" :
				   "<input type=\"button\" value=\"수정\" onclick=\"DeliverySet('" + RS["STORAGE_PK"] + "', '" + RS["TRANSPORT_PK"] + "')\" /><br /><input type=\"button\" value=\"취소\" onclick=\"DeliveryCancel('" + RS["STORAGE_PK"] + "','" + RequestFormPk + "','" + AccountID + "')\"  />";
			}
			RowValue[9] = RS["TRANSPORT_PK"] + "";
			RowValue[10] = RS["Memo"] + "" == "" ? "" : "Memo:" + RS["Memo"] + "";


			TempRow.Append(String.Format(Row, RowValue));
		}
		RS.Dispose();
		DB.DBCon.Close();

		if (CheckIsReaded) {
			HtmlDelivery.Append(String.Format(DeliveryTable, "&nbsp;", TempRow));
		} else {
			HtmlDelivery.Append(String.Format(DeliveryTable, "<input type=\"button\" value=\"Add\" onclick=\"DeliverySet('0', '0');\" />", ""));
		}
	}
	private void LoadRequestHistory(string RequestFormPk) {
		DB = new DBConn();
		DB.DBCon.Open();


		////////////140125 김상수
		bool CheckFreightModify = false;
		bool IsFreightModify = false;
		if (MemberInformation[1] == "2886") {
			DB.SqlCmd.CommandText = "SELECT COUNT(*) FROM [INTL2010].[dbo].[RequestForm] WHERE DepartureBranchPk=3157 and ArrivalBranchPk=3843 and RequestFormPk=" + RequestFormPk + ";";
			if (DB.SqlCmd.ExecuteScalar() + "" == "1") {
				CheckFreightModify = true;
			}
		}
		////////////140125 김상수


		DB.SqlCmd.CommandText = "SELECT [CODE], [ACCOUNT_ID], [DESCRIPTION], [REGISTERD] FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK]=" + RequestFormPk + " ORDER BY [REGISTERD] ASC, [CODE] ASC ;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		StringBuilder RequestHistory = new StringBuilder();
		while (RS.Read()) {
			string history = "";
			switch (RS[0] + "") {
				case "0":
					////////////140125 김상수
					if (CheckFreightModify) {
						IsFreightModify = true;
					}
					////////////140125 김상수
					history = "Document Ok";
					break;
				case "1":
					history = "BL Make";
					break;
				case "2":
					history = "관세사 전송";
					break;
				case "3":
					history = "자가통관";
					break;
				case "4":
					history = "샘플";
					break;
				case "5":
					history = "관부가세 정산완료";
					break;
				case "6":
					history = "<strong>통관지시</strong>";
					break;
				case "7":
					history = "검사생략";
					break;
				case "8":
					history = "서류제출";
					break;
				case "9":
					history = "실물검사";
					break;
				case "10":
					history = "세금납부지시";
					break;
				case "11":
					history = "세금납부지시";
					break;
				case "12":
					history = "세금납부지시";
					break;
				case "13":
					history = "면허완료";
					break;
				case "14":
					history = "면허완료";
					break;
				case "15":
					history = "면허완료";
					break;
				case "16":
					history = "계산서 발행 완료";
					break;
				case "17":
					history = "계산서 발행 취소";
					break;
				case "18":
					history = "배송지 정보 입력";
					break;
				case "19":
					history = "배송지 정보 취소";
					break;

				/*
			 case "30" :
				history = "관부가세 정산";
				break;
				 */
				case "40":
					history = "<strong>발화인 확인</strong>";
					break;
				case "41":
					history = "<strong>수하인 확인</strong>";
					IsConsigneeConfirmedStyle = "style=\" border:2px solid black; background-color:#FFFACD;\"";
					break;
				case "45":
					history = GetGlobalResourceObject("qjsdur", "DaeNapHold") + "";
					break;
				case "46":
					history = GetGlobalResourceObject("qjsdur", "DaeNapDone") + "";
					break;
				case "49":
					history = "보류해제";
					break; //보류해제
				case "50":
					history = "접수";
					break;
				case "51":
					history = "접수확인";
					break;
				case "52":
					history = "보류";
					break;
				case "53":
					history = "픽업예약";
					break;

				case "54":
					history = "재고삭제";
					break;
				case "55":
					history = "재고수정";
					break;
				case "56":
					history = "추가입고";
					break;
				case "57":
					history = "입고완료";
					break;
				case "58":
					history = "운임확정";
					break;
				case "59":
					history = "운임수정";
					break;
				case "60":
					history = "배달비입력";
					break;
				case "61":
					history = "접수증 수정";
					break;
				case "62":
					history = "지사간운송出";
					break;
				case "64":
					history = "선출고";
					break;
				case "65":
					history = "출고지시";
					break;
				case "66":
					history = "도착지入";
					break;
				case "67":
					history = "경유지入";
					break;
				case "68":
					history = "추가入";
					break;
				case "69":
					history = "컨테이너 입항창고 변경";
					break;
				case "70":
					history = "Shipper 입출금";
					break;
				case "71":
					history = "Consignee 입출금";
					break;
				case "80":
					history = "Shipper 정산완료";
					break;
				case "81":
					history = "Consignee 정산완료";
					////////////140125 김상수
					if (CheckFreightModify) {
						IsFreightModify = false;
					}
					////////////140125 김상수
					break;
				case "82":
					history = "<span style='color:blue;'>경유지 정산완료</span>";
					////////////140125 김상수
					if (CheckFreightModify) {
						IsFreightModify = false;
					}
					////////////140125 김상수
					break;
				case "761":
					history = "세납지시";
					break;
				case "762":
					history = "세납";
					break;
				default:
					history = "&nbsp;";
					break;
			}
			RequestHistory.Append(history + " (" + RS["ACCOUNT_ID"] + ") : " + RS["DESCRIPTION"] + " " + RS["REGISTERD"].ToString().Substring(2, RS["REGISTERD"].ToString().Length - 5) + "<br />");
		}
		RS.Dispose();
		DB.DBCon.Close();
		HtmlRequestHistory = RequestHistory + "";
		////////////140125 김상수

		if (IsFreightModify) {
			string BTN_ModifyCharge = "<br/><input type=\"button\" onclick=\"SetRequestFormStep('ModifyCharge')\" value=\"" + GetGlobalResourceObject("qjsdur", "dnsdlatnwjd") + "\" /><input type=\"button\" onclick=\"SetRequestFormStep('HistoryAppend2886FixCharge')\" value=\"" + GetGlobalResourceObject("qjsdur", "ruddbwldnsthdqlghkrwjd") + "\" />";
			HtmlButton += BTN_ModifyCharge;
		}
		////////////140125 김상수
	}
	private void LoadDO() {
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
DECLARE @ReturnValue int; 

SELECT @ReturnValue =[TRANSPORT_HEAD_PK]
  FROM [dbo].[TRANSPORT_BODY]
  WHERE [REQUEST_PK]=" + RequestFormPk + @"
  
 if (ISNULL(@ReturnValue, 0 )=0) 
SELECT TOP 1 [TransportBetweenBranchPk]
  FROM [dbo].[TransportBBHistory]
  WHERE RequestFormPk=" + RequestFormPk + @" 
  ORDER BY [TransportBetweenBranchPk]
else 
	SELECT @ReturnValue;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		if (RS.Read()) {
			BBHPk = RS[0] + "";
		}
		RS.Dispose();
		DB.DBCon.Close();
	}
}