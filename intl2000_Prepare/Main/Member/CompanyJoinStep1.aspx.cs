using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_CompanyJoinStep1 : System.Web.UI.Page
{
	
	protected void Page_Load(object sender, EventArgs e)
	{
		try { if (Request.Params["Language"].Trim() != null || Request.Params["Language"].Trim() != "") { Session["Language"] = Request.Params["Language"]; } }
		catch (NullReferenceException) { }
		switch (Session["Language"] + "")
		{
			case "en": Page.UICulture = "en"; break;
			case "ko": Page.UICulture = "ko"; break;
			case "zh": Page.UICulture = "zh-cn"; break;
		}
	}
	protected void btn_Submit_Click(object sender, EventArgs e)
	{
		Session["CompanyJoinStep1"] = Request.Params["SESSION1"];
        Response.Redirect("CompanyJoinStep2.aspx");
	}
}