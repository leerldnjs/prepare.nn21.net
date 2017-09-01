using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Intro : System.Web.UI.Page
{
	public string Attention;
	public string RecentRequest;
	public string WelcomeMessage;
	protected String ExchangeRateHistory;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] == null) { Response.Redirect("../Default.aspx"); }
		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } } catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }

		SetRecentRequest(Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[1]);
		SetWelcomeMessage();
		SetExchangeRateHistory();
		Attention = "" + GetGlobalResourceObject("qjsdur", "wndmltkgkd") + "&nbsp;&nbsp;<a onclick=\"Attention();\">" + GetGlobalResourceObject("qjsdur", "wktpgl") + "</a>";
	}
	private void SetExchangeRateHistory() {
		DBConn DB = new DBConn();
		StringBuilder TableBody = new StringBuilder();
		string Now = DateTime.Now.ToString().Replace("-", string.Empty).Substring(0, 8);

		DB.DBCon.Open();
		DB.SqlCmd.CommandText = "	SELECT ExchangeRateStandard, MonetaryUnitFrom, MonetaryUnitTo, ExchangeRate " +
													"	FROM ExchangeRateHistory " +
													"	WHERE ETCSettingPk=1 and left(DateSpan, 8)<='" + Now + "' and right(DateSpan, 8)>='" + Now + "' " +
													"	ORDER BY MonetaryUnitFrom ASC, MonetaryUnitTo ASC, RegisteredDate DESC  ;";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string from = string.Empty;
		string to = string.Empty;
		while (RS.Read()) {
			TableBody.Append("<tr><td style='text-align:right; width:60px; height:30px; '>" + Common.GetMonetaryUnit(RS[1] + "") + " " + Common.NumberFormat(RS[0] + "") + "</td>" +
											"	   <td style='width:30px; text-align:center;'>=></td>" +
											"	   <td  style='text-align:left; width:90px;'>" + Common.GetMonetaryUnit(RS[2] + "") + " " + Common.NumberFormat(RS[3] + "") + "</td></tr>");
		}

		ExchangeRateHistory = "<table border='0' cellpadding='0' cellspacing='0' style=\"margin:0 auto;\" ><tr><td colspan=\"3\" style='font-size:15px; text-align:center; font-weight:bold; height:30px; '>금일 적용 환율</td></tr>" + TableBody + "</table>";
	}
	private void SetRecentRequest(string pk) {
		string Source = new RequestP().IntroRecentRequestList(pk);
		if (Source == "NN") { RecentRequest = string.Empty; } else {
			StringBuilder ReturnValue = new StringBuilder();
			//ReturnValue.Append("<table border='0' cellpadding='0' cellspacing='0' style=\"width:650px;\"><thead><tr height='30px'><td class='RecentRequestTHEAD' style='width:250px;'>");
			ReturnValue.Append("<table border='0' cellpadding='0' cellspacing='0' style=\"width:800px;\"><thead><tr height='30px'>");
			ReturnValue.Append("<td class='RecentRequestTHEAD' style='width:40px; text-align:left;'>No</td>");
			ReturnValue.Append("<td class='RecentRequestTHEAD' style='width:60px; text-align:left;'>" + GetGlobalResourceObject("qjsdur", "ehckrdPwjd") + "</td>");
			ReturnValue.Append("<td class='RecentRequestTHEAD' style='width:250px;'>" + GetGlobalResourceObject("Member", "HowLong") + " (" + GetGlobalResourceObject("Member", "Area") + ")</td>");
			ReturnValue.Append("<td class='RecentRequestTHEAD' style='width:70px;'>" + GetGlobalResourceObject("Member", "TransportWay") + "</td>");
			ReturnValue.Append("<td class='RecentRequestTHEAD' >" + GetGlobalResourceObject("Member", "Customer") + " (" + GetGlobalResourceObject("Member", "TEL") + ")</td>");
			ReturnValue.Append("<td class='RecentRequestTHEAD' style='width:70px;'>" + GetGlobalResourceObject("Member", "RequestDate") + "</td>");
			//ReturnValue.Append("<td class='RecentRequestTHEAD' style='width:90px;'>" + GetGlobalResourceObject("Member", "Step") + "</td></tr></thead>");

			string[] SourceRow = Source.Split(new string[] { "####" }, StringSplitOptions.RemoveEmptyEntries);
			string[] SourceEach;
			string[] Split22 = new string[] { "@@" };

			string strPageNo = Request.Params["pageNo"] + "";
			int PageNo = 1;
			int PageLimit = 30;
			if (strPageNo != "") {
				PageNo = Int32.Parse(strPageNo);
			}

			int StartPage = 0;
			if (PageNo > 1) {
				StartPage += (PageNo - 1) * PageLimit;
			}

			int EndPage = StartPage + PageLimit;
			if (EndPage > SourceRow.Length) {
				EndPage = SourceRow.Length;
			}


			for (int i = StartPage; i < EndPage; i++) {
				SourceEach = SourceRow[i].Split(Split22, StringSplitOptions.None);
				string cssClass = "TBody1G";
				if (SourceEach[9] == "64" || SourceEach[9] == "65") {
					cssClass = "RecentRequestTBODY";
				}

				int No = EndPage - i;
				ReturnValue.Append("<tr height='35px'>");
				ReturnValue.Append("<td style=\"text-align:left;\" class='" + cssClass + "'>" + No + "</td>");
				ReturnValue.Append("<td class='" + cssClass + "'>" + SourceEach[12] + "</td>");
				ReturnValue.Append("<td class='" + cssClass + "'><a href='../Request/RequestFormView.aspx?pk=" + SourceEach[0] + "'>");
				ReturnValue.Append("'" + SourceEach[1] + ". (" + SourceEach[2] + ") ~ " + "'" + SourceEach[3] + ". (" + SourceEach[4] + ")</a></td>");
				string temp = Common.GetTransportWay(SourceEach[5]);

				ReturnValue.Append("<td class='" + cssClass + "'>" + temp + "</td>");
				if (SourceEach[10] == pk) {
					ReturnValue.Append("<td class='" + cssClass + "' >" + SourceEach[6] + "<br />(" + SourceEach[7] + ")</td>");
				} else {
					ReturnValue.Append("<td class='" + cssClass + "' >" + SourceEach[11] + "</td>");
				}

				ReturnValue.Append("<td class='" + cssClass + "' >" + SourceEach[8] + "</td>");
				switch (SourceEach[9]) {
					case "50": temp = "<span style='color:green;'>" + GetGlobalResourceObject("qjsdur", "wjqtndPdir") + "</span>"; break;
					case "51": temp = "<span style='color:green;'>" + GetGlobalResourceObject("qjsdur", "wjqtndPdirdhksfy") + "</span>"; break;
					case "52": temp = "<span style='color:green;'>" + GetGlobalResourceObject("qjsdur", "wjqtnqhfb") + "</span>"; break;
					case "53": temp = "<span style='color:green;'>" + GetGlobalResourceObject("qjsdur", "vlrdjqdPdir") + "</span>"; break;
					case "54": temp = "<span style='color:green;'>" + GetGlobalResourceObject("qjsdur", "ghkanfwjqtndhksfy") + "</span>"; break;
					case "55": temp = "<span style='color:green;'>화물 계량 완료</span>"; break;
					case "56": temp = "<span style='color:green;'>일정 확정</span>"; break;
					case "57": temp = "<span style='color:green;'>화물접수 완료</span>"; break;
					case "58": temp = "<span style='color:green;'>운송진행중 <br /> 운임확정 완료</span>"; break;
					case "64": temp = "<span style='color:black;'>배송완료</span>"; break;
					case "65": temp = "<span style='color:black;'>출고완료</span>"; break;
				}
				//ReturnValue.Append("<td class='RecentRequestTBODY' ><a href='../Request/RequestFormView.aspx?pk=" + SourceEach[0] + "'>" + temp + "</a></td></tr>");
			}

			RecentRequest = ReturnValue + "<tr height='10px'><td colspan='9' >&nbsp;</td></tr><TR Height='20px'><td colspan='9' style='background-color:#F5F5F5; text-align:center; padding:20px; '>" +
				new Common().SetPageListByNo(PageLimit, PageNo, SourceRow.Length, "Intro.aspx", "?") + "</TD></TR></Table>";
		}
	}

	private void SetWelcomeMessage() {
		if (Application["IntroMessage"] == null) { Application["IntroMessage"] = 0; }
		switch ((int)Application["IntroMessage"]) {
			case 0: WelcomeMessage = GetLocalResourceObject("IntroAdd1") + ""; break;
			case 1: WelcomeMessage = GetLocalResourceObject("IntroAdd2") + ""; break;
			case 2: WelcomeMessage = GetLocalResourceObject("IntroAdd3") + ""; break;
			case 3: WelcomeMessage = GetLocalResourceObject("IntroAdd4") + ""; break;
		}
		Application["IntroMessage"] = Int32.Parse(Application["IntroMessage"].ToString()) + 1;
		if ((int)Application["IntroMessage"] == 4) {
			Application["IntroMessage"] = 0;
		}
	}
}