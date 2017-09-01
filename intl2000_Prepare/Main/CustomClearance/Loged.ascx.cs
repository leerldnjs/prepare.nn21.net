using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CustomClearance_Loged : System.Web.UI.UserControl
{
	protected String[] MEMBERINFO;
    protected void Page_Load(object sender, EventArgs e)
    {
		//CustomerView.Text = GetGlobalResourceObject("Member", "InfoCustomer") + "";
		//ExchangeRate.Text = GetGlobalResourceObject("qjsdur", "ghksdbfwjdqh") + "";
		try {
			MEMBERINFO = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);
		} catch (Exception) {
			Response.Redirect("../Default.aspx");
		}
		LogOut.Text = GetGlobalResourceObject("Member", "LogOut") + "";
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

		Response.Cookies["IL"]["MemberInfo"] = null;
		Response.Cookies["IL"]["SubInfo"] = null;
		Response.Cookies["IL"]["Gubun"] = null;
		Response.Cookies["IL"].Expires = DateTime.Now.AddDays(-1);

		Session["MemberInfo"] = null;
		Session["SubInfo"] = null;
		Response.Redirect("../Default.aspx");
	}
	protected void Btn_GoSerch_Click(object sender, EventArgs e)
	{
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
}