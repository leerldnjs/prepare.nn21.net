using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_LogedTopMenuWithoutScript : System.Web.UI.UserControl
{
	public String CustomerCode;
	private String NowUri;
    protected void Page_Load(object sender, EventArgs e)
    {
		if (Session["MemberInfo"] == null)
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "alertscript", "<script type='text/javascript'>alert('로그인 해주세요'); location.href='../Default.aspx';</script>", false);
			Response.Redirect("~/Default.aspx");
		}
		else
		{
			if (Session["SubInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[0] == null || Session["SubInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[0] == "")
			{ CustomerCode = GetGlobalResourceObject("RequestForm", "NEW") + ""; }
			else
			{ CustomerCode = Session["SubInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[0]; }

			NowUri = Request.Url.LocalPath.Substring(Request.Url.LocalPath.LastIndexOf("/") + 1, Request.Url.LocalPath.LastIndexOf('.') - Request.Url.LocalPath.LastIndexOf("/") - 1);
		}
    }
	protected void BTN_Click_Logout(object sender, EventArgs e)
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