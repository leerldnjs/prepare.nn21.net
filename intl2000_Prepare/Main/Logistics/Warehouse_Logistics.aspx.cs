using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Logistics_Warehouse_Logistics : System.Web.UI.Page
{
	protected String[] MEMBERINFO;
	protected String BTN_wldurtnwjd;
	
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) { Response.Redirect("../Default.aspx"); }
		MEMBERINFO = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		BTN_wldurtnwjd = "<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "wldurtnwjd") + "\" onclick=\"document.getElementById('spRegionCodeChange').style.visibility='visible';\" />";

	}
}