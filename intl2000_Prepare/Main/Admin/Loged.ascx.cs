using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Admin_Loged : System.Web.UI.UserControl
{
	public String CustomerCode;
	public String RecentRequest;
	protected String[] MEMBERINFO;
	protected String ButtonRight;
	protected String SerchBL;
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			MEMBERINFO = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);
		}
		catch (Exception)
		{
			Response.Redirect("../Default.aspx");
		}
		CustomerCode = MEMBERINFO[2];

      if (MEMBERINFO[2] == "ilyw0" || MEMBERINFO[2] == "ilgz0" || MEMBERINFO[2] == "ilyt0" || MEMBERINFO[2] == "ilman" || MEMBERINFO[2] == "ilic30" || MEMBERINFO[2] == "ilogistics")
		{
			if (IsPostBack)
			{
				SelectBranch.Visible = true;
			}
			else
			{
				SelectBranch.Visible = true;
				SelectBranch.SelectedValue = MEMBERINFO[1];
			}
		}
		


		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } }
		catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }

		LogOut.Text = GetGlobalResourceObject("Member", "LogOut") + "";

		if (MEMBERINFO[1]+""=="3157") {
         //ButtonRight = "<input type=\"button\" value=\"통관\" onclick=\"location.href = '../Admin/StorageOutForCustoms.aspx'\" style=\" height:25px; font-size:12px; \"/>";
         ButtonRight = "<input type=\"button\" value=\"세납\" onclick=\"location.href = '../Admin/Taxpaid.aspx'\" style=\" height:25px; font-size:12px; \"/>&nbsp;";
		}
			ButtonRight += "<input type=\"button\" value=\"未\" onclick=\"location.href = '../Finance/SimpleMisu.aspx';\" style=\"background-color:#ffcccc;\" /> " +
								"<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "ghksdbfwjdqh") + "\" onclick=\"location.href = '../Admin/ExchangeRate.aspx?G=V';\" style=\" height:25px; font-size:12px; \" /> " +
								"<input type=\"button\" value=\"" + GetGlobalResourceObject("Member", "InfoCustomer") + "\" onclick=\"location.href = '../Admin/CompanyList.aspx';\" style=\"height:25px; font-size:12px; \" /> " +
								"<input type=\"button\" value=\"" + GetGlobalResourceObject("qjsdur", "djqcpemdfhr") + "\" style=\" height:25px;font-size:12px;\" onclick=\"AddNewCustomer();\" />";
			SerchBL=   "<option value=\"SerchBL\">BLNo</option>";
		

		SetRecentRequest(MEMBERINFO[1]);

	}

	private void SetRecentRequest(string Pk)
	{
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "EXEC SP_SelectRecentRequest @BranchCode = " + Pk + ";";
		StringBuilder Return = new StringBuilder();
		//result RequestFormPk ShipperCode RequestDate StepCL
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			//Query.Append(RS[0] + "#@!" + RS[1] + "#@!" + RS[2] + "#@!" + RS[3] + "@@");
			string companycode;
			string TempString;
			if (RS[1] + "" == "") {
				TempString = "g=sn";
				companycode = "<span style='color:red;'>NEW</span>";
			} else {
				TempString = "g=s";
				companycode = RS[1] + "";
			}
			DateTime TempDateTime = Convert.ToDateTime(RS[2] + "");
			string Temp = TempDateTime.Date == DateTime.Now.Date ? TempDateTime.TimeOfDay.ToString().Substring(0, 5) : TempDateTime.Date.ToString().Substring(5, 5);
			string Temp2 = RS[3] + "" == "50" ? "<span style=\"color:red;cursor:hand;\" >" : "<span style=\"cursor:hand;\" >";

			Return.Append("<tr><td><a href=\"../Admin/RequestView.aspx?" + TempString + "&pk=" + RS[0] + "\">" + Temp2 + Temp + "</span></a></td><td><a href=\"../Admin/RequestView.aspx?" + TempString + "&pk=" + RS[0] + "\">" + Temp2 + companycode + "</span></a></td></tr>");
		}
		RS.Dispose();
		DB.DBCon.Close();
		RecentRequest = Return + "";
	}

	protected void Button1_Click(object sender, EventArgs e)
	{
		HttpCookie aCookie;
		string cookieName;
		int limit = Request.Cookies.Count;
		for (int i = 0; i < limit; i++) {
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
	protected void Btn_GoSerch_Click(object sender, EventArgs e)
	{
		if (Request.Params["SerchValue"].Trim() == "") {
			Response.Redirect("../Admin/CompanyList.aspx");
		} else {
			if (Request.Params["HSerchOption"] == "SerchBL") {
				Response.Redirect("../Admin/CheckDescriptionList.aspx?Type=SerchBL&Value=" + Request.Params["SerchValue"]);					
			} else {
				Response.Redirect("../Admin/CompanyList.aspx?Gubun=" + Request.Params["HSerchOption"] + "&Value=" + Request.Params["SerchValue"]);		
			}
		}
	}
	protected void SelectBranch_SelectedIndexChanged(object sender, EventArgs e)
	{
		Session["MemberInfo"] = MEMBERINFO[0] + "!" + SelectBranch.SelectedValue + "!" + MEMBERINFO[2];
		MEMBERINFO[1] = SelectBranch.SelectedValue;
	}
	
}