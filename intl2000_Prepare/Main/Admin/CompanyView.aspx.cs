using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Admin_CompanyView : System.Web.UI.Page
{
	protected String[] MEMBERINFO;
	protected DBConn DB;
	protected String CompanyPk;
	protected String CompanyInfo;
	protected String RecentRequest;
	protected String TalkBusiness;
	protected String StaffInfo;
	protected String WarehouseInfo;
	protected String RelatedCompany;
	protected String CompanyRelatedInfo;
	protected String FileList;
	protected String ClearanceList;
	protected String CompanyName;
	protected String HtmlButton;
	protected String HtmlButtonforBusiness;
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

		DB = new DBConn();
		CompanyPk = Request.Params["S"] + "";

		DB.DBCon.Open();
		if (Session["Type"] + "" == "ShippingBranch") {
			DB.SqlCmd.CommandText = @"
				SELECT COUNT(*)
				FROM[INTL2010].[dbo].[CompanyRelation]
				WHERE GubunCL = 70 and MainCompanyPk = " + Session["CompanyPk"] + " and TargetCompanyPk = " + CompanyPk + ";";
			string CheckPermission = DB.SqlCmd.ExecuteScalar() + "";
			if (CheckPermission == "0") {
				Response.Redirect("/");
			}
		}

		LoadCompany(CompanyPk);
		LoadStaff(CompanyPk);
		LoadWarehouse(CompanyPk);
		LoadRequestList(CompanyPk);
		LoadTalkBusiness(CompanyPk);
		LoadRelatedCompany(CompanyPk);
		LoadRelatedCompany_(CompanyPk);
		LoadFileList(CompanyPk);
		LoadClearanceList(CompanyPk);
		DB.DBCon.Close();

		HtmlButtonforBusiness = "";

		string AccountType = Session["Type"] + "";
		switch (AccountType) {
			case "ShippingBranch":
				break;
		}





		if (MEMBERINFO[2] == "ilic01" || MEMBERINFO[2] == "ilic30" || MEMBERINFO[2] == "ilic06" || MEMBERINFO[2] == "ilic66" || MEMBERINFO[2] == "ilgz0" || MEMBERINFO[2] == "ilyw0" || MEMBERINFO[2] == "ilogistics" || MEMBERINFO[2] == "ilsy0" || MEMBERINFO[2] == "ilyt0" || MEMBERINFO[2] == "ilqd3") {
			HtmlButtonforBusiness = "<input type=\"button\" value=\"!!Business Note!!\" onclick=\"Goto('TalkBusinessforBusiness');\" />&nbsp;&nbsp;";
		}

		if (MEMBERINFO[0] == "Customs") {
			LogedWithoutRecentRequest1.Visible = false;
			Loged1.Visible = true;
			HtmlButton = "<input type=\"button\" value=\"Company Info\" onclick=\"Goto('basic');\" />&nbsp;" +
								  "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "rjfosodurqhrl") + "\" onclick=\"Goto('request');\" />&nbsp;" +
								  "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "tkdekasodur") + "\" onclick=\"Goto('talkBusiness');\" />&nbsp;";
		} else {
			HtmlButton = "<input type=\"button\" value=\"Company Info\" onclick=\"Goto('basic');\" />&nbsp;" +
								  "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "rjfosodurqhrl") + "\" onclick=\"Goto('request');\" />&nbsp;" +
								  "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "tkdekasodur") + "\" onclick=\"Goto('talkBusiness');\" />&nbsp;" +
						  "<input type=\"button\" value=\"Multi_Comment\" onclick=\"Goto('TalkBusinessforRelatedCompany');\" />&nbsp;" +
								  "<br />" +
								  "<input type =\"button\" value=\"한중FTA\" onclick=\"goto_hscode();\" />&nbsp;" +
								  "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "tndlqtjfb") + "\" onclick=\"Goto('Tradingtransfer');\" />&nbsp;" +
								  "<input type=\"button\" value=\"File Upload\" onclick=\"Goto('fileupload');\" />&nbsp;" +
								  "<input type=\"button\" value=\"ClearanceDoc Upload\" onclick=\"Goto('Clearance');\" />&nbsp;" +
								  "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "rjfocjdusrufgkrl") + "\" onclick=\"Goto('addcustomer');\" />&nbsp;" +
								  "<input type=\"button\" value=\"" + GetGlobalResourceObject("Admin", "wjqtngkrl") + "\" onclick=\"Goto('RequestWrite');\" />";
		}
	}
	private Boolean LoadCompany(string CompanyPk) {
		string regioncode = "";
		DB.SqlCmd.CommandText = @"
		SELECT 
			[GubunCL], [CompanyCode], [CompanyName], [CompanyNamee], [CompanyAddress], [CompanyTEL], 
			[CompanyFAX], [PresidentName], [PresidentEmail], [CompanyNo], [LastRequestDate], [RegionCode]
 
		FROM Company 
		WHERE CompanyPk=" + CompanyPk + ";";
		string row1 = "<tr><td class=\"td01\" >{0}</td><td colspan=\"3\" class=\"td023\">{1}</td></tr>";
		string row2 = "<tr><td class=\"td01\">{0}</td><td class=\"td02\">{1}</td><td class=\"td01\">{2}</td><td class=\"td03\">{3}</td></tr>";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string[] companyinfo;
		if (RS.Read()) {
			regioncode = RS["RegionCode"] + "";
			companyinfo = new string[] { RS[0] + "", RS[1] + "", RS[2] + "", RS[3] + "", RS[4] + "", RS[5] + "", RS[6] + "", RS[7] + "", RS[8] + "", RS[9] + "", RS[10] + "" };
		} else {
			RS.Dispose();
			return false;
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = "SELECT Title, Value FROM CompanyAdditionalInfomation WHERE CompanyPk=" + CompanyPk + " and Title in (62, 63, 64, 65, 80, 152) ORDER BY Title asc;";
		string homepage = "";
		string businesstype = "";
		string upjong = "";
		string majoritem = "";
		string Pyeongtaek = "";
		string IsAACO = "INTL";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			switch (RS[0] + "") {
				case "62":
					homepage = RS[1] + "";
					break;
				case "63":
					businesstype = RS[1] + "";
					string[] split = businesstype.Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
					StringBuilder splitSum = new StringBuilder();
					foreach (string e in split) {
						switch (e) {
							case "57":
								splitSum.Append(GetGlobalResourceObject("Member", "Production"));
								break;
							case "58":
								if (splitSum + "" == "") {
									splitSum.Append(GetGlobalResourceObject("Member", "Distribution"));
								} else {
									splitSum.Append("&nbsp;/&nbsp;" + GetGlobalResourceObject("Member", "Distribution"));
								}
								break;
							case "59":
								if (splitSum + "" == "") {
									splitSum.Append(GetGlobalResourceObject("Member", "Saler"));
								} else {
									splitSum.Append("&nbsp;/&nbsp;" + GetGlobalResourceObject("Member", "Saler"));
								}
								break;
						}
					}
					businesstype = splitSum + "";
					break;
				case "64":
					upjong = (RS[1] + "").Replace("!", " / ");
					break;
				case "80":
					Pyeongtaek = RS[1] + "";
					break;
				case "152":
					IsAACO = RS[1] + "";
					break;
			}
		}
		RS.Dispose();

		StringBuilder Row = new StringBuilder();
		Row.Append(
			string.Format(row2, GetGlobalResourceObject("Member", "CompanyName"), Pyeongtaek != "" ? "<span style =\"color:red; font-weight:bold;\">" + companyinfo[2] + "</span>" + (companyinfo[1] == "" ? "" : Pyeongtaek != "" ? "<span style =\"color:red; font-weight:bold;\">[" + companyinfo[1] + "]</span>" : "<strong>[" + companyinfo[1] + "]</strong>") : companyinfo[2] + (companyinfo[1] == "" ? "" : Pyeongtaek != "" ? "<span style =\"color:red; font-weight:bold;\">[" + companyinfo[1] + "]</span>" : "<strong>[" + companyinfo[1] + "]</strong>"), GetGlobalResourceObject("Member", "PresidentName"), companyinfo[7]) +
			string.Format(row1, GetGlobalResourceObject("Member", "CompanyNamee"), companyinfo[3]) +
			string.Format(row1, GetGlobalResourceObject("Member", "CompanyAddress"), companyinfo[4]) +
			string.Format(row2, "TEL", companyinfo[5], GetGlobalResourceObject("Member", "SaupjaNo"), companyinfo[9]) +
			string.Format(row2, "FAX", companyinfo[6], "Homepage", homepage) +
			string.Format(row2, "E-mail", companyinfo[8], GetGlobalResourceObject("Member", "Upjong"), upjong) +
			string.Format(row1, "Major Item", businesstype + (businesstype == "" ? "" : " :: ") + majoritem));

		if (regioncode.Length > 2 && regioncode.Substring(0, 2) == "1!") {
			if (Session["CompanyPk"].ToString() == "3157") {
				string BTN_To = "";
				if (IsAACO == "INTL") {
					BTN_To = "AACO";
				} else {
					BTN_To = "INTL";
				}

				Row.Append(string.Format(row1, "물류회사지정", "<input type='button' value='" + IsAACO + "' onclick=\"SetCompanyAddInfo('152', '"+BTN_To+"');\" /> "));
			} else {
				Row.Append(string.Format(row1, "물류회사지정", IsAACO));
			}
		}



		DB.SqlCmd.CommandText = "SELECT [CompanyInDocumentPk], [GubunCL], [GubunPk], [Title], [Name], [Address], [DefaultConnection], [CompanyNo] FROM CompanyInDocument WHERE GubunPk=" + CompanyPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		List<string> companynameforclearance = new List<string>();
		while (RS.Read()) {
			companynameforclearance.Add(
				(RS["Title"] + "" == "" ? "" : RS["Title"] + " : ") +
				RS["Name"] +
				(RS["CompanyNo"] + "" == "" ? "" : " (" + RS["CompanyNo"] + ")") +
				(RS["Address"] + "" == "" ? "" : "<br /><span style=\"color:#696969;\" >" + RS["Address"] + "</span>")
				);
		}
		RS.Dispose();

		if (companynameforclearance.Count > 0) {
			Row.Append("<tr><td class=\"td01\" rowspan=\"" + companynameforclearance.Count + "\" >Clearance</td>");
			for (int i = 0; i < companynameforclearance.Count; i++) {
				if (i != 0) {
					Row.Append("<tr>");
				}
				Row.Append("<td colspan=\"3\" class=\"td023\">" + companynameforclearance[i] + "</td></tr>");
			}
		}

		CompanyInfo = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"padding-right:10px; padding-top:10px; padding-left:10px; \">" +
								"	<tr><td class=\"tdSubT\" colspan=\"4\">&nbsp;&nbsp;&nbsp;<strong>Company Info</strong></td></tr>" + Row + "</table>" +
								"	<div style=\"margin-left:10px;   width:510px;  background-color:#777777; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#BBBBBB; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#CCCCCC; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#DDDDDD; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#EEEEEE; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#FFFFFF; height:1px; font-size:1px; \"></div>";
		return true;
	}
	private Boolean LoadStaff(string CompanyPk) {
		DB.SqlCmd.CommandText = @"
	SELECT A.AccountPk, A.GubunCL, A.Duties, A.Name, A.TEL, A.Mobile, A.Email, AAI.Value , A.AccountID
	FROM Account_ AS A
		left join (
			SELECT [AccountPk], [Value] FROM AccountAdditionalInfo_ WHERE GubunCL=1) AS AAI ON A.AccountPk=AAI.AccountPk
	WHERE A.CompanyPk=" + CompanyPk + @"
	order by GubunCL;";
		//Response.Write(DB.SqlCmd.CommandText);	
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder Row = new StringBuilder();
		string row1 = "	<tr style=\"height:25px;\" >" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{1}</td>" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{2}</td>" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{3}</td >" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{4}</td>" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{5}</td>" +
							"	</tr>";
		string row2 = "	<tr style=\"height:25px;\" >" +
							"		<td rowspan=\"2\" style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{1}</td>" +
							"		<td rowspan=\"2\" style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{2}</td>" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{3}</td >" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{4}</td>" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{7}</td>" +
							"	</tr>" +
							"	<tr style=\"height:25px;\" >" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" >{5}</td >" +
							"		<td style=\"border-bottom:1px dotted #E8E8E8; {0}\" align=\"center\" colspan=\"2\" >{6}</td>" +
							"	</tr>";
		while (RS.Read()) {
			string value0 = "";
			string idpwOrdelete = "";
			if (RS["AccountID"] + "" != "" && RS["AccountID"] + "" != "undefined") {
				value0 = "background-color:#FFFACD;";
				idpwOrdelete = "<span onclick=\"SendSMSAccountInfo('" + CompanyName + "', '" + RS["Name"] + "', '" + RS["Mobile"] + "', '" + RS["AccountID"] + "',  '" + RS[0] + "');\" style=\"color:Blue; cursor:hand;\" >p</span>";
			} else {
				value0 = "";
				idpwOrdelete = "<span onclick=\"DELETESTAFF('" + RS[0] + "');\" style=\"color:Red; cursor:hand;\" >X</span>";
			}
			if (RS["TEL"] + "" == "" && RS["Value"] + "" == "") {
				Row.Append(string.Format(
					row1,
					value0,
					Common.DBToHTML(RS[2]),
					Common.DBToHTML(RS[3]),
					Common.DBToHTML(RS[5]),
					Common.DBToHTML(RS[6]),
					idpwOrdelete
					));
			} else {
				Row.Append(string.Format(row2, value0, Common.DBToHTML(RS[2]),
					Common.DBToHTML(RS[3]),
					Common.DBToHTML(RS[5]),
					Common.DBToHTML(RS[6]),
					Common.DBToHTML(RS[4]),
					Common.DBToHTML(RS[7]),
					idpwOrdelete));
			}
		}
		RS.Dispose();

		if (Row + "" == "") {
			StaffInfo = "";
		} else {
			StaffInfo = "<table id=\"TabStaff\" style=\"background-color:White; padding-left:10px; width:520px;\"  border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
								"		<thead>" +
								"			<tr><td class=\"tdSubT\" colspan=\"6\">&nbsp;&nbsp;&nbsp;<strong>staff</strong></td></tr>" +
								"			<tr style=\"height:30px;\" >" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style=\"width:70px;\" >" + GetGlobalResourceObject("Member", "Duties") + "</td>" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style=\"width:90px;\" >" + GetGlobalResourceObject("Member", "Name") + "</td>" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style=\"width:110px;\" >Mobile</td >" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\"  >Email</td>" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style=\"width:15px;\" >&nbsp;</td >" +
								"			</tr>" +
								"		</thead><tbody>" + Row + "</tbody></table>" +
								"	<div style=\"margin-left:10px;   width:510px;  background-color:#777777; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#BBBBBB; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#CCCCCC; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#DDDDDD; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#EEEEEE; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#FFFFFF; height:1px; font-size:1px; \"></div>";
		}
		return true;
	}
	private Boolean LoadWarehouse(string CompanyPk) {
		string TableRowFormat = "<tr style=\"height:25px;\" >" +
			"	<td class=\"TBody1\" >{1}</td>" +
			"	<td class=\"TBody1\" style=\"text-align:left;\" >{2}</td>" +
			"	<td class=\"TBody1\" >{3}</td>" +
			"	<td class=\"TBody1\" ><span onclick=\"DELETEWAREHOUSE('{0}');\" style=\"color:Red; cursor:hand;\" >X</span></td></tr>";
		DB.SqlCmd.CommandText = "SELECT [WarehousePk], [Title], [Address], [TEL], [Staff], [Mobile], [Memo] FROM CompanyWarehouse WHERE CompanyPk=" + CompanyPk + ";";
		StringBuilder Row = new StringBuilder();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			Row.Append(string.Format(TableRowFormat,
				RS[0] + "", RS[1] + "", RS[2] + "", RS[3] + ""));
		}
		RS.Dispose();
		if (Row + "" == "") {
			WarehouseInfo = "";
		} else {
			WarehouseInfo = "<table id=\"TabStaff\" style=\"background-color:White; padding-left:10px; width:520px;\"  border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
								"		<thead>" +
								"			<tr><td class=\"tdSubT\" colspan=\"6\">&nbsp;&nbsp;&nbsp;<strong>Warehouse</strong></td></tr>" +
								"			<tr style=\"height:25px;\" >" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style=\"width:70px;\" >Title</td>" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" >Address</td>" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style=\"width:110px;\" >TEL</td >" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style=\"width:15px;\" >&nbsp;</td >" +
								"			</tr>" +
								"		</thead><tbody>" + Row + "</tbody></table>" +
								"	<div style=\"margin-left:10px; width:510px;  background-color:#777777; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#BBBBBB; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#CCCCCC; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#DDDDDD; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#EEEEEE; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"margin-left:10px; width:510px; background-color:#FFFFFF; height:1px; font-size:1px; \"></div>";
		}
		return true;
	}
	private Boolean LoadRequestList(string CompanyPk) {
		if (Session["Type"] + "" == "ShippingBranch") {
			DB.SqlCmd.CommandText = @"
SELECT TOP 8 
	RF.RequestFormPk, RF.ShipperPk, ConsigneePk , RF.ShipperCode, RF.ConsigneeCode
	, right(RF.DepartureDate, 4) AS DepartureD, right(RF.ArrivalDate, 4) AS ArrivalD, RF.StepCL, RF.DocumentStepCL
	, C.CompanyName
	, CC.CompanyName as CCompanyName
	, Departure.Name, Arrival.Name AS ArrivalN
	, RF.TotalPackedCount, RF.PackingUnit, RCH.[MONETARY_UNIT], RCH.[TOTAL_PRICE], RCH.[DEPOSITED_PRICE], RCH.[LAST_DEPOSITED_DATE], RF.ExchangeDate 
FROM RequestForm AS RF 
	Left join Company AS C on RF.ShipperPk=C.CompanyPk 
	Left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
	Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
	Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RCH on RF.RequestFormPk=RCH.[TABLE_PK] 
	WHERE (( StepCL>49 and ShipperPk=" + CompanyPk + ")  or ( StepCL>52 and ConsigneePk=" + CompanyPk + @") )
	AND ISNULL(RCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
	AND ((RF.DepartureBranchPk=" + Session["CompanyPk"] + @") or  (RF.ArrivalBranchPk=" + Session["CompanyPk"] + @"))" +
	  " Order by RF.ArrivalDate DESC, RequestDate DESC;";
		} else {
			if (MEMBERINFO[1] == "7898") {
				DB.SqlCmd.CommandText = @"
SELECT TOP 8 
	RF.RequestFormPk, RF.ShipperPk, ConsigneePk , RF.ShipperCode, RF.ConsigneeCode
	, right(RF.DepartureDate, 4) AS DepartureD, right(RF.ArrivalDate, 4) AS ArrivalD, RF.StepCL, RF.DocumentStepCL
	, C.CompanyName
	, CC.CompanyName as CCompanyName
	, Departure.Name, Arrival.Name AS ArrivalN
	, RF.TotalPackedCount, RF.PackingUnit, RCH.[MONETARY_UNIT], RCH.[TOTAL_PRICE], RCH.[DEPOSITED_PRICE], RCH.[LAST_DEPOSITED_DATE], RF.ExchangeDate 
FROM RequestForm AS RF 
	Left join Company AS C on RF.ShipperPk=C.CompanyPk 
	Left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
	Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
	Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RCH on RF.RequestFormPk=RCH.[TABLE_PK] 
WHERE (( StepCL>49 and ShipperPk=" + CompanyPk + ")  or ( StepCL>52 and ConsigneePk=" + CompanyPk + " ))" +
"AND ISNULL(RCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'" + 
		"and (RF.ArrivalRegionCode in('2!34!6','2!34!10')  or RF.DepartureRegionCode in('2!34!6','2!34!10') )" +
	   " Order by RF.ArrivalDate DESC, RequestDate DESC;";
			} else {
				DB.SqlCmd.CommandText = @"
SELECT TOP 8 
	RF.RequestFormPk, RF.ShipperPk, ConsigneePk , RF.ShipperCode, RF.ConsigneeCode
	, right(RF.DepartureDate, 4) AS DepartureD, right(RF.ArrivalDate, 4) AS ArrivalD, RF.StepCL, RF.DocumentStepCL
	, C.CompanyName
	, CC.CompanyName as CCompanyName
	, Departure.Name, Arrival.Name AS ArrivalN
	, RF.TotalPackedCount, RF.PackingUnit, RCH.[MONETARY_UNIT], RCH.[TOTAL_PRICE], RCH.[DEPOSITED_PRICE], RCH.[LAST_DEPOSITED_DATE], RF.ExchangeDate 
FROM RequestForm AS RF 
	Left join Company AS C on RF.ShipperPk=C.CompanyPk 
	Left join Company AS CC on RF.ConsigneePk=CC.CompanyPk 
	Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
	Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RCH on RF.RequestFormPk=RCH.[TABLE_PK] 
WHERE ( StepCL>49 and ShipperPk=" + CompanyPk + ")  or ( StepCL>52 and ConsigneePk=" + CompanyPk + " )" +@"
AND ISNULL(RCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
Order by RF.ArrivalDate DESC, RequestDate DESC;";
			}
		}



		//Shipper만 예약접수건 보이도록 변경
		//WHERE StepCL>49 and( ShipperPk="+CompanyPk+" or ConsigneePk="+CompanyPk+" ) "+
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		StringBuilder RowValue = new StringBuilder();
		while (RS.Read()) {

			String StyleID;
			string rowformat;
			if (MEMBERINFO[1] == "7898") {
				rowformat = "<tr style=\"height:25px; \"><td class='{0}'><a href=\"RequestView.aspx?g={1}&pk={2} \">{4}</a></td>" +
				   "<td class='{0}' >{5}</td>" +
				   "<td class='{0}' ><a href=\"RequestView.aspx?g={1}&pk={2} \">{6}</a></td>" +
				   "<td class='{0}'\"><span  onclick=\"ShowFreightChargeView('{2}', '{1}');\"  style=\"cursor:hand;\">{7}</span></a></td></tr>";
			} else {
				rowformat = "<tr style=\"height:25px; \"><td class='{0}'><a href=\"RequestView.aspx?g={1}&pk={2} \">{4}</a></td>" +
				   "<td class='{0}' ><a href=\"CompanyInfo.aspx?M=View&S={3} \">{5}</a></td>" +
				   "<td class='{0}' ><a href=\"RequestView.aspx?g={1}&pk={2} \">{6}</a></td>" +
				   "<td class='{0}'\"><span  onclick=\"ShowFreightChargeView('{2}', '{1}');\"  style=\"cursor:hand;\">{7}</span></a></td></tr>";
			}

			if (RS["StepCL"] + "" == "65") {
				StyleID = "TBody1";
			} else if (RS["DocumentStepCL"] + "" == "10" || RS["DocumentStepCL"] + "" == "11" || RS["DocumentStepCL"] + "" == "12") {
				StyleID = "TBody1B";
			} else {
				StyleID = "TBody1G";
			}

			string[] TableInnerData = new string[13];
			TableInnerData[0] = StyleID;
			TableInnerData[2] = RS["RequestFormPk"] + "";
			if (RS["ShipperPk"] + "" == CompanyPk) {
				TableInnerData[1] = "s";
				TableInnerData[3] = RS["ConsigneePk"] + "";
				TableInnerData[5] = "<span style=\"color:red;\">To</span> <strong>" + RS["ConsigneeCode"] + "</strong>";

			} else {
				TableInnerData[1] = "c";
				TableInnerData[3] = RS["ShipperPk"] + "";
				TableInnerData[5] = "<span style=\"color:blue;\">From</span> <strong>" + RS["ShipperCode"] + "</strong>";
			}

			TableInnerData[4] = (RS["DepartureD"] + "" == "" ? "" : (RS["DepartureD"] + "").Substring(0, 2) + "/" + (RS["DepartureD"] + "").Substring(2)) + " (" + RS["Name"] + ") ~ " +
				(RS["ArrivalD"] + "" == "" ? "" : (RS["ArrivalD"] + "").Substring(2)) + " (" + RS["ArrivalN"] + ")";
			TableInnerData[6] = RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "");

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

			if (TotalCharge > 0 && Deposited == 0) {
				TableInnerData[7] = "<span style=\"color:red;\">未</span>";
			} else if (TotalCharge == 0) {
				TableInnerData[7] = "--";
			} else {
				decimal tempminus = Deposited - TotalCharge;
				if (tempminus == 0) {
					TableInnerData[7] = "<span style=\"color:green;\">完</span>";
				} else if (tempminus > 0) {
					TableInnerData[7] = "<span style=\"color:blue;\">" + MonetaryUnit + " " + Common.NumberFormat(tempminus + "") + "</span>";
				} else {
					TableInnerData[7] = "<span style=\"color:red;\">" + MonetaryUnit + " " + Common.NumberFormat(tempminus + "") + "</span>";
				}
			}

			if (RS["ShipperPk"] + "" != CompanyPk) {
				if (RS["StepCL"] + "" == "50" || RS["StepCL"] + "" == "51") {
					TableInnerData[0] = "";
					TableInnerData[1] = "";
					TableInnerData[2] = "";
					TableInnerData[3] = "";
					TableInnerData[4] = "";
					TableInnerData[5] = "";
					TableInnerData[6] = "";
					TableInnerData[7] = "";
				}
			}
			RowValue.Append(string.Format(rowformat, TableInnerData));
			//ReturnValue.Append(string.Format(TableInnerRow, TableInnerData));
			continue;
		}

		//---------------------------------------------------------------------------------
		RS.Dispose();

		RecentRequest = "<table id=\"TabStaff\" style=\"background-color:White; width:350px;\"  border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
								"		<thead>" +
								"			<tr><td class=\"tdSubT\" colspan=\"4\">&nbsp;&nbsp;&nbsp;<strong>Recent Request</strong></td></tr>" +
								"			<tr style=\"height:30px;\" >" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style='width:162px; ' >schedule</td>" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style=\"width:90px;\" >Company</td>" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style='width:35px;'>CT</td>" +
								"				<td bgcolor=\"#F5F5F5\" align=\"center\" style='width:20px;' >&nbsp;</td>" +
								"			</tr>" +
								"		</thead><tbody>" + RowValue + "</tbody></table>" +
								"	<div style=\"width:350px;  background-color:#777777; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"width:350px; background-color:#BBBBBB; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"width:350px; background-color:#CCCCCC; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"width:350px; background-color:#DDDDDD; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"width:350px; background-color:#EEEEEE; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"width:350px; background-color:#FFFFFF; height:1px; font-size:1px; \"></div>";
		return true;
	}

	private Boolean LoadTalkBusiness(string CompanyPk) {
		string TableRow = "<tr><td class=\"{4}\" align='center' ><span style=\"color:Blue;\">{0}</span></td>" +
										"<td class=\"{4}\" style=\"text-align:left;\">{1} <span style=\"color:gray;\">- {2}</span></td></tr>";
		//20131216 홍차장님 요청
		//DB.SqlCmd.CommandText = "SELECT TOP 7 TalkBusinessPk, GubunCL, AccountID, Contents, Registerd FROM TalkBusiness WHERE GubunPk=" + CompanyPk + " and GubunCL in (0, 10)  ORDER BY GubunCL DESC, Registerd DESC;";
		HistoryC HisC = new HistoryC();
		List<sComment> Comment = new List<sComment>();
		Comment = HisC.LoadList_Comment("Company", CompanyPk, "'Basic_Important'", ref DB);
		StringBuilder retunValue = new StringBuilder();

		for (int i = 0; i < Comment.Count; i++) {
			if (i == 7) {
				break;
			}
			string deleteButton = "";
			if (MEMBERINFO[2] == Comment[i].Account_Id || MEMBERINFO[2][MEMBERINFO[2].Length - 1] == '0' || MEMBERINFO[2] == "ilman") {
				deleteButton = " <span style=\"cursor:hand; color:red;\" onclick=\"CommentDelete('" + Comment[i].Comment_Pk + "')\" >X</span>";
			}
			string[] RowData = new string[] {
				Comment[i].Account_Id,
				(Comment[i].Contents).Replace("\r\n", "<br />"),
				Comment[i].Registerd.ToString().Substring(5, 5)+deleteButton,
				"<span style=\"cursor:hand;\" onclick=\"SetGubunCL('"+Comment[i].Comment_Pk+"', '"+Comment[i].Category+"')\">"+((Comment[i].Category)=="Basic"?"-":"+")+"</span>",
				(Comment[i].Category)=="Basic"?"TBody1":"TBody1G"
			};
			retunValue.Append(string.Format(TableRow, RowData));

		}
		if (retunValue + "" != "") {
			TalkBusiness = "<table id=\"TabStaff\" style=\"background-color:White; width:350px;\"  border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
									"		<thead>" +
									"			<tr><td class=\"tdSubT\" colspan=\"4\">&nbsp;&nbsp;&nbsp;<strong>Comment</strong></td></tr>" +
									"		</thead><tbody>" + retunValue + "</tbody></table>" +
									"	<div style=\"width:350px;  background-color:#777777; height:1px; font-size:1px; \"></div>" +
									"	<div style=\"width:350px; background-color:#BBBBBB; height:1px; font-size:1px; \"></div>" +
									"	<div style=\"width:350px; background-color:#CCCCCC; height:1px; font-size:1px; \"></div>" +
									"	<div style=\"width:350px; background-color:#DDDDDD; height:1px; font-size:1px; \"></div>" +
									"	<div style=\"width:350px; background-color:#EEEEEE; height:1px; font-size:1px; \"></div>" +
									"	<div style=\"width:350px; background-color:#FFFFFF; height:1px; font-size:1px; \"></div>";
		}
		else {
			TalkBusiness = "";
		}
		return true;
	}
	private Boolean LoadRelatedCompany_(string CompanyPk) {
		Boolean IsMainCompany = true;
		string[] MainCompany = new string[] { };
		string MainCompanyPk = "";
		List<string[]> MainManaged = new List<string[]>();
		List<string[]> MainCustomer = new List<string[]>();
		List<string[]> SelectedCustomer = new List<string[]>();

		DB.SqlCmd.CommandText = @"
SELECT 
	CR.CompanyRelationPk, C.[CompanyPk], C.GubunCL, C.CompanyCode, C.CompanyName
FROM CompanyRelation AS CR 
	left join Company AS C ON CR.[MainCompanyPk]=C.CompanyPk 
WHERE TargetCompanyPk=" + CompanyPk + " and CR.GubunCL=0;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			IsMainCompany = false;
			MainCompanyPk = RS["CompanyPk"] + "";
			MainCompany = new string[] { RS["CompanyPk"] + "", RS["GubunCL"] + "", RS["CompanyCode"] + "", RS["CompanyName"] + "" };
		} else {
			IsMainCompany = true;
		}
		RS.Dispose();

		if (IsMainCompany) {
			DB.SqlCmd.CommandText = @"
SELECT [CompanyPk], [GubunCL], [CompanyCode], [CompanyName] 
FROM [Company] 
WHERE CompanyPk=" + CompanyPk + ";";
			RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				MainCompanyPk = RS["CompanyPk"] + "";
				MainCompany = new string[] { RS["CompanyPk"] + "", RS["GubunCL"] + "", RS["CompanyCode"] + "", RS["CompanyName"] + "" };
			} else {
				RS.Dispose();
				return false;
			}
			RS.Dispose();
		}

		DB.SqlCmd.CommandText = @"
