using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Components;
using System.Data.SqlClient;
using System.Text;

/// <summary>
/// Finance의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
[System.Web.Script.Services.ScriptService]
public class Finance : System.Web.Services.WebService
{
	private DBConn DB;
	public Finance() {
	}

	[WebMethod]
	public String SetDelete_BankDeposit(string BankDepositPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"

			DECLARE @DatetimeStandard varchar(50); 
			DECLARE @BankDepositPk int; 

			SELECT TOP (1) @DatetimeStandard=[DATETIME] 
			FROM [INTL2010].[dbo].[BANK_DEPOSIT] 
			WHERE [BANK_DEPOSIT_PK]=" + BankDepositPk + @"; 

			SELECT TOP 1 @BankDepositPk=[BANK_DEPOSIT_PK] 
			FROM [INTL2010].[dbo].[BANK_DEPOSIT] 
			WHERE [DATETIME] < @DatetimeStandard 
			ORDER BY [DATETIME] DESC;
			
			UPDATE  [INTL2010].[dbo].[BANK_DEPOSIT] SET [PRICE_REMAIN]=null WHERE [BANK_DEPOSIT_PK]=@BankDepositPk; 
			DELETE FROM [INTL2010].[dbo].[BANK_DEPOSIT] WHERE [BANK_DEPOSIT_PK]=" + BankDepositPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		Calc_BankRemain("1", ref DB);
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String SetSave_BankDeposit(string BankPk, string Type, string TypePk, string Description, string MonetaryUnit, string Price, string DateTime) {
		Dictionary<string, string> Data = new Dictionary<string, string>();
		Data.Add("BANK_DEPOSIT_PK", "");
		Data.Add("BANK_PK", BankPk);
		Data.Add("CATEGORY", "");
		Data.Add("TYPE", Type);
		Data.Add("TYPE_PK", TypePk);
		Data.Add("DESCRIPTION", Description);
		Data.Add("MONETARY_UNIT", MonetaryUnit);
		Data.Add("PRICE", Price);
		Data.Add("PRICE_REMAIN", "");
		Data.Add("DATETIME", DateTime);
		Utility U = new Utility();
		string query = U.GetQuery("BANK_DEPOSIT", Data);
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = query;
		DB.SqlCmd.ExecuteNonQuery();

		Calc_BankRemain(BankPk, ref DB);
		DB.DBCon.Close();
		return "1";
	}
	public string SetBank_RequestTariff(string RequestPk) {
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT 
	a.rpt_no,a.BlNo	  
,r.Consigneecode
,company.CompanyName as CompanyName
,R.TotalPackedCount,R.PackingUnit
,a.Tot_Gs + a.Tot_Vat AS Price
FROM   [edicus].[dbo].CUSDEC929A1  a
left join [INTL2010].[dbo].[CommercialDocument]  c on a.BlNo=c.BLNo
left join [INTL2010].[dbo].[CommerdialConnectionWithRequest] ccwr on c.CommercialDocumentHeadPk=ccwr.CommercialDocumentPk
left join [INTL2010].[dbo].[RequestForm] R on ccwr.RequestFormPk=r.RequestFormPk
left join [INTL2010].[dbo].[Company] Company on R.ConsigneePk=Company.CompanyPk 
WHERE R.RequestFormPk=" + RequestPk + ";";


		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string BankPk = "1";
		string Type = "Request";
		string TypePk = RequestPk;
		string MonetaryUnit = "￦";
		string datetime = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("HH:mm");

		string Description = "";
		string Price = "";

		if (RS.Read()) {
			Description = RS["rpt_no"] + " " + RS["CompanyName"] + "[" + RS["Consigneecode"] + "] " + RS["TotalPackedCount"] + "GT";
			Price = "-" + RS["Price"];
		}
		RS.Close();
		DB.DBCon.Close();

		SetSave_BankDeposit(BankPk, Type, TypePk, Description, MonetaryUnit, Price, datetime);
		return "1";
	}
	public string Calc_BankRemain(string BankPk, ref DBConn DB) {
		DB.SqlCmd.CommandText = @"
			DECLARE @DatetimeStandard varchar(50); 
			DECLARE @BankPk int; 
			
			SET @BankPk=" + BankPk + @"; 

			SELECT TOP (1) @DatetimeStandard=[DATETIME] 
			FROM [INTL2010].[dbo].[BANK_DEPOSIT] 
			WHERE BANK_PK=@BankPk and PRICE_REMAIN is null ORDER BY [DATETIME] ASC; 

			SELECT TOP (1) @DatetimeStandard=[DATETIME]
			FROM [INTL2010].[dbo].[BANK_DEPOSIT] 
			WHERE BANK_PK=@BankPk and [DATETIME]<@DatetimeStandard
			ORDER BY [DATETIME] DESC; 

			SELECT [BANK_DEPOSIT_PK],[PRICE],[PRICE_REMAIN] 
			FROM [INTL2010].[dbo].[BANK_DEPOSIT] 
			WHERE BANK_PK=@BankPk and [DATETIME]>=@DatetimeStandard
			ORDER BY [DATETIME] ASC; ";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder Query = new StringBuilder();

		decimal PriceRemain = 0;
		if (RS.Read()) {
			if (RS["PRICE_REMAIN"] + "" == "") {
				PriceRemain = decimal.Parse(RS["PRICE"] + "");
				Query.Append("UPDATE [INTL2010].[dbo].[BANK_DEPOSIT] SET [PRICE_REMAIN]=" + RS["PRICE"]  + " WHERE [BANK_DEPOSIT_PK]=" + RS["BANK_DEPOSIT_PK"] + ";");
			} else {
				PriceRemain = decimal.Parse(RS["PRICE_REMAIN"] + "");
			}
		}

		while (RS.Read()) {
			decimal current = decimal.Parse(RS["PRICE"] + "");
			PriceRemain += current;
			Query.Append("UPDATE [INTL2010].[dbo].[BANK_DEPOSIT] SET [PRICE_REMAIN]=" + PriceRemain + " WHERE [BANK_DEPOSIT_PK]=" + RS["BANK_DEPOSIT_PK"] + ";");
		}
		RS.Close();
		DB.SqlCmd.CommandText = Query.ToString();
		DB.SqlCmd.ExecuteNonQuery();
		return "1";
	}




	[WebMethod]
	public String DepositedConfirm(string RequestFormDepositedPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE RequestFormDeposited SET [Confirmed] = getDate() WHERE RequestFormDepositedPk=" + RequestFormDepositedPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String TransportBetweenCompanyConfirm(string TBCPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE TransportBC SET [StepCL] = 0 WHERE [TransportBetweenCompanyPk]=" + TBCPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String DepositedCancel(string RequestFormDepositedPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE RequestFormDeposited  SET [Confirmed]=null WHERE [RequestFormDepositedPk]=" + RequestFormDepositedPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.SqlCmd.Clone();
		return "1";
	}
	[WebMethod]
	public String DepositedDelete(string RequestFormDepositedPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT RequestFormPk, GubunCL FROM RequestFormDeposited WHERE [RequestFormDepositedPk]=" + RequestFormDepositedPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		string RequestFormPk;
		string GubunCL;
		if (RS.Read()) {
			RequestFormPk = RS[0] + "";
			GubunCL = RS[1] + "";
		} else {
			RS.Dispose();
			DB.DBCon.Close();
			return "0";
		}
		RS.Dispose();

		string TempColumn = GubunCL == "0" ? "ShipperDeposited" : "ConsigneeDeposited";
		DB.SqlCmd.CommandText = "Declare @Deposited money;" +
			"DELETE FROM RequestFormDeposited WHERE [RequestFormDepositedPk]=" + RequestFormDepositedPk + "; " +
			"SELECT @Deposited =SUM([Charge]) FROM RequestFormDeposited WHERE RequestFormPk=" + RequestFormPk + " and GubunCL=" + GubunCL + "; " +
			"UPDATE RequestFormCalculateHead SET " + TempColumn + " = @Deposited WHERE [RequestFormPk]=" + RequestFormPk + ";";

		DB.SqlCmd.ExecuteNonQuery();
		DB.SqlCmd.Clone();
		return "1";
	}
}