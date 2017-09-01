using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Request_newTradingScheduleWirte : System.Web.UI.Page
{
	protected String[] MemberInfo;
	protected String[] SubInfo;
	protected String Shipper;
	protected String TODAY;
	public string RegionCode, CompanyName, PresidentName, CompanyNo, Email, CompanyAddress, TEL, FAX;
	public string SaupjaGubun, Homepage, Upmu57, Upmu58, Upmu59, upjong, uptae;
	public string CompanyPk;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] == null || Session["MemberInfo"] + "" == "") { Response.Redirect("~/Default.aspx"); }
		CompanyPk = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[1];
		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } } catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }

		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		SubInfo = Session["SubInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		Shipper = string.Empty;
		TODAY = DateTime.Now.Date.ToShortDateString().Replace("-", "");
		List<string> AdditionalData = new Company().CompanyView(CompanyPk, out RegionCode, out CompanyName, out PresidentName, out CompanyNo, out TEL, out FAX, out Email, out CompanyAddress);
		char[] Splitdash = new char[] { '-' };
		string[] Split63 = new string[] { "^^^" };
		string[] TempStringArray;
		string[] TempStringArray2;
		char[] Split11 = new char[] { '!' };
		for (int i = 0; i < AdditionalData.Count; i++) {
			TempStringArray = AdditionalData[i].Split(Split63, StringSplitOptions.None);
			if (TempStringArray[0] == "64") {
				TempStringArray2 = TempStringArray[1].Split(Split11, StringSplitOptions.None);
				upjong = TempStringArray2[0];
				uptae = TempStringArray2[1];
			}
		}
	}
}
