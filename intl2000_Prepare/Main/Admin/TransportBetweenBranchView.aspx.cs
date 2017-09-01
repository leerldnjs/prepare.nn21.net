using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Admin_TransportBetweenBranchView : System.Web.UI.Page
{
	protected String[] MemberInformation;
	protected String Description;
	protected String FileList;
	protected String PackedList;
	protected String TransportCL;
	protected String DOBLNo;
	protected String CommentList;
	protected String DeliveryOrder;
	protected DateTime SelectedDate;
	protected String BUTTON = "";
	protected String DOcheck = "";
	protected String DOcheck2 = "";
	private String Gubun;
	private String BBHPk;
	private DBConn DB;
	protected String stepcl;
	private String tempStorageSelect;
	protected string TransportBB_ChargeList;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("../Default.aspx");
		}
		MemberInformation = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
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

		Gubun = Request.Params["G"] + "";
		BBHPk = Request.Params["S"] + "";
		tempStorageSelect = new Request().LoadBranchStorage(MemberInformation[1]);

		DB = new DBConn();
		DB.DBCon.Open();
		Description = LoadDescription(BBHPk);
		DeliveryOrder = LoadDeliveryOrder(BBHPk);
		//CommentList = LoadCommentList(BBHPk);
		PackedList = LoadPackedList(BBHPk);
		FileList = LoadFileList(BBHPk);

		if (MemberInformation[0] == "Customs") {
			LogedWithoutRecentRequest1.Visible = false;
			Loged1.Visible = true;
			CommentList = "";
		} else {
			CommentList = LoadCommentList(BBHPk);
			TransportBB_ChargeList = LoadTransportBB_ChargeList(BBHPk);
		}

		DB.DBCon.Close();
	}

	private string LoadTransportBB_ChargeList(string BBPk) {

		DB.SqlCmd.CommandText = @"
SELECT TBBC.[TransportBBChargePk]
      ,TBBC.[TransportBBHeadPk]
      ,TBBC.[PaymentBranchPk]
      ,TBBC.[Date]
      ,TBBC.[Title]
      ,TBBC.[MonetaryUnitCL]
      ,TBBC.[Price]
      ,TBBC.[Registerd]
      ,TBBC.[Comment]
	  , F.FilePk
  FROM
	[INTL2010].[dbo].[TransportBBCharge]
	AS TBBC
    left join(
		select* FROM [INTL2010].[dbo].[File] WHERE ISNULL(GubunCL, '31')='31'
	) AS F ON TBBC.[TransportBBChargePk]= F.GubunPk
  WHERE TBBC.TransportBBHeadPk= " + BBPk + " ";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder TempStringBuilder = new StringBuilder();
		string tempTBBHPk = "";
		while (RS.Read()) {
			if (tempTBBHPk != RS["TransportBBChargePk"] + "") {
				if (TempStringBuilder.ToString() != "") {
					TempStringBuilder.Append("           </td>         </tr>");
				}

				TempStringBuilder.Append(@"
                    <tr height = ""30"" >
                           <td class=""TBody1"">" + RS["Title"] + @"<span onclick=""DeleteTransportBBCharge('" + RS["TransportBBChargePk"] + @"');"" style='color:red;'>X</span></td><td class=""TBody1"">" + Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "") + " " + Common.NumberFormat(RS["Price"].ToString()) + @"</td>
<td class=""TBody1"">
				");

			}
			if (RS["FilePk"] + "" != "") {
				TempStringBuilder.Append("<a href='../UploadedFiles/FileDownload.aspx?S=" + RS["FilePk"] + "' >File</a>");
			}
		}
		string ReturnValue = "";
		if (TempStringBuilder.ToString() != "") {
			ReturnValue = " <fieldset style =\"width:420px; margin-top:10px;  padding:10px; \"><legend><strong>매입자료</strong></legend>" +
				@"
			   <table style=""width:420px;  "" border=""0"" cellspacing=""0"" cellpadding=""0"">
                <tbody>" + TempStringBuilder + @"</td>         </tr></tbody>
            </table>" + "</fieldset>";
		}

		RS.Dispose();
		return ReturnValue;
	}

	public String Check_confirm16_All(string stepcl, string BBPk) {
		string table = "";
		if (stepcl == "3") {
			table = " TransportBBHistory ";
		} else {
			table = " OurBranchStorage ";
		}
		DB.SqlCmd.CommandText = @"
SELECT R.RequestFormPk , (ISNULL(C16, 0)-ISNULL(C17, 0)) AS IsConfirm
FROM " + table + @"  AS R
	left join (
		Select [TABLE_PK],   COUNT([CODE])  AS C16 
		from [dbo].[HISTORY] 
		where [CODE] ='16'    
		GROUP BY [TABLE_NAME], [TABLE_PK] 
	) AS Count16 ON Count16.[TABLE_PK]=R.RequestFormPk
	left join (
		Select [TABLE_PK],   COUNT([CODE])  AS C17 
		from [dbo].[HISTORY] 
		where [CODE] ='17'    
		GROUP BY [TABLE_NAME], [TABLE_PK] 
	) AS Count17 ON Count17.[TABLE_PK]=R.RequestFormPk 
WHERE R.TransportBetweenBranchPk =" + BBPk + ";";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		bool isALL = true;
		while (RS.Read()) {
			if (RS["IsConfirm"] + "" == "1") {
				continue;
			} else {
				isALL = false;
				break;

			}
		}
		RS.Dispose();
		return isALL + "";
	}
	private String LoadDescription(string BBPk) {
		StringBuilder ReturnValue = new StringBuilder();

		DB.SqlCmd.CommandText = @"
			SELECT COUNT(*) 
			FROM [INTL2010].[dbo].[Document] 
			WHERE [Type]='DebiCredit' and TypePk=" + BBPk + ";";
		string IsDebitCredit = DB.SqlCmd.ExecuteScalar() + "";

		DB.SqlCmd.CommandText = @"		
SELECT BBHead.TransportCL, BBHead.BLNo, BBHead.FromDateTime, BBHead.ToDateTime
	, BBHead.Description, BBHead.WriteID, TBHStep.Step
	, DC.CompanyName AS FromCompany, AC.CompanyName AS ToCompany
FROM TransportBBHead AS BBHead 
	left join Company AS DC on DC.CompanyPk=BBHead.FromBranchPk 
	left join Company AS AC on AC.CompanyPk=BBHead.ToBranchPk 	
	left join TransportBBStep AS TBHStep ON BBHead.TransportBetweenBranchPk=TBHStep.TransportBetweenBranchPk 
WHERE BBHead.TransportBetweenBranchPk=" + BBPk + ";";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			stepcl = RS["Step"] + "" == "" ? "3" : RS["Step"] + "";
			//qdlo1#@!하북주구-위해 직접발송#@!2011-04-13 19:45:59Z
			//%!$@#qdlo1#@!101#@!2011-04-13 19:47:07Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:09Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:10Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:12Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:13Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:13Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:14Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:14Z%!$@#qdlo1#@!101#@!2011-04-13 19:47:14Z
			string[] description = (RS["Description"] + "").Split(Common.Splite321, StringSplitOptions.None);
			string FromDate = "";
			string FromHour = "";
			string FromMin = "";
			string ToDate = "";
			string ToHour = "";
			string ToMin = "";
			if (RS["FromDateTime"] + "" != "... :" && RS["FromDateTime"] + "" != "") {
				string temp = RS["FromDateTime"] + "";
				FromDate = temp.Substring(0, temp.IndexOf(" ") - 1).Replace(".", "");
				FromHour = temp.Substring(temp.IndexOf(" "), temp.IndexOf(":") - temp.IndexOf(" "));
				FromMin = temp.Substring(temp.IndexOf(":") + 1);
			}
			if (RS["ToDateTime"] + "" != "... :" && RS["ToDateTime"] + "" != "") {
				string temp = RS["ToDateTime"] + "";
				ToDate = temp.Substring(0, temp.IndexOf(" ") - 1).Replace(".", "");
				ToHour = temp.Substring(temp.IndexOf(" "), temp.IndexOf(":") - temp.IndexOf(" "));
				ToMin = temp.Substring(temp.IndexOf(":") + 1);
			}
			TransportCL = RS["TransportCL"] + "";
			DOBLNo = RS["BLNo"] + "";
			switch (RS["TransportCL"] + "") {
				case "1":
					ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "gkdrhdtk") + "</td><td><input type=\"text\" id=\"TBAirCompanyName\" readonly=\"readonly\" value=\"" + description[0] + "\" /><input type=\"hidden\" id=\"HTransportBBCLPk\" value=\"" + RS["TransportCL"] + "\" /></td>" +
					   "		<td>MASTER B/L No</td><td><input type=\"text\" id=\"TBMasterBL\" value=\"" + RS["BLNo"] + "\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "cnfqkfrhdgkd") + "</td><td><input type=\"text\" id=\"TBDepartureRegion\" value=\"" + description[1] + "\" /></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") + "</td><td><input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" value=\"" + FromDate + "\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\" value=\"" + FromHour + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\" value=\"" + FromMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "ehckrwrhdgkd") + "</td><td><input type=\"text\" id=\"TBArrivalRegion\" value=\"" + description[2] + "\" /></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") + "</td><td><input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\"  value=\"" + ToDate + "\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\" value=\"" + ToHour + "\" type=\"text\" maxLength='2' style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\" value=\"" + ToMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "gkdrhdaud") + "</td><td><input type=\"text\" id=\"TBAirName\" value=\"" + description[3] + "\" /></td><td></td><td></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "cnfqkfgkdrhdtkdusfkrcj") + "</td><td><input type=\"text\" id=\"TBAirDepartureTEL\" value=\"" + description[4] + "\"  /></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "ehckrgkdrhdtkdusfkrcj") + "</td><td><input type=\"text\" id=\"TBAirArrivalTEL\" value=\"" + description[5] + "\" /></td></tr></table>");
					break;
				case "2":
					DOcheck2 = "Y";
					ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "ckfidghltkaud") + "</td><td><input type=\"text\" id=\"TBCarCompanyName\" value=\"" + description[0] + "\" /></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "ghkanfthdwkdqjsgh") + "</td><td><input type=\"text\" id=\"TBMasterBL\" value=\"" + RS["BLNo"] + "\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "cnfqkfwl") + "</td><td><input type=\"text\" id=\"TBDepartureRegion\" value=\"" + description[1] + "\" /></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") + "</td><td><input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\"  value=\"" + FromDate + "\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\" value=\"" + FromHour + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\" value=\"" + FromMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "ehckrwl") + "</td><td><input type=\"text\" id=\"TBArrivalRegion\" value=\"" + description[2] + "\" /></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") + "</td><td><input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\"  value=\"" + ToDate + "\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\" value=\"" + ToHour + "\" type=\"text\" maxLength='2' style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\" value=\"" + ToMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "ckfidqjsgh") + "</td><td><input type=\"text\" id=\"TBCarNo\" value=\"" + description[3] + "\" /></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "ckfidrbrur") + "</td><td><input type=\"text\" id=\"TBCarSize\" value=\"" + description[4] + "\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "ckfiddusfkrcj") + "</td><td><input type=\"text\" id=\"TBDriverTEL\" value=\"" + description[6] + "\" /></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "rltktjdaud") + "</td><td><input type=\"text\" id=\"TBDriverName\" value=\"" + description[5] + "\" /></td></tr></table>");
					break;
				case "3":
					if (description.Length == 9) {
						ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjsqkrghltkaud") + "</td><td><input type=\"text\" id=\"TBShipCompanyName\" readonly=\"readonly\" value=\"" + description[0] + "\" /><input type=\"hidden\" id=\"HTransportBBCLPk\" /></td>" +
						   "		<td>MASTER B/L</td><td><input type=\"text\" id=\"TBMasterBL\" value=\"" + RS["BLNo"] + "\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjswjrgkd") + "</td><td><input type=\"text\" id=\"TBDepartureRegion\" value=\"" + description[1] + "\" /></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") + "</td><td><input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" value=\"" + FromDate + "\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\" value=\"" + FromHour + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\" value=\"" + FromMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "ehckrgkd") + "</td><td><input type=\"text\" id=\"TBArrivalRegion\" value=\"" + description[2] + "\" /></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") + "</td><td><input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\"  value=\"" + ToDate + "\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\" value=\"" + ToHour + "\" type=\"text\" maxLength='2' style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\" value=\"" + ToMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjsqkraud") + "</td><td><input type=\"text\" id=\"TBShipName\" value=\"" + description[3] + "\" /></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "tncnfwktkdgh") + "</td><td><input type=\"text\" id=\"TBShipperName\" value=\"" + description[4] + "\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjrbrur") + "</td><td ><input type=\"text\" id=\"TBContainerSize\" value=\"" + description[5] + "\" /></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "gkdck") + "</td><td><input type=\"text\" id=\"TBShippingTime\" value=\"" + description[6] + "\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjqjsgh") + "</td><td><input type=\"text\" id=\"TBContainerNo\" value=\"" + description[7] + "\" maxlength=\"11\" /></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "Tlfqjsgh") + "</td><td><input type=\"text\" id=\"TBSealNo\" value=\"" + description[8] + "\" /></td></tr></table>");
					} else {
						ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjsqkrghltkaud") + "</td><td><input type=\"text\" id=\"TBShipCompanyName\" readonly=\"readonly\" value=\"" + description[0] + "\" /><input type=\"hidden\" id=\"HTransportBBCLPk\" /></td>" +
						   "		<td>MASTER B/L</td><td><input type=\"text\" id=\"TBMasterBL\" value=\"" + RS["BLNo"] + "\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjswjrgkd") + "</td><td><input type=\"text\" id=\"TBDepartureRegion\" value=\"" + description[1] + "\" /></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") + "</td><td><input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" value=\"" + FromDate + "\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\" value=\"" + FromHour + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\" value=\"" + FromMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "ehckrgkd") + "</td><td><input type=\"text\" id=\"TBArrivalRegion\" value=\"" + description[2] + "\" /></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") + "</td><td><input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\"  value=\"" + ToDate + "\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\" value=\"" + ToHour + "\" type=\"text\" maxLength='2' style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\" value=\"" + ToMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjsqkraud") + "</td><td><input type=\"text\" id=\"TBShipName\" value=\"" + description[3] + "\" /></td>" +
						   "		<td>&nbsp;</td><td>&nbsp;</td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjrbrur") + "</td><td ><input type=\"text\" id=\"TBContainerSize\" value=\"" + description[4] + "\" /></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "gkdck") + "</td><td><input type=\"text\" id=\"TBShippingTime\" value=\"" + description[5] + "\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjqjsgh") + "</td><td><input type=\"text\" id=\"TBContainerNo\" value=\"" + description[6] + "\" maxlength=\"11\"/></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "Tlfqjsgh") + "</td><td><input type=\"text\" id=\"TBSealNo\" value=\"" + description[7] + "\" /></td></tr></table>");
					}
					break;
				case "4":
					ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
					   "<tr><td>Hand carry Name</td><td><input type=\"text\" id=\"TBHandCarryCompanyName\" readonly=\"readonly\" value=\"" + description[0] + "\" /><input type=\"hidden\" id=\"HTransportBBCLPk\" /></td>" +
					   "		<td>MASTER B/L</td><td><input type=\"text\" id=\"TBMasterBL\" value=\"" + RS["BLNo"] + "\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjswjrgkd") + "</td><td><input type=\"text\" id=\"TBDepartureRegion\" value=\"" + description[1] + "\" /></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") + "</td><td><input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\"  value=\"" + FromDate + "\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\" value=\"" + FromHour + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\" value=\"" + FromMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "ehckrgkd") + "</td><td><input type=\"text\" id=\"TBArrivalRegion\" value=\"" + description[2] + "\" /></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") + "</td><td><input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" value=\"" + ToDate + "\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\" value=\"" + ToHour + "\" type=\"text\" maxLength='2' style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\" value=\"" + ToMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
					   "<tr><td>Departure Staff</td><td><input type=\"text\" id=\"TBHandCarryDepartureTEL\" value=\"" + description[3] + "\" /></td>" +
					   "		<td>Arrival Staff</td><td><input type=\"text\" id=\"TBHandCarryArrivalTEL\" value=\"" + description[4] + "\" /></td></tr></table>");
					break;
				case "5":
					ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjsqkrghltkaud") + "</td><td><input type=\"text\" id=\"TBShipCompanyName\" readonly=\"readonly\" value=\"" + description[0] + "\" /><input type=\"hidden\" id=\"HTransportBBCLPk\" /></td>" +
					   "		<td>MASTER B/L</td><td><input type=\"text\" id=\"TBMasterBL\" value=\"" + RS["BLNo"] + "\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjswjrgkd") + "</td><td><input type=\"text\" id=\"TBDepartureRegion\" value=\"" + description[1] + "\" /></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") + "</td><td><input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\"  value=\"" + FromDate + "\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\" value=\"" + FromHour + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\" value=\"" + FromMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "ehckrgkd") + "</td><td><input type=\"text\" id=\"TBArrivalRegion\" value=\"" + description[2] + "\" /></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") + "</td><td><input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\"  value=\"" + ToDate + "\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\" value=\"" + ToHour + "\" type=\"text\" maxLength='2' style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\" value=\"" + ToMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjsqkraud") + "</td><td><input type=\"text\" id=\"TBShipName\" value=\"" + description[3] + "\" /></td>" +
					   "		<td>Title | Memo</td><td><input type=\"text\" id=\"TBFCLTitle\" value=\"" + description[4] + "\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjrbrur") + "</td><td><input type=\"text\" id=\"TBContainerSize\" value=\"" + description[5] + "\" /></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "gkdck") + "</td><td><input type=\"text\" id=\"TBShippingTime\" value=\"" + description[6] + "\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjqjsgh") + "</td><td><input type=\"text\" id=\"TBContainerNo\" value=\"" + description[7] + "\" maxlength=\"11\"/></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "Tlfqjsgh") + "</td><td><input type=\"text\" id=\"TBSealNo\" value=\"" + description[8] + "\" /></td></tr></table>");
					break;
				case "6":
					ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
					   "<tr><td>Agency Name</td><td><input type=\"text\" id=\"TBShipCompanyName\" readonly=\"readonly\" value=\"" + description[0] + "\" /><input type=\"hidden\" id=\"HTransportBBCLPk\" /></td>" +
					   "		<td>MASTER B/L</td><td><input type=\"text\" id=\"TBMasterBL\" value=\"" + RS["BLNo"] + "\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjswjrgkd") + "</td><td><input type=\"text\" id=\"TBDepartureRegion\" value=\"" + description[1] + "\" /></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") + "</td><td><input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\"value=\"" + FromDate + "\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\" value=\"" + FromHour + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\" value=\"" + FromMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "ehckrgkd") + "</td><td><input type=\"text\" id=\"TBArrivalRegion\" value=\"" + description[2] + "\" /></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") + "</td><td><input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" value=\"" + ToDate + "\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\" value=\"" + ToHour + "\" type=\"text\" maxLength='2' style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\" value=\"" + ToMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjsqkraud") + "</td><td><input type=\"text\" id=\"TBShipName\" value=\"" + description[3] + "\" /></td>" +
					   "		<td>&nbsp;</td><td>&nbsp;</td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjrbrur") + "</td><td><input type=\"text\" id=\"TBContainerSize\" value=\"" + description[4] + "\" /></td>" +
					   "		<td>&nbsp;</td><td>&nbsp;</td></tr>" +
					   "<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjqjsgh") + "</td><td><input type=\"text\" id=\"TBContainerNo\" value=\"" + description[5] + "\" maxlength=\"11\"/></td>" +
					   "		<td>" + GetGlobalResourceObject("qjsdur", "Tlfqjsgh") + "</td><td><input type=\"text\" id=\"TBSealNo\" value=\"" + description[6] + "\" /></td></tr>" +
					   "<tr><td>Departure Staff</td><td><input type=\"text\" id=\"TBDepartureStaff\" value=\"" + description[7] + "\" /></td>" +
					   "		<td>Arrival Staff</td><td><input type=\"text\" id=\"TBArrivalStaff\" value=\"" + description[8] + "\" /></td></tr></table>");
					break;
				default:
					if (description.Length == 9) {
						ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjsqkrghltkaud") + "</td><td><input type=\"text\" id=\"TBShipCompanyName\" readonly=\"readonly\" value=\"" + description[0] + "\" /><input type=\"hidden\" id=\"HTransportBBCLPk\" /></td>" +
						   "		<td>MASTER B/L</td><td><input type=\"text\" id=\"TBMasterBL\" value=\"" + RS["BLNo"] + "\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjswjrgkd") + "</td><td><input type=\"text\" id=\"TBDepartureRegion\" value=\"" + description[1] + "\" /></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") + "</td><td><input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\"  value=\"" + FromDate + "\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\" value=\"" + FromHour + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\" value=\"" + FromMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "ehckrgkd") + "</td><td><input type=\"text\" id=\"TBArrivalRegion\" value=\"" + description[2] + "\" /></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") + "</td><td><input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\"  value=\"" + ToDate + "\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\" value=\"" + ToHour + "\" type=\"text\" maxLength='2' style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\" value=\"" + ToMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjsqkraud") + "</td><td><input type=\"text\" id=\"TBShipName\" value=\"" + description[3] + "\" /></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "tncnfwktkdgh") + "</td><td><input type=\"text\" id=\"TBShipperName\" value=\"" + description[4] + "\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjrbrur") + "</td><td ><input type=\"text\" id=\"TBContainerSize\" value=\"" + description[5] + "\" /></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "gkdck") + "</td><td><input type=\"text\" id=\"TBShippingTime\" value=\"" + description[6] + "\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjqjsgh") + "</td><td><input type=\"text\" id=\"TBContainerNo\" value=\"" + description[7] + "\" maxlength=\"11\"/></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "Tlfqjsgh") + "</td><td><input type=\"text\" id=\"TBSealNo\" value=\"" + description[8] + "\" /></td></tr></table>");

					} else {
						ReturnValue.Append("<table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjsqkrghltkaud") + "</td><td><input type=\"text\" id=\"TBShipCompanyName\" readonly=\"readonly\" value=\"" + description[0] + "\" /><input type=\"hidden\" id=\"HTransportBBCLPk\" /></td>" +
						   "		<td>MASTER B/L</td><td><input type=\"text\" id=\"TBMasterBL\" value=\"" + RS["BLNo"] + "\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjswjrgkd") + "</td><td><input type=\"text\" id=\"TBDepartureRegion\" value=\"" + description[1] + "\" /></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "cnfqkfdPwjddlf") + "</td><td><input id=\"TBDepartureDate\" size=\"10\" style=\"text-align:center;\" type=\"text\"  value=\"" + FromDate + "\" />&nbsp;&nbsp;<input id=\"TBDepartureHour\" value=\"" + FromHour + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" />:<input id=\"TBDepartureMin\" value=\"" + FromMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "ehckrgkd") + "</td><td><input type=\"text\" id=\"TBArrivalRegion\" value=\"" + description[2] + "\" /></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjddlf") + "</td><td><input id=\"TBArrivalDate\" size=\"10\" style=\"text-align:center;\" type=\"text\" value=\"" + ToDate + "\" />&nbsp;&nbsp;<input id=\"TBArrivalHour\" value=\"" + ToHour + "\" type=\"text\" maxLength='2' style=\"width:18px;text-align:center;\" />:<input id=\"TBArrivalMin\" value=\"" + ToMin + "\" maxLength='2' type=\"text\" style=\"width:18px;text-align:center;\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "tjsqkraud") + "</td><td><input type=\"text\" id=\"TBShipName\" value=\"" + description[3] + "\" /></td>" +
						   "		<td>&nbsp;</td><td>&nbsp;</td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjrbrur") + "</td><td ><input type=\"text\" id=\"TBContainerSize\" value=\"" + description[4] + "\" /></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "gkdck") + "</td><td><input type=\"text\" id=\"TBShippingTime\" value=\"" + description[5] + "\" /></td></tr>" +
						   "<tr><td>" + GetGlobalResourceObject("qjsdur", "zjsxpdlsjqjsgh") + "</td><td><input type=\"text\" id=\"TBContainerNo\" value=\"" + description[6] + "\" maxlength=\"11\"/></td>" +
						   "		<td>" + GetGlobalResourceObject("qjsdur", "Tlfqjsgh") + "</td><td><input type=\"text\" id=\"TBSealNo\" value=\"" + description[7] + "\" /></td></tr></table>");
					}
					break;
			}

			BUTTON = "<p><input type=\"button\" value=\"Modify\" onclick=\"PackingModify();\" />&nbsp;&nbsp;" +
						   "<input type=\"button\" value=\"File Upload\" onclick=\"GoFileupload();\" />&nbsp;&nbsp;" +
						   "<input type=\"button\" value=\"Excel Down\" onclick=\"GoExcelDown();\" />&nbsp;&nbsp;" +
						"</p>";
			if (Gubun == "in" && RS["Step"] + "" == "2") {
				BUTTON += "<p>" + tempStorageSelect + " <input type=\"button\" id='BTN_SetStorageIn' value=\"입고확인\" onclick=\"BTN_Submit_Click();\" /></p>";
			}
			if (Gubun == "out" && RS["Step"] + "" == "1") {
				BUTTON += "<p>&nbsp;</p><input type=\"button\" value=\"발송완료\" onclick=\"PackingSend('real' ,'" + BBPk + "');\" />";

			}

			if (MemberInformation[0] == "OurBranch") {
				string BTN_PrintDebitCredit = "";
				if (IsDebitCredit != "0") {
					BTN_PrintDebitCredit = "<input type=\"button\" value=\"DEBIT\" onclick=\"Goto('Debit_View', '" + BBPk + "');\" />" +
						"<input type=\"button\" value=\"CREDIT\" onclick=\"Goto('Credit_View', '" + BBPk + "');\" />";

				}
				BUTTON += "<p><input type=\"button\" value=\"무역서류 선적데이터 전송\" onclick=\"BTN_SendDataToCommercialDocu('" + BBPk + "');\" />&nbsp;&nbsp;&nbsp;" +
							   "<input type=\"button\" value=\"Warehouse Info\" onclick=\"location.href='../Admin/ManagementStorage.aspx?S=" + MemberInformation[1] + "';\");\" />" +
							   "</p><p>" +
								  "<input type=\"button\" value=\"겉지\" onclick=\"BTN_ExcelDown('Abji', '" + BBPk + "');\" />" +
								  "<input type=\"button\" value=\"적화전송\" onclick=\"BTN_ExcelDown('JukWha', '" + BBPk + "');\" />" +
								  "<input type=\"button\" value=\"2\" onclick=\"BTN_ExcelDown('JukWhaSend', '" + BBPk + "');\" />" +
								  "<input type=\"button\" value=\"CLP\" onclick=\"BTN_ExcelDown('JukWha2', '" + BBPk + "');\" /></p>" +
							   "<p>" +
								  "<input type=\"button\" value=\"DebitCredit\" onclick=\"Goto('DebitCredit_Detail', '" + BBPk + "');\" />" + BTN_PrintDebitCredit +
							   "<p>" +
							   "<p>" +
									  "<input type=\"button\" value=\"매입비용 입력\" onclick=\"GoFileupload2();\" />&nbsp;&nbsp;" +
								"</p>";
				if (MemberInformation[1] == "3157") {
					//BUTTON += "<p>" +
					//						"<input type=\"button\" value=\"유한 적화 전송\" onclick=\"Goto('YuhanDetail', '" + BBPk + "');\" />" +
					//						"<input type=\"button\" value=\"유한 적화 전송N\" onclick=\"Goto('YuhanDetailN', '" + BBPk + "');\" />" +
					//					"</p>";
					BUTTON += "<p>" +
											"<input type=\"button\" value=\"유한 적화 전송N\" onclick=\"Goto('YuhanDetailN', '" + BBPk + "');\" />" +
										"</p>";
				}
				/*
										   "<p>" +
											  "<input type=\"button\" value=\"DEBIT\" onclick=\"BTN_ExcelDown('Debit', '" + BBPk + "');\" />" +
											  "<input type=\"button\" value=\"CREDIT\" onclick=\"BTN_ExcelDown('Credit', '" + BBPk + "');\" />" +
											  "<input type=\"button\" value=\"DEBIT W\" onclick=\"BTN_ExcelDown('DebitW', '" + BBPk + "');\" />" +
											  "<input type=\"button\" value=\"CREDIT W\" onclick=\"BTN_ExcelDown('CreditW', '" + BBPk + "');\" /></p>"+

								*/

				if (RS["Step"] + "" != "1" && RS["Step"] + "" != "2" && Gubun == "in") {
					if (MemberInformation[2] == "ilic66" || MemberInformation[2] == "ilic00" || MemberInformation[2] == "ilic01" || MemberInformation[2] == "ilic06") {
						BUTTON += "<p>&nbsp;</p>" + tempStorageSelect + " <input type=\"button\" value=\"창고변경\" onclick=\"StorageChange();\" />";
					}
				}
			} else if (MemberInformation[0] == "Customs") {

			} else {
				BUTTON += "<p><input type=\"button\" value=\"겉지\" onclick=\"BTN_ExcelDown('Abji', '" + BBPk + "');\" />" +
							   "<input type=\"button\" value=\"적화전송\" onclick=\"BTN_ExcelDown('JukWha', '" + BBPk + "');\" /></p>";
			}
			RS.Dispose();

			string Check_confirm16 = Check_confirm16_All(stepcl, BBPk);
			if (Check_confirm16 != "True") {
				if (MemberInformation[2] == "ilic31" || MemberInformation[2] == "ilic30" || MemberInformation[2] == "ilic01" || MemberInformation[2] == "ilic55" || MemberInformation[2] == "ilic66" || MemberInformation[2] == "ilic77") {
					BUTTON += "<input type=\"button\" style='background-color:#FAEBD7;' value=\"세금계산서 발행\" onclick=\"receipt_confirm16_All('" + stepcl + "','" + BBPk + "');\" />";
				}
			}
			BUTTON += "<input type=\"button\" value=\"Message\" onclick=\"MsgSendFromTransport();\" />";
		}
		return ReturnValue + "";
	}
	private String LoadDeliveryOrder(string BBPk) {
		StringBuilder CommentTemp = new StringBuilder();
		DB.SqlCmd.CommandText = "SELECT  TransportBBPk, MRN, MSN FROM CommercialDocumentDO WHERE TransportBBPk=" + BBPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			CommentTemp.Append("<br><table border=\"1\" cellpadding=\"0\" cellspacing=\"0\">" +
						"<tr><td style=\"text-align:center; width:77px;\" >MRN</td><td><input type=\"text\" id=\"MRN\"  style=\"width:136px;\" value=\"" + RS["MRN"] + "\" /></td>" +
						"		 <td style=\"text-align:center; width:75px;\" >MSN</td><td><input type=\"text\" id=\"MSN\"  style=\"width:90px;\"  value=\"" + RS["MSN"] + "\" /></td>" +
						"        <td style=\"text-align:center;\" ><input type=\"button\"  value=\"수정\" onclick=\"DO_update();\" /></td></tr></table>");
			DOcheck = "Y";
		}

		if (DOcheck == "") {
			if (DOcheck2 == "") {
				CommentTemp.Append("<br><table border=1 cellpadding=0 cellspacing=0>" +
														   " <tr><td style='text-align:center; width:77px;' >MRN</td><td><input type='text'  id='MRN' style='width:136px;'  value=''></td>" +
														   "         <td style='text-align:center; width:75px;' >MSN</td><td><input type='text'  id='MSN' style='width:90px;'  value=''></td>" +
														   "         <td style='text-align:center;' ><input type=button  value='입력'  onclick='DO_insert();'></td></tr></table>");
			} else {
				CommentTemp.Append("<br>");
			}
		}

		RS.Dispose();
		//Response.Write(DOBLNo + "");
		return CommentTemp + "" == "" ? "" : "" + CommentTemp;
	}
	private String LoadCommentList(string BBPk) {
		StringBuilder CommentTemp = new StringBuilder();

		DB.SqlCmd.CommandText = "SELECT TransportBBCommentPk, AccountID, Comment, Registerd FROM TransportBBComment WHERE TransportBBPk=" + BBPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			CommentTemp.Append(RS[2] + " : <strong>" + RS[1] + "</strong> " + (RS[3] + "").Substring(5, (RS[3] + "").Length - 8));
			if (RS[1] + "" == MemberInformation[2]) {
				CommentTemp.Append(" <span style=\"color:red; cursor:hand;\" onclick=\"TransportBBCommentDELETE('" + RS[0] + "')\">X</span><br />");
			} else {
				CommentTemp.Append("<br />");
			}
		}
		RS.Dispose();
		CommentTemp.Append("<br /><input type=\"text\" id=\"TBHeadComment\" style=\"width:230px;\" /><input type=\"button\" value=\"입력\" onclick=\"CommentAdd();\" />");

		return CommentTemp + "" == "" ? "" : "<fieldset style=\"width:420px; padding:10px;  \"><legend><strong>Comment</strong></legend>" + CommentTemp + "</fieldset>";
	}
	private String LoadPackedList(string BBPk) {
		StringBuilder ReturnValue = new StringBuilder();
		if (stepcl == "2") {
			stepcl = "1";
		}
		DB.SqlCmd.CommandText = " EXECUTE SP_Select_TransportBetweenBranchView @StepCL=" + stepcl + ", @BBHPk=" + BBHPk + ";";

		//return DB.SqlCmd.CommandText;
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		int count = 0;
		int TotalPackedCount = 0;
		int TotalRequestPackedCount = 0;
		Decimal TotalVolume = 0;
		Decimal TotalWeight = 0;
		string BeforeRequest = "";
		List<string> ItemSum = new List<string>();
		StringBuilder itemTemp;
		string CommentTemp = "";
		string TempBLNo = "";
		string OtherBLPk = "";

		string TempBLPrintButton = "";
		string INPUTBL = "";
		INPUTBL = "<input type=\"button\" value=\"YT\" style=\"width:25px; padding:0px;  \" onclick=\"Print('B_YT', '{0}','');\" />&nbsp;" +
					"<input type=\"button\" value=\"YT2\" style=\"width:25px; padding:0px;  \" onclick=\"Print('B_YT2', '{0}','');\" />&nbsp;" +
					"<input type=\"button\" value=\"B\" style=\"width:20px; padding:0px;  \" onclick=\"Print('B', '{0}','{1}');\" />&nbsp;" +
					"<input type=\"button\" value=\"I\" style=\"width:20px; padding:0px;  \"  onclick=\"Print('I', '{0}','');\" />&nbsp;" +
					"<input type=\"button\" value=\"P\" style=\"width:20px; padding:0px;  \"  onclick=\"Print('P', '{0}','');\" />&nbsp;" +
					"<input type=\"button\" value=\"DO\" style=\"width:25px; padding:0px;  \" onclick=\"Print('DO', '{0}','');\" />";

		string INPUTDeliveryPrint = "&nbsp;<input type=\"button\" value=\"D\" style=\"width:20px; padding:0px;  \" onclick=\"Print('D', '{0}','');\" />";
		string aTagStart = "";
		string aTagEnd = "";
		string tdclass = "";
		bool SAValue = false;
		bool CAValue = false;
		while (RS.Read()) {
			tdclass = RS["GubunCL"] + "" == "0" ? "TBody1G" : "TBody1";
			SAValue = RS["SAValue"] + "" == "" ? false : true;
			CAValue = RS["CAValue"] + "" == "" ? false : true;
			if (count == 0) {
				if (RS["CommercialDocumentPk"] + "" != "" && TempBLNo != RS["BLNo"] + "") {
					if (RS["DocumentStepCL"] + "" != "4") {
						TempBLNo = RS["BLNo"] + "";
						TempBLPrintButton = string.Format(INPUTBL, RS["CommercialDocumentPk"] + "", RS["FilePk"] + "");
					} else {
						TempBLPrintButton = "&nbsp;&nbsp;<span style=\"color:blue;\">Sample</span> ";
					}
				}
				aTagStart = "<a href='RequestView.aspx?g=s&pk=" + RS["RequestFormPk"] + "'>";
				aTagEnd = "</a>";
				//if (MemberInformation[1] == RS["DOBC"] + "" || MemberInformation[1] == RS["AOBC"] + "") {
				//	aTagStart = "<a href='RequestView.aspx?g=s&pk=" + RS["RequestFormPk"] + "'>";
				//	aTagEnd = "</a>";
				//} else {
				//	aTagStart = "";
				//	aTagEnd = "";
				//}

				BeforeRequest = RS["RequestFormPk"] + "";
				count++;
				TotalPackedCount += Int32.Parse(RS["BoxCount"] + "");
				if (RS["TotalPackedCount"] + "" != "") {
					TotalRequestPackedCount += Int32.Parse(RS["TotalPackedCount"] + "");
				}

				try {
					TotalVolume += Decimal.Parse(RS["TotalVolume"] + "");
				} catch (Exception) {
				}
				try {
					TotalWeight += Decimal.Parse(RS["TotalGrossWeight"] + "");
				} catch (Exception) {
				}
				ItemSum = new List<string>();
				ItemSum.Add(RS["Description"] + "");
				itemTemp = new StringBuilder();
				ReturnValue.Append("<tr>" +
										"	<td class='TBody1' >" + (SAValue == false ? RS["ShipperCode"] : "<span style =\"color:red; font-weight:bold;\">" + RS["ShipperCode"] + "</span>") + "</td>" +
										"	<td class='" + tdclass + "' >" + (CAValue == false ? RS["ConsigneeCode"] : "<span style =\"color:red; font-weight:bold;\">" + RS["ConsigneeCode"] + "</span>") + "</td>" +
										"	<td class='TBody1' >" + aTagStart + (RS["DepartureDate"] + "" == "" ? "&nbsp" : (RS["DepartureDate"] + "").Substring(4, 4)) + aTagEnd + "</td>" +
										"	<td class='TBody1' >" + aTagStart + (RS["ArrivalDate"] + "" == "" ? "&nbsp" : (RS["ArrivalDate"] + "").Substring(4, 4)) + aTagEnd + "</td>" +
										"	<td class='TBody1' ><strong>" + RS["Initial"] + "</strong></td>" +
										"	<td class='TBody1' style=\"text-align:right;\">" + RS["BoxCount"] + " / " + RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "") + "</td>" +
										"	<td class='TBody1' style=\"text-align:right;\">" + Common.NumberFormat(RS["TotalVolume"] + "") + " CBM</td>" +
										"	<td class='TBody1' style=\"text-align:right;\">" + Common.NumberFormat(RS["TotalGrossWeight"] + "") + " Kg</td>");
				CommentTemp = RS["Value"] + "" == "" ? "" : "<strong>" + RS["ActID"] + "</strong> : " + RS["Value"] + " -" + RS["ActDate"];

				//20140714 한경리님
				if (RS["DocumentRequestCL"] + "" != "") {
					string HtmlDocumentRequest = "<div>" + GetGlobalResourceObject("RequestForm", "TradeDocument") + " : ";
					string DocumentRequestCL = RS["DocumentRequestCL"] + "";
					string[] Documents = DocumentRequestCL.Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
					foreach (string T in Documents) {
						switch (T) {
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
								HtmlDocumentRequest += "FTA원산지대리신청(FTA代理申请产地证) ";
								break;
						}
					}

					CommentTemp += HtmlDocumentRequest;
				}
				continue;
			}
			if (BeforeRequest == RS["RequestFormPk"] + "") {
				bool isinitemsum = false;
				foreach (string each in ItemSum) {
					if (each == RS["Description"] + "") {
						isinitemsum = true;
						break;
					}
				}
				if (!isinitemsum) {
					ItemSum.Add(RS["Description"] + "");
				}
			} else {
				aTagStart = "<a href='RequestView.aspx?g=s&pk=" + RS["RequestFormPk"] + "'>";
				aTagEnd = "</a>";
				//출발지사 / 도착지사만 접수증 링크
				//if (MemberInformation[1] == RS["DOBC"] + "" || MemberInformation[1] == RS["AOBC"] + "") {
				//	aTagStart = "<a href='RequestView.aspx?g=s&pk=" + RS["RequestFormPk"] + "'>";
				//	aTagEnd = "</a>";
				//} else {
				//	aTagStart = "";
				//	aTagEnd = "";
				//}
				//출발지사 / 도착지사만 접수증 링크

				itemTemp = new StringBuilder();
				for (int i = 0; i < ItemSum.Count; i++) {
					if (i == 0) {
						itemTemp.Append(ItemSum[i]);
					} else {
						itemTemp.Append(", " + ItemSum[i]);
					}
				}
				ReturnValue.Append("	<td class='TBody1' style=\"text-align:left;\" >" + TempBLPrintButton + string.Format(INPUTDeliveryPrint, BeforeRequest) + "&nbsp;&nbsp;&nbsp;" + itemTemp + "&nbsp;</td></tr>");
				TempBLPrintButton = "";
				if (RS["CommercialDocumentPk"] + "" != "" && TempBLNo != RS["BLNo"] + "") {
					if (RS["DocumentStepCL"] + "" != "4") {
						TempBLNo = RS["BLNo"] + "";
						TempBLPrintButton = string.Format(INPUTBL, RS["CommercialDocumentPk"] + "", RS["FilePk"] + "");
					} else {
						TempBLPrintButton = "&nbsp;&nbsp;<span style=\"color:blue;\">Sample</span> ";
					}
				}

				if (CommentTemp != "") {
					ReturnValue.Append("<tr><td colspan='2' class='TBody1' >&nbsp;</td><td class='TBody1'  colspan='7' style=\"text-align:left;\"  >" + CommentTemp + "</td>");
				}
				BeforeRequest = RS["RequestFormPk"] + "";
				count++;
				TotalPackedCount += Int32.Parse(RS["BoxCount"] + "");

				if (RS["TotalPackedCount"] + "" != "") {
					TotalRequestPackedCount += Int32.Parse(RS["TotalPackedCount"] + "");
				}

				try {
					TotalVolume += Decimal.Parse(RS["TotalVolume"] + "");
				} catch (Exception) {
				}
				try {
					TotalWeight += Decimal.Parse(RS["TotalGrossWeight"] + "");
				} catch (Exception) {
				}

				SAValue = RS["SAValue"] + "" == "" ? false : true;
				CAValue = RS["CAValue"] + "" == "" ? false : true;


				ItemSum = new List<string>();
				CommentTemp = RS["Value"] + "" == "" ? "" : "<strong>" + RS["ActID"] + "</strong> : " + RS["Value"] + " -" + RS["ActDate"];
				if (RS["DocumentRequestCL"] + "" != "") {
					string HtmlDocumentRequest = "<div>" + GetGlobalResourceObject("RequestForm", "TradeDocument") + " : ";
					string DocumentRequestCL = RS["DocumentRequestCL"] + "";
					string[] Documents = DocumentRequestCL.Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
					foreach (string T in Documents) {
						switch (T) {
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
								HtmlDocumentRequest += "FTA원산지대리신청(FTA代理申请产地证) ";
								break;
						}
					}

					CommentTemp += HtmlDocumentRequest;
				}
				ItemSum.Add(RS["Description"] + "");
				itemTemp = new StringBuilder();
				ReturnValue.Append("<tr>" +
										"	<td class='TBody1' >" + (SAValue == false ? RS["ShipperCode"] : "<span style =\"color:red; font-weight:bold;\">" + RS["ShipperCode"] + "</span>") + "</td>" +
										"	<td class='" + tdclass + "' >" + (CAValue == false ? RS["ConsigneeCode"] : "<span style =\"color:red; font-weight:bold;\">" + RS["ConsigneeCode"] + "</span>") + "</td>" +
										"	<td class='TBody1' >" + aTagStart + ((RS["DepartureDate"] + "") == "" ? "--" : (RS["DepartureDate"] + "").Substring(4, 4)) + aTagEnd + "</td>" +
										"	<td class='TBody1' >" + aTagStart + ((RS["ArrivalDate"] + "") == "" ? "--" : (RS["ArrivalDate"] + "").Substring(4, 4)) + aTagEnd + "</td>" +
										"	<td class='TBody1' ><strong>" + RS["Initial"] + "</strong></td>" +
										"	<td class='TBody1' style=\"text-align:right;\">" + RS["BoxCount"] + " / " + RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "") + "</td>" +
										"	<td class='TBody1' style=\"text-align:right;\">" + Common.NumberFormat(RS["TotalVolume"] + "") + " CBM</td>" +
										"	<td class='TBody1' style=\"text-align:right;\">" + Common.NumberFormat(RS["TotalGrossWeight"] + "") + " Kg</td>");
			}
		}
		RS.Dispose();

		itemTemp = new StringBuilder();
		for (int i = 0; i < ItemSum.Count; i++) {
			if (i == 0) {
				itemTemp.Append(ItemSum[i]);
			} else {
				itemTemp.Append(", " + ItemSum[i]);
			}
		}
		ReturnValue.Append("	<td class='TBody1' style=\"text-align:left;\"  >" + TempBLPrintButton + string.Format(INPUTDeliveryPrint, BeforeRequest) + "&nbsp;&nbsp;&nbsp;" + itemTemp + "&nbsp;</td></tr>");
		if (CommentTemp != "") {
			ReturnValue.Append("<tr><td class='TBody1'  colspan='2'>&nbsp;</td><td class='TBody1' style=\"text-align:left;\"  colspan='7'>" + CommentTemp + "</td>");
		}

		if (ReturnValue + "" != "") {
			return "<p>&nbsp;</p><table border='0' cellpadding='0' cellspacing='0' style=\"width:850px;\"><tr height='30px;'>" +
					 "	<td class='THead1' style=\"width:55px;\" >Shipper</td>" +
					 "	<td class='THead1' style=\"width:55px;\" >Consignee</td>" +
					 "	<td class='THead1' style=\"width:45px;\" >Start</td>" +
					 "	<td class='THead1' style=\"width:45px;\" >" + GetGlobalResourceObject("qjsdur", "ehckrdlf") + "</td>" +
					 "	<td class='THead1' style=\"width:25px;\" >&nbsp;</td>" +
					 "	<td class='THead1' style=\"width:80px;\" >" + GetGlobalResourceObject("qjsdur", "vhwkdtnfid") + "</td>" +
					 "	<td class='THead1' style=\"width:70px;\" >" + GetGlobalResourceObject("qjsdur", "cpwjr") + "</td>" +
					 "	<td class='THead1' style=\"width:70px;\" >" + GetGlobalResourceObject("qjsdur", "wndfid") + "</td>" +
					 "	<td class='THead1' >Description</td></tr>" +
					 ReturnValue + "<tr height='30px;'><td colspan=\"5\" class='THead1'  style=\"text-align:center; font-weight:bold;\">Total : " + count + "</td><td class='THead1' style=\"text-align:center; font-weight:bold;\">" + TotalPackedCount + " / " + TotalRequestPackedCount + "</td><td class='THead1' style=\"text-align:center; font-weight:bold;\">" + Common.NumberFormat(TotalVolume + "") + "</td><td class='THead1' style=\"text-align:center; font-weight:bold;\">" + Common.NumberFormat(TotalWeight + "") + "</td><td class='THead1' >&nbsp;</td></tr></table>";
		} else {
			return "";
		}
	}
	private String LoadFileList(string BBPk) {
		DB.SqlCmd.CommandText = "SELECT [FilePk], [Title], [FileName] FROM [INTL2010].[dbo].[File] WHERE GubunCL=0 and GubunPk=" + BBPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder filelist = new StringBuilder();
		while (RS.Read()) {
			filelist.Append(String.Format("<a href='../UploadedFiles/FileDownload.aspx?S={0}' >ㆍ{1} : {2} </a><span onclick=\"FileDelete('{0}');\" style='color:red;'>X</span><br />", RS[0] + "", RS[1] + "", RS[2] + ""));
		}
		RS.Dispose();
		return filelist + "" == "" ? "" : "<fieldset><legend><strong>Attached File</strong></legend>" + filelist + "</fieldset>";
	}
}