using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_BranchInfo : System.Web.UI.Page
{
    protected String[] MEMBERINFO;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) 
        { 
            Response.Redirect("../Default.aspx"); 
        }
        MEMBERINFO = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

    }
}