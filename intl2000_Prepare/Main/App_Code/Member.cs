using System;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using Components;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

/// <summary>
/// MemberJoin의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
[System.Web.Script.Services.ScriptService]
public class Member : System.Web.Services.WebService
{
	private DBConn DB;
	public Member() {
		//디자인된 구성 요소를 사용하는 경우 다음 줄의 주석 처리를 제거합니다. 
		//InitializeComponent(); 
	}
	/// <summary>
	/// 컬럼에 데이터 있는지 검사
	/// </summary>
	/// <param name="Column"></param>
	/// <param name="Data"></param>
	/// <returns></returns>
	[WebMethod]
	public int UniqueCheck(string Column, string Data) {
		DB = new DBConn();
        DB.SqlCmd.CommandText = "select count(*) from Account_ where " + Column + "='" + Data + "';";
        DB.DBCon.Open();
		int Check = (Int32)DB.SqlCmd.ExecuteScalar();
		DB.DBCon.Close();
		return Check;
	}
	[WebMethod]
	public string JoinFromEmailStep2Submit(string CompanyPk, string CompanyName, string CompanyNamee, string CompanyAddress, string CompanyTEL, string CompanyFAX, string PresidentName, string PresidentEmail, string CompanyNo, string RegionCodePk, string AdditionalInformationSum, string Step1Session, string StaffSum) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "Select RegionCode from RegionCode where RegionCodePk=" + RegionCodePk + ";";
		DB.DBCon.Open();
		string regionCode = DB.SqlCmd.ExecuteScalar() + "";//RegionCodePk => RegionCode
		StringBuilder ResultQuery = new StringBuilder();
		ResultQuery.Append("		UPDATE Company " +
											"		SET [GubunCL] = 0 " +
											"			,[CompanyName] = " + Common.StringToDB(CompanyName, true, true) +
											"			,[CompanyNamee] = " + Common.StringToDB(CompanyNamee, true, true) +
											"			,[RegionCode] = " + Common.StringToDB(regionCode, true, false) +
											"			,[CompanyAddress] = " + Common.StringToDB(CompanyAddress, true, true) +
											"			,[CompanyTEL] = " + Common.StringToDB(CompanyTEL, true, false) +
											"			,[CompanyFAX] = " + Common.StringToDB(CompanyFAX, true, false) +
											"			,[PresidentName] = " + Common.StringToDB(PresidentName, true, true) +
											"			,[PresidentEmail] = " + Common.StringToDB(PresidentEmail, true, false) +
											"			,[CompanyNo] = " + Common.StringToDB(CompanyNo, true, false) +
											"		WHERE CompanyPk=" + CompanyPk + ";");
		//CompanyPk = DB.SqlCmd.ExecuteScalar() + "";
		//InsertCompany
		string[] AccountValue = Step1Session.Split(new string[] { "#@!" }, StringSplitOptions.None);
		//Session["CompanyJoinStep1"] = "student83#@!1q2w3e#@!김상수#@!President#@!010-3137-0410#@!010-3137-0410#@!stud83@gmail.com#@!830410-1017716#@!3";

