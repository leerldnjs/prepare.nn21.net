using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Admin_ExchangeRate : System.Web.UI.Page
{
	private DBConn DB;
	private String SelectedTitle;
	protected StringBuilder ExchangeRateListTableBody;
	protected StringBuilder St_ExchangeRateTitle;
	protected String TODAY;
	protected String Paging;
	protected String[] MemberInfo;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] == null) { Response.Redirect("../Default.aspx"); }

		SelectedTitle = Request.Params["S"] + "" == "" ? "1" : Request.Params["S"];
		string stringPageNo=Request.Params["pageNo"];
		int intPageNo=1; 
		if (stringPageNo !=null){
			intPageNo=Int32.Parse(stringPageNo);
		}
		LoadExchangeRateTitle(SelectedTitle);
		LoadExchangeRateHistory(SelectedTitle, Request.Params["F"] + "", Request.Params["T"] + "", intPageNo);
		TODAY = DateTime.Now.Date.ToShortDateString().Replace("-", "");
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		if (MemberInfo[0] == "Customs") {
			Loged1.Visible = false;
			Loged2.Visible = true;
		}
	}
	private void LoadExchangeRateTitle(string selectedtitle)
	{
		DB = new DBConn();
		St_ExchangeRateTitle = new StringBuilder();
		St_ExchangeRateTitle.Append("<select id=\"St_ExchangeRateTitle\" size=\"1\" onchange=\"SelectTitleNMonetaryUnit();\" >");
		DB.SqlCmd.CommandText = "SELECT [ETCSettingPk], [Value] FROM ETCSetting WHERE GubunCL=0";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			//Response.Write(RS[0] + "");
			if (RS[0] + "" == selectedtitle) {
				St_ExchangeRateTitle.Append("<option value=\"" + RS[0] + "\" selected=\"selected\" >" + RS[1] + "</option>");
			} else {
				St_ExchangeRateTitle.Append("<option value=\"" + RS[0] + "\">" + RS[1] + "</option>");
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		St_ExchangeRateTitle.Append("</select>");
	}
	private void LoadExchangeRateHistory(string selectedtitle, string selectedfrom, string selectedto, int PageNo)
	{
		DB = new DBConn();
		ExchangeRateListTableBody = new StringBuilder();
		string Where = "WHERE ETCSettingPk=" + SelectedTitle;

		if (selectedfrom != "" && selectedfrom != "0") { Where += " and MonetaryUnitFrom=" + selectedfrom; }
		if (selectedto != "" && selectedto != "0") { Where += " and MonetaryUnitTo=" + selectedto; }

		Int32 pageLength = 50;
		Int32 totlaRecord;

		DB.SqlCmd.CommandText = "SELECT COUNT(*) " +
													  "FROM ExchangeRateHistory " +
														Where + "; ";
		DB.DBCon.Open();
		totlaRecord = (Int32)DB.SqlCmd.ExecuteScalar();
		
		DB.SqlCmd.CommandText = "SELECT [ExchangeRatePk], [DateSpan], [ETCSettingPk], [ExchangeRateStandard], [MonetaryUnitFrom], [MonetaryUnitTo], [ExchangeRate] " +
													  "FROM ExchangeRateHistory " +
														Where + " ORDER BY DateSpan DESC";
		//Response.Write(DB.SqlCmd.CommandText);
	
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		for (int i = 0; i < (PageNo - 1) * pageLength; i++) {
			if (RS.Read()) {
				continue;
			} else {
				break;
			}
		}
		for (int i = 0; i < pageLength; i++) {
			if (RS.Read()) {
				if (Session["Type"]+"" == "ShippingBranch") {
					ExchangeRateListTableBody.Append("<tr>" +
																			"<td class=\"TBody1\">" + RS["DateSpan"] + "</td>" +
																			"<td class=\"TBody1\">" + Common.GetMonetaryUnit(RS["MonetaryUnitFrom"] + "") + " " + RS["ExchangeRateStandard"] + "</td>" +
																			"<td class=\"TBody1\">" + Common.GetMonetaryUnit(RS["MonetaryUnitTo"] + "") + " " + Common.NumberFormat(RS["ExchangeRate"] + "") + "</td>" +
																			"<td class=\"TBody1\">&nbsp;</td>" +
																			"<td class=\"TBody1\">&nbsp;</td>" +
																		"</tr>");

				} else {
					ExchangeRateListTableBody.Append("<tr>" +
																			"<td class=\"TBody1\">" + RS["DateSpan"] + "</td>" +
																			"<td class=\"TBody1\">" + Common.GetMonetaryUnit(RS["MonetaryUnitFrom"] + "") + " " + RS["ExchangeRateStandard"] + "</td>" +
																			"<td class=\"TBody1\">" + Common.GetMonetaryUnit(RS["MonetaryUnitTo"] + "") + " " + Common.NumberFormat(RS["ExchangeRate"] + "") + "</td>" +
																			"<td class=\"TBody1\"><input type=\"button\" onclick=\"BTN_Modify('" + RS[0] + "');\" value=\"Modify\" /></td>" +
																			"<td class=\"TBody1\"><input type=\"button\" onclick=\"BTN_Delete('" + RS[0] + "');\" value=\"Delete\" /></td>" +
																		"</tr>");

				}
			} else {
				break;
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		Paging = new Common().SetPageListByNo(pageLength, PageNo, totlaRecord, "ExchangeRate.aspx?G=V&S=" + SelectedTitle + "&F=" + selectedfrom + "&T=" + selectedto + "&", "");
	}
}