using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

public partial class Admin_Dialog_SelectTransportWay : System.Web.UI.Page
{
	protected String OurBranchSelect;
	protected String IsUpload = "N";
    protected void Page_Load(object sender, EventArgs e)
    {
		try { switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; } }
		catch (Exception) { }

		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
		OurBranchSelect = "도착지사 : <select id='ToBranch' >" + Common.SelectOurBranch + "</select>";
    }
}