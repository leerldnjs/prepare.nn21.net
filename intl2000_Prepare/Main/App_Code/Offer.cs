using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Components;
using System.Data.SqlClient;
using System.Text;

/// <summary>
/// Offer의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
[System.Web.Script.Services.ScriptService]
public class Offer : System.Web.Services.WebService
{
	private DBConn DB;
	private String TempString;
	public Offer()
	{
		//디자인된 구성 요소를 사용하는 경우 다음 줄의 주석 처리를 제거합니다. 
		//InitializeComponent(); 
	}
	[WebMethod]
	public String GetCompanyNameNAddressbyE(string CompanyPk)
	{
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [CompanyNameE], [CompanyAddressE] FROM Company where CompanyPk=" + CompanyPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		RS.Read();
		TempString = RS[0] + "##" + RS[1];
		RS.Dispose();
		DB.DBCon.Close();
		return TempString;
	}
	[WebMethod]
	public String UpdateCompanyNameNAddressByE(string CompanyPk, string NameE, string AddressE, int Count)
	{
		DB = new DBConn();
		if (Count == 2) { DB.SqlCmd.CommandText = "UPDATE Company SET [CompanyNameE] = '" + NameE + "', [CompanyAddressE] = '" + AddressE + "' WHERE [CompanyPk]=" + CompanyPk + ";"; }
		else if (NameE != "N") { DB.SqlCmd.CommandText = "UPDATE Company SET [CompanyNameE] = '" + NameE + "' WHERE [CompanyPk]=" + CompanyPk + ";"; }
		else if (AddressE != "N") { DB.SqlCmd.CommandText = "UPDATE Company SET [CompanyAddressE] = '" + AddressE + "' WHERE [CompanyPk]=" + CompanyPk + ";"; }
		try {
			DB.DBCon.Open();
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
		}
		catch (Exception) {
			return "-1";
		}
		return "1";
	}
}