		ResultQuery.Append(@"INSERT INTO [Account_] ([AccountID], [Password], [GubunCL], [CompanyPk], [Duties], [Name], [TEL], [Mobile], [Email], [Authority], [IsEmailNSMS], [LastVisit])
     VALUES (" +
						Common.StringToDB(AccountValue[0], true, false) + ", " +
						Common.StringToDB(AccountValue[1], true, false) + ", " +
						"90, " +
						CompanyPk + ", " +
						Common.StringToDB(AccountValue[3], true, true) + ", " +
						Common.StringToDB(AccountValue[2], true, true) + ", " +
						Common.StringToDB(AccountValue[4], true, false) + ", " +
						Common.StringToDB(AccountValue[5], true, false) + ", " +
						Common.StringToDB(AccountValue[6], true, false) + ", " +
						"NULL, " +
						 Common.StringToDB(AccountValue[8], false, false) + ", " +
						 "NULL);");

		if (AccountValue[7] != "N") {
			ResultQuery.Append(" INSERT INTO CompanyAdditionalInfomation (CompanyPk, Title, Value) VALUES (" + CompanyPk + ", 60 , " + Common.StringToDB(AccountValue[7], true, true) + "); ");
		}

		///////////////////////STAFF
		string[] staffrow = StaffSum.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < staffrow.Length; i++) {
			string[] staffEach = staffrow[i].Split(Common.Splite22, StringSplitOptions.None);
			ResultQuery.Append(@"INSERT INTO [Account_] ([AccountID], [Password], [GubunCL], [CompanyPk], [Duties], [Name], [TEL], [Mobile], [Email], [Authority], [IsEmailNSMS], [LastVisit])
				VALUES (" +
					Common.StringToDB(staffEach[0], true, false) + ", " +
					Common.StringToDB(staffEach[1], true, false) + ", " +
					"91, " +
					CompanyPk + ", " +
					Common.StringToDB(staffEach[2], true, true) + ", " +
					Common.StringToDB(staffEach[3], true, true) + ", " +
					Common.StringToDB(staffEach[4], true, false) + ", " +
					Common.StringToDB(staffEach[5], true, false) + ", " +
					Common.StringToDB(staffEach[6], true, false) + ", " +
					Common.StringToDB(staffEach[7], true, false) + ", " +
					 Common.StringToDB(staffEach[8], false, false) + ", " +
					 "NULL);");
		}
		///////////////////////STAFF
		string[] data = AdditionalInformationSum.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < data.Length; i++) {
			string[] dataSplit = data[i].Split(new string[] { "###" }, StringSplitOptions.None);
			DB.SqlCmd.CommandText = "SELECT Count(*) FROM CompanyAdditionalInfomation WHERE CompanyPk=" + CompanyPk + " and Title=" + dataSplit[0] + ";";
			string counttemp = DB.SqlCmd.ExecuteScalar() + "";
			if (counttemp == "0") {
				ResultQuery.Append(" INSERT INTO CompanyAdditionalInfomation (CompanyPk, Title, Value) VALUES (" + CompanyPk + ", " + dataSplit[0] + ", N" + dataSplit[1] + "); ");
			} else {
				ResultQuery.Append("UPDATE CompanyAdditionalInfomation SET Value=N" + dataSplit[1] + " WHERE CompanyPk=" + CompanyPk + " and Title=" + dataSplit[0] + ";");
			}
		}

		DB.SqlCmd.CommandText = ResultQuery + "";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public String CompanyJoinStep2Submit(string CompanyName, string CompanyNamee, string CompanyAddress, string CompanyTEL, string CompanyFAX, string PresidentName, string PresidentEmail, string CompanyNo, string RegionCodePk, string AdditionalInformationSum, string Step1Session, string StaffSum) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "Select RegionCode from RegionCode where RegionCodePk=" + RegionCodePk + ";";
		DB.DBCon.Open();
		string regionCode = DB.SqlCmd.ExecuteScalar() + "";//RegionCodePk => RegionCode

		DB.SqlCmd.CommandText = "	INSERT INTO Company " +
			" (GubunCL, CompanyCode, CompanyName, CompanyNamee, RegionCode, CompanyAddress, CompanyTEL, CompanyFAX, PresidentName, PresidentEmail, CompanyNo) " +
			" VALUES " +
			"   (0, NULL, " + Common.StringToDB(CompanyName, true, true) + ", " + Common.StringToDB(CompanyNamee, true, true) + ", " + Common.StringToDB(regionCode, true, false) + ", " + Common.StringToDB(CompanyAddress, true, true) + ", " + Common.StringToDB(CompanyTEL, true, false) + ", " + Common.StringToDB(CompanyFAX, true, false) + ", " + Common.StringToDB(PresidentName, true, true) + ", " + Common.StringToDB(PresidentEmail, true, false) + ", " + Common.StringToDB(CompanyNo, true, false) + " ) " +
			"	SELECT @@IDENTITY;";
		string CompanyPk = DB.SqlCmd.ExecuteScalar() + "";  //InsertCompany
		StringBuilder ResultQuery = new StringBuilder();

