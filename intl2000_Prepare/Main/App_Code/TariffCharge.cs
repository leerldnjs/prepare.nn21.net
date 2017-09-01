using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using Components;

/// <summary>
/// TariffCharge의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class TariffCharge : System.Web.Services.WebService
{
	private DBConn DB;
	public TariffCharge() {
	}

	[WebMethod]
	public String DeleteLChargeList(string ChargeTariffPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "DELETE FROM [INTL2010].[dbo].[ChargeTariff] WHERE [ChargeTariffPk]=" + ChargeTariffPk + ";" +
			"UPDATE [CommerdialConnectionWithRequest] SET [TariffCharge]=NULL WHERE [TariffCharge]=" + ChargeTariffPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public string ChargeTariff(string[] CDPk, string AccountID, string Amount, string Date) {
		StringBuilder CDPkSum = new StringBuilder();
		for (var i = 0; i < CDPk.Length; i++) {
			if (CDPkSum + "" != "") {
				CDPkSum.Append(", ");
			}
			CDPkSum.Append(CDPk[i]);
		}


		DB = new DBConn();
		DB.SqlCmd.CommandText = "INSERT INTO [ChargeTariff] ([TotalAmount], [Date], [Registerd]) VALUES ( " + Common.StringToDB(Amount, false, false) + ", " + Common.StringToDB(Date, true, false) + ", getDate() );" +
			"SELECT @@IDENTITY;";
		DB.DBCon.Open();
		string Identity = DB.SqlCmd.ExecuteScalar() + "";
		DB.SqlCmd.CommandText = "UPDATE [CommercialDocument] SET [StepCL]=3 WHERE [CommercialDocumentHeadPk] in (" + CDPkSum + ");" +
			"UPDATE [CommerdialConnectionWithRequest] SET [TariffCharge]=" + Identity + " WHERE [CommercialDocumentPk] in (" + CDPkSum + ");";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();

		return "1";
	}
}