using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_RequestViewEstimation : System.Web.UI.Page
{
	protected String Contents;
	protected String HtmlSchedule;
	protected StringBuilder HtmlOwnerOfGoods;
	protected StringBuilder HtmlItem;
	protected StringBuilder HtmlJubsuWayCL;
	protected StringBuilder HtmlPayment;
	protected String HtmlDocumentRequest;
	protected String HtmlButton;
	protected String HtmlButtonPayment;
	protected String[] MemberInformation;
	protected StringBuilder HtmlOurStaff;
	protected String HtmlFileList;
	protected String Gubun;
	protected String BBHPk;

	protected String ShipperPk;
	protected String ConsigneePk;
	private String RequestFormPk;
	private Int32 StepCL;
	private String DocumentStepCL;
	private String CommercialDocumentPk;
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
	protected String TransportBetweenBranchPk;
	protected String ConsigeeInDocument;
	protected String CalcHeadPk;

	private bool IsOnlyilic66BTN;
	protected string BTN_Onlyilic66;
	protected void Page_Load(object sender, EventArgs e)
	{
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

		MemberInformation = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		Gubun = Request.Params["g"];
		RequestFormPk = Request.Params["Pk"];

		// 회계팀요청 계산서 완료체크 기능
		//2014. 02. 13. 김상수 		
		IsOnlyilic66BTN = Onlyilic66BTN(RequestFormPk);

      if (MemberInformation[2] == "ilic55" || MemberInformation[2] == "ilic66" || MemberInformation[2] == "ilic77" || MemberInformation[2] == "ilic30")
      {
			BTN_Onlyilic66 = MakeOnlyilic66BTN(IsOnlyilic66BTN);
			//2014. 02. 13. 김상수
			ConsigeeInDocument = "<br />" + LoadConsgieeInDocument(Request.Params["Pk"]);
			// 회계팀요청 계산서 완료체크 기능
			//2014. 03. 06. 김상수 		

		} else {
			ConsigeeInDocument = "";
			BTN_Onlyilic66 = "";
		}



		//2014. 02. 13. 김상수 		


		RequestFormLoad(Request.Params["pk"], Gubun, MemberInformation[2]);


		decimal CarryOverS = 0;
		decimal CarryOverC = 0;


		if (StepCL > 54 || StepCL != 56) {
			LoadAdditionalData(Request.Params["pk"], CarryOverS, CarryOverC);
		}
		LoadRequestComment(Request.Params["pk"], MemberInformation[2]);
		// 회계팀요청 계산서 완료체크 기능
		//2014. 02. 13. 김상수 		
		if (StepCL < 57) {
			BTN_Onlyilic66 = "";
		}

		LoadRequestHistory(Request.Params["Pk"]);

		LoadDaenap();
		LoadDO();

		if (MemberInformation[0] == "Customs") {
			LogedWithoutRecentRequest111.Visible = false;
			Loged1.Visible = true;
		}

	}

	private String LoadConsgieeInDocument(string RequetFormPk)
	{
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"SELECT [Consignee]
	FROM [INTL2010].[dbo].CommercialDocument 
	WHERE CommercialDocumentHeadPk=(SELECT [CommercialDocumentPk]
  FROM [INTL2010].[dbo].[CommerdialConnectionWithRequest]
  WHERE RequestFormPk=" + RequestFormPk + ");";
		DB.DBCon.Open();
		string ReturnValue = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		return ReturnValue;
	}
	private bool Onlyilic66BTN(string RequestFormPk)
	{
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
	private string MakeOnlyilic66BTN(bool isOnlyilic66BTN)
	{
		if (!isOnlyilic66BTN) {
			return "<input type=\"button\" style='background-color:#FAEBD7;' onclick=\"SetRequestFormStep('receipt_confirm16')\" value=\"계산서 발행완료\" />";
		} else {
			return "<input type=\"button\" style='background-color:#FAEBD7;' onclick=\"SetRequestFormStep('receipt_confirm17')\" value=\"계산서 발행취소\" />";
		}
	}

	private void LoadDaenap()
	{
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
	private void RequestFormLoad(string RequestFormPk, string Gubun, string ID)
	{
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

		if (documentstepcl != "" && documentstepcl != "0") {
			DB.SqlCmd.CommandText = "SELECT CommercialDocumentPk FROM CommerdialConnectionWithRequest WHERE RequestFormPk=" + RequestFormPk + ";";
			CommercialDocumentPk = DB.SqlCmd.ExecuteScalar() + "";

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
			SN = RS["SN"] + "";
			SA = RS["SA"] + "";
			CN = RS["CN"] + "";
			CA = RS["CA"] + "";
		}
		RS.Dispose();
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
		LoadItemTable(itemmodifycount, RequestFormPk);
		LoadBTNSUM(msgsendcount);
	}
	private void LoadFileList()
	{
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
		string BTN_FileUpload = "<input type=\"button\" onclick=\"FileUpload('" + RequestFormPk + "')\" value=\"" + GetGlobalResourceObject("qjsdur", "fileupload") + "\" />";
		//HtmlFileList = filelist + "" == "" ? BTN_FileUpload : "<fieldset><legend><strong>Attached File " + BTN_FileUpload + "</strong></legend>" + filelist + "</fieldset>";
		HtmlFileList = filelist + "" == "" ? BTN_FileUpload :
			"<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:580px;\">" +
			"	<tr>" +
			"		<td class='THead1' style=\"width:100px; text-align:center;\" colspan=\"2\">Attached File " + BTN_FileUpload + "</td>" +
			//			"		<td style=\"width:18px; text-align:center;\">A</td>" +
			"		<td class='THead1' style=\"width:18px; text-align:center;\">S</td>" +
			"		<td class='THead1' style=\"width:18px; text-align:center;\">C</td>" +
			"		<td class='THead1' style=\"width:50px; text-align:center;\">ID</td>" +
			"		<td class='THead1' style=\"width:130px; text-align:center;\">Registerd</td>" +
			"		<td class='THead1' style=\"width:18px; text-align:center;\">D</td>" +
			"	</tr><tr>" + filelist + "</table>";
	}
	private void LoadBTNSUM(string MSGCount)
	{
		string BTN_Clearance = "";
		if (DocumentStepCL == "") {
			BTN_Clearance = "<input type=\"button\" onclick=\"SetRequestFormStep('DepartureOK')\" value=\"Document OK\" />" +
				"<input type=\"button\" onclick=\"SetRequestFormStep('Stoced')\" value=\"" + GetGlobalResourceObject("Admin", "dlqrhdhksfy") + "\" /></br>";
		} else if (CommercialDocumentPk != "") {
			BTN_Clearance = "<input type=\"button\" onclick=\"GoClearance('Clearance', '" + CommercialDocumentPk + "')\" value=\"" + GetGlobalResourceObject("qjsdur", "xhdrhks") + "\" /> " +
										"<input type=\"button\" onclick=\"GoClearance('ShowBL', '" + CommercialDocumentPk + "')\" value=\"BL\" /> " +
										"<input type=\"button\" onclick=\"GoClearance('ShowInvoice', '" + CommercialDocumentPk + "')\" value=\"Invoice\" /> " +
										"<input type=\"button\" onclick=\"GoClearance('ShowPacking', '" + CommercialDocumentPk + "')\" value=\"Packing\" /> " +
										"<input type=\"button\" onclick=\"GoClearance('DO', '" + CommercialDocumentPk + "')\" value=\"DO\" /> </br>";
		} else if (MemberInformation[0] == "OurBranch" && SorC == "C" && DocumentStepCL == "0") {
			BTN_Clearance = "<input type=\"button\" id=\"BTNMakeBL\" onclick=\"SetRequestFormStep('LoadBLNo')\" value=\"Make BL\" /></br>";
		} else if (MemberInformation[2] == "ilic31" || MemberInformation[2] == "ilic30") {
			BTN_Clearance = "<input type=\"button\" id=\"BTNMakeBL\" onclick=\"SetRequestFormStep('LoadBLNo')\" value=\"Make BL\" /></br>";
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
		//string BTN_DocumentStepCLTo = "<input type=\"button\" onclick=\"DocumentStepCLTo()\" value=\"통관지시\" />";
		
		if (MemberInformation[0] == "OurBranch") {
			switch (StepCL.ToString()) {
				case "33":
					HtmlButton = BTN_Modify + " " + BTN_RequestDelete + "<input type=\"button\" onclick=\"RequestWriteforEstimation()\" value=\"발송예약으로\" />" + "<br />";
					break;	//입고완료
			}
		} else {
			HtmlButton = BTN_MsgSend + BTN_DeliveryCharge;
		}
	}
	private void LoadItemTable(string ModifyCount, string RequestFormPk)
	{
		HtmlItem = new StringBuilder();
		if (Int32.Parse(ModifyCount) != 0) {
			ModifyCount = "<input type=\"button\" value=\"수정내역 (" + ModifyCount + ")\" onclick=\"PopupItemModifyList('" + RequestFormPk + "')\" style=\"width:100px; height:20px;\" />";
		} else {
			ModifyCount = "";
		}

		if (MemberInformation[0] != "Customs") {
			ModifyCount = "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "audtptnwjd") + "\" onclick=\"PopupItemModify('" + RequestFormPk + "', '" + MemberInformation[2] + "')\" style=\"width:100px; height:20px;\" /> " + ModifyCount;
		}

		HtmlItem.Append("	<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:580px; \" >" +
									"		<tr><td align=\"right\" colspan='8'>" +
									ModifyCount + "</td></tr>" +
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
	private void LoadAdditionalData(string RequestFormPk, decimal CarryOverS, decimal CarryOverC)
	{
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
		}
		else {
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
			Int32 tempint = Int32.Parse(RS["GubunCL"] + "");
			string tempstring = RS["Title"] + "";
			if (RS["StandardPriceHeadPkNColumn"] + "" != "D") {
				tempstringbuilder.Append(string.Format(tempint < 201 ? PaymentRowS : PaymentRowC, tempstring, Common.GetMonetaryUnit(RS["MonetaryUnit"] + "") + " " + Common.NumberFormat(RS["Price"] + "")));
			} else {
				DeliveryRow = String.Format(PaymentRowTotal, (IsOnlyilic66BTN ? "#FAEBD7" : "#D3D3D3"), tempstring, tempint < 201 ? Common.GetMonetaryUnit(RS["MonetaryUnit"] + "") + " " + Common.NumberFormat(RS["Price"] + "") : "&nbsp;", tempint > 201 ? Common.GetMonetaryUnit(RS["MonetaryUnit"] + "") + " " + Common.NumberFormat(RS["Price"] + "") : "&nbsp;");
				if (tempint < 201) {
					DeliveryChargeS = decimal.Parse(RS["Price"] + "");
				} else {
					DeliveryChargeC = decimal.Parse(RS["Price"] + "");
				}
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
		string BTN_ShipperCharge = "<input type=\"button\" onclick=\"SetRequestFormStep('ShipperCharge')\" style=\"padding:0px; \" value=\"" + GetGlobalResourceObject("qjsdur", "qkfghkdlsdnsdla") + "\" />";
		string BTN_ConsigneeCharge = "<input type=\"button\" onclick=\"SetRequestFormStep('ConsigneeCharge')\" style=\"padding:0px; \" value=\"" + GetGlobalResourceObject("qjsdur", "tngkdlsdnsdla") + "\" />";

		HtmlButtonPayment = BTN_ShipperCharge + " " + BTN_ConsigneeCharge;

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
			temp.Append("<div class=\"Line1E8E8E8\" style=\"padding:3px; background-color:" + bgcolor + ";\" >" + (Shipper[i].Contents).Replace("\r\n", "<br />") + " <span style=\"color:gray;\">" + (Shipper[i].Registerd).Substring(5, 5) + "</span> <span style=\"color:Blue;\">" + Shipper[i].Account_Id + "</span>" + deleteButton + "</div>");
		}
		for (int i = 0; i < Consignee.Count; i++) {
			string deleteButton = "";
			string bgcolor = "#C0C0C0";
			if (Consignee[i].Account_Id == AccountID || AccountID == "ilman") {
				deleteButton = "<span style=\"color:red; cursor:pointer;\" onclick=\"CommentDelete('" + Consignee[i].Comment_Pk + "')\">X</span>";
			}
			temp.Append("<div class=\"Line1E8E8E8\" style=\"padding:3px; background-color:" + bgcolor + ";\" >" + (Consignee[i].Contents).Replace("\r\n", "<br />") + " <span style=\"color:gray;\">" + (Consignee[i].Registerd).Substring(5, 5) + "</span> <span style=\"color:Blue;\">" + Consignee[i].Account_Id + "</span>" + deleteButton + "</div>");
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
			temp.Append("<div class=\"Line1E8E8E8\" style=\"padding:3px;\">" + (AddCmt).Replace("\r\n", "<br />") + " <span style=\"color:gray;\">" + (Request[i].Registerd).Substring(5, 5) + "</span> <span style=\"color:Blue;\">" + Request[i].Account_Id + "</span>" + deleteButton + "</div>");
		}
		DB.DBCon.Close();
		HtmlCommentList = temp + "";
	}
	private String LoadDeposied(string SorC, string RequestFormPk, ref decimal Aleady)
	{
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
	private void LoadRequestHistory(string RequestFormPk)
	{
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
					history = "배송지 정보 취소";
					break;
				case "19":
					history = "배송지 정보 수정";
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
					IsFreightModify = true;
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
					if (CheckFreightModify) {
						IsFreightModify = false;
					}
					break;
				case "82":
					history = "<span style='color:blue;'>경유지 정산완료</span>";
					if (CheckFreightModify) {
						IsFreightModify = false;
					}
					break;
				case "85":
					history = "<span style='color:blue;'>접수로 이동</span>";
					if (CheckFreightModify) {
						IsFreightModify = false;
					}
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
			string BTN_ModifyCharge = "<br/><input type=\"button\" onclick=\"SetRequestFormStep('ModifyCharge')\" value=\"" + GetGlobalResourceObject("qjsdur", "dnsdlatnwjd") + "\" />";
			HtmlButton += BTN_ModifyCharge;
		} else {
			HtmlButton += "<input type=\"button\" onclick=\"SetRequestFormStep('FixCharge')\" value=\"" + GetGlobalResourceObject("qjsdur", "dnsdlaghkrwjd") + "\" />";
		}
		////////////140125 김상수
	}
	private void LoadDO()
	{
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
DECLARE @ReturnValue int; 

SELECT @ReturnValue = [TRANSPORT_HEAD_PK]
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