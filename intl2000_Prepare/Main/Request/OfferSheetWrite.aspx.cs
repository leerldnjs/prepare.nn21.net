using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Request_OfferSheetWrite : System.Web.UI.Page
{
	protected String[] MemberInfo;
	protected String[] SubInfo;
	protected String Shipper;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] == null || Session["MemberInfo"] + "" == "") { Response.Redirect("~/Default.aspx"); }
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		SubInfo = Session["SubInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		Shipper = string.Empty;
	}
}
