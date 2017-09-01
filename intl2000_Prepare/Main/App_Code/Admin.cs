using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;

/// <summary>
/// Admin_Con의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
 [System.Web.Script.Services.ScriptService]
public class Admin :System.Web.Services.WebService
{

	private DBConn DB;
	private StringBuilder Query;
	private String TempString;

	public Admin() {
	}

	[WebMethod]
	public string DELETECompanyBank(string Pk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = string.Format("update CompanyBank set IsDel=1 where CompanyBankPk={0}", Pk);
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public string DeleteTransportBBCharge(string Pk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = string.Format("DELETE FROM [dbo].[TransportBBCharge] WHERE [TransportBBChargePk]={0}", Pk);
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public string SetTaxpaid(string RequestFormPk, string AccountID, string GubunCL, string DocumentStepCL) {
		//SetTaxpaid('75380','ilic31','762','');
		HistoryP HisP = new HistoryP();
		HistoryC HisC = new HistoryC();
		sHistory History;
		DB = new DBConn();
		DB.DBCon.Open();
		StringBuilder Query = new StringBuilder();
		HisP.Delete_History("RequestForm", RequestFormPk, GubunCL, ref DB);

		History = new sHistory();
		History.Table_Name = "RequestForm";
		History.Table_Pk = RequestFormPk;
		History.Code = GubunCL;
		History.Account_Id = AccountID;
		History.Description = "세납지시";
		HisC.Set_History(History, ref DB);

		if (DocumentStepCL != "") {
			HisP.Delete_History("RequestForm", RequestFormPk, DocumentStepCL, ref DB);
			History.Code = DocumentStepCL;
			Query.Append("UPDATE RequestForm SET [DocumentStepCL] = " + DocumentStepCL + " WHERE RequestFormPk=" + RequestFormPk + ";");
		}
		else {
			HisP.Delete_History("RequestForm", RequestFormPk, GubunCL, ref DB);
			History.Code = GubunCL;
		}
		HisC.Set_History(History, ref DB);
		DB.SqlCmd.CommandText = Query.ToString();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		if (GubunCL == "762") {
			new Finance().SetBank_RequestTariff(RequestFormPk);
		}
		return "1";
	}

	[WebMethod]
	public string companyaddinfo199_3157(string value) {
		DB = new DBConn();
		//value = value.Replace("\r\n", "<br/>");
		DB.SqlCmd.CommandText = string.Format(@"
			DELETE FROM [dbo].[CompanyAdditionalInfomation] WHERE Title=199 and CompanyPk=3157;
			INSERT INTO CompanyAdditionalInfomation ([CompanyPk], [Title], [Value]) VALUES (3157, 199, '{0}');", value);
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	//   [WebMethod]
	//   public string companyaddinfo199_3157(string value)
	//   {
	//      DB = new DBConn();
	//      //value = value.Replace("\r\n", "<br/>");
	//      DB.SqlCmd.CommandText = string.Format(@"
	//			DELETE FROM [dbo].[CompanyAdditionalInfomation] WHERE Title=199 and CompanyPk=3157;
	//			INSERT INTO CompanyAdditionalInfomation ([CompanyPk], [Title], [Value]) VALUES (3157, 199, '{0}');", value);
	//      DB.DBCon.Open();
	//      DB.SqlCmd.ExecuteNonQuery();
	//      DB.DBCon.Close();
	//      return "1";
	//   }
	[WebMethod]
	public string SetClearance_Ready(string BlNo, string AccountID) {
		Ready ready = new Ready();
		return ready.SetClearance_Ready_SS(BlNo, AccountID);
	}

	[WebMethod]
	public string SetOrder_RAN(string CommercialDocumentHeadPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT
	RI.RequestFormItemsPk
	, CICK.Material AS CMaterial
	, REPLACE(REPLACE(REPLACE(CICK.HSCode, '.', ''), ' ', ''), '-', '') AS HSCode
FROM RequestForm AS RF
		inner join CommerdialConnectionWithRequest AS CCWR  ON RF.RequestFormPk=CCWR.RequestFormPk
		Left join RequestFormItems as RI on RF.RequestFormPk=RI.RequestFormPk
		left join ClearanceItemCodeKOR AS CICK ON RI.ItemCode=CICK.ItemCode
WHERE CCWR.CommercialDocumentPk =" + CommercialDocumentHeadPk + @"
ORDER BY HSCode, CMaterial, RequestFormItemsPk ; ";
		StringBuilder ReturnValue = new StringBuilder();
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		int CurrentRAN = 0;
		string CurrentMaterial = "";
		string CurrentHSCode = "akldsfjaslkflksadfjkl";
		while (RS.Read()) {
			if (CurrentHSCode == RS["HSCode"] + "" && CurrentMaterial == RS["CMaterial"] + "") {
				ReturnValue.Append("@" + RS["RequestFormItemsPk"] + "!" + CurrentRAN);
			}
			else {
				CurrentRAN++;
				CurrentHSCode = RS["HSCode"] + "";
				CurrentMaterial = RS["CMaterial"] + "";
				ReturnValue.Append("@" + RS["RequestFormItemsPk"] + "!" + CurrentRAN);
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue.ToString().Substring(1);
	}

	[WebMethod]
	public String RequestWriteforEstimation(string RequestformPk, string AccountID) {
		try {
			DB = new DBConn();
			DB.DBCon.Open();
			StringBuilder Query = new StringBuilder();

			Query.Append("DECLARE @RequestPk int ;" +
				"INSERT INTO [RequestForm] ([ShipperPk], [ConsigneePk], [AccountID], [ShipperCode], [ConsigneeCode], [ShipperClearanceNamePk], [ConsigneeClearanceNamePk], [DepartureDate], [ArrivalDate], [SHIPMENTDATE] " +
				", [DepartureRegionCode], [ArrivalRegionCode], [DepartureBranchPk], [ArrivalBranchPk], [TransportWayCL], [PaymentWhoCL], [DocumentRequestCL], [MonetaryUnitCL], [StepCL], [Memo], [RequestDate])" +
				"select [ShipperPk], [ConsigneePk], '" + AccountID + "', [ShipperCode], [ConsigneeCode], [ShipperClearanceNamePk], [ConsigneeClearanceNamePk], [DepartureDate], [ArrivalDate], [SHIPMENTDATE] " +
				",[DepartureRegionCode], [ArrivalRegionCode], [DepartureBranchPk], [ArrivalBranchPk], [TransportWayCL], [PaymentWhoCL], [DocumentRequestCL], [MonetaryUnitCL], '50', [Memo], getDate() from RequestForm" +
				" where RequestFormPk=" + RequestformPk + ";" +
				"SET @RequestPk=@@IDENTITY;");

			Query.Append("INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID]) VALUES ('RequestForm', @RequestPk, '50', '" + AccountID + "');");

			Query.Append("INSERT INTO [INTL2010].[dbo].[RequestFormItems]([RequestFormPk],[ItemCode],[MarkNNumber],[Description],[Label],[Material],[Quantity],[QuantityUnit],[PackedCount],[PackingUnit]" +
						",[GrossWeight],[NetWeight],[Volume],[UnitPrice],[Amount],[LastModify],[RAN],[HSCodeForCO])" +
						 "SELECT @RequestPk,[ItemCode],[MarkNNumber],[Description],[Label],[Material],[Quantity],[QuantityUnit],[PackedCount],[PackingUnit]" +
						 ",[GrossWeight],[NetWeight],[Volume],[UnitPrice],[Amount],[LastModify],[RAN],[HSCodeForCO] FROM [INTL2010].[dbo].[RequestFormItems]" +
						 "where RequestFormPk=" + RequestformPk + ";");

			Query.Append("INSERT INTO [dbo].[COMMENT] ([TABLE_NAME], [TABLE_PK], [CATEGORY], [ACCOUNT_PK], [ACCOUNT_ID], [ACCOUNT_NAME], [CONTENTS], [REGISTERD]) " +
				"SELECT [TABLE_NAME], @RequestPk, [CATEGORY], [ACCOUNT_PK], [ACCOUNT_ID], [ACCOUNT_NAME], [CONTENTS],[REGISTERD]" +
				"FROM [dbo].[COMMENT]" +
				"WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK]=" + RequestformPk + ";");
			//
			Query.Append("INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID]) VALUES ('RequestForm', " + RequestformPk + ", '85', '" + AccountID + "');");
			DB.SqlCmd.CommandText = Query + "";
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		} catch (Exception ex) {
			return ex.Message;
		}
	}

	[WebMethod]
	public String SetCarryover(string Method, string AttachedRequestFormPk, string OriginalRequestFormPk, string SorC, string Amount) {
		DB = new DBConn();

		switch (Method) {
			case "Set":
				DB.SqlCmd.CommandText = @"
					INSERT INTO [dbo].[RequestFormCalculateCarryover]
						([AttachedRequestFormPk], [OriginalRequestFormPk], [SorC], [Price]) VALUES (
						" + AttachedRequestFormPk + ", " + OriginalRequestFormPk + ", '" + SorC + "', " + Amount + ");";
				break;

			case "Remove":
				DB.SqlCmd.CommandText = "DELETE FROM [dbo].[RequestFormCalculateCarryover] WHERE [OriginalRequestFormPk]=" + OriginalRequestFormPk + ";";
				break;

			default:
				DB.SqlCmd.CommandText = "DELETE FROM [dbo].[RequestFormCalculateCarryover] WHERE [OriginalRequestFormPk]=" + OriginalRequestFormPk + @";
					INSERT INTO [dbo].[RequestFormCalculateCarryover]
						([AttachedRequestFormPk], [OriginalRequestFormPk], [SorC], [Price]) VALUES (
						" + AttachedRequestFormPk + ", " + OriginalRequestFormPk + ", '" + SorC + "', " + Amount + ");";
				break;
		}
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String AddOurBranchStorage(string OurBranchStoragePk, string OurBranchCode, string StorageName, string StorageCode, string StorageAddress, string TEL, string FAX, string NaverMapPathX, string NaverMapPathY) {
		DB = new DBConn();
		if (OurBranchStoragePk == "New") {
			DB.SqlCmd.CommandText = "INSERT INTO [dbo].[OurBranchStorageCode] ([OurBranchCode],[StorageName],[StorageCode],[StorageAddress],[TEL],[FAX],[NaverMapPathX],[NaverMapPathY]) VALUES (" +
				Common.StringToDB(OurBranchCode, false, false) + ", " +
				Common.StringToDB(StorageName, true, true) + ", " +
				Common.StringToDB(StorageCode, true, true) + ", " +
				Common.StringToDB(StorageAddress, true, true) + ", " +
				Common.StringToDB(TEL, true, false) + ", " +
				Common.StringToDB(FAX, true, false) + ", " +
				Common.StringToDB(NaverMapPathX, true, false) + ", " +
				Common.StringToDB(NaverMapPathY, true, false) + ");";
		}
		else {
			DB.SqlCmd.CommandText = "   UPDATE [dbo].[OurBranchStorageCode] SET " +
				" [StorageName]=" + Common.StringToDB(StorageName, true, true) +
				", [StorageCode]=" + Common.StringToDB(StorageCode, true, true) +
				", [StorageAddress]=" + Common.StringToDB(StorageAddress, true, true) +
				", [TEL]=" + Common.StringToDB(TEL, true, false) +
				", [FAX]=" + Common.StringToDB(FAX, true, false) +
				", [NaverMapPathX]=" + Common.StringToDB(NaverMapPathX, true, false) +
				", [NaverMapPathY]=" + Common.StringToDB(NaverMapPathY, true, false) +
				" WHERE [OurBranchStoragePk]=" + OurBranchStoragePk + ";";
		}
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String DaeNapP(string Type, string RequestFormPk, string AccountID) {
		DB = new DBConn();
		HistoryC HisC = new HistoryC();
		sHistory History;
		sComment Comment;
		DB.DBCon.Open();
		if (Type == "Hold") {
			DB.SqlCmd.CommandText = "INSERT INTO [RequestFormHold] ([RequestFormPk], [AccountID]) VALUES (" + Common.StringToDB(RequestFormPk, false, false) + ", " + Common.StringToDB(AccountID, true, false) + ");";
			History = new sHistory();
			Comment = new sComment();
			History.Table_Name = "RequestForm";
			History.Table_Pk = RequestFormPk;
			History.Code = "45";
			History.Account_Id = AccountID;
			HisC.Set_History(History, ref DB);

			Comment.Table_Name = "RequestForm";
			Comment.Table_Pk = RequestFormPk;
			Comment.Category = "Request";
			Comment.Contents = "<span style=\"color:red;\">대납지시 [ 代納指示 ]</span>";
			Comment.Account_Id = AccountID;
			HisC.Set_Comment(Comment, ref DB);
		}
		else {
			DB.SqlCmd.CommandText = "DELETE FROM [RequestFormHold] WHERE [RequestFormPk]=" + RequestFormPk + ";";
			History = new sHistory();
			Comment = new sComment();
			History.Table_Name = "RequestForm";
			History.Table_Pk = RequestFormPk;
			History.Code = "46";
			History.Account_Id = AccountID;
			HisC.Set_History(History, ref DB);

			Comment.Table_Name = "RequestForm";
			Comment.Table_Pk = RequestFormPk;
			Comment.Category = "Request";
			Comment.Contents = "<span style=\"color:red;\">대납완료 [ 代納完成 ]</span>";
			Comment.Account_Id = AccountID;
			HisC.Set_Comment(Comment, ref DB);
		}

		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String FileDelete(string FilePk, string Table) {
		DB = new DBConn();
		if (Table == "ClearancedFile") {
			DB.SqlCmd.CommandText = "DELETE FROM ClearancedFile WHERE [ClearancedFilePk]=" + FilePk + ";";
		}
		else {
			DB.SqlCmd.CommandText = "DELETE FROM [File] WHERE FilePk=" + FilePk + ";";
		}
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String FileGubunCLChange(string Table, string FilePk, string GubunCL) {
		DB = new DBConn();
		if (Table == "ClearancedFile") {
			DB.SqlCmd.CommandText = "update ClearancedFile SET GubunCL=" + GubunCL + " WHERE ClearancedFilePk=" + FilePk + ";";
		}
		else {
			DB.SqlCmd.CommandText = "UPDATE [INTL2010].[dbo].[File] SET GubunCL=" + GubunCL + " WHERE [FilePk]=" + FilePk + ";";
		}
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String UpdateCompanyInfo(string GUBUNCL, string COMPANYPK, string COMPANYNAME, string COMPANYNAMEE, string REGIONCODE, string COMPANYADDRESS, string COMPANYTEL, string COMPANYFAX, string PRESIDENTNAME, string PRESIDENTEMAIL, string COMPANYNO, string COMPANYADDITIONAL61, string COMPANYADDITIONAL62, string COMPANYADDITIONAL63, string COMPANYADDITIONAL64, string COMPANYADDITIONAL65, string COMPANYADDITIONAL80, string STAFFSUM, string WAREHOUSESUM, string RESPONSIBLESTAFF) {
		//return PRESIDENTNAME;
		Query = new StringBuilder();
		DB = new DBConn();
		if (REGIONCODE != "*N!C$") {
			DB.SqlCmd.CommandText = "SELECT [RegionCode] FROM RegionCode WHERE RegionCodePk=" + REGIONCODE + ";";
			DB.DBCon.Open();
			REGIONCODE = DB.SqlCmd.ExecuteScalar() + "";
			DB.DBCon.Close();
		}
		Query.Append(" UPDATE Company SET GubunCL = " + GUBUNCL +
			(COMPANYNAME == "*N!C$" ? "" : ", CompanyName = " + Common.StringToDB(COMPANYNAME, true, true)) +
			(COMPANYNAMEE == "*N!C$" ? "" : ", CompanyNamee = " + Common.StringToDB(COMPANYNAMEE, true, true)) +
			(REGIONCODE == "*N!C$" ? "" : ", RegionCode = " + Common.StringToDB(REGIONCODE, true, false)) +
			(COMPANYADDRESS == "*N!C$" ? "" : ", CompanyAddress = " + Common.StringToDB(COMPANYADDRESS, true, true)) +
			(COMPANYTEL == "*N!C$" ? "" : ", CompanyTEL = " + Common.StringToDB(COMPANYTEL, true, false)) +
			(COMPANYFAX == "*N!C$" ? "" : ", CompanyFAX = " + Common.StringToDB(COMPANYFAX, true, false)) +
			(PRESIDENTNAME == "*N!C$" ? "" : ", PresidentName = " + Common.StringToDB(PRESIDENTNAME, true, true)) +
			(PRESIDENTEMAIL == "*N!C$" ? "" : ", PresidentEmail = " + Common.StringToDB(PRESIDENTEMAIL, true, false)) +
			(COMPANYNO == "*N!C$" ? "" : ", CompanyNo = " + Common.StringToDB(COMPANYNO, true, false)) +
			(RESPONSIBLESTAFF == "*N!C$" ? "" : ", [ResponsibleStaff]= " + Common.StringToDB(RESPONSIBLESTAFF, true, false)) +
			" WHERE CompanyPk=" + COMPANYPK + ";");
		//return Query + "";
		string CAIUpdateQ = "	DELETE FROM CompanyAdditionalInfomation WHERE [CompanyPk] = {0} and [Title] = {1};" +
										"	INSERT INTO CompanyAdditionalInfomation ([CompanyPk], [Title], [Value]) VALUES ({0}, {1}, {2});";

		if (COMPANYADDITIONAL61 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL61, true, true) != "NULL") {
			Query.Append(string.Format(CAIUpdateQ, COMPANYPK, "61", Common.StringToDB(COMPANYADDITIONAL61, true, true)));
		}
		if (COMPANYADDITIONAL62 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL62, true, true) != "NULL") {
			Query.Append(string.Format(CAIUpdateQ, COMPANYPK, "62", Common.StringToDB(COMPANYADDITIONAL62, true, true)));
		}
		if (COMPANYADDITIONAL63 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL63, true, true) != "NULL") {
			Query.Append(string.Format(CAIUpdateQ, COMPANYPK, "63", Common.StringToDB(COMPANYADDITIONAL63, true, true)));
		}
		if (COMPANYADDITIONAL64 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL64, true, true) != "NULL") {
			Query.Append(string.Format(CAIUpdateQ, COMPANYPK, "64", Common.StringToDB(COMPANYADDITIONAL64, true, true)));
		}
		if (COMPANYADDITIONAL65 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL65, true, true) != "NULL") {
			Query.Append(string.Format(CAIUpdateQ, COMPANYPK, "65", Common.StringToDB(COMPANYADDITIONAL65, true, true)));
		}
		if (COMPANYADDITIONAL80 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL80, true, true) != "NULL") {
			Query.Append(string.Format(CAIUpdateQ, COMPANYPK, "80", Common.StringToDB(COMPANYADDITIONAL80, true, true)));
		}

		string CAIUpdateD = "	DELETE FROM CompanyAdditionalInfomation WHERE [CompanyPk] = {0} and [Title] = {1};";

		if (COMPANYADDITIONAL61 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL61, true, true) == "NULL") {
			Query.Append(string.Format(CAIUpdateD, COMPANYPK, "61", Common.StringToDB(COMPANYADDITIONAL61, true, true)));
		}
		if (COMPANYADDITIONAL62 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL62, true, true) == "NULL") {
			Query.Append(string.Format(CAIUpdateD, COMPANYPK, "62", Common.StringToDB(COMPANYADDITIONAL62, true, true)));
		}
		if (COMPANYADDITIONAL63 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL63, true, true) == "NULL") {
			Query.Append(string.Format(CAIUpdateD, COMPANYPK, "63", Common.StringToDB(COMPANYADDITIONAL63, true, true)));
		}
		if (COMPANYADDITIONAL64 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL64, true, true) == "NULL") {
			Query.Append(string.Format(CAIUpdateD, COMPANYPK, "64", Common.StringToDB(COMPANYADDITIONAL64, true, true)));
		}
		if (COMPANYADDITIONAL65 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL65, true, true) == "NULL") {
			Query.Append(string.Format(CAIUpdateD, COMPANYPK, "65", Common.StringToDB(COMPANYADDITIONAL65, true, true)));
		}
		if (COMPANYADDITIONAL80 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL80, true, true) == "NULL") {
			Query.Append(string.Format(CAIUpdateD, COMPANYPK, "80", Common.StringToDB(COMPANYADDITIONAL80, true, true)));
		}

		string[] StaffRow = STAFFSUM.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
		foreach (string EachRow in StaffRow) {
			string[] Each = EachRow.Split(Common.Splite321, StringSplitOptions.None);
			if (Each[0] == "") {
				Query.Append(" INSERT INTO [Account_] ([GubunCL], [CompanyPk], [Duties], [Name], [TEL], [Mobile], [Email], [IsEmailNSMS]) VALUES ( " +
										"99, " +
										COMPANYPK + ", " +
										Common.StringToDB(Each[1], true, true) + ", " +
										Common.StringToDB(Each[2], true, true) + ", " +
										Common.StringToDB(Each[3], true, false) + ", " +
										Common.StringToDB(Each[4], true, false) + ", " +
										Common.StringToDB(Each[5], true, false) + ", " +
										Common.StringToDB(Each[6], false, false) + ");");
				if (Each[7] != "") {
					Query.Append("INSERT INTO AccountAdditionalInfo_ ([AccountPk], [GubunCL], [Value]) VALUES (@@IDENTITY, 1, " + Common.StringToDB(Each[7], true, true) + ");");
				}
			}
			else {
				Query.Append("UPDATE [Account_] SET [Duties] = " + Common.StringToDB(Each[1], true, true) +
					", [Name] = " + Common.StringToDB(Each[2], true, true) +
					", [TEL] = " + Common.StringToDB(Each[3], true, false) +
					", [Mobile] = " + Common.StringToDB(Each[4], true, false) +
					", [Email] = " + Common.StringToDB(Each[5], true, false) +
					", [IsEmailNSMS] = " + Common.StringToDB(Each[6], false, false) +
				" WHERE AccountPk=" + Each[0] + ";");
				Query.Append("	DELETE FROM AccountAdditionalInfo_ WHERE GubunCL=1 and [AccountPk]=" + Each[0] + ";" +
										"	INSERT INTO AccountAdditionalInfo_ ([AccountPk], [GubunCL], [Value]) VALUES (" + Each[0] + ", 1, " + Common.StringToDB(Each[7], true, true) + ");");
			}
		}
		if (WAREHOUSESUM != "") {
			string[] warehouseRow = WAREHOUSESUM.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
			foreach (string row in warehouseRow) {
				string[] each = row.Split(Common.Splite22, StringSplitOptions.None);
				if (each[0] == "") {
					Query.Append("INSERT INTO [CompanyWarehouse] ([CompanyPk], [Title], [Address], [TEL], [Staff], [Mobile], [Memo]) VALUES (" +
						COMPANYPK + ", " +
						Common.StringToDB(each[1], true, true) + ", " +
						Common.StringToDB(each[2], true, true) + ", " +
						Common.StringToDB(each[3], true, false) + ", " +
						Common.StringToDB(each[4], true, true) + ", " +
						Common.StringToDB(each[5], true, false) + ", " +
						Common.StringToDB(each[6], true, false) + ");");
				}
				else {
					Query.Append("UPDATE [CompanyWarehouse] SET [Title] = " + Common.StringToDB(each[1], true, true) +
						", [Address] = " + Common.StringToDB(each[2], true, true) +
						", [TEL] = " + Common.StringToDB(each[3], true, false) +
						", [Staff] = " + Common.StringToDB(each[4], true, true) +
						", [Mobile] = " + Common.StringToDB(each[5], true, false) +
						", [Memo] = " + Common.StringToDB(each[6], true, false) + " WHERE WarehousePk=" + each[0] + ";");
				}
			}
		}
		DB.SqlCmd.CommandText = Query + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}


	[WebMethod]
	public String UpdateCompanyInfo_Logistics(string COMPANYPK, string STAFFSUM, string WAREHOUSESUM) {
		//return PRESIDENTNAME;
		Query = new StringBuilder();
		DB = new DBConn();

		string[] StaffRow = STAFFSUM.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
		foreach (string EachRow in StaffRow) {
			string[] Each = EachRow.Split(Common.Splite321, StringSplitOptions.None);
			if (Each[0] == "") {
				Query.Append(" INSERT INTO [Account_] ([GubunCL], [CompanyPk], [Duties], [Name], [TEL], [Mobile], [Email], [IsEmailNSMS]) VALUES ( " +
										"99, " +
										COMPANYPK + ", " +
										Common.StringToDB(Each[1], true, true) + ", " +
										Common.StringToDB(Each[2], true, true) + ", " +
										Common.StringToDB(Each[3], true, false) + ", " +
										Common.StringToDB(Each[4], true, false) + ", " +
										Common.StringToDB(Each[5], true, false) + ", " +
										Common.StringToDB(Each[6], false, false) + ");");
				if (Each[7] != "") {
					Query.Append("INSERT INTO AccountAdditionalInfo_ ([AccountPk], [GubunCL], [Value]) VALUES (@@IDENTITY, 1, " + Common.StringToDB(Each[7], true, true) + ");");
				}
			}
			else {
				Query.Append("UPDATE [Account_] SET [Duties] = " + Common.StringToDB(Each[1], true, true) +
					", [Name] = " + Common.StringToDB(Each[2], true, true) +
					", [TEL] = " + Common.StringToDB(Each[3], true, false) +
					", [Mobile] = " + Common.StringToDB(Each[4], true, false) +
					", [Email] = " + Common.StringToDB(Each[5], true, false) +
					", [IsEmailNSMS] = " + Common.StringToDB(Each[6], false, false) +
				" WHERE AccountPk=" + Each[0] + ";");
				Query.Append("	DELETE FROM AccountAdditionalInfo_ WHERE GubunCL=1 and [AccountPk]=" + Each[0] + ";" +
										"	INSERT INTO AccountAdditionalInfo_ ([AccountPk], [GubunCL], [Value]) VALUES (" + Each[0] + ", 1, " + Common.StringToDB(Each[7], true, true) + ");");
			}
		}
		if (WAREHOUSESUM != "") {
			string[] warehouseRow = WAREHOUSESUM.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
			foreach (string row in warehouseRow) {
				string[] each = row.Split(Common.Splite22, StringSplitOptions.None);
				if (each[0] == "") {
					Query.Append("INSERT INTO [CompanyWarehouse] ([CompanyPk], [Title], [Address], [TEL], [Staff], [Mobile], [Memo]) VALUES (" +
						COMPANYPK + ", " +
						Common.StringToDB(each[1], true, true) + ", " +
						Common.StringToDB(each[2], true, true) + ", " +
						Common.StringToDB(each[3], true, false) + ", " +
						Common.StringToDB(each[4], true, true) + ", " +
						Common.StringToDB(each[5], true, false) + ", " +
						Common.StringToDB(each[6], true, false) + ");");
				}
				else {
					Query.Append("UPDATE [CompanyWarehouse] SET [Title] = " + Common.StringToDB(each[1], true, true) +
						", [Address] = " + Common.StringToDB(each[2], true, true) +
						", [TEL] = " + Common.StringToDB(each[3], true, false) +
						", [Staff] = " + Common.StringToDB(each[4], true, true) +
						", [Mobile] = " + Common.StringToDB(each[5], true, false) +
						", [Memo] = " + Common.StringToDB(each[6], true, false) + " WHERE WarehousePk=" + each[0] + ";");
				}
			}
		}
		DB.SqlCmd.CommandText = Query + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String LoadCompanyInfo(string CompanyPk) {
		DB = new DBConn();
		StringBuilder ReturnValue = new StringBuilder();
		string RegionNameBefore;
		DB.SqlCmd.CommandText = @"SELECT CompanyCode, CompanyName, RegionCode, CompanyAddress, CompanyTEL, CompanyFAX, PresidentName
										, PresidentEmail, CompanyNo, CompanyNameE, CompanyAddressE, GubunCL, ResponsibleStaff , Memo
									FROM Company WHERE CompanyPk=" + CompanyPk + ";";

		string RegionCode;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			RegionCode = RS[2] + "";
			RegionNameBefore = RS[0] + "#@!" + RS[1] + "#@!";
			ReturnValue.Append("#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5] + "#@!" + RS[6] + "#@!" + RS[7] + "#@!" + RS[8] + "#@!" + RS[9] + "#@!" + RS[10] + "#@!" + RS[11] + "#@!" + RS[12] + "#@!" + RS[13] + "%!$@#");
		}
		else {
			DB.DBCon.Close();
			return "N";
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"
SELECT
	A.AccountPk, A.AccountID, A.Duties, A.Name, A.TEL, A.Mobile, A.Email, A.IsEmailNSMS,
	AAI.Value
FROM
	Account_ AS A
	left join AccountAdditionalInfo_ AS AAI ON A.AccountPk=AAI.AccountPk
WHERE
	A.CompanyPk=" + CompanyPk + @"
	and (AAI.GubunCL=1 or Isnull(AAI.GubunCL, 0)=0)
ORDER BY A.GubunCL;";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append("A#@!" + RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5] + "#@!" + RS[6] + "#@!" + RS[7] + "#@!" + RS[8] + "%!$@#");
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = "SELECT Title, Value FROM CompanyAdditionalInfomation WHERE CompanyPk=" + CompanyPk + " and Title in (61, 62, 63, 64, 65, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80) ORDER BY Title asc;";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append(RS[0] + "#@!" + RS[1] + "%!$@#");
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = "SELECT [WarehousePk], [Title], [Address], [TEL], [Staff], [Mobile], [Memo] FROM [CompanyWarehouse] WHERE [CompanyPk]=" + CompanyPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append("W#@!" + RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5] + "#@!" + RS[6] + "%!$@#");
		}
		RS.Dispose();
		/*CompanyBroker 주석
        DB.SqlCmd.CommandText = "SELECT [BrokerPk], [Title], [Address], [TEL], [Staff], [Mobile], [Memo] FROM [CompanyBroker] WHERE [CompanyPk]=" + CompanyPk + ";";
        RS = DB.SqlCmd.ExecuteReader();
        while (RS.Read())
        {
            ReturnValue.Append("B#@!" + RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5] + "#@!" + RS[6] + "%!$@#");
        }
        RS.Dispose();
        */
		DB.SqlCmd.CommandText = "SELECT [FilePk], [Title], [FileName] FROM [INTL2010].[dbo].[File] WHERE GubunCL=1 and GubunPk=" + CompanyPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append("F#@!" + RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "%!$@#");
		}
		RS.Dispose();
		DB.DBCon.Close();
		return RegionNameBefore + (RegionCode == "" ? "" : GetRegionNameFromRegionCode(RegionCode)) + ReturnValue + "";
	}

	private String GetRegionNameFromRegionCode(string RegionCode) {
		bool isFirst = true;
		string RegionName = "";
		DB = new DBConn();
		DB.DBCon.Open();
		while (RegionCode.Length != 1) {
			if (isFirst) {
				isFirst = false;
			}
			else {
				RegionCode = RegionCode.Substring(0, RegionCode.Length - 3);
			}
			DB.SqlCmd.CommandText = "SELECT [Name],[NameE] FROM RegionCode WHERE RegionCode='" + RegionCode + "';";

			if (RegionName == "")
				RegionName = DB.SqlCmd.ExecuteScalar() + "";
			else
				RegionName = DB.SqlCmd.ExecuteScalar() + " >> " + RegionName;
		}

		DB.DBCon.Close();
		return RegionName;
	}

	private String GetRegionNameFromRegionCodeBranch(string RegionCode) {
		bool isFirst = true;
		string RegionName = "";
		DB = new DBConn();
		DB.DBCon.Open();
		while (RegionCode.Length != 1) {
			if (isFirst) {
				isFirst = false;
			}
			else {
				RegionCode = RegionCode.Substring(0, RegionCode.Length - 3);
			}
			DB.SqlCmd.CommandText = "SELECT [Name],[NameE] FROM RegionCode WHERE RegionCode='" + RegionCode + "';";
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			//if (RegionName == "")
			//	RegionName = DB.SqlCmd.ExecuteScalar() + "";
			//else
			//	RegionName = DB.SqlCmd.ExecuteScalar() + " >> " + RegionName;
			while (RS.Read()) {
				if (RegionName == "")
					RegionName = RS[0] + "|" + RS[1] + "";
				else
					RegionName = RS[0] + "|" + RS[1] + " >> " + RegionName;
			}
			RS.Dispose();
		}

		DB.DBCon.Close();
		return RegionName;
	}

	[WebMethod]
	public String AskCompanyCustomerCodeSetAuto(string RequestFormPk) {
		string ShipperPk = string.Empty;
		string Country = string.Empty;
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT ShipperPk, ShipperCode, DepartureRegionCode FROM RequestForm WHERE RequestFormPk=" + RequestFormPk;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		RS.Read();
		if (RS[1] + "" != "") {
			RS.Dispose();
			DB.DBCon.Close();
			return "0";
		}
		ShipperPk = RS[0] + "";
		Country = RS[2] + "";
		RS.Dispose();
		DB.DBCon.Close();

		switch (Country[0]) {
			case '1':
				Country = "KOR";
				break;

			case '2':
				Country = "CHN";
				break;
		}
		TempString = AutoCompanyCode(Country);
		return Country + TempString + "!!" + ShipperPk;
	}//RequestFormView.aspx 에서 수출담당지사에서 Shipper 신규고객일경우 번호 검색해서 묻기

	[WebMethod]
	public String AutoCompanyCode(string Country) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "	SELECT TOP 20 cast(right(CompanyCode, len(CompanyCode)-" + Country.Length + ") AS int) as CC " +
													"	FROM Company " +
													"	WHERE left(CompanyCode, " + Country.Length + ")='" + Country + "' and GubunCL in (0, 1)" +
													"	ORDER BY  CC";
		//return DB.SqlCmd.CommandText;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		int tempInt = 0;
		while (RS.Read()) {
			if (RS[0] + "" == "") {
				continue;
			}
			else if (RS[0] + "" == tempInt + "") {
				tempInt++;
				continue;
			}
			else if (tempInt == 4 || tempInt == 44 || tempInt == 444) {
				tempInt++;
				if (RS[0] + "" == tempInt + "") {
					tempInt++;
					continue;
				}
			}
			else {
				break;
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return tempInt + "";
	}

	[WebMethod]
	public String CheckCompanyCode(string CompanyCode) {
		string Left2 = CompanyCode.Substring(0, 2).ToUpper();
		string Else = CompanyCode.Substring(2);
		int Number;

		try {
			Number = Int32.Parse(Else);
		} catch (Exception) {
			switch (Left2) {
				case "QD": Number = 1600; break;
				case "SY": Number = 1800; break;
				case "WH": Number = 1800; break;
				case "SH": Number = 2000; break;
				case "YW": Number = 2300; break;
				case "WZ": Number = 2700; break;
				case "YT": Number = 6800; break;
				default: Number = 0; break;
			}
		}
		CompanyCode = Left2 + Number.ToString("###############000");
		DB = new DBConn();
		DB.DBCon.Open();

		if (CompanyCode.Length > 2) {
			DB.SqlCmd.CommandText = @"
SELECT COUNT(*)
FROM [dbo].[Company]
WHERE CompanyCode='" + CompanyCode + "';";
			string COUNT = DB.SqlCmd.ExecuteScalar() + "";
			if (COUNT == "0") {
				DB.DBCon.Close();
				return CompanyCode;
			}
		}

		DB.SqlCmd.CommandText = @"
SELECT CompanyCode
FROM Company
WHERE
	left(CompanyCode, 2)='" + Left2 + @"'
	and GubunCL in (0, 1, 5)
ORDER BY CompanyCode;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		List<int> aleadynumber = new List<int>();
		while (RS.Read()) {
			string strTmp = Regex.Replace(RS[0] + "", @"\D", "");
			int current = Int32.Parse(strTmp);
			aleadynumber.Add(current);
		}
		RS.Dispose();
		DB.DBCon.Close();
		int[] sorted = aleadynumber.ToArray();
		Array.Sort(sorted);

		bool isFind = false;
		for (var i = 0; i < sorted.Length; i++) {
			if (sorted[i] < Number) {
				continue;
			}
			else if (sorted[i] == Number) {
				Number++;
				continue;
			}
			else {
				isFind = true;
			}
		}
		return Left2 + Number.ToString("###############000");
	}

	[WebMethod]
	public String SetCompanyCustomerCodeAuto(string CompanyPk, string CompanyCode) {
		try {
			DBConn DB = new DBConn();
			DB.SqlCmd.CommandText = "UPDATE Company SET CompanyCode='" + CompanyCode + "' WHERE CompanyPk=" + CompanyPk + "; UPDATE RequestForm SET ShipperCode='" + CompanyCode + "' WHERE ShipperPk=" + CompanyPk + ";";
			DB.DBCon.Open();
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return CompanyCode;
		} catch (Exception) {
			return "0";
		}
	}       //번호 자동 업데이트 일단 shipper만

	[WebMethod]
	public String SetCompanyCustomerCodeManual(string CompanyPk, string CompanyCode)        //번호자동업데이트 일단 shipper만
	{
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT count(*) FROM Company where CompanyCode='" + CompanyCode + "'";
		DB.DBCon.Open();
		if (DB.SqlCmd.ExecuteScalar() + "" != "0") {
			DB.DBCon.Close();
			return "0";
		}
		DB.SqlCmd.CommandText = "	UPDATE Company SET CompanyCode='" + CompanyCode.ToUpper() + "' WHERE CompanyPk=" + CompanyPk + "; " +
													"	UPDATE RequestForm SET ShipperCode='" + CompanyCode.ToUpper() + "' WHERE ShipperPk=" + CompanyPk + ";" +
													"	UPDATE RequestForm SET ConsigneeCode='" + CompanyCode.ToUpper() + "' WHERE ConsigneePk=" + CompanyPk + ";";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return CompanyCode;
	}

	[WebMethod]
	public string AskCompanyCodeUsed(string CompanyCode) {
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [CompanyPk] ,[CompanyName] FROM Company WHERE [CompanyCode]='" + CompanyCode + "'";
		//return DB.SqlCmd.CommandText;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string T = "N";
		if (RS.Read()) {
			T = RS[0] + "!!" + RS[1];
		}
		RS.Dispose();
		DB.DBCon.Close();
		return T;
	}

	[WebMethod]
	public string AskCompanyCodeUsed_(string CompanyCode) {
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [CompanyPk] ,[CompanyName] FROM Company WHERE [CompanyCode]='" + CompanyCode + "'";
		//return DB.SqlCmd.CommandText;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string T = "N";
		if (RS.Read()) {
			T = RS[0] + "";
		}
		RS.Dispose();
		DB.DBCon.Close();
		string CompanyCode_Left2 = "";
		int CompanyCode_Number = 0;

		if (CompanyCode.Length > 2) {
			CompanyCode_Left2 = CompanyCode.Substring(0, 2).ToUpper();
			string str_number = CompanyCode.Substring(2);

			try {
				CompanyCode_Number = Int32.Parse(str_number);
			} catch (Exception) {
				CompanyCode_Number = 0;
			}

			int from;
			int to;
			switch (CompanyCode_Left2) {
				case "GZ":
					from = 0001;
					to = 1799;
					if (CompanyCode_Number < from || CompanyCode_Number >= to) {
						T = "Warning#!@GZ#!@" + from + "#!@" + to;
					}
					break;

				case "SY":
					from = 1800;
					to = 1999;
					if (CompanyCode_Number < from || CompanyCode_Number >= to) {
						T = "Warning#!@SY#!@" + from + "#!@" + to;
					}
					break;

				case "SH":
					from = 3000;
					to = 3499;
					if (CompanyCode_Number < from || CompanyCode_Number >= to) {
						T = "Warning#!@SH#!@" + from + "#!@" + to;
					}
					break;

				case "YW":
					from = 2000;
					to = 2599;
					if (CompanyCode_Number < from || CompanyCode_Number >= to) {
						T = "Warning#!@YW#!@" + from + "#!@" + to;
					}
					break;

				case "WZ":
					from = 2600;
					to = 2999;
					if (CompanyCode_Number < from || CompanyCode_Number >= to) {
						T = "Warning#!@WZ#!@" + from + "#!@" + to;
					}
					break;

				case "YT":
					from = 6000;
					to = 6399;
					if (CompanyCode_Number < from || CompanyCode_Number >= to) {
						T = "Warning#!@YT#!@" + from + "#!@" + to;
					}
					break;

				case "WH":
					from = 6600;
					to = 6699;
					if (CompanyCode_Number < from || CompanyCode_Number >= to) {
						T = "Warning#!@WH#!@" + from + "#!@" + to;
					}
					break;

				case "QD":
					from = 6700;
					to = 6999;
					if (CompanyCode_Number < from || CompanyCode_Number >= to) {
						T = "Warning#!@QD#!@" + from + "#!@" + to;
					}
					break;
			}
		}
		return T;
	}

	[WebMethod]
	public String SetRequestDefer(string RequestFormPk, string ID, string Comment) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [DESCRIPTION] FROM [dbo].[HISTORY] WHERE [TABLE_PK]=" + RequestFormPk + " AND [CODE]='49';";
		DB.DBCon.Open();
		TempString = DB.SqlCmd.ExecuteScalar() + "52:" + Comment + "!" + ID + "@" + DateTime.Now.GetDateTimeFormats('u')[0] + "####";
		DB.SqlCmd.CommandText = "UPDATE [dbo].[HISTORY] SET [DESCRIPTION]=N'" + TempString + "' WHERE [TABLE_PK]=" + RequestFormPk + " AND [CODE]='49';	UPDATE RequestForm SET [StepCL] = 52 WHERE RequestFormPk=" + RequestFormPk + ";";
		try {
			DB.SqlCmd.ExecuteNonQuery();
		} catch (Exception ex) {
			DB.DBCon.Close();
			return ex.Message;
		}
		DB.DBCon.Close();
		return "Y";
	}

	[WebMethod]
	public String SetRequestAccept(string RequestFormPk, string ID, string Comment, string PickupRequestDate) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [DESCRIPTION] FROM [dbo].[HISTORY] WHERE [TABLE_PK]=" + RequestFormPk + " AND [CODE]='49';";
		DB.DBCon.Open();
		TempString = DB.SqlCmd.ExecuteScalar() + "53:" + Comment + "!" + ID + "@" + DateTime.Now.GetDateTimeFormats('u')[0] + "####";
		DB.SqlCmd.CommandText = "UPDATE [dbo].[HISTORY] SET [DESCRIPTION]=N'" + TempString + "' WHERE TABLE_PK=" + RequestFormPk + " AND [CODE]='49';	UPDATE RequestForm SET [StepCL] = 53, [PickupRequestDate]='" + PickupRequestDate + "'  WHERE RequestFormPk=" + RequestFormPk + ";";
		try {
			DB.SqlCmd.ExecuteNonQuery();
		} catch (Exception ex) {
			DB.DBCon.Close();
			return ex.Message;
		}
		DB.DBCon.Close();
		return "Y";
	}

	///////////////////////RequestForm/////////////////////////////////////////////////////
	[WebMethod]
	public String InsertWarehouse(string Gubun, string CompanyPk, string Value, string ReturnValue) {
		DB = new DBConn();
		if (Gubun == "Insert") {
			DB.SqlCmd.CommandText = "INSERT INTO CompanyAdditionalInfomation ([CompanyPk], [Title], [Value]) VALUES (" + CompanyPk + ", 65, N'" + Value + "')";
		}
		else {
			DB.SqlCmd.CommandText = "UPDATE CompanyAdditionalInfomation SET [Value] = N'" + Value + "' WHERE [CompanyPk] = " + CompanyPk + " and [Title] = 65";
		}
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return ReturnValue;
	}

	[WebMethod]
	public String ItemRowDelete(string RequestFormPk, string RequestFormItemsPk, string ItemName, string ID) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "DELETE FROM RequestFormItems WHERE RequestFormItemsPk=" + RequestFormItemsPk + ";" +
													"INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID], [DESCRIPTION], [REGISTERD]) VALUES ('RequestForm', " + RequestFormPk + ", '71', '" + ID + "', '*" + ItemName + " || DELETE' ,getDate())";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	//BMK Admin/ExchangeRate.aspx
	[WebMethod]
	public String InsertExchangeRate(string datespan, string etcsettingpk, string exchangeratestandard, string monetaryunitfrom, string monetaryunitto, string exchangerate) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "INSERT INTO ExchangeRateHistory ([DateSpan], [ETCSettingPk], [ExchangeRateStandard], " +
																											  "[MonetaryUnitFrom], [MonetaryUnitTo], [ExchangeRate], [RegisteredDate]) " +
													"  VALUES (" + Common.StringToDB(datespan, true, false) + ", " +
																			Common.StringToDB(etcsettingpk, false, false) + ", " +
																			Common.StringToDB(exchangeratestandard, false, false) + ", " +
																			Common.StringToDB(monetaryunitfrom, false, false) + ", " +
																			Common.StringToDB(monetaryunitto, false, false) + ", " +
																			Common.StringToDB(exchangerate, false, false) + ", getDate());";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String ExchangeRateModify(string exchangeratepk, string datespan, string etcsettingpk, string exchangeratestandard, string monetaryunitfrom, string monetaryunitto, string exchangerate) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "	UPDATE ExchangeRateHistory " +
													"	SET [DateSpan] =" + Common.StringToDB(datespan, true, false) +
													"	,[ETCSettingPk] =" + Common.StringToDB(etcsettingpk, false, false) +
													"	,[ExchangeRateStandard] =" + Common.StringToDB(exchangeratestandard, false, false) +
													"	,[MonetaryUnitFrom] =" + Common.StringToDB(monetaryunitfrom, false, false) +
													"	,[MonetaryUnitTo] =" + Common.StringToDB(monetaryunitto, false, false) +
													"	,[ExchangeRate] =" + Common.StringToDB(exchangerate, false, false) +
													"	,[RegisteredDate] =getDate() " +
													"	WHERE ExchangeRatePk=" + exchangeratepk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String DeleteExchangeRate(string ExchangeRatePk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "DELETE ExchangeRateHistory WHERE ExchangeRatePk=" + ExchangeRatePk;
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String[] ExchangeRateSetModify(string ExchangeRatePk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"	SELECT DateSpan, ETCSettingPk, ExchangeRateStandard, MonetaryUnitFrom, MonetaryUnitTo, ExchangeRate
															FROM ExchangeRateHistory WHERE ExchangeRatePk=" + ExchangeRatePk;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		RS.Read();
		string[] ReturnValue = new string[] { RS[0] + "", RS[1] + "", RS[2] + "", RS[3] + "", RS[4] + "", RS[5] + "" };
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue;
	}

	//Admin/ExchangeRate.aspx

	//BMK Admin/Dialog/FixCharge.aspx
	//전환 후 안 쓸 메서드
	/*
	[WebMethod]
	public String[] LoadCalculated(string RequestFormPk) {
		List<string> ReturnValue = new List<string>();

		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
DECLARE @SHIPPERCOMMENT nvarchar(MAX);
DECLARE @CONSIGNEECOMMENT nvarchar(MAX);

SELECT @SHIPPERCOMMENT = [DESCRIPTION]
  FROM [dbo].[HISTORY]
 WHERE [TABLE_PK]=" + RequestFormPk + @" and [CODE]=5
SELECT @CONSIGNEECOMMENT = [DESCRIPTION]
  FROM [dbo].[HISTORY]
 WHERE [TABLE_PK]=" + RequestFormPk + @" and [CODE]=6

SELECT StandardPrice.StandardPriceListPk, [StandardPriceA], [CriterionValue], [ExchangeRate], [ShipperMonetaryUnit]
		, [ShipperCharge], [ConsigneeMonetaryUnit], [ConsigneeCharge], [ShipperBankAccountPk], [ConsigneeBankAccountPk]
		, [WillPayTariff], @SHIPPERCOMMENT  , @CONSIGNEECOMMENT
FROM RequestFormCalculateHead
	left join StandardPrice ON RequestFormCalculateHead.StandardPriceHeadPk=StandardPrice.StandardPriceListPk
WHERE RequestFormPk=" + RequestFormPk + ";";

		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue.Add(RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5] + "#@!" + RS[6] + "#@!" + RS[7] + "#@!" + RS[8] + "#@!" + RS[9] + "#@!" + RS[10] + "#@!" + RS[11] + "#@!" + RS[12]);
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = "SELECT [RequestFormCalculateBodyPk], [GubunCL], [Title], [Price], [MonetaryUnit], [StandardPriceHeadPkNColumn] FROM RequestFormCalculateBody WHERE RequestFormPk=" + RequestFormPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Add(RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5]);
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}
	*/

	[WebMethod]
	public String LoadStandardPrice(string StandardPricePk) {
		DB = new DBConn();
		string[] StandardPrice;
		string[] ColumnName = new String[] { "Length", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"	SELECT top 1 SP.Guideline, SP.FreightCharge, SP.DepartureCharge, SP.ArrivalCharge, SPB.A
															FROM StandardPrice as SP
																left join StandardPriceBody AS SPB ON SP.FreightCharge=SPB.StandardPriceHeadPk
															WHERE SP.StandardPriceListPk=" + StandardPricePk +
														"	ORDER BY SPB.A DESC;";

		DB.DBCon.Open();
		//return DB.SqlCmd.CommandText;
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue.Append(RS["Guideline"] + "!" + RS["A"]);
			StandardPrice = new string[3] { RS["FreightCharge"] + "", RS["DepartureCharge"] + "", RS["ArrivalCharge"] + "" };
		}
		else {
			RS.Dispose();
			DB.DBCon.Close();
			return "1";
		}
		RS.Dispose();

		for (int j = 0; j < StandardPrice.Length; j++) {
			if (StandardPrice[j] == "") {
				continue;
			}
			ReturnValue.Append("####");
			DB.SqlCmd.CommandText = "SELECT [Length], [A], [B], [C], [D], [E], [F], [G], [H], [I], [J] FROM StandardPriceHead WHERE [StandardPriceHeadPk]=" + StandardPrice[j];
			string Temp = StandardPrice[j];
			RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				ReturnValue.Append(RS[1]);
				int Length = Int32.Parse(RS[0] + "") + 1;
				for (int i = 2; i < Length; i++) {
					//ReturnValue.Append("@@" + RS[i]);
					ReturnValue.Append("@@" + RS[i] + "*" + Temp + ColumnName[i]);
				}
			}
			RS.Dispose();
		}
		//여기까지가 단가표 타이틀
		DB.DBCon.Close();
		return ReturnValue + "";
	}

	[WebMethod]
	public String[] LoadOurStaffSBankAccount(string RequestFormPk, string SorC) {
		DB = new DBConn();
		List<string> returnValue = new List<string>();

		string temp = SorC == "S" ? "DepartureRegionCode" : "ArrivalRegionCode";

		DB.SqlCmd.CommandText = @"	SELECT CB.CompanyBankPk, CB.BankName, CB.OwnerName, CB.AccountNo, CB.BankMemo
															FROM RequestForm AS R
																left join RegionCode AS RC ON RC.RegionCode=R." + temp + @"
																left join (select * from CompanyBank where IsDel=0) AS CB ON RC.OurBranchCode=GubunPk
															WHERE RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			returnValue.Add(RS[0] + "!" + RS[1] + "!" + RS[2] + "!" + RS[3] + "!" + RS[4]);
		}
		RS.Dispose();
		DB.DBCon.Close();
		return returnValue.ToArray();
	}

	[WebMethod]
	public String LoadOurStaffSBankAccountForFixCharge(string RequestFormPk) {
		DB = new DBConn();
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"
SELECT '10', CB.CompanyBankPk, CB.BankName, CB.OwnerName, CB.AccountNo, CB.BankMemo
FROM RequestForm AS R
	left join RegionCode AS RC ON RC.RegionCode=R.DepartureRegionCode
	left join (select * from CompanyBank where IsDel=0) AS CB ON RC.OurBranchCode=GubunPk
WHERE RequestFormPk=" + RequestFormPk + @"

union
SELECT '11', CB.CompanyBankPk, CB.BankName, CB.OwnerName, CB.AccountNo, CB.BankMemo
FROM RequestForm AS R
	left join RegionCode AS RC ON RC.RegionCode=R.ArrivalRegionCode
	left join (select * from CompanyBank where IsDel=0) AS CB ON RC.OurBranchCode=GubunPk
WHERE RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue.Append(RS[0] + "!" + RS[1] + "!" + RS[2] + "!" + RS[3] + "!" + RS[4] + "!" + RS[5]);
			while (RS.Read()) {
				ReturnValue.Append("##" + RS[0] + "!" + RS[1] + "!" + RS[2] + "!" + RS[3] + "!" + RS[4] + "!" + RS[5]);
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue + "";
	}

	[WebMethod]
	public String[] LoadStandardPriceEachValue(string StandardPricePk, string totalvalue, string overweightvalue) {
		DB = new DBConn();
		string[] StandardPrice;
		decimal GuideLine;
		decimal TempDecimal;
		decimal TotalValue = decimal.Parse(totalvalue);
		decimal OverWeightValue = decimal.Parse(overweightvalue);

		string[] ColumnName = new String[] { "Length", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
		DB.SqlCmd.CommandText = "SELECT [Guideline], [FreightCharge], [DepartureCharge], [ArrivalCharge] FROM StandardPrice WHERE [StandardPriceListPk]=" + StandardPricePk;
		//return DB.SqlCmd.CommandText;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			GuideLine = Decimal.Parse(RS["Guideline"] + "");
			StandardPrice = new string[3] { RS[1] + "", RS[2] + "", RS[3] + "" };
		}
		else {
			RS.Dispose();
			DB.DBCon.Close();
			return new string[] { "0" };
		}
		RS.Dispose();

		string SelectedValue = "";
		List<string> Result = new List<string>();
		List<string> StandardPricePkNColumn = new List<string>();
		for (int j = 0; j < StandardPrice.Length; j++) {
			if (StandardPrice[j] != "") {
				DB.SqlCmd.CommandText = "SELECT Top 10 [StandardPriceBodyPk], [A], [B], [C], [D], [E], [F], [G], [H], [I], [J] FROM [StandardPriceBody] WHERE [StandardPriceHeadPk]=" + StandardPrice[j] + " and [A]>=" + totalvalue;
				RS = DB.SqlCmd.ExecuteReader();
				while (RS.Read()) {
					TempDecimal = Decimal.Parse(RS["A"] + "");
					if (TempDecimal % GuideLine == 0) {
						SelectedValue += RS[1] + "!";
						for (int i = 2; i < RS.FieldCount; i++) {
							if (RS[i] + "" == "") {
								continue;
							}
							Result.Add(StandardPrice[j] + ColumnName[i] + "*" + Common.NumberFormat(RS[i] + "") + "*");
							StandardPricePkNColumn.Add(StandardPrice[j] + ColumnName[i]);
						}
						break;
					}
				}
				RS.Dispose();
			}
		}
		if (overweightvalue != totalvalue) {
			SelectedValue += "@@";
			for (int j = 0; j < StandardPrice.Length; j++) {
				if (StandardPrice[j] != "") {
					DB.SqlCmd.CommandText = "SELECT Top 10 [StandardPriceBodyPk], [A], [B], [C], [D], [E], [F], [G], [H], [I], [J] FROM [StandardPriceBody] WHERE [StandardPriceHeadPk]=" + StandardPrice[j] + " and [A]>=" + overweightvalue;
					RS = DB.SqlCmd.ExecuteReader();
					while (RS.Read()) {
						TempDecimal = Decimal.Parse(RS["A"] + "");
						if (TempDecimal % GuideLine == 0) {
							SelectedValue += RS[1] + "!";
							for (int i = 2; i < RS.FieldCount; i++) {
								if (RS[i] + "" == "") {
									break;
								}
								for (int k = 0; k < StandardPricePkNColumn.Count; k++) {
									if (StandardPricePkNColumn[k] == StandardPrice[j] + ColumnName[i]) {
										Result[k] += Common.NumberFormat(RS[i] + "");
										break;
									}
								}
							}
							break;
						}
					}
					RS.Dispose();
				}
			}
		}
		DB.DBCon.Close();
		return Result.ToArray();
	}

	[WebMethod]
	public String GetExchangeRate(string WhichMonetaryUnitNeed) {
		string[] EachRow = WhichMonetaryUnitNeed.Split(Common.Splite22, StringSplitOptions.RemoveEmptyEntries);
		StringBuilder ReturnValue = new StringBuilder();
		DB = new DBConn();
		DB.DBCon.Open();
		for (int i = 0; i < EachRow.Length; i++) {
			DB.SqlCmd.CommandText = "SELECT TOP 1 [DateSpan], [ExchangeRateStandard], [ExchangeRate], [RegisteredDate] FROM ExchangeRateHistory WHERE ETCSettingPk=1 and MonetaryUnitFrom=" + EachRow[i].Substring(0, 2) + " and [MonetaryUnitTo]=" + EachRow[i].Substring(3, 2) + "  ORDER BY DateSpan DESC";
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				ReturnValue.Append(EachRow[i] + "!" + RS[0] + "!" + RS[1] + "!" + RS[2] + "@@");
			}
			RS.Dispose();
		}
		DB.DBCon.Close();
		//return "1";
		//return WhichMonetaryUnitNeed;
		return ReturnValue + "";
	}

	[WebMethod]
	public String[] LoadReginoCodeCountry() {
		DB = new DBConn();
		List<string> ReturnValue = new List<string>();
		DB.SqlCmd.CommandText = "SELECT [RegionCode], [RegionCodePk], [NameE] FROM RegionCode WHERE len(RegionCode)=1 order by OrderBy;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Add(RS[0] + "$" + RS[2]);
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}

	[WebMethod]
	public String[] LoadReginoCodeCountry_ReturnWithPk() {
		DB = new DBConn();
		List<string> ReturnValue = new List<string>();
		DB.SqlCmd.CommandText = "SELECT [RegionCode], [RegionCodePk], [NameE] FROM RegionCode WHERE len(RegionCode)=1 order by OrderBy;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Add(RS[0] + "|" + RS[1] + "$" + RS[2]);
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}

	[WebMethod]
	public String[] LoadRegionCode(string SelectedRegionCode) {
		DB = new DBConn();
		List<string> ReturnValue = new List<string>();
		DB.SqlCmd.CommandText = "SELECT TOP 1 len(RegionCode) FROM RegionCode WHERE left(RegionCode, " + (SelectedRegionCode.Length + 1) + ")='" + SelectedRegionCode + "!' order by len(RegionCode) DESC;";
		DB.DBCon.Open();
		ReturnValue.Add(DB.SqlCmd.ExecuteScalar() + "");

		DB.SqlCmd.CommandText = "	SELECT [RegionCode], [Name], [NameE] " +
													"	FROM RegionCode " +
													"	WHERE left(RegionCode, " + (SelectedRegionCode.Length + 1) + ")='" + SelectedRegionCode + "!' and len(RegionCode)=" + (SelectedRegionCode.Length + 3) +
													"	ORDER BY OrderBy;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Add(RS[0] + "$" + RS[1] + " || " + RS[2]);
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}

	[WebMethod]
	public String[] LoadRegionCode_ReturnWithPk(string SelectedRegionCode) {
		DB = new DBConn();
		List<string> ReturnValue = new List<string>();
		DB.SqlCmd.CommandText = "SELECT TOP 1 len(RegionCode) FROM RegionCode WHERE left(RegionCode, " + (SelectedRegionCode.Length + 1) + ")='" + SelectedRegionCode + "!' order by len(RegionCode) DESC;";
		DB.DBCon.Open();
		ReturnValue.Add(DB.SqlCmd.ExecuteScalar() + "");

		DB.SqlCmd.CommandText = "	SELECT [RegionCode], [RegionCodePk], [Name], [NameE] " +
													"	FROM RegionCode " +
													"	WHERE left(RegionCode, " + (SelectedRegionCode.Length + 1) + ")='" + SelectedRegionCode + "!' and len(RegionCode)=" + (SelectedRegionCode.Length + 3) +
													"	ORDER BY OrderBy;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Add(RS[0] + "|" + RS[1] + "$" + RS[2] + " || " + RS[3]);
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}

	[WebMethod]
	public String LoadSuperRegionCode(string SelectedRegionCode) {
		DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT TOP 1 CONCAT([RegionCode], '|', [RegionCodePk]) FROM [dbo].[RegionCode] WHERE [RegionCode] = '" + SelectedRegionCode + @"';";
		string ReturnValue = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		return ReturnValue;
	}


	/// <summary>
	///
	/// </summary>
	/// <param name="ShipperMonetaryUnit"></param>
	/// <param name="ShipperSUM"></param>
	/// <param name="ConsigneeMonetaryUnit"></param>
	/// <param name="ConsigneeSUM"></param>
	/// <returns>0번째 배열이 -1이면 쉬퍼계산 안됌 -2면 수하인 계산 안됌 1이면 정상</returns>
	[WebMethod]
	public String[] GetTotalPrice(string ShipperMonetaryUnit, string ShipperSUM, string ConsigneeMonetaryUnit, string ConsigneeSUM, string ExchangeSetDate, string IsEstimation) {
		//Smonetary 18
		//ShipperSUM	19!165@@
		try {
			Decimal Shipper = 0;
			Decimal Consignee = 0;
			StringBuilder ExchageRate = new StringBuilder();
			bool flag = true;
			if (ShipperSUM != "") {
				string[] EachRow = ShipperSUM.Split(Common.Splite22, StringSplitOptions.RemoveEmptyEntries);
				foreach (string Each in EachRow) {
					if (Each.Substring(0, 2) == ShipperMonetaryUnit) {
						ExchageRate.Append("@@");
						Shipper += Decimal.Parse(Each.Substring(3));
					}
					else {
						string exchageRatePk;
						if (IsEstimation == "Y") {
							Shipper += Math.Round(GetExchangeRatedRecent(Each.Substring(0, 2), ShipperMonetaryUnit, Decimal.Parse(Each.Substring(3)), out exchageRatePk, ExchangeSetDate));
						}
						else {
							Shipper += Math.Round(GetExchangeRated(Each.Substring(0, 2), ShipperMonetaryUnit, Decimal.Parse(Each.Substring(3)), out exchageRatePk, ExchangeSetDate));
						}

						if (exchageRatePk == "") {
							flag = false;
							break;
						}
						else {
							ExchageRate.Append(exchageRatePk);
						}
					}
				}
				if (!flag) {
					return new string[] { "-1" };
				}
				else {
					switch (ShipperMonetaryUnit) {
						case "18":
							Shipper = Math.Round(Shipper, 0, MidpointRounding.AwayFromZero);
							break;

						case "19":
							Shipper = Math.Round(Shipper, 2, MidpointRounding.AwayFromZero);
							break;

						case "20":
							Shipper = Math.Round(Shipper, 0, MidpointRounding.AwayFromZero);
							break;
					}
				}
			}

			if (ConsigneeSUM != "") {
				string[] EachRow = ConsigneeSUM.Split(Common.Splite22, StringSplitOptions.RemoveEmptyEntries);
				foreach (string Each in EachRow) {
					if (Each.Substring(0, 2) == ConsigneeMonetaryUnit) {
						Consignee += Decimal.Parse(Each.Substring(3));
					}
					else {
						string exchageRatePk;
						//						return new string[] { GetExchangeRate(Each.Substring(0, 2), ConsigneeMonetaryUnit, Decimal.Parse(Each.Substring(3)), out exchageRatePk) + "" };
						if (IsEstimation == "Y") {
							Consignee += Math.Round(GetExchangeRatedRecent(Each.Substring(0, 2), ConsigneeMonetaryUnit, Decimal.Parse(Each.Substring(3)), out exchageRatePk, ExchangeSetDate));
						}
						else {
							Consignee += Math.Round(GetExchangeRated(Each.Substring(0, 2), ConsigneeMonetaryUnit, Decimal.Parse(Each.Substring(3)), out exchageRatePk, ExchangeSetDate));
						}

						if (exchageRatePk == "") {
							flag = false;
							break;
						}
						else {
							ExchageRate.Append(exchageRatePk);
						}
					}
				}
				if (!flag) {
					return new string[] { "-2" };
				}
				else {
					switch (ConsigneeMonetaryUnit) {
						case "18":
							Consignee = Math.Round(Consignee, 0, MidpointRounding.AwayFromZero);
							break;

						case "19":
							Consignee = Math.Round(Consignee, 2, MidpointRounding.AwayFromZero);
							break;

						case "20":
							Consignee = Math.Round(Consignee, 0, MidpointRounding.AwayFromZero);
							break;
					}
				}
			}
			return new string[] { ExchageRate + "", Common.NumberFormat(Shipper + ""), Consignee + "" };
		} catch (Exception ex) {
			return new string[] { ex.Message };
			throw;
		}
	}

	public Decimal GetExchangeRatedRecent(string From, string To, decimal Value, out string ExchageRate, string SetDateTime) {
		Decimal result;
		//string date = SetDateTime == "" ? String.Empty : " and DateSpan<='" + SetDateTime + "'";
		string QWhereDate = "";
		if (SetDateTime != "") {
			DateTime A = new DateTime(Int32.Parse(SetDateTime.Substring(0, 4)), Int32.Parse(SetDateTime.Substring(4, 2)), Int32.Parse(SetDateTime.Substring(6, 2)));
			string Week = A.DayOfWeek.ToString();
			if (Week == "Sunday") {
				QWhereDate = " and DateSpan in ('" + SetDateTime + "', '" + A.AddDays(-1).ToString("yyyyMMdd") + "' , '" + A.AddDays(-2).ToString("yyyyMMdd") + "')";
			}
			else if (Week == "Saturday") {
				QWhereDate = " and DateSpan in ('" + SetDateTime + "', '" + A.AddDays(-1).ToString("yyyyMMdd") + "')";
			}
			else {
				QWhereDate = " and DateSpan<='" + SetDateTime + "'";
			}
		}

		DB = new DBConn();
		DB.SqlCmd.CommandText = "	SELECT TOP 1 ExchangeRatePk, ExchangeRateStandard, ExchangeRate, MonetaryUnitFrom, MonetaryUnitTo, DateSpan " +
													"	FROM ExchangeRateHistory " +
													"	WHERE ETCSettingPk=1 and MonetaryUnitFrom =" + From + " and [MonetaryUnitTo] =" + To + " " + QWhereDate +
													"	ORDER BY DateSpan DESC";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ExchageRate = RS["MonetaryUnitFrom"] + "!" + RS["MonetaryUnitTo"] + "!" + RS["DateSpan"] + "!" + RS["ExchangeRate"] + "@@";
			result = Value * Decimal.Parse(RS[2] + "") / decimal.Parse(RS[1] + "");
		}
		else {
			result = -1;
			ExchageRate = "";
		}
		RS.Dispose();
		DB.DBCon.Close();
		return result;
	}

	public Decimal GetExchangeRated(string From, string To, decimal Value, out string ExchageRate, string SetDateTime) {
		Decimal result;
		//string date = SetDateTime == "" ? String.Empty : " and DateSpan<='" + SetDateTime + "'";
		string QWhereDate = "";
		if (SetDateTime != "") {
			DateTime A = new DateTime(Int32.Parse(SetDateTime.Substring(0, 4)), Int32.Parse(SetDateTime.Substring(4, 2)), Int32.Parse(SetDateTime.Substring(6, 2)));
			string Week = A.DayOfWeek.ToString();
			if (Week == "Sunday") {
				QWhereDate = " and DateSpan in ('" + SetDateTime + "', '" + A.AddDays(-1).ToString("yyyyMMdd") + "' , '" + A.AddDays(-2).ToString("yyyyMMdd") + "')";
			}
			else if (Week == "Saturday") {
				QWhereDate = " and DateSpan in ('" + SetDateTime + "', '" + A.AddDays(-1).ToString("yyyyMMdd") + "')";
			}
			else {
				QWhereDate = " and DateSpan='" + SetDateTime + "'";
			}
		}

		DB = new DBConn();
		DB.SqlCmd.CommandText = "	SELECT TOP 1 ExchangeRatePk, ExchangeRateStandard, ExchangeRate, MonetaryUnitFrom, MonetaryUnitTo, DateSpan " +
													"	FROM ExchangeRateHistory " +
													"	WHERE ETCSettingPk=1 and MonetaryUnitFrom =" + From + " and [MonetaryUnitTo] =" + To + " " + QWhereDate +
													"	ORDER BY DateSpan DESC";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ExchageRate = RS["MonetaryUnitFrom"] + "!" + RS["MonetaryUnitTo"] + "!" + RS["DateSpan"] + "!" + RS["ExchangeRate"] + "@@";
			result = Value * Decimal.Parse(RS[2] + "") / decimal.Parse(RS[1] + "");
		}
		else {
			result = -1;
			ExchageRate = "";
		}
		RS.Dispose();
		DB.DBCon.Close();
		return result;
	}

	/* 전환 후 안쓸 메서드 */
	/*
	[WebMethod]
	public String SetFixCharge(string PaymentWhoCL, string RequestFormPk, string StandardPriceListPk, string StandardPriceA, string CriterionValue, string ExchangeRate, string ShipperMonetaryUnit, string ShipperCharge, string ConsigneeMonetaryUnit, string ConsigneeCharge, string CalculateBodySum, string AccountID, string ShipperBankAccount, string ConsigneeBankAccount, string WhoWillPaidTariff, string CommentShipper, string CommentConsignee, string ShipperContentsSum, string ConsigneeContentsSum) {
		StringBuilder dbcommand = new StringBuilder();
		GetQuery GQ = new GetQuery();
		string[] calculatebodyeach = CalculateBodySum.Split(Common.Splite22, StringSplitOptions.RemoveEmptyEntries);
		foreach (string Row in calculatebodyeach) {
			string[] Each = Row.Split(Common.Splite11, StringSplitOptions.None);
			dbcommand.Append("		INSERT INTO RequestFormCalculateBody " +
											"								([RequestFormPk], [GubunCL], [Title], [Price], [MonetaryUnit], [StandardPriceHeadPkNColumn]) " +
											"							VALUES " +
											"								(" + RequestFormPk + ", " + Common.StringToDB(Each[0], false, false) + ", " + Common.StringToDB(Each[1], true, true) + ", " +
																					Common.StringToDB(Each[2], false, false) + ", " + Common.StringToDB(Each[3], false, false) + ", " + Common.StringToDB(Each[4], true, false) + ");");
		}
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [StepCL] FROM [dbo].[RequestForm] WHERE RequestFormPk=" + RequestFormPk + ";";

		DB.DBCon.Open();
		string IsEstimation = DB.SqlCmd.ExecuteScalar() + "";
		if (IsEstimation == "33") {
			DB.SqlCmd.CommandText = @"
				DECLARE @PackedCount int;
				DECLARE @GrossWeight smallmoney;
				DECLARE @Volume smallmoney;

				SELECT @PackedCount=SUM([PackedCount]), @GrossWeight=SUM([GrossWeight]), @Volume=SUM([Volume])
				FROM [dbo].[RequestFormItems]
				WHERE RequestFormPk=" + RequestFormPk + @"
				GROUP BY RequestFormPk;

				INSERT INTO [dbo].[RequestFormCalculateHead] (
					[RequestFormPk],[TotalPackedCount],[PackingUnit],[TotalGrossWeight],[TotalVolume]
					,[StandardPriceHeadPk],[StandardPriceA],[CriterionValue],[ExchangeRate],[ShipperMonetaryUnit]
					,[ShipperCharge],[ConsigneeMonetaryUnit],[ConsigneeCharge],[WillPayTariff])
				VALUES
					(" + Common.StringToDB(RequestFormPk, false, false) +
					  @", @PackedCount, 15, @GrossWeight, @Volume, " + Common.StringToDB(StandardPriceListPk, false, false) +
					", " + Common.StringToDB(StandardPriceA, false, false) +
					", " + Common.StringToDB(CriterionValue, true, false) +
					", " + Common.StringToDB(ExchangeRate, true, true) +
					", " + Common.StringToDB(ShipperMonetaryUnit, false, false) +
					", " + Common.StringToDB(ShipperCharge, false, false) +
					", " + Common.StringToDB(ConsigneeMonetaryUnit, false, false) +
					", " + Common.StringToDB(ConsigneeCharge, false, false) +
					", " + Common.StringToDB(WhoWillPaidTariff, true, false) + ");" +
					dbcommand +
					GQ.AddRequestHistory(RequestFormPk, "58", AccountID, "") +
					"UPDATE RequestForm SET [PaymentWhoCL] = " + PaymentWhoCL + " WHERE RequestFormPk=" + RequestFormPk;
		}
		else {
			DB.SqlCmd.CommandText = "SP_UpdateCalculateHeadForSetFixCharge  @RequestFormPk=" + Common.StringToDB(RequestFormPk, false, false) +
														"																	, @StandardPriceListPk=" + Common.StringToDB(StandardPriceListPk, false, false) +
														"																	, @StandardPriceA=" + Common.StringToDB(StandardPriceA, false, false) +
														"																	, @CriterionValue=" + Common.StringToDB(CriterionValue, true, false) +
														"																	, @ExchangeRate=" + Common.StringToDB(ExchangeRate, true, true) +
														"																	, @ShipperMonetaryUnit=" + Common.StringToDB(ShipperMonetaryUnit, false, false) +
														"																	, @ShipperCharge=" + Common.StringToDB(ShipperCharge, false, false) +
														"																	, @ConsigneeMonetaryUnit=" + Common.StringToDB(ConsigneeMonetaryUnit, false, false) +
														"																	, @ConsigneeCharge=" + Common.StringToDB(ConsigneeCharge, false, false) +
														"																	, @ShipperBankAccountPk=" + Common.StringToDB(ShipperBankAccount, false, false) +
														"																	, @ConsigneeBankAccountPk=" + Common.StringToDB(ConsigneeBankAccount, false, false) +
														"																	, @WillPayTariff=" + Common.StringToDB(WhoWillPaidTariff, true, false) + ";" +
														dbcommand +
														GQ.AddRequestHistory(RequestFormPk, "58", AccountID, "") +
														 "INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [DESCRIPTION], [ACCOUNT_ID]) VALUES('RequestForm', " + RequestFormPk + ", '5', " + CommentShipper + ", " + AccountID + ")" +
														"INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [DESCRIPTION], [ACCOUNT_ID]) VALUES('RequestForm', " + RequestFormPk + ", '6', " + CommentConsignee + ", " + AccountID + ")" +
														"UPDATE RequestForm SET [PaymentWhoCL] = " + PaymentWhoCL + ", [StepCL] = 58 WHERE RequestFormPk=" + RequestFormPk;
														
		}
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		if (ShipperContentsSum != "") {
			string[] EachShipperContents = ShipperContentsSum.Split(Common.Splite51423, StringSplitOptions.None);
			SetTalkBusiness(EachShipperContents[0], EachShipperContents[1], EachShipperContents[2], EachShipperContents[3]);
		}
		if (ConsigneeContentsSum != "") {
			string[] EachConsigneeContents = ConsigneeContentsSum.Split(Common.Splite51423, StringSplitOptions.None);
			SetTalkBusiness(EachConsigneeContents[0], EachConsigneeContents[1], EachConsigneeContents[2], EachConsigneeContents[3]);
		}
		return "1";
	}
	*/
	/*
        [WebMethod]
    public String ModifyFixCharge(string PaymentWhoCL, string RequestFormPk, string StandardPriceListPk, string StandardPriceA, string CriterionValue, string ExchangeRate, string ShipperMonetaryUnit, string ShipperCharge, string ConsigneeMonetaryUnit, string ConsigneeCharge, string CalculateBodySum, string AccountID, string ShipperBankAccount, string ConsigneeBankAccount, string WhoWillPaidTariff, string CommentShipper, string CommentConsignee, string ShipperContentsSum, string ConsigneeContentsSum) {
        StringBuilder dbcommand = new StringBuilder();
        GetQuery GQ = new GetQuery();
        string[] calculatebodyeach = CalculateBodySum.Split(Common.Splite22, StringSplitOptions.RemoveEmptyEntries);
        dbcommand.Append("		DELETE FROM RequestFormCalculateBody WHERE RequestFormPk=" + RequestFormPk + "; " +
                                                            "		UPDATE RequestForm SET [ShipperSignID] = NULL, [ShipperSignDate] = NULL, [ConsigneeSignID] = NULL, [ConsigneeSignDate] = NULL WHERE RequestFormPk=" + RequestFormPk + ";");
        foreach (string Row in calculatebodyeach) {
            string[] Each = Row.Split(Common.Splite11, StringSplitOptions.None);
            dbcommand.Append("	INSERT INTO RequestFormCalculateBody " +
                                                            "			([RequestFormPk], [GubunCL], [Title], [Price], [MonetaryUnit], [StandardPriceHeadPkNColumn]) " +
                                                            "		VALUES " +
                                                            "			(" + RequestFormPk + ", " + Common.StringToDB(Each[0], false, false) + ", " + Common.StringToDB(Each[1], true, true) + ", " +
                                                                            Common.StringToDB(Each[2], false, false) + ", " + Common.StringToDB(Each[3], false, false) + ", " + Common.StringToDB(Each[4], true, false) + ");");
        }
        DB = new DBConn();
        DB.SqlCmd.CommandText = "	SP_UpdateCalculateHeadForSetFixCharge  @RequestFormPk=" + Common.StringToDB(RequestFormPk, false, false) +
                                                    "								, @StandardPriceListPk=" + Common.StringToDB(StandardPriceListPk, false, false) +
                                                    "								, @StandardPriceA=" + Common.StringToDB(StandardPriceA, false, false) +
                                                    "								, @CriterionValue=" + Common.StringToDB(CriterionValue, true, false) +
                                                    "								, @ExchangeRate=" + Common.StringToDB(ExchangeRate, true, true) +
                                                    "								, @ShipperMonetaryUnit=" + Common.StringToDB(ShipperMonetaryUnit, false, false) +
                                                    "								, @ShipperCharge=" + Common.StringToDB(ShipperCharge, false, false) +
                                                    "								, @ConsigneeMonetaryUnit=" + Common.StringToDB(ConsigneeMonetaryUnit, false, false) +
                                                    "								, @ConsigneeCharge=" + Common.StringToDB(ConsigneeCharge, false, false) +
                                                    "								, @ShipperBankAccountPk=" + Common.StringToDB(ShipperBankAccount, false, false) +
                                                    "								, @ConsigneeBankAccountPk=" + Common.StringToDB(ConsigneeBankAccount, false, false) +
                                                    "								, @WillPayTariff=" + Common.StringToDB(WhoWillPaidTariff, true, false) + ";" +
                                                    dbcommand +
                                                    "DELETE FROM RequestFormAdditionalInfo WHERE RequestFormPk=" + RequestFormPk + " and GubunCL in (5, 6);" +
                                                    GQ.ExecSP_InsertRequestFormAdditionalInfo(RequestFormPk, "5", AccountID, CommentShipper) +
                                                    GQ.ExecSP_InsertRequestFormAdditionalInfo(RequestFormPk, "6", AccountID, CommentConsignee) +
                                                    GQ.AddRequestHistory(RequestFormPk, "59", AccountID, "") +
                                                    "UPDATE RequestForm SET [PaymentWhoCL] = " + PaymentWhoCL + " WHERE RequestFormPk=" + RequestFormPk;
        //return DB.SqlCmd.CommandText;
        DB.DBCon.Open();
        DB.SqlCmd.ExecuteNonQuery();
        DB.DBCon.Close();
        if (ShipperContentsSum != "") {
            string[] EachShipperContents = ShipperContentsSum.Split(Common.Splite51423, StringSplitOptions.None);
            SetTalkBusiness(EachShipperContents[0], EachShipperContents[1], EachShipperContents[2], EachShipperContents[3]);
        }
        if (ConsigneeContentsSum != "") {
            string[] EachConsigneeContents = ConsigneeContentsSum.Split(Common.Splite51423, StringSplitOptions.None);
            SetTalkBusiness(EachConsigneeContents[0], EachConsigneeContents[1], EachConsigneeContents[2], EachConsigneeContents[3]);
        }
        return "1";
    }

     */

	[WebMethod]
	public String ModifyFixCharge(string PaymentWhoCL, string RequestFormPk, string StandardPriceListPk, string StandardPriceA, string CriterionValue, string ExchangeRate, string ShipperMonetaryUnit, string ShipperCharge, string ConsigneeMonetaryUnit, string ConsigneeCharge, string CalculateBodySum, string AccountID, string ShipperBankAccount, string ConsigneeBankAccount, string WhoWillPaidTariff, string CommentShipper, string CommentConsignee, string ShipperContentsSum, string ConsigneeContentsSum) {
		StringBuilder dbcommand = new StringBuilder();
		GetQuery GQ = new GetQuery();
		string[] calculatebodyeach = CalculateBodySum.Split(Common.Splite22, StringSplitOptions.RemoveEmptyEntries);
		List<string> NewCalculateBody = new List<string>();
		foreach (string row in calculatebodyeach) {
			NewCalculateBody.Add(row);
		}

		dbcommand.Append("		UPDATE RequestForm SET [ShipperSignID] = NULL, [ShipperSignDate] = NULL, [ConsigneeSignID] = NULL, [ConsigneeSignDate] = NULL WHERE RequestFormPk=" + RequestFormPk + ";");
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT
	[RequestFormCalculateBodyPk], [GubunCL], [Title], [Price], [MonetaryUnit], [StandardPriceHeadPkNColumn]
FROM
	[dbo].[RequestFormCalculateBody]
WHERE
	RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder ModifyHistoryValue = new StringBuilder();
		while (RS.Read()) {
			bool IsIn = false;
			for (var i = 0; i < NewCalculateBody.Count; i++) {
				string[] Each = NewCalculateBody[i].Split(Common.Splite11, StringSplitOptions.None);
				if (Each[0] == RS["GubunCL"] + "" && Each[1] == RS["Title"] + "") {
					IsIn = true;

					if (Common.NumberFormat(Each[2]) != Common.NumberFormat(RS["Price"] + "") || Each[3] != RS["MonetaryUnit"] + "") {
						dbcommand.Append("	UPDATE [dbo].[RequestFormCalculateBody] SET " +
																		"			[Price] = " + Common.StringToDB(Each[2], false, false) +
																		"			, [MonetaryUnit]=" + Common.StringToDB(Each[3], false, false) +
																		"		 WHERE [RequestFormCalculateBodyPk]=" + RS["RequestFormCalculateBodyPk"] + ";");
						ModifyHistoryValue.Append("*" + Each[1] + " : " +
							Common.GetMonetaryUnit(RS["MonetaryUnit"] + "") + " " + Common.NumberFormat(RS["Price"] + "") + " -> " +
							Common.GetMonetaryUnit(Each[3]) + " " + Common.NumberFormat(Each[2]));
					}
					NewCalculateBody.RemoveAt(i);
					break;
				}
			}
			if (!IsIn) {
				dbcommand.Append("	DELETE FROM [dbo].[RequestFormCalculateBody] WHERE [RequestFormCalculateBodyPk]=" + RS["RequestFormCalculateBodyPk"] + ";");
				ModifyHistoryValue.Append("*" + RS["Title"] + " : DELETE " +
					Common.GetMonetaryUnit(RS["MonetaryUnit"] + "") + " " + Common.NumberFormat(RS["Price"] + ""));
			}
		}
		RS.Dispose();

		for (var i = 0; i < NewCalculateBody.Count; i++) {
			string[] Each = NewCalculateBody[i].Split(Common.Splite11, StringSplitOptions.None);
			dbcommand.Append("	INSERT INTO RequestFormCalculateBody " +
															"			([RequestFormPk], [GubunCL], [Title], [Price], [MonetaryUnit], [StandardPriceHeadPkNColumn]) " +
															"		VALUES " +
															"			(" + RequestFormPk + ", " + Common.StringToDB(Each[0], false, false) + ", " + Common.StringToDB(Each[1], true, true) + ", " +
																			Common.StringToDB(Each[2], false, false) + ", " + Common.StringToDB(Each[3], false, false) + ", " + Common.StringToDB(Each[4], true, false) + ");");
			ModifyHistoryValue.Append("*" + Each[1] + " : New " +
				Common.GetMonetaryUnit(Each[3]) + " " + Common.NumberFormat(Each[2]));
		}

		DB.SqlCmd.CommandText = @"
SELECT [ShipperMonetaryUnit]
      ,[ShipperCharge]
      ,[ConsigneeMonetaryUnit]
      ,[ConsigneeCharge]
  FROM [dbo].[RequestFormCalculateHead]
  WHERE RequestFormPk=" + RequestFormPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			if (RS["ShipperMonetaryUnit"] + "" != ShipperMonetaryUnit || Common.NumberFormat(ShipperCharge) != Common.NumberFormat(RS["ShipperCharge"] + "")) {
				ModifyHistoryValue.Append("*Shipper Charge : " +
							Common.GetMonetaryUnit(RS["ShipperMonetaryUnit"] + "") + " " + Common.NumberFormat(RS["ShipperCharge"] + "") + " -> " +
							Common.GetMonetaryUnit(ShipperMonetaryUnit) + " " + Common.NumberFormat(ShipperCharge));
			}
			if (RS["ConsigneeMonetaryUnit"] + "" != ConsigneeMonetaryUnit || Common.NumberFormat(ConsigneeCharge) != Common.NumberFormat(RS["ConsigneeCharge"] + "")) {
				ModifyHistoryValue.Append("*Consignee Charge : " +
							Common.GetMonetaryUnit(RS["ConsigneeMonetaryUnit"] + "") + " " + Common.NumberFormat(RS["ConsigneeCharge"] + "") + " -> " +
							Common.GetMonetaryUnit(ConsigneeMonetaryUnit) + " " + Common.NumberFormat(ConsigneeCharge));
			}
		}
		RS.Dispose();
		//
		string queryModifyHistory = "";
		if (ModifyHistoryValue.ToString() != "") {
			queryModifyHistory = "INSERT INTO [dbo].[RequestModifyHistory] ([RequestFormPk],[ModifyWhich],[Value],[AccountID],[Registerd]) VALUES (" + RequestFormPk + ", 'Charge', N'" + ModifyHistoryValue + "', '" + AccountID + "', getdate());";
		}

		DB.SqlCmd.CommandText = "	SP_UpdateCalculateHeadForSetFixCharge  @RequestFormPk=" + Common.StringToDB(RequestFormPk, false, false) +
													"								, @StandardPriceListPk=" + Common.StringToDB(StandardPriceListPk, false, false) +
													"								, @StandardPriceA=" + Common.StringToDB(StandardPriceA, false, false) +
													"								, @CriterionValue=" + Common.StringToDB(CriterionValue, true, false) +
													"								, @ExchangeRate=" + Common.StringToDB(ExchangeRate, true, true) +
													"								, @ShipperMonetaryUnit=" + Common.StringToDB(ShipperMonetaryUnit, false, false) +
													"								, @ShipperCharge=" + Common.StringToDB(ShipperCharge, false, false) +
													"								, @ConsigneeMonetaryUnit=" + Common.StringToDB(ConsigneeMonetaryUnit, false, false) +
													"								, @ConsigneeCharge=" + Common.StringToDB(ConsigneeCharge, false, false) +
													"								, @ShipperBankAccountPk=" + Common.StringToDB(ShipperBankAccount, false, false) +
													"								, @ConsigneeBankAccountPk=" + Common.StringToDB(ConsigneeBankAccount, false, false) +
													"								, @WillPayTariff=" + Common.StringToDB(WhoWillPaidTariff, true, false) + ";" +
													dbcommand +
													  "DELETE FROM [dbo].[HISTORY] WHERE TABLE_PK=" + RequestFormPk + " AND CODE IN ('5', '6');" +
														 "INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [DESCRIPTION], [ACCOUNT_ID]) VALUES('RequestForm', " + RequestFormPk + ", '5', " + CommentShipper + ", " + AccountID + ")" +
														"INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [DESCRIPTION], [ACCOUNT_ID]) VALUES('RequestForm', " + RequestFormPk + ", '6', " + CommentConsignee + ", " + AccountID + ")" +
													GQ.AddRequestHistory(RequestFormPk, "59", AccountID, "") +
													"UPDATE RequestForm SET [PaymentWhoCL] = " + PaymentWhoCL + " WHERE RequestFormPk=" + RequestFormPk + ";" +
													queryModifyHistory;

		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		if (ShipperContentsSum != "") {
			string[] EachShipperContents = ShipperContentsSum.Split(Common.Splite51423, StringSplitOptions.None);
			SetTalkBusiness(EachShipperContents[0], EachShipperContents[1], EachShipperContents[2], EachShipperContents[3]);
		}
		if (ConsigneeContentsSum != "") {
			string[] EachConsigneeContents = ConsigneeContentsSum.Split(Common.Splite51423, StringSplitOptions.None);
			SetTalkBusiness(EachConsigneeContents[0], EachConsigneeContents[1], EachConsigneeContents[2], EachConsigneeContents[3]);
		}
		return "1";
	}

	//Admin/Dialog/Fixchrge.aspx
	[WebMethod]
	public String ModifyDeliveryCharge(string RequestFormPk, string CalculateBodySum, string AccountID) {
		StringBuilder dbcommand = new StringBuilder();
		GetQuery GQ = new GetQuery();
		string[] calculatebodyeach = CalculateBodySum.Split(Common.Splite22, StringSplitOptions.RemoveEmptyEntries);
		dbcommand.Append("	DELETE FROM RequestFormCalculateBody WHERE RequestFormPk=" + RequestFormPk + " and StandardPriceHeadPkNColumn='d';");
		foreach (string Row in calculatebodyeach) {
			string[] Each = Row.Split(Common.Splite11, StringSplitOptions.None);
			dbcommand.Append("	INSERT INTO RequestFormCalculateBody " +
											"		([RequestFormPk], [GubunCL], [Title], [Price], [MonetaryUnit], [StandardPriceHeadPkNColumn]) " +
											"	VALUES " +
											"		(" + RequestFormPk + ", " + Common.StringToDB(Each[0], false, false) + ", " + Common.StringToDB(Each[1], true, true) + ", " +
													Common.StringToDB(Each[2], false, false) + ", " + Common.StringToDB(Each[3], false, false) + ", " + Common.StringToDB(Each[4], true, false) + ");");
		}
		DB = new DBConn();
		DB.SqlCmd.CommandText = dbcommand +
													 GQ.AddRequestHistory(RequestFormPk, "60", AccountID, "");

		//return DB.SqlCmd.CommandText;
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	//Admin/Dialog/DeliveryCharge.aspx
	//Admin/RequestFormView

	[WebMethod]
	public String[] PrepareSendMessageFromRequest(string RequestFormPk) {
		List<string> ReturnValue = new List<string>();
		string ShipperPk = "";
		string ConsigneePk = "";
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT RFAI.[CODE], RFAI.[DESCRIPTION], A.Duties, A.Name, A.TEL, A.Mobile, A.Email
FROM [dbo].[HISTORY] AS RFAI
	LEFT JOIN Account_ AS A ON RFAI.DESCRIPTION=A.AccountID
WHERE RFAI.[TABLE_PK]=" + RequestFormPk +
	@" and (RFAI.[CODE]='10' or RFAI.[CODE]='11') " +
@"ORDER BY RFAI.[CODE] ";
		DB.DBCon.Open();

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (RS[0] + "" == "10") {
				ReturnValue.Add("DepartureStaff#@!" + RS["Duties"] + " " + RS["Name"] + " ( TEL :  " + RS["TEL"] + " Mobile :  " + RS["Mobile"] + ")");
				continue;
			}
			if (RS[0] + "" == "11") {
				ReturnValue.Add("ArrivalStaff#@!" + RS["Duties"] + " " + RS["Name"] + " ( TEL :  " + RS["TEL"] + " Mobile :  " + RS["Mobile"] + ")");
				continue;
			}
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = "EXECUTE SP_SelectRequestViewLoad @RequestFormPk=" + RequestFormPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ShipperPk = "" + RS["ShipperPk"];
			ConsigneePk = "" + RS["ConsigneePk"];
			ReturnValue.Add("ShipperPk#@!" + RS["ShipperPk"]);
			ReturnValue.Add("ConsigneePk#@!" + RS["ConsigneePk"]);
			ReturnValue.Add("CompanyCode#@!<li>발화인 : " + RS["ShipperCompanyName"] + " [" + RS["ShipperCode"] + "]</li><li>수하인 : " + RS["ConsigneeCompanyName"] + "" + " [" + RS["ConsigneeCode"] + "]</li>");
			ReturnValue.Add("Schedule#@!<li><input type=\"hidden\" id=\"HSchedule\" value=\"" + (RS["ArrivalDate"] + "" == "" ? "" : (RS["ArrivalDate"] + "").Substring(4)) + "\" /><input type=\"hidden\" id=\"HArrivalName\" value=\"" + RS["ArrivalName"] + "" + "\" />일정 : " +
				(RS["DepartureDate"] + "" == "" ? "" : (RS["DepartureDate"] + "").Substring(4)) + " " + RS["DepartureName"] + " → " +
				(RS["ArrivalDate"] + "" == "" ? "" : (RS["ArrivalDate"] + "").Substring(4)) + " " + RS["ArrivalName"] +
				"&nbsp;[ " + Common.GetTransportWay(RS["TransportWayCL"] + "") + " ]</li>");
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"	SELECT	TotalPackedCount, PackingUnit, TotalGrossWeight, TotalVolume
															FROM	[dbo].[RequestForm] 
															WHERE	RequestFormPk=" + RequestFormPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue.Add("Fright#@!<li>화물 : " + Common.NumberFormat(RS["TotalPackedCount"] + "") + Common.GetPackingUnit(RS["PackingUnit"] + "") + " " + Common.NumberFormat(RS["TotalGrossWeight"] + "") + "Kg " + Common.NumberFormat(RS["TotalVolume"] + "") + "CBM <input type=\"hidden\" id=\"HPackedCount\" value=\"" + Common.NumberFormat(RS["TotalPackedCount"] + "") + "\" /><input type=\"hidden\" id=\"HPackingUnit\" value=\"" + Common.GetPackingUnit(RS["PackingUnit"] + "") + "\" /></li>");
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = "SELECT A.Duties, A.Name, A.Mobile, A.Email, A.IsEmailNSMS, A.AccountPk FROM Company AS C left join Account_ AS A ON C.CompanyPk=A.CompanyPk WHERE C.CompanyPk=" + ShipperPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Add("ShipperStaff#@!" + RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5]);
		}
		RS.Dispose();
		if (ConsigneePk != "") {
			DB.SqlCmd.CommandText = "SELECT A.Duties, A.Name, A.Mobile, A.Email, A.IsEmailNSMS, A.AccountPk FROM Company AS C left join Account_ AS A ON C.CompanyPk=A.CompanyPk WHERE C.CompanyPk=" + ConsigneePk + ";";
			RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ReturnValue.Add("ConsigneeStaff#@!" + RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5]);
			}
			RS.Dispose();
		}

		DB.SqlCmd.CommandText = @"
			SELECT TBBH.[AREA_TO] 
			FROM [INTL2010].[dbo].[TransportBBHistory] AS TBBHistory 
				left join [INTL2010].[dbo].[OurBranchStorageCode] AS OBSC ON TBBHistory.StorageCode=OBSC.OurBranchStoragePk 
				left join [INTL2010].[dbo].[TRANSPORT_HEAD] AS TBBH ON TBBHistory.TransportBetweenBranchPk=TBBH.TRANSPORT_PK 
			WHERE TBBHistory.RequestFormPk=" + RequestFormPk + " and OurBranchCode=3157;";
		string Area_To = DB.SqlCmd.ExecuteScalar() + "";
		if (Area_To != "") {
			ReturnValue.Add("ArrivalPort#@!" + Area_To);
		}

		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}

	[WebMethod]
	public String[] MakeCompanyS_MsgSendFromTransport(string BBHPk, string WHO, string STEPCL) {
		List<string> ReturnValue = new List<string>();
		DB = new DBConn();
		if (STEPCL == "2") {
			STEPCL = "1";
		}
		DB.SqlCmd.CommandText = " EXECUTE SP_Select_TransportBetweenBranchView @StepCL=" + STEPCL + ", @BBHPk=" + BBHPk + ";";

		string toCode = WHO == "1" ? "ConsigneeCode" : "ShipperCode";

		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string TempRequestPK = "";
		while (RS.Read()) {
			if (TempRequestPK != RS["RequestFormPk"].ToString()) {
				TempRequestPK = RS["RequestFormPk"].ToString();
				ReturnValue.Add(RS["RequestFormPk"].ToString() + "#@!" + RS[toCode].ToString() + "#@!" + RS["DepartureDate"].ToString() + "#@!" + RS["ArrivalDate"].ToString() + "#@!" + RS["BoxCount"].ToString());
			}
			else {
				continue;
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}

	[WebMethod]
	public String MsgSendFromTransport(string RequestFormPkSum, string SenderID, string OurBranchPk, string SMSMessage, string WHO, string STEPCL) {
		Email emailsend = new Email();
		winic.Service1 ws = new winic.Service1();
		DB = new DBConn();
		//string RequestFormPkSum = "";
		//if (STEPCL == "2")
		//{
		//   STEPCL = "1";
		//}
		//DB.SqlCmd.CommandText = " EXECUTE SP_Select_TransportBetweenBranchView @StepCL=" + STEPCL + ", @BBHPk=" + BBHPk + ";";
		//DB.DBCon.Open();
		//SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		//while (RS.Read())
		//{
		//   RequestFormPkSum += RS["RequestFormPk"] + "#@!";
		//}
		//RS.Dispose();
		//DB.DBCon.Close();
		string[] split = RequestFormPkSum.Split(Common.Splite321, StringSplitOptions.RemoveEmptyEntries);
		string Companypk = "";
		string toPk = "";
		string name = "";
		string phoneNo = "";
		string phoneFirst3 = "";
		string gubuncl = "";
		List<string> history = new List<string>();
		StringBuilder Query = new StringBuilder();
		foreach (string e in split) {
			toPk = WHO == "1" ? "ConsigneePk" : "ShipperPk";
			DB.SqlCmd.CommandText = string.Format(@"
select R.ShipperPk,R.ConsigneePk,A.Name,A.Mobile,A.IsEmailNSMS from RequestForm R
left join Account_ A on R.{1}=A.CompanyPk
where RequestFormpk={0}", e, toPk);
			//1수입자 0 수출자
			DB.DBCon.Open();
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				if (RS["Mobile"].ToString().Trim() != "") {
					if (RS["IsEmailNSMS"].ToString() == "1" || RS["IsEmailNSMS"].ToString() == "3") {
						name = RS["Name"] + "";
						phoneNo = RS["Mobile"].ToString().Replace("-", "");
						if (phoneNo.Length < 4) {
							continue;
						}
						phoneFirst3 = phoneNo.Trim().Substring(0, 3);
						Companypk = RS[toPk] + "";
						bool isInhistory = false;
						for (int i = 0; i < history.Count; i++) {
							if (phoneNo == history[i]) {
								isInhistory = true;
								break;
							}
						}
						if (isInhistory) {
							continue;
						}
						else {
							history.Add(phoneNo);
						}

						if (phoneFirst3 == "010" || phoneFirst3 == "011" || phoneFirst3 == "016" || phoneFirst3 == "017" || phoneFirst3 == "018" || phoneFirst3 == "019" || phoneFirst3 == "070") {
							try {
								emailsend.SendMobileMsgKorea("0327728481", phoneNo, SMSMessage);
								gubuncl = "0";
							} catch (Exception) {
								gubuncl = "1";
							}
						}
						else if (phoneFirst3 == "139" || phoneFirst3 == "138" || phoneFirst3 == "137" || phoneFirst3 == "136" || phoneFirst3 == "135" || phoneFirst3 == "134" || phoneFirst3 == "159" || phoneFirst3 == "158" || phoneFirst3 == "152" || phoneFirst3 == "150" || phoneFirst3 == "188" || phoneFirst3 == "130" || phoneFirst3 == "131" || phoneFirst3 == "132" || phoneFirst3 == "155" || phoneFirst3 == "156" || phoneFirst3 == "186" || phoneFirst3 == "133" || phoneFirst3 == "153" || phoneFirst3 == "189" || phoneFirst3 == "185") {
							try {
								string resultmsgTochina = ws.SendMessages("intl2000", "ythq1717", phoneNo, SMSMessage, "");
								gubuncl = "2";
							} catch (Exception) {
								gubuncl = "3";
							}
						}
						else {
							gubuncl = "9";
						}
						Query.Append("INSERT INTO MsgSendedHistory ([GubunCL], [GubunPk], [SenderID], [Receiver], [ReceiverAddress], [FromPk], [ToPk], [Title], [Contents], [SendedTime]) " +
					 "VALUES (" + gubuncl + ", " + e + ", '" + SenderID + "', N'" + name + "', '" + phoneNo + "', '" + OurBranchPk + "' , " + Companypk + ", NULL, " + Common.StringToDB(SMSMessage, true, true) + ", getDate());");
					}
				}
			}
			RS.Dispose();
			DB.DBCon.Close();
		}
		DB.SqlCmd.CommandText = Query + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String MsgSendFromRequest(string SenderID, string MainContents, string EmailSum, string SMSSum, string ShipperStaff, string ConsigneeStaff, string ShipperCompanyPk, string ConsigneeCompanyPk, string RequestFormPk, string OurBranchPk, string SMSMessage, string K1YN) {
		Email emailsend = new Email();
		winic.Service1 ws = new winic.Service1();
		DB = new DBConn();
		StringBuilder Query = new StringBuilder();
		string[] smsRow = SMSSum.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
		List<string> history = new List<string>();
		foreach (string each in smsRow) {
			string[] Splited = each.Split(Common.Splite321, StringSplitOptions.None);
			string toPk = Splited[0] == "S" ? ShipperCompanyPk : ConsigneeCompanyPk;
			string name = Splited[1];
			string phoneNo = Splited[2].Replace("-", "");
			string phoneFirst3 = phoneNo.Trim().Substring(0, 3);
			string gubuncl = "";

			bool isInhistory = false;
			for (int i = 0; i < history.Count; i++) {
				if (phoneNo == history[i]) {
					isInhistory = true;
					break;
				}
			}
			if (isInhistory) {
				continue;
			}
			else {
				history.Add(phoneNo);
			}

			if (phoneFirst3 == "010" || phoneFirst3 == "011" || phoneFirst3 == "016" || phoneFirst3 == "017" || phoneFirst3 == "018" || phoneFirst3 == "019" || phoneFirst3 == "070") {
				try {
					emailsend.SendMobileMsgKorea("0327728481", phoneNo, SMSMessage);
					gubuncl = "0";
				} catch (Exception) {
					gubuncl = "1";
				}
			}
			else if (phoneFirst3 == "139" || phoneFirst3 == "138" || phoneFirst3 == "137" || phoneFirst3 == "136" || phoneFirst3 == "135" || phoneFirst3 == "134" || phoneFirst3 == "159" || phoneFirst3 == "158" || phoneFirst3 == "152" || phoneFirst3 == "150" || phoneFirst3 == "188" || phoneFirst3 == "130" || phoneFirst3 == "131" || phoneFirst3 == "132" || phoneFirst3 == "155" || phoneFirst3 == "156" || phoneFirst3 == "186" || phoneFirst3 == "133" || phoneFirst3 == "153" || phoneFirst3 == "189" || phoneFirst3 == "185") {
				try {
					string resultmsgTochina = ws.SendMessages("intl2000", "ythq1717", phoneNo, SMSMessage, "");
					gubuncl = "2";
				} catch (Exception) {
					gubuncl = "3";
				}
			}
			else {
				gubuncl = "9";
			}
			Query.Append("INSERT INTO MsgSendedHistory ([GubunCL], [GubunPk], [SenderID], [Receiver], [ReceiverAddress], [FromPk], [ToPk], [Title], [Contents], [SendedTime]) " +
		"VALUES (" + gubuncl + ", " + RequestFormPk + ", '" + SenderID + "', N'" + name + "', '" + phoneNo + "', '" + OurBranchPk + "' , " + toPk + ", NULL, " + Common.StringToDB(SMSMessage, true, true) + ", getDate());");
		}

		DB.DBCon.Open();
		string FromEmail = "korea@nn21.com";
		string From = "아이엘국제물류주식회사";

		DB.SqlCmd.CommandText = Query + " SELECT C.CompanyName, C.PresidentEmail FROM Company AS C left join Account_ AS A ON C.CompanyPk=A.CompanyPk WHERE A.AccountID='" + SenderID + "';";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			From = RS[0] + "";
			FromEmail = RS[1] + "";
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = "SELECT [GubunCL] FROM [Company] WHERE CompanyPk=" + ShipperCompanyPk + ";";
		string shippergubunCL = DB.SqlCmd.ExecuteScalar() + "";
		DB.SqlCmd.CommandText = "SELECT [GubunCL] FROM [Company] WHERE CompanyPk=" + ConsigneeCompanyPk + ";";
		string consigneegubunCL = DB.SqlCmd.ExecuteScalar() + "";

		string[] emailRow = EmailSum.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
		history = new List<string>();
		foreach (string row in emailRow) {
			string[] Each = row.Split(Common.Splite321, StringSplitOptions.None);

			bool isInhistory = false;
			for (int i = 0; i < history.Count; i++) {
				if (Each[2] == history[i]) {
					isInhistory = true;
					break;
				}
			}
			if (isInhistory) {
				continue;
			}
			else {
				history.Add(Each[2]);
			}

			if (Each[0] == "S") {
				if (shippergubunCL == "0") {
					DB.SqlCmd.CommandText = "	INSERT INTO MsgSendedHistory ([GubunCL], [GubunPk], [SenderID], [Receiver], [ReceiverAddress], [FromPk], [ToPk], [Title], [Contents], [SendedTime]) " +
																	"VALUES (4, " + RequestFormPk + ", '" + SenderID + "', N'" + Each[1] + "', '" + Each[2] + "', " + OurBranchPk + " , " + ShipperCompanyPk + ", NULL, N'" + MainContents + "', getDate())" +
																"	SELECT @@IDENTITY;";
					string Identity = DB.SqlCmd.ExecuteScalar() + "";
					emailsend.SendMailByCafe24(FromEmail, From, Each[2], Each[1], "(주)아이엘 화물 입고 안내메일", EmailMSGType1(MainContents + "<br /><a href=\"http://www.nn21.net\"> nn21.net</a>에서 상세한 내역을 확인해 주세요.  <div style=\" text-align:right; font-weight:bold; font-size:15px; letter-spacing:-1px;\">" + ShipperStaff + "</div></div>", Identity));
					continue;
				}
				if (shippergubunCL == "1") {
					DB.SqlCmd.CommandText = "	INSERT INTO MsgSendedHistory ([GubunCL], [GubunPk], [SenderID], [Receiver], [ReceiverAddress], [FromPk], [ToPk], [Title], [Contents], [SendedTime]) " +
																"VALUES (5, " + RequestFormPk + ", '" + SenderID + "', N'" + Each[1] + "', '" + Each[2] + "', " + OurBranchPk + " , " + ShipperCompanyPk + ", NULL, N'" + MainContents + "', getDate())" +
																"	SELECT @@IDENTITY;";
					string Identity = DB.SqlCmd.ExecuteScalar() + "";
					string temp = EmailMSGType1(MainContents + "<br /><a href=\"http://www.nn21.net/member/CompanyJoinFromEmail.aspx?E=" + Each[2] + "&S=" + ShipperCompanyPk + "\"> nn21.net</a>에서 상세한 내역을 확인해 주세요. " +
																								"<br />혹 위 링크가 작동하지 않는다면 주소표시줄에<br /> http://www.nn21.net/member/CompanyJoinFromEmail.aspx?E=" + Each[2] + "&S=" + ShipperCompanyPk + " 를 복사해서 사용해 주시면 감사하겠습니다. <div style=\" text-align:right; font-weight:bold; font-size:15px; letter-spacing:-1px;\">" + ShipperStaff + "</div></div>", Identity);
					emailsend.SendMailByCafe24(FromEmail, From, Each[2], Each[1], "(주)아이엘 화물 입고 안내메일", temp);
					continue;
				}
			}
			if (Each[0] == "C") {
				if (consigneegubunCL == "0") {
					DB.SqlCmd.CommandText = "	INSERT INTO MsgSendedHistory ([GubunCL], [GubunPk], [SenderID], [Receiver], [ReceiverAddress], [FromPk], [ToPk], [Title], [Contents], [SendedTime]) " +
																	"VALUES (4, " + RequestFormPk + ", '" + SenderID + "', N'" + Each[1] + "', " + Common.StringToDB(Each[2], true, false) + ", " + OurBranchPk + " , " + ConsigneeCompanyPk + ", NULL, N'" + MainContents + "', getDate())" +
							"	SELECT @@IDENTITY;";
					string Identity = DB.SqlCmd.ExecuteScalar() + "";
					if (K1YN == "") {
						emailsend.SendMailByCafe24(Each[2], Each[1], "(주)아이엘 화물 입고 안내메일", EmailMSGType1(MainContents + "<br /><a href=\"http://www.nn21.net\"> nn21.net</a>에서 상세한 내역을 확인해 주세요.  <div style=\" text-align:right; font-weight:bold; font-size:15px; letter-spacing:-1px;\">" + ConsigneeStaff + "</div></div>", Identity));
					}
					else {
						emailsend.SendMailByCafe24(Each[2], Each[1], "(주)아이엘 화물 입고 안내메일", EmailMSGType1(MainContents + "<br /><a href=\"http://www.nn21.net/Request/RequestFormView.aspx?Q=" + K1YN + "\"> nn21.net</a>에서 상세한 내역을 확인해 주세요.  <div style=\" text-align:right; font-weight:bold; font-size:15px; letter-spacing:-1px;\">" + ConsigneeStaff + "</div></div>", Identity));
					}

					continue;
				}
				if (consigneegubunCL == "1") {
					DB.SqlCmd.CommandText = "	INSERT INTO MsgSendedHistory ([GubunCL], [GubunPk], [SenderID], [Receiver], [ReceiverAddress], [FromPk], [ToPk], [Title], [Contents], [SendedTime]) " +
																		"VALUES (5, " + RequestFormPk + ", '" + SenderID + "', N'" + Each[1] + "', " + Common.StringToDB(Each[2], true, false) + ", " + OurBranchPk + " , " + ConsigneeCompanyPk + ", NULL, N'" + MainContents + "', getDate())" +
							"	SELECT @@IDENTITY;";
					string Identity = DB.SqlCmd.ExecuteScalar() + "";
					string temp = EmailMSGType1(MainContents + "<br /><a href=\"http://www.nn21.net/member/CompanyJoinFromEmail.aspx?E=" + Each[2] + "&S=" + ConsigneeCompanyPk + "\"> nn21.net</a>에서 상세한 내역을 확인해 주세요.  " +
																							"<br />혹 위 링크가 작동하지 않는다면 주소표시줄에<br /> http://www.nn21.net/member/CompanyJoinFromEmail.aspx?E=" + Each[2] + "&S=" + ConsigneeCompanyPk + " 를 복사해서 사용해 주시면 감사하겠습니다. <div style=\" text-align:right; font-weight:bold; font-size:15px; letter-spacing:-1px;\">" + ConsigneeStaff + "</div></div>", Identity);
					emailsend.SendMailByCafe24(Each[2], Each[1], "(주)아이엘 화물 입고 안내메일", temp);
					continue;
				}
			}
		}
		DB.DBCon.Close();
		return "1";
	}

	private String EmailMSGType1(string Main, string MsgSendedHistoryPk) {
		return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" +
"<html xmlns=\"http://www.w3.org/1999/xhtml\" >" +
	"<head>" +
		"<title>Untitled Page</title>" +
		"<style type=\"text/css\">" +
			"*{ font-family:돋움; font-size: 15px; }" +
		"</style>" +
	"</head>" +
"<body>" +
"<div style=\"width:700px;\">" +
	"<div style=\"height:95px; padding-top:5px; padding-left:20px;  padding-bottom:5px; border:2px solid #444444;\">" +
		"<div style=\"float:left; width:95px; padding:5px;  \">" +
			"<img alt=\"\" src=\"http://nn21.net/ForProvideDocuments/IL_Logo.aspx?S=" + MsgSendedHistoryPk + "\" width=\"90px\" height=\"80px\" />" +
		"</div>" +
		"<div style=\"padding-left:10px;\">" +
			"<div style=\"font-family:Sans-Serif; font-size:20px; letter-spacing:5px; padding-top:8px; \">INTERNATIONAL LOGISTICS CO.,LTD</div><div style=\"font-weight:bold; font-size:35px; letter-spacing:4px; padding-top:15px; \">아이엘국제종합물류주식회사</div>" +
		"</div>" +
	"</div>" +
	"<div style=\"font-size:16px; font-weight:bold; padding-left:20px; padding-right:20px; padding-top:10px; padding-bottom:30px;  line-height:34px; border:2px solid #444444; margin-top:-2px; \">" +
		Main +
	"</div>" +
	"<div style=\"padding:10px; border:2px solid #444444; margin-top:-2px;\">" +
		"<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
			"<tr>" +
				"<td style=\"font-size:17px; letter-spacing:2px; width:530px; padding-top:8px; padding-bottom:8px;\">" +
					"<strong>인천</strong> :  인천광역시 중구 신흥동 1가 31-1 아이엘빌딩 3층</td><td style=\"font-size:13px;\">TEL : 82)032-772-8480/3" +
				"</td>" +
			"</tr>" +
			"<tr>" +
				"<td style=\"font-size:17px; letter-spacing:2px; width:530px; padding-top:8px; padding-bottom:8px;\">" +
					"<strong>부산</strong> :  부산시 동래구 안락 2동 59-36" +
				"</td>" +
				"<td style=\"font-size:13px;\">TEL : 82)051-521-8481</td>" +
			"</tr>" +
			"<tr>" +
				"<td style=\"font-size:17px; letter-spacing:2px; width:530px; padding-top:8px; padding-bottom:8px;\">" +
					"<strong>华南</strong> : <span style=\"font-size:14px;\">廣州市 白雲區 嘉禾望岗 奧旺大廈 A棟 1層</span>" +
				"</td>" +
				"<td style=\"font-size:13px;\">TEL : 86)400-880-8300</td>" +
			"</tr>" +
			"<tr>" +
				"<td style=\"font-size:17px; letter-spacing:2px; width:530px; padding-top:8px; padding-bottom:8px;\">" +
					"<strong>浙江</strong> : <span style=\"font-size:14px;\">浙江省 义乌市 前成小区 30栋 3号</span></td>" +
					"<td style=\"font-size:13px;\">TEL : 86)400-708-1600</td>" +
			"</tr>" +
			"<tr>" +
				"<td style=\"font-size:17px; letter-spacing:2px; width:530px; padding-top:8px; padding-bottom:8px;\">" +
					"<strong>山东</strong> : <span style=\"font-size:14px;\">青岛市 城阳区 浦东路24号</span>" +
				"</td>" +
				"<td style=\"font-size:13px;\">TEL : 86)400-708-0060</td>" +
			"</tr>" +
			"<tr>" +
				"<td style=\"font-size:17px; letter-spacing:2px; width:530px; padding-top:8px; padding-bottom:8px;\">" +
					"<strong>東北</strong> : <span style=\"font-size:14px;\">沈阳市 和平区 安图北街2号 喜来送货运</span>" +
				"</td>" +
				"<td style=\"font-size:13px;\">TEL : 86)400-708-0060</td>" +
			"</tr>" +
		"</table>" +
	"</div>" +
"</div></body></html>";
	}

	//BMK RequestFormView.aspx
	[WebMethod]
	public String RequestDelete(string RequestFormPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "	DELETE FROM CommerdialConnectionWithRequest WHERE RequestFormPk=" + RequestFormPk + ";" +
													"	DELETE FROM MsgSendedHistory WHERE GubunCL=0 and GubunPk=" + RequestFormPk + ";" +
													"	DELETE FROM OurBranchStorage WHERE RequestFormPk=" + RequestFormPk + ";" +
													"	DELETE FROM RequestForm  WHERE RequestFormPk=" + RequestFormPk + ";" +
													"	DELETE FROM [dbo].[HISTORY] WHERE [TABLE_PK]=" + RequestFormPk + ";" +
													"	DELETE FROM TransportBBHistory WHERE RequestFormPk=" + RequestFormPk + ";" +
													"	DELETE FROM RequestFormItems WHERE RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String InsertCompanyByAdmin_(string AccountID, string[] CompanyValue, string StaffSum, string warehouseSum, string AdditionalInfoSum, string businessType) {

		string BranchPk_Own = "";
		if (AccountID.IndexOf("S!h@i#p$p%i^n&g*B(r)a_n+ch") > 0) {
			BranchPk_Own = AccountID.Substring(0, AccountID.IndexOf("S!h@i#p$p%i^n&g*B(r)a_n+ch"));
			AccountID = "";
		}

		DB = new DBConn();
		GetQuery GQ = new GetQuery();
		Query = new StringBuilder();

		DB.SqlCmd.CommandText = "INSERT INTO Company " +
			"([GubunCL], [CompanyCode], [CompanyName], [RegionCode], [CompanyAddress], [CompanyTEL], [CompanyFAX], [PresidentName], [PresidentEmail], [CompanyNo], [CompanyNamee], [ResponsibleStaff]) VALUES " +
				"( " + Common.StringToDB(CompanyValue[0], false, false) +   //(<GubunCL, smallint,>
				", " + Common.StringToDB(CompanyValue[1], true, false) +    //,<CompanyCode, varchar(50),>
				", " + Common.StringToDB(CompanyValue[2], true, true) + //,<CompanyName, nvarchar(50),>
				", " + Common.StringToDB(CompanyValue[3], true, false) +    //,<RegionCode, varchar(50),>
				", " + Common.StringToDB(CompanyValue[4], true, true) + //,<CompanyAddress, nvarchar(255),>
				", " + Common.StringToDB(CompanyValue[5], true, false) +    //,<CompanyTEL, varchar(40),>
				", " + Common.StringToDB(CompanyValue[6], true, false) +    //,<CompanyFAX, varchar(40),>
				", " + Common.StringToDB(CompanyValue[7], true, true) + //,<PresidentName, nvarchar(50),>
				", " + Common.StringToDB(CompanyValue[8], true, false) +    //,<PresidentEmail, varchar(50),>
				", " + Common.StringToDB(CompanyValue[9], true, false) + //,<CompanyNo, varchar(50),>
				", " + Common.StringToDB(CompanyValue[10], true, true) +    //,<CompanyNamee,>
				", " + Common.StringToDB(AccountID, true, false) +
				"); SELECT @@IDENTITY;";
		DB.DBCon.Open();
		string companypk = DB.SqlCmd.ExecuteScalar() + "";
		if (businessType != "") {
			Query.Append(GQ.InsertCompanyAdditional(companypk, "61", businessType));
		}

		if (BranchPk_Own != "") {
			Query.Append(@"
				INSERT INTO [dbo].[CompanyRelation] ( 
					[MainCompanyPk],[TargetCompanyPk],[GubunCL],[TargetCompanyNick],[Memo]
				) VALUES (
					" + BranchPk_Own + "," + companypk + ",70," + Common.StringToDB(CompanyValue[2], true, true) + ",NULL);");
		}

		string QueryCompanyWarehouse = "INSERT INTO CompanyWarehouse ([CompanyPk], [Title], [Address], [TEL], [Staff], [Mobile]) VALUES ({0}, {1}, {2}, {3}, {4}, {5});";

		string[] warehouseRow = warehouseSum.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
		foreach (string row in warehouseRow) {
			string[] Each = row.Split(Common.Splite22, StringSplitOptions.None);
			Query.Append(string.Format(QueryCompanyWarehouse, companypk, Each[0], Each[1], Each[2], Each[3], Each[4]));
		}

		string[] AdditionalInfoSumArray = AdditionalInfoSum.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);

		foreach (string AdditionalEach in AdditionalInfoSumArray) {
			Query.Append(GQ.InsertCompanyAdditional(companypk, AdditionalEach.Substring(0, 2), AdditionalEach.Substring(5)));
		}

		string[] StaffRow = StaffSum.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
		string AccountInsertQuery = "INSERT INTO Account_ ([GubunCL], [CompanyPk], [Duties], [Name], [TEL], [Mobile], [Email], [IsEmailNSMS]) VALUES (99, {0}, {1}, {2}, {3}, {4}, {5}, {6});";
		string[] tempstringarray = StaffSum.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);

		foreach (string thisRow in tempstringarray) {
			string[] ThisStaffEach = thisRow.Split(Common.Splite321, StringSplitOptions.None);
			Query.Append(String.Format(AccountInsertQuery, companypk,
				Common.StringToDB(ThisStaffEach[0], true, true),
				Common.StringToDB(ThisStaffEach[1], true, true),
				Common.StringToDB(ThisStaffEach[2], true, false),
				Common.StringToDB(ThisStaffEach[3], true, false),
				Common.StringToDB(ThisStaffEach[4], true, false),
				Common.StringToDB(ThisStaffEach[5], false, false)));
		}

		if (Query + "" != "") {
			DB.SqlCmd.CommandText = Query + "";
			DB.SqlCmd.ExecuteNonQuery();
		}
		DB.DBCon.Close();
		return companypk;
	}

	[WebMethod]
	public String SetTalkBusiness(string CompanyPk, string ID, string Contents, string GubunCL) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
DECLARE @Count int;
Select @Count=count(*) from [dbo].[COMMENT] WHERE [TABLE_NAME] = 'RequestForm' AND [CATEGORY]=" + GubunCL + @" and [TABLE_PK] = " + CompanyPk + @";
if (@Count!=0)
   UPDATE [dbo].[COMMENT] SET [CONTENTS]=N'" + Contents + @"' ,  [ACCOUNT_ID] = '" + ID + @"' WHERE [CATEGORY]=" + GubunCL + @" AND [TABLE_PK] = " + CompanyPk + @";
else
INSERT INTO [dbo].[COMMENT] ([TABLE_NAME], [TABLE_PK], [CATEGORY], [ACCOUNT_ID], [CONTENTS]) VALUES ('RequestForm', " + CompanyPk + ", " + GubunCL + ", '" + ID + "', N'" + Contents + "' );";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String InsertTalkBusiness(string CompanyPk, string ID, string Contents, string GubunCL) {
		DB = new DBConn();
		if (GubunCL == "4") {
			string[] ContentsSplit = Contents.Split(Common.Splite51423, StringSplitOptions.None);
			DB.SqlCmd.CommandText = "INSERT INTO FileComment ([FilePk], [FileAttachedPk], [AccountID], [Comment]) VALUES (" + CompanyPk + ", " + Common.StringToDB(ContentsSplit[0], false, false) + ", '" + ID + "', " + Common.StringToDB(ContentsSplit[1], true, true) + ");";
		}
		else if (GubunCL == "1") {
			DB.SqlCmd.CommandText = "INSERT INTO TalkBusiness (GubunPk, GubunCL, AccountID, Contents) VALUES " +
				"(" + CompanyPk + ", " + GubunCL + ", '" + ID + "', N'" + Contents.Replace("\n", "<br />").Replace("\r\n", "<br />") + "' );";
		}
		else {
			DB.SqlCmd.CommandText = "INSERT INTO TalkBusiness (GubunPk, GubunCL, AccountID, Contents) VALUES (" + CompanyPk + ", " + GubunCL + ", '" + ID + "', N'" + Contents.Replace("\n", "<br />").Replace("\r\n", "<br />") + "' );";
		}
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String InsertTalkBusiness_RelatedCompany(string CompanyPkSum, string ID, string Contents, string GubunCL) {
		DB = new DBConn();
		StringBuilder query = new StringBuilder();
		string[] temp = CompanyPkSum.Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
		foreach (string tempeach in temp) {
			query.Append("INSERT INTO [dbo].[COMMENT] ([TABLE_NAME], [TABLE_PK], [CATEGORY], [ACCOUNT_ID], [CONTENTS]) VALUES ('Company', " + tempeach + ", " + GubunCL + ", '" + ID + "', N'" + Contents + "' );");
		}

		DB.SqlCmd.CommandText = query.ToString();

		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	
	[WebMethod]
	public String ChangeGubunCLFromTalkBusiness(string TalkBusinessPk, string GubunCL) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "update [dbo].[COMMENT] SET [CATEGORY]=" + GubunCL + " WHERE [COMMENT_PK]=" + TalkBusinessPk + ";";

		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String[] LoadCompanyInfoLight(string RequestFormPk, string Gubun, string TextDetailView, string TextSangDamList, string AccountID) {
		string[] retun = new string[2];
		DB = new DBConn();
		if (Gubun == "s") {
			DB.SqlCmd.CommandText = "SELECT [ShipperPk] FROM RequestForm WHERE [RequestFormPk]=" + RequestFormPk + ";";
		}
		else if (Gubun == "c") {
			DB.SqlCmd.CommandText = "SELECT [ConsigneePk] FROM RequestForm WHERE [RequestFormPk]=" + RequestFormPk + ";";
		}
		else {
			return new string[] { "0" };
		}

		DB.DBCon.Open();
		string CompanyPk = DB.SqlCmd.ExecuteScalar() + "";
		if (CompanyPk == "") {
			DB.DBCon.Close();
			return new string[] { "0" };
		}
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = "SELECT CompanyCode, CompanyName, CompanyTEL, ResponsibleStaff FROM Company WHERE CompanyPk=" + CompanyPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		//20131125커쵸지사 수정
		//if (RS.Read()) {
		//		ReturnValue.Append("<fieldset style=\"padding-top:5px; padding-bottom:5px;\"><legend><strong>" + RS[1] + "</strong> : " + RS[2] +
		//						"	<input type=\"button\" value=\"" + TextDetailView + "\" onclick=\"location.href='" + (RS[0] + "" == "" ? "CompanyInfo.aspx?M=View&" : "CompanyView.aspx?") + "S=" + CompanyPk + "';\" /></legend>");
		//}
		if (AccountID == "ilqz1" || AccountID == "ilqz2" || AccountID == "ilqz3") {
			while (RS.Read()) {
				if (RS[3] + "" == "ilqz1" || RS[3] + "" == "ilqz2" || RS[3] + "" == "ilqz3") {
					ReturnValue.Append("<fieldset style=\"padding-top:5px; padding-bottom:5px;\"><legend><strong>" + RS[1] + "</strong> : " + RS[2] +
									"	<input type=\"button\" value=\"" + TextDetailView + "\" onclick=\"location.href='" + (RS[0] + "" == "" ? "CompanyInfo.aspx?M=View&" : "CompanyView.aspx?") + "S=" + CompanyPk + "';\" /></legend>");
				}
				else {
					ReturnValue.Append("<fieldset style=\"padding-top:5px; padding-bottom:5px;\"><legend><strong>" + RS[1] + "</strong> : " + RS[2] +
									"</legend>");
				}
			}
		}
		else {
			while (RS.Read()) {
				ReturnValue.Append("<fieldset style=\"padding-top:5px; padding-bottom:5px;\"><legend><strong>" + RS[1] + "</strong> : " + RS[2] +
								"	<input type=\"button\" value=\"" + TextDetailView + "\" onclick=\"location.href='" + (RS[0] + "" == "" ? "CompanyInfo.aspx?M=View&" : "CompanyView.aspx?") + "S=" + CompanyPk + "';\" /></legend>");
			}
		}

		RS.Dispose();
		//담당자
		DB.SqlCmd.CommandText = "SELECT A.Duties, A.Name, A.TEL, A.Mobile, AAI.Value FROM Account_ AS A left join AccountAdditionalInfo_ AS AAI ON A.AccountPk=AAI.AccountPk WHERE A.CompanyPk=" + CompanyPk + " and (AAI.GubunCL=1 or Isnull(AAI.GubunCL, 0)=0) order by A.GubunCL;";
		RS = DB.SqlCmd.ExecuteReader();
		ReturnValue.Append("<div style=\"padding-top:5px; padding-bottom:5px; text-align:left;\" >");
		while (RS.Read()) {
			ReturnValue.Append("<p>" + RS[0] + " " + RS[1] + " : " + RS[3] + "</p>");
		}
		ReturnValue.Append("</div></fieldset>");
		RS.Dispose();
		//담당자
		retun[0] = ReturnValue + "";
		ReturnValue = new StringBuilder();
		ReturnValue.Append("	<fieldset style=\"padding-top:5px; padding-bottom:5px;padding-left:5px;\">" +
										"		<legend><strong>" + TextSangDamList + "</strong>&nbsp;<input type=\"button\" value=\"Write\" style=\"height:17px; padding:0px;  \" onclick=\"popTalkBusiness('" + CompanyPk + "');\" /></legend>");

		List<sComment> Comment = new List<sComment>();
		HistoryC HisC = new HistoryC();
		Comment = HisC.LoadList_Comment("Company", CompanyPk, "'Basic'", ref DB);

		for (int i = 0; i < Comment.Count; i++) {
			if (Comment[i].Account_Id == AccountID || AccountID == "ilman") {
				ReturnValue.Append("<div class=\"Line1E8E8E8\">" + (Comment[i].Contents).Replace("\r\n", "<br />") + "<span style=\"color:red; cursor:hand;\" onclick=\"CommentDelete('" + Comment[i].Comment_Pk + "')\">X</span> <span style=\"color:gray;\">" + (Comment[i].Registerd) + "</span> <span style=\"color:Blue;\">" + Comment[i].Account_Id + "</span></div>");
			}
			else {
				ReturnValue.Append("<div class=\"Line1E8E8E8\">" + (Comment[i].Contents).Replace("\r\n", "<br />") + " <span style=\"color:gray;\">" + (Comment[i].Registerd) + "</span> <span style=\"color:Blue;\">" + Comment[i].Account_Id + "</span></div>");
			}
		}

		DB.DBCon.Close();
		retun[1] = ReturnValue + "</fieldset>";

		return retun;
	}

	[WebMethod]
	public String CheckStaffID(string StaffID) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT Count(*) FROM Account_ WHERE GubunCL in (93) and [AccountID]='" + StaffID + "';";
		DB.DBCon.Open();
		string StaffCount = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		return StaffCount;
	}

	//BMK 지사간 운송 Admin/Dialog/SelectTransportWay
	[WebMethod]
	public String LoadBranchStorage(string CompanyPk) {
		return new Request().LoadBranchStorage(CompanyPk);
	}

	[WebMethod]
	public String InsertRequestComment(string RequestFormPk, string Comment, string AccountID) {
		DB = new DBConn();
		DB.DBCon.Open();
		HistoryC HisC = new HistoryC();
		sHistory History = new sHistory();
		History.Table_Name = "RequestForm";
		History.Table_Pk = RequestFormPk;
		History.Code = "0";
		History.Account_Id = AccountID;
		History.Description = Comment;
		HisC.Set_History(History, ref DB);
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String TransportBBCLInsert(string TransportBBCL, string OurBranchPk, string Value) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "  INSERT INTO TransportBBCL (TransportBBCL, OurBranchPk, Value) VALUES " +
													"   (" + TransportBBCL + ", " + OurBranchPk + ", " + Common.StringToDB(Value, true, true) + "); " +
													"   select @@IDENTITY;";
		DB.DBCon.Open();
		string pk = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		return pk;
	}

	[WebMethod]
	public String[] TransportBBCLLoad(string TransportBBCL, string OurBranchPk) {
		List<string> ReturnValue = new List<string>();
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT TransportBBCLPk, [Value] FROM TransportBBCL WHERE TransportBBCL=" + TransportBBCL + " and OurBranchPk=" + OurBranchPk + " order by Value;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue.Add(RS[0] + "#@!" + RS[1]);
			while (RS.Read()) {
				ReturnValue.Add(RS[0] + "#@!" + RS[1]);
			}
		}
		else {
			ReturnValue.Add("N");
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}

	[WebMethod]
	public String TransportBBCLDelete(string Pk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "DELETE FROM TransportBBCL WHERE TransportBBCLPk=" + Pk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String[] LoadTransportBBHead(string BBHPk) {
		String[] ReturnValue;

		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
		SELECT
			TBH.ToBranchPk, TBH.FromDateTime, TBH.ToDateTime, TBH.Registerd, TBHStep.Step
			, TBHSub.[BLNo], TBHSub.[Description0], TBHSub.[Description1], TBHSub.[Description2]
			, TBHSub.[Description3]
			, TBHSub.[Description4]
			, TBHSub.[Description5]
			, TBHSub.[Description6]
			, TBHSub.[Description7]
			, TBHSub.[Description8]
			, TBHSub.[Description9]
		FROM TransportBBMain AS TBH
			left join TransportBBSub AS TBHSub ON TBH.TBBPk=TBHSub.TBBPk
			left join TransportBBStep AS TBHStep ON TBH.TBBPk=TBHStep.TransportBetweenBranchPk
		WHERE TBH.TBBPk=" + BBHPk;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue = new string[] { RS[0] + "", RS[1] + "", RS[2] + "", RS[3] + "", RS[4] + "", RS[5] + "", RS[6] + "", RS[7] + "", RS[8] + "", RS[9] + "", RS[10] + "", RS[11] + "", RS[12] + "", RS[13] + "", RS[14] + "", RS[15] + "" };
		}
		else {
			ReturnValue = new string[] { "", "", "", "", "",
				"", "", "", "", "",
				"", "", "", "", "",
				""};
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue;
	}

	//발송준비 이우지사에서 커쵸 지사내용 보일수 있게
	[WebMethod]
	public String[] PrepareTransportOnloadOurStorage(string BranchPk) {
		List<string> ReturnValue = new List<string>();
		DB = new DBConn();

		DB.SqlCmd.CommandText = @" 
SELECT 
	R.RequestFormPk, R.ShipperPk, R.ConsigneePk, R.ConsigneeCCLPk, R.ShipperCode, 
	R.CompanyInDocumentPk, R.ConsigneeCode, R.DepartureDate, R.ArrivalDate, R.DepartureRegionCode, 
	R.ArrivalRegionCode, R.TransportWayCL, R.StepCL, R.StockedDate, R.TotalPackedCount, 
	R.PackingUnit, R.TotalGrossWeight, R.TotalVolume, Storage.BoxCount, DepratureR.Name AS DName , 
	ArrivalR.Name AS AName , RFAI.ActID, RFAI.Value, RFAI.ActDate , 
	Storage.StorageCode, OSC.StorageName, C.CompanyName , C.CompanyNamee ,R.DocumentRequestCL,
	SCompanyAdd.Value AS SValue,CCompanyAdd.Value AS CValue
	, Storage.StatusCL
FROM OurBranchStorage AS Storage 
	left join RequestForm AS R on R.RequestFormPk=Storage.RequestFormPk  
	left join OurBranchStorageCode AS OSC On Storage.StorageCode=OSC.OurBranchStoragePk 
	left join RegionCode AS DepratureR on R.DepartureRegionCode=DepratureR.RegionCode
	left join RegionCode AS ArrivalR on R.ArrivalRegionCode=ArrivalR.RegionCode 
	left join (SELECT [TABLE_PK] RequestFormPk, [ACCOUNT_ID] ActID, [DESCRIPTION] Value, [REGISTERD] ActDate FROM [dbo].[HISTORY] WHERE [CODE]='0') AS RFAI on R.RequestFormPk=RFAI.RequestFormPk
	left join Company AS C on R.ShipperPk=C.CompanyPk 
	left join (select * from CompanyAdditionalInfomation where Title=80) SCompanyAdd on R.ShipperPk=SCompanyAdd .CompanyPk
	left join (select * from CompanyAdditionalInfomation where Title=80) CCompanyAdd on R.ConsigneePk=CCompanyAdd.CompanyPk
	WHERE 
		OSC.OurBranchCode=" + BranchPk + @" and 
		Storage.StatusCL in (0, 8) and (isnull(R.TotalPackedCount, 0)=0 or Storage.BoxCount<>0) 
	ORDER BY StorageCode ASC, ArrivalR.RegionCode ASC, Storage.StatusCL ASC , CAST(right(isnull(R.ConsigneeCode, '00'), len(isnull(R.ConsigneeCode, '00'))-2) AS varchar) ASC ";

		//return new String[] { DB.SqlCmd.CommandText };
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Add(RS["RequestFormPk"] + "#@!" + RS["ShipperCode"] + "#@!" + RS["ConsigneeCode"] + "#@!" + RS["DepartureDate"] + "#@!" + RS["ArrivalDate"] + "#@!" +
										Common.GetTransportWay(RS["TransportWayCL"] + "") + "#@!" + RS["TotalPackedCount"] + "#@!" + RS["PackingUnit"] + "#@!" + RS["TotalGrossWeight"] + "#@!" + RS["TotalVolume"] + "#@!" +
										RS["BoxCount"] + "#@!" + RS["DName"] + "#@!" + RS["AName"] + "#@!" + RS["ActID"] + "#@!" + RS["Value"] + "#@!" + RS["ActDate"] + "#@!" + RS["StorageCode"] + "#@!" + RS["StorageName"] + "#@!" + RS["Companyname"] + (RS["Companynamee"] + "" == "" ? "&nbsp;" : "/" + RS["Companynamee"]) + "#@!" + RS["StepCL"] + "#@!" + RS["DocumentRequestCL"] + "#@!" + RS["SValue"] + "#@!" + RS["CValue"] + "#@!" + RS["StatusCL"]);
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}

	[WebMethod]
	public String DeferRestore(string RequestFormPk, string ActID) {
		DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = "UPDATE RequestForm SET [StepCL] = 57 WHERE RequestFormPk=" + RequestFormPk + ";" +
													new GetQuery().AddRequestHistory(RequestFormPk, "49", ActID, "");
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	
	[WebMethod]
	public String AddComment(string BBHeadPk, string AccountID, string Comment) {
		DB = new DBConn();
		DB.DBCon.Open();
		HistoryC HisC = new HistoryC();
		sComment TC = new sComment();
		TC.Table_Name = "TRANSPORT_HEAD";
		TC.Table_Pk = BBHeadPk;
		TC.Account_Id = AccountID;
		TC.Category = "Transport_Head";
		TC.Contents = Comment;
		HisC.Set_Comment(TC, ref DB);

		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String AddDO(string BBHeadPk, string HDOBLNo, string MRN, string MSN, string AccountID) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "INSERT INTO CommercialDocumentDO (TransportBBPk, BLNo, MRN, MSN, AccountID) VALUES (" + BBHeadPk + ", '" + HDOBLNo + "', '" + MRN + "', '" + MSN + "', '" + AccountID + "');";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String UpdateDO(string BBHeadPk, string HDOBLNo, string MRN, string MSN, string AccountID) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE CommercialDocumentDO SET [MRN] = '" + MRN +
													 "', [BLNo] = '" + HDOBLNo +
													 "', [MSN] = '" + MSN +
													 "', [AccountID] = '" + AccountID +
													 "' WHERE TransportBBPk=" + BBHeadPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return BBHeadPk;
	}

	[WebMethod]
	public String FixBoxCountOnPrepareTransport(string AccountID, string[] RequestFormPk, string[] BoxCount, string[] BeforeStorage) {
		DB = new DBConn();
		Query = new StringBuilder();
		HistoryC HisC = new HistoryC();
		sHistory History;
		DB.DBCon.Open();
		for (int i = 0; i < RequestFormPk.Length; i++) {
			DB.SqlCmd.CommandText = "SELECT [StorageName] FROM [INTL2010].[dbo].[OurBranchStorageCode] WHERE OurBranchStoragePk=" + BeforeStorage[i] + ";";
			string comment = "=> " + BoxCount[i] + " FROM " + DB.SqlCmd.ExecuteScalar();

			if (BoxCount[i] == "0") {
				History = new sHistory();
				History.Table_Name = "RequestForm";
				History.Table_Pk = RequestFormPk[i];
				History.Code = "54";
				History.Account_Id = AccountID;
				History.Description = comment;
				HisC.Set_History(History, ref DB);
				Query.Append("DELETE FROM OurBranchStorage WHERE  [StatusCL]=0 and [StorageCode] = " + BeforeStorage[i] + " and [RequestFormPk] = " + RequestFormPk[i] + ";");
			}
			else {
				History = new sHistory();
				History.Table_Name = "RequestForm";
				History.Table_Pk = RequestFormPk[i];
				History.Code = "55";
				History.Account_Id = AccountID;
				History.Description = comment;
				HisC.Set_History(History, ref DB);
				Query.Append("UPDATE OurBranchStorage SET [BoxCount] = " + BoxCount[i] + " WHERE  [StatusCL]=0 and [StorageCode] = " + BeforeStorage[i] + " and [RequestFormPk] = " + RequestFormPk[i] + ";");
			}
		}
		DB.SqlCmd.CommandText = Query + "";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	
	[WebMethod]
	public String ReStorcedInFromPacked(string ToStorage, string BBHeadPk, string[] RequestFormPkSum, string[] BoxCountSum, string AccountID) {
		DB = new DBConn();
		Query = new StringBuilder();
		for (int i = 0; i < RequestFormPkSum.Length; i++) {
			Query.Append("EXECUTE SP_StorageUnPacked @RequestFormPk=" + RequestFormPkSum[i] + ", @ToStorage=" + ToStorage + ", @PackedCount=" + BoxCountSum[i] + ";" +
									"EXECUTE SP_StorageUnPacked2 @TransportBBPk=" + BBHeadPk + ", @RequestFormPk=" + RequestFormPkSum[i] + ", @PackedCount=" + BoxCountSum[i] + ";");
		}
		DB.SqlCmd.CommandText = Query + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	
	[WebMethod]
	public String PackingSend(string Gubun, string BBHPk, string AccountID) {
		DB = new DBConn();

		if (Gubun == "pre") {
			DB.SqlCmd.CommandText = "UPDATE TransportBBStep SET Step = 1 WHERE TransportBetweenBranchPk=" + BBHPk + ";";
			DB.DBCon.Open();
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		}
		else {
			DB.SqlCmd.CommandText = @"SELECT TBH.TransportCL, FC.CompanyName, DC.CompanyName
FROM TransportBBHead as TBH
	left join Company AS FC ON FC.CompanyPk=TBH.FromBranchPk
	left join Company AS DC ON DC.CompanyPk=TBH.ToBranchPk
WHERE TBH.TransportBetweenBranchPk=" + BBHPk + ";";
			DB.DBCon.Open();
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();

			string comment;
			if (RS.Read()) {
				comment = Common.GetBetweenBranchTransportWay(RS[0] + "") + " <a href=\"TransportBetweenBranchView.aspx?S=" + BBHPk + "\">" + RS[1] + "-" + RS[2] + "</a>";
				RS.Dispose();
			}
			else {
				RS.Dispose();
				DB.DBCon.Close();
				return "0";
			}

			DB.SqlCmd.CommandText = "SELECT RequestFormPk FROM OurBranchStorage WHERE TransportBetweenBranchPk=" + BBHPk + " and StatusCL=1;";
			RS = DB.SqlCmd.ExecuteReader();
			StringBuilder Query = new StringBuilder();
			if (RS.Read()) {
				Query.Append("UPDATE TransportBBStep SET Step = 2 WHERE TransportBetweenBranchPk=" + BBHPk + ";" +
					new GetQuery().AddRequestHistory(RS[0] + "", "62", AccountID, comment));
				while (RS.Read()) {
					Query.Append(new GetQuery().AddRequestHistory(RS[0] + "", "62", AccountID, comment));
				}
			}
			RS.Dispose();
			DB.SqlCmd.CommandText = "" + Query;

			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		}
	}

	[WebMethod]
	public String PackingCancle(string BBHPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT Count(*) FROM OurBranchStorage WHERE TransportBetweenBranchPk=" + BBHPk + ";";
		//return DB.SqlCmd.CommandText;
		DB.DBCon.Open();
		string Count = DB.SqlCmd.ExecuteScalar() + "";
		if (Count == "0") {
			DB.SqlCmd.CommandText = "	DELETE FROM [dbo].[TRANSPORT_HEAD] WHERE [TRANSPORT_PK]=" + BBHPk + ";" +
														"	DELETE FROM TransportBBStep WHERE TransportBetweenBranchPk=" + BBHPk + ";" +
														"	DELETE FROM [File] WHERE [GubunCL] = 0 and [GubunPk] = " + BBHPk + ";";
			//return DB.SqlCmd.CommandText;
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		}
		else {
			DB.DBCon.Close();
			return "0";
		}
	}

	[WebMethod]
	public String ReceiveTransportBB(string BBHPk, string ToBranchPk, string ToStorage, string AccountID) {
		try {
			DB = new DBConn();
			StringBuilder Query = new StringBuilder();
			HistoryC HisC = new HistoryC();
			sHistory History;

			DB.SqlCmd.CommandText = "SELECT [StorageName] FROM OurBranchStorageCode WHERE OurBranchStoragePk=" + ToStorage + ";";
			DB.DBCon.Open();
			string comment = "<a href=\"TransportBetweenBranchView.aspx?S=" + BBHPk + "\">To " + DB.SqlCmd.ExecuteScalar() + "</a>";

			DB.SqlCmd.CommandText = @"
				SELECT OS.RequestFormPk, OS.BoxCount
				FROM OurBranchStorage AS OS
					left join RequestForm AS R ON OS.RequestFormPk=R.RequestFormPk
					left join RegionCode AS DRC ON R.ArrivalRegionCode=DRC.RegionCode
				WHERE OS.TransportBetweenBranchPk=" + BBHPk + " and DRC.OurBranchCode<>" + ToBranchPk + ";";
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				History = new sHistory();
				History.Table_Name = "RequestForm";
				History.Table_Pk = RS["RequestFormPk"] + "";
				History.Code = "67";
				History.Description = comment;
				History.Account_Id = AccountID;
				HisC.Set_History(History, ref DB);
				Query.Append("EXECUTE SP_StorageUnPacked @RequestFormPk=" + RS["RequestFormPk"] + ", @ToStorage=" + ToStorage + ", @PackedCount=" + RS["BoxCount"] + ";");
			}
			RS.Dispose();

			DB.SqlCmd.CommandText = @"
SELECT OS.RequestFormPk, OS.BoxCount, OS.Comment , RO.OurBranchStorageOutPk, RO.BoxCount AS AleadySet
FROM OurBranchStorage AS OS
	left join RequestForm AS R ON OS.RequestFormPk=R.RequestFormPk
	left join RegionCode AS DRC ON R.ArrivalRegionCode=DRC.RegionCode
	left join (SELECT OurBranchStorageOutPk, RequestFormPk, BoxCount FROM OurBranchStorageOut WHERE isnull(TransportBetweenBranchPk, 0)=0 ) AS RO ON RO.RequestFormPk=OS.RequestFormPk
WHERE OS.TransportBetweenBranchPk=" + BBHPk + " and DRC.OurBranchCode=" + ToBranchPk + ";";
			RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				if (RS["BoxCount"] + "" == RS["AleadySet"] + "") {
					Query.Append("UPDATE OurBranchStorageOut SET [StorageCode] = " + ToStorage +
						", StockedDate = getDate() " +
						", TransportBetweenBranchPk = " + BBHPk +
						", [StatusCL] = 5 " +
						", Comment = " + Common.StringToDB(RS["Comment"] + "", true, true) +
						" WHERE OurBranchStorageOutPk=" + RS["OurBranchStorageOutPk"] + ";");
				}
				else if (RS["AleadySet"] + "" == "") {
					Query.Append("INSERT INTO OurBranchStorageOut (StorageCode, RequestFormPk, BoxCount, StockedDate, TransportBetweenBranchPk, StatusCL, Comment) VALUES ( " +
					ToStorage + ", " + RS["RequestFormPk"] + ", " + RS["BoxCount"] + ", getDate(), " + BBHPk + ", 4, " + Common.StringToDB(RS["Comment"] + "", true, true) + ");");
				}
				else {
					int boxcount = Int32.Parse(RS["BoxCount"] + "");
					int aleadyset = Int32.Parse(RS["AleadySet"] + "");
					if (boxcount < aleadyset) {
						Query.Append("UPDATE OurBranchStorageOut SET [StorageCode] = " + ToStorage +
						", StockedDate = getDate() " +
						", TransportBetweenBranchPk = " + BBHPk +
						", [StatusCL] = 5 " +
						", Comment = " + Common.StringToDB(RS["Comment"] + "", true, true) +
						" WHERE OurBranchStorageOutPk=" + RS["OurBranchStorageOutPk"] + ";");

						Query.Append("INSERT INTO OurBranchStorageOut (StorageCode, RequestFormPk, BoxCount, StockedDate, TransportBetweenBranchPk, StatusCL, Comment) VALUES ( " +
							ToStorage + ", " + RS["RequestFormPk"] + ", " + (aleadyset - boxcount) + ", getDate(), " + BBHPk + ", 4, " + Common.StringToDB(RS["Comment"] + "", true, true) + ");");
					}
					else {
						Query.Append("UPDATE OurBranchStorageOut SET [StorageCode] = " + ToStorage +
						",BoxCount = " + aleadyset +
						", StockedDate = getDate() " +
						", TransportBetweenBranchPk = " + BBHPk +
						", StatusCL = 5 " +
						", Comment = " + Common.StringToDB(RS["Comment"] + "", true, true) +
						" WHERE OurBranchStorageOutPk=" + RS["OurBranchStorageOutPk"] + ";");

						Query.Append("INSERT INTO OurBranchStorageOut (StorageCode, RequestFormPk, BoxCount, StockedDate, TransportBetweenBranchPk, StatusCL, Comment) VALUES ( " +
							ToStorage + ", " + RS["RequestFormPk"] + ", " + (boxcount - aleadyset) + ", getDate(), " + BBHPk + ", 4, " + Common.StringToDB(RS["Comment"] + "", true, true) + ");");
					}
				}
			}
			RS.Dispose();
			//"	UPDATE TransportBBHead SET ArrivalDateTime = getDate() WHERE TransportBetweenBranchPk=" + BBHPk + "; " +
			DB.SqlCmd.CommandText = @"
			INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID], [DESCRIPTION])
				SELECT 'RequestForm', OS.RequestFormPk , '66', '" + AccountID + "', N'" + comment + "'" +
				"	FROM OurBranchStorage AS OS " +
				"		left join RequestForm AS R ON OS.RequestFormPk=R.RequestFormPk " +
				"		left join RegionCode AS DRC ON R.ArrivalRegionCode=DRC.RegionCode " +
				"	WHERE OS.TransportBetweenBranchPk=" + BBHPk + " and DRC.OurBranchCode=" + ToBranchPk + ";" +
				Query +
				"	update [INTL2010].[dbo].[TransportBBStep] SET Step=3 WHERE TransportBetweenBranchPk=" + BBHPk + "; " +
				"	INSERT INTO TransportBBHistory (StorageCode, RequestFormPk, BoxCount, TransportBetweenBranchPk) " +
				"	SELECT " + ToStorage + ", RequestFormPk, BoxCount, " + BBHPk + " FROM OurBranchStorage WHERE TransportBetweenBranchPk=" + BBHPk + " and StatusCL=1; " +
				"	DELETE FROM OurBranchStorage WHERE TransportBetweenBranchPk=" + BBHPk + " and StatusCL=1;";
			DB.SqlCmd.ExecuteNonQuery();
			DB.SqlCmd.CommandText = "	DELETE FROM [TransportBBStep] WHERE Step=3; ";
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		} catch (Exception ex) {
			return DB.SqlCmd.CommandText;
			throw;
		}
	}

	[WebMethod]
	public String ChangeStorage(string BBHPk, string ToBranchPk, string ToStorage, string AccountID) {
		DB = new DBConn();
		StringBuilder Query = new StringBuilder();

		DB.SqlCmd.CommandText = "SELECT [StorageName] FROM OurBranchStorageCode WHERE OurBranchStoragePk=" + ToStorage + ";";
		DB.DBCon.Open();
		string comment = "<a href=\"TransportBetweenBranchView.aspx?S=" + BBHPk + "\">To " + DB.SqlCmd.ExecuteScalar() + "</a>";

		DB.SqlCmd.CommandText = "update [OurBranchStorageOut] SET [StorageCode] = '" + ToStorage + "' where [TransportBetweenBranchPk]=" + BBHPk + ";" +
			"update [TransportBBHistory] SET [StorageCode] = '" + ToStorage + "' where [TransportBetweenBranchPk]=" + BBHPk + ";" +
			"INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID], [DESCRIPTION]) " +
			"SELECT 'RequestForm', [RequestFormPk], '69', '" + AccountID + "', N'" + comment + "' FROM [TransportBBHistory] WHERE [TransportBetweenBranchPk]=" + BBHPk + ";";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String[] InsertCommercialDocument(string RequestFormPk, string BLNo, string ShipperName, string ShipperAddress, string ConsigneeName, string ConsigneeAddress, string ShipperNamePk) {
		DB = new DBConn();
		DB.DBCon.Open();
		string fileName = "";
		if (BLNo != "") {
			DB.SqlCmd.CommandText = "SELECT [CommercialDocumentHeadPk] FROM CommercialDocument WHERE [BLNo]='" + BLNo + "';";
			String DocumentPk = DB.SqlCmd.ExecuteScalar() + "";
			if (DocumentPk != "") {
				DB.DBCon.Close();
				return new String[] { "0", DocumentPk };
			}
		}
		if (ShipperNamePk != "") {
			DB.SqlCmd.CommandText = "SELECT [FilePk], [FileName] FROM [INTL2010].[dbo].[File] WHERE GubunCL=3 and GubunPk=" + ShipperNamePk + ";";
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				fileName = "/3/" + ShipperNamePk + "_" + RS[0] + "_" + RS[1];
			}
			else {
				fileName = "";
			}
			RS.Dispose();
		}

		DB.SqlCmd.CommandText = "INSERT INTO CommercialDocument ([BLNo], [Shipper], [ShipperAddress], [Consignee], [ConsigneeAddress], [StampImg], [Registerd]) VALUES " +
													"('" + BLNo + "', " + Common.StringToDB(ShipperName, true, false) + ", " + Common.StringToDB(ShipperAddress, true, false) + ", " + Common.StringToDB(ConsigneeName, true, false) + ", " + Common.StringToDB(ConsigneeAddress, true, false) + ", " + Common.StringToDB(fileName, true, false) + ", getDate());" +
													"Select @@IDENTITY;";
		string identity = DB.SqlCmd.ExecuteScalar() + "";
		DB.SqlCmd.CommandText = "INSERT INTO CommerdialConnectionWithRequest ([CommercialDocumentPk], [RequestFormPk]) VALUES (" + identity + ", " + RequestFormPk + ");";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return new String[] { "1" };
	}

	[WebMethod]
	public String AddCommercialDocument(string RequestFormPk, string DocumentPk, string AccountID) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "INSERT INTO CommerdialConnectionWithRequest ([CommercialDocumentPk], [RequestFormPk]) VALUES (" + DocumentPk + ", " + RequestFormPk + ");" +
													"	UPDATE RequestForm SET [DocumentStepCL] = 1 WHERE RequestFormPk=" + RequestFormPk + ";" +
													new GetQuery().AddRequestHistory(RequestFormPk, "1", AccountID, "");
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String LoadBLNo(string RequestFormPk) {
		string BLNo;
		DB = new DBConn();
		DB.SqlCmd.CommandText = "	SELECT R.ConsigneePk,  R.ConsigneeCode, R.DepartureDate, TD.Initial " +
													"	FROM RequestForm AS R	" +
													"		left join TransportWayCLDescription AS TD ON R.TransportWayCL=TD.TransportWayCL " +
													"	WHERE R.RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string consigneepk = "";
		if (RS.Read()) {
			if ((RS["DepartureDate"] + "") != "" && (RS["Initial"] + "") != "" && (RS["ConsigneeCode"] + "") != "") {
				string companyCode = RS["ConsigneeCode"] + "";
				//kr092a 이런식의 고객번호로 인하여 뒷자리 짜름
				//Regex onlyAlpha = new Regex("\\D");
				//companyCode = companyCode.Replace(companyCode, "");
				companyCode = companyCode.Substring(2);
				consigneepk = RS["ConsigneePk"] + "";
				while (companyCode.Length < 4) {
					companyCode = "0" + companyCode;
				}
				BLNo = (RS["DepartureDate"] + "").Substring(2) + RS["Initial"] + companyCode;
			}
			else {
				BLNo = "";
			}
		}
		else {
			BLNo = "";
		}
		RS.Dispose();

		string fourcode = "";
		if (consigneepk != "") {
			DB.SqlCmd.CommandText = @"
			SELECT [Value] 
			FROM [INTL2010].[dbo].[CompanyAdditionalInfomation] 
			WHERE [Title]=152 and CompanyPk=" + consigneepk + "; ";
			string value = DB.SqlCmd.ExecuteScalar() + "";
			if (value == "AACO") {
				fourcode = "AACO";
			}
			else {
				fourcode = "INTL";
			}
		}

		if (BLNo != "") {
			BLNo = fourcode + BLNo;
		}

		DB.DBCon.Close();
		return BLNo;
	}

	[WebMethod]
	public String[] MakeBL(string RequestFormPk, string FourCode, string BLNo, string AccountID) {
		DB = new DBConn();
		SqlDataReader RS;
		DB.DBCon.Open();
		if (BLNo != "") {
			//DB.SqlCmd.CommandText = "SELECT [CommercialDocumentHeadPk] FROM CommercialDocument WHERE [BLNo]='" + BLNo + "';";
			DB.SqlCmd.CommandText = @"
DECLARE @HeadPk int;
DECLARE @BeforeM smallint;
DECLARE @NowM smallint;

SELECT @HeadPk=CD.[CommercialDocumentHeadPk], @BeforeM=R.MonetaryUnitCL
  FROM [CommercialDocument] AS CD
	left join [CommerdialConnectionWithRequest] AS CCWR ON CD.[CommercialDocumentHeadPk]=CCWR.[CommercialDocumentPk]
	left join RequestForm AS R ON CCWR.RequestFormPk=R.RequestFormPk
	WHERE CD.BLNo='" + BLNo + @"';
SELECT @NowM=[MonetaryUnitCL] FROM [RequestForm] WHERE RequestFormPk=" + RequestFormPk + @";
  SELECT @HeadPk, @BeforeM, @NowM;";

			RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				if (RS[0] + "" != "") {
					if (RS[1] + "" == RS[2] + "") {
						string documentPk = RS[0] + "";
						RS.Dispose();
						DB.DBCon.Close();
						return new String[] { "0", documentPk };
					}
					else {
						RS.Dispose();
						DB.DBCon.Close();
						return new String[] { "-1" };
					}
				}
			}
			RS.Dispose();
			//String DocumentPk = DB.SqlCmd.ExecuteScalar() + "";
		}
		DB.SqlCmd.CommandText = @"
			DECLARE @ShipperC int;
			DECLARE @ConsigneeC int;
			SELECT @ShipperC=[ShipperClearanceNamePk], @ConsigneeC=[ConsigneeClearanceNamePk] FROM [RequestForm] WHERE RequestFormPk=" + RequestFormPk + @";
			SELECT
				'S', CID.Name, CID.Address, F.[PhysicalPath] 
			FROM 
				CompanyInDocument AS CID 
				left join ( 
					SELECT FilePk, GubunPk, FileName, [PhysicalPath] FROM [File] 
					WHERE isnull(GubunCL, 3)=3 ) AS F On CID.CompanyInDocumentPk=F.GubunPk 
					WHERE CID.CompanyInDocumentPk=@ShipperC 
				
					UNION ALL

					SELECT 'C', CID.Name, CID.Address, F.[PhysicalPath] 
					FROM 
						CompanyInDocument AS CID 
						left join (
							SELECT FilePk, GubunPk, FileName, [PhysicalPath] FROM [File] WHERE isnull(GubunCL, 3)=3 ) AS F On CID.CompanyInDocumentPk=F.GubunPk
WHERE CID.CompanyInDocumentPk=@ConsigneeC";
		string Shipper = "";
		string ShipperAddress = "";
		string Consignee = "";
		string ConsigneeAddress = "";
		string StampImg = "";

		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (RS[0] + "" == "S") {
				Shipper = RS[1] + "";
				ShipperAddress = RS[2] + "";
				StampImg = "/" + RS[3] + "";
			}
			else {
				Consignee = RS[1] + "";
				ConsigneeAddress = RS[2] + "";
			}
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = "	INSERT INTO CommercialDocument ([BLNo], [Shipper], [ShipperAddress], [Consignee], [ConsigneeAddress], [StampImg], [Registerd]) VALUES ('" +
			BLNo + "', " +
			Common.StringToDB(Shipper, true, false) + ", " +
			Common.StringToDB(ShipperAddress, true, false) + ", " +
			Common.StringToDB(Consignee, true, false) + ", " +
			Common.StringToDB(ConsigneeAddress, true, false) + ", " +
			Common.StringToDB(StampImg, true, false) +
			", getDate());" +
			"	Select @@IDENTITY;";
		string identity = DB.SqlCmd.ExecuteScalar() + "";

		DB.SqlCmd.CommandText = "	INSERT INTO CommerdialConnectionWithRequest ([CommercialDocumentPk], [RequestFormPk]) VALUES (" + identity + ", " + RequestFormPk + ");" +
													"	UPDATE RequestForm SET [DocumentStepCL] = 1 WHERE RequestFormPk=" + RequestFormPk + ";";
		DB.SqlCmd.ExecuteNonQuery();
		HistoryC HisC = new HistoryC();
		sHistory History = new sHistory();
		History.Table_Name = "RequestForm";
		History.Table_Pk = RequestFormPk;
		History.Code = "1";
		History.Account_Id = AccountID;
		HisC.Set_History(History, ref DB);

		History = new sHistory();
		History.Table_Name = "RequestForm";
		History.Table_Pk = RequestFormPk;
		History.Code = "152";
		History.Account_Id = AccountID;
		History.Description = FourCode;
		HisC.Set_History(History, ref DB);

		DB.DBCon.Close();
		return new String[] { "1" };
	}

	//BMK Admin/CheckDescription.aspx
	[WebMethod]
	public String ModifyBLNo(string CommercialDocumenPk, string BLNo) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT CommercialDocumentHeadPk FROM CommercialDocument WHERE BLNo='" + BLNo + "';";
		DB.DBCon.Open();
		string result = DB.SqlCmd.ExecuteScalar() + "";
		if (result == "") {
			DB.SqlCmd.CommandText = "UPDATE CommercialDocument SET [BLNo] = '" + BLNo + "' WHERE CommercialDocumentHeadPk=" + CommercialDocumenPk + ";";
			DB.SqlCmd.ExecuteNonQuery();
		}
		DB.DBCon.Close();
		return result;
	}

	[WebMethod]
	public String UnionBL(string CommercialDocumentPkBefore, string CommercialDocumentPkAfter, string RequestFormPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT COUNT(*) FROM CommerdialConnectionWithRequest WHERE [CommercialDocumentPk]=" + CommercialDocumentPkBefore + ";";
		DB.DBCon.Open();
		String BeforeCount = DB.SqlCmd.ExecuteScalar() + "";
		if (BeforeCount == "1") {
			DB.SqlCmd.CommandText = "DELETE FROM CommerdialConnectionWithRequest WHERE [CommercialDocumentPk]=" + CommercialDocumentPkBefore + ";" +
				"DELETE FROM CommercialDocument WHERE CommercialDocumentHeadPk=" + CommercialDocumentPkBefore + ";";
		}
		else {
			DB.SqlCmd.CommandText = "DELETE FROM CommerdialConnectionWithRequest WHERE [RequestFormPk]=" + RequestFormPk + ";";
		}
		DB.SqlCmd.CommandText += "DELETE FROM CommercialDocumentTariff WHERE [CommercialDocumentHeadPk] in (" + CommercialDocumentPkBefore + ", " + CommercialDocumentPkAfter + ");" +
			"INSERT INTO CommerdialConnectionWithRequest ([CommercialDocumentPk], [RequestFormPk]) VALUES (" + CommercialDocumentPkAfter + ", " + RequestFormPk + ");";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return CommercialDocumentPkAfter;
	}

	[WebMethod]
	public String BLSplit(string CommercialDocumentPk, string RequestFormPk, string NewBLNo) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"INSERT INTO CommercialDocument ([BLNo], [InvoiceNo], [Shipper], [ShipperAddress], [Consignee], [ConsigneeAddress], [NotifyParty], [NotifyPartyAddress], [PortOfLoading]
	, [FinalDestination],[Carrier], [SailingOn], [PaymentTerms], [OtherReferences], [StampImg], [FOBorCNF], [VoyageNo], [VoyageCompany], [ContainerNo], [SealNo], [ContainerSize], [Registerd], [ClearanceDate], [StepCL])
	SELECT '" + NewBLNo + @"', [InvoiceNo], [Shipper], [ShipperAddress], [Consignee], [ConsigneeAddress], [NotifyParty], [NotifyPartyAddress], [PortOfLoading], [FinalDestination], [Carrier], [SailingOn], [PaymentTerms], [OtherReferences]
		, [StampImg], [FOBorCNF], [VoyageNo], [VoyageCompany], [ContainerNo], [SealNo], [ContainerSize], [Registerd], [ClearanceDate], [StepCL] FROM CommercialDocument WHERE CommercialDocumentHeadPk=" + CommercialDocumentPk + @" ;
	SELECT @@IDENTITY;";
		DB.DBCon.Open();
		string identity = DB.SqlCmd.ExecuteScalar() + "";
		DB.SqlCmd.CommandText = "UPDATE CommerdialConnectionWithRequest SET [CommercialDocumentPk] = " + identity + " WHERE [RequestFormPk] = " + RequestFormPk + ";";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String[] CheckDescriptionPageLoad(string CommercialDocumentHeadPk) {
		StringBuilder ReturnValue = new StringBuilder();
		DB = new DBConn();
		DB.DBCon.Open();
		string ClearanceDate = "";
		ReturnValue.Append(CheckDescriptionLoadCommercialDocument(CommercialDocumentHeadPk, ref ClearanceDate));
		ReturnValue.Append(CheckDescriptionLoadCO(CommercialDocumentHeadPk));
		ReturnValue.Append(CheckDescriptionLoadRequestForm(CommercialDocumentHeadPk, ClearanceDate));
		ReturnValue.Append(CheckDescriptionLoadTariffSUM(CommercialDocumentHeadPk));
		DB.DBCon.Close();
		return (ReturnValue + "").Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
	}

	private String CheckDescriptionLoadCO(string CommercialDocumentHeadPk) {
		DB.SqlCmd.CommandText = @"
SELECT	RF.DocumentRequestCL
FROM RequestForm AS RF
		inner join CommerdialConnectionWithRequest AS CCWR  ON RF.RequestFormPk=CCWR.RequestFormPk
WHERE CCWR.CommercialDocumentPk =" + CommercialDocumentHeadPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		bool IsCo = false;
		while (RS.Read()) {
			if ((RS[0] + "").IndexOf("10!") > -1) {
				IsCo = true;
				break;
			}
		}
		RS.Dispose();
		string returnvalue = "";
		if (IsCo) {
			DB.SqlCmd.CommandText = @"
SELECT CO.[PublicationName], CO.[PublicationAddress], CO.[BranchPk], CO.[TotalCount], CO.[IssueDate], CO.[Step], CO.[FilePk]
FROM [CommercialDocumentCO] AS CO
WHERE CO.[CommercialDocumentPk]=" + CommercialDocumentHeadPk + ";";
			RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				returnvalue = "%!$@#CO#@!" + RS["PublicationName"] + "#@!" + RS["PublicationAddress"] + "#@!" + RS["BranchPk"] + "#@!" + RS["TotalCount"] + "#@!" + RS["IssueDate"] + "#@!" + RS["Step"] + "#@!" + RS["FilePk"];
			}
			else {
				returnvalue = "%!$@#CO#@!" + "SHENZHEN ZHONGRUI TIANHONG COMMERCE CO.,LTD. CHINA" + "#@!" +
					"FROM SHENZHEN,CHINA PORT TO INCHEON,KOREA BY SEA" + "#@!" + "2886" + "#@!" + "" + "#@!" + "" + "#@!" + "" + "#@!" + "";
			}
			RS.Dispose();
		}
		return returnvalue;
	}

	private String CheckDescriptionLoadCommercialDocument(string CommercialDocumentHeadPk, ref string ClearanceDate) {
		string RVCommercialDocumentInfo = "";
		string Shipper = "";
		string ShipperAddress = "";
		string Consignee = "";
		string ConsigneeAddress = "";
		string StampImg = "";
		string TempInnerData1 = "";
		string TempInnerData2 = "";
		string InvoiceNo, NotifyParty, NotifyPartyAddress, PaymentTerms, OtherReferences, FOBorCNF;
		DB.SqlCmd.CommandText = @"
	SELECT
		[CommercialDocumentHeadPk], [BLNo], [InvoiceNo], [Shipper], [ShipperAddress],
		[Consignee], [ConsigneeAddress], [NotifyParty], [NotifyPartyAddress], [PortOfLoading],
		[FinalDestination], [Carrier], [SailingOn], [PaymentTerms], [OtherReferences],
		[StampImg], [FOBorCNF], [VoyageNo], [VoyageCompany], [ContainerNo],
		[SealNo], [ContainerSize], [ClearanceDate], [StepCL], MarksNNo, LCNo
	FROM CommercialDocument
	WHERE CommercialDocumentHeadPk=" + CommercialDocumentHeadPk + ";";

		//        DB.SqlCmd.CommandText = @"	SELECT CommercialDocumentHeadPk, BLNo, Shipper, ShipperAddress, Consignee, ConsigneeAddress, NotifyParty, NotifyPartyAddress, PaymentTerms, OtherReferences, FOBorCNF, ClearanceDate, StepCL, InvoiceNo
		//															FROM CommercialDocument
		//															WHERE CommercialDocumentHeadPk=" + CommercialDocumentHeadPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ClearanceDate = RS["ClearanceDate"] + "";
			if (RS["StepCL"] + "" == "") {
				RVCommercialDocumentInfo = "D#@!" + RS["CommercialDocumentHeadPk"] + "#@!" + RS["BLNo"];
				Shipper = RS["Shipper"] + "";
				ShipperAddress = RS["ShipperAddress"] + "";
				StampImg = RS["StampImg"] + "";
				Consignee = RS["Consignee"] + "";
				ConsigneeAddress = RS["ConsigneeAddress"] + "";
				TempInnerData1 = RS["PortOfLoading"] + "#@!" + RS["FinalDestination"] + "#@!" + RS["Carrier"] + "#@!" + RS["SailingOn"];
				TempInnerData2 = RS["VoyageNo"] + "#@!" + RS["VoyageCompany"] + "#@!" + RS["ContainerNo"] + "#@!" + RS["SealNo"] + "#@!" + RS["ContainerSize"];
			}
			else {
				RVCommercialDocumentInfo = "D#@!" + RS["CommercialDocumentHeadPk"] + "#@!" + RS["BLNo"] + "#@!" + RS["InvoiceNo"] + "#@!" + RS["Shipper"] + "#@!" +
																RS["ShipperAddress"] + "#@!" + RS["Consignee"] + "#@!" + RS["ConsigneeAddress"] + "#@!" + RS["NotifyParty"] + "#@!" + RS["NotifyPartyAddress"] + "#@!" +
																RS["PortOfLoading"] + "#@!" + RS["FinalDestination"] + "#@!" + RS["Carrier"] + "#@!" + RS["SailingOn"] + "#@!" + RS["PaymentTerms"] + "#@!" +
																RS["OtherReferences"] + "#@!" + RS["StampImg"] + "#@!" + RS["FOBorCNF"] + "#@!" +
																RS["VoyageNo"] + "#@!" + RS["VoyageCompany"] + "#@!" + RS["ContainerNo"] + "#@!" + RS["SealNo"] + "#@!" + RS["ContainerSize"] + "#@!" +
																RS["ClearanceDate"] + "#@!" + RS["MarksNNo"] + "#@!" + RS["LCNo"];
				RS.Dispose();
				return RVCommercialDocumentInfo;
			}
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = @"
			SELECT
				RF.ConsigneeCode, RF.DepartureDate, RF.PaymentWayCL
				,invo.PaymentTerms
				, RF.DocumentRequestCL, RF.NotifyPartyName, RF.NotifyPartyAddress
				, FOB.Price, FOB.MonetaryUnit, invo.invoiceno, invo.FOBorCNF ,invo.PaymentTerms,invo.NotifyParty, invo.NotifyPartyAddress as invoNotifyPartyAddress, invo.OtherReferences
				, RF.ExchangeDate, MarksNNo, LCNo
			FROM RequestForm AS RF
				inner join CommerdialConnectionWithRequest AS CCWR  ON RF.RequestFormPk=CCWR.RequestFormPk
                left join commercialdocument as invo on CCWR.commercialdocumentpk = invo.CommercialDocumentHeadPk
				left join (
							SELECT RFCH.[TABLE_PK], RFCH.[CUSTOMER_COMPANY_PK], RFCB.[REQUESTFORMCALCULATE_HEAD_PK], RFCB.[ORIGINAL_PRICE] AS Price, RFCB.[ORIGINAL_MONETARY_UNIT] AS MonetaryUnit
							FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH
							LEFT JOIN [dbo].[RequestForm] AS R ON RFCH.[TABLE_PK] = R.[RequestFormPk]
							LEFT JOIN [dbo].[REQUESTFORMCALCULATE_BODY] AS RFCB ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK] = RFCB.[REQUESTFORMCALCULATE_HEAD_PK]
							WHERE RFCH.[TABLE_NAME] = 'RequestForm'
							AND R.[ConsigneePk] = RFCH.[CUSTOMER_COMPANY_PK]
							AND RFCB.[CATEGORY] = '해운비'
							AND RFCB.[ORIGINAL_PRICE] != 0
							) AS FOB ON FOB.TABLE_PK=RF.RequestFormPk
				WHERE CCWR.CommercialDocumentPk=" + CommercialDocumentHeadPk + "";
		RS = DB.SqlCmd.ExecuteReader();
		OtherReferences = "";
		NotifyParty = "";
		NotifyPartyAddress = "";
		PaymentTerms = "";
		FOBorCNF = "";
		InvoiceNo = "";
		string MarksNNo = "";
		string LCNo = "";
		string[] ForExchange_Date = new string[] { "" };
		string ForExchange_Price = "";
		string ForExchage_MonetaryUnit = "";
		string tempExchangedDate = "";
		if (RS.Read()) {
			if (RS["PaymentTerms"] + "" != "") {
				PaymentTerms = RS["PaymentTerms"] + "";
			}
			else {
				PaymentTerms = RS["PaymentWayCL"] + "" == "3" ? "L / C" : "T / T";
			}
			if (RS["NotifyParty"] + "" != "") {
				NotifyParty = RS["NotifyParty"] + "";
			}
			else {
				if (RS["NotifyPartyName"] + "" == "") {
					NotifyParty = "SAME AS ABOVE";
					NotifyPartyAddress = "";
				}
				else {
					NotifyParty = RS["NotifyPartyName"] + "";
					NotifyPartyAddress = RS["NotifyPartyAddress"] + "";
				}
			}
			MarksNNo = RS["MarksNNo"] + "";
			LCNo = RS["LCNo"] + "";
			if (RS["invoNotifyPartyAddress"] + "" != "") {
				NotifyPartyAddress = RS["invoNotifyPartyAddress"] + "";
			}
			else {
				if (RS["NotifyPartyName"] + "" == "") {
					NotifyPartyAddress = "";
				}
				else {
					NotifyPartyAddress = RS["NotifyPartyAddress"] + "";
				}
			}

			if (RS["OtherReferences"] + "" == "") {
				if (RS["DocumentRequestCL"] + "" != "") {
					string[] temp = (RS["DocumentRequestCL"] + "").Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
					foreach (string tempeach in temp) {
						switch (tempeach) {
							case "10":
								OtherReferences += "C/O\n";
								break;

							case "31":
								OtherReferences += "C/O\n";
								break;

							case "32":
								OtherReferences += "C/O\n";
								break;
							case "34":
								OtherReferences += "FTA C/O\n";
								break;

							case "11":
								OtherReferences += "식품위생허가증\n";
								break;

							case "12":
								OtherReferences += "전기안전승인\n";
								break;

							case "24":
								OtherReferences += "상검\n";
								break;

							case "25":
								OtherReferences += "수책\n";
								break;

							default:
								break;
						}
					}
				}
			}
			else {
				OtherReferences = RS["OtherReferences"] + "";
			}

			// 20130429 FOBorCNF 저장 안되서 수정
			if (RS["FOBorCNF"] + "" == "") {
				if (RS["Price"] + "" == "") {
					FOBorCNF = "CNF#@!#@!";
				}
				else if (RS["MonetaryUnit"] + "" == "20") {
					FOBorCNF = "FOB#@!20#@!" + Common.NumberFormat(RS["Price"] + "");
				}
				else {
					FOBorCNF = "FOB#@!" + RS["MonetaryUnit"] + "#@!" + Common.NumberFormat(RS["Price"] + "");
				}
				ForExchange_Price = RS["Price"] + "";
				ForExchage_MonetaryUnit = RS["MonetaryUnit"] + "";
			}
			else {
				FOBorCNF = RS["FOBorCNF"] + "";
				string[] arrFOBCNF = FOBorCNF.Split(new string[] { "#@!" }, StringSplitOptions.None);
				ForExchange_Price = arrFOBCNF[2];
				ForExchage_MonetaryUnit = arrFOBCNF[1];
			}
			DateTime departure = new DateTime(Int32.Parse((RS["DepartureDate"] + "").Substring(0, 4)), Int32.Parse((RS["DepartureDate"] + "").Substring(4, 2)), Int32.Parse((RS["DepartureDate"] + "").Substring(6, 2)));
			if (RS["InvoiceNo"] + "" != "") {
				InvoiceNo = RS["InvoiceNo"] + "";
			}
			else {
				InvoiceNo = RS["ConsigneeCode"] + " " + departure.Date.ToString("d, MMM, yyyy");
			}
			tempExchangedDate = RS["ExchangeDate"] + "";
		}
		RS.Dispose();

		decimal tempExchangedAmount = 0;
		string AAAAAAA = "";

		if (ForExchange_Price == "") {
			tempExchangedAmount = 0;
		}
		else {
			tempExchangedAmount = Math.Truncate(new Admin().GetExchangeRated(ForExchage_MonetaryUnit, "20", decimal.Parse(ForExchange_Price), out AAAAAAA, tempExchangedDate));
		}

		//RVCommercialDocumentInfo += "#@!" + NotifyParty + "#@!" + NotifyPartyAddress + "#@!" + PaymentTerms + "#@!" + OtherReferences + "#@!" + FOBorCNF + "#@!" + InvoiceNo;

		/*
                                                                    RS["PortOfLoading"] + "#@!" + RS["FinalDestination"] + "#@!" + RS["Carrier"] + "#@!" + RS["SailingOn"] + "#@!" + RS["PaymentTerms"] + "#@!" +
                                                                    RS["OtherReferences"] + "#@!" + RS["StampImg"] + "#@!" + RS["FOBorCNF"] + "#@!" +
                                                                    RS["VoyageNo"] + "#@!" + RS["VoyageCompany"] + "#@!" + RS["ContainerNo"] + "#@!" + RS["SealNo"] + "#@!" + RS["ContainerSize"] + "#@!" +
                                                                    RS["ClearanceDate"];
        */
		return RVCommercialDocumentInfo + "#@!" + InvoiceNo + "#@!" + Shipper + "#@!" +
				ShipperAddress + "#@!" + Consignee + "#@!" + ConsigneeAddress + "#@!" + NotifyParty + "#@!" + NotifyPartyAddress + "#@!" +
				TempInnerData1 + "#@!" + PaymentTerms + "#@!" +
				OtherReferences + "#@!" + StampImg + "#@!" + FOBorCNF + "#@!" +
				TempInnerData2 + "#@!" + ClearanceDate + "#@!" + "20" + "#@!" + Common.NumberFormat(tempExchangedAmount.ToString()) + "#@!" + MarksNNo + "#@!" + LCNo;
	}

	private String CheckDescriptionLoadRequestForm(string CommercialDocumentHeadPk, string ClearanceDate) {
		string tempRequestFormPk = "";
		StringBuilder RequestFormList = new StringBuilder();
		StringBuilder ItemList = new StringBuilder();
		DB.SqlCmd.CommandText = @"
SELECT	RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.ShipperCode, RF.ConsigneeCode, RF.DepartureDate, RF.ArrivalDate, RF.TransportWayCL, RF.DocumentRequestCL, RF.MonetaryUnitCL, RF.StepCL, RF.Memo
		, SC.CompanyName AS SCompanyName, SC.CompanyTEL AS SCompanyTEL, SC.CompanyFAX AS SCompanyFAX
		, CC.CompanyName AS CCompanyName, CC.CompanyTEL AS CCompanyTEL, CC.CompanyFAX AS CCompanyFAX
		, Departure.NameE
		, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume
		, RI.RequestFormItemsPk, RI.ItemCode, RI.MarkNNumber, RI.Description, RI.Label, RI.Material, RI.Quantity, RI.QuantityUnit, RI.PackedCount, RI.PackingUnit, RI.GrossWeight, RI.NetWeight, RI.Volume, RI.UnitPrice, RI.Amount, RI.RAN, RI.HSCodeForCO
		, CICK.Description AS CDescription, CICK.Material AS CMaterial, CICK.TarriffRate, CICK.AdditionalTaxRate ,CICK.HSCode,CICK.Law_Nm
FROM RequestForm AS RF
		inner join CommerdialConnectionWithRequest AS CCWR  ON RF.RequestFormPk=CCWR.RequestFormPk
		Left join Company AS SC on RF.ShipperPk=SC.CompanyPk
		left join Company AS CC on RF.ConsigneePk=CC.CompanyPk
		Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode
		Left join RequestFormItems as RI on RF.RequestFormPk=RI.RequestFormPk
		left join ClearanceItemCodeKOR AS CICK ON RI.ItemCode=CICK.ItemCode
WHERE CCWR.CommercialDocumentPk =" + CommercialDocumentHeadPk + @"
ORDER BY RI.RAN;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (tempRequestFormPk != RS["RequestFormPk"] + "") {
				RequestFormList.Append("%!$@#R#@!" + RS["RequestFormPk"] + "#@!" + RS["ShipperPk"] + "#@!" + RS["ConsigneePk"] + "#@!" + RS["ShipperCode"] + "#@!" + RS["ConsigneeCode"] + "#@!" + RS["DepartureDate"] + "#@!" +
					RS["ArrivalDate"] + "#@!" +
					Common.GetTransportWay(RS["TransportWayCL"] + "") + "#@!" +
					RS["StepCL"] + "#@!" +
					RS["Memo"] + "#@!" +
					RS["SCompanyName"] + "#@!" +
					RS["SCompanyTEL"] + "#@!" +
					RS["SCompanyFAX"] + "#@!" +
					RS["CCompanyName"] + "#@!" +
					RS["CCompanyTEL"] + "#@!" +
					RS["CCompanyFAX"] + "#@!" +
					RS["NameE"] + "#@!" +
					RS["TotalPackedCount"] + "#@!" +
					RS["PackingUnit"] + "#@!" +
					RS["TotalGrossWeight"] + "#@!" +
					ClearanceDate);
				tempRequestFormPk = RS["RequestFormPk"] + "";
			}
			string netweight = "";
			if (RS["NetWeight"] + "" == "" && RS["GrossWeight"] + "" != "" && RS["PackedCount"] + "" != "") {
				netweight = Common.NumberFormat(decimal.Parse(RS["GrossWeight"] + "") - decimal.Parse(RS["PackedCount"] + "") + "");
			}
			else {
				netweight = Common.NumberFormat("" + RS["NetWeight"]);
			}
			ItemList.Append("%!$@#I#@!" +
				RS["RequestFormItemsPk"] + "#@!" +
				RS["RequestFormPk"] + "#@!" +
				RS["ItemCode"] + "#@!" +
				RS["MarkNNumber"] + "#@!" +
				RS["Description"].ToString().Replace("'", "") + "#@!" +
				RS["Label"] + "#@!" +
				RS["Material"] + "#@!" +
				Common.NumberFormat("" + RS["Quantity"]) + "#@!" +
				Common.NumberFormat("" + RS["QuantityUnit"]) + "#@!" +
				RS["PackedCount"] + "#@!" +
				RS["PackingUnit"] + "#@!" +
				Common.NumberFormat("" + RS["GrossWeight"]) + "#@!" +
				Common.NumberFormat("" + RS["Volume"]) + "#@!" +
				Common.GetMonetaryUnit("" + RS["MonetaryUnitCL"]) + "#@!" +
				Common.NumberFormat("" + RS["UnitPrice"]) + "#@!" +
				Common.NumberFormat("" + RS["Amount"]) + "#@!" +
				RS["MonetaryUnitCL"] + "#@!" +
				RS["CDescription"] + "#@!" +
				RS["CMaterial"] + "#@!" +
				RS["TarriffRate"] + "#@!" +
				RS["AdditionalTaxRate"] + "#@!" +
				netweight + "#@!" +
				RS["RAN"] + "#@!" +
				RS["HSCodeForCO"] + "#@!" +
				RS["HSCode"] + "#@!" +
				RS["Law_Nm"]
				);
		}
		RS.Dispose();
		return RequestFormList + "" + ItemList;
	}

	private String CheckDescriptionLoadTariffSUM(string CommercialDocumentHeadPk) {
		DB.SqlCmd.CommandText = @"
		SELECT ISNULL(RFCB.[TITLE], '0') AS TITLE, ISNULL(RFCB.[ORIGINAL_MONETARY_UNIT], '20') AS MONETARY_UNIT, ISNULL(RFCB.[ORIGINAL_PRICE], '0') AS PRICE
		FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH
		LEFT JOIN [dbo].[CommerdialConnectionWithRequest] AS CCWR ON RFCH.[TABLE_PK] = CCWR.[RequestFormPk]
		INNER JOIN ( 
					SELECT [REQUESTFORMCALCULATE_HEAD_PK], [REQUESTFORMCALCULATE_BODY_PK], [TITLE], [ORIGINAL_MONETARY_UNIT], [ORIGINAL_PRICE] 
					FROM [dbo].[REQUESTFORMCALCULATE_BODY] 
					WHERE [TITLE] IN ('관세', '부가세', '관세사비')
					) AS RFCB ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK] = RFCB.[REQUESTFORMCALCULATE_HEAD_PK]
		WHERE RFCH.[TABLE_NAME] = 'RequestForm'
		AND CCWR.[CommercialDocumentPk] = " + CommercialDocumentHeadPk;
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder TariffSum = new StringBuilder();
		while (RS.Read()) {
			TariffSum.Append("%!$@#T#@!" + RS[0] + "#@!" + RS[1] + "#@!" + Common.NumberFormat(RS[2] + ""));
		}
		return TariffSum + "";
	}

	//BMK 통관
	[WebMethod]
	public string CalcTarriffKOR_FTA(string HsCode, string MonetaryUnit, string Charge) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = string.Format(@"
SELECT 
  D.TariffRate0 DTariff
, C.TariffRate0 CTariff
, E1.TariffRate0 E1Tariff
, F2015.UseLimit_Description
, F2015.TariffRate0 F2015Tariff
, F2016.TariffRate0 F2016Tariff
, F2017.TariffRate0 F2017Tariff
, F2018.TariffRate0 F2018Tariff
, F2019.TariffRate0 F2019Tariff
FROM[INTL2010].[dbo].[Lib_HSCode] L
left join(select * from[dbo].[Lib_HSCode_TariffRate] where kind = '') D on L.Lib_HSCodePk = D.Lib_HSCodePk
left join (select * from[dbo].[Lib_HSCode_TariffRate] where kind = 'C') C on L.Lib_HSCodePk = C.Lib_HSCodePk
left join (select * from[dbo].[Lib_HSCode_TariffRate] where kind = 'E1') E1 on L.Lib_HSCodePk = E1.Lib_HSCodePk
left join (SELECT * FROM[dbo].[Lib_HSCode_TariffRate] where Kind = 'F' and UseLimit_Date = '2015') F2015 on L.Lib_HSCodePk = F2015.Lib_HSCodePk
left join (SELECT * FROM[dbo].[Lib_HSCode_TariffRate] where Kind = 'F' and UseLimit_Date = '2016') F2016 on L.Lib_HSCodePk = F2016.Lib_HSCodePk
left join (SELECT * FROM[dbo].[Lib_HSCode_TariffRate] where Kind = 'F' and UseLimit_Date = '2017') F2017 on L.Lib_HSCodePk = F2017.Lib_HSCodePk
left join (SELECT * FROM[dbo].[Lib_HSCode_TariffRate] where Kind = 'F' and UseLimit_Date = '2018') F2018 on L.Lib_HSCodePk = F2018.Lib_HSCodePk
left join (SELECT * FROM[dbo].[Lib_HSCode_TariffRate] where Kind = 'F' and UseLimit_Date = '2019') F2019 on L.Lib_HSCodePk = F2019.Lib_HSCodePk
where [HSCode] like '%{0}%'", HsCode.Trim());

		List<string> TariffRate = new List<string>();
		decimal TotalGuanse = 0;
		decimal TotalBugase = 0;
		decimal guan = 0;
		decimal buga = 0;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			TariffRate.Add(RS["DTariff"] + "");
			TariffRate.Add(RS["CTariff"] + "");
			TariffRate.Add(RS["E1Tariff"] + "");
			TariffRate.Add(RS["F2016Tariff"] + "");
			TariffRate.Add(RS["F2017Tariff"] + "");
			TariffRate.Add(RS["F2018Tariff"] + "");
			TariffRate.Add(RS["F2019Tariff"] + "");
		}
		RS.Dispose();
		DB.DBCon.Close();
		StringBuilder ReturnValue = new StringBuilder();
		DB.DBCon.Open();
		for (int i = 0; i < TariffRate.Count; i++) {
			if (TariffRate[i] != "") {


				decimal TotalCharge = ExchangeCharge("2", DateTime.Now.ToString("yyyyMMdd"), MonetaryUnit, "20", decimal.Parse(Charge));

				TotalGuanse = Math.Truncate((TotalCharge * decimal.Parse(TariffRate[i]) / 100));
				TotalBugase = Math.Truncate((TotalCharge + TotalGuanse) * 10 / 100);
				guan = Math.Truncate(TotalGuanse / 10) * 10;
				buga = Math.Truncate(TotalBugase / 10) * 10;
				if (guan + buga < 10000) {
					ReturnValue.Append("#@!" + "0");
					ReturnValue.Append("#@!" + "0");
				}
				else {
					ReturnValue.Append("#@!" + guan + "");
					ReturnValue.Append("#@!" + buga + "");
				}
			}
			else {
				ReturnValue.Append("#@!" + "x");
				ReturnValue.Append("#@!" + "x");
			}
		}
		DB.DBCon.Close();
		return ReturnValue.ToString().Substring(3);
	}
	/// <summary>
	/// 한국 관부가세 계산
	/// </summary>
	/// <param name="ClearanceDate">20110425 <= 이양식으로 통관예정일</param>
	/// <param name="MonetaryUnitFreightCharge">MonetaryUnitCL <= 운임 화폐단위</param>
	/// <param name="FreightCharge">CNF 운임</param>
	/// <returns></returns>
	[WebMethod]
	public String[] CalcTarriffKOR(string ClearanceDate, string MonetaryUnitFreightCharge, string FreightCharge, string MonetaryUnitItemPrice, string ItemPriceSum, string ItemGuanseRate, string ItemBugaseRate) {
		String[] ArrItemPrice = ItemPriceSum.Split(Common.Splite321, StringSplitOptions.None);
		String[] ArrItemGuanseRate = ItemGuanseRate.Split(Common.Splite321, StringSplitOptions.None);
		String[] ArrItemBugaseRate = ItemBugaseRate.Split(Common.Splite321, StringSplitOptions.None);
		decimal TotalGuanse = 0;
		decimal TotalBugase = 0;
		Decimal TotalItemPrice = 0;
		DB = new DBConn();
		bool IsAllSameGuanseBugase = true;
		string GuanseTemp = "";
		string BugaseTemp = "";
		for (int i = 0; i < ArrItemGuanseRate.Length; i++) {
			if (i == 0) {
				GuanseTemp = ArrItemGuanseRate[i];
				BugaseTemp = ArrItemBugaseRate[i];
			}
			if (GuanseTemp == ArrItemGuanseRate[i] && BugaseTemp == ArrItemBugaseRate[i]) {
				continue;
			}
			else {
				IsAllSameGuanseBugase = false;
				break;
			}
		}

		if (IsAllSameGuanseBugase) {
			for (int i = 0; i < ArrItemPrice.Length; i++) {
				TotalItemPrice += decimal.Parse(ArrItemPrice[i]);
			}//물품대금 합계
			DB.DBCon.Open();

			decimal Total = ExchangeCharge("2", ClearanceDate, MonetaryUnitFreightCharge, "20", decimal.Parse(FreightCharge)) + ExchangeCharge("2", ClearanceDate, MonetaryUnitItemPrice, "20", TotalItemPrice);
			DB.DBCon.Close();
			TotalGuanse = Math.Truncate(Total * decimal.Parse(ArrItemGuanseRate[0]) / 100);
			TotalBugase = Math.Truncate((Total + TotalGuanse) * 10 / 100);
		}
		else {
			for (int i = 0; i < ArrItemPrice.Length; i++) {
				TotalItemPrice += decimal.Parse(ArrItemPrice[i]);
			}//물품대금 합계
			Decimal FreightExchanged = 0;
			Decimal ItemPriceExchanged = 0;
			DB.DBCon.Open();
			if (MonetaryUnitFreightCharge != "20" && FreightCharge != "0") {
				FreightExchanged = ExchangeCharge("2", ClearanceDate, MonetaryUnitFreightCharge, "20", decimal.Parse(FreightCharge));
			}
			else {
				FreightExchanged = decimal.Parse(FreightCharge);
			}

			if (MonetaryUnitItemPrice != "20" && TotalItemPrice != 0) {
				ItemPriceExchanged = ExchangeCharge("2", ClearanceDate, MonetaryUnitItemPrice, "20", TotalItemPrice);
			}
			else {
				ItemPriceExchanged = TotalItemPrice;
			}
			DB.DBCon.Close();
			decimal TotalSUM = Math.Truncate(FreightExchanged + ItemPriceExchanged);
			decimal DanwiValue = Math.Round((TotalSUM / TotalItemPrice), 10, MidpointRounding.AwayFromZero);

			List<decimal> EachGuanse = new List<decimal>();
			List<decimal> EachBugase = new List<decimal>();

			for (int i = 0; i < ArrItemPrice.Length; i++) {
				decimal tempPrice = Math.Truncate(decimal.Parse(ArrItemPrice[i]) * DanwiValue);     //물건값
				EachGuanse.Add(Math.Truncate(tempPrice * decimal.Parse(ArrItemGuanseRate[i]) / 100));
				EachBugase.Add(Math.Truncate((tempPrice + EachGuanse[i]) * 10 / 100));
				TotalGuanse += EachGuanse[i];
				TotalBugase += EachBugase[i];
			}
		}
		decimal guan = Math.Truncate(TotalGuanse / 10) * 10;
		decimal buga = Math.Truncate(TotalBugase / 10) * 10;
		if (guan + buga < 10000) {
			return new String[] { "0", "0" };
		}
		else {
			return new String[] { guan + "", buga + "" };
		}
	}

	[WebMethod]
	public String[] CalcTarriffKOR2(string ClearanceDate, string MonetaryUnitFreightCharge, string FreightCharge, string MonetaryUnitItemPrice, string ItemPriceSum, string ItemGuanseRate, string ItemBugaseRate, string RAN) {
		String[] ArrItemPrice = ItemPriceSum.Split(Common.Splite321, StringSplitOptions.None);
		String[] ArrItemGuanseRate = ItemGuanseRate.Split(Common.Splite321, StringSplitOptions.None);
		String[] ArrItemBugaseRate = ItemBugaseRate.Split(Common.Splite321, StringSplitOptions.None);
		String[] ArrItemRAN = RAN.Split(Common.Splite321, StringSplitOptions.None);

		decimal TotalGuanse = 0;
		decimal TotalBugase = 0;
		Decimal TotalItemPrice = 0;
		DB = new DBConn();

		decimal[] EachRANItemPriceSUM = new Decimal[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		string[] EachRANGuanseRate = new String[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
		string[] EachRANBugaseRate = new String[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

		for (int i = 0; i < ArrItemPrice.Length; i++) {
			EachRANItemPriceSUM[(Int32.Parse(ArrItemRAN[i]) - 1)] += decimal.Parse(ArrItemPrice[i]);
		}

		bool IsAllSameGuanseBugase = true;
		string GuanseTemp = "";
		string BugaseTemp = "";
		for (int i = 0; i < ArrItemGuanseRate.Length; i++) {
			if (i == 0) {
				GuanseTemp = ArrItemGuanseRate[i];
				BugaseTemp = ArrItemBugaseRate[i];
			}
			if (GuanseTemp == ArrItemGuanseRate[i] && BugaseTemp == ArrItemBugaseRate[i]) {
				continue;
			}
			else {
				IsAllSameGuanseBugase = false;
				break;
			}
		}

		if (IsAllSameGuanseBugase) {
			for (int i = 0; i < ArrItemPrice.Length; i++) {
				TotalItemPrice += decimal.Parse(ArrItemPrice[i]);
			}//물품대금 합계
			DB.DBCon.Open();
			decimal Total = ExchangeCharge("2", ClearanceDate, MonetaryUnitFreightCharge, "20", decimal.Parse(FreightCharge)) + ExchangeCharge("2", ClearanceDate, MonetaryUnitItemPrice, "20", TotalItemPrice);
			DB.DBCon.Close();
			TotalGuanse = Math.Truncate(Total * decimal.Parse(ArrItemGuanseRate[0]) / 100);
			TotalBugase = Math.Truncate((Total + TotalGuanse) * 10 / 100);
		}
		else {
			for (int i = 0; i < ArrItemPrice.Length; i++) {
				TotalItemPrice += decimal.Parse(ArrItemPrice[i]);
			}//물품대금 합계
			Decimal FreightExchanged = 0;
			Decimal ItemPriceExchanged = 0;
			DB.DBCon.Open();
			if (MonetaryUnitFreightCharge != "20" && FreightCharge != "0") {
				FreightExchanged = ExchangeCharge("2", ClearanceDate, MonetaryUnitFreightCharge, "20", decimal.Parse(FreightCharge));
			}
			else {
				FreightExchanged = decimal.Parse(FreightCharge);
			}

			if (MonetaryUnitItemPrice != "20" && TotalItemPrice != 0) {
				ItemPriceExchanged = ExchangeCharge("2", ClearanceDate, MonetaryUnitItemPrice, "20", TotalItemPrice);
			}
			else {
				ItemPriceExchanged = TotalItemPrice;
			}
			DB.DBCon.Close();
			decimal TotalSUM = Math.Truncate(FreightExchanged + ItemPriceExchanged);
			decimal DanwiValue = Math.Round((TotalSUM / TotalItemPrice), 10, MidpointRounding.AwayFromZero);

			List<decimal> EachGuanse = new List<decimal>();
			List<decimal> EachBugase = new List<decimal>();

			for (int i = 0; i < ArrItemPrice.Length; i++) {
				decimal tempPrice = Math.Truncate(decimal.Parse(ArrItemPrice[i]) * DanwiValue);     //물건값
				EachGuanse.Add(Math.Truncate(tempPrice * decimal.Parse(ArrItemGuanseRate[i]) / 100));
				EachBugase.Add(Math.Truncate((tempPrice + EachGuanse[i]) * 10 / 100));
				TotalGuanse += EachGuanse[i];
				TotalBugase += EachBugase[i];
			}
		}
		return new String[] { (Math.Truncate(TotalGuanse / 10) * 10) + "", (Math.Truncate(TotalBugase / 10) * 10) + "" };
	}

	[WebMethod]
	public string ExchangeRateHistory_nullCheck(string ETCSettingPk, string Date) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "	SELECT Count(*) as Count" +
								"	FROM ExchangeRateHistory " +
								"	WHERE ETCSettingPk=" + ETCSettingPk + " and left([DateSpan],8)<='" + Date + "' and right([DateSpan],8)>='" + Date + "';";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string Check = "";
		if (RS.Read()) {
			Check = RS["Count"] + "";
		}
		RS.Close();
		DB.DBCon.Close();
		return Check;
	}

	private Decimal ExchangeCharge(string ETCSettingPk, string Date, string MonetaryUnitFrom, string MonetaryUnitTo, decimal From) {
		if (MonetaryUnitFrom == MonetaryUnitTo) {
			return From;
		}
		decimal To = 0;
		DB.SqlCmd.CommandText = "	SELECT [ExchangeRatePk], [ExchangeRateStandard], [MonetaryUnitFrom], [MonetaryUnitTo], [ExchangeRate] " +
														"	FROM ExchangeRateHistory " +
														"	WHERE ETCSettingPk=" + ETCSettingPk + " and left([DateSpan],8)<='" + Date + "' and right([DateSpan],8)>='" + Date + "' and MonetaryUnitFrom in (" + MonetaryUnitFrom + ", " + MonetaryUnitTo + ") and MonetaryUnitTo in (" + MonetaryUnitFrom + ", " + MonetaryUnitTo + ");";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			decimal exchangeratestandard = decimal.Parse(RS["ExchangeRateStandard"] + "");
			decimal exchangerate = decimal.Parse(RS["ExchangeRate"] + "");
			if (RS[2] + "" == MonetaryUnitFrom) {
				To = From / exchangeratestandard * exchangerate;
			}
			else {
				To = From / exchangerate * exchangeratestandard;
			}
		}
		RS.Dispose();
		return To;
	}

	//BMK CommercialItemHistory.aspx
	[WebMethod]
	public String InsertClearanceItemCodeKOR(string Description, string Material, string TarriffRate, string AdditionalTaxRate, string CompanyPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "INSERT INTO ClearanceItemCodeKOR ([Description], [Material], [TarriffRate], [AdditionalTaxRate], [CompanyPk]) " +
																"	VALUES (" + Common.StringToDB(Description, true, false) + ", " + Common.StringToDB(Material, true, false) + ", " + Common.StringToDB(TarriffRate, false, false) + ", " + Common.StringToDB(AdditionalTaxRate, false, false) + ", " + CompanyPk + ");" +
																" SELECT @@IDENTITY;";
		DB.DBCon.Open();
		string ItemCode = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		return ItemCode;
	}

	[WebMethod]
	public String readyTOintl2000_Law(string HSCode, string ItemCode) {
		string TempHSCode = HSCode.Replace(".", "").Replace("-", "").Trim();
		List<string> Law_Nm = new List<string>();
		DBConn DB_Ready = new DBConn("ReadyKorea");
		DB_Ready.SqlCmd.CommandText = @"SELECT  [Law_Nm]
  FROM [edicus].[dbo].[CDHSB2]
  WHERE HS='" + TempHSCode + @"';";
		DB_Ready.DBCon.Open();
		SqlDataReader RS = DB_Ready.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			Law_Nm.Add(RS["Law_Nm"].ToString());
		}
		DB_Ready.DBCon.Close();
		DB = new DBConn();
		StringBuilder Query = new StringBuilder();

		for (int i = 0; i < Law_Nm.Count; i++) {
			Query.Append(@"UPDATE [dbo].[ClearanceItemCodeKOR]
   SET [Law_Nm] = " + Common.StringToDB(Law_Nm[i], true, true) + @"
 WHERE [ItemCode]='" + ItemCode + "';");
		}
		if (Query.ToString() != "") {
			DB.SqlCmd.CommandText = Query.ToString();
			DB.DBCon.Open();
			DB.SqlCmd.ExecuteScalar();
			DB.DBCon.Close();
		}
		return "1";
	}

	[WebMethod]
	public String InsertClearanceItemCodeKOR_HSCode(string HItemCode, string HSCode, string Description, string RANDescription, string RANTradingDescription, string Material, string TarriffRate, string AdditionalTaxRate, string FCN1, string E1, string C, string Law_Nm, string CompanyPk) {
		DB = new DBConn();
		if (HItemCode == "") {
			DB.SqlCmd.CommandText = @"INSERT INTO ClearanceItemCodeKOR ([HSCode],[Description],[RANTradingDescription],[RANDescription],[Material],[TarriffRate],[AdditionalTaxRate],[FCN1],[E1],[C],[Law_Nm],[CompanyPk]) " +
									"	VALUES (" + Common.StringToDB(HSCode, true, true) + ", " + Common.StringToDB(Description, true, true) + ", " + Common.StringToDB(RANDescription, true, true) + ", " + Common.StringToDB(RANTradingDescription, true, true) + ", " + Common.StringToDB(Material, true, true) + ", " + Common.StringToDB(TarriffRate, true, true) + ", " + Common.StringToDB(AdditionalTaxRate, true, true) + ", " + Common.StringToDB(FCN1, true, true) + ", " + Common.StringToDB(E1, true, true) + ", " + Common.StringToDB(C, true, true) + ", " + Common.StringToDB(Law_Nm, true, true) + ", " + CompanyPk + "); SELECT @@IDENTITY;";
		}
		else {
			DB.SqlCmd.CommandText = @"UPDATE [dbo].[ClearanceItemCodeKOR]
   SET [HSCode] = " + Common.StringToDB(HSCode, true, true) + @"
      ,[Description] = " + Common.StringToDB(Description, true, true) + @"
	  ,[RANDescription] = " + Common.StringToDB(RANDescription, true, true) + @"
      ,[RANTradingDescription] = " + Common.StringToDB(RANTradingDescription, true, true) + @"
      ,[Material] = " + Common.StringToDB(Material, true, true) + @"
      ,[TarriffRate] = " + Common.StringToDB(TarriffRate, true, true) + @"
      ,[AdditionalTaxRate] = " + Common.StringToDB(AdditionalTaxRate, true, true) + @"
	  ,[FCN1] = " + Common.StringToDB(FCN1, true, true) + @"
      ,[E1] = " + Common.StringToDB(E1, true, true) + @"
	  ,[C] = " + Common.StringToDB(C, true, true) + @"
	  ,[Law_Nm] = " + Common.StringToDB(Law_Nm, true, true) + @"
      ,[CompanyPk] = " + CompanyPk + @"
 WHERE [ItemCode]=" + HItemCode + ";";
		}

		DB.DBCon.Open();
		string ItemCode = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		return ItemCode;
	}

	[WebMethod]
	public String DeleteClearanceItemCodeKOR_All(string CompanyPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE ClearanceItemCodeKOR SET Deleted=getdate()  WHERE CompanyPk=" + CompanyPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String DeleteClearanceItemCodeKOR(string Pk) {
		DB = new DBConn();
		//DB.SqlCmd.CommandText = "UPDATE ClearanceItemCodeKOR SET [CompanyPk] = 0 WHERE ItemCode=" + Pk + ";";
		DB.SqlCmd.CommandText = "UPDATE ClearanceItemCodeKOR SET Deleted=getdate()  WHERE ItemCode=" + Pk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String SetTarriffKor(string CommercialDocumentHeadPk, string RequestFormPk, string ConsigneeCompanyPk, string InvoiceNo,
											string ShipperName, string ShipperAddress, string ConsigneeName, string ConsigneeAddress, string PaymentTerms,
											string FOBNCNF, string OtherRefferance, string NotifyParty, string NotifyPartyAddress, string StepCL,
											string ClearanceDate, string StampImg, string ItemSUM, string TariffSUM, string AccountID,
											string PortOfLoading, string FinalDestination, string Carrier, string SailingOn, string VoyageNo, string VoyageCompany, string ContainerNo, string SealNo, string ContainerSize, string COData, string CDShipperNamePk, string CDConsigneeNamePk, string MarksNNo, string LCNo) {
		DB = new DBConn();
		DB.DBCon.Open();
		StringBuilder Query = new StringBuilder();
		if (CDShipperNamePk != "") {
			Query.Append("UPDATE [dbo].[RequestForm] SET [ShipperClearanceNamePk] =  " + CDShipperNamePk + "  WHERE [RequestFormPk]=" + RequestFormPk + ";");
		}
		if (CDConsigneeNamePk != "") {
			Query.Append("UPDATE [dbo].[RequestForm] SET [ConsigneeClearanceNamePk] =  " + CDConsigneeNamePk + "  WHERE [RequestFormPk]=" + RequestFormPk + ";");
		}

		if (COData != "N") {
			string[] codata = COData.Split(Common.Splite321, StringSplitOptions.None);
			Query.Append("DELETE FROM CommercialDocumentCO WHERE CommercialDocumentPk=" + CommercialDocumentHeadPk + ";" +
				"INSERT INTO [CommercialDocumentCO] ([CommercialDocumentPk], [PublicationName], [PublicationAddress], [BranchPk], [TotalCount], [IssueDate]) VALUES (" +
					CommercialDocumentHeadPk + ", " +
					Common.StringToDB(codata[0], true, false) + ", " +
					Common.StringToDB(codata[1], true, false) + ", " +
					Common.StringToDB(codata[2], false, false) + ", " +
					Common.StringToDB(codata[3], true, false) + ", " +
					Common.StringToDB(codata[4], true, false) + ")");
		}
		Query.Append(
					@"DELETE FROM [REQUESTFORMCALCULATE_BODY] WHERE REQUESTFORMCALCULATE_BODY_PK IN 
						( 
							SELECT RFCB.[REQUESTFORMCALCULATE_BODY_PK]
							FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH
							LEFT JOIN [dbo].[CommerdialConnectionWithRequest] AS CCWR ON RFCH.[TABLE_PK] = CCWR.[RequestFormPk]
							INNER JOIN ( 
											SELECT [REQUESTFORMCALCULATE_HEAD_PK], [REQUESTFORMCALCULATE_BODY_PK], [TITLE], [ORIGINAL_MONETARY_UNIT], [ORIGINAL_PRICE] 
											FROM [dbo].[REQUESTFORMCALCULATE_BODY] 
											WHERE [TITLE] IN ('관세', '부가세', '관세사비')
										) AS RFCB ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK] = RFCB.[REQUESTFORMCALCULATE_HEAD_PK]
							WHERE RFCH.[TABLE_NAME] = 'RequestForm'
							AND CCWR.[CommercialDocumentPk] = " + CommercialDocumentHeadPk + @"
						);" +
			"UPDATE CommercialDocument SET [InvoiceNo] = " + Common.StringToDB(InvoiceNo, true, true) +
					", [Shipper] = " + Common.StringToDB(ShipperName, true, false) +
					", [ShipperAddress] = " + Common.StringToDB(ShipperAddress, true, false) +
					", [Consignee] = " + Common.StringToDB(ConsigneeName, true, false) +
					", [ConsigneeAddress] = " + Common.StringToDB(ConsigneeAddress, true, false) +
					", [NotifyParty] = " + Common.StringToDB(NotifyParty, true, false) +
					", [NotifyPartyAddress] = " + Common.StringToDB(NotifyPartyAddress, true, false) +
					", [PortOfLoading] = " + Common.StringToDB(PortOfLoading, true, false) +
					", [FinalDestination] = " + Common.StringToDB(FinalDestination, true, false) +
					", [Carrier] = " + Common.StringToDB(Carrier, true, false) +
					", [SailingOn] = " + Common.StringToDB(SailingOn, true, false) +
					", [PaymentTerms] = " + Common.StringToDB(PaymentTerms, true, false) +
					", [OtherReferences] = " + Common.StringToDB(OtherRefferance, true, false) +
					", [StampImg] =" + Common.StringToDB(StampImg, true, true) +
					", [FOBorCNF] = " + Common.StringToDB(FOBNCNF, true, false) +
					", [VoyageNo] = " + Common.StringToDB(VoyageNo, true, false) +
					", [VoyageCompany] = " + Common.StringToDB(VoyageCompany, true, false) +
					", [ContainerNo] = " + Common.StringToDB(ContainerNo, true, false) +
					", [SealNo] = " + Common.StringToDB(SealNo, true, false) +
					", [ContainerSize] = " + Common.StringToDB(ContainerSize, true, false) +
					", ClearanceDate=" + Common.StringToDB(ClearanceDate, true, false) +
					", MarksNNo=" + Common.StringToDB(MarksNNo, true, false) +
					", LCNo=" + Common.StringToDB(LCNo, true, false) +                                                                          //				", StepCL=1" +
																			" WHERE CommercialDocumentHeadPk=" + CommercialDocumentHeadPk + ";");
		string[] TariffRow = TariffSUM.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
		DB.SqlCmd.CommandText = @"SELECT RFCH.[REQUESTFORMCALCULATE_HEAD_PK], RFCH.[MONETARY_UNIT], RFCH.[TOTAL_PRICE], RFCB.[REQUESTFORMCALCULATE_BODY_PK], 
								RFCB.[TITLE] AS Title, RFCB.[ORIGINAL_PRICE] AS Value, RF.[ExchangeDate]
							FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH
							LEFT JOIN [dbo].[CommerdialConnectionWithRequest] AS CCWR ON RFCH.[TABLE_PK] = CCWR.[RequestFormPk]
							LEFT JOIN [dbo].[RequestForm] AS RF ON RFCH.[TABLE_PK] = RF.[RequestFormPk]
							INNER JOIN ( 
										SELECT [REQUESTFORMCALCULATE_HEAD_PK], [REQUESTFORMCALCULATE_BODY_PK], [TITLE], [ORIGINAL_MONETARY_UNIT], [ORIGINAL_PRICE] 
										FROM [dbo].[REQUESTFORMCALCULATE_BODY] 
										WHERE [TITLE] IN ('관세', '부가세', '관세사비')
										) AS RFCB ON RFCH.[REQUESTFORMCALCULATE_HEAD_PK] = RFCB.[REQUESTFORMCALCULATE_HEAD_PK]
							WHERE RFCH.[TABLE_NAME] = 'RequestForm'
							AND CCWR.[CommercialDocumentPk] = " + CommercialDocumentHeadPk + @";";
		string savedtariff0 = "";
		string savedtariff1 = "";
		string savedtariff2 = "";
		string SavedHeadPk = "";
		string SavedMonetaryUnit = "";
		string SavedTotalPrice = "";
		string SavedExchangedDate = "";
		SqlDataReader AAA = DB.SqlCmd.ExecuteReader();
		while (AAA.Read()) {
			if (AAA["Title"] + "" == "관세") {
				savedtariff0 = AAA["Value"] + "";
			}
			if (AAA["Title"] + "" == "부가세") {
				savedtariff1 = AAA["Value"] + "";
			}
			if (AAA["Title"] + "" == "관세사비") {
				savedtariff2 = AAA["Value"] + "";
			}
			SavedHeadPk = AAA["REQUESTFORMCALCULATE_HEAD_PK"] + "";
			SavedMonetaryUnit = AAA["MONETARY_UNIT"] + "";
			SavedTotalPrice = AAA["TOTAL_PRICE"] + "";
			SavedExchangedDate = AAA["ExchangeDate"] + "";
		}
		AAA.Dispose();
		StringBuilder ModifyComment = new StringBuilder();
		foreach (string row in TariffRow) {
			string[] each = row.Split(Common.Splite321, StringSplitOptions.None);
			string savedTemp = "";
			if (each[0] == "관세") {
				savedTemp = savedtariff0;
			}
			if (each[0] == "부가세") {
				savedTemp = savedtariff1;
			}
			if (each[0] == "관세사비") {
				savedTemp = savedtariff2;
			}

			if (Common.NumberFormat(each[2]) != Common.NumberFormat(savedTemp)) {
				ModifyComment.Append("*" + each[0] + " : " + Common.NumberFormat(savedTemp) + " -> " + Common.NumberFormat(each[2]));
			}
			if (SavedHeadPk == "") {
				DB.SqlCmd.CommandText = @"SELECT TOP (1) RFCH.[REQUESTFORMCALCULATE_HEAD_PK], RFCH.[MONETARY_UNIT], RF.[ExchangeDate]
							FROM [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH
							LEFT JOIN [dbo].[RequestForm] AS RF ON RF.[RequestFormPk] = RFCH.[TABLE_PK]
							LEFT JOIN [dbo].[CommerdialConnectionWithRequest] AS CCWR ON RF.[RequestFormPk] = CCWR.[RequestFormPk]
							WHERE RFCH.[TABLE_NAME] = 'RequestForm'
							AND RF.[ConsigneePk] = RFCH.[CUSTOMER_COMPANY_PK]
							AND CCWR.[CommercialDocumentPk] = " + CommercialDocumentHeadPk + @"
							ORDER BY RFCH.[REQUESTFORMCALCULATE_HEAD_PK]";
				AAA = DB.SqlCmd.ExecuteReader();
				if (AAA.Read()) {
					SavedHeadPk = AAA["REQUESTFORMCALCULATE_HEAD_PK"] + "";
					SavedMonetaryUnit = AAA["MONETARY_UNIT"] + "";
					SavedExchangedDate = AAA["ExchangeDate"] + "";
				}
				AAA.Dispose();
			}
			if (each[2] != "") {
				string ExchangeRate = "";
				decimal ExchangedPrice = 0;
				ExchangedPrice = Math.Round(GetExchangeRated(each[1], SavedMonetaryUnit, Decimal.Parse(each[2]), out ExchangeRate, SavedExchangedDate));
				string ToExchangedRate = ExchangeRate.Split(new string[] { "@@" }, StringSplitOptions.None)[0].Split(new string[] { "!" }, StringSplitOptions.None)[3];
				Query.Append(@"INSERT INTO [dbo].[REQUESTFORMCALCULATE_BODY] 
								([REQUESTFORMCALCULATE_HEAD_PK], [CATEGORY], [TITLE], [ORIGINAL_MONETARY_UNIT], [EXCHANGED_MONETARY_UNIT], [TO_EXCHANGE_RATE], [ORIGINAL_PRICE], [EXCHANGED_PRICE])
								VALUES
								(" + SavedHeadPk + ", " + Common.StringToDB("제세금", true, false) + ", " + Common.StringToDB(each[0], true, true) + ", " + Common.StringToDB(each[1], false, false) + ", " + Common.StringToDB(SavedMonetaryUnit, false, false) + ", " + Common.StringToDB(ToExchangedRate, false, false) + ", " + Common.StringToDB(each[2], false, false) + ", " + Common.StringToDB(ExchangedPrice + "", false, false) + ");");
			}
		}
		DB.DBCon.Open();
		if (ModifyComment + "" != "") {
			Query.Append("INSERT INTO [dbo].[RequestModifyHistory] ([RequestFormPk],[ModifyWhich],[Value],[AccountID],[Registerd]) VALUES (" + RequestFormPk + ", 'Charge', N'" + ModifyComment + "', '" + AccountID + "', getdate());");
		}

		if (ItemSUM != "") {
			string[] ItemRow = ItemSUM.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
			foreach (string ROW in ItemRow) {
				string[] Each = ROW.Split(Common.Splite321, StringSplitOptions.None);
				Query.Append("	UPDATE RequestFormItems SET NetWeight = " + Common.StringToDB(Each[6], false, false) +
																							", [RAN]=" + Common.StringToDB(Each[7], false, false) +
																							", [HSCodeForCO]=" + Common.StringToDB(Each[8], true, false) +
																"	 WHERE RequestFormItemsPk=" + Each[5] + ";");
				if (Each[0] == "") {
					DB.SqlCmd.CommandText = "SELECT ItemCode FROM ClearanceItemCodeKOR WHERE Description=" + Common.StringToDB(Each[1], true, false) +
																																					" and Material=" + Common.StringToDB(Each[2], true, false) +
																																					" and TarriffRate=" + Common.StringToDB(Each[3], false, false) +
																																					" and AdditionalTaxRate=" + Common.StringToDB(Each[4], false, false) +
																																					" and CompanyPk=" + ConsigneeCompanyPk + ";";
					string Tempitemcode = DB.SqlCmd.ExecuteScalar() + "";
					if (Tempitemcode == "") {
						DB.SqlCmd.CommandText = "INSERT INTO ClearanceItemCodeKOR ([Description], [Material], [TarriffRate], [AdditionalTaxRate], [CompanyPk]) " +
																"	VALUES (" + Common.StringToDB(Each[1], true, false) + ", " + Common.StringToDB(Each[2], true, false) + ", " + Common.StringToDB(Each[3], false, false) + ", " + Common.StringToDB(Each[4], false, false) + ", " + ConsigneeCompanyPk + ");" +
																" SELECT @@IDENTITY;";
						Tempitemcode = DB.SqlCmd.ExecuteScalar() + "";
						//return DB.SqlCmd.CommandText;
					}
					Query.Append("UPDATE RequestFormItems SET [ItemCode] = " + Tempitemcode + " WHERE RequestFormItemsPk=" + Each[5] + ";");
				}
				else {
					Query.Append("UPDATE RequestFormItems SET [ItemCode] = " + Each[0] + " WHERE RequestFormItemsPk=" + Each[5] + ";");
					string tempGuanse = "";
					string tempBugase = "";
					if (Each[3] != "") {
						tempGuanse = " [TarriffRate] = " + Each[3];
					}
					if (Each[4] != "") {
						if (tempGuanse != "") {
							tempBugase = " , ";
						}
						tempBugase += " [AdditionalTaxRate]=" + Each[4];
					}
					if (tempGuanse != "" || tempBugase != "") {
						Query.Append(" UPDATE ClearanceItemCodeKOR SET " + tempGuanse + tempBugase + " WHERE ItemCode=" + Each[0] + ";");
					}
				}
			}
		}
		DB.SqlCmd.CommandText = Query + "";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();

		return "1";
	}

	[WebMethod]
	public String SendDateToCommercialDocu(string BBHPk) {
		DBConn DB = new DBConn();
		string TransportCL, FromDateTime, TempTable;
		string[] Description;
		DB.SqlCmd.CommandText = @"	SELECT TBBH.[TRANSPORT_WAY], TBBH.[DATETIME_FROM], TBBH.[VALUE_STRING_0], TBBH.[AREA_FROM], TBBH.[AREA_TO], TBBH.[VESSELNAME]
									, TBBH.[VOYAGE_NO], TBBH.[VALUE_STRING_1], TBBH.[VALUE_STRING_2], TBBH.[VALUE_STRING_3], TBBH.[TRANSPORT_STATUS]
									, TBPA.[NO], TBPA.[SIZE], TBPA.[SEAL_NO]
															FROM [dbo].[TRANSPORT_HEAD] AS TBBH
															LEFT JOIN [dbo].[TRANSPORT_PACKED] AS TBPA ON TBBH.[TRANSPORT_PK] = TBPA.[TRANSPORT_HEAD_PK]
                                                            WHERE TBBH.[TRANSPORT_PK]=" + BBHPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		if (RS.Read()) {
			TransportCL = RS["TRANSPORT_WAY"] + "";
			FromDateTime = RS["DATETIME_FROM"] + "";
			TempTable = RS["TRANSPORT_STATUS"] + "" == "" ? " TransportBBHistory " : " STORAGE ";
		}
		else {
			TransportCL = "";
			FromDateTime = "";
			Description = new string[] { "", "", "", "", "", "", "", "", "" };
			TempTable = "";
		}
		RS.Dispose();
		string PortOfLoading, FinalDestination, Carrier, SailingOn;
		string VoyageNo, VoyageCompany, ContainerNo, SealNo, ContainerSize;
		if (TransportCL == "Air") {
			PortOfLoading = RS["AREA_FROM"] + "";  //출발항
			FinalDestination = RS["AREA_TO"] + "";//도착항
			Carrier = RS["VESSELNAME"] + "";   // 선박명
			VoyageCompany = RS["VOYAGE_NO"] + "";   //선박회사명			
			SailingOn = FromDateTime.Substring(0, 10); // 선적날
			DB.SqlCmd.CommandText = "	UPDATE CommercialDocument " +
														"		SET PortOfLoading = " + Common.StringToDB(PortOfLoading, true, false) +
														"		, FinalDestination = " + Common.StringToDB(FinalDestination, true, false) +
														"		, Carrier = " + Common.StringToDB(Carrier, true, false) +
														"		, SailingOn = " + Common.StringToDB(SailingOn, true, false) +
														"		, VoyageCompany = " + Common.StringToDB(VoyageCompany, true, false) +
														"	WHERE CommercialDocumentHeadPk in (" +
														"	SELECT CCWR.CommercialDocumentPk " +
														"	FROM " + TempTable + " AS OBS " +
														"		left join CommerdialConnectionWithRequest AS CCWR ON OBS.RequestFormPk=CCWR.RequestFormPk " +
														"	WHERE OBS.TransportBetweenBranchPk=" + BBHPk + ");";
			DB.SqlCmd.ExecuteNonQuery();
		}
		else if (TransportCL == "Ship") {
			PortOfLoading = RS["AREA_FROM"] + "";  //출발항
			FinalDestination = RS["AREA_TO"] + "";//도착항
			Carrier = RS["VESSELNAME"] + "";   // 선박명
			VoyageCompany = RS["VESSELNAME"] + "";   //선박회사명
			ContainerSize = RS["SIZE"] + "";   // 컨테이너 규격
			VoyageNo = RS["VOYAGE_NO"] + "";  //항차
			ContainerNo = RS["NO"] + "";   //컨테이너번호
			SealNo = RS["SEAL_NO"] + "";//씰번호
			SailingOn = FromDateTime.Substring(0, 10); // 선적날
			DB.SqlCmd.CommandText = "	UPDATE CommercialDocument " +
														"		SET PortOfLoading = " + Common.StringToDB(PortOfLoading, true, false) +
														"		, FinalDestination = " + Common.StringToDB(FinalDestination, true, false) +
														"		, Carrier = " + Common.StringToDB(Carrier, true, false) +
														"		, SailingOn = " + Common.StringToDB(SailingOn, true, false) +
														"		, VoyageNo = " + Common.StringToDB(VoyageNo, true, false) +
														"		, VoyageCompany = " + Common.StringToDB(VoyageCompany, true, false) +
														"		, ContainerNo = " + Common.StringToDB(ContainerNo, true, false) +
														"		, SealNo = " + Common.StringToDB(SealNo, true, false) +
														"		, ContainerSize = " + Common.StringToDB(ContainerSize, true, false) +
														"	WHERE CommercialDocumentHeadPk in (" +
														"	SELECT CCWR.CommercialDocumentPk " +
														"	FROM " + TempTable + " AS OBS " +
														"		left join CommerdialConnectionWithRequest AS CCWR ON OBS.RequestFormPk=CCWR.RequestFormPk " +
														"	WHERE OBS.TransportBetweenBranchPk=" + BBHPk + ");";
			DB.SqlCmd.ExecuteNonQuery();
		}

		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String UnionBeforeCompanyToNewAccount(string CompanyBefore, string CompanyNew) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT COUNT(*) FROM RequestForm WHERE [ShipperPk] =" + CompanyNew + " or [ConsigneePk]=" + CompanyNew + ";";
		DB.DBCon.Open();
		string countNewCompanyRequest = DB.SqlCmd.ExecuteScalar() + "";
		if (countNewCompanyRequest != "0") {
			DB.DBCon.Close();
			return "0";
		}
		else {
			DB.SqlCmd.CommandText = "	UPDATE Account_ SET [CompanyPk] = " + CompanyBefore + " WHERE CompanyPk=" + CompanyNew + ";" +
														"	UPDATE Company SET [GubunCL] = 0 WHERE CompanyPk=" + CompanyBefore + ";" +
														"	DELETE FROM Company WHERE CompanyPk=" + CompanyNew + ";";
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		}
	}

	[WebMethod]
	public String UnionTwoCompany(string From, string To) {
		StringBuilder Query = new StringBuilder();
		DB = new DBConn();
		Query.Append("	UPDATE Account_ SET [CompanyPk] = " + To + " WHERE CompanyPk=" + From + ";" +
							"		UPDATE CompanyWarehouse SET [CompanyPk]=" + To + " WHERE CompanyPk=" + From + ";" +
							"		UPDATE CompanyRelation SET [MainCompanyPk] = " + To + " WHERE [MainCompanyPk] = " + From + ";" +
							"		UPDATE CompanyRelation SET [TargetCompanyPk] = " + To + " WHERE [TargetCompanyPk] = " + From + ";" +
							"		UPDATE CompanyInDocument SET [GubunPk] = " + To + " WHERE [GubunPk] =" + From + ";" +
							"		UPDATE [dbo].[COMMENT] SET [TABLE_PK] = " + To + " WHERE [TABLE_NAME] = 'Company' AND [TABLE_PK] =" + From + ";" +
							"		UPDATE TransportBC SET [CompanyPk] = " + To + " WHERE [CompanyPk] =" + From + ";" +
							"		UPDATE RequestForm SET [ShipperPk] = " + To + " WHERE [ShipperPk] =" + From + ";" +
							"		UPDATE RequestForm SET [ConsigneePk] = " + To + " WHERE [ConsigneePk] =" + From + ";" +
							"		UPDATE [dbo].[File] SET [GubunPk] = " + To + " WHERE [GubunPk] =" + From + " and GubunCL=1;" +
							"		UPDATE Company SET [GubunCL]=0 WHERE CompanyPk=" + To + ";" +
							"		DELETE FROM Company WHERE CompanyPk=" + From + ";" +
							"		DELETE FROM CompanyAdditionalInfomation WHERE CompanyPk=" + From + ";");
		DB.SqlCmd.CommandText = Query + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String UpdateRequestFormDocumentStepCL(string RequestFormPk, string DocumentStepCL, string AccountID) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE RequestForm SET [DocumentStepCL] = " + DocumentStepCL + " WHERE RequestFormPk=" + RequestFormPk + ";" +
			new GetQuery().AddRequestHistory(RequestFormPk, DocumentStepCL, AccountID, "");
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String RequestToStep(string RequestFormPk, string StepCL) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE RequestForm SET [StepCL] = " + StepCL + " WHERE RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String TempEnd(string RequestFormPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE RequestForm SET [StepCL] = 64 WHERE RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	//BMK CollectPayment.aspx
	[WebMethod]
	public String DepositedOK(string RequestFormPk, string GubunCL, string CompanyPk, string BankAccount, string MonetaryUnitCL, string Value, string DepositedDate, string AccountID, string Memo) {
		StringBuilder Query = new StringBuilder();
		Query.Append("	INSERT INTO RequestFormDeposited ([RequestFormPk], [GubunCL], [BankAccountPk], [MonetaryUnitCL], [Charge], [DepositedDate], [Registerd], [Memo]) " +
								"	VALUES (" + RequestFormPk + ", " + GubunCL + ", " + BankAccount + ", " + MonetaryUnitCL + ", " + Decimal.Parse(Value) + ", '" + DepositedDate + "',getDate(), " + Common.StringToDB(Memo, true, true) + ");" +
		new GetQuery().AddRequestHistory(RequestFormPk, "7" + GubunCL, AccountID, Value) +
		"SELECT SUM([Charge]) FROM RequestFormDeposited WHERE RequestFormPk=" + RequestFormPk + " and GubunCL=" + GubunCL + " Group By RequestFormPk");
		DB = new DBConn();
		DB.SqlCmd.CommandText = Query + "";
		DB.DBCon.Open();
		string SUM = DB.SqlCmd.ExecuteScalar() + "";

		if (GubunCL == "0") {
			DB.SqlCmd.CommandText = "	UPDATE RequestFormCalculateHead SET [ShipperDeposited] = " + SUM + "  WHERE RequestFormPk=" + RequestFormPk + ";";
		}
		else {
			DB.SqlCmd.CommandText = "	UPDATE RequestFormCalculateHead SET [ConsigneeDeposited] = " + SUM + " WHERE RequestFormPk=" + RequestFormPk + ";";
		}
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String DepositedOKMultiple(string ReturnValue, string ShipperPk, string ConsigneePk, string AccountID, string DepositedDate, string Memo) {
		try {
			string[] Row = ReturnValue.Split(new string[] { "%%$$##" }, StringSplitOptions.None);
			foreach (var EachRow in Row) {
				var Each = EachRow.Split(new string[] { "!@#" }, StringSplitOptions.None);
				DepositedOK(Each[1], Each[5], Each[0] == "S" ? ShipperPk : ConsigneePk, "47", Each[3], Each[4], DepositedDate, AccountID, Memo);
			}
			return "1";
		} catch (Exception ex) {
			return ex.Message;
		}
	}

	[WebMethod]
	public String DepositModifyOK(string RequestFormDepositedPk, string[] ModifyValue, string BeforeCharge, string AccountID, string GubunCL, string RequestFormPk) {
		StringBuilder InnerQuery = new StringBuilder();
		bool IsModifyCharge = false;
		if (ModifyValue[0] != "NN") {
			InnerQuery.Append(", [BankAccountPk] = " + Common.StringToDB(ModifyValue[0], false, false));
		}

		if (ModifyValue[1] != "NN") {
			InnerQuery.Append(", [Charge] = " + Common.StringToDB(ModifyValue[1], false, false));
			IsModifyCharge = true;
		}
		if (ModifyValue[2] != "NN") {
			InnerQuery.Append(", [DepositedDate] = " + Common.StringToDB(ModifyValue[2], true, false));
		}
		if (ModifyValue[3] != "NN") {
			InnerQuery.Append(", [Memo] = " + Common.StringToDB(ModifyValue[3], true, true));
		}

		if (InnerQuery + "" == "") {
			return "0";
		}

		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE RequestFormDeposited SET [Confirmed]=NULL" + InnerQuery + " WHERE [RequestFormDepositedPk]=" + RequestFormDepositedPk + ";";

		DB.DBCon.Open();
		if (IsModifyCharge) {
			DB.SqlCmd.CommandText += "SELECT SUM([Charge]) FROM RequestFormDeposited WHERE RequestFormPk=" + RequestFormPk + " and GubunCL=" + GubunCL + " Group By RequestFormPk;";
			string SUM = DB.SqlCmd.ExecuteScalar() + "";
			if (GubunCL == "0") {
				DB.SqlCmd.CommandText = "	UPDATE RequestFormCalculateHead SET [ShipperDeposited] = " + SUM + "  WHERE RequestFormPk=" + RequestFormPk + ";";
			}
			else {
				DB.SqlCmd.CommandText = "	UPDATE RequestFormCalculateHead SET [ConsigneeDeposited] = " + SUM + " WHERE RequestFormPk=" + RequestFormPk + ";";
			}
		}
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String DepositedCancel(string GubunCL, string RequestFormPk) {
		DB = new DBConn();
		if (GubunCL == "0")
			DB.SqlCmd.CommandText = "	UPDATE RequestFormCalculateHead SET [ShipperDepositedDate] = NULL, [ShipperDeposited] = NULL WHERE RequestFormPk=" + RequestFormPk + ";";
		else {
			DB.SqlCmd.CommandText = "	UPDATE RequestFormCalculateHead SET [ConsigneeDepositedDate] = NULL, [ConsigneeDeposited] = NULL WHERE RequestFormPk=" + RequestFormPk + ";";
		}
		DB.SqlCmd.CommandText += "DELETE FROM RequestFormDeposited WHERE RequestFormPk=" + RequestFormPk + " and GubunCL=" + GubunCL + ";";

		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String DepositedConfirm(string GubunCL, string AccountID, string RequestFormPk, string LessOrOverValue, string CompanyPk) {
		DB = new DBConn();
		if (GubunCL == "0") {
			DB.SqlCmd.CommandText = "	UPDATE RequestFormCalculateHead SET [ShipperDepositedDate] = getDate() WHERE RequestFormPk=" + RequestFormPk + ";" +
				new GetQuery().AddRequestHistory(RequestFormPk, "8" + GubunCL, AccountID, "");
		}
		else {
			DB.SqlCmd.CommandText = "	UPDATE RequestFormCalculateHead SET [ConsigneeDepositedDate] = getDate() WHERE RequestFormPk=" + RequestFormPk + ";" +
				new GetQuery().AddRequestHistory(RequestFormPk, "8" + GubunCL, AccountID, "");
		}
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="SorC">대문자로</param>
	/// <param name="RequestFormPk"></param>
	/// <returns></returns>
	[WebMethod]
	public String[] LoadCollectPayment(string SorC, string RequestFormPk, string CompanyPk) {
		List<string> ReturnValue = new List<string>();
		DB = new DBConn();
		DB.DBCon.Open();
		decimal Total = 0;
		decimal Aleady = 0;
		ReturnValue.Add(LoadCharge(SorC, RequestFormPk, ref Total, CompanyPk));
		ReturnValue.Add(LoadDeposied(SorC, RequestFormPk, ref Aleady));
		ReturnValue.Add(Common.NumberFormat((Total - Aleady) + ""));
		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}

	private String LoadCharge(string SorC, string RequestFormPk, ref decimal Total, string CompanyPk) {
		StringBuilder ReturnValue = new StringBuilder();
		Total = 0;
		string exchangeRate = "";
		string monetaryUnitCL = "";
		string tempWhereGubunCL;
		if (SorC == "S") {
			tempWhereGubunCL = "200";
			DB.SqlCmd.CommandText = @"	SELECT RFCH.ShipperMonetaryUnit, RFCH.ShipperCharge, RFCH.ShipperBankAccountPk, RFCH.WillPayTariff, RFCH.ShipperDepositedDate, RFCH.ShipperDeposited, RFCH.ExchangeRate
															FROM RequestFormCalculateHead AS RFCH
															WHERE RFCH.RequestFormPk=" + RequestFormPk + ";";
		}
		else {
			tempWhereGubunCL = "300";
			DB.SqlCmd.CommandText = "	SELECT RFCH.ConsigneeMonetaryUnit, RFCH.ConsigneeCharge, RFCH.ConsigneeBankAccountPk, RFCH.WillPayTariff, RFCH.ConsigneeDepositedDate, RFCH.ConsigneeDeposited, RFCH.ExchangeRate " +
														"	FROM RequestFormCalculateHead AS RFCH " +
														"	WHERE RFCH.RequestFormPk=" + RequestFormPk + ";";
		}
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string tempWillPayTariff = "";
		if (RS.Read()) {
			monetaryUnitCL = RS[0] + "";
			Total += decimal.Parse(RS[1] + "");
			ReturnValue.Append(RS[4] + " : " + RS[5] + "#@!" + RS[2] + "%!$@#" + Common.GetMonetaryUnit(RS[0] + "") + " " + Common.NumberFormat(RS[1] + ""));
			tempWillPayTariff = RS[3] + "";
			exchangeRate = RS["ExchangeRate"] + "";
			RS.Dispose();
		}
		else {
			RS.Dispose();
			return "N";
		}

		if (tempWillPayTariff == SorC) {
			DB.SqlCmd.CommandText = @"	SELECT TOP 1 Tariff.MonetaryUnitCL, Total.TotalValue
																FROM CommercialDocumentTariff AS Tariff
																	left join (SELECT [CommercialDocumentHeadPk], SUM([Value]) AS TotalValue FROM CommercialDocumentTariff WHERE GubunPk=" + RequestFormPk + @" group by [CommercialDocumentHeadPk])
																	AS Total ON Total.CommercialDocumentHeadPk=Tariff.CommercialDocumentHeadPk
																WHERE Tariff.GubunPk=" + RequestFormPk + ";";
			RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				if (RS[0] + "" == monetaryUnitCL) {
					Total += decimal.Parse(RS[1] + "");
					ReturnValue.Append("%!$@#" + Common.GetMonetaryUnit(RS[0] + "") + " " + Common.NumberFormat(RS[1] + ""));
					RS.Dispose();
					DB.DBCon.Close();
				}
				else {
					string tempExchangedDate = "";
					if (exchangeRate != "") {
						string[] RowExchangeRate = exchangeRate.Split(Common.Splite22, StringSplitOptions.RemoveEmptyEntries);
						for (int i = 0; i < RowExchangeRate.Length; i++) {
							if (tempExchangedDate == "") {
								tempExchangedDate = RowExchangeRate[i].Substring(6, 8);
								continue;
							}
							if (Int32.Parse(RowExchangeRate[i].Substring(6, 8)) > Int32.Parse(tempExchangedDate)) {
								tempExchangedDate = RowExchangeRate[i].Substring(6, 8);
							}
						}
					}
					string temp = "";

					string tempmonetary = RS[0] + "";
					decimal tempvalue = decimal.Parse(RS[1] + "");
					RS.Dispose();
					DB.DBCon.Close();

					decimal tempexchangedTariff = GetExchangeRated(tempmonetary, monetaryUnitCL, tempvalue, out temp, tempExchangedDate);
					decimal exchangedtariff;
					switch (monetaryUnitCL) {
						case "18":
							exchangedtariff = Math.Round(tempexchangedTariff, 1, MidpointRounding.AwayFromZero);
							break;

						case "19":
							exchangedtariff = Math.Round(tempexchangedTariff, 2, MidpointRounding.AwayFromZero);
							break;

						case "20":
							exchangedtariff = Math.Round(tempexchangedTariff, 0, MidpointRounding.AwayFromZero);
							break;

						default:
							exchangedtariff = tempexchangedTariff;
							break;
					}
					Total += exchangedtariff;
					ReturnValue.Append("%!$@#" + Common.GetMonetaryUnit(monetaryUnitCL) + " " + exchangedtariff);
				}
			}
			else {
				ReturnValue.Append("%!$@#N");
				RS.Dispose();
				DB.DBCon.Close();
			}
		}
		else {
			ReturnValue.Append("%!$@#N");
			DB.DBCon.Close();
		}

		DB.SqlCmd.CommandText = @"	SELECT MonetaryUnit, Price
															FROM RequestFormCalculateBody
															WHERE RequestFormPk=" + RequestFormPk + " and  StandardPriceHeadPkNColumn='D' and GubunCL=" + tempWhereGubunCL + ";";
		DB.DBCon.Open();
		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			Total += decimal.Parse(RS[1] + "");
			ReturnValue.Append("%!$@#" + Common.GetMonetaryUnit(RS[0] + "") + " " + Common.NumberFormat(RS[1] + ""));
		}
		else {
			ReturnValue.Append("%!$@#N");
		}
		RS.Dispose();
		ReturnValue.Append("%!$@#" + Common.GetMonetaryUnit(monetaryUnitCL) + " " + Common.NumberFormat(Total + ""));
		return ReturnValue + "";
	}

	private String LoadDeposied(string SorC, string RequestFormPk, ref decimal AleadyDeposited) {
		StringBuilder ReturnValue = new StringBuilder();
		string WhereGubun = SorC == "S" ? "RFD.GubunCL=0" : "RFD.GubunCL=1";
		DB.SqlCmd.CommandText = @"	SELECT RFD.RequestFormDepositedPk, RFD.MonetaryUnitCL, RFD.Charge, RFD.DepositedDate, RFD.Memo, CB.BankName, CB.OwnerName
															FROM RequestFormDeposited AS RFD
																left join CompanyBank AS CB ON RFD.BankAccountPk=CB.CompanyBankPk
															WHERE RFD.RequestFormPk=" + RequestFormPk + " and " + WhereGubun +
															" ORDER BY RFD.DepositedDate ASC ";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			AleadyDeposited += Decimal.Parse(RS["Charge"] + "");
			string tempIsinorout = "入";
			if ((RS[3] + "").Substring(0, 1) == "-") {
				tempIsinorout = "出";
			}
			ReturnValue.Append(RS[0] + "#@!" + tempIsinorout + "#@!" + Common.GetMonetaryUnit(RS[1] + "") + " " + Common.NumberFormat(RS[2] + "") + "#@!" + RS[3] + "#@!" + RS[5] + " (" + RS[6] + ") : " + (RS["DepositedDate"] + "").Substring(4, 4) + "#@!" + RS["Memo"] + "%!$@#");
		}
		RS.Dispose();
		return ReturnValue + "aleady#@!" + AleadyDeposited;
	}

	private String GetExchangedDateFromRequestFormPk(string RequestFormPk, string From, string To) {
		DB.SqlCmd.CommandText = "SELECT [ExchangeDate] FROM [dbo].[RequestForm] WHERE [RequestFormPk]=" + RequestFormPk + ";";
		string ExchangeDate = DB.SqlCmd.ExecuteScalar() + "";
		if (ExchangeDate == "") {
			return "NULL";
		}
		else {
			return ExchangeDate;
		}
	}

	[WebMethod]
	public String DelectAccount(string AccountPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "	DELETE FROM Account_ WHERE [AccountPk]='" + AccountPk + "';" +
													"	DELETE FROM [AccountAdditionalInfo_] WHERE AccountPk='" + AccountPk + "';";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String DeleteWarehouse(string WarehousePk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "DELETE FROM [CompanyWarehouse] WHERE [WarehousePk]=" + WarehousePk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	//BMK AddDeliveryPlace.aspx
	[WebMethod]
	public String CancelDeliveryOrder(string OurBranchStorageOutPk, string RequestFormPk, string AccountID) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "	UPDATE OurBranchStorageOut SET TransportBetweenCompanyPk=NULL WHERE OurBranchStorageOutPk=" + OurBranchStorageOutPk + ";";
		DB.SqlCmd.CommandText += new GetQuery().AddRequestHistory(RequestFormPk, "19", AccountID, "");
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	//[WebMethod]
	//public String SendDeliveryMSG(string TBCPk, string AccountID, string OurBranchPk, string ToPk) {
	//    Email emailsend = new Email("Local");
	//    DB = new DBConn();
	//    DB.SqlCmd.CommandText = "SELECT TOP 1 [RequestFormPk] FROM OurBranchStorageOut WHERE [TransportBetweenCompanyPk]=" + TBCPk + " ;";
	//    DB.DBCon.Open();
	//    string requestformPk = DB.SqlCmd.ExecuteScalar() + "";

	//    DB.SqlCmd.CommandText = "SELECT [DriverName], [DriverTEL], [ToDate], [WarehouseInfo], [WarehouseMobile], [PackedCount], [PackingUnit], [DepositWhere], [Price] FROM TransportBC WHERE [TransportBetweenCompanyPk]=" + TBCPk + ";";
	//    SqlDataReader RS = DB.SqlCmd.ExecuteReader();
	//    string MSGTypeKOR = "{0}화물이 {1} 편으로 배차되었습니다. {2} 도착예정, {3} 입니다.";
	//    string MSG;
	//    Query = new StringBuilder();
	//    if (RS.Read()) {
	//        string todate = "";
	//        if (( RS["ToDate"] + "" ).Length < 9) {
	//            todate = ( RS["ToDate"] + "" ).Substring(4, 2) + "/" + ( RS["ToDate"] + "" ).Substring(6, 2);
	//        } else if (( RS["ToDate"] + "" ).Length < 11) {
	//            todate = ( RS["ToDate"] + "" ).Substring(4, 2) + "/" + ( RS["ToDate"] + "" ).Substring(6, 2) + " " + ( RS["ToDate"] + "" ).Substring(8, 2);
	//        } else if (( RS["ToDate"] + "" ).Length == 12) {
	//            todate = ( RS["ToDate"] + "" ).Substring(4, 2) + "/" + ( RS["ToDate"] + "" ).Substring(6, 2) + " " + ( RS["ToDate"] + "" ).Substring(8, 2) + ":" + ( RS["ToDate"] + "" ).Substring(10, 2);
	//        }

	//        string deposit = RS["DepositWhere"] + "" == "0" ? "현불" : "착불";
	//        MSG = String.Format(MSGTypeKOR,
	//            RS["PackedCount"] + Common.GetPackingUnit(RS["PackingUnit"] + ""),
	//            RS["DriverName"] + "" + ( RS["DriverTEL"] + "" == "" ? "" : "(" + RS["DriverTEL"] + ")" ),
	//            todate,
	//            deposit + " " + Common.NumberFormat(RS["Price"] + ""));
	//        string[] ToMobile = ( RS["WarehouseMobile"] + "" ).Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
	//        string damdangja = ( RS["WarehouseInfo"] + "" ).Split(Common.Splite22, StringSplitOptions.None)[2];

	//        foreach (string phoneNo in ToMobile) {
	//            if (phoneNo != "") {
	//                string phoneNumber = phoneNo.Replace("-", "");
	//                string phoneFirst3 = phoneNo.Substring(0, 3);
	//                if (phoneFirst3 == "010" || phoneFirst3 == "011" || phoneFirst3 == "016" || phoneFirst3 == "017" || phoneFirst3 == "018" || phoneFirst3 == "019" || phoneFirst3 == "070") {
	//                    try {
	//                        emailsend.SendSMSKorea(RS["DriverTEL"] + "", phoneNumber, MSG);
	//                        Query.Append("INSERT INTO MsgSendedHistory ([GubunCL], [GubunPk], [SenderID], [Receiver], [ReceiverAddress], [FromPk], [ToPk], [Title], [Contents], [SendedTime]) " +
	//                                                                                        "VALUES (6, " + requestformPk + ", '" + AccountID + "', N'" + damdangja + "', '" + phoneNo + "', '" + OurBranchPk + "' , " + ToPk + ", NULL, '" + MSG + "', getDate())");
	//                    } catch (Exception) {
	//                        Query.Append("INSERT INTO MsgSendedHistory ([GubunCL], [GubunPk], [SenderID], [Receiver], [ReceiverAddress], [FromPk], [ToPk], [Title], [Contents], [SendedTime]) " +
	//                                                                                                "VALUES (7, " + requestformPk + ", '" + AccountID + "', N'" + damdangja + "', '" + phoneNo + "', '" + OurBranchPk + "' , " + ToPk + ", NULL, '" + MSG + "', getDate())");
	//                    }
	//                    continue;
	//                }
	//            }
	//        }
	//    }
	//    RS.Dispose();
	//    if (Query + "" != "") {
	//        DB.SqlCmd.CommandText = Query + "";
	//        DB.SqlCmd.ExecuteNonQuery();
	//    }
	//    DB.DBCon.Close();
	//    return "1";
	//}
	[WebMethod]
	public string AddTransportBCHistory(string TransportBetweenCompanyPk, string GubunCL, string AccountID, string Comment) {
		DB = new DBConn();
		string Query = new GetQuery().AddTransportBCHistory(TransportBetweenCompanyPk, GubunCL, AccountID, Comment);
		DB.SqlCmd.CommandText = Query + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String SendSMS(string From, string To, string Contents) {
		Email emailsend = new Email("SMS");
		String[] ToMobile = To.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
		StringBuilder Query = new StringBuilder();
		winic.Service1 ws = new winic.Service1();
		foreach (string phoneNo in ToMobile) {
			if (phoneNo != "") {
				string phoneNumber = phoneNo.Replace("-", "");
				string phoneFirst3 = phoneNo.Substring(0, 3);
				if (phoneFirst3 == "010" || phoneFirst3 == "011" || phoneFirst3 == "016" || phoneFirst3 == "017" || phoneFirst3 == "018" || phoneFirst3 == "019" || phoneFirst3 == "070") {

					emailsend.SendMobileMsgKorea(From.Trim(), phoneNumber.Trim(), Contents.Trim());
					continue;
				}

				if (phoneFirst3 == "139" || phoneFirst3 == "138" || phoneFirst3 == "137" || phoneFirst3 == "136" || phoneFirst3 == "135" || phoneFirst3 == "134" || phoneFirst3 == "159" || phoneFirst3 == "158" || phoneFirst3 == "152" || phoneFirst3 == "150" || phoneFirst3 == "188" || phoneFirst3 == "130" || phoneFirst3 == "131" || phoneFirst3 == "132" || phoneFirst3 == "155" || phoneFirst3 == "156" || phoneFirst3 == "186" || phoneFirst3 == "133" || phoneFirst3 == "153" || phoneFirst3 == "189" || phoneFirst3 == "185") {
					//您好 50箱货物已入库，详情请上 nn21.net网站上确认--国际物流。
					string resultmsgTochina = ws.SendMessages("intl2000", "ythq1717", phoneNumber.Trim(), Contents.Trim(), "");
					continue;
				}
			}
		}
		return "1";
	}

	[WebMethod]
	public String[] ModifyDeliveryPlaceOnLoad(string OurBranchStorageOutPk, string RequestFormPk) {
		DB = new DBConn();
		string[] ReturnValue;
		if (OurBranchStorageOutPk == "N") {
			DB.SqlCmd.CommandText = @"declare  @RequestFormPk int; SET @RequestFormPk=" + RequestFormPk + @";
 SELECT R.ArrivalDate
	, R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume
	, RFCB.Price , RFCB.MonetaryUnit
 FROM RequestForm AS R
	left join (
		SELECT RequestFormPk ,Price ,MonetaryUnit
		FROM RequestFormCalculateBody
		WHERE StandardPriceHeadPkNColumn='D' and GubunCL=300 and RequestFormPk=@RequestFormPk)
	AS RFCB ON R.RequestFormPk=RFCB.RequestFormPk
 WHERE R.RequestFormPk=@RequestFormPk;";
			DB.DBCon.Open();
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				ReturnValue = new string[]{
						RS["ArrivalDate"]+"",
						RS["TotalPackedCount"]+"",
						Common.GetPackingUnit(RS["PackingUnit"]+""),
						Common.NumberFormat(RS["TotalGrossWeight"]+""),
						Common.NumberFormat(RS["TotalVolume"]+""),
						Common.GetMonetaryUnit(RS["MonetaryUnit"]+""),
						RS["Price"]+""==""?"": Common.NumberFormat(RS["Price"]+""),
						RS["PackingUnit"]+""
						};
			}
			else {
				RS.Dispose();
				DB.DBCon.Close();
				return new String[] { "N" };
			}
			RS.Dispose();
			DB.DBCon.Close();
			return ReturnValue;
		}
		else {
			DB.SqlCmd.CommandText = @"SELECT
OBSO.StorageCode, OBSO.BoxCount, OBSO.TransportBetweenBranchPk, OBSO.TransportBetweenCompanyPk, OBSO.StatusCL
, R.TotalPackedCount, R.PackingUnit, R.TotalGrossWeight, R.TotalVolume
, TBC.Type
, TBC.Title, TBC.DriverName, TBC.DriverTEL, TBC.TEL, TBC.CarSize, TBC.FromDate, TBC.ToDate
, TBC.WarehouseInfo, TBC.WarehouseMobile
, TBC.DepositWhere, TBC.Price, TBC.Memo ,TBC.[DeliveryPrice]
, OBSC.StorageName
FROM OurBranchStorageOut AS OBSO
	left join [dbo].[RequestForm] AS R ON OBSO.RequestFormPk=R.[RequestFormPk]
	left join TransportBC AS TBC ON OBSO.TransportBetweenCompanyPk=TBC.TransportBetweenCompanyPk
	left join OurBranchStorageCode AS OBSC ON OBSO.StorageCode=OBSC.OurBranchStoragePk
WHERE OBSO.OurBranchStorageOutPk=" + OurBranchStorageOutPk + ";";
			DB.DBCon.Open();
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			string LeftCount = "";
			string TempStorageCode = "";
			string TempTBCPk = "";
			if (RS.Read()) {
				TempTBCPk = RS["TransportBetweenCompanyPk"] + "";
				LeftCount = RS["TransportBetweenCompanyPk"] + "" == "" ? "0" : "";
				TempStorageCode = RS["StorageCode"] + "";
				ReturnValue = new string[] {
					RS["StorageName"]+"",
					RS["BoxCount"]+"",
					RS["TransportBetweenCompanyPk"]+"",
					RS["StatusCL"]+"",
					LeftCount,
					RS["TotalPackedCount"]+"",
					Common.GetPackingUnit(RS["PackingUnit"]+""),
					Common.NumberFormat(RS["TotalGrossWeight"]+""),
					Common.NumberFormat(RS["TotalVolume"]+""),
					RS["Type"]+"",
					RS["Title"]+"",
					RS["DriverName"]+"",
					RS["DriverTEL"]+"",
					RS["TEL"]+"",
					RS["CarSize"]+"",
					RS["FromDate"]+"",
					RS["ToDate"]+"",
					RS["WarehouseInfo"]+"",
					RS["WarehouseMobile"]+"",
					RS["DepositWhere"]+"",
					Common.NumberFormat(RS["Price"]+""),
					RS["Memo"]+"",
					RS["PackingUnit"]+"",
					RS["StorageCode"]+"",
					RS["TransportBetweenBranchPk"]+"",
					Common.NumberFormat(RS["DeliveryPrice"]+"")
					//,
					//RS["MemberMemo"]+""
				};
			}
			else {
				RS.Dispose();
				DB.DBCon.Close();
				return new String[] { "N" };
			}
			RS.Dispose();
			if (LeftCount == "") {
				DB.SqlCmd.CommandText = "SELECT BoxCount FROM OurBranchStorageOut WHERE StorageCode=" + TempStorageCode + " and RequestFormPk=" + RequestFormPk + " and isnull(TransportBetweenCompanyPk, 0)=0;";
				LeftCount = DB.SqlCmd.ExecuteScalar() + "";
				ReturnValue[4] = LeftCount == "" ? "0" : LeftCount;
			}

			if (TempTBCPk == "") {
				DB.SqlCmd.CommandText = @"declare  @RequestFormPk int; SET @RequestFormPk=" + RequestFormPk + @";
 SELECT R.ArrivalDate, RFCB.Price , RFCB.MonetaryUnit
 FROM RequestForm AS R
	left join (
		SELECT RequestFormPk ,Price ,MonetaryUnit
		FROM RequestFormCalculateBody
		WHERE StandardPriceHeadPkNColumn='D' and GubunCL=300 and RequestFormPk=@RequestFormPk)
	AS RFCB ON R.RequestFormPk=RFCB.RequestFormPk
 WHERE R.RequestFormPk=@RequestFormPk;";
				RS = DB.SqlCmd.ExecuteReader();
				if (RS.Read()) {
					ReturnValue[15] = RS[0] + "";
					ReturnValue[16] = RS[0] + "";
					if (RS[1] + "" != "") {
						ReturnValue[20] = Common.NumberFormat(RS[1] + "");
					}
				}
				RS.Dispose();
			}
			DB.DBCon.Close();
			return ReturnValue;
		}
	}

	//[WebMethod]
	//public String SetDeliveryPlace2(string RequestFormPk, string TransportBBCLPk, string CompanyPk, string Type, string Title, string DriverName, string DriverTEL, string ToDate,
	//    string WarehouseInfo, string WarehouseMobile, string PackedCount, string PackingUnit, string Weight, string Volume, string DepositWhere, string Price, string Memo, string FromDate, string TEL, string CarSize,
	//    string TotalBoxCount, string OurBranchStorageOutPk) {
	//    DB = new DBConn();
	//    DB.SqlCmd.CommandText = "INSERT INTO TransportBC ([RequestFormPk], [TransportBBCLPk], [CompanyPk], [Type], [Title], [DriverName], [DriverTEL], [TEL], [CarSize], [FromDate], [ToDate], [WarehouseInfo], [WarehouseMobile], [PackedCount], [PackingUnit], [Weight], [Volume], [DepositWhere], [Price], [Memo]) " +
	//    "	VALUES (" + Common.StringToDB(RequestFormPk, false, false) + ", " + Common.StringToDB(TransportBBCLPk, false, false) + ", " + Common.StringToDB(CompanyPk, false, false) + ", " +
	//    Common.StringToDB(Type, true, true) + ", " + Common.StringToDB(Title, true, true) + ", " + Common.StringToDB(DriverName, true, true) + ", " + Common.StringToDB(DriverTEL, true, false) + ", " +
	//    Common.StringToDB(TEL, true, false) + ", " + Common.StringToDB(CarSize, true, false) + ", " +
	//    Common.StringToDB(FromDate, true, false) + ", " + Common.StringToDB(ToDate, true, false) + ", " + Common.StringToDB(WarehouseInfo, true, true) + ", " + Common.StringToDB(WarehouseMobile, true, false) + ", " +
	//    Common.StringToDB(PackedCount, false, false) + ", " + Common.StringToDB(PackingUnit, false, false) + ", " + Common.StringToDB(Weight, false, false) + ", " + Common.StringToDB(Volume, false, false) + ", " +
	//    Common.StringToDB(DepositWhere, false, false) + ", " + Common.StringToDB(Price, false, false) + ", " + Common.StringToDB(Memo, true, true) + "); SELECT @@IDENTITY;";
	//    DB.DBCon.Open();
	//    string Identity = DB.SqlCmd.ExecuteScalar() + "";

	//    if (OurBranchStorageOutPk == "N") {
	//        if (TotalBoxCount == PackedCount) {
	//            DB.SqlCmd.CommandText = "INSERT INTO OurBranchStorageOut (StorageCode, RequestFormPk, BoxCount, TransportBetweenCompanyPk, StatusCL) VALUES (" +
	//                "0, " + Common.StringToDB(RequestFormPk, false, false) + ", " + Common.StringToDB(PackedCount, false, false) + ", " + Identity + ", " + "1 );";
	//        } else {
	//            DB.SqlCmd.CommandText = "INSERT INTO OurBranchStorageOut (StorageCode, RequestFormPk, BoxCount, TransportBetweenCompanyPk, StatusCL) VALUES (" +
	//                "0, " + Common.StringToDB(RequestFormPk, false, false) + ", " + Common.StringToDB(PackedCount, false, false) + ", " + Identity + ", " + "1 );" +
	//            "INSERT INTO OurBranchStorageOut (StorageCode, RequestFormPk, BoxCount, TransportBetweenCompanyPk, StatusCL) VALUES (" +
	//                "0, " + Common.StringToDB(RequestFormPk, false, false) + ", " + Common.StringToDB(( Int32.Parse(TotalBoxCount) - Int32.Parse(PackedCount) ) + "", false, false) + ", " + "NULL , " + "0 );";
	//        }
	//    } else {
	//        if (TotalBoxCount == PackedCount) {
	//            DB.SqlCmd.CommandText = "UPDATE OurBranchStorageOut SET [TransportBetweenCompanyPk] = " + Identity + " WHERE OurBranchStorageOutPk=" + OurBranchStorageOutPk + ";";
	//        } else {
	//            DB.SqlCmd.CommandText = "UPDATE OurBranchStorageOut SET [BoxCount] = " + Common.StringToDB(( Int32.Parse(TotalBoxCount) - Int32.Parse(PackedCount) ) + "", false, false) + " WHERE OurBranchStorageOutPk=" + OurBranchStorageOutPk + ";" +
	//                "INSERT INTO OurBranchStorageOut ([StorageCode], [RequestFormPk], [BoxCount], [StockedDate], [TransportBetweenBranchPk], [TransportBetweenCompanyPk], [StatusCL], [Comment]) " +
	//                "SELECT [StorageCode], [RequestFormPk], " + PackedCount + ", [StockedDate], [TransportBetweenBranchPk], " + Identity + ", [StatusCL]+1, [Comment] FROM OurBranchStorageOut;";
	//        }
	//    }
	//    DB.SqlCmd.ExecuteNonQuery();
	//    DB.DBCon.Close();
	//    return "1";
	//}
	//   작업중
	[WebMethod]
	public String SetDeliveryPlaceByMember(string RequestFormPk, string ConsigneePk, string StorageCode, string Selected, string NOTSELECTEDBOXCOUNT, string BOXCOUNTLIMIT, string TOTALBOXCOUNT, string PackingUnit, string FromDate, string ArrivalTime, string WarehouseInfo, string StaffMobile, string DepositWhere, string AccountID) {
		try {
			DB = new DBConn();

			DB.SqlCmd.CommandText = "Declare @ID int; INSERT INTO TransportBC ([RequestFormPk], [CompanyPk], [FromDate], [ToDate], [WarehouseInfo], [WarehouseMobile], [PackedCount], [PackingUnit], [DepositWhere]) " +
		"	VALUES (" + Common.StringToDB(RequestFormPk, false, false) + ", " + Common.StringToDB(ConsigneePk, false, false) + ", " + Common.StringToDB(FromDate, true, false) + ", " +
				Common.StringToDB(ArrivalTime, true, false) + ", " + Common.StringToDB(WarehouseInfo, true, true) + ", " + Common.StringToDB(StaffMobile, true, false) + ", " +
				Common.StringToDB(Selected, false, false) + ", " + Common.StringToDB(PackingUnit, false, false) + ", " + Common.StringToDB(DepositWhere, false, false) + "); SELECT @ID=@@IDENTITY;";

			DB.SqlCmd.CommandText += "INSERT INTO OurBranchStorageOut (StorageCode, RequestFormPk, BoxCount, TransportBetweenCompanyPk) VALUES (" +
				"0, " +
				Common.StringToDB(RequestFormPk, false, false) + ", " +
				Common.StringToDB(Selected, false, false) + ", " +
				"@ID );";
			DB.SqlCmd.CommandText += new GetQuery().AddRequestHistory(RequestFormPk, "18", AccountID, "");
			//if (MemberMemo != "") {
			//	DB.SqlCmd.CommandText += "INSERT INTO [TransportBCMemo] ([TransportBetweenCompanyPk],[Memo]) VALUES (" +
			//						 "@ID ," +
			//						 Common.StringToDB(MemberMemo, true, true) + ");";
			//}

			DB.DBCon.Open();
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		} catch (Exception ex) {
			return ex.Message;
			throw;
		}
	}

	/// <summary>
	///
	/// </summary>
	/// <param name="OurBranchStorageOutPk"></param>
	/// <param name="RequestFormPk"></param>
	/// <param name="TransportBetweenCompanyPk"></param>
	/// <param name="ConsigneePk"></param>
	/// <param name="Selected"></param>
	/// <param name="NOTSELECTEDBOXCOUNT">같은 창고에 있는데 배차되지 않은것</param>
	/// <param name="BOXCOUNTLIMIT">한 창고에 있는 총 수량</param>
	/// <param name="TOTALBOXCOUNT">접수증 총 수량</param>
	/// <param name="PackingUnit"></param>
	/// <param name="FromDate"></param>
	/// <param name="ArrivalTime"></param>
	/// <param name="WarehouseInfo"></param>
	/// <param name="StaffMobile"></param>
	/// <param name="Memo"></param>
	/// <param name="DepositWhere"></param>
	/// <param name="Price"></param>
	/// <param name="Delivery_Type"></param>
	/// <param name="Delivery_Title"></param>
	/// <param name="Delivery_TEL"></param>
	/// <param name="Delivery_DriverName"></param>
	/// <param name="Delivery_DriverMobile"></param>
	/// <param name="Delivery_CarSize"></param>
	/// <returns></returns>
	[WebMethod]
	public String SetDeliveryPlace(string OurBranchStorageOutPk, string RequestFormPk, string TransportBetweenBranchPk, string TransportBetweenCompanyPk, string ConsigneePk, string StorageCode, string Selected, string NOTSELECTEDBOXCOUNT, string BOXCOUNTLIMIT, string TOTALBOXCOUNT, string PackingUnit, string FromDate, string ArrivalTime, string WarehouseInfo, string StaffMobile, string Memo, string DepositWhere, string Price, string Delivery_Type, string Delivery_Title, string Delivery_TEL, string Delivery_DriverName, string Delivery_DriverMobile, string Delivery_CarSize, string Delivery_Price, string AccountID) {
		try {
			DB = new DBConn();
			if (OurBranchStorageOutPk == "N") {
				DB.SqlCmd.CommandText = "Declare @ID int; Declare @HistoryCount int; INSERT INTO TransportBC ([RequestFormPk], [CompanyPk], [Type], [Title], [DriverName], [DriverTEL], [TEL], [CarSize], [FromDate], [ToDate], [WarehouseInfo], [WarehouseMobile], [PackedCount], [PackingUnit], [DepositWhere], [Price],[DeliveryPrice], [Memo]) " +
			"	VALUES (" + Common.StringToDB(RequestFormPk, false, false) + ", " + Common.StringToDB(ConsigneePk, false, false) + ", " + Common.StringToDB(Delivery_Type, true, true) + ", " +
					Common.StringToDB(Delivery_Title, true, true) + ", " + Common.StringToDB(Delivery_DriverName, true, true) + ", " + Common.StringToDB(Delivery_DriverMobile, true, false) + ", " +
					Common.StringToDB(Delivery_TEL, true, false) + ", " + Common.StringToDB(Delivery_CarSize, true, false) + ", " + Common.StringToDB(FromDate, true, false) + ", " +
					Common.StringToDB(ArrivalTime, true, false) + ", " + Common.StringToDB(WarehouseInfo, true, true) + ", " + Common.StringToDB(StaffMobile, true, false) + ", " +
					Common.StringToDB(Selected, false, false) + ", " + Common.StringToDB(PackingUnit, false, false) + ", " + Common.StringToDB(DepositWhere, false, false) + ", " +
					Common.StringToDB(Price, false, false) + ", " + Common.StringToDB(Delivery_Price, false, false) + ", " + Common.StringToDB(Memo, true, true) + "); SELECT @ID=@@IDENTITY;";
				DB.SqlCmd.CommandText += new GetQuery().AddRequestHistory(RequestFormPk, "18", AccountID, "");
				//if (MemberMemo != "") {
				//	DB.SqlCmd.CommandText += "INSERT INTO [TransportBCMemo] ([TransportBetweenCompanyPk],[Memo]) VALUES (" +
				//							 "@ID ," +
				//							 Common.StringToDB(MemberMemo, true, true) + ");";
				//}
				if (TOTALBOXCOUNT == Selected) {
					DB.SqlCmd.CommandText += "INSERT INTO OurBranchStorageOut (StorageCode, RequestFormPk, BoxCount, TransportBetweenCompanyPk) VALUES (" +
						"0, " +
						Common.StringToDB(RequestFormPk, false, false) + ", " +
						Common.StringToDB(Selected, false, false) + ", " +
						"@ID );";
				}
				else {
					DB.SqlCmd.CommandText += "INSERT INTO OurBranchStorageOut (StorageCode, RequestFormPk, BoxCount, TransportBetweenCompanyPk) VALUES (" +
						"0, " +
						Common.StringToDB(RequestFormPk, false, false) + ", " +
						Common.StringToDB(Selected, false, false) + ", " +
						"@ID );" +
					"INSERT INTO OurBranchStorageOut (StorageCode, RequestFormPk, BoxCount, TransportBetweenCompanyPk) VALUES (" +
						"0, " +
						Common.StringToDB(RequestFormPk, false, false) + ", " +
						Common.StringToDB((Int32.Parse(TOTALBOXCOUNT) - Int32.Parse(Selected)) + "", false, false) + ", " +
						"NULL );";
				}
				DB.SqlCmd.CommandText += @" select @HistoryCount=count(*) from TransportBCHistory where TransportBetweenCompanyPk=@ID
if @HistoryCount=0
INSERT INTO TransportBCHistory (TransportBetweenCompanyPk, GubunCL, ActID, Comment, Registerd) VALUES (@ID,0, '" + AccountID + @"', '', getDate());
else
INSERT INTO TransportBCHistory (TransportBetweenCompanyPk, GubunCL, ActID, Comment, Registerd) VALUES (@ID,1, '" + AccountID + @"', '', getDate());";
				DB.SqlCmd.CommandText += "";
			}
			else {
				Int32 intSelected = Int32.Parse(Selected);
				Int32 intBOXCOUNTLIMIT = Int32.Parse(BOXCOUNTLIMIT);
				Int32 intNOTSELECTEDBOXCOUNT = Int32.Parse(NOTSELECTEDBOXCOUNT);

				if (TransportBetweenCompanyPk == "N" || TransportBetweenCompanyPk == "") {
					DB.SqlCmd.CommandText = "Declare @ID int; Declare @HistoryCount int; INSERT INTO TransportBC ([RequestFormPk], [CompanyPk], [Type], [Title], [DriverName], [DriverTEL], [TEL], [CarSize], [FromDate], [ToDate], [WarehouseInfo], [WarehouseMobile], [PackedCount], [PackingUnit], [DepositWhere], [Price],[DeliveryPrice], [Memo]) " +
			"	VALUES (" + Common.StringToDB(RequestFormPk, false, false) + ", " + Common.StringToDB(ConsigneePk, false, false) + ", " + Common.StringToDB(Delivery_Type, true, true) + ", " +
					Common.StringToDB(Delivery_Title, true, true) + ", " + Common.StringToDB(Delivery_DriverName, true, true) + ", " + Common.StringToDB(Delivery_DriverMobile, true, false) + ", " +
					Common.StringToDB(Delivery_TEL, true, false) + ", " + Common.StringToDB(Delivery_CarSize, true, false) + ", " + Common.StringToDB(FromDate, true, false) + ", " +
					Common.StringToDB(ArrivalTime, true, false) + ", " + Common.StringToDB(WarehouseInfo, true, true) + ", " + Common.StringToDB(StaffMobile, true, false) + ", " +
					Common.StringToDB(Selected, false, false) + ", " + Common.StringToDB(PackingUnit, false, false) + ", " + Common.StringToDB(DepositWhere, false, false) + ", " +
					Common.StringToDB(Price, false, false) + ", " + Common.StringToDB(Delivery_Price, false, false) + ", " + Common.StringToDB(Memo, true, true) + "); SELECT @ID=@@IDENTITY;";
					DB.SqlCmd.CommandText += new GetQuery().AddRequestHistory(RequestFormPk, "18", AccountID, "");
					//if (MemberMemo != "") {
					//	DB.SqlCmd.CommandText += "INSERT INTO [TransportBCMemo] ([TransportBetweenCompanyPk],[Memo]) VALUES (" +
					//							 "@ID ," +
					//							 Common.StringToDB(MemberMemo, true, true) + ");";
					//}

					if (intSelected == intBOXCOUNTLIMIT) {
						DB.SqlCmd.CommandText += "UPDATE OurBranchStorageOut " +
							" SET [TransportBetweenCompanyPk] = @ID " +
							" WHERE OurBranchStorageOutPk=" + OurBranchStorageOutPk + ";";
					}
					else {
						DB.SqlCmd.CommandText += "INSERT INTO OurBranchStorageOut (StorageCode, RequestFormPk, BoxCount, TransportBetweenBranchPk,  TransportBetweenCompanyPk) VALUES (" +
							StorageCode + ", " + RequestFormPk + ", " + Selected + ", " + Common.StringToDB(TransportBetweenBranchPk, false, false) + ", @ID );" +
							"UPDATE OurBranchStorageOut SET [BoxCount] = " + (intBOXCOUNTLIMIT - intSelected) + " WHERE OurBranchStorageOutPk=" + OurBranchStorageOutPk + ";";
					}
					DB.SqlCmd.CommandText += @" select @HistoryCount=count(*) from TransportBCHistory where TransportBetweenCompanyPk=@ID
if @HistoryCount=0
INSERT INTO TransportBCHistory (TransportBetweenCompanyPk, GubunCL, ActID, Comment, Registerd) VALUES (@ID,0, '" + AccountID + @"', '', getDate());
else
INSERT INTO TransportBCHistory (TransportBetweenCompanyPk, GubunCL, ActID, Comment, Registerd) VALUES (@ID,1, '" + AccountID + @"', '', getDate());";
				}
				else {
					//DB.SqlCmd.CommandText = "select count(*) from TransportBCMemo WHERE [TransportBetweenCompanyPk] = " + TransportBetweenCompanyPk + ";";
					//DB.DBCon.Open();
					//string MemoCount = DB.SqlCmd.ExecuteScalar() + "";
					//DB.DBCon.Close();

					DB.SqlCmd.CommandText = "Declare @HistoryCount int; UPDATE TransportBC SET " +
						"  [Type] = " + Common.StringToDB(Delivery_Type, true, true) +
						", [Title] = " + Common.StringToDB(Delivery_Title, true, true) +
						", [DriverName] = " + Common.StringToDB(Delivery_DriverName, true, true) +
						", [DriverTEL] = " + Common.StringToDB(Delivery_DriverMobile, true, false) +
						", [TEL] = " + Common.StringToDB(Delivery_TEL, true, false) +
						", [CarSize] = " + Common.StringToDB(Delivery_CarSize, true, false) +
						", [FromDate] = " + Common.StringToDB(FromDate, true, false) +
						", [ToDate] = " + Common.StringToDB(ArrivalTime, true, false) +
						", [WarehouseInfo] = " + Common.StringToDB(WarehouseInfo, true, true) +
						", [WarehouseMobile] = " + Common.StringToDB(StaffMobile, true, false) +
						", [PackedCount] = " + Common.StringToDB(Selected, false, false) +
						", [PackingUnit] = " + Common.StringToDB(PackingUnit, false, false) +
						", [DepositWhere] = " + Common.StringToDB(DepositWhere, false, false) +
						", [Price] = " + Common.StringToDB(Price, false, false) +
						", [DeliveryPrice] = " + Common.StringToDB(Delivery_Price, false, false) +
						", [Memo] = " + Common.StringToDB(Memo, true, true) +
						" WHERE TransportBetweenCompanyPk=" + TransportBetweenCompanyPk + ";";
					DB.SqlCmd.CommandText += new GetQuery().AddRequestHistory(RequestFormPk, "18", AccountID, "");

					//if (MemberMemo != "") {
					//	if (MemoCount == "0") {
					//		DB.SqlCmd.CommandText += "INSERT INTO [TransportBCMemo] ([TransportBetweenCompanyPk],[Memo]) VALUES (" +
					//								 Common.StringToDB(TransportBetweenCompanyPk, true, true) + "," +
					//								 Common.StringToDB(MemberMemo, true, true) + ");";
					//	} else {
					//		DB.SqlCmd.CommandText += "UPDATE [TransportBCMemo] " +
					//							 " SET [Memo] = " + Common.StringToDB(MemberMemo, true, true) +
					//							 " WHERE [TransportBetweenCompanyPk] = " + TransportBetweenCompanyPk + ";";
					//	}
					//} else {
					//	if (MemoCount == "0") {
					//	} else {
					//		DB.SqlCmd.CommandText += "UPDATE [TransportBCMemo] " +
					//							 " SET [Memo] = " + Common.StringToDB(MemberMemo, true, true) +
					//							 " WHERE [TransportBetweenCompanyPk] = " + TransportBetweenCompanyPk + ";";
					//	}
					//}

					if (intSelected == intBOXCOUNTLIMIT - intNOTSELECTEDBOXCOUNT) {
						DB.SqlCmd.CommandText += "UPDATE OurBranchStorageOut " +
							" SET [TransportBetweenCompanyPk] = " + TransportBetweenCompanyPk +
							" WHERE OurBranchStorageOutPk=" + OurBranchStorageOutPk + ";";
					}
					else if (intSelected == intBOXCOUNTLIMIT) {
						DB.SqlCmd.CommandText += "UPDATE OurBranchStorageOut SET [BoxCount] = " + Selected + ", [TransportBetweenCompanyPk]=" + TransportBetweenCompanyPk + " WHERE OurBranchStorageOutPk=" + OurBranchStorageOutPk + ";" +
							"DELETE FROM OurBranchStorageOut WHERE [RequestFormPk]=" + RequestFormPk + " and isnull([TransportBetweenCompanyPk], 0)=0;";
					}
					else if (intNOTSELECTEDBOXCOUNT > 0) {
						DB.SqlCmd.CommandText += "UPDATE OurBranchStorageOut SET [BoxCount] = " + Selected + ", [TransportBetweenCompanyPk] = " + TransportBetweenCompanyPk + " WHERE OurBranchStorageOutPk=" + OurBranchStorageOutPk + ";" +
							"UPDATE OurBranchStorageOut SET [BoxCount] = " + (intBOXCOUNTLIMIT - intSelected) + " WHERE [RequestFormPk]=" + RequestFormPk + " and isnull([TransportBetweenCompanyPk], 0)=0;";
					}
					else {
						DB.SqlCmd.CommandText += "UPDATE OurBranchStorageOut SET [BoxCount] = " + Selected + ", [TransportBetweenCompanyPk] = " + TransportBetweenCompanyPk + " WHERE OurBranchStorageOutPk=" + OurBranchStorageOutPk + ";" +
						"INSERT INTO OurBranchStorageOut (StorageCode, RequestFormPk, BoxCount, TransportBetweenBranchPk) VALUES (" +
							StorageCode + ", " + RequestFormPk + ", " + (intBOXCOUNTLIMIT - intSelected) + ", " + Common.StringToDB(TransportBetweenBranchPk, false, false) + ");";
					}
					DB.SqlCmd.CommandText += @" select @HistoryCount=count(*) from TransportBCHistory where TransportBetweenCompanyPk=" + TransportBetweenCompanyPk + @"
if @HistoryCount=0
INSERT INTO TransportBCHistory (TransportBetweenCompanyPk, GubunCL, ActID, Comment, Registerd) VALUES ('" + TransportBetweenCompanyPk + @"',0, '" + AccountID + @"', '', getDate());
else
INSERT INTO TransportBCHistory (TransportBetweenCompanyPk, GubunCL, ActID, Comment, Registerd) VALUES ('" + TransportBetweenCompanyPk + @"',1, '" + AccountID + @"', '', getDate());";
				}
			}
			//return DB.SqlCmd.CommandText;
			DB.DBCon.Open();
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		} catch (Exception ex) {
			return ex.Message;
			throw;
		}
	}

	[WebMethod]
	public String[] AddDeliveryPlaceOnload(string ConsigneePk) {
		List<string> ReturnValue = new List<string>();

		ReturnValue.Add(LoadWareHouse(ConsigneePk));
		ReturnValue.Add(LoadBeforeDeliveryPlace(ConsigneePk));
		ReturnValue.Add(LoadTalkBusiness(ConsigneePk, "Delivery"));
		//ReturnValue.Add(LoadRequestInformation(RequestFormPk));

		return ReturnValue.ToArray();
	}

	[WebMethod]
	public String LoadTransportBCHistory(string TransportBetweenCompanyPk) {
		StringBuilder ReturnValue = new StringBuilder();
		DB = new DBConn();

		DB.SqlCmd.CommandText = string.Format(@"SELECT
       T.[TransportBCHistoryPk]
      ,T.[TransportBetweenCompanyPk]
      ,T.[GubunCL]
      ,T.[ActID]
      ,T.[Comment]
      ,T.[Registerd]
      ,A.[Name]
  FROM [dbo].[TransportBCHistory] T
left join Account_ A  on T.ActID= A.AccountID
where [TransportBetweenCompanyPk]={0}", TransportBetweenCompanyPk);
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append("<p>" + Common.GetTransportBCHistoryGubunCL(RS["GubunCL"] + "") + " : <strong>" + RS["ActID"].ToString() + "</strong>  " + RS["Name"].ToString() + " / " + RS["Registerd"].ToString() + "</p>");
		}
		DB.DBCon.Close();
		return ReturnValue + "";
	}

	public String LoadTalkBusiness(string CompanyPk, string GubunCL) {
		DB = new DBConn();
		DB.DBCon.Open();
		StringBuilder ReturnValue = new StringBuilder();
		HistoryC HisC = new HistoryC();
		List<sComment> Delivery = new List<sComment>();
		Delivery = HisC.LoadList_Comment("Company", CompanyPk, "'" + GubunCL + "'", ref DB);
		for (int i = 0; i < Delivery.Count; i++) {
			ReturnValue.Append(Delivery[i].Account_Id + "@@" + Delivery[i].Contents + "@@" + Delivery[i].Registerd.ToString().Substring(0, 10));
		}
		DB.DBCon.Close();
		return ReturnValue + "";
	}

	private String LoadWareHouse(string CompanyPk) {
		DB = new DBConn();
		DB.DBCon.Open();
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = "SELECT CompanyName FROM Company WHERE CompanyPk=" + CompanyPk + ";";
		ReturnValue.Append(DB.SqlCmd.ExecuteScalar());

		DB.SqlCmd.CommandText = "SELECT [Title], [Address], [TEL], [Staff], [Mobile], [Memo] FROM [CompanyWarehouse] WHERE CompanyPk=" + CompanyPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append("####" + RS["Title"] + "@@" + RS["Address"] + "@@" + RS["TEL"] + "@@" + RS["Staff"] + "@@" + RS["Mobile"] + "@@" + RS["Memo"]);
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue + "";
	}

	private String LoadBeforeDeliveryPlace(string CompanyPk) {
		DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"
SELECT TOP 5
	TBC.Type, TBC.Title, TBC.DriverName, TBC.DriverTEL, TBC.TEL, TBC.CarSize, TBC.ToDate, TBC.WarehouseInfo, TBC.WarehouseMobile
	, TBC.PackedCount, TBC.PackingUnit, TBC.DepositWhere, TBC.Price
FROM RequestForm AS R
	left join TransportBC AS TBC ON R.RequestFormPk=TBC.RequestFormPk
WHERE ConsigneePk=" + CompanyPk + @"
order by R.ArrivalDate DESC ;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder ReturnValue = new StringBuilder();
		while (RS.Read()) {
			ReturnValue.Append(RS["Type"] + "#@!" +
				RS["Title"] + "#@!" +
				RS["DriverName"] + "#@!" +
				RS["DriverTEL"] + "#@!" +
				RS["TEL"] + "#@!" +
				RS["CarSize"] + "#@!" +
				RS["ToDate"] + "#@!" +
				RS["WarehouseInfo"] + "#@!" +
				RS["WarehouseMobile"] + "#@!" +
				RS["PackedCount"] + "#@!" +
				RS["PackingUnit"] + "#@!" +
				RS["DepositWhere"] + "#@!" +
				Common.NumberFormat(RS["Price"] + "") + "%!$@#");
			//ReturnValue.Append(RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5] + "#@!" + RS[6] + "#@!" + RS[7] + "#@!" + RS[8] + "#@!" + RS[9] + "#@!" + RS[10] + "#@!" + RS[11] + "#@!" + RS[12] + "#@!" + RS[13] + "#@!" + RS[14] + "%!$@#");
		}
		RS.Dispose();
		DB.DBCon.Close();
		if (ReturnValue + "" == "") {
			return "N";
		}
		return ReturnValue + "";
	}

	// Conversion 중 - 조회안됨 얘.....FROM OurBranchStorage WHERE [StatusCL]=4 없음
	private String LoadRequestInformation(string RequestFormPk) {
		DB.SqlCmd.CommandText = @"
	SELECT RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume, RF.TotalPackedCount-OBS.BoxCount, R.ArrivalDate, SC.StorageCode
	FROM [dbo].[RequestForm] AS RF
		left join (
			SELECT [RequestFormPk], SUM([BoxCount]) AS BoxCount FROM OurBranchStorageOut WHERE [StatusCL]=5 Group By RequestFormPk
			) AS OBS  ON RF.RequestFormPk=OBS.RequestFormPk
		left join (
			SELECT [RequestFormPk], [StorageCode] FROM OurBranchStorage WHERE [StatusCL]=4
			) AS SC  ON RF.RequestFormPk=SC.RequestFormPk
		left join RequestForm AS R On RF.RequestFormPk=R.RequestFormPk
	WHERE RF.RequestFormPk=" + RequestFormPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string ReturnValue = "N";
		if (RS.Read()) {
			ReturnValue = RS[0] + "#@!" + Common.GetPackingUnit("" + RS[1]) + "#@!" + Common.NumberFormat("" + RS[2]) + "#@!" + Common.NumberFormat("" + RS[3]) + "#@!" + RS[4] + "#@!" + RS[1] + "#@!" + RS["ArrivalDate"] + "#@!" + RS["StorageCode"];
		}
		return ReturnValue;
	}

	[WebMethod]
	public String SetDocumentStepCLTo(string CommercialDocumentPk, string ToValue, string AccountID) {
		StringBuilder Query = new StringBuilder();
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [RequestFormPk] FROM CommerdialConnectionWithRequest WHERE [CommercialDocumentPk]=" + CommercialDocumentPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (ToValue == "6") {
				Query.Append(new GetQuery().AddRequestHistory(RS["RequestFormPk"] + "", ToValue, AccountID, "") +
									"	UPDATE RequestForm SET StepCL=63, [DocumentStepCL] = " + ToValue + " WHERE RequestFormPk=" + RS["RequestFormPk"] + ";");
			}
			else if (ToValue == "2") {
				Query.Append(new GetQuery().AddRequestHistory(RS["RequestFormPk"] + "", ToValue, AccountID, "") +
									"	UPDATE RequestForm SET [DocumentStepCL] = " + ToValue + " , GubunCL=null WHERE RequestFormPk=" + RS["RequestFormPk"] + ";");
			}
			else {
				Query.Append(new GetQuery().AddRequestHistory(RS["RequestFormPk"] + "", ToValue, AccountID, "") +
									"	UPDATE RequestForm SET [DocumentStepCL] = " + ToValue + " WHERE RequestFormPk=" + RS["RequestFormPk"] + ";");
			}
		}

		switch (ToValue) {
			case "8":
				Query.Append("INSERT INTO Highlighter (GubunCL, GubunPk, Color) VALUES (1, " + CommercialDocumentPk + ", 0);");
				break;

			case "9":
				Query.Append("INSERT INTO Highlighter (GubunCL, GubunPk, Color) VALUES (1, " + CommercialDocumentPk + ", 1);");
				break;

			case "13":
				Query.Append("DELETE FROM Highlighter WHERE [GubunPk]=" + CommercialDocumentPk + " and [GubunCL] in (0, 1);");
				break;

			case "14":
				Query.Append("DELETE FROM Highlighter WHERE [GubunPk]=" + CommercialDocumentPk + " and [GubunCL] in (0, 1);");
				break;

			case "15":
				Query.Append("DELETE FROM Highlighter WHERE [GubunPk]=" + CommercialDocumentPk + " and [GubunCL] in (0, 1);");
				break;

			default:
				break;
		}

		RS.Dispose();
		DB.SqlCmd.CommandText = Query + "";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String SetDocumentStepCLTo_SungSim(string CommercialDocumentPk, string ToValue, string AccountID) {
		StringBuilder Query = new StringBuilder();
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [RequestFormPk] FROM CommerdialConnectionWithRequest WHERE [CommercialDocumentPk]=" + CommercialDocumentPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			Query.Append(new GetQuery().AddRequestHistory(RS["RequestFormPk"] + "", ToValue, AccountID, "") +
								"	UPDATE RequestForm SET [DocumentStepCL] = " + ToValue + " , GubunCL=1 WHERE RequestFormPk=" + RS["RequestFormPk"] + ";");
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = Query + "";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public string AppendRequestFormHistory(string RequestForm, string Code, string AccountID) {
		HistoryC HisC = new HistoryC();
		sHistory History = new sHistory();
		DB = new DBConn();
		DB.DBCon.Open();
		History.Table_Name = "RequestForm";
		History.Table_Pk = RequestForm;
		History.Code = Code;
		History.Account_Id = AccountID;
		HisC.Set_History(History, ref DB);
		DB.DBCon.Close();
		return "1";
	}

	//20131224 관세사 관련작업 requestform에서 통관지시
	[WebMethod]
	public String SetDocumentStepCLToRequestView(string RequestFormPk, string AccountID) {
		StringBuilder Query = new StringBuilder();
		DB = new DBConn();
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = "UPDATE RequestForm SET StepCL=63, [DocumentStepCL] = 6 WHERE RequestFormPk=" + RequestFormPk + ";" +
								new GetQuery().AddRequestHistory(RequestFormPk, "6", AccountID, "");
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String SetDocumentStepCLToMultiple(string CommercialDocumentPk, string ToValue, string AccountID) {
		try {
			StringBuilder Query = new StringBuilder();
			HistoryC HisC = new HistoryC();
			sHistory History = new sHistory();
			DB = new DBConn();
			DB.SqlCmd.CommandText = "SELECT [RequestFormPk] FROM CommerdialConnectionWithRequest WHERE [CommercialDocumentPk] in (" + CommercialDocumentPk + ");";
			DB.DBCon.Open();
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				if (ToValue == "6") {
					History = new sHistory();
					History.Table_Name = "RequestForm";
					History.Table_Pk = RS["RequestFormPk"] + "";
					History.Code = ToValue;
					History.Account_Id = AccountID;
					HisC.Set_History(History, ref DB);
					Query.Append("	UPDATE RequestForm SET StepCL=63, [DocumentStepCL] = " + ToValue + " WHERE RequestFormPk=" + RS["RequestFormPk"] + ";");
				}
				else if (ToValue == "10" || ToValue == "11" || ToValue == "12") {
					History = new sHistory();
					History.Table_Name = "RequestForm";
					History.Table_Pk = RS["RequestFormPk"] + "";
					History.Code = "761";
					History.Account_Id = AccountID;
					History.Description = "세납지시";
					HisC.Set_History(History, ref DB);

					History = new sHistory();
					History.Table_Name = "RequestForm";
					History.Table_Pk = RS["RequestFormPk"] + "";
					History.Code = ToValue;
					History.Account_Id = AccountID;
					HisC.Set_History(History, ref DB);
					Query.Append("	UPDATE RequestForm SET [DocumentStepCL] = " + ToValue + " WHERE RequestFormPk=" + RS["RequestFormPk"] + ";");
				}
				else {
					History = new sHistory();
					History.Table_Name = "RequestForm";
					History.Table_Pk = RS["RequestFormPk"] + "";
					History.Code = ToValue;
					History.Account_Id = AccountID;
					HisC.Set_History(History, ref DB);
					Query.Append("	UPDATE RequestForm SET [DocumentStepCL] = " + ToValue + " WHERE RequestFormPk=" + RS["RequestFormPk"] + ";");
				}
			}

			if (ToValue == "13" || ToValue == "14" || ToValue == "15") {
				Query.Append("DELETE FROM Highlighter WHERE [GubunPk] in (" + CommercialDocumentPk + ") and [GubunCL] in (0, 1);");
			}

			RS.Dispose();
			DB.SqlCmd.CommandText = Query + "";
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		} catch (Exception ex) {
			return ex.Message + "/r/n" + DB.SqlCmd.CommandText;
		}
	}

	[WebMethod]
	public String DeleteCompany(string CompanyPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE Company SET GubunCL=5 WHERE CompanyPk=" + CompanyPk + "; " +
			"UPDATE Account_ SET AccountID=null WHERE CompanyPk=" + CompanyPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String HardDeleteCompany(string CompanyPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "DELETE Company  WHERE CompanyPk=" + CompanyPk + "; " +
			"DELETE Account_ WHERE CompanyPk=" + CompanyPk + ";" +
			"DELETE RequestForm  where ShipperPk=" + CompanyPk + "; " +
			"DELETE RequestForm  where ConsigneePk=" + CompanyPk + ";" +
			"DELETE CompanyRelation WHERE MainCompanyPk=" + CompanyPk + ";" +
			"DELETE CompanyRelation WHERE TargetCompanyPk=" + CompanyPk + ";";

		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String RestoreCompany(string CompanyPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE Company SET GubunCL=1 WHERE CompanyPk=" + CompanyPk + "; ";

		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	//BMK Admin/PrepareDelivery.aspx
	[WebMethod]
	public String GoDeliveryOrder(string TransportBetweenBranchPk, string RequestFormPk, string AccountID, string isDepositedTorF) {
		DB = new DBConn();
		DB.DBCon.Open();

		if (RequestFormPk == "") {
			DB.SqlCmd.CommandText = "SELECT TOP 1 [RequestFormPk] FROM OurBranchStorageOut WHERE TransportBetweenCompanyPk=" + TransportBetweenBranchPk + " ;";
			RequestFormPk = DB.SqlCmd.ExecuteScalar() + "";
		}
		if (isDepositedTorF == "") {
			DB.SqlCmd.CommandText = "SELECT [ShipperCharge], [ConsigneeCharge], [ShipperDepositedDate], [ConsigneeDepositedDate] FROM RequestFormCalculateHead WHERE RequestFormPk=" + RequestFormPk + ";";
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			isDepositedTorF = "T";
			if (RS.Read()) {
				if (RS["ShipperCharge"] + "" != "0.0000" && RS["ShipperDepositedDate"] + "" == "") {
					isDepositedTorF = "F";
				}
				if (RS["ConsigneeCharge"] + "" != "0.0000" && RS["ConsigneeDepositedDate"] + "" == "") {
					isDepositedTorF = "F";
				}
			}
			RS.Dispose();
		}

		DB.SqlCmd.CommandText = "	SELECT SUM(BoxCount) FROM OurBranchStorageOut WHERE RequestFormPk=" + RequestFormPk + " and StatusCL<6 and TransportBetweenCompanyPk<>" + TransportBetweenBranchPk + ";";
		String StorageLeft = DB.SqlCmd.ExecuteScalar() + "";
		if (StorageLeft == "") {
			StorageLeft = "0";
		}
		if (StorageLeft == "0" && isDepositedTorF == "F") {
			DB.SqlCmd.CommandText = "UPDATE OurBranchStorageOut SET [StatusCL] = 6 WHERE  TransportBetweenCompanyPk=" + TransportBetweenBranchPk + "; " +
				"UPDATE RequestForm SET [StepCL] = 65 WHERE RequestFormPk=" + RequestFormPk + "; " +
				new GetQuery().AddRequestHistory(RequestFormPk, "66", AccountID, "");
		}
		else {
			DB.SqlCmd.CommandText = "UPDATE OurBranchStorageOut SET [StatusCL] = 6 WHERE  TransportBetweenCompanyPk=" + TransportBetweenBranchPk + ";" +
				(StorageLeft == "0" ? "UPDATE RequestForm SET [StepCL] = 65 WHERE RequestFormPk=" + RequestFormPk + ";" : "") +
				new GetQuery().AddRequestHistory(RequestFormPk, "65", AccountID, "");
		}
		DB.SqlCmd.CommandText += new GetQuery().AddTransportBCHistory(TransportBetweenBranchPk, "10", AccountID, "");
		DB.SqlCmd.ExecuteNonQuery();

		///////////////////////////////////
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String SendAccountInfoBySMS(string CompanyName, string Name, string Mobile, string ID) {
		Email emailsend = new Email("Local");
		winic.Service1 ws = new winic.Service1();

		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [Password] FROM Account_ WHERE AccountID='" + ID + "';";
		DB.DBCon.Open();
		string password = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();

		string phoneNo = Mobile.Replace("-", "");
		string phoneFirst3 = phoneNo.Substring(0, 3);
		if (phoneFirst3 == "010" || phoneFirst3 == "011" || phoneFirst3 == "016" || phoneFirst3 == "017" || phoneFirst3 == "018" || phoneFirst3 == "019" || phoneFirst3 == "070") {
			try {
				emailsend.SendSMSKorea("0327728481", phoneNo, Name + "님 문의하신 계정정보 알려드립니다. ID : " + ID + " / PW : " + password + "  -아이엘");
			} catch (Exception) {
			}
		}

		if (phoneFirst3 == "139" || phoneFirst3 == "138" || phoneFirst3 == "137" || phoneFirst3 == "136" || phoneFirst3 == "135" || phoneFirst3 == "134" || phoneFirst3 == "159" || phoneFirst3 == "158" || phoneFirst3 == "152" || phoneFirst3 == "150" || phoneFirst3 == "188" || phoneFirst3 == "130" || phoneFirst3 == "131" || phoneFirst3 == "132" || phoneFirst3 == "155" || phoneFirst3 == "156" || phoneFirst3 == "186" || phoneFirst3 == "133" || phoneFirst3 == "153" || phoneFirst3 == "189" || phoneFirst3 == "185") {
			try {
				string resultmsgTochina = ws.SendMessages("intl2000", "ythq1717", phoneNo, "爱尔国际综合物流有限公司：短信通知您ID及密码信息. ID : " + ID + " / Password : " + password, "");
			} catch (Exception) {
			}
		}
		return "1";
	}

	[WebMethod]
	public String[] LoadDeliveryReceiptPk(string RequestFormPk) {
		DB = new DBConn();
		List<string> returnValue = new List<string>();
		DB.SqlCmd.CommandText = "SELECT [TransportBetweenCompanyPk] FROM TransportBC WHERE RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			returnValue.Add(RS[0] + "");
		}
		RS.Dispose();
		DB.DBCon.Close();
		return returnValue.ToArray();
	}

	[WebMethod]
	public String SetHighlight(string type, string Gubun, string Pk, string SetValue) {
		DB = new DBConn();
		switch (Gubun) {
			case "N":
				DB.SqlCmd.CommandText = "INSERT INTO Highlighter ([GubunCL], [GubunPk], [Color]) VALUES (" + type + ", " + Pk + ", 0);";
				break;

			case "A":
				DB.SqlCmd.CommandText = "UPDATE Highlighter SET [Color] = " + SetValue + " WHERE HighlighterPk=" + Pk + ";";
				break;

			case "D":
				DB.SqlCmd.CommandText = "DELETE FROM Highlighter WHERE HighlighterPk=" + Pk + ";";
				break;
		}
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String TransportBBCommentDELETE(string Pk) { //TransportBetweenBranchView.aspx에서 호출하는데 Conversion 후 -> TransportView.aspx에서 새로 ajax
		DB = new DBConn();
		DB.SqlCmd.CommandText = "DELETE FROM TransportBBComment WHERE TransportBBCommentPk=" + Pk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();

		return "1";
	}

	//[WebMethod]
	//public String[] DepositMinusCharge(string CompanyPk)
	//{
	//    DB = new DBConn();
	//    DB.SqlCmd.CommandText = "EXEC SP_DepositMinusCharge @CompanyPk=" + CompanyPk + ";";
	//    DB.DBCon.Open();
	//    SqlDataReader RS = DB.SqlCmd.ExecuteReader();
	//    List<string> MonetaryUnit = new List<string>();
	//    List<decimal> result = new List<decimal>();

	//    while (RS.Read()) {
	//        if (RS[1] + "" != "0.0000") {
	//            DepositMinusChargeCalc(ref MonetaryUnit, ref result, false, RS[0] + "", RS[1] + "");
	//        }
	//    }

	//    for (int i = 0; i < MonetaryUnit.Count; i++) {
	//        if (result[i] > 0) {
	//            MonetaryUnit[i] = "<span style=\"color:blue;\">" + Common.GetMonetaryUnit(MonetaryUnit[i]) + " " + Common.NumberFormat(result[i] + "") + "</span>";
	//        } else {
	//            MonetaryUnit[i] = "<span style=\"color:red;\">" + Common.GetMonetaryUnit(MonetaryUnit[i]) + " " + Common.NumberFormat(result[i] + "") + "</span>";
	//        }
	//    }
	//    return MonetaryUnit.ToArray();
	//}
	//private void DepositMinusChargeCalc(ref List<string> ListMonetaryUnit, ref List<decimal> ListValue, bool isPlus, string MonetaryUnit, string Value)
	//{
	//    int isin = -1;
	//    for (int i = 0; i < ListMonetaryUnit.Count; i++) {
	//        if (ListMonetaryUnit[i] == MonetaryUnit) {
	//            isin = i;
	//            break;
	//        }
	//    }
	//    if (isin == -1) {
	//        ListMonetaryUnit.Add(MonetaryUnit);
	//        if (isPlus)
	//            ListValue.Add(decimal.Parse(Value));
	//        else
	//            ListValue.Add(decimal.Parse(Value) * -1);
	//    } else {
	//        if (isPlus)
	//            ListValue[isin] += decimal.Parse(Value);
	//        else
	//            ListValue[isin] += decimal.Parse(Value) * -1;
	//    }
	//}

	[WebMethod]
	public String[] LoadDepositedData(string RequestFormDepositedPk) {
		String[] ReturnValue;
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [BankAccountPk], [MonetaryUnitCL], [Charge], [DepositedDate], [Memo] FROM RequestFormDeposited WHERE [RequestFormDepositedPk]=" + RequestFormDepositedPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue = new String[5] {
				RS[0]+"",
				Common.GetMonetaryUnit(RS[1]+""),
				Common.NumberFormat(RS[2]+""),
				RS[3]+"",
				RS[4]+""
			};
		}
		else {
			ReturnValue = new string[] { "N" };
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue;
	}

	[WebMethod]
	public String InsertComment(string Gubun, string Pk, string AccountID, string Comment) {
		if (Gubun == "F") {
			DB = new DBConn();
			DB.SqlCmd.CommandText = "INSERT INTO FileComment ([FilePk], AccountID, Comment) VALUES (" + Pk + ", '" + AccountID + "', " + Common.StringToDB(Comment, true, true) + ");";
			DB.DBCon.Open();
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		}
		else {
			return "0";
		}
	}

	[WebMethod]
	public String DeleteforRequestCommentsNCustom(string Gubun, string Pk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"delete
  FROM [dbo].[COMMENT]
  where [ACCOUNT_ID] in(SELECT [ACCOUNT_ID]	FROM [dbo].[COMMENT] WHERE [COMMENT_PK]=" + Pk + @")
  and [CONTENTS] in(SELECT [CONTENTS]	FROM [dbo].[COMMENT] WHERE [COMMENT_PK]=" + Pk + @")
  and [REGISTERD] in(SELECT [REGISTERD]	FROM [dbo].[COMMENT] WHERE [COMMENT_PK]=" + Pk + @");";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();

		return "1";
	}

	[WebMethod]
	public String DeleteComment(string Gubun, string Pk) {
		DB = new DBConn();
		if (Gubun == "F") {
			DB.SqlCmd.CommandText = "SELECT [FileAttachedPk] FROM FileComment WHERE [FileCommentPk]=" + Pk + ";";
			DB.DBCon.Open();
			string FilePk = DB.SqlCmd.ExecuteScalar() + "";
			DB.SqlCmd.CommandText = "DELETE FROM FileComment WHERE FileCommentPk=" + Pk + ";" + (FilePk == "" ? "" : " DELETE FROM [File] WHERE [FilePk]=" + FilePk + ";");
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
		}
		else {
			DB.SqlCmd.CommandText = "DELETE FROM [dbo].[COMMENT] WHERE [COMMENT_PK]=" + Pk + ";";
			DB.DBCon.Open();
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
		}
		return "1";
	}

	[WebMethod]
	public String EmailSend(string FromEmail, string FromName, string ToEmail, string ToName, string Title, string Body) {
		Email E = new Email();
		E.SendMailByCafe24(FromEmail, FromName, ToEmail, ToName, Title, Body);
		return "1";
	}

	[WebMethod]
	public String RequestStorcedIn(string RequestFormPk, string AccountID, string PackedCount, string PackingUnitCL, string Weight, string Volume, string StorageCode, string StorageName, string StorcedDate, string CommentText, string TotalPackedCount, string TotalGrossWeight, string TotalVolume, string IsAleadyCalculated) {
		DB = new DBConn();
		TransportC TransC = new TransportC();
		HistoryC HisC = new HistoryC();
		sHistory History = new sHistory();
		sComment Comment = new sComment();
		string StoragePk = "";

		DB.DBCon.Open();
		if (IsAleadyCalculated == "Y") {
			History.Code = "56";
			StoragePk = TransC.Find_StoragePk(RequestFormPk, "RequestToWarehouse", StorageCode, ref DB);
			TransC.StorageAddCount(StoragePk, Int32.Parse(PackedCount), ref DB);

			DB.SqlCmd.CommandText = " UPDATE [dbo].[STORAGE] SET [WAREHOUSE_PK] = " + StorageCode + ", [PACKING_UNIT] = '" + PackingUnitCL + "', [WEIGHT] = " + Weight + ", [VOLUME] = " + Volume + " WHERE [STORAGE_PK] = " + StoragePk + ";";
		}
		else {
			History.Code = "57";
			StoragePk = TransC.Find_StoragePk(RequestFormPk, "RequestToWarehouse", StorageCode, ref DB);
			TransC.StorageAddCount(StoragePk, Int32.Parse(PackedCount), ref DB);

			DB.SqlCmd.CommandText = "   UPDATE [dbo].[RequestForm] SET " +
				"[TotalPackedCount] = " + Common.StringToDB(TotalPackedCount, false, false) +
				", [PackingUnit] = " + Common.StringToDB(PackingUnitCL, false, false) + 
				", [TotalGrossWeight] = " + Common.StringToDB(TotalGrossWeight, false, false) + 
				", [TotalVolume] = " + Common.StringToDB(TotalVolume, false, false) + 
				"WHERE [RequestFormPk] = " + RequestFormPk + ";" + 
				"	UPDATE RequestForm SET [StepCL] = 57, [StockedDate]='" + StorcedDate + "' WHERE RequestFormPk=" + RequestFormPk + ";" +
				"   UPDATE [dbo].[STORAGE] SET [WAREHOUSE_PK] = " + StorageCode + ", [PACKING_UNIT] = '" + PackingUnitCL + "', [WEIGHT] = " + Weight + ", [VOLUME] = " + Volume + " WHERE [STORAGE_PK] = " + StoragePk + ";";
		}
		DB.SqlCmd.ExecuteNonQuery();

		History.Table_Name = "RequestForm";
		History.Table_Pk = RequestFormPk;
		History.Account_Id = AccountID;
		History.Description = StorageName + "^" + PackedCount + Common.GetPackingUnit(PackingUnitCL);
		HisC.Set_History(History, ref DB);

		Comment.Table_Name = "RequestForm";
		Comment.Table_Pk = RequestFormPk;
		Comment.Category = "Request_Confirm";
		Comment.Account_Id = AccountID;
		Comment.Contents = CommentText;
		HisC.Set_Comment(Comment, ref DB);
		
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String DeleteCompanyRelation(string RelatedPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "DELETE FROM [CompanyRelation] WHERE [CompanyRelationPk]=" + RelatedPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String[] LoadChargeForMSG(string RequestFormPk, string SorC) {
		string[] ReturnValue = new string[5];
		DB = new DBConn();
		DB.SqlCmd.CommandText = "EXEC SP_SelectCalculatedHead @RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string monetaryUnit = "";
		decimal TotalCharge = 0;
		string queryDeliveryCharge = "SELECT Title, Price, MonetaryUnit FROM RequestFormCalculateBody WHERE RequestFormPk=" + RequestFormPk + " and StandardPriceHeadPkNColumn='D' ";
		if (RS.Read()) {
			if (SorC == "S") {
				monetaryUnit = Common.GetMonetaryUnit(RS["ShipperMonetaryUnit"] + "");
				if (RS["ShipperCharge"] + "" != "0.0000") {
					ReturnValue[0] = monetaryUnit + " " + Common.NumberFormat(RS["ShipperCharge"] + "");
					TotalCharge = decimal.Parse(RS["ShipperCharge"] + "");
					ReturnValue[4] = RS["SBankName"] + " " + RS["SBankAccountNo"] + " " + RS["SBankOwnerName"];
				}
				else {
					ReturnValue[0] = "0";
				}
				queryDeliveryCharge += " and GubunCL=200;";
			}
			else {
				monetaryUnit = Common.GetMonetaryUnit(RS["ConsigneeMonetaryUnit"] + "");
				if (RS["ConsigneeCharge"] + "" != "0.0000") {
					ReturnValue[0] = monetaryUnit + " " + Common.NumberFormat(RS["ConsigneeCharge"] + "");
				}
				else {
					ReturnValue[0] = "0";
				}
				TotalCharge = decimal.Parse(RS["ConsigneeCharge"] + "" == "" ? "0" : RS["ConsigneeCharge"] + "");
				ReturnValue[4] = RS["CBankName"] + " " + RS["CBankAccountNo"] + " " + RS["CBankOwnerName"];

				queryDeliveryCharge += " and GubunCL=300;";
			}
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = queryDeliveryCharge;
		RS = DB.SqlCmd.ExecuteReader();
		ReturnValue[2] = "0";
		while (RS.Read()) {
			ReturnValue[2] = Common.GetMonetaryUnit(RS["MonetaryUnit"] + "") + " " + Common.NumberFormat(RS["Price"] + "");
			TotalCharge += decimal.Parse(RS["Price"] + "");
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = "SELECT sum([Value]) AS TariffSUM FROM CommercialDocumentTariff WHERE GubunPk=" + RequestFormPk + " and GubunCL=0 and Value>0 group by [CommercialDocumentHeadPk] ";
		string tariffSum = DB.SqlCmd.ExecuteScalar() + "";

		DB.DBCon.Close();
		ReturnValue[1] = "0";
		if (tariffSum != "") {
			ReturnValue[1] = monetaryUnit + " " + Common.NumberFormat(tariffSum);
			TotalCharge += decimal.Parse(tariffSum);
		}

		ReturnValue[3] = monetaryUnit + " " + Common.NumberFormat(TotalCharge.ToString());
		return ReturnValue;
	}

	[WebMethod]
	public String[] LoadChargeForMSG_Carryover(string RequestFormPk, string SorC) {
		string[] ReturnValue = new string[6];
		DB = new DBConn();
		DB.SqlCmd.CommandText = "EXEC SP_SelectCalculatedHead @RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string monetaryUnit = "";
		decimal TotalCharge = 0;
		string queryDeliveryCharge = "SELECT Title, Price, MonetaryUnit FROM RequestFormCalculateBody WHERE RequestFormPk=" + RequestFormPk + " and StandardPriceHeadPkNColumn='D' ";
		if (RS.Read()) {
			if (SorC == "S") {
				monetaryUnit = Common.GetMonetaryUnit(RS["ShipperMonetaryUnit"] + "");
				if (RS["ShipperCharge"] + "" != "0.0000") {
					ReturnValue[0] = monetaryUnit + " " + Common.NumberFormat(RS["ShipperCharge"] + "");
					TotalCharge = decimal.Parse(RS["ShipperCharge"] + "");
					ReturnValue[4] = RS["SBankName"] + " " + RS["SBankAccountNo"] + " " + RS["SBankOwnerName"];
				}
				else {
					ReturnValue[0] = "0";
				}
				queryDeliveryCharge += " and GubunCL=200;";
			}
			else {
				monetaryUnit = Common.GetMonetaryUnit(RS["ConsigneeMonetaryUnit"] + "");
				if (RS["ConsigneeCharge"] + "" != "0.0000") {
					ReturnValue[0] = monetaryUnit + " " + Common.NumberFormat(RS["ConsigneeCharge"] + "");
				}
				else {
					ReturnValue[0] = "0";
				}
				TotalCharge = decimal.Parse(RS["ConsigneeCharge"] + "" == "" ? "0" : RS["ConsigneeCharge"] + "");
				ReturnValue[4] = RS["CBankName"] + " " + RS["CBankAccountNo"] + " " + RS["CBankOwnerName"];

				queryDeliveryCharge += " and GubunCL=300;";
			}
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = queryDeliveryCharge;
		RS = DB.SqlCmd.ExecuteReader();
		ReturnValue[2] = "0";
		while (RS.Read()) {
			ReturnValue[2] = Common.GetMonetaryUnit(RS["MonetaryUnit"] + "") + " " + Common.NumberFormat(RS["Price"] + "");
			TotalCharge += decimal.Parse(RS["Price"] + "");
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = "SELECT sum([Value]) AS TariffSUM FROM CommercialDocumentTariff WHERE GubunPk=" + RequestFormPk + " and GubunCL=0 and Value>0 group by [CommercialDocumentHeadPk] ";
		string tariffSum = DB.SqlCmd.ExecuteScalar() + "";

		ReturnValue[1] = "0";
		if (tariffSum != "") {
			ReturnValue[1] = monetaryUnit + " " + Common.NumberFormat(tariffSum);
			TotalCharge += decimal.Parse(tariffSum);
		}

		DB.SqlCmd.CommandText = "EXECUTE SP_SelectRequestViewLoad @RequestFormPk=" + RequestFormPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		string ShipperPk = "";
		string ConsigneePk = "";
		if (RS.Read()) {
			ShipperPk = RS["ShipperPk"] + "";
			ConsigneePk = RS["ConsigneePk"] + "";
		}
		RS.Dispose();
		DB.DBCon.Close();

		decimal CarryOver = 0;

		if (SorC == "S") {
			GetCarryover(ShipperPk, "S", RequestFormPk, out CarryOver);
		}
		else {
			GetCarryover(ConsigneePk, "C", RequestFormPk, out CarryOver);
		}

		if (CarryOver != 0) {
			TotalCharge += CarryOver;
			ReturnValue[5] = monetaryUnit + " " + Common.NumberFormat(CarryOver.ToString());
		}
		else {
			ReturnValue[5] = "0";
		}

		ReturnValue[3] = monetaryUnit + " " + Common.NumberFormat(TotalCharge.ToString());
		return ReturnValue;
	}

	[WebMethod]
	public String GetCarryover(string CompanyPk, string SorC, string RequestFormPk, out decimal Carryover) {
		string ArrivalDate = "";
		DB.SqlCmd.CommandText = string.Format(@"
select ArrivalDate from RequestForm
where RequestFormPk='{0}'", RequestFormPk);

		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ArrivalDate = RS["ArrivalDate"] + "" == "" ? "29991231" : RS["ArrivalDate"] + "";
		}
		RS.Dispose();

		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"
SELECT
	RF.RequestFormPk, RF.ShipperPk, ConsigneePk , RF.ShipperCode, RF.ConsigneeCode, RF.DepartureDate, RF.ArrivalDate, RF.StepCL, RF.DocumentStepCL, RF.RequestDate
	, RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume, RCH.[MONETARY_UNIT], RCH.[TOTAL_PRICE], RCH.[DEPOSITED_PRICE], RCH.[LAST_DEPOSITED_DATE], RF.ExchangeDate
	, RFCC.[AttachedRequestFormPk]
FROM RequestForm AS RF
	Left join [dbo].[REQUESTFORMCALCULATE_HEAD] as RCH on RF.RequestFormPk=RCH.[TABLE_PK] 
	left join [RequestFormCalculateCarryover] AS RFCC ON RF.RequestFormPk=RFCC.[OriginalRequestFormPk]
WHERE RF.ArrivalDate>'20130000' and StepCL>58 
AND ISNULL(RCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
AND ( ShipperPk=" + CompanyPk + " or ConsigneePk=" + CompanyPk + @" ) 
AND isnull(RF.ArrivalDate,20191231) <" + ArrivalDate + @" 
AND isnull(RFCC.SorC, '" + SorC + @"')='" + SorC + @"'
	Order by RF.ArrivalDate DESC, RF.RequestFormPk DESC;";
		RS = DB.SqlCmd.ExecuteReader();
		String TempRequestFormPk = string.Empty;
		StringBuilder Temp = new StringBuilder();

		int k = 0;

		decimal TotalMinus = 0;
		Carryover = 0;

		while (RS.Read()) {
			if (SorC == "S") {
				if (RS["ShipperPk"] + "" != CompanyPk) {
					SorC = "C";
				}
			}
			else {
				if (RS["ShipperPk"] + "" == CompanyPk) {
					SorC = "S";
				}
			}
			if (TempRequestFormPk != RS[0] + "") {
				TempRequestFormPk = RS[0] + "";
			}
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
			decimal tempminus = Deposited - TotalCharge;
			TotalMinus += tempminus;
			if (tempminus == 0) {
				continue;
			}
			if (RS["AttachedRequestFormPk"] + "" == RequestFormPk) {
				Carryover += tempminus * -1;
			}
			continue;
		}
		RS.Dispose();
		DB.DBCon.Close();

		return "1";
	}

	[WebMethod]
	public String[] LoadSelfgeForMSG(string RequestFormPk) {
		string[] ReturnValue = new string[5];
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"SELECT [BLNo]
  FROM [INTL2010].[dbo].[CommercialDocument]
WHERE CommercialDocumentHeadPk in (
									SELECT TOP 1 [CommercialDocumentPk]
									FROM [INTL2010].[dbo].[CommerdialConnectionWithRequest]
									where RequestFormPk=" + RequestFormPk + @"
									order by [CommercialDocumentPk])";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue[0] = RS["BLNo"] + "";
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = @"
		Select StorageName, StorageAddress,TEL
from OurBranchStorageCode
where OurBranchStoragePk in(
SELECT top 1 [StorageCode]
  FROM [INTL2010].[dbo].[OurBranchStorageOut]
  where  RequestFormPk=" + RequestFormPk + @")
  and OurBranchCode = 3157";
		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue[1] = RS["StorageName"] + "," + RS["StorageAddress"] + "," + RS["TEL"];
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = @"	SELECT	TotalPackedCount, PackingUnit, TotalGrossWeight, TotalVolume
															FROM	[dbo].[RequestForm]
															WHERE	RequestFormPk=" + RequestFormPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue[2] = "화물 : " + Common.NumberFormat(RS["TotalPackedCount"] + "") + Common.GetPackingUnit(RS["PackingUnit"] + "") + " " + Common.NumberFormat(RS["TotalGrossWeight"] + "") + "Kg " + Common.NumberFormat(RS["TotalVolume"] + "") + "CBM ";
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue;
	}

	[WebMethod]
	public String SetCDStepCL(string[] CommercialDocumentHeadPk, string StepTo, string AccountID) {
		string QCDHPk = "";
		for (int i = 0; i < CommercialDocumentHeadPk.Length; i++) {
			if (QCDHPk != "") {
				QCDHPk += ", ";
			}
			QCDHPk += CommercialDocumentHeadPk[i];
		}
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE [CommercialDocument] SET [StepCL] = " + StepTo + " WHERE CommercialDocumentHeadPk in (" + QCDHPk + @");
			INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID])
			SELECT 'RequestForm', [RequestFormPk], '30', '" + AccountID + @"' FROM [CommerdialConnectionWithRequest] WHERE [CommercialDocumentPk] in (" + QCDHPk + ");";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String TariffModify(string CDHPk, string T1, string T2, string T3) {
		string UpdateQFormat = "UPDATE CommercialDocumentTariff SET [Value] = {1} WHERE [CommercialDocumentHeadPk] = {0} and [Title] = '관세';" +
	"UPDATE CommercialDocumentTariff SET [Value] = {2} WHERE [CommercialDocumentHeadPk] = {0} and [Title] = '부가세';" +
	"UPDATE CommercialDocumentTariff SET [Value] = {3} WHERE [CommercialDocumentHeadPk] = {0} and [Title] = '관세사비';";
		DB = new DBConn();
		DB.SqlCmd.CommandText = string.Format(UpdateQFormat, CDHPk, T1, T2, T3);
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public string ChargeTariff(string[] CDPk) {
		StringBuilder CDPkSum = new StringBuilder();
		for (var i = 0; i < CDPk.Length; i++) {
			if (CDPkSum + "" != "") {
				CDPkSum.Append(", ");
			}
			CDPkSum.Append(CDPk[i]);
		}

		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT SUM(	isnull(T1.Value, 0)+isnull(T2.Value, 0)+isnull(T3.Value, 0))
FROM
	CommercialDocument AS CD
	left join (
		SELECT [CommercialDocumentHeadPk], [MonetaryUnitCL], [Value] FROM [CommercialDocumentTariff] WHERE Title='관세'
	) AS T1 ON T1.[CommercialDocumentHeadPk]=CD.CommercialDocumentHeadPk
	left join (
		SELECT [CommercialDocumentHeadPk], [MonetaryUnitCL], [Value] FROM [CommercialDocumentTariff] WHERE Title='부가세'
	) AS T2 ON T2.[CommercialDocumentHeadPk]=CD.CommercialDocumentHeadPk
	left join (
		SELECT [CommercialDocumentHeadPk], [MonetaryUnitCL], [Value] FROM [CommercialDocumentTariff] WHERE Title='관세사비'
	) AS T3 ON T3.[CommercialDocumentHeadPk]=CD.CommercialDocumentHeadPk
WHERE CD.CommercialDocumentHeadPk in (" + CDPkSum + ");";
		DB.DBCon.Open();
		string Amount = DB.SqlCmd.ExecuteScalar().ToString();

		DB.SqlCmd.CommandText = "INSERT INTO [ChargeTariff] ([TotalAmount], [Date], [Registerd]) VALUES ( " + Common.StringToDB(Amount, false, false) + ", CONVERT(varchar(10), GETDATE(), 102), getDate() );" +
			"SELECT @@IDENTITY;";
		string Identity = DB.SqlCmd.ExecuteScalar() + "";
		DB.SqlCmd.CommandText = "UPDATE [CommercialDocument] SET [StepCL]=3 WHERE [CommercialDocumentHeadPk] in (" + CDPkSum + ");" +
			"UPDATE [CommerdialConnectionWithRequest] SET [TariffCharge]=" + Identity + " WHERE [CommercialDocumentPk] in (" + CDPkSum + ");";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String WarehouseUseNo(string StorageCodePk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"UPDATE [dbo].[OurBranchStorageCode]
   SET [IsUse] = GETDATE()
 WHERE OurBranchStoragePk=" + StorageCodePk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteScalar();
		DB.DBCon.Close();
		return "1";
	}

	//20131017 지사정보 페이지
	[WebMethod]
	public String LoadBranchInfo(string CompanyPk) {
		DB = new DBConn();
		StringBuilder ReturnValue = new StringBuilder();
		string RegionNameBefore;
		DB.SqlCmd.CommandText = @"SELECT CompanyCode, CompanyName, RegionCode, CompanyAddress, CompanyTEL, CompanyFAX, PresidentName
                                       , PresidentEmail, CompanyNo, CompanyNameE, CompanyAddressE, GubunCL, ResponsibleStaff, Memo
                                    FROM Company WHERE CompanyPk=" + CompanyPk + ";";
		string RegionCode;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			RegionCode = RS[2] + "";
			RegionNameBefore = RS[0] + "#@!" + RS[1] + "#@!";
			ReturnValue.Append("#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5] + "#@!" + RS[6] + "#@!" + RS[7] + "#@!" + RS[8] + "#@!" + RS[9] + "#@!" + RS[10] + "#@!" + RS[11] + "#@!" + RS[12] + "#@!" + RS[13] + "%!$@#");
		}
		else {
			DB.DBCon.Close();
			return "N";
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"
SELECT A.AccountPk, A.AccountID, A.Password, A.Duties, A.Name, A.TEL, A.Mobile, A.Email, A.IsEmailNSMS
FROM   Account_ AS A
WHERE  A.CompanyPk=" + CompanyPk + @"
AND GubunCL=93
ORDER BY A.GubunCL,A.AccountID;";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append("A#@!" + RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5] + "#@!" + RS[6] + "#@!" + RS[7] + "#@!" + RS[8] + "%!$@#");
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = @"
SELECT
    Title, Value
FROM
    CompanyAdditionalInfomation
WHERE CompanyPk=" + CompanyPk + " and Title in (61, 62, 63, 64, 65) ORDER BY Title asc;";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append(RS[0] + "#@!" + RS[1] + "%!$@#");
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = @"
SELECT
    OurBranchStoragePk, StorageName ,StorageAddress ,StaffName , TEL ,StaffMobile ,Memo
FROM
    OurBranchStorageCode
WHERE OurBranchCode=" + CompanyPk + " order by StorageName;";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append("W#@!" + RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5] + "#@!" + RS[6] + "%!$@#");
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"
SELECT
    [FilePk], [Title], [FileName]
FROM
    [File]
WHERE GubunCL=1 and GubunPk=" + CompanyPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append("F#@!" + RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "%!$@#");
		}
		RS.Dispose();
		/////////////////////
		DB.SqlCmd.CommandText = @"
SELECT [CompanyBankPk]
      ,[BankName]
      ,[OwnerName]
      ,[AccountNo]
      ,[Address]
      ,[BankMemo]
  FROM [CompanyBank]
WHERE isDel=0 and GubunPk=" + CompanyPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append("B#@!" + RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5] + "%!$@#");
		}
		RS.Dispose();
		DB.DBCon.Close();
		return RegionNameBefore + (RegionCode == "" ? "" : GetRegionNameFromRegionCodeBranch(RegionCode)) + ReturnValue + "";
	}

	//20131021 지사등록
	[WebMethod]
	public String InsertCompanyBranchByAdmin(string AccountID, string[] CompanyValue, string StaffSum, string BranchStorageSum, string CompanyBankSum, string AdditionalInfoSum, string businessType) {
		DB = new DBConn();
		GetQuery GQ = new GetQuery();
		Query = new StringBuilder();

		DB.SqlCmd.CommandText = "INSERT INTO Company " +
			"([GubunCL], [CompanyCode], [CompanyName], [RegionCode], [CompanyAddress], [CompanyTEL], [CompanyFAX], [PresidentName], [PresidentEmail], [CompanyNo], [Memo], [ResponsibleStaff]) VALUES " +
				"( " + Common.StringToDB(CompanyValue[0], false, false) +   //(<GubunCL, smallint,>
				", " + Common.StringToDB(CompanyValue[1], true, false) +    //,<CompanyCode, varchar(50),>
				", " + Common.StringToDB(CompanyValue[2], true, true) + //,<CompanyName, nvarchar(50),>
				", " + Common.StringToDB(CompanyValue[3], true, false) +    //,<RegionCode, varchar(50),>
				", " + Common.StringToDB(CompanyValue[4], true, true) + //,<CompanyAddress, nvarchar(255),>
				", " + Common.StringToDB(CompanyValue[5], true, false) +    //,<CompanyTEL, varchar(40),>
				", " + Common.StringToDB(CompanyValue[6], true, false) +    //,<CompanyFAX, varchar(40),>
				", " + Common.StringToDB(CompanyValue[7], true, true) + //,<PresidentName, nvarchar(50),>
				", " + Common.StringToDB(CompanyValue[8], true, false) +    //,<PresidentEmail, varchar(50),>
				", " + Common.StringToDB(CompanyValue[9], true, false) + //,<CompanyNo, varchar(50),>
				", " + Common.StringToDB(CompanyValue[10], true, true) + //,<Memo, nvarchar(MAX),>
				", " + Common.StringToDB(AccountID, true, false) +
				"); SELECT @@IDENTITY;";
		DB.DBCon.Open();
		string companypk = DB.SqlCmd.ExecuteScalar() + "";
		if (businessType != "") {
			Query.Append(GQ.InsertCompanyAdditional(companypk, "61", businessType));
		}

		string QueryBranchStorage = "INSERT INTO OurBranchStorageCode ([OurBranchCode], [StorageName], [StorageAddress], [StaffName], [TEL], [StaffMobile], [Memo]) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6});";

		string[] BranchStoragerow = BranchStorageSum.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
		foreach (string row in BranchStoragerow) {
			string[] Each = row.Split(Common.Splite22, StringSplitOptions.None);
			Query.Append(string.Format(QueryBranchStorage, companypk,
										Common.StringToDB(Each[0], true, true),
										Common.StringToDB(Each[1], true, true),
										Common.StringToDB(Each[2], true, true),
										Common.StringToDB(Each[3], true, false),
										Common.StringToDB(Each[4], true, false),
										Common.StringToDB(Each[5], true, true)));
		}

		string[] CompanyBankrow = CompanyBankSum.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
		foreach (string row in CompanyBankrow) {
			string[] Each = row.Split(Common.Splite22, StringSplitOptions.None);

			Query.Append("INSERT INTO [CompanyBank] ([GubunPk],[GubunCL],[IsDel], [BankName], [OwnerName], [AccountNo], [Address], [BankMemo]) VALUES (" +
						companypk + ",1,0, " +
						Common.StringToDB(Each[0], true, true) + ", " +
						Common.StringToDB(Each[1], true, true) + ", " +
						Common.StringToDB(Each[2], true, true) + ", " +
						Common.StringToDB(Each[3], true, true) + ", " +
						Common.StringToDB(Each[4], true, true) + ");");
		}

		string[] AdditionalInfoSumArray = AdditionalInfoSum.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);

		foreach (string AdditionalEach in AdditionalInfoSumArray) {
			Query.Append(GQ.InsertCompanyAdditional(companypk, AdditionalEach.Substring(0, 2), AdditionalEach.Substring(5)));
		}

		string[] StaffRow = StaffSum.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
		string AccountInsertQuery = "INSERT INTO Account_ ([GubunCL], [CompanyPk], [AccountID], [Password], [Duties], [Name], [TEL], [Mobile], [Email], [IsEmailNSMS]) VALUES (93, {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8});";
		string[] tempstringarray = StaffSum.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);

		foreach (string thisRow in tempstringarray) {
			string[] ThisStaffEach = thisRow.Split(Common.Splite321, StringSplitOptions.None);
			Query.Append(String.Format(AccountInsertQuery, companypk,
						Common.StringToDB(ThisStaffEach[0], true, true),
						Common.StringToDB(ThisStaffEach[1], true, true),
						Common.StringToDB(ThisStaffEach[2], true, true),
						Common.StringToDB(ThisStaffEach[3], true, true),
						Common.StringToDB(ThisStaffEach[4], true, false),
						Common.StringToDB(ThisStaffEach[5], true, false),
						Common.StringToDB(ThisStaffEach[6], true, false),
						Common.StringToDB(ThisStaffEach[7], false, false)));
		}

		if (Query + "" != "") {
			DB.SqlCmd.CommandText = Query + "";
			DB.SqlCmd.ExecuteNonQuery();
		}
		DB.DBCon.Close();
		return companypk;
	}

	//20131021 지사 수정
	[WebMethod]
	public String UpdateBranchInfo(string GUBUNCL, string COMPANYPK, string COMPANYNAME, string REGIONCODE, string COMPANYADDRESS, string COMPANYTEL, string COMPANYFAX, string PRESIDENTNAME, string PRESIDENTEMAIL, string COMPANYNO, string COMPANYADDITIONAL61, string COMPANYADDITIONAL62, string COMPANYADDITIONAL63, string COMPANYADDITIONAL64, string COMPANYADDITIONAL65, string StaffSum, string BranchStorageSum, string CompanyBankSum, string MEMO) {
		Query = new StringBuilder();
		DB = new DBConn();
		if (REGIONCODE != "*N!C$") {
			DB.SqlCmd.CommandText = "SELECT [RegionCode] FROM RegionCode WHERE RegionCode=" + Common.StringToDB(REGIONCODE, true, true) + ";";
			DB.DBCon.Open();
			REGIONCODE = DB.SqlCmd.ExecuteScalar() + "";
			DB.DBCon.Close();
		}
		Query.Append(" UPDATE Company SET GubunCL = " + GUBUNCL +
			(COMPANYNAME == "*N!C$" ? "" : ", CompanyName = " + Common.StringToDB(COMPANYNAME, true, true)) +
			(REGIONCODE == "*N!C$" ? "" : ", RegionCode = " + Common.StringToDB(REGIONCODE, true, false)) +
			(COMPANYADDRESS == "*N!C$" ? "" : ", CompanyAddress = " + Common.StringToDB(COMPANYADDRESS, true, true)) +
			(COMPANYTEL == "*N!C$" ? "" : ", CompanyTEL = " + Common.StringToDB(COMPANYTEL, true, false)) +
			(COMPANYFAX == "*N!C$" ? "" : ", CompanyFAX = " + Common.StringToDB(COMPANYFAX, true, false)) +
			(PRESIDENTNAME == "*N!C$" ? "" : ", PresidentName = " + Common.StringToDB(PRESIDENTNAME, true, true)) +
			(PRESIDENTEMAIL == "*N!C$" ? "" : ", PresidentEmail = " + Common.StringToDB(PRESIDENTEMAIL, true, false)) +
			(COMPANYNO == "*N!C$" ? "" : ", CompanyNo = " + Common.StringToDB(COMPANYNO, true, false)) +
			(MEMO == "*N!C$" ? "" : ", [Memo]= " + Common.StringToDB(MEMO, true, true)) +
			" WHERE CompanyPk=" + COMPANYPK + ";");
		//return Query + "";
		string CAIUpdateQ = "	DELETE FROM CompanyAdditionalInfomation WHERE [CompanyPk] = {0} and [Title] = {1};" +
							"	INSERT INTO CompanyAdditionalInfomation ([CompanyPk], [Title], [Value]) VALUES ({0}, {1}, {2});";

		if (COMPANYADDITIONAL61 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL61, true, true) != "NULL") {
			Query.Append(string.Format(CAIUpdateQ, COMPANYPK, "61", Common.StringToDB(COMPANYADDITIONAL61, true, true)));
		}
		if (COMPANYADDITIONAL62 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL62, true, true) != "NULL") {
			Query.Append(string.Format(CAIUpdateQ, COMPANYPK, "62", Common.StringToDB(COMPANYADDITIONAL62, true, true)));
		}
		if (COMPANYADDITIONAL63 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL63, true, true) != "NULL") {
			Query.Append(string.Format(CAIUpdateQ, COMPANYPK, "63", Common.StringToDB(COMPANYADDITIONAL63, true, true)));
		}
		if (COMPANYADDITIONAL64 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL64, true, true) != "NULL") {
			Query.Append(string.Format(CAIUpdateQ, COMPANYPK, "64", Common.StringToDB(COMPANYADDITIONAL64, true, true)));
		}
		if (COMPANYADDITIONAL65 != "*N!C$" && Common.StringToDB(COMPANYADDITIONAL65, true, true) != "NULL") {
			Query.Append(string.Format(CAIUpdateQ, COMPANYPK, "65", Common.StringToDB(COMPANYADDITIONAL65, true, true)));
		}

		string[] StaffRow = StaffSum.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
		foreach (string EachRow in StaffRow) {
			string[] Each = EachRow.Split(Common.Splite321, StringSplitOptions.None);
			if (Each[0] == "") {
				Query.Append(" INSERT INTO [Account_] ([GubunCL], [CompanyPk], [AccountID], [Password], [Duties], [Name], [TEL], [Mobile], [Email], [IsEmailNSMS]) VALUES ( " +
										"93, " +
										COMPANYPK + ", " +
										Common.StringToDB(Each[1], true, true) + ", " +
										Common.StringToDB(Each[2], true, true) + ", " +
										Common.StringToDB(Each[3], true, true) + ", " +
										Common.StringToDB(Each[4], true, true) + ", " +
										Common.StringToDB(Each[5], true, true) + ", " +
										Common.StringToDB(Each[6], true, true) + ", " +
										Common.StringToDB(Each[7], true, true) + ", " +
										Common.StringToDB(Each[8], false, true) + ");");
				//if (Each[9] != "") {
				//	Query.Append("INSERT INTO AccountAdditionalInfo_ ([AccountPk], [GubunCL], [Value]) VALUES (@@IDENTITY, 1, " + Common.StringToDB(Each[9], true, true) + ");");
				//}
			}
			else {
				Query.Append("UPDATE [Account_] SET [AccountID] = " + Common.StringToDB(Each[1], true, true) +
												", [Password] = " + Common.StringToDB(Each[2], true, true) +
												", [Duties] = " + Common.StringToDB(Each[3], true, true) +
												", [Name] = " + Common.StringToDB(Each[4], true, true) +
												", [TEL] = " + Common.StringToDB(Each[5], true, false) +
												", [Mobile] = " + Common.StringToDB(Each[6], true, false) +
												", [Email] = " + Common.StringToDB(Each[7], true, false) +
												", [IsEmailNSMS] = " + Common.StringToDB(Each[8], false, false) +
											" WHERE AccountPk=" + Each[0] + ";");
				//Query.Append("	DELETE FROM AccountAdditionalInfo_ WHERE GubunCL=1 and [AccountPk]=" + Each[0] + ";" +
				//			 "	INSERT INTO AccountAdditionalInfo_ ([AccountPk], [GubunCL], [Value]) VALUES (" + Each[0] + ", 1, " + Common.StringToDB(Each[9], true, true) + ");");
			}
		}
		if (BranchStorageSum != "") {
			string[] BranchStorageRow = BranchStorageSum.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
			foreach (string row in BranchStorageRow) {
				string[] each = row.Split(Common.Splite22, StringSplitOptions.None);
				if (each[0] == "") {
					Query.Append("INSERT INTO [OurBranchStorageCode] ([OurBranchCode], [StorageName], [StorageAddress], [Staffname], [TEL], [StaffMobile], [Memo]) VALUES (" +
						COMPANYPK + ", " +
						Common.StringToDB(each[1], true, true) + ", " +
						Common.StringToDB(each[2], true, true) + ", " +
						Common.StringToDB(each[3], true, false) + ", " +
						Common.StringToDB(each[4], true, true) + ", " +
						Common.StringToDB(each[5], true, false) + ", " +
						Common.StringToDB(each[6], true, false) + ");");
				}
				else {
					Query.Append("UPDATE [OurBranchStorageCode] SET [StorageName] = " + Common.StringToDB(each[1], true, true) +
						", [StorageAddress] = " + Common.StringToDB(each[2], true, true) +
						", [Staffname] = " + Common.StringToDB(each[3], true, true) +
						", [TEL] = " + Common.StringToDB(each[4], true, false) +
						", [StaffMobile] = " + Common.StringToDB(each[5], true, false) +
						", [Memo] = " + Common.StringToDB(each[6], true, false) + " WHERE OurBranchStoragePk=" + each[0] + ";");
				}
			}
		}
		if (CompanyBankSum != "") {
			string[] CompanyBankRow = CompanyBankSum.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
			foreach (string row in CompanyBankRow) {
				string[] each = row.Split(Common.Splite22, StringSplitOptions.None);
				if (each[0] == "") {
					Query.Append("INSERT INTO [CompanyBank] ([GubunPk],[GubunCL],[IsDel], [BankName], [OwnerName], [AccountNo], [Address], [BankMemo]) VALUES (" +
						COMPANYPK + ",1,0, " +
						Common.StringToDB(each[1], true, true) + ", " +
						Common.StringToDB(each[2], true, true) + ", " +
						Common.StringToDB(each[3], true, true) + ", " +
						Common.StringToDB(each[4], true, true) + ", " +
						Common.StringToDB(each[5], true, true) + ");");
				}
				else {
					Query.Append("UPDATE [CompanyBank] SET [BankName] = " + Common.StringToDB(each[1], true, true) +
						", [OwnerName] = " + Common.StringToDB(each[2], true, true) +
						", [AccountNo] = " + Common.StringToDB(each[3], true, true) +
						", [Address] = " + Common.StringToDB(each[4], true, false) +
						", [BankMemo] = " + Common.StringToDB(each[5], true, false) + " WHERE CompanyBankPk=" + each[0] + ";");
				}
			}
		}
		DB.SqlCmd.CommandText = Query + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	//Region 수정,신규 저장
	[WebMethod]
	public string SaveNewRegionCode(string Mode, string Code, string Name, string NameE, string BranchCode, string GUBUN) {
		if (Mode == "Modify") {
			DB = new DBConn();
			DB.SqlCmd.CommandText = "UPDATE [RegionCode] SET [Name] = " + Common.StringToDB(Name, true, true) +
														  ", [NameE]= " + Common.StringToDB(NameE, true, true) +
														  ", [GUBUN]= " + Common.StringToDB(GUBUN, true, true) +
														  ", [OurBranchCode] = " + Common.StringToDB(BranchCode, true, false) +
													 " WHERE [RegionCode] = " + Common.StringToDB(Code, true, true) + ";";
			DB.DBCon.Open();
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		}
		else {
			DB = new DBConn();
			DB.SqlCmd.CommandText = "SELECT Count(*) FROM [RegionCode] WHERE [RegionCode]=" + Common.StringToDB(Code, true, false);
			DB.DBCon.Open();
			string codecount = DB.SqlCmd.ExecuteScalar() + "";
			if (codecount != "0") {
				DB.DBCon.Close();
				return "0";
			}

			DB.SqlCmd.CommandText = "INSERT INTO [RegionCode] ([RegionCode], [Name], [NameE], [OurBranchCode], [GUBUN]) VALUES (" + Common.StringToDB(Code, true, true) + "," + Common.StringToDB(Name, true, true) + "," + Common.StringToDB(NameE, true, true) + "," + Common.StringToDB(BranchCode, true, true) + "," + Common.StringToDB(GUBUN, true, true) + ");";

			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		}
	}

	[WebMethod]
	public String DeleteRegion(string RegionCode) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "DELETE FROM [RegionCode] WHERE RegionCode=" + Common.StringToDB(RegionCode, true, false) + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String receipt_confirm16_All(string stepcl, string BBPk, string AccountID) {
		Query = new StringBuilder();
		DB = new DBConn();

		if (stepcl == "3") {
			DB.SqlCmd.CommandText = "SELECT RequestFormPk FROM TransportBBHistory WHERE TransportBetweenBranchPk =" + BBPk + ";";
		}
		else {
			DB.SqlCmd.CommandText = "SELECT [REQUEST_PK] AS RequestFormPk FROM [dbo].[TRANSPORT_BODY] WHERE [TRANSPORT_HEAD_PK] =" + BBPk + ";";
		}
		DB.DBCon.Open();

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		List<string> Sum_RequestFormPk = new List<string>();
		while (RS.Read()) {
			Sum_RequestFormPk.Add(RS["RequestFormPk"] + "");
		}
		RS.Dispose();
		Query.Append(@"
DECLARE @GubunCL smallint;
DECLARE @RequestFormPk int;
DECLARE @Registerd smalldatetime;
");
		foreach (string RequestFormPk in Sum_RequestFormPk) {
			Query.Append(@"
SET @RequestFormPk=" + RequestFormPk + @";
select @GubunCL=(ISNULL(C16, 0)-ISNULL(C17, 0))  from [dbo].[HISTORY] R
left join (Select [TABLE_PK],   COUNT([CODE])  AS C16
		from [dbo].[HISTORY]
		where [TABLE_NAME] = 'RequestForm'
		AND [CODE] ='16'
		GROUP BY [TABLE_NAME], [TABLE_PK]) AS Count16 ON Count16.[TABLE_PK]=R.[TABLE_PK]
		left join (
		Select [TABLE_PK],   COUNT([CODE])  AS C17
		from [dbo].[HISTORY]
		where [TABLE_NAME] = 'RequestForm'
		AND [CODE] ='17'
		GROUP BY [TABLE_NAME], [TABLE_PK]
	) AS Count17 ON Count17.[TABLE_PK]=R.[TABLE_PK] where R.[TABLE_PK]=" + RequestFormPk + @";
if (@GubunCL=0)
	BEGIN
		INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID]) VALUES ('RequestFrom', " + RequestFormPk + @", '16', '" + AccountID + @"');
	END
--if (@GubunCL<>16)
--	BEGIN
	--	if (@Registerd>=cast(getdate() AS smalldatetime))
		--	BEGIN
			--	INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID], [REGISTERD]) VALUES ('RequestForm', @RequestFormPk, '16', '" + AccountID + @"', dateadd(mi, +1, @Registerd));
		--	END
		--else
		--	BEGIN
			--	INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID], [REGISTERD]) VALUES ('RequestForm', @RequestFormPk, '16', '" + AccountID + @"', getDate());
		--	END
--	END
");
		}
		if (Query + "" != "") {
			DB.SqlCmd.CommandText = Query + "";
			DB.SqlCmd.ExecuteNonQuery();
		}
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String StepBack(string BBPk) {
		DB = new DBConn();

		DB.SqlCmd.CommandText = "update [INTL2010].[dbo].[TransportBBStep] set Step=1 where [TransportBetweenBranchPk]=" + BBPk + ";";

		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String SendMailByCafe24_Document(string FromEmail, string FromName, string EmailPhysicalPath, string Companypk, string RelationPk) {
		Email E = new Email();
		string ToEmail = "";
		string ToName = "";
		string QueryIn = "";
		DB = new DBConn();

		if (RelationPk + "" == "") {
			QueryIn = Companypk;
		}
		else {
			QueryIn = Companypk + "," + RelationPk;
		}

		DB.SqlCmd.CommandText = @"SELECT count(*)
								    FROM  [dbo].[account_] A
									left join AccountAdditionalInfo_ AS AAI ON A.AccountPk=AAI.AccountPk
								   where [companypk] in (" + QueryIn + ") and (AAI.GubunCL=1 or Isnull(AAI.GubunCL, 0)=0) and AAI.[Value] like '%수입서류담당자%';";
		DB.DBCon.Open();
		string count = DB.SqlCmd.ExecuteScalar() + "";
		if (count == "0") {
			DB.DBCon.Close();
			return "0";
		}
		else {
			DB.SqlCmd.CommandText = @"SELECT A.[name],A.[email],AAI.[Value]
								    FROM  [dbo].[account_] A
									left join AccountAdditionalInfo_ AS AAI ON A.AccountPk=AAI.AccountPk
								   where [companypk] in (" + QueryIn + ") and (AAI.GubunCL=1 or Isnull(AAI.GubunCL, 0)=0) and AAI.[Value] like '%수입서류담당자%';";

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ToEmail = RS["email"].ToString().Trim();
				ToName = RS["name"].ToString().Trim();
				E.SendMailByCafe24_Document(FromEmail, FromName, ToEmail, ToName, "국제물류 아이엘 || 무역송금 결과", "국제물류 아이엘 || 무역송금 결과 안내 메일입니다. 첨부파일을 확인해주십시요", EmailPhysicalPath);
			}
			RS.Dispose();
			DB.DBCon.Close();

			return "1";
		}
	}

	[WebMethod]
	public string SetCompanyAddInfo(string CompanyPk, string Title, string Value) {
		StringBuilder Query = new StringBuilder();
		if (Title == "152") {
			Query.Append("DELETE FROM [CompanyAdditionalInfomation] WHERE Title=152 and CompanyPk=" + CompanyPk + ";");
		}
		Query.Append(new GetQuery().InsertCompanyAdditional(CompanyPk, Title, Value));
		if (Title == "152") {
			Query.Append("DELETE FROM [CompanyAdditionalInfomation] WHERE Title=152 and Value=N'INTL';");
		}

		DB = new DBConn();
		DB.SqlCmd.CommandText = Query.ToString();
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

}
