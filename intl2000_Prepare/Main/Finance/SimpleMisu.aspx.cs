using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Finance_SimpleMisu : System.Web.UI.Page
{
	protected String ArrivalBranch;
	protected String StartDate;
	protected String EndDate;
	protected String Value;
	protected String SerchValue;
	protected String InnerSelect;
	protected String Gubun;
	protected String ListHtml;
	protected Decimal TotalIncome;
	protected String Confirm;
	protected String Misu;
	protected String BankAccount;
	protected String InorOut;
	protected String[] MEMBERINFO;
	protected String LeftMenu;
	private DBConn DB;

	protected void Page_Load(object sender, EventArgs e) {
		try {
			MEMBERINFO = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);
		} catch (Exception) {
			Response.Redirect("../Default.aspx");
		}

		ArrivalBranch = MEMBERINFO[1];
		StartDate = Request.Params["SD"] + "";
		EndDate = Request.Params["ED"] + "";
		Value = Request.Params["V"] + "" == "" ? "0" : Request.Params["V"] + "";
		Gubun = Request.Params["G"] + "";
		TotalIncome = 0;

		DB = new DBConn();
		DB.DBCon.Open();
		List<string[]> TargetCompany;
		string Selected = Request.Params["Selected"] + "";
		string TotalMisuValue = "";
		string Color = "";
		string InnerHtml = MisuList(out TargetCompany, Selected, out TotalMisuValue, out Color);

		string trTotalMisu = "";

		if (Color == "red") {
			trTotalMisu = "<tr height='40px'><td colspan=\"3\"></td><td style='text-align:right;'><strong>TOTAL</strong></td><td style='text-align:right;'><span style ='color:red;'>" + TotalMisuValue + "</span></td><tr>";
		} else {
			trTotalMisu = "<tr height='40px'><td colspan=\"3\"></td><td style='text-align:right;'><strong>TOTAL</strong></td><td style='text-align:right;'><span style ='color:blue;'>" + TotalMisuValue + "</span></td><tr>";
		}

		ListHtml = "<table border='0' cellpadding='0' cellspacing='0' style='width:650px;' >" +
		"<thead><tr style=\"height:40px;\">" +
		"	<td class='THead1' style=\"width:120px; \" >Arrival & Box</td>" +
		"	<td class='THead1' >Company</td>" +
		"	<td class='THead1' style=\"width:30px; \"  >&nbsp;</td>" +
		"	<td class='THead1' style=\"width:100px; \"  >Charge</td>" +
		"	<td class='THead1' style=\"width:100px; \"  >Deposit</td>" +
		"	</tr></thead>" + InnerHtml + trTotalMisu +
		"	</table>";
		LeftMenu = LeftPanner(TargetCompany, Selected);
		DB.DBCon.Close();

		if (MEMBERINFO[0] == "Customs") {
			Loged1.Visible = false;
			Loged2.Visible = true;
		}
	}

	private String MisuList(out List<string[]> TargetCompanyList, string Selected, out string TotalMisuValue, out string Color) {
		List<string[]> TargetCompany = new List<string[]>();
		String Row = "<tr>" +
				"<td class='TBody1'  style='text-align:left;'><a href=\"../Admin/RequestView.aspx?g=c&pk={0}\" >{2}</a></td>" +
				"<td class='TBody1' style='text-align:left;'><a href=\"../Admin/CompanyInfo.aspx?M=View&S={1}\" >{3}</a></td>" +
				"<td class='TBody1' >{4}</td>" +
				"<td class='TBody1' style='text-align:right;' >{5}</td>" +
				"<td class='TBody1' style='text-align:right;' >{6}</td></tr>";

		StringBuilder Query = new StringBuilder();
		StringBuilder ReturnValue = new StringBuilder();


		DB.SqlCmd.CommandText = @"
SELECT 
	'Out' AS Type, 
	R.RequestFormPk, R.ShipperPk AS CompanyPk, R.ShipperCode AS CompanyCode, R.DepartureDate, R.ArrivalDate 
	, C.CompanyName AS CompanyName
	, R.TotalPackedCount, R.PackingUnit
	, RFCH.[MONETARY_UNIT] AS MonetaryUnit
	, R.ExchangeDate
	, ISNULL(RFCH.[TOTAL_PRICE], 0) AS Charge
	, ISNULL(RFCH.[DEPOSITED_PRICE], 0) AS Deposited
FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH 
	left join RequestForm AS R ON RFCH.[TABLE_PK]=R.RequestFormPk 
	left join Company AS C ON R.ShipperPk=C.CompanyPk	
	left join RegionCode AS DRC ON R.DepartureRegionCode=DRC.RegionCode 
WHERE R.ArrivalDate>'20141231' 
AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
AND DRC.OurBranchCode=" + ArrivalBranch + @" 
AND (ISNULL(RFCH.[TOTAL_PRICE], 0)<>isnull(RFCH.[DEPOSITED_PRICE], 0)) 
AND R.StepCL not in (33)
order by ArrivalDate desc";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		decimal TotalMisu = 0;
		while (RS.Read()) {
			bool isin = false;
			foreach (string[] each in TargetCompany) {
				if (each[0] == RS["CompanyPk"] + "") {
					isin = true;
					break;
				}
			}
			if (!isin) {
				TargetCompany.Add(new string[] { RS["CompanyPk"] + "", "<strong>" + RS["CompanyCode"] + "</strong> " + RS["CompanyName"] });
			}
			if (Selected != "") {
				if (Selected != RS["CompanyPk"] + "") {
					continue;
				}
			}

			String[] RowData = new String[7];
			RowData[0] = RS["RequestFormPk"] + "";
			RowData[1] = RS["CompanyPk"] + "";
			RowData[2] = (RS["ArrivalDate"] + "" == "" ? "" : (RS["ArrivalDate"] + "").Substring(2)) + " : " + RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "");
			if (RS["Type"] + "" == "Out") {
				RowData[3] = "<span style=\"color:red; font-weight:bold;\">出</span> <strong>" + RS["CompanyCode"] + "</strong> " + RS["CompanyName"];
			} else {
				RowData[3] = "<span style=\"color:blue; font-weight:bold;\">入</span> <strong>" + RS["CompanyCode"] + "</strong> " + RS["CompanyName"];
			}
			decimal tempTariffS = 0;


			RowData[4] = Common.GetMonetaryUnit(RS["MonetaryUnit"] + "");
			RowData[5] = Common.NumberFormat((decimal.Parse(RS["Charge"] + "")).ToString());
			decimal d_Charge = decimal.Parse(RS["Charge"] + "" == "" ? "0" : RS["Charge"] + "");

			string Deposited = RS["Deposited"] + "";
			if (Deposited == "") {
				Deposited = "0";
			}
			decimal d_Deposited = decimal.Parse(Deposited);

			if (Deposited == "") {
				RowData[6] = "<span style='color:green;'>--</span>";
			} else {
				decimal calc = d_Deposited - d_Charge;
				if (calc == 0) {
					continue;
				}
				if (calc > 0) {
					TotalMisu += calc;
					RowData[6] = "<span style='color:blue;'>" + Common.NumberFormat(calc.ToString()) + "</span>";
				} else {
					RowData[6] = "<span style='color:red;'>" + Common.NumberFormat(calc.ToString()) + "</span>";
					TotalMisu += calc;
				}
			}
			ReturnValue.Append(String.Format(Row, RowData));
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = @"
SELECT 
	'Out' AS Type, 
	R.RequestFormPk, R.ShipperPk AS CompanyPk, R.ShipperCode AS CompanyCode, R.DepartureDate, R.ArrivalDate 
	, C.CompanyName AS CompanyName
	, R.TotalPackedCount, R.PackingUnit
	, RFCH.[MONETARY_UNIT] AS MonetaryUnit
	, R.ExchangeDate
	, ISNULL(RFCH.[TOTAL_PRICE], 0) AS Charge
	, ISNULL(RFCH.[DEPOSITED_PRICE], 0) AS Deposited
FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH 
	left join RequestForm AS R ON RFCH.[TABLE_PK]=R.RequestFormPk 
	left join Company AS C ON R.ShipperPk=C.CompanyPk	
	left join RegionCode AS DRC ON R.DepartureRegionCode=DRC.RegionCode 
WHERE 
	R.ArrivalDate>'20141231' 
	AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
	AND DRC.OurBranchCode=" + ArrivalBranch + @" 
	AND (ISNULL(RFCH.[TOTAL_PRICE], 0)<>isnull(RFCH.[DEPOSITED_PRICE], 0)) 
	and R.StepCL not in (33)
	and StepCL>58 
	and R.DocumentStepCL in (0,3,4, 10, 11, 12, 13, 14, 15)
UNION ALL

SELECT 
	'In' AS Type, 
	R.RequestFormPk, R.ConsigneePk AS CompanyPk, R.ConsigneeCode AS CompanyCode, R.DepartureDate, R.ArrivalDate 
	, C.CompanyName AS CompanyName
	, R.TotalPackedCount, R.PackingUnit
	, RFCH.[MONETARY_UNIT] AS MonetaryUnit
	, R.ExchangeDate
	, ISNULL(RFCH.[TOTAL_PRICE], 0) AS Charge
	, ISNULL(RFCH.[DEPOSITED_PRICE], 0) AS Deposited
FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH 
	left join RequestForm AS R ON RFCH.[TABLE_PK]=R.RequestFormPk 
	left join Company AS C ON R.ConsigneePk=C.CompanyPk	
	left join RegionCode AS ARC ON R.ArrivalRegionCode=ARC.RegionCode 
WHERE R.ArrivalDate>'20141231' 
AND ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
AND ARC.OurBranchCode=" + ArrivalBranch + @" 
AND (ISNULL(RFCH.[TOTAL_PRICE], 0)<>isnull(RFCH.[DEPOSITED_PRICE], 0)) 
AND R.StepCL not in (33)
and R.StepCL>58 
	and R.DocumentStepCL in (0,3,4, 10, 11, 12, 13, 14, 15)
order by ArrivalDate desc
";

		//return DB.SqlCmd.CommandText;
		//Response.Write(DB.SqlCmd.CommandText);
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			bool isin = false;
			foreach (string[] each in TargetCompany) {
				if (each[0] == RS["CompanyPk"] + "") {
					isin = true;
					break;
				}
			}
			if (!isin) {
				TargetCompany.Add(new string[] { RS["CompanyPk"] + "", "<strong>" + RS["CompanyCode"] + "</strong> " + RS["CompanyName"] });
			}

			if (Selected != "" && Selected != RS["CompanyPk"] + "") {
				continue;
			}

			String[] RowData = new String[7];
			RowData[0] = RS["RequestFormPk"] + "";
			RowData[1] = RS["CompanyPk"] + "";
			RowData[2] = (RS["ArrivalDate"] + "" == "" ? "" : (RS["ArrivalDate"] + "").Substring(2)) + " : " + RS["TotalPackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + "");
			if (RS["Type"] + "" == "Out") {
				RowData[3] = "<span style=\"color:red; font-weight:bold;\">出</span> <strong>" + RS["CompanyCode"] + "</strong> " + RS["CompanyName"];
			} else {
				RowData[3] = "<span style=\"color:blue; font-weight:bold;\">入</span> <strong>" + RS["CompanyCode"] + "</strong> " + RS["CompanyName"];
			}

			RowData[4] = Common.GetMonetaryUnit(RS["MonetaryUnit"] + "");
			RowData[5] = Common.NumberFormat(RS["Charge"] + "");
			decimal d_Charge = decimal.Parse(RS["Charge"] + "" == "" ? "0" : RS["Charge"] + "");

			string Deposited = RS["Deposited"] + "";
			if (Deposited == "") {
				Deposited = "0";
			}
			decimal d_Deposited = decimal.Parse(Deposited);

			if (Deposited == "") {
				RowData[6] = "<span style='color:green;'>--</span>";
			} else {
				decimal calc = d_Deposited - d_Charge;
				if (calc > 0) {
					TotalMisu += calc;
					RowData[6] = "<span style='color:blue;'>" + Common.NumberFormat(calc.ToString()) + "</span>";
				} else {
					TotalMisu += calc;
					RowData[6] = "<span style='color:red;'>" + Common.NumberFormat(calc.ToString()) + "</span>";
				}
			}
			ReturnValue.Append(String.Format(Row, RowData));
		}
		RS.Dispose();
		TargetCompanyList = TargetCompany;
		if (TotalMisu > 0) { Color = "blue"; } else { Color = "red"; }
		TotalMisuValue = Common.NumberFormat(TotalMisu.ToString());

		return ReturnValue + "";
	}
	private String LeftPanner(List<string[]> TargetCompanyList, string Selected) {
		StringBuilder ReturnValue = new StringBuilder();
		ReturnValue.Append("<tr><td class='TBody1' style='text-align:left; cursor:pointer;' onclick=\"location.href='/Finance/SimpleMisu.aspx';\">ALL</td></tr>");
		foreach (string[] each in TargetCompanyList) {
			if (Selected == each[0]) {
				ReturnValue.Append("<tr><td class='TBody1' style='text-align:left; cursor:pointer; background-color:gray;' onclick=\"location.href='/Finance/SimpleMisu.aspx?Selected=" + each[0] + "';\">" + each[1] + "</td></tr>");
			} else {
				ReturnValue.Append("<tr><td class='TBody1' style='text-align:left; cursor:pointer;' onclick=\"location.href='/Finance/SimpleMisu.aspx?Selected=" + each[0] + "';\">" + each[1] + "</td></tr>");
			}
		}
		return "<table border='0' cellpadding='0' cellspacing='0' style='width:150px;' >" + ReturnValue + "	</table>";
	}
}