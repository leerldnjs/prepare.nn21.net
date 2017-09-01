using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// HistoryP의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
[System.Web.Script.Services.ScriptService]
public class HistoryP : System.Web.Services.WebService
{
	public string CommentPk { get; private set; }

	public HistoryP() {

		//디자인된 구성 요소를 사용하는 경우 다음 줄의 주석 처리를 제거합니다. 
		//InitializeComponent(); 
	}
	
	[WebMethod]
	public string Set_Comment(string Table_Name, string Table_Pk, string Category, string Contents, string Account_Id) {
		HistoryC HisC = new HistoryC();
		sComment Comment = new sComment();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		Comment.Table_Name = Table_Name;
		Comment.Table_Pk = Table_Pk;
		Comment.Category = Category;
		Comment.Contents = Contents;
		Comment.Account_Id = Account_Id;
		string[] Account = HisC.Load_AccountInfo(Account_Id, ref DB);
		Comment.Account_Pk = Account[0];
		Comment.Account_Name = Account[1];
		HisC.Set_Comment(Comment, ref DB);
		DB.DBCon.Close();

		return "1";
	}

	[WebMethod]
	public string Delete_Comment(string Comment_Pk) {
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"DELETE FROM [dbo].[COMMENT] WHERE [COMMENT_PK] = " + Comment_Pk;
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public string Delete_History(string History_Pk) {
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"DELETE FROM [dbo].[HISTORY] WHERE [HISTORY_PK] = @" + History_Pk + @";";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public string Delete_History(string Table_Name, string Table_Pk, string Code, ref DBConn DB) {
		DB.SqlCmd.CommandText = @"DELETE FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = '" + Table_Name + @"' [TABLE_PK] = " + Table_Pk + @" [CODE] = '" + Code + @"';";
		DB.SqlCmd.ExecuteNonQuery();
		return "1";
	}

	[WebMethod]
	public String ChangeCategory_FromComment(string Comment_Pk, string Category) {
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"UPDATE [dbo].[COMMENT] SET [CATEGORY]='" + Category + @"' WHERE [TABLE_NAME] = 'Company' AND [COMMENT_PK]=" + Comment_Pk + ";";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

}
