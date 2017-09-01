using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Charge_ChargeBase :System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e) {

	}

	[WebMethod]
	public static string[] LoadCollectList(string TableName, string TablePk, string Type) {
		StringBuilder CollectList = new StringBuilder();
		List<sRequestFormCalculateHead> Head = new List<sRequestFormCalculateHead>();
		List<string> ReturnValue = new List<string>();
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = @"SELECT MAX([CHARGE_NO]) FROM [dbo].[REQUESTFORMCALCULATE_HEAD] WHERE [TABLE_NAME] = 'TRANSPORT_HEAD' AND [TABLE_PK] = " + TablePk;
		string MaxChargeNo = DB.SqlCmd.ExecuteScalar() + "";

		DB.SqlCmd.CommandText = @"SELECT
			RH.[REQUESTFORMCALCULATE_HEAD_PK]
			,RH.[TABLE_NAME]
			,RH.[TABLE_PK]
			,RH.[TYPE]
			,RH.[CHARGE_NO]
			,RH.[BRANCH_COMPANY_PK]
			,BRANCH.[CompanyCode] AS [BRANCH_CODE]
			,RH.[BRANCH_BANK_PK]
			,RH.[CUSTOMER_COMPANY_PK]
			,CUSTOMER.[CompanyCode] AS [CUSTOMER_CODE]
			,RH.[CUSTOMER_BANK_PK]
			,RH.[STATUS]
			,RH.[CHARGE_DATE]
			,RH.[MONETARY_UNIT]
			,RH.[TOTAL_PRICE]
			,ISNULL(RH.[DEPOSITED_PRICE], 0) AS DEPOSITED_PRICE
			,RH.[LAST_DEPOSITED_DATE]
			,TH.[VESSELNAME]
			,THFROM.[CompanyCode] AS [TRANSPORT_FROM]
			,THTO.[CompanyCode] AS [TRANSPORT_TO]
		FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS RH
		LEFT JOIN [dbo].[TRANSPORT_HEAD] AS TH ON RH.[TABLE_PK] = TH.[TRANSPORT_PK]
		LEFT JOIN [dbo].[Company] AS BRANCH ON RH.[BRANCH_COMPANY_PK] = BRANCH.[CompanyPk]
		LEFT JOIN [dbo].[Company] AS CUSTOMER ON RH.[CUSTOMER_COMPANY_PK] = CUSTOMER.[CompanyPk]
		LEFT JOIN [dbo].[Company] AS THFROM ON TH.[BRANCHPK_FROM] = THFROM.[CompanyPk]
		LEFT JOIN [dbo].[Company] AS THTO ON TH.[BRANCHPK_TO] = THTO.[CompanyPk]
		WHERE [TABLE_NAME] = '" + TableName + @"'
		AND [TABLE_PK] = " + TablePk + @"
		ORDER BY [CHARGE_NO], [TABLE_PK]";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			Head.Add(new sRequestFormCalculateHead {
				RequestFormCalculate_Head_Pk = RS["REQUESTFORMCALCULATE_HEAD_PK"] + "",
				Table_Name = RS["TABLE_NAME"] + "",
				Table_Pk = RS["TABLE_PK"] + "",
				Type = RS["TYPE"] + "",
				Charge_No = RS["CHARGE_NO"] + "",
				Branch_Company_Pk = RS["BRANCH_COMPANY_PK"] + "",
				Branch_Code = RS["BRANCH_CODE"] + "",
				Branch_Bank_Pk = RS["BRANCH_BANK_PK"] + "",
				Customer_Company_Pk = RS["CUSTOMER_COMPANY_PK"] + "",
				Customer_Code = RS["CUSTOMER_CODE"] + "",
				Customer_Bank_Pk = RS["CUSTOMER_BANK_PK"] + "",
				Charge_Date = RS["CHARGE_DATE"] + "",
				Monetary_Unit = RS["MONETARY_UNIT"] + "",
				Total_Price = RS["TOTAL_PRICE"] + "",
				Deposited_Price = RS["DEPOSITED_PRICE"] + "",
				Last_Deposited_Date = RS["LAST_DEPOSITED_DATE"] + "",
				VesselName = RS["VESSELNAME"] + "",
				Transport_From = RS["TRANSPORT_FROM"] + "",
				Transport_To = RS["TRANSPORT_TO"] + ""
			});
		}

		CollectList.Append("<table class=\"table text-sm\">");
		CollectList.Append("<thead><tr><th style=\"text-align:left;\">운송노선</th>");
		CollectList.Append("<th style=\"text-align:left;\">운송업체</th>");
		CollectList.Append("<th style=\"text-align:left;\">청구회사</th>");
		CollectList.Append("<th style=\"text-align:left;\">지불회사</th>");
		CollectList.Append("<th style=\"text-align:left;\">청구비용</th>");
		CollectList.Append("<th style=\"text-align:left;\">지불비용</th>");
		CollectList.Append("<th style=\"text-align:left;\">차액</th>");
		CollectList.Append("<th style=\"text-align:left;\">청구상태</th></tr></thead><tbody>");
		string Current = "";
		for (int i = 0; i < Head.Count; i++) {
			string Transport_FromTo;
			string Vessel;
			string Status;

			if (Current != Head[i].Charge_No) {
				Transport_FromTo = Head[i].Transport_From + " - " + Head[i].Transport_To;
				Vessel = Head[i].VesselName;
			}
			else {
				Transport_FromTo = "";
				Vessel = "";
			}
			CollectList.Append("<tr><td>" + Transport_FromTo + "</td>");
			CollectList.Append("<td>" + Vessel + "</td>");
			CollectList.Append("<td>" + Head[i].Branch_Code + "</td>");
			CollectList.Append("<td>" + Head[i].Customer_Code + "</td>");
			CollectList.Append("<td>" + Common.NumberFormat(Head[i].Total_Price) + " " + Common.GetMonetaryUnit(Head[i].Monetary_Unit) + "</td>");
			CollectList.Append("<td>" + Common.NumberFormat(Head[i].Deposited_Price) + " " + Common.GetMonetaryUnit(Head[i].Monetary_Unit) + "</td>");
			CollectList.Append("<td>" + Common.NumberFormat((float.Parse(Head[i].Deposited_Price) - float.Parse(Head[i].Total_Price)) + "") + Common.GetMonetaryUnit(Head[i].Monetary_Unit) + "</td>");
			if (Common.NumberFormat(Head[i].Deposited_Price) == "0") {
				Status = "입금대기";
			}
			else if (float.Parse(Head[i].Deposited_Price) > 0 && float.Parse(Head[i].Deposited_Price) < float.Parse(Head[i].Total_Price)) {
				Status = "입금 진행중";
			}
			else {
				Status = "입금완료";
			}
			CollectList.Append("<td onclick=\"OpenCollect('" + Head[i].Table_Name + "', '" + Head[i].Table_Pk + "', '" + Head[i].RequestFormCalculate_Head_Pk + "')\">" + Status + "</td></tr>");

			Current = Head[i].Charge_No;
		}
		CollectList.Append("</tbody></table>");
		RS.Close();
		DB.DBCon.Close();

		ReturnValue.Add(MaxChargeNo);
		ReturnValue.Add(CollectList + "");
		return ReturnValue.ToArray();
	}
}