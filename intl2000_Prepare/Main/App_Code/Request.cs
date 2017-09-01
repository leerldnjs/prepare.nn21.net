using System;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using Components;
using System.Text.RegularExpressions;
using System.Collections.Generic;

/// <summary>
/// RequestForm의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
[System.Web.Script.Services.ScriptService]
public class Request : System.Web.Services.WebService
{
	private string TempString;
	DBConn DB;
	public Request() {
		//디자인된 구성 요소를 사용하는 경우 다음 줄의 주석 처리를 제거합니다. 
		//InitializeComponent(); 
	}
	[WebMethod]
	public String CompanyNoYN(string CompanyInDocumentPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
select CompanyNo from CompanyInDocument;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string ReturnValue = "";

		while (RS.Read()) {
			ReturnValue = RS["CompanyNo"] + "" == "" ? "N" : "Y";
		}

		DB.DBCon.Close();
		return ReturnValue;
	}
	[WebMethod]
	public String CompanyInDocument_CompanyNoAdd(string CompanyInDocumentPk) {
		DB = new DBConn();

		DB.SqlCmd.CommandText = @"
SELECT CID.CompanyInDocumentPk, CID.Name, CID.Address, F.FilePk, F.FileName, F.[PhysicalPath], CID.CompanyNo
From CompanyInDocument CID
left join (SELECT FilePk, GubunPk, [PhysicalPath], FileName FROM [File] WHERE isnull(GubunCL, 3)=3 ) AS F On CID.CompanyInDocumentPk=F.GubunPk 
WHERE CID.CompanyInDocumentPk=" + CompanyInDocumentPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string Check;
		string R_CompanyInDocumentPk = "";
		string R_Name = "";
		string R_Address = "";
		string R_FilePk = "";
		string R_FileName = "";
		string R_PhysicalPath = "";
		string Check_CompanyNo = "";
		while (RS.Read()) {
			Check_CompanyNo = RS["CompanyNo"] + "" == "" ? "" : "(" + RS["CompanyNo"] + ")";
			if (RS["Name"].ToString().Substring(RS["Name"].ToString().Length - 1, 1) == ")") {
				Check = RS["Name"].ToString().Substring(0, RS["Name"].ToString().Length - 1).Substring(RS["Name"].ToString().Length - 5);
				double Num;
				bool isNum = double.TryParse(Check, out Num);
				if (!isNum) {
					R_Name = RS["Name"] + Check_CompanyNo;
				} else {
					R_Name = RS["Name"] + "";
				}
			} else {
				R_Name = RS["Name"] + Check_CompanyNo;
			}
			R_CompanyInDocumentPk = RS["CompanyInDocumentPk"] + "";
			R_Address = RS["Address"] + "";
			R_FilePk = RS["FilePk"] + "";
			R_FileName = RS["FileName"] + "";
			R_PhysicalPath = RS["PhysicalPath"] + "";
		}

		RS.Dispose();
		DB.DBCon.Close();

		return R_CompanyInDocumentPk + "##" + R_Name + "##" + R_Address + "##" + R_FilePk + "##" + R_FileName + "##" + R_PhysicalPath;
	}
	[WebMethod]
	public String CompanyInDocument_CompanyNoAdd_Transport(string CompanyInDocumentPk) {
		DB = new DBConn();

		DB.SqlCmd.CommandText = @"
SELECT CID.CompanyInDocumentPk, CID.Name, CID.Address, CID.Address_KOR AS Memo
From CompanyInDocument CID
WHERE CID.CompanyInDocumentPk=" + CompanyInDocumentPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		string R_CompanyInDocumentPk = "";
		string R_Name = "";
		string R_Address = "";
		string R_Memo = "";
		while (RS.Read()) {
			R_CompanyInDocumentPk = RS["CompanyInDocumentPk"] + "";
			R_Name = RS["Name"] + "";
			R_Address = RS["Address"] + "";
			R_Memo = RS["Memo"] + "";
		}

		RS.Dispose();
		DB.DBCon.Close();

		return R_CompanyInDocumentPk + "##" + R_Name + "##" + R_Address + "##" + R_Memo;
	}
	[WebMethod]
	public String SetTalkBusiness(string CompanyPk, string ID, string Contents, string GubunCL) {
		DB = new DBConn();

		DB.SqlCmd.CommandText = @"
DECLARE @Count int;
Select @Count=count(*) from [dbo].[COMMENT] WHERE [TABLE_NAME] = 'Company' AND [CATEGORY]=" + GubunCL + @" and [TABLE_PK] = " + CompanyPk + @";
if (@Count!=0)
   UPDATE [dbo].[COMMENT] SET [CONTENTS]=N'" + Contents + @"' ,  [ACCOUNT_ID] = '" + ID + @"' WHERE [CATEGORY]=" + GubunCL + @" AND [TABLE_PK] = " + CompanyPk + @";
else
INSERT INTO [dbo].[COMMENT] ([TABLE_NAME], [TABLE_PK], [CATEGORY], [ACCOUNT_ID], [CONTENTS]) VALUES ('Company', " + CompanyPk + ", " + GubunCL + ", '" + ID + "', N'" + Contents + "' );";

		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String OwnCustomerAdd(string RelationType, string ListOwnerCompanyPk, string AccountID, string companyName, string regionCode, string companyAddress, string TEL, string FAX, string Email, string PresidentName, string Homepage, string Memo, string StaffSum) {
		DB = new DBConn();
		StringBuilder Query = new StringBuilder();
		Query.Append(@"
DECLARE @RegionCode nvarchar(10); 
DECLARE @CompanyPk int;
			SELECT @RegionCode=[RegionCode] FROM [RegionCode] WHERE RegionCodePk=" + regionCode + @";
			INSERT INTO Company ([GubunCL], [CompanyName], [RegionCode], [CompanyAddress], [CompanyTEL], [CompanyFAX], [PresidentName], [PresidentEmail]) VALUES (" +
			"1, " +
			Common.StringToDB(companyName, true, true) + ", @RegionCode, " +
			Common.StringToDB(companyAddress, true, true) + ", " +
			Common.StringToDB(TEL, true, false) + ", " +
			Common.StringToDB(FAX, true, false) + ", " +
			Common.StringToDB(PresidentName, true, true) + ", " +
			Common.StringToDB(Email, true, false) + ");" +
			" select @CompanyPk=@@identity;");

		Query.Append(@"
INSERT INTO [CompanyRelation] ([MainCompanyPk], [TargetCompanyPk], [GubunCL], [Memo]) VALUES ( " +
	Common.StringToDB(ListOwnerCompanyPk, false, false) + ", " +
	"@CompanyPk, " +
	Common.StringToDB(RelationType, false, false) + ", " +
	Common.StringToDB(Memo, true, true) + ");");
		if (RelationType == "1")    //거래처일땐 상대방도 등록해줘야...
		{
			Query.Append(@"
INSERT INTO [CompanyRelation] ([MainCompanyPk], [TargetCompanyPk], [GubunCL], [Memo]) VALUES ( " +
	"@CompanyPk, " +
	Common.StringToDB(ListOwnerCompanyPk, false, false) + ", " +
	Common.StringToDB(RelationType, false, false) + ", " +
	Common.StringToDB(Memo, true, true) + ");");
		}

		if (StaffSum != "") {
			string[] StaffRow = StaffSum.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < StaffRow.Length; i++) {
				string[] each = StaffRow[i].Split(Common.Splite321, StringSplitOptions.None);
				Query.Append(@"	
INSERT INTO [Account_] ([GubunCL], [CompanyPk], [Duties], [Name], [TEL], [Mobile], [Email]) VALUES (
	100, @CompanyPk," + Common.StringToDB(each[0], true, true) + ", " + Common.StringToDB(each[1], true, true) + ", " + Common.StringToDB(each[2], true, false) + ", " + Common.StringToDB(each[3], true, false) + ", " +
	Common.StringToDB(each[4], true, false) + ");");
			}
		}
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = Query + "";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String OwnCustomerAddWithNoRequest(string GubunCL, string ListOwnerCompanyPk, string TargetCompanyPk) {
		DB = new DBConn();
		if (GubunCL == "0") {
			DB.SqlCmd.CommandText = "	INSERT INTO [CompanyRelation] ([MainCompanyPk], [TargetCompanyPk], [GubunCL]) VALUES (" + ListOwnerCompanyPk + ", " + TargetCompanyPk + ", " + GubunCL + ");";
		} else {
			DB.SqlCmd.CommandText = "	INSERT INTO [CompanyRelation] ([MainCompanyPk], [TargetCompanyPk], [GubunCL]) VALUES (" + ListOwnerCompanyPk + ", " + TargetCompanyPk + ", " + GubunCL + ");" +
														"	INSERT INTO [CompanyRelation] ([MainCompanyPk], [TargetCompanyPk], [GubunCL]) VALUES (" + TargetCompanyPk + ", " + ListOwnerCompanyPk + ", " + GubunCL + ");";
		}
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String[] RequestLoad(string RequestFormPk) {
		try {
			DB = new DBConn();
			List<string> returnvalue = new List<string>();
			DB.SqlCmd.CommandText = @"
SELECT R.[ShipperPk], R.[ConsigneePk], R.[AccountID], R.[ShipperCode], R.[ConsigneeCode]
	, R.[DepartureDate], R.[ArrivalDate]
	, R.[ShipperClearanceNamePk], R.[ConsigneeClearanceNamePk]
	, R.[DepartureBranchPk], R.[ArrivalBranchPk], R.[TransportWayCL]
	, R.[PaymentWhoCL], R.[DocumentRequestCL], R.[MonetaryUnitCL], R.[StepCL], R.[Memo], R.[ShipmentDate]
	, SC.CompanyName AS SCompanyName, CC.CompanyName AS CCompanyName
	, DRC.[RegionCode] AS DRCRegion, ARC.[RegionCode] AS ARCRegion
	, DRC.[RegionCodePk] AS DRCRegionCode, ARC.[RegionCodePk] AS ARCRegionCode
	, SName.[Name] AS SNN, SName.[Address] AS SNA
	, AName.[Name] AS ANN, AName.[Address] AS ANA
  FROM [RequestForm] AS R
	left join Company AS SC ON R.ShipperPk=SC.CompanyPk
	left join Company AS CC ON R.ConsigneePk=CC.CompanyPk
	left join RegionCode AS DRC ON R.[DepartureRegionCode]=DRC.RegionCode 
	left join RegionCode AS ARC ON R.[ArrivalRegionCode]=ARC.RegionCode 
	left join [CompanyInDocument] AS SName ON R.[ShipperClearanceNamePk]=SName.[CompanyInDocumentPk] 
	left join [CompanyInDocument] AS AName ON R.[ConsigneeClearanceNamePk]=AName.[CompanyInDocumentPk] 
 WHERE RequestFormPk=" + RequestFormPk + ";";
			DB.DBCon.Open();
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {

				returnvalue.Add("C#@!" +
					RS["ShipperPk"] + "#@!" +
					RS["SCompanyName"] + "#@!" +
					RS["ShipperCode"] + "#@!" +
					RS["DRCRegion"] + "|" + RS["DRCRegionCode"] + "#@!" +
					RS["DepartureBranchPk"] + "#@!" +
					RS["ConsigneePk"] + "#@!" +
					RS["CCompanyName"] + "#@!" +
					RS["ConsigneeCode"] + "#@!" +
					RS["ARCRegion"] + "|" + RS["ARCRegionCode"] + "#@!" +
					RS["ArrivalBranchPk"] + "#@!" +
					RS["DepartureDate"] + "#@!" +
					RS["ArrivalDate"] + "#@!" +
					RS["ShipperClearanceNamePk"] + "#@!" +
					RS["SNN"] + "#@!" +
					RS["SNA"] + "#@!" +
					RS["ConsigneeClearanceNamePk"] + "#@!" +
					RS["ANN"] + "#@!" +
					RS["ANA"] + "#@!" +
					RS["TransportWayCL"] + "#@!" +
					RS["PaymentWhoCL"] + "#@!" +
					RS["DocumentRequestCL"] + "#@!" +
					RS["MonetaryUnitCL"] + "#@!" +
					RS["Memo"] + "#@!" +
					RS["ShipmentDate"] + "#@!");
			}
			RS.Dispose();
			DB.SqlCmd.CommandText = @"
SELECT [RequestFormItemsPk], [MarkNNumber], [Description], [Label], [Material], [Quantity], [QuantityUnit], [PackedCount], [PackingUnit], [GrossWeight], [Volume], [UnitPrice], [Amount]
  FROM [RequestFormItems]
 WHERE RequestFormPk=" + RequestFormPk + @"
ORDER BY RAN;";
			RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				returnvalue.Add("I#@!" + RS["RequestFormItemsPk"] + "#@!" + RS["MarkNNumber"] + "#@!" +
					RS["Description"] + "#@!" +
					RS["Label"] + "#@!" +
					RS["Material"] + "#@!" +
					RS["Quantity"] + "#@!" +
					RS["QuantityUnit"] + "#@!" +
					RS["PackedCount"] + "#@!" +
					RS["PackingUnit"] + "#@!" +
					RS["GrossWeight"] + "#@!" +
					RS["Volume"] + "#@!" +
					RS["UnitPrice"] + "#@!" +
					RS["Amount"]);
			}
			RS.Dispose();

			DB.SqlCmd.CommandText = "SELECT [TotalPackedCount], [PackingUnit], [TotalGrossWeight], [TotalVolume] FROM [dbo].[RequestForm] WHERE RequestFormPk=" + RequestFormPk + ";";
			RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				returnvalue.Add("T#@!" + RS["TotalPackedCount"] + "#@!" + RS["PackingUnit"] + "#@!" + RS["TotalGrossWeight"] + "#@!" + RS["TotalVolume"]);
			}
			RS.Dispose();
			DB.DBCon.Close();
			return returnvalue.ToArray();
		} catch (Exception ex) {
			return new string[] { ex.Message };
		}
	}
	[WebMethod]
	public String RequestWrite(string RequestCompanyPk, string ShipperPk, string ConsigneePk, string AccountID, string ShipperCode, string ConsigneeCode, string ShipperClearanceNamePk, string ConsigneeClearanceNamePk, string DepartureDate, string ArrivalDate, string SHIPMENTDATE, string DepartureRegion, string ArrivalRegion, string TransportWayCL, string PaymentWhoCL, string DocumentRequestCL, string MonetaryUnitCL, string Memo, string MemoForAdmin, string DepartureBranch, string ArrivalBranch, string ITEMSUM, string IsEstimation) {
		try {
			DB = new DBConn();
			DB.DBCon.Open();
			DB.SqlCmd.CommandText = "Select RegionCode from RegionCode where RegionCodePk=" + DepartureRegion + ";";
			string DepartureRegionCode = DB.SqlCmd.ExecuteScalar() + "";
			DB.SqlCmd.CommandText = "Select RegionCode from RegionCode where RegionCodePk=" + ArrivalRegion + ";";
			string ArrivalRegionCode = DB.SqlCmd.ExecuteScalar() + "";
			StringBuilder Query = new StringBuilder();


			if (IsEstimation == "1") {
				//견적접수
				Query.Append("DECLARE @RequestPk int ;" +
					"INSERT INTO [RequestForm] ([ShipperPk], [ConsigneePk], [AccountID], [ShipperCode], [ConsigneeCode], [ShipperClearanceNamePk], [ConsigneeClearanceNamePk], [DepartureDate], [ArrivalDate], [SHIPMENTDATE] " +
					", [DepartureRegionCode], [ArrivalRegionCode], [DepartureBranchPk], [ArrivalBranchPk], [TransportWayCL], [PaymentWhoCL], [DocumentRequestCL], [MonetaryUnitCL], [StepCL], [Memo], [RequestDate]) VALUES (" +
					Common.StringToDB(ShipperPk, false, false) + ", " +
					Common.StringToDB(ConsigneePk, false, false) + ", " +
					Common.StringToDB(AccountID, true, false) + ", " +
					Common.StringToDB(ShipperCode, true, false) + ", " +
					Common.StringToDB(ConsigneeCode, true, false) + ", " +
					Common.StringToDB(ShipperClearanceNamePk, false, false) + ", " +
					Common.StringToDB(ConsigneeClearanceNamePk, false, false) + ", " +
					Common.StringToDB(DepartureDate, true, false) + ", " +
					Common.StringToDB(ArrivalDate, true, false) + ", " +
					Common.StringToDB(SHIPMENTDATE, true, false) + ", " +
					Common.StringToDB(DepartureRegionCode, true, false) + ", " +
					Common.StringToDB(ArrivalRegionCode, true, false) + ", " +
					Common.StringToDB(DepartureBranch, false, false) + ", " +
					Common.StringToDB(ArrivalBranch, false, false) + ", " +
					Common.StringToDB(TransportWayCL, false, false) + ", " +
					Common.StringToDB(PaymentWhoCL, false, false) + ", " +
					Common.StringToDB(DocumentRequestCL, true, false) + ", " +
					Common.StringToDB(MonetaryUnitCL, false, false) + ", 33, " +
					Common.StringToDB(Memo.Replace("\n", "<br />").Replace("\r\n", "<br />"), true, true) + ", getDate() );" +
					"SET @RequestPk=@@IDENTITY;");
				Query.Append("INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID]) VALUES ('RequestForm', @RequestPk, '50', '" + AccountID + "');");
				if (ITEMSUM != "") {
					string[] ItemRow = ITEMSUM.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
					foreach (string row in ItemRow) {
						string[] each = row.Split(Common.Splite321, StringSplitOptions.None);
						Query.Append("EXEC SP_InsertRequestFormItems @RequestFormPk=@RequestPk" +
							", @MarkNNumber=" + Common.StringToDB(each[0], true, false) +
							", @Description=" + Common.StringToDB(each[1].Replace("'", "").Replace("\"", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace("=", "").Replace("+", "").Replace(";", "").Replace(":", ""), true, true) +
							", @Label=" + Common.StringToDB(each[2], true, true) +
							", @Material=" + Common.StringToDB(each[3], true, true) +
							", @Quantity=" + Common.StringToDB(each[4], false, false) +
							", @QuantityUnit=" + Common.StringToDB(each[5], false, false) +
							", @PackedCount=" + Common.StringToDB(each[8], false, false) +
							", @PackingUnit=" + Common.StringToDB(each[9], false, false) +
							", @GrossWeight=" + Common.StringToDB(each[10], false, false) +
							", @Volume=" + Common.StringToDB(each[11], false, false) +
							", @UnitPrice=" + Common.StringToDB(each[6], false, false) +
							", @Amount=" + Common.StringToDB(each[7], false, false) + ";");
					}
				}
				if (MemoForAdmin != "") {
					Query.Append("INSERT INTO [dbo].[COMMENT] ([TABLE_NAME], [TABLE_PK], [CATEGORY], [ACCOUNT_ID], [CONTENTS]) VALUES ('RequestForm', @RequestPk, 'Request', '" + AccountID + "', N'" + MemoForAdmin.Replace("\n", "<br />").Replace("\r\n", "<br />") + "' );");
				}
			} else {
				// 일반접수


				Query.Append("DECLARE @RequestPk int ;" +
					"INSERT INTO [RequestForm] ([ShipperPk], [ConsigneePk], [AccountID], [ShipperCode], [ConsigneeCode], [ShipperClearanceNamePk], [ConsigneeClearanceNamePk], [DepartureDate], [ArrivalDate], [SHIPMENTDATE] " +
					", [DepartureRegionCode], [ArrivalRegionCode], [DepartureBranchPk], [ArrivalBranchPk], [TransportWayCL], [PaymentWhoCL], [DocumentRequestCL], [MonetaryUnitCL], [StepCL], [Memo], [RequestDate]) VALUES (" +
					Common.StringToDB(ShipperPk, false, false) + ", " +
					Common.StringToDB(ConsigneePk, false, false) + ", " +
					Common.StringToDB(AccountID, true, false) + ", " +
					Common.StringToDB(ShipperCode, true, false) + ", " +
					Common.StringToDB(ConsigneeCode, true, false) + ", " +
					Common.StringToDB(ShipperClearanceNamePk, false, false) + ", " +
					Common.StringToDB(ConsigneeClearanceNamePk, false, false) + ", " +
					Common.StringToDB(DepartureDate, true, false) + ", " +
					Common.StringToDB(ArrivalDate, true, false) + ", " +
					Common.StringToDB(SHIPMENTDATE, true, false) + ", " +
					Common.StringToDB(DepartureRegionCode, true, false) + ", " +
					Common.StringToDB(ArrivalRegionCode, true, false) + ", " +
					Common.StringToDB(DepartureBranch, false, false) + ", " +
					Common.StringToDB(ArrivalBranch, false, false) + ", " +
					Common.StringToDB(TransportWayCL, false, false) + ", " +
					Common.StringToDB(PaymentWhoCL, false, false) + ", " +
					Common.StringToDB(DocumentRequestCL, true, false) + ", " +
					Common.StringToDB(MonetaryUnitCL, false, false) + ", 50, " +
					Common.StringToDB(Memo.Replace("\n", "<br />").Replace("\r\n", "<br />"), true, true) + ", getDate() );" +
					"SET @RequestPk=@@IDENTITY;");
				Query.Append("UPDATE Company SET LastRequestDate = GetDate() WHERE CompanyPk=" + RequestCompanyPk + "; " +
										"INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID]) VALUES ('RequestForm', @RequestPk, '50', '" + AccountID + "');");
				if (ITEMSUM != "") {
					string[] ItemRow = ITEMSUM.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
					foreach (string row in ItemRow) {
						string[] each = row.Split(Common.Splite321, StringSplitOptions.None);
						Query.Append("EXEC SP_InsertRequestFormItems @RequestFormPk=@RequestPk" +
							", @MarkNNumber=" + Common.StringToDB(each[0], true, false) +
							", @Description=" + Common.StringToDB(each[1].Replace("'", "").Replace("\"", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace("=", "").Replace("+", "").Replace(";", "").Replace(":", ""), true, true) +
							", @Label=" + Common.StringToDB(each[2], true, true) +
							", @Material=" + Common.StringToDB(each[3], true, true) +
							", @Quantity=" + Common.StringToDB(each[4], false, false) +
							", @QuantityUnit=" + Common.StringToDB(each[5], false, false) +
							", @PackedCount=" + Common.StringToDB(each[8], false, false) +
							", @PackingUnit=" + Common.StringToDB(each[9], false, false) +
							", @GrossWeight=" + Common.StringToDB(each[10], false, false) +
							", @Volume=" + Common.StringToDB(each[11], false, false) +
							", @UnitPrice=" + Common.StringToDB(each[6], false, false) +
							", @Amount=" + Common.StringToDB(each[7], false, false) + ";");
					}
				}
				if (MemoForAdmin != "") {
					Query.Append("INSERT INTO [dbo].[COMMENT] ([TABLE_NAME], [TABLE_PK], [CATEGORY], [ACCOUNT_ID], [CONTENTS]) VALUES ('RequestForm', @RequestPk, 'Request', '" + AccountID + "', N'" + MemoForAdmin.Replace("\n", "<br />").Replace("\r\n", "<br />") + "' );");
				}

			}
			DB.SqlCmd.CommandText = Query + "";
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		} catch (Exception ex) {
			return ex.Message;
		}
	}
	[WebMethod]
	public String RequestFormCalculateHeadModify(string RequestFormPk, string PackingUnit, string PackedCount, string Weight, string Volume) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE [dbo].[RequestForm] SET [TotalPackedCount] = " + Common.StringToDB(PackedCount, false, false) +
			", [PackingUnit] = " + Common.StringToDB(PackingUnit, false, false) +
			", [TotalGrossWeight] = " + Common.StringToDB(Weight, false, false) +
			", [TotalVolume] = " + Common.StringToDB(Volume, false, false) +
			" WHERE RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String RequestModify(string RequestFormPk, string ShipperPk, string ConsigneePk, string AccountID, string ShipperCode, string ConsigneeCode, string ShipperClearanceNamePk, string ConsigneeClearanceNamePk, string DepartureDate, string ArrivalDate, string SHIPMENTDATE, string DepartureRegion, string ArrivalRegion, string TransportWayCL, string PaymentWhoCL, string DocumentRequestCL, string MonetaryUnitCL, string Memo, string MemoForAdmin, string DepartureBranch, string ArrivalBranch, string ITEMSUM) {
		try {
			DB = new DBConn();
			DB.DBCon.Open();
			DB.SqlCmd.CommandText = "Select RegionCode from RegionCode where RegionCodePk=" + DepartureRegion + ";";
			string DepartureRegionCode = DB.SqlCmd.ExecuteScalar() + "";
			DB.SqlCmd.CommandText = "Select RegionCode from RegionCode where RegionCodePk=" + ArrivalRegion + ";";
			string ArrivalRegionCode = DB.SqlCmd.ExecuteScalar() + "";
			StringBuilder Query = new StringBuilder();
			Query.Append("UPDATE [RequestForm] SET [ShipperPk] = " + Common.StringToDB(ShipperPk, false, false) +
				", [ConsigneePk] = " + Common.StringToDB(ConsigneePk, false, false) +
				", [ShipperCode] = " + Common.StringToDB(ShipperCode, true, false) +
				", [ConsigneeCode] = " + Common.StringToDB(ConsigneeCode, true, false) +
				", [ShipperClearanceNamePk] = " + Common.StringToDB(ShipperClearanceNamePk, false, false) +
				", [ConsigneeClearanceNamePk] = " + Common.StringToDB(ConsigneeClearanceNamePk, false, false) +
				", [DepartureDate] = " + Common.StringToDB(DepartureDate, true, false) +
				", [ArrivalDate] = " + Common.StringToDB(ArrivalDate, true, false) +
				", [SHIPMENTDATE]=" + Common.StringToDB(SHIPMENTDATE, true, false) +
				", [DepartureRegionCode] = " + Common.StringToDB(DepartureRegionCode, true, false) +
				", [ArrivalRegionCode] = " + Common.StringToDB(ArrivalRegionCode, true, false) +
				", [DepartureBranchPk] = " + Common.StringToDB(DepartureBranch, false, false) +
				", [ArrivalBranchPk] = " + Common.StringToDB(ArrivalBranch, false, false) +
				", [TransportWayCL] = " + Common.StringToDB(TransportWayCL, false, false) +
				", [PaymentWhoCL] =" + Common.StringToDB(PaymentWhoCL, false, false) +
				", [DocumentRequestCL] = " + Common.StringToDB(DocumentRequestCL, true, false) +
				", [MonetaryUnitCL] = " + Common.StringToDB(MonetaryUnitCL, false, false) +
				", [Memo] = " + Common.StringToDB(Memo.Replace("\n", "<br />").Replace("\r\n", "<br />"), true, true) +
			" WHERE RequestFormPk=" + RequestFormPk + ";");
			Query.Append("INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID]) VALUES ('RequestForm', " + RequestFormPk + ", '61', '" + AccountID + "');");

			if (ITEMSUM != "") {
				string[] ItemRow = ITEMSUM.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
				foreach (string row in ItemRow) {
					string[] each = row.Split(Common.Splite321, StringSplitOptions.None);
					if (each[0] == "NEW") {
						Query.Append("EXEC SP_InsertRequestFormItems @RequestFormPk=" + RequestFormPk +
							", @MarkNNumber=" + Common.StringToDB(each[1], true, false) +
							", @Description=" + Common.StringToDB(each[2], true, true) +
							", @Label=" + Common.StringToDB(each[3], true, true) +
							", @Material=" + Common.StringToDB(each[4], true, true) +
							", @Quantity=" + Common.StringToDB(each[5], false, false) +
							", @QuantityUnit=" + Common.StringToDB(each[6], false, false) +
							", @PackedCount=" + Common.StringToDB(each[7], false, false) +
							", @PackingUnit=" + Common.StringToDB(each[8], false, false) +
							", @GrossWeight=" + Common.StringToDB(each[9], false, false) +
							", @Volume=" + Common.StringToDB(each[10], false, false) +
							", @UnitPrice=" + Common.StringToDB(each[11], false, false) +
							", @Amount=" + Common.StringToDB(each[12], false, false) + ";");
					} else {
						Query.Append("UPDATE [RequestFormItems] SET " +
							"[MarkNNumber] = " + Common.StringToDB(each[1], true, false) +
							", [Description] =" + Common.StringToDB(each[2], true, true) +
							", [Label] =" + Common.StringToDB(each[3], true, true) +
							", [Material] = " + Common.StringToDB(each[4], true, true) +
							", [Quantity] = " + Common.StringToDB(each[5], false, false) +
							", [QuantityUnit] = " + Common.StringToDB(each[6], false, false) +
							", [PackedCount] = " + Common.StringToDB(each[7], false, false) +
							", [PackingUnit] = " + Common.StringToDB(each[8], false, false) +
							", [GrossWeight] = " + Common.StringToDB(each[9], false, false) +
							", [Volume] = " + Common.StringToDB(each[10], false, false) +
							", [UnitPrice] = " + Common.StringToDB(each[11], false, false) +
							", [Amount] = " + Common.StringToDB(each[12], false, false) +
							", [LastModify] = getDate() " +
						" WHERE RequestFormItemsPk=" + each[0] + ";");

					}
				}
			}
			if (MemoForAdmin != "") {
				Query.Append("INSERT INTO [dbo].[COMMENT] ([TABLE_NAME], [TABLE_PK], [CATEGORY], [ACCOUNT_ID], [CONTENTS]) VALUES ('RequestForm', " + RequestFormPk + ", 1, '" + AccountID + "', N'" + MemoForAdmin.Replace("\n", "<br />").Replace("\r\n", "<br />") + "' );");
			}
			DB.SqlCmd.CommandText = Query + "";
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		} catch (Exception ex) {
			return DB.SqlCmd.CommandText;
		}
	}
	[WebMethod]
	public String RequestFormWrite(string MemberPk, string MemberCode, string AccountID, string CCLPk, string ConsigneeCode, string ConsigneePk, string ExportRegionCode, string ImportRegionCode, string DepartureDate, string ArrivalDate, string MonetaryUnit, string ItemInfoGroup0, string ItemSenderInfo0, string ExportWay, string TongGuanWay, string PaymentWay, string PaymentWho, string DocumentRequest, string DepartureAreaBranchCode, string TransportWay, string FCLData, string RequestFormETC) {

		StringBuilder Query = new StringBuilder();
		string QShipperStaff = string.Empty;
		if (ItemSenderInfo0 != "@@@@") {
			string[] ShipperStaff = ItemSenderInfo0.Split(Common.Splite22, StringSplitOptions.None);
			QShipperStaff += ", @ShipperStaffName=" + Common.StringToDB(ShipperStaff[0], true, true);
			QShipperStaff += ", @ShipperStaffTEL=" + Common.StringToDB(ShipperStaff[1], true, false);
			QShipperStaff += ", @ShipperStaffMobile=" + Common.StringToDB(ShipperStaff[2], true, false);
		}

		string QPickupRequestDateNAddress = string.Empty;   //픽업일부터 자기통관 문서까징
		string exportWayCode = string.Empty;
		//0 선택안함 1 픽업예약##픽업요청일시 2 직접배송 3 기타
		// 주의 기타 3을 13으로 바꿔야함!!!!
		exportWayCode = ExportWay[0] == '3' ? "13" : ExportWay[0] + "";
		//PickupRequestAddress
		if (ExportWay[0] == '1') {
			QPickupRequestDateNAddress += ", @PickupRequestDate='" + ExportWay.Substring(1, ExportWay.Length - 1) + "'";
		}

		string[] PaymentWaySplit = PaymentWay.Split(new Char[] { '!' }, StringSplitOptions.None);
		if (PaymentWaySplit[0] == "3") {
			QPickupRequestDateNAddress += ", @NotifyPartyName=N'" + PaymentWaySplit[1] + "'";
			if (PaymentWaySplit[2] != null && PaymentWaySplit[2] != "") {
				QPickupRequestDateNAddress += ", @NotifyPartyAddress=N'" + PaymentWaySplit[2] + "'";
			}
		} else if (PaymentWaySplit[0] == "4") {
			if (PaymentWaySplit[1] != "Same") {
				QPickupRequestDateNAddress += ", @NotifyPartyName='" + PaymentWaySplit[1] + "'";
				if (PaymentWaySplit[2] != null && PaymentWaySplit[2] != "") {
					QPickupRequestDateNAddress += ", @NotifyPartyAddress='" + PaymentWaySplit[2] + "'";
				}
			}
		}

		DB = new DBConn();

		DB.DBCon.Open();
		string ExportRegion = string.Empty;
		DB.SqlCmd.CommandText = "Select RegionCode from RegionCode where RegionCodePk=" + ExportRegionCode;
		ExportRegion = "'" + DB.SqlCmd.ExecuteScalar() + "'";
		string ImportRegion = string.Empty;
		DB.SqlCmd.CommandText = "Select RegionCode, OurBranchCode from RegionCode where RegionCodePk=" + ImportRegionCode;
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ImportRegion = "'" + RS[0] + "'";
			DepartureAreaBranchCode = RS[1] + "";
		}
		RS.Dispose();
		DB.DBCon.Close();


		Query.Append("	declare @Pk int ;" +
								"	EXEC @Pk=SP_InsertRequestForm @ShipperPk=" + MemberPk +
										", @CompanyInDocumentPk=" + TongGuanWay +
										", @ConsigneePk=" + Common.StringToDB(ConsigneePk, false, false) +
										", @AccountID='" + AccountID + "'" +
										", @ConsigneeCCLPk=" + CCLPk +
										", @ShipperCode=" + Common.StringToDB(MemberCode, true, false) +
										", @ConsigneeCode=" + Common.StringToDB(ConsigneeCode, true, false) +
										", @DepartureDate='" + DepartureDate + "', @ArrivalDate='" + ArrivalDate + "', @DepartureRegionCode=" + ExportRegion +
										", @ArrivalRegionCode=" + ImportRegion +
										", @DepartureAreaBranchCode=" + DepartureAreaBranchCode
										+ QShipperStaff +
										", @TransportWayCL=" + Common.StringToDB(TransportWay, false, false) + ", @JubsuWayCL=" + exportWayCode +
										", @PaymentWayCL=" + PaymentWay[0] +
										", @PaymentWhoCL=" + PaymentWho[0] +
										", @DocumentRequestCL=" + Common.StringToDB(DocumentRequest, true, false) +
										", @MonetaryUnitCL=" + MonetaryUnit +
										", @Memo=" + (RequestFormETC == "N" ? "" : Common.StringToDB(RequestFormETC, true, true)) + ";" +
										"UPDATE Company SET LastRequestDate = GetDate() WHERE CompanyPk=" + MemberPk + "; " +
										"INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID]) VALUES ('RequestForm', @Pk, '50', '" + AccountID + "');");
		if (TransportWay == "31" && FCLData != "0") {
			Query.Append("INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [DESCRIPTION], [ACCOUNT_ID]) VALUES (@Pk, '72', '" + FCLData + "' ," + AccountID + ");");
		}

		////////////////////////////////////////
		string[] Separators2 = new string[] { "@@" };
		string[] ItemInfo = ItemInfoGroup0.Split(new string[] { "!!!!" }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < ItemInfo.Length; i++) {
			string[] EachItem = ItemInfo[i].Split(Separators2, StringSplitOptions.None);
			Query.Append("EXEC SP_InsertRequestFormItems @RequestFormPk=@Pk" +
				", @MarkNNumber=" + Common.StringToDB(EachItem[0], true, false) +
				", @Description=" + Common.StringToDB(EachItem[1], true, true) +
				", @Label=" + Common.StringToDB(EachItem[2], true, true) +
				", @Material=" + Common.StringToDB(EachItem[3], true, true) +
				", @Quantity=" + Common.StringToDB(EachItem[4], false, false) +
				", @QuantityUnit=" + Common.StringToDB(EachItem[5], false, false) +
				", @PackedCount=" + Common.StringToDB(EachItem[8], false, false) +
				", @PackingUnit=" + Common.StringToDB(EachItem[9], false, false) +
				", @GrossWeight=" + Common.StringToDB(EachItem[10], false, false) +
				", @Volume=" + Common.StringToDB(EachItem[11], false, false) +
				", @UnitPrice=" + Common.StringToDB(EachItem[6], false, false) +
				", @Amount=" + Common.StringToDB(EachItem[7], false, false) + ";");
		}
		///////////////////////////////////
		DB.SqlCmd.CommandText = Query + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	private String RegionCodePkConvertToRegionCode(string RegionCodePk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT RegionCode FROM RegionCode WHERE RegionCodePk=" + RegionCodePk + ";";
		DB.DBCon.Open();
		string result = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		return result;
	}
	[WebMethod]
	public String RequestFormModify(string RequestFormPk, string ShipperPk, string CompanyInDocumentPk, string DepartureDate, string ArrivalDate, string DepartureRegionCode, string ArrivalRegionCode, string ShipperStaffName, string ShipperStaffTEL, string ShipperStaffMobile, string TransportWayCL, string JubsuWayCL, string PaymentWayCL, string PaymentWhoCL, string DocumentRequestCL, string MonetaryUnitCL, string PickupRequestDate, string NotifyPartyName, string NotifyPartyAddress, string Memo, string ItemInfoGroup0, string FCLData) {
		GetQuery GQ = new GetQuery();
		StringBuilder Query = new StringBuilder();
		StringBuilder QueryF = new StringBuilder();
		if (CompanyInDocumentPk != "") {
			Query.Append(", CompanyInDocumentPk = " + Common.StringToDB(CompanyInDocumentPk, false, false));
		}
		if (DepartureDate != "") {
			Query.Append(", DepartureDate = " + Common.StringToDB(DepartureDate, true, false));
		}
		if (ArrivalDate != "") {
			Query.Append(", ArrivalDate = " + Common.StringToDB(ArrivalDate, true, false));
		}
		if (DepartureRegionCode != "") {
			Query.Append(", DepartureRegionCode = " + Common.StringToDB(RegionCodePkConvertToRegionCode(DepartureRegionCode), true, true));
		}
		if (ArrivalRegionCode != "") {
			Query.Append(", ArrivalRegionCode = " + Common.StringToDB(RegionCodePkConvertToRegionCode(ArrivalRegionCode), true, true));
		}
		if (ShipperStaffName != "") {
			Query.Append(", ShipperStaffName = " + Common.StringToDB(ShipperStaffName, true, true));
		}
		if (ShipperStaffTEL != "") {
			Query.Append(", ShipperStaffTEL = " + Common.StringToDB(ShipperStaffTEL, true, false));
		}
		if (ShipperStaffMobile != "") {
			Query.Append(", ShipperStaffMobile = " + Common.StringToDB(ShipperStaffMobile, true, false));
		}
		if (TransportWayCL != "") {
			Query.Append(", TransportWayCL = " + Common.StringToDB(TransportWayCL, false, false));
		}
		if (JubsuWayCL != "") {
			Query.Append(", JubsuWayCL = " + Common.StringToDB(JubsuWayCL, false, false));
		}
		if (PaymentWayCL != "") {
			Query.Append(", PaymentWayCL = " + Common.StringToDB(PaymentWayCL, false, false));
		}
		if (PaymentWhoCL != "") {
			Query.Append(", PaymentWhoCL = " + Common.StringToDB(PaymentWhoCL, false, false));
		}
		if (DocumentRequestCL != "N") {
			Query.Append(", DocumentRequestCL = " + Common.StringToDB(DocumentRequestCL, true, false));
		}
		if (MonetaryUnitCL != "") {
			Query.Append(", MonetaryUnitCL = " + Common.StringToDB(MonetaryUnitCL, false, false));
		}
		if (PickupRequestDate != "") {
			Query.Append(", PickupRequestDate = " + Common.StringToDB(PickupRequestDate, true, false));
		}
		if (NotifyPartyName != "") {
			Query.Append(", NotifyPartyName = " + Common.StringToDB(NotifyPartyName, true, true));
		}
		if (NotifyPartyAddress != "") {
			Query.Append(", NotifyPartyAddress = " + Common.StringToDB(NotifyPartyAddress, true, true));
		}
		if (Memo != "") {
			Query.Append(", Memo = " + Common.StringToDB(Memo, true, true));
		}

		QueryF.Append("UPDATE RequestForm SET ShipperPk = " + ShipperPk + Query + " WHERE RequestFormPk=" + RequestFormPk + ";");
		/////////////////////////////////////////////////
		string[] EachItemRow = ItemInfoGroup0.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
		foreach (string row in EachItemRow) {
			string[] Each = row.Split(Common.Splite321, StringSplitOptions.None);
			if (Each[0] == "NEW") {
				QueryF.Append(GQ.UpdateOrInsertRequestFormItem("Insert", RequestFormPk, "", Each[1], Each[2], Each[3], Each[4], Each[5], Each[6], Each[7], Each[8], Each[9], Each[10], Each[11], Each[12]));
			} else {
				QueryF.Append(GQ.UpdateOrInsertRequestFormItem("Update", Each[0], "", Each[1], Each[2], Each[3], Each[4], Each[5], Each[6], Each[7], Each[8], Each[9], Each[10], Each[11], Each[12]));
			}
		}
		////////////////////////////////////////////////
		DB = new DBConn();
		DB.SqlCmd.CommandText = QueryF + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String ResetRequestFormCalculate(string RequestFormPk) {
		StringBuilder Query = new StringBuilder();
		DB = new DBConn();
		DB.DBCon.Open();
		Query.Append("UPDATE [dbo].[REQUESTFORMCALCULATE_HEAD] SET [TOTAL_PRICE] = 0 WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK] = " + RequestFormPk + ";");
		DB.SqlCmd.CommandText = "SELECT [REQUESTFORMCALCULATE_HEAD_PK] FROM [dbo].[REQUESTFORMCALCULATE_HEAD] WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK] = " + RequestFormPk;
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			Query.Append("UPDATE [dbo].[REQUESTFORMCALCULATE_BODY] SET [ORIGINAL_PRICE] = 0, [EXCHANGED_PRICE] = 0 WHERE [REQUESTFORMCALCULATE_HEAD_PK] = " + RS["REQUESTFORMCALCULATE_HEAD_PK"] + ";");
		}
		RS.Close();
		DB.SqlCmd.CommandText = Query + ""; ;
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String UpdateItemModify2(string MonetaryUnit, string After, string WriteID, string RequestFormPk, string[] Calculated, string CommentSum, string MemberGubun) {
		try {
			StringBuilder Query = new StringBuilder();
			DB = new DBConn();
			DB.DBCon.Open();

			if (MonetaryUnit != "") {
				Query.Append(" UPDATE RequestForm SET [MonetaryUnitCL] = " + MonetaryUnit + " WHERE RequestFormPk=" + RequestFormPk + "; ");
			}

			string GubunCL = MemberGubun == "OurBranch" ? "71" : "70";
			if (CommentSum != "") {
				HistoryC HisC = new HistoryC();
				sHistory History = new sHistory();
				History.Table_Name = "RequestForm";
				History.Table_Pk = RequestFormPk;
				History.Code = GubunCL;
				History.Account_Id = WriteID;
				History.Description = CommentSum;
				HisC.Set_History(History, ref DB);
			}

			if (GubunCL == "71") {
				Query.Append("UPDATE [dbo].[RequestForm] SET [TotalPackedCount] = " + Common.StringToDB(Calculated[0], false, false) +
					", [PackingUnit] = " + Common.StringToDB(Calculated[1], false, false) +
					", [TotalGrossWeight] = " + Common.StringToDB(Calculated[2], false, false) +
					", [TotalVolume] = " + Common.StringToDB(Calculated[3], false, false) +
				" WHERE [RequestFormPk] = " + RequestFormPk + ";");
			} else {
				Query.Append(" INSERT INTO [RequestModifyHistory] ([RequestFormPk], [Value], [AccountID], [Registerd]) VALUES (" +
					RequestFormPk + ", " + Common.StringToDB(CommentSum, true, true) + ", " + Common.StringToDB(WriteID, true, false) + ", getDate());");
			}

			string[] Row = After.Split(Common.Splite51423, StringSplitOptions.RemoveEmptyEntries);
			foreach (string EachRow in Row) {
				string[] EachValue = EachRow.Split(Common.Splite321, StringSplitOptions.None);
				if (EachValue[12] == "N") {
					Query.Append(" INSERT INTO RequestFormItems ([RequestFormPk], [MarkNNumber], [Description], [Label], [Material], [Quantity], [QuantityUnit], [PackedCount], [PackingUnit], [GrossWeight], [Volume], [UnitPrice], [Amount])" +
						" VALUES (" + RequestFormPk + ", " +
						Common.StringToDB(EachValue[0], true, false) + ", " +
						Common.StringToDB(EachValue[1].Replace("'", "").Replace("\"", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace("=", "").Replace("+", "").Replace(";", "").Replace(":", ""), true, true) + ", " +
						Common.StringToDB(EachValue[2], true, true) + ", " +
						Common.StringToDB(EachValue[3], true, true) + ", " +
						Common.StringToDB(EachValue[4], false, false) + ", " +
						Common.StringToDB(EachValue[5], false, false) + ", " +
						Common.StringToDB(EachValue[6], false, false) + ", " +
						Common.StringToDB(EachValue[7], false, false) + ", " +
						Common.StringToDB(EachValue[8], false, false) + ", " +
						Common.StringToDB(EachValue[9], false, false) + ", " +
						Common.StringToDB(EachValue[10], false, false) + ", " +
						Common.StringToDB(EachValue[11], false, false) + ");");
				} else {
					Query.Append("UPDATE RequestFormItems SET ");

					bool IsFirst = true;
					if (EachValue[0] != "*NC*") {
						if (IsFirst) {
							IsFirst = false;
						} else {
							Query.Append(", ");
						}
						Query.Append("[MarkNNumber] = " + Common.StringToDB(EachValue[0], true, false));
					}
					if (EachValue[1] != "*NC*") {
						if (IsFirst) {
							IsFirst = false;
						} else {
							Query.Append(", ");
						}
						Query.Append("[Description] = " + Common.StringToDB(EachValue[1].Replace("'", "").Replace("\"", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace("=", "").Replace("+", "").Replace(";", "").Replace(":", ""), true, true));
					}
					if (EachValue[2] != "*NC*") {
						if (IsFirst) {
							IsFirst = false;
						} else {
							Query.Append(", ");
						}
						Query.Append("[Label] = " + Common.StringToDB(EachValue[2], true, true));
					}
					if (EachValue[3] != "*NC*") {
						if (IsFirst) {
							IsFirst = false;
						} else {
							Query.Append(", ");
						}
						Query.Append("[Material] = " + Common.StringToDB(EachValue[3], true, true));
					}
					if (EachValue[4] != "*NC*") {
						if (IsFirst) {
							IsFirst = false;
						} else {
							Query.Append(", ");
						}
						Query.Append("[Quantity] = " + Common.StringToDB(EachValue[4], false, false));
					}
					if (EachValue[5] != "*NC*") {
						if (IsFirst) {
							IsFirst = false;
						} else {
							Query.Append(", ");
						}
						Query.Append("[QuantityUnit] = " + Common.StringToDB(EachValue[5], false, false));
					}
					if (EachValue[6] != "*NC*") {
						if (IsFirst) {
							IsFirst = false;
						} else {
							Query.Append(", ");
						}
						Query.Append("[PackedCount] = " + Common.StringToDB(EachValue[6], false, false));
					}
					if (EachValue[7] != "*NC*") {
						if (IsFirst) {
							IsFirst = false;
						} else {
							Query.Append(", ");
						}
						Query.Append("[PackingUnit] = " + Common.StringToDB(EachValue[7], false, false));
					}
					if (EachValue[8] != "*NC*") {
						if (IsFirst) {
							IsFirst = false;
						} else {
							Query.Append(", ");
						}
						Query.Append("[GrossWeight] = " + Common.StringToDB(EachValue[8], false, false));
					}
					if (EachValue[9] != "*NC*") {
						if (IsFirst) {
							IsFirst = false;
						} else {
							Query.Append(", ");
						}
						Query.Append("[Volume] = " + Common.StringToDB(EachValue[9], false, false));
					}
					if (EachValue[10] != "*NC*") {
						if (IsFirst) {
							IsFirst = false;
						} else {
							Query.Append(", ");
						}
						Query.Append("[UnitPrice]= " + Common.StringToDB(EachValue[10], false, false));
					}
					if (EachValue[11] != "*NC*") {
						if (IsFirst) {
							IsFirst = false;
						} else {
							Query.Append(", ");
						}
						Query.Append("[Amount]  = " + Common.StringToDB(EachValue[11], false, false));
					}
					Query.Append(" WHERE RequestFormItemsPk=" + EachValue[12] + ";");
				}
			}

			DB.SqlCmd.CommandText = Query + "";
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		} catch (Exception ex) {
			return ex.Message;
			throw;
		}
	}

	[WebMethod]
	public String AskCompanyCustomerCodeSetAuto(string RequestFormPk) {
		string ShipperPk = string.Empty;
		string Country = string.Empty;
		DB = new DBConn();
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
		switch (Country[0]) {
			case '1':
				Country = "KOR";
				break;
			case '2':
				Country = "CHN";
				break;
		}
		DB.SqlCmd.CommandText = "(SELECT cast(substring(CompanyCode, 4, len(CompanyCode)-3) as int) as CC FROM Company where left(CompanyCode, 3)='" + Country + "' union SELECT cast(substring(CompanyCode, 4, len(CompanyCode)-3) as int) as CC FROM CompanyCertification where left(CompanyCode, 3)='" + Country + "' ) order by CC";
		RS = DB.SqlCmd.ExecuteReader();
		int tempInt = 1;
		while (RS.Read()) {
			if (Int32.Parse(RS["CC"] + "") == tempInt) {
				tempInt++;
				continue;
			} else {
				break;
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return Country + tempInt + "!!" + ShipperPk;
	}//RequestFormView.aspx 에서 수출담당지사에서 Shipper 신규고객일경우 번호 검색해서 묻기
	[WebMethod]
	public String SetCompanyCustomerCodeAuto(string CompanyPk, string CompanyCode) {
		try {
			DB = new DBConn();
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
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT count(*) FROM Company where CompanyCode='" + CompanyCode + "'";
		DB.DBCon.Open();
		if (DB.SqlCmd.ExecuteScalar() + "" != "0") {
			DB.DBCon.Close();
			return "0";
		}
		DB.SqlCmd.CommandText = "SELECT count(*) FROM CompanyCertification WHERE CompanyCode='" + CompanyCode + "'";
		if (DB.SqlCmd.ExecuteScalar() + "" != "0") {
			DB.DBCon.Close();
			return "0";
		}
		DB.SqlCmd.CommandText = "UPDATE Company SET CompanyCode='" + CompanyCode + "' WHERE CompanyPk=" + CompanyPk + "; UPDATE RequestForm SET ShipperCode='" + CompanyCode + "' WHERE ShipperPk=" + CompanyPk + ";";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return CompanyCode;
	}
	[WebMethod]
	public string AskCompanyCodeUsed(string CompanyCode) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [CompanyPk] ,[CompanyName] FROM Company WHERE [CompanyCode]='" + CompanyCode + "'";
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
	public String GetRegionCodeJs() {
		StringBuilder ReturnValue = new StringBuilder();
		string[] RegionCode;
		char[] Separators1 = new char[] { '!' };
		DB = new DBConn();
		DB.SqlCmd.CommandText = "Select RegionCodePk, RegionCode, Name, NameE, OrderBy From RegionCode where len(RegionCode)>1 and left(RegionCode, 1)=2 order by RegionCode";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		bool flag = true;
		int temp2 = 0;
		int temp3 = 0;
		while (RS.Read()) {
			if (RS["OrderBy"] + "" == "1") {
				flag = true;
			} else if (RS["OrderBy"] + "" == "0") {
				flag = false;
			}

			if (flag) {
				RegionCode = RS[1].ToString().Split(Separators1, StringSplitOptions.None);
				if (RegionCode.Length == 2) {
					temp2++;
					temp3 = 0;
					ReturnValue.Append("cate2[2][" + temp2 + "]= new Option('" + RS[0] + "', '" + RS["Name"] + " | " + RS["NameE"] + "');");
					continue;
				}
				if (RegionCode.Length == 3) {
					temp3++;
					ReturnValue.Append("cate3[2][" + temp2 + "][" + temp3 + "]= new Option('" + RS[0] + "', '" + RS["Name"] + " | " + RS["NameE"] + "');");
					continue;
				}
			}
		}
		return ReturnValue.ToString();
	}
	[WebMethod]
	public string InsertShipperNameInDocument(string GubunCL, string CompanyPk, string ShipperName, string ShipperAddress) {
		DB = new DBConn();

		DB.SqlCmd.CommandText = "INSERT INTO CompanyInDocument ([GubunCL], [GubunPk], [Name], [Address]) VALUES (" + GubunCL + ", " + CompanyPk + ", '" + ShipperName + "', '" + ShipperAddress + "'); " +
													"SELECT @@IDENTITY;";
		DB.DBCon.Open();
		string ReturnValue = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		return ReturnValue + "##" + ShipperName + "##" + ShipperAddress;
	}
	[WebMethod]
	public string InsertShipperNameInDocument_CustomsCode(string GubunCL, string CompanyPk, string ShipperName, string ShipperAddress, string CompanyNo, string CustomsCode, string ZipCode1, string ZipCode2, string Name_KOR, string Address_KOR, string Title, string HiddenPk) {
		DB = new DBConn();
		string ReturnValue = "";
		CompanyNo = CompanyNo == "" ? " NULL " : "'" + CompanyNo + "'";
		CustomsCode = CustomsCode == "" ? " NULL " : "'" + CustomsCode + "'";
		Title = Title == "" ? " NULL " : "'" + Title + "'";
		ZipCode1 = ZipCode1 == "" ? " NULL " : "'" + ZipCode1 + "'";
		ZipCode2 = ZipCode2 == "" ? " NULL " : "'" + ZipCode2 + "'";
		Name_KOR = Name_KOR == "" ? " NULL " : "'" + Name_KOR + "'";
		Address_KOR = Address_KOR == "" ? " NULL " : "'" + Address_KOR + "'";

		if (HiddenPk + "" == "") {
			DB.SqlCmd.CommandText = "INSERT INTO CompanyInDocument ([GubunCL], [GubunPk], [Name], [Address],[CompanyNo],[CustomsCode],[Title],[ZipCode1],[ZipCode2],[Name_KOR],[Address_KOR]) VALUES (" + GubunCL + ", " + CompanyPk + ", '" + ShipperName + "', '" + ShipperAddress + "', " + CompanyNo + ", " + CustomsCode + ", " + Title + ", " + ZipCode1 + ", " + ZipCode2 + ", " + Name_KOR + ", " + Address_KOR + ");" +
													"SELECT @@IDENTITY;";
			DB.DBCon.Open();
			ReturnValue = DB.SqlCmd.ExecuteScalar() + "";
		} else {
			DB.SqlCmd.CommandText = " UPDATE [dbo].[CompanyInDocument] SET " +
	  "  [CompanyNo] = " + CompanyNo +
	  " ,[CustomsCode] = " + CustomsCode +
	  " ,[Title] = " + Title +
	  " ,[ZipCode1] = " + ZipCode1 +
	  " ,[ZipCode2] = " + ZipCode2 +
	  " ,[Name_KOR] = " + Name_KOR +
	  " ,[Address_KOR] = " + Address_KOR +
 " WHERE [CompanyInDocumentPk]=" + HiddenPk + ";";

			DB.DBCon.Open();
			DB.SqlCmd.ExecuteNonQuery();
			ReturnValue = HiddenPk + "";
		}

		DB.DBCon.Close();
		return ReturnValue + "##" + ShipperName + "##" + ShipperAddress;
	}
	[WebMethod]
	public string InsertShipperNameInDocument_Transport(string GubunCL, string ShipperName, string ShipperAddress, string Memo) {
		DB = new DBConn();
		string ReturnValue = "";


		DB.SqlCmd.CommandText = "INSERT INTO CompanyInDocument ([GubunCL], [GubunPk], [Name], [Address],[Address_KOR]) VALUES (" + GubunCL + ", 3157, N'" + ShipperName + "', N'" + ShipperAddress + "', N'" + Memo + "');" +
												"SELECT @@IDENTITY;";
		DB.DBCon.Open();
		ReturnValue = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();

		return ReturnValue + "##" + ShipperName + "##" + ShipperAddress;
	}

	[WebMethod]
	public string SetPrimaryShipperNameInDocument(string CompanyPk, string TargetPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = string.Format(@"
			DELETE FROM [dbo].[CompanyAdditionalInfomation] WHERE Title=99 and CompanyPk={0}; 
			INSERT INTO CompanyAdditionalInfomation ([CompanyPk], [Title], [Value]) VALUES ({0}, 99, {1});", CompanyPk, TargetPk);
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public string DelShipperNameInDocument(string RequestFormShipperPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE CompanyInDocument SET GubunCL = 10 WHERE CompanyInDocumentPk=" + RequestFormShipperPk;
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String InsertCompanyInDocument(string CompanyPk, string Name, string Address) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "INSERT INTO CompanyInDocument ([GubunCL], [GubunPk], [Name], [Address]) VALUES (0, " + CompanyPk + ", '" + Name + "', '" + Address + "'); SELECT @@IDENTITY;";
		DB.DBCon.Open();
		string ReturnValue = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		return ReturnValue + "!!" + Name + "@@" + Address;
	}
	[WebMethod]
	public String OfferSave(string ShipperPk, string AccountID, string ShipperCode, string MonetaryUnitCL, string StepCL, string ItemSum) {
		GetQuery GQ = new GetQuery();
		DB = new DBConn();
		//DB.SqlCmd.CommandText = GQ.InsertRequestForm(ShipperPk, ConsigneePk, AccountID, CCLPk, ShipperCode, CompanyInDocumentPk, ConsigneeCode, MonetaryUnitCL, StepCL);
		DB.SqlCmd.CommandText = GQ.InsertRequestForm(ShipperPk, AccountID, ShipperCode, MonetaryUnitCL, StepCL);
		DB.DBCon.Open();
		string RequestFormPk = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		string[] ItemArray = ItemSum.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
		StringBuilder QueryItemInsert = new StringBuilder();
		foreach (string EachItem in ItemArray) {
			string[] EachArray = EachItem.Split(Common.Splite22, StringSplitOptions.None);
			QueryItemInsert.Append("EXEC SP_InsertRequestFormItems @RequestFormPk=" + RequestFormPk + ", " +
																										"@MarkNNumber=" + Common.StringToDB(EachArray[0], true, false) + ", " +
																										"@Description=" + Common.StringToDB(EachArray[1], true, true) + ", " +
																										"@Label=" + Common.StringToDB(EachArray[2], true, true) + ", " +
																										"@Material=" + Common.StringToDB(EachArray[3], true, true) + ", " +
																										"@Quantity=" + Common.StringToDB(EachArray[4], false, false) + ", " +
																										"@QuantityUnit=" + Common.StringToDB(EachArray[5], false, false) + ", " +
																										"@PackedCount=" + Common.StringToDB(EachArray[6], false, false) + ", " +
																										"@PackingUnit=" + Common.StringToDB(EachArray[7], false, false) + ", " +
																										"@GrossWeight=" + Common.StringToDB(EachArray[10], false, false) + ", " +
																										"@Volume=" + Common.StringToDB(EachArray[11], false, false) + ", " +
																										"@UnitPrice=" + Common.StringToDB(EachArray[8], false, false) + ", " +
																										"@Amount=" + Common.StringToDB(EachArray[9], false, false) + ";");
		}
		HistoryC HisC = new HistoryC();
		sHistory History = new sHistory();
		History.Table_Name = "RequestForm";
		History.Table_Pk = RequestFormPk;
		History.Code = "2";
		History.Account_Id = AccountID;
		History.Description = "0:!" + AccountID + "@" + DateTime.Now.GetDateTimeFormats('u')[0] + "####";
		HisC.Set_History(History, ref DB);

		DB.SqlCmd.CommandText = QueryItemInsert + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return RequestFormPk;
	}
	[WebMethod]
	public String OfferSaveOrUpdate(string Gubun, string RequestFormPk, string ShipperPk, string AccountID, string ShipperCode, string MonetaryUnitCL, string ItemSum) {
		try {
			GetQuery GQ = new GetQuery();
			DB = new DBConn();
			string[] ItemArray;
			string[] EachArray;
			StringBuilder Query = new StringBuilder();

			switch (Gubun) {
				case "Modify":
					Query.Append("UPDATE RequestForm SET [MonetaryUnitCL] = " + MonetaryUnitCL + " WHERE RequestFormPk=" + RequestFormPk + ";");
					ItemArray = ItemSum.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
					foreach (string EachItem in ItemArray) {
						EachArray = EachItem.Split(Common.Splite22, StringSplitOptions.None);
						/*		0	ItemPk		1	ItemCode		2	BoxNo		3	ItemName		4	Brand		5	Matarial		6	Quantity		7	QuantityUnit		8	UnitCost		9	Price		10	Weight		11	Volume */


						switch (EachArray[0]) {
							case "N":
								Query.Append("EXEC SP_InsertRequestFormItems @RequestFormPk=" + RequestFormPk + ", " +
																											"@ItemCode=" + Common.StringToDB(EachArray[1], false, false) + ", " +
																											"@MarkNNumber=" + Common.StringToDB(EachArray[2], true, false) + ", " +
																											"@Description=" + Common.StringToDB(EachArray[3], true, true) + ", " +
																											"@Label=" + Common.StringToDB(EachArray[4], true, true) + ", " +
																											"@Material=" + Common.StringToDB(EachArray[5], true, true) + ", " +
																											"@Quantity=" + Common.StringToDB(EachArray[6], false, false) + ", " +
																											"@QuantityUnit=" + Common.StringToDB(EachArray[7], false, false) + ", " +
																											"@PackedCount=" + Common.StringToDB(EachArray[8], false, false) + ", " +
																											"@PackingUnit=" + Common.StringToDB(EachArray[9], false, false) + ", " +
																											"@GrossWeight=" + Common.StringToDB(EachArray[12], false, false) + ", " +
																											"@Volume=" + Common.StringToDB(EachArray[13], false, false) + ", " +
																											"@UnitPrice=" + Common.StringToDB(EachArray[10], false, false) + ", " +
																											"@Amount=" + Common.StringToDB(EachArray[11], false, false) + ";");
								break;
							case "!@#DeleteRow":
								Query.Append("DELETE FROM RequestFormItems WHERE RequestFormItemsPk=" + EachArray[1] + ";");
								break;
							default:
								Query.Append("UPDATE RequestFormItems SET ItemCode = " + Common.StringToDB(EachArray[1], false, false) + ", " +
																									  "MarkNNumber = " + Common.StringToDB(EachArray[2], true, false) + ", " +
																									  "Description = " + Common.StringToDB(EachArray[3], true, true) + ", " +
																									  "Label=" + Common.StringToDB(EachArray[4], true, true) + ", " +
																									  "Material=" + Common.StringToDB(EachArray[5], true, true) + ", " +
																									  "Quantity=" + Common.StringToDB(EachArray[6], false, false) + ", " +
																									  "QuantityUnit=" + Common.StringToDB(EachArray[7], false, false) + ", " +
																									  "PackedCount=" + Common.StringToDB(EachArray[8], false, false) + ", " +
																									  "PackingUnit=" + Common.StringToDB(EachArray[9], false, false) + ", " +
																									  "GrossWeight=" + Common.StringToDB(EachArray[12], false, false) + ", " +
																									  "Volume=" + Common.StringToDB(EachArray[13], false, false) + ", " +
																									  "UnitPrice=" + Common.StringToDB(EachArray[10], false, false) + ", " +
																									  "Amount=" + Common.StringToDB(EachArray[11], false, false) + ", " +
																									  "LastModify=GetDate() " +
													" WHERE RequestFormItemsPk=" + EachArray[0] + ";");
								break;
						}
					}
					string Value = "3:!" + AccountID + "@" + DateTime.Now.GetDateTimeFormats('u')[0] + "####";
					Query.Append(@"DECLARE @Before nvarchar(max)=null;
							SELECT @Before=[DESCRIPTION] FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK]=" + RequestFormPk + @" and [CODE]='2';
							UPDATE [dbo].[HISTORY] SET [DESCRIPTION] = @Before+" + Value + @" WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK]=" + RequestFormPk + @" and [CODE]='2';");

					DB.SqlCmd.CommandText = Query + "";
					DB.DBCon.Open();
					DB.SqlCmd.ExecuteNonQuery();
					DB.DBCon.Close();
					break;
				case "SaveAs":
					DB.SqlCmd.CommandText = GQ.InsertRequestForm(ShipperPk, AccountID, ShipperCode, MonetaryUnitCL, "0");
					DB.DBCon.Open();
					RequestFormPk = DB.SqlCmd.ExecuteScalar() + "";
					DB.DBCon.Close();
					ItemArray = ItemSum.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
					foreach (string EachItem in ItemArray) {
						EachArray = EachItem.Split(Common.Splite22, StringSplitOptions.None);
						Query.Append("EXEC SP_InsertRequestFormItems @RequestFormPk=" + RequestFormPk + ", " +
																									"@ItemCode=" + Common.StringToDB(EachArray[1], false, false) + ", " +
																									"@MarkNNumber=" + Common.StringToDB(EachArray[2], true, false) + ", " +
																									"@Description=" + Common.StringToDB(EachArray[3], true, true) + ", " +
																									"@Label=" + Common.StringToDB(EachArray[4], true, true) + ", " +
																									"@Material=" + Common.StringToDB(EachArray[5], true, true) + ", " +
																									"@Quantity=" + Common.StringToDB(EachArray[6], false, false) + ", " +
																									"@QuantityUnit=" + Common.StringToDB(EachArray[7], false, false) + ", " +
																								  "@PackedCount=" + Common.StringToDB(EachArray[8], false, false) + ", " +
																								  "@PackingUnit=" + Common.StringToDB(EachArray[9], false, false) + ", " +
																									"@GrossWeight=" + Common.StringToDB(EachArray[12], false, false) + ", " +
																									"@Volume=" + Common.StringToDB(EachArray[13], false, false) + ", " +
																									"@UnitPrice=" + Common.StringToDB(EachArray[10], false, false) + ", " +
																									"@Amount=" + Common.StringToDB(EachArray[11], false, false) + ";");
					}
					DB.SqlCmd.CommandText = Query + "";
					DB.DBCon.Open();
					DB.SqlCmd.ExecuteNonQuery();
					HistoryC HIsC = new HistoryC();
					sHistory History = new sHistory();
					History.Table_Name = "RequestForm";
					History.Table_Pk = RequestFormPk;
					History.Code = "2";
					History.Account_Id = AccountID;
					History.Description = "0:!" + AccountID + "@" + DateTime.Now.GetDateTimeFormats('u')[0] + "####";
					HIsC.Set_History(History, ref DB);
					DB.DBCon.Close();
					break;
			}
			return Gubun[0] + RequestFormPk;
		} catch (Exception ex) {
			return ex.Message;

		}
	}

	[WebMethod]
	public void OfferSend() {
		new Email().SendMailByCafe24("Stu83@korea.com", "인간김상수", "stud83@gmail.com", "야 김상수!!", "안녕~", "잘 전달이 됐겠지?");
	}
	[WebMethod]
	public String LoadSavedOffer(string RequestFormPk) {
		StringBuilder ReturnValue = new StringBuilder();
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"SELECT RFI.RequestFormItemsPk, RFI.ItemCode, RFI.MarkNNumber, RFI.Description, RFI.Label, RFI.Material, RFI.Quantity, RFI.QuantityUnit, RFI.PackedCount, 
																	RFI.PackingUnit, RFI.GrossWeight, RFI.Volume, RFI.UnitPrice, RFI.Amount, RF.MonetaryUnitCL, RF.StepCL
														 FROM RequestForm as RF left outer join RequestFormItems as RFI on RF.RequestFormPk=RFI.RequestFormPk
														 WHERE RF.RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		/*	0		RFI.RequestFormItemsPk, 
		 *	1		RFI.ItemCode, 
		 *	2		RFI.MarkNNumber, 
		 *	3		RFI.Description, 
		 *	4		RFI.Label, 
		 *	5		RFI.Material, 
		 *	6		RFI.Quantity, 
		 *	7		RFI.QuantityUnit, 
		 *	8		RFI.PackedCount, 
		 *	9	RFI.PackingUnit, 
		 *	10	RFI.GrossWeight, 
		 *	11	RFI.Volume, 
		 *	12	RFI.UnitPrice, 
		 *	13	RFI.Amount, 
		 *	14	RF.MonetaryUnitCL, 
		 *	15	RF.StepCL		*/

		if (RS.Read()) {
			ReturnValue.Append(RS[14] + "!$^" + RS[15] + "@@" + RS[0] + "!$^" + RS[1] + "!$^" + RS[2] + "!$^" + RS[3] + "!$^" + RS[4] + "!$^" + RS[5] + "!$^" + Common.NumberFormat(RS[6] + "") + "!$^" + RS[7] + "!$^" + Common.NumberFormat(RS[8] + "") + "!$^" + RS[9] + "!$^" + Common.NumberFormat(RS[10] + "") + "!$^" + Common.NumberFormat(RS[11] + "") + "!$^" + Common.NumberFormat(RS[12] + "") + "!$^" + Common.NumberFormat(RS[13] + ""));
			while (RS.Read()) {
				ReturnValue.Append("@@" + RS[0] + "!$^" + RS[1] + "!$^" + RS[2] + "!$^" + RS[3] + "!$^" + RS[4] + "!$^" + RS[5] + "!$^" + Common.NumberFormat(RS[6] + "") + "!$^" + RS[7] + "!$^" + Common.NumberFormat(RS[8] + "") + "!$^" + RS[9] + "!$^" + Common.NumberFormat(RS[10] + "") + "!$^" + Common.NumberFormat(RS[11] + "") + "!$^" + Common.NumberFormat(RS[12] + "") + "!$^" + Common.NumberFormat(RS[13] + ""));
			}
		}
		return ReturnValue + "";
	}

	[WebMethod]
	public String[] LoadInvoice(string RequestFormPk, string Gubun) {
		List<string> ReturnValue = new List<string>();
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT R.[ShipperPk], R.[DepartureDate], R.[ShipperStaffName], R.[ShipperStaffTEL], R.[ShipperStaffMobile], R.[PaymentWayCL], R.[MonetaryUnitCL], R.[StepCL], R.[NotifyPartyName], R.[NotifyPartyAddress], R.[Memo], 
	R.[DepartureRegionCode], R.[ArrivalRegionCode]
FROM [RequestForm] AS R 
WHERE 	R.RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue.Add(RS["DepartureDate"] + "#@!" + RS["ShipperStaffName"] + "#@!" + RS["ShipperStaffTEL"] + "#@!" + RS["ShipperStaffMobile"] + "#@!" + RS["PaymentWayCL"] + "#@!" + RS["MonetaryUnitCL"] + "#@!" +
				RS["StepCL"] + "#@!" +
				RS["DepartureRegionCode"] + "#@!" +
				RS["ArrivalRegionCode"] + "#@!" +
				RS["NotifyPartyAddress"] +
				"#@!" + RS["NotifyPartyName"] + "#@!" +
				RS["Memo"]);
		} else {
			RS.Dispose();
			DB.DBCon.Close();
			return new string[] { "N" };
		}
		RS.Dispose();
		if (Gubun == "C") {
			DB.SqlCmd.CommandText = @"
SELECT [RequestFormItemsPk], [Description], [Label], [Material], [Quantity], [QuantityUnit], [UnitPrice], [Amount] 
FROM [RequestFormItems] 
WHERE RequestFormPk=" + RequestFormPk + ";";
			RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ReturnValue.Add(RS["RequestFormItemsPk"] + "#@!" + RS["Description"] + "#@!" + RS["Label"] + "#@!" + RS["Material"] + "#@!" + RS["Quantity"] + "#@!" + RS["QuantityUnit"] + "#@!" + RS["UnitPrice"] + "#@!" + RS["Amount"]);
			}
			RS.Dispose();
		} else {
			DB.SqlCmd.CommandText = @"
SELECT [RequestFormItemsPk], [MarkNNumber], [Description], [Label], [Material], [Quantity], [QuantityUnit], [PackedCount], [PackingUnit], [GrossWeight], [Volume] 
FROM [RequestFormItems] 
WHERE RequestFormPk=" + RequestFormPk + ";";
			RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ReturnValue.Add(RS["RequestFormItemsPk"] + "#@!" + RS["MarkNNumber"] + "#@!" + RS["Description"] + "#@!" + RS["Label"] + "#@!" + RS["Material"] + "#@!" + RS["Quantity"] + "#@!" +
					RS["QuantityUnit"] + "#@!" +
					RS["PackedCount"] + "#@!" + RS["PackingUnit"] + "#@!" + RS["GrossWeight"] + "#@!" + RS["Volume"]);
			}
			RS.Dispose();
		}
		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="RequestFormPk"></param>
	/// <param name="Mode"></param>
	/// <param name="CompanyPk">우리 지사일때는 OurBranch 를 넣어야 됩니다.</param>
	/// <returns></returns>
	[WebMethod]
	public String LoadDocuments(string RequestFormPk, string Mode, string CompanyPk) {
		StringBuilder Result = new StringBuilder();
		DB = new DBConn();
		if (Mode == "BranchModify" || Mode == "CompanyModify" || Mode == "StorcedIn") {
			DB.SqlCmd.CommandText = "EXECUTE SP_SelectRequestViewLoad @RequestFormPk=" + RequestFormPk;
		} else {
			DB.SqlCmd.CommandText = "EXECUTE SP_SelectCommercialInvoice @RequestFormPk=" + RequestFormPk;
		}

		//return DB.SqlCmd.CommandText;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			if (RS[0] + "" == CompanyPk || RS[1] + "" == CompanyPk || CompanyPk == "OurBranch") {
				for (int i = 0; i < RS.FieldCount; i++) {
					Result.Append(RS[i] + "@@");
				}
				RS.Dispose();
			} else {
				RS.Dispose();
				DB.DBCon.Close();
				return "N";
			}
		} else {
			RS.Dispose();
			DB.DBCon.Close();
			return "N";
		}

		DB.SqlCmd.CommandText = "SELECT * FROM RequestFormItems WHERE RequestFormPk=" + RequestFormPk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			Result.Append("####");
			for (int i = 0; i < RS.FieldCount; i++) {
				Result.Append(RS[i] + "!");
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return Result + "";
	}

	[WebMethod]
	public String SaveForOfferWrite(string Gubun, string companyPk, string RequestFormPk, string MonetaryUnit, string StepCL, string accountID, string SailingOn, string PortOfLanding, string FilnalDestination, string paymentWho, string ItemSum, string NotifyParty, string NotifyPartyAddress, string DateNNoofInvoice, string BuyerNOtherReferences, string ShipperName, string ShipperAddress, string ConsigneeName, string ConsigneeAddress) {
		GetQuery GQ = new GetQuery();
		StringBuilder Query = new StringBuilder();
		DB = new DBConn();
		if (RequestFormPk == "0") {
			DB.SqlCmd.CommandText = " declare @return int " +
					   " EXECUTE @return=SP_InsertRequestForm  @ShipperPk=" + companyPk +
																		", @AccountID=" + Common.StringToDB(accountID, true, true) +
																		", @DepartureDate=" + Common.StringToDB(SailingOn, true, false) +
																		", @DepartureRegionCode=" + Common.StringToDB(PortOfLanding, true, false) +
																		", @ArrivalRegionCode=" + Common.StringToDB(FilnalDestination, true, false) +
																		", @ShipperStaffName=" + Common.StringToDB(DateNNoofInvoice, true, true) +
																		", @ShipperStaffTEL=" + Common.StringToDB(ShipperName, true, true) +
																		", @ShipperStaffMobile=" + Common.StringToDB(ConsigneeName, true, true) +
																		", @PaymentWayCL=" + Common.StringToDB(paymentWho, false, false) +
																		", @MonetaryUnitCL=" + Common.StringToDB(MonetaryUnit, false, false) +
																		", @StepCL=" + Common.StringToDB(StepCL, false, false) +
																		", @NotifyPartyName=" + Common.StringToDB(NotifyParty + "#@!" + NotifyPartyAddress, true, true) +
																		", @NotifyPartyAddress=" + Common.StringToDB(BuyerNOtherReferences, true, true) +
																		", @Memo=" + Common.StringToDB(ShipperAddress + "#@!" + ConsigneeAddress, true, true) +
						" select @return ;";
			DB.DBCon.Open();
			RequestFormPk = DB.SqlCmd.ExecuteScalar() + "";
			DB.DBCon.Close();
		} else {
			Query.Append("UPDATE RequestForm SET [DepartureDate] = " + Common.StringToDB(SailingOn, true, false) +
											", [DepartureRegionCode] = " + Common.StringToDB(PortOfLanding, true, false) +
											", [ArrivalRegionCode] = " + Common.StringToDB(FilnalDestination, true, false) +
											", [ShipperStaffName] = " + Common.StringToDB(DateNNoofInvoice, true, true) +
											", [ShipperStaffTEL]=" + Common.StringToDB(ShipperName, true, true) +
											", [ShipperStaffMobile]=" + Common.StringToDB(ConsigneeName, true, true) +
											", [PaymentWayCL] = " + Common.StringToDB(paymentWho, false, false) +
											(Gubun == "C" ? ", [MonetaryUnitCL] = " + Common.StringToDB(MonetaryUnit, false, false) : string.Empty) +
											", [StepCL] = " + Common.StringToDB(StepCL, false, false) +
											", [NotifyPartyName] = " + Common.StringToDB(NotifyParty + "#@!" + NotifyPartyAddress, true, true) +
											", [NotifyPartyAddress] = " + Common.StringToDB(BuyerNOtherReferences, true, true) +
											", [Memo] = " + Common.StringToDB(ShipperAddress + "#@!" + ConsigneeAddress, true, true) +
									" WHERE RequestFormPk=" + RequestFormPk + ";");
		}

		if (ItemSum != "") {
			string[] Row = ItemSum.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
			string[] Each;
			foreach (string R in Row) {
				Each = R.Split(Common.Splite11, StringSplitOptions.None);
				if (Each[0] == "N") {
					if (Gubun == "C") {
						Query.Append(GQ.UpdateOrInsertRequestFormItem("Insert", RequestFormPk, Each[1], Each[2], Each[3], Each[4], Each[5], Each[6], Each[7], Each[8]));
					} else if (Gubun == "P") {
						Query.Append(GQ.UpdateOrInsertRequestFormItem("Insert", RequestFormPk, Each[1], Each[2], Each[3], Each[4], Each[5], Each[6], Each[7], Each[8], Each[9], Each[10], Each[11]));
					}
				} else {
					if (Gubun == "C") {
						Query.Append(GQ.UpdateOrInsertRequestFormItem("Update", Each[0], Each[1], Each[2], Each[3], Each[4], Each[5], Each[6], Each[7], Each[8]));
					} else if (Gubun == "P") {
						Query.Append(GQ.UpdateOrInsertRequestFormItem("Update", Each[0], Each[1], Each[2], Each[3], Each[4], Each[5], Each[6], Each[7], Each[8], Each[9], Each[10], Each[11]));
					}
				}
			}
		}

		DB.SqlCmd.CommandText = Query + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return RequestFormPk; //DB.SqlCmd.CommandText;// RequestFormPk;
	}
	[WebMethod]
	public String SaveApplicantUsingCCL(string CompanyInDocumentPk, string CCLPk, string Name, string Address) {
		DBConn DB = new DBConn();
		Common C = new Common();
		if (CompanyInDocumentPk == "") {
			DB.SqlCmd.CommandText = "INSERT INTO CompanyInDocument ([GubunCL], [GubunPk], [Name], [Address]) VALUES (2, " + CCLPk + ", " + C.CheckNull(Name, true, false) + ", " + C.CheckNull(Address, true, false) + " ); select @@identity;";
		} else {
			DB.SqlCmd.CommandText = "UPDATE CompanyInDocument SET [Name] = " + C.CheckNull(Name, true, false) + ", [Address] = " + C.CheckNull(Address, true, false) + " WHERE CompanyInDocumentPk=" + CompanyInDocumentPk + ";";

		}
		DB.DBCon.Open();
		string Identity = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		if (CompanyInDocumentPk == "") {
			return Identity;
		} else {
			return CompanyInDocumentPk;
		}
	}
	[WebMethod]
	public String SaveOfferSheetBankInfo(string Gubun, string Pk, string BankName, string BankAddress, string SwiftCode, string AccountNo) {
		DBConn DB = new DBConn();
		Common C = new Common();
		if (Gubun == "I") {
			DB.SqlCmd.CommandText = "INSERT INTO CompanyInDocumentBank ([CompanyInDocumentPk], [Name], [Address], [SwiftCode], [AccountNo]) VALUES (" +
							Pk + ", " + C.CheckNull(BankName, true, false) + ",  " + C.CheckNull(BankAddress, true, false) + "," + C.CheckNull(SwiftCode, true, false) + " , " + C.CheckNull(AccountNo, true, false) + ");" +
							"select @@identity;";
		} else {
			DB.SqlCmd.CommandText = "UPDATE CompanyInDocumentBank	" +
														"SET [Name] = " + C.CheckNull(BankName, true, false) +
																", [Address] = " + C.CheckNull(BankAddress, true, false) +
																", [SwiftCode] = " + C.CheckNull(SwiftCode, true, false) +
																", [AccountNo] = " + C.CheckNull(AccountNo, true, false) +
														"WHERE [CompanyInDocumentBankPk]=" + Pk + ";";
		}
		DB.DBCon.Open();
		string Identity = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		if (Gubun == "I") {
			return Identity;
		} else {
			return Pk;
		}
	}
	[WebMethod]
	public String DeleteRequestForm(string RequestFormPk) {
		DB = new DBConn();
		GetQuery GQ = new GetQuery();
		DB.SqlCmd.CommandText = "	DELETE FROM RequestForm WHERE RequestFormPk=" + RequestFormPk + ";" +
													"	DELETE FROM RequestFormItems WHERE RequestFormPk=" + RequestFormPk + ";" +
													"	DELETE FROM [dbo].[COMMENT] WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK] = " + RequestFormPk + ";" +
													"	DELETE FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK] = " + RequestFormPk + ";" ;
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
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
	[WebMethod]
	public String ModifyStaffID(string GubunCL, string RequestFormPk, string StaffID) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE [dbo].[HISTORY] SET [DESCRIPTION]=" + Common.StringToDB(StaffID, true, true) + " WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK] = " + RequestFormPk + " and [CODE] = '" + GubunCL + "' ;";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String LoadTransportWayCL(string DepartureRegionPk, string ArrivalRegionPk) {
		DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = "	SELECT TCL.TransportWayCL, D.Description, D.OrderBy " +
													"	FROM TransportWayCLSetting AS TCL left join " +
													"				TransportWayCLDescription AS D on TCL.TransportWayCL=D.TransportWayCL " +
													"	WHERE TCL.DepartureRegionCodePk=" + DepartureRegionPk + " and TCL.ArrivalRegionCodePk=" + ArrivalRegionPk +
													"	ORDER BY D.OrderBy;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder SB = new StringBuilder();
		while (RS.Read()) {
			SB.Append(RS[0] + "!" + RS[1] + "!" + RS[2] + "@@");
		}
		RS.Dispose();
		DB.DBCon.Close();
		return SB + "";
	}
	[WebMethod]
	public String LoadTransportWayCL2(string DepartureGubun, string DepartureRegionCode, string ArrivaGubun, string ArrivalRegionCode) {
		DB = new DBConn();
		DB.DBCon.Open();
		string DepartureRegionPk = "";
		if (DepartureGubun != "Pk") {
			DB.SqlCmd.CommandText = "SELECT [RegionCodePk] FROM RegionCode WHERE [RegionCode]='" + DepartureRegionCode + "';";
			DepartureRegionPk = DB.SqlCmd.ExecuteScalar() + "";
		} else {
			DepartureRegionPk = DepartureRegionCode;
		}
		string ArrivalRegionPk = "";
		if (ArrivaGubun != "Pk") {
			DB.SqlCmd.CommandText = "SELECT [RegionCodePk] FROM RegionCode WHERE [RegionCode]='" + ArrivalRegionCode + "';";
			ArrivalRegionPk = DB.SqlCmd.ExecuteScalar() + "";
		} else {
			ArrivalRegionPk = ArrivalRegionCode;
		}
		DB.SqlCmd.CommandText = "	SELECT TCL.TransportWayCL, D.Description, D.OrderBy " +
													"	FROM TransportWayCLSetting AS TCL left join " +
													"				TransportWayCLDescription AS D on TCL.TransportWayCL=D.TransportWayCL " +
													"	WHERE TCL.DepartureRegionCodePk=" + DepartureRegionPk + " and TCL.ArrivalRegionCodePk=" + ArrivalRegionPk +
													"	ORDER BY D.OrderBy;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder SB = new StringBuilder();
		while (RS.Read()) {
			SB.Append(RS[0] + "!" + RS[1] + "!" + RS[2] + "@@");
		}
		RS.Dispose();
		DB.DBCon.Close();
		return SB + "";
	}
	[WebMethod]
	public String[] LoadCalculateHeadForItemModify(string RequestFormPk) {
		String[] Result;
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT TotalPackedCount, PackingUnit, TotalGrossWeight, TotalVolume FROM [dbo].[RequestForm] WHERE RequestFormPk=" + RequestFormPk;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			Result = new string[4] { RS[0] + "", RS[1] + "", RS[2] + "", RS[3] + "" };
		} else {
			Result = new string[1] { "N" };
		}
		return Result;
	}
	[WebMethod]
	public String ModifyCalculateHeadASItemModify(string OurBranchPk, string RequestFormPk, string PackedCount, string PackingUnit, string Weight, string Volume, string AccountID) {
		DB = new DBConn();
		GetQuery GQ = new GetQuery();
		DB.SqlCmd.CommandText =
			GQ.AddRequestHistory(RequestFormPk, "61", AccountID, "") +
			"	UPDATE [dbo].[RequestForm] SET TotalPackedCount = " + Common.StringToDB(PackedCount, false, false) + ", PackingUnit=" + Common.StringToDB(PackingUnit, false, false) + ", TotalGrossWeight = " + Common.StringToDB(Weight, false, false) + ", TotalVolume = " + Common.StringToDB(Volume, false, false) + " WHERE [RequestFormPk] = " + RequestFormPk + ";";

		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String[] LoadCompanyInfForCompanyCode(string CompanyCode) {
		DB = new DBConn();
		string companypk;
		List<string> ReturnValue = new List<string>();
		DB.SqlCmd.CommandText = @"
SELECT 
	C.CompanyPk, C.CompanyName, C.RegionCode, C.CompanyAddress, C.CompanyTEL, C.CompanyFAX, C.PresidentName
	, C.PresidentEmail, C.GubunCL, RC.RegionCodePk, CAI.Value 
FROM 
	Company AS C
	left join RegionCode AS RC ON C.RegionCode=RC.RegionCode
	left join (
		SELECT [CompanyPk], [Value] FROM CompanyAdditionalInfomation WHERE Title=62
	) AS CAI ON C.CompanyPk=CAI.CompanyPk 
WHERE C.CompanyCode='" + CompanyCode + "';";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			companypk = RS["CompanyPk"] + "";
			ReturnValue.Add(RS["CompanyPk"] + "#@!" +
				RS["CompanyName"] + "#@!" +
				RS["CompanyAddress"] + "#@!" +
				RS["CompanyTEL"] + "#@!" +
				RS["CompanyFAX"] + "#@!" +
				RS["PresidentName"] + "#@!" +
				RS["PresidentEmail"] + "#@!" +
				RS["GubunCL"] + "#@!" +
				RS["RegionCode"] + "#@!" +
				RS["RegionCodePk"] + "#@!" +
				RS["Value"]);
		} else {
			DB.DBCon.Close();
			return new string[] { "N" };
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"
SELECT TOP 3 
	[Duties], [Name], [TEL], [Mobile], [Email] 
FROM 
	[Account_] 
WHERE 
	CompanyPk=" + companypk + ";";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Add(RS["Duties"] + "#@!" +
				RS["Name"] + "#@!" +
				RS["TEL"] + "#@!" +
				RS["Mobile"] + "#@!" +
				RS["Email"]);
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}
	[WebMethod]
	public String UpdateDefaultConnection(string CompanyInDocumentPk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "	UPDATE CompanyInDocument SET [DefaultConnection] = NULL WHERE GubunPk=(SELECT [GubunPk] FROM CompanyInDocument WHERE [CompanyInDocumentPk]=" + CompanyInDocumentPk + ");" +
													"	UPDATE CompanyInDocument SET [DefaultConnection] = 1 WHERE [CompanyInDocumentPk]=" + CompanyInDocumentPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	//BMK RequestComfirm
	[WebMethod]
	public String[] LoadStorcedData(string RequestFormPk) {
		string[] ReturnValue = new string[3] { "", "", "" };
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [StockedDate] FROM RequestForm WHERE RequestFormPk=" + RequestFormPk + ";";
		DB.DBCon.Open();
		ReturnValue[0] = DB.SqlCmd.ExecuteScalar() + "";


		DB.SqlCmd.CommandText = "SELECT [CODE], [DESCRIPTION]  FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK]=" + RequestFormPk + " AND [CODE] in ('10', '11') Order by [CODE] ASC;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (RS[0] + "" == "10") {
				ReturnValue[1] = RS[1] + "";
			} else {
				ReturnValue[2] = RS[1] + "";
			}
		}
		return ReturnValue;
	}
	[WebMethod]
	public String LoadBranchStorage(string CompanyPk) {
		StringBuilder ReturnValue = new StringBuilder();
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [OurBranchStoragePk], [StorageName] FROM OurBranchStorageCode WHERE OurBranchCode=" + CompanyPk + " and IsUse is null order by [StorageName] ;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		int RSCount = 0;
		while (RS.Read()) {
			ReturnValue.Append("<option value=\"" + RS[0] + "\">" + RS[1] + "</option>");
			RSCount++;
		}
		RS.Dispose();
		DB.DBCon.Close();
		if (RSCount == 1) {
			return "<select id=\"STStorage\" style=\"visibility:hidden;\">" + ReturnValue + "</select>";
		} else {
			return "<select id=\"STStorage\"><option value=\"0\">WAREHOUSE</option>" + ReturnValue + "</select>";
		}
	}
	//20140106 통관명 작업
	//	[WebMethod]
	//	public String[] LoadAdminDefault(string S1)
	//	{
	//		if (S1 == "3157") {
	//			List<string> ReturnValue = new List<string>();
	//			//Gubun = Gubun == "S" ? "6" : "7";
	//			DB = new DBConn();
	//			DB.SqlCmd.CommandText = @"	SELECT CID.CompanyInDocumentPk, CID.Name, CID.Address, F.FilePk, F.FileName, F.[PhysicalPath] 
	//															FROM CompanyInDocument AS CID 
	//																left join (SELECT FilePk, GubunPk, [PhysicalPath], FileName FROM [File] WHERE isnull(GubunCL, 3)=3 ) AS F On CID.CompanyInDocumentPk=F.GubunPk 
	//															WHERE CID.GubunCL = 6;";
	//			DB.DBCon.Open();
	//			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
	//			while (RS.Read()) {
	//				ReturnValue.Add(RS[0] + "##" + RS[1] + "##" + RS[2] + "##" + RS[3] + "##" + RS[4] + "##" + RS[5]);
	//			}
	//			RS.Dispose();
	//			DB.DBCon.Close();
	//			if (ReturnValue.Count == 0) {
	//				return new string[] { "0" };
	//			} else {
	//				return ReturnValue.ToArray();
	//			}
	//		} else {
	//			List<string> ReturnValue = new List<string>();
	//			DB = new DBConn();
	//			DB.SqlCmd.CommandText = @"	SELECT CID.CompanyInDocumentPk, CID.Name, CID.Address, F.FilePk, F.FileName, F.[PhysicalPath] 
	//															FROM CompanyInDocument AS CID 
	//																left join (SELECT FilePk, GubunPk, [PhysicalPath], FileName FROM [File] WHERE isnull(GubunCL, 3)=3 ) AS F On CID.CompanyInDocumentPk=F.GubunPk 
	//															WHERE CID.GubunCL = 6;";
	//			DB.DBCon.Open();
	//			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
	//			while (RS.Read()) {

	//				ReturnValue.Add(RS[0] + "##" + RS[1] + "##" + RS[2] + "##" + RS[3] + "##" + RS[4] + "##" + RS[5]);
	//			}
	//			RS.Dispose();
	//			DB.DBCon.Close();
	//			if (ReturnValue.Count == 0) {
	//				return new string[] { "0" };
	//			} else {
	//				return ReturnValue.ToArray();
	//			}

	//		}
	//	}
	[WebMethod]
	public String[] LoadAdminDefault(string CompanyPk) {
		List<string> ReturnValue = new List<string>();
		//Gubun = Gubun == "S" ? "6" : "7";
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"	  
SELECT CID.CompanyInDocumentPk, CID.Name, CID.Address, F.FilePk, F.FileName, F.[PhysicalPath], ISNULL(IsPrimary.Value, 0) AS IsPrimary ,CID.CompanyNo,CID.CustomsCode,CID.Title,CID.ZipCode1,CID.ZipCode2,CID.Name_KOR,CID.Address_KOR
FROM CompanyInDocument AS CID 
	left join (SELECT FilePk, GubunPk, [PhysicalPath], FileName FROM [File] WHERE isnull(GubunCL, 3)=3 ) AS F On CID.CompanyInDocumentPk=F.GubunPk 
	left join (
		SELECT [Value] FROM [dbo].[CompanyAdditionalInfomation] WHERE Title=99 and CompanyPk=" + CompanyPk + @"
	) AS IsPrimary ON CID.CompanyInDocumentPk=IsPrimary.Value
  WHERE CID.GubunCL = 6 AND CID.CompanyInDocumentPk <> 4670
  ORDER BY IsPrimary DESC;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Add(RS[0] + "##" + RS[1] + "##" + RS[2] + "##" + RS[3] + "##" + RS[4] + "##" + RS[5] + "##" + RS[6] + "##" + RS[7] + "##" + RS[8] + "##" + RS[9] + "##" + RS[10] + "##" + RS[11] + "##" + RS[12] + "##" + RS[13]);
		}
		RS.Dispose();
		DB.DBCon.Close();
		if (ReturnValue.Count == 0) {
			return new string[] { "0" };
		} else {
			return ReturnValue.ToArray();
		}
	}
	[WebMethod]
	public String[] LoadAdminDefault_Transport() {
		List<string> ReturnValue = new List<string>();
		//Gubun = Gubun == "S" ? "6" : "7";
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"	  
SELECT [CompanyInDocumentPk], [Name], [Address],[Address_KOR]
FROM [CompanyInDocument]
  WHERE GubunCL = 8;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Add(RS[0] + "##" + RS[1] + "##" + RS[2] + "##" + RS[3]);
		}
		RS.Dispose();
		DB.DBCon.Close();
		if (ReturnValue.Count == 0) {
			return new string[] { "0" };
		} else {
			return ReturnValue.ToArray();
		}

	}

	//BMK 수신확인
	[WebMethod]
	public String DeleteRequestFormItemsPk(string RequestFormPk, string RequestFormItemsPk, string Count, string AccountID) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "	INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID], [DESCRIPTION]) " +
													"	VALUES ('RequestForm', " + Common.StringToDB(RequestFormPk, false, false) + ", '71', " + Common.StringToDB(AccountID, true, false) + ", '*" + Count + " || DELETE'); " +
													"	DELETE FROM RequestFormItems WHERE [RequestFormItemsPk]=" + RequestFormItemsPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String DeleteRequestFormItems_All(string RequestFormPk, string AccountID) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "	INSERT INTO [dbo].[HISTORY] ([TABLE_NAME], [TABLE_PK], [CODE], [ACCOUNT_ID], [DESCRIPTION]) " +
													"	VALUES ('RequestForm', " + Common.StringToDB(RequestFormPk, false, false) + ", '71', " + Common.StringToDB(AccountID, true, false) + ", '*ALL || DELETE'); " +
													"	DELETE FROM RequestFormItems WHERE [RequestFormPk]=" + RequestFormPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String QuantityXUnitCost(string X, string Y) {
		decimal temp = decimal.Parse(X) * decimal.Parse(Y);
		return Common.NumberFormat(temp + "");
	}
	[WebMethod]
	public String[] ItemCALC(string Quantity, string Price, string PackingCount, string Weight, string Volume) {
		StringBuilder resultPrice = new StringBuilder();
		decimal totalPrice = 0;
		string[] rowPrice = Price.Split(Common.Splite321, StringSplitOptions.None);
		for (int i = 0; i < rowPrice.Length - 1; i++) {
			if (rowPrice[i] == "*") {
				resultPrice.Append("!");
				continue;
			}
			decimal temp;
			if (rowPrice[i].Substring(0, rowPrice[i].IndexOf("*")) == "" || rowPrice[i].Substring(rowPrice[i].IndexOf("*") + 1) == "") {
				temp = 0;
			} else {
				temp = decimal.Parse(rowPrice[i].Substring(0, rowPrice[i].IndexOf("*"))) * decimal.Parse(rowPrice[i].Substring(rowPrice[i].IndexOf("*") + 1));
			}
			resultPrice.Append(temp + "!");
			totalPrice += temp;
		}
		resultPrice.Append(totalPrice + "");
		return new String[5] { calc(Quantity), resultPrice + "", calc(PackingCount), calc(Weight), calc(Volume) };
	}
	private string calc(string value) {
		string[] each = value.Split(Common.Splite321, StringSplitOptions.RemoveEmptyEntries);
		float Total = 0;
		foreach (string eachvalue in each) {
			Total += float.Parse(eachvalue);
		}
		return Total + "";
	}
	[WebMethod]
	public String CustomerConfirm(string Gubun, string RequestFormPk, string AccountID) {
		DB = new DBConn();
		if (Gubun == "S") {
			DB.SqlCmd.CommandText = "	UPDATE RequestForm SET [ShipperSignID] = '" + AccountID + "', [ShipperSignDate] = getDate() WHERE RequestFormPk=" + RequestFormPk + ";" +
															new GetQuery().AddRequestHistory(RequestFormPk, "40", AccountID, "");
		} else {
			DB.SqlCmd.CommandText = "	UPDATE RequestForm SET [ConsigneeSignID] = '" + AccountID + "', [ConsigneeSignDate] = getDate() WHERE RequestFormPk=" + RequestFormPk + ";" +
														"	INSERT INTO [RequestModifyHistory] ([RequestFormPk], [Value], [AccountID], [Registerd]) VALUES (" +
																RequestFormPk + ", " + Common.StringToDB("41", true, true) + ", " + Common.StringToDB(AccountID, true, false) + ", getDate());" +
															new GetQuery().AddRequestHistory(RequestFormPk, "41", AccountID, "");
		}
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String FileGubunCLChange(string Table, string FilePk, string GubunCL) {
		try {
			DB = new DBConn();
			if (Table == "ClearancedFile") {
				DB.SqlCmd.CommandText = "update ClearancedFile SET GubunCL=" + GubunCL + " WHERE ClearancedFilePk=" + FilePk + ";";
			} else {
				DB.SqlCmd.CommandText = "UPDATE [INTL2010].[dbo].[File] SET GubunCL=" + GubunCL + " WHERE [FilePk]=" + FilePk + ";";
			}
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
	public String RequestModifyConfirm(string Pk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE [RequestModifyHistory] SET [Confirmed] = getDate() WHERE Pk=" + Pk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String FileDeleteWithGubunPk(string FilePk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "DELETE FROM [File] WHERE FilePk=" + FilePk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	//화주문서페이지작업
	[WebMethod]
	public String[] LoadInvoiceDocument(string DocumentFormPk, string Gubun) {
		List<string> ReturnValue = new List<string>();
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
                                    SELECT InvoiceNo,Shipper,ShipperAddress,Consignee, ConsigneeAddress,NotifyParty,NotifyPartyAddress,PortOfLoading,FinalDestination,Carrier
											   ,SailingOn,PaymentTerms,OtherReferences,Buyer,MonetaryUnitCL
									FROM   newCommercialDocument
									WHERE CommercialDocumentHeadPk=" + DocumentFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue.Add(RS["InvoiceNo"] + "#@!" + RS["Shipper"] + "#@!" + RS["ShipperAddress"] + "#@!" + RS["Consignee"] + "#@!" + RS["ConsigneeAddress"] + "#@!" +
				RS["NotifyParty"] + "#@!" + RS["NotifyPartyAddress"] + "#@!" + RS["PortOfLoading"] + "#@!" + RS["FinalDestination"] + "#@!" + RS["Carrier"] +
				"#@!" + RS["SailingOn"] + "#@!" + RS["PaymentTerms"] + "#@!" + RS["OtherReferences"] + "#@!" + RS["Buyer"] + "#@!" + RS["MonetaryUnitCL"]);
		} else {
			RS.Dispose();
			//DB.DBCon.Close();
			//return new string[] { "N" };
		}
		RS.Dispose();
		if (Gubun == "C") {
			DB.SqlCmd.CommandText = @"
                                        SELECT [DocumentFormItemsPk], [Description], [Label], [Material], [Quantity], [QuantityUnit], [UnitPrice], [Amount] 
                                        FROM [DocumentFormItems] 
                                        WHERE DocumentFormPk=" + DocumentFormPk + ";";
			SqlDataReader RS1 = DB.SqlCmd.ExecuteReader();
			while (RS1.Read()) {
				ReturnValue.Add(RS1["DocumentFormItemsPk"] + "#@!" + RS1["Description"] + "#@!" + RS1["Label"] + "#@!" + RS1["Material"] + "#@!" + RS1["Quantity"] + "#@!" + RS1["QuantityUnit"] + "#@!" + RS1["UnitPrice"] + "#@!" + RS1["Amount"]);
			}
			RS1.Dispose();
		} else {
			DB.SqlCmd.CommandText = @"
                                        SELECT [DocumentFormItemsPk], [MarkNNumber], [Description], [Label], [Material], [Quantity], [QuantityUnit], [PackedCount], [PackingUnit], [GrossWeight], [Volume] 
                                        FROM [DocumentFormItems] 
                                        WHERE DocumentFormPk=" + DocumentFormPk + ";";
			SqlDataReader RS2 = DB.SqlCmd.ExecuteReader();
			while (RS2.Read()) {
				ReturnValue.Add(RS2["DocumentFormItemsPk"] + "#@!" + RS2["MarkNNumber"] + "#@!" + RS2["Description"] + "#@!" + RS2["Label"] + "#@!" + RS2["Material"] + "#@!" + RS2["Quantity"] + "#@!" +
					RS2["QuantityUnit"] + "#@!" +
					RS2["PackedCount"] + "#@!" + RS2["PackingUnit"] + "#@!" + RS2["GrossWeight"] + "#@!" + RS2["Volume"]);
			}
			RS2.Dispose();
		}
		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}

	//화주문서페이지작업
	[WebMethod]
	public String[] LoadPackingDocument(string DocumentFormPk, string Gubun) {
		List<string> ReturnValue = new List<string>();
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
                                    SELECT InvoiceNo,Shipper,ShipperAddress,Consignee, ConsigneeAddress,NotifyParty,NotifyPartyAddress,PortOfLoading,FinalDestination,Carrier
											   ,SailingOn,PaymentTerms,OtherReferences,Buyer,MonetaryUnitCL
									FROM   newCommercialDocument
									WHERE CommercialDocumentHeadPk=" + DocumentFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue.Add(RS["InvoiceNo"] + "#@!" + RS["Shipper"] + "#@!" + RS["ShipperAddress"] + "#@!" + RS["Consignee"] + "#@!" + RS["ConsigneeAddress"] + "#@!" +
				RS["NotifyParty"] + "#@!" + RS["NotifyPartyAddress"] + "#@!" + RS["PortOfLoading"] + "#@!" + RS["FinalDestination"] + "#@!" + RS["Carrier"] +
				"#@!" + RS["SailingOn"] + "#@!" + RS["PaymentTerms"] + "#@!" + RS["OtherReferences"] + "#@!" + RS["Buyer"] + "#@!" + RS["MonetaryUnitCL"]);
		} else {
			RS.Dispose();
			//DB.DBCon.Close();
			//return new string[] { "N" };
		}
		RS.Dispose();
		if (Gubun == "C") {
			DB.SqlCmd.CommandText = @"
                                        SELECT [DocumentFormItemsPk], [Description], [Label], [Material], [Quantity], [QuantityUnit], [UnitPrice], [Amount] 
                                        FROM [DocumentFormItems] 
                                        WHERE DocumentFormPk=" + DocumentFormPk + ";";
			SqlDataReader RS1 = DB.SqlCmd.ExecuteReader();
			while (RS1.Read()) {
				ReturnValue.Add(RS1["DocumentFormItemsPk"] + "#@!" + RS1["Description"] + "#@!" + RS1["Label"] + "#@!" + RS1["Material"] + "#@!" + RS1["Quantity"] + "#@!" + RS1["QuantityUnit"] + "#@!" + RS1["UnitPrice"] + "#@!" + RS1["Amount"]);
			}
			RS1.Dispose();
		} else {
			DB.SqlCmd.CommandText = @"
                                        SELECT [DocumentFormItemsPk], [MarkNNumber], [Description], [Label], [Material], [Quantity], [QuantityUnit], [PackedCount], [PackingUnit], [GrossWeight], [Volume] 
                                        FROM [DocumentFormItems] 
                                        WHERE DocumentFormPk=" + DocumentFormPk + ";";
			SqlDataReader RS2 = DB.SqlCmd.ExecuteReader();
			while (RS2.Read()) {
				ReturnValue.Add(RS2["DocumentFormItemsPk"] + "#@!" + RS2["MarkNNumber"] + "#@!" + RS2["Description"] + "#@!" + RS2["Label"] + "#@!" + RS2["Material"] + "#@!" + RS2["Quantity"] + "#@!" +
					RS2["QuantityUnit"] + "#@!" +
					RS2["PackedCount"] + "#@!" + RS2["PackingUnit"] + "#@!" + RS2["GrossWeight"] + "#@!" + RS2["Volume"]);
			}
			RS2.Dispose();
		}
		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}
	//화주문서페이지작업
	[WebMethod]
	public String[] LoadTradingScheduleDocument(string DocumentFormPk, string Gubun) {
		List<string> ReturnValue = new List<string>();
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
                                    SELECT Date, Name , Companyno ,Companyname ,Presidentname ,Companyaddress ,Upjong ,Uptae 
									           ,Totalquantity ,Totalprice ,Totalamount ,Totaltax ,TTotalamount ,Misuamout
									FROM   newCommercialDocument
									WHERE CommercialDocumentHeadPk=" + DocumentFormPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			ReturnValue.Add(RS["Date"] + "#@!" + RS["Name"] + "#@!" + RS["Companyno"] + "#@!" + RS["Companyname"] + "#@!" + RS["Presidentname"] + "#@!" +
				RS["Companyaddress"] + "#@!" + RS["Upjong"] + "#@!" + RS["Uptae"] + "#@!" + RS["Totalquantity"] + "#@!" + RS["Totalprice"] +
				"#@!" + RS["Totalamount"] + "#@!" + RS["Totaltax"] + "#@!" + RS["TTotalamount"] + "#@!" + RS["Misuamout"]);
		} else {
			RS.Dispose();
			//DB.DBCon.Close();
			//return new string[] { "N" };
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"
									SELECT [TradingScheduleItemsPk] ,[Date], [Description], [Volume], [Quantity], [Price], [Amount], [Tax] 
									FROM [Tradingscheduleitems] 
									WHERE Tradingscheduleheadpk=" + DocumentFormPk + ";";
		SqlDataReader RS1 = DB.SqlCmd.ExecuteReader();
		while (RS1.Read()) {
			ReturnValue.Add(RS1["TradingScheduleItemsPk"] + "#@!" + RS1["Date"] + "#@!" + RS1["Description"] + "#@!" + RS1["Volume"] + "#@!" + RS1["Quantity"] + "#@!" + RS1["Price"] + "#@!" + RS1["Amount"] + "#@!" + RS1["Tax"]);
		}
		RS1.Dispose();

		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}
	//화주문서페이지작업
	[WebMethod]
	public String SaveForOfferWriteDocument(string Gubun, string companyPk, string CommercialDocumentHeadPk, string MonetaryUnit, string StepCL, string accountID, string SailingOn, string PortOfLanding, string FinalDestination, string paymentWho, string ItemSum, string NotifyParty, string NotifyPartyAddress, string DateNNoofInvoice, string Buyer, string OtherReferences, string Carrier, string ShipperName, string ShipperAddress, string ConsigneeName, string ConsigneeAddress, string GubunCL) {
		GetQuery GQ = new GetQuery();
		StringBuilder Query = new StringBuilder();
		DB = new DBConn();
		if (CommercialDocumentHeadPk == "0") {
			DB.SqlCmd.CommandText = " declare @return int " +
					   " EXECUTE @return=SP_InsertnewCommercialDocument  @CompanyPk=" + Common.StringToDB(companyPk, true, true) +
																										",@AccountID=" + Common.StringToDB(accountID, true, true) +
																										",@Shipper=" + Common.StringToDB(ShipperName, true, true) +
																										",@ShipperAddress=" + Common.StringToDB(ShipperAddress, true, true) +
																										",@Consignee=" + Common.StringToDB(ConsigneeName, true, true) +
																										",@ConsigneeAddress=" + Common.StringToDB(ConsigneeAddress, true, true) +
																										",@SailingOn=" + Common.StringToDB(SailingOn, true, false) +
																										",@PortOfLoading=" + Common.StringToDB(PortOfLanding, true, false) +
																										",@FinalDestination=" + Common.StringToDB(FinalDestination, true, false) +
																										", @StepCL=" + Common.StringToDB(StepCL, false, false) +
																										", @NotifyParty=" + Common.StringToDB(NotifyParty, true, true) +
																										", @NotifyPartyAddress=" + Common.StringToDB(NotifyPartyAddress, true, true) +
																										", @InvoiceNo=" + Common.StringToDB(DateNNoofInvoice, true, true) +
																										", @PaymentTerms=" + Common.StringToDB(paymentWho, true, true) +
																										", @Carrier=" + Common.StringToDB(Carrier, true, true) +
																										", @OtherReferences=" + Common.StringToDB(OtherReferences, true, true) +
																										", @Buyer=" + Common.StringToDB(Buyer, true, true) +
																										", @GubunCL=" + Common.StringToDB(GubunCL, true, true) +
																										", @MonetaryUnitCL=" + Common.StringToDB(MonetaryUnit, false, false) +
							" select @return ;";
			DB.DBCon.Open();
			CommercialDocumentHeadPk = DB.SqlCmd.ExecuteScalar() + "";
			DB.DBCon.Close();
		} else {
			Query.Append("UPDATE newCommercialDocument SET [Shipper]=" + Common.StringToDB(ShipperName, true, true) +
																		",[ShipperAddress]=" + Common.StringToDB(ShipperAddress, true, true) +
																		",[Consignee]=" + Common.StringToDB(ConsigneeName, true, true) +
																		",[ConsigneeAddress]=" + Common.StringToDB(ConsigneeAddress, true, true) +
																		",[SailingOn]=" + Common.StringToDB(SailingOn, true, false) +
																		",[PortOfLoading]=" + Common.StringToDB(PortOfLanding, true, false) +
																		",[FinalDestination]=" + Common.StringToDB(FinalDestination, true, false) +
																		",[StepCL]=" + Common.StringToDB(StepCL, false, false) +
																		",[NotifyParty]=" + Common.StringToDB(NotifyParty, true, true) +
																		",[NotifyPartyAddress]=" + Common.StringToDB(NotifyPartyAddress, true, true) +
																		",[InvoiceNo]=" + Common.StringToDB(DateNNoofInvoice, true, true) +
																		",[PaymentTerms]=" + Common.StringToDB(paymentWho, true, true) +
																		",[Carrier]=" + Common.StringToDB(Carrier, true, true) +
																		",[OtherReferences]=" + Common.StringToDB(OtherReferences, true, true) +
																		",[Buyer]=" + Common.StringToDB(Buyer, true, true) +
																		",[GubunCL]=" + Common.StringToDB(GubunCL, true, true) +
																		",[MonetaryUnitCL]=" + Common.StringToDB(MonetaryUnit, false, false) +
									" WHERE CommercialDocumentHeadPk=" + CommercialDocumentHeadPk + ";");
		}

		if (ItemSum != "") {
			string[] Row = ItemSum.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
			string[] Each;
			foreach (string R in Row) {
				Each = R.Split(Common.Splite11, StringSplitOptions.None);
				if (Each[0] == "N") {
					if (Gubun == "C") {
						Query.Append(GQ.UpdateOrInsertDocumentFormItem("Insert", CommercialDocumentHeadPk, Each[1], Each[2], Each[3], Each[4], Each[5], Each[6], Each[7], Each[8]));
					} else if (Gubun == "P") {
						Query.Append(GQ.UpdateOrInsertDocumentFormItem("Insert", CommercialDocumentHeadPk, Each[1], Each[2], Each[3], Each[4], Each[5], Each[6], Each[7], Each[8], Each[9], Each[10], Each[11]));
					}
				} else {
					if (Gubun == "C") {
						Query.Append(GQ.UpdateOrInsertDocumentFormItem("Update", Each[0], Each[1], Each[2], Each[3], Each[4], Each[5], Each[6], Each[7], Each[8]));
					} else if (Gubun == "P") {
						Query.Append(GQ.UpdateOrInsertDocumentFormItem("Update", Each[0], Each[1], Each[2], Each[3], Each[4], Each[5], Each[6], Each[7], Each[8], Each[9], Each[10], Each[11]));
					}
				}
			}
		}

		DB.SqlCmd.CommandText = Query + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return CommercialDocumentHeadPk; //DB.SqlCmd.CommandText;// RequestFormPk;
	}
	//화주문서페이지작업
	[WebMethod]
	public String SaveTradingScheduleDocument(string TradingScheduleWirteHeadPk, string CompanyPk, string AccountID, string Date, string BusinessNumber, string Name, string CompanyName, string PresidentName, string CompanyAddress, string Upjong, string Uptae, string TotalQuantity, string TotalPrice, string TotalAmount, string TotalTax, string MisuAmout, string TotalAmount2, string ItemSum) {
		GetQuery GQ = new GetQuery();
		StringBuilder Query = new StringBuilder();
		DB = new DBConn();
		if (TradingScheduleWirteHeadPk == "0") {
			DB.SqlCmd.CommandText = " declare @return int " +
					   " EXECUTE @return=SP_InsertnewTradingSchedule    @CompanyPk=" + Common.StringToDB(CompanyPk, true, true) +
																										",@AccountID=" + Common.StringToDB(AccountID, true, true) +
																										",@Date=" + Common.StringToDB(Date, true, true) +
																										",@Name=" + Common.StringToDB(Name, true, true) +
																										",@CompanyNo=" + Common.StringToDB(BusinessNumber, true, true) +
																										",@CompanyName=" + Common.StringToDB(CompanyName, true, false) +
																										",@PresidentName=" + Common.StringToDB(PresidentName, true, false) +
																										",@CompanyAddress=" + Common.StringToDB(CompanyAddress, true, false) +
																										",@upjong=" + Common.StringToDB(Upjong, false, false) +
																										",@uptae=" + Common.StringToDB(Uptae, true, true) +
																										",@TotalQuantity=" + Common.StringToDB(TotalQuantity, true, true) +
																										",@TotalPrice=" + Common.StringToDB(TotalPrice, true, true) +
																										",@TotalAmount=" + Common.StringToDB(TotalAmount, true, true) +
																										",@TotalTax=" + Common.StringToDB(TotalTax, true, true) +
																										",@TTotalAmount=" + Common.StringToDB(TotalAmount2, true, true) +
																										",@MisuAmout=" + Common.StringToDB(MisuAmout, true, true) +
							" select @return ;";
			DB.DBCon.Open();
			TradingScheduleWirteHeadPk = DB.SqlCmd.ExecuteScalar() + "";
			DB.DBCon.Close();
		} else {
			Query.Append("UPDATE newCommercialDocument SET @CompanyPk=" + Common.StringToDB(CompanyPk, true, true) +
																										",@AccountID=" + Common.StringToDB(AccountID, true, true) +
																										",@Date=" + Common.StringToDB(Date, true, true) +
																										",@Name=" + Common.StringToDB(Name, true, true) +
																										",@CompanyNo=" + Common.StringToDB(BusinessNumber, true, true) +
																										",@CompanyName=" + Common.StringToDB(CompanyName, true, false) +
																										",@PresidentName=" + Common.StringToDB(PresidentName, true, false) +
																										",@CompanyAddress=" + Common.StringToDB(CompanyAddress, true, false) +
																										",@upjong=" + Common.StringToDB(Upjong, false, false) +
																										",@uptae=" + Common.StringToDB(Uptae, true, true) +
																										",@TotalQuantity=" + Common.StringToDB(TotalQuantity, true, true) +
																										",@TotalPrice=" + Common.StringToDB(TotalPrice, true, true) +
																										",@TotalAmount=" + Common.StringToDB(TotalAmount, true, true) +
																										",@TotalTax=" + Common.StringToDB(TotalTax, true, true) +
																										",@TTotalAmount=" + Common.StringToDB(TotalAmount2, true, true) +
																										",@MisuAmout=" + Common.StringToDB(MisuAmout, true, true) +
									" WHERE TradingScheduleWirteHeadPk=" + TradingScheduleWirteHeadPk + ";");
		}
	
		if (ItemSum != "") {
			string[] Row = ItemSum.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
			string[] Each;
			foreach (string R in Row) {
				Each = R.Split(Common.Splite11, StringSplitOptions.None);
				if (Each[0] == "N") {
					Query.Append(GQ.UpdateOrInsertTradingScheduleItems("Insert", TradingScheduleWirteHeadPk, Each[1], Each[2], Each[3], Each[4], Each[5], Each[6], Each[7]));
				} else {
					Query.Append(GQ.UpdateOrInsertTradingScheduleItems("Update", Each[0], Each[1], Each[2], Each[3], Each[4], Each[5], Each[6], Each[7]));
				}
			}
		}

		DB.SqlCmd.CommandText = Query + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return TradingScheduleWirteHeadPk;
	}
	//화주문서페이지작업
	[WebMethod]
	public String DeleteDocument(string pk) {
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "delete newCommercialDocument where CommercialDocumentHeadPk=" + pk + ";" +
													  "delete DocumentFormItems where DocumentFormPk=" + pk + ";";

		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	//화주문서페이지작업
	[WebMethod]
	public String QuantityXUnitCostDocument(string X, string Y) {
		decimal temp = decimal.Parse(X) * decimal.Parse(Y);
		return temp + "";
	}
}