using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Admin_CompanyInfoDocumentList : System.Web.UI.Page
{
	protected String[] MEMBERINFO;
	protected StringBuilder HtmlList;
	protected String CompanyPk;
	protected String CompanyName;
	protected DBConn DB;
	private int PageNo;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("../Default.aspx");
		}
		try {
			if (Request["Language"].Length == 2) {
				Session["Language"] = Request["Language"];
			}
		} catch (Exception) {
		}
		switch (Session["Language"] + "") {
			case "en":
				Page.UICulture = "en";
				break;
			case "ko":
				Page.UICulture = "ko";
				break;
			case "zh":
				Page.UICulture = "zh-cn";
				break;
		}

		MEMBERINFO = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		DB = new DBConn();
		CompanyPk = MEMBERINFO[1];
		try {
			PageNo = Int32.Parse(Request["PageNo"]);
		} catch (Exception) {
			PageNo = 1;
		}

		if (string.IsNullOrEmpty(Request["S"])) {
			LoadRequestList(Request["G"], MEMBERINFO[1]);
		} else {
			LoadRequestList(Request["G"], Request["S"]);
		}
	}
	private void LoadRequestList(string Gubun, string MemberPk)
	{

		CompanyPk = MemberPk;

		//Gubun
		//5051		접수완료 관리자 읽지않음 / 읽음
		//5253		접수보류 / 픽업예약
		//5455		입고됨 중량, 체적 입력안됨 / 중량, 체적 입력완료
		HtmlList = new StringBuilder();
		Int32 pageLength = 20;
		Int32 totlaRecord;

		HtmlList.Append("<table border='0' cellpadding='0' cellspacing='0' style='width:850px;' ><thead><tr height='30px'>" +
						"<td class='THead1' style='width:150px;' >" + GetGlobalResourceObject("qjsdur", "wkrtjddlf") + "</td>" +
						"<td class='THead1' style='width:150px;' >Shipper</td>" +
						"<td class='THead1' style='width:150px;' >Consignee</td>" +
						"<td class='THead1' style='width:150px;' >" + GetGlobalResourceObject("qjsdur", "wpvnaaud") + "</td>" +
						"<td class='THead1' ></td>" +
					"</tr></thead>" + DocumentListLoadByAdmin(MemberPk, pageLength, PageNo, out totlaRecord));

		HtmlList.Append("<tr height='10px'><td colspan='9' >&nbsp;</td></tr><TR Height='20px'><td colspan='9' style='background-color:#F5F5F5; text-align:center; padding:20px; '>" + new Common().SetPageListByNo(pageLength, PageNo, totlaRecord, "CompanyInfoRequestList.aspx", "?G=CI&S=" + MemberPk + "&") + "</TD></TR></Table>");
	}
	public String DocumentListLoadByAdmin(string CompanyPk, int pageLength, int pageNo, out int totlaRecord)
	{
		DBConn DB = new DBConn();
		StringBuilder ReturnValue = new StringBuilder();
		StringBuilder BTN_ConsigneeCharge = new StringBuilder();
		GetQuery GQ = new GetQuery();
		DB.DBCon.Open();

		string TableInnerRow = "";

		DB.SqlCmd.CommandText = "	select	(select count(*) from newCommercialDocument where CompanyPk=" + CompanyPk + ")";
													  //"             (select count(*) from newTradingSchedule where CompanyPk=" + CompanyPk + ")";
		
		totlaRecord = (Int32)DB.SqlCmd.ExecuteScalar();
		if (totlaRecord == 0) {
			return "";
		}
		DB.SqlCmd.CommandText = "EXEC SP_DocumentListByEachCompany @CompanyPk=" + CompanyPk + ";";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		String StyleID;
		String TempDocumentFormPk = string.Empty;
		StringBuilder Temp = new StringBuilder();
		//bool IsFirst = true;
		int k = 0;
		while (k < (pageNo - 1) * pageLength) {
			if (RS.Read()) {
				if (TempDocumentFormPk != RS[0] + "") {
					TempDocumentFormPk = RS[0] + "";
					k++;
				}
			} else {
				break;
			}
		}
		for (int i = 0; i < pageLength; i++) {
			if (RS.Read()) {
				if (TempDocumentFormPk != RS[0] + "") {
					string PackedCount = RS["PackedCount"] + "";
					string PackingUnit = RS["PackingUnit"] + "";
					string GrossWeight = RS["GrossWeight"] + "";
					string UnitPrice = RS["UnitPrice"] + "";
					string Amount = RS["Amount"] + "";
					if (UnitPrice != "" || Amount != "") {
						BTN_ConsigneeCharge.Append("<input type=\"button\" onclick=\"GoClearance('ShowInvoice', '{1}')\" value=\"Invoice\" /> ");
					}
					if (PackedCount != "" || PackingUnit != "" || GrossWeight != "") {
						BTN_ConsigneeCharge.Append("<input type=\"button\" onclick=\"GoClearance('ShowPacking','{1}')\" value=\"Packing\" /> ");
						//"<input type=\"button\" onclick=\"GoClearance('ShowPacking','{1}')\" value=\"Packing\" /> " +
					}
					if (PackedCount == "" && PackingUnit == "" && GrossWeight == "" && UnitPrice == "" && Amount == "") {
						BTN_ConsigneeCharge.Append("<input type=\"button\" onclick=\"GoClearance('ShowTrade','{1}')\" value=\"Trade\" /> ");
					}
					
					BTN_ConsigneeCharge.Append("<input type=\"button\" style=\"color:Red;\" onclick=\"DeleteDocument('{1}')\" value=\"Delete\" /> ");

					TableInnerRow = "<tr>" +
						//"<td class='{0}'><a href=\"../Request/DocumentWrite.aspx?&pk={1} \">{2}</a></td>" +
														"<td class='{0}'>{2}</a></td>" +
														"<td class='{0}'>{3}</td>" +
														"<td class='{0}'>{4}</td>" +
														"<td class='{0}'>{5}</td>" +
														"<td class='{0}'>" + BTN_ConsigneeCharge + "</td>" +
														"</tr>";
					BTN_ConsigneeCharge = new StringBuilder();
					TempDocumentFormPk = RS[0] + "";
					StyleID = "TBody1G";
					string[] TableInnerData = new string[6];
					TableInnerData[0] = StyleID;
					TableInnerData[1] = TempDocumentFormPk;
					TableInnerData[2] = RS["Registerd"].ToString().Substring(0, 10);
					TableInnerData[3] = RS["Shipper"] + "";
					TableInnerData[4] = RS["Consignee"] + "";
					TableInnerData[5] = RS["Description"] + "" == "" ? ((RS["Description2"] + "").Length > 11 ? (RS["Description2"] + "").Substring(0, 10) + "..." : RS["Description2"] + "") : ((RS["Description"] + "").Length > 11 ? (RS["Description"] + "").Substring(0, 10) + "..." : RS["Description"] + "");

					ReturnValue.Append(string.Format(TableInnerRow, TableInnerData));
					continue;
				} else {
					i--;
					continue;
				}
			} else {
				break;
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return ReturnValue + "";
	}
}