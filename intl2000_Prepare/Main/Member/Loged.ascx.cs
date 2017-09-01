using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

public partial class Member_Loged : System.Web.UI.UserControl
{
	public String CustomerCode;
	private String NowUri;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] == null) { Response.Redirect("../Default.aspx"); }
		if (Session["SubInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[0] == null || Session["SubInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[0] == "")
		{ CustomerCode = GetGlobalResourceObject("RequestForm", "NEW") + ""; }
		else
		{ CustomerCode = Session["SubInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[0]; }

		try{if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; }}
		catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }
		RequestFormL.Text = GetGlobalResourceObject("qjsdur", "anffbrhksfl") + "";
		Button2.Text = GetGlobalResourceObject("qjsdur", "anstjrhksfl") + "";
		BTN_Fianacial.Text = GetGlobalResourceObject("qjsdur", "woanrhksfl") + "";

		NowUri = Request.Url.LocalPath.Substring(Request.Url.LocalPath.LastIndexOf("/") + 1, Request.Url.LocalPath.LastIndexOf('.') - Request.Url.LocalPath.LastIndexOf("/") - 1);
		if (NowUri == "RequestFormView" || NowUri == "RequestFormList")	{ RequestFormL.Font.Bold = true; OwnCustomerList.Font.Bold = false; }
		else if (NowUri == "OwnCustomerList" || NowUri=="OwnCustomerView"||  NowUri=="MyAccountView") { RequestFormL.Font.Bold = false; OwnCustomerList.Font.Bold = true; }

		//RequestFormW.Text = GetGlobalResourceObject("Member", "RequestFormWrite") + "";
		RequestFormL.Text = GetGlobalResourceObject("Member", "RequestFormView") + "";
		OwnCustomerList.Text = GetGlobalResourceObject("Member", "MyInformation") + "";
		Button1.Text = GetGlobalResourceObject("Member", "LogOut") + "";
		BTN_Order.Text = GetGlobalResourceObject("qjsdur", "dhejrhksfl") + "";
	}
	protected void Button1_Click(object sender, EventArgs e)
	{
		HttpCookie aCookie;
		string cookieName;
		int limit = Request.Cookies.Count;
		for (int i = 0; i < limit; i++)
		{
			cookieName = Request.Cookies[i].Name;
			aCookie = new HttpCookie(cookieName);
			aCookie.Expires = DateTime.Now.AddDays(-1);
			Response.Cookies.Add(aCookie);
		}
		Session["MemberInfo"] = null;
		Session["SubInfo"] = null;
		Response.Redirect("../Default.aspx");
	}
}