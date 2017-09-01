using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_CompanyJoinFromEmail : System.Web.UI.Page
{
    protected String EMAIL;
    protected String COMPANYPK;
    protected void Page_Load(object sender, EventArgs e)
    {
		if (Request.Params["E"] == null || Request.Params["S"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        EMAIL = Request.Params["E"];
        COMPANYPK = Request.Params["S"];
        string result = new Member().VerifyBetweenCompanyCodeNEmail(COMPANYPK, EMAIL);
        if (result == "N")
        {
            Response.Redirect("../default.aspx");
        }
        else
        {
            Session["JFEC"] = COMPANYPK;
            Session["JFEE"] = EMAIL;
        }
		//Response.Redirect("CompanyJoinStep1.aspx");
        //Response.Write(Session["JFEE"] + " " + Session["JFEC"]);
    }
}