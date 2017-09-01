using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Member_MyCompanyView : System.Web.UI.Page
{
	public string RegionCode, CompanyName, PresidentName, CompanyNo, Email, CompanyAddress, TEL, FAX;
	public string SaupjaGubun, Homepage, Upmu57, Upmu58, Upmu59, upjong, uptae;
	public string Country;
	public string CompanyPk;
	protected String[] MEMBERINFO;
	protected String BTNSUBMIT;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] == null) { Response.Redirect("../Default.aspx"); }
		CompanyPk = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[1];
		MEMBERINFO = (Session["MemberInfo"] + "").Split(Common.Splite11, StringSplitOptions.None);
		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } } catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }
		List<string> AdditionalData = new Company().CompanyView(CompanyPk, out RegionCode, out CompanyName, out PresidentName, out CompanyNo, out TEL, out FAX, out Email, out CompanyAddress);
		char[] Splitdash = new char[] { '-' };
		string[] Split63 = new string[] { "^^^" };
		if (MEMBERINFO[4] == "President") {
			BTNSUBMIT = "<input type=\"button\" id=\"BTN_Submit\" style=\"width:130px; height:35px;\" onclick=\"GoModify()\" value=\"" + GetGlobalResourceObject("Member", "Submit") + "\"/>";
		} else {
			BTNSUBMIT = "";
		}
		if (RegionCode != "") {
			switch (RegionCode[0]) {
				case '1': Country = "Korea"; break;
				case '2': Country = "China"; break;
			}
		}
		string[] TempStringArray;
		string[] TempStringArray2;
		SaupjaGubun = string.Empty;
		Homepage = string.Empty;
		Upmu57 = string.Empty;
		Upmu58 = string.Empty;
		Upmu59 = string.Empty;
		char[] Split11 = new char[] { '!' };
		for (int i = 0; i < AdditionalData.Count; i++) {
			TempStringArray = AdditionalData[i].Split(Split63, StringSplitOptions.None);
			switch (TempStringArray[0]) {
				case "61":
					if (TempStringArray[1] == "55") { SaupjaGubun = GetGlobalResourceObject("Member", "BusinessType1") + ""; } else { SaupjaGubun = GetGlobalResourceObject("Member", "BusinessType2") + ""; }
					break;
				case "62": Homepage = TempStringArray[1]; break;
				case "63":
					if (TempStringArray[1].Length == 2) {
						switch (TempStringArray[1]) {
							case "57": Upmu57 = "checked=\"checked\""; break;
							case "58": Upmu58 = "checked=\"checked\""; break;
							case "59": Upmu59 = "checked=\"checked\""; break;
						}
					} else {
						TempStringArray2 = TempStringArray[1].Split(Split11, StringSplitOptions.RemoveEmptyEntries);
						for (int j = 0; j < TempStringArray2.Length; j++) {
							switch (TempStringArray2[j]) {
								case "57": Upmu57 = "checked=\"checked\""; break;
								case "58": Upmu58 = "checked=\"checked\""; break;
								case "59": Upmu59 = "checked=\"checked\""; break;
							}
						}
					}
					break;
				case "64":
					TempStringArray2 = TempStringArray[1].Split(Split11, StringSplitOptions.None);
					upjong = TempStringArray2[0];
					uptae = TempStringArray2[1];
					break;
			}
		}
	}
}