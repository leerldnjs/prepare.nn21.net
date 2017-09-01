using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Request_RequestModify : System.Web.UI.Page
{
	protected String[] MEMBERINFO;
	protected String[] SUBINFO;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] == null || Session["MemberInfo"] + "" == "") {
			Page.ClientScript.RegisterStartupScript(this.GetType(), "alertscript", "<script type='text/javascript'>alert('로그인 해주세요'); location.href='../Default.aspx';</script>", false);
			Response.Redirect("~/Default.aspx");
		}
		MEMBERINFO = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);
		SUBINFO = Session["SubInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);

		SetLanguage();
	}
	private void SetLanguage() {
		if (Request["Language"] == null) {
			switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }
		} else {
			switch (Request["Language"]) { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }
			Session["Language"] = Request["Language"];
		}
	}
}