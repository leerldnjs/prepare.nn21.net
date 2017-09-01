using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Member_CompanyJoinStep2 : System.Web.UI.Page
{
	protected String[] CompanyJoinStep1;
    protected String JFECompanyPk = "N";
    //CompanyCode], [CompanyName], [RegionCode], [CompanyAddress], [CompanyTEL], [CompanyFAX], [PresidentName], [PresidentEmail], [CompanyNameE], [CompanyAddressE]
    //[ID], [Duties], [Name], [TEL], [Mobile], [Email]
    protected void Page_Load(object sender, EventArgs e)
	{
		//Session["CompanyJoinStep1"] = "stud83#@!1q2w3e#@!김상수#@!President#@!032-576-9508#@!010-3137-0410#@!stud83@gmail.com#@!830410-1017716#@!3";
        if (Session["CompanyJoinStep1"] == null) { Response.Redirect("../Default.aspx");  }
        //Response.Write(Session["CompanyJoinStep1"] + "");
		CompanyJoinStep1 = Session["CompanyJoinStep1"].ToString().Split(Common.Splite321, StringSplitOptions.None);
        try { if (Request.Params["Language"].Trim() != null || Request.Params["Language"].Trim() != "") { Session["Language"] = Request.Params["Language"]; } }
        catch (NullReferenceException) { }
		switch (Session["Language"] + "")
		{
			case "en": Page.UICulture = "en"; break;
			case "ko": Page.UICulture = "ko"; break;
			case "zh": Page.UICulture = "zh-cn"; break;
		}
		if (Session["JFEC"] != null) { JFECompanyPk = Session["JFEC"] + ""; }
	}
    protected void BTN_Submit_Click(object sender, EventArgs e)
    {
        Session["CompanyJoinStep1"] = null;
        Session["CompanyJoinStep2"] = null;
        Response.Redirect("../Default.aspx");
        //Session["CompanyJoinStep2"] = Request.Params["CompanyPk"];
        //Session["MemberInfo"] = "Company!" + Request.Params["CompanyPk"] + "!" + CompanyJoinStep1[0] + "!" + CompanyJoinStep1[2] + "!" + CompanyJoinStep1[3];
        //Session["SubInfo"] = "!" + Request.Params["TB_CompanyName"];
        //Response.Redirect("Intro.aspx");
        //Response.Redirect("CompanyJoinStep3.aspx");
    }
}