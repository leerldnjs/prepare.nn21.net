using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Dialog_DeliverySetByMember : System.Web.UI.Page
{
	protected String OurBranchStorageOutPk;
	protected String CONSIGNEEPK;
	protected String ACCOUNTID;
	protected String RequestFormPk;
	protected void Page_Load(object sender, EventArgs e)
	{
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");

		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } } catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }


		OurBranchStorageOutPk = Request.Params["P"] + "";
		RequestFormPk = Request.Params["S"] + "";
		CONSIGNEEPK = Request.Params["C"] + "";
		ACCOUNTID = Request.Params["A"] + "";
	}
}