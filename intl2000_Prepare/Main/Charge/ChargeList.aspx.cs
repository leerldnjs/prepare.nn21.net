using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Charge_ChargeList :System.Web.UI.Page
{
	protected string BranchPk = "3157";
	protected string Html_ChargeBody;
	protected void Page_Load(object sender, EventArgs e) {
		Html_ChargeBody = MakeHtml_ChargeBody();
	}

	private string MakeHtml_ChargeBody() {
		StringBuilder ReturnValue = new StringBuilder();

		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT
			HEAD.[REQUESTFORMCALCULATE_HEAD_PK]
			,HEAD.[TABLE_NAME]
			,HEAD.[TABLE_PK]
			,HEAD.[TYPE]
			,HEAD.[BRANCH_COMPANY_PK]
			,HEAD.[BRANCH_BANK_PK]
			,HEAD.[CUSTOMER_COMPANY_PK]
			,HEAD.[CUSTOMER_BANK_PK]
			,HEAD.[CHARGE_DATE]
			,HEAD.[LAST_DEPOSITED_DATE]
			,HEAD.[MONETARY_UNIT]
			,HEAD.[TOTAL_PRICE]
			,ISNULL(HEAD.[DEPOSITED_PRICE], 0) AS DEPOSITED_PRICE
			,RF.[TotalPackedCount]
			,RF.[PackingUnit]
			,RF.[ArrivalDate]
			,RF.[StepCL]
			,BRANCH.[CompanyCode] AS BRANCH_CODE
			,BRANCH.[CompanyName] AS BRANCH_NAME
			,CUSTOMER.[CompanyCode] AS CUSTOMER_CODE
			,CUSTOMER.[CompanyName] AS CUSTOMER_NAME
			,BBANK.[BankName] AS BRANCH_BANKNAME
			,BBANK.[OwnerName] AS BRANCH_BANKOWNER
		FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS HEAD
		LEFT JOIN [dbo].[RequestForm] AS RF ON HEAD.[TABLE_PK] = RF.[RequestFormPk]
		LEFT JOIN [dbo].[Company] AS BRANCH ON HEAD.[BRANCH_COMPANY_PK] = BRANCH.[CompanyPk]
		LEFT JOIN [dbo].[Company] AS CUSTOMER ON HEAD.[CUSTOMER_COMPANY_PK] = CUSTOMER.[CompanyPk]
		LEFT JOIN [dbo].[CompanyBank] AS BBANK ON HEAD.[BRANCH_BANK_PK] = BBANK.[CompanyBankPk]
		--WHERE HEAD.[BRANCH_COMPANY_PK] = " + BranchPk + @"
		WHERE (HEAD.[TOTAL_PRICE] >= ISNULL(HEAD.[DEPOSITED_PRICE], 0) AND RF.[StepCL] < 65)
		AND HEAD.[TABLE_NAME] = 'RequestForm'
		AND RF.[ArrivalDate] is not null
		ORDER BY RF.[ArrivalDate] DESC;";
		SqlDataReader RS =  DB.SqlCmd.ExecuteReader();
		string CurrentArrival = "";
		while (RS.Read()) {
			if (CurrentArrival != RS["ArrivalDate"] + "") {
				ReturnValue.Append("<tr><td class=\"text-sm text-dark\" colspan=\"10\" style=\"background-color:whitesmoke;\"><strong>도착일자 : " + RS["ArrivalDate"].ToString().Substring(0, 4) + "." + RS["ArrivalDate"].ToString().Substring(4, 2) + "." + RS["ArrivalDate"].ToString().Substring(6, 2) + "</strong></td></tr>");
			}
			ReturnValue.Append("<tr><td>" + RS["TotalPackedCount"] + " " + Common.GetPackingUnit(RS["PackingUnit"] + "") + "</td>");
			ReturnValue.Append("<td>" + RS["CUSTOMER_CODE"] + "</td>");
			ReturnValue.Append("<td>" + RS["CUSTOMER_NAME"] + "</td>");
			/*
			ReturnValue.Append("<td>" + RS["CHARGE_DATE"] + "</td>");
			ReturnValue.Append("<td>" + RS["LAST_DEPOSITED_DATE"] + "</td>");
			*/
			ReturnValue.Append("<td>" + RS["BRANCH_BANKNAME"] + "</td>");
			ReturnValue.Append("<td>" + Common.NumberFormat(RS["TOTAL_PRICE"] + "") + " " + Common.GetMonetaryUnit(RS["MONETARY_UNIT"] + "") + "</td>");
			ReturnValue.Append("<td>" + Common.NumberFormat(RS["DEPOSITED_PRICE"] + "") + " " + Common.GetMonetaryUnit(RS["MONETARY_UNIT"] + "") + "</td>");
			ReturnValue.Append("<td>" + Common.NumberFormat((float.Parse(RS["DEPOSITED_PRICE"] + "") - float.Parse(RS["TOTAL_PRICE"] + "")) + "") + Common.GetMonetaryUnit(RS["MONETARY_UNIT"] + "") + "</td>");
			string Status = "";
			if(Common.NumberFormat(RS["DEPOSITED_PRICE"] + "") == "0") {
				Status = "입금대기";
			}
			else if(float.Parse(RS["DEPOSITED_PRICE"] + "") > 0 && float.Parse(RS["DEPOSITED_PRICE"] + "") < float.Parse(RS["TOTAL_PRICE"] + "")) {
				Status = "입금 진행중";
			}
			else {
				Status = "입금완료";
			}
			ReturnValue.Append("<td onclick=\"OpenCollect('" + RS["TABLE_NAME"] + "', '" + RS["TABLE_PK"] + "', '" + RS["REQUESTFORMCALCULATE_HEAD_PK"] + "')\">" + Status + "</td>");
			ReturnValue.Append("<td>" + Common.GetStepCL(RS["StepCL"] + "") + "</td></tr>");

			CurrentArrival = RS["ArrivalDate"] + "";
		}

		DB.DBCon.Close();
		return ReturnValue + "";
	}
}