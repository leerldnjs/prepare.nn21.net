using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Components;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Services;

public partial class Admin_Dialog_DeliverySet : System.Web.UI.Page
{
	protected String OurBranchStorageOutPk;
	protected String CONSIGNEEPK;
	protected String OURBRANCHPK;
	protected String ACCOUNTID;
	protected String RequestFormPk;
	protected String[] MemberInformation;

    protected void Page_Load(object sender, EventArgs e)
    {
		MemberInformation = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		if (MemberInformation[0] == "Customs") {
			Response.Write("<script type='text/javascript'>alert(\"권한이 없습니다\"); window.close();</script>");	

		}
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");

		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } }
		catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }

		OurBranchStorageOutPk = Request.Params["P"] + "";
		RequestFormPk = Request.Params["S"] + "";
		CONSIGNEEPK = Request.Params["C"] + "";
		OURBRANCHPK = Request.Params["O"] + "";
		ACCOUNTID = Request.Params["A"] + "";
	}

}