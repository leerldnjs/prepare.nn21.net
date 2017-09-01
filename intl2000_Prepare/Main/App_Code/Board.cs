using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using Components;

/// <summary>
/// Summary description for Board
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class Board : System.Web.Services.WebService {
	private DBConn DB;
	public Board () {
		DB = new DBConn();
    }

	//SetBoardCode
	[WebMethod]
	public string SaveNewBoardCode(string mode, string Code, string Title) {
		if (mode == "Modify") {
			DB.SqlCmd.CommandText = "UPDATE [BoardLibCode] SET [Title] = " + Common.StringToDB(Title, true, true) + " WHERE [BoardCode] = " + Common.StringToDB(Code, true, false) + ";";
			DB.DBCon.Open();
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";

		} else {
			DB.SqlCmd.CommandText = "SELECT Count(*) FROM [BoardLibCode] WHERE [BoardCode]=" + Common.StringToDB(Code, true, false);
			DB.DBCon.Open();
			string codecount = DB.SqlCmd.ExecuteScalar() + "";
			if (codecount != "0") {
				DB.DBCon.Close();
				return "0";
			}
			string OrderByQ = @"
				Declare @OrderBy smallint;
				SELECT top 1 @OrderBy=[OrderBy]+{0}
				FROM [BoardLibCode]
				WHERE {1}
				ORDER BY OrderBy DESC;
				SELECT isnull(@OrderBy, 100)";
			DB.SqlCmd.CommandText = (Code.Length < 3 ? string.Format(OrderByQ, "100", "len([BoardCode])=2") : string.Format(OrderByQ, "1", "left([BoardCode], 2)='" + Code.Substring(0, 2) + "'")) +
				"INSERT INTO [BoardLibCode] ([BoardCode], [Title], [OrderBy]) VALUES (" + Common.StringToDB(Code, true, false) + ", " + Common.StringToDB(Title, true, true) +
				", isnull(@OrderBy, 100));";
			//return DB.SqlCmd.CommandText;
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		}
	}
	[WebMethod]
	public String ChangeBoardCodeOrder(string UpOrDown, string BoardCode)
	{
		string q = @"
DECLARE @TargetOrder smallint;
DECLARE @TargetCode varchar(5);	
SET @TargetCode ='{0}';
SELECT @TargetOrder=[OrderBy] FROM [BoardLibCode] WHERE [BoardCode]=@TargetCode; 
  if (len(@TargetCode)=2)
	BEGIN
		SELECT TOP 1 [BoardCode] , ABS(@TargetOrder-[OrderBy]) FROM [BoardLibCode] 
		WHERE OrderBy {1} @TargetOrder and len([BoardCode])=len(@TargetCode) 
		ORDER BY OrderBy {2};
	END
else
	BEGIN
		SELECT TOP 1 [BoardCode] , ABS(@TargetOrder-[OrderBy]) FROM [BoardLibCode] 
		WHERE OrderBy {1} @TargetOrder and len([BoardCode])=len(@TargetCode) and left([BoardCode], 2)=left(@TargetCode, 2)
		ORDER BY OrderBy {2};
	END";

		if (UpOrDown == "Up") {
			DB.SqlCmd.CommandText = string.Format(q, BoardCode, @"<", "DESC");
		} else {
			DB.SqlCmd.CommandText = string.Format(q, BoardCode, @">", "ASC");
		}
		//return DB.SqlCmd.CommandText;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		String targetCode = "";
		String Span = "";
		if (RS.Read()) {
			targetCode = RS[0] + "";
			if (UpOrDown == "Up") {
				Span = RS[1] + "";
			} else { 
				Span=RS[1] + "";
			}
			
		} else {
			RS.Dispose();
			DB.DBCon.Close();
			return "0";
		
		}
		if (targetCode == "") {
			RS.Dispose();
			DB.DBCon.Close();
			return "0";
		}
		RS.Dispose();

		string q2;
		if (BoardCode.Length == 2) {
			q2 = @"
UPDATE [BoardLibCode] SET [OrderBy] = [OrderBy] +{2} WHERE left([BoardCode], 2)='{0}';
UPDATE [BoardLibCode] SET [OrderBy] = [OrderBy] -{2} WHERE left([BoardCode], 2)='{1}';";
		} else {
			q2 = @"
UPDATE [BoardLibCode] SET [OrderBy] = [OrderBy] +{2} WHERE [BoardCode]='{0}';
UPDATE [BoardLibCode] SET [OrderBy] = [OrderBy] -{2} WHERE [BoardCode]='{1}';";
		}


		if (UpOrDown == "Up") {
			DB.SqlCmd.CommandText = string.Format(q2, targetCode, BoardCode, Span);
		} else {
			DB.SqlCmd.CommandText = string.Format(q2, BoardCode, targetCode, Span);
		}
		//DB.DBCon.Close();
		//return DB.SqlCmd.CommandText;

		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		//return DB.SqlCmd.CommandText;
		//BoardLibCodeOrderBySetClear();
		return "1";
	}
	private String BoardLibCodeOrderBySetClear()
	{
		string UpdateFormat = "UPDATE [BoardLibCode] SET [OrderBy] = {1} WHERE [BoardCode] = '{0}';";
		StringBuilder ReturnValue = new StringBuilder();

		int MainCodeOrder = 0;
		int SubCodeOrder = 0;

		DB.SqlCmd.CommandText = @"SELECT [BoardCode] FROM [BoardLibCode] ORDER BY OrderBy;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string code = RS[0] + "";
			if (code.Length == 2) {
				MainCodeOrder += 100;
				SubCodeOrder = 0;
			} else {
				SubCodeOrder++;
			}
			ReturnValue.Append(String.Format(UpdateFormat, code, (MainCodeOrder + SubCodeOrder).ToString()));
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = ReturnValue + "";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String DeleteBoard(string Mode, string BoardCode)
	{
		switch (Mode) {
			case "MainCode":
				DB.SqlCmd.CommandText = @"
DECLARE @BoardCode varchar(5); 
DECLARE @Count int; 
SET @BoardCode='" + BoardCode + @"';
SELECT @Count=Count(*) FROM [BoardLibCode] WHERE Left(BoardCode, 2)=@BoardCode; 
if (@Count<2)
	BEGIN
		DELETE FROM [BoardLibCode] WHERE BoardCode=@BoardCode; 
		SELECT '1'; 
	END
else 
	BEGIN
		SELECT '0';
	END";
				DB.DBCon.Open();
				string ReturnValue = DB.SqlCmd.ExecuteScalar() + "";
				DB.DBCon.Close();
				return ReturnValue;
			case "Code":
				DB.SqlCmd.CommandText = @"
DECLARE @BoardCode varchar(5); 
SET @BoardCode='" + BoardCode + @"';
DELETE FROM [BoardLibCode] WHERE BoardCode=@BoardCode; 
DELETE FROM [BoardLibHeader]  WHERE BoardCode=@BoardCode;
DELETE FROM [BoardLibPermission] WHERE BoardCode=@BoardCode;";
				DB.DBCon.Open();
				DB.SqlCmd.ExecuteNonQuery();
				DB.DBCon.Close();
				return "1";
			default:
				return "0";
		}
	}
	//SetBoardCode

	//SetBoardHeader
	[WebMethod]
	public String SaveNewBoardHeader(string Pk, string Code, string Header)
	{
		if (Pk == "N") {
			DB.SqlCmd.CommandText = @"
Declare @OrderBy smallint;
Declare @BoardCode char(5); 
Declare @Header nvarchar(10); 
Set @BoardCode=" + Common.StringToDB(Code, true, false) + @"
Set @Header =" + Common.StringToDB(Header, true, true) + @"
SELECT top 1 @OrderBy=[OrderBy]
FROM [BoardLibHeader]
WHERE BoardCode=@BoardCode
ORDER BY OrderBy DESC;
INSERT INTO [BoardLibHeader] ([BoardCode], [Header], [OrderBy]) VALUES (@BoardCode, @Header, isnull(@OrderBy, 0)+1);";
		} else {
			DB.SqlCmd.CommandText = @"
Declare @Header nvarchar(10); 
Declare @Pk int;

Set @Header =" + Common.StringToDB(Header, true, true) + @"
Set @Pk=" + Pk + @"

UPDATE [BoardLibHeader] SET [Header] = @Header WHERE Pk=@Pk;";
		}
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String ChangeBoardHeaderOrder(string UpOrDown, string BoardCode, string Pk)
	{
		string q = @"
DECLARE @TargetOrder smallint;
DECLARE @TargetCode varchar(5);
DECLARE @TargetPk int;
SET @TargetCode ='{0}';
SET @TargetPk={1};

SELECT @TargetOrder=[OrderBy] FROM [BoardLibHeader] WHERE [Pk]=@TargetPk; 
SELECT TOP 1 [Pk] FROM [BoardLibHeader] WHERE OrderBy{2}@TargetOrder and [BoardCode]=@TargetCode ORDER BY OrderBy {3};";
		if (UpOrDown == "Up") {
			DB.SqlCmd.CommandText = string.Format(q, BoardCode, Pk, "<", "DESC");
		} else {
			DB.SqlCmd.CommandText = string.Format(q, BoardCode, Pk, ">", "ASC");
		}
		//return DB.SqlCmd.CommandText;
		DB.DBCon.Open();
		String targetPk = DB.SqlCmd.ExecuteScalar() + "";

		if (targetPk == "") {
			DB.DBCon.Close();
			return "0";
		}

		string q2 = @"
UPDATE [BoardLibHeader] SET [OrderBy] = [OrderBy] +1 WHERE [Pk]='{0}';
UPDATE [BoardLibHeader] SET [OrderBy] = [OrderBy] -1 WHERE [Pk]='{1}';";


		if (UpOrDown == "Up") {
			DB.SqlCmd.CommandText = string.Format(q2, targetPk, Pk);
		} else {
			DB.SqlCmd.CommandText = string.Format(q2, Pk, targetPk);
		}

		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		//return DB.SqlCmd.CommandText;
		return "1";
	}
	[WebMethod]
	public String DeleteBoardHeader(string BoardCode, string Pk)
	{
		DB.SqlCmd.CommandText = "DELETE FROM [BoardLibHeader] WHERE Pk=" + Pk + ";" +
			"SELECT [Pk] From BoardLibHeader WHERE BoardCode='" + BoardCode + "' ORDER BY OrderBy ASC;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		StringBuilder QuerySum = new StringBuilder();
		int OrderByC = 1;
		while (RS.Read()) {
			QuerySum.Append("UPDATE [BoardLibHeader] SET [OrderBy] = " + OrderByC + " WHERE Pk=" + RS["Pk"] + ";");
			OrderByC++;
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = QuerySum + "";
		if (DB.SqlCmd.CommandText != "") {
			DB.SqlCmd.ExecuteNonQuery();
		}
		DB.DBCon.Close();
		return "1";
	}
	//SetBoardHeader
	//SetBoardPermission
	[WebMethod]
	public String InsertBoardPermission(string BoardCode, string Type, string Value, string PermissionType)
	{
		if (Type == "AccountID") {
			DB.SqlCmd.CommandText = @"
DECLARE @Count int; 
SELECT @Count=count(*) FROM [Account_] WHERE gubunCL=93 and AccountID='" + Value + @"'; 
  
if (@Count=0)
	BEGIN SELECT '0'; END; 
else 
	BEGIN 
		DELETE FROM [BoardLibPermission] WHERE BoardCode='" + BoardCode + "' and TargetID='" + Value + "';" +
		"INSERT INTO [BoardLibPermission] ([BoardCode], [PermissionType], [TargetID]) VALUES (" +
		Common.StringToDB(BoardCode, true, false) + ", " + Common.StringToDB(PermissionType, true, false) + ", " + Common.StringToDB(Value, true, false) + @");
		SELECT '1'; 
	end ";
			DB.DBCon.Open();
			string ReturnValue = DB.SqlCmd.ExecuteScalar() + "";
			DB.DBCon.Close();
			return ReturnValue;
		} else {
			DB.SqlCmd.CommandText = "DELETE FROM [BoardLibPermission] WHERE BoardCode='" + BoardCode + "' and TargetID in (SELECT [AccountID] FROM [Account_] WHERE CompanyPk=" + Value + ");" +
				"INSERT INTO [BoardLibPermission] ([BoardCode], [PermissionType], [TargetID]) SELECT '" + BoardCode + "', '" + PermissionType + "' , [AccountID] FROM [Account_] WHERE CompanyPk=" + Value + ";";
			DB.DBCon.Open();
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";

		}
	}
	[WebMethod]
	public String DeleteBoardPermission(string Pk)
	{
		DB.SqlCmd.CommandText = "DELETE FROM [BoardLibPermission] WHERE [Pk]=" + Pk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	//SetBoardPermission
	//C_View
	[WebMethod]
	public String ModifyComment(string CommentPk, string Comment)
	{
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = "UPDATE [BoardComment] SET [Comment] = " + Common.StringToDB(Comment.Replace("\n", "<br />").Replace("\r\n", "<br />"), true, true) + " WHERE Pk=" + CommentPk+ ";";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String SaveComment(string ContentsPk, string Position, string Comment, string AccountID, string Name)
	{
		if (Position == "") {
			DB.SqlCmd.CommandText = @"
DECLARE @Position varchar(20); 
SELECT TOP 1 @Position=left(Position, 2) FROM [BoardComment] WHERE ContentsPk=" + ContentsPk + @" ORDER BY Position DESC;
SELECT isnull(@Position, '00'); ";
		} else {
			DB.SqlCmd.CommandText = @"
DECLARE @Position varchar(20); 
SELECT TOP 1 @Position=right(Position, 2) FROM [BoardComment] WHERE ContentsPk=" + ContentsPk + @" and left(Position, " + Position.Length + @")='" + Position + @"' and len(Position)=" + (Position.Length + 3) + @" ORDER BY Position DESC;
SELECT isnull(@Position, '00'); ";
		}
		DB.DBCon.Open();
		string tempPosition = DB.SqlCmd.ExecuteScalar() + "";
		int intPosition = Int32.Parse(tempPosition) + 1;
		if (Position != "") {
			Position += "_";
		}
		if (intPosition < 10) {
			Position += "0" + intPosition.ToString();
		} else {
			Position += intPosition.ToString();
		}

		DB.SqlCmd.CommandText = "INSERT INTO [BoardComment] ([ContentsPk], [Position], [Comment], [AccountID], [Name]) VALUES (" +
			Common.StringToDB(ContentsPk, false, false) + ", " +
			Common.StringToDB(Position, true, false) + ", " +
			Common.StringToDB(Comment.Replace("\n", "<br />").Replace("\r\n", "<br />"), true, true) + ", " +
			Common.StringToDB(AccountID, true, false) + ", " +
			Common.StringToDB(Name, true, true) + ");";

		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String SerchUpperOrUnderPk(string ContentsPk, string Mode)
	{
		if (Mode == "upper") {
			DB.SqlCmd.CommandText = @"
DECLARE @Pk int; 
DECLARE @ParentsPk int; 
DECLARE @Position varchar(20); 
DECLARE @BoardCode char(5);
SET @Pk=" + ContentsPk + @"; 

SELECT @BoardCode=[BoardCode], @Position=[Position], @ParentsPk=[ParentsPk] FROM [BoardContents] WHERE Pk=@Pk

SELECT TOP 1 ParentsPk 
FROM [BoardContents] 
WHERE isnull(DeletedID, '')='' and [BoardCode]=@BoardCode and [ParentsPk] >@ParentsPk 
ORDER BY ParentsPk ASC;";
		} else {
			DB.SqlCmd.CommandText = @"
DECLARE @Pk int; 
DECLARE @ParentsPk int; 
DECLARE @Position varchar(20); 
DECLARE @BoardCode char(5);
SET @Pk=" + ContentsPk + @"; 

SELECT @BoardCode=[BoardCode], @Position=[Position], @ParentsPk=[ParentsPk] FROM [BoardContents] WHERE Pk=@Pk

SELECT TOP 1 ParentsPk 
FROM [BoardContents] 
WHERE isnull(DeletedID, '')='' and [BoardCode]=@BoardCode and [ParentsPk] <@ParentsPk 
ORDER BY ParentsPk DESC;";
		}
		DB.DBCon.Open();
		string ReturnValue = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();
		return ReturnValue;
	}
	[WebMethod]
	public String DeleteContents(string ContentsPk, string AccountID)
	{
		DB.SqlCmd.CommandText = "UPDATE [BoardContents] SET [Deleted] = getDate(), [DeletedID] = '" + AccountID + "' WHERE Pk=" + ContentsPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String DeleteComment(string Pk)
	{
		DB.SqlCmd.CommandText = "UPDATE [BoardComment] SET [Deleted] = getDate() WHERE Pk=" + Pk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public String MoveBoardCode(string ContentsPk, string ToBoardCode)
	{
		DB.SqlCmd.CommandText = "  UPDATE [BoardContents] SET [BoardCode] = '" + ToBoardCode + "' WHERE ParentsPk=" + ContentsPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	//C_View
	//C_Write
	[WebMethod]
	public String DeleteFile(string FilePk)
	{
		DB.SqlCmd.CommandText = "UPDATE [BoardAttachedFile] SET [Deleted] = getDate() WHERE Pk=" + FilePk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	//C_Write
}
