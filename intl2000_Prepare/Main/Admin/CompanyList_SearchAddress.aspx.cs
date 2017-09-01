using Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_CompanyList_SearchAddress : System.Web.UI.Page
{
	protected String COMPANYLIST;
	protected String BTN_companyaddinfo199_3157;
	protected String Memo;
	protected String Txt_companyaddinfo199_3157;

	protected String ACCOUNTID;
	protected String NAVIGATION;
	private String COMPANYPK;
	private DBConn DB;
	private Int32 PageNo;
	private Int32 PageLength;
	protected String[] MEMBERINFO;

	protected void Page_Load(object sender, EventArgs e) {
		try {
			MEMBERINFO = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);
		} catch (Exception) {
			Response.Redirect("../Default.aspx");
		}
		if (Session["Type"] + "" != "OurBranch" && Session["Type"] + "" != "ShippingBranch") {
			Response.Redirect("../Default.aspx");
		}

		DB = new DBConn();
		ACCOUNTID = Session["ID"] + "";
		COMPANYPK = Session["CompanyPk"] + "";

		PageLength = 30;
		PageNo = Request.Params["pageNo"] + "" == "" ? 1 : Int32.Parse(Request.Params["pageNo"]);

		string gubun = Request.Params["Gubun"] == null ? "Own" : Request.Params["Gubun"];
		string TypeSerch = "";
		string navigation = "";
		if (MEMBERINFO[2] == "ilic31" || MEMBERINFO[2] == "ilic01" || MEMBERINFO[2] == "ilic66" || MEMBERINFO[2] == "ilic03") {
			navigation = "<p><input type=\"button\" {0} value=\"My\" onclick=\"window.location='CompanyList.aspx?Gubun=Own'\" /> " +
												"<input type=\"button\" {1} value=\"New\" onclick=\"window.location='CompanyList.aspx?Gubun=New'\" /> " +
												"<input type=\"button\" {2} value=\"All\" onclick=\"window.location='CompanyList.aspx?Gubun=All'\" /> " +
												"<input type=\"button\" {2} value=\"KR_거래일자정렬\" onclick=\"window.location='CompanyList.aspx?Gubun=KR_'\" /> " +
												"<input type=\"button\" {3} value=\"Del\" onclick=\"window.location='CompanyList.aspx?Gubun=Del'\" /> " +
												"<input type=\"button\" value=\"지역검색\" onclick=\"window.location='/Admin/CompanyList_SearchAddress.aspx'\" /></p>";
			switch (gubun) {
				case "Own":
					NAVIGATION = string.Format(navigation, "style='font-weight:bold;'", "", "", "");
					break;
				case "New":
					NAVIGATION = string.Format(navigation, "", "style='font-weight:bold;'", "", "");
					break;
				case "All":
					NAVIGATION = string.Format(navigation, "", "", "style='font-weight:bold;'", "");
					break;
				case "Del":
					NAVIGATION = string.Format(navigation, "", "", "", "style='font-weight:bold;'");
					break;
				default:
					NAVIGATION = string.Format(navigation, "", "", "", "");
					break;
			}
			TypeSerch = "Default";
		} else if (MEMBERINFO[1] == "7898" || Session["Type"] + "" == "ShippingBranch") {
			navigation = "<p><input type=\"button\" {0} value=\"My\" onclick=\"window.location='CompanyList.aspx?Gubun=Own'\" /> ";
			switch (gubun) {
				case "Own":
					NAVIGATION = string.Format(navigation, "style='font-weight:bold;'", "", "");
					break;
				case "New":
					NAVIGATION = string.Format(navigation, "", "style='font-weight:bold;'", "");
					break;
				case "All":
					NAVIGATION = string.Format(navigation, "", "", "style='font-weight:bold;'");
					break;
				default:
					NAVIGATION = string.Format(navigation, "", "", "");
					break;
			}
			TypeSerch = "Default";
			gubun = "ShippingBranch";
		} else {
			navigation = "<p><input type=\"button\" {0} value=\"My\" onclick=\"window.location='CompanyList.aspx?Gubun=Own'\" /> " +
								"<input type=\"button\" {1} value=\"New\" onclick=\"window.location='CompanyList.aspx?Gubun=New'\" /> " +
								"<input type=\"button\" {2} value=\"All\" onclick=\"window.location='CompanyList.aspx?Gubun=All'\" /></p>";
			switch (gubun) {
				case "Own":
					NAVIGATION = string.Format(navigation, "style='font-weight:bold;'", "", "");
					break;
				case "New":
					NAVIGATION = string.Format(navigation, "", "style='font-weight:bold;'", "");
					break;
				case "All":
					NAVIGATION = string.Format(navigation, "", "", "style='font-weight:bold;'");
					break;
				default:
					NAVIGATION = string.Format(navigation, "", "", "");
					break;
			}
			TypeSerch = "Default";
		}

		string SearchValue = Request.Params["SearchValue"];
		COMPANYLIST = SetCompanyList("3157", SearchValue);
	}
	protected string SetCompanyList(string CompanyPk, string SearchValue) {
		StringBuilder ReturnValue = new StringBuilder();
		if (SearchValue != null) {
			string queryWhere = "";
			if (SearchValue.Trim() != "") {
				queryWhere = " and ( RC.[Name] like ('%" + SearchValue + "%') or C.[CompanyAddress] like ('%" + SearchValue + "%')  )";
			}

			DB.SqlCmd.CommandText = @"
SELECT 
	C.[CompanyPk],C.[GubunCL],C.[CompanyCode],C.[CompanyName],C.[RegionCode],C.[CompanyAddress],C.[CompanyTEL],C.[CompanyFAX]
	,C.[PresidentName],C.[PresidentEmail], C.[CompanyNo],C.[LastRequestDate],C.[ResponsibleStaff],C.[Memo]
	,RC.[OurBranchCode], RC.[Name]
	, TotalB2.Import_TotalAmount AS Import_TotalAmount_B2, TotalB2.Import_TotalCount AS Import_TotalCount_B2 
	, TotalB1.Import_TotalAmount AS Import_TotalAmount_B1, TotalB1.Import_TotalCount AS Import_TotalCount_B1
  FROM [INTL2010].[dbo].[Company] AS C 
	left join [INTL2010].[dbo].[RegionCode] AS RC ON C.RegionCode=RC.RegionCode 
	left join ( 
		SELECT R.[ConsigneePk], sum([TOTAL_PRICE]) AS Import_TotalAmount, count(*) AS Import_TotalCount 
		FROM [INTL2010].[dbo].[RequestForm] AS R 
		LEFT JOIN [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON R.RequestFormPk=RFCH.[TABLE_PK]
		WHERE left(R.DepartureDate, 4)=DATEPART(year, GETDATE()) 
		AND R.StepCL=65
		AND R.[ConsigneePk] = RFCH.[CUSTOMER_COMPANY_PK]
		AND ISNULL([TABLE_NAME], 'RequestForm') = 'RequestForm'
		Group BY ConsigneePk	
	) AS TotalB1 ON C.CompanyPk=TotalB1.ConsigneePk 
	left join ( 
		SELECT R.[ConsigneePk], sum([TOTAL_PRICE]) AS Import_TotalAmount, count(*) AS Import_TotalCount 
		FROM [INTL2010].[dbo].[RequestForm] AS R 
		LEFT JOIN [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON R.RequestFormPk=RFCH.[TABLE_PK]
		WHERE left(R.DepartureDate, 4)=DATEPART(year, GETDATE())-1 
		AND R.StepCL=65
		AND R.[ConsigneePk] = RFCH.[CUSTOMER_COMPANY_PK]
		AND ISNULL([TABLE_NAME], 'RequestForm') = 'RequestForm'
		Group BY ConsigneePk	
	) AS TotalB2 ON C.CompanyPk=TotalB2.ConsigneePk 
  WHERE RC.[OurBranchCode]=" + CompanyPk + @" and (TotalB2.Import_TotalCount>0 or TotalB1.Import_TotalCount>0) " + queryWhere + @" 
  ORDER BY C.RegionCode 
";
			DB.DBCon.Open();
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				string style = RS["GubunCL"] + "" == "0" ? "TBody1G" : "TBody1";

				ReturnValue.Append("	<tr><td class='" + style + "' style='width:60px;'><a href=\"CompanyView.aspx?M=View&S=" + RS["CompanyPk"] + "\">" + RS["CompanyCode"] + "</a></td>" +
												"	<td class='" + style + "' ><a href=\"CompanyView.aspx?M=View&S=" + RS["CompanyPk"] + "\">" + RS["CompanyName"] + "</a></td>" +
												"	<td class='" + style + "' style='text-align:left;' ><a href=\"CompanyView.aspx?M=View&S=" + RS["CompanyPk"] + "\">" + RS["CompanyAddress"] + "</a></td>" +
												"	<td class='" + style + "' style='text-align:left; padding-left:8px; ' >" + RS["PresidentName"] + "</td>" +
												"	<td class='" + style + "'  >" + RS["CompanyNo"] + "</td>" +
												"	<td class='" + style + "' ' >" + RS["CompanyTEL"] + "</td>" +
												"	<td class='" + style + "' '  style='text-align:right;'>" + Common.NumberFormat(RS["Import_TotalAmount_B2"] + "") + "</td>" +
												"	<td class='" + style + "' ' >" + RS["Import_TotalCount_B2"] + "</td>" +
												"	<td class='" + style + "' ' style='text-align:right;'>" + Common.NumberFormat(RS["Import_TotalAmount_B1"] + "") + "</td>" +
												"	<td class='" + style + "' ' >" + RS["Import_TotalCount_B1"] + "</td></tr>"
				);
			}
			RS.Close();
			DB.DBCon.Close();
		} else {
			SearchValue = "";
		}
		return "<table border='0' cellpadding='0' cellspacing='1' style=\"width:1050px;\" ><thead><tr height='30px'>" +
							"	<td class='THead1' style='width:70px;' >" + GetGlobalResourceObject("Member", "CompanyCode") + "</td>" +
							"	<td class='THead1' style='width:180px;'>" + GetGlobalResourceObject("Member", "CompanyName") + "</td>" +
							"	<td class='THead1' ><input type='text' id='SearchAddress' style='width:150px;' value='" + SearchValue + "' /><input type='button' value='검색' onclick='SetSearch();' /> </td>" +
							"	<td class='THead1' style='width:60px;' >대표</td>" +
							"	<td class='THead1' style='width:100px;' >사업자번호</td>" +
							"	<td class='THead1' style='width:100px;' >TEL</td>" +
							"	<td class='THead1' style='width:100px;' >`" + (DateTime.Now.Year - 1).ToString().Substring(2) + " 매출</td>" +
							"	<td class='THead1' style='width:60px;' >`" + (DateTime.Now.Year - 1).ToString().Substring(2) + " 건수</td>" +
							"	<td class='THead1' style='width:100px;' >`" + (DateTime.Now.Year.ToString().Substring(2)) + " 매출</td>" +
							"	<td class='THead1' style='width:60px;' >`" + (DateTime.Now.Year.ToString().Substring(2)) + " 건수</td>" +
							"	</tr></thead>" + ReturnValue +
								   "<tr height='10px'><td colspan='9' >&nbsp;</td></tr><TR Height='20px'><td colspan='9' style='background-color:#F5F5F5; text-align:center; padding:20px; '>&nbsp;</TD></TR></Table>";

	}
}