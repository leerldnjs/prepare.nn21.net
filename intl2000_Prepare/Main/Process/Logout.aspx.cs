using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Process_Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
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
}