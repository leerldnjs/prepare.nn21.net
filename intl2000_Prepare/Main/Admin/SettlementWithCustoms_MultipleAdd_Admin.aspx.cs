using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_SettlementWithCustoms_MultipleAdd_Admin : System.Web.UI.Page
{
	protected String[] MemberInfo;
	protected String Html;
	protected String SelectedDate;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) { Response.Redirect("../Default.aspx"); }
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		if (MemberInfo[0] == "Customs") {
			LogedWithoutRecentRequest111.Visible = false;
			Loged1.Visible = true;
		}
		if (IsPostBack) {
			//SelectedDate = Request.Form["Date"].ToString();
			if (Request.Form["HClipBoard"].ToString() != "") {
				Html = Make_HtmlList(Request.Form["HClipBoard"].ToString());
			}
		}
	}
	private String Make_HtmlList(string ClipBoard)
	{
		string[] EachRow = ClipBoard.Split(new string[] { "%!$@#" }, StringSplitOptions.RemoveEmptyEntries);
		List<string[]> ClipBoardData = new List<string[]>();

		StringBuilder QueryWhereIn = new StringBuilder();
		for (var i = 0; i < EachRow.Length; i++) {
			if (EachRow[i] == "@#$@#$@#$\r\n") {
				continue;
			}

			string[] Each = EachRow[i].Split(new string[] { "@#$" }, StringSplitOptions.None);
			QueryWhereIn.Append(",'" + Each[1] + "'");
			ClipBoardData.Add(Each);
		}

		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT [BLNo]
  FROM [dbo].[CommercialDocument]
  WHERE BLNo in (" + QueryWhereIn.ToString().Substring(1) + @");";
		DB.DBCon.Open();
		List<string> CheckedByDB = new List<string>();

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			CheckedByDB.Add(RS[0].ToString().Trim() + "");
		}
		RS.Dispose();
		DB.DBCon.Close();


		int Count = 0;
		decimal Total = 0;

		StringBuilder ReturnValueTrue = new StringBuilder();
		StringBuilder ReturnValueFalse = new StringBuilder();
		for (var i = 0; i < ClipBoardData.Count; i++) {
			bool Checked = false;
			foreach (string BLNo in CheckedByDB) {
				if (BLNo == ClipBoardData[i][1]) {
					Checked = true;
					break;
				}
			}
			if (Checked) {
				ReturnValueTrue.Append("<tr><td class='TBody1' ><input type='text' id='Date" + Count + "' value=\"" + ClipBoardData[i][0] + "\" /></td>" +
											"<td class='TBody1'style='text-align:center;'><input type='text' style='text-align:right;' id='BLNo" + Count + "' value=\"" + ClipBoardData[i][1] + "\" /></td>" +
											"<td class='TBody1'><input type='text' style='text-align:right;' id='Price" + Count + "' value=\"" + ClipBoardData[i][2] + "\" /></td>" +
											"<td class='TBody1'><input type='text' id='Description" + Count + "' style='width:350px; ' " + (ClipBoardData[i].Length > 2 ? " value=\"" + ClipBoardData[i][3] + "\" " : "") + " /> </td></tr>");
				Total += decimal.Parse(ClipBoardData[i][2]);
				Count++;
			} else {
				ReturnValueFalse.Append("<p>" + ClipBoardData[i][1] + "</p>");
			}
		}

		
		//string[] Pasted = EachRow[0].Split(new string[] { "@#$" }, StringSplitOptions.None);
		return "	<table border='0' cellpadding='0' cellspacing='0' style='width:750px; margin:0 auto; ' ><thead>" +
										"		<tr style=\"height:40px;\">" +
										"		<td class='THead1' style=\"bold; width:170px;\" >Date</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; width:170px;  \" >House BL</td>" +
										"		<td class='THead1' style=\"bold; width:170px;\" >금액</td>" +
										"		<td class='THead1' >Description</td>" +
										"		</tr></thead>" + ReturnValueTrue +
										"		<tr style=\"height:40px;\">" +
										"		<td class='THead1' >&nbsp;</td>" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; width:170px;  \" >총 " + Count + "건</td>" +
										"		<td class='THead1' style=\"bold; width:170px;\" >총 " + Common.NumberFormat(Total.ToString()) + "원</td>" +
										"		<td class='THead1' >&nbsp;</td>" +
										"		</tr></table>" + (ReturnValueFalse + "" == "" ? "" : "<div><p>잘못된 BL번호</p>" + ReturnValueFalse + "</div>");
	}
	/*
	private String Make_HtmlList(string ClipBoard)
	{
		string[] EachRow = ClipBoard.Split(new string[] { "%!$@#" }, StringSplitOptions.RemoveEmptyEntries);
		Dictionary<string, string> ClipBoardBL = new Dictionary<string, string>();

		StringBuilder QueryWhereIn = new StringBuilder();
		for (var i = 0; i < EachRow.Length; i++) {
			string[] Each = EachRow[i].Split(new string[] { "@#$" }, StringSplitOptions.None);
			QueryWhereIn.Append(",'" + Each[0] + "'");
			ClipBoardBL.Add(Each[0], Each[1]);
		}

		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT [BLNo]
  FROM [dbo].[CommercialDocument]
  WHERE BLNo in (" + QueryWhereIn.ToString().Substring(1) + @");";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder ReturnValue = new StringBuilder();
		int Count = 0;
		decimal Total = 0;
		while (RS.Read()) {
			ReturnValue.Append("<tr><td class='TBody1' style='text-align:center;'><input type='text' id='BLNo" + Count + "' value=\"" + RS[0] + "\" /></td><td class='TBody1'><input type='text' style='text-align:right;' id='Price" + Count + "' value=\"" + ClipBoardBL[RS[0] + ""] + "\" /> </td><td class='TBody1'><input type='text' id='Description" + Count + "' style='width:350px; ' /> </td></tr>");
			Total += decimal.Parse(ClipBoardBL[RS[0] + ""] + "");
			Count++;
		}
		RS.Dispose();
		DB.DBCon.Close();
		//string[] Pasted = EachRow[0].Split(new string[] { "@#$" }, StringSplitOptions.None);
		return "	<table border='0' cellpadding='0' cellspacing='0' style='width:750px; margin:0 auto; ' ><thead>" +
										"		<tr style=\"height:40px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; width:170px;  \" >House BL</td>" +
										"		<td class='THead1' style=\"bold; width:170px;\" >금액</td>" +
										"		<td class='THead1' >Description</td>" +
										"		</tr></thead>" + ReturnValue + 
										"		<tr style=\"height:40px;\">" +
										"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; width:170px;  \" >총 "+Count+"건</td>" +
										"		<td class='THead1' style=\"bold; width:170px;\" >총 "+Common.NumberFormat(Total.ToString())+"원</td>" +
										"		<td class='THead1' >&nbsp;</td>" +
										"		</tr></table>";
	}	 
	 */
}