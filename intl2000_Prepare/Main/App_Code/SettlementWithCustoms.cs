using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using Components;

/// <summary>
/// SettlementWithCustoms의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
[System.Web.Script.Services.ScriptService]
public class SettlementWithCustoms : System.Web.Services.WebService
{

	public SettlementWithCustoms() {
		//디자인된 구성 요소를 사용하는 경우 다음 줄의 주석 처리를 제거합니다. 
		//InitializeComponent(); 
	}

	[WebMethod]
	public string SetMemoSave(string SettlementWithCustomsPk, string Memo) {
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE [dbo].[SettlementWithCustoms] SET [Memo] = " + Common.StringToDB(Memo, true, true) + " WHERE [SettlementWithCustomsPk]=" + SettlementWithCustomsPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public string DelSettlement(string SettlementWithCustomsPk) {
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "DELETE FROM [dbo].[SettlementWithCustoms] WHERE SettlementWithCustomsPk=" + SettlementWithCustomsPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public string DelSettlement2(string SettlementWithCustomsPk) {
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "DELETE FROM [dbo].[SettlementCustomerSend] WHERE [SettlementCustomsSendPk]=" + SettlementWithCustomsPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public string SetCustomsSend(string Date, string Price, string Description) {
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = string.Format("INSERT INTO [dbo].[SettlementCustomerSend] ([Date],[Price],[Description]) VALUES ({0}, {1}, {2});", Common.StringToDB(Date, true, false), Common.StringToDB(Price.Trim(), false, false), Common.StringToDB(Description.Trim(), true, true));
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public string SetSettlement_StorageMultiple(string Sum_TBBHPk, string Sum_Price, string Comment, string Date) {
		string[] TBBHPk = Sum_TBBHPk.Split(new string[] { "#!@" }, StringSplitOptions.None);
		string[] Price = Sum_Price.Split(new string[] { "#!@" }, StringSplitOptions.None);

		StringBuilder Query = new StringBuilder();
		for (var i = 0; i < TBBHPk.Length; i++) {
			if (TBBHPk[i].Trim() != "") {
				Query.Append(@"
					INSERT INTO [dbo].[TransportBBCharge]([TransportBBHeadPk],[PaymentBranchPk],[Date],[Title],[MonetaryUnitCL],[Price],[Registerd],[Comment]) VALUES (" + TBBHPk[i] + ",3157,'" + Date + "',N'" + Comment + "',20," + Price[i].Replace(",", "") + ",GETDATE(),'');");
			}
		}

		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = Query + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}


	[WebMethod]
	public string SetSettlement(string BLNoSum, string DateSum, string PriceSum, string DescriptionSum, string AccountIdSum) {
		string[] BLNo = BLNoSum.Split(new string[] { "#!@" }, StringSplitOptions.None);
		string[] Date = DateSum.Split(new string[] { "#!@" }, StringSplitOptions.None);
		string[] Price = PriceSum.Split(new string[] { "#!@" }, StringSplitOptions.None);
		string[] Description = DescriptionSum.Split(new string[] { "#!@" }, StringSplitOptions.None);
		string[] AccountId = AccountIdSum.Split(new string[] { "#!@" }, StringSplitOptions.None);

		StringBuilder Query = new StringBuilder();
		Components.SettlementWithCustoms SwC = new Components.SettlementWithCustoms();
		for (var i = 0; i < BLNo.Length; i++) {
			if (BLNo[i] == "") {
				continue;
			}
			Query.Append(Query_SetSettlement(BLNo[i], Date[i].Trim(), Price[i].Trim(), Description[i], AccountId[i]));
		}

		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = Query + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	private String Query_SetSettlement(string BLNo, string Date, string Price, string Description, string AccountId) {
		string Format_Query = @"INSERT INTO [dbo].[SettlementWithCustoms] ([BLNo],[Date],[Price],[Description],[AccountId]) VALUES ({0}, {1}, {2}, {3}, {4});";
		return string.Format(Format_Query,
			Common.StringToDB(BLNo, true, false),
			(Date == "" ? "Convert(varchar(10),Getdate(),111)" : Common.StringToDB(Date, true, false)),
			Common.StringToDB(Price, false, false),
			Common.StringToDB(Description, true, false),
			Common.StringToDB(AccountId, true, false));
	}

}
