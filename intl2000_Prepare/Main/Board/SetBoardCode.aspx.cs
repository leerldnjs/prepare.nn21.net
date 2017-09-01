using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Board_SetBoardCode : System.Web.UI.Page
{
	protected String BoardList;
	protected StringBuilder CategoryOption;
	private DBConn DB;
	protected void Page_Load(object sender, EventArgs e)
	{
		BoardList = LoadBoardCodeLib();
	}
	private string LoadBoardCodeLib()
	{
		StringBuilder ReturnValue = new StringBuilder();
		CategoryOption = new StringBuilder();
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"SELECT [BoardCode], [Title], [OrderBy] FROM [BoardLibCode] ORDER BY OrderBy;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string BoardCode = RS["BoardCode"] + "";
			string Title = RS["Title"] + "";
			if (BoardCode.Length == 2) {
				if (ReturnValue + "" != "") {
					ReturnValue.Append("</td></tr></table>");
				}
				ReturnValue.Append(string.Format("<table border=\"1\" align=\"center\"  cellpadding=\"0\" cellspacing=\"0\" bordercolor=\"#708090\"  style=\"border-collapse:collapse;width:490px;background-color:#FFFFFF; margin-bottom:10px;\" >" +
					"<tr><td bgcolor=\"#D1E3E5\" align=\"center\" style=\"height:35px; \"><b>" +
					"{0}" + "</b></td><td bgcolor=\"#D1E3E5\" style=\"width:280px; padding-left:5px;\">" +
					"<input type=\"button\" onclick=\"ChageOrder('Up', '{1}');\" value=\"↑ up\" />" +
					"<input type=\"button\" onclick=\"ChageOrder('Down', '{1}');\" value=\"↓ down\" />" +
					"<input type=\"button\" onclick=\"SetModify('MainCode', '" + BoardCode + "', '" + Title + "');\" value=\"수정\" />" +
					"<input type=\"button\" onclick=\"DeleteBoard('MainCode', '" + BoardCode + "');\" style=\"color:red;\" value=\"삭제\" />" +
					"</td></tr>", Title, BoardCode));
				CategoryOption.Append("<option value=\"" + BoardCode + "\">" + Title + "</option>");
			} else {
				ReturnValue.Append("<tr><td bgcolor=\"#F0F5F6\"  align=\"center\"  style=\"height:30px; \">" + BoardCode + " : " + Title + "</td>" +
					"<td style=\"padding-left:5px; width:280px;\">" +
					"<input type=\"button\" onclick=\"ChageOrder('Up', '" + BoardCode + "');\" value=\"↑\" />" +
					"<input type=\"button\" onclick=\"ChageOrder('Down', '" + BoardCode + "');\" value=\"↓\" />" +
					"<input type=\"button\" onclick=\"PopOpen('Permission', '" + BoardCode + "');\" value=\"권한\" />" +
					"<input type=\"button\" onclick=\"PopOpen('Header', '" + BoardCode + "');\" value=\"글머리\" />" +
					"<input type=\"button\" onclick=\"SetModify('Code', '" + BoardCode + "', '" + Title + "');\" value=\"수정\" />" +
					"<input type=\"button\" onclick=\"DeleteBoard('Code', '" + BoardCode + "');\" style=\"color:red;\" value=\"삭제\" /></li>");
			}
		}

		if (ReturnValue + "" != "") {
			ReturnValue.Append("</td></tr></table>");
		}

		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue + "";
	}
}