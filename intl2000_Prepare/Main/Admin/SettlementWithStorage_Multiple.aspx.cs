using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_SettlementWithStorage_Multiple : System.Web.UI.Page
{
	protected String[] MemberInfo;
	protected String Html;
	protected String SelectedDate;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) { Response.Redirect("../Default.aspx"); }
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		if (IsPostBack) {
			if (Request.Form["HClipBoard"].ToString() != "") {
				Html = Make_HtmlList(Request.Form["HClipBoard"].ToString());
			}
		}
	}
	private String Make_HtmlList(string ClipBoard) {
		string[] EachRow = ClipBoard.Split(new string[] { "%!$@#" }, StringSplitOptions.RemoveEmptyEntries);
		List<string[]> ClipBoardData = new List<string[]>();

		StringBuilder QueryWhereIn = new StringBuilder();
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		List<dynamic> ReturnValue = new List<dynamic>();
		List<string[]> ReturnValueNotIn = new List<string[]>();
		for (var i = 0; i < EachRow.Length; i++) {
			string[] Each = EachRow[i].Split(new string[] { "@#$" }, StringSplitOptions.None);
			DB.SqlCmd.CommandText = @"
				SELECT 
					[TRANSPORT_PK],[TRANSPORT_WAY],[VALUE_STRING_0],[BRANCHPK_FROM],[BRANCHPK_TO],[AREA_FROM], [AREA_TO], [DATETIME_FROM],[DATETIME_TO], [VESSELNAME], [VOYAGE_NO], [VALUE_STRING_1], [VALUE_STRING_2], [VALUE_STRING_3], PACKED.[SEAL_NO]
				FROM [INTL2010].[dbo].[TRANSPORT_HEAD] AS HEAD
				INNER JOIN [dbo].[TRANSPORT_PACKED] AS PACKED ON HEAD.[TRANSPORT_PK] = PACKED.[TRANSPORT_HEAD_PK]
				WHERE [VALUE_STRING_0] ='" + Each[0] + @"';";

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			var count = 0;
			dynamic each = new ExpandoObject();
			while (RS.Read()) {
				each = new ExpandoObject();
				count++;
				each.TransportBetweenBranchPk = RS["TRANSPORT_PK"] + "";
				each.TransportCL = RS["TRANSPORT_WAY"] + "";
				each.BLNo = RS["VALUE_STRING_0"] + "";
				each.FromBranchPk = RS["BRANCHPK_FROM"] + "";
				each.ToBranchPk = RS["BRANCHPK_TO"] + "";
				each.AreaFrom = RS["AREA_FROM"] + "";
				each.AreaTo = RS["AREA_TO"] + "";
				each.FromDateTime = RS["DATETIME_FROM"] + "";
				each.ToDateTime = RS["DATETIME_TO"] + "";
				each.VesselName = RS["VESSELNAME"] + "";
				each.VoyageNo = RS["VOYAGE_NO"] + "";
				each.FromHp = RS["VALUE_STRING_1"] + "";
				each.ToHp = RS["VALUE_STRING_2"] + "";
				each.CarSize = RS["VALUE_STRING_3"] + "";
				each.SealNo = RS["SEAL_NO"] + "";
				each.Price = Each[1];
			}
			RS.Dispose();
			if (count == 1) {
				ReturnValue.Add(each);
			} else {
				ReturnValueNotIn.Add(Each);
			}
		}
		DB.DBCon.Close();

		StringBuilder TableBody = new StringBuilder();

		for (var i = 0; i < ReturnValue.Count; i++) {
			string Title = "";
			if (ReturnValue[i].TransportCL == "Ship") {
				if (ReturnValue[i].VesselName.Length > 15) {
					Title += ReturnValue[i].VesselName.Substring(0, 14) + " <em>" + ReturnValue[i].VoyageNo + "</em> " + ReturnValue[i].SealNo;
				} else {
					Title += ReturnValue[i].VesselName + " <em>" + ReturnValue[i].VoyageNo + "</em> " + ReturnValue[i].SealNo;
				}
			} else {
				Title = ReturnValue[i].VesselName + "" == "" ? "&nbsp;" : ReturnValue[i].VesselName;
			}

			var date = Common.DateFormat("MD~D", ReturnValue[i].FromDateTime, ReturnValue[i].ToDateTime);

			TableBody.Append("<tr height=\"30px; \">" +
				"<td style=\"text-align:left;\" class=\"TBody1\" >" + ReturnValue[i].TransportCL + " From " + "</td>" +
				"<td class=\"TBody1\" style=\"text-align:left;\" >" + Title + "</td>" +
				"<td class=\"TBody1\" style=\"text-align:center;\" ><a href=\"TransportBetweenBranchView.aspx?G=In&S=" + ReturnValue[i].TransportBetweenBranchPk + "\"> " + date + "</td>" +
				"<td class=\"TBody1\">" + ReturnValue[i].AreaFrom + " ~ " + ReturnValue[i].AreaTo + "</td>" +
				"<td class=\"TBody1\" style=\"text-align:center;\"><input type='hidden' id='TBRow_" + i + "_TBBHPk' value='" + ReturnValue[i].TransportBetweenBranchPk + "' /><input type='text' style='width:90px; text-align:right; ' id='TBRow_" + i + "_Price' value='" + ReturnValue[i].Price + "' /></td>" +
				"</tr>");
		}


		StringBuilder NotIn = new StringBuilder();
		for (var i = 0; i < ReturnValueNotIn.Count; i++) {
			NotIn.Append("<p>" + ReturnValueNotIn[i][0] + " : " + ReturnValueNotIn[i][1] + "</p>");
		}

		return "<table border='0' cellpadding='0' cellspacing='0' style=\"width:1050px;\" ><thead><tr height='30px'>" +
									"<td class='THead1' style='width:130px;' >Description</td>" +
									"<td class='THead1' style='width:300px;' >Title</td>" +
									"<td class='THead1' style='width:90px;' >Date</td>" +
									"<td class='THead1' >Area</td>" +
									"<td class='THead1' >Price</td>" +
								"</tr></thead>" + TableBody + "<tr><td colspan='5' style='text-align:right; '>" +
									"<input type='text' id='ActDate' value='20170000' />" +
									"<input type='text' id='Comment' />" +
									"<input type='button' value='저장' onclick='SetSave();' />" +
								"</td></tr></table>" + NotIn;

		/*
				int Count = 0;
				decimal Total = 0;

				StringBuilder ReturnValueTrue = new StringBuilder();
				StringBuilder ReturnValueFalse = new StringBuilder();
				for (var i = 0; i < ClipBoardData.Count; i++) {
					bool Checked = false;
					foreach (string BLNo in CheckedByDB) {
						if (BLNo == ClipBoardData[i][0]) {
							Checked = true;
							break;
						}
					}
					if (Checked) {
						ReturnValueTrue.Append("<tr><td class='TBody1' style='text-align:center;'><input type='text' id='BLNo" + Count + "' value=\"" + ClipBoardData[i][0] + "\" /></td>" +
								"<td class='TBody1'><input type='text' style='text-align:right;' id='Price" + Count + "' value=\"" + ClipBoardData[i][1] + "\" /></td>" +
								"<td class='TBody1'><input type='text' id='Description" + Count + "' style='width:350px; ' " + (ClipBoardData[i].Length > 2 ? " value=\"" + ClipBoardData[i][2] + "\" " : "") + " /> </td></tr>");
						Total += decimal.Parse(ClipBoardData[i][1]);
						Count++;
					} else {
						ReturnValueFalse.Append("<p>" + ClipBoardData[i][0] + "</p>");
					}
				}


				//string[] Pasted = EachRow[0].Split(new string[] { "@#$" }, StringSplitOptions.None);
				return "	<table border='0' cellpadding='0' cellspacing='0' style='width:750px; margin:0 auto; ' ><thead>" +
												"		<tr style=\"height:40px;\">" +
												"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; width:170px;  \" >House BL</td>" +
												"		<td class='THead1' style=\"bold; width:170px;\" >금액</td>" +
												"		<td class='THead1' >Description</td>" +
												"		</tr></thead>" + ReturnValueTrue +
												"		<tr style=\"height:40px;\">" +
												"		<td class='THead1' style=\"background-color:#E8E8E8; text-align:center; font-weight:bold; width:170px;  \" >총 " + Count + "건</td>" +
												"		<td class='THead1' style=\"bold; width:170px;\" >총 " + Common.NumberFormat(Total.ToString()) + "원</td>" +
												"		<td class='THead1' >&nbsp;</td>" +
												"		</tr></table>" + (ReturnValueFalse + "" == "" ? "" : "<div><p>잘못된 BL번호</p>" + ReturnValueFalse + "</div>");
												*/
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