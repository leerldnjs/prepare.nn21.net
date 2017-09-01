using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;

public partial class Member_MyAccountView : System.Web.UI.Page
{
	public String Name, Duties, TEL, Mobile, Email;
	public String AccountID;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] == null) {
			Response.Redirect("../Default.aspx");
		}
		AccountID = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[2];
		try {
			if (Request["Language"].Length == 2) {
				Session["Language"] = Request["Language"];
			}
		} catch (Exception) {
		}

		switch (Session["Language"] + "") {
			case "en":
				Page.UICulture = "en";
				break;
			case "ko":
				Page.UICulture = "ko";
				break;
			case "zh":
				Page.UICulture = "zh-cn";
				break;
		}
		CompanyAccountView(AccountID, out Name, out Duties, out TEL, out Mobile, out Email);
	}
	public void CompanyAccountView(string ID, out string Name, out string Duties, out string TEL, out string Mobile, out string Email) {
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = " SELECT A.Duties, A.Name, A.TEL, A.Mobile, A.Email FROM Account_ as A WHERE A.[AccountID]='" + ID + "';";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		RS.Read();
		Duties = RS[0] + "";
		Name = RS[1] + "";
		TEL = RS[2] + "";
		Mobile = RS[3] + "";
		Email = RS[4] + "";
		RS.Dispose();
		DB.DBCon.Close();

	}   //Member/MyAccountView.aspx 
}