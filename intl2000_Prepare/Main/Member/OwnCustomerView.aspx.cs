using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Member_OwnCustomerView : System.Web.UI.Page
{
	public String CompanyCode;
	public String CompanyName;
	public String Memo;
	protected String TargetPk;

	protected StringBuilder RequestList;
	protected String GubunCL;
	protected String CompanyInformation;
	protected String CompanyFAX;
	private DBConn DB;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Request["pk"] == "" || Request["pk"] == null || Session["MemberInfo"] + "" == "")
		{
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

		string companyfax = string.Empty;
		string Result = OwnCustomerView(Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[1], Request["pk"], ref CompanyCode, ref CompanyName, ref Memo);
		
		if (Result == "-1") { Response.Redirect("./Intro.aspx"); }
		if (CompanyCode == "") { CompanyCode = GetGlobalResourceObject("RequestForm", "NEW")+""; }
	}
	private String OwnCustomerView(string MyPk, string pk, ref string CompanyCode, ref string CompanyName, ref string Memo)
	{
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"select 
															C.CompanyPk, C.CompanyCode , C.CompanyTEL, C.CompanyFAX, C.PresidentName, C.PresidentEmail, C.GubunCL
															, CR.CompanyRelationPk, CR.TargetCompanyPk, CR.TargetCompanyNick, CR.Memo 
														from CompanyRelation as CR 
															left join Company as C on CR.TargetCompanyPk=C.CompanyPk 
														where CR.CompanyRelationPk=" + pk + " and CR.MainCompanyPk=" + MyPk;	//뒤에 ListOwnerCompanyPk를 넣은건 자기가 등록한게 맞는지 확인하려고...
		//Response.Write(DB.SqlCmd.CommandText);
		string QueryWhere;
		int TargetCompanyPk;
		DB.DBCon.Open();
		SqlDataReader SDR = DB.SqlCmd.ExecuteReader();
		if (SDR.Read()) {
			GubunCL = SDR["GubunCL"] + "";
			TargetPk = SDR["CompanyPk"] + "";
			if (GubunCL== "0") {
				CompanyInformation = "<ul style=\"line-height:30px;\"><li>" + GetGlobalResourceObject("Member", "CompanyCode") + " : " + SDR["CompanyCode"] +
					"</li><li>" + GetGlobalResourceObject("Member", "CompanyName") + " : " + SDR["TargetCompanyNick"] +
					"</li><li>TEL : " + SDR["CompanyTEL"] +
					"</li><li>FAX : " + SDR["CompanyFAX"] +
					"</li><li>" + GetGlobalResourceObject("Member", "PresidentName") + " : " + SDR["PresidentName"] +
					"</li><li>E-mail : " + SDR["PresidentEmail"] + "</li></ul>";
			} else {
				CompanyInformation = "<ul style=\"line-height:30px;\"><li>" + GetGlobalResourceObject("Member", "CompanyCode") + " : " + SDR["CompanyCode"] +
					"</li><li>" + GetGlobalResourceObject("Member", "CompanyName") + " : " + SDR["TargetCompanyNick"] +
					"</li><li>TEL : <input type=\"text\" id=\"TB_TEL\" value=\"" + SDR["CompanyTEL"] + "\" />" +
					"</li><li>FAX : <input type=\"text\" id=\"TB_FAX\" value=\"" + SDR["CompanyFAX"] + "\" />" +
					"</li><li>" + GetGlobalResourceObject("Member", "PresidentName") + " : <input type=\"text\" id=\"TB_PresidentName\" style=\"width:100px;\" value=\"" + SDR["PresidentName"] + "\" />" +
					"</li><li>E-mail : <input type=\"text\" id=\"TB_Email\" style=\"width:190px;\" value=\"" + SDR["PresidentEmail"] + "\" /></li></ul>";
			}

			CompanyCode = SDR["CompanyCode"] + "";
			CompanyName = SDR["TargetCompanyNick"] + "";
			Memo = SDR["Memo"] + "";

			if (CompanyCode == "") {
				TargetCompanyPk = 0;
				QueryWhere = "R.ShipperPk in (" + MyPk + ", " + pk + ") and R.ConsigneeCCLPk in (" + MyPk + ", " + pk + ")";
			} else {
				TargetCompanyPk = (int)SDR["TargetCompanyPk"];
				QueryWhere = "R.ShipperPk in (" + MyPk + ", " + TargetCompanyPk + ") and R.ConsigneePk in (" + MyPk + ", " + TargetCompanyPk + ")";
			}
			SDR.Dispose();

			RequestList = new StringBuilder();
			RequestList.Append("<table border='0' cellpadding='0' cellspacing='0' width=680><thead><tr height='30px'><td class='RecentRequestTHEAD' style='width:40px;'>Type</td><td class='RecentRequestTHEAD' >" +
			GetGlobalResourceObject("Member", "HowLong") + " (" + GetGlobalResourceObject("Member", "Area") + ")</td>" +
			"<td class='RecentRequestTHEAD' style='width:80px;'>" + GetGlobalResourceObject("Member", "TransportWay") + "</td>" +
			"<td class='RecentRequestTHEAD' style='width:75px;'>" + GetGlobalResourceObject("RequestForm", "PackingCount") + "</td>" +
			"<td class='RecentRequestTHEAD' style='width:90px;'>" + GetGlobalResourceObject("RequestForm", "GrossWeight") + "</td>" +
			"<td class='RecentRequestTHEAD' style='width:90px;'>" + GetGlobalResourceObject("RequestForm", "Volume") + "</td>" +
			"<td class='RecentRequestTHEAD' style='width:70px;'>" + GetGlobalResourceObject("Member", "Step") + "</td></tr></thead>");

			DB.SqlCmd.CommandText = @"SELECT TOP 20 
											R.RequestFormPk, R.ShipperPk, R.DepartureDate, R.ArrivalDate, R.TransportWayCL, R.PaymentWhoCL, R.StepCL, R.RequestDate
											, R.[TotalPackedCount], R.[PackingUnit], R.[TotalGrossWeight], R.[TotalVolume]
											, DR.Name AS DName, AR.Name AS AName 
										FROM RequestForm as R 
											left join RegionCode as DR On DR.RegionCode=R.DepartureRegionCode 
											left join RegionCode as AR On AR.RegionCode=R.ArrivalRegionCode 
										WHERE " + QueryWhere + " and R.StepCL>49 " +
										" ORDER BY RequestFormPk desc;";
			SDR = DB.SqlCmd.ExecuteReader();
			StringBuilder Result = new StringBuilder();

			string RowFormat = "<tr height='20px'>"+
				"<td class='RecentRequestTBODY'><span style='color:{0};'>{2}</span></td>"+
				"<td class='RecentRequestTBODY'><a href='../Request/RequestFormView.aspx?pk={1}'>{3}</a></td>"+
				"<td class='RecentRequestTBODY'>{4}</td>"+
				"<td class='RecentRequestTBODY'>{5}</td>"+
				"<td class='RecentRequestTBODY'>{6}</td>"+
				"<td class='RecentRequestTBODY'>{7}</td>"+
				"<td class='RecentRequestTBODY'><a href='../Request/RequestFormView.aspx?pk={1}'>{8}</a></td></tr>";

			while (SDR.Read()) {
				string step;
				switch (SDR["StepCL"] + "") {
					case "50":
						step = "<span style='color:green;'>" + GetGlobalResourceObject("qjsdur", "wjqtndPdir") + "</span>";
						break;
					case "51":
						step = "<span style='color:green;'>" + GetGlobalResourceObject("qjsdur", "wjqtndPdirdhksfy") + "</span>";
						break;
					case "52":
						step = "<span style='color:green;'>" + GetGlobalResourceObject("qjsdur", "wjqtnqhfb") + "</span>";
						break;
					case "53":
						step = "<span style='color:green;'>" + GetGlobalResourceObject("qjsdur", "vlrdjqdPdir") + "</span>";
						break;
					case "54":
						step = "<span style='color:green;'>" + GetGlobalResourceObject("qjsdur", "ghkanfwjqtndhksfy") + "</span>";
						break;
					case "55":
						step = "<span style='color:green;'>화물 계량 완료</span>";
						break;
					case "56":
						step = "<span style='color:green;'>일정 확정</span>";
						break;
					case "57":
						step = "<span style='color:green;'>화물접수 완료</span>";
						break;
					case "58":
						step = "<span style='color:green;'>운송진행중 <br /> 운임확정 완료</span>";
						break;
					case "64":
						step = "<span style='color:black;'>배송완료</span>";
						break;
					case "65":
						step = "<span style='color:black;'>출고완료</span>";
						break;
					default:
						step = "??";
						break;
				}
				
				String[] RowData = new string[]{
					SDR["ShipperPk"]+""==MyPk?"red":"blue", 
					SDR["RequestFormPk"]+"", 
					SDR["ShipperPk"]+""==MyPk?"OUT": "IN", 
					(SDR["DepartureDate"]+"").Substring(2, 6) + "(" + SDR["DName"] + ") ~ " + (SDR["ArrivalDate"]+"").Substring(2, 6) + "(" + SDR["AName"]+ ")", 
					Common.GetTransportWay(SDR["TransportWayCL"]+""), 
					SDR["TotalPackedCount"]+Common.GetPackingUnit(SDR["PackingUnit"]+""), 
					Common.NumberFormat(SDR["TotalGrossWeight"]+"")+"Kg", 
					Common.NumberFormat(SDR["TotalVolume"]+"")+"CBM", 
					step};

				RequestList.Append(String.Format(RowFormat, RowData));
			}
			DB.DBCon.Close();
			return Result + "";
		} else {
			DB.DBCon.Close();
			return "-1";//거래내역은 없음
		}
	}	//Member/OwnCustomerView.aspx 상세보기
}