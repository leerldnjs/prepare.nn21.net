using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Member_Attention : System.Web.UI.Page
{
	protected string Image;
	protected string Language;
	protected String[] SubInfo;
    protected void Page_Load(object sender, EventArgs e)
    {
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");

		if (Session["MemberInfo"] == null) {
			Response.Redirect("../Default.aspx");
		}
		SubInfo = Session["SubInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		if (Request.Params["H"] + "" == "") {
			Page.UICulture = "ko";
		} else if (Request.Params["H"] + "" == "ko") {
			Page.UICulture = "ko";
		} else if (Request.Params["H"] + "" == "en") {
			Page.UICulture = "en";
		} else if (Request.Params["H"] + "" == "zh") {
			Page.UICulture = "zh-cn";
		}
		
		Image = "" + GetGlobalResourceObject("qjsdur", "wktpgkswndmltkgkd") + "";

    }
}