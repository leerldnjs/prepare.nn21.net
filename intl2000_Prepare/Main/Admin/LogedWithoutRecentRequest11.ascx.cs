using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_LogedWithoutRecentRequest11 : System.Web.UI.UserControl
{
	public String CustomerCode;
	public String RecentRequest;
	protected String[] MEMBERINFO;
	protected String FinanceOnly;
	protected String FinanceButton;
	protected String ButtonRight;
	protected String SerchBL;
	protected void Page_Load(object sender, EventArgs e) {
		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } } catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }

		try { MEMBERINFO = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None); } catch (Exception) { Response.Redirect("../Default.aspx"); }


		if (MEMBERINFO[2] == "ilyw0" || MEMBERINFO[2] == "ilgz0" || MEMBERINFO[2] == "ilyt0" || MEMBERINFO[2] == "ilman" || MEMBERINFO[2] == "ilic30" || MEMBERINFO[2] == "ilogistics") {
			if (IsPostBack) { SelectBranch.Visible = true; } else {
				SelectBranch.Visible = true;
				SelectBranch.SelectedValue = MEMBERINFO[1];
			}
		}

		CustomerCode = MEMBERINFO[2];
		//통관,미수등 오른쪽 버튼들///////////////////
		LogOut.Text = GetGlobalResourceObject("Member", "LogOut") + "";


		if (Session["Type"] + "" == "ShippingBranch") {
			ButtonRight += "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "ghksdbfwjdqh") + "\" onclick=\"location.href = '../Admin/ExchangeRate.aspx?G=V';\" style=\" height:25px; font-size:12px; \" /> " + "<input type=\"button\" value=\"" + GetGlobalResourceObject("Member", "InfoCustomer") + "\" onclick=\"location.href = '../Admin/CompanyList.aspx';\" style=\"height:25px; font-size:12px; \" /> " + "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "djqcpemdfhr") + "\" style=\" height:25px;font-size:12px;\" onclick=\"GotoForShippingBranch('NewCompany', '" + Session["CompanyPk"] + "');\" />";
		} else {

			if (MEMBERINFO[1] + "" == "3157") {
				//ButtonRight = "<input type=\"button\" value=\"통관\" onclick=\"location.href = '../Admin/StorageOutForCustoms.aspx'\" style=\" height:25px; font-size:12px; \"/>&nbsp;&nbsp;";
				ButtonRight = "<input type=\"button\" value=\"세납\" onclick=\"location.href = '../Admin/Taxpaid.aspx'\" style=\" height:25px; font-size:12px; \"/>&nbsp;";
			}
			ButtonRight += "<input type=\"button\" value=\"未\" onclick=\"location.href = '../Finance/SimpleMisu.aspx';\" style=\"background-color:#ffcccc;\" /> " +
								"<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "ghksdbfwjdqh") + "\" onclick=\"location.href = '../Admin/ExchangeRate.aspx?G=V';\" style=\" height:25px; font-size:12px; \" /> " +
								"<input type=\"button\" value=\"" + GetGlobalResourceObject("Member", "InfoCustomer") + "\" onclick=\"location.href = '../Admin/CompanyList.aspx';\" style=\"height:25px; font-size:12px; \" /> " +
								"<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "djqcpemdfhr") + "\" style=\" height:25px;font-size:12px;\" onclick=\"AddNewCustomer();\" />";
			SerchBL = "<option value=\"SerchBL\">BLNo</option>";


			FinanceButton = "";

			//수입서류
			if (MEMBERINFO[1] == "3157" || MEMBERINFO[1] == "2886" || MEMBERINFO[1] == "2887" || MEMBERINFO[1] == "3843" || MEMBERINFO[1] == "3388" || MEMBERINFO[1] == "11456" || MEMBERINFO[2] == "ilyw0") {
				FinanceButton += "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "tndlqtjfb") + "\" onclick=\"location.href='../UploadedFiles/FileList.aspx?G=4&T=&V=&pageNo=1'\" />&nbsp;";
			}
			//매출집계
			if (MEMBERINFO[2] == "ilic00" || MEMBERINFO[2] == "ilic01" || MEMBERINFO[2] == "ilic55" || MEMBERINFO[2] == "ilic66" || MEMBERINFO[2] == "ilic77" || MEMBERINFO[2] == "ilman" || MEMBERINFO[2] == "ilogistics" || MEMBERINFO[2] == "ilic30" || MEMBERINFO[2] == "ilic31" || MEMBERINFO[2] == "ilic32" || MEMBERINFO[2] == "ilyw0") {
				FinanceButton += "<input type=\"button\" value=\"매출집계\" onclick=\"location.href='../Finance/Sales.aspx'\" />&nbsp;";
			}
			//통관내역
			if (MEMBERINFO[2] == "ilic00" || MEMBERINFO[2] == "ilic01" || MEMBERINFO[2] == "ilic03" || MEMBERINFO[2] == "ilic55" || MEMBERINFO[2] == "ilic66" || MEMBERINFO[2] == "ilic77" || MEMBERINFO[2] == "ilman" || MEMBERINFO[2] == "ilogistics" || MEMBERINFO[2] == "ilic30" || MEMBERINFO[2] == "ilic31" || MEMBERINFO[2] == "ilic32") {
				FinanceButton += "<input type=\"button\" value=\"통관내역\" onclick=\"location.href='../Finance/ClearanceList.aspx'\" />&nbsp;";
			}
			//지사간운송
			if (MEMBERINFO[1] == "3157" || MEMBERINFO[2] == "ilyw0") {
				FinanceButton += "<input type=\"button\" value=\"지사간 운송\" onclick=\"location.href='../Finance/TransportBB.aspx'\" />&nbsp;";
			}
			//배송비
			if (MEMBERINFO[1] == "3157") {
				FinanceButton += "<input type=\"button\" value=\"배송비\" onclick=\"location.href='../Finance/DeliveryCharge.aspx'\" />&nbsp;";
			}
			//입금내역
			if (MEMBERINFO[1] == "3157" || MEMBERINFO[2] == "ilgz0" || MEMBERINFO[2] == "ilyt0") {
				FinanceButton += "<input type=\"button\" value=\"입금내역\" onclick=\"location.href='../Charge/ChargeList.aspx'\" />&nbsp;";
			}
			//지사관리
			if (MEMBERINFO[2] == "ilic00" || MEMBERINFO[2] == "ilic01" || MEMBERINFO[2] == "ilic66" || MEMBERINFO[2] == "ilic31" || MEMBERINFO[2] == "ilic32" || MEMBERINFO[2] == "ilman") {
				FinanceButton += "<input type=\"button\" value=\"지사관리\" onclick=\"location.href='../Admin/BranchList.aspx'\" />&nbsp;";
			}
			//if (MEMBERINFO[1] == "3157")
			//{
			//    FinanceButton += "<input type=\"button\" value=\"거래처\" onclick=\"location.href='../Admin/CompanyList_Search.aspx'\" />&nbsp;";
			//}

			FinanceOnly = "<tr>" +
						"<td style=\"color:#FAEBD7; font-weight:bold; font-size:15px; text-align:right; \">Finance Only</td>" +
						"<td colspan=\"2\">&nbsp;&nbsp;" +
						FinanceButton +
						"</td></tr>";
		}
	}

	protected void Button1_Click(object sender, EventArgs e) {
		HttpCookie aCookie;
		string cookieName;
		int limit = Request.Cookies.Count;
		for (int i = 0; i < limit; i++) {
			cookieName = Request.Cookies[i].Name;
			aCookie = new HttpCookie(cookieName);
			aCookie.Expires = DateTime.Now.AddDays(-1);
			Response.Cookies.Add(aCookie);
		}

		Response.Cookies["IL"]["MemberInfo"] = null;
		Response.Cookies["IL"]["SubInfo"] = null;
		Response.Cookies["IL"]["Gubun"] = null;
		Response.Cookies["IL"].Expires = DateTime.Now.AddDays(-1);

		Session["MemberInfo"] = null;
		Session["SubInfo"] = null;
		Response.Redirect("../Default.aspx");
	}
	protected void Btn_GoSerch_Click(object sender, EventArgs e) {
		if (Request.Params["SerchValue"].Trim() == "") {
			Response.Redirect("../Admin/CompanyList.aspx");
		} else {
			if (Request.Params["HSerchOption"] == "SerchBL") {
				Response.Redirect("../Admin/CheckDescriptionList.aspx?Type=SerchBL&Value=" + Request.Params["SerchValue"]);
			} else {
				Response.Redirect("../Admin/CompanyList.aspx?Gubun=" + Request.Params["HSerchOption"] + "&Value=" + Request.Params["SerchValue"]);
			}
		}
	}
	protected void SelectBranch_SelectedIndexChanged(object sender, EventArgs e) {
		Session["MemberInfo"] = MEMBERINFO[0] + "!" + SelectBranch.SelectedValue + "!" + MEMBERINFO[2];
		MEMBERINFO[1] = SelectBranch.SelectedValue;
	}
}