SELECT 
	CR.CompanyRelationPk, CR.TargetCompanyPk, CR.GubunCL, CR.TargetCompanyNick 
	, C.GubunCL AS CompanyGubun, C.CompanyCode, C.CompanyName
FROM 
	CompanyRelation AS CR 
	left join Company AS C ON CR.TargetCompanyPk=C.CompanyPk 
WHERE 
	MainCompanyPk=" + MainCompanyPk + @" 
ORDER BY 
	GubunCL ASC;";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (RS["GubunCL"] + "" == "0") {
				MainManaged.Add(new string[] { RS["TargetCompanyPk"] + "", RS["CompanyGubun"] + "", RS["CompanyCode"] + "", RS["TargetCompanyNick"] + "" == "" ? RS["CompanyName"] + "" : RS["TargetCompanyNick"] + "" });
			} else {
				MainCustomer.Add(new string[] { RS["TargetCompanyPk"] + "", RS["CompanyGubun"] + "", RS["CompanyCode"] + "", RS["TargetCompanyNick"] + "" == "" ? RS["CompanyName"] + "" : RS["TargetCompanyNick"] + "" });
			}
		}
		RS.Dispose();

		if (!IsMainCompany) {
			DB.SqlCmd.CommandText = @"
SELECT 
	CR.CompanyRelationPk, CR.TargetCompanyPk, CR.GubunCL, CR.TargetCompanyNick 
	, C.GubunCL AS CompanyGubun, C.CompanyCode, C.CompanyName
FROM 
	CompanyRelation AS CR 
	left join Company AS C ON CR.TargetCompanyPk=C.CompanyPk 
WHERE 
	MainCompanyPk=" + CompanyPk + @" and CR.GubunCL=1
ORDER BY 
	GubunCL ASC;";
			RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				if (RS["GubunCL"] + "" == "0") {
					SelectedCustomer.Add(new string[] { RS["TargetCompanyPk"] + "", RS["CompanyGubun"] + "", RS["CompanyCode"] + "", RS["TargetCompanyNick"] + "" == "" ? RS["CompanyName"] + "" : RS["TargetCompanyNick"] + "" });
				} else {
					SelectedCustomer.Add(new string[] { RS["TargetCompanyPk"] + "", RS["CompanyGubun"] + "", RS["CompanyCode"] + "", RS["TargetCompanyNick"] + "" == "" ? RS["CompanyName"] + "" : RS["TargetCompanyNick"] + "" });
				}
			}
			RS.Dispose();
		}

		StringBuilder ReturnValue = new StringBuilder();
		int IsAleadyNowCompany = -1;
		string FormatCompanyLink = "<td colspan=\"{0}\" rowspan=\"{1}\" style=\"text-align:center; padding-top:5px; padding-bottom:3px; border:1px solid #93A9B8; background-color:{5};\" ><a href=\"CompanyView.aspx?S={2} \">{3}<br />{4}</a></td>";
		string FormatBlankCompany = "<td colspan=\"{0}\" rowspan=\"{1}\" style=\"text-align:center; padding-top:5px; padding-bottom:3px; \" >&nbsp;<br />&nbsp;</td>";
		int RowCount = 0;
		if (MainCompany[0] == CompanyPk) {
			IsAleadyNowCompany = 0;
		}

		Boolean IsUp1 = false;
		Boolean IsUp2 = true;
		Boolean IsUp3 = false;

		ReturnValue.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" +
			String.Format(FormatCompanyLink, "2", "2", MainCompany[0], MainCompany[2], MainCompany[3].Length > 12 ? MainCompany[3].Substring(0, 11) + ".." : MainCompany[3], (IsMainCompany ? "#FFFACD" : "#F0F8FF")));
		if (MainCustomer.Count > 0) {
			IsUp3 = true;
			ReturnValue.Append("<td class=\"BB\">&nbsp;</td><td class=\"BB\">&nbsp;</td>" +
				String.Format(FormatCompanyLink, "1", "2", MainCustomer[RowCount][0], MainCustomer[RowCount][2], MainCustomer[RowCount][3].Length > 12 ? MainCustomer[RowCount][3].Substring(0, 12) + ".." : MainCustomer[RowCount][3], "#F0F8FF"));
		} else {
			ReturnValue.Append("<td>&nbsp;</td><td>&nbsp;</td>" +
				String.Format(FormatBlankCompany, "1", "2"));
		}
		if (RowCount + 1 < MainCustomer.Count) {
			ReturnValue.Append("</tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td class=\"BR\">&nbsp;</td><td>&nbsp;</td></tr>");
		} else {
			ReturnValue.Append("</tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");
		}

		if (IsAleadyNowCompany == -1) {
			for (int i = 0; i < MainManaged.Count; i++) {
				if (MainManaged[i][0] == CompanyPk) {
					IsAleadyNowCompany = i + 1;
					break;
				}
			}
		}
		RowCount++;
		ReturnValue.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>");
		if (MainManaged.Count >= RowCount) {
			ReturnValue.Append("<td class=\"BR\">&nbsp;</td><td>&nbsp;</td>");
		} else {
			ReturnValue.Append("<td>&nbsp;</td><td>&nbsp;</td>");
		}

		if (MainCustomer.Count > RowCount) {
			ReturnValue.Append("<td class=\"BR\">&nbsp;</td>");
		} else {
			ReturnValue.Append("<td>&nbsp;</td>");
		}
		ReturnValue.Append("<td>&nbsp;</td><td>&nbsp;</td></tr>");


		while (true) {
			int RowM1 = RowCount - 1;
			StringBuilder SecondRow = new StringBuilder();
			StringBuilder FirstRow = new StringBuilder();
			if (SelectedCustomer.Count >= RowCount) {
				FirstRow.Append(String.Format(FormatCompanyLink, "1", "2", SelectedCustomer[RowM1][0], SelectedCustomer[RowM1][2], SelectedCustomer[RowM1][3], "#F0F8FF"));
				if (IsUp1) {
					FirstRow.Append("<td class=\"BB BR\">&nbsp;</td>");
				} else {
					FirstRow.Append("<td class=\"BB\">&nbsp;</td>");
					IsUp1 = true;
				}
				if (RowCount == IsAleadyNowCompany) {
					FirstRow.Append("<td class=\"BB\">&nbsp;</td>");
				} else {
					FirstRow.Append("<td>&nbsp;</td>");
				}
				if (SelectedCustomer.Count > RowCount) {
					SecondRow.Append("<td class=\"BR\">&nbsp;</td><td>&nbsp;</td>");
				} else {
					SecondRow.Append("<td>&nbsp;</td><td>&nbsp;</td>");
					IsUp1 = false;
				}
			} else {
				FirstRow.Append(String.Format(FormatBlankCompany, "1", "2") + "<td>&nbsp;</td><td>&nbsp;</td>");
				SecondRow.Append("<td>&nbsp;</td><td>&nbsp;</td>");
				IsUp1 = false;
			}

			if (MainManaged.Count >= RowCount) {
				FirstRow.Append(String.Format(FormatCompanyLink, "1", "2", MainManaged[RowM1][0], MainManaged[RowM1][2], MainManaged[RowM1][3], CompanyPk == MainManaged[RowM1][0] ? "#FFFACD" : "#F0F8FF") + "<td class=\"BB BR\">&nbsp;</td><td>&nbsp;</td>");
				if (MainManaged.Count > RowCount) {
					SecondRow.Append("<td class=\"BR\">&nbsp;</td><td>&nbsp;</td>");
				} else {
					SecondRow.Append("<td>&nbsp;</td><td>&nbsp;</td>");
					IsUp2 = false;
				}
			} else {
				FirstRow.Append(String.Format(FormatBlankCompany, "1", "2") + "<td>&nbsp;</td><td>&nbsp;</td>");
				SecondRow.Append("<td>&nbsp;</td><td>&nbsp;</td>");
				IsUp2 = false;
			}

			if (MainCustomer.Count > RowCount) {
				FirstRow.Append("<td class=\"BR\">&nbsp;</td><td class=\"BB\">&nbsp;</td>" + String.Format(FormatCompanyLink, "1", "2", MainCustomer[RowCount][0], MainCustomer[RowCount][2], MainCustomer[RowCount][3], "#F0F8FF"));
				if (MainCustomer.Count > RowCount + 1) {
					SecondRow.Append("<td class=\"BR\">&nbsp;</td><td>&nbsp;</td>");
				} else {
					SecondRow.Append("<td>&nbsp;</td><td>&nbsp;</td>");
					IsUp3 = false;
				}
			} else {
				IsUp3 = false;
				FirstRow.Append("<td>&nbsp;</td><td>&nbsp;</td>" + String.Format(FormatBlankCompany, "1", "2"));
				SecondRow.Append("<td>&nbsp;</td><td>&nbsp;</td>");
			}

			ReturnValue.Append("<tr>" + FirstRow + "</tr>" + "<tr>" + SecondRow + "</tr>");
			RowCount++;
			if (SelectedCustomer.Count < RowCount && MainManaged.Count < RowCount && MainCustomer.Count <= RowCount) {
				break;
			} else {
				ReturnValue.Append("<tr><td>&nbsp;</td>");
				if (IsUp1) {
					ReturnValue.Append("<td class=\"BR\">&nbsp;</td>");
				} else {
					ReturnValue.Append("<td>&nbsp;</td>");
				}
				ReturnValue.Append("<td>&nbsp;</td><td>&nbsp;</td>");
				if (IsUp2) {
					ReturnValue.Append("<td class=\"BR\">&nbsp;</td>");
				} else {
					ReturnValue.Append("<td>&nbsp;</td>");
				}
				ReturnValue.Append("<td>&nbsp;</td>");
				if (IsUp3) {
					ReturnValue.Append("<td class=\"BR\">&nbsp;</td>");
				} else {
					ReturnValue.Append("<td>&nbsp;</td>");
				}
				ReturnValue.Append("<td>&nbsp;</td><td>&nbsp;</td></tr>");
			}
		}

		CompanyRelatedInfo = ReturnValue + "";
		return true;
	}
	private Boolean LoadRelatedCompany(string CompanyPk) {
		string rowformat;
		if (MEMBERINFO[1] == "7898") {
			rowformat = "<tr style=\"height:25px; \"><td class='{0}'>{2}</td><td class='{0}'>{3}</td>" +
											"<td class='{0}' >{4}</td><td class='{0}'><span style=\"color:red; cursor:hand;\" onclick=\"DeleteRelatedCompany({5}, {6})\">X</span></td></tr>";
		} else {
			rowformat = "<tr style=\"height:25px; \"><td class='{0}'>{2}</td><td class='{0}'>{3}</td>" +
												"<td class='{0}' ><a href=\"CompanyView.aspx?S={1} \">{4}</td><td class='{0}'><span style=\"color:red; cursor:hand;\" onclick=\"DeleteRelatedCompany({5}, {6})\">X</span></td></tr>";
		}
		DB.SqlCmd.CommandText = @"
SELECT 
	CR.CompanyRelationPk, CR.TargetCompanyPk, CR.GubunCL, CR.TargetCompanyNick 
  , C.GubunCL AS CompanyGubun, C.CompanyCode, C.CompanyName 
  , CompanyAdd.Value
FROM 
	CompanyRelation AS CR 
	left join Company AS C ON CR.TargetCompanyPk=C.CompanyPk 
	left join (select * from CompanyAdditionalInfomation where Title=80) CompanyAdd on C.CompanyPk=CompanyAdd.CompanyPk
WHERE 
	MainCompanyPk=" + CompanyPk + @" 
ORDER BY 
	GubunCL ASC;";
		//Response.Write(DB.SqlCmd.CommandText);
		string GubunCL = "";
		StringBuilder Row = new StringBuilder();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (GubunCL != "" + RS["GubunCL"]) {
				Row.Append(string.Format(rowformat,
					RS["CompanyGubun"] + "" == "0" ? "TBody1G" : "TBody1",
					RS["TargetCompanyPk"] + "",
					RS["GubunCL"] + "" == "0" ? "관리업체" : "거래처",
					RS["Value"] + "" == "" ? RS["CompanyCode"] + "" : "<span style =\"color:red; font-weight:bold;\">" + RS["CompanyCode"] + "</span>",
					RS["Value"] + "" == "" ? RS["TargetCompanyNick"] + "" == "" ? RS["CompanyName"] + "" : RS["TargetCompanyNick"] + "" : "<span style =\"color:red; font-weight:bold;\">" + RS["CompanyName"] + "" == "" ? RS["CompanyName"] + "" : RS["CompanyName"] + "" + "</span>",
					RS["CompanyRelationPk"] + "",
					RS["CompanyGubun"] + ""
					));
			}
		}
		RS.Dispose();

		RelatedCompany = "<table id=\"TabStaff\" style=\"background-color:White; width:350px;\"  border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
								"		<thead>" +
								"			<tr><td class=\"tdSubT\" colspan=\"4\">&nbsp;&nbsp;&nbsp;<strong>Related Company</strong></td></tr>" +
								"			<tr style=\"height:30px;\" >" +
								"				<td style='background-color:#F5F5F5; text-align:center; width:60px; ' >구분</td>" +
								"				<td style='background-color:#F5F5F5; text-align:center; width:60px; ' >Code</td>" +
								"				<td style='background-color:#F5F5F5; text-align:center; ' >Name</td>" +
								"				<td style='background-color:#F5F5F5; text-align:center; width:15px; ' >D</td>" +
								"			</tr>" +
								"		</thead><tbody>" + Row + "</tbody></table>" +
								"	<div style=\"width:350px;  background-color:#777777; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"width:350px; background-color:#BBBBBB; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"width:350px; background-color:#CCCCCC; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"width:350px; background-color:#DDDDDD; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"width:350px; background-color:#EEEEEE; height:1px; font-size:1px; \"></div>" +
								"	<div style=\"width:350px; background-color:#FFFFFF; height:1px; font-size:1px; \"></div>";
		return true;
	}
	private Boolean LoadFileList(string CompanyPk) {
		string TableRow = "<tr><td class='TBody1' align='center' ><span style=\"color:Green;\">{0}</span></td>" +
								"<td class='TBody1' style=\"text-align:left; color:gray;\"><a href='../UploadedFiles/FileDownload.aspx?S={1}' >{2}</a></td>" +
								"<td class='TBody1' style=\"text-align:center;\"><span onclick=\"FileDelete('{1}');\" style='color:red;'>X</span></td></tr>";
		DB.SqlCmd.CommandText = "SELECT [FilePk], [Title], [FileName] FROM [File] WHERE GubunCL=1 and GubunPk=" + CompanyPk + ";";
		StringBuilder returnvalue = new StringBuilder();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			returnvalue.Append(String.Format(TableRow, RS["Title"] + "", RS["FilePk"] + "", RS["FileName"] + ""));
		}
		RS.Dispose();
		if (returnvalue + "" != "") {
			FileList = "<table id=\"TabStaff\" style=\"background-color:White; width:350px;\"  border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
									"		<thead>" +
									"			<tr><td class=\"tdSubT\" colspan=\"3\">&nbsp;&nbsp;&nbsp;<strong>FileList</strong></td></tr>" +
									"		</thead><tbody>" + returnvalue + "</tbody></table>" +
									"	<div style=\"width:350px;  background-color:#777777; height:1px; font-size:1px; \"></div>" +
									"	<div style=\"width:350px; background-color:#BBBBBB; height:1px; font-size:1px; \"></div>" +
									"	<div style=\"width:350px; background-color:#CCCCCC; height:1px; font-size:1px; \"></div>" +
									"	<div style=\"width:350px; background-color:#DDDDDD; height:1px; font-size:1px; \"></div>" +
									"	<div style=\"width:350px; background-color:#EEEEEE; height:1px; font-size:1px; \"></div>" +
									"	<div style=\"width:350px; background-color:#FFFFFF; height:1px; font-size:1px; \"></div>";
		} else {
			FileList = "";
		}
		return true;
	}

	private Boolean LoadClearanceList(string CompanyPk) {
		string TableRow = "<tr><td class='TBody1' align='center' ><span style=\"color:Green;\">{0}</span></td>" +
								"<td class='TBody1' style=\"text-align:left; color:gray;\"><a href='../UploadedFiles/FileDownload.aspx?S={1}' >{2}</a></td>" +
								"<td class='TBody1' style=\"text-align:center;\"><span onclick=\"FileDelete('{1}');\" style='color:red;'>X</span></td></tr>";
		DB.SqlCmd.CommandText = "SELECT [FilePk], [Title], [FileName] FROM [File] WHERE GubunCL=99 and GubunPk=" + CompanyPk + ";";
		StringBuilder returnvalue = new StringBuilder();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			returnvalue.Append(String.Format(TableRow, RS["Title"] + "", RS["FilePk"] + "", RS["FileName"] + ""));
		}
		RS.Dispose();
		if (returnvalue + "" != "") {
			ClearanceList = "<table id=\"TabStaff\" style=\"background-color:White; width:350px;\"  border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
									"		<thead>" +
									"			<tr><td class=\"tdSubT\" colspan=\"3\">&nbsp;&nbsp;&nbsp;<strong>ClearanceDocList</strong></td></tr>" +
									"		</thead><tbody>" + returnvalue + "</tbody></table>" +
									"	<div style=\"width:350px;  background-color:#777777; height:1px; font-size:1px; \"></div>" +
									"	<div style=\"width:350px; background-color:#BBBBBB; height:1px; font-size:1px; \"></div>" +
									"	<div style=\"width:350px; background-color:#CCCCCC; height:1px; font-size:1px; \"></div>" +
									"	<div style=\"width:350px; background-color:#DDDDDD; height:1px; font-size:1px; \"></div>" +
									"	<div style=\"width:350px; background-color:#EEEEEE; height:1px; font-size:1px; \"></div>" +
									"	<div style=\"width:350px; background-color:#FFFFFF; height:1px; font-size:1px; \"></div>";
		}
		else {
			ClearanceList = "";
		}
		return true;
	}
}