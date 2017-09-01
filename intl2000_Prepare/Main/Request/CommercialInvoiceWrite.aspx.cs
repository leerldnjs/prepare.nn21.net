﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Request_CommercialInvoiceWrite : System.Web.UI.Page
{
	protected String[] MemberInfo;
	protected String[] SubInfo;
	protected void Page_Load(object sender, EventArgs e)
	{
		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } }
		catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }

		if (Session["MemberInfo"] == null || Session["MemberInfo"] + "" == "") { Response.Redirect("~/Default.aspx"); }
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		SubInfo = Session["SubInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
	}
}