		string[] AccountValue = Step1Session.Split(new string[] { "#@!" }, StringSplitOptions.None);
		//Session["CompanyJoinStep1"] = "stud83#@!1q2w3e#@!김상수#@!President#@!032-576-9508#@!010-3137-0410#@!stud83@gmail.com#@!830410-1017716#@!3";
		ResultQuery.Append(@"INSERT INTO [Account_] ([AccountID], [Password], [GubunCL], [CompanyPk], [Duties], [Name], [TEL], [Mobile], [Email], [Authority], [IsEmailNSMS], [LastVisit])
     VALUES (" +
				Common.StringToDB(AccountValue[0], true, false) + ", " +
				Common.StringToDB(AccountValue[1], true, false) + ", " +
				"90, " +
				CompanyPk + ", " +
				Common.StringToDB(AccountValue[3], true, true) + ", " +
				Common.StringToDB(AccountValue[2], true, true) + ", " +
				Common.StringToDB(AccountValue[4], true, false) + ", " +
				Common.StringToDB(AccountValue[5], true, false) + ", " +
				Common.StringToDB(AccountValue[6], true, false) + ", " +
				"NULL, " +
				 Common.StringToDB(AccountValue[8], false, false) + ", " +
				 "NULL);");

		if (AccountValue[7] != "N") {
			ResultQuery.Append(" INSERT INTO CompanyAdditionalInfomation (CompanyPk, Title, Value) VALUES (" + CompanyPk + ", 60 , " + Common.StringToDB(AccountValue[7], true, true) + "); ");
		}

		string[] data = AdditionalInformationSum.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < data.Length; i++) {
			string[] dataSplit = data[i].Split(new string[] { "###" }, StringSplitOptions.None);
			ResultQuery.Append(" INSERT INTO CompanyAdditionalInfomation (CompanyPk, Title, Value) VALUES (" + CompanyPk + ", " + dataSplit[0] + ", N" + dataSplit[1] + "); ");
		}

		///////////////////////STAFF
		string[] staffrow = StaffSum.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
		//StringBuilder Query = new StringBuilder();
		for (int i = 0; i < staffrow.Length; i++) {
			string[] staffEach = staffrow[i].Split(Common.Splite22, StringSplitOptions.None);
			ResultQuery.Append(@"INSERT INTO [Account_] ([AccountID], [Password], [GubunCL], [CompanyPk], [Duties], [Name], [TEL], [Mobile], [Email], [Authority], [IsEmailNSMS], [LastVisit])
				VALUES (" +
					Common.StringToDB(staffEach[0], true, false) + ", " +
					Common.StringToDB(staffEach[1], true, false) + ", " +
					"91, " +
					CompanyPk + ", " +
					Common.StringToDB(staffEach[2], true, true) + ", " +
					Common.StringToDB(staffEach[3], true, true) + ", " +
					Common.StringToDB(staffEach[4], true, false) + ", " +
					Common.StringToDB(staffEach[5], true, false) + ", " +
					Common.StringToDB(staffEach[6], true, false) + ", " +
					Common.StringToDB(staffEach[7], true, false) + ", " +
					 Common.StringToDB(staffEach[8], false, false) + ", " +
					 "NULL);");
		}
		///////////////////////STAFF
		DB.SqlCmd.CommandText = ResultQuery + "";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return CompanyPk;
	}

