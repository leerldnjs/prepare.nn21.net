using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_LogedTopMenu : System.Web.UI.UserControl
{
	public String CustomerCode;
	protected String NowUri;
	protected String PARAM;
	protected void Page_Load(object sender, EventArgs e) {
		NowUri = Request.Url.LocalPath.Substring(Request.Url.LocalPath.LastIndexOf("/") + 1, Request.Url.LocalPath.LastIndexOf('.') - Request.Url.LocalPath.LastIndexOf("/") - 1);
		if (Request.Url.Query.Length > 3) {
			PARAM = Request.Url.Query.Substring(1, 1);
		}

		if (NowUri == "RequestFormView" && PARAM == "M") {

		} else {
			if (Session["MemberInfo"] == null) {
				Page.ClientScript.RegisterStartupScript(this.GetType(), "alertscript", "<script type='text/javascript'>alert('로그인 해주세요'); location.href='../Default.aspx';</script>", false);
				Response.Redirect("~/Default.aspx");
			} else {
				if (Session["SubInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[0] == null || Session["SubInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[0] == "") { CustomerCode = GetGlobalResourceObject("RequestForm", "NEW") + ""; } else { CustomerCode = Session["SubInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[0]; }

				NowUri = Request.Url.LocalPath.Substring(Request.Url.LocalPath.LastIndexOf("/") + 1, Request.Url.LocalPath.LastIndexOf('.') - Request.Url.LocalPath.LastIndexOf("/") - 1);
			}
			//if ( NowUri == "RequestFormView" || NowUri == "RequestFormList" ) { RequestFormL.Font.Bold = true; OwnCustomerList.Font.Bold = false; }
			//else if ( NowUri == "OwnCustomerList" || NowUri == "OwnCustomerView" || NowUri == "MyAccountView" ) { RequestFormL.Font.Bold = false; OwnCustomerList.Font.Bold = true; }

			//RequestFormW.Text = GetGlobalResourceObject("Member", "RequestFormWrite") + "";
			//RequestFormL.Text = GetGlobalResourceObject("Member", "RequestFormView") + "";
			//OwnCustomerList.Text = GetGlobalResourceObject("Member", "MyInformation") + "";
			//Button1.Text = GetGlobalResourceObject("Member", "LogOut") + "";
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