	[WebMethod]
	public void OwnCustomerListTo99(string pk) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = " UPDATE CompanyRelation SET  GubunCL = 99 WHERE CompanyRelationPk=" + pk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
	}

	[WebMethod]
	public String UpdateCompanyRelationMemo(string CompanyRelationPk, string Memo) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE [CompanyRelation] SET [Memo]=" + Common.StringToDB(Memo, true, true) + " WHERE [CompanyRelationPk]=" + CompanyRelationPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String UpdateCompanyRelationInformateion(string CompanyRelationPk, string CompanyPk, string TEL, string FAX, string Email, string PresidentName, string Memo) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE [Company] SET " +
			"  [CompanyTEL] = " + Common.StringToDB(TEL, true, false) +
			", [CompanyFAX] = " + Common.StringToDB(FAX, true, false) +
			", [PresidentName] = " + Common.StringToDB(PresidentName, true, true) +
			", [PresidentEmail] = " + Common.StringToDB(Email, true, false) +
			" WHERE [CompanyPk]=" + CompanyPk + ";" +
			" UPDATE [CompanyRelation] SET [Memo]=" + Common.StringToDB(Memo, true, true) + " WHERE [CompanyRelationPk]=" + CompanyRelationPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public string UpdateCompanyAccount(string ID, string Duties, string Name, string TEL, string Mobile, string Email) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE Account_ SET Duties = '" + Duties + "', Name='" + Name + "', TEL = '" + TEL + "' ,Mobile = '" + Mobile + "', Email = '" + Email + "' WHERE AccountID= '" + ID + "'";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public string FindIDPWEmailHint(string CompanyName, string PresidentName, string WriterName) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT C.PresidentEmail, A.Email FROM Account_ AS A left Join Company AS C ON C.CompanyPk=A.CompanyPk WHERE C.CompanyName='" + CompanyName + "' and C.PresidentName='" + PresidentName + "' and A.Name='" + WriterName + "';";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			string CompanyMail = RS[0] + "";
			string AccountMail = RS[1] + "";
			RS.Dispose();
			DB.DBCon.Close();
			return CompanyMail.Substring(0, 3) + "##" + CompanyMail.Substring(5, 3) + "##" + CompanyMail.Substring(10, CompanyMail.Length - 10) + "????" + AccountMail.Substring(0, 3) + "####" + (AccountMail.Substring(7, AccountMail.Length - 7));
		} else {
			RS.Dispose();
			DB.DBCon.Close();
			return "0";
		}
	}

	[WebMethod]
	public string ModifyCompanyInfo(string CompanyPk, string CompanyTEL, string CompanyFAX, string CompanyEmail, string AdditionalInfo) {
		StringBuilder Q = new StringBuilder();
		Q.Append("UPDATE [Company] SET [CompanyTEL] = " + Common.StringToDB(CompanyTEL, true, false) +
						"	,[CompanyFAX] = " + Common.StringToDB(CompanyFAX, true, false) +
						"	,[PresidentEmail] = " + Common.StringToDB(CompanyEmail, true, false) +
						" WHERE CompanyPk=" + CompanyPk + ";");
		Q.Append("DELETE FROM CompanyAdditionalInfomation WHERE CompanyPk=" + CompanyPk + " and Title in (62, 63, 64);");
		string[] data = AdditionalInfo.Split(new string[] { "@@@@" }, StringSplitOptions.RemoveEmptyEntries);
		foreach (string each in data) {
			string[] eachvalue = each.Split(new string[] { "##" }, StringSplitOptions.None);
			if (eachvalue[1] != "") {
				Q.Append("INSERT INTO CompanyAdditionalInfomation (CompanyPk, Title, Value) VALUES (" + CompanyPk + ", " + eachvalue[0] + ", " + Common.StringToDB(eachvalue[1], true, true) + "); ");
			}
		}
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = Q + "";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	//private String MakeQueryUpdateCompanyInfo(string CompanyPk, string CompanyNameE, string CompanyAddressE, string CompanyTEL, string CompanyFAX, string CompanyEmail)
	//    {
	//    StringBuilder Query = new StringBuilder();
	//        Query.Append("UPDATE Company SET ");
	//        Boolean isFirst=true;
	//        if (CompanyTEL != "N")
	//        {
	//            if (isFirst) { isFirst = false; }
	//            else { Query.Append(", "); }
	//            Query.Append("CompanyTEL = '" + CompanyTEL + "'");
	//        }
	//        if (CompanyFAX != "N")
	//        {
	//            if (isFirst) { isFirst = false; }
	//            else { Query.Append(", "); }
	//            Query.Append("CompanyFAX = '" + CompanyFAX + "'");
	//        }
	//        if (CompanyEmail != "N")
	//        {
	//            if (isFirst) { isFirst = false; }
	//            else { Query.Append(", "); }
	//            Query.Append("PresidentEmail = '" + CompanyEmail + "'");
	//        }
	//        if (CompanyNameE != "N")
	//        {
	//            if (isFirst) { isFirst = false; }
	//            else { Query.Append(", "); }
	//            Query.Append("CompanyNameE = '" + CompanyNameE + "'");
	//        }
	//        if (CompanyAddressE != "N")
	//        {
	//            if (isFirst) { isFirst = false; }
	//            else { Query.Append(", "); }
	//            Query.Append("CompanyAddressE = N'" + CompanyAddressE + "'");
	//        }
	//        Query.Append(" WHERE CompanyPk=" + CompanyPk + ";");

	//        if (isFirst) { return ""; }
	//        else { return Query + ""; }
	//    }

	[WebMethod]
	public string ChangePassword(string ID, string OldPwd, string NewPwd) {
		DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE Account_ SET Password = '" + NewPwd + "' WHERE AccountID='" + ID + "' and Password='" + OldPwd + "';";
		DB.DBCon.Open();
		int ActRow = DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return ActRow + "";
	}
	//Member/MyStaffView.aspx
	[WebMethod]
	public String[] MyStaffLoad(string CompanyPk) {
		List<string> ReturnValue = new List<string>();
		DB = new DBConn();
		DB.SqlCmd.CommandText = "	SELECT AccountPk, AccountID, Duties, Name, TEL, Mobile, Email, Authority, IsEmailNSMS " +
													"	FROM Account_ " +
													"	WHERE CompanyPk=" + CompanyPk + " and GubunCL=91 ;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			ReturnValue.Add(RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5] + "#@!" + RS[6] + "#@!" + RS[7] + "#@!" + RS[8]);
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue.ToArray();
	}
	[WebMethod]
	public string MyStaffModify(string StaffSum) {
		GetQuery GQ = new GetQuery();
		StringBuilder query = new StringBuilder();
		string[] StaffRow = StaffSum.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < StaffRow.Length; i++) {
			string[] Each = StaffRow[i].Split(Common.Splite22, StringSplitOptions.None);
			string id = Each[1] == "N0tCh@nged" ? "" : " [AccountID] = " + Common.StringToDB(Each[1], true, false) + ", ";
			string password = Each[2] == "N0tCh@nged" ? "" : " [Password] = " + Common.StringToDB(Each[2], true, false) + ", ";
			query.Append("UPDATE [Account_] SET " +
				id +
				password +
				"[Duties] = " + Common.StringToDB(Each[3], true, true) +
				", [Name] =" + Common.StringToDB(Each[4], true, true) +
				", [TEL] = " + Common.StringToDB(Each[5], true, false) +
				", [Mobile] = " + Common.StringToDB(Each[6], true, false) +
				", [Email] =" + Common.StringToDB(Each[7], true, false) +
				", [Authority] = " + Common.StringToDB(Each[8], true, false) +
				", [IsEmailNSMS] = " + Common.StringToDB(Each[9], false, false) +
				" WHERE AccountPk=" + Each[0]);
		}
		//return query + "";
		DB = new DBConn();
		DB.SqlCmd.CommandText = query + "";
		DB.DBCon.Open();

		try {
			DB.SqlCmd.ExecuteNonQuery();
		} catch (Exception) {
			DB.DBCon.Close();
			return "2";
		}
		DB.DBCon.Close();
		return "1";
	}
	//Member/MyStaffView.aspx
	// BMK CompanyJoinFromEmail
	[WebMethod]
	public String VerifyBetweenCompanyCodeNEmail(string CompanyPk, string Email) {
		string VerifyCheck = "N";
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [PresidentEmail] FROM Company WHERE CompanyPk=" + CompanyPk + " Union SELECT [Email] FROM Account_ WHERE CompanyPk=" + CompanyPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (Email == RS[0] + "") {
				VerifyCheck = "Y";
				break;
			}
		}
		RS.Dispose();
		if (VerifyCheck == "Y") {
			DB.SqlCmd.CommandText = "SELECT GubunCL FROM Company WHERE CompanyPk=" + CompanyPk + ";";
		}
		string gubncl = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		if (gubncl == "0") {
			VerifyCheck = "N";
		}
		return VerifyCheck;
	}
	public String[] LoadDataFromAlreadyData(string CompanyPk, out string CompanySum, out string StaffSum) {
		string[] ReturnValue = new string[] { "N" };
		;
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [CompanyCode], [CompanyName], [RegionCode], [CompanyAddress], [CompanyTEL], [CompanyFAX], [PresidentName], [PresidentEmail], [CompanyNameE], [CompanyAddressE] FROM Company WHERE CompanyPk=" + CompanyPk + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			CompanySum = RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5] + "#@!" + RS[6] + "#@!" + RS[7] + "#@!" + RS[8] + "#@!" + RS[9];
		} else {
			RS.Dispose();
			DB.DBCon.Close();
			StaffSum = "";
			CompanySum = "";
			return ReturnValue;
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = "SELECT [ID], [Duties], [Name], [TEL], [Mobile], [Email] FROM Account_ WHERE CompanyPk=" + CompanyPk + " ORDER BY [AccountID] ASC;";

		StringBuilder SB = new StringBuilder();
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (RS[1] + "" == "President") {
				ReturnValue = new string[] { RS[0] + "", RS[1] + "", RS[2] + "", RS[3] + "", RS[4] + "", RS[5] + "" };
			} else {
				SB.Append(RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "#@!" + RS[4] + "#@!" + RS[5] + "%!$@#");
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		StaffSum = SB + "";
		return ReturnValue;
	}